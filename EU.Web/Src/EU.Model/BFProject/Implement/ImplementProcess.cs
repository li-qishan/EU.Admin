using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EU.Domain;

namespace EU.Model.BFProject
{
    //实施过程
    public class ImplementProcess : PersistPoco
    {
        public virtual ProjectImplement ProjectImplement { get; set; }
        public Guid? ProjectImplementId { get; set; }

        [Display(Name = "日期")]
        public DateTime Date { get; set; }

        [Display(Name = "实施进度")]
        public string ImplementProgress { get; set; }
    }
}
