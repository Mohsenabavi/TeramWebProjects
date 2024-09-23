using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.FileUploader.Models;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Migrations;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Module.AttachmentsManagement.Enums;
using Teram.Module.AttachmentsManagement.Models;
using Teram.Module.FileUploader.Models;
using Teram.ServiceContracts;
using Teram.Web.Core.Model;

namespace Teram.HR.Module.Recruitment.Logic
{
    public class JobApplicantFileLogic : BusinessOperations<JobApplicantFileModel, JobApplicantFile, int>, IJobApplicantFileLogic
    {
        private readonly IConfiguration configuration;
        private readonly List<string> CurrectFileExtentions = new List<string> { "jpg", "jpeg", "png", "pdf", "jfif", "bmp" };

        #region privateMethods
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




        private ShowAttachmentModel CreateShowAttachmentModel(List<JobApplicantFileModel> attachments)
        {
            var initialPreview = new List<string>();
            var init = new List<InitialPreviewConfig>();
            var downloadurl = configuration.GetSection("Attachment").Get<AttachmentSection>().DownloadUrl;
            int i = 0;
            foreach (var item in attachments)
            {
                bool isApproved = false;

                if (item.ApproveStatus==ApproveStatus.FisrtApproved)
                {
                    isApproved=true;
                }

                initialPreview.Add(item.FileName + '.' + item.FileExtension);

                init.Add(
                new InitialPreviewConfig
                {
                    type = item.FileExtension,
                    caption = item.FileName + '.' + item.FileExtension,
                    width = "120px",
                    url = "/",
                    downloadUrl = downloadurl + item.AttachmentId,
                    key = i,
                    extra = { id = item.JobApplicantFileId },
                    isApproved = isApproved
                });
                i++;
            }
            var showAttachmentModel = new ShowAttachmentModel { InitialPreview = initialPreview, InitialPreviewConfig = init };
            return showAttachmentModel;
        }

        #endregion
        public JobApplicantFileLogic(IPersistenceService<JobApplicantFile> service, IConfiguration configuration) : base(service)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public BusinessOperationResult<JobApplicantFileModel> AddFile(JobApplicantFileModel newModel, IFormFile file)
        {
            long fileSizeInBytes = file.Length;
            double fileSizeInKilobytes = fileSizeInBytes / 1024.0;
            double fileSizeInMegabytes = fileSizeInKilobytes / 1024.0;

            BusinessOperationResult<JobApplicantFileModel> result = new();

            if (fileSizeInMegabytes > 5)
            {               
                result.SetErrorMessage("سایز فایل پیوست پشتیبانی نمی شود");
                return result;
            }

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
                FileName = newModel.FileName,
                File = Convert.ToBase64String(ConvertToByteArray(file)),
                ContentType = file.ContentType
            };
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
            newModel.AttachmentId = jsonModel.AttachmentId.Value;
            return AddNew(newModel);
        }

        public async Task<BusinessOperationResult<string>> GetFileBytes(Guid attachmentId)
        {

            var finalResult = new BusinessOperationResult<string>();
            var apiUrl = configuration.GetSection("Attachment").Get<AttachmentSection>();
            var requestUri = $"{apiUrl?.GetFileBytesUrl}{attachmentId}";
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("accept", "text/plain");
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            finalResult.SetSuccessResult(responseBody);
            return finalResult;

        }
        public BusinessOperationResult<JobApplicantFileModel> SaveToDataBase(IFormFile file, int jobApplicantId, int attachemntTypeId)
        {
            var modelFileInformation = new JobApplicantFileModel()
            {
                FileExtension = Path.GetExtension(file.FileName).Replace(".", ""),
                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                CreatedOn = DateTime.Now,
                JobApplicantId = jobApplicantId,
                AttachmentTypeId = attachemntTypeId
            };
            var finalResult = AddFile(modelFileInformation, file);
            return finalResult;
        }

        public BusinessOperationResult<ShowAttachmentModel> GetAttachmentsByJobApplicantIdAndAttachmentTypeId(int jobApplicantId, int attachmentTypeId)
        {
            var result = new BusinessOperationResult<ShowAttachmentModel>();

            var attachmentResult = GetByJobApplicantIdAndAttachmentTypeId(jobApplicantId, attachmentTypeId);

            if (attachmentResult.ResultStatus != OperationResultStatus.Successful || attachmentResult.ResultEntity == null)
                throw new Exception("فایل یافت نشد");

            var showAttachmentModel = CreateShowAttachmentModel(attachmentResult.ResultEntity);
            result.SetSuccessResult(showAttachmentModel);
            return result;
        }

        public BusinessOperationResult<List<JobApplicantFileModel>> GetByJobApplicantIdAndAttachmentTypeId(int jobApplicantId, int attachmentTypeId)
        {
            return GetData<JobApplicantFileModel>(x => x.JobApplicantId == jobApplicantId && x.AttachmentTypeId == attachmentTypeId && x.DeletedOn == null);
        }

        public BusinessOperationResult<List<JobApplicantFileModel>> GetByJobApplicantId(int jobApplicantId)
        {
            return GetData<JobApplicantFileModel>(x => x.JobApplicantId == jobApplicantId && x.DeletedOn == null);
        }

        public BusinessOperationResult<List<FileUploadResultModel>> SaveFiles(IFormFileCollection files, int jobApplicantId, List<JobApplicantFileModel> uploadedFiles)
        {
            var finalResult = new BusinessOperationResult<List<FileUploadResultModel>>();

            var resultList = new List<FileUploadResultModel>();


            foreach (var file in files)
            {
                var uploadResult = new FileUploadResultModel();
                var contentDisposition = file.ContentDisposition;
                int attachmentTypeId = RecognizeAttachmentType(contentDisposition);
                if (attachmentTypeId == (int)DocumentType.PersonalImage)
                {
                    string fileExtension = Path.GetExtension(file.FileName);
                    if (!fileExtension.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) &&
                        !fileExtension.Equals(".png", StringComparison.CurrentCultureIgnoreCase) &&
                        !fileExtension.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase) &&
                        !fileExtension.Equals(".tiff", StringComparison.CurrentCultureIgnoreCase) &&
                        !fileExtension.Equals(".bmp", StringComparison.CurrentCultureIgnoreCase) &&
                        !fileExtension.Equals(".gif", StringComparison.CurrentCultureIgnoreCase) &&
                        !fileExtension.Equals(".webp", StringComparison.CurrentCultureIgnoreCase) &&
                        !fileExtension.Equals(".jfif", StringComparison.CurrentCultureIgnoreCase))
                    {
                        uploadResult.IsSuccess=false;
                        uploadResult.Message="خطا در ذخیره فایل عکس پرسنلی، فرمت فایل مجاز نیست";
                        uploadResult.File=file;
                        resultList.Add(uploadResult);
                        continue;
                    }
                }
                var checkForExisting = uploadedFiles.FirstOrDefault(x => x.AttachmentTypeId==attachmentTypeId);
                if (checkForExisting==null)
                {
                    var saveResult = SaveToDataBase(file, jobApplicantId, attachmentTypeId);
                    if (saveResult.ResultStatus != OperationResultStatus.Successful || saveResult.ResultEntity is null)
                    {
                        uploadResult.IsSuccess=false;
                        uploadResult.Message=saveResult.AllMessages;
                        uploadResult.File=file;
                        resultList.Add(uploadResult);
                        continue;
                    }
                    else
                    {
                        uploadResult.IsSuccess=true;
                        uploadResult.AttachmentId=saveResult.ResultEntity.AttachmentId;
                        uploadResult.File=file;
                        resultList.Add(uploadResult);
                    }
                }
                else
                {
                    DocumentType documentType = (DocumentType)attachmentTypeId;
                    uploadResult.IsSuccess=false;
                    uploadResult.Message=$"{documentType.GetDescription()} قبلا آپلود شده است";
                    uploadResult.File=file;
                    resultList.Add(uploadResult);
                    continue;
                }
            }

            if (resultList.FirstOrDefault(x => x.IsSuccess==false)==null)
            {
                finalResult.SetSuccessResult(resultList);
            }

            else
            {
                finalResult.ResultEntity=new List<FileUploadResultModel>();
                finalResult.SetErrorMessage("فرمت و سایز فایل را چک کنید،حجم مجاز حداکثر 2 مگابایت و فرمت های مجاز  jpg / jpeg / png / pdf / jfif می باشد  ");
            }

            return finalResult;
        }

        public int RecognizeAttachmentType(string contentDisposition)
        {
            int attachemntTypeId = 0;

            if (contentDisposition.Contains("OnNationalCard"))
            {

                attachemntTypeId = (int)(DocumentType.OnNationalCard);
            }
            else if (contentDisposition.Contains("BehindNationalCard"))
            {

                attachemntTypeId = (int)(DocumentType.BehindNationalCard);
            }
            else if (contentDisposition.Contains("BirthCertificateFirstPage"))
            {

                attachemntTypeId = (int)(DocumentType.BirthCertificateFirstPage);
            }

            else if (contentDisposition.Contains("BirthCertificateSecondPage"))
            {

                attachemntTypeId = (int)(DocumentType.BirthCertificateSecondPage);
            }
            else if (contentDisposition.Contains("BirthCertificateThirdPage"))
            {

                attachemntTypeId = (int)(DocumentType.BirthCertificateThirdPage);
            }

            else if (contentDisposition.Contains("BirthCertificateForthPage"))
            {

                attachemntTypeId = (int)(DocumentType.BirthCertificateForthPage);
            }
            else if (contentDisposition.Contains("MilitaryService"))
            {

                attachemntTypeId = (int)(DocumentType.MilitaryService);
            }
            else if (contentDisposition.Contains("PersonalImage"))
            {

                attachemntTypeId = (int)(DocumentType.PersonalImage);
            }

            else if (contentDisposition.Contains("EducationCertificate"))
            {

                attachemntTypeId = (int)(DocumentType.EducationCertificate);
            }

            else if (contentDisposition.Contains("InsuranceCard"))
            {

                attachemntTypeId = (int)(DocumentType.InsuranceCard);
            }

            else if (contentDisposition.Contains("Resume"))
            {
                attachemntTypeId = (int)(DocumentType.Resume);
            }

            else if (contentDisposition.Contains("Resume"))
            {
                attachemntTypeId = (int)(DocumentType.Resume);
            }

            else if (contentDisposition.Contains("Referral"))
            {
                attachemntTypeId = (int)(DocumentType.Referral);
            }

            else if (contentDisposition.Contains("FileSummary"))
            {
                attachemntTypeId = (int)(DocumentType.FileSummary);
            }

            else if (contentDisposition.Contains("PromissoryNote"))
            {
                attachemntTypeId = (int)(DocumentType.PromissoryNote);
            }

            else if (contentDisposition.Contains("NoBadBackground"))
            {
                attachemntTypeId = (int)(DocumentType.NoBadBackground);
            }

            else if (contentDisposition.Contains("InterviewEvaluation"))
            {
                attachemntTypeId = (int)(DocumentType.InterviewEvaluation);
            }
            else if (contentDisposition.Contains("EysenckFormFront"))
            {
                attachemntTypeId = (int)(DocumentType.EysenckFormFront);
            }
            else if (contentDisposition.Contains("NoAddictionForm"))
            {
                attachemntTypeId = (int)(DocumentType.NoAddictionForm);
            }
            else if (contentDisposition.Contains("PartnerBirthCertFirstPage"))
            {
                attachemntTypeId = (int)(DocumentType.PartnerBirthCertFirstPage);
            }
            else if (contentDisposition.Contains("PartnerBirthCertSecondPage"))
            {
                attachemntTypeId = (int)(DocumentType.PartnerBirthCertSecondPage);
            }
            else if (contentDisposition.Contains("PartnerMelliCard"))
            {
                attachemntTypeId = (int)(DocumentType.PartnerMelliCard);
            }
            else if (contentDisposition.Contains("FirstChildBirthCert"))
            {
                attachemntTypeId = (int)(DocumentType.FirstChildBirthCert);
            }
            else if (contentDisposition.Contains("SecondChildBirthCert"))
            {
                attachemntTypeId = (int)(DocumentType.SecondChildBirthCert);
            }
            else if (contentDisposition.Contains("ThirdChildBirthCert"))
            {
                attachemntTypeId = (int)(DocumentType.ThirdChildBirthCert);
            }
            else if (contentDisposition.Contains("ForthChildBirthCert"))
            {
                attachemntTypeId = (int)(DocumentType.ForthChildBirthCert);
            }
            else if (contentDisposition.Contains("FifthChildBirthCert"))
            {
                attachemntTypeId = (int)(DocumentType.FifthChildBirthCert);
            }
            else if (contentDisposition.Contains("SixthChildBirthCert"))
            {
                attachemntTypeId = (int)(DocumentType.SixthChildBirthCert);
            }
            else if (contentDisposition.Contains("EysenckFormBehind"))
            {
                attachemntTypeId = (int)(DocumentType.EysenckFormBehind);
            }
            else if (contentDisposition.Contains("OnEmploymentQuestionnaire"))
            {
                attachemntTypeId = (int)(DocumentType.OnEmploymentQuestionnaire);
            }
            else if (contentDisposition.Contains("BehindEmploymentQuestionnaire"))
            {
                attachemntTypeId = (int)(DocumentType.BehindEmploymentQuestionnaire);
            }
            else if (contentDisposition.Contains("BackgroundAttchament1"))
            {
                attachemntTypeId = (int)(DocumentType.BackgroundAttchament1);
            }
            else if (contentDisposition.Contains("BackgroundAttchament2"))
            {
                attachemntTypeId = (int)(DocumentType.BackgroundAttchament2);
            }
            return attachemntTypeId;
        }

        public BusinessOperationResult<JobApplicantFileModel> GetPersonalImage(int jobApplicantId)
        {
            return GetFirst<JobApplicantFileModel>(x => x.JobApplicantId == jobApplicantId && x.AttachmentTypeId==(int)DocumentType.PersonalImage && x.DeletedOn==null);
        }
        public BusinessOperationResult<List<JobApplicantFileModel>> GetJobApplicantDocumentsExceptResume(int jobApplicantId)
        {
            return GetData<JobApplicantFileModel>(x => x.AttachmentTypeId!=11 && x.JobApplicantId==jobApplicantId &&  x.DeletedOn==null);
        }

        public BusinessOperationResult<JobApplicantFileModel> GetResumeFile(int jobApplicantId)
        {
            return GetFirst<JobApplicantFileModel>(x => x.JobApplicantId == jobApplicantId && x.AttachmentTypeId==(int)DocumentType.Resume && x.DeletedOn==null);
        }
    }
}
