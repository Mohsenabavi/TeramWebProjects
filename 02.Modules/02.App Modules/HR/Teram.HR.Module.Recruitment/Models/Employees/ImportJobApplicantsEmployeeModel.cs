using DocumentFormat.OpenXml.Wordprocessing;
using Teram.Framework.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.Employees
{
    public class ImportJobApplicantsEmployeeModel
    {
        [ImportFromExcel(ColumnIndex = 1)]
        public string PersonnelCode { get; set; }

        [ImportFromExcel(ColumnIndex = 2)]
        public string FirstName { get; set; }

        [ImportFromExcel(ColumnIndex = 3)]
        public string LastName { get; set; }

        [ImportFromExcel(ColumnIndex = 4)]

        public string FatherName {  get; set; }

        [ImportFromExcel(ColumnIndex = 5)]
        public string IdentityNumber {  get; set; }

        [ImportFromExcel(ColumnIndex = 6)]
        public string JobPositionTitle { get; set; }

        [ImportFromExcel(ColumnIndex = 7)]
        public string NationalCode {  get; set; }

        [ImportFromExcel(ColumnIndex = 8)]
        public string MobileNumber {  get; set; }

    }
}
