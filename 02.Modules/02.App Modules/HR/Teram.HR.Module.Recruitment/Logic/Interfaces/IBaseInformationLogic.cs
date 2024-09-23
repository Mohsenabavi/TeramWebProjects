using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;

namespace Teram.HR.Module.Recruitment.Logic.Interfaces
{
    public interface IBaseInformationLogic : IBusinessOperations<BaseInformationModel, BaseInformation, int>
    {
        BusinessOperationResult<List<BaseInformationModel>> GetBaseInformationByFilter(string firstName, string lastName, DateTime? startDateRegistration, DateTime? endDateRegistration, int? start, int? length);
        BusinessOperationResult<BaseInformationModel> GetBaseInformationById(int baseInformationId);
        BusinessOperationResult<BaseInformationModel> GetBaseInformationByNationalCode(string nationalcode);
        BusinessOperationResult<BaseInformationModel> GetByJobApplicantId(int jobApplicantId);
        BusinessOperationResult<List<BaseInformationModel>> GetByJobApplicantIds(List<int> jobApplicantIds);
    }

}
