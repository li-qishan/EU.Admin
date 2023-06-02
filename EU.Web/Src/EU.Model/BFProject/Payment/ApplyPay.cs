using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;

namespace EU.Model.BFProject.Payment
{
    public class ApplyPay : PersistPoco
    {
        public virtual BFContract Contract { get; set; }

        public Guid? ContractId { get; set; }

        [Display(Name = "合同总金额")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal TotalContractAmount { get; set; }

        [Display(Name = "已付金额")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal HasPayAmount { get; set; }

        [Display(Name = "未付金额")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal NoPayAmount { get; set; }

        [Display(Name = "项目实施进度")]
        [Column(TypeName = "nvarchar(50)")]
        public string ProjectProgress { get; set; }

        [Display(Name = "支付进度")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal PayProgress { get; set; }

        [Display(Name = "本次申请金额")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal ApplyPayAmount { get; set; }

        [Display(Name = "申请事由")]
        [Column(TypeName = "nvarchar(50)")]
        public string ApplyReason { get; set; }

        [Display(Name = "申请日期")]
        [Column(TypeName = "datetime")]
        public DateTime? ApplyDate { get; set; }

        [Display(Name = "经办人")]
        [Column(TypeName = "nvarchar(50)")]
        public string Agent { get; set; }
    }
}
