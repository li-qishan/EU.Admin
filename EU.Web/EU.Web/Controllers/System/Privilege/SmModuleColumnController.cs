using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EU.Core.CacheManager;
using EU.Core.Utilities;
using EU.Model.System;
using EU.DataAccess;
using EU.Domain;
using EU.Domain.System;
using EU.Model.Auth;
using EU.Model.BFProject;
using EU.Model.System.Privilege;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmModuleColumnController : BaseController<SmModuleColumn>
    {
        RedisCacheService RedisCacheService = new RedisCacheService(2);
        public SmModuleColumnController(DataContext _context, IBaseCRUDVM<SmModuleColumn> BaseCrud) : base(_context, BaseCrud)
        {

        }

        #region 删除重写
        [HttpGet]
        public override IActionResult Delete(Guid Id)
        {
            RedisCacheService.Remove("SmModuleColumn");
            return base.Delete(Id);
        }
        #endregion

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(SmModuleColumn Model)
        {
            RedisCacheService.Remove("SmModuleColumn");
            return base.Add(Model);
        }
        #endregion

        #region 更新重写
        [HttpPost]
        public override IActionResult Update(dynamic modelModify)
        {
            RedisCacheService.Remove("SmModuleColumn");

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Update<SmModuleColumn>(modelModify);
                _context.SaveChanges();

                status = "ok";
                message = "修改成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }
        #endregion
    }
}
