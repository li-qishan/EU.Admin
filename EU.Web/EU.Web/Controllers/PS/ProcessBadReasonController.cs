using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using EU.Common;
using EU.Core;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.PS
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PS)]
    public class ProcessBadReasonController : BaseController<ProcessBadReason>
    {

        public ProcessBadReasonController(DataContext _context, IBaseCRUDVM<ProcessBadReason> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(ProcessBadReason Model)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //#region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "PsProcessBadReason", "BadCode", Model.BadCode, ModifyType.Add, null, "不良原因代码");
                //#endregion 

                return base.Add(Model);
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

        #region 更新重写
        /// <summary>
        /// 更新重写
        /// </summary>
        /// <param name="modelModify"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Update(dynamic modelModify)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {

                #region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "PsProcessBadReason", "BadCode", modelModify.BadCode.Value, ModifyType.Edit, modelModify.ID.Value, "不良原因代码");
                #endregion

                Update<ProcessBadReason>(modelModify);
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
