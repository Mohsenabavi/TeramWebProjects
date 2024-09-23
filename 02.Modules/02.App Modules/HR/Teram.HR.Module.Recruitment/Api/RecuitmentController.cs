using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Services;
using Teram.Module.AttachmentsManagement.Logic;

namespace Teram.HR.Module.Recruitment.Api
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "TokenBase")]
    public class RecuitmentController : Controller
    {
        private readonly PdfConverterService pdfConverterService;

        public RecuitmentController(PdfConverterService pdfConverterService)
        {
            this.pdfConverterService=pdfConverterService;
        }

        [HttpGet]
        public ApiResult<string> GetHealth()
        {

            return new ApiResult<string>
            {

                Count = 1,
                Data = "Ok",
                HasError = false,
                Message = "done",
                Result = "ok"

            };
        }        
    }
}
