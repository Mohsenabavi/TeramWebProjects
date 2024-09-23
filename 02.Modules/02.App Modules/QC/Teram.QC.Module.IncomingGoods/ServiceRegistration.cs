
using Teram.Framework.Core.Logic;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Logic;
using Teram.QC.Module.IncomingGoods.Logic.Interfaces;
using Teram.QC.Module.IncomingGoods.Models;
using Teram.QC.Module.IncomingGoods.Services;

namespace Teram.QC.Module.IncomingGoods
{
    public class ServiceRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IRahkaranService, RahkaranService>();

            services.AddScoped<IControlPlanCategoryLogic, ControlPlanCategoryLogic>();
            services.AddScoped<ILogic<ControlPlanCategoryModel>, ControlPlanCategoryLogic>();

            services.AddScoped<IControlPlanLogic, ControlPlanLogic>();
            services.AddScoped<ILogic<ControlPlanModel>, ControlPlanLogic>();

            services.AddScoped<IIncomingGoodsInspectionLogic, IncomingGoodsInspectionLogic>();
            services.AddScoped<ILogic<IncomingGoodsInspectionModel>, IncomingGoodsInspectionLogic>();

            services.AddScoped<IIncomingGoodsInspectionItemLogic, IncomingGoodsInspectionItemLogic>();
            services.AddScoped<ILogic<IncomingGoodsInspectionItemModel>, IncomingGoodsInspectionItemLogic>();

            services.AddScoped<IIncomingGoodsInspectionFileLogic, IncomingGoodsInspectionFileLogic>();
            services.AddScoped<ILogic<IncomingGoodsInspectionFileModel>, IncomingGoodsInspectionFileLogic>();

            services.AddScoped<IIncomingGoodsInspectionCartableItemLogic, IncomingGoodsInspectionCartableItemLogic>();
            services.AddScoped<ILogic<IncomingGoodsInspectionCartableItemModel>, IncomingGoodsInspectionCartableItemLogic>();
        }
    }
}
