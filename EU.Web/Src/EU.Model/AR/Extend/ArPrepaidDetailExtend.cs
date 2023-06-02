using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU.Model
{
    /// <summary>
    /// 应收预付单明细
    /// </summary>
    public class ArPrepaidDetailExtend : ArPrepaidDetail
    {
        /// <summary>
        /// 预付金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 预付比例
        /// </summary>
        public decimal? Percent { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal? MaxCollectionAmount { get; set; }

        /// <summary>
        /// 含税金额
        /// </summary>
        public decimal? TaxIncludedAmount { get; set; }

    }
}
