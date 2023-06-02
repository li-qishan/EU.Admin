using System;
using System.Dynamic;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.BD
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.Base)]
    public class UnitController : BaseController<Unit>
    {
        public UnitController(DataContext _context, IBaseCRUDVM<Unit> BaseCrud) : base(_context, BaseCrud)
        {

        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(Unit Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "BdUnit", "UnitNo", Model.UnitNo, ModifyType.Add, null, "单位编号");
                #endregion

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
        [HttpPost]
        public override IActionResult Update(dynamic modelModify)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "BdUnit", "UnitNo", modelModify.UnitNo.Value, ModifyType.Edit, modelModify.ID.Value, "单位编号");
                #endregion

                Update<Unit>(modelModify);
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
