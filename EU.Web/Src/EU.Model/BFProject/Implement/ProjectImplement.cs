using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;

namespace EU.Model.BFProject
{
    //项目实施
    public class ProjectImplement : PersistPoco
    {

        public virtual BFContract BfContract { get; set; }

        public Guid? BfContractId { get; set; }

        [Display(Name = "实施进度")]
        [Column(TypeName = "nvarchar(50)")]
        public string ImplementProgress { get; set; }

        [Display(Name = "是否竣工")]
        public bool IsComplated { get; set; }

        [Display(Name = "监理单位")]
        [Column(TypeName = "nvarchar(50)")]
        public string ControlUnit { get; set; }

        [Display(Name = "录入人")]
        [Column(TypeName = "nvarchar(50)")]
        public string InputUser { get; set; }

        [Display(Name = "录入日期")]
        public DateTime InputDate { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }
    }
}
