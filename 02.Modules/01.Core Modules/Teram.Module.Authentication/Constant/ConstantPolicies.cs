using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teram.Module.Authentication.Constant
{
    public static class ConstantPolicies
    {
        public static readonly string RolePermission = nameof(RolePermission);
        public static readonly string Permission = nameof(Permission);
        public static readonly string PersonTypeClaim = nameof(PersonTypeClaim);
        public static readonly string NationalCodeClaim = nameof(NationalCodeClaim);
    }
}
