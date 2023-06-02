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
    /// 生产工单-工模治具
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PD)]
    public class PdOrderMouldController : BaseController<PdOrderMould>
    {
        /// <summary>
        /// 生产工单-工模治具
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public PdOrderMouldController(DataContext _context, IBaseCRUDVM<PdOrderMould> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
