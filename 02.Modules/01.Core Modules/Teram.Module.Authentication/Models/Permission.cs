using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teram.Module.Authentication.Models
{
    public class Permission
    {

        public string Path { get; set; }
        public bool HasPermission { get; set; }
    }
}
