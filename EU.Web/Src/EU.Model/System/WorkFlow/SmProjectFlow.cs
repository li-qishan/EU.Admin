using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;
using EU.Domain.System;

namespace EU.Model.System.WorkFlow
{
    //项目流程
    public class SmProjectFlow : PersistPoco
    {
        public string StructureId { get; set; }

        [Display(Name = "流程编号")]
        [Column(TypeName = "nvarchar(50)")]
        public string FlowCode { get; set; }

        [Display(Name = "流程名称")]
        [Column(TypeName = "nvarchar(50)")]
        public string FlowName { get; set; }

        [Display(Name = "排序号")]
        public int TaxisNo { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        public virtual SmModule SmModule { get; set; }

        [Display(Name = "模块")]
        public Guid? SmModuleId { get; set; }

        [Display(Name = "是否启用")]
        public bool IsActive { get; set; }
    }
}
