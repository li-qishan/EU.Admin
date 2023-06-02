using System;
using System.Dynamic;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.BD
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.Base)]
    public class CustomerContactController : BaseController<CustomerContact>
    {
        public CustomerContactController(DataContext _context, IBaseCRUDVM<CustomerContact> BaseCrud) : base(_context, BaseCrud)
        {

        }
    }
}
