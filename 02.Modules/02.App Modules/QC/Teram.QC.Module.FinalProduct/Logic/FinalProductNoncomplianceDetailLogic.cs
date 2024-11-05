using System.Data;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.Web.Core.Security;
using System.Data.SqlClient;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.Common;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class FinalProductNoncomplianceDetailLogic : BusinessOperations<FinalProductNoncomplianceDetailModel, FinalProductNoncomplianceDetail, int>, IFinalProductNoncomplianceDetailLogic
    {
        private readonly IUserPrincipal userPrincipal;
        private readonly IConfiguration configuration;
        private readonly IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic;

        public FinalProductNoncomplianceDetailLogic(IPersistenceService<FinalProductNoncomplianceDetail> service,
            IUserPrincipal userPrincipal, IConfiguration configuration,
            IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic) : base(service)
        {
            this.userPrincipal = userPrincipal ?? throw new ArgumentNullException(nameof(userPrincipal));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.finalProductNoncomplianceLogic = finalProductNoncomplianceLogic ?? throw new ArgumentNullException(nameof(finalProductNoncomplianceLogic)); 
            BeforeAdd += FinalProductNoncomplianceDetailLogic_BeforeAdd;
        }

        private void FinalProductNoncomplianceDetailLogic_BeforeAdd(TeramEntityEventArgs<FinalProductNoncomplianceDetail, FinalProductNoncomplianceDetailModel, int> entity)
        {
            entity.NewEntity.CreatedBy = userPrincipal.CurrentUserId;
            entity.NewEntity.CreateDate = DateTime.Now;
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductInspectionId(int finalProductInspectionId)
        {
            return GetData<FinalProductNoncomplianceDetailModel>(x => x.FinalProductInspectionId == finalProductInspectionId);
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductNoncomplianceIdsAndFinalProductInspectionId(List<int> finalProductNoncomplianceIds, int finalProductInspectionId)
        {
            return GetData<FinalProductNoncomplianceDetailModel>(x => finalProductNoncomplianceIds.Contains(x.FinalProductNoncomplianceId) && x.FinalProductInspectionId == finalProductInspectionId);
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductNoncomplianceIdsAndPalletNumber(List<int> finalProductNoncomplianceIds, int palletNumber)
        {
            return GetData<FinalProductNoncomplianceDetailModel>(x => finalProductNoncomplianceIds.Contains(x.FinalProductNoncomplianceId) && x.Number == palletNumber);
        }

        public BusinessOperationResult<FinalProductNoncomplianceDetailModel> GetByPalletNumberAndControlPlanDefectId(int palletNumber, int controlPlanDefcetId)
        {
            var relatedFinalProductNoncompliancesResult = finalProductNoncomplianceLogic.GetByControlPlanDefctId(controlPlanDefcetId);

            var relatedFinalProductNoncompliancesResultIds = relatedFinalProductNoncompliancesResult.ResultEntity.Select(x => x.FinalProductNoncomplianceId).ToList();

            return GetFirst<FinalProductNoncomplianceDetailModel>(x => x.Number == palletNumber && relatedFinalProductNoncompliancesResultIds.Contains(x.FinalProductNoncomplianceId));
        }
        public BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductNoncomplianceId(int finalProductNoncomplianceId)
        {
            return GetData<FinalProductNoncomplianceDetailModel>(x => x.FinalProductNoncomplianceId == finalProductNoncomplianceId);
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductInspectionIds(List<int> finalProductInspectionIds)
        {
            return GetData<FinalProductNoncomplianceDetailModel>(x => finalProductInspectionIds.Contains(x.FinalProductInspectionId.Value));
        }
        public async Task<(List<PalletsStatusModel> Items, int TotalCount)> GetPalletsStatusAsync(
        string number = null,
        string orderNo = null,
        string productName = null,
        int? sampleCount = null,
        string tracingCode = null,
        string productCode = null,
        int skip = 0,
        int take = 10)
        {
            using (var dbConnection = new SqlConnection(configuration.GetConnectionString("TeramConnectionString")))
            {
                await dbConnection.OpenAsync(); // Open the connection

                var parameters = new DynamicParameters();
                parameters.Add("@Number", number);
                parameters.Add("@OrderNo", orderNo);
                parameters.Add("@ProductName", productName);
                parameters.Add("@SampleCount", sampleCount);
                parameters.Add("@TracingCode", tracingCode);
                parameters.Add("@ProductCode", productCode);
                parameters.Add("@Skip", skip);
                parameters.Add("@Take", take);
                parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

                string storedProcedure = "GetPalletsStatus"; // Name of your stored procedure

                var items = await dbConnection.QueryAsync<PalletsStatusModel>(
                storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                int totalCount = parameters.Get<int>("@TotalCount");

                return (items.ToList(), totalCount);
            }

        }
    }
}
