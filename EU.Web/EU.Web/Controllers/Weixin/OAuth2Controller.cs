using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EU.Core;
using EU.Core.Utilities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin;
using EU.Core.Services;
using Senparc.CO2NET.Extensions;
using System.Data;
using EU.Domain.System;
using EU.Model.System;
using static EU.Core.Const.Consts;
using EU.Core.Entry;
using EU.Core.Const;
using EU.Core.WeiXin;
using EU.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.Weixin
{
    /// <summary>
    /// OAuth2.0
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    [Route("api/Weixin/[controller]/[action]")]
    public class OAuth2Controller : BaseController<SmModule>
    {
        public WxConfigContainer wxConfigContainer = new WxConfigContainer();
        /// <summary>
        /// OAuth2.0
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public OAuth2Controller(DataContext _context, IBaseCRUDVM<SmModule> BaseCrud) : base(_context, BaseCrud)
        {
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, HttpPost]
        public async Task<ServiceResult<dynamic>> GetUserInfo(string code, string weixinId)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            string openid = string.Empty;

            try
            {
                Logger.WriteLog("GetAuthorizeUrl", "code:" + code);
                Logger.WriteLog("GetAuthorizeUrl", "weixinId:" + weixinId);

                WxConfig wxConfig = wxConfigContainer.GetConfig(weixinId);

                if (wxConfig is null)
                    return ServiceResult<dynamic>.OprateFailed("无效的weixinId！");
                string appId = wxConfig.AppId;
                string appSecret = wxConfig.AppSecret;

                Logger.WriteLog("GetAuthorizeUrl", "appId：" + appId);
                Logger.WriteLog("GetAuthorizeUrl", "appSecret：" + appSecret);

                var result = OAuthApi.GetAccessToken(appId, appSecret, code);
                Logger.WriteLog("result：" + result.ToJson());
                openid = result.openid;

                //obj.wxresult = result;
                if (result.errcode != 0)
                {
                    //return Content("错误：" + result.errmsg);
                }

                Logger.WriteLog("GetAuthorizeUrl", "result.access_token：" + result.access_token);
                Logger.WriteLog("GetAuthorizeUrl", "result.openid：" + openid);
                //因为这里还不确定用户是否关注本微信，所以只能试探性地获取一下
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);

                //try
                //{
                //    //userInfo = OAuthApi.GetUserInfo(result.access_token, openid);
                //    userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                //    string aaa = $"您的OpenID为：{userInfo.openid}\r\n昵称：{userInfo.nickname}\r\n性别：{userInfo.sex}\r\n地区（国家/省/市）：{userInfo.country}/{userInfo.province}/{userInfo.city}\r\n关注时间：{userInfo.subscribe_time}\r\n关注状态：{userInfo.subscribe}\r\n\r\n说明：从2021年12月27日起，公众号无法直接获取用户昵称、性别、地区等信息，如需获取相关信息，需要使用<a href=\"https://sdk.weixin.senparc.com/oauth2?returnUrl=%2FOAuth2%2FTestReturnUrl\">OAuth 2.0 接口</a>。";
                //    Logger.WriteLog("GetAuthorizeUrl", "userInfo：" + aaa);
                //}
                //catch (Exception E)
                //{
                //    Logger.WriteLog("GetAuthorizeUrl", "Exception：" + E);
                //}

                //obj.userInfo = userInfo;
                //Task.Factory.StartNew(() => RecordWxUserInfo(userInfo, wxConfig.OriginId));

                #region 处理用户数据
                var user = _context.SmUsers.FirstOrDefault(o => o.UserAccount == openid);
                obj.openid = openid;

                if (user == null)
                {
                    user = new SmUser()
                    {
                        ID = Guid.NewGuid(),
                        CreatedBy = UserId,
                        CreatedTime = Utility.GetSysDate(),
                        UserAccount = openid,
                        GroupId = GroupId,
                        CompanyId = CompanyId,
                        IsDeleted = true,
                        UserType = "WxUser"
                    };
                    await _context.SmUsers.AddAsync(user);
                    await _context.SaveChangesAsync();
                    #endregion

                    string sql = "SELECT * FROM WxUser WHERE OpenId='{0}'";
                    sql = string.Format(sql, userInfo.openid);
                    DataTable dt = DBHelper.Instance.GetDataTable(sql, null);
                    if (dt.Rows.Count == 0)
                    {
                        DbInsert di = new DbInsert("WxUser");
                        di.Values("OriginId", wxConfig.OriginId);
                        di.Values("OpenId", userInfo.openid);
                        di.Values("UserId", user.ID);
                        di.Values("NickName", userInfo.nickname);
                        di.Values("HeadImgUrl", userInfo.headimgurl);
                        DBHelper.Instance.ExecuteScalar(di.GetSql(), null);
                    }
                }
                obj.userId = user.ID;
                return ServiceResult<dynamic>.OprateSuccess(obj, ResponseText.QUERY_SUCCESS);
            }
            catch (Exception E)
            {
                Logger.WriteLog("GetAuthorizeUrl", "StackTrace：" + E);
                message = E.Message;
                return ServiceResult<dynamic>.OprateFailed(message);
            }
        }

        private void RecordWxUserInfo(OAuthUserInfo userInfo, string originId)
        {
            Logger.WriteLog("GetAuthorizeUrl", "RecordWxUserInfo：" + userInfo.openid);
            string sql = "SELECT * FROM WxUser WHERE OpenId='{0}'";
            sql = string.Format(sql, userInfo.openid);
            DataTable dt = DBHelper.Instance.GetDataTable(sql, null);
            if (dt.Rows.Count == 0)
            {
                DbInsert di = new DbInsert("WxUser");
                di.Values("OriginId", originId);
                di.Values("OpenId", userInfo.openid);
                di.Values("NickName", userInfo.nickname);
                di.Values("HeadImgUrl", userInfo.headimgurl);
                DBHelper.Instance.ExecuteScalar(di.GetSql(), null);
            }
        }

        /// <summary>
        /// 获取认证URL
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, HttpPost]
        public ServiceResult<dynamic> GetAuthorizeUrl(string appId, string url, string weixinType)
        {
            string message = string.Empty;

            try
            {
                dynamic obj = new ExpandoObject();

                #region 判断参数是否为空
                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId不能为空！");
                if (string.IsNullOrEmpty(url))
                    throw new Exception("url不能为空！");
                #endregion

                Logger.WriteLog("GetAuthorizeUrl", "appId:" + appId);
                Logger.WriteLog("GetAuthorizeUrl", "url:" + url);
                Logger.WriteLog("GetAuthorizeUrl", "weixinType:" + weixinType);

                //
                //var returnUrl = url;
                //if (returnUrl.IndexOf("?") > -1)
                //{
                //    returnUrl = returnUrl + "&appid=" + appId;
                //}
                //else
                //{
                //    returnUrl = returnUrl + "?appid=" + appId;
                //}
                //url = OAuthApi.GetAuthorizeUrl(appId, returnUrl, state, OAuthScope.snsapi_userinfo);

                var returnUrl = url;
                if (returnUrl.IndexOf("?") > -1)
                {
                    returnUrl = returnUrl + "&appid=" + appId;
                }
                else
                {
                    returnUrl = returnUrl + "?appid=" + appId;
                }
                var state = "eu-" + SystemTime.Now.Millisecond;//随机数，用于识别请求可靠性
                OAuthScope scope = OAuthScope.snsapi_userinfo;
                //if (weixinType == "mp")
                //    scope = OAuthScope.snsapi_base;
                url = OAuthApi.GetAuthorizeUrl(appId, returnUrl, state, scope);

                Logger.WriteLog("GetAuthorizeUrl", "url:" + url);
                obj.url = url;
                return ServiceResult<dynamic>.OprateSuccess(obj, ResponseText.QUERY_SUCCESS);
            }
            catch (Exception E)
            {
                message = E.Message;
                Logger.WriteLog("GetAuthorizeUrl", "Exception:" + message);
                return ServiceResult<dynamic>.OprateFailed(message);
            }


        }

        /// <summary>
        /// 获取微信用户ID
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, HttpPost]
        public IActionResult GetWxUserId()
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            string openid = string.Empty;
            string userId = string.Empty;
            try
            {
                openid = HttpContext.Request.Form["openid"].ToString();
                string sql = "SELECT [USER_ID] FROM WX_USER WHERE OPEN_ID='{0}'";
                sql = string.Format(sql, openid);
                DataTable dt = DBHelper.Instance.GetDataTable(sql, null);
                if (dt.Rows.Count > 0)
                {
                    userId = dt.Rows[0]["USER_ID"].ToString();
                    if (string.IsNullOrEmpty(userId))
                        userId = string.Empty;
                }
                status = "ok";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.userId = userId;
            obj.message = message;
            return Ok(obj);
        }
    }
}
