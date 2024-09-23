using Newtonsoft.Json;
using System.Configuration;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.FileUploader.Models;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Module.AttachmentsManagement.Models;
using Teram.Web.Core.Security;

namespace Teram.HR.Module.Recruitment.Logic
{
    public class HSEApproveLogic : BusinessOperations<HSEGridModel, JobApplicant, int>, IHSEApproveLogic
    {
        private readonly IHSEApproveHistoryLogic approveHistoryLogic;
        private readonly IUserPrincipal userPrincipal;
        private readonly IConfiguration configuration;
        private readonly List<string> CurrectFileExtentions = new List<string> { "jpg", "jpeg", "png", "pdf" };
        public HSEApproveLogic(IPersistenceService<JobApplicant> service, IHSEApproveHistoryLogic approveHistoryLogic, IUserPrincipal userPrincipal,
            IConfiguration configuration) : base(service)
        {
            BeforeUpdate+=EntityLogic_BeforeUpdate;
            this.approveHistoryLogic=approveHistoryLogic??throw new ArgumentNullException(nameof(approveHistoryLogic));
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.configuration=configuration??throw new ArgumentNullException(nameof(configuration));
        }

        public BusinessOperationResult<HSEGridModel> GetByJobApplicantId(int jobApplicantId)
        {
            return GetFirst<HSEGridModel>(x => x.JobApplicantId==jobApplicantId);
        }     
        private void EntityLogic_BeforeUpdate(TeramEntityEventArgs<JobApplicant, HSEGridModel, int> entity)
        {
            var historyModeel = new HSEApproveHistoryModel
            {
                OccupationalMedicineApprovedBy= entity.NewEntity.OccupationalMedicineApprovedBy,
                OccupationalMedicineApproveStatus=entity.NewEntity.OccupationalMedicineApproveStatus,
                OccupationalMedicineDate=entity.NewEntity.OccupationalMedicineDate,
                OccupationalMedicineRemarks = entity.NewEntity.OccupationalMedicineRemarks,
                JobApplicantId=entity.NewEntity.JobApplicantId,
                FileSummaryAttchmanetId=entity.Model.FileSummaryAttchmanetId,
                ReferralAtachmentId=entity.Model.ReferralAtachmentId,
                ApprovedByUserId=userPrincipal.CurrentUserId
            };
            approveHistoryLogic.AddNew(historyModeel);
        }
        private bool CheckFileExtention(List<string> list)
        {
            var postFixes = list.Select(x => x.ToLower()).ToList();
            var inValidTypes = postFixes.Except(CurrectFileExtentions).ToList();

            if (inValidTypes.Any())
            {
                return false;
            }
            return true;
        }
        private byte[] ConvertToByteArray(IFormFile file)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();
            return fileBytes;
        }

        private Expression<Func<JobApplicant, bool>> CreateFilterExpression(string? firstName, string? lastName, string? personnelCode, string? nationalCode, bool viewInprogressStatus,FlowType? flowType,ProcessStatus? processStatus)
        {
            Expression<Func<JobApplicant, bool>> query = x => true;

            if (viewInprogressStatus)
            {
                query = query.AndAlso(x => x.ProcessStatus!=ProcessStatus.FinalApproveByHR);
            }

            if (flowType.HasValue) {

                query = query.AndAlso(x => x.FlowType==flowType);
            }

            if (processStatus.HasValue)
            {
                query = query.AndAlso(x => x.ProcessStatus==processStatus);
            }

            if (!string.IsNullOrEmpty(nationalCode))
            {
                query = query.AndAlso(x => x.NationalCode.Contains(nationalCode));
            }

            if (!string.IsNullOrEmpty(personnelCode))
            {
                query = query.AndAlso(x => x.PersonnelCode.Contains(personnelCode));
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.AndAlso(x => x.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.AndAlso(x => x.LastName.Contains(lastName));
            }
            return query;
        }
        public BusinessOperationResult<Guid> AddFile(HSEGridModel approveModel, IFormFile file)
        {

            var result = new BusinessOperationResult<Guid>();

            var checkResult = CheckFileExtention(new List<string> { Path.GetExtension(file.FileName).Replace(".", "") });
            if (!checkResult)
            {
                result.SetErrorMessage("فرمت فایل پیوست پشتیبانی نمی شود");
                return result;
            }

            var model = new FileModel
            {
                ApplicationAttachementTypeId = 1,
                Description = "",
                FileName = file.FileName,
                File = Convert.ToBase64String(ConvertToByteArray(file)),
                ContentType = file.ContentType
            };

            try
            {
                var uploadUrl = configuration.GetSection("Attachment").Get<AttachmentSection>();
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uploadUrl.UploadUrl);
                request.Headers.Add("accept", "text/plain");
                string jsonString = JsonConvert.SerializeObject(model);
                request.Content = new StringContent(jsonString);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                var jsonModel = JsonConvert.DeserializeObject<FileResultModel>(responseBody);
                result.SetSuccessResult(jsonModel.AttachmentId.Value);
            }
            catch (Exception)
            {
                result.SetErrorMessage("Error in Upload Files");
            }

            return result;
        }

        public BusinessOperationResult<List<HSEGridModel>> GetHSEDataByFilter(string firstName, string lastName, string personnelCode, string nationalCode, bool viewInprogressStatus, FlowType? flowType, ProcessStatus? processStatus, int? start = null, int? length = null)
        {
            var query = CreateFilterExpression(firstName, lastName, personnelCode, nationalCode, viewInprogressStatus, flowType, processStatus);
            query = query.AndAlso(x => x.ProcessStatus>=ProcessStatus.AdmittingToDoExpriments && x.FlowType==flowType);
            return GetByProject<HSEGridModel>(query, row: start, max: length, orderByDescending: true);
        }
    }

}
