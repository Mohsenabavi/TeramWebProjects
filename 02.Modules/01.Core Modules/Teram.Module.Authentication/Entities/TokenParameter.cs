using Teram.Framework.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Teram.Module.Authentication.Entities
{
    [Table(nameof(TokenParameter)+"s", Schema = "api")]
    public class TokenParameter : EntityBase
    {
        [Key]
        public int TokenParameterId { get; set; }
        public string Name { get; set; }
       

    }
}
