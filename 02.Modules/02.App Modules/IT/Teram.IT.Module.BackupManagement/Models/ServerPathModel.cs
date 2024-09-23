using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.IT.Module.BackupManagement.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.IT.Module.BackupManagement.Models
{
    public class ServerPathModel:ModelBase<ServerPath,int>
    {
        public int ServerPathId { get; set; }

        [GridColumn(nameof(ApplicationTitle))]
        public string ApplicationTitle { get; set; }
       
        public int ApplicationId {  get; set; }

        public string SourcePath {  get; set; }

        public string DestinationPath {  get; set; }
       
        [GridColumn(nameof(Description))]
        public string Description {  get; set; }

        [GridColumn(nameof(FileName))]
        public string? FileName {  get; set; }
    }
}
