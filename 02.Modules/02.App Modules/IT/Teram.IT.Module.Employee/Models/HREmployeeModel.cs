using Teram.Framework.Core.Logic;
using Teram.IT.Module.Employee.Enums;

namespace Teram.IT.Module.Employee.Models
{
    public class HREmployeeModel : ModelBase<Entities.HREmployee, int>
    {
        public int HREmployeeId { get; set; }

        public string Code {  get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? EmploymentNumber { get; set; }

        public string? NationalID { get; set; }

        public GenderType Gender { get; set; }

        public string? Nationality { get; set; }

        public string? Allegiance { get; set; }

        public string? Religion { get; set; }

        public string? Mobile { get; set; }

        public string? FatherName { get; set; }
        public DateTime BirthDate { get; set; }
        public int EmployeeID { get; set; }
        public int Status {  get; set; }
    }
}
