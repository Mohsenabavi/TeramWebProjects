using Teram.Framework.Core.Logic;
using Teram.HR.Module.Assets.Entities;
using Teram.HR.Module.Assets.Jobs.Assets;
using Teram.HR.Module.Assets.Logics;
using Teram.HR.Module.Assets.Logics.Interfaces;
using Teram.HR.Module.Assets.Models;
using Teram.HR.Module.Assets.Services;

namespace Teram.HR.Module.Assets
{
    public class ServiceRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IRahkaranAssetLogic, RahkaranAssetLogic>();
            services.AddScoped<ILogic<RahkaranAssetModel>, RahkaranAssetLogic>();
            services.AddScoped<IGetAssetService, GetAssetService>();
            services.AddScoped<GetAssetsFromRahkarancs>();
            services.AddScoped<IUpdateAssetsService, UpdateAssetsService>();
            services.AddScoped<IAssetSelfExpressionLogic, AssetSelfExpressionLogic>();
            services.AddScoped<ILogic<AssetSelfExpressionModel>, AssetSelfExpressionLogic>();
        }

        public static void JobRegister(IServiceProvider provider)
        {
            var runGetAssetsFromRahkaran = provider.GetService<GetAssetsFromRahkarancs>();
            runGetAssetsFromRahkaran?.Initilize();
        }
    }
}
