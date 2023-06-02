using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.BFProject.Project
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmLovDetailController : BaseController<SmLovDetail>
    {
        public SmLovDetailController(DataContext _context, IBaseCRUDVM<SmLovDetail> BaseCrud) : base(_context, BaseCrud)
        {

        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(SmLovDetail Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //#region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "SmLovDetail", "Value", Model.Value, ModifyType.Add, null, "参数值", "SmLovId ='" + Model.SmLovId + "'");
                //#endregion

                LOVHelper.Init();

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

                //#region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "SmLovDetail", "Value", modelModify.Value.Value, ModifyType.Edit, modelModify.ID.Value, "参数值", "SmLovId ='" + modelModify.SmLovId.Value + "'");
                //#endregion

                Update<SmLovDetail>(modelModify);
                _context.SaveChanges();

                LOVHelper.Init();

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
