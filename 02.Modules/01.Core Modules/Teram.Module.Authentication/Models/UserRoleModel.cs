
using Teram.Web.Core.Attributes;
using System;
using System.Collections.Generic;

namespace Teram.Module.Authentication.Models
{
    public class UserRoleModel
    {

        public Guid Key { get; set; }

        [GridColumn("User Name", Width = "150")]
        public string UserName { get; set; }
        [GridColumn("Name", Width = "150")]
        public string Name { get; set; }

        [GridColumn("Email", Width = "150")]
        public string Email { get; set; }
         
        [GridColumn("Roles")]
        public string Roles { get; set; }
        public List<ServiceContracts.UserRoleModel> RolesList { get; set; }


    }
}
