using Microsoft.AspNetCore.Mvc;
using Teram.Framework.Core.Logic;
using Teram.Module.AttachmentsManagement.Entities;
using Teram.Module.AttachmentsManagement.Models;

namespace Teram.Module.AttachmentsManagement.Logic.Interfaces
{  
    public interface IAttachmentLogic : IBusinessOperations<AttachmentModel, Attachmant, int>
    {
        BusinessOperationResult<AttachmentModel> GetFileById(Guid attachmenmtId);
        BusinessOperationResult<FileResultModel> AddNewAttachment(AttachmentModel attachment);
        byte[] ConvertToByteArray(IFormFile file);
    }

}
