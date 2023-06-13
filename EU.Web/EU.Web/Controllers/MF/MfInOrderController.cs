using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.Core;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static EU.Core.Const.Consts;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.MF
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.MF)]
    public class MfInOrderController : BaseController1<MfInOrder>
    {

        public MfInOrderController(DataContext _context, IBaseCRUDVM<MfInOrder> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(MfInOrder Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdColor", "ColorNo", Model.ColorNo, ModifyType.Add, null, "材质编号");
                //#endregion
                Model.OrderNo = Utility.GenerateContinuousSequence("MfInOrderNo");
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
        /// <param name="MfInOrderId"></param>
        /// <param name="auditStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AuditMfInOrder(dynamic modelModify)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            string MfInOrderId = modelModify.MfInOrderId;
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
                                  FROM SdOutMfInOrderDetail A
                                  WHERE     A.SalesMfInOrderId = '{0}'
                                        AND A.IsDeleted = 'false'
                                        AND A.IsActive = 'true'";
                    sql = string.Format(sql, MfInOrderId);
                    DataTable dt = DBHelper.Instance.GetDataTable(sql);
                    #endregion

                    if (dt.Rows.Count == 0)
                        auditStatus = "Add";
                    else throw new Exception("该单据已被引用，不可撤销！");
                }

                #endregion

                DbUpdate du = new DbUpdate("MfInOrder");
                du.Set("AuditStatus", auditStatus);
                du.Where("ID", "=", MfInOrderId);
                DBHelper.Instance.ExecuteScalar(du.GetSql());

                #region 导入订单操作历史
                DBHelper.RecordOperateLog(User.Identity.Name, "SD_SALES_MfInOrder_MNG", "MfInOrder", MfInOrderId, OperateType.Update, "Audit", "修改订单审核状态为：" + auditStatus);
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
                var MfInOrder = _context.MfInOrder.Where(x => x.ID == Id).SingleOrDefault();

                if (MfInOrder == null)
                    throw new Exception("无效的数据ID！");

                if (MfInOrder.AuditStatus == "CompleteAudit")
                    throw new Exception("该销售单已审核通过，暂不可进行删除操作！");

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

        #region 获取详情
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
                            FROM MfInOrderDetail A
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
    }
}
