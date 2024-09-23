using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.Web.Core.Security;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class FinalProductNoncomplianceDetailLogic : BusinessOperations<FinalProductNoncomplianceDetailModel, FinalProductNoncomplianceDetail, int>, IFinalProductNoncomplianceDetailLogic
    {
        private readonly IUserPrincipal userPrincipal;
        private readonly IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic;

        public FinalProductNoncomplianceDetailLogic(IPersistenceService<FinalProductNoncomplianceDetail> service, IUserPrincipal userPrincipal,
            IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic) : base(service)
        {
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.finalProductNoncomplianceLogic=finalProductNoncomplianceLogic??throw new ArgumentNullException(nameof(finalProductNoncomplianceLogic));
            BeforeAdd+=FinalProductNoncomplianceDetailLogic_BeforeAdd;
        }

        private void FinalProductNoncomplianceDetailLogic_BeforeAdd(TeramEntityEventArgs<FinalProductNoncomplianceDetail, FinalProductNoncomplianceDetailModel, int> entity)
        {
            entity.NewEntity.CreatedBy=userPrincipal.CurrentUserId;
            entity.NewEntity.CreateDate=DateTime.Now;
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductInspectionId(int finalProductInspectionId)
        {
            return GetData<FinalProductNoncomplianceDetailModel>(x => x.FinalProductInspectionId==finalProductInspectionId);
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductNoncomplianceIdsAndFinalProductInspectionId(List<int> finalProductNoncomplianceIds, int finalProductInspectionId)
        {
            return GetData<FinalProductNoncomplianceDetailModel>(x => finalProductNoncomplianceIds.Contains(x.FinalProductNoncomplianceId) && x.FinalProductInspectionId==finalProductInspectionId);
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductNoncomplianceIdsAndPalletNumber(List<int> finalProductNoncomplianceIds, int palletNumber)
        {
            return GetData<FinalProductNoncomplianceDetailModel>(x => finalProductNoncomplianceIds.Contains(x.FinalProductNoncomplianceId) && x.Number==palletNumber);
        }

        public BusinessOperationResult<FinalProductNoncomplianceDetailModel> GetByPalletNumberAndControlPlanDefectId(int palletNumber, int controlPlanDefcetId)
        {
            var relatedFinalProductNoncompliancesResult = finalProductNoncomplianceLogic.GetByControlPlanDefctId(controlPlanDefcetId);

            var relatedFinalProductNoncompliancesResultIds = relatedFinalProductNoncompliancesResult.ResultEntity.Select(x => x.FinalProductNoncomplianceId).ToList();

            return GetFirst<FinalProductNoncomplianceDetailModel>(x => x.Number==palletNumber && relatedFinalProductNoncompliancesResultIds.Contains(x.FinalProductNoncomplianceId));
        }
        public BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductNoncomplianceId(int finalProductNoncomplianceId)
        {
            return GetData<FinalProductNoncomplianceDetailModel>(x => x.FinalProductNoncomplianceId==finalProductNoncomplianceId);
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductInspectionIds(List<int> finalProductInspectionIds)
        {
            return GetData<FinalProductNoncomplianceDetailModel>(x => finalProductInspectionIds.Contains(x.FinalProductInspectionId.Value));
        }
    }

}
