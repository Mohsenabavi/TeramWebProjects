using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Logic.Interfaces;
using Teram.QC.Module.IncomingGoods.Models;

namespace Teram.QC.Module.IncomingGoods.Logic
{

    public class ControlPlanLogic : BusinessOperations<ControlPlanModel, ControlPlan, int>, IControlPlanLogic
    {
        public ControlPlanLogic(IPersistenceService<ControlPlan> service) : base(service)
        {

        }

        public BusinessOperationResult<List<ControlPlanModel>> GetByCategoryId(int categoryId)
        {
            return GetData<ControlPlanModel>(x => x.ControlPlanCategoryId==categoryId);
        }

        public BusinessOperationResult<List<ControlPlanModel>> GetByFilter(int? controlPlanCategoryId,int? start = null, int? length = null)
        {
            var query = CreateFilterExpression(controlPlanCategoryId);
            return GetData<ControlPlanModel>(query, row: start.Value, max: length.Value, orderByMember: "ControlPlanId", orderByDescending: true);
        }

        private Expression<Func<ControlPlan, bool>> CreateFilterExpression(int? controlPlanCategoryId)
        {
            Expression<Func<ControlPlan, bool>> query = x => true;

            if (controlPlanCategoryId>0)
            {
                query = query.AndAlso(x => x.ControlPlanCategoryId == controlPlanCategoryId);
            }           
            return query;
        }
    }

}
