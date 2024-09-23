
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Teram.Framework.Core;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Tools;
using Teram.Module.Authentication.Logic;
using Teram.Module.Authentication.Models;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Model;
using Teram.Web.Core.Security;

namespace Teram.Module.Authentication.Controllers
{
    [Display(Description = "Token Provider")]
    public class TokenProviderController : ControlPanelBaseController<TokenModel, Entities.Token, Guid>
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IOptions<TokenSettings> tokenSettings;
        private readonly IUserSharedService userSharedService;

        public TokenProviderController(IServiceProvider serviceProvider, ILogger<TokenProviderController> logger,
            IStringLocalizer<TokenProviderController> localizer, IStringLocalizer<SharedResource> sharedlocalizer,
            IOptions<TokenSettings> tokenSettings, IUserSharedService userSharedService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.sharedLocalizer = sharedlocalizer ?? throw new ArgumentNullException(nameof(sharedLocalizer));
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.tokenSettings = tokenSettings ?? throw new ArgumentNullException(nameof(tokenSettings));
            this.userSharedService = userSharedService ?? throw new ArgumentNullException(nameof(userSharedService));

            ViewBag.PageName = localizer["Token Provider"];
            Model = new ViewInformation<TokenModel>
            {
                Layout = "~/Views/Shared/_AdminControlPanelLayout.fa-IR.cshtml",
                Title = localizer["Token Provider"],
                HomePage = "/TokenProvider/Index",
                HasGrid = true,
                EditInSamePage = true,
                GridId = "TokenProviderGrid",
                GetDataUrl = nameof(GetGridData),
                LoadAjaxData = false,
                LoadDefaultSetting = false,
                ExtraScripts = "/ExternalModule/Module/Authentication/Scripts/TokenProvider.js"
            };
        }

        [Display(Description = "Show")]
        [ControlPanelMenu("Token Provider", ParentName = "Security", Icon = "fa-key", PanelType = PanelType.Managment, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index([FromServices] IBaseLogic<TokenParameterModel, IIdentityUnitOfWork> tokenParameterLogic)
        {
            var result = tokenParameterLogic.GetAll();
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity == null)
            {
                ViewBag.TokenParameters = new List<TokenParameterModel>();
            }
            ViewBag.TokenParameters = result.ResultEntity;

            ViewBag.Users = GetAllUsers();
            return View("Index", Model);
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetGridData([FromServices] IBaseLogic<TokenModel, IIdentityUnitOfWork> service, DatatablesSentModel model)
        {
            var sortInformation = new List<SortInformation>();
            if (model.Order != null)
            {
                sortInformation = GetSortInformation(model.Columns, model.Order);
            }
            var data = new List<TokenModel>();

            var result = service.GetAll(model.Start, model.Length, sortInformation);
            if (result.ResultStatus == OperationResultStatus.Successful && result.ResultEntity != null)
            {
                data = result.ResultEntity;
            }

            var userIds = data.Select(x => x.UserId).ToList();
            var users = userSharedService.GetUserInfos(userIds);

            foreach (var tokenModel in data)
            {
                tokenModel.UserName = users.FirstOrDefault(x => x.UserId == tokenModel.UserId).Username;
            }

            if (data == null) return null;
            return Json(new { model.Draw, recordsTotal = result.Count, recordsFiltered = result.Count, data, error = "" });
        }


        [Display(Description = "Edit")]
        [ParentalAuthorize(nameof(Edit))]
        public IActionResult EditPartial([FromServices] IBaseLogic<TokenModel, IIdentityUnitOfWork> service,
            [FromServices] IBaseLogic<TokenParameterModel, IIdentityUnitOfWork> tokenParameterLogic, Guid id)
        {

            ViewBag.Users = GetAllUsers();

            if (id == default || id == Guid.Empty)
            {
                var result = tokenParameterLogic.GetAll();
                if (result.ResultStatus == OperationResultStatus.Successful && result.ResultEntity != null)
                {
                    ViewBag.TokenParameters = result.ResultEntity;
                }

                return PartialView("Add", new TokenModel());
            }

            var row = service.GetRow(id);
            var tokenModel = row.ResultEntity;

            // اعتبار سنجی و گرفتن کلیم های توکن
            var decodeTokenResult = DecodeToken(tokenParameterLogic, tokenModel.Content);
            if (!decodeTokenResult.Status)
            {
                return Json(new { result = "fail", message = decodeTokenResult.Message, title = localizer["Token Is Invalid"] });
            }
            var tokenParams = decodeTokenResult.TokenParameterModels;

            ViewBag.TokenParameters = tokenParams;

            Model.ModelData = row.ResultEntity;
            Model.Key = row.ResultEntity.Key;
            return PartialView("Add", Model.ModelData);
        }

        private DateTime ConvertToDateTime(long milliSec)
        {
            var timeInTicks = milliSec * TimeSpan.TicksPerSecond;
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddTicks(timeInTicks).ToLocalTime();
        }


        /// <summary>
        /// در این متد ولید بودن توکن بررسی میشود و لیست توکن پارمترها و کلیم ها را برمیگرداند
        /// </summary>
        /// <param name="tokenParameterLogic"></param>
        /// <param name="tokenContent"></param>
        /// <returns></returns>
        [ParentalAuthorize(nameof(Edit))]
        public DecodeTokenResult DecodeToken(IBaseLogic<TokenParameterModel, IIdentityUnitOfWork> tokenParameterLogic, String tokenContent)
        {
            ClaimsPrincipal claims;
            var decodeTokenResult = ValidateJwtToken(tokenContent, out claims);
            var tokenParameters = GetTokenParameters(tokenParameterLogic, claims);

            decodeTokenResult.TokenParameterModels = tokenParameters.OrderBy(x => x.Name).ToList();

            return decodeTokenResult;
        }

        /// <summary>
        /// این متد برای یک توکن لیست  توکن پارامترز و  کلیم های توکن را ترکیب میکند و لیست همه را میدهد
        /// که موقع ویرایش در لیست کلیم ها نمایش دهیم
        /// </summary>
        /// <param name="tokenParameterLogic"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        private List<TokenParameterModel> GetTokenParameters(IBaseLogic<TokenParameterModel, IIdentityUnitOfWork> tokenParameterLogic, ClaimsPrincipal claims)
        {
            var tokenParameters = new List<TokenParameterModel>();

            // لیست همه ی پارمترها
            var result = tokenParameterLogic.GetAll();
            if (result.ResultStatus == OperationResultStatus.Successful && result.ResultEntity != null)
            {
                tokenParameters = result.ResultEntity;
            }

            foreach (var tokenParam in tokenParameters)
            {
                tokenParam.Checked = claims.Claims.Any(x => x.Type == tokenParam.Name);
                tokenParam.Value = claims.Claims.FirstOrDefault(x => x.Type == tokenParam.Name)?.Value ?? "";
            }

            string[] defaultclaimTypes = { "id", "issuedFor", "issuerId", "nbf", "exp", "iat", "iss", "aud" };
            // کلیم هایی که توکن دارد را میگیرم چک میکنم اگر  توکن پارامترش  پاک شده نمایشش بدهم
            var tokenClaims = claims.Claims.Select(x => x.Type).ToList();

            foreach (var item in tokenClaims)
            {
                var claimTokenExitstInParameters = tokenParameters.Any(x => x.Name == item);
                if (!claimTokenExitstInParameters && !defaultclaimTypes.Contains(item))
                {
                    tokenParameters.Add(new TokenParameterModel
                    {
                        Name = item,
                        Value = claims.Claims.FirstOrDefault(x => x.Type == item)?.Value ?? "",
                        Checked = true
                    });
                }
            }
            return tokenParameters;
        }

        [Display(Description = "Save")]
        public IActionResult SaveToken([FromServices] IBaseLogic<TokenModel, IIdentityUnitOfWork> service, [FromServices] IUserPrincipal userPrincipal, TokenModel model, List<TokenParameterModel> claimsList)
        {
            try
            {
                model.IssuerId = userPrincipal.CurrentUserId;
                model.IssuedOn = DateTime.Now;

                if (model.ExpireDate < DateTime.Now)
                {
                    var message = localizer["Expiration date is less than the current day"];
                    return Json(new { result = "fail", message = message, title = localizer["Something wrong"] });

                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(tokenSettings.Value.Secret);

                var encryptionkey = Encoding.UTF8.GetBytes("Entekhab@EncKey_"); //must be 16 character
                var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

                var claims = new List<Claim>();
                claims.Add(new Claim("id", model.UserId.ToString()));
                claims.Add(new Claim("issuedFor", model.IssuedFor.ToString()));
                claims.Add(new Claim("issuerId", model.IssuerId.ToString()));

                foreach (var item in claimsList)
                {
                    claims.Add(new Claim(item.Name, item.Value));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = model.ExpireDate,
                    Audience = tokenSettings.Value.Audience,
                    IssuedAt = DateTime.Now,
                    Issuer = tokenSettings.Value.Authority,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    EncryptingCredentials = encryptingCredentials,

                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                model.Content = tokenHandler.WriteToken(token);

                //برای دیباگ گذاشتم که بعد از ذخیره کلیم هاشو بررسی کنم
                //var mm = DecodeToken(tokenParameterLogic, model.Content);
                if (model.TokenId == Guid.Empty)
                {
                    var addResult = service.AddNew(model);
                    if (addResult.ResultStatus != OperationResultStatus.Successful || addResult.ResultEntity is null)
                    {
                        return Json(new { result = "fail", message = localizer[addResult.AllMessages] });
                    }
                }
                else
                {
                    var updateResult = service.Update(model);
                    if (updateResult.ResultStatus != OperationResultStatus.Successful)
                    {
                        return Json(new { result = "fail", message = localizer[updateResult.AllMessages] });
                    }
                }

                return Json(new
                {
                    result = "ok",
                    message = sharedLocalizer["Your data has been saved"],
                    title = sharedLocalizer["SaveTitle"]
                });

            }
            catch (Exception ex)
            {
                var message = ExceptionParser.Parse(ex);
                logger.LogError(new EventId(500), message, ex);
                throw;
            }
        }

        /// <summary>
        /// برای اینکه موقع ویرایش، اجازه تغییر توکن های منقضی شده را بدهد و خطای منقضی شدن ندهد
        /// </summary>
        /// <param name="notBefore"></param>
        /// <param name="expires"></param>
        /// <param name="tokenToValidate"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
        {

            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }
        private DecodeTokenResult ValidateJwtToken(string tokenContent, out ClaimsPrincipal claims)
        {

            var encryptionkey = Encoding.UTF8.GetBytes("Teram@EncKey_"); //must be 16 character
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenSettings.Value.Secret);
            var handler = new JwtSecurityTokenHandler();
            // var tokenSecure = handler.ReadToken(tokenContent) as SecurityToken;
            SecurityToken tokenSecure;
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                //ValidIssuers = new string[] { tokenSettings.Value.Secret },
                ValidateAudience = true,
                ValidAudiences = new string[] { tokenSettings.Value.Audience },
                IssuerSigningKey = new SymmetricSecurityKey(key),
                TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey),
                LifetimeValidator = CustomLifetimeValidator,
                ClockSkew = TimeSpan.Zero
            };


            claims = new ClaimsPrincipal();
            try
            {
                claims = handler.ValidateToken(tokenContent, validations, out tokenSecure);

                //// این قسمت برای دیباگ کردن تاریخ انقضا نوشته شده است
                var expClaim = claims.Claims.Where(x => x.Type == "exp").FirstOrDefault();
                long exp = long.Parse(expClaim.Value);
                var expireDate = ConvertToDateTime(exp);
                ////////////
            }
            catch (SecurityTokenExpiredException)
            {
                return new DecodeTokenResult
                {
                    Status = false,
                    Message = localizer["Expiration date is less than the current day. Edit does not allowed"]
                };
            }
            catch (Exception ex)
            {
                return new DecodeTokenResult
                {
                    Status = false,
                    Message = localizer[ex.Message]
                };
            }

            var decodeTokenResult = new DecodeTokenResult
            {
                Status = true,
                Message = "Token is valid",

            };

            return decodeTokenResult;
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetUsers(string term)
        {
            var users = userSharedService.GetUserInfo(term)
                .Select(x => new { id = x.UserId, text = x.Username }).ToList();
            return Json(new { results = users });
        }


        private List<SelectListItem> GetAllUsers()
        {
            return userSharedService.GetAllUsers().Select(item =>
                                 new SelectListItem
                                 {
                                     Value = item.UserId.ToString(),
                                     Text = item.Username
                                 }).ToList();
        }

    }


}