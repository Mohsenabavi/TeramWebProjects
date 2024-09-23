using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Controllers;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.ExtensionMethods;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;
using Teram.HR.Module.Recruitment.Services;
using Teram.Module.SmsSender.Models.AsiaSms;
using Teram.Module.SmsSender.Services;
using Teram.Module.SmsSender.Services.AsiaSms;
using Teram.ServiceContracts;

namespace Teram.HR.Module.Recruitment.Logic
{
    public class IntroductionGenerationService : IIntroductionGenerationService
    {
        private readonly IJobApplicantLogic jobApplicantLogic;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly PdfConverterService pdfConverterService;
        private readonly IJobApplicantsIntroductionLetterLogic jobApplicantsIntroductionLetterLogic;
        private readonly IStringLocalizer<JobApplicantControlPanelController> localizer;
        private readonly ISendAsiaSMSService sendAsiaSMSService;

        public IntroductionGenerationService(IJobApplicantLogic jobApplicantLogic, IWebHostEnvironment webHostEnvironment, PdfConverterService pdfConverterService,
            IJobApplicantsIntroductionLetterLogic jobApplicantsIntroductionLetterLogic, IStringLocalizer<JobApplicantControlPanelController> localizer, ISendAsiaSMSService sendAsiaSMSService)
        {
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
            this.webHostEnvironment=webHostEnvironment??throw new ArgumentNullException(nameof(webHostEnvironment));
            this.pdfConverterService=pdfConverterService;
            this.jobApplicantsIntroductionLetterLogic=jobApplicantsIntroductionLetterLogic??throw new ArgumentNullException(nameof(jobApplicantsIntroductionLetterLogic));
            this.localizer=localizer??throw new ArgumentNullException(nameof(localizer));
            this.sendAsiaSMSService=sendAsiaSMSService??throw new ArgumentNullException(nameof(sendAsiaSMSService));
        }
        public JobApplicantApproveResultModel CreateIntroductionFiles(JobApplicantModel jobApplicatnt)
        {
            var result = new JobApplicantApproveResultModel();

            var relatedNationCode = jobApplicatnt.NationalCode;
            var relatedFullName = $"{jobApplicatnt.FirstName} {jobApplicatnt.LastName}";
            var relatedJobPosition = jobApplicatnt.JobPositionTitle;
            var relatedPromissoryNoteAmount = jobApplicatnt.PromissoryNoteAmount;


            var deadLineText = DateExtensions.CalculateDaysDifference(DateTime.Now, (jobApplicatnt.ExpreminetDeadline.HasValue) ? jobApplicatnt.ExpreminetDeadline.Value : null);

            long maxLetterNu;

            maxLetterNu = jobApplicantsIntroductionLetterLogic.GetMaxLetterNo().ResultEntity + 1;

            maxLetterNu++;

            var createOMFileResult = jobApplicantLogic.GenerateOMFile(relatedNationCode, relatedFullName, relatedJobPosition, maxLetterNu, deadLineText);

            if (createOMFileResult.ResultStatus != OperationResultStatus.Successful || createOMFileResult.ResultEntity is null)
            {
                return new JobApplicantApproveResultModel
                {
                    OperationResultStatus=OperationResultStatus.Fail,
                    Message=localizer["Error In Generate Files"]
                };
            }

            var rootpath = webHostEnvironment.ContentRootPath;

            var omFileName = $"OM-{relatedNationCode}-{relatedFullName}.pdf";
            var omPdfBytes = pdfConverterService.ConvertWordToPdf(createOMFileResult.ResultEntity, false, jobApplicatnt.JobApplicantId, true);
            var omfilePath = Path.Combine(rootpath, "wwwroot", "Introductions", omFileName);
            System.IO.File.WriteAllBytes(omfilePath, omPdfBytes);

            maxLetterNu = jobApplicantsIntroductionLetterLogic.GetMaxLetterNo().ResultEntity +1;

            var createNAFileResult = jobApplicantLogic.GenerateNoAddictionFile(jobApplicatnt.JobApplicantId, relatedNationCode, relatedFullName, maxLetterNu, deadLineText);


            if (createNAFileResult.ResultStatus != OperationResultStatus.Successful || createNAFileResult.ResultEntity is null)
            {
                return new JobApplicantApproveResultModel
                {
                    OperationResultStatus = OperationResultStatus.Fail,
                    Message=localizer["Error In Generate Files"]
                };
            }

            var naFileName = $"NA-{relatedNationCode}-{relatedFullName}.pdf";
            var naPdfBytes = pdfConverterService.ConvertWordToPdf(createNAFileResult.ResultEntity, true, jobApplicatnt.JobApplicantId, true);
            var nafilePath = Path.Combine(rootpath, "wwwroot", "Introductions", naFileName);
            System.IO.File.WriteAllBytes(nafilePath, naPdfBytes);


            var createDocumentFileResult = jobApplicantLogic.GenerateDocumentFile(relatedNationCode, relatedPromissoryNoteAmount);


            if (createDocumentFileResult.ResultStatus != OperationResultStatus.Successful || createDocumentFileResult.ResultEntity is null)
            {
                return new JobApplicantApproveResultModel
                {
                    OperationResultStatus = OperationResultStatus.Fail,
                    Message=localizer["Error In Generate Files"]
                };
            }

            var docFileName = $"Document-{relatedNationCode}-{relatedFullName}.pdf";
            var docPdfBytes = pdfConverterService.ConvertWordToPdf(createDocumentFileResult.ResultEntity, false, jobApplicatnt.JobApplicantId, false);
            var docfilePath = Path.Combine(rootpath, "wwwroot", "Introductions", docFileName);
            System.IO.File.WriteAllBytes(docfilePath, docPdfBytes);


            var existingLetters = jobApplicantsIntroductionLetterLogic.GetByJobApplicantId(jobApplicatnt.JobApplicantId);

            var existingOMLetter = existingLetters.ResultEntity.FirstOrDefault(x => x.IntroductionLetterType==IntroductionLetterType.NoAddiction);

            var existingNALetter = existingLetters.ResultEntity.FirstOrDefault(x => x.IntroductionLetterType==IntroductionLetterType.OccupationalMedicine);


            var existingDocumentLetter = existingLetters.ResultEntity.FirstOrDefault(x => x.IntroductionLetterType==IntroductionLetterType.Document);




            if (existingOMLetter != null)
            {
                existingOMLetter.FileUrl=nafilePath;
                existingOMLetter.CreateDate=DateTime.Now;
                jobApplicantsIntroductionLetterLogic.Update(existingOMLetter);
            }
            else
            {
                maxLetterNu = jobApplicantsIntroductionLetterLogic.GetMaxLetterNo().ResultEntity;
                var inserNAFileModel = new JobApplicantsIntroductionLetterModel
                {
                    JobApplicantId = jobApplicatnt.JobApplicantId,
                    IntroductionLetterType=IntroductionLetterType.NoAddiction,
                    FileUrl=nafilePath,
                    CreateDate=DateTime.Now,
                    LetterNumber=maxLetterNu+1
                };
                jobApplicantsIntroductionLetterLogic.AddNew(inserNAFileModel);
            }

            if (existingNALetter != null)
            {
                existingNALetter.FileUrl=omfilePath;
                existingNALetter.CreateDate=DateTime.Now;
                jobApplicantsIntroductionLetterLogic.Update(existingNALetter);
            }
            else
            {
                maxLetterNu = jobApplicantsIntroductionLetterLogic.GetMaxLetterNo().ResultEntity;
                var insertOMFileModel = new JobApplicantsIntroductionLetterModel
                {
                    JobApplicantId = jobApplicatnt.JobApplicantId,
                    IntroductionLetterType=IntroductionLetterType.OccupationalMedicine,
                    FileUrl=omfilePath,
                    CreateDate=DateTime.Now,
                    LetterNumber=maxLetterNu+1
                };
                jobApplicantsIntroductionLetterLogic.AddNew(insertOMFileModel);
            }

            if (existingDocumentLetter != null)
            {
                existingDocumentLetter.FileUrl=docfilePath;
                existingDocumentLetter.CreateDate=DateTime.Now;
                jobApplicantsIntroductionLetterLogic.Update(existingDocumentLetter);
            }
            else
            {
                var insertDocumentFileModel = new JobApplicantsIntroductionLetterModel
                {
                    JobApplicantId = jobApplicatnt.JobApplicantId,
                    IntroductionLetterType=IntroductionLetterType.Document,
                    FileUrl=docfilePath,
                    CreateDate=DateTime.Now,
                };
                jobApplicantsIntroductionLetterLogic.AddNew(insertDocumentFileModel);
            }

            var links = new List<FileLinkModel>
            {
                new() { Link=omfilePath, Title=localizer["OccupationalMedicineLetter"] },
                new() { Link=nafilePath, Title=localizer["NoAddictionLetter"] },
                new() { Link=docfilePath, Title=localizer["DocumentLetter"] }
            };

            result.FileLinks = links;
            result.Message=localizer["Introduction Files Generated Successfully"];
            result.OperationResultStatus=OperationResultStatus.Successful;

            return result;
        }
    }
}
