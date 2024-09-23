using Microsoft.AspNetCore.Mvc;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models.WorkFlow;

namespace Teram.QC.Module.FinalProduct.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FinalProductController : Controller
    {
        private readonly IFlowInstructionLogic flowInstructionLogic;

        public FinalProductController(IFlowInstructionLogic flowInstructionLogic)
        {
            this.flowInstructionLogic=flowInstructionLogic??throw new ArgumentNullException(nameof(flowInstructionLogic));
        }

        [HttpPost]
        public ApiResult<List<FlowInstructionModel>> AddFlowInstructions(List<FlowInstructionModel> flowInstructions)
        {

            var result = new List<FlowInstructionModel>();

            foreach (var item in flowInstructions)
            {
                var addResult = flowInstructionLogic.AddNew(item);
                result.Add(addResult.ResultEntity);
            }
            return new ApiResult<List<FlowInstructionModel>>
            {

                Count = result.Count,
                Data = result,
                HasError=false,
                Result="ok"
            };
        }
    }
}
