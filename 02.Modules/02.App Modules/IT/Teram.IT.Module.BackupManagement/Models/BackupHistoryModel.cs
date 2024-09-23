using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.IT.Module.BackupManagement.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.IT.Module.BackupManagement.Models
{
    public class BackupHistoryModel:ModelBase<BackupHistory,int>
    {

        public int BackupHistoryId { get; set; }

        [GridColumn(nameof(PersianBackupDate))]
        public string PersianBackupDate => BackupDate.ToPersianDateTime();
     
        public DateTime BackupDate { get; set; }

        [GridColumn(nameof(IsSuccessText))]
        public string IsSuccessText => IsSuccess ? "موفق" : "ناموفق";

        [GridColumn(nameof(ApplicationTitle))]
        public string ApplicationTitle {  get; set; } = string.Empty;

        [GridColumn(nameof(SourcePath))]
        public string SourcePath {  get; set; }= string.Empty;

        [GridColumn(nameof(DestinationPath))]
        public string DestinationPath {  get; set; } = string.Empty;    
        public bool IsSuccess {  get; set; }
      
        [GridColumn(nameof(Message))]
        public string? Message {  get; set; }

    }
}
