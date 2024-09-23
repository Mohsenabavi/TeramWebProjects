using DocumentFormat.OpenXml.Spreadsheet;
using Teram.QC.Module.FinalProduct.Models;
using Teram.ServiceContracts;
using UserInfo = Teram.ServiceContracts.UserInfo;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{
    public interface INotificationService
    {
        public string GenerateEmailNotoification(FinalProductNoncomplianceModel finalProductNoncomplianceModel,UserInfo userInfo);
        public string GenerateSMSNotoification(FinalProductNoncomplianceModel finalProductNoncomplianceModel, UserInfo userInfo);
    }
}
