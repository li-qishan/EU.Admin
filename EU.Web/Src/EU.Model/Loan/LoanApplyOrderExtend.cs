using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EU.Domain;
using EU.Model.System;

namespace EU.Model.Loan
{
    public class LoanApplyOrderExtend : PersistPoco
    {
        [Display(Name = "ID")]
        public Guid? ID { get; set; }

        [Display(Name = "贷款类型ID")]
        public Guid? APPLY_TYPE_ID { get; set; }

        public virtual LoanType APPLY_TYPE_ { get; set; }

        [Display(Name = "申请单号")]
        public string APPLY_NO { get; set; }

        [Display(Name = "员工ID")]
        public Guid? ASSIGN_USER_ID { get; set; }

        public virtual SmUser ASSIGN_USER_ { get; set; }

        [Display(Name = "申请人")]
        [Column(TypeName = "varchar(32)")]
        public string APPLY_MAN { get; set; }

        [Display(Name = "申请人电话")]
        [Column(TypeName = "varchar(32)")]
        public string APPLY_PHONE { get; set; }

        [Display(Name = "申请状态")]
        [Column(TypeName = "varchar(32)")]
        public string APPLY_STATUS { get; set; }

        public string CreatedTimeStr { get; set; }
        public string APPLY_STATUS_TEXT { get; set; }

        
    }
}
