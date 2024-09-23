using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic
{

    public class QCControlPlanLogic : BusinessOperations<QCControlPlanModel, QCControlPlan, int>, IQCControlPlanLogic
    {
        public QCControlPlanLogic(IPersistenceService<QCControlPlan> service) : base(service)
        {

        }

        public BusinessOperationResult<QCControlPlanModel> GetByTitle(string title)
        {
            var data = GetFirst<QCControlPlanModel>(x => x.Title==title);
            return data;
        }
    }

}
