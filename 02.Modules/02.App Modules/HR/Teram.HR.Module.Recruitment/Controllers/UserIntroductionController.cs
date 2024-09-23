using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.FileUploader.Models;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Migrations;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Module.AttachmentsManagement.Models;
using Teram.Module.GeographicRegion.Logic.Interfaces;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.ControlPanel;
using Teram.Framework.Core.Extensions;
using Teram.Web.Core.Security;
using Teram.Web.Core.Attributes;
using Microsoft.AspNetCore.Identity;
using Teram.Module.Authentication.Models;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class UserIntroductionController : BasicControlPanelController
    {
        private readonly IUserPrincipal userPrincipal;
        private readonly IUserSharedService userSharedService;
        private readonly IJobApplicantLogic jobApplicantLogic;
        private readonly IJobApplicantsIntroductionLetterLogic jobApplicantsIntroductionLetterLogic;

        public UserIntroductionController(ILogger<BaseInformationController> logger
            , IStringLocalizer<BaseInformationController> localizer, IGeographicRegionLogic geographicRegionLogic, IGeoSharedService geoSharedService,
            IUserPrincipal userPrincipal, IUserSharedService userSharedService, IJobApplicantLogic jobApplicantLogic, IJobApplicantsIntroductionLetterLogic jobApplicantsIntroductionLetterLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
            this.jobApplicantsIntroductionLetterLogic=jobApplicantsIntroductionLetterLogic??throw new ArgumentNullException(nameof(jobApplicantsIntroductionLetterLogic));
        }

        [ControlPanelMenu("UserIntroduction", ParentName = "ManageJobApplicants", Icon = "fa fa-envelope-open", PanelType = PanelType.User, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar, Order = 3)]
        public async Task<IActionResult> Index([FromServices] UserManager<TeramUser> userManager)
        {                      
            var returnModel = new List<FileLinkModel>();
            var userInfotmation = await userSharedService.GetUserById(userPrincipal.CurrentUserId);
            if (userInfotmation != null)
            {
                var relatedJobApplicant = jobApplicantLogic.GetByUserId(userInfotmation.UserId);
                if (relatedJobApplicant.ResultStatus == OperationResultStatus.Successful && relatedJobApplicant.ResultEntity is not null)
                {
                    var relatedIntroductions = jobApplicantsIntroductionLetterLogic.GetByJobApplicantId(relatedJobApplicant.ResultEntity.JobApplicantId);

                    if (relatedIntroductions.ResultStatus != OperationResultStatus.Successful || relatedIntroductions.ResultEntity is null)
                    {
                        return View("Add", returnModel);
                    }
                    returnModel = relatedIntroductions.ResultEntity.Select(x => new FileLinkModel
                    {
                        Link=x.FileUrl,
                        Title=((IntroductionLetterType)Enum.Parse(typeof(IntroductionLetterType), x.IntroductionLetterType.ToString())).GetDescription()
                    }).ToList();
                }
            }
            return View("Add", returnModel);
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult DownloadFile(string link)
        {

            var fileContent = System.IO.File.ReadAllBytes(link);

            return File(fileContent, "application/octet-stream", link);
        }
    }

}
