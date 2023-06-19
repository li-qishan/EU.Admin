using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using EU.Core;
using EU.Core.DBManager;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static EU.Core.Const.Consts;
using EU.Core.Entry;
using EU.Core.Const;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.System.Setup
{
    /// <summary>
    /// 系统表字典
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmTableCatalogController : BaseController1<SmTableCatalog>
    {
        public SmTableCatalogController(DataContext _context, IBaseCRUDVM<SmTableCatalog> BaseCrud) : base(_context, BaseCrud)
        {
        }
        [HttpGet]
        public override async Task<IActionResult> GetById(Guid Id)
        {
            //var table = _context.SmTableCatalog.Where(x => x.ID == Id).SingleOrDefault();

            string sql = @"SELECT * FROM SmTableCatalog  WHERE ID='{0}'";
            sql = string.Format(sql, Id);
            SmTableCatalog table = DBHelper.Instance.QueryFirst<SmTableCatalog>(sql);
            return Ok(table);
        }

        #region 初始化指定表
        /// <summary>
        /// 初始化指定表
        /// </summary>
        /// <param name="tableCode"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InitAssignmentTable(dynamic modelModify)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            string sql = string.Empty;
            try
            {
                string tableCode = modelModify.tableCode;
                Utility.IsNullOrEmpty(tableCode, "表代码不能传空！");
                TableManager.InitTableAndField(tableCode);

                status = "ok";
                message = "执行成功！";
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

        #region 初始化所有表
        /// <summary>
        /// 初始化指定表
        /// </summary>
        /// <param name="tableCode"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InitAllTable()
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            try
            {
                Task.Run(() =>
                {
                    #region 初始化所有表
                    DataTable dvUserTables = null;
                    if (DBHelper.MySql)
                    {
                        DbSelect dsUserTables = new DbSelect("INFORMATION_SCHEMA.TABLES A", "A", null);
                        dsUserTables.IsInitDefaultValue = false;
                        string dbName = DBServerProvider.GetConnectionString().Split("Database=")[1].Split(";")[0]?.Trim();
                        dsUserTables.Where($"A.TABLE_SCHEMA='{dbName}' AND A.TABLE_TYPE='BASE TABLE'");
                        dsUserTables.Select("A.TABLE_NAME");
                        dvUserTables = DBHelper.Instance.GetDataTable(dsUserTables.GetSql(), null);
                    }
                    else
                    {
                        DbSelect dsUserTables = new DbSelect("SYSOBJECTS A", "A", null);
                        dsUserTables.IsInitDefaultValue = false;
                        dsUserTables.Select("A.NAME", "TABLE_NAME");
                        dsUserTables.Where("A.TYPE='U'");
                        dvUserTables = DBHelper.Instance.GetDataTable(dsUserTables.GetSql(), null);
                    }
                    if (dvUserTables != null && dvUserTables.Rows.Count > 0)
                    {
                        for (int i = 0; i < dvUserTables.Rows.Count; i++)
                        {
                            TableManager.InitTableAndField(dvUserTables.Rows[i]["TABLE_NAME"].ToString().ToUpper());
                        }
                    }
                    #endregion

                    #region 初始化所有视图
                    DataTable dvUserViews = null;
                    if (DBHelper.MySql)
                    {
                        //DbSelect dsUserViews = new DbSelect("USER_VIEWS A", "A", null);
                        //dsUserViews.IsInitDefaultValue = false;
                        //dsUserViews.Select("A.VIEW_NAME");
                        string dvSql = "SHOW TABLE STATUS WHERE COMMENT='VIEW'";
                        dvUserViews = DBHelper.Instance.GetDataTable(dvSql, null);
                    }
                    else
                    {
                        DbSelect dsUserTables = new DbSelect("SYSOBJECTS A", "A", null);
                        dsUserTables.IsInitDefaultValue = false;
                        dsUserTables.Select("A.NAME", "VIEW_NAME");
                        dsUserTables.Where("A.TYPE='V'");
                        dvUserViews = DBHelper.Instance.GetDataTable(dsUserTables.GetSql(), null);
                    }
                    if (dvUserViews != null && dvUserViews.Rows.Count > 0)
                    {
                        for (int i = 0; i < dvUserViews.Rows.Count; i++)
                        {
                            TableManager.InitTableAndField(dvUserViews.Rows[i]["VIEW_NAME"].ToString().ToUpper());
                        }
                    }
                    #endregion

                    #region 删除所有不存在的表和视图
                    int count = -1;
                    DbSelect dsUserTables1 = null;
                    //DbDelete ddTableCatalog = null;
                    DbSelect dsTableCatalog = new DbSelect("SmTableCatalog A", "A");
                    dsTableCatalog.Select("A.*");
                    DataTable dtTableCatalog = DBHelper.Instance.GetDataTable(dsTableCatalog.GetSql());
                    for (int i = 0; i < dtTableCatalog.Rows.Count; i++)
                    {
                        if (dtTableCatalog.Rows[i]["TypeCode"].ToString() == "TABLE")
                        {
                            if (DBHelper.MySql)
                            {
                                dsUserTables1 = new DbSelect("INFORMATION_SCHEMA.TABLES A", "A", null);
                                dsUserTables1.IsInitDefaultValue = false;
                                string dbName = DBServerProvider.GetConnectionString().Split("Database=")[1].Split(";")[0]?.Trim();
                                dsUserTables1.Where($"A.TABLE_SCHEMA='{dbName}' AND A.TABLE_TYPE='BASE TABLE'");
                                dsUserTables1.Select("COUNT(*)");
                                count = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(dsUserTables1.GetSql()));
                            }
                            else
                            {
                                dsUserTables1 = new DbSelect("SYSOBJECTS A", "A", null);
                                dsUserTables1.IsInitDefaultValue = false;
                                dsUserTables1.Select("COUNT(*)");
                                dsUserTables1.Where("A.TYPE='U'");
                                dsUserTables1.Where("A.NAME", "=", dtTableCatalog.Rows[i]["TableCode"].ToString());
                                count = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(dsUserTables1.GetSql()));
                            }
                        }
                        else if (dtTableCatalog.Rows[i]["TypeCode"].ToString() == "VIEW")
                        {
                            if (DBHelper.MySql)
                            {
                                dsUserTables1 = new DbSelect("USER_VIEWS A", "A", null);
                                dsUserTables1.IsInitDefaultValue = false;
                                dsUserTables1.Select("COUNT(*)");
                                dsUserTables1.Where("A.VIEW_NAME", "=", dtTableCatalog.Rows[i]["TableCode"].ToString());
                                count = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(dsUserTables1.GetSql()));
                            }
                            else
                            {
                                dsUserTables1 = new DbSelect("SYSOBJECTS A", "A", null);
                                dsUserTables1.IsInitDefaultValue = false;
                                dsUserTables1.Select("COUNT(*)");
                                dsUserTables1.Where("A.TYPE='V'");
                                dsUserTables1.Where("A.NAME", "=", dtTableCatalog.Rows[i]["TableCode"].ToString());
                                count = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(dsUserTables1.GetSql()));
                            }
                        }
                        if (count == 0)
                        {
                            string sql = string.Empty;
                            sql = "DELETE A FROM SmFieldCatalog A WHERE A.TableCode='" + dtTableCatalog.Rows[i]["TableCode"] + "' AND A.IsActive='false'";
                            DBHelper.Instance.ExcuteNonQuery(sql);

                            sql = $"DELETE A FROM SmTableCatalog A WHERE A.ID='{ dtTableCatalog.Rows[i]["ID"]?.ToString()}'";
                            DBHelper.Instance.ExcuteNonQuery(sql);
                        }
                    }
                    #endregion
                });
                status = "ok";
                message = "执行成功！";
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

        #region 获取List
        /// <summary>
        /// 获取List
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public override async Task<ServiceResult> GetPageList(string paramData, string sorter = "{}", string filter = "{}", string parentColumn = null, string parentId = null, string moduleCode = null)
        //{
        //    dynamic obj = new ExpandoObject();
        //    string status = "error";
        //    string message = string.Empty;
        //    int current = 1;
        //    int pageSize = 10000;
        //    int total = 0;
        //    string sql = string.Empty;

        //    try
        //    {
        //        var searchParam = JsonConvert.DeserializeObject<Dictionary<string, object>>(paramData);
        //        string queryCodition = "1=1";

        //        #region 处理查询条件
        //        foreach (var item in searchParam)
        //        {
        //            if (item.Key == "current")
        //            {
        //                current = int.Parse(item.Value.ToString());
        //                continue;
        //            }

        //            if (item.Key == "pageSize")
        //            {
        //                pageSize = int.Parse(item.Value.ToString());
        //                continue;
        //            }
        //            queryCodition += " AND A." + item.Key + " like '%" + item.Value.ToString() + "%'";
        //        }
        //        #endregion

        //        int _pageSize = pageSize;
        //        //计算分页起始索引
        //        int startIndex = current > 1 ? (current - 1) * _pageSize : 0;

        //        //计算分页结束索引
        //        int endIndex = current * _pageSize;


        //        sql = @"SELECT *
        //                    FROM (SELECT *, ROW_NUMBER () OVER (ORDER BY CreatedTime ASC) ROWNUM
        //                          FROM (SELECT *
        //                                FROM (SELECT A.*
        //                                      FROM SmTableCatalog A
        //                                      WHERE A.IsDeleted = 'false' AND A.IsActive = 'true' AND {2}) A) B)
        //                         C
        //                        WHERE ROWNUM <= {1} AND ROWNUM > {0}";
        //        sql = string.Format(sql, startIndex, endIndex, queryCodition);
        //        obj.data = DBHelper.Instance.QueryList<SmTableCatalog>(sql);

        //        string countString = @"SELECT COUNT (1)
        //                                FROM SmTableCatalog A
        //                            WHERE A.IsDeleted = 'false' AND A.IsActive = 'true' AND {0}";
        //        countString = string.Format(countString, queryCodition);
        //        total = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(countString));
        //        return ServiceResult<List<SmFieldCatalog>>.OprateSuccess(entities, total, current, pageSize, ResponseText.QUERY_SUCCESS);

        //    }
        //    catch (Exception E)
        //    {
        //        message = E.Message;
        //    }

        //    obj.current = current;
        //    obj.pageSize = pageSize;
        //    obj.total = total;
        //    obj.status = status;
        //    obj.message = message;
        //    return Ok(obj);
        //}

        #endregion
    }
}
