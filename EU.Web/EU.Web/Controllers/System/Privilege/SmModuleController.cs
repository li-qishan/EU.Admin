using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EU.Core.CacheManager;
using EU.Core.Enums;
using EU.Core.Module;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Domain.System;
using EU.Model.Auth;
using EU.Model.BFProject;
using EU.Model.System;
using EU.Model.System.Privilege;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using EU.Core.Services;
using EU.Core.Extensions;
using System.IO;
using EU.Core.Configuration;
using EU.Core;
using static EU.Core.Const.Consts;
using Microsoft.EntityFrameworkCore;
using EU.Core.Entry;
using NPOI.SS.Formula.Functions;
using EU.Core.Const;

namespace EU.Web.Controllers
{
    /// <summary>
    /// 系统模块
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmModuleController : BaseController1<SmModule>
    {
        RedisCacheService RedisCacheService = new RedisCacheService(1);

        /// <summary>
        /// 系统模块
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public SmModuleController(DataContext _context, IBaseCRUDVM<SmModule> BaseCrud) : base(_context, BaseCrud)
        {

        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(SmModule Model)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //#region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "SmModules", "ModuleCode", Model.ModuleCode, ModifyType.Add, null, "模块代码");
                //#endregion 

                #region 隐藏默认权限
                string[] temArr = { "ImportExcel", "ExportExcel" };
                List<string> typeList = new List<string>(temArr);
                List<SmRoleFunction1> smRoleFunctionList = new List<SmRoleFunction1>();
                List<SmRole> smRoleList = _context.SmRoles.Where(x => x.IsDeleted == false).ToList();
                foreach (SmRole item in smRoleList)
                {
                    foreach (string type in typeList)
                    {
                        SmRoleFunction1 smRoleFunction = new SmRoleFunction1();
                        smRoleFunction.CreatedBy = Guid.Parse(User.Identity.Name);
                        smRoleFunction.CreatedTime = Utility.GetSysDate();
                        smRoleFunction.SmRoleId = item.ID;
                        smRoleFunction.NoActionCode = Model.ID + type;
                        smRoleFunctionList.Add(smRoleFunction);
                    }
                }
                if (smRoleFunctionList.Count > 0)
                    DBHelper.Instance.AddRange(smRoleFunctionList);

                #endregion

                ModuleInfo.Init();

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

        #region App.js动态加载路由
        /// <summary>
        /// App.js动态加载路由
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPatchRoutes()
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                List<TreeMenuData> _menus = RedisCacheService.Get<List<TreeMenuData>>(User.Identity.Name, "UserPatchRoutes");
                if (_menus != null && _menus.Any())
                    obj.data = _menus;
                else
                {
                    //   var roleModule = _context.Set<SmRoleModule>().Where(a => a.IsDeleted == false)
                    //.Join(_context.Set<SmUserRole>(), x => x.SmRoleId, y => y.SmRoleId, (x, y) => new { x, y })
                    //.Where(z => z.y.SmUserId.ToString() == User.Identity.Name && z.x.IsDeleted == false &&
                    //            z.y.IsDeleted == false && z.x.SmModule.IsDeleted == false).Select(d => d.x.SmModuleId)
                    //.Distinct().ToList();

                    List<Guid> moduleIds = new List<Guid>();
                    List<SmModule> modules = new List<SmModule>();
                    string sql = @"SELECT DISTINCT C.ID
                                    FROM SmRoleModule A
                                         JOIN SmUserRole_V B
                                            ON     A.SmRoleId = B.SmRoleId
                                               AND B.SmUserId = '{0}'
                                         JOIN SmModules C ON A.SmModuleId = C.ID AND C.IsDeleted = 'false'
                                    WHERE A.IsDeleted = 'false'";
                    sql = string.Format(sql, User.Identity.Name);
                    modules = DBHelper.Instance.QueryList<SmModule>(sql);
                    moduleIds = modules.Select(o => o.ID).ToList();

                    List<SmModule> moduleList = ModuleInfo.GetModuleList();
                    List<TreeMenuData> TreeMenuData = moduleList.Where(x => x.IsActive == true && x.IsParent == false && x.IsDetail == false && moduleIds.Contains(x.ID) && !string.IsNullOrEmpty(x.RoutePath))
                        .Select(x => new TreeMenuData
                        {
                            id = Guid.NewGuid().ToString(),
                            path = x.RoutePath,
                            name = x.ModuleName,
                            icon = x.Icon,
                            component = !string.IsNullOrEmpty(x.RoutePath) ? "." + x.RoutePath : null,
                            moduleCode = x.ModuleCode
                        }).ToList();
                    TreeMenuData.Add(new TreeMenuData
                    {
                        id = Guid.NewGuid().ToString(),
                        path = "/account/settings",
                        name = "个人设置",
                        icon = "",
                        component = "./account/settings",
                        moduleCode = "SM_USER_SETTING_MNG"
                    });
                    RedisCacheService.AddObject(User.Identity.Name, "UserPatchRoutes", TreeMenuData);
                    obj.data = TreeMenuData;
                }
                status = "ok";
                message = "获取成功！";
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

        #region 左侧菜单
        /// <summary>
        /// 左侧菜单递归
        /// </summary>
        /// <param name="roleModule"></param>
        /// <param name="smModules"></param>
        /// <param name="moduleTree"></param>
        [NonAction]
        public static void LoopToAppendChildren(List<Guid?> roleModule, List<SmModule> smModules, TreeMenuData moduleTree)
        {
            List<TreeMenuData> subItems = new List<TreeMenuData>();
            if (string.IsNullOrEmpty(moduleTree.id))
            {
                subItems = smModules.Where(x => x.IsParent == true && string.IsNullOrEmpty(x.ParentId.ToString()) && roleModule.Contains(x.ID)).OrderBy(x => x.TaxisNo).Select(y => new TreeMenuData
                {
                    id = y.ID.ToString(),
                    path = y.RoutePath,
                    name = y.ModuleName,
                    icon = y.Icon,
                    component = (!string.IsNullOrEmpty(y.RoutePath) && y.IsParent == false) ? "." + y.RoutePath : null
                }).ToList();
            }
            else
            {
                subItems = smModules.Where(x => x.ParentId == Guid.Parse(moduleTree.id) && roleModule.Contains(x.ID)).OrderBy(x => x.TaxisNo).Select(y => new TreeMenuData
                {
                    id = y.ID.ToString(),
                    path = y.RoutePath,
                    name = y.ModuleName,
                    icon = y.Icon,
                    component = (!string.IsNullOrEmpty(y.RoutePath) && y.IsParent == false) ? "." + y.RoutePath : null
                }).ToList();
            }
            moduleTree.children = new List<TreeMenuData>();
            moduleTree.children.AddRange(subItems);
            foreach (var subItem in subItems)
            {
                LoopToAppendChildren(roleModule, smModules, subItem);
            }
        }


        //获取用户有权限的目录及子模块
        /// <summary>
        /// 获取用户有权限的目录及子模块
        /// </summary>
        /// <param name="smModules"></param>
        /// <param name="smRoleList"></param>
        [NonAction]
        public static void LoopGetRoleModule(List<SmModule> smModules, List<Guid?> smRoleList)
        {
            for (int i = 0; i < smModules.Count; i++)
            {
                smRoleList.Add(smModules[i].ID);
                if (smModules[i].ParentId != null && smModules[i].ParentId != Guid.Empty)//递归获取子模块的上级目录
                {
                    List<SmModule> moduleList = ModuleInfo.GetModuleList();
                    var data = moduleList.Where(x => x.ID == smModules[i].ParentId && x.IsDeleted == false && x.IsActive == true).ToList();
                    LoopGetRoleModule(data, smRoleList);
                }
            }
        }

        //获取左侧菜单
        /// <summary>
        /// 获取左侧菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMenuData()
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                List<TreeMenuData> _menus = RedisCacheService.Get<List<TreeMenuData>>(User.Identity.Name, "UserMenu");
                if (_menus != null && _menus.Any())
                    obj.data = _menus;
                else
                {
                    //获取用户角色有权限的子模块
                    //var roleModule = _context.Set<SmRoleModule>()
                    //    .Join(_context.Set<SmUserRole>(), x => x.SmRoleId, y => y.SmRoleId, (x, y) => new { x, y })
                    //    .Where(z => z.y.SmUserId.ToString() == User.Identity.Name && z.x.IsDeleted == false && z.y.IsDeleted == false)
                    //    .Join(_context.SmModules, a => a.x.SmModuleId, b => b.ID, (a, b) => new { a, b }).Select(c => c.b)
                    //    .ToList();

                    List<SmModule> roleModule = new List<SmModule>();
                    string sql = @"SELECT DISTINCT C.*
                                    FROM SmRoleModule A
                                         JOIN SmUserRole_V B
                                            ON     A.SmRoleId = B.SmRoleId
                                               AND B.SmUserId = '{0}'
                                         JOIN SmModules C ON A.SmModuleId = C.ID AND C.IsDeleted = 'false'
                                    WHERE A.IsDeleted = 'false'";
                    sql = string.Format(sql, User.Identity.Name);
                    roleModule = DBHelper.Instance.QueryList<SmModule>(sql);

                    List<Guid?> smRoleList = new List<Guid?>();
                    LoopGetRoleModule(roleModule, smRoleList);//递归获取有权限的子模块的上级目录

                    TreeMenuData treeMenuData = new TreeMenuData();
                    List<SmModule> moduleList = ModuleInfo.GetModuleList();
                    List<SmModule> smModules = moduleList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    LoopToAppendChildren(smRoleList, smModules, treeMenuData);//将模块递归成树
                    var data = new List<TreeMenuData>();
                    data.Add(new TreeMenuData()
                    {
                        id = Guid.NewGuid().ToString(),
                        path = "/",
                        name = "首页",
                        icon = "home",
                        component = "/"
                    });
                    data = data.Concat(treeMenuData.children).ToList();
                    RedisCacheService.AddObject(User.Identity.Name, "UserMenu", data);

                    obj.data = data;
                }

                status = "ok";
                message = "获取成功！";
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

        #region 获取模块信息
        /// <summary>
        /// 获取模块信息
        /// </summary>
        /// <param name="moduleCode">模块代码</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetModuleInfo(string moduleCode)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //获取模块信息
                //var module = _context.SmModules.Where(x => x.IsDeleted == false && x.ModuleCode == moduleCode).SingleOrDefault();
                SmModule module = ModuleInfo.GetModuleInfo(moduleCode);

                if (module == null)
                    throw new Exception("未查询到模块【" + moduleCode + "】相关配置信息！");

                string moduleId = module.ID.ToString().ToLower();
                obj.columns = GetModuleColumn(Guid.Parse(moduleId), module);

                //获取从表信息
                //var detailModules = _context.Set<SmRoleModule>()
                //    .Join(_context.Set<SmUserRole>(), x => x.SmRoleId, y => y.SmRoleId, (x, y) => new { x, y })
                //    .Where(z => z.y.SmUserId.ToString() == User.Identity.Name && z.x.IsDeleted == false &&
                //                z.y.IsDeleted == false)
                //    .Join(_context.SmModules, a => a.x.SmModuleId, b => b.ID, (a, b) => new { a, b }).Select(c => c.b)
                //    .Where(d => d.BelongModuleId == module.ID && d.IsActive == true && d.IsDeleted == false).Distinct()
                //    .ToList();

                List<SmRoleFunction> functions = RedisCacheService.Get<List<SmRoleFunction>>(User.Identity.Name, "UserFunction");
                if (functions == null)
                {
                    functions = await _context.SmUserRole.Where(x => x.IsDeleted == false && x.SmUserId == Guid.Parse(User.Identity.Name))
                 .Join(_context.SmRoleFunction, a => a.SmRoleId, b => b.SmRoleId, (a, b) => new { a, b }).Where(y => y.b.IsDeleted == false)
                 .Select(z => z.b).ToListAsync();

                    RedisCacheService.AddObjectAsync(User.Identity.Name, "UserFunction", functions);
                }

                //获取没有权限的基本按钮
                var noActions = functions.Where(x => !string.IsNullOrEmpty(x.NoActionCode) && x.NoActionCode.Contains(moduleId)).Select(x => x.NoActionCode).ToList();
                List<SmFunctionPrivilege> Privilege = FunctionPrivilege.GetList(moduleId);
                //获取操作栏按钮
                var actionData = functions
                     .Join(Privilege, a => a.SmFunctionId, b => b.ID, (a, b) => new { a, b })
                     .Where(z => z.b.SmModuleId == module.ID && z.b.DisplayPosition == "Action")
                     .Select(y => y.b)
                     .OrderBy(y => y.TaxisNo).ToList();

                //获取菜单栏按钮
                var menuData = functions
                    .Join(Privilege, a => a.SmFunctionId, b => b.ID, (a, b) => new { a, b })
                    .Where(z => z.b.SmModuleId == module.ID && z.b.DisplayPosition == "Menu")
                    .Select(y => y.b)
                    .OrderBy(y => y.TaxisNo).ToList();

                //获取隐藏菜单栏按钮
                var hideMenu = functions
                    .Join(Privilege, a => a.SmFunctionId, b => b.ID, (a, b) => new { a, b })
                    .Where(z => z.b.SmModuleId == module.ID && z.b.DisplayPosition == "HideMenu")
                    .Select(y => y.b)
                    .OrderBy(y => y.TaxisNo).ToList();

                //获取模块没有的按钮
                //var noBasicAction = _context.SmModules.Where(x => x.ModuleCode == moduleCode && x.IsDeleted == false).SingleOrDefault();
                var noBasicAction = module;

                #region 操作栏按钮个数
                List<ActionActions> allAction = new List<ActionActions>();//显示的按钮
                var actionCount = actionData.Count;
                if (!noActions.Contains(moduleId + "Update") && noBasicAction.IsShowUpdate == true)
                {
                    allAction.Add(new ActionActions
                    {
                        id = moduleId + "Update",
                        taxisNo = 100
                    });
                    actionCount = actionCount + 1;
                }
                if (!noActions.Contains(moduleId + "View") && noBasicAction.IsShowView == true)
                {
                    allAction.Add(new ActionActions
                    {
                        id = moduleId + "View",
                        taxisNo = 200
                    });
                    actionCount = actionCount + 1;
                }
                if (!noActions.Contains(moduleId + "Delete") && noBasicAction.IsShowDelete == true)
                {
                    allAction.Add(new ActionActions
                    {
                        id = moduleId + "Delete",
                        taxisNo = 300
                    });
                    actionCount = actionCount + 1;
                }
                #endregion

                if (noBasicAction.IsShowAdd == false)
                {
                    if (!noActions.Contains(moduleId + "Add"))
                        noActions.Add(moduleId + "Add");
                }
                if (noBasicAction.IsShowDelete == false)
                {
                    if (!noActions.Contains(moduleId + "Delete"))
                        noActions.Add(moduleId + "Delete");
                }
                if (noBasicAction.IsShowUpdate == false)
                {
                    if (!noActions.Contains(moduleId + "Update"))
                        noActions.Add(moduleId + "Update");
                }
                if (noBasicAction.IsShowView == false)
                {
                    if (!noActions.Contains(moduleId + "View"))
                        noActions.Add(moduleId + "View");
                }
                if (noBasicAction.IsShowSubmit == false)
                {
                    if (!noActions.Contains(moduleId + "Submit"))
                        noActions.Add(moduleId + "Submit");
                }
                if (noBasicAction.IsShowBatchDelete == false)
                {
                    if (!noActions.Contains(moduleId + "BatchDelete"))
                        noActions.Add(moduleId + "BatchDelete");
                }

                #region 操作栏下拉计算
                List<ActionActions> beforeActions = new List<ActionActions>();
                List<ActionActions> dropActions = new List<ActionActions>();
                //beforeActions = allAction;//操作栏前面平铺按钮

                //for (int i = 0; i < actionData.Count; i++)
                //{
                //    dropActions.Add(new ActionActions
                //    {
                //        id = actionData[i].ID.ToString(),
                //        taxisNo = actionData[i].TaxisNo
                //    });
                //}


                for (int i = 0; i < actionData.Count; i++)
                {
                    allAction.Add(new ActionActions
                    {
                        id = actionData[i].ID.ToString(),
                        taxisNo = actionData[i].TaxisNo
                    });
                }

                var data = allAction;
                if (actionCount > 3)
                {
                    beforeActions = data.Take(2).ToList();
                    dropActions = data.Skip(2).Take(actionCount - 2).ToList();
                }
                else
                {
                    beforeActions = data.Take(3).ToList();
                }
                #endregion

                obj.beforeActions = beforeActions;//操作栏前面平铺按钮
                obj.dropActions = dropActions;//操作栏下拉区域按钮

                obj.noActions = noActions;//不显示的按钮
                obj.actionData = actionData;
                obj.menuData = menuData;//菜单栏按钮
                obj.hideMenu = hideMenu;//菜单栏隐藏区按钮
                obj.actionCount = actionCount;//操作栏个数
                obj.moduleId = moduleId;//模块代码
                obj.isDetail = module.IsDetail;//是否从表
                //obj.detailModules = detailModules;//需要显示的从表
                obj.moduleCode = moduleCode;//模块代码
                obj.moduleName = module.ModuleName;//模块名称
                obj.IsShowAudit = module.IsShowAudit;//是否显示审核

                status = "ok";
                message = "获取成功！";
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

        #region 获取表实体
        /// <summary>
        /// 获取表实体
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, Produces("text/html")]
        public async Task GetTableEntity(string tableName)
        {
            dynamic obj = new ExpandoObject();
            string message = string.Empty;

            try
            {
                string modelName = tableName;
                StringBuilder build = new StringBuilder();
                string sql = @"SELECT A.name table_name,
                                   B.name column_name,
                                   cp.value column_description,
                                   c.data_type
                            FROM sys.tables A
                                 INNER JOIN sys.columns B ON B.object_id = A.object_id
                                 LEFT JOIN sys.extended_properties cp
                                    ON cp.major_id = B.object_id AND cp.minor_id = B.column_id
                                 LEFT JOIN information_schema.columns C
                                    ON A.[name] = C.TABLE_NAME AND B.name = C.COLUMN_NAME
                            WHERE A.name = '" + tableName + "'";

                DataTable dtColumn = DBHelper.Instance.GetDataTable(sql, null);

                build.Append("using System;<br/>");
                build.Append("using System.ComponentModel.DataAnnotations;<br/>");
                build.Append("using System.ComponentModel.DataAnnotations.Schema;<br/>");
                build.Append("<br/>");
                build.Append("namespace EU.Model.System<br/>");
                build.Append("{<br/>");
                build.Append("public class " + modelName + " : Base.PersistPoco<br/>");
                build.Append("{<br/>");


                #region 属性

                #region 处理表字段
                string columnCode = string.Empty;
                string dataType = string.Empty;
                string description = string.Empty;
                for (int i = 0; i < dtColumn.Rows.Count; i++)
                {
                    columnCode = dtColumn.Rows[i]["column_name"].ToString();
                    description = dtColumn.Rows[i]["column_description"].ToString();
                    if (columnCode == "ROW_ID")
                        continue;
                    build.Append("<br/>");
                    build.Append("/// &lt;summary&gt;<br/>");
                    build.Append("/// " + description + "<br/>");
                    build.Append("/// &lt;/summary&gt;<br/>");
                    build.Append("[Display(Name = \"" + description + "\")]<br/>");
                    dataType = dtColumn.Rows[i]["data_type"].ToString();
                    switch (dataType)
                    {
                        #region 字符串
                        case "varchar":
                            {
                                build.Append("public string " + columnCode + " { get; set; }<br/>");
                                //build.Append("private string " + Utility.GetCamelString(columnCode) + ";<br/>");
                                //build.Append("public string " + Utility.GetPascalString(columnCode) + "<br/>");
                                //build.Append("{<br/>");
                                //build.Append("get<br/>");
                                //build.Append("if (string.IsNullOrEmpty(" + Utility.GetCamelString(columnCode) + "))<br/>");
                                //build.Append("{<br/>");
                                //build.Append("return " + Utility.GetCamelString(columnCode) + ";<br/>");
                                //build.Append("}<br/>");
                                //build.Append("else<br/>");
                                //build.Append("{<br/>");
                                //build.Append("return " + Utility.GetCamelString(columnCode) + ";<br/>");
                                //build.Append("}<br/>");
                                //build.Append("}<br/>");
                                //build.Append("set { " + Utility.GetCamelString(columnCode) + " = value; }<br/>");
                                break;
                            }
                        case "char":
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
                        #endregion
                        #region 数字
                        case "decimal":
                            {

                                build.Append("public decimal " + columnCode + " { get; set; }<br/>");
                            }
                            break;
                        case "int":
                            {
                                build.Append("public int " + columnCode + " { get; set; }<br/>");

                                break;
                            }
                        #endregion

                        #region Guid_ID
                        case "uniqueidentifier":
                            {

                                build.Append("public Guid? " + columnCode + " { get; set; }<br/>");
                            }
                            break;
                        #endregion

                        #region Guid_ID
                        case "bit":
                            {

                                build.Append("public bool " + columnCode + " { get; set; }<br/>");
                            }
                            break;
                        #endregion

                        case "text":
                            {

                                build.Append("public string " + columnCode + " { get; set; }<br/>");
                            }
                            break;

                    }
                }
                #endregion


                build.Append("}<br/>");
                build.Append("}<br/>");
                #endregion

                //return Content(build.ToString(), "text/html", Encoding.UTF8);
                var data = Encoding.UTF8.GetBytes(build.ToString());
                //if (accept.Any(x => x.MediaType == "text/html"))
                //{

                //}
                //else
                //{
                //    Response.ContentType = "text/plain";
                //}
                Response.ContentType = "text/html;charset=UTF-8";
                await Response.Body.WriteAsync(data, 0, data.Length);
            }
            catch (Exception E)
            {
                message = E.Message;
            }

        }
        #endregion

        #region 获取模块列
        /// <summary>
        /// 获取模块列
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <param name="moduleInfo">模块</param>
        /// <returns></returns>

        [NonAction]
        public JArray GetModuleColumn(Guid moduleId, SmModule moduleInfo)
        {
            JArray column = new JArray();
            ModuleSqlColumn moduleColumnInfo = new ModuleSqlColumn(moduleInfo.ModuleCode);
            List<SmModuleColumn> moduleColumns = moduleColumnInfo.GetModuleSqlColumn();
            //moduleColumns = moduleColumns.OrderBy(y => y.TaxisNo).ToList();
            var data = moduleColumns;
            if (moduleColumns.Count == 0 || moduleColumns == null)
            {
                data = _context.SmModuleColumn
               .Where(x => x.SmModuleId == moduleId && x.IsDeleted == false).OrderBy(x => x.TaxisNo).ToList();
            }

            for (int i = 0; i < data.Count; i++)
            {
                JObject item = new JObject();
                item.Add(new JProperty("title", data[i].title));
                item.Add(new JProperty("dataIndex", data[i].dataIndex.Split(",")));
                item.Add(new JProperty("hideInTable", data[i].hideInTable));
                if (data[i].width != null)
                    item.Add(new JProperty("width", data[i].width));
                item.Add(new JProperty("sorter", data[i].sorter));
                if (data[i].valueType != null && string.IsNullOrEmpty(data[i].DataFormate))
                    item.Add(new JProperty("valueType", data[i].valueType));
                if (moduleInfo.DefaultSort == data[i].dataIndex)
                {
                    item.Add(new JProperty("defaultSortOrder", moduleInfo.DefaultSortOrder));
                }

                if (data[i].IsBool)
                {
                    JObject enumobj = new JObject();
                    enumobj.Add(new JProperty("true", new JObject(new JProperty("text", "是"))));
                    enumobj.Add(new JProperty("false", new JObject(new JProperty("text", "否"))));
                    item.Add(new JProperty("valueEnum", enumobj));
                    item.Add(new JProperty("filters", false));
                }
                else if (data[i].IsLovCode)
                {
                    JObject enumobj = new JObject();
                    //var enumData = _context.SmLov.Where(x => x.LovCode == data[i].dataIndex && x.IsDeleted == false)
                    //    .Join(_context.SmLovDetail, a => a.ID, b => b.SmLovId, (a, b) => new { a, b })
                    //    .Where(x => x.b.IsDeleted == false).Select(x => x.b).OrderBy(x => x.TaxisNo).ToList();

                    var enumData = LOVHelper.GetLovList(data[i].dataIndex).ToList();

                    if (enumData.Count() > 0)
                    {
                        for (int n = 0; n < enumData.Count(); n++)
                        {
                            enumobj.Add(new JProperty(enumData[n].Value, new JObject(new JProperty("text", enumData[n].Text))));
                        }
                        item.Add(new JProperty("valueEnum", enumobj));
                        item.Add(new JProperty("filters", false));
                    }
                }

                if (data[i].hideInSearch)
                    item.Add(new JProperty("hideInSearch", true));

                if ((!string.IsNullOrWhiteSpace(data[i].QueryValue) || !string.IsNullOrWhiteSpace(data[i].QueryValueType)) && data[i].hideInSearch != true)
                {
                    item.Add(new JProperty("hideInSearch", true));

                    JObject searchItem = new JObject();
                    searchItem.Add(new JProperty("title", data[i].title));
                    searchItem.Add(new JProperty("dataIndex", !string.IsNullOrWhiteSpace(data[i].QueryValue) ? data[i].QueryValue : data[i].dataIndex));
                    searchItem.Add(new JProperty("valueType", !string.IsNullOrWhiteSpace(data[i].QueryValueType) ? data[i].QueryValueType : data[i].valueType));
                    searchItem.Add(new JProperty("hideInTable", true));
                    column.Add(searchItem);
                }

                column.Add(item);
            }


            return column;
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

            //#region 检查是否存在相同的编码
            Utility.CheckCodeExist("", "SmModules", "ModuleCode", modelModify.ModuleCode.Value, ModifyType.Edit, modelModify.ID.Value, "模块代码");
            //#endregion


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Update<SmModule>(modelModify);
                _context.SaveChanges();

                ModuleInfo.Init();

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

        #region 获取模块日志信息
        /// <summary>
        /// 获取模块日志信息
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ServiceResult<dynamic>> GetModuleLogInfo(string moduleCode, string id)
        {
            try
            {
                dynamic data = new ExpandoObject();
                //获取模块信息
                var module = await _context.SmModules.AsNoTracking().Where(x => x.IsDeleted == false && x.ModuleCode == moduleCode).FirstOrDefaultAsync();

                if (module == null)
                    throw new Exception("未查询到模块【" + moduleCode + "】相关配置信息！");

                ModuleSql moduleSql = new ModuleSql(moduleCode);
                string tableName = moduleSql.GetTableName();
                data.tableName = tableName;
                data.ID = id;
                data.CreatedBy = null;
                data.CreatedTime = null;
                data.UpdateBy = null;
                data.UpdateTime = null;

                if (!string.IsNullOrEmpty(tableName))
                {
                    string sql = @"SELECT A.ID,
                                           B.UserName CreatedBy,
                                           A.CreatedTime,
                                           C.UserName UpdateBy,
                                           A.UpdateTime
                                    FROM {1} A
                                         LEFT JOIN SmUsers B ON A.CreatedBy = B.ID
                                         LEFT JOIN SmUsers C ON A.UpdateBy = C.ID
                                    WHERE A.ID = '{0}' AND A.IsDeleted = 'false'";
                    sql = string.Format(sql, id, tableName);
                    DataTable dt = await DBHelper.Instance.GetDataTableAsync(sql);
                    if (dt.Rows.Count > 0)
                    {
                        data.CreatedBy = dt.Rows[0]["CreatedBy"].ToString();
                        data.CreatedTime = dt.Rows[0]["CreatedTime"].ToString();
                        data.UpdateBy = dt.Rows[0]["UpdateBy"].ToString();
                        data.UpdateTime = dt.Rows[0]["UpdateTime"].ToString();
                    }
                }

                return ServiceResult<dynamic>.OprateSuccess(data, ResponseText.QUERY_SUCCESS);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 导出模块SQL
        /// <summary>
        /// 导出模块SQL
        /// </summary>
        /// <param name="list">ids</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ExportModuleSqlScript(List<SmModule> list)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            dynamic data = new ExpandoObject();

            try
            {
                string sql = string.Empty;
                if (list.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    StringBuilder temp;
                    DBHelper dBHelper = new DBHelper();
                    foreach (SmModule item in list)
                    {
                        sb.Append("DELETE FROM SmModuleColumn WHERE SmModuleId='" + item.ID + "';\n");
                        sb.Append("DELETE FROM SmModuleSql WHERE ModuleId='" + item.ID + "';\n");
                        sb.Append("DELETE FROM SmModules WHERE ID='" + item.ID + "';\n");

                        temp = dBHelper.GetInsertSql("SmModules", "ID", item.ID.ToString());
                        sb.Append(temp.ToString());
                        temp = dBHelper.GetInsertSql("SmModuleColumn", "SmModuleId", item.ID.ToString());
                        sb.Append(temp.ToString());
                        temp = dBHelper.GetInsertSql("SmModuleSql", "ModuleId", item.ID.ToString());
                        sb.Append(temp.ToString() + "\n");
                    }

                    string fileName = $"系统模块.sql";
                    string folder = Utility.GetSysID();
                    string savePath = $"/Download/SqlExport/{folder}/";
                    if (!Directory.Exists("wwwroot" + savePath))
                        Directory.CreateDirectory("wwwroot" + savePath);
                    FileHelper.WriteFile("wwwroot" + savePath, fileName, sb.ToString());

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
                    di.Values("FileExt", "sql");
                    di.Values("Path", savePath);
                    DBHelper.Instance.ExcuteNonQuery(di.GetSql(), null);
                    #endregion

                    //return responseContent.OK("导出成功！", (savePath + "/" + fileName).EncryptDES(AppSetting.Secret.ExportFile));
                }
                status = "ok";
                message = "导出成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.data = data;
            obj.message = message;
            return Ok(obj);
        }
        #endregion

    }

    public class TreeMenuData
    {
        public string id { get; set; }
        public string path { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public string component { get; set; }
        public List<TreeMenuData> children { get; set; }
        public string moduleCode { get; set; }
    }

    //操作栏按钮
    public class ActionActions
    {
        public string id { get; set; }
        public int taxisNo { get; set; }
    }
}
