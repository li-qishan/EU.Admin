using EU.Core.WeiXin;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.Weixin
{
    /// <summary>
    /// 微信菜单明细
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class WxMenuDetailController : BaseController1<WxMenuDetail>
    {

        /// <summary>
        /// 微信菜单明细
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public WxMenuDetailController(DataContext _context, IBaseCRUDVM<WxMenuDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
