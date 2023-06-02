using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Domain.System;
using EU.Model.System;
using EU.Model.System.Privilege;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.Privilege
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmUserRoleController : BaseController<SmUserRole>
    {
        public SmUserRoleController(DataContext _context, IBaseCRUDVM<SmUserRole> BaseCrud) : base(_context, BaseCrud)
        {
        }

        [HttpPost]
        public IActionResult BatchInsertUserRole(UserRoleVM userRoleVm)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                var roleList = userRoleVm.RoleList;
                var UserId = userRoleVm.UserId;
                if (roleList.Contains("All"))
                {
                    roleList.Remove("All");
                }

                var deleteData = _context.Set<SmUserRole>().Where(x =>
                    x.IsDeleted == false & x.SmUserId == UserId & !roleList.Contains(x.SmRoleId.ToString())).ToList();
                for (int i = 0; i < deleteData.Count; i++)
                {
                    deleteData[i].IsDeleted = true;
                    _context.Update(deleteData[i]);
                }

                var data = _context.Set<SmUserRole>().Where(x =>
                    x.IsDeleted == false & x.SmUserId == UserId & roleList.Contains(x.SmRoleId.ToString())).ToList();
                for (int i = 0; i < data.Count; i++)
                {
                    roleList.Remove(data[i].SmRoleId.ToString());
                }

                for (int i = 0; i < roleList.Count; i++)
                {
                    SmUserRole smUserRole = new SmUserRole();
                    smUserRole.SmRoleId = Guid.Parse(roleList[i].ToString());
                    smUserRole.SmUserId = UserId;
                    _context.Add(smUserRole);
                }

                _context.SaveChanges();

                EU.Core.Utilities.Utility.ClearCache();

                status = "ok";
                message = "用户角色保存成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        [HttpGet]
        public IActionResult GetRoleList()
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                RoleTree roleTree = new RoleTree();
                roleTree.key = "All";
                roleTree.title = "请选择用户的功能角色";
                roleTree.children = _context.Set<SmRole>().Where(x => x.IsDeleted == false & x.IsActive == true).OrderByDescending(x => x.CreatedTime).Select(x => new RoleTree
                {
                    title = x.RoleName,
                    key = x.ID.ToString().ToLower(),
                    isLeaf = true,
                    children = new List<RoleTree>()

                }).ToList();

                obj.data = roleTree;

                status = "ok";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        [HttpGet]
        public IActionResult GetUserRole(Guid UserId)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                obj.data = _context.Set<SmUserRole>().Where(x => x.SmUserId == UserId && x.IsDeleted == false);
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
    }
    public class RoleTree
    {
        public string title { get; set; }

        public string key { get; set; }

        public bool isLeaf { get; set; }

        public List<RoleTree> children { get; set; }
    }

    public class UserRoleVM
    {
        public List<string> RoleList { get; set; }
        public Guid UserId { get; set; }
    }
}
