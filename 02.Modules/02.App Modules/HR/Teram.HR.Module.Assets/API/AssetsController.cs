using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Assets.Logics.Interfaces;
using Teram.HR.Module.Assets.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Teram.HR.Module.Assets.API
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "TokenBase")]
    public class AssetsController : Controller
    {
        private readonly IRahkaranAssetLogic rahkaranAssetLogic;

        public AssetsController(IRahkaranAssetLogic rahkaranAssetLogic)
        {
            this.rahkaranAssetLogic = rahkaranAssetLogic ?? throw new ArgumentNullException(nameof(rahkaranAssetLogic));
        }

        [HttpGet]

        public ApiResult<List<RahkaranAssetModel>> GetAssetsByPersonnelCode(int personnelCode)
        {
            try
            {
                var data = rahkaranAssetLogic.GetAssetsByPersonnelCode(personnelCode);

                if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
                {
                    return new ApiResult<List<RahkaranAssetModel>>
                    {
                        Count = 0,
                        HasError = true,
                        Message = "خطا در بازیابی اطلاعات",
                        Result = "fail"
                    };
                }
                return new ApiResult<List<RahkaranAssetModel>>
                {
                    Count = data.ResultEntity.Count,
                    HasError = false,
                    Data = data.ResultEntity,
                    Result = "ok",
                    Message = "اطلاعات با موفقیت بازیابی شد"
                };
            }
            catch (Exception ex)
            {
                return new ApiResult<List<RahkaranAssetModel>>
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
