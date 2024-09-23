using Microsoft.AspNetCore.Identity;
using System;

namespace Teram.Module.Authentication.Models
{
    public class TeramRole : IdentityRole<Guid>
    {
        public bool IsDefaultRole { get; set; }
        public string Title { get; set; }

    }
}
