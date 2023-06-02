using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;
using EU.Domain.System;
using Newtonsoft.Json;

namespace EU.Model.System
{
    public class SmRole : PersistPoco
    {
        [Display(Name = "角色编号")]
        [Column(TypeName = "nvarchar(50)")]
        public string RoleCode { get; set; }

        [Display(Name = "角色名称")]
        [Column(TypeName = "nvarchar(50)")]
        public string RoleName { get; set; }

        [Display(Name = "是否启用")]
        public bool IsActive { get; set; }

        [JsonIgnore]
        public virtual ICollection<SmModule> RoleModules { get; set; }
    }
}
