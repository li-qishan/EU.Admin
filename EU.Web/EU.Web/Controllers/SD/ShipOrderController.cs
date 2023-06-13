using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ShipOrderController : BaseController1<ShipOrder>
    {
        public new readonly IBaseCRUDVM<ShipOrder> _BaseCrud;

        public ShipOrderController(DataContext _context, IBaseCRUDVM<ShipOrder> BaseCrud) : base(_context, BaseCrud)
        {
            _BaseCrud = BaseCrud;
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(ShipOrder Model)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
                //#endregion

                Model.OrderNo = Utility.GenerateContinuousSequence("SdShipOrderNo");
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
                Update<ShipOrder>(modelModify);
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
                            FROM SdOutOrderDetail A
                                 JOIN SdShipOrderDetail B
                                    ON     A.SalesOrderDetailId = B.SalesOrderDetailId
                                       AND B.IsDeleted = 'false'
                                       AND B.IsActive = 'true'
                                       AND B.OrderId = '{0}'
                            WHERE     A.IsDeleted = 'false'
                                  AND A.IsActive = 'true'
                                  AND A.OrderSource = 'Ship'";
                    sql = string.Format(sql, orderId);
                    DataTable dt = DBHelper.Instance.GetDataTable(sql);
                    #endregion

                    if (dt.Rows.Count == 0)
                        auditStatus = "Add";
                    else throw new Exception("该单据已被引用，不可撤销！");
                }

                #endregion

                DbUpdate du = new DbUpdate("SdShipOrder");
                du.Set("AuditStatus", auditStatus);
                du.Where("ID", "=", orderId);
                DBHelper.Instance.ExecuteScalar(du.GetSql());

                #region 导入订单操作历史
                DBHelper.RecordOperateLog(User.Identity.Name, "SD_OUT_ORDER_MNG", "SdShipOrder", orderId, OperateType.Update, "Audit", "修改订单审核状态为：" + auditStatus);
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

        #region 获取待出库订单
        /// <summary>
        /// 获取待出库订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual IActionResult GetWaitOutList(string paramData, string masterId, string source)
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
                List<ShipOrderExtend> list = new List<ShipOrderExtend>();
                string StockName = string.Empty;
                string GoodsLocationName = string.Empty;
                var Order = _context.SdOutOrder.Where(x => x.ID == Guid.Parse(masterId)).SingleOrDefault();
                if (Order == null)
                    throw new Exception("无效的发货订单ID");

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
                    Order.StockId = null;
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
                }
                else
                {
                    Order.GoodsLocationId = null;
                    if (Order.StockId != null)
                        GoodsLocationList = _context.BdGoodsLocation.Where(x => x.IsDeleted == false && x.StockId == Order.StockId).ToList();
                    if (GoodsLocationList.Count == 1)
                    {
                        Order.GoodsLocationId = GoodsLocationList[0].ID;
                        GoodsLocationName = GoodsLocationList[0].LocationNames;
                    }
                }

                int _pageSize = pageSize;
                //计算分页起始索引
                int startIndex = current > 1 ? (current - 1) * _pageSize : 0;

                //计算分页结束索引
                int endIndex = current * _pageSize;

                string OutIsSalesOrder = ConfigCache.GetValue("OutIsSalesOrder");

                if (OutIsSalesOrder == "Y")
                {
                    string defaultSql = @"SELECT *
                                            FROM (SELECT A.ID,
                                                         B.OrderNo,
                                                         A.ID SalesOrderDetailId,
                                                         B.ID SalesOrderId,
                                                         A.CreatedTime,
                                                         A.SerialNumber,
                                                         C.ID MaterialId,
                                                         C.MaterialNo,
                                                         A.MaterialName,
                                                         C.Specifications,
                                                         C.UnitId,
                                                         C.UnitName,
                                                         ISNULL (C.DecimalPlaces, 0) DecimalPlaces,
                                                         A.QTY,
                                                         A.QTY - ISNULL (E.OutQTY, 0) WaitQTY,
                                                         A.CustomerMaterialCode,
                                                         A.Price,
                                                         A.TaxAmount,
                                                         A.TaxIncludedAmount,
                                                         A.NoTaxAmount,
                                                         CONVERT (VARCHAR (100), B.DeliveryrDate, 23) DeliveryrDate,
                                                         'Sales' OrderSource,
                                                         ISNULL (F.QTY, 0) InventoryQTY
                                                  FROM SdOrderDetail A
                                                       JOIN SdOrder B
                                                          ON     A.OrderId = B.ID
                                                             AND A.IsDeleted = B.IsDeleted
                                                             AND B.CustomerId = '{0}'
                                                       LEFT JOIN BdMaterial_V C ON A.MaterialId = C.ID
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
                                                       LEFT JOIN BdMaterialInventory F
                                                          ON     A.MaterialId = F.MaterialId
                                                             AND F.StockId = '{1}'
                                                             AND F.GoodsLocationId =
                                                                 '{2}'
                                                  WHERE     A.IsDeleted = 'false'
                                                        AND A.IsActive = 'true'
                                                        AND B.AuditStatus = 'CompleteAudit') A
                                            WHERE WaitQTY > 0";
                    defaultSql = string.Format(defaultSql, Order.CustomerId, Order.StockId != null ? Order.StockId : Guid.Empty, Order.GoodsLocationId != null ? Order.GoodsLocationId : Guid.Empty);

                    string sql = @"SELECT * FROM(SELECT *, ROW_NUMBER() OVER(ORDER BY CreatedTime ASC) ROWNUM FROM(" + defaultSql + ") B) C WHERE ROWNUM <= {1} AND ROWNUM > {0}";
                    sql = string.Format(sql, startIndex, endIndex);
                    list = DBHelper.Instance.QueryList<ShipOrderExtend>(sql);

                    string countString = @"SELECT COUNT(0)
                                        FROM (SELECT A.QTY - ISNULL (E.OutQTY, 0) WaitQTY
                                              FROM SdOrderDetail A
                                                   JOIN SdOrder B
                                                      ON     A.OrderId = B.ID
                                                         AND A.IsDeleted = B.IsDeleted
                                                         AND B.CustomerId = '{0}'
                                                         AND B.AuditStatus = 'CompleteAudit'
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
                                              WHERE A.IsDeleted = 'false' AND A.IsActive = 'true') A
                                        WHERE WaitQTY > 0";
                    countString = string.Format(countString, Order.CustomerId);
                    total = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(countString));
                }
                else
                {

                    string defaultSql = @"SELECT *
                                            FROM (SELECT A.ID,
                                                         B.OrderNo,
                                                         A.CreatedTime,
                                                         A.SerialNumber,
                                                         C.ID MaterialId,
                                                         C.MaterialNo,
                                                         A.MaterialName,
                                                         C.Specifications,
                                                         C.UnitId,
                                                         C.UnitName,
                                                         ISNULL (C.DecimalPlaces, 0) DecimalPlaces,
                                                         D.QTY,
                                                         A.ShipQTY,
                                                         A.ShipQTY - ISNULL (E.OutQTY, 0) WaitQTY,
                                                         D.CustomerMaterialCode,
                                                         D.Price,
                                                         D.TaxAmount,
                                                         D.TaxIncludedAmount,
                                                         D.NoTaxAmount,
                                                         CONVERT (VARCHAR (100), A.DeliveryrDate, 23) DeliveryrDate,
                                                         'Ship' OrderSource,
                                                         D.ID SalesOrderDetailId,
                                                         A.ID ShipOrderDetailId,
                                                         D.OrderId SalesOrderId,
                                                         ISNULL (F.QTY, 0) InventoryQTY
                                                  FROM SdShipOrderDetail A
                                                       JOIN SdShipOrder B
                                                          ON     A.OrderId = B.ID
                                                             AND A.IsDeleted = B.IsDeleted
                                                             AND B.CustomerId = '{0}'
                                                       LEFT JOIN BdMaterial_V C ON A.MaterialId = C.ID
                                                       JOIN SdOrderDetail D
                                                          ON A.SalesOrderDetailId = D.ID AND A.IsDeleted = D.IsDeleted
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
                                                       LEFT JOIN BdMaterialInventory F
                                                          ON     A.MaterialId = F.MaterialId
                                                             AND F.StockId = '{1}'
                                                             AND F.GoodsLocationId = '{2}'
                                                  WHERE     A.IsDeleted = 'false'
                                                        AND A.IsActive = 'true'
                                                        AND B.AuditStatus = 'CompleteAudit') A
                                            WHERE WaitQTY > 0";
                    defaultSql = string.Format(defaultSql, Order.CustomerId, Order.StockId != null ? Order.StockId : Guid.Empty, Order.GoodsLocationId != null ? Order.GoodsLocationId : Guid.Empty);

                    string sql = @"SELECT * FROM(SELECT *, ROW_NUMBER() OVER(ORDER BY CreatedTime ASC) ROWNUM FROM(" + defaultSql + ") B) C WHERE ROWNUM <= {1} AND ROWNUM > {0}";
                    sql = string.Format(sql, startIndex, endIndex);
                    list = DBHelper.Instance.QueryList<ShipOrderExtend>(sql);

                    string countString = @"SELECT COUNT (1)
                                            FROM (SELECT A.ShipQTY - ISNULL (E.OutQTY, 0) WaitQTY
                                                  FROM SdShipOrderDetail A
                                                       JOIN SdShipOrder B
                                                          ON     A.OrderId = B.ID
                                                             AND A.IsDeleted = B.IsDeleted
                                                             AND B.CustomerId = '{0}'
                                                       LEFT JOIN BdMaterial_V C ON A.MaterialId = C.ID
                                                       JOIN SdOrderDetail D
                                                          ON     A.SalesOrderDetailId = D.ID
                                                             AND A.IsDeleted = D.IsDeleted
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
                                                        GROUP BY A.ShipOrderId) E
                                                          ON A.ID = E.ShipOrderId
                                                  WHERE     A.IsDeleted = 'false'
                                                        AND A.IsActive = 'true'
                                                        AND B.AuditStatus = 'CompleteAudit') A
                                            WHERE WaitQTY > 0";
                    countString = string.Format(countString, Order.CustomerId);
                    total = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(countString));
                }





                for (int i = 0; i < list.Count; i++)
                {
                    decimal Step = 1;
                    for (int j = 0; j < list[i].DecimalPlaces; j++)
                        Step = Step / 10;

                    list[i].Step = Step;
                    list[i].Min = Step;
                    list[i].StockId = Order.StockId;
                    list[i].GoodsLocationId = Order.GoodsLocationId;
                    list[i].GoodsLocationName = GoodsLocationName;
                    list[i].StockName = StockName;
                    list[i].StockList = StockList;
                    list[i].GoodsLocationList = GoodsLocationList;
                }
                obj.data = list;

                status = "ok";
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

        #region 获取详情
        [HttpGet]
        public override async Task<IActionResult> GetById(Guid Id)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            int count = 0;

            try
            {
                string sql = @"SELECT COUNT (0)
                            FROM SdShipOrderDetail A
                            WHERE     A.IsDeleted = 'false'
                                  AND A.IsActive = 'true'
                                  AND A.OrderId = '{0}'";
                sql = string.Format(sql, Id);
                count = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(sql));

                obj.data = _BaseCrud.GetById(Id);

            }
            catch (Exception E)
            {
                message = E.Message;
            }
            obj.count = count;
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
                var Order = _context.SdShipOrder.Where(x => x.ID == Id).SingleOrDefault();

                if (Order == null)
                    throw new Exception("无效的数据ID！");

                if (Order.AuditStatus == "CompleteAudit")
                    throw new Exception("该发货单已审核通过，暂不可进行删除操作！");

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
