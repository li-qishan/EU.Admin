using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU.Model
{
    /// <summary>
    /// 销售收款核销明细
    /// </summary>
    public class ArSalesCollectionWriteOffExtend : ArSalesCollectionWriteOff
    {
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal? MaxReceivableAmount { get; set; }

    }
}
