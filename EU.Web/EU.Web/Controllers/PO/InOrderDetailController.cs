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
using Newtonsoft.Json;
using static EU.Core.Const.Consts;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.PO
{
    /// <summary>
    /// 采购入库单明细
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PO)]
    public class InOrderDetailController : BaseController1<InOrderDetail>
    {
        /// <summary>
        /// 采购入库单明细
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public InOrderDetailController(DataContext _context, IBaseCRUDVM<InOrderDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Add(InOrderDetail Model)
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

                Model.SerialNumber = Utility.GenerateContinuousSequence("PoInOrderDetail", "SerialNumber", "OrderId", Model.OrderId.ToString());

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
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult BatchAdd(List<InOrderDetail> list)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            string OrderId = string.Empty;

            try
            {
                if (list.Count > 0)
                {
                    OrderId = list[0].OrderId.ToString();
                    InOrder order = _context.PoInOrder.Where(x => x.ID == Guid.Parse(OrderId)).SingleOrDefault();
                    //Supplier supplier = _context.BdSupplier.Where(x => x.ID == Guid.Parse(OrderId)).SingleOrDefault();

                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].ID = Guid.NewGuid();
                        list[i].CreatedTime = Utility.GetSysDate();
                        if (order != null && list[i].StockId == null && list[i].GoodsLocationId == null)
                        {
                            list[i].StockId = order.StockId;
                            list[i].GoodsLocationId = order.GoodsLocationId;
                        }
                        DoAddPrepare(list[i]);
                    }
                    DBHelper.Instance.AddRange(list);
                    BatchUpdateSerialNumber(OrderId);
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
                        FROM PoInOrderDetail A
                             JOIN
                             (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) NUM
                              FROM (SELECT *
                                    FROM (SELECT A.*
                                          FROM PoInOrderDetail A
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

                //InOrderDetail Model = _context.PoInOrderDetail.Where(x => x.ID == Id).SingleOrDefault();
                //if (Model != null)
                //    BatchUpdateSerialNumber(Model.OrderId.ToString());

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
        public override IActionResult BatchDelete(List<InOrderDetail> entryList)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {

                for (int i = 0; i < entryList.Count; i++)
                {
                    DbUpdate du = new DbUpdate("PoInOrderDetail");
                    du.Set("IsDeleted", "true");
                    du.Where("ID", "=", entryList[i].ID);
                    DBHelper.Instance.ExecuteScalar(du.GetSql());
                }

                InOrderDetail Model = _context.PoInOrderDetail.Where(x => x.ID == entryList[0].ID).SingleOrDefault();
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

        #region 获取待入库订单
        /// <summary>
        /// 获取待入库订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual IActionResult GetWaitInList(string paramData, string masterId, string Source)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            int current = 1;
            int pageSize = 10000;
            int total = 0;
            string sql = string.Empty;

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

                var PoInOrder = _context.PoInOrder.Where(x => x.ID == Guid.Parse(masterId)).SingleOrDefault();


                int _pageSize = pageSize;
                //计算分页起始索引
                int startIndex = current > 1 ? (current - 1) * _pageSize : 0;

                //计算分页结束索引
                int endIndex = current * _pageSize;

                if (Source == "POOrder")
                {
                    sql = @"SELECT *
                                FROM(SELECT *, ROW_NUMBER() OVER(ORDER BY CreatedTime ASC) ROWNUM
                                      FROM(SELECT *
                                            FROM (SELECT A.SerialNumber,
                                                         'POOrder' OrderSource,
                                                         B.ID SourceOrderId,
                                                         B.OrderNo SourceOrderNo,
                                                         A.ID SourceOrderDetailId,
                                                         A.MaterialId,
                                                         C.MaterialName,
                                                         C.MaterialNo,
                                                         C.Specifications,
                                                         C.UnitId,
                                                         C.UnitName,
                                                         A.PurchaseQTY - ISNULL (D.InQTY, 0) InQTY,
                                                         A.PurchaseQTY - ISNULL (D.InQTY, 0) MaxInQTY,
                                                         '0' ArrivalQTY,
                                                         A.PurchaseQTY,
                                                         A.ReserveDeliveryTime,
                                                         A.CreatedTime
                                                  FROM PoOrderDetail A
                                                       JOIN PoOrder B
                                                          ON     A.OrderId = B.ID
                                                             AND B.IsDeleted = 'false'
                                                             AND B.IsActive = 'true'
                                                             AND B.AuditStatus = 'CompleteAudit'
                                                             AND B.SupplierId = '{2}'
                                                       LEFT JOIN BdMaterial_V C ON A.MaterialId = C.ID
                                                       LEFT JOIN
                                                       (SELECT SUM (A.InQTY) InQTY, A.SourceOrderDetailId
                                                        FROM PoInOrderDetail A
                                                             JOIN PoInOrder B
                                                                ON     A.OrderId = B.ID
                                                                   AND B.IsActive = 'true'
                                                                   AND B.IsDeleted = 'false'AND B.SupplierId = '{2}'
                                                        WHERE     A.IsActive = 'true'
                                                              AND A.IsDeleted = 'false'
                                                              AND A.InQTY > 0 AND A.SourceOrderDetailId IS NOT NULL
                                                        GROUP BY A.SourceOrderDetailId) D
                                                          ON A.ID = D.SourceOrderDetailId
                                                  WHERE A.IsDeleted = 'false' AND A.IsActive = 'true'
                        AND A.ID NOT IN
                               (SELECT A.SourceOrderDetailId
                                FROM PoArrivalOrderDetail A
                                     JOIN PoArrivalOrder B
                                        ON     A.OrderId = B.ID
                                           AND B.IsDeleted = 'false'
                                           AND B.IsActive = 'true'
                                WHERE     A.IsDeleted = 'false'
                                      AND A.IsActive = 'true'
                                      AND A.OrderSource = 'POOrder')) A
                                            WHERE A.InQTY > 0) B)
                                     C
                                WHERE ROWNUM <= {1} AND ROWNUM > {0}";
                    sql = string.Format(sql, startIndex, endIndex, PoInOrder.SupplierId);
                    obj.data = DBHelper.Instance.QueryList<InOrderDetailExtend>(sql);
                    string countString = @"SELECT COUNT (1)
                                                FROM (SELECT A.PurchaseQTY - ISNULL (D.InQTY, 0) InQTY
                                                      FROM PoOrderDetail A
                                                           JOIN PoOrder B
                                                              ON A.OrderId = B.ID AND B.IsDeleted = 'false' AND B.IsActive = 'true' AND B.AuditStatus = 'CompleteAudit'
                                                           LEFT JOIN
                                                           (SELECT SUM (A.InQTY) InQTY, A.SourceOrderDetailId
                                                            FROM PoInOrderDetail A
                                                                 JOIN PoInOrder B
                                                                    ON     A.OrderId = B.ID
                                                                       AND B.IsActive = 'true'
                                                                       AND B.IsDeleted = 'false'
                                                                       AND B.SupplierId = '{0}'
                                                            WHERE     A.IsActive = 'true'
                                                                  AND B.IsDeleted = 'false'
                                                                  AND A.InQTY > 0
                                                            GROUP BY A.SourceOrderDetailId) D
                                                              ON A.ID = D.SourceOrderDetailId
                                                      WHERE A.IsDeleted = 'false' AND A.IsActive = 'true'
                        AND A.ID NOT IN
                               (SELECT A.SourceOrderDetailId
                                FROM PoArrivalOrderDetail A
                                     JOIN PoArrivalOrder B
                                        ON     A.OrderId = B.ID
                                           AND B.IsDeleted = 'false'
                                           AND B.IsActive = 'true'
                                WHERE     A.IsDeleted = 'false'
                                      AND A.IsActive = 'true'
                                      AND A.OrderSource = 'POOrder')) A
                                                WHERE A.InQTY > 0";
                    countString = string.Format(countString, PoInOrder.SupplierId);
                    total = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(countString));
                }
                else
                {
                    sql = @"SELECT *
                                FROM(SELECT *, ROW_NUMBER() OVER(ORDER BY CreatedTime ASC) ROWNUM
                                      FROM(SELECT *
                                            FROM (SELECT A.SerialNumber,
                                                         'ArrivalOrder' OrderSource,
                                                         B.ID SourceOrderId,
                                                         B.OrderNo SourceOrderNo,
                                                         A.ID SourceOrderDetailId,
                                                         A.MaterialId,
                                                         C.MaterialName,
                                                         C.MaterialNo,
                                                         C.Specifications,
                                                         C.UnitId,
                                                         C.UnitName,
                                                         A.ArrivalQTY - ISNULL (D.InQTY, 0) InQTY,
                                                         A.ArrivalQTY - ISNULL (D.InQTY, 0) MaxInQTY,
                                                         A.ArrivalQTY,
                                                         '0' PurchaseQTY,
                                                         A.ReserveDeliveryTime,
                                                         A.CreatedTime
                                                  FROM PoArrivalOrderDetail A
                                                       JOIN PoArrivalOrder B
                                                          ON     A.OrderId = B.ID
                                                             AND B.IsDeleted = 'false'
                                                             AND B.IsActive = 'true'
                                                             AND B.AuditStatus = 'CompleteAudit' AND B.SupplierId = '{2}'
                                                       LEFT JOIN BdMaterial_V C ON A.MaterialId = C.ID
                                                       LEFT JOIN
                                                       (SELECT SUM (A.InQTY) InQTY, A.SourceOrderDetailId
                                                        FROM PoInOrderDetail A
                                                             JOIN PoInOrder B
                                                                ON     A.OrderId = B.ID
                                                                   AND B.IsActive = 'true'
                                                                   AND B.IsDeleted = 'false'
                                                        WHERE     A.IsActive = 'true'
                                                              AND B.IsDeleted = 'false'
                                                              AND A.InQTY > 0
                                                        GROUP BY A.SourceOrderDetailId) D
                                                          ON A.ID = D.SourceOrderDetailId
                                                  WHERE A.IsDeleted = 'false' AND A.IsActive = 'true') A
                                            WHERE A.InQTY > 0) B)
                                     C
                                WHERE ROWNUM <= {1} AND ROWNUM > {0}";
                    sql = string.Format(sql, startIndex, endIndex, PoInOrder.SupplierId);
                    obj.data = DBHelper.Instance.QueryList<InOrderDetailExtend>(sql);

                    string countString = @"SELECT COUNT (1)
                                        FROM (SELECT A.ArrivalQTY - ISNULL (D.InQTY, 0) InQTY
                                                  FROM PoArrivalOrderDetail A
                                                       JOIN PoArrivalOrder B
                                                          ON     A.OrderId = B.ID
                                                             AND B.IsDeleted = 'false'
                                                             AND B.IsActive = 'true'
                                                             AND B.AuditStatus = 'CompleteAudit' AND B.SupplierId = '{0}'
                                                       LEFT JOIN
                                                       (SELECT SUM (A.InQTY) InQTY, A.SourceOrderDetailId
                                                        FROM PoInOrderDetail A
                                                             JOIN PoInOrder B
                                                                ON     A.OrderId = B.ID
                                                                   AND B.IsActive = 'true'
                                                                   AND B.IsDeleted = 'false'
                                                        WHERE     A.IsActive = 'true'
                                                              AND B.IsDeleted = 'false'
                                                              AND A.InQTY > 0
                                                        GROUP BY A.SourceOrderDetailId) D
                                                          ON A.ID = D.SourceOrderDetailId
                                                  WHERE A.IsDeleted = 'false' AND A.IsActive = 'true') A
                                            WHERE A.InQTY > 0";
                    countString = string.Format(countString, PoInOrder.SupplierId);
                    total = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(countString));
                }
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
    }
}
