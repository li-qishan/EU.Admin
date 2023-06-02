using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;
using EU.Domain.System;
using EU.Model.System;

namespace EU.Domain.System
{
    public class SmModuleColumn : PersistPoco
    {
        public virtual SmModule SmModule { get; set; }

        public Guid? SmModuleId { get; set; }

        public int TaxisNo { get; set; }

        public string title { get; set; }

        public string dataIndex { get; set; }

        public string valueType { get; set; }

        [Column(TypeName = "decimal(18, 6)")]
        public decimal? width { get; set; }

        public bool hideInTable { get; set; }

        public bool hideInSearch { get; set; }

        public bool sorter { get; set; }

        public bool filters { get; set; }

        public bool filterMultiple { get; set; }

        public bool IsExport { get; set; }

        public bool IsLovCode { get; set; }

        public bool IsBool { get; set; }

        public string QueryValue { get; set; }

        public string QueryValueType { get; set; }
        public string DataFormate { get; set; }

        /// <summary>
        /// 对齐方式
        /// </summary>
        public string align { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        [NotMapped]
        public string ModuleCode { get; set; }

        /// <summary>
        /// 表別名
        /// </summary>
        public string TableAlias { get; set; }

        /// <summary>
        /// 是否合计
        /// </summary>
        [Display(Name = "IsSum")]
        public bool IsSum { get; set; }
    }
}
