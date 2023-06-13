using EU.Core.WeiXin;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.Weixin
{
    /// <summary>
    /// 微信配置
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class WxConfigController : BaseController1<WxConfig>
    {
        public WxConfigContainer wxConfigContainer = new WxConfigContainer();

        /// <summary>
        /// 微信菜单
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public WxConfigController(DataContext _context, IBaseCRUDVM<WxConfig> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
