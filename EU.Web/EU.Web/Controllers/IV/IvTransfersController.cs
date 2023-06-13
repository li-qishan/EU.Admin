using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using EU.Common;
using EU.Core;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.IV
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.IV)]
    public class IvTransfersController : BaseController1<IvTransfers>
    {

        public IvTransfersController(DataContext _context, IBaseCRUDVM<IvTransfers> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(IvTransfers Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
                //#endregion 

                //var supplier = _context.BdSupplier.Where(x => x.ID == Model.SupplierId).SingleOrDefault();
                //if (supplier != null)
                //{
                //    Model.SupplierName = supplier.FullName;
                //}
                Model.OrderNo = Utility.GenerateContinuousSequence("IvTransfersNo");
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

                    //#region 检查单据是否被引用
                    //sql = @"SELECT A.ID
                    //              FROM SdOutOrderDetail A
                    //              WHERE     A.SalesOrderId = '{0}'
                    //                    AND A.IsDeleted = 'false'
                    //                    AND A.IsActive = 'true'";
                    //sql = string.Format(sql, orderId);
                    //DataTable dt = DBHelper.Instance.GetDataTable(sql);
                    //#endregion

                    //if (dt.Rows.Count == 0)
                    auditStatus = "Add";
                    //else throw new Exception("该单据已被引用，不可撤销！");
                }

                #endregion

                DbUpdate du = new DbUpdate("IvTransfers");
                du.Set("AuditStatus", auditStatus);
                du.Where("ID", "=", orderId);
                DBHelper.Instance.ExecuteScalar(du.GetSql());

                #region 导入订单操作历史
                DBHelper.RecordOperateLog(User.Identity.Name, "IV_STOCK_TRANSFERS_MNG", "IvTransfers", orderId, OperateType.Update, "Audit", "修改订单审核状态为：" + auditStatus);
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
                var Order = _context.IvTransfers.Where(x => x.ID == Id).SingleOrDefault();

                if (Order == null)
                    throw new Exception("无效的数据ID！");

                if (Order.AuditStatus == "CompleteAudit")
                    throw new Exception("该单据已审核通过，暂不可进行删除操作！");

                if (Order.AuditStatus == "CompleteTransfers")
                    throw new Exception("该单据已完成调整，暂不可进行删除操作！");

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

        #region 确认调整
        /// <summary>
        /// 确认调整
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ConfirmTransfers(Guid Id)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            IDbTransaction trans = DBHelper.Instance.GetNewTransaction();

            try
            {

                IvTransfers Model = _context.IvTransfers.Where(x => x.ID == Id).SingleOrDefault();

                if (Model.AuditStatus == "CompleteTransfers")
                    throw new Exception("该单据已完成调整！");

                string sql = @"SELECT A.*,
                                   B.StockName OutStockName,
                                   D.MaterialName,
                                   B.GoodsLocationName OutGoodsLocationName,
                                   C.StockName InStockName,
                                   C.GoodsLocationName InGoodsLocationName
                            FROM IvTransfersDetail A
                                 LEFT JOIN BdGoodsLocation_V B ON A.OutGoodsLocationId = B.ID
                                 LEFT JOIN BdGoodsLocation_V C ON A.OutGoodsLocationId = C.ID
                                 LEFT JOIN BdMaterial_V D ON A.MaterialId = D.ID
                            WHERE     A.OrderId = '{0}'
                                  AND A.IsDeleted = 'false'
                                  AND A.IsActive = 'true'
                            ORDER BY A.SerialNumber ASC";
                sql = string.Format(sql, Id);
                List<IvTransfersDetailExtend> list = DBHelper.Instance.QueryList<IvTransfersDetailExtend>(sql);

                foreach (IvTransfersDetailExtend item in list)
                {
                    #region 扣除移出仓数量
                    decimal qty = IVChangeHelper.GetMaterialInventory(item.MaterialId, item.OutStockId, item.OutGoodsLocationId, trans);

                    #region 判断是否有效库存
                    if (qty < item.QTY)
                        throw new Exception("物料【" + item.MaterialName + "】在移出仓【" + item.OutStockName + "】-移出货位【" + item.OutGoodsLocationName + "】库存不足，当前仓库存为：【" + qty + "】");
                    #endregion

                    MaterialIVChange change = new MaterialIVChange();
                    change.CreatedBy = Guid.Parse(User.Identity.Name);
                    change.MaterialId = item.MaterialId;
                    change.StockId = item.OutStockId;
                    change.GoodsLocationId = item.OutGoodsLocationId;
                    change.BeforeQTY = qty;
                    change.QTY = item.QTY;
                    change.AfterQTY = qty - item.QTY;
                    change.ChangeType = IVChangeHelper.ChangeType.InventoryTransfers.ToString();
                    change.OrderId = item.OrderId;
                    change.OrderDetailId = item.ID;
                    IVChangeHelper.Add(change, trans);

                    sql = @"UPDATE BdMaterialInventory
                            SET QTY = QTY-'{3}'
                            WHERE MaterialId = '{0}'
                                  AND StockId = '{1}'
                                  AND GoodsLocationId = '{2}'";
                    sql = string.Format(sql, item.MaterialId, item.OutStockId, item.OutGoodsLocationId, item.QTY);
                    DBHelper.Instance.ExecuteDML(sql, null, null, trans);
                    #endregion

                    #region 增加移入仓数量

                    qty = IVChangeHelper.GetMaterialInventory(item.MaterialId, item.InStockId, item.InGoodsLocationId, trans);

                    change = new MaterialIVChange();
                    change.CreatedBy = Guid.Parse(User.Identity.Name);
                    change.MaterialId = item.MaterialId;
                    change.StockId = item.InStockId;
                    change.GoodsLocationId = item.InGoodsLocationId;
                    change.BeforeQTY = qty;
                    change.QTY = item.QTY;
                    change.AfterQTY = qty + item.QTY;
                    change.ChangeType = IVChangeHelper.ChangeType.InventoryTransfers.ToString();
                    change.OrderId = item.OrderId;
                    change.OrderDetailId = item.ID;
                    IVChangeHelper.Add(change, trans);

                    sql = @"UPDATE BdMaterialInventory
                            SET QTY = QTY+'{3}'
                            WHERE MaterialId = '{0}'
                                  AND StockId = '{1}'
                                  AND GoodsLocationId = '{2}'";

                    sql = string.Format(sql, item.MaterialId, item.InStockId, item.InGoodsLocationId, item.QTY);
                    DBHelper.Instance.ExecuteDML(sql, null, null, trans);
                    #endregion
                }

                #region 修改单据状态
                DbUpdate du = new DbUpdate("IvTransfers");
                du.Set("AuditStatus", "CompleteTransfers");
                du.Where("ID", "=", Id);
                DBHelper.Instance.ExecuteDML(du.GetSql(), null, null, trans);
                #endregion

                #region 导入订单操作历史
                DBHelper.RecordOperateLog(User.Identity.Name, "IV_STOCK_TRANSFERS_MNG", "IvTransfers", Id.ToString(), OperateType.Update, "Audit", "用户进行确认转换！");
                #endregion

                DBHelper.Instance.CommitTransaction(trans);

                status = "ok";
                message = "确认成功！";
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
    }
}
