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
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PS)]
    public class ProcessTemplateMaterialController : BaseController1<ProcessTemplateMaterial>
    {

        public ProcessTemplateMaterialController(DataContext _context, IBaseCRUDVM<ProcessTemplateMaterial> BaseCrud) : base(_context, BaseCrud)
        {
        }
        
    }
}
