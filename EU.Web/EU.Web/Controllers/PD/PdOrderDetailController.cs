using System;
using System.Collections.Generic;
using System.Dynamic;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.PD
{
    /// <summary>
    /// 生产工单-对应订单
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PD)]
    public class PdOrderDetailController : BaseController<PdOrderDetail>
    {
        /// <summary>
        /// 生产工单-对应订单
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public PdOrderDetailController(DataContext _context, IBaseCRUDVM<PdOrderDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
