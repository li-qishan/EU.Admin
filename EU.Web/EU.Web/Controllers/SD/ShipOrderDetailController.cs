using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using EU.Core;
using EU.Core.Enums;
using EU.Core.Module;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Domain.System;
using EU.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static EU.Core.Const.Consts;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.SD
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.SD)]
    public class ShipOrderDetailController : BaseController1<ShipOrderDetail>
    {

        public ShipOrderDetailController(DataContext _context, IBaseCRUDVM<ShipOrderDetail> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(ShipOrderDetail Model)
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

                Model.SerialNumber = Utility.GenerateContinuousSequence("SdShipOrderDetail", "SerialNumber", "OrderId", Model.OrderId.ToString());

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
        /// 批量导入
        /// </summary>
        /// <param name="orderExtends"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult BatchAdd1(List<OrderExtend> orderExtends)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Guid? OrderId = orderExtends[0].ShipOrderId;
                var ShipOrder = _context.SdShipOrder.Where(x => x.ID == OrderId).SingleOrDefault();
                List<ShipOrderDetail> list = new List<ShipOrderDetail>();
                int i = 1;
                foreach (OrderExtend item in orderExtends)
                {
                    ShipOrderDetail Model = new ShipOrderDetail();
                    Model.ID = Guid.NewGuid();
                    DoAddPrepare(Model);
                    Model.OrderId = item.ShipOrderId;
                    Model.SalesOrderId = item.SalesOrderId;
                    Model.MaterialId = item.MaterialId;
                    Model.MaterialName = item.MaterialName;
                    Model.UnitId = item.UnitId;
                    Model.SerialNumber = i;
                    Model.SalesOrderDetailId = item.ID;
                    Model.ShipQTY = item.ShipQTY;
                    Model.CustomerMaterialCode = item.CustomerMaterialCode;
                    Model.StockId = item.StockId;
                    Model.GoodsLocationId = item.GoodsLocationId;
                    Model.DeliveryrDate = item.DeliveryrDate;
                    Model.ShipDate = ShipOrder.ShipDate;
                    if (item.ShipQTY > 0)
                    {
                        list.Add(Model);
                        i++;
                    }
                }

                if (list.Count > 0)
                    DBHelper.Instance.AddRange(list);

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

                string id = Convert.ToString(modelModify.ID);
                var SdShipOrderDetail = _context.SdShipOrderDetail.Where(x => x.ID == Guid.Parse(id)).SingleOrDefault();
                if (SdShipOrderDetail == null)
                    throw new Exception("无效的数据ID！");

                decimal NewShipQTY = modelModify.ShipQTY.Value;
                decimal? ShipQTY = SdShipOrderDetail.ShipQTY;
                if (NewShipQTY > ShipQTY)
                {
                    Guid? SalesOrderDetailId = SdShipOrderDetail.SalesOrderDetailId;
                    var SdShipOrder = _context.SdShipOrder.Where(x => x.ID == SdShipOrderDetail.OrderId).SingleOrDefault();

                    string sql = @"SELECT A.QTY - ISNULL (E.ShipQTY, 0) QTY
                                FROM SdOrderDetail A
                                     JOIN SdOrder B
                                        ON     A.OrderId = B.ID
                                           AND A.IsDeleted = B.IsDeleted
                                           AND B.CustomerId = '{0}'
                                     LEFT JOIN
                                     (SELECT SUM (A.ShipQTY) ShipQTY, A.SalesOrderDetailId
                                      FROM SdShipOrderDetail A
                                           JOIN SdShipOrder B
                                              ON     A.OrderId = B.ID
                                                 AND A.IsActive = B.IsActive
                                                 AND A.IsDeleted = B.IsDeleted
                                      WHERE     A.IsDeleted = 'false'
                                            AND A.IsActive = 'true'
                                            AND B.CustomerId = '{0}'
                                      GROUP BY A.SalesOrderDetailId) E
                                        ON A.ID = E.SalesOrderDetailId
                                WHERE     A.IsDeleted = 'false'
                                      AND A.IsActive = 'true'
                                      AND B.AuditStatus = 'CompleteAudit'
                                      AND A.ID = '{1}'";
                    sql = string.Format(sql, SdShipOrder.CustomerId, SalesOrderDetailId);
                    decimal QTY = Convert.ToDecimal(DBHelper.Instance.ExecuteScalar(sql));
                    if (QTY < (NewShipQTY - ShipQTY))
                        throw new Exception("待发货数量不足，当前待发货:" + QTY + "！");
                }

                //modelModify.NoShipQTY = modelModify.ShipQTY;
                //Update<ShipOrderDetail>(modelModify);
                //_context.SaveChanges();

                DbUpdate du = new DbUpdate("SdShipOrderDetail", "ID", id);
                du.Set("ShipQTY", modelModify.ShipQTY.Value);
                du.Set("NoShipQTY", modelModify.ShipQTY.Value);
                DBHelper.Instance.ExecuteScalar(du.GetSql());

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
                        FROM SdShipOrderDetail A
                             JOIN
                             (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) NUM
                              FROM (SELECT *
                                    FROM (SELECT A.*
                                          FROM SdShipOrderDetail A
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

                ShipOrderDetail Model = _context.SdShipOrderDetail.Where(x => x.ID == Id).SingleOrDefault();
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

        [HttpPost]
        public override IActionResult BatchDelete(List<ShipOrderDetail> entryList)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {

                for (int i = 0; i < entryList.Count; i++)
                {
                    DbUpdate du = new DbUpdate("SdShipOrderDetail");
                    du.Set("IsDeleted", "true");
                    du.Where("ID", "=", entryList[i].ID);
                    DBHelper.Instance.ExecuteScalar(du.GetSql());
                }

                ShipOrderDetail Model = _context.SdShipOrderDetail.Where(x => x.ID == entryList[0].ID).SingleOrDefault();
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

        #region 自定义列模块数据返回
        /// <summary>
        /// 自定义列模块数据返回
        /// </summary>
        /// <param name="paramData"></param>
        /// <param name="moduleCode"></param>
        /// <param name="sorter"></param>
        /// <param name="filter"></param>
        /// <param name="parentColumn"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>

        [HttpGet]
        public IActionResult GetGridList1(string paramData, string moduleCode, string sorter = "{}", string filter = "{}", string parentColumn = null, string parentId = null)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            int current = 1;
            int pageSize = 20;
            int total = 0;
            try
            {

                var searchParam = JsonConvert.DeserializeObject<Dictionary<string, object>>(paramData);
                var sorterParam = JsonConvert.DeserializeObject<Dictionary<string, string>>(sorter);

                string queryCodition = "1=1";

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

                    if (item.Key == "_timestamp")
                    {
                        continue;
                    }
                    //if (string.IsNullOrEmpty(item.Value.ToString()))
                    //    queryCodition += " AND A." + item.Key + " =''";
                    //else
                    //    queryCodition += " AND A." + item.Key + " like '%" + item.Value.ToString() + "%'";

                    if (!string.IsNullOrEmpty(item.Value.ToString()))
                        queryCodition += " AND A." + item.Key + " like '%" + item.Value.ToString() + "%'";

                }
                if (!string.IsNullOrEmpty(parentId) && !string.IsNullOrEmpty(parentColumn))
                    queryCodition += " AND A." + parentColumn + " = '" + parentId + "'";
                #endregion

                string userId = string.Empty;
                ModuleSql moduleSql = new ModuleSql(moduleCode);
                GridList grid = new GridList();
                string tableName = moduleSql.GetTableName();
                string SqlSelectBrwAndTable = moduleSql.GetSqlSelectBrwAndTable();
                string SqlSelectAndTable = moduleSql.GetSqlSelectAndTable();
                if (!string.IsNullOrEmpty(tableName))
                {
                    SqlSelectBrwAndTable = string.Format(SqlSelectBrwAndTable, tableName);
                    SqlSelectAndTable = string.Format(SqlSelectAndTable, tableName);
                }
                string SqlDefaultCondition = moduleSql.GetSqlDefaultCondition();
                //SqlDefaultCondition = SqlDefaultCondition.Replace("[UserId]", userId);
                string DefaultSortField = moduleSql.GetDefaultSortField();
                string DefaultSortDirection = moduleSql.GetDefaultSortDirection();
                if (string.IsNullOrEmpty(DefaultSortDirection))
                {
                    DefaultSortDirection = "ASC";
                }
                grid.SqlSelect = SqlSelectBrwAndTable;
                grid.SqlDefaultCondition = SqlDefaultCondition;
                grid.SqlQueryCondition = queryCodition;
                grid.SortField = DefaultSortField;
                grid.SortDirection = DefaultSortDirection;

                #region 处理排序
                if (sorterParam.Count > 0)
                    foreach (var item in sorterParam)
                    {
                        grid.SortField = item.Key;
                        if (item.Value == "ascend")
                            grid.SortDirection = "ASC";
                        else if (item.Value == "descend")
                            grid.SortDirection = "DESC";

                    }
                #endregion

                grid.PageSize = pageSize;
                grid.CurrentPage = current;
                grid.ModuleCode = moduleCode;
                total = grid.GetTotalCount();
                string sql = grid.GetQueryString();
                DataTable dtTemp = DBHelper.Instance.GetDataTable(sql);
                DataTable dt = Utility.FormatDataTableForTree(moduleCode, userId, dtTemp);
                obj.data = dt;

                status = "ok";
                message = "查询成功！";
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

    }
}
