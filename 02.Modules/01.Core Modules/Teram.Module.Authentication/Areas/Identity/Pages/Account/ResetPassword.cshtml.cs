using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Teram.Module.Authentication.Models;
using Microsoft.Extensions.Localization;

namespace Teram.Module.Authentication.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<TeramUser> _userManager;
        private readonly IStringLocalizer<AuthenticationSharedResource> localizer1;

        public ResetPasswordModel(UserManager<TeramUser> userManager, IStringLocalizer<AuthenticationSharedResource> localizer1)
        {
            _userManager = userManager;
            this.localizer1 = localizer1;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Email_is_required))]
            [RegularExpression("^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_correct_Email))]
            [StringLength(150)]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [DataType(DataType.Password)]
            //[Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Password_is_required))]
           // [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_valid_Password))]
           // [MinLength(8, ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Password_Min_Length_Error))]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "ConfirmPassword")]
            [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_valid_Password))]
            [Compare("Password", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Password_mismatch))]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }
        [HttpGet]
        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest(localizer1["A code must be supplied for password reset."]);
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
        }
        [HttpPost("OnPostAsync")]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
               return new JsonResult(new { result = "redirect", url = $"/Identity/Account/ResetPasswordConfirmation", message = "", title = "" });
               // return RedirectToPage("./ResetPasswordConfirmation");
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, code, Input.Password);
            if (result.Succeeded)
            {
                return new JsonResult(new { result = "redirect", url = $"/Identity/Account/ResetPasswordConfirmation", message = "", title = "" });
            }

            else
            {
                var message = result.Errors.Select(x => x.Description).Aggregate((x, c) => x + Environment.NewLine + c);
                return new JsonResult(new { result = "fail", message = message, title = "" });
            }

            return Page();
        }
        [AllowAnonymous]
        public string CallBackHome()
        {
            return "/Index";
        }
    }

}
