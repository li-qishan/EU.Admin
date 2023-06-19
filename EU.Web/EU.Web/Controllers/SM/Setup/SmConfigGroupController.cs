using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using EU.Model.System;
using EU.Model.System.Setup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.Setup
{
    /// <summary>
    /// 系统参数配置
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmConfigGroupController : BaseController1<SmConfigGroup>
    {
        private readonly IConfiguration Configuration;
        public SmConfigGroupController(IConfiguration configuration, DataContext _context, IBaseCRUDVM<SmConfigGroup> BaseCrud) : base(_context, BaseCrud)
        {
            Configuration = configuration;
        }

    }
}
