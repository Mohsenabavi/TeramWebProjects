using System.Collections.Generic;

namespace Teram.Module.Authentication.Models
{
    public class DecodeTokenResult
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<TokenParameterModel> TokenParameterModels { get; set; }
    }
}
