using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;

namespace EU.Model.System
{
    public class SmLovDetail : PersistPoco
    {
        public virtual SmLov SmLov { get; set; }
        public Guid SmLovId { get; set; }

        [Display(Name = "排序号")]
        [Column(TypeName = "int")]
        public int TaxisNo { get; set; }

        [Display(Name = "参数值")]
        [Column(TypeName = "nvarchar(50)")]
        public string Value { get; set; }

        [Display(Name = "参数名称")]
        [Column(TypeName = "nvarchar(50)")]
        public string Text { get; set; }

        [Display(Name = "生效日期")]
        [Column(TypeName = "datetime")]
        public DateTime? InureTime { get; set; }

        [Display(Name = "失效日期")]
        [Column(TypeName = "datetime")]
        public DateTime? AbateTime { get; set; }
    }
}
