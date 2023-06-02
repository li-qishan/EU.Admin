using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.Core.CacheManager;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Domain.System;
using EU.Model.System.Privilege;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.Privilege
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmRoleFunctionController : BaseController<SmRoleFunction>
    {
        RedisCacheService RedisCacheService = new RedisCacheService(1);

        public SmRoleFunctionController(DataContext _context, IBaseCRUDVM<SmRoleFunction> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 获取按钮权限
        [HttpGet]
        public IActionResult GetModuleFunction(Guid roleId, string moduleId)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                string moduleCode = string.Empty;

                List<functionData> functionData = new List<functionData>();
                List<string> checkValue = new List<string>();

                if (!string.IsNullOrEmpty(moduleId) && moduleId != "All")
                {
                    //moduleCode = _context.SmModules.Where(x => x.ID == Guid.Parse(moduleId)).SingleOrDefault().ModuleCode;

                    //newFuncData = _context.SmFunctionPrivilege.Where(x => x.IsDeleted == false && x.SmModuleId == Guid.Parse(moduleId))
                    //    .OrderBy(x => x.TaxisNo).Select(y => new functionData
                    //    {
                    //        label = y.FunctionName,
                    //        value = y.ID.ToString().ToLower()
                    //    }).ToList();

                    functionData.Add(new functionData
                    {
                        label = "新建",
                        value = moduleId + "Add"
                    });
                    checkValue.Add(moduleId + "Add");

                    functionData.Add(new functionData
                    {
                        label = "修改",
                        value = moduleId + "Update"
                    });
                    checkValue.Add(moduleId + "Update");

                    functionData.Add(new functionData
                    {
                        label = "查看",
                        value = moduleId + "View"
                    });
                    checkValue.Add(moduleId + "View");

                    functionData.Add(new functionData
                    {
                        label = "删除",
                        value = moduleId + "Delete"
                    });
                    checkValue.Add(moduleId + "Delete");

                    functionData.Add(new functionData
                    {
                        label = "批量删除",
                        value = moduleId + "BatchDelete"
                    });
                    checkValue.Add(moduleId + "BatchDelete");

                    functionData.Add(new functionData
                    {
                        label = "导出Excel",
                        value = moduleId + "ExportExcel"
                    });
                    checkValue.Add(moduleId + "ExportExcel");

                    functionData.Add(new functionData
                    {
                        label = "导入Excel",
                        value = moduleId + "ImportExcel"
                    });
                    checkValue.Add(moduleId + "ImportExcel");
                }

                var noAction = _context.SmRoleFunction.Where(x =>
                    x.IsDeleted == false && x.SmRoleId == roleId && checkValue.Contains(x.NoActionCode)).Select(y => y.NoActionCode).ToList();

                foreach (var novalue in noAction)
                {
                    checkValue.Remove(novalue);
                }

                obj.checkValue = checkValue;
                obj.functionData = functionData;

                status = "ok";
                message = "查询成功！";
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

        #region 保存基础按钮权限
        [HttpPost]
        public IActionResult SaveRoleFunction(RoleFuncVM roleFuncVm)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                string RoleId = roleFuncVm.RoleId;

                var noActionCodes = _context.SmRoleFunction.Where(x => x.IsDeleted == false && x.SmRoleId == Guid.Parse(RoleId))
                    .Select(y => y.NoActionCode).ToList();

                for (int i = 0; i < roleFuncVm.RoleFuncData.Count; i++)
                {
                    if (!noActionCodes.Contains(roleFuncVm.RoleFuncData[i]))
                    {
                        SmRoleFunction smRoleFunction = new SmRoleFunction();
                        smRoleFunction.CreatedBy = Guid.Parse(User.Identity.Name);
                        smRoleFunction.CreatedTime = Utility.GetSysDate();
                        smRoleFunction.SmRoleId = Guid.Parse(RoleId);
                        smRoleFunction.NoActionCode = roleFuncVm.RoleFuncData[i];
                        _context.Add(smRoleFunction);
                    }
                }

                for (int i = 0; i < roleFuncVm.AddAction.Count; i++)
                {
                    SmRoleFunction smRoleFunction = _context.SmRoleFunction
                        .Where(x => x.IsDeleted == false && x.NoActionCode == roleFuncVm.AddAction[i] && x.SmRoleId == Guid.Parse(RoleId))
                        .SingleOrDefault();
                    smRoleFunction.IsDeleted = true;
                    _context.Update(smRoleFunction);
                }

                _context.SaveChanges();

                RedisCacheService.Clear();

                status = "ok";
                message = "保存成功！";
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

        #region 获取功能权限定义

        [HttpGet]
        public IActionResult GetAllFuncPriv()
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                DataTree roleTree = new DataTree();
                roleTree.key = "All";
                roleTree.title = "请选择功能定义";
                roleTree.children = _context.SmFunctionPrivilege.Where(x => x.IsDeleted == false).Select(y => new DataTree
                {
                    title = y.SmModule.ModuleName + "/" + y.FunctionName,
                    key = y.ID.ToString().ToLower(),
                    isLeaf = true
                }).ToList();

                obj.data = roleTree;

                status = "ok";
                message = "查询成功！";
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

        #region 保存功能权限定义

        [HttpPost]
        public IActionResult SaveRoleFuncPriv(RoleFuncPric roleFuncPric)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                var functionList = roleFuncPric.FunctionList;
                var roleId = roleFuncPric.RoleId;

                if (functionList.Contains("All"))
                {
                    functionList.Remove("All");
                }

                //删除不包含新数据的数据
                var deleteFuncPrivs = _context.SmRoleFunction
                    .Where(x => x.IsDeleted == false && x.SmFunctionId != null && x.SmRoleId == roleId && !functionList.Contains(x.SmFunctionId.ToString())).ToList();
                for (int i = 0; i < deleteFuncPrivs.Count; i++)
                {
                    deleteFuncPrivs[i].IsDeleted = true;
                    _context.Update(deleteFuncPrivs[i]);
                }

                //查询相同的数据
                var sameFuncPriv = _context.SmRoleFunction
                    .Where(x => x.IsDeleted == false && x.SmFunctionId != null && x.SmRoleId == roleId && functionList.Contains(x.SmFunctionId.ToString())).ToList();
                for (int i = 0; i < sameFuncPriv.Count; i++)
                {
                    functionList.Remove(sameFuncPriv[i].SmFunctionId.ToString());
                }

                //添加剩下的数据
                for (int i = 0; i < functionList.Count; i++)
                {
                    SmRoleFunction smRoleFunction = new SmRoleFunction();
                    smRoleFunction.SmRoleId = roleId;
                    smRoleFunction.SmFunctionId = Guid.Parse(functionList[i]);
                    _context.Add(smRoleFunction);
                }

                _context.SaveChanges();

                status = "ok";
                message = "功能定义保存成功！";
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

        #region 获取角色功能定义

        [HttpGet]
        public IActionResult GetRoleFuncPriv(Guid RoleId)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                obj.data = _context.SmRoleFunction.Where(x => x.IsDeleted == false && x.SmRoleId == RoleId && x.SmFunctionId != null)
                    .Select(y => y.SmFunctionId).ToList();

                status = "ok";
                message = "查询成功！";
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
    #region 基础按钮
    public class functionData
    {
        public string label { get; set; }
        public string value { get; set; }
    }

    public class RoleFuncVM
    {
        public List<string> RoleFuncData { get; set; }
        public List<string> AddAction { get; set; }
        public string RoleId { get; set; }
    }
    #endregion

    #region 自定义功能

    public class DataTree
    {
        public string title { get; set; }

        public string key { get; set; }

        public bool isLeaf { get; set; }

        public List<DataTree> children { get; set; }
    }

    public class RoleFuncPric
    {
        public List<string> FunctionList { get; set; }
        public Guid RoleId { get; set; }
    }
    #endregion
}
