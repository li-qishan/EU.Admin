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
using Newtonsoft.Json;
using static EU.Core.Const.Consts;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.SD
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.SD)]
    public class ReturnOrderController : BaseController1<ReturnOrder>
    {

        public ReturnOrderController(DataContext _context, IBaseCRUDVM<ReturnOrder> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(ReturnOrder Model)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
                //#endregion

                Model.OrderNo = Utility.GenerateContinuousSequence("SdReturnOrderNo");
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
            string isReloadDetail = "N";

            try
            {

                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", modelModify.ColorNo.Value, ModifyType.Edit, modelModify.ID.Value, "材质编号");
                //#endregion
                Update<ReturnOrder>(modelModify);
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
            obj.isReloadDetail = isReloadDetail;
            return Ok(obj);
        }
        #endregion

        #region 获取已出库订单
        /// <summary>
        /// 获取已出库订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual IActionResult GetHasOutList(string paramData, string masterId)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            int current = 1;
            int pageSize = 10000;
            int total = 0;

            try
            {
                var searchParam = JsonConvert.DeserializeObject<Dictionary<string, object>>(paramData);

                #region 处理查询条件
                foreach (var item in searchParam)
                {
                    if (item.Key == "current")
                    {
                        current = int.Parse(item.Value.ToString());
                        continue;
                    }

                    if (item.Key == "pageSize")
                    {
                        pageSize = int.Parse(item.Value.ToString());
                        continue;
                    }
                }
                #endregion

                var Order = _context.SdReturnOrder.Where(x => x.ID == Guid.Parse(masterId)).SingleOrDefault();

                #region 处理货位
                string StockName = string.Empty;
                string GoodsLocationName = string.Empty;
                List<GoodsLocation> GoodsLocationList = new List<GoodsLocation>();
                List<Stock> StockList = _context.BdStock.Where(x => x.IsDeleted == false).ToList();

                if (Order.StockId != null)
                {
                    var Stock = _context.BdStock.Where(x => x.ID == Order.StockId).SingleOrDefault();
                    if (Stock != null)
                        StockName = Stock.StockNames;
                }
                else
                {
                    Order.StockId = Guid.Empty;
                    if (StockList.Count == 1)
                    {
                        Order.StockId = StockList[0].ID;
                        StockName = StockList[0].StockNames;
                    }

                }
                if (Order.GoodsLocationId != null)
                {
                    var GoodsLocation = _context.BdGoodsLocation.Where(x => x.ID == Order.GoodsLocationId).SingleOrDefault();
                    if (GoodsLocation != null)
                        GoodsLocationName = GoodsLocation.LocationNames;
                    GoodsLocationList = _context.BdGoodsLocation.Where(x => x.IsDeleted == false && x.StockId == Order.StockId).ToList();
                }
                else
                {
                    Order.GoodsLocationId = Guid.Empty;
                    if (Order.StockId != Guid.Empty)
                        GoodsLocationList = _context.BdGoodsLocation.Where(x => x.IsDeleted == false && x.StockId == Order.StockId)?.ToList();
                    if (GoodsLocationList != null)
                        if (GoodsLocationList.Count == 1)
                        {
                            Order.GoodsLocationId = GoodsLocationList[0].ID;
                            GoodsLocationName = GoodsLocationList[0].LocationNames;
                        }
                }
                #endregion

                int _pageSize = pageSize;
                //计算分页起始索引
                int startIndex = current > 1 ? (current - 1) * _pageSize : 0;

                //计算分页结束索引
                int endIndex = current * _pageSize;

                string defaultSql = @"SELECT *
                                            FROM (SELECT A.ID,
                                                         A.ID OutOrderDetailId,
                                                         B.ID OutOrderId,
                                                         B.OrderNo OutOrderNo,
                                                         C.OrderNo SalesOrderNo,
                                                         C.ID SalesOrderId,
                                                         D.ID SalesOrderDetailId,
                                                         A.CreatedTime,
                                                         A.SerialNumber,
                                                         A.MaterialId,
                                                         A.MaterialNo,
                                                         A.MaterialName,
                                                         A.MaterialSpecifications,
                                                         A.OutQTY,
                                                         D.Price,
                                                         A.OutQTY - ISNULL (E.ReturnQTY, 0) ReturnQTY,
                                                         D.CustomerMaterialCode,
                                                         F.UnitNames UnitName,
                                                         ISNULL (G.QTY, 0) InventoryQTY,
                                                         B.OutTime,'false' IsEntity, ISNULL (F.DecimalPlaces, 0) DecimalPlaces
                                                  FROM SdOutOrderDetail A
                                                       JOIN SdOutOrder B
                                                          ON     A.OrderId = B.ID
                                                             AND A.IsDeleted = B.IsDeleted
                                                             AND B.AuditStatus = 'CompleteOut'
                                                             AND B.CustomerId = '{0}'
                                                       JOIN SdOrder C ON A.SalesOrderId = C.ID
                                                       JOIN SdOrderDetail D ON A.SalesOrderDetailId = D.ID
                                                       LEFT JOIN
                                                       (SELECT SUM (A.ReturnQTY) ReturnQTY, A.OutOrderDetailId
                                                        FROM SdReturnOrderDetail A
                                                             JOIN SdReturnOrder B
                                                                ON     A.OrderId = B.ID
                                                                   AND A.IsActive = B.IsActive
                                                                   AND A.IsDeleted = B.IsDeleted
                                                        WHERE A.IsDeleted = 'false' AND A.IsActive = 'true' AND B.CustomerId = '{0}'
                                                        GROUP BY A.OutOrderDetailId) E
                                                          ON A.ID = E.OutOrderDetailId
                                                       LEFT JOIN BdUnit F ON A.UnitId = F.ID
                                                       LEFT JOIN BdMaterialInventory G
                                                          ON     A.MaterialId = G.MaterialId
                                                             AND G.StockId = '{1}'
                                                             AND G.GoodsLocationId = '{2}'
                                                  WHERE A.IsDeleted = 'false' AND A.IsActive = 'true') A
                                            WHERE A.ReturnQTY > 0";
                defaultSql = string.Format(defaultSql, Order.CustomerId, Order.StockId, Order.GoodsLocationId);
                string sql = @"SELECT * FROM(SELECT *, ROW_NUMBER() OVER(ORDER BY CreatedTime ASC) ROWNUM FROM(" + defaultSql + ") B) C WHERE ROWNUM <= {1} AND ROWNUM > {0}";
                sql = string.Format(sql, startIndex, endIndex);
                List<ReturnOrderExtend> list = DBHelper.Instance.QueryList<ReturnOrderExtend>(sql);

                obj.data = list;
                string countString = @"SELECT COUNT (1)
                                            FROM (SELECT A.OutQTY - ISNULL (E.ReturnQTY, 0) ReturnQTY
                                                  FROM SdOutOrderDetail A
                                                       JOIN SdOutOrder B
                                                          ON     A.OrderId = B.ID
                                                             AND A.IsDeleted = B.IsDeleted
                                                             AND B.AuditStatus = 'CompleteOut'
                                                             AND B.CustomerId = '{0}'
                                                       JOIN SdOrder C ON A.SalesOrderId = C.ID
                                                       JOIN SdOrderDetail D ON A.SalesOrderDetailId = D.ID
                                                       LEFT JOIN
                                                       (SELECT SUM (A.ReturnQTY) ReturnQTY, A.OutOrderDetailId
                                                        FROM SdReturnOrderDetail A
                                                             JOIN SdReturnOrder B
                                                                ON     A.OrderId = B.ID
                                                                   AND A.IsActive = B.IsActive
                                                                   AND A.IsDeleted = B.IsDeleted
                                                        WHERE A.IsDeleted = 'false' AND A.IsActive = 'true'
                                                        GROUP BY A.OutOrderDetailId) E
                                                          ON A.ID = E.OutOrderDetailId
                                                  WHERE A.IsDeleted = 'false' AND A.IsActive = 'true') A
                                            WHERE A.ReturnQTY > 0";
                countString = string.Format(countString, Order.CustomerId);
                total = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(countString));
                status = "ok";

                for (int i = 0; i < list.Count; i++)
                {
                    decimal Step = 1;
                    for (int j = 0; j < list[i].DecimalPlaces; j++)
                        Step = Step / 10;

                    list[i].Step = Step;
                    list[i].Min = Step;
                    list[i].StockId = Order.StockId != Guid.Empty ? Order.StockId : null;
                    list[i].GoodsLocationId = Order.GoodsLocationId != Guid.Empty ? Order.GoodsLocationId : null;
                    list[i].GoodsLocationName = GoodsLocationName;
                    list[i].StockName = StockName;
                    list[i].StockList = StockList;
                    list[i].GoodsLocationList = GoodsLocationList;
                }
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.current = current;
            obj.pageSize = pageSize;
            obj.total = total;
            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        #endregion

        #region 审核
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="auditStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AuditOrder(dynamic modelModify)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            string orderId = modelModify.orderId;
            string auditStatus = modelModify.auditStatus;
            string sql = string.Empty;
            try
            {

                #region 修改订单审核状态
                if (auditStatus == "Add")
                    auditStatus = "CompleteAudit";
                else if (auditStatus == "CompleteAudit")
                {

                    #region 检查单据是否被引用
                    sql = @"SELECT A.*
                            FROM SdReturnOrderDetail A
                                 LEFT JOIN SdOutOrderDetail B ON A.OutOrderDetailId = B.ID
                            WHERE B.OrderId = '{0}' AND A.IsDeleted = 'false' AND A.IsActive = 'true'";
                    sql = string.Format(sql, orderId);
                    DataTable dt = DBHelper.Instance.GetDataTable(sql);
                    #endregion

                    if (dt.Rows.Count == 0)
                        auditStatus = "Add";
                    else throw new Exception("该单据已被引用，不可撤销！");
                }
                #endregion

                DbUpdate du = new DbUpdate("SdReturnOrder");
                du.Set("AuditStatus", auditStatus);
                du.Where("ID", "=", orderId);
                DBHelper.Instance.ExecuteScalar(du.GetSql());

                #region 导入订单操作历史
                DBHelper.RecordOperateLog(User.Identity.Name, "SD_OUT_ORDER_MNG", "SdReturnOrder", orderId, OperateType.Update, "Audit", "修改订单审核状态为：" + auditStatus);
                #endregion

                status = "ok";
                message = "提交成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            obj.auditStatus = auditStatus;
            return Ok(obj);
        }
        #endregion

        #region 确认退货
        /// <summary>
        /// 确认退货
        /// </summary>
        /// <param name="Id">退货单ID</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ConfirmReturn(Guid Id)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            string sql = string.Empty;

            IDbTransaction trans = DBHelper.Instance.GetNewTransaction();

            try
            {

                Utility.IsNullOrEmpty(Id, "退货单ID不能为空！");

                sql = @"SELECT A.*
                            FROM SdReturnOrderDetail A
                            WHERE A.OrderId = '{0}'
                                  AND A.IsDeleted = 'false'
                                  AND A.IsActive = 'true'AND A.IsEntity = 'true'
                            ORDER BY A.SerialNumber ASC";
                sql = string.Format(sql, Id);
                List<ReturnOrderDetail> list = DBHelper.Instance.QueryList<ReturnOrderDetail>(sql);

                #region 更新订单状态
                DbUpdate du = new DbUpdate("SdReturnOrder");
                du.Set("ReturnStatus", "HasReturn");
                du.Set("ReturnDate", Utility.GetSysDate());
                du.Set("AuditStatus", "CompleteReturn");
                du.Where("ID", "=", Id);
                DBHelper.Instance.ExecuteDML(du.GetSql(), null, null, trans);
                #endregion

                #region 批量更新退货数量
                sql = @"UPDATE A
                        SET A.ReturnQTY = (A.ReturnQTY + B.ReturnQTY)
                        FROM SdOutOrderDetail A
                             JOIN
                             (SELECT SUM (A.ReturnQTY) ReturnQTY, A.OutOrderDetailId
                              FROM SdReturnOrderDetail A
                                   JOIN SdOutOrderDetail B
                                      ON     A.OutOrderDetailId = B.ID
                                         AND B.IsDeleted = 'false'
                                         AND B.IsActive = 'true'
                              WHERE A.OrderId = '{0}'
                              GROUP BY A.OutOrderDetailId) B
                                ON A.ID = B.OutOrderDetailId
                        WHERE A.IsDeleted = 'false' AND A.IsActive = 'true'";
                sql = string.Format(sql, Id);
                DBHelper.Instance.ExecuteDML(sql, null, null, trans);
                #endregion

                #region 还原物料库存
                //sql = @"UPDATE A
                //            SET A.QTY = (A.QTY + B.ReturnQTY)
                //            FROM BdMaterialInventory A
                //                 JOIN
                //                 (SELECT SUM (A.ReturnQTY) ReturnQTY,
                //                   A.OutOrderDetailId,
                //                   A.MaterialId,
                //                   A.StockId,
                //                   A.GoodsLocationId
                //            FROM SdReturnOrderDetail A
                //                 JOIN SdOutOrderDetail B
                //                    ON     A.OutOrderDetailId = B.ID
                //                       AND B.IsDeleted = 'false'
                //                       AND B.IsActive = 'true'
                //            WHERE     A.OrderId = '{0}'
                //                  AND A.StockId IS NOT NULL
                //                  AND A.IsEntity = 'true'
                //            GROUP BY A.OutOrderDetailId,
                //                     A.MaterialId,
                //                     A.StockId,
                //                     A.GoodsLocationId) B
                //                    ON     A.ID = B.MaterialId
                //                       AND A.StockId = B.StockId
                //                       AND A.GoodsLocationId = B.GoodsLocationId
                //            WHERE A.IsDeleted = 'false' AND A.IsActive = 'true'";
                //sql = string.Format(sql, Id);
                //DBHelper.Instance.ExecuteDML(sql, null, null, trans);
                foreach (ReturnOrderDetail item in list)
                {
                    decimal qty = IVChangeHelper.GetMaterialInventory(item.MaterialId, item.StockId, item.GoodsLocationId, trans);

                    MaterialIVChange change = new MaterialIVChange();
                    change.CreatedBy = Guid.Parse(User.Identity.Name);
                    change.MaterialId = item.MaterialId;
                    change.StockId = item.StockId;
                    change.GoodsLocationId = item.GoodsLocationId;
                    change.BeforeQTY = qty;
                    change.QTY = item.ReturnQTY;
                    change.AfterQTY = qty + item.ReturnQTY;
                    change.ChangeType = IVChangeHelper.ChangeType.SalesReturn.ToString();
                    change.OrderId = item.OrderId;
                    change.OrderDetailId = item.ID;
                    IVChangeHelper.Add(change, trans);

                    sql = @"UPDATE BdMaterialInventory
                            SET QTY = QTY+'{3}'
                            WHERE MaterialId = '{0}'
                                  AND StockId = '{1}'
                                  AND GoodsLocationId = '{2}'";

                    sql = string.Format(sql, item.MaterialId, item.StockId, item.GoodsLocationId, item.ReturnQTY);
                    DBHelper.Instance.ExecuteDML(sql, null, null, trans);
                }
                #endregion

                DBHelper.Instance.CommitTransaction(trans);


                #region 导入订单操作历史
                DBHelper.RecordOperateLog(User.Identity.Name, "SD_RETURN_ORDER_MNG", "SdReturnOrder", Id.ToString(), OperateType.Update, "CompleteReturn", "修改订单状态为：CompleteReturn");
                #endregion

                status = "ok";
                message = "退回成功！";
            }
            catch (Exception E)
            {
                DBHelper.Instance.RollbackTransaction(trans);
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }
        #endregion

        #region 删除
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
                var Order = _context.SdReturnOrder.Where(x => x.ID == Id).SingleOrDefault();

                if (Order == null)
                    throw new Exception("无效的数据ID！");

                if (Order.AuditStatus != "Add")
                    throw new Exception("该退货单已审核通过，暂不可进行删除操作！");

                _BaseCrud.DoDelete(Id);

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
