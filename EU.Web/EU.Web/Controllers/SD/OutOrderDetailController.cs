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
    public class OutOrderDetailController : BaseController<OutOrderDetail>
    {

        public OutOrderDetailController(DataContext _context, IBaseCRUDVM<OutOrderDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(OutOrderDetail Model)
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

                Model.SerialNumber = Utility.GenerateContinuousSequence("SdOutOrderDetail", "SerialNumber", "OrderId", Model.OrderId.ToString());

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
        public IActionResult BatchAdd1(List<ShipOrderExtend> orderExtends)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                string OutIsSalesOrder = ConfigCache.GetValue("OutIsSalesOrder");

                Guid? OrderId = orderExtends[0].OrderId;
                var OutOrder = _context.SdOutOrder.Where(x => x.ID == OrderId).SingleOrDefault();
                List<OutOrderDetail> list = new List<OutOrderDetail>();
                int i = 1;
                foreach (ShipOrderExtend item in orderExtends)
                {
                    OutOrderDetail Model = new OutOrderDetail();
                    Model.ID = Guid.NewGuid();
                    DoAddPrepare(Model);
                    Model.OrderId = item.OrderId;
                    Model.SerialNumber = i;
                    Model.SalesOrderId = item.SalesOrderId;
                    Model.SalesOrderNo = item.OrderNo;
                    Model.ShipQTY = item.ShipQTY;
                    Model.OutQTY = item.OutQTY;
                    Model.CustomerMaterialCode = item.CustomerMaterialCode;
                    Model.StockId = item.StockId;
                    Model.GoodsLocationId = item.GoodsLocationId;
                    Model.DeliveryrDate = item.DeliveryrDate;
                    Model.ShipDate = OutOrder.OutDate;
                    Model.OrderSource = item.OrderSource;
                    Model.SalesOrderDetailId = item.SalesOrderDetailId;
                    Model.MaterialId = item.MaterialId;
                    Model.MaterialNo = item.MaterialNo;
                    Model.MaterialName = item.MaterialName;
                    Model.MaterialSpecifications = item.MaterialSpecifications;
                    Model.ShipOrderDetailId = item.ShipOrderDetailId;
                    if (item.OrderSource == "Ship")
                    {
                        Model.ShipOrderId = item.ID;
                    }

                    if (Model.OutQTY > 0)
                    {
                        list.Add(Model);
                        i++;
                    }
                }


                if (list.Count > 0)
                {
                    DBHelper.Instance.AddRange(list);
                    BatchUpdateSerialNumber(OrderId.ToString());

                    #region 批量更新销售单出库数量
                    //string sql = "SELECT  1;";
                    //foreach (OutOrderDetail item in list)
                    //{
                    //    string sql1 = "UPDATE SdOrderDetail SET OutQTY = ISNULL(OutQTY,0) +'{0}', UpdateTime=GETDATE(), UpdateBy='{2}' WHERE ID='{1}';";
                    //    sql1 = string.Format(sql1, item.OutQTY, item.SalesOrderDetailId, User.Identity.Name);
                    //    sql += sql1;
                    //}
                    //DBHelper.Instance.ExcuteNonQuery(sql);
                    #endregion

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
            string sql = string.Empty;
            try
            {
                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", modelModify.ColorNo.Value, ModifyType.Edit, modelModify.ID.Value, "材质编号");
                //#endregion

                string id = modelModify.ID.Value;
                var outOrderDetail = _context.SdOutOrderDetail.Where(x => x.ID == Guid.Parse(id)).SingleOrDefault();
                if (outOrderDetail == null)
                    throw new Exception("无效的数据ID！");

                decimal NewOutQTY = modelModify.OutQTY.Value;
                decimal OutQTY = outOrderDetail.OutQTY;
                string OrderSource = outOrderDetail.OrderSource;
                Guid? ShipOrderDetailId = outOrderDetail.ShipOrderDetailId;
                decimal QTY = 0;
                if (NewOutQTY > OutQTY)
                {
                    Guid? SalesOrderDetailId = outOrderDetail.SalesOrderDetailId;
                    var OutOrder = _context.SdOutOrder.Where(x => x.ID == outOrderDetail.OrderId).SingleOrDefault();

                    if (OrderSource == "Sales")
                    {

                        sql = @"SELECT A.QTY - ISNULL (E.OutQTY, 0) WaitQTY
                                            FROM SdOrderDetail A
                                                 JOIN SdOrder B
                                                    ON     A.OrderId = B.ID
                                                       AND A.IsDeleted = B.IsDeleted
                                                       AND B.CustomerId = '{0}'
                                                 LEFT JOIN
                                                 (SELECT SUM (A.OutQTY) OutQTY, A.SalesOrderDetailId
                                                  FROM SdOutOrderDetail A
                                                       JOIN SdOutOrder B
                                                          ON     A.OrderId = B.ID
                                                             AND A.IsActive = B.IsActive
                                                             AND A.IsDeleted = B.IsDeleted
                                                  WHERE     A.IsDeleted = 'false'
                                                        AND A.IsActive = 'true'
                                                        AND A.OrderSource = 'Sales'
                                                        AND B.CustomerId = '{0}'
                                                  GROUP BY A.SalesOrderDetailId) E
                                                    ON A.ID = E.SalesOrderDetailId
                                            WHERE     A.IsDeleted = 'false'
                                                  AND A.IsActive = 'true'
                                                  AND B.AuditStatus = 'CompleteAudit'
                                                  AND A.ID = '{1}'";
                        sql = string.Format(sql, OutOrder.CustomerId, SalesOrderDetailId);
                    }
                    else
                    {
                        sql = @"SELECT A.ShipQTY - ISNULL (E.OutQTY, 0) WaitQTY
                                    FROM SdShipOrderDetail A
                                         JOIN SdShipOrder B
                                            ON     A.OrderId = B.ID
                                               AND A.IsDeleted = B.IsDeleted
                                               AND B.CustomerId = '{0}'
                                         LEFT JOIN
                                         (SELECT SUM (A.OutQTY) OutQTY, A.ShipOrderId
                                          FROM SdOutOrderDetail A
                                               JOIN SdOutOrder B
                                                  ON     A.OrderId = B.ID
                                                     AND A.IsActive = B.IsActive
                                                     AND A.IsDeleted = B.IsDeleted
                                          WHERE     A.IsDeleted = 'false'
                                                AND A.IsActive = 'true'
                                                AND A.OrderSource = 'Ship'
                                                AND B.CustomerId = '{0}'
                                          GROUP BY A.ShipOrderId) E
                                            ON A.ID = E.ShipOrderId
                                    WHERE     A.IsDeleted = 'false'
                                          AND A.IsActive = 'true'
                                          AND B.AuditStatus = 'CompleteAudit'
                                          AND A.ID = '{1}'";
                        sql = string.Format(sql, OutOrder.CustomerId, ShipOrderDetailId);
                    }

                    QTY = Convert.ToDecimal(DBHelper.Instance.ExecuteScalar(sql));
                    if (QTY < (NewOutQTY - OutQTY))
                        throw new Exception("待出库数量不足，当前待出库:" + QTY + "！");
                }

                //Update<OutOrderDetail>(modelModify);
                //_context.SaveChanges();

                outOrderDetail.OutQTY = modelModify.OutQTY.Value;
                outOrderDetail.CustomerMaterialCode = modelModify.CustomerMaterialCode.Value;
                outOrderDetail.StockId = Guid.Parse(modelModify.StockId.Value);
                outOrderDetail.GoodsLocationId = Guid.Parse(modelModify.GoodsLocationId.Value);
                _context.SaveChanges();

                //DbUpdate du = new DbUpdate("SdOutOrderDetail", "ID", id);
                //du.Set("OutQTY", modelModify.OutQTY.Value);
                //du.Set("CustomerMaterialCode", modelModify.CustomerMaterialCode.Value);
                //DBHelper.Instance.ExecuteScalar(du.GetSql());

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
                        FROM SdOutOrderDetail A
                             JOIN
                             (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) NUM
                              FROM (SELECT *
                                    FROM (SELECT A.*
                                          FROM SdOutOrderDetail A
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

                OutOrderDetail Model = _context.SdOutOrderDetail.Where(x => x.ID == Id).SingleOrDefault();
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
