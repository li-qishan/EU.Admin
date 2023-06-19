using EU.DataAccess;
using EU.Model.System;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using EU.Domain;
using EU.Core.CacheManager;
using static EU.Core.Const.Consts;
using System.Threading.Tasks;
using EU.Core.Entry;
using Microsoft.EntityFrameworkCore;
using EU.Core.Const;
using Castle.DynamicProxy;
using EU.Core.Module;
using EU.Core.Utilities;

namespace EU.Web.Controllers.System.Privilege
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmModuleSqlController : EU.Web.Controllers.BaseController1<SmModuleSql>
    {
        RedisCacheService RedisCacheService = new RedisCacheService(2);
        public SmModuleSqlController(DataContext _context, IBaseCRUDVM<SmModuleSql> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 获取模块信息
        /// <summary>
        /// 获取模块信息
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <returns></returns>
        [HttpPost, Route("{moduleId}")]
        public async Task<ServiceResult<dynamic>> GetByModuleId(Guid moduleId)
        {
            dynamic data = new ExpandoObject();

            try
            {
                //获取模块信息
                var moduleSql = await _context.SmModuleSql.Where(x => x.ModuleId == moduleId).FirstOrDefaultAsync();

                var module = await _context.SmModules.FindAsync(moduleId);
                data.module = module;
                data.moduleSql = moduleSql;
                return ServiceResult<dynamic>.OprateSuccess(data, ResponseText.QUERY_SUCCESS);

            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region 获取模块SQL信息
        /// <summary>
        /// 获取模块SQL信息
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <returns></returns>
        [HttpPost, Route("{moduleId}")]
        public async Task<ServiceResult<string>> GetModuleFullSql(Guid moduleId)
        {
            string sql = string.Empty;

            try
            {
                var module = await _context.SmModules.FindAsync(moduleId);
                ModuleSql moduleSql = new ModuleSql(module.ModuleCode);
                string tableName = moduleSql.GetTableName();
                string SqlSelectBrwAndTable = moduleSql.GetSqlSelectBrwAndTable();
                string SqlSelectAndTable = moduleSql.GetSqlSelectAndTable();
                if (!string.IsNullOrEmpty(tableName))
                {
                    SqlSelectBrwAndTable = string.Format(SqlSelectBrwAndTable, tableName);
                    SqlSelectAndTable = string.Format(SqlSelectAndTable, tableName);
                }
                string queryCodition = "1=1";
                string SqlDefaultCondition = moduleSql.GetSqlDefaultCondition();

                GridList grid = new GridList();
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
                grid.ModuleCode = module.ModuleCode;
                sql = grid.GetQueryString();
                sql = ModuleInfo.FormatSqlVariable(sql);
                return ServiceResult<string>.OprateSuccess(sql, ResponseText.QUERY_SUCCESS);

            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(SmModuleSql Model)
        {
            RedisCacheService.Remove("SmModuleSql");
            return base.Add(Model);
        }
        #endregion

        #region 更新重写
        [HttpPost]
        public override IActionResult Update(dynamic modelModify)
        {
            RedisCacheService.Remove("SmModuleSql");

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Update<SmModuleSql>(modelModify);
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

    }
}
