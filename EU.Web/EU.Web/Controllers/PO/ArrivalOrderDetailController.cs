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
using static EU.Core.Const.Consts;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.PO
{
    /// <summary>
    /// 采购到货通知单明细
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PO)]
    public class ArrivalOrderDetailController : BaseController<ArrivalOrderDetail>
    {
        /// <summary>
        /// 采购到货通知单明细
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public ArrivalOrderDetailController(DataContext _context, IBaseCRUDVM<ArrivalOrderDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Add(ArrivalOrderDetail Model)
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

                Model.SerialNumber = Utility.GenerateContinuousSequence("PoArrivalOrderDetail", "SerialNumber", "OrderId", Model.OrderId.ToString());

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
        public override IActionResult BatchAdd(List<ArrivalOrderDetail> list)
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
                    //POOrder order = _context.PoOrder.Where(x => x.ID == Guid.Parse(OrderId)).SingleOrDefault();
                    //Supplier supplier = _context.BdSupplier.Where(x => x.ID == Guid.Parse(OrderId)).SingleOrDefault();

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
                var ArrivalOrderDetail = _context.PoArrivalOrderDetail.Where(x => x.ID == Guid.Parse(id)).SingleOrDefault();
                if (ArrivalOrderDetail == null)
                    throw new Exception("无效的数据ID！");

                decimal NewArrivalQTY = modelModify.ArrivalQTY.Value;
                decimal ArrivalQTY = ArrivalOrderDetail.ArrivalQTY;

                if (NewArrivalQTY > ArrivalQTY)
                {
                    Guid? SourceOrderDetailId = ArrivalOrderDetail.SourceOrderDetailId;
                    var ArrivalOrder = _context.PoArrivalOrder.Where(x => x.ID == ArrivalOrderDetail.OrderId).SingleOrDefault();
                    string sql = @"SELECT A.PurchaseQTY - ISNULL (D.ArrivalQTY, 0) ArrivalQTY
                                        FROM PoOrderDetail A
                                             JOIN PoOrder B
                                                ON     A.OrderId = B.ID
                                                   AND B.IsDeleted = 'false'
                                                   AND B.IsActive = 'true'
                                                   AND B.SupplierId = '{1}'
                                                   AND B.AuditStatus = 'CompleteAudit'
                                             LEFT JOIN
                                             (SELECT SUM (A.ArrivalQTY) ArrivalQTY, A.SourceOrderDetailId
                                              FROM PoArrivalOrderDetail A
                                                   JOIN PoArrivalOrder B
                                                      ON     A.OrderId = B.ID
                                                         AND B.IsActive = 'true'
                                                         AND B.IsDeleted = 'false'
                                              WHERE A.IsActive = 'true' AND B.IsDeleted = 'false'
                                              GROUP BY A.SourceOrderDetailId) D
                                                ON A.ID = D.SourceOrderDetailId
                                        WHERE A.IsDeleted = 'false' AND A.IsActive = 'true' AND A.ID = '{0}'";
                    sql = string.Format(sql, SourceOrderDetailId, ArrivalOrder.SupplierId);
                    decimal QTY = Convert.ToDecimal(DBHelper.Instance.ExecuteScalar(sql));
                    if (QTY < (NewArrivalQTY - ArrivalQTY))
                        throw new Exception("待到货数量不足，当前待到货:" + QTY + "！");
                }

                DbUpdate du = new DbUpdate("PoArrivalOrderDetail", "ID", id);
                du.Set("ArrivalQTY", NewArrivalQTY);
                du.Set("ReserveDeliveryTime", modelModify.ReserveDeliveryTime.Value);
                DBHelper.Instance.ExecuteScalar(du.GetSql());

                //Update<ArrivalOrderDetail>(modelModify);
                //_context.SaveChanges();

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
                        FROM PoArrivalOrderDetail A
                             JOIN
                             (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) NUM
                              FROM (SELECT *
                                    FROM (SELECT A.*
                                          FROM PoArrivalOrderDetail A
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

                //ArrivalOrderDetail Model = _context.PoArrivalOrderDetail.Where(x => x.ID == Id).SingleOrDefault();
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
        #endregion
    }
}
