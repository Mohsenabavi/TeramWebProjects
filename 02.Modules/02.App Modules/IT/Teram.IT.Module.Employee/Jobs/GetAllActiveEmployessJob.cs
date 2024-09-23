using Hangfire;
using Teram.IT.Module.Employee.Logic.Interface;
using Teram.IT.Module.Employee.Services;

namespace Teram.IT.Module.Employee.Jobs
{
    public class GetAllActiveEmployessJob
    {
        private readonly IRecurringJobManager recurringJobManager;
        private readonly IGetEmployessService getEmployessService;
        private readonly ILogger<GetAllActiveEmployessJob> logger;
        private readonly IHREmployeeLogic hREmployeeLogic;
        private readonly IUpdateUsersService updateUsersService;

        public GetAllActiveEmployessJob(IRecurringJobManager recurringJobManager, IGetEmployessService getEmployessService, ILogger<GetAllActiveEmployessJob> logger, IHREmployeeLogic hREmployeeLogic, IUpdateUsersService updateUsersService)
        {
            if (hREmployeeLogic is null)
            {
                throw new ArgumentNullException(nameof(hREmployeeLogic));
            }

            this.recurringJobManager=recurringJobManager??throw new ArgumentNullException(nameof(recurringJobManager));
            this.getEmployessService=getEmployessService??throw new ArgumentNullException(nameof(getEmployessService));
            this.logger=logger??throw new ArgumentNullException(nameof(logger));
            this.hREmployeeLogic=hREmployeeLogic;
            this.updateUsersService=updateUsersService??throw new ArgumentNullException(nameof(updateUsersService));
        }

        public void Initilize()
        {
            recurringJobManager.AddOrUpdate("RunUpdateEmployeesList", () => UpdateEmployeesList(), Cron.Hourly(), TimeZoneInfo.Local);
        }
        public async Task UpdateEmployeesList()
        {
            try
            {                
                var data = await getEmployessService.GetAllActiveEmployess();
                var updateResult = await hREmployeeLogic.UpdateEmployeesList(data);
                var updateUsersResult = updateUsersService.UpdateUsersList();
            }
            catch (Exception ex)
            {
                logger.LogError(1007, ex, $"Exception Error in Job Update Employee Info Service : {Environment.NewLine} {ex.Message} ");
            }
        }

    }
}
