using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using EU.Core;
using EU.Core.Extensions;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.PO
{
    /// <summary>
    /// 请购单明细
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PO)]
    public class RequestionDetailController : BaseController<RequestionDetail>
    {
        /// <summary>
        /// 请购单明细
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public RequestionDetailController(DataContext _context, IBaseCRUDVM<RequestionDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Add(RequestionDetail Model)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
                #endregion
                //Model.OrderDetailNo = Utility.GenerateContinuousSequence("SdOrderDetailNo");
                Model.SerialNumber = Utility.GenerateContinuousSequence("PoRequestionDetail", "SerialNumber", "OrderId", Model.OrderId.ToString());

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

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult BatchAdd(List<RequestionDetail> data)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                data.ForEach(item =>
                {
                    item.ID = Guid.NewGuid();
                    DoAddPrepare(item);
                    item.CreatedBy = UserId;
                });

                if (data.Count > 0)
                {
                    Guid? OrderId = data[0].OrderId;
                    DBHelper.Instance.AddRange(data);
                    BatchUpdateSerialNumber(OrderId.ToString());
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

        #region 批量更新排序号
        /// <summary>
        /// 批量更新排序号
        /// </summary>
        /// <param name="orderId">订单ID</param>
        private void BatchUpdateSerialNumber(string orderId)
        {
            string sql = @"UPDATE A
                        SET A.SerialNumber = C.NUM
                        FROM PoRequestionDetail A
                             JOIN
                             (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) NUM
                              FROM (SELECT *
                                    FROM (SELECT A.*
                                          FROM PoRequestionDetail A
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
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public override IActionResult Delete(Guid Id)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                _BaseCrud.DoDelete(Id);

                RequestionDetail Model = _context.PoRequestionDetail.Where(x => x.ID == Id).SingleOrDefault();
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

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entryList"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult BatchDelete(List<RequestionDetail> entryList)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                entryList.ForEach(item =>
                {
                    //item.UpdateBy = UserId;
                    //item.UpdateTime = Utility.GetSysDate();
                    //item.PrivateSet("IsDeleted", true);
                    //item.IsDeleted = !item.IsDeleted;

                    DbUpdate du = new DbUpdate("PoRequestionDetail");
                    du.Set("IsDeleted", "true");
                    du.Where("ID", "=", item.ID);
                    DBHelper.Instance.ExecuteScalar(du.GetSql());
                });

                if (entryList.Count > 0)
                {
                    //DBHelper.Instance.UpdateRange(entryList, x => new { x.UpdateBy, x.UpdateTime, x.IsDeleted });

                    RequestionDetail Model = _context.PoRequestionDetail.Where(x => x.ID == entryList[0].ID).SingleOrDefault();
                    if (Model != null)
                        BatchUpdateSerialNumber(Model.OrderId.ToString());
                }

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
