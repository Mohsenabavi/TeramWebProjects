using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.Module.AttachmentsManagement.Entities;
using Teram.Module.AttachmentsManagement.Enums;
using Teram.Module.AttachmentsManagement.Logic.Interfaces;
using Teram.Module.AttachmentsManagement.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Teram.Module.AttachmentsManagement.Logic
{
    public class AttachmentLogic : BusinessOperations<AttachmentModel, Attachmant, int>, IAttachmentLogic
    {
        public AttachmentLogic(IPersistenceService<Attachmant> service) : base(service)
        {

        }

        public BusinessOperationResult<FileResultModel> AddNewAttachment(AttachmentModel attachment)
        {

            var result = new BusinessOperationResult<FileResultModel>();

            try
            {
                var data = attachment.Adapt<Entities.Attachmant>();
                Service.Entities.Add(data);
                Service.Save();
                var resultModel = new FileResultModel
                {
                    AttachmentId = data.AttachmantId,
                    EntityId = data.EntityRealId,
                    Message = "File Saved Succefully",
                    Status = ResultStatus.Successful
                };

                result.SetSuccessResult(resultModel);
                return result;
            }
            catch (Exception ex)
            {
                var resultModel = new FileResultModel
                {
                    AttachmentId = Guid.Empty,
                    EntityId = 0,
                    Message = "Error In Saving File" + ex.Message + ex.InnerException,
                    Status = ResultStatus.Fail
                };

                result.SetSuccessResult(resultModel);
                return result;
            }
        }

        public BusinessOperationResult<AttachmentModel> GetFileById(Guid attachmenmtId)
        {

            var result = new BusinessOperationResult<AttachmentModel>();

            try
            {
                var data = Service.DeferrQuery(x => x.AttachmantId == attachmenmtId).FirstOrDefault();

                if (data != null)
                {
                    var resultModel = new AttachmentModel
                    {

                        AttachmantId = data.AttachmantId,
                        ContentType = data.ContentType,
                        CreatedOn = data.CreatedOn,
                        EntityRealId = data.EntityRealId,
                        FileData = data.FileData,
                        FileName = data.FileName,
                        FileSize = data.FileSize,
                        IsDeleted = data.IsDeleted,
                    };
                    result.SetSuccessResult(resultModel);

                    return result;
                }
                else
                {
                    result.SetErrorMessage("file Not found");
                    return result;
                }
            }
            catch (Exception)
            {
                result.SetErrorMessage("error in get File");
                return result;
            }
        }

        public byte[] ConvertToByteArray(IFormFile file)
        {

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                return fileBytes;
            }
        }
    }
}
