using System;
using System.ComponentModel.DataAnnotations;

namespace EU.Model.Base
{
    /// <summary>
    /// 所有持久化model的基类，所有的不应被物理删除的model都应该继承这个类
    /// </summary>
    public class PersistPoco : BasePoco<Guid>
    {
        private bool _IsDeleted;

        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        public bool IsDeleted
        {
            get
            {
                _IsDeleted = false;
                return _IsDeleted;
            }
            set
            {
                _IsDeleted = value;
            }
        }
    }
}
