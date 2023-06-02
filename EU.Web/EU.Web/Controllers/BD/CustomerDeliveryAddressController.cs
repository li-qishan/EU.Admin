using System;
using System.Dynamic;
using System.Linq;
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
    public class CustomerDeliveryAddressController : BaseController<CustomerDeliveryAddress>
    {
        public CustomerDeliveryAddressController(DataContext _context, IBaseCRUDVM<CustomerDeliveryAddress> BaseCrud) : base(_context, BaseCrud)
        {

        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(CustomerDeliveryAddress Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                DoAddPrepare(Model);
                _BaseCrud.DoAdd(Model);

                obj.Id = Model.ID.ToString();

                if (Model.IsDefault)
                {
                    string sql = "UPDATE BdCustomerDeliveryAddress SET IsDefault='false' WHERE CustomerId='{0}' AND ID !='{1}' AND IsDefault='true'";
                    sql = string.Format(sql, Model.CustomerId, Model.ID);
                    DBHelper.Instance.ExcuteNonQuery(sql);
                }

                status = "ok";
                message = "添加成功！";
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
                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", modelModify.ColorNo.Value, ModifyType.Edit, modelModify.ID.Value, "材质编号");
                //#endregion

                Update<CustomerDeliveryAddress>(modelModify);
                _context.SaveChanges();

                bool IsDefault = modelModify.IsDefault.Value;
                if (IsDefault)
                {
                    string sql = "UPDATE BdCustomerDeliveryAddress SET IsDefault='false' WHERE CustomerId='{0}' AND ID !='{1}' AND IsDefault='true'";
                    sql = string.Format(sql, modelModify.CustomerId.Value, modelModify.ID.Value);
                    DBHelper.Instance.ExcuteNonQuery(sql);
                }

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

        #region 获取客户默认地址
        [HttpGet]
        public IActionResult GetDefaultData(Guid masterId)
        {
            dynamic obj = new ExpandoObject();
            dynamic data = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {

                CustomerDeliveryAddress address = _context.BdCustomerDeliveryAddress.Where(x => x.CustomerId == masterId && x.IsDefault == true && x.IsActive == true && x.IsDeleted == false).SingleOrDefault(); ;
                if (address == null)
                    address = _context.BdCustomerDeliveryAddress.Where(x => x.CustomerId == masterId && x.IsActive == true && x.IsDeleted == false).SingleOrDefault(); ;
                data = address;
                status = "ok";
                message = "查询成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.data = data;
            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }
        #endregion

    }
}
