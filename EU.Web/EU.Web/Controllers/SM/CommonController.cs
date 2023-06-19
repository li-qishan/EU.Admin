using System;
using System.Data;
using System.Dynamic;
using System.Text;
using EU.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using EU.Core.Module;
using EU.Model.System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using EU.Core;
using EU.Core.CacheManager;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EU.Model;
using static EU.Core.Const.Consts;
using EU.Domain.System;
using EU.Core.Entry;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Dapper.SqlMapper;
using EU.DataAccess;
using SqlSugar;
using EU.Domain;
using EU.Model.System.CompanyStructure;
using EU.Core.DBManager;

namespace EU.Web.Controllers.System
{
    /// <summary>
    /// 公共方法
    /// </summary>
    [ApiController, Authorize(Policy = "Permission"), Route("api/[controller]/[action]"), GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class CommonController : BaseController<SmCounty>
    {
        public CommonController(IUnitOfWorkManage unitOfWorkManage) : base(unitOfWorkManage)
        {
        }
        #region 生成表模型
        /// <summary>
        /// 生成表模型
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, Route("{tableName}")]
        public ContentResult GenerateTableModel(string tableName)
        {

            #region MyRegion
            //DECLARE @CurrentUser   sysname;
            //SELECT @CurrentUser = user_name();
            //EXECUTE sp_addextendedproperty 'MS_Description', '颜色', 'user', @CurrentUser, 'table', 'BdColor';
            #endregion
            string modelName = tableName;
            string TableCnName = string.Empty;

            StringBuilder build = new StringBuilder();
            string sql = @"SELECT A.name AS table_name,
                                       B.name AS column_name,
                                       D.data_type,
                                       C.value AS column_description,
                                       D.NUMERIC_PRECISION, D.NUMERIC_SCALE
                                FROM sys.tables A
                                     INNER JOIN sys.columns B ON B.object_id = A.object_id
                                     LEFT JOIN sys.extended_properties C
                                        ON C.major_id = B.object_id AND C.minor_id = B.column_id
                                     LEFT JOIN information_schema.columns D
                                        ON D.column_name = B.name AND D.TABLE_NAME = '{0}'
                                WHERE A.name = '{0}'";
            sql = string.Format(sql, tableName);
            DataTable dtColumn = DBHelper.Instance.GetDataTable(sql);

            #region 获取表中文名
            sql = @"SELECT f.value TableName
                            FROM sysobjects d
                                 LEFT JOIN sys.extended_properties f
                                    ON d.id = f.major_id AND f.minor_id = 0
                            WHERE d.name = '{0}'";
            sql = string.Format(sql, tableName);
            TableCnName = Convert.ToString(DBHelper.Instance.ExecuteScalar(sql));
            #endregion

            build.Append("/*  代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。<br/>");
            build.Append("* " + tableName + ".cs<br/>");
            build.Append("*<br/>");
            build.Append("*功 能： N / A<br/>");
            build.Append("* 类 名： " + tableName + "<br/>");
            build.Append("*<br/>");
            build.Append("* Ver    变更日期 负责人  变更内容<br/>");
            build.Append("* ───────────────────────────────────<br/>");
            build.Append("*V0.01  " + DateTime.Now.ToString() + "  SimonHsiao   初版<br/>");
            build.Append("*<br/>");
            build.Append("* Copyright(c) " + DateTime.Now.Year + " SUZHOU EU Corporation. All Rights Reserved.<br/>");
            build.Append("*┌──────────────────────────────────┐<br/>");
            build.Append("*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│<br/>");
            build.Append("*│　版权所有：苏州一优信息技术有限公司&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;│<br/>");
            build.Append("*└──────────────────────────────────┘<br/>");
            build.Append("*/ <br/>");
            build.Append("using EU.Entity;<br/>");
            build.Append("using System;<br/>");
            build.Append("using System.ComponentModel;<br/>");
            build.Append("using System.ComponentModel.DataAnnotations;<br/>");
            build.Append("using System.ComponentModel.DataAnnotations.Schema;<br/>");
            build.Append("<br/>");
            build.Append("namespace EU.Model<br/>");
            build.Append("{<br/>");
            build.Append("<br/>");
            build.Append("/// &lt;summary&gt;<br/>");
            build.Append("/// " + tableName + "<br/>");
            build.Append("/// &lt;/summary&gt;<br/>");
            build.Append(@"[Entity(TableCnName = " + "\"" + TableCnName + "\"" + ", TableName =" + "\"" + tableName + "\"" + ")]<br/>");
            build.Append("public class " + modelName + " : Base.PersistPoco<br/>");
            build.Append("{<br/>");

            #region 属性
            //build.Append("<br/>");

            #region 处理表字段
            string columnCode = string.Empty;
            string dataType = string.Empty;
            string column_description = string.Empty;
            string NUMERIC_PRECISION = string.Empty;
            string NUMERIC_SCALE = string.Empty;

            string[] a = {
                        "ID", "CreatedBy", "CreatedTime", "UpdateBy", "UpdateTime", "ImportDataId", "ModificationNum",
                    "Tag", "GroupId", "CompanyId", "CurrentNode", "Remark","IsActive","AuditStatus","IsDeleted"

            };

            for (int i = 0; i < dtColumn.Rows.Count; i++)
            {
                columnCode = dtColumn.Rows[i]["column_name"].ToString();
                dataType = dtColumn.Rows[i]["data_type"].ToString();
                column_description = dtColumn.Rows[i]["column_description"].ToString();
                NUMERIC_PRECISION = dtColumn.Rows[i]["NUMERIC_PRECISION"].ToString();
                NUMERIC_SCALE = dtColumn.Rows[i]["NUMERIC_SCALE"].ToString();

                if (a.Contains(columnCode))
                    continue;

                build.Append("<br/>");
                build.Append("/// &lt;summary&gt;<br/>");
                build.Append("/// " + column_description + "<br/>");
                build.Append("/// &lt;/summary&gt;<br/>");
                if (dataType == "decimal")
                    build.Append($"[Display(Name = \"" + columnCode + "\"), Description(\"" + column_description + "\"), Column(TypeName = \"decimal("+ NUMERIC_PRECISION + ","+ NUMERIC_SCALE + ")\")]<br/>");
                else build.Append("[Display(Name = \"" + columnCode + "\"), Description(\"" + column_description + "\")]<br/>");

                switch (dataType)
                {
                    #region 字符串
                    case "varchar":
                        {
                            build.Append("public string " + columnCode + " { get; set; }<br/>");
                            break;
                        }
                    case "char":
                        {
                            build.Append("public string " + columnCode + " { get; set; }<br/>");
                            break;
                        }
                    case "text":
                        {
                            build.Append("public string " + columnCode + " { get; set; }<br/>");
                            break;
                        }
                    #endregion

                    #region 日期
                    case "datetime":
                        {
                            build.Append("public DateTime? " + columnCode + " { get; set; }<br/>");
                            break;
                        }
                    case "date":
                        {
                            build.Append("public DateTime? " + columnCode + " { get; set; }<br/>");
                            break;
                        }
                    #endregion

                    #region 数字
                    case "decimal":
                        {

                            build.Append("public decimal? " + columnCode + " { get; set; }<br/>");
                        }
                        break;
                    case "int":
                        {

                            build.Append("public int? " + columnCode + " { get; set; }<br/>");

                            break;
                        }
                    case "uniqueidentifier":
                        {

                            build.Append("public Guid? " + columnCode + " { get; set; }<br/>");

                            break;
                        }
                    case "bit":
                        {

                            build.Append("public bool? " + columnCode + " { get; set; }<br/>");

                            break;
                        }
                        #endregion
                }
            }
            #endregion
            #endregion


            build.Append("}<br/>");
            build.Append("}<br/>");
            return Content(build.ToString(), "text/html", Encoding.UTF8);
        }
        #endregion

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public IActionResult Test(SmModule module)
        {
            Convert.ToInt16(module.ModuleName);
            dynamic obj = new ExpandoObject();
            //SmModule module = ModuleInfo.GetModuleInfo("BF_APPLY_PAY_MNG");
            //ModuleSql moduleSql = new ModuleSql("BF_APPLY_PAY_MNG");
            //SmModuleSqlExtend ModuleSql = moduleSql.GetModuleSql();
            //obj.ModuleSql = ModuleSql;
            //obj.data = LOVHelper.GetLovList("APPLY_STATUS");

            //TableManager.InitTableAndField("SmModuleColumn");


            //var cache = _context.SmModuleColumn.Where(o => o.SmModuleId == Guid.Parse("53c6f4c2-ef16-4580-9454-dad4f5cf6eea")).ToList();
            //cache?.ForEach(item => {
            //    item.SmModuleId = Guid.Parse("52525af5-cf82-4d3d-8c50-4c7dc92bab04");
            //    item.CreatedTime = DateTime.Now;
            //    item.ID = Guid.NewGuid();
            //});
            //_context.AddRange(cache);

            return Ok(obj);
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public async Task<ServiceResult> Test1()
        {
            //var cache = await _context.SmModuleColumn.Where(o => o.SmModuleId == Guid.Parse("53c6f4c2-ef16-4580-9454-dad4f5cf6eea")).ToListAsync();

            //return Ok(obj);

            //string sql = "SELECT CheckQTY FROM ApCheckDetailSum_V";
            //var dtColumn = (await DBHelper.Instance.QueryListAsync<ApCheckDetailExtend>(sql));

            using var _context = ContextFactory.CreateContext();
            var totalCount = 0;

            var list = await _db.Queryable<SmProvince>()
              //.OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
              //.WhereIF(whereExpression != null, whereExpression)
              .ToPageListAsync(1, 15, totalCount);

            return ServiceResult<List<SmProvince>>.OprateSuccess(list, "操作成功");

        }

        #region Excel导出
        /// <summary>
        /// Excel导出
        /// </summary>
        /// <param name="moduleCode">模块代码</param>
        /// <param name="paramData">参数</param>
        /// <param name="sorter">排序</param>
        /// <param name="exportExcelColumns">导出栏位</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ExportExcel(string moduleCode, string paramData = "{}", string sorter = "{}", string exportExcelColumns = "")
        {
            dynamic obj = new ExpandoObject();
            dynamic data = new ExpandoObject();
            int current = 1;
            string queryCondition = "1=1 ";
            string defaultCondition = string.Empty;
            int pageCount = 999999999;// options.Rows;
            int totalCount;
            int outPageSize;
            string status = "error";
            string message = string.Empty;

            try
            {

                var searchParam = JsonConvert.DeserializeObject<Dictionary<string, object>>(paramData);
                var sorterParam = JsonConvert.DeserializeObject<Dictionary<string, string>>(sorter);

                #region 处理模块信息
                SmModule module = ModuleInfo.GetModuleInfo(moduleCode);

                ModuleSql moduleSql = new ModuleSql(moduleCode);
                ModuleSqlColumn moduleSqlColumn = new ModuleSqlColumn(moduleCode);
                #endregion

                #region 处理默认条件
                //if (!string.IsNullOrEmpty(options.RowId))
                //{
                //    defaultCondition = " AND " + moduleSql.GetTableAliasName() + ".ROW_ID='" + options.RowId + "'";
                //}
                //if (!string.IsNullOrEmpty(options.MasterId))
                //{
                //    defaultCondition = " AND " + moduleSql.GetTableAliasName() + "." + moduleSql.GetPrimaryKey() + "='" + options.MasterId + "'";
                //}
                #endregion

                #region 处理查询条件
                foreach (var item in searchParam)
                {
                    if (item.Key == "current")
                        continue;

                    if (item.Key == "pageSize")
                        continue;

                    if (item.Key == "_timestamp")
                        continue;
                    if (!string.IsNullOrEmpty(item.Value.ToString()))
                        queryCondition += " AND A." + item.Key + " like '%" + item.Value.ToString() + "%'";

                }
                #endregion

                #region 处理排序
                string DefaultSortField = moduleSql.GetDefaultSortField();
                string DefaultSortDirection = moduleSql.GetDefaultSortDirection();
                if (sorterParam.Count > 0)
                    foreach (var item in sorterParam)
                    {
                        DefaultSortField = item.Key;
                        if (item.Value == "ascend")
                            DefaultSortDirection = "ASC";
                        else if (item.Value == "descend")
                            DefaultSortDirection = "DESC";

                    }
                #endregion

                string sql = moduleSql.GetCurrentSql(moduleCode, current, DefaultSortField, DefaultSortDirection, defaultCondition, queryCondition, pageCount, out totalCount, out outPageSize);
                string moduleColumns = moduleSqlColumn.GetExportExcelColumns();
                if (!string.IsNullOrEmpty(exportExcelColumns))
                    moduleColumns = exportExcelColumns;

                string excelSql = "SELECT " + moduleColumns + " FROM (" + sql + ") A";
                string tableName = module.ModuleName;
                string fileName = tableName + DateTime.Now.ToString("yyyyMMddHHssmm") + ".xlsx";
                string folder = DateTime.Now.ToString("yyyyMMdd");
                string filePath = $"/Download/ExcelExport/{folder}/";
                string savePath = "wwwroot" + filePath;
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                try
                {
                    string customSql = string.Empty;
                    //BeforeExport(moduleColumns, sql, queryCondition, out customSql);
                    //Logger.WriteLog(moduleSql.GetTableName(), "ExportExcel:" + excelSql);
                    //Logger.Info(LoggerType.ExcelSql, excelSql);
                    //if (!string.IsNullOrEmpty(customSql))
                    //{
                    //    excelSql = customSql;
                    //}
                    DataTable dt = DBHelper.Instance.GetDataTable(excelSql, null);
                    foreach (DataColumn column in dt.Columns)
                        column.Caption = moduleSqlColumn.GetExportExcelColumnRenderer(column.ColumnName);
                    NPOIHelper.ExportExcel(dt, "", savePath + fileName);
                    string fileId = StringHelper.Id;
                    data.fileId = fileId;

                    #region 导入文件数据
                    DbInsert di = new DbInsert("FileAttachment");
                    di.IsInitDefaultValue = false;
                    di.Values("ID", fileId);
                    di.Values("OriginalFileName", fileName);
                    di.Values("CreatedTime", Utility.GetSysDate());
                    di.Values("CreatedBy", !string.IsNullOrEmpty(User.Identity.Name) ? User.Identity.Name : string.Empty);
                    di.Values("FileName", fileName);
                    di.Values("FileExt", "xlsx");
                    di.Values("Path", filePath);
                    DBHelper.Instance.ExcuteNonQuery(di.GetSql(), null);
                    #endregion

                    #region 记录模块操作日志
                    try
                    {
                        DBHelper.RecordOperateLog(User.Identity.Name, moduleCode, moduleSql.GetTableName(), "", OperateType.Excel, savePath + fileName);
                    }
                    catch
                    {

                    }
                    #endregion
                }
                catch (Exception E)
                {
                    return Ok(E);
                    //Logger.WriteLog("ErrorSql", moduleCode + ":" + excelSql + ",ErrorMsg:" + Ex.Message);
                    //Logger.Error(LoggerType.ErrorSql, moduleCode + ":" + excelSql + ",ErrorMsg:" + Ex.Message);
                    //return Response.Error("导出失败: " + Ex.Message);
                }
                //WebSocketHelper.SendMessage(new WebSocketMessage
                //{
                //    SendClientId = UserContext.Current.UserInfo.UserId,
                //    Action = WebSocketActionConst.notice,
                //    Message = "导出成功！"
                //});
                //return Response.OK("导出成功！", (savePath + "/" + fileName).EncryptDES(AppSetting.Secret.ExportFile));

                status = "ok";
                message = "查询成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
                return Ok(E);
            }

            obj.status = status;
            obj.data = data;
            obj.message = message;
            return Ok(obj);
        }

        #endregion

        #region Excel导入
        /// <summary>
        /// Excel导入
        /// </summary>
        /// <param name="fileList">文件</param>
        /// <param name="moduleCode">模块代码</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ImportExcelAsync(IFormFileCollection fileList, string moduleCode, string fileName)
        {
            dynamic obj = new ExpandoObject();
            dynamic data = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            string filePath = string.Empty;
            string importDataId = StringHelper.Id;

            try
            {

                SmModule Module = ModuleInfo.GetModuleInfo(moduleCode);
                if (Module == null)
                    throw new Exception("模块代码【" + moduleCode + "】不存在！");

                if (fileList.Count > 0)
                {
                    foreach (var file in fileList)
                    {
                        var ext = string.Empty;
                        if (string.IsNullOrEmpty(file.FileName) == false)
                        {
                            var dotPos = file.FileName.LastIndexOf('.');
                            ext = file.FileName.Substring(dotPos + 1);
                        }

                        filePath = "wwwroot/importexcel/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Utility.GetSysID();
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        var filepath = Path.Combine(filePath, $"{fileName}");
                        //var filepath = Path.Combine(pathHeader, file.FileName);
                        using (var stream = global::System.IO.File.Create(filepath))
                        {
                            await file.CopyToAsync(stream);
                        }
                        //FileAttachment fileAttachment = new FileAttachment();
                        //fileAttachment.OriginalFileName = file.FileName;
                        //fileAttachment.CreatedBy = !string.IsNullOrEmpty(User.Identity.Name) ? new Guid(User.Identity.Name) : Guid.Empty;
                        //fileAttachment.CreatedTime = Utility.GetSysDate();
                        //fileAttachment.FileName = fileName;
                        //fileAttachment.FileExt = ext;
                        //fileAttachment.Length = file.Length;
                        //fileAttachment.Path = pathHeader;
                        //url = fileName + "." + ext;



                        string sql = "SELECT * FROM SmImpTemplate WHERE ModuleCode='{0}'";
                        sql = string.Format(sql, moduleCode);
                        SmImpTemplate impTemplate = DBHelper.Instance.QueryFirst<SmImpTemplate>(sql);

                        if (impTemplate == null)
                            throw new Exception("请配置模块【" + Module.ModuleName + "】的导入模板，详情请联系客服！");

                        string SheetName = impTemplate.SheetName;

                        DataTable dt = NPOIHelper.ImportExcel(filepath, SheetName);
                        if (dt.Rows.Count > 0)
                        {
                            string userId = !string.IsNullOrEmpty(User.Identity.Name) ? User.Identity.Name : string.Empty;
                            ImportHelper.ImportData(impTemplate, importDataId, filePath, fileName, dt, userId);
                        }
                        List<string> Importobj = new List<string>();
                        DataTable dtImportData = ImportHelper.GetImportDataDetailList(importDataId, impTemplate.ID);

                        for (int i = 0; i < dtImportData.Columns.Count; i++)
                            Importobj.Add(dtImportData.Columns[i].ColumnName);

                        data.importColumns = Importobj;
                        data.importList = dtImportData;

                    }
                }

                status = "ok";
            }
            catch (Exception E)
            {
                message = E.Message;
                data.errorList = ImportHelper.GetImportErrorList(importDataId);
            }
            data.importDataId = importDataId;
            obj.data = data;
            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        #endregion

        #region Excel导入数据转换
        [HttpPost]
        public IActionResult TransferExcelData(dynamic modelModify)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                string importDataId = modelModify.importDataId;
                string importTemplateCode = modelModify.importTemplateCode;
                string type = modelModify.type;
                string masterId = modelModify.masterId;
                string moduleCode = modelModify.moduleCode;

                string userId = !string.IsNullOrEmpty(User.Identity.Name) ? User.Identity.Name : string.Empty;
                ImportHelper.TransferData(importDataId, importTemplateCode, userId, false);
                ImportHelper.AfterImport(importTemplateCode, importDataId, masterId);

                status = "ok";
                message = "导入成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
                return Ok(E);
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        #endregion

        #region 清空缓存
        /// <summary>
        /// 清空缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ClearCache()
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                new RedisCacheService(1).Clear();
                new RedisCacheService(3).Clear();
                new RedisCacheService(4).Clear();

                #region 初始化缓存
                ModuleInfo.Init();
                ModuleSql.Init();
                ModuleSqlColumn.Init();
                LOVHelper.Init();
                ConfigCache.Init();
                FunctionPrivilege.Init();
                #endregion

                status = "ok";
                message = "清空成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
                return Ok(E);
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        #endregion
    }
}
