using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;
using Newtonsoft.Json;

namespace EU.Model.System
{
    public class SmLov : PersistPoco
    {
        [Display(Name = "参数代码")]
        [Column(TypeName = "nvarchar(50)")]
        public string LovCode { get; set; }

        [Display(Name = "参数名称")]
        [Column(TypeName = "nvarchar(50)")]
        public string LovName { get; set; }

        [Display(Name = "生效日期")]
        [Column(TypeName = "datetime")]
        public DateTime? InureTime { get; set; }

        [Display(Name = "失效日期")]
        [Column(TypeName = "datetime")]
        public DateTime? AbateTime { get; set; }

        [JsonIgnore]
        public virtual ICollection<SmLovDetail> SmLovDetail { get; set; }
    }
}
