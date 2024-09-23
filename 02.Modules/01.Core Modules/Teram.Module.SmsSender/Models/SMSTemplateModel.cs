using Teram.Framework.Core.Logic;
using Teram.Module.SmsSender.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.Module.SmsSender.Models
{
    public class SMSTemplateModel:ModelBase<SMSTemplate,int>
    {
        public int SMSTemplateId { get; init; }

        [GridColumn(nameof(Title))]
        public string Title {  get; init; }
       
        public string Template {  get; init; }
    }
}
