using DocumentFormat.OpenXml.Spreadsheet;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.ServiceContracts;
using UserInfo = Teram.ServiceContracts.UserInfo;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class NotificationService : INotificationService
    {
        public string GenerateEmailNotoification(FinalProductNoncomplianceModel finalProductNoncomplianceModel, UserInfo userInfo)
        {
            string emailContext = string.Empty;
            emailContext+="<div style='text-align:right;direction:rtl;font-family:Calibri Light'>";
            emailContext+="<b>" + "همکار گرامی" + "</b>" + "<br/>";
            emailContext+="<b>" + $"{userInfo.Name} {userInfo.Family}" + "</b>" + "<br/>";
            emailContext+="<b>" + "با سلام و احترام" + "</b>" + "<br/><br/>";
            emailContext+="<b>" + "فرم عدم انطباق محصول نهایی به کارتابل شما وارد شده است " + "</b>" + "<br/>";
            emailContext+="<b>" + $"شماره عدم انطباق : {finalProductNoncomplianceModel.FinalProductNoncomplianceNumber}" + "</b>" + "<br/>";
            emailContext+="<b>" + $"شماره سفارش : {finalProductNoncomplianceModel.OrderNo}" + "</b>" + "<br/>";
            emailContext+="<b>" + $"نام محصول : {finalProductNoncomplianceModel.ProductName}" + "</b>" + "<br/>";
            emailContext+="<b>" + $"لینک مربوطه :  https://B2n.ir/u27048" + "</b>" + "<br/>";
            emailContext+="<br/><b>" + "این ایمیل به طور خودکار برای شما ارسال شده است لطفاً به آن پاسخ ندهید" + "</b>" + "<br/>";
            emailContext+="</div>";
            return emailContext;
        }
        public string GenerateSMSNotoification(FinalProductNoncomplianceModel finalProductNoncomplianceModel, UserInfo userInfo)
        {
            string smsContext = string.Empty;   
            smsContext+= "همکار گرامی" + "\n";
            smsContext+= $"{userInfo.Name} {userInfo.Family}"+ "\n";
            smsContext+= "با سلام و احترام" + "\n";
            smsContext+= "فرم عدم انطباق محصول نهایی به کارتابل شما وارد شده است "  + "\n";
            smsContext+= $"شماره عدم انطباق : {finalProductNoncomplianceModel.FinalProductNoncomplianceNumber}" + "\n";
            smsContext+= $"شماره سفارش : {finalProductNoncomplianceModel.OrderNo}" + "\n";
            smsContext+= $"نام محصول : {finalProductNoncomplianceModel.ProductName}" + "\n";
            smsContext+= $"لینک مربوطه :  https://B2n.ir/u27048" + "\n";            
            return smsContext;
        }
    }
}
