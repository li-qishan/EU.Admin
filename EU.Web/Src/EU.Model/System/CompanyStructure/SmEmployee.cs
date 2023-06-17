/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SmEmployee.cs
*
*功 能： N / A
* 类 名： SmEmployee
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/12/14 17:12:59 SimonHsiao 初版
*
* Copyright(c) 2020 SUZHOU EU Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：苏州一优信息技术有限公司                                │
*└──────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EU.Model
{
    public class SmEmployee : Base.PersistPoco
    {


        /// <summary>
        /// 部门ID
        /// </summary>
        [Display(Name = "DepartmentId")]
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [Display(Name = "ParentEmployeeId")]
        public Guid? ParentEmployeeId { get; set; }

        /// <summary>
        /// 员工代码
        /// </summary>
        [Display(Name = "EmployeeCode")]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        [Display(Name = "EmployeeName")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "Sex")]
        public string Sex { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        [Display(Name = "EName")]
        public string EName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "NickName")]
        public string NickName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>
        [Display(Name = "HireDate")]
        public DateTime? HireDate { get; set; }

        /// <summary>
        /// 离职日期
        /// </summary>
        [Display(Name = "TermDate")]
        public DateTime? TermDate { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "HeadUrl")]
        public string HeadUrl { get; set; }

        /// <summary>
        /// 月销售目标
        /// </summary>
        [Display(Name = "MonthsSalesAmount"), Column(TypeName = "decimal(20,2)")]
        public virtual decimal? MonthsSalesAmount { get; set; }
    }
}