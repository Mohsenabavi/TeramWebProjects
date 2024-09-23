using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;

namespace Teram.HR.Module.Recruitment.Logic
{
 
    public class HSEApproveHistoryLogic : BusinessOperations<HSEApproveHistoryModel, HSEApproveHistory, int>, IHSEApproveHistoryLogic
    {
        public HSEApproveHistoryLogic(IPersistenceService<HSEApproveHistory> service) : base(service)
        {

        }

        public BusinessOperationResult<List<HSEApproveHistoryModel>> GetbyJobApplicantId(int jobApplicantId, int start, int length)
        {
          return  GetData<HSEApproveHistoryModel>(x => x.JobApplicantId == jobApplicantId, null, "HSEApproveHistoryId", true, row: start, max: length);
        }        
    }

}
