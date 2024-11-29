using System.Linq.Expressions;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Module.EmailSender.Services;
using Teram.Module.EmailSender.Services.Interfaces;
using Teram.Module.SmsSender.Services;
using Teram.QC.Module.FinalProduct.Entities.WorkFlow;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.WorkFlow;
using Teram.ServiceContracts;
using Teram.Web.Core.Security;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class ManageCartableLogic : IManageCartableLogic
    {
        private readonly IUserSharedService userSharedService;
        private readonly IUserPrincipal userPrincipal;
        private readonly IFlowInstructionLogic flowInstructionLogic;
        private readonly INotificationService notificationService;
        private readonly IEmailService emailService;
        private readonly ISendAsiaSMSService sendAsiaSMSService;
        private readonly IRoleSharedService roleSharedService;
        private readonly IFinalProductNonComplianceCartableItemLogic finalProductNonComplianceCartableItemLogic;

        public ManageCartableLogic(IUserSharedService userSharedService, IUserPrincipal userPrincipal, IFlowInstructionLogic flowInstructionLogic,
            INotificationService notificationService, IEmailService emailService, ISendAsiaSMSService sendAsiaSMSService,
            IRoleSharedService roleSharedService, IFinalProductNonComplianceCartableItemLogic finalProductNonComplianceCartableItemLogic)
        {
            this.userSharedService = userSharedService ?? throw new ArgumentNullException(nameof(userSharedService));
            this.userPrincipal = userPrincipal ?? throw new ArgumentNullException(nameof(userPrincipal));
            this.flowInstructionLogic = flowInstructionLogic ?? throw new ArgumentNullException(nameof(flowInstructionLogic));
            this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            this.sendAsiaSMSService = sendAsiaSMSService ?? throw new ArgumentNullException(nameof(sendAsiaSMSService));
            this.roleSharedService = roleSharedService ?? throw new ArgumentNullException(nameof(roleSharedService));
            this.finalProductNonComplianceCartableItemLogic = finalProductNonComplianceCartableItemLogic ?? throw new ArgumentNullException(nameof(finalProductNonComplianceCartableItemLogic));
        }

        public BusinessOperationResult<FlowInstructionModel> GetNextStep(FinalProductNoncomplianceModel model)
        {
            var result = new BusinessOperationResult<FlowInstructionModel>();
            var userMainRole = GetUserMainRole();
            if (userMainRole.ResultStatus != OperationResultStatus.Successful || userMainRole.ResultEntity is null)
            {
                result.SetErrorMessage("User Role Not Found");
                return result;
            }
            var data = flowInstructionLogic.GetByFiler(userMainRole.ResultEntity.Id, model);
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            var nextCartableRoleId = data.ResultEntity.NextCartableRoleId;
            var relatedRole = roleSharedService.GetRoleById(nextCartableRoleId).Result;
            var userInRole = userSharedService.GetUsersInRole(relatedRole.Name).Result;
            data.ResultEntity.DestinationUsers = userInRole;
            return data;
        }

        public BusinessOperationResult<List<FinalProductNonComplianceCartableItemModel>> AddToCartable(FlowInstructionModel flowInstructionModel, FinalProductNoncomplianceModel finalProductNoncomplianceModel, List<int>? sampleIds = null, Guid? destinationUserId = null)
        {
            var result = new BusinessOperationResult<List<FinalProductNonComplianceCartableItemModel>>();
            var destinationUserIds = new List<Guid>();

            var cartableItemsOfCurrentUser = finalProductNonComplianceCartableItemLogic.GetByUserIdAndFinalProductNonComplianceId(userPrincipal.CurrentUserId, finalProductNoncomplianceModel.FinalProductNoncomplianceId);

            foreach (var cartableItem in cartableItemsOfCurrentUser.ResultEntity)
            {
                cartableItem.OutputDate = DateTime.Now;
                finalProductNonComplianceCartableItemLogic.Update(cartableItem);
            }

            var listOfCartableItems = new List<FinalProductNonComplianceCartableItemModel>();

            if (destinationUserId != null)
            {
                var userMainRole = GetUserMainRole(destinationUserId.Value);
                var mainRoleClaims = roleSharedService.GetClaimsAsync(userMainRole.ResultEntity).Result;

                if (mainRoleClaims.Any(x => x.Value == ":FinalProductNoncompliance:EmailReciverPermission"))
                {
                    //var emialContext = notificationService.GenerateEmailNotoification(finalProductNoncomplianceModel, destinationUser);
                    //emailService.SendEmail($"ورود عدم انطباق محصول نهایی به کارتابل - {DateTime.Now.ToPersianDate()}", emialContext, DateTime.Now, destinationUser.Email);
                }

                if (mainRoleClaims.Any(x => x.Value == ":FinalProductNoncompliance:SMSReciverPermission"))
                {
                    //var smsContext = notificationService.GenerateSMSNotoification(finalProductNoncomplianceModel, destinationUser);
                    //sendAsiaSMSService.SendMessage(new Teram.Module.SmsSender.Models.AsiaSms.SendSmsModel
                    //{
                    //    Receivers=destinationUser.PhoneNumber,
                    //    SmsText=smsContext
                    //});
                }

                listOfCartableItems.Add(new FinalProductNonComplianceCartableItemModel
                {
                    InputDate = DateTime.Now,
                    FinalProductNoncomplianceId = finalProductNoncomplianceModel.FinalProductNoncomplianceId,
                    ReferredBy = userPrincipal.CurrentUserId,
                    UserId = destinationUserId.Value,
                });
            }
            else
            {
                foreach (var nextCartableUser in flowInstructionModel.DestinationUsers)
                {
                    var userMainRole = GetUserMainRole(nextCartableUser.UserId);
                    var mainRoleClaims = roleSharedService.GetClaimsAsync(userMainRole.ResultEntity).Result;

                    if (mainRoleClaims.Any(x => x.Value == ":FinalProductNoncompliance:EmailReciverPermission"))
                    {
                        var emialContext = notificationService.GenerateEmailNotoification(finalProductNoncomplianceModel, nextCartableUser);
                        emailService.SendEmail($"ورود عدم انطباق محصول نهایی به کارتابل - {DateTime.Now.ToPersianDate()}", emialContext, DateTime.Now, nextCartableUser.Email);
                    }

                    if (mainRoleClaims.Any(x => x.Value == ":FinalProductNoncompliance:SMSReciverPermission"))
                    {
                        var smsContext = notificationService.GenerateSMSNotoification(finalProductNoncomplianceModel, nextCartableUser);
                        sendAsiaSMSService.SendMessage(new Teram.Module.SmsSender.Models.AsiaSms.SendSmsModel
                        {
                            Receivers = nextCartableUser.PhoneNumber,
                            SmsText = smsContext
                        });
                    }
                    listOfCartableItems.Add(new FinalProductNonComplianceCartableItemModel
                    {
                        InputDate = DateTime.Now,
                        FinalProductNoncomplianceId = finalProductNoncomplianceModel.FinalProductNoncomplianceId,
                        ReferredBy = userPrincipal.CurrentUserId,
                        UserId = nextCartableUser.UserId
                    });
                }
            }
            foreach (var cartableItem in listOfCartableItems)
            {
                cartableItem.Comments = (finalProductNoncomplianceModel.LastComment != null) ? finalProductNoncomplianceModel.LastComment : " ";
                var addResult = finalProductNonComplianceCartableItemLogic.AddNew(cartableItem);
            }
            result.SetSuccessResult(listOfCartableItems);

            return result;
        }



        public BusinessOperationResult<RoleInfo> GetUserMainRole()
        {
            var result = new BusinessOperationResult<RoleInfo>();

            try
            {
                var userRoleIds = userSharedService.GetRoleIdsUser(userPrincipal.CurrentUserId).Result;
                var userRoles = roleSharedService.GetRoleByListRoleId(userRoleIds);
                var mainRoleOfUsser = userRoles.
                    Where(x => x.NormalizedName != "MEMBERS"
                    && x.NormalizedName != "OPERATIONMANAGER"
                    && x.NormalizedName != "REPORTVIEWER"
                    && x.NormalizedName != "QA"
                    && x.NormalizedName != "ADMINISTRATORS").FirstOrDefault();
                if (mainRoleOfUsser != null)
                {
                    result.SetSuccessResult(mainRoleOfUsser);
                }
                else
                {
                    result.SetErrorMessage("Role Not Found");
                }

                return result;
            }
            catch (Exception ex)
            {
                result.SetErrorMessage("Error In Get User Roles" + ex.Message + ex.InnerException);
                return result;
            }

        }

        public BusinessOperationResult<RoleInfo> GetUserMainRole(Guid userId)
        {
            var result = new BusinessOperationResult<RoleInfo>();

            try
            {
                var userRoleIds = userSharedService.GetRoleIdsUser(userId).Result;
                var userRoles = roleSharedService.GetRoleByListRoleId(userRoleIds);
                var mainRoleOfUsser = userRoles.Where(x => x.NormalizedName != "MEMBERS" && x.NormalizedName != "ADMINISTRATORS").FirstOrDefault();
                if (mainRoleOfUsser != null)
                {
                    result.SetSuccessResult(mainRoleOfUsser);
                }
                else
                {
                    result.SetErrorMessage("Role Not Found");
                }
                return result;
            }
            catch (Exception ex)
            {
                result.SetErrorMessage("Error In Get User Roles" + ex.Message + ex.InnerException);
                return result;
            }

        }


        private Expression<Func<FlowInstruction, bool>> CreateFilterExpression(Guid fromRoleId, FinalProductNoncomplianceModel model)
        {
            Expression<Func<FlowInstruction, bool>> query = x => true;

            query = query.AndAlso(x => x.FromStatus == model.ReferralStatus && x.CurrentCartableRoleId == fromRoleId);

            if (model.IsApproved.HasValue)
            {

                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "IsApproved" && condition.FieldValue.ToLower() == model.IsApproved.Value.ToString().ToLower()));
            }

            if (model.HasFinalResult.HasValue)
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "HasFinalResult" && condition.FieldValue.ToLower() == model.HasFinalResult.Value.ToString().ToLower()));
            }

            if (model.HasSeperationOrder.HasValue)
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "HasSeperationOrder" && condition.FieldValue.ToLower() == model.HasSeperationOrder.Value.ToString().ToLower()));
            }


            if (model.HasWasteOrder.HasValue)
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "HasWasteOrder" && condition.FieldValue.ToLower() == model.HasWasteOrder.Value.ToString().ToLower()));
            }

            if (model.IsSeperated.HasValue)
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "IsSeperated" && condition.FieldValue.ToLower() == model.IsSeperated.Value.ToString().ToLower()));
            }

            if (model.NeedToAdvisoryOpinion.HasValue)
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "NeedToAdvisoryOpinion" && condition.FieldValue.ToLower() == model.NeedToAdvisoryOpinion.Value.ToString().ToLower()));
            }

            if (model.NeedToCkeckByOther.HasValue)
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "NeedToCkeckByOther" && condition.FieldValue.ToLower() == model.NeedToCkeckByOther.Value.ToString().ToLower()));
            }

            if (model.NeedToRefferToCEO.HasValue)
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "NeedToRefferToCEO" && condition.FieldValue.ToLower() == model.NeedToRefferToCEO.Value.ToString().ToLower()));
            }
            return query;
        }
    }
}
