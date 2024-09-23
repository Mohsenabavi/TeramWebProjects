using System;
using System.Collections.Generic;

namespace Teram.Module.Authentication.Models
{
    public class PermissionModel
    {
        public Guid Key { get; set; }
        public List<ActionInfo> Actions { get; set; }
        public string DisplayName { get; set; }
        public string ControllerKey { get; set; }

    }

    public class ActionInfo
    {
        public Guid ParentControllerKey { get; set; }
        public string ActionName { get; set; }
        public string Displayname { get; set; }
        public string Path { get; set; }
        public bool HasAccess { get; set; }
    }
}

