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
    /// <summary>
    /// 其他入库单
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.IV)]
    public class IvOtherOutController : BaseController1<IvOtherOut>
    {
        /// <summary>
        /// 其他入库单
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public IvOtherOutController(DataContext _context, IBaseCRUDVM<IvOtherOut> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Add(IvOtherOut Model)
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
                Model.OrderNo = Utility.GenerateContinuousSequence("IvOtherOutNo");
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

                DbUpdate du = new DbUpdate("IvOtherOut");
                du.Set("AuditStatus", auditStatus);
                du.Where("ID", "=", orderId);
                DBHelper.Instance.ExecuteScalar(du.GetSql());

                #region 导入订单操作历史
                DBHelper.RecordOperateLog(User.Identity.Name, "IV_STOCK_OTHER_OUT_MNG", "IvOtherOut", orderId, OperateType.Update, "Audit", "修改订单审核状态为：" + auditStatus);
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
                var Order = _context.IvOtherOut.Where(x => x.ID == Id).SingleOrDefault();

                if (Order == null)
                    throw new Exception("无效的数据ID！");

                if (Order.AuditStatus == "CompleteAudit")
                    throw new Exception("该单据已审核通过，暂不可进行删除操作！");

                if (Order.AuditStatus == "CompleteOut")
                    throw new Exception("该单据已完成出库，暂不可进行删除操作！");

                _BaseCrud.DoDelete(Id);

                #region 导入订单操作历史
                DBHelper.RecordOperateLog(User.Identity.Name, "IV_STOCK_OTHER_OUT_MNG", "IvOtherOut", Id.ToString(), OperateType.Delete, "IvOtherOutController.Delete", "删除数据");
                #endregion

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

        #region 确认出库
        /// <summary>
        /// 确认出库
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ConfirmOut(Guid Id)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            IDbTransaction trans = DBHelper.Instance.GetNewTransaction();

            try
            {

                IvOtherOut Model = _context.IvOtherOut.Where(x => x.ID == Id).SingleOrDefault();

                if (Model.AuditStatus == "CompleteOut")
                    throw new Exception("该单据已完成出库！");

                string sql = @"SELECT A.*
                                FROM IvOtherOutDetail A
                                WHERE     A.IsDeleted = 'false'
                                      AND A.OrderId = '{0}'
                                      AND A.IsActive = 'true'
                                      AND A.StockId IS NOT NULL
                                      AND A.GoodsLocationId IS NOT NULL
                                ORDER BY A.SerialNumber ASC";
                sql = string.Format(sql, Id);
                List<IvOtherInDetailExtend> list = DBHelper.Instance.QueryList<IvOtherInDetailExtend>(sql);

                foreach (IvOtherInDetailExtend item in list)
                {
                    decimal qty = IVChangeHelper.GetMaterialInventory(item.MaterialId, item.StockId, item.GoodsLocationId, trans);
                    if (qty < item.QTY)
                        throw new Exception("物料【" + item.MaterialName + "】在【" + item.StockName + "】-【" + item.GoodsLocationName + "】库存不足，当前仓库存为：【" + qty + "】");

                    MaterialIVChange change = new MaterialIVChange();
                    change.CreatedBy = Guid.Parse(User.Identity.Name);
                    change.MaterialId = item.MaterialId;
                    change.StockId = item.StockId;
                    change.GoodsLocationId = item.GoodsLocationId;
                    change.BeforeQTY = qty;
                    change.QTY = item.QTY;
                    change.AfterQTY = qty + item.QTY;
                    change.ChangeType = IVChangeHelper.ChangeType.InventoryOtherOut.ToString();
                    change.OrderId = item.OrderId;
                    change.OrderDetailId = item.ID;
                    IVChangeHelper.Add(change, trans);

                    sql = @"UPDATE BdMaterialInventory
                            SET QTY = QTY-'{3}'
                            WHERE MaterialId = '{0}'
                                  AND StockId = '{1}'
                                  AND GoodsLocationId = '{2}'";
                    sql = string.Format(sql, item.MaterialId, item.StockId, item.GoodsLocationId, item.QTY);
                    DBHelper.Instance.ExecuteDML(sql, null, null, trans);
                }

                DbUpdate du = new DbUpdate("IvOtherOut");
                du.Set("AuditStatus", "CompleteOut");
                du.Where("ID", "=", Id);
                DBHelper.Instance.ExecuteDML(du.GetSql(), null, null, trans);

                #region 导入订单操作历史
                DBHelper.RecordOperateLog(User.Identity.Name, "IV_STOCK_OTHER_OUT_MNG", "IvOtherOut", Id.ToString(), OperateType.Update, "Audit", "用户进行确认其他入库");
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
