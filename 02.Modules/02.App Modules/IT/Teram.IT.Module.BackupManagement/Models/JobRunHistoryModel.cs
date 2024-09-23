using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.IT.Module.BackupManagement.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.IT.Module.BackupManagement.Models
{
    public class JobRunHistoryModel : ModelBase<JobRunHistory, int>
    {
        public int JobRunHistoryId { get; set; }

        [GridColumn(nameof(Title))]

        public string Title { get; set; }

        public DateTime RunDate { get; set; }

        public DateTime RunFinishDate {  get; set; }

        public bool IsSucess { get; set; }

        [GridColumn(nameof(Message))]
        public string? Message { get; set; }

        [GridColumn(nameof(PersianRunDate))]
        public string PersianRunDate => RunDate.ToPersianDateTime();

        [GridColumn(nameof(PersianRunFinishDate))]
        public string PersianRunFinishDate => RunFinishDate.ToPersianDateTime();


        [GridColumn(nameof(IsSucessText))]
        public string IsSucessText => IsSucess ? "موفق" : "نام موفق";
    }
}
