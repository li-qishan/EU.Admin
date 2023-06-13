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
using EU.Core.Services;
using EU.Domain.System;
using EU.Model.System;
using static EU.Core.Const.Consts;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.Weixin
{
    /// <summary>
    /// Interface
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class GetInterfaceController : BaseController1<SmModule>
    {
        /// <summary>
        /// Interface
        /// </summary>
        public GetInterfaceController(DataContext _context, IBaseCRUDVM<SmModule> BaseCrud) : base(_context, BaseCrud)
        {

        }
       
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, HttpPost]
        public IActionResult init()
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Logger.WriteLog(DateTime.Now.ToString());
                DbUpdate du = new DbUpdate("TEST_TEMPLATE");
                du.Set("CREATED_BY", "12345");
                du.Where("ID", "=", "0cb655f4-1bf8-4c6b-baec-544492d089ce");
                int result = DBHelper.Instance.ExcuteNonQuery(du.GetSql(), null);


                DbInsert di = new DbInsert("WX_USER");
                di.Values("OPEN_ID", "1");

                DBHelper.Instance.ExcuteNonQuery(di.GetSql(), null);

                status = "ok";
                message = "查询成功111！";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }
    }
}
