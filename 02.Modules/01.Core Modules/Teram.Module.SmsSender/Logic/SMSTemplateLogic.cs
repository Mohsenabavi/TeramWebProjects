using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.Module.SmsSender.Entities;
using Teram.Module.SmsSender.Logic.Interfaces;
using Teram.Module.SmsSender.Models;

namespace Teram.Module.SmsSender.Logic
{  
    public class SMSTemplateLogic : BusinessOperations<SMSTemplateModel, SMSTemplate, int>, ISMSTemplateLogic
    {
        public SMSTemplateLogic(IPersistenceService<SMSTemplate> service) : base(service)
        {

        }
    }

}
