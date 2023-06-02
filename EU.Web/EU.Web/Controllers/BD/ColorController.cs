using System;
using System.Dynamic;
using EU.Core.Enums;
using EU.Core.UserManager;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.BD
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.Base)]
    public class ColorController : BaseController<Color>
    {

        public ColorController(DataContext _context, IBaseCRUDVM<Color> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(Color Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
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

                //Guid? userId = CompanyId;

                #region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "BdColor", "ColorNo", modelModify.ColorNo.Value, ModifyType.Edit, modelModify.ID.Value, "材质编号");
                #endregion

                Update<Color>(modelModify);
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
