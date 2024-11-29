using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Entities.WorkFlow;
using Teram.QC.Module.FinalProduct.Jobs;
using Teram.QC.Module.FinalProduct.Logic;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.QC.Module.FinalProduct.Models.WorkFlow;
using Teram.QC.Module.FinalProduct.Services;

namespace Teram.QC.Module.FinalProduct
{
    public class ServiceRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<ILogic<QCControlPlanModel>, QCControlPlanLogic>();
            services.AddScoped<IQCControlPlanLogic, QCControlPlanLogic>();

            services.AddScoped<ILogic<QCDefectModel>, QCDefectLogic>();
            services.AddScoped<IQCDefectLogic, QCDefectLogic>();

            services.AddScoped<ILogic<AcceptancePeriodModel>, AcceptancePeriodLogic>();
            services.AddScoped<IAcceptancePeriodLogic, AcceptancePeriodLogic>();

            services.AddScoped<ILogic<ControlPlanDefectModel>, ControlPlanDefectLogic>();
            services.AddScoped<IControlPlanDefectLogic, ControlPlanDefectLogic>();

            services.AddScoped<IFinalProductInspectionDefectLogic, FinalProductInspectionDefectLogic>();
            services.AddScoped<ILogic<FinalProductInspectionDefectModel>, FinalProductInspectionDefectLogic>();

            services.AddScoped<IQueryService, QueryService>();

            services.AddScoped<IFinalProductInspectionLogic, FinalProductInspectionLogic>();
            services.AddScoped<ILogic<FinalProductInspectionModel>, FinalProductInspectionLogic>();

            services.AddScoped<IFinalProductNoncomplianceLogic, FinalProductNoncomplianceLogic>();
            services.AddScoped<ILogic<FinalProductNoncomplianceModel>, FinalProductNoncomplianceLogic>();

            services.AddScoped<IFinalProductNoncomplianceDetailLogic, FinalProductNoncomplianceDetailLogic>();
            services.AddScoped<ILogic<FinalProductNoncomplianceDetailModel>, FinalProductNoncomplianceDetailLogic>();

            services.AddScoped<IFinalProductNoncomplianceFileLogic, FinalProductNoncomplianceFileLogic>();
            services.AddScoped<ILogic<FinalProductNoncomplianceFileModel>, FinalProductNoncomplianceFileLogic>();

            services.AddScoped<IFinalProductNoncomplianceDetailSampleLogic, FinalProductNoncomplianceDetailSampleLogic>();
            services.AddScoped<ILogic<FinalProductNoncomplianceDetailSampleModel>, FinalProductNoncomplianceDetailSampleLogic>();

            services.AddScoped<IFlowInstructionLogic, FlowInstructionLogic>();
            services.AddScoped<ILogic<FlowInstructionModel>, FlowInstructionLogic>();

            services.AddScoped<IFlowInstructionConditionLogic, FlowInstructionConditionLogic>();
            services.AddScoped<ILogic<FlowInstructionConditionModel>, FlowInstructionConditionLogic>();

            services.AddScoped<IFinalProductNonComplianceCartableItemLogic, FinalProductNonComplianceCartableItemLogic>();
            services.AddScoped<ILogic<FinalProductNonComplianceCartableItemModel>, FinalProductNonComplianceCartableItemLogic>();

            services.AddScoped<IManageCartableLogic, ManageCartableLogic>();

            services.AddScoped<ICausationLogic, CausationLogic>();
            services.AddScoped<ILogic<CausationModel>, CausationLogic>();

            services.AddScoped<IInstructionLogic, InstructionLogic>();
            services.AddScoped<ILogic<InstructionModel>, InstructionLogic>();

            services.AddScoped<IMachineLogic, MachineLogic>();
            services.AddScoped<ILogic<MachineModel>, MachineLogic>();

            services.AddScoped<IOperatorLogic, OperatorLogic>();
            services.AddScoped<ILogic<OperatorModel>, OperatorLogic>();

            services.AddScoped<IRootCauseLogic, RootCauseLogic>();
            services.AddScoped<ILogic<RootCauseModel>, RootCauseLogic>();

            services.AddScoped<IUnitLogic, UnitLogic>();
            services.AddScoped<ILogic<UnitModel>, UnitLogic>();

            services.AddScoped<IWorkStationLogic, WorkStationLogic>();
            services.AddScoped<ILogic<WorkStationModel>, WorkStationLogic>();

            services.AddScoped<ICorrectiveActionLogic, CorrectiveActionLogic>();
            services.AddScoped<ILogic<CorrectiveActionModel>, CorrectiveActionLogic>();

            services.AddScoped<IActionerLogic, ActionerLogic>();
            services.AddScoped<ILogic<ActionerModel>, ActionerLogic>();

            services.AddScoped<IRawMaterialLogic, RawMaterialLogic>();
            services.AddScoped<ILogic<RawMaterialModel>, RawMaterialLogic>();

            services.AddScoped<IMachineryCauseLogic, MachineryCauseLogic>();
            services.AddScoped<ILogic<MachineryCauseModel>, MachineryCauseLogic>();

            services.AddScoped<INotificationService, NotificationService>();

            Mapster.TypeAdapterConfig<AcceptancePeriod, AcceptancePeriodModel>.NewConfig()
            .Map(x => x.ControlPlanTitle, x => x.QCControlPlan.Title);

            Mapster.TypeAdapterConfig<ControlPlanDefect, ControlPlanDefectModel>.NewConfig()
            .Map(x => x.ControlPlanTitle, x => x.QCControlPlan.Title)
            .Map(x => x.DefectTitle, x => x.QCDefect.Title)
            .Map(x => x.DefectCode, x => x.QCDefect.Code);

            Mapster.TypeAdapterConfig<FinalProductNoncompliance, FinalProductNoncomplianceModel>.NewConfig()
                .Map(x => x.ControlPlanDefectTitle, x => x.ControlPlanDefect.QCDefect.Title)
            .Map(x => x.ControlPlanDefectValue, x => x.ControlPlanDefect.ControlPlanDefectVal)
            .Map(x => x.ControlPlanDefectUserId, x => x.ControlPlanDefect.QCDefect.UserId)
            .Map(x => x.ControlPlanDefectCode, x => x.ControlPlanDefect.QCDefect.Code);

            Mapster.TypeAdapterConfig<FinalProductInspectionDefect, FinalProductInspectionDefectModel>.NewConfig()
               .Map(x => x.ControlPlanTitle, x => x.ControlPlanDefect.QCDefect.Title);


            services.AddScoped<GetAllPersonnelJob>();

        }

        public static void JobRegister(IServiceProvider provider)
        {
            var getEmployessServiceList = provider.GetService<GetAllPersonnelJob>();
            getEmployessServiceList?.Initilize();
        }
    }
}
