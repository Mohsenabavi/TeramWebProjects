
using Teram.Framework.Core.Logic;
using Teram.HR.Module.TicketRegister.Logic;
using Teram.HR.Module.TicketRegister.Logic.Interfaces;
using Teram.HR.Module.TicketRegister.Models;

namespace Teram.HR.Module.TicketRegister
{
    public class ServiceRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IAreaLogic, AreaLogic>();
            services.AddScoped<ILogic<AreaModel>, AreaLogic>();

            services.AddScoped<IAreaRowLogic, AreaRowLogic>();
            services.AddScoped<ILogic<AreaRowModel>, AreaRowLogic>();

            services.AddScoped<ISeatLogic, SeatLogic>();
            services.AddScoped<ILogic<SeatModel>, SeatLogic>();
        }
    }
}
