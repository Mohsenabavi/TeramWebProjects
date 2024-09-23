using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.Module.SmsSender.Entities;
using Teram.Module.SmsSender.Logic.Interfaces;
using Teram.Module.SmsSender.Models;

namespace Teram.Module.SmsSender.Logic
{

    public class SMSHistoryLogic : BusinessOperations<SMSHistoryModel, SMSHistory, int>, ISMSHistoryLogic
    {
        public SMSHistoryLogic(IPersistenceService<SMSHistory> service) : base(service)
        {

        }
        public BusinessOperationResult<List<SMSHistoryModel>> GetSMSByRecieverNumber(string recieverNumber)
        {
            return GetData<SMSHistoryModel>(x => x.RecieverNumber.Contains(recieverNumber));
        }
    }

}
