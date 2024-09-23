using Teram.Framework.Core.Logic;
using Teram.HR.Module.OC.Logic;
using Teram.HR.Module.OC.Logic.Interface;
using Teram.HR.Module.OC.Models;

namespace Teram.HR.Module.OC
{
    public class ServiceRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<ILogic<OrganizationChartModel>, OrganizationChartLogic>();
            services.AddScoped<IOrganizationChartLogic, OrganizationChartLogic>();

            services.AddScoped<ILogic<PositionModel>, PositionLogic>();
            services.AddScoped<IPositionLogic, PositionLogic>();
        }
    }
}
