using EU.Core;
using EU.Core.Entry;
using EU.Core.Extensions;
using EU.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace EU.Web
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpRequest request = context.HttpContext.Request;
            string host = request.Host.Value;
            string patch = request.Path;
            string QueryString = request.QueryString.Value;
            string Path = request.Path;
            string Method = request.Method;
            var ip = HttpContextExtension.GetUserIp(EU.Core.Utilities.HttpContext.Current);

            // 记录日志
            var error = context.Exception.InnerException ?? context.Exception;
            //LoggerHelper.SendLogError(error.ToString());
            context.ExceptionHandled = true;

            //记录日志(GET请求不记录)
            Task.Factory.StartNew(async () =>
            {
                DbInsert di = new DbInsert("SmApiLog");
                di.Values("Path", Path);
                di.Values("Method", Method);
                di.Values("IP", ip);
                di.Values("Content", QueryString);
                di.Values("Source", "GlobalException");
                di.Values("Remark", error.Message);
                await DBHelper.Instance.ExecuteDMLAsync(di.GetSql());
            });
            context.Result = ConvertResult(error);
        }

        private static JsonResult ConvertResult(Exception exception)
        {
            //dynamic obj = new ExpandoObject();
            //string status = "error";
            //string message = exception.Message;

            //var result1 = exception.GetBaseException().Message;


            //obj.status = status;
            //obj.message = message;
            //return new JsonResult(obj);
            var code = ServiceResultCode.Failed;
            var result = exception.GetBaseException().Message;
            return new JsonResult(ServiceResult.OprateFailed(result, code));
        }
    }
}
