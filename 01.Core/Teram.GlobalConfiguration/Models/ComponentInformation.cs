using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teram.Web.Models
{
    public class ComponentInformation
    {
        public string ComponentName { get; set; }
        public Dictionary<string,object> Arguments { get; set; }
    }
}
