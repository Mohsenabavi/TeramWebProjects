
using Teram.Web.Core.Attributes;
using System;

namespace Teram.Module.Authentication.Models
{
    public class UserModel
    {
        public Guid Key { get; set; }

        [GridColumn("Name")]
        public string Name { get; set; }

        [GridColumn("User Name")]
        public string UserName { get; set; }

        [GridColumn("Email")]
        public string Email { get; set; }

        [GridColumn("Phone Number")]
        public string PhoneNumber { get; set; }


        public bool EmailConfirmed { get; set; }
        [GridColumn("Email is Confirmed")]
        public string EmailConfirmedText
        {
            get
            {
                return EmailConfirmed ? "دارد" : "ندارد";
            }
        }

        public bool PhoneNumberConfirmed { get; set; }

        [GridColumn("PhoneNumber is Confirmed")]
        public string PhoneNumberConfirmedText
        {
            get
            {
                return PhoneNumberConfirmed ? "دارد" : "ندارد";
            }
        }

        public bool Lockout { get; set; }

        [GridColumn("Lockout")] 
        public string LockoutText
        {
            get
            {
                return Lockout ? "دارد" : "ندارد";
            }
        }
    }
}
