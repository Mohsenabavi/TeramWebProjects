using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Teram.Module.Authentication.Models
{
    public class PasswordModel
    {
        [Required]
        public Guid UserId { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Password_is_required))]
        [MinLength(8, ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Password_Min_Length_Error))]        
        [DataType(DataType.Password)]      
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Password_is_required))]
        [MinLength(8, ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Password_Min_Length_Error))]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Password_is_required))]
        [MinLength(8, ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Password_Min_Length_Error))]
        public string ConfirmPassword { get; set; }
    }
}
