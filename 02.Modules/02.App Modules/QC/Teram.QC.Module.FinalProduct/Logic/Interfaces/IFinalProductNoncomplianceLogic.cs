using Microsoft.AspNetCore.Mvc;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Enums;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{
    public interface IFinalProductNoncomplianceLogic : IBusinessOperations<FinalProductNoncomplianceModel, FinalProductNoncompliance, int>
    {
        BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetHasBasisByOrderNoAndProductCodeAndControlPlanDefectId(int orderNo, string productCode, int controlPlanDefectId);
        BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetWithouBasisByOrderNoAndProductCodeAndControlPlanDefectId(int orderNo, string productCode, int controlPlanDefectId);
        BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetAllByOrderNoAndProductCodeAndControlPlanDefectId(int orderNo, string productCode, int controlPlanDefectId);
        BusinessOperationResult<FinalProductNoncomplianceModel> GetById(int finalProductNoncomplianceId);
        BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetByIds(List<int> finalProductNoncomplianceIds);
        BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetByControlPlanDefctId(int controlPlanDefectId);
        BusinessOperationResult<string> GetLastNonComliance();
        BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetByControlPlanDefectIdsAndFinalProductInspectionId(List<int> controlPlanDefectIds, int finalProductInspectionId);
        BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetByFilter(bool isOperator, bool isAdmin, int? orderNo, string? productCode, string finalProductNoncomplianceNumber, FormStatus? formStatus, ReferralStatus? referralStatus, int? start = null, int? length = null);
        NoncomplianceDetailSampleSummaryModel GetSamplesSummaryData(FinalProductNoncomplianceModel relatedNonCompianceResult, List<FinalProductInspectionModel> finalProductInspections);
        FinalProductNoncomplianceModel GetNonComplianceDetails(FinalProductNoncomplianceModel finalProductNoncompliance,List<FinalProductInspectionModel> finalProductInspections);       
    }
}
