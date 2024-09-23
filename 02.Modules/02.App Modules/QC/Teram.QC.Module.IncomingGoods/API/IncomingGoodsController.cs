using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.IncomingGoods.Models.ServiceModels;
using Teram.QC.Module.IncomingGoods.Services;

namespace Teram.QC.Module.IncomingGoods.API
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Authorize(AuthenticationSchemes = "TokenBase")]
    public class IncomingGoodsController : Controller
    {
        private readonly IRahkaranService rahkaranService;

        public IncomingGoodsController(IRahkaranService rahkaranService)
        {
            this.rahkaranService=rahkaranService??throw new ArgumentNullException(nameof(rahkaranService));
        }

        [HttpGet]
        public async Task<ApiResult<List<QualityInspectionResultModel>>> GetQualityInspectionData(string number)
        {
            try
            {
                var data = await rahkaranService.GetQualityInspectionData(number);

                return new ApiResult<List<QualityInspectionResultModel>>
                {
                    Count = data.Count,
                    Data = data,
                    HasError = false,
                    Message="اطلاعات با موفقیت بازیابی شد",
                    Result="ok"
                };
            }
            catch (Exception ex)
            {
                return new ApiResult<List<QualityInspectionResultModel>>
                {
                    Count = 0,
                    HasError = false,
                    Result = "fail",
                    Message = "خطا در بازیابی اطلاعات"
                };
            }
        }
    }
}
