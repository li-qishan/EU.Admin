using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.Core;
using EU.Core.Const;
using EU.Core.Entry;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using static EU.Core.Const.Consts;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.System.Setup
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmFieldCatalogController : BaseController<SmFieldCatalog>
    {

        public SmFieldCatalogController(DataContext _context, IBaseCRUDVM<SmFieldCatalog> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 获取List
        /// <summary>
        /// 获取List
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public override async Task<ServiceResult> GetPageList(string paramData, string sorter = "{}", string filter = "{}", string parentColumn = null, string parentId = null, string moduleCode = null)
        //{
        //    //dynamic obj = new ExpandoObject();
        //    int current = 1;
        //    int pageSize = 10000;
        //    long total = 0;
        //    string sql = string.Empty;

        //    try
        //    {

        //        var aa = await _context.SmApi.FindAsync(1);
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
        //        if (!string.IsNullOrEmpty(parentId) && !string.IsNullOrEmpty(parentColumn))
        //            queryCodition += " AND A." + parentColumn + " = '" + parentId + "'";
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
        //                                      FROM SmFieldCatalog A
        //                                      WHERE A.IsDeleted = 'false' AND A.IsActive = 'true' AND {2}) A) B)
        //                         C
        //                        WHERE ROWNUM <= {1} AND ROWNUM > {0}";
        //        sql = string.Format(sql, startIndex, endIndex, queryCodition);
        //        var entities = DBHelper.Instance.QueryList<SmFieldCatalog>(sql);

        //        string countString = @"SELECT COUNT (1)
        //                                FROM SmFieldCatalog A
        //                            WHERE A.IsDeleted = 'false' AND A.IsActive = 'true' AND {0}";
        //        countString = string.Format(countString, queryCodition);
        //        total = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(countString));

        //        return ServiceResult<List<SmFieldCatalog>>.OprateSuccess(entities, total, current, pageSize, ResponseText.QUERY_SUCCESS);

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    //obj.current = current;
        //    //obj.pageSize = pageSize;
        //    //obj.total = total;
        //    //obj.status = status;
        //    //obj.message = message;
        //    //return Ok(obj);
        //}

        #endregion
    }
}
