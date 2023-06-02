using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.BD
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.Base)]
    public class MaterialTypeController : BaseController<MaterialType>
    {
        public MaterialTypeController(DataContext _context, IBaseCRUDVM<MaterialType> BaseCrud) : base(_context, BaseCrud)
        {

        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(MaterialType Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "BdMaterialType", "MaterialTypeNo", Model.MaterialTypeNo, ModifyType.Add, null, "类型编号");
                #endregion

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

        #region 更新重写
        [HttpPost]
        public override IActionResult Update(dynamic modelModify)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "BdMaterialType", "MaterialTypeNo", modelModify.MaterialTypeNo.Value, ModifyType.Edit, modelModify.ID.Value, "类型编号");
                #endregion

                Update<MaterialType>(modelModify);
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

        #region 一次性加载
        [NonAction]
        public void LoopToAppendChildren(List<MaterialType> list, ModuleTree moduleTree)
        {
            List<ModuleTree> subItems = new List<ModuleTree>();
            if (moduleTree.key == "All")
            {
                subItems = list.Where(x => string.IsNullOrEmpty(x.ParentTypeId.ToString())).OrderBy(x => x.TaxisNo).Select(y => new ModuleTree
                {
                    title = y.MaterialTypeNames,
                    key = y.ID.ToString().ToLower()
                }).ToList();
            }
            else
            {
                subItems = list.Where(x => x.ParentTypeId == Guid.Parse(moduleTree.key)).Select(y => new ModuleTree
                {
                    title = y.MaterialTypeNames,
                    key = y.ID.ToString().ToLower(),
                }).ToList();
            }
            moduleTree.children = new List<ModuleTree>();
            moduleTree.children.AddRange(subItems);
            foreach (var subItem in subItems)
            {
                LoopToAppendChildren(list, subItem);
            }
        }

        [HttpGet]
        public IActionResult GetAllMaterialType()
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                ModuleTree moduleTree = new ModuleTree();
                moduleTree.key = "All";
                moduleTree.title = "物料类型";

                List<MaterialType> list = _context.BdMaterialType.Where(x => x.IsDeleted == false).ToList();
                list = list.OrderBy(y => y.TaxisNo).ToList();
                LoopToAppendChildren(list, moduleTree);

                obj.data = moduleTree;

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
    public class ModuleTree
    {
        public string title { get; set; }

        public string key { get; set; }
        
        public List<ModuleTree> children { get; set; }
    }

}
