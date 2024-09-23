
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Module.Authentication.Entities;
using Teram.Web.Core.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Teram.Module.Authentication.Models
{
    public class TokenModel : ModelBase<Token, Guid>
    {
        [GridColumn("Token ID")]
        public Guid TokenId { get; set; }
        public string Content { get; set; }
        [GridColumn("Issued By")]
        public string Issuer { get; set; }


        [GridColumn("User")]
        public string UserName { get; set; }

        public Guid IssuerId { get; set; }
        public DateTime IssuedOn { get; set; }
        [GridColumn("Issued On")]
        public string IssuedOnPersian { get { return IssuedOn.ToPersianDate(); } }
        [GridColumn("Issued For")]
        public string IssuedFor { get; set; } 

        //[RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.TokenModel.Please_enter_valid_Date))]
        public DateTime? ExpireDate { get; set; }

        [GridColumn("Expire Date")]
        public string ExpireDatePersian
        {
            get
            {
                if (ExpireDate.Equals(DateTime.MinValue))
                {
                    return string.Empty;
                }

                return !ExpireDate.HasValue ? string.Empty : ExpireDate.Value.ToPersianDate();
            }
        }

        [GridColumn("Is Active")]
        public bool IsActive { get; set; }

        public Guid UserId { get; set; }


        [GridColumn("Policy")]
        public string Policy { get; set; }

        public string Description { get; set; }


    }
}
