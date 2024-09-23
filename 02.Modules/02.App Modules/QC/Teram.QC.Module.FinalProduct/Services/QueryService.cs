using Dapper;
using System.Data.SqlClient;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Models.ServiceModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Teram.QC.Module.FinalProduct.Services
{
    public class QueryService : IQueryService
    {
        private readonly IConfiguration configuration;
        private readonly string rahkaranConnectionString;
        public QueryService(IConfiguration configuration)
        {
            this.configuration=configuration??throw new ArgumentNullException(nameof(configuration));
            this.rahkaranConnectionString = configuration.GetSection("ConnectionStrings").GetValue<string>("RahkaranConnectionString");
        }

        public async Task<BusinessOperationResult<List<EmployeeModel>>> GetActiveEmployees()
        {
            var result = new BusinessOperationResult<List<EmployeeModel>>();

            try
            {
                var query = @"                                                                                                     
                    SELECT 
                    e.EmployeeID,
                    e.Code,        
                    p.FirstName,
                    p.LastName,
                    p.Mobile,
                    p.NationalID,
                    e.Status
                    FROM HCM3.Employee e JOIN GNR3.Party p ON p.PartyID=e.PartyRef Where e.Status<>3";

                using (var connection = new SqlConnection(rahkaranConnectionString))
                {
                    var dataList = await connection.QueryAsync<EmployeeModel>(query);
                    result.SetSuccessResult(dataList.ToList());
                    return result;

                }
            }
            catch (Exception ex)
            {
                result.SetErrorMessage(ex.Message);
                return result;                
            }
        }

        public async Task<BusinessOperationResult<List<OrderProductModel>>> GetOrderProducts(int orderNo)
        {
            var result = new BusinessOperationResult<List<OrderProductModel>>();

            var query = string.Format(@"                                                                               
                    Use TeramChap
                    SELECT DISTINCT iv.Level5DLCode AS OrderNo,p.PartID AS PartId,
                    p.Code , p.Name FROM
                    LGS3.InventoryVoucher iv LEFT JOIN LGS3.InventoryVoucherItem ivi
                    ON ivi.InventoryVoucherRef=iv.InventoryVoucherID
                    JOIN LGS3.Part p ON p.PartID=ivi.PartRef AND p.PartNature=2
                    WHERE iv.Level5DLCode ={0}", orderNo);

            using (var connection = new SqlConnection(rahkaranConnectionString))
            {
                var dateList = await connection.QueryAsync<OrderProductModel>(query);
                result.SetSuccessResult(dateList.ToList());
            }
            return result;
        }

        public async Task<BusinessOperationResult<PalletInfoModel>> GetPalletInfo(int PalletNo)
        {
            var result = new BusinessOperationResult<PalletInfoModel>();

            try
            {
                var query = string.Format(@"                                                                               
                    SELECT m.OrderNo,
                    m.OrderTitle,
                    m.Number,
                    m.Date,
                    m.ProductCode,
                    m.ProductName,
                    m.quantity,
                    m.ControlPlan
                    FROM (
                    SELECT ISNULL(CONVERT(INT,ivi.Level5DLCode),CONVERT(INT,iv.Level5DLCode)) AS OrderNo,
                    iv.Number,
                    iv.date AS Date,
                    (ivi.quantity) AS Quantity,
                    p.Code AS ProductCode,
                    p.Name AS ProductName,
                    ROW_NUMBER() OVER (PARTITION BY iv.Number,iv.InventoryVoucherSpecificationRef ORDER BY iv.Date DESC) AS row1,
                    ISNULL(ISNULL(pr.new_OrderNo,m1.new_name), er.new_name) AS OrderTitle,
                    ISNULL(ISNULL(m.new_ControlMap,m1.new_ControlMap),s.new_controlmap) AS ControlPlan
                    FROM
                    LGS3.InventoryVoucher iv
                    JOIN LGS3.InventoryVoucherItem ivi on iv.InventoryVoucherID=ivi.InventoryVoucherRef AND iv.InventoryVoucherSpecificationRef=124
                    JOIN LGS3.Part p ON p.PartID=ivi.PartRef
                    JOIN FIN3.DL dl ON CONVERT(INT , (dl.Code))=ISNULL(CONVERT(INT,ivi.Level5DLCode),CONVERT(INT,iv.Level5DLCode))
                    LEFT JOIN [CRM-SERVER].TeramChap_MSCRM.dbo.new_ProducrOrdersBase pr ON CONVERT(INT , pr.new_name)=CONVERT(INT , (dl.Code))
                    LEFT JOIN [CRM-SERVER].TeramChap_MSCRM.dbo.new_masterdataBase m ON m.new_masterdataId=pr.new_MasterData
                    LEFT JOIN [CRM-SERVER].TeramChap_MSCRM.dbo.new_TestProductionorderBase t ON CONVERT(INT , t.new_name)=CONVERT(INT,(dl.Code))
                    LEFT JOIN [CRM-SERVER].TeramChap_MSCRM.dbo.new_masterdataBase m1 ON m1.new_masterdataId=t.new_MasterData
                    LEFT JOIN [CRM-SERVER].TeramChap_MSCRM.dbo.new_ProductOrderRanginBase R ON CONVERT(INT , R.new_name)=CONVERT(INT,(dl.Code))
                    LEFT JOIN [CRM-SERVER].TeramChap_MSCRM.dbo.new_MasterDataOutSourceBase S ON S.new_MasterDataOutSourceId=R.new_montagemasterdata
                    LEFT JOIN [CRM-SERVER].TeramChap_MSCRM.dbo.new_EmkansanjiRanginBasteBase er ON er.new_EmkansanjiRanginBasteId=r.new_EmkansanjiRangin
                    WHERE ISNULL(CONVERT(INT,ivi.Level5DLCode),CONVERT(INT,iv.Level5DLCode)) <> 800000
                    )m
                    WHERE row1=1
                    AND m.Number={0}", PalletNo);
                using (var connection = new SqlConnection(rahkaranConnectionString))
                {
                    var data = await connection.QueryFirstAsync<PalletInfoModel>(query);
                    result.SetSuccessResult(data);
                    return result;
                }
            }
            catch (Exception ex)
            {

                result.SetErrorMessage(ex.Message + " - " + ex.InnerException);
                return result;
            }
        }
    }
}
