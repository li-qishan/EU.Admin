using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Model.System.CompanyStructure;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.CompanyStructure
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmDepartmentController : BaseController<SmDepartment>
    {
        public SmDepartmentController(DataContext _context, IBaseCRUDVM<SmDepartment> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
 