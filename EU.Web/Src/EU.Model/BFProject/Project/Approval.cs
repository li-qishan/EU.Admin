using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EU.Domain;

namespace EU.Model.BFProject
{
    public class Approval : PersistPoco
    {
        [Display(Name = "项目编码")]
        [Column(TypeName = "nvarchar(50)")]
        public string ProjectCode { get; set; }

        [Display(Name = "申报部门")]
        [Column(TypeName = "nvarchar(50)")]
        public string DeclareDepartment { get; set; }

        [Display(Name = "申报时间")]
        [Column(TypeName = "datetime")]
        public DateTime? DeclareTime { get; set; }

        [Display(Name = "项目名称")]
        [Column(TypeName = "nvarchar(50)")]
        public string ProjectName { get; set; }

        [Display(Name = "项目地点")]
        [Column(TypeName = "nvarchar(50)")]
        public string ProjectAddress { get; set; }

        [Display(Name = "工程概算")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal ProjectBudget { get; set; }

        [Display(Name = "资金来源")]
        [Column(TypeName = "nvarchar(50)")]
        public string AmountSource { get; set; }

        [Display(Name = "工程类别")]
        public string EngineeringType { get; set; }

        [Display(Name = "项目类别")]
        public string ProjectType { get; set; }

        [Display(Name = "立项来源")]
        public string ProjectSource { get; set; }

        [Display(Name = "招标方式")]
        public string BiddingType { get; set; }

        [Display(Name = "申报人")]
        [Column(TypeName = "nvarchar(50)")]
        public string DeclarePerson { get; set; }

        [Display(Name = "联系电话")]
        [Column(TypeName = "nvarchar(50)")]
        public string ContractPhone { get; set; }

        [Display(Name = "项目内容")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string ProjectContent { get; set; }

        [Display(Name = "项目立项原因")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string ProjectDeclareReason { get; set; }

        [Display(Name = "申报（部门/社区）负责人")]
        [Column(TypeName = "nvarchar(50)")]
        public string ReasonPerson { get; set; }

        [Display(Name = "处理日期")]
        [Column(TypeName = "datetime")]
        public DateTime? ReasonDate { get; set; }

        [Display(Name = "申报（部门/社区）分管领导意见")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string DeclareLeaderOption { get; set; }

        [Display(Name = "处理人")]
        [Column(TypeName = "nvarchar(50)")]
        public string DeclareLeader { get; set; }

        [Display(Name = "处理日期")]
        [Column(TypeName = "datetime")]
        public DateTime? DeclareLeaderDate { get; set; }

        [Display(Name = "工程部门意见")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string EngineeringDepartmentOption { get; set; }

        [Display(Name = "处理人")]
        [Column(TypeName = "nvarchar(50)")]
        public string EngineeringDepartmentPerson { get; set; }

        [Display(Name = "处理日期")]
        [Column(TypeName = "datetime")]
        public DateTime? EngineeringDepartmentDate { get; set; }

        [Display(Name = "工程部门意见")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string EngineeringDepartmentOption1 { get; set; }

        [Display(Name = "处理人")]
        [Column(TypeName = "nvarchar(50)")]
        public string EngineeringDepartmentPerson1 { get; set; }

        [Display(Name = "处理日期")]
        [Column(TypeName = "datetime")]
        public DateTime? EngineeringDepartmentDate1 { get; set; }

        [Display(Name = "工程部门分管领导意见")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string EngineeringDepartmentLeaderOption { get; set; }

        [Display(Name = "处理人")]
        [Column(TypeName = "nvarchar(50)")]
        public string EngineeringDepartmentLeaderPerson { get; set; }

        [Display(Name = "处理日期")]
        [Column(TypeName = "datetime")]
        public DateTime? EngineeringDepartmentLeaderDate { get; set; }

        [Display(Name = "主要领导意见")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string MainLeaderOption { get; set; }

        [Display(Name = "处理人")]
        [Column(TypeName = "nvarchar(50)")]
        public string MainLeaderPerson { get; set; }

        [Display(Name = "处理日期")]
        [Column(TypeName = "datetime")]
        public DateTime? MainLeaderDate { get; set; }



        [Display(Name = "状态")]
        public string Status { get; set; }

        [Display(Name = "查阅状态")]
        public string ReadStatus { get; set; }

        [Display(Name = "是否三万以上")]
        public bool IsOverThree { get; set; }
    }
}
