using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
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
    /// 采购单
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PO)]
    public class POOrderController : BaseController1<POOrder>
    {
        /// <summary>
        /// 采购单
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public POOrderController(DataContext _context, IBaseCRUDVM<POOrder> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(POOrder Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
                //#endregion 
                var supplier = _context.BdSupplier.Where(x => x.ID == Model.SupplierId).SingleOrDefault();
                if (supplier != null)
                {
                    Model.SupplierName = supplier.FullName;
                    Model.TaxType = supplier.TaxType;
                    Model.TaxRate = supplier.TaxRate;
                    Model.DeliveryWayId = supplier.DeliveryWayId;
                    Model.SettlementWayId = supplier.SettlementWayId;
                }
                Model.OrderNo = Utility.GenerateContinuousSequence("PoOrderNo");
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

            try
            {

                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", modelModify.ColorNo.Value, ModifyType.Edit, modelModify.ID.Value, "材质编号");
                //#endregion

                string SupplierId = modelModify.SupplierId;
                var supplier = _context.BdSupplier.Where(x => x.ID == Guid.Parse(SupplierId)).SingleOrDefault();
                if (supplier != null)
                {
                    modelModify.SupplierName = supplier.FullName;
                    modelModify.TaxType = supplier.TaxType;
                    modelModify.TaxRate = supplier.TaxRate;
                    modelModify.DeliveryWayId = supplier.DeliveryWayId;
                    modelModify.SettlementWayId = supplier.SettlementWayId;
                }

                Update<POOrder>(modelModify);
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
            return Ok(obj);
        }
        #endregion

        #region 获取待到货订单
        /// <summary>
        /// 获取待到货订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual IActionResult GetWaitArrivalList(string paramData, string masterId)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            int current = 1;
            int pageSize = 10000;
            int total = 0;

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

                var ArrivalOrder = _context.PoArrivalOrder.Where(x => x.ID == Guid.Parse(masterId)).SingleOrDefault();


                int _pageSize = pageSize;
                //计算分页起始索引
                int startIndex = current > 1 ? (current - 1) * _pageSize : 0;

                //计算分页结束索引
                int endIndex = current * _pageSize;
                string keyWordCondition = string.Empty;
                if (!string.IsNullOrEmpty(keyWord))
                    keyWordCondition = " AND (C.MaterialNo LIKE '%" + keyWord + "%' " +
                        "OR C.MaterialName LIKE '%" + keyWord + "%'" +
                        "OR C.Description LIKE '%" + keyWord + "%'" +
                        "OR C.UnitName LIKE '%" + keyWord + "%'" +
                        "OR C.Specifications LIKE '%" + keyWord + "%'" +
                        "OR B.OrderNo LIKE '%" + keyWord + "%'" +
                        ")";

                string sql = @"SELECT *
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
                                                         A.PurchaseQTY - ISNULL (D.ArrivalQTY, 0) ArrivalQTY,
                                                         A.PurchaseQTY - ISNULL (D.ArrivalQTY, 0) MaxArrivalQTY,
                                                         A.PurchaseQTY,
                                                         A.ReserveDeliveryTime,
                                                         A.CreatedTime
                                                  FROM PoOrderDetail A
                                                       JOIN PoOrder B
                                                          ON     A.OrderId = B.ID
                                                             AND B.IsDeleted = 'false'
                                                             AND B.IsActive = 'true' AND B.SupplierId='{2}'
                                                       LEFT JOIN BdMaterial_V C ON A.MaterialId = C.ID
                                                       LEFT JOIN
                                                       (SELECT SUM (A.ArrivalQTY) ArrivalQTY, A.SourceOrderDetailId
                                                        FROM PoArrivalOrderDetail A
                                                             JOIN PoArrivalOrder B
                                                                ON     A.OrderId = B.ID
                                                                   AND B.IsActive = 'true'
                                                                   AND B.IsDeleted = 'false'
                                                        WHERE A.IsActive = 'true' AND A.IsDeleted = 'false'
                                                        GROUP BY A.SourceOrderDetailId) D
                                                          ON A.ID = D.SourceOrderDetailId
                                                  WHERE A.IsDeleted = 'false' AND A.IsActive = 'true' AND B.AuditStatus = 'CompleteAudit'{3}) A
                                            WHERE A.ArrivalQTY > 0) B)
                                     C
                                WHERE ROWNUM <= {1} AND ROWNUM > {0}";
                sql = string.Format(sql, startIndex, endIndex, ArrivalOrder.SupplierId, keyWordCondition);
                obj.data = DBHelper.Instance.QueryList<ArrivalOrderDetailExtend>(sql);

                string countString = @"SELECT COUNT (1)
                                        FROM (SELECT A.PurchaseQTY - ISNULL (D.ArrivalQTY, 0) ArrivalQTY
                                              FROM PoOrderDetail A
                                                   JOIN PoOrder B
                                                      ON     A.OrderId = B.ID
                                                         AND B.IsDeleted = 'false'
                                                         AND B.IsActive = 'true' AND B.SupplierId='{0}'
                                                   LEFT JOIN
                                                   (SELECT SUM (A.ArrivalQTY) ArrivalQTY, A.SourceOrderDetailId
                                                    FROM PoArrivalOrderDetail A
                                                         JOIN PoArrivalOrder B
                                                            ON     A.OrderId = B.ID
                                                               AND B.IsActive = 'true'
                                                               AND B.IsDeleted = 'false'
                                                    WHERE A.IsActive = 'true' AND A.IsDeleted = 'false'
                                                    GROUP BY A.SourceOrderDetailId) D
                                                      ON A.ID = D.SourceOrderDetailId
                                              WHERE A.IsDeleted = 'false' AND A.IsActive = 'true' AND B.AuditStatus = 'CompleteAudit'{1}) A
                                        WHERE A.ArrivalQTY > 0";
                countString = string.Format(countString, ArrivalOrder.SupplierId, keyWordCondition);
                total = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(countString));
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
                                  AND A.OrderSource = 'POOrder'
                                  AND A.IsActive = 'true'
                            UNION
                            SELECT A.ID
                            FROM ApPrepaidDetail A
                            WHERE     A.SourceOrderId = '{0}'
                                  AND A.IsDeleted = 'false'
                                  AND A.OrderSource = 'POOrder'
                                  AND A.IsActive = 'true'";
                    sql = string.Format(sql, orderId);
                    DataTable dt = DBHelper.Instance.GetDataTable(sql);
                    #endregion

                    if (dt.Rows.Count == 0)
                        auditStatus = "Add";
                    else throw new Exception("该单据已被引用，不可撤销！");
                }

                #endregion

                DbUpdate du = new DbUpdate("PoOrder");
                du.Set("AuditStatus", auditStatus);
                du.Where("ID", "=", orderId);
                DBHelper.Instance.ExecuteScalar(du.GetSql());

                #region 导入订单操作历史
                DBHelper.RecordOperateLog(User.Identity.Name, "PO_ORDER_MNG", "PoOrder", orderId, OperateType.Update, "Audit", "修改订单审核状态为：" + auditStatus);
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
                var Order = _context.PoOrder.Where(x => x.ID == Id).SingleOrDefault();

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

    }
}
