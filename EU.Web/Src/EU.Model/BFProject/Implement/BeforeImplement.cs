using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EU.Domain;
using EU.Model.System;

namespace EU.Model.BFProject
{
    //实施前
    public class BeforeImplement : PersistPoco
    {
        public virtual ProjectImplement ProjectImplement { get; set; }
        public Guid? ProjectImplementId { get; set; }

        [Display(Name = "日期")]
        public DateTime Date { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }
    }
}
