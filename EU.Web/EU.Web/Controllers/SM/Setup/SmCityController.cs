using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.Setup
{
    /// <summary>
    /// 城市
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmCityController : BaseController1<SmCity>
    {
        private readonly IConfiguration Configuration;
        public SmCityController(IConfiguration configuration, DataContext _context, IBaseCRUDVM<SmCity> BaseCrud) : base(_context, BaseCrud)
        {
            Configuration = configuration;
        }
    }
}
