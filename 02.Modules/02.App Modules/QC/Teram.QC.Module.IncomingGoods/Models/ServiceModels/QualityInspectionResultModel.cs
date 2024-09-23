using Teram.Framework.Core.Extensions;

namespace Teram.QC.Module.IncomingGoods.Models.ServiceModels
{
    public class QualityInspectionResultModel
    {
        public string Number {  get; set; }
        public string Name { get; set; }
        public string Code { get; set; }        
        public long Quantity { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime TestDate { get; set; }
        public string Title {  get; set; }
        public string Refkind {  get; set; }

        public string Counterkind {  get; set; }

        public string PersianTestDate => TestDate.ToPersianDate();
        public string PersianDocumentDate => DocumentDate.ToPersianDate();
    }
}
