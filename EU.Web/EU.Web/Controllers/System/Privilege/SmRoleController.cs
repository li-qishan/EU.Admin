using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Model.System;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.Privilege
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmRoleController : BaseController<SmRole>
    {
        public SmRoleController(DataContext _context, IBaseCRUDVM<SmRole> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
