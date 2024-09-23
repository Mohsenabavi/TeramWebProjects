using Newtonsoft.Json;
using System.Net.Http.Headers;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.Module.AttachmentsManagement.Models;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Logic.Interfaces;
using Teram.QC.Module.IncomingGoods.Models;

namespace Teram.QC.Module.IncomingGoods.Logic
{

    public class IncomingGoodsInspectionFileLogic : BusinessOperations<IncomingGoodsInspectionFileModel, IncomingGoodsInspectionFile, int>, IIncomingGoodsInspectionFileLogic
    {
        public IncomingGoodsInspectionFileLogic(IPersistenceService<IncomingGoodsInspectionFile> service, IConfiguration configuration) : base(service)
        {
            this.configuration=configuration??throw new ArgumentNullException(nameof(configuration));
        }
        private readonly List<string> CurrectFileExtentions = new List<string> { "jpg", "jpeg", "png", "pdf" };
        private readonly IConfiguration configuration;

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
        public BusinessOperationResult<IncomingGoodsInspectionFileModel> SaveToDataBase(IFormFile file, int incomingGoodsInspectionId)
        {
            var modelFileInformation = new IncomingGoodsInspectionFileModel()
            {
                FileExtension = Path.GetExtension(file.FileName).Replace(".", ""),
                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                CreatedOn = DateTime.Now,
                IncomingGoodsInspectionId = incomingGoodsInspectionId,
            };
            var finalResult = AddFile(modelFileInformation, file);
            return finalResult;
        }
        public BusinessOperationResult<IncomingGoodsInspectionFileModel> AddFile(IncomingGoodsInspectionFileModel newModel, IFormFile file)
        {
            var result = new BusinessOperationResult<IncomingGoodsInspectionFileModel>();
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
            result.SetSuccessResult(newModel);
            return result;

        }
    }
}
