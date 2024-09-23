using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.Module.AttachmentsManagement.Entities;
using Teram.Module.AttachmentsManagement.Enums;
using Teram.Module.AttachmentsManagement.Logic.Interfaces;
using Teram.Module.AttachmentsManagement.Models;
using Teram.ServiceContracts;

namespace Teram.Module.AttachmentsManagement.Api
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttachmentController : Controller
    {
        private readonly ILogger<AttachmentController> logger;
        private readonly IStringLocalizer<AttachmentController> localizer;
        private readonly IAttachmentLogic attachmentLogic;


        public AttachmentController(ILogger<AttachmentController> logger, IStringLocalizer<AttachmentController> localizer,
            IAttachmentLogic attachmentLogic
            )
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            this.attachmentLogic = attachmentLogic ?? throw new ArgumentNullException(nameof(attachmentLogic));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FileResultModel), StatusCodes.Status400BadRequest)]
        [RequestFormLimits(MultipartBodyLengthLimit = 2147483647)]
        public async Task<FileResultModel> UploadFileAsync(FileModel data)
        {
            if (data == null)
            {
                return new FileResultModel { Status = ResultStatus.Fail, Message = localizer["There is no file to upload"] };
            }
            var result = await WriteFilesAsync(new List<FileModel> { data });
            GC.Collect();
            return result.First();
        }
        private async Task<List<FileResultModel>> WriteFilesAsync(List<FileModel> amsModel)
        {
            var task = await Task.Run(() =>
            {
                var result = new List<FileResultModel>();
                try
                {
                    var attachments = amsModel.Select(x => new AttachmentModel
                    {
                        EntityRealId = x.EntityId,
                        FileData = Convert.FromBase64String(x.File),
                        ContentType = x.ContentType,
                        FileName = x.FileName,
                        FileSize = x.File.Length,
                        IsDeleted = false
                    }).ToList();

                    foreach (var item in attachments)
                    {
                        var saveResult = attachmentLogic.AddNewAttachment(item);
                        if (saveResult.ResultStatus != OperationResultStatus.Successful)
                        {
                            result.Add(new FileResultModel { EntityId = item.EntityRealId, Message = saveResult.ResultEntity.Message, Status = ResultStatus.Fail });
                        }
                        else
                        {
                            result.Add(new FileResultModel { AttachmentId = saveResult.ResultEntity.AttachmentId, EntityId = item.EntityRealId, Status = ResultStatus.Successful, Message = saveResult.ResultEntity.Message });
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(1001, ex.Message, ex);
                    result.Add(new FileResultModel { Status = ResultStatus.Fail, Message = localizer["Something wrong"] });
                }
                return result;
            });
            GC.Collect();
            return task;
        }
        private static byte[] FileToBytes(IFormFile file)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();
            ms.Dispose();
            return fileBytes;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public ActionResult GetFileByGuid(Guid attachmentId)
        {
            var attachment = attachmentLogic.GetFileById(attachmentId);
            if (attachment.ResultStatus == OperationResultStatus.Successful)
            {
                return File(attachment.ResultEntity.FileData, attachment.ResultEntity.ContentType);
            }
            return BadRequest(new { message = "خطای نامشخص" });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public string GetFileBytesByGuid(Guid attachmentId)
        {
            var result = "File No Found";
            var attachment = attachmentLogic.GetFileById(attachmentId);
            if (attachment.ResultStatus == OperationResultStatus.Successful)
            {
                var fileBytes = attachment.ResultEntity.FileData;
                var filaBase64String = Convert.ToBase64String(fileBytes);
                return filaBase64String;
            }
            return result;
        }
    }
}
