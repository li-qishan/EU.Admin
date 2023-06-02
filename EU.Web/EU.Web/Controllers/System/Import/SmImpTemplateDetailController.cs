using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using EU.Core;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static EU.Core.Const.Consts;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.Import
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmImpTemplateDetailController : BaseController<SmImpTemplateDetail>
    {

        public SmImpTemplateDetailController(DataContext _context, IBaseCRUDVM<SmImpTemplateDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
