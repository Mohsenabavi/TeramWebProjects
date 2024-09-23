using Teram.Framework.Core.Logic;
using Teram.Module.SmsSender.Entities;
using Teram.Module.SmsSender.Models;

namespace Teram.Module.SmsSender.Logic.Interfaces
{
    public interface ISMSHistoryLogic : IBusinessOperations<SMSHistoryModel, SMSHistory, int>
    {
        BusinessOperationResult<List<SMSHistoryModel>> GetSMSByRecieverNumber(string recieverNumber);
    }

}
