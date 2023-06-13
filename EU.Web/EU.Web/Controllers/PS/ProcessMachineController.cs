using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using EU.Common;
using EU.Core;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.PS
{
    /// <summary>
    /// 工序机台
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PS)]
    public class ProcessMachineController : BaseController1<ProcessMachine>
    {
        /// <summary>
        /// 工序机台
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public ProcessMachineController(DataContext _context, IBaseCRUDVM<ProcessMachine> BaseCrud) : base(_context, BaseCrud)
        {

        }

    }
}
