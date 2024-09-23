using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq.Expressions;
using System.Transactions;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Module.AttachmentsManagement.Logic.Interfaces;
using Teram.Module.SmsSender.Services;
using Teram.ServiceContracts;
using Teram.Web.Core.Security;
using Xceed.Words.NET;


namespace Teram.HR.Module.Recruitment.Logic
{
    public class JobApplicantLogic : BusinessOperations<JobApplicantModel, JobApplicant, int>, IJobApplicantLogic
    {
        private readonly IJobApplicantFileLogic jobApplicantFileLogic;
        private readonly IUserPrincipal userPrincipal;
        private readonly ISendAsiaSMSService sendAsiaSMSService;
        private readonly IJobApplicantApproveHistoryLogic jobApplicantApproveHistoryLogic;
        private readonly IUserSharedService userSharedService;
        private readonly IAttachmentLogic attachmentLogic;

        public JobApplicantLogic(IPersistenceService<JobApplicant> service,
            IJobApplicantFileLogic jobApplicantFileLogic, IUserPrincipal userPrincipal, ISendAsiaSMSService sendAsiaSMSService,
            IJobApplicantApproveHistoryLogic jobApplicantApproveHistoryLogic, IUserSharedService userSharedService,
            IAttachmentLogic attachmentLogic) : base(service)
        {
            this.jobApplicantFileLogic=jobApplicantFileLogic;
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.sendAsiaSMSService=sendAsiaSMSService??throw new ArgumentNullException(nameof(sendAsiaSMSService));
            this.jobApplicantApproveHistoryLogic=jobApplicantApproveHistoryLogic??throw new ArgumentNullException(nameof(jobApplicantApproveHistoryLogic));
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
            this.attachmentLogic=attachmentLogic??throw new ArgumentNullException(nameof(attachmentLogic));
            BeforeUpdate+=JobApplicantLogic_BeforeUpdate;
            AfterAdd+=JobApplicantLogic_AfterAdd;
        }

        private void JobApplicantLogic_AfterAdd(TeramEntityEventArgs<JobApplicant, JobApplicantModel, int> entity)
        {
            if (entity.NewEntity.NeededForBackgroundCheck)
            {

                sendAsiaSMSService.SendMessage(new Teram.Module.SmsSender.Models.AsiaSms.SendSmsModel
                {
                    Receivers="09131257541,09135653195",
                    //Receivers="09135653195",
                    SmsText=$"پرونده {entity.NewEntity.FirstName} {entity.NewEntity.LastName} در سامانه منابع انسانی ثبت گردید لطفاً نسبت به ثبت اطلاعات پیشینه شغلی ایشان اقدام فرمایید\nکدملی : {entity.NewEntity.NationalCode}"
                });
            }
        }

        private void JobApplicantLogic_BeforeUpdate(TeramEntityEventArgs<JobApplicant, JobApplicantModel, int> entity)
        {
            var currentState = GetById(entity.NewEntity.JobApplicantId);

            if (currentState.ResultStatus == OperationResultStatus.Successful && currentState.ResultEntity is not null)
            {
                if (entity.NewEntity.BaseInformationApproveStatus!=currentState.ResultEntity.BaseInformationApproveStatus)
                {

                    jobApplicantApproveHistoryLogic.AddNewHistory(entity.NewEntity.JobApplicantId, entity.NewEntity.BaseInformationApproveStatus, (entity.NewEntity.BaseInformationErrors!=null) ? entity.NewEntity.BaseInformationErrors : " ");
                }

                switch (entity.NewEntity.ProcessStatus)
                {
                    case ProcessStatus.BaseInformationAdded:

                        sendAsiaSMSService.SendMessage(new Teram.Module.SmsSender.Models.AsiaSms.SendSmsModel
                        {
                            Receivers="09014988824,09135748102,09135653195",
                            //Receivers="09135653195",
                            SmsText=$"اطلاعات پایه توسط {entity.NewEntity.FirstName} {entity.NewEntity.LastName} در این لحظه ثبت گردید"
                        });
                        break;
                    case ProcessStatus.DoumentsUploaded:
                        sendAsiaSMSService.SendMessage(new Teram.Module.SmsSender.Models.AsiaSms.SendSmsModel
                        {
                            Receivers="09014988824,09135748102,09135653195",
                            //Receivers="09135653195",
                            SmsText=$"مدارک {entity.NewEntity.FirstName} {entity.NewEntity.LastName} در این لحظه بارگذاری گردید"
                        });
                        break;
                    case ProcessStatus.AdmittingToDoExpriments:
                        sendAsiaSMSService.SendMessage(new Teram.Module.SmsSender.Models.AsiaSms.SendSmsModel
                        {
                            Receivers="9010593996,09135653195",
                            //Receivers="09135653195",
                            SmsText=$"اقرار انجام آزمایشات توسط  {entity.NewEntity.FirstName} {entity.NewEntity.LastName} در این لحظه ثبت گردید"
                        });
                        break;
                }
            }
        }


        public BusinessOperationResult<bool> FirstApprove(JobApplicantModel jobApplicant)
        {

            var result = new BusinessOperationResult<bool>();

            if (jobApplicant.BaseInformationApproveStatus==ApproveStatus.FisrtApproved)
            {
                result.SetErrorMessage("Information Has Already Approved");
                return result;
            }

            jobApplicant.BaseInformationApproveDate = DateTime.Now;
            jobApplicant.BaseInformationApprovedBy =userPrincipal.CurrentUserId;
            jobApplicant.BaseInformationApproveStatus=ApproveStatus.FisrtApproved;
            jobApplicant.ProcessStatus=ProcessStatus.FirstApprove;
            var updateResult = Update(jobApplicant);

            if (updateResult.ResultEntity)
            {

                sendAsiaSMSService.SendMessage(new Teram.Module.SmsSender.Models.AsiaSms.SendSmsModel
                {
                    Receivers=jobApplicant.MobileNumber,
                    SmsText=$"تایید اولیه پرونده شما توسط منابع انسانی انجام شده است لطفاً در صورت انجام آزمایشات ، از طریق سامانه اطلاعات لازم را وارد نمایید \nلینک اعلام مراحل : B2n.ir/z46407 "
                });

                result.SetSuccessResult(updateResult.ResultEntity);
                return result;
            }
            else
            {
                result.SetErrorMessage("Error In Update Status");
                return result;
            }
        }

        public BusinessOperationResult<bool> FinalApprove(JobApplicantModel jobApplicant)
        {

            var result = new BusinessOperationResult<bool>();

            if (jobApplicant.BaseInformationApproveStatus==ApproveStatus.FinalApprove)
            {
                result.SetErrorMessage("Information Has Already Approved");
                return result;
            }

            jobApplicant.BaseInformationApproveDate = DateTime.Now;
            jobApplicant.BaseInformationApprovedBy =userPrincipal.CurrentUserId;
            jobApplicant.BaseInformationApproveStatus=ApproveStatus.FinalApprove;
            jobApplicant.ProcessStatus=ProcessStatus.FinalApproveByHR;


            var userInfo = userSharedService.GetUserInfoByUserName(jobApplicant.NationalCode).Result;

            if (userInfo!=null)
            {

                userSharedService.AddToRoleAsync(userInfo, "Employee");
            }

            var updateResult = Update(jobApplicant);

            if (updateResult.ResultEntity)
            {
                result.SetSuccessResult(updateResult.ResultEntity);
                return result;
            }
            else
            {
                result.SetErrorMessage("Error In Update Status");
                return result;
            }
        }

        public BusinessOperationResult<string> GenerateNoAddictionFile(int jobApplicantId, string nationalCode, string fullName, long maxLetterNu, string deadline)
        {
            var result = new BusinessOperationResult<string>();

            try
            {
                var currentDate = DateTime.Now.ToPersianDate();
                var LetterNuPrefix = currentDate.Substring(2, 2) + currentDate.Substring(5, 2);

                var personnalImageInfo = jobApplicantFileLogic.GetPersonalImage(jobApplicantId);


                if (personnalImageInfo.ResultStatus != OperationResultStatus.Successful || personnalImageInfo.ResultEntity is null)
                {
                    result.SetErrorMessage("Personnel Image Not found");
                    return result;
                }

                var imageData = attachmentLogic.GetFileById(personnalImageInfo.ResultEntity.AttachmentId);

                System.Drawing.Image image;

                using (MemoryStream ms = new MemoryStream(imageData.ResultEntity.FileData))
                {
                    image =  System.Drawing.Image.FromStream(ms);
                    string templatePath = Path.Combine("wwwroot", "Templates", "fa-IR", "Word", "NoAddiction.docx");

                    string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "fa-IR", "Word", $"NA-{nationalCode}.docx");

                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), templatePath);

                    using (var doc = DocX.Load(fullPath))
                    {
                        doc.ReplaceText("{{Name}}", $"{fullName}");
                        doc.ReplaceText("{{PersianDate}}", DateTime.Now.ToPersianDate());
                        doc.ReplaceText("{{NationalCode}}", nationalCode);
                        doc.ReplaceText("{{LetterNo}}", $"{maxLetterNu.ToString()}-{LetterNuPrefix}");
                        doc.ReplaceText("{{Deadline}}", $"{deadline}");

                        var paragraph = doc.Paragraphs.FirstOrDefault(p => p.Text.Contains("عکس"));

                        if (paragraph != null)
                        {
                            paragraph.RemoveText(0, paragraph.Text.Length, false);
                            var picture = doc.AddImage(ms);
                            paragraph.InsertPicture(picture.CreatePicture(100, 100));
                        }
                        doc.SaveAs(outputPath);
                    }
                    System.IO.File.ReadAllBytes(outputPath);
                    result.SetSuccessResult(outputPath);
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.SetErrorMessage(ex.Message + ex.InnerException);
                return result;
            }
        }
        public BusinessOperationResult<string> GenerateOMFile(string nationalCode, string fullName, string relatedJobPosition, long maxLetterNu, string deadline)
        {
            var result = new BusinessOperationResult<string>();

            try
            {
                var currentDate = DateTime.Now.ToPersianDate();
                var LetterNuPrefix = currentDate.Substring(2, 2) + currentDate.Substring(5, 2);

                string templatePath = Path.Combine("wwwroot", "Templates", "fa-IR", "Word", "OM.docx");

                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "fa-IR", "Word", $"OM-{nationalCode}.docx");

                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), templatePath);

                using (var doc = DocX.Load(fullPath))
                {
                    doc.ReplaceText("{{Name}}", $"{fullName}");
                    doc.ReplaceText("{{PersianDate}}", DateTime.Now.ToPersianDate());
                    doc.ReplaceText("{{JobTitle}}", relatedJobPosition);
                    doc.ReplaceText("{{LetterNo}}", $"{maxLetterNu.ToString()}-{LetterNuPrefix}");
                    doc.ReplaceText("{{Deadline}}", $"{deadline}");
                    doc.SaveAs(outputPath);
                }
                System.IO.File.ReadAllBytes(outputPath);
                result.SetSuccessResult(outputPath);
                return result;
            }
            catch (Exception ex)
            {
                result.SetErrorMessage(ex.Message + ex.InnerException);
                return result;
            }
        }


        public BusinessOperationResult<JobApplicantModel> GetById(int jobApplicantId)
        {
            return GetFirst<JobApplicantModel>(x => x.JobApplicantId==jobApplicantId, new List<string> { "JobPosition" });
        }

        public BusinessOperationResult<JobApplicantModel> GetByUserId(Guid userId)
        {
            return GetFirst<JobApplicantModel>(x => x.UserId == userId);
        }

        public BusinessOperationResult<List<JobApplicantModel>> GetByFilter(string? personnelCode, string? nationalCode, string? firstName, string? lastName, bool viewInprogressStatus, FlowType? flowType, ProcessStatus? processStatus, int? start = null, int? length = null)
        {
            var query = CreateFilterExpression(personnelCode, nationalCode, firstName, lastName, viewInprogressStatus, flowType, processStatus);
            if (start.HasValue && length.HasValue)
            {
                return GetData<JobApplicantModel>(query, row: start.Value, max: length.Value, orderByMember: "JobApplicantId", orderByDescending: true);
            }
            return GetData<JobApplicantModel>(query, null, null, false, null);
        }
        private Expression<Func<JobApplicant, bool>> CreateFilterExpression(string? personnelCode, string? nationalCode, string? firstName, string? lastName, bool viewInprogressStatus, FlowType? flowType, ProcessStatus? processStatus)
        {

            Expression<Func<JobApplicant, bool>> query = x => true;

            if (viewInprogressStatus)
            {
                query = query.AndAlso(x => x.ProcessStatus!=ProcessStatus.FinalApproveByHR);
            }

            if (flowType.HasValue)
            {
                query = query.AndAlso(x => x.FlowType==flowType);
            }

            if (processStatus.HasValue)
            {
                query = query.AndAlso(x => x.ProcessStatus==processStatus);
            }

            var isAdmin = userPrincipal.CurrentUser.HasClaim("Permission", ":JobApplicant:AdminPermission");

            if (!isAdmin)
            {
                query = query.AndAlso(x => x.UserId==userPrincipal.CurrentUserId);
            }

            var onlyViewNeededForBackgroundCheck = userPrincipal.CurrentUser.HasClaim("Permission", ":BackgroundJobRequest:ViewOnlyNeededToBackGroundCheckProtectionUnitOnly");

            if (onlyViewNeededForBackgroundCheck)
            {
                query = query.AndAlso(x => x.NeededForBackgroundCheck);
            }

            if (!string.IsNullOrEmpty(personnelCode))
            {
                query = query.AndAlso(x => x.PersonnelCode == personnelCode);
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.AndAlso(x => x.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.AndAlso(x => x.LastName.Contains(lastName));
            }

            if (!string.IsNullOrEmpty(nationalCode))
            {
                query = query.AndAlso(x => x.NationalCode == nationalCode);
            }
            return query;
        }
        public BusinessOperationResult<List<JobApplicantModel>> GetWorkersJobApplicants()
        {
            return GetData<JobApplicantModel>(x => x.JobCategory==JobCategory.Worker && x.FlowType==FlowType.JobApplicant);
        }
        public BusinessOperationResult<List<JobApplicantModel>> GetEmployeesJobApplicants()
        {
            return GetData<JobApplicantModel>(x => x.JobCategory==JobCategory.Employee && x.FlowType==FlowType.JobApplicant);
        }

        public BusinessOperationResult<List<JobApplicantModel>> GetByPeriod(DateTime startDate, DateTime endDate)
        {
            return GetData<JobApplicantModel>(x => x.CreateDate.Date >= startDate.Date && x.CreateDate <= endDate.Date);
        }
        public BusinessOperationResult<JobApplicantModel> GetByPersonnelCode(string personnelCode)
        {
            var personnelCodeTrimed = personnelCode.Trim();
            return GetFirst<JobApplicantModel>(x => x.PersonnelCode==personnelCodeTrimed);
        }
        public BusinessOperationResult<List<JobApplicantModel>> GetByIds(List<int> jobApplicantIds)
        {
            return GetData<JobApplicantModel>(x => jobApplicantIds.Contains(x.JobApplicantId));
        }

        public BusinessOperationResult<string> GenerateDocumentFile(string nationalCode, string promissoryNoteAmount)
        {
            var result = new BusinessOperationResult<string>();

            try
            {

                string templatePath = Path.Combine("wwwroot", "Templates", "fa-IR", "Word", "Documents.docx");

                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "fa-IR", "Word", $"Documents-{nationalCode}.docx");

                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), templatePath);

                using (var doc = DocX.Load(fullPath))
                {
                    doc.ReplaceText("{{Promissory Note}}", $"{promissoryNoteAmount}");
                    doc.SaveAs(outputPath);
                }
                System.IO.File.ReadAllBytes(outputPath);
                result.SetSuccessResult(outputPath);
                return result;
            }
            catch (Exception ex)
            {
                result.SetErrorMessage(ex.Message + ex.InnerException);
                return result;
            }
        }

        public BusinessOperationResult<JobApplicantModel> GetByNationalCode(string nationalCode)
        {
            return GetFirst<JobApplicantModel>(x => x.NationalCode == nationalCode);
        }

        public BusinessOperationResult<bool> ApproveDocuments(JobApplicantModel jobApplicant)
        {
            var result = new BusinessOperationResult<bool>();

            if (jobApplicant.BaseInformationApproveStatus==ApproveStatus.DocumentsApprove)
            {
                result.SetErrorMessage("Information Has Already Approved");
                return result;
            }
            jobApplicant.BaseInformationApproveStatus=ApproveStatus.DocumentsApprove;
            var updateResult = Update(jobApplicant);
            if (updateResult.ResultEntity)
            {
                sendAsiaSMSService.SendMessage(new Teram.Module.SmsSender.Models.AsiaSms.SendSmsModel
                {
                    Receivers=jobApplicant.MobileNumber,
                    SmsText=$"مدارک بارگذاری شده‌ی شما در سامانه منابع انسانی مورد تایید قرار گرفت"
                });

                result.SetSuccessResult(updateResult.ResultEntity);
                return result;
            }
            else
            {
                result.SetErrorMessage("Error In Update Status");
                return result;
            }
        }
    }
}
