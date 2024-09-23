using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class FinalProductNoncomplianceDetailSampleLogic : BusinessOperations<FinalProductNoncomplianceDetailSampleModel, FinalProductNoncomplianceDetailSample, int>, IFinalProductNoncomplianceDetailSampleLogic
    {
        public FinalProductNoncomplianceDetailSampleLogic(IPersistenceService<FinalProductNoncomplianceDetailSample> service) : base(service)
        {

        }
    }

}
