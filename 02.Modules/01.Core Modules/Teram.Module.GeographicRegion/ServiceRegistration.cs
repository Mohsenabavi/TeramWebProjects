
using Teram.Module.GeographicRegion.Logic;
using Teram.Module.GeographicRegion.Logic.Interfaces;
using Teram.ServiceContracts;

namespace Teram.Module.GeographicRegion
{
    public class ServiceRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IGeographicRegionLogic,GeographicRegionLogic>();
            services.AddScoped<IGeoSharedService, GeographicRegionLogic>();
        }
    }
}
