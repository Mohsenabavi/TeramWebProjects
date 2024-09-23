using Hangfire;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models.ServiceModels;
using Teram.QC.Module.FinalProduct.Services;
using Teram.ServiceContracts;

namespace Teram.QC.Module.FinalProduct.Jobs
{
    public class GetAllPersonnelJob
    {
        private readonly IRecurringJobManager recurringJobManager;
        private readonly IQueryService queryService;
        private readonly ILogger<GetAllPersonnelJob> logger;
        private readonly IOperatorLogic operatorLogic;
        private readonly IUserSharedService userSharedService;

        public GetAllPersonnelJob(IRecurringJobManager recurringJobManager, IQueryService queryService,
            ILogger<GetAllPersonnelJob> logger, IOperatorLogic operatorLogic, IUserSharedService userSharedService)
        {
            this.recurringJobManager=recurringJobManager??throw new ArgumentNullException(nameof(recurringJobManager));
            this.queryService=queryService??throw new ArgumentNullException(nameof(queryService));
            this.logger=logger??throw new ArgumentNullException(nameof(logger));
            this.operatorLogic=operatorLogic??throw new ArgumentNullException(nameof(operatorLogic));
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
        }

        public void Initilize()
        {
            recurringJobManager.AddOrUpdate("RunUpdateEmployeesList", () => UpdateOperatorList(), Cron.Hourly(), TimeZoneInfo.Local);
        }
        public async Task UpdateOperatorList()
        {
            try
            {
                var data = await queryService.GetActiveEmployees();
                var updateResult = await operatorLogic.UpdateEmployeesList(data.ResultEntity);
                var updateUserInfo = CreateUsers(data.ResultEntity);
            }
            catch (Exception ex)
            {
                logger.LogError(1007, ex, $"Exception Error in Job Update Employee Info Service : {Environment.NewLine} {ex.Message} ");
            }
        }
        public bool CreateUsers(List<EmployeeModel> employees)
        {
            try
            {
                foreach (var employee in employees)
                {

                    var userInformation = new UserInfo();

                    var existUserInfo = userSharedService.GetUserInfo(employee.NationalID);

                    if (!existUserInfo.Any() && existUserInfo.Count == 0)
                    {
                        var userInfoModel = new UserInfo { Name=$"{employee.FirstName} {employee.LastName}", IsActive=true, Username=employee.NationalID, PhoneNumber=employee.Mobile };

                        var result = userSharedService.CreateUserAsync(userInfoModel, employee.NationalID).Result;

                        if (result.Succeeded)
                        {
                            var createdUserInfo = userSharedService.GetUserInfo(employee.NationalID);
                            userInformation = createdUserInfo.FirstOrDefault();
                            var roleResult = userSharedService.AddToRoleAsync(userInformation, "Operator").Result;
                        }
                    }
                    else
                    {

                        var rolesOfUser =  userSharedService.GetRolesOfUser(existUserInfo.FirstOrDefault()).Result;
                        if (!rolesOfUser.Contains("Operator"))
                        {
                            var roleResult = userSharedService.AddToRoleAsync(existUserInfo.FirstOrDefault(), "Operator").Result;
                        }
                    }
                    userInformation=existUserInfo.FirstOrDefault();
                    var operatorResult = operatorLogic.GetByNationalId(employee.NationalID);
                    operatorResult.ResultEntity.UserId=userInformation?.UserId;
                    var updateUserId = operatorLogic.Update(operatorResult.ResultEntity);
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.InnerException, ex.Message, ex);
                return false;
            }
        }
    }
}
