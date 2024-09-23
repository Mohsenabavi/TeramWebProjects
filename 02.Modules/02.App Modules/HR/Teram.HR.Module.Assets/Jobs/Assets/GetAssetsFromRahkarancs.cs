using Hangfire;
using Teram.HR.Module.Assets.Services;

namespace Teram.HR.Module.Assets.Jobs.Assets
{
    public class GetAssetsFromRahkarancs
    {
        private readonly ILogger<GetAssetsFromRahkarancs> logger;
        private readonly IRecurringJobManager recurringJobManager;
        private readonly IGetAssetService getAssetService;
        private readonly IUpdateAssetsService updateAssetsService;

        public GetAssetsFromRahkarancs(ILogger<GetAssetsFromRahkarancs> logger,
            IRecurringJobManager recurringJobManager, IGetAssetService getAssetService, IUpdateAssetsService updateAssetsService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.recurringJobManager = recurringJobManager ?? throw new ArgumentNullException(nameof(recurringJobManager));
            this.getAssetService = getAssetService;
            this.updateAssetsService = updateAssetsService ?? throw new ArgumentNullException(nameof(updateAssetsService));
        }

        public void Initilize()
        {
            recurringJobManager.AddOrUpdate("RunUpdateAssetsInfo", () => UpdateAssetsInfo(), Cron.Hourly(), TimeZoneInfo.Local);
        }

        public void UpdateAssetsInfo()
        {
            try
            {
                var data = getAssetService.GetAllAssetsFromRahkaran().Result;
                var updateResult = updateAssetsService.UpdateAssets(data).Result;
            }
            catch (Exception ex)
            {
                logger.LogError(1007, ex, $"Exception Error in Job Update Assets Info Service : {Environment.NewLine} {ex.Message} ");
            }
        }
    }
}
