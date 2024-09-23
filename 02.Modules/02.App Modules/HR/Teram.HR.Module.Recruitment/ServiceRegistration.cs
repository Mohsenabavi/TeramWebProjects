using DinkToPdf;
using DinkToPdf.Contracts;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Logic;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.BaseInfo;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.HR.Module.Recruitment.Models.Questionaire;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;
using Teram.HR.Module.Recruitment.Services;


namespace Teram.HR.Module.Recruitment
{
    public class ServiceRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IMajorLogic, MajorLogic>();
            services.AddScoped<ILogic<MajorModel>, MajorLogic>();

            services.AddScoped<IJobApplicantLogic, JobApplicantLogic>();
            services.AddScoped<ILogic<JobApplicantModel>, JobApplicantLogic>();

            services.AddScoped<IJobApplicantFileLogic, JobApplicantFileLogic>();
            services.AddScoped<ILogic<JobApplicantFileModel>, JobApplicantFileLogic>();

            services.AddScoped<IQuestionLogic, QuestionLogic>();
            services.AddScoped<ILogic<QuestionModel>, QuestionLogic>();

            services.AddScoped<IQuestionnaireLogic, QuestionnaireLogic>();
            services.AddScoped<ILogic<QuestionnaireModel>, QuestionnaireLogic>();

            services.AddScoped<IQuestionnaireQuestionLogic, QuestionnaireQuestionLogic>();
            services.AddScoped<ILogic<QuestionnaireQuestionModel>, QuestionnaireQuestionLogic>();

            services.AddScoped<IPersonQuestionnaireQuestionLogic, PersonQuestionnaireQuestionLogic>();
            services.AddScoped<ILogic<PersonQuestionnaireQuestionModel>, PersonQuestionnaireQuestionLogic>();

            services.AddScoped<IPersonnelQuestionnaireLogic, PersonnelQuestionnaireLogic>();
            services.AddScoped<ILogic<PersonnelQuestionnaireModel>, PersonnelQuestionnaireLogic>();

            services.AddScoped<IBaseInformationLogic, BaseInformationLogic>();
            services.AddScoped<ILogic<BaseInformationModel>, BaseInformationLogic>();

            services.AddScoped<IJobPositionLogic, JobPositionLogic>();
            services.AddScoped<ILogic<JobPositionModel>, JobPositionLogic>();

            services.AddScoped<IJobApplicantsIntroductionLetterLogic, JobApplicantsIntroductionLetterLogic>();
            services.AddScoped<ILogic<JobApplicantsIntroductionLetterModel>, JobApplicantsIntroductionLetterLogic>();

            services.AddScoped<IJobApplicantApproveHistoryLogic, JobApplicantApproveHistoryLogic>();
            services.AddScoped<ILogic<JobApplicantApproveHistoryModel>, JobApplicantApproveHistoryLogic>();

            services.AddScoped<IEmployeeJobBackgroundLogic, EmployeeJobBackgroundLogic>();
            services.AddScoped<ILogic<EmployeeJobBackgroundModel>, EmployeeJobBackgroundLogic>();

            services.AddScoped<IWorkerJobBackgroundLogic, WorkerJobBackgroundLogic>();
            services.AddScoped<ILogic<WorkerJobBackgroundModel>, WorkerJobBackgroundLogic>();

            services.AddScoped<IHSEApproveHistoryLogic, HSEApproveHistoryLogic>();
            services.AddScoped<ILogic<HSEApproveHistoryModel>, HSEApproveHistoryLogic>();

            services.AddScoped<ILogic<HSEGridModel>, HSEApproveLogic>();

            services.AddScoped<IHSEApproveLogic, HSEApproveLogic>();

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.AddScoped<PdfConverterService>();

            services.AddScoped<IIntroductionGenerationService, IntroductionGenerationService>();

            Mapster.TypeAdapterConfig<JobApplicant, JobApplicantModel>.NewConfig().Map(x => x.JobPositionTitle, x => x.JobPosition.Title);
        }
    }
}
