using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;

namespace EU.Model.BFProject
{
    //付款计划
    public class Payplan : PersistPoco
    {
        public virtual BFContract Contract { get; set; }
        public Guid? ContractId { get; set; }

        [Display(Name = "排序号")]
        [Column(TypeName = "int")]
        public int TaxisNo { get; set; }

        [Display(Name = "付款阶段")]
        [Column(TypeName = "nvarchar(50)")]
        public string PaymentStage { get; set; }

        [Display(Name = "付款比例")]
        [Column(TypeName = "nvarchar(50)")]
        public string PaymentProportion { get; set; }

        [Display(Name = "付款金额")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal PaymentAmount { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }
    }
}
