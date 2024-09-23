using Teram.Framework.Core.Attributes;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.IncomingGoods.Models.ImportModels
{
    public class ImportControlPlanModel
    {
        [ImportFromExcel(ColumnIndex = 1)]        
        public string ControlPlanCategoryTitle { get; set; }

        [ImportFromExcel(ColumnIndex = 2)]
        public string ControlPlanParameter { get; set; }

        [ImportFromExcel(ColumnIndex = 3)]
        public string QuantityDescription { get; set; }

        [ImportFromExcel(ColumnIndex = 4)]
        public string AcceptanceCriteria { get; set; }
    }
}
