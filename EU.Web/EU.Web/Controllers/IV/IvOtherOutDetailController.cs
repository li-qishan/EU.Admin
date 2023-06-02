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

namespace EU.Web.Controllers.IV
{
    /// <summary>
    /// 其他入库单明细
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.IV)]
    public class IvOtherOutDetailController : BaseController<IvOtherOutDetail>
    {
        /// <summary>
        /// 其他入库单明细
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public IvOtherOutDetailController(DataContext _context, IBaseCRUDVM<IvOtherOutDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Add(IvOtherOutDetail Model)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
                #endregion

                Model.OutTime = Utility.GetSysDate();
                Model.SerialNumber = Utility.GenerateContinuousSequence("IvOtherOutDetail", "SerialNumber", "OrderId", Model.OrderId.ToString());
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
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult BatchAdd(List<IvOtherOutDetail> data)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Guid? OrderId = data[0].OrderId;

                for (int i = 0; i < data.Count; i++)
                {
                    data[i].ID = Guid.NewGuid();
                    DoAddPrepare(data[i]);
                    data[i].CreatedBy = new Guid(User.Identity.Name);
                    data[i].OutTime = Utility.GetSysDate();
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

        #region 批量更新排序号
        /// <summary>
        /// 批量更新排序号
        /// </summary>
        /// <param name="orderId">订单ID</param>
        private void BatchUpdateSerialNumber(string orderId)
        {
            string sql = @"UPDATE A
                        SET A.SerialNumber = C.NUM
                        FROM IvOtherOutDetail A
                             JOIN
                             (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) NUM
                              FROM (SELECT *
                                    FROM (SELECT A.*
                                          FROM IvOtherOutDetail A
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

                IvOtherOutDetail Model = _context.IvOtherOutDetail.Where(x => x.ID == Id).SingleOrDefault();
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
        /// 删除
        /// </summary>
        /// <param name="entryList"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult BatchDelete(List<IvOtherOutDetail> entryList)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {

                for (int i = 0; i < entryList.Count; i++)
                {
                    DbUpdate du = new DbUpdate("IvOtherOutDetail");
                    du.Set("IsDeleted", "true");
                    du.Where("ID", "=", entryList[i].ID);
                    DBHelper.Instance.ExecuteScalar(du.GetSql());
                }

                IvOtherOutDetail Model = _context.IvOtherOutDetail.Where(x => x.ID == entryList[0].ID).SingleOrDefault();
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
    }
}
