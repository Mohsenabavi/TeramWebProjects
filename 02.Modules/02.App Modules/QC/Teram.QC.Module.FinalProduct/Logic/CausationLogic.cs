using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Entities.WorkFlow;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.QC.Module.FinalProduct.Models.ReportsModels;
using Teram.Web.Core.Security;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Teram.QC.Module.FinalProduct.Logic
{

    public class CausationLogic : BusinessOperations<CausationModel, Causation, int>, ICausationLogic
    {
        private readonly IConfiguration configuration;
        private readonly IOperatorLogic operatorLogic;
        private readonly IUserPrincipal userPrincipal;

        public CausationLogic(IPersistenceService<Causation> service, IConfiguration configuration,
            IOperatorLogic operatorLogic,
            IUserPrincipal userPrincipal) : base(service)
        {
            BeforeUpdate += CausationLogic_BeforeUpdate;
            this.configuration = configuration;
            this.operatorLogic = operatorLogic ?? throw new ArgumentNullException(nameof(operatorLogic));
            this.userPrincipal = userPrincipal ?? throw new ArgumentNullException(nameof(userPrincipal));
        }

        public BusinessOperationResult<CausationModel> GetByFinalProductNonComplianceId(int finalProductNonComplianceId)
        {
            return GetFirst<CausationModel>(x => x.FinalProductNoncomplianceId == finalProductNonComplianceId);
        }

        public async Task<List<WrongDoerReportModel>> GetWrongDoerReport(int wrongDoerId)
        {
            using var dbConnection = new SqlConnection(configuration.GetConnectionString("TeramConnectionString"));
            await dbConnection.OpenAsync();
            var parameters = new DynamicParameters();
            parameters.Add("@WrongdoerId", wrongDoerId);
            string storedProcedure = "GetFinalProductNoncomplianceByWrongdoerId";

            var items = await dbConnection.QueryAsync<WrongDoerReportModel>(
            storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var spData = items.ToList();

            return items.ToList();

        }

        public async Task<(List<WrongDoersListReportModel> Items, int TotalCount)> GetWrongDoerReportData(
       int? WrongDoerId = null,
       DateTime? statInputDate = null,
       DateTime? endInputDate = null,
       int skip = 0,
       int take = 10)
        {
            using (var dbConnection = new SqlConnection(configuration.GetConnectionString("TeramConnectionString")))
            {
                await dbConnection.OpenAsync(); // Open the connection

                var parameters = new DynamicParameters();
                parameters.Add("@WrongdoerId", WrongDoerId);
                parameters.Add("@StartDate", statInputDate);
                parameters.Add("@EndDate", endInputDate);
                parameters.Add("@Skip", skip);
                parameters.Add("@Take", take);
                parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

                string storedProcedure = "WrongDoersResport"; // Name of your stored procedure

                var spData = await dbConnection.QueryAsync<WrongDoersListReportModel>(
                storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var wrongDoer1Ids = spData.Select(x => x.WrongDoerId).Distinct().ToList();
                var wrongDoer2Ids = spData.Select(x => x.WrongDoerId2).Distinct().ToList();
                var wrongDoer3Ids = spData.Select(x => x.WrongDoerId3).Distinct().ToList();
                var wrongDoer4Ids = spData.Select(x => x.WrongDoerId4).Distinct().ToList();
                var unionWrongDoersIds = wrongDoer1Ids.Union(wrongDoer2Ids).Union(wrongDoer3Ids).Union(wrongDoer4Ids).Distinct().ToList();
                var operatorsList = operatorLogic.GetByWrongdoerIds(unionWrongDoersIds);

                foreach (var item in spData)
                {
                    var wrongdoer1Result = operatorsList.ResultEntity.Where(x => x.OperatorId == item.WrongDoerId).FirstOrDefault();
                    if (wrongdoer1Result != null)
                        item.WrongDoers += string.Join("/", string.Concat(wrongdoer1Result.PersonnelCode, "-", wrongdoer1Result.FirstName, " ", wrongdoer1Result.LastName));
                    var wrongdoer2Result = operatorsList.ResultEntity.Where(x => x.OperatorId == item.WrongDoerId2).FirstOrDefault();
                    if (wrongdoer2Result != null)
                        item.WrongDoers += string.Join("/", string.Concat(wrongdoer2Result.PersonnelCode, "-", wrongdoer2Result.FirstName, " ", wrongdoer2Result.LastName));
                    var wrongdoer3Result = operatorsList.ResultEntity.Where(x => x.OperatorId == item.WrongDoerId3).FirstOrDefault();
                    if (wrongdoer3Result != null)
                        item.WrongDoers += string.Join("/", string.Concat(wrongdoer3Result.PersonnelCode, "-", wrongdoer3Result.FirstName, " ", wrongdoer3Result.LastName));
                    var wrongdoer4Result = operatorsList.ResultEntity.Where(x => x.OperatorId == item.WrongDoerId4).FirstOrDefault();
                    if (wrongdoer4Result != null)
                        item.WrongDoers += string.Join("/", string.Concat(wrongdoer4Result.PersonnelCode, "-", wrongdoer4Result.FirstName, " ", wrongdoer4Result.LastName));
                }

                int totalCount = parameters.Get<int>("@TotalCount");

                return (spData.ToList(), totalCount);
            }

        }

        private void CausationLogic_BeforeUpdate(TeramEntityEventArgs<Causation, CausationModel, int> entity)
        {
            var cauationResult = Service.DeferrQuery().Include(x => x.CorrectiveActions)
                  .FirstOrDefault(x => x.CausationId == entity.NewEntity.CausationId);

            if (cauationResult != null)
            {
                var copyOfResult = cauationResult;

                if (cauationResult != null && cauationResult?.CorrectiveActions != null && cauationResult?.CorrectiveActions.Count != 0)
                {
                    foreach (var item in cauationResult.CorrectiveActions)
                    {
                        Service.MarkAsRemovedEntity(item);
                    }
                }
                var hasLimitedAccessToCausation = userPrincipal.CurrentUser.HasClaim("Permission", ":Causation:HasLimitedAccessForCausation");

                if (!hasLimitedAccessToCausation && cauationResult != null)
                {
                    cauationResult.HasHRCause = entity.NewEntity.HasHRCause;
                    cauationResult.HasMethodCause = entity.NewEntity.HasMethodCause;
                    cauationResult.HasRawMaterialCause = entity.NewEntity.HasRawMaterialCause;
                    cauationResult.HasEssentialCause = entity.NewEntity.HasEssentialCause;
                    cauationResult.HasEquipmentCause = entity.NewEntity.HasEquipmentCause;
                    cauationResult.MachineId = entity.NewEntity.MachineId;
                    cauationResult.OperatorId = entity.NewEntity.OperatorId;
                    cauationResult.IsLackOfFit = entity.NewEntity.IsLackOfFit;
                    cauationResult.IsCaseError = entity.NewEntity.IsCaseError;
                    cauationResult.HasLackOfFitWorkerAndJob = entity.NewEntity.HasLackOfFitWorkerAndJob;
                    cauationResult.HasLackOfEducation = entity.NewEntity.HasLackOfEducation;
                    cauationResult.HasFailureOfDefineJob = entity.NewEntity.HasFailureOfDefineJob;
                    cauationResult.RootCauseId = entity.NewEntity.RootCauseId;
                    cauationResult.InstructionId = entity.NewEntity.InstructionId;
                    cauationResult.UnitId = entity.NewEntity.UnitId;
                    cauationResult.WorkStationId = entity.NewEntity.WorkStationId;
                    cauationResult.CanBeIdentifiedAtEntrance = entity.NewEntity.CanBeIdentifiedAtEntrance;
                    cauationResult.HasEntitlementLicense = entity.NewEntity.HasEntitlementLicense;
                    cauationResult.HasNotification = entity.NewEntity.HasNotification;
                    cauationResult.NotificationNumber = entity.NewEntity.NotificationNumber;
                    cauationResult.FinalProductNoncomplianceId = entity.NewEntity.FinalProductNoncomplianceId;
                }
                else
                {
                    entity.NewEntity.HasHRCause = copyOfResult.HasHRCause;
                    entity.NewEntity.HasMethodCause = copyOfResult.HasMethodCause;
                    entity.NewEntity.HasRawMaterialCause = copyOfResult.HasRawMaterialCause;
                    entity.NewEntity.HasEssentialCause = copyOfResult.HasEssentialCause;
                    entity.NewEntity.HasEquipmentCause = copyOfResult.HasEquipmentCause;
                    entity.NewEntity.MachineId = copyOfResult.MachineId;
                    entity.NewEntity.OperatorId = copyOfResult.OperatorId;
                    entity.NewEntity.IsLackOfFit = copyOfResult.IsLackOfFit;
                    entity.NewEntity.IsCaseError = copyOfResult.IsCaseError;
                    entity.NewEntity.HasLackOfFitWorkerAndJob = copyOfResult.HasLackOfFitWorkerAndJob;
                    entity.NewEntity.HasLackOfEducation = copyOfResult.HasLackOfEducation;
                    entity.NewEntity.HasFailureOfDefineJob = copyOfResult.HasFailureOfDefineJob;
                    entity.NewEntity.RootCauseId = copyOfResult.RootCauseId;
                    entity.NewEntity.InstructionId = copyOfResult.InstructionId;
                    entity.NewEntity.UnitId = copyOfResult.UnitId;
                    entity.NewEntity.WorkStationId = copyOfResult.WorkStationId;
                    entity.NewEntity.CanBeIdentifiedAtEntrance = copyOfResult.CanBeIdentifiedAtEntrance;
                    entity.NewEntity.HasEntitlementLicense = copyOfResult.HasEntitlementLicense;
                    entity.NewEntity.HasNotification = copyOfResult.HasNotification;
                    entity.NewEntity.NotificationNumber = copyOfResult.NotificationNumber;
                    entity.NewEntity.FinalProductNoncomplianceId = copyOfResult.FinalProductNoncomplianceId;
                }
                cauationResult.CorrectiveActions = (entity.NewEntity.CorrectiveActions != null && entity.NewEntity.CorrectiveActions.Count > 0) ? entity.NewEntity.CorrectiveActions.Select(x => new CorrectiveAction
                {
                    ActionerId = x.ActionerId,
                    Descriiption = x.Descriiption,
                    CausationId = x.CausationId,
                    ApproverId = x.ApproverId,
                    ActionDate = x.ActionDate,

                }).ToList() : [];
                var affectedRowsCount = Service.Save();
            }
        }

    }
}
