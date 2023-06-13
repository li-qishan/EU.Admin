﻿using System;
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

namespace EU.Web.Controllers.IV
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.IV)]
    public class IvActualCheckDetailController : BaseController1<IvActualCheckDetail>
    {

        public IvActualCheckDetailController(DataContext _context, IBaseCRUDVM<IvActualCheckDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(IvActualCheckDetail Model)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
                #endregion

                Model.SerialNumber = Utility.GenerateContinuousSequence("IvActualCheckDetail", "SerialNumber", "OrderId", Model.OrderId.ToString());
                //Model.DiffQTY = Model.ActualQTY - Model.InventoryQTY;
                //Model.ProfitLoss = Model.DiffQTY > 0 ? "Profit" : "Loss";
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
        public override IActionResult BatchAdd(List<IvActualCheckDetail> data)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Guid? OrderId = data[0].OrderId;

                foreach (IvActualCheckDetail item in data)
                {
                    item.ID = Guid.NewGuid();
                    DoAddPrepare(item);
                    item.CreatedBy = new Guid(User.Identity.Name);
                    item.DiffQTY = item.ActualQTY - item.QTY;
                    item.ProfitLoss = "NoDiff";
                    if (item.DiffQTY > 0)
                        item.ProfitLoss = "Profit";
                    else if (item.DiffQTY < 0)
                        item.ProfitLoss = "Loss";
                }

                //for (int i = 0; i < data.Count; i++)
                //{
                //    data[i].ID = Guid.NewGuid();
                //    DoAddPrepare(data[i]);
                //    data[i].CreatedBy = new Guid(User.Identity.Name);
                //    //data[i].Amount = data[i].QTY * data[i].Price;
                //}

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

                decimal ActualQTY = Convert.ToDecimal(modelModify.ActualQTY);
                decimal QTY = Convert.ToDecimal(modelModify.QTY);
                decimal DiffQTY = ActualQTY - QTY;
                modelModify.DiffQTY = DiffQTY;
                modelModify.ProfitLoss = "NoDiff";

                if (DiffQTY > 0)
                    modelModify.ProfitLoss = "Profit";
                else if (DiffQTY < 0)
                    modelModify.ProfitLoss = "Loss";

                Update<IvActualCheckDetail>(modelModify);
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

        #region 批量更新排序号
        /// <summary>
        /// 批量更新排序号
        /// </summary>
        /// <param name="orderId">订单ID</param>
        private void BatchUpdateSerialNumber(string orderId)
        {
            string sql = @"UPDATE A
                        SET A.SerialNumber = C.NUM
                        FROM IvActualCheckDetail A
                             JOIN
                             (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) NUM
                              FROM (SELECT *
                                    FROM (SELECT A.*
                                          FROM IvActualCheckDetail A
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

                IvActualCheckDetail Model = _context.IvActualCheckDetail.Where(x => x.ID == Id).SingleOrDefault();
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
    }
}
