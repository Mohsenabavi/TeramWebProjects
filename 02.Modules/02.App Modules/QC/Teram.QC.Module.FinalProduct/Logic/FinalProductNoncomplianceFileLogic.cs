using Newtonsoft.Json;
using System.Net.Http.Headers;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.FileUploader.Models;
using Teram.Module.AttachmentsManagement.Models;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic
{

    public class FinalProductNoncomplianceFileLogic : BusinessOperations<FinalProductNoncomplianceFileModel, FinalProductNoncomplianceFile, int>, IFinalProductNoncomplianceFileLogic
    {
        private readonly IConfiguration configuration;
        private readonly List<string> CurrectFileExtentions = new List<string> { "jpg", "jpeg", "png", "pdf" };
        public FinalProductNoncomplianceFileLogic(IPersistenceService<FinalProductNoncomplianceFile> service,
            IConfiguration configuration) : base(service)
        {
            this.configuration=configuration;
            BeforeAdd+=FinalProductNoncomplianceFileLogic_BeforeAdd;
        }
        private void FinalProductNoncomplianceFileLogic_BeforeAdd(TeramEntityEventArgs<FinalProductNoncomplianceFile, FinalProductNoncomplianceFileModel, int> entity)
        {
            entity.NewEntity.CreatedOn = DateTime.Now;
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

        private ShowAttachmentModel CreateShowAttachmentModel(List<FinalProductNoncomplianceFileModel> attachments)
        {
            var initialPreview = new List<string>();
            var init = new List<InitialPreviewConfig>();
            var downloadurl = configuration.GetSection("Attachment").Get<AttachmentSection>().DownloadUrl;
            int i = 0;
            foreach (var item in attachments)
            {

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
                    extra = { id = item.FinalProductNoncomplianceFileId },
                });
                i++;
            }
            var showAttachmentModel = new ShowAttachmentModel { InitialPreview = initialPreview, InitialPreviewConfig = init };
            return showAttachmentModel;
        }

        private BusinessOperationResult<FinalProductNoncomplianceFileModel> AddFile(FinalProductNoncomplianceFileModel newModel, IFormFile file)
        {
            var checkResult = CheckFileExtention(new List<string> { Path.GetExtension(file.FileName).Replace(".", "") });
            if (!checkResult)
            {
                BusinessOperationResult<FinalProductNoncomplianceFileModel> result = new();
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

        public BusinessOperationResult<FinalProductNoncomplianceFileModel> SaveToDataBase(IFormFile file, int finalProductNoncomplianceId)
        {
            var modelFileInformation = new FinalProductNoncomplianceFileModel()
            {
                FileExtension = Path.GetExtension(file.FileName).Replace(".", ""),
                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                CreatedOn = DateTime.Now,
                FinalProductNoncomplianceId = finalProductNoncomplianceId,
            };
            var finalResult = AddFile(modelFileInformation, file);
            return finalResult;
        }

        public BusinessOperationResult<ShowAttachmentModel> GetAttachmentsByFinalProductNoncomplianceFileId(int finalProductNoncomplianceFileId)
        {
            var result = new BusinessOperationResult<ShowAttachmentModel>();

            var attachmentResult = GetByFinalProductNoncomplianceId(finalProductNoncomplianceFileId);

            if (attachmentResult.ResultStatus != OperationResultStatus.Successful || attachmentResult.ResultEntity == null)
                throw new Exception("فایل یافت نشد");

            var showAttachmentModel = CreateShowAttachmentModel(attachmentResult.ResultEntity);
            result.SetSuccessResult(showAttachmentModel);
            return result;
        }
        public BusinessOperationResult<List<FinalProductNoncomplianceFileModel>> GetByFinalProductNoncomplianceId(int finalProductNoncomplianceId)
        {
            return GetData<FinalProductNoncomplianceFileModel>(x => x.FinalProductNoncomplianceId == finalProductNoncomplianceId);
        }
    }

}
