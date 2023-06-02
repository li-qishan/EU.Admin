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
    /// 采购费用单明细
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PO)]
    public class PoFeeDetailController : BaseController<PoFeeDetail>
    {
        /// <summary>
        /// 采购费用单明细
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public PoFeeDetailController(DataContext _context, IBaseCRUDVM<PoFeeDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Add(PoFeeDetail Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
                #endregion
                //Model.PoFeeDetailNo = Utility.GenerateContinuousSequence("SdPoFeeDetailNo");
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
        public override IActionResult BatchAdd(List<PoFeeDetail> list)
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

                    list?.ForEach(o =>
                    {
                        o.ID = Guid.NewGuid();
                        o.CreatedBy = UserId;
                        o.CreatedTime = Utility.GetSysDate();
                        if (string.IsNullOrEmpty(o.OrderSource) && o.MaterialId != null)
                            o.OrderSource = "Material";
                        DoAddPrepare(o);
                    });

                    //for (int i = 0; i < list.Count; i++)
                    //{
                    //    list[i].ID = Guid.NewGuid();
                    //    list[i].CreatedBy = UserId;
                    //    if(string.IsNullOrEmpty())
                    //    DoAddPrepare(list[i]);
                    //}
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

        #region 批量更新排序号
        /// <summary>
        /// 批量更新排序号
        /// </summary>
        /// <param name="orderId">订单ID</param>
        private void BatchUpdateSerialNumber(string orderId)
        {
            string sql = @"UPDATE A
                        SET A.SerialNumber = C.NUM
                        FROM PoFeeDetail A
                             JOIN
                             (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) NUM
                              FROM (SELECT *
                                    FROM (SELECT A.*
                                          FROM PoFeeDetail A
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

                PoFeeDetail Model = _context.PoFeeDetail.Where(x => x.ID == Id).SingleOrDefault();
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
