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
using Newtonsoft.Json;
using static EU.Core.Const.Consts;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.PO
{
    /// <summary>
    /// 采购单明细
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PO)]
    public class POOrderDetailController : BaseController1<POOrderDetail>
    {
        /// <summary>
        /// 采购单明细
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public POOrderDetailController(DataContext _context, IBaseCRUDVM<POOrderDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Add(POOrderDetail Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
                #endregion
                //Model.POOrderDetailNo = Utility.GenerateContinuousSequence("SdPOOrderDetailNo");
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
        public override IActionResult BatchAdd(List<POOrderDetail> list)
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
                    POOrder order = _context.PoOrder.Where(x => x.ID == Guid.Parse(OrderId)).SingleOrDefault();

                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].ID = Guid.NewGuid();
                        list[i].CreatedTime = Utility.GetSysDate();
                        list[i].CreatedBy = UserId;
                        DoAddPrepare(list[i]);

                        #region 税额计算
                        //比如税率13%情况下，客户按未税价计算，含税金额=单价x数量x1.13，未税金额=单价x数量
                        //比如税率13 % 情况下，客户按含税价计算，含税金额 = 单价x数量，未税金额 = 单价x数量 / 1.13
                        //零税
                        if (order.TaxType == "ZeroTax" || order.TaxRate == 0)
                        {
                            list[i].NoTaxAmount = list[i].Price * list[i].PurchaseQTY;
                            list[i].TaxAmount = 0;
                            list[i].TaxIncludedAmount = list[i].NoTaxAmount;
                        }//未税
                        else if (order.TaxType == "ExcludingTax")
                        {
                            list[i].NoTaxAmount = list[i].Price * list[i].PurchaseQTY;
                            list[i].TaxIncludedAmount = list[i].NoTaxAmount / ((100 + order.TaxRate) / 100);
                            list[i].TaxAmount = list[i].TaxIncludedAmount - list[i].NoTaxAmount;
                        }//含税
                        else if (order.TaxType == "IncludingTax")
                        {
                            list[i].TaxIncludedAmount = list[i].Price * list[i].PurchaseQTY;
                            list[i].NoTaxAmount = list[i].TaxIncludedAmount / ((100 + order.TaxRate) / 100);
                            list[i].TaxAmount = list[i].TaxIncludedAmount - list[i].NoTaxAmount;
                        }
                        #endregion

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
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="modelModify"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Update(dynamic modelModify)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {

                string id = modelModify.ID.Value;
                var PoOrderDetail = _context.PoOrderDetail.Where(x => x.ID == Guid.Parse(id)).SingleOrDefault();
                if (PoOrderDetail == null)
                    throw new Exception("无效的数据ID！");

                PoOrderDetail.Price = Convert.ToDecimal(modelModify.Price.Value);
                PoOrderDetail.PurchaseQTY = Convert.ToDecimal(modelModify.PurchaseQTY.Value);

                POOrder order = _context.PoOrder.Where(x => x.ID == PoOrderDetail.OrderId).SingleOrDefault();

                #region 税额计算
                //比如税率13%情况下，客户按未税价计算，含税金额=单价x数量x1.13，未税金额=单价x数量
                //比如税率13 % 情况下，客户按含税价计算，含税金额 = 单价x数量，未税金额 = 单价x数量 / 1.13
                //零税
                if (order.TaxType == "ZeroTax" || order.TaxRate == 0)
                {
                    PoOrderDetail.NoTaxAmount = PoOrderDetail.Price * PoOrderDetail.PurchaseQTY;
                    PoOrderDetail.TaxAmount = 0;
                    PoOrderDetail.TaxIncludedAmount = PoOrderDetail.NoTaxAmount;
                }//未税
                else if (order.TaxType == "ExcludingTax")
                {
                    PoOrderDetail.NoTaxAmount = PoOrderDetail.Price * PoOrderDetail.PurchaseQTY;
                    PoOrderDetail.TaxIncludedAmount = PoOrderDetail.NoTaxAmount / ((100 + order.TaxRate) / 100);
                    PoOrderDetail.TaxAmount = PoOrderDetail.TaxIncludedAmount - PoOrderDetail.NoTaxAmount;
                }//含税
                else if (order.TaxType == "IncludingTax")
                {
                    PoOrderDetail.TaxIncludedAmount = PoOrderDetail.Price * PoOrderDetail.PurchaseQTY;
                    PoOrderDetail.NoTaxAmount = PoOrderDetail.TaxIncludedAmount / ((100 + order.TaxRate) / 100);
                    PoOrderDetail.TaxAmount = PoOrderDetail.TaxIncludedAmount - PoOrderDetail.NoTaxAmount;
                }
                #endregion

                _context.SaveChanges();
                //DbUpdate du = new DbUpdate("POOrderDetail", "ID", id);
                //du.Set("PurchaseQTY", PurchaseQTY);
                //du.Set("Price", Convert.ToDecimal(modelModify.Price.Value));
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
                        FROM PoOrderDetail A
                             JOIN
                             (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) NUM
                              FROM (SELECT *
                                    FROM (SELECT A.*
                                          FROM PoOrderDetail A
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

                POOrderDetail Model = _context.PoOrderDetail.Where(x => x.ID == Id).SingleOrDefault();
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

        #region 获取采购单订单列表
        /// <summary>
        /// 获取采购单订单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual IActionResult GetWaitPurchaseList(string paramData, string masterId, string Source)
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
                string keyWord = string.Empty;
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

                    if (item.Key == "keyWord")
                    {
                        keyWord = item.Value.ToString();
                        continue;
                    }
                }
                #endregion

                int _pageSize = pageSize;
                //计算分页起始索引
                int startIndex = current > 1 ? (current - 1) * _pageSize : 0;

                //计算分页结束索引
                int endIndex = current * _pageSize;
                string keyWordCondition = string.Empty;
                if (Source == "Sales")
                {
                    if (!string.IsNullOrEmpty(keyWord))
                        keyWordCondition = " AND (C.MaterialNo LIKE '%" + keyWord + "%' " +
                            "OR C.MaterialName LIKE '%" + keyWord + "%'" +
                            "OR C.Description LIKE '%" + keyWord + "%'" +
                            "OR C.UnitName LIKE '%" + keyWord + "%'" +
                            "OR C.Specifications LIKE '%" + keyWord + "%'" +
                            "OR B.OrderNo LIKE '%" + keyWord + "%'" +
                            ")";

                    sql = @"SELECT *
                                FROM (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) ROWNUM
                                      FROM (SELECT *
                                            FROM (SELECT A.ID,
                                                         A.ID SourceOrderDetailId,
                                                         'Sales' OrderSource,
                                                         A.OrderId SourceOrderId,
                                                         B.OrderNo SourceOrderNo,
                                                         A.CreatedTime,
                                                         A.SerialNumber,
                                                         A.MaterialId,
                                                         C.MaterialNo,
                                                         C.MaterialName,
                                                         C.Description,
                                                         C.UnitName,
                                                         C.Specifications,
                                                         C.UnitId,
                                                         A.Price,
                                                         A.QTY,
                                                         A.QTY - ISNULL (E.PurchaseQTY, 0) PurchaseQTY,
                                                         A.QTY - ISNULL (E.PurchaseQTY, 0) MaxPurchaseQTY,
                                                         A.NoTaxAmount,
                                                         A.TaxAmount,
                                                         A.TaxIncludedAmount,
                                                         A.DeliveryrDate ReserveDeliveryTime
                                                  FROM SdOrderDetail A
                                                       JOIN SdOrder B
                                                          ON A.OrderId = B.ID AND A.IsDeleted = B.IsDeleted
                                                       LEFT JOIN BdMaterial_V C ON A.MaterialId = C.ID
                                                       LEFT JOIN
                                                       (SELECT SUM (A.PurchaseQTY) PurchaseQTY,
                                                               A.SourceOrderDetailId
                                                        FROM PoOrderDetail A
                                                             JOIN PoOrder B
                                                                ON     A.OrderId = B.ID
                                                                   AND A.IsActive = B.IsActive
                                                                   AND A.IsDeleted = B.IsDeleted
                                                        WHERE A.IsDeleted = 'false' AND A.IsActive = 'true'
                                                        GROUP BY A.SourceOrderDetailId) E
                                                          ON A.ID = E.SourceOrderDetailId
                                                  WHERE     A.IsDeleted = 'false'
                                                        AND A.IsActive = 'true'
                                                        AND B.AuditStatus = 'CompleteAudit'{2}) A
                                            WHERE PurchaseQTY > 0) B) C
                                 WHERE ROWNUM <= {1} AND ROWNUM > {0}";
                    sql = string.Format(sql, startIndex, endIndex, keyWordCondition);
                    obj.data = DBHelper.Instance.QueryList<POOrderDetailExtend>(sql);

                    string countString = @"SELECT COUNT (1)
                                FROM (SELECT A.QTY - ISNULL (E.PurchaseQTY, 0) PurchaseQTY
                                      FROM SdOrderDetail A
                                           JOIN SdOrder B ON A.OrderId = B.ID AND A.IsDeleted = B.IsDeleted
                                           LEFT JOIN
                                           (SELECT SUM (A.PurchaseQTY) PurchaseQTY, A.SourceOrderDetailId
                                            FROM PoOrderDetail A
                                                 JOIN PoOrder B
                                                    ON     A.OrderId = B.ID
                                                       AND A.IsActive = B.IsActive
                                                       AND A.IsDeleted = B.IsDeleted
                                            WHERE A.IsDeleted = 'false' AND A.IsActive = 'true'
                                            GROUP BY A.SourceOrderDetailId) E
                                              ON A.ID = E.SourceOrderDetailId
                                      WHERE     A.IsDeleted = 'false'
                                            AND A.IsActive = 'true'
                                            AND B.AuditStatus = 'CompleteAudit'{0}) A
                                WHERE PurchaseQTY > 0";
                    countString = string.Format(countString, keyWordCondition);
                    total = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(countString));
                }
                else
                {

                    if (!string.IsNullOrEmpty(keyWord))
                        keyWordCondition = " AND (D.MaterialNo LIKE '%" + keyWord + "%' " +
                            "OR D.MaterialName LIKE '%" + keyWord + "%'" +
                            "OR D.Description LIKE '%" + keyWord + "%'" +
                            "OR D.UnitName LIKE '%" + keyWord + "%'" +
                            "OR D.Specifications LIKE '%" + keyWord + "%'" +
                            "OR B.OrderNo LIKE '%" + keyWord + "%'" +
                            ")";
                    sql = @"SELECT *
                                FROM (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) ROWNUM
                                      FROM (SELECT *
                                            FROM (SELECT *
                                                  FROM (SELECT A.ID,
                                                               A.ID SourceOrderDetailId,
                                                               'Requestion' OrderSource,
                                                               A.OrderId SourceOrderId,
                                                               B.OrderNo SourceOrderNo,
                                                               A.CreatedTime,
                                                               A.SerialNumber,
                                                               A.MaterialId,
                                                               A.QTY,
                                                               D.MaterialNo,
                                                               D.MaterialName,
                                                               D.Description,
                                                               D.UnitName,
                                                               D.Specifications,
                                                               D.UnitId,
                                                               '0' Price,
                                                               A.QTY - ISNULL (C.PurchaseQTY, 0) PurchaseQTY,
                                                               A.QTY - ISNULL (C.PurchaseQTY, 0) MaxPurchaseQTY,
                                                               '0' NoTaxAmount,
                                                               '0' TaxAmount,
                                                               '0' TaxIncludedAmount,
                                                               B.RequestionDate ReserveDeliveryTime,
                                                               B.AuditStatus
                                                        FROM PoRequestionDetail A
                                                             JOIN PoRequestion B
                                                                ON     A.OrderId = B.ID
                                                                   AND A.IsActive = B.IsActive
                                                                   AND A.IsDeleted = B.IsDeleted
                                                                   AND B.AuditStatus = 'CompleteAudit'
                                                             LEFT JOIN BdMaterial_V D ON A.MaterialId = D.ID
                                                             LEFT JOIN
                                                             (SELECT SUM (A.PurchaseQTY) PurchaseQTY,
                                                                     A.SourceOrderDetailId
                                                              FROM PoOrderDetail A
                                                                   JOIN PoOrder B
                                                                      ON     A.OrderId = B.ID
                                                                         AND A.IsActive = B.IsActive
                                                                         AND A.IsDeleted = B.IsDeleted
                                                              WHERE     A.IsDeleted = 'false'
                                                                    AND A.IsActive = 'true'
                                                                    AND B.IsDeleted = 'false'
                                                                    AND B.IsActive = 'true'
                                                              GROUP BY A.SourceOrderDetailId) C
                                                                ON A.ID = C.SourceOrderDetailId
                                                        WHERE A.IsDeleted = 'false' AND A.IsActive = 'true'{2})
                                                       A
                                                  WHERE A.PurchaseQTY > 0) A) B) C
                                  WHERE ROWNUM <= {1} AND ROWNUM > {0}";
                    sql = string.Format(sql, startIndex, endIndex, keyWordCondition);
                    obj.data = DBHelper.Instance.QueryList<POOrderDetailExtend>(sql);
                    string countString = @"SELECT COUNT (1)
                                        FROM (SELECT A.QTY - ISNULL (C.PurchaseQTY, 0) PurchaseQTY
                                              FROM PoRequestionDetail A
                                                   JOIN PoRequestion B
                                                      ON     A.OrderId = B.ID
                                                         AND A.IsActive = B.IsActive
                                                         AND A.IsDeleted = B.IsDeleted
                                                         AND B.AuditStatus = 'CompleteAudit'
                                                    LEFT JOIN BdMaterial_V D ON A.MaterialId = D.ID
                                                   LEFT JOIN
                                                   (SELECT SUM (A.PurchaseQTY) PurchaseQTY, A.SourceOrderDetailId
                                                    FROM PoOrderDetail A
                                                         JOIN PoOrder B
                                                            ON     A.OrderId = B.ID
                                                               AND A.IsActive = B.IsActive
                                                               AND A.IsDeleted = B.IsDeleted
                                                    WHERE     A.IsDeleted = 'false'
                                                          AND A.IsActive = 'true'
                                                          AND B.IsDeleted = 'false'
                                                          AND B.IsActive = 'true'
                                                    GROUP BY A.SourceOrderDetailId) C
                                                      ON A.ID = C.SourceOrderDetailId
                                              WHERE A.IsDeleted = 'false' AND A.IsActive = 'true'{0}) A
                                        WHERE A.PurchaseQTY > 0";
                    countString = string.Format(countString, keyWordCondition);
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
