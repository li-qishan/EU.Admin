using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EU.Domain;

namespace EU.Model.Loan
{
    public class LoanProduct : PersistPoco
    {
        [Display(Name = "产品ID")]
        public Guid? ID { get; set; }

        [Display(Name = "产品名称")]
        [Column(TypeName = "varchar(32)")]
        public string ProducName { get; set; }

        [Display(Name = "产品描述")]
        public string ProducDesc { get; set; }

        [Display(Name = "图片URL")]
        [Column(TypeName = "varchar(32)")]
        public string ImageUrl { get; set; }

        [Display(Name = "贷款类型ID")]
        public Guid? LOAN_TYPE_ID { get; set; }

        public virtual LoanType LOAN_TYPE_ { get; set; }
    }
}
