using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;

namespace EU.Model.BFProject
{
    public class CompletionFinalAccount : PersistPoco
    {
        public virtual Approval Approval { get; set; }
        public Guid? ApprovalId { get; set; }

        [Display(Name = "实际开工日期")]
        public DateTime? ActualStartDate { get; set; }

        [Display(Name = "实际竣工日期")]
        public DateTime? ActualCompleteDate { get; set; }

        [Display(Name = "决算价")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal FinalAccount { get; set; }

        [Display(Name = "备注")]
        public string FinalRemark { get; set; }

        [Display(Name = "竣工决算录入人")]
        [Column(TypeName = "nvarchar(50)")]
        public string FinalAccountPerson { get; set; }

        [Display(Name = "录入日期")]
        public DateTime? InputDate { get; set; }

        [Display(Name = "审计单位")]
        [Column(TypeName = "nvarchar(50)")]
        public string AuditUnit { get; set; }

        [Display(Name = "送审金额")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal SubmitAuditAmount { get; set; }

        [Display(Name = "审计金额")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal AuditAmount { get; set; }

        [Display(Name = "核减金额")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal CutAmount { get; set; }

        [Display(Name = "核减率")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal CutRate { get; set; }

        [Display(Name = "备注")]
        public string AuditRemark { get; set; }



        [Display(Name = "状态")]
        public string Status { get; set; }

        [Display(Name = "查阅状态")]
        public string ReadStatus { get; set; }

    }
}
