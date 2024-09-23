using Teram.Framework.Core.Logic;
using Teram.IT.Module.BackupManagement.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.IT.Module.BackupManagement.Models
{
    public class ApplicationModel : ModelBase<Application, int>
    {
        public int ApplicationId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title { get; set; }
        public bool IsActive { get; set; }


        [GridColumn(nameof(IsActiveText))]

        public string IsActiveText => IsActive ? "فعال" : "غیر فعال";


        [GridColumn(nameof(DestinationPath))]
        public string DestinationPath { get;set; }

        [GridColumn(nameof(SourcePath))]
        public string SourcePath {  get; set; }


        public List<ServerPathModel> ServerPaths {  get; set; }
    }
}
