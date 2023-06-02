using System;
using System.ComponentModel.DataAnnotations;

namespace EU.Domain
{
    /// <summary>
    /// 所有持久化model的基类，所有的不应被物理删除的model都应该继承这个类
    /// </summary>
    public class PersistPoco : BasePoco<Guid>
    {
        [Display(Name = "是否删除")]
        public bool IsDeleted { get; set; }
    }
}
