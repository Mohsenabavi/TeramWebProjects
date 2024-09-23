
using Teram.Web.Core.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Teram.Module.Authentication.Models
{
    public class RoleModel
    {
        public Guid Key { get; set; }

        [GridColumn("Name")]
        [RegularExpression(@"^[A-Za-z\d]+$", ErrorMessageResourceType = typeof(Resources.Controllers.RoleControlPanelController), ErrorMessageResourceName = nameof(Resources.Controllers.RoleControlPanelController.Please_inter_english_characters))]
        public string Name { get; set; }

        [GridColumn("Title")]
        [RegularExpression(@"^[\u0600-\u06FF\s]+$", ErrorMessageResourceType = typeof(Resources.Controllers.RoleControlPanelController), ErrorMessageResourceName = nameof(Resources.Controllers.RoleControlPanelController.Please_inter_persian_characters))]
        public string Title { get; set; }

        [GridColumn("Default")]
        public bool IsDefaultRole { get; set; }

        [GridColumn("")]
        public String Test { get; set; }
    }
}
