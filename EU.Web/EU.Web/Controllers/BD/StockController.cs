using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using EU.Core;
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
    public class StockController : BaseController<Stock>
    {
        public StockController(DataContext _context, IBaseCRUDVM<Stock> BaseCrud) : base(_context, BaseCrud)
        {

        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(Stock Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "BdStock", "StockNo", Model.StockNo, ModifyType.Add, null, "类型编号");
                #endregion

                DbInsert di = new DbInsert("BdGoodsLocation");
                di.Values("LocationNo", Model.StockNo + "001");
                di.Values("StockId", Model.ID);
                di.Values("LocationNames", "默认仓");
                DBHelper.Instance.ExcuteNonQuery(di.GetSql());

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
                Utility.CheckCodeExist("", "BdStock", "StockNo", modelModify.StockNo.Value, ModifyType.Edit, modelModify.ID.Value, "类型编号");
                #endregion
                bool IsVirtual = modelModify.IsVirtual.Value;
                if (IsVirtual)
                {
                    Guid id = Guid.Parse(modelModify.ID.Value);
                    List<GoodsLocation> locationList = _context.BdGoodsLocation.Where(x => x.StockId == id && x.IsActive == true && x.IsDeleted == false).ToList();
                    if (locationList.Count > 1)
                        throw new("该仓库下存在多个货位，不可变更为虚拟仓!");
                }


                Update<Stock>(modelModify);
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
