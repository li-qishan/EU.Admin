using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.Setup
{
    /// <summary>
    /// 区县
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmCountyController : BaseController1<SmCounty>
    {
        private readonly IConfiguration Configuration;
        public SmCountyController(IConfiguration configuration, DataContext _context, IBaseCRUDVM<SmCounty> BaseCrud) : base(_context, BaseCrud)
        {
            Configuration = configuration;
        }
    }
}
