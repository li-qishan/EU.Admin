using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using EU.Domain;
using EU.Model.System;
using Newtonsoft.Json;

namespace EU.Model.System
{
    public class SmModule : PersistPoco
    {
        [Display(Name = "模块代码")]
        [Column(TypeName = "nvarchar(50)")]
        public string ModuleCode { get; set; }

        [Display(Name = "模块名称")]
        [Column(TypeName = "nvarchar(50)")]
        public string ModuleName { get; set; }

        [Display(Name = "排序号")]
        public int TaxisNo { get; set; }

        [Display(Name = "图标")]
        public string Icon { get; set; }

        [Display(Name = "路由")]
        [Column(TypeName = "nvarchar(50)")]
        public string RoutePath { get; set; }


        [ForeignKey("ParentId")]
        public virtual SmModule Parent { get; set; }
        [Display(Name = "上级模块")]
        public Guid? ParentId { get; set; }

        //[ForeignKey("BelongModuleId")]
        //public virtual SmModule BelongModule { get; set; }
        [Display(Name = "所属模块")]
        public Guid? BelongModuleId { get; set; }

        [Display(Name = "是否目录")]
        public bool IsParent { get; set; }

        [Display(Name = "是否显示")]
        public bool IsActive { get; set; }

        [Display(Name = "是否从表")]
        public bool IsDetail { get; set; }

        #region 操作按钮
        public bool IsShowAdd { get; set; }
        public bool IsShowUpdate { get; set; }
        public bool IsShowView { get; set; }
        public bool IsShowDelete { get; set; }
        public bool IsShowSubmit { get; set; }
        public bool IsShowBatchDelete { get; set; }
        #endregion

        [Display(Name = "是否执行查询")]
        public bool IsExecQuery { get; set; }

        public string DefaultSort { get; set; }
        public string DefaultSortOrder { get; set; }
        
        public bool IsShowAudit { get; set; }

        /// <summary>
        /// 是否合计
        /// </summary>
        [Display(Name = "IsSum")]
        public bool IsSum { get; set; }

    }
}
