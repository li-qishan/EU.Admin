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
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PO)]
    public class POReturnOrderDetailController : BaseController<POReturnOrderDetail>
    {

        public POReturnOrderDetailController(DataContext _context, IBaseCRUDVM<POReturnOrderDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(POReturnOrderDetail Model)
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

                Model.SerialNumber = Utility.GenerateContinuousSequence("PoPOReturnOrderDetail", "SerialNumber", "OrderId", Model.OrderId.ToString());

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
        public override IActionResult BatchAdd(List<POReturnOrderDetail> list)
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

                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].ID = Guid.NewGuid();
                        list[i].CreatedTime = Utility.GetSysDate();
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
                var PoReturnOrderDetail = _context.PoReturnOrderDetail.Where(x => x.ID == Guid.Parse(id)).SingleOrDefault();
                if (PoReturnOrderDetail == null)
                    throw new Exception("无效的数据ID！");

                string OrderSource = PoReturnOrderDetail.OrderSource;
                decimal NewReturnQTY = modelModify.ReturnQTY.Value;
                decimal ReturnQTY = PoReturnOrderDetail.ReturnQTY;
                if ((OrderSource == "InOrder" || OrderSource == "ArrivalOrder") && NewReturnQTY > ReturnQTY)
                {
                    var PoReturnOrder = _context.PoReturnOrder.Where(x => x.ID == PoReturnOrderDetail.OrderId).SingleOrDefault();
                    Guid? SourceOrderDetailId = PoReturnOrderDetail.SourceOrderDetailId;

                    string sql = string.Empty;
                    if (OrderSource == "InOrder")
                        sql = @"SELECT A.InQTY - ISNULL (C.ReturnQTY, 0) ReturnQTY
                                    FROM PoInOrderDetail A
                                         JOIN PoInOrder B
                                            ON     A.OrderId = B.ID
                                               AND B.IsDeleted = 'false'
                                               AND B.IsActive = 'true'
                                               AND B.AuditStatus = 'CompleteIn'
                                               AND B.SupplierId = '{1}'
                                         LEFT JOIN
                                         (SELECT SUM (A.ReturnQTY) ReturnQTY, A.SourceOrderDetailId
                                          FROM PoReturnOrderDetail A
                                               JOIN PoReturnOrder B
                                                  ON     A.OrderId = B.ID
                                                     AND B.IsActive = 'true'
                                                     AND B.IsDeleted = 'false'
                                          WHERE     A.IsActive = 'true'
                                                AND B.IsDeleted = 'false'
                                                AND A.ReturnQTY > 0
                                                AND B.SupplierId = '{1}'
                                          GROUP BY A.SourceOrderDetailId) C
                                            ON A.ID = C.SourceOrderDetailId
                                    WHERE A.IsDeleted = 'false' AND A.IsActive = 'true' AND A.ID = '{0}'";
                    else
                        sql = @"SELECT A.ArrivalQTY - ISNULL (C.ReturnQTY, 0) ReturnQTY
                                    FROM PoArrivalOrderDetail A
                                         JOIN PoArrivalOrder B
                                            ON     A.OrderId = B.ID
                                               AND B.IsDeleted = 'false'
                                               AND B.IsActive = 'true'
                                               AND B.AuditStatus = 'CompleteAudit'
                                               AND B.SupplierId = '{2}'
                                         LEFT JOIN
                                         (SELECT SUM (A.ReturnQTY) ReturnQTY, A.SourceOrderDetailId
                                          FROM PoReturnOrderDetail A
                                               JOIN PoReturnOrder B
                                                  ON     A.OrderId = B.ID
                                                     AND B.IsActive = 'true'
                                                     AND B.IsDeleted = 'false'
                                          WHERE     A.IsActive = 'true'
                                                AND B.IsDeleted = 'false'
                                                AND A.ReturnQTY > 0
                                                AND B.SupplierId = '{2}'
                                          GROUP BY A.SourceOrderDetailId) C
                                            ON A.ID = C.SourceOrderDetailId
                                    WHERE A.IsDeleted = 'false' AND A.IsActive = 'true' AND A.ID = '{0}'";
                    sql = string.Format(sql, SourceOrderDetailId, PoReturnOrder.SupplierId);
                    decimal QTY = Convert.ToDecimal(DBHelper.Instance.ExecuteScalar(sql));
                    if (QTY < (NewReturnQTY - ReturnQTY))
                        throw new Exception("待退货数量不足，当前待退货:" + QTY + "！");
                }

                DbUpdate du = new DbUpdate("PoReturnOrderDetail", "ID", id);
                du.Set("ReturnQTY", NewReturnQTY);
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
                        FROM PoReturnOrderDetail A
                             JOIN
                             (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) NUM
                              FROM (SELECT *
                                    FROM (SELECT A.*
                                          FROM PoReturnOrderDetail A
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

                //POReturnOrderDetail Model = _context.PoPOReturnOrderDetail.Where(x => x.ID == Id).SingleOrDefault();
                //if (Model != null)
                //    BatchUpdateSerialNumber(Model.OrderId.ToString());

                BatchUpdateSerialNumber(Id.ToString());

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
        public override IActionResult BatchDelete(List<POReturnOrderDetail> entryList)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {

                for (int i = 0; i < entryList.Count; i++)
                {
                    DbUpdate du = new DbUpdate("POReturnOrderDetail");
                    du.Set("IsDeleted", "true");
                    du.Where("ID", "=", entryList[i].ID);
                    DBHelper.Instance.ExecuteScalar(du.GetSql());
                }

                POReturnOrderDetail Model = _context.PoReturnOrderDetail.Where(x => x.ID == entryList[0].ID).SingleOrDefault();
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

        #region 获取待退货订单
        /// <summary>
        /// 获取待退货订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual IActionResult GetWaitReturnList(string paramData, string masterId, string Source)
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

                var ReturnOrder = _context.PoReturnOrder.Where(x => x.ID == Guid.Parse(masterId)).SingleOrDefault();


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

                int _pageSize = pageSize;
                //计算分页起始索引
                int startIndex = current > 1 ? (current - 1) * _pageSize : 0;

                //计算分页结束索引
                int endIndex = current * _pageSize;

                if (Source == "InOrder")
                {
                    sql = @"SELECT *
                                FROM (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) ROWNUM
                                      FROM (SELECT *
                                            FROM (SELECT A.SerialNumber,
                                                         'InOrder' OrderSource,
                                                         B.ID SourceOrderId,
                                                         B.OrderNo SourceOrderNo,
                                                         A.ID SourceOrderDetailId,
                                                         A.MaterialId,
                                                         D.MaterialNo,
                                                         D.MaterialName,
                                                         D.Specifications,
                                                         D.Description,
                                                         D.UnitName,
                                                         A.InQTY QTY,
                                                         A.InQTY - ISNULL (C.ReturnQTY, 0) ReturnQTY,
                                                         A.InQTY - ISNULL (C.ReturnQTY, 0) MaxReturnQTY,
                                                         A.ReserveDeliveryTime,
                                                         A.CreatedTime,
                                                         A.StockId,
                                                         A.GoodsLocationId
                                                  FROM PoInOrderDetail A
                                                       JOIN PoInOrder B
                                                          ON     A.OrderId = B.ID
                                                             AND B.IsDeleted = 'false'
                                                             AND B.IsActive = 'true'
                                                             AND B.AuditStatus = 'CompleteIn'
                                                             AND B.SupplierId = '{2}'
                                                       LEFT JOIN
                                                       (SELECT SUM (A.ReturnQTY) ReturnQTY,
                                                               A.SourceOrderDetailId
                                                        FROM PoReturnOrderDetail A
                                                             JOIN PoReturnOrder B
                                                                ON     A.OrderId = B.ID
                                                                   AND B.IsActive = 'true'
                                                                   AND B.IsDeleted = 'false'
                                                        WHERE     A.IsActive = 'true'
                                                              AND B.IsDeleted = 'false'
                                                              AND A.ReturnQTY > 0
                                                              AND B.SupplierId = '{2}'
                                                        GROUP BY A.SourceOrderDetailId) C
                                                          ON A.ID = C.SourceOrderDetailId
                                                       LEFT JOIN BdMaterial_V D ON A.MaterialId = D.ID
                                                  WHERE A.IsDeleted = 'false' AND A.IsActive = 'true') A
                                            WHERE A.ReturnQTY > 0) B) C
                               WHERE ROWNUM <= {1} AND ROWNUM > {0}";
                    sql = string.Format(sql, startIndex, endIndex, ReturnOrder.SupplierId);
                    obj.data = DBHelper.Instance.QueryList<POReturnOrderDetailExtend>(sql);

                    string countString = @"SELECT COUNT (0)
                                                FROM (SELECT A.InQTY - ISNULL (C.ReturnQTY, 0) ReturnQTY
                                                      FROM PoInOrderDetail A
                                                           JOIN PoInOrder B
                                                              ON     A.OrderId = B.ID
                                                                 AND B.IsDeleted = 'false'
                                                                 AND B.IsActive = 'true'
                                                                 AND B.AuditStatus = 'CompleteIn'
                                                                 AND B.SupplierId = '{0}'
                                                           LEFT JOIN
                                                           (SELECT SUM (A.ReturnQTY) ReturnQTY, A.SourceOrderDetailId
                                                            FROM PoReturnOrderDetail A
                                                                 JOIN PoReturnOrder B
                                                                    ON     A.OrderId = B.ID
                                                                       AND B.IsActive = 'true'
                                                                       AND B.IsDeleted = 'false'
                                                            WHERE     A.IsActive = 'true'
                                                                  AND B.IsDeleted = 'false'
                                                                  AND A.ReturnQTY > 0
                                                                  AND B.SupplierId = '{0}'
                                                            GROUP BY A.SourceOrderDetailId) C
                                                              ON A.ID = C.SourceOrderDetailId
                                                           LEFT JOIN BdMaterial_V D ON A.MaterialId = D.ID
                                                      WHERE A.IsDeleted = 'false' AND A.IsActive = 'true') A
                                                WHERE A.ReturnQTY > 0";
                    countString = string.Format(countString, ReturnOrder.SupplierId);
                    total = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(countString));
                }
                else
                {
                    sql = @"SELECT *
                                FROM (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) ROWNUM
                                      FROM (SELECT *
                                            FROM (SELECT A.SerialNumber,
                                                         'ArrivalOrder' OrderSource,
                                                         B.ID SourceOrderId,
                                                         B.OrderNo SourceOrderNo,
                                                         A.ID SourceOrderDetailId,
                                                         D.MaterialNo,
                                                         D.MaterialName,
                                                         D.Specifications,
                                                         D.Description,
                                                         D.UnitName,
                                                         A.ArrivalQTY QTY,
                                                         A.ArrivalQTY - ISNULL (C.ReturnQTY, 0) ReturnQTY,
                                                         A.ArrivalQTY - ISNULL (C.ReturnQTY, 0) MaxReturnQTY,
                                                         A.ReserveDeliveryTime,
                                                         A.CreatedTime
                                                  FROM PoArrivalOrderDetail A
                                                       JOIN PoArrivalOrder B
                                                          ON     A.OrderId = B.ID
                                                             AND B.IsDeleted = 'false'
                                                             AND B.IsActive = 'true'
                                                             AND B.AuditStatus = 'CompleteAudit'
                                                             AND B.SupplierId = '{2}'
                                                       LEFT JOIN
                                                       (SELECT SUM (A.ReturnQTY) ReturnQTY,
                                                               A.SourceOrderDetailId
                                                        FROM PoReturnOrderDetail A
                                                             JOIN PoReturnOrder B
                                                                ON     A.OrderId = B.ID
                                                                   AND B.IsActive = 'true'
                                                                   AND B.IsDeleted = 'false'
                                                        WHERE     A.IsActive = 'true'
                                                              AND B.IsDeleted = 'false'
                                                              AND A.ReturnQTY > 0
                                                              AND B.SupplierId = '{2}'
                                                        GROUP BY A.SourceOrderDetailId) C
                                                          ON A.ID = C.SourceOrderDetailId
                                                       LEFT JOIN BdMaterial_V D ON A.MaterialId = D.ID
                                                  WHERE A.IsDeleted = 'false' AND A.IsActive = 'true') A
                                            WHERE A.ReturnQTY > 0) B) C
                                WHERE ROWNUM <= {1} AND ROWNUM > {0}";
                    sql = string.Format(sql, startIndex, endIndex, ReturnOrder.SupplierId);
                    obj.data = DBHelper.Instance.QueryList<POReturnOrderDetailExtend>(sql);
                    string countString = @"SELECT COUNT (1)
                                                FROM (SELECT A.ArrivalQTY - ISNULL (C.ReturnQTY, 0) ReturnQTY
                                                      FROM PoArrivalOrderDetail A
                                                           JOIN PoArrivalOrder B
                                                              ON     A.OrderId = B.ID
                                                                 AND B.IsDeleted = 'false'
                                                                 AND B.IsActive = 'true'
                                                                 AND B.AuditStatus = 'CompleteAudit'
                                                                 AND B.SupplierId = '{0}'
                                                           LEFT JOIN
                                                           (SELECT SUM (A.ReturnQTY) ReturnQTY, A.SourceOrderDetailId
                                                            FROM PoReturnOrderDetail A
                                                                 JOIN PoReturnOrder B
                                                                    ON     A.OrderId = B.ID
                                                                       AND B.IsActive = 'true'
                                                                       AND B.IsDeleted = 'false'
                                                            WHERE     A.IsActive = 'true'
                                                                  AND B.IsDeleted = 'false'
                                                                  AND A.ReturnQTY > 0
                                                                  AND B.SupplierId = '{0}'
                                                            GROUP BY A.SourceOrderDetailId) C
                                                              ON A.ID = C.SourceOrderDetailId
                                                           LEFT JOIN BdMaterial_V D ON A.MaterialId = D.ID
                                                      WHERE A.IsDeleted = 'false' AND A.IsActive = 'true') A
                                                WHERE A.ReturnQTY > 0";
                    countString = string.Format(countString, ReturnOrder.SupplierId);
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
