using Teram.Web.Core.Attributes;
using Microsoft.AspNetCore.Identity;
using System;

namespace Teram.Module.Authentication.Models
{
    public class TeramUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
       // public string LastName { get; set; }
        public DateTime? LastLogin { get; set; } 
        public DateTime CreatedOn { get; set; }
        public string FullName => $"{Name}";
        public int LoginCount {  get; set; }
        public bool PassWordChanged {  get; set; }
    }
}
