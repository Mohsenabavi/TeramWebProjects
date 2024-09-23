using System.Linq.Expressions;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;

namespace Teram.HR.Module.Recruitment.Logic
{
    public class BaseInformationLogic : BusinessOperations<BaseInformationModel, BaseInformation, int>, IBaseInformationLogic
    {
        private readonly IJobApplicantLogic jobApplicantLogic;

        public BaseInformationLogic(IPersistenceService<BaseInformation> persistenceService, IJobApplicantLogic jobApplicantLogic) : base(persistenceService)
        {
            AfterAdd += BaseInformationLogic_AfterAdd;
            this.jobApplicantLogic = jobApplicantLogic ?? throw new ArgumentNullException(nameof(jobApplicantLogic));
        }

        private void BaseInformationLogic_AfterAdd(TeramEntityEventArgs<BaseInformation, BaseInformationModel, int> entity)
        {
            var relatedJobApplicant = jobApplicantLogic.GetRow(entity.NewEntity.JobApplicantId);
            relatedJobApplicant.ResultEntity.ProcessStatus = Enums.ProcessStatus.BaseInformationAdded;
            jobApplicantLogic.Update(relatedJobApplicant.ResultEntity);
        }
        public BusinessOperationResult<List<BaseInformationModel>> GetBaseInformationByFilter(string firstName, string lastName, DateTime? startDateRegistration, DateTime? endDateRegistration, int? start, int? length)
        {
            var result = new BusinessOperationResult<List<BaseInformationModel>>();
            Expression<Func<BaseInformation, bool>> query = null;

            if (startDateRegistration.HasValue)
            {
                if (query == null)
                    query = x => x.CreatedOn.Date >= startDateRegistration.Value.Date;
                else
                    query = query.AndAlso(x => x.CreatedOn.Date >= startDateRegistration.Value.Date);
            }

            if (endDateRegistration.HasValue)
            {
                if (query == null)
                    query = x => x.CreatedOn.Date <= endDateRegistration.Value.Date;
                else
                    query = query.AndAlso(x => x.CreatedOn.Date <= endDateRegistration.Value.Date);
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                if (query == null)
                    query = x => x.Name == firstName;
                else
                    query = query.AndAlso(x => x.Name == firstName);
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                if (query == null)
                    query = x => x.Lastname == lastName;
                else
                    query = query.AndAlso(x => x.Lastname == lastName);
            }

            result = GetData<BaseInformationModel>(query, null, row: start, max: length);

            return result;
        }

        public BusinessOperationResult<BaseInformationModel> GetBaseInformationById(int baseInformationId)
        {
            return GetFirst<BaseInformationModel>(x => x.BaseInformationId == baseInformationId);
        }

        public BusinessOperationResult<BaseInformationModel> GetBaseInformationByNationalCode(string nationalcode)
        {
            return GetFirst<BaseInformationModel>(x => x.NationalCode == nationalcode);
        }

        public BusinessOperationResult<BaseInformationModel> GetByJobApplicantId(int jobApplicantId)
        {
            return GetFirst<BaseInformationModel>(x => x.JobApplicantId == jobApplicantId);
        }

        public BusinessOperationResult<List<BaseInformationModel>> GetByJobApplicantIds(List<int> jobApplicantIds)
        {
            return GetData<BaseInformationModel>(x => jobApplicantIds.Contains(x.JobApplicantId));
        }
    }
}
