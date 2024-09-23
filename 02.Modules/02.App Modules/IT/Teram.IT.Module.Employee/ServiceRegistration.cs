using Teram.Framework.Core.Logic;
using Teram.IT.Module.Employee.Jobs;
using Teram.IT.Module.Employee.Logic;
using Teram.IT.Module.Employee.Logic.Interface;
using Teram.IT.Module.Employee.Models;
using Teram.IT.Module.Employee.Services;

namespace Teram.IT.Module.Employee
{
    public class ServiceRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IHREmployeeLogic, HREmployeeLogic>();           
            services.AddScoped<ILogic<HREmployeeModel>, HREmployeeLogic>();
            services.AddScoped<IGetEmployessService, GetEmployessService>();
            services.AddScoped<IUpdateUsersService, UpdateUsersService>();
            services.AddScoped<GetAllActiveEmployessJob>();            
        }

        public static void JobRegister(IServiceProvider provider)
        {
            var getEmployessServiceList = provider.GetService<GetAllActiveEmployessJob>();
            getEmployessServiceList?.Initilize();       
        }
    }
}
