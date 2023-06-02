using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Model.System;
using EU.Model.System.Privilege;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.Privilege
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmFunctionPrivilegeController : BaseController<SmFunctionPrivilege>
    {
        public SmFunctionPrivilegeController(DataContext _context, IBaseCRUDVM<SmFunctionPrivilege> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
