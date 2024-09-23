using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Teram.Module.Authentication.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        //public Guid Key { get; set; }

        [Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_UserName_is_required))]
        //[RegularExpression("^([\\w-\\.\\#\\$\\^\\+\\=\\!\\*\\(\\)\\@\\%\\&\\÷\\×\\<\\>\\?]+)$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_valid_UserName))]
        [RegularExpression("^([\\w-\\.]+)$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_valid_UserName))]

        public string UserName { get; set; }
        public string Name { get; set; }

        public string ConcurrencyStamp { get; set; }

        public string SecurityStamp { get; set; }

        [RegularExpression("^\\d+$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.Controllers.UserControlPanelController), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.Controllers.UserControlPanelController.Please_enter_digit))]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        [Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Email_is_required))]
        [RegularExpression("^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_correct_Email))]
        [StringLength(150)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool LockoutEnabled { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Password_is_required))]
        [MinLength(8, ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Password_Min_Length_Error))]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_valid_Password))]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Password_is_required))]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_valid_Password))]
        [MinLength(8, ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Password_Min_Length_Error))]
        public string NewPassword { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Password_is_required))]
        [MinLength(8, ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Password_Min_Length_Error))]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_valid_Password))]
        [Compare(nameof(NewPassword), ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Password_mismatch))]
        public string ConfirmPassword { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
