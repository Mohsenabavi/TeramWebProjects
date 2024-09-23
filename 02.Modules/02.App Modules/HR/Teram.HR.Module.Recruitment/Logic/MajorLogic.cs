using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.BaseInfo;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.BaseInfo;

namespace Teram.HR.Module.Recruitment.Logic
{

    public class MajorLogic : BusinessOperations<MajorModel, Major, int>, IMajorLogic
    {
        public MajorLogic(IPersistenceService<Major> service) : base(service)
        {

        }
    }

}
