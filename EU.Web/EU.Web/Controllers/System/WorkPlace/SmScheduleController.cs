using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Model.System;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.WorkPlace
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmScheduleController : BaseController1<SmSchedule>
    {
        public SmScheduleController(DataContext _context, IBaseCRUDVM<SmSchedule> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
