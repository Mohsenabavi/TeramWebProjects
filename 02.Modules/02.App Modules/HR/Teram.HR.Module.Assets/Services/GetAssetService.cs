using Dapper;
using Mapster;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Data.SqlClient;
using System.Globalization;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Assets.Logics.Interfaces;
using Teram.HR.Module.Assets.Models;
using Teram.HR.Module.Assets.Models.QueryModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static MongoDB.Libmongocrypt.CryptContext;

namespace Teram.HR.Module.Assets.Services
{
    public class GetAssetService : IGetAssetService
    {
        private readonly IConfiguration configuration;
        private readonly IRahkaranAssetLogic rahkaranAssetLogic;
        private readonly string rahkaranConnectionString;

        public GetAssetService(IConfiguration configuration, IRahkaranAssetLogic rahkaranAssetLogic)
        {
            this.rahkaranConnectionString = configuration.GetSection("ConnectionStrings").GetValue<string>("RahkaranConnectionString");
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.rahkaranAssetLogic = rahkaranAssetLogic;
        }
        public async Task<List<AssetQueryModel>> GetAllAssetsFromRahkaran()
        {
            var query = @"                                                                               
                       SELECT DISTINCT
	                   p.NationalID,
                       A.AssetID,
                       A.PlaqueNumber,
                       A.Title,
                       A.UtilizationDate as UtilizeDate,
                       AG.Title AS GroupTitle,
                       DM.Title AS DepreciatedMethodTitle,
                       CR.Name AS CostCenter,
                       SP.Title AS SettlementPlace,
                       (
                           SELECT TOP 1
                                  PR.FullName
                           FROM FAM3.Collector CL
                               JOIN GNR3.Party PR
                                   ON PR.PartyID = CL.PartyRef
                           WHERE CollectorID = A.CollectorRef
                       ) AS Collector,
                       P.FullName,
                       CPR.Cost,
                       PR.Code,
                       CAST(E.Code AS INT) as PersonnelCode
                FROM FAM3.Asset A
                    LEFT JOIN GNR3.Party P
                        ON P.PartyID = A.DelivererRef
                    LEFT JOIN HCM3.Employee E
                        ON E.PartyRef = P.PartyID
                    LEFT JOIN FAM3.AssetGroup AG
                        ON AG.AssetGroupID = A.AssetGroupRef
                    LEFT JOIN FAM3.AssetGroupInCalculationArea ACA
                        ON ACA.AssetGroupRef = A.AssetGroupRef
                    LEFT JOIN FAM3.DepreciationMethod DM
                        ON DM.DepreciationMethodID = ACA.DepreciationMethodRef
                    LEFT JOIN GNR3.CostCenter CR
                        ON CR.CostCenterID = A.CostCenterRef
                    LEFT JOIN FAM3.SettlementPlace SP
                        ON SP.SettlementPlaceID = A.SettlementPlaceRef
                    LEFT JOIN FAM3.Collector CTR
                        ON CTR.CollectorID = A.CollectorRef
                    LEFT JOIN FAM3.CostPart CPR
                        ON CPR.AssetRef = A.AssetID
                    LEFT JOIN LGS3.Part PR
                        ON PR.PartID = A.PartRef  		
                ORDER BY P.FullName, A.UtilizationDate";

            using (var connection = new SqlConnection(rahkaranConnectionString))
            {
                var dataList = await connection.QueryAsync<AssetQueryModel>(query);
                foreach (var asset in dataList)
                {
                    if (asset.UtilizeDate != null)
                        asset.UtilizationDate = asset.UtilizeDate.Value.ToString("yyyy/MM/dd", new CultureInfo("fa-IR"));
                }
                return dataList.ToList();
            }
        }

        public async Task<BusinessOperationResult<AssetInfo>> GetAssetByPlaqueNumber(string plaqueNumber)
        {
            var finalResult = new BusinessOperationResult<AssetInfo>();
            var query = string.Format(@"Select A.AssetID, A.PlaqueNumber,A.Title,PR.Code FROM FAM3.Asset A
                                     LEFT JOIN LGS3.Part PR
                                     ON PR.PartID = A.PartRef    
            where PlaqueNumber='{0}'", plaqueNumber);
            using var connection = new SqlConnection(rahkaranConnectionString);
            var dataList = await connection.QueryAsync<AssetInfo>(query);
            var firstAsset = dataList.FirstOrDefault();
            if (firstAsset != null)
            {
                finalResult.SetSuccessResult(firstAsset);
            }
            else
            {
                finalResult.SetErrorMessage("Asset Not Found");
            }
            return finalResult;
        }
    }
}
