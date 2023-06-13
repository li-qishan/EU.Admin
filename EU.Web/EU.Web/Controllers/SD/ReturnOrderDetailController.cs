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

namespace EU.Web.Controllers.SD
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.SD)]
    public class ReturnOrderDetailController : BaseController1<ReturnOrderDetail>
    {

        public ReturnOrderDetailController(DataContext _context, IBaseCRUDVM<ReturnOrderDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(ReturnOrderDetail Model)
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

                Model.SerialNumber = Utility.GenerateContinuousSequence("SdReturnOrderDetail", "SerialNumber", "OrderId", Model.OrderId.ToString());

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
        public IActionResult BatchAdd1(List<ReturnOrderExtend> orderExtends)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Guid? OrderId = orderExtends[0].OrderId;
                var Order = _context.SdReturnOrder.Where(x => x.ID == OrderId).SingleOrDefault();
                List<ReturnOrderDetail> list = new List<ReturnOrderDetail>();
                int i = 1;
                foreach (ReturnOrderExtend item in orderExtends)
                {
                    ReturnOrderDetail Model = new ReturnOrderDetail();
                    Model.ID = Guid.NewGuid();
                    DoAddPrepare(Model);
                    Model.OrderId = item.OrderId;
                    Model.SalesOrderId = item.SalesOrderId;
                    Model.SalesOrderNo = item.SalesOrderNo;
                    Model.SalesOrderDetailId = item.SalesOrderDetailId;
                    Model.OutOrderNo = item.OutOrderNo;
                    Model.OutOrderId = item.OutOrderId;
                    Model.OutOrderDetailId = item.OutOrderDetailId;
                    Model.SerialNumber = i;
                    Model.MaterialId = item.MaterialId;
                    Model.ReturnQTY = item.ReturnQTY;
                    //Model.NoTaxAmount = item.NoTaxAmount;
                    //Model.TaxAmount = item.TaxAmount;
                    //Model.TaxIncludedAmount = item.TaxIncludedAmount;
                    Model.StockId = item.StockId;
                    Model.GoodsLocationId = item.GoodsLocationId;
                    Model.ReturnDate = Order.ReturnDate;
                    Model.ReturnStatus = "WaitReturn";
                    Model.CustomerMaterialCode = item.CustomerMaterialCode;
                    Model.IsEntity = item.IsEntity;
                    if (Model.ReturnQTY > 0)
                    {
                        list.Add(Model);
                        i++;
                    }
                }

                if (list.Count > 0)
                    DBHelper.Instance.AddRange(list);

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

        #region 更新重写
        [HttpPost]
        public override IActionResult Update(dynamic modelModify)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {

                string id = modelModify.ID.Value;
                var OutOrderDetail = _context.SdReturnOrderDetail.Where(x => x.ID == Guid.Parse(id)).SingleOrDefault();
                if (OutOrderDetail == null)
                    throw new Exception("无效的数据ID！");

                Guid? OutOrderDetailId = OutOrderDetail.OutOrderDetailId;
                decimal NewReturnQTY = modelModify.ReturnQTY.Value;
                decimal? ReturnQTY = OutOrderDetail.ReturnQTY;
                if (NewReturnQTY > ReturnQTY)
                {
                    var ReturnOrder = _context.SdReturnOrder.Where(x => x.ID == OutOrderDetail.OrderId).SingleOrDefault();
                    string sql = @"SELECT A.OutQTY - ISNULL (E.ReturnQTY, 0) ReturnQTY
                                        FROM SdOutOrderDetail A
                                             JOIN SdOutOrder B
                                                ON     A.OrderId = B.ID
                                                   AND A.IsDeleted = B.IsDeleted
                                                   AND B.AuditStatus = 'CompleteOut'
                                                   AND B.CustomerId = '{0}'
                                             LEFT JOIN
                                             (SELECT SUM (A.ReturnQTY) ReturnQTY, A.OutOrderDetailId
                                              FROM SdReturnOrderDetail A
                                                   JOIN SdReturnOrder B
                                                      ON     A.OrderId = B.ID
                                                         AND A.IsActive = B.IsActive
                                                         AND A.IsDeleted = B.IsDeleted
                                              WHERE     A.IsDeleted = 'false'
                                                    AND A.IsActive = 'true'
                                                    AND B.CustomerId = '{0}'
                                              GROUP BY A.OutOrderDetailId) E
                                                ON A.ID = E.OutOrderDetailId
                                        WHERE     A.IsDeleted = 'false'
                                              AND A.IsActive = 'true'
                                              AND A.ID = '{1}'";
                    sql = string.Format(sql, ReturnOrder.CustomerId, OutOrderDetailId);
                    decimal QTY = Convert.ToDecimal(DBHelper.Instance.ExecuteScalar(sql));
                    if (QTY < (NewReturnQTY - ReturnQTY))
                        throw new Exception("待退货数量不足，当前待退货:" + QTY + "！");
                }

                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", modelModify.ColorNo.Value, ModifyType.Edit, modelModify.ID.Value, "材质编号");
                //#endregion
                //_context.Dispose();
                //modelModify.NoShipQTY = modelModify.ShipQTY;
                //Update<ReturnOrderDetail>(modelModify);
                //_context.SaveChanges();

                DbUpdate du = new DbUpdate("SdReturnOrderDetail", "ID", id);
                du.Set("ReturnQTY", modelModify.ReturnQTY.Value);
                du.Set("CustomerMaterialCode", modelModify.CustomerMaterialCode.Value);
                DBHelper.Instance.ExecuteScalar(du.GetSql());

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

        #region 批量更新排序号
        /// <summary>
        /// 批量更新排序号
        /// </summary>
        /// <param name="orderId">订单ID</param>
        private void BatchUpdateSerialNumber(string orderId)
        {
            string sql = @"UPDATE A
                        SET A.SerialNumber = C.NUM
                        FROM SdReturnOrderDetail A
                             JOIN
                             (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) NUM
                              FROM (SELECT *
                                    FROM (SELECT A.*
                                          FROM SdReturnOrderDetail A JOIN SdOutOrderDetail B ON A.OutOrderDetailId=B.ID
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

                ReturnOrderDetail Model = _context.SdReturnOrderDetail.Where(x => x.ID == Id).SingleOrDefault();
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
        #endregion
    }
}
