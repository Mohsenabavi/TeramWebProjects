using Dapper;
using System.Data.SqlClient;
using System.Globalization;
using Teram.Framework.Core.Logic;
using Teram.IT.Module.Employee.Models;

namespace Teram.IT.Module.Employee.Services
{
    public class GetEmployessService : IGetEmployessService
    {
        private readonly IConfiguration configuration;
        private readonly string rahkaranConnectionString;
        public GetEmployessService(IConfiguration configuration)
        {
            this.configuration=configuration??throw new ArgumentNullException(nameof(configuration));
            this.rahkaranConnectionString = configuration.GetSection("ConnectionStrings").GetValue<string>("RahkaranConnectionString");
        }

        public async Task<List<HREmployeeModel>> GetAllActiveEmployess()
        {
            var query = @"                                                                               
                        SELECT 
                        e.EmployeeID,
                        e.Code,
                        e.EmploymentNumber,
                        p.FirstName,
                        p.LastName,
                        p.NationalID,
                        p.Gender,
                        p.Nationality,
                        p.Allegiance,
                        p.Religion,
                        p.Mobile,
                        p.FatherName,
                        p.BirthDate,
                        p.Mobile,
                        e.Status
                        FROM HCM3.Employee e JOIN GNR3.Party p ON p.PartyID=e.PartyRef";

            using (var connection = new SqlConnection(rahkaranConnectionString))
            {
                var dataList = await connection.QueryAsync<HREmployeeModel>(query);               
                return dataList.ToList();
            }
        }
    }
}
