using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;
using EU.Model.System.Privilege;
using Newtonsoft.Json;

namespace EU.Model.System
{
    public class SmUser : PersistPoco
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        [Display(Name = "用户账号")]
        [Column(TypeName = "nvarchar(50)")]
        public string UserAccount { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [Column(TypeName = "nvarchar(50)")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [Column(TypeName = "nvarchar(50)")]
        public string PassWord { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        [Display(Name = "EmployeeId")]
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// 集团ID
        /// </summary>
        [Display(Name = "集团ID")]
        public Guid? GroupId { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        [Display(Name = "公司ID")]
        public Guid? CompanyId { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        [Display(Name = "用户类型")]
        [Column(TypeName = "nvarchar(50)")]
        public string UserType { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        [Display(Name = "是否激活")]
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public virtual ICollection<SmUserRole> UserRoles { get; set; }
    }
}
