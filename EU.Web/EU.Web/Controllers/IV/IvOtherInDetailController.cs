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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.IV
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.IV)]
    public class IvOtherInDetailController : BaseController<IvOtherInDetail>
    {

        public IvOtherInDetailController(DataContext _context, IBaseCRUDVM<IvOtherInDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(IvOtherInDetail Model)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
                #endregion

                Model.SerialNumber = Utility.GenerateContinuousSequence("IvOtherInDetail", "SerialNumber", "OrderId", Model.OrderId.ToString());
                Model.InTime = Utility.GetSysDate();
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

        [HttpPost]
        public override IActionResult BatchAdd(List<IvOtherInDetail> data)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Guid? OrderId = data[0].OrderId;

                for (int i = 0; i < data.Count; i++)
                {
                    data[i].ID = Guid.NewGuid();
                    DoAddPrepare(data[i]);
                    data[i].CreatedBy = new Guid(User.Identity.Name);
                    data[i].InTime = Utility.GetSysDate();
                }

                if (data.Count > 0)
                    DBHelper.Instance.AddRange(data);

                BatchUpdateSerialNumber(OrderId.ToString());

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

        #region 批量更新排序号
        /// <summary>
        /// 批量更新排序号
        /// </summary>
        /// <param name="orderId">订单ID</param>
        private void BatchUpdateSerialNumber(string orderId)
        {
            string sql = @"UPDATE A
                        SET A.SerialNumber = C.NUM
                        FROM IvOtherInDetail A
                             JOIN
                             (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) NUM
                              FROM (SELECT *
                                    FROM (SELECT A.*
                                          FROM IvOtherInDetail A
                                          WHERE     1 = 1
                                                AND A.OrderId =
                                                    '{0}'
                                                AND A.IsDeleted = 'false'
                                                AND A.IsActive = 'true') A) B) C
                                ON A.ID = C.ID";
            sql = string.Format(sql, orderId);
            DBHelper.Instance.ExecuteScalar(sql);

        }
        #endregion

        #region 删除重写

        [HttpGet]
        public override IActionResult Delete(Guid Id)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                _BaseCrud.DoDelete(Id);

                IvOtherInDetail Model = _context.IvOtherInDetail.Where(x => x.ID == Id).SingleOrDefault();
                if (Model != null)
                    BatchUpdateSerialNumber(Model.OrderId.ToString());

                status = "ok";
                message = "删除成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        [HttpPost]
        public override IActionResult BatchDelete(List<IvOtherInDetail> entryList)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {

                for (int i = 0; i < entryList.Count; i++)
                {
                    DbUpdate du = new DbUpdate("IvOtherInDetail");
                    du.Set("IsDeleted", "true");
                    du.Where("ID", "=", entryList[i].ID);
                    DBHelper.Instance.ExecuteScalar(du.GetSql());
                }

                IvOtherInDetail Model = _context.IvOtherInDetail.Where(x => x.ID == entryList[0].ID).SingleOrDefault();
                if (Model != null)
                    BatchUpdateSerialNumber(Model.OrderId.ToString());

                status = "ok";
                message = "批量删除成功！";
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
