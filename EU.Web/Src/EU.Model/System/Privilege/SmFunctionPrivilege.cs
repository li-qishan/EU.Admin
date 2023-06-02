using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;
using EU.Domain.System;

namespace EU.Model.System.Privilege
{
    public class SmFunctionPrivilege : PersistPoco
    {
        [Display(Name = "功能代码")]
        [Column(TypeName = "nvarchar(50)")]
        public string FunctionCode { get; set; }

        [Display(Name = "功能名称")]
        [Column(TypeName = "nvarchar(50)")]
        public string FunctionName { get; set; }

        [Display(Name = "显示位置")]
        [Column(TypeName = "nvarchar(50)")]
        public string DisplayPosition { get; set; }

        public virtual SmModule SmModule { get; set; }
        public Guid? SmModuleId { get; set; }

        [Display(Name = "排序号")]
        public int TaxisNo { get; set; }

        [Display(Name = "颜色")]
        public string Color { get; set; }

        [Display(Name = "图标")]
        public string Icon { get; set; }

        
    }
}
