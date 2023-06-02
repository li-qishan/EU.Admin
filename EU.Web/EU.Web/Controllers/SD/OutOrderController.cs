using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.Common;
using EU.Core;
using EU.Core.EFDbContext;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using static EU.Core.Const.Consts;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.SD
{
    /// <summary>
    /// 出库单
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.SD)]
    public class OutOrderController : BaseController<OutOrder>
    {
        /// <summary>
        /// 出库单
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public OutOrderController(DataContext _context, IBaseCRUDVM<OutOrder> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Add(OutOrder Model)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
                //#endregion

                Model.OrderNo = Utility.GenerateContinuousSequence("SdOutOrderNo");
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
                Update<OutOrder>(modelModify);
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

        #region 获取详情
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id">ID</param>
        /// <returns></returns>
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
                            FROM SdOutOrderDetail A
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

        #region 审核
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="modelModify">订单</param>
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
                                 LEFT JOIN SdOutOrderDetail B ON A.OutOrderId = B.ID
                            WHERE B.OrderId = '{0}' AND A.IsDeleted = 'false' AND A.IsActive = 'true'";
                    sql = string.Format(sql, orderId);
                    DataTable dt = DBHelper.Instance.GetDataTable(sql);
                    #endregion

                    if (dt.Rows.Count == 0)
                        auditStatus = "Add";
                    else throw new Exception("该单据已被引用，不可撤销！");
                }
                #endregion

                DbUpdate du = new DbUpdate("SdOutOrder");
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

        #region 获取货位
        /// <summary>
        /// 获取货位
        /// </summary>
        /// <param name="StockId">仓库ID</param>
        /// <param name="MaterialId">货位ID</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetLocationList(Guid StockId, Guid MaterialId)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            decimal InventoryQTY = 0;

            try
            {

                List<GoodsLocation> goodsLocationlist = _context.BdGoodsLocation.Where(x => x.IsDeleted == false && x.StockId == StockId).ToList();


                string sql = @"SELECT A.*
                            FROM BdMaterialInventory A
                            WHERE A.MaterialId = '{0}'";
                sql = string.Format(sql, MaterialId);
                if (goodsLocationlist.Count > 0)
                {
                    sql = @"SELECT A.*
                            FROM BdMaterialInventory A
                            WHERE A.MaterialId = '{0}' AND A.GoodsLocationId='{1}'";
                    sql = string.Format(sql, MaterialId, goodsLocationlist[0].ID);
                }
                List<BdMaterialInventory> list = DBHelper.Instance.QueryList<BdMaterialInventory>(sql);
                if (list.Count > 0)
                    InventoryQTY = list[0].QTY;

                obj.data = goodsLocationlist;
                status = "ok";
                message = "查询成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
            }
            obj.InventoryQTY = InventoryQTY;
            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }
        #endregion

        #region 确认出库
        /// <summary>
        /// 确认出库
        /// </summary>
        /// <param name="Id">出库单ID</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ConfirmOut(Guid Id)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            string sql = string.Empty;

            IDbTransaction trans = DBHelper.Instance.GetNewTransaction();

            try
            {

                Utility.IsNullOrEmpty(Id, "出库单ID不能为空！");

                #region 判断单据库存
                sql = @"SELECT *
                        FROM (SELECT A.OutQTY,
                                    -- A.SerialNumber,
                                     D.MaterialName,
                                     ISNULL (B.QTY, 0) QTY,
                                     C.GoodsLocationName,
                                     C.StockName
                              FROM (SELECT SUM (A.OutQTY) OutQTY,
                                           A.MaterialId,
                                           A.StockId,
                                           A.GoodsLocationId
                                    FROM SdOutOrderDetail A
                                    WHERE     A.IsDeleted = 'false'
                                          AND A.IsActive = 'true'
                                          AND A.OrderId = '{0}'
                                          AND A.GoodsLocationId IS NOT NULL
                                    GROUP BY A.MaterialId, A.StockId, A.GoodsLocationId) A
                                   LEFT JOIN BdMaterialInventory B
                                      ON     A.MaterialId = B.MaterialId
                                         AND A.StockId = B.StockId
                                         AND A.GoodsLocationId = B.GoodsLocationId
                                         AND B.IsDeleted = 'false'
                                   LEFT JOIN BdGoodsLocation_V C
                                      ON A.StockId = C.StockId AND A.GoodsLocationId = C.ID
                                       LEFT JOIN BdMaterial_V  D
                                      ON A.MaterialId = D.ID
                                      ) A
                        WHERE A.OutQTY > QTY";
                sql = string.Format(sql, Id);
                DataTable dt = DBHelper.Instance.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                    throw new Exception("出库单明细-物料【" + dt.Rows[0]["MaterialName"] + "】在【" + dt.Rows[0]["StockName"] + "】【" + dt.Rows[0]["GoodsLocationName"] + "】库存不足，暂无法完成出库！");
                #endregion

                sql = @"SELECT A.*
                            FROM SdOutOrderDetail A
                            WHERE     A.OrderId = '{0}'
                                  AND A.IsDeleted = 'false'
                                  AND A.IsActive = 'true'
                                  AND A.OutQTY > 0
                            ORDER BY A.SerialNumber ASC";
                sql = string.Format(sql, Id);
                List<OutOrderDetail> list = DBHelper.Instance.QueryList<OutOrderDetail>(sql);

                #region 更新订单状态
                DbUpdate du = new DbUpdate("SdOutOrder");
                du.Set("AuditStatus", "CompleteOut");
                du.Set("OutTime", Utility.GetSysDate());
                du.Where("ID", "=", Id);
                DBHelper.Instance.ExecuteDML(du.GetSql(), null, null, trans);
                #endregion

                #region 修改物料库存
                foreach (OutOrderDetail item in list)
                {
                    decimal qty = IVChangeHelper.GetMaterialInventory(item.MaterialId, item.StockId, item.GoodsLocationId, trans);

                    MaterialIVChange change = new MaterialIVChange();
                    change.CreatedBy = Guid.Parse(User.Identity.Name);
                    change.MaterialId = item.MaterialId;
                    change.StockId = item.StockId;
                    change.GoodsLocationId = item.GoodsLocationId;
                    change.BeforeQTY = qty;
                    change.QTY = item.OutQTY;
                    change.AfterQTY = qty - item.OutQTY;
                    change.ChangeType = IVChangeHelper.ChangeType.SalesOut.ToString();
                    change.OrderId = item.OrderId;
                    change.OrderDetailId = item.ID;
                    IVChangeHelper.Add(change, trans);

                    sql = @"UPDATE BdMaterialInventory
                            SET QTY = QTY - '{3}'
                            WHERE MaterialId = '{0}' AND StockId = '{1}' AND GoodsLocationId = '{2}';

                            UPDATE SdOrderDetail
                            SET DeliveryrQTY = DeliveryrQTY + '{3}'
                            WHERE ID = '{4}'";

                    sql = string.Format(sql, item.MaterialId, item.StockId, item.GoodsLocationId, item.OutQTY, item.SalesOrderDetailId);
                    DBHelper.Instance.ExecuteDML(sql, null, null, trans);
                }

                var salesOrderIds = list.Select(o => o.SalesOrderId).Distinct();
                foreach (Guid? salesOrderId in salesOrderIds)
                {

                    #region 修改销售单状态
                    string SalesOrderStatus = "AllShip";
                    var count = _context.SdOrderDetail.Where(o => o.OrderId == Id && o.QTY > o.DeliveryrQTY).LongCount();
                    if (count > 0)
                        SalesOrderStatus = "PartShip";

                    sql = @"UPDATE SdOrder
                            SET SalesOrderStatus = '{0}'
                            WHERE ID = '{1}'";

                    sql = string.Format(sql, SalesOrderStatus, salesOrderId);
                    DBHelper.Instance.ExecuteDML(sql, null, null, trans);
                    #endregion
                }

                //sql = @"UPDATE A
                //        SET A.QTY = (A.QTY - B.OutQTY)
                //        FROM BdMaterialInventory A
                //             JOIN
                //             (SELECT SUM (A.OutQTY) OutQTY,
                //                     A.MaterialId,
                //                     A.StockId,
                //                     A.GoodsLocationId
                //              FROM SdOutOrderDetail A
                //              WHERE     A.IsDeleted = 'false'
                //                    AND A.IsActive = 'true'
                //                    AND A.OrderId = '{0}'
                //                    AND A.GoodsLocationId IS NOT NULL
                //              GROUP BY A.MaterialId, A.StockId, A.GoodsLocationId) B
                //                ON     A.ID = B.MaterialId
                //                   AND A.StockId = B.StockId
                //                   AND A.GoodsLocationId = B.GoodsLocationId
                //        WHERE A.QTY > B.OutQTY AND A.IsDeleted = 'false' AND A.IsActive = 'true'";
                //sql = string.Format(sql, Id);
                //DBHelper.Instance.ExcuteNonQuery(sql);
                //DBHelper.Instance.ExecuteDML(sql, null, null, trans);
                #endregion



                DBHelper.Instance.CommitTransaction(trans);

                #region 导入订单操作历史
                DBHelper.RecordOperateLog(User.Identity.Name, "SD_OUT_ORDER_MNG", "SdOutOrder", Id.ToString(), OperateType.Update, "CompleteOut", "修改订单审核状态为：CompleteOut");
                #endregion

                status = "ok";
                message = "出库成功！";
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
                var Order = _context.SdOutOrder.Where(x => x.ID == Id).SingleOrDefault();

                if (Order == null)
                    throw new Exception("无效的数据ID！");

                if (Order.AuditStatus != "Add")
                    throw new Exception("该出库单已审核通过，暂不可进行删除操作！");

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
