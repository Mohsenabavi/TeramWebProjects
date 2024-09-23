using Teram.Framework.Core.Attributes;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models.ImportModels
{
    public class QcDefectImportModel
    {
        [ImportFromExcel(ColumnIndex = 1)]
        public string Code { get; set; }

        [ImportFromExcel(ColumnIndex = 2)]
        public string Title { get; set; }

      
    }
}
