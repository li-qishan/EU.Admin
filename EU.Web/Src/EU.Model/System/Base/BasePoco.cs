using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EU.Model.Base
{
    public class BasePoco<T> : TopBasePoco<Guid> where T : struct
    {
        /// <summary>
        /// 创建人
        /// </summary>
        [Display(Name = "创建人")]
        public T? CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Column(TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        [Display(Name = "最后修改人")]
        public T? UpdateBy { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Display(Name = "最后修改时间")]
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 导入模板ID
        /// </summary>
        [Display(Name = "导入模板ID")]
        public T? ImportDataId { get; set; }

        /// <summary>
        /// 修改次数
        /// </summary>
        [Display(Name = "修改次数")]
        public int? ModificationNum { get; set; }

        /// <summary>
        /// 修改标志
        /// </summary>
        [Display(Name = "修改标志")]
        public int? Tag { get; set; }

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
        /// 审核状态
        /// </summary>
        [Display(Name = "审核状态")]
        [Column(TypeName = "varchar(32)")]
        public string AuditStatus { get; set; }

        /// <summary>
        /// 当前流程节点
        /// </summary>
        [Display(Name = "当前流程节点")]
        [Column(TypeName = "nvarchar(50)")]
        public string CurrentNode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 'true':有效,'false':未生效
        /// </summary>
        [Display(Name = "'true':有效,'false':未生效")]
        public bool IsActive { get; set; }

    }
}
