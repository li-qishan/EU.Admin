using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EU.Domain
{
    public class BasePoco<T> : TopBasePoco<Guid> where T : struct
    {
        [Display(Name = "创建人")]
        public T? CreatedBy { get; set; }

        [Display(Name = "创建时间")]
        [Column(TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }

        [Display(Name = "修改人")]
        public T? UpdateBy { get; set; }

        [Display(Name = "修改时间")]
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }

        [Display(Name = "导入编号")]
        public T? ImportDataId { get; set; }

        [Display(Name = "审核状态")]
        [Column(TypeName = "nvarchar(50)")]
        public string AuditStatus { get; set; }

        [Display(Name = "当前流程节点")]
        [Column(TypeName = "nvarchar(50)")]
        public string CurrentNode { get; set; }
    }
}
