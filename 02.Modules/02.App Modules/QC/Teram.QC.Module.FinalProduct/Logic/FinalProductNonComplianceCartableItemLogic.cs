using System.Transactions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.WorkFlow;
using Teram.ServiceContracts;
using Teram.Web.Core.Security;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class FinalProductNonComplianceCartableItemLogic : BusinessOperations<FinalProductNonComplianceCartableItemModel, FinalProductNonComplianceCartableItem, int>, IFinalProductNonComplianceCartableItemLogic
    {
        private readonly IUserSharedService userSharedService;

        public FinalProductNonComplianceCartableItemLogic(IPersistenceService<FinalProductNonComplianceCartableItem> service,
            IUserSharedService userSharedService) : base(service)
        {
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
        }

        public BusinessOperationResult<List<FinalProductNonComplianceCartableItemModel>> GetByUserId(Guid userId)
        {
            return GetData<FinalProductNonComplianceCartableItemModel>(x => x.UserId==userId && x.OutputDate==null);
        }

        public BusinessOperationResult<List<FinalProductNonComplianceCartableItemModel>> GetByUserIdAndFinalProductNonComplianceId(Guid userId, int finalProductNonComplianceId)
        {
            return GetData<FinalProductNonComplianceCartableItemModel>(x => x.UserId==userId && x.FinalProductNoncomplianceId==finalProductNonComplianceId);
        }

        public BusinessOperationResult<List<FinalProductNonComplianceCartableItemModel>> GetByUserIdsAndNonComplianceId(List<Guid> userId, int finalProductNonComplianceId)
        {
            return GetData<FinalProductNonComplianceCartableItemModel>(x => userId.Contains(x.ReferredBy) && x.FinalProductNoncomplianceId==finalProductNonComplianceId);
        }

        public BusinessOperationResult<FinalProductNonComplianceCartableItemModel> GetLastByUserIdAndFinalProductNonComplianceId(Guid userId, int finalProductNonComplianceId)
        {
            var result = new BusinessOperationResult<FinalProductNonComplianceCartableItemModel>();
            var data = GetByUserIdAndFinalProductNonComplianceId(userId, finalProductNonComplianceId);
            var lastResult = data.ResultEntity.LastOrDefault();
            if (lastResult != null)
            {
                result.SetSuccessResult(lastResult);
            }
            else
            {
                result.SetErrorMessage("not Found");
            }
            return result;
        }

        public BusinessOperationResult<List<FinalProductNonComplianceCartableItemModel>> GetSalesUniCartableItems(int finalProductNonComplianceId)
        {
            var salesUsers = userSharedService.GetUsersInRole("Sales").Result;
            var userIds = salesUsers.Select(x => x.UserId).ToList();
            var data = GetByUserIdsAndNonComplianceId(userIds, finalProductNonComplianceId);
            var SourceUserIds = data.ResultEntity.Select(x => x.ReferredBy).ToList();
            var destinationUserIds = data.ResultEntity.Select(x => x.UserId).ToList();

            userIds.AddRange(SourceUserIds);
            userIds.AddRange(destinationUserIds);

            var usersInfo = userSharedService.GetUserInfos(userIds);

            foreach (var cartableItem in data.ResultEntity)
            {
                var sourceUserInfo = usersInfo.Where(x => x.UserId==cartableItem.ReferredBy).FirstOrDefault();
                var destinationUsers = usersInfo.Where(x => x.UserId==cartableItem.UserId).FirstOrDefault();
                if (sourceUserInfo!=null)
                {
                    cartableItem.SourceUserName=$"{sourceUserInfo.Name} {sourceUserInfo.Family}";
                }
                if (destinationUsers!=null)
                {
                    cartableItem.DestinationUserName=$"{destinationUsers.Name} {destinationUsers.Family}";
                }
            }
            return data;
        }
    }
}
