using EU.Core.Const;
using EU.Core.Entry;
using EU.Core.WeiXin;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.Weixin
{
    /// <summary>
    /// 微信菜单
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class WxMenuController : BaseController1<WxMenu>
    {
        public WxConfigContainer wxConfigContainer = new WxConfigContainer();

        /// <summary>
        /// 微信菜单
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public WxMenuController(DataContext _context, IBaseCRUDVM<WxMenu> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 创建菜单
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="configId">微信配置ID</param>
        /// <returns></returns>
        [HttpGet, Route("{configId}")]
        public async Task<ServiceResult<WxJsonResult>> CreateMenu(Guid configId)
        {
            dynamic obj = new ExpandoObject();
            string message = string.Empty;

            WxConfig wxConfig = await _context.WxConfig.FirstOrDefaultAsync(o => o.ID == configId);
            string originId = wxConfig.OriginId;
            string appId = wxConfig.AppId;
            string menuType, menuName, menuCode, menuTypeValue, rowId = string.Empty;
            MenuFull_ButtonGroup buttonGroup = new MenuFull_ButtonGroup();
            buttonGroup.button = new List<MenuFull_RootButton>();

            var menus = await _context.WxMenu.Where(o => o.IsActive == true && o.IsDeleted == false).OrderBy(o => o.TaxisNo).ToListAsync();

            menus?.ForEach(o =>
            {
                menuType = o.MenuType;
                menuName = o.MenuName;
                //menuCode = dtMenu.Rows[i]["MENU_CODE"].ToString();
                menuTypeValue = o.MenuTypeValue;

                //包含二级菜单
                if (menuType == "Container")
                {

                }
                else
                {
                    MenuFull_RootButton button1 = new MenuFull_RootButton();
                    button1.key = menuTypeValue;
                    button1.name = menuName;
                    button1.type = menuType;
                    if (o.isAuth != null)
                        if (o.isAuth.Value)
                        {
                            if (menuTypeValue.Contains("?") == true)
                                menuTypeValue = menuTypeValue + "&origin_id=" + originId + "&app_id=" + appId;
                            else
                                menuTypeValue = menuTypeValue + "?origin_id=" + originId + "&app_id=" + appId;
                            menuTypeValue = OAuthApi.GetAuthorizeUrl(appId, menuTypeValue, "SimonHsiao", OAuthScope.snsapi_userinfo);
                        }
                    button1.url = menuTypeValue;
                    buttonGroup.button.Add(button1);
                }
            });

            var resultFull = new GetMenuResultFull()
            {
                menu = buttonGroup
            };

            IButtonGroupBase buttonGroupBase = null;
            buttonGroupBase = CommonApi.GetMenuFromJsonResult(resultFull, new ButtonGroup()).menu;
            var token = wxConfigContainer.GetToken(wxConfig.WeixinId);
            WxJsonResult menuResult = CommonApi.CreateMenu(token, buttonGroupBase);

            return ServiceResult<WxJsonResult>.OprateSuccess(menuResult, "发布成功！");
        }
        #endregion
    }
}
