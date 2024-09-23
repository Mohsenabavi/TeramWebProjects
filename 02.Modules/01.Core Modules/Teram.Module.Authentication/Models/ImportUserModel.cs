using Teram.Framework.Core.Attributes;

namespace Teram.Module.Authentication.Models
{
    public class ImportUserModel
    {

        [ImportFromExcel(ColumnIndex = 1)]
        public string Name { get; set; }

        [ImportFromExcel(ColumnIndex = 2)]
        public string Family { get; set; }

        [ImportFromExcel(ColumnIndex = 3)]
        public string NationalCode { get; set; }

        [ImportFromExcel(ColumnIndex = 4)]
        public string BirthDate {  get; set; }
    }
}
