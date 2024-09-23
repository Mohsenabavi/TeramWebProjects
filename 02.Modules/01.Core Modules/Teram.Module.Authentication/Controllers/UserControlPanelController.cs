using Teram.Framework.Core.Tools;
using Teram.Module.Authentication.Models;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Exceptions;
using Teram.Web.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.IO;
using Teram.Framework.Core.Extensions;

namespace Teram.Module.Authentication.Controllers
{
    [Display(Description = "Users")]

    public class UserControlPanelController : BasicControlPanelController
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IUserSharedService userSharedService;
        private readonly ISignInSharedService signInSharedService;

        public UserControlPanelController(IServiceProvider serviceProvider, ILogger<UserControlPanelController> logger,
            IStringLocalizer<UserControlPanelController> localizer, IStringLocalizer<SharedResource> sharedlocalizer,
            IUserSharedService userSharedService, ISignInSharedService signInSharedService)
        {
            this.serviceProvider = serviceProvider;
            this.userSharedService = userSharedService;
            this.signInSharedService = signInSharedService;
            this.logger = logger;
            this.sharedLocalizer = sharedlocalizer;
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            ViewBag.PageName = localizer["Users"];
            Model = new ViewInformation<UserModel>(true)
            {
                Title = localizer["Users"],
                HomePage = "/UserControlPanel/Index",
                GridId = "UserControlPanelGrid",
                LoadDefaultSetting = false,
                LoadAjaxData = false,
                GetDataUrl = $"",
                ExtraScripts = "/ExternalModule/Module/Authentication/Scripts/UserControlPanel.js",
                HasGrid = true,
                EditInSamePage = true,
                HasToolbar = true,
                ToolbarName = "_adminToolbar",
            };

        }
        [ControlPanelMenu("SignOut", Icon = "fa-sign-out", PanelType = PanelType.Managment | PanelType.User,
            Position = Web.Core.Enums.ControlPanelMenuPosition.LeftNavbar, Order = 1)]
        [AllowAnonymous]
        public async Task<IActionResult> SignOut()
        {
            await signInSharedService.SignOutAsync();
            return Redirect("/");
        }

        [Display(Description = "ShowPage")]
        [ControlPanelMenu("Users", ParentName = "Security", Icon = "fa-users", PanelType = PanelType.Managment, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar, Order = 4)]
        public IActionResult Index()
        {
            Model.ModelData = new UserViewModel();
            return View("Index", Model);
        }

        [HttpPost]
        [ParentalAuthorize(nameof(Index))]
        public virtual IActionResult GetData(DatatablesSentModel model, string name, string userName, string email, string phoneNumber)
        {
            var users = userSharedService.GetUsers(name ?? userName ?? email ?? phoneNumber ?? "", model.Start, model.Length);
            if (!string.IsNullOrWhiteSpace(name))
            {
                users = users.Where(x => x.Name == name).ToList();
            }
            if (!string.IsNullOrWhiteSpace(userName))
            {
                users = users.Where(x => x.Username == userName).ToList();
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                users = users.Where(x => x.Email == email).ToList();
            }
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                users = users.Where(x => x.PhoneNumber == phoneNumber).ToList();
            }

            var data = users.Select(x => new UserModel
            {
                Key = x.UserId,
                UserName = x.Username,
                Name = x.Name,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                PhoneNumber = x.PhoneNumber,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
            }).ToList();

            if (data == null) return null;
            var count = userSharedService.GetUserInfo(name ?? userName ?? email ?? phoneNumber ?? "").Count();
            return Json(new { model.Draw, recordsTotal = count, recordsFiltered = count, data, error = "" });
        }





        [Display(Description = "Edit")]
        public async Task<IActionResult> EditPartialAsync(Guid? id)
        {
            if (id is null || id == Guid.Empty)
                return PartialView("Add", new UserViewModel());

            var userInfo = await LoadData(id.Value);
            var userViewModel = new UserViewModel
            {
                Id = userInfo.UserId,
                UserName = userInfo.Username,
                PhoneNumber = userInfo.PhoneNumber,
                PhoneNumberConfirmed = userInfo.PhoneNumberConfirmed,
                Email = userInfo.Email,
                EmailConfirmed = userInfo.EmailConfirmed,
                Name = userInfo.Name,
            };


            Model.ModelData = userViewModel;
            return PartialView("Add", Model.ModelData);
        }


        [ParentalAuthorize(nameof(Index))]
        private async Task<UserInfo> LoadData(Guid key)
        {
            var userInfo = await userSharedService.GetUserById(key);

            if (userInfo == null)
                throw new UIException(localizer["User not found"]);


            Model.ModelData = userInfo;
            Model.Key = key;
            return userInfo;
        }


        [AllowAnonymous]
        public IActionResult GetAllUsersByFilter(string term)
        {
            var people = new List<SelectListItem>();
            var users = userSharedService.GetUserInfo("");
            if (users is not null)
            {
                people = users.Where(x => x.Username.Contains(term) || x.Name.Contains(term) || x.Email.Contains(term) || x.PhoneNumber.Contains(term))
                    .Select(x => new SelectListItem { Value = x.UserId.ToString(), Text = x.Username + " _ " + x.Name }).ToList();
            }
            return Json(new { results = people.ToList() });
        }


        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> Detail(Guid id)
        {
            await LoadData(id);
            return View("ItemDetail", Model);
        }

        [HttpDelete]
        [Display(Description = "Remove")]
        public async Task<IActionResult> Remove(Guid key)
        {
            try
            {
                var result = await userSharedService.DeleteUser(key);
                if (!result.Succeeded)
                {
                    var message = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
                    return Json(new { result = "fail", message = message });
                }

                return Json(new { result = "ok", message = sharedLocalizer["Your record removed successfully."] });
            }
            catch (Exception ex)
            {
                var message = ExceptionParser.Parse(ex);
                logger.LogError(message, ex);
                return Json(new { result = "fail", message = sharedLocalizer["I couldn't remove your record"] });
            }
        }

        [HttpPost]
        [Display(Description = "Save")]
        public async Task<IActionResult> Save(UserViewModel model)
        {
            try
            {
                IdentityResult result;
                IdentityResult passwordResult;
                var userInfo = new UserInfo();
                if (String.IsNullOrEmpty(model.UserName))
                {
                    var message = localizer["UserName is Required"];
                    return Json(new { result = "fail", message = message, title = localizer["UserName is Required"] });
                }
                if (String.IsNullOrEmpty(model.Email) && model.EmailConfirmed)
                {
                    var message = localizer["Email is Required"];
                    return Json(new { result = "fail", message = message, title = localizer["Email is Required"] });
                }
                if (String.IsNullOrEmpty(model.PhoneNumber) && model.PhoneNumberConfirmed)
                {
                    var message = localizer["PhoneNumber Is Required For Confirm"];
                    return Json(new { result = "fail", message = message, title = localizer["PhoneNumber Is Required For Confirm"] });
                }

                if (ModelState.IsValid)
                {
                    if (model.Id == Guid.Empty)
                    {// ایجاد
                        userInfo = new UserInfo
                        {
                            PhoneNumber = model.PhoneNumber,
                            EmailConfirmed = string.IsNullOrEmpty(model.Email) ? false : model.EmailConfirmed,
                            PhoneNumberConfirmed = string.IsNullOrEmpty(model.PhoneNumber) ? false : model.PhoneNumberConfirmed,
                            Email = model.Email,
                            Username = model.UserName,
                            Name = model.Name,
                            CreatedOn = DateTime.Now
                        };

                        if (String.IsNullOrEmpty(model.NewPassword) || String.IsNullOrEmpty(model.ConfirmPassword))
                        {
                            var message = localizer["Password and Confirm Password are Required"];
                            return Json(new { result = "fail", message = message, title = localizer["UserName is Required"] });
                        }
                        result = await userSharedService.CreateUserAsync(userInfo, model.NewPassword);
                    }
                    else
                    {// ویرایش
                        userInfo = await LoadData(model.Id);

                        userInfo.PhoneNumber = model.PhoneNumber;
                        userInfo.EmailConfirmed = string.IsNullOrEmpty(model.Email) ? false : model.EmailConfirmed;
                        userInfo.PhoneNumberConfirmed = string.IsNullOrEmpty(model.PhoneNumber) ? false : model.PhoneNumberConfirmed;
                        userInfo.Email = model.Email;
                        userInfo.Username = model.UserName;
                        userInfo.Name = model.Name;

                        result = await userSharedService.UpdateUser(userInfo);

                    }

                    // برای اینکه چک کنم اگر یکی از مقادیر پسورد را وارد کرده بقیه را هم وارد کنه 
                    int emptyCount =
                       (String.IsNullOrEmpty(model.NewPassword) ? 1 : 0) +
                       (String.IsNullOrEmpty(model.OldPassword) ? 1 : 0) +
                       (String.IsNullOrEmpty(model.ConfirmPassword) ? 1 : 0);

                    if (result.Succeeded)
                    {
                        if (model.Id != Guid.Empty)
                        { /// ویرایش
                            if (emptyCount > 0 && emptyCount != 3)
                            {
                                // اگر یکی از پسوردها را وارد کرده باید همه را وارد کند
                                var message = localizer["OldPassword,NewPasswor,ConfirmPassword are required"];
                                return Json(new { result = "fail", message = message, title = localizer["all Passwords is Required"] });

                            }
                            else if (model.NewPassword?.Length > 0 && model.OldPassword?.Length > 0 && model.ConfirmPassword?.Length > 0)
                            {
                                passwordResult = await userSharedService.ChangePassword(userInfo.UserId, model.OldPassword, model.NewPassword);
                                if (passwordResult.Succeeded)
                                {// پیغام موفقیت تغییر یوزر و تغییر رمز
                                    return Json(new { result = "ok", message = sharedLocalizer["Your data has been saved"], title = localizer["SaveTitle"] });
                                }
                                else
                                {// خطای تغییر رمز
                                    var message = passwordResult.Errors.Select(x => x.Description).Aggregate((x, c) => x + Environment.NewLine + c);
                                    return Json(new { result = "fail", message = message, title = localizer["Password not changed"] });
                                }
                            }
                        }

                        return Json(new { result = "ok", message = sharedLocalizer["Your data has been saved"], title = sharedLocalizer["SaveTitle"] });

                    }
                    else
                    {// خطای ثبت یوزر
                        var message = result.Errors.Select(x => x.Description).Aggregate((x, c) => x + Environment.NewLine + c);
                        return Json(new { result = "fail", message = message, title = sharedLocalizer["Something wrong"] });
                    }

                }
                else
                    return Json(new { result = "fail", message = "Model State Error", title = sharedLocalizer["Something wrong"] });
            }
            catch (Exception ex)
            {
                var message = ExceptionParser.Parse(ex);
                logger.LogError(new EventId(500), message, ex);
                throw ex;
            }
        }

        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> ReConfirm([FromServices] IEmailSender emailSender,
            [FromServices] HtmlTemplateParser htmlTemplateParser, string userEmail)
        {
            try
            {
                var user = userSharedService.GetInfoByEmail(userEmail);
                var code = await userSharedService.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.UserId, code = code },
                    protocol: Request.Scheme);

                var values = new Dictionary<string, string>
                    {
                        { "token", HtmlEncoder.Default.Encode(callbackUrl) },
                    };


                await emailSender.SendEmailAsync(userEmail, "Confirm your email", htmlTemplateParser.Parse("Email", "EmailConfirmation", CultureInfo.CurrentCulture, values.ToArray()));
                return Json(new { result = "ok" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "fail", message = ex.Message });

            }
        }


        /// <summary>
        /// وقتی روی دکمه تغییر رمز عبور کلیک میکند صفحه تغییر رمز باز شود
        /// دکمه را حذف کردیم و فعلا از این متد استفاده نمیشود
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ParentalAuthorize("Edit")]
        public async Task<IActionResult> ChangePasswordAsync(Guid userId)
        {
            var userInfo = await LoadData(userId);
            var passwordModel = new PasswordModel
            {
                UserId = userInfo.UserId,
                UserName = userInfo.Username,

            };
            return PartialView("ChangePassword", passwordModel);
        }

        /// <summary>
        /// متد ذخیره پسورد در صفحه ی تغییر پسورد
        /// الان استفاده نمیشود
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="model"></param>
        /// <param name="passwordModel"></param>
        /// <returns></returns>
        [ParentalAuthorize("Edit")]
        public async Task<IActionResult> SavePassword(PasswordModel model, PasswordModel passwordModel)
        {
            try
            {
                var userInfo = await LoadData(model.UserId);

                var result = await userSharedService.ChangePassword(userInfo.UserId, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    return Json(new { result = "ok", message = localizer["Your data has been saved"], title = localizer["SaveTitle"] });
                }
                else
                {
                    var message = result.Errors.Select(x => x.Description).Aggregate((x, c) => x + Environment.NewLine + c);
                    return Json(new { result = "fail", message = message, title = localizer["Something wrong"] });
                }

            }
            catch (Exception ex)
            {
                return Json(new { result = "fail", message = ex.Message });

            }
        }


        [ParentalAuthorize("Edit")]
        public async Task<IActionResult> ResetPassword(PasswordModel model)
        {
            try
            {
                var userInfo = await LoadData(model.UserId);

                // TeramUser.PasswordHash = userManager.PasswordHasher.HashPassword(TeramUser, "Abc@123456");
                var result = await userSharedService.UpdateUser(userInfo);

                if (result.Succeeded)
                {
                    return Json(new { result = "ok", message = localizer["Password set to Abc@123456"], title = localizer["SaveTitle"] });
                }
                else
                {
                    var message = result.Errors.Select(x => x.Description).Aggregate((x, c) => x + Environment.NewLine + c);
                    return Json(new { result = "fail", message = message, title = localizer["Something wrong"] });
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "fail", message = ex.Message });

            }
        }


        [HttpPost]
        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> ImportFromExcel()
        {
            try
            {
                if (!Request.Form.Files.Any())
                {
                    return Json(new { Result = "fail", message = "هیچ فایلی انتخاب نشده است" });
                }
                var file = Request.Form.Files[0];
                var usersList = new List<ImportUserModel>();
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                usersList = usersList.ImportFromExcel(ms).ToList();
                foreach (var user in usersList)
                {
                    var userInfo = new UserInfo
                    {
                        Username = user.NationalCode,
                        Name = $"{user.Name} {user.Family}",
                        Family = user.Family,
                        CreatedOn = DateTime.Now
                    };
                    var password = $"Teram@{user.NationalCode}@{user.BirthDate}";
                    var existUserInfo = userSharedService.GetUserInfo(user.NationalCode);
                    if (!existUserInfo.Any())
                    {
                        var result = await userSharedService.CreateUserAsync(userInfo, password);
                        if (result.Succeeded)
                        {
                            var createdUserInfo = userSharedService.GetUserInfo(user.NationalCode);
                            var rorlResult = await userSharedService.AddToRoleAsync(createdUserInfo.FirstOrDefault(), "Members");
                        }
                    }
                }
                return Json(new { Result = "ok", Message = localizer["Users Uploaded Successfully"] });
            }
            catch (Exception ex)
            {
                logger.LogError("Error In Import Users" + ex.Message + ex.InnerException);
                return Json(new { Result = "fail", Message = localizer["Error In Upload Useres"] });
            }
        }
    }
}