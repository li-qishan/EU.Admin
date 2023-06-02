using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EU.Domain;

namespace EU.Model.BFProject
{
    //合同管理
    public class BFContract : PersistPoco
    {
        [Display(Name = "合同编码")]
        [Column(TypeName = "nvarchar(50)")]
        public string ContractCode { get; set; }

        [Display(Name = "合同名称")]
        [Column(TypeName = "nvarchar(50)")]
        public string ContractName { get; set; }

        [ForeignKey("ApprovalId")]
        public virtual Approval Approval { get; set; }
        [Display(Name = "项目")]
        public Guid? ApprovalId { get; set; }

        [Display(Name = "工程立项批准文号")]
        [Column(TypeName = "nvarchar(50)")]
        public string ApprovalNo { get; set; }

        [Display(Name = "中标金额")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal ContractAmount { get; set; }//合同价

        [Display(Name = "签订日期")]
        public DateTime? SignDate { get; set; }//签订日期

        [Display(Name = "计划开工日期")]
        public DateTime? StartDate { get; set; }//开工日期

        [Display(Name = "计划竣工日期")]
        public DateTime? CompleteDate { get; set; }//竣工日期

        [Display(Name = "承包人")]
        [Column(TypeName = "nvarchar(50)")]
        public string Contractor { get; set; }//甲方负责人

        [Display(Name = "承包负责人")]
        [Column(TypeName = "nvarchar(50)")]
        public string ContractorCharge { get; set; }

        [Display(Name = "发包人")]
        [Column(TypeName = "nvarchar(50)")]
        public string Employer { get; set; }

        [Display(Name = "发包联系方式")]
        [Column(TypeName = "nvarchar(50)")]
        public string EmployerPhone { get; set; }//甲方联系方式

        [Display(Name = "项目负责人")]
        [Column(TypeName = "nvarchar(50)")]
        public string ProjectCharge { get; set; }//乙方项目经理

        [Display(Name = "项目负责人身份证")]
        [Column(TypeName = "nvarchar(50)")]
        public string ProjectChargeCard { get; set; }

        [Display(Name = "项目负责人电话")]
        [Column(TypeName = "nvarchar(50)")]
        public string ProjectChargePhone { get; set; }//乙方联系电话

        [Column(TypeName = "nvarchar(50)")]
        public string Operator { get; set; }//甲方经办人

        [Column(TypeName = "nvarchar(50)")]
        public string OperatorPhone { get; set; }//甲方经办人联系电话

        [Display(Name = "备注")]
        public string Remark { get; set; }


        [Display(Name = "状态")]
        public string Status { get; set; }

        [Display(Name = "查阅状态")]
        public string ReadStatus { get; set; }

        [Display(Name = "是否三万以上")]
        public bool IsOverThree { get; set; }
    }
}
