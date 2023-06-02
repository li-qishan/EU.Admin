using EU.Core;
using EU.Core.Extensions;
using EU.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EU.Web
{
    /// <summary>
    /// 全局请求验证
    /// </summary>
    public class GlobalActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //检查实体合法性
            //if (!filterContext.ModelState.IsValid)
            //{
            //    StringBuilder msg = new StringBuilder();
            //    foreach (var value in filterContext.ModelState.Values)
            //    {
            //        if (value.Errors.Count > 0)
            //        {
            //            foreach (var error in value.Errors)
            //            {
            //                msg.AppendLine(error.ErrorMessage);
            //            }
            //        }
            //    }
            //    dynamic obj = new ExpandoObject();
            //    obj.status = "error"; ;
            //    obj.message = $"参数验证失败:{msg}";
            //    filterContext.Result = new JsonResult(obj);
            //    return;
            //}

            //检查用户信息
            var userId = filterContext.HttpContext.User?.Claims?.Where(o => o.Type == "UserId").FirstOrDefault()?.Value;
            var userName = filterContext.HttpContext.User?.Claims?.Where(o => o.Type == "UserName").FirstOrDefault()?.Value;

            //记录ip
            //var ip = filterContext.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            //if (string.IsNullOrEmpty(ip))
            //    ip = filterContext.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();

            var ip = HttpContextExtension.GetUserIp(EU.Core.Utilities.HttpContext.Current);

            string Path = filterContext.HttpContext.Request.Path;
            string Method = filterContext.HttpContext.Request.Method;

            if (Path != "/api/Account/GetAccessToken")
            {
                //记录日志(GET请求不记录)
                if (Method != "GET")
                {
                    //var AA = new
                    //{
                    //    Module = filterContext.HttpContext.Request.Path,
                    //    Type = GetType(filterContext.HttpContext.Request.Method, filterContext.HttpContext.Request.Path),
                    //    Level = $"INFO",
                    //    Content = $"{filterContext.HttpContext.User.Claims.FirstOrDefault(o => o.Type == "Issuer")?.Value} {filterContext.HttpContext.Request.Path} {}",
                    //    Creator = string.IsNullOrEmpty(userName) ? "sys" : userName,
                    //    CreatorId = string.IsNullOrEmpty(userId) ? new Guid?() : Guid.Parse(userId),
                    //    CreationTime = Utility.GetSysDate()
                    //};
                    Task.Factory.StartNew(async () =>
                    {
                        DbInsert di = new DbInsert("SmApiLog");
                        di.Values("Path", Path);
                        di.Values("Method", Method);
                        di.Values("IP", ip);
                        di.Values("Source", "GlobalAction");
                        di.Values("Content", Utility.ConvertDictionaryToString(filterContext.ActionArguments));
                        await DBHelper.Instance.ExecuteDMLAsync(di.GetSql());

                    });
                }
                else
                {
                    Task.Factory.StartNew(async () =>
                    {
                        DbInsert di = new DbInsert("SmApiLog");
                        di.Values("Path", Path);
                        di.Values("Method", Method);
                        di.Values("IP", ip);
                        di.Values("Source", "GlobalAction");
                        di.Values("Content", filterContext.HttpContext.Request.QueryString.Value);
                        await DBHelper.Instance.ExecuteDMLAsync(di.GetSql());

                    });
                }
            }

            base.OnActionExecuting(filterContext);
        }

    }
}
