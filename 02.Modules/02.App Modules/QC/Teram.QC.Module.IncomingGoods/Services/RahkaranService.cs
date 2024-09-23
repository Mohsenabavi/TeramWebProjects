using Dapper;
using System.Data.SqlClient;
using System.Globalization;
using Teram.QC.Module.IncomingGoods.Models.ServiceModels;

namespace Teram.QC.Module.IncomingGoods.Services
{
    public class RahkaranService : IRahkaranService
    {
        private readonly IConfiguration configuration;
        private readonly string rahkaranConnectionString;
        public RahkaranService(IConfiguration configuration)
        {
            this.configuration=configuration??throw new ArgumentNullException(nameof(configuration));
            this.rahkaranConnectionString = configuration.GetSection("ConnectionStrings").GetValue<string>("RahkaranConnectionString");
        }
        public async Task<List<QualityInspectionResultModel>> GetQualityInspectionData(string Number)
        {
            var query = string.Format(@"                                                                               
                    SELECT qr.Number, p.Name, p.Code, qri.MajorUnitQuantity, qri.Quantity, qri.Date[DocumentDate],
                    qri.ApprovedQuantity, qri.MajorUnitApprovedQuantity, qri.MajorUnitRejectedQuantity , qri.TestDate , dl.Title ,N'تحویل' AS Refkind , 'طرف مقابل' AS Counterkind

                    FROM QCM3.QualityInspectionResult qr
                    JOIN QCM3.QualityInspectionResultItem qri ON qr.QualityInspectionResultID=qri.QualityInspectionResultRef
                    INNER JOIN LGS3.Part p ON qri.PartRef = p.PartID
                    JOIN SYS3.RecursiveEntityRelation rer ON rer.SourceType=569 AND rer.TargetType=497 AND rer.TargetRef=qri.QualityInspectionResultItemID
                    JOIN PRC3.DeliveryItem di ON di.DeliveryItemID=rer.SourceRef
                    JOIN FIN3.DL dl ON dl.DLID=di.CounterpartRef
                    WHERE qr.Number={0}
                    union
                    SELECT qr.Number, p.Name, p.Code, qri.MajorUnitQuantity, qri.Quantity, qri.Date[DocumentDate],
                    qri.ApprovedQuantity, qri.MajorUnitApprovedQuantity, qri.MajorUnitRejectedQuantity , qri.TestDate , pr.FullName,N'رسید موقت امانی' AS Refkind , 'تامین کننده' AS Counterkind
                    FROM QCM3.QualityInspectionResult qr
                    JOIN QCM3.QualityInspectionResultItem qri ON qr.QualityInspectionResultID=qri.QualityInspectionResultRef
                    INNER JOIN LGS3.Part p ON qri.PartRef = p.PartID
                    JOIN SYS3.RecursiveEntityRelation rer ON rer.SourceType=478 AND rer.TargetType=497 AND rer.TargetRef=qri.QualityInspectionResultItemID
                    JOIN LGS3.InventoryVoucherItem ivi ON ivi.InventoryVoucherItemID=rer.SourceRef AND ivi.InventoryVoucherSpecificationRef=114
                    JOIN LGS3.InventoryVoucher iv ON iv.InventoryVoucherID=ivi.InventoryVoucherRef
                    JOIN GNR3.Party pr ON pr.PartyID=iv.CounterpartEntityRef
                    WHERE qr.Number={0}
                    union
                    SELECT qr.Number, p.Name, p.Code, qri.MajorUnitQuantity, qri.Quantity, qri.Date[DocumentDate],
                    qri.ApprovedQuantity, qri.MajorUnitApprovedQuantity, qri.MajorUnitRejectedQuantity , qri.TestDate , cc.Name,N'رسید موقت برگشت کالا' AS Refkind , 'مرکز هزینه' AS Counterkind
                    FROM QCM3.QualityInspectionResult qr
                    JOIN QCM3.QualityInspectionResultItem qri ON qr.QualityInspectionResultID=qri.QualityInspectionResultRef
                    INNER JOIN LGS3.Part p ON qri.PartRef = p.PartID
                    JOIN SYS3.RecursiveEntityRelation rer ON rer.SourceType=478 AND rer.TargetType=497 AND rer.TargetRef=qri.QualityInspectionResultItemID
                    JOIN LGS3.InventoryVoucherItem ivi ON ivi.InventoryVoucherItemID=rer.SourceRef AND ivi.InventoryVoucherSpecificationRef=94
                    JOIN LGS3.InventoryVoucher iv ON iv.InventoryVoucherID=ivi.InventoryVoucherRef
                    JOIN GNR3.CostCenter cc ON iv.CounterpartEntityRef=cc.CostCenterID
                    WHERE qr.Number={0}
                    ", Number);

            using (var connection = new SqlConnection(rahkaranConnectionString))
            {
                var dateList = await connection.QueryAsync<QualityInspectionResultModel>(query);
                return dateList.ToList();
            }
        }
    }
}
