using Teram.Framework.Core.Logic;
using Teram.Module.Authentication.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.Module.Authentication.Models
{
    public class TokenParameterModel : ModelBase<TokenParameter, int>
    {
         
        public int TokenParameterId { get; set; }
        [GridColumn("Name")]
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
        [GridColumn("Test")]
        public string Test { get; set; }
    }
}
