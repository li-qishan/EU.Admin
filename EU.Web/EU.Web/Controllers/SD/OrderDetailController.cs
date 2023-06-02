using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using EU.Core;
using EU.Core.Enums;
using EU.Core.Module;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Domain.System;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static EU.Core.Const.Consts;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.SD
{
    /// <summary>
    /// 销售订单明细
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.SD)]
    public class OrderDetailController : BaseController<OrderDetail>
    {
        /// <summary>
        /// 销售订单明细
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public OrderDetailController(DataContext _context, IBaseCRUDVM<OrderDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Add(OrderDetail Model)
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

                var material = _context.BdMaterial.Where(x => x.ID == Model.MaterialId).SingleOrDefault();
                var order = _context.SdOrder.Where(x => x.ID == Model.OrderId).SingleOrDefault();
                Model.SerialNumber = Utility.GenerateContinuousSequence("SdOrderDetail", "SerialNumber", "OrderId", Model.OrderId.ToString());
                Model.MaterialName = material.MaterialNames;
                Model.MaterialUnitId = material.UnitId;
                Model.DeliveryrDate = order.DeliveryrDate;

                #region 税额计算
                Model = UpdataTaxAmount(order, Model);
                #endregion

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
        ///批量新增
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult BatchAdd(List<OrderDetail> data)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Guid? OrderId = data[0].OrderId;
                var order = _context.SdOrder.Where(x => x.ID == OrderId).SingleOrDefault();

                //var ShipOrder = _context.SdShipOrder.Where(x => x.ID == OrderId).SingleOrDefault();
                //List<OrderDetail> list = new List<OrderDetail>();
                //int i = 1;
                //foreach (OrderDetail item in data)
                //{
                //    OrderDetail Model = new OrderDetail();
                //    Model.ID = Guid.NewGuid();
                //    DoAddPrepare(Model);
                //    Model.OrderId = item.ShipOrderId;
                //    Model.SalesOrderId = item.SalesOrderId;
                //    Model.MaterialId = item.MaterialId;
                //    Model.MaterialName = item.MaterialName;
                //    Model.MaterialSpecifications = item.MaterialSpecifications;
                //    Model.UnitId = item.UnitId;
                //    Model.SerialNumber = i;
                //    Model.SalesOrderDetailId = item.ID;
                //    Model.NoShipQTY = item.UnfilledOrderyQTY;
                //    Model.ShipQTY = item.ShipQTY;
                //    Model.CustomerMaterialCode = item.CustomerMaterialCode;
                //    Model.StockId = item.StockId;
                //    Model.GoodsLocationId = item.GoodsLocationId;
                //    Model.DeliveryrDate = item.DeliveryrDate;
                //    Model.ShipDate = ShipOrder.ShipDate;
                //    if (item.ShipQTY > 0)
                //    {
                //        list.Add(Model);
                //        i++;
                //    }
                //}
                for (int i = 0; i < data.Count; i++)
                {
                    data[i].ID = Guid.NewGuid();
                    DoAddPrepare(data[i]);
                    data[i] = UpdataTaxAmount(order, data[i]);
                    data[i].CreatedBy = new Guid(User.Identity.Name);
                }

                if (data.Count > 0)
                    DBHelper.Instance.AddRange(data);

                BatchUpdateSerialNumber(OrderId.ToString());

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
                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", modelModify.ColorNo.Value, ModifyType.Edit, modelModify.ID.Value, "材质编号");
                //#endregion

                Update<OrderDetail>(modelModify);
                _context.SaveChanges();

                string ID = modelModify.ID;
                OrderDetail Model = _context.SdOrderDetail.Where(x => x.ID == Guid.Parse(ID)).SingleOrDefault();
                Model.IsActive = true;
                var material = _context.BdMaterial.Where(x => x.ID == Model.MaterialId).SingleOrDefault();
                var order = _context.SdOrder.Where(x => x.ID == Model.OrderId).SingleOrDefault();
                Model.MaterialName = material.MaterialNames;
                Model.MaterialUnitId = material.UnitId;
                Model.DeliveryrDate = order.DeliveryrDate;

                #region 税额计算
                Model = UpdataTaxAmount(order, Model);
                #endregion

                DbUpdate du = new DbUpdate("SdOrderDetail");
                du.Set("MaterialName", Model.MaterialName);
                du.Set("MaterialUnitId", Model.MaterialUnitId);
                du.Set("DeliveryrDate", Model.DeliveryrDate);
                du.Set("NoTaxAmount", Model.NoTaxAmount);
                du.Set("TaxAmount", Model.TaxAmount);
                du.Set("TaxIncludedAmount", Model.TaxIncludedAmount);
                du.Where("ID", "=", ID);
                DBHelper.Instance.ExecuteScalar(du.GetSql());
                //BatchUpdateSerialNumber(ID);

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
                        FROM SdOrderDetail A
                             JOIN
                             (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) NUM
                              FROM (SELECT *
                                    FROM (SELECT A.*
                                          FROM SdOrderDetail A
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
                _BaseCrud.DoDelete(Id);

                OrderDetail Model = _context.SdOrderDetail.Where(x => x.ID == Id).SingleOrDefault();
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

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entryList"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult BatchDelete(List<OrderDetail> entryList)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {

                for (int i = 0; i < entryList.Count; i++)
                {
                    DbUpdate du = new DbUpdate("SdOrderDetail");
                    du.Set("IsDeleted", "true");
                    du.Where("ID", "=", entryList[i].ID);
                    DBHelper.Instance.ExecuteScalar(du.GetSql());
                }

                OrderDetail Model = _context.SdOrderDetail.Where(x => x.ID == entryList[0].ID).SingleOrDefault();
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

        #region 修改税额
        /// <summary>
        /// 修改税额
        /// 比如税率13%情况下，客户按未税价计算，含税金额=单价x数量x1.13，未税金额=单价x数量
        /// 比如税率13 % 情况下，客户按含税价计算，含税金额 = 单价x数量，未税金额 = 单价x数量 / 1.13
        /// </summary>
        /// <param name="order"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static OrderDetail UpdataTaxAmount(Order order, OrderDetail Model)
        {
            if (order.TaxType == "ZeroTax" || order.TaxRate == 0)
            {
                Model.NoTaxAmount = Model.Price * Model.QTY;
                Model.TaxAmount = 0;
                Model.TaxIncludedAmount = Model.NoTaxAmount;
            }//未税
            else if (order.TaxType == "ExcludingTax")
            {
                Model.NoTaxAmount = Model.Price * Model.QTY;
                Model.TaxIncludedAmount = Model.NoTaxAmount * ((100 + order.TaxRate) / 100);
                Model.TaxAmount = Model.TaxIncludedAmount - Model.NoTaxAmount;
            }//含税
            else if (order.TaxType == "IncludingTax")
            {
                Model.TaxIncludedAmount = Model.Price * Model.QTY;
                Model.NoTaxAmount = Model.TaxIncludedAmount / ((100 + order.TaxRate) / 100);
                Model.TaxAmount = Model.TaxIncludedAmount - Model.NoTaxAmount;
            }
            return Model;
        }
        #endregion

        #region 获取物料当前库存
        [HttpPost]
        public IActionResult GetMaterialInventory(List<OrderDetailExtend> list)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                List<OrderDetailExtend> temlist = list;

                string queryCodition = " AND A.ID IN (";
                for (int i = 0; i < temlist.Count; i++)
                {
                    if (i == 0)
                        queryCodition += "'" + temlist[i].MaterialId + "'";
                    else
                        queryCodition += ",'" + temlist[i].MaterialId + "'";
                }
                queryCodition += ")";

                string sql = @"SELECT A.ID MaterialId,A.MaterialNo,
                                   A.MaterialNames MaterialName,
                                   A.Specifications MaterialSpecifications,
                                   A.UnitName,
                                   ISNULL (B.QTY, 0) OccupyQTY, A.UnitId MaterialUnitId
                            FROM BdMaterial_V A
                                 LEFT JOIN BdMaterialInventorySum_V B ON A.ID = B.MaterialId
                            WHERE 1 = 1";

                if (list.Count > 0)
                    sql += queryCodition;

                temlist = DBHelper.Instance.QueryList<OrderDetailExtend>(sql, null);
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < temlist.Count; j++)
                        if (list[i].MaterialId == temlist[j].MaterialId)
                        {
                            list[i].MaterialNo = temlist[j].MaterialNo;
                            list[i].MaterialName = temlist[j].MaterialName;
                            list[i].UnitName = temlist[j].UnitName;
                            list[i].OccupyQTY = temlist[j].OccupyQTY;
                            list[i].MaterialUnitId = temlist[j].MaterialUnitId;
                            list[i].CustomerMaterialCode = temlist[j].CustomerMaterialCode;
                        }
                }

                status = "ok";
                message = "查询成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.data = list;
            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }
        #endregion

    }
}
