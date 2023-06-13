using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.Core.Const;
using EU.Core.Entry;
using EU.Core.Module;
using EU.Core.Services;
using EU.DataAccess;
using EU.Domain;
using EU.Domain.System;
using EU.Model.System;
using EU.Model.System.Privilege;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.Privilege
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmRoleModuleController : BaseController1<SmRoleModule>
    {
        public SmRoleModuleController(DataContext _context, IBaseCRUDVM<SmRoleModule> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 添加模块权限
        [NonAction]
        public void SaveParentModule(Guid roleId, Guid parentId)
        {
            List<SmModule> result = _context.SmModules.Where(x => x.ParentId == parentId).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].IsParent == true)
                {
                    SaveParentModule(roleId, result[i].ID);
                }
                else
                {
                    SmRoleModule smRoleModule = new SmRoleModule();
                    smRoleModule.SmModuleId = result[i].ID;
                    smRoleModule.SmRoleId = roleId;
                    _context.Add(smRoleModule);
                }
            }
        }

        [HttpPost]
        public IActionResult BatchInsertRoleModule(RoleModuleVM roleModuleVm)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                var moduleList = roleModuleVm.ModuleList;
                var roleId = roleModuleVm.RoleId;
                if (moduleList.Contains("All"))
                {
                    var data = _context.Set<SmModule>().Where(x => x.IsDeleted == false).ToList();
                    for (int i = 0; i < data.Count; i++)
                    {
                        SmRoleModule smRoleModule = new SmRoleModule();
                        smRoleModule.SmModuleId = data[i].ID;
                        smRoleModule.SmRoleId = roleId;
                        _context.Add(smRoleModule);
                    }
                }
                else
                {
                    var deleteData = _context.Set<SmRoleModule>().Where(x =>
                        x.IsDeleted == false & x.SmRoleId == roleId & !moduleList.Contains(x.SmModuleId.ToString())).ToList();
                    for (int i = 0; i < deleteData.Count; i++)
                    {
                        deleteData[i].IsDeleted = true;
                        _context.Update(deleteData[i]);
                    }

                    var data = _context.Set<SmRoleModule>().Where(x =>
                        x.IsDeleted == false & x.SmRoleId == roleId & moduleList.Contains(x.SmModuleId.ToString())).ToList();
                    for (int i = 0; i < data.Count; i++)
                    {
                        moduleList.Remove(data[i].SmModuleId.ToString());
                    }

                    for (int i = 0; i < moduleList.Count; i++)
                    {
                        //如果是父目录
                        var result = _context.SmModules.Where(x => x.ID == Guid.Parse(moduleList[i].ToString())).SingleOrDefault();
                        if (result.IsParent == true)
                        {
                            SaveParentModule(roleId, result.ID);
                        }
                        else
                        {
                            SmRoleModule smRoleModule = new SmRoleModule();
                            smRoleModule.SmModuleId = Guid.Parse(moduleList[i].ToString());
                            smRoleModule.SmRoleId = roleId;
                            _context.Add(smRoleModule);
                        }
                    }
                }

                _context.SaveChanges();
                EU.Core.Utilities.Utility.ClearCache();
                status = "ok";
                message = "角色模块保存成功！";
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

        #region 受控加载
        [HttpGet]
        public IActionResult GetModuleList(Guid parentKey)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                if (parentKey == Guid.Empty)
                {
                    ModuleTree moduleTree = new ModuleTree();
                    moduleTree.key = "All";
                    moduleTree.title = "请选择角色模块";
                    moduleTree.children = _context.Set<SmModule>().Where(x =>
                        x.IsDeleted == false & x.IsActive == true & x.IsParent == true &
                        string.IsNullOrEmpty(x.ParentId.ToString())).OrderBy(x => x.TaxisNo).Select(x => new ModuleTree
                        {
                            title = x.ModuleName,
                            key = x.ID.ToString().ToLower(),
                            isLeaf = !x.IsParent
                            //children = new List<ModuleTree>()
                        }).ToList();
                    obj.data = moduleTree;
                }
                else
                {
                    obj.data = _context.Set<SmModule>()
                        .Where(y => y.IsDeleted == false & y.IsActive == true & y.ParentId == parentKey).OrderBy(x => x.TaxisNo).Select(y =>
                            new ModuleTree
                            {
                                title = y.ModuleName,
                                key = y.ID.ToString().ToLower(),
                                isLeaf = !y.IsParent
                            }).ToList();
                }

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

        #region 一次性加载
        [NonAction]
        public void LoopToAppendChildren(List<SmModule> smModules, ModuleTree moduleTree)
        {
            List<ModuleTree> subItems = new List<ModuleTree>();
            if (moduleTree.key == "All")
            {
                subItems = smModules.Where(x => x.IsParent == true && string.IsNullOrEmpty(x.ParentId.ToString())).OrderBy(x => x.TaxisNo).ThenBy(x => x.ModuleName).Select(y => new ModuleTree
                {
                    title = y.ModuleName,
                    key = y.ID.ToString().ToLower(),
                    isLeaf = !y.IsParent
                }).ToList();
            }
            else
            {
                subItems = smModules.Where(x => x.ParentId == Guid.Parse(moduleTree.key)).OrderBy(x => x.TaxisNo).ThenBy(x => x.ModuleName).Select(y => new ModuleTree
                {
                    title = y.IsDetail && y.BelongModuleId != null ? ModuleInfo.GetModuleNameById(y.BelongModuleId) + "/" + y.ModuleName : y.ModuleName,
                    key = y.ID.ToString().ToLower(),
                    isLeaf = !y.IsParent
                }).ToList();
            }
            moduleTree.children = new List<ModuleTree>();
            moduleTree.children.AddRange(subItems);
            foreach (var subItem in subItems)
            {
                LoopToAppendChildren(smModules, subItem);
            }
        }

        [HttpGet]
        public async Task<ServiceResult<ModuleTree>> GetAllModuleList()
        {
            try
            {

                ModuleTree moduleTree = new ModuleTree();
                moduleTree.key = "All";
                moduleTree.title = "请选择角色模块";

                List<SmModule> smModules = await _context.SmModules
                    .OrderBy(x => x.TaxisNo)
                    .ThenBy(x => x.ModuleName)
                    .Where(x => x.IsDeleted == false)
                    .ToListAsync();
                LoopToAppendChildren(smModules, moduleTree);

                return ServiceResult<ModuleTree>.OprateSuccess(moduleTree, ResponseText.QUERY_SUCCESS);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        public async Task<ServiceResult<List<SmRoleModule>>> GetRoleModule(Guid RoleId)
        {
            try
            {

                var roleModules = await _context.SmRoleModule.Where(x => x.SmRoleId == RoleId && x.IsDeleted == false && x.SmModule.IsParent == false).ToListAsync();
                return ServiceResult<List<SmRoleModule>>.OprateSuccess(roleModules, ResponseText.QUERY_SUCCESS);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    public class ModuleTree
    {
        public string title { get; set; }

        public string key { get; set; }

        public bool isLeaf { get; set; }

        public List<ModuleTree> children { get; set; }
    }

    public class RoleModuleVM
    {
        public List<string> ModuleList { get; set; }

        public Guid RoleId { get; set; }
    }
}
