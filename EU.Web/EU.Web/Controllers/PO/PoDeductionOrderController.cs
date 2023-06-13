using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.Core;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.PO
{
    /// <summary>
    /// 采购扣款单
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PO)]
    public class PoDeductionOrderController : BaseController1<PoDeductionOrder>
    {
        /// <summary>
        /// 采购扣款单
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public PoDeductionOrderController(DataContext _context, IBaseCRUDVM<PoDeductionOrder> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Add(PoDeductionOrder Model)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
                //#endregion 
                Model.OrderNo = Utility.GenerateContinuousSequence("PoDeductionOrderNo");
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

        #region 获取详情
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id">数据ID</param>
        /// <returns></returns>
        [HttpGet]
        public override async Task<IActionResult> GetById(Guid Id)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                PoDeductionOrder order = _BaseCrud.GetById(Id);
                Supplier supplier = null;
                if (order != null)
                    supplier = _context.BdSupplier.FirstOrDefault(o => o.ID == order.SupplierId);
                obj.data = new
                {
                    ID = order.ID,
                    CreatedBy = order.CreatedBy,
                    CreatedTime = order.CreatedTime,
                    UpdateBy = order.UpdateBy,
                    UpdateTime = order.UpdateTime,
                    OrderNo = order.OrderNo,
                    OrderDate = order.OrderDate,
                    UserId = order.UserId,
                    SupplierId = order.SupplierId,
                    TaxRate = supplier?.TaxRate
                };
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

        #region 审核
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="modelModify"></param>
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
                    sql = @"SELECT A.ID
                                FROM PoArrivalOrderDetail A
                                WHERE     A.SourceOrderId = '{0}'
                                      AND A.IsDeleted = 'false'
                                      AND A.IsActive = 'true'
                                UNION
                                SELECT A.ID
                                FROM PoInOrderDetail A
                                WHERE     A.SourceOrderId = '{0}'
                                      AND A.IsDeleted = 'false'
                                      AND A.OrderSource = 'PoDeductionOrder'
                                      AND A.IsActive = 'true'";
                    sql = string.Format(sql, orderId);
                    DataTable dt = DBHelper.Instance.GetDataTable(sql);
                    #endregion

                    if (dt.Rows.Count == 0)
                        auditStatus = "Add";
                    else throw new Exception("该单据已被引用，不可撤销！");
                }

                #endregion

                DbUpdate du = new DbUpdate("PoDeductionOrder");
                du.Set("AuditStatus", auditStatus);
                du.Where("ID", "=", orderId);
                DBHelper.Instance.ExecuteScalar(du.GetSql());

                #region 导入订单操作历史
                DBHelper.RecordOperateLog(User.Identity.Name, "PO_ORDER_MNG", "PoDeductionOrder", orderId, OperateType.Update, "Audit", "修改订单审核状态为：" + auditStatus);
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
                var Order = _context.PoDeductionOrder.Where(x => x.ID == Id).SingleOrDefault();

                if (Order == null)
                    throw new Exception("无效的数据ID！");

                if (Order.AuditStatus != "Add")
                    throw new Exception("该单据已审核通过，暂不可进行删除操作！");

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

        #region 获取采购扣款单订单
        /// <summary>
        /// 获取采购扣款单订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual IActionResult GetFeeSourceList(string paramData, string masterId, string Source)
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

                //var PoDeductionOrder = _context.PoDeductionOrder.Where(x => x.ID == Guid.Parse(masterId))
                //    .Join(_context.BdSupplier, a => a, b => b, (a, b) => new { a, b }).Where(y => y.b.IsDeleted == false);

                var PoDeductionOrder = _context.PoDeductionOrder.Where(x => x.ID == Guid.Parse(masterId))
                    .Join(_context.BdSupplier, x => x.SupplierId, y => y.ID, (x, y) => new { x, y })
                    .Select(o => new { o.x.SupplierId, o.y.TaxRate }).FirstOrDefault();

                Guid? supplierId = PoDeductionOrder.SupplierId;
                decimal? taxRate = PoDeductionOrder.TaxRate;

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

                if (Source == "Material")
                {
                    sql = @"SELECT *
                                FROM (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) ROWNUM
                                      FROM (SELECT ID,
                                                   A.CreatedTime,
                                                   'Material' OrderSource,
                                                   NULL SourceOrderId,
                                                   NULL SourceOrderNo,
                                                   NULL SourceOrderDetailId,
                                                   A.ID MaterialId,
                                                   A.MaterialNo,
                                                   A.MaterialName,
                                                   A.Specifications,
                                                   A.UnitName,
                                                   A.Description,
                                                   1  QTY,
                                                   1  Price,
                                                  '{2}' TaxRate,
                                                   0 NoTaxAmount,
                                                   0 TaxAmount,
                                                   0 TaxIncludedAmount,
                                                   NULL Reason
                                            FROM BdMaterial_V A
                                            WHERE A.IsActive = 'true' AND A.IsDeleted = 'false') A) B
                               WHERE ROWNUM <= {1} AND ROWNUM > {0}";
                    sql = string.Format(sql, startIndex, endIndex, taxRate);
                    obj.data = DBHelper.Instance.QueryList<PoFeeDetailExtend>(sql);

                    string countString = @"SELECT COUNT (0) FROM BdMaterial_V A WHERE A.IsActive = 'true' AND A.IsDeleted = 'false'";
                    countString = string.Format(countString);
                    total = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(countString));
                }
                else if (Source == "POOrder")
                {
                    sql = @"SELECT *
                                FROM (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) ROWNUM
                                      FROM (SELECT A.ID,
                                                   A.CreatedTime,
                                                   'POOrder' OrderSource,
                                                   A.OrderId SourceOrderId,
                                                   B.OrderNo SourceOrderNo,
                                                   A.ID SourceOrderDetailId,
                                                   A.MaterialId,
                                                   D.MaterialNo,
                                                   D.MaterialName,
                                                   D.Specifications,
                                                   D.UnitName,
                                                   D.Description,
                                                   A.PurchaseQTY QTY,
                                                   A.Price,
                                                   '{3}' TaxRate,
                                                   A.NoTaxAmount,
                                                   A.TaxAmount,
                                                   A.TaxIncludedAmount,
                                                   NULL Reason
                                            FROM PoOrderDetail A
                                                 JOIN PoOrder B
                                                    ON     A.OrderId = B.ID
                                                       AND B.IsDeleted = 'false'
                                                       AND B.IsActive = 'true'
                                                       AND B.AuditStatus = 'CompleteAudit'
                                                 AND B.SupplierId = '{2}'
                                                 LEFT JOIN BdMaterial_V D ON A.MaterialId = D.ID
                                            WHERE     A.IsDeleted = 'false'
                                                  AND A.IsActive = 'true'
                                                  AND A.ID NOT IN
                                                         (SELECT A.SourceOrderDetailId
                                                          FROM PoFeeDetail A
                                                               JOIN PoDeductionOrder B
                                                                  ON     A.OrderId = B.ID
                                                                     AND B.IsActive = 'true'
                                                                     AND B.IsDeleted = 'false'
                                                          WHERE     A.IsActive = 'true'
                                                                AND B.IsDeleted = 'false'
                                                                AND A.SourceOrderDetailId IS NOT NULL
                                                                 AND B.SupplierId ='{2}'
                                                                                                  )) A) B
                                   WHERE ROWNUM <= {1} AND ROWNUM > {0}";
                    sql = string.Format(sql, startIndex, endIndex, supplierId, taxRate);
                    obj.data = DBHelper.Instance.QueryList<PoFeeDetailExtend>(sql);
                    string countString = @"SELECT COUNT (0)
                                                FROM PoOrderDetail A
                                                     JOIN PoOrder B
                                                        ON     A.OrderId = B.ID
                                                           AND B.IsDeleted = 'false'
                                                           AND B.IsActive = 'true'
                                                           AND B.AuditStatus = 'CompleteAudit'
                                                           AND B.SupplierId = '{0}'
                                                WHERE     A.IsDeleted = 'false'
                                                      AND A.IsActive = 'true'
                                                      AND A.ID NOT IN
                                                             (SELECT A.SourceOrderDetailId
                                                              FROM PoFeeDetail A
                                                                   JOIN PoDeductionOrder B
                                                                      ON     A.OrderId = B.ID
                                                                         AND B.IsActive = 'true'
                                                                         AND B.IsDeleted = 'false'
                                                              WHERE     A.IsActive = 'true'
                                                                    AND B.IsDeleted = 'false'
                                                                    AND A.SourceOrderDetailId IS NOT NULL
                                                                    AND B.SupplierId = '{0}')";
                    countString = string.Format(countString, supplierId);
                    total = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(countString));
                }
                else
                {
                    sql = @"SELECT *
                                FROM (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) ROWNUM
                                      FROM (SELECT A.SerialNumber,
                                                   'POInOrder' OrderSource,
                                                   B.ID SourceOrderId,
                                                   B.OrderNo SourceOrderNo,
                                                   A.ID SourceOrderDetailId,
                                                   D.MaterialNo,
                                                   D.MaterialName,
                                                   D.Specifications,
                                                   D.Description,
                                                   D.UnitName,
                                                   A.InQTY QTY,
                                                   NULL Price,
                                                   '{3}' TaxRate,
                                                   0 NoTaxAmount,
                                                   0 TaxAmount,
                                                   0 TaxIncludedAmount,
                                                   --  A.ArrivalQTY - ISNULL (C.ReturnQTY, 0) ReturnQTY,
                                                   --   A.ArrivalQTY - ISNULL (C.ReturnQTY, 0) MaxReturnQTY,
                                                   NULL Reason,
                                                   A.CreatedTime
                                            FROM PoInOrderDetail A
                                                 JOIN PoInOrder B
                                                    ON     A.OrderId = B.ID
                                                       AND B.IsDeleted = 'false'
                                                       AND B.IsActive = 'true'
                                                       AND B.AuditStatus = 'CompleteAudit'
                                                       AND B.SupplierId = '{2}'
                                                 LEFT JOIN BdMaterial_V D ON A.MaterialId = D.ID
                                            WHERE     A.IsDeleted = 'false'
                                                  AND A.IsActive = 'true'
                                                  AND A.ID NOT IN
                                                         (SELECT A.SourceOrderDetailId
                                                          FROM PoFeeDetail A
                                                               JOIN PoDeductionOrder B
                                                                  ON     A.OrderId = B.ID
                                                                     AND B.IsActive = 'true'
                                                                     AND B.IsDeleted = 'false'
                                                          WHERE     A.IsActive = 'true'
                                                                AND B.IsDeleted = 'false'
                                                                AND A.SourceOrderDetailId IS NOT NULL
                                                                AND A.OrderSource = 'POInOrder'
                                                                AND B.SupplierId = '{2}')) B) C
                                   WHERE ROWNUM <= {1} AND ROWNUM > {0}";
                    sql = string.Format(sql, startIndex, endIndex, supplierId, taxRate);
                    obj.data = DBHelper.Instance.QueryList<PoFeeDetailExtend>(sql);
                    string countString = @"SELECT COUNT (1)
                                                FROM PoInOrderDetail A
                                                     JOIN PoInOrder B
                                                        ON     A.OrderId = B.ID
                                                           AND B.IsDeleted = 'false'
                                                           AND B.IsActive = 'true'
                                                           AND B.AuditStatus = 'CompleteAudit'
                                                           AND B.SupplierId = '{0}'
                                                WHERE     A.IsDeleted = 'false'
                                                      AND A.IsActive = 'true'
                                                      AND A.ID NOT IN
                                                             (SELECT A.SourceOrderDetailId
                                                              FROM PoFeeDetail A
                                                                   JOIN PoDeductionOrder B
                                                                      ON     A.OrderId = B.ID
                                                                         AND B.IsActive = 'true'
                                                                         AND B.IsDeleted = 'false'
                                                              WHERE     A.IsActive = 'true'
                                                                    AND B.IsDeleted = 'false'
                                                                    AND A.SourceOrderDetailId IS NOT NULL
                                                                    AND A.OrderSource = 'POInOrder'
                                                                    AND B.SupplierId = '{0}')";
                    countString = string.Format(countString, supplierId);
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
