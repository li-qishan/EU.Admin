using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using EU.Model.System;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.CompanyStructure
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmEmployeeController : BaseController1<SmEmployee>
    {
        public SmEmployeeController(DataContext _context, IBaseCRUDVM<SmEmployee> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(SmEmployee Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "SmEmployee", "EmployeeCode", Model.EmployeeCode, ModifyType.Add, null, "员工代码");
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
                Utility.CheckCodeExist("", "SmEmployee", "EmployeeCode", modelModify.EmployeeCode.Value, ModifyType.Edit, modelModify.ID.Value, "员工代码");
                #endregion

                Update<SmEmployee>(modelModify);
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

        [HttpGet]
        public virtual IActionResult GetParentList(string paramData= null)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            int current = 1;
            int pageSize = 20;
            int total = 0;

            string defaultSorter = "EmployeeCode";
            string sortType = string.Empty;

            try
            {
                IQueryable<SmEmployee> query = null;
                query = _context.Set<SmEmployee>();

                var lamadaExtention = new LamadaExtention<SmEmployee>();
              
                lamadaExtention.GetExpression("IsDeleted", "false", ExpressionType.Equal);

                var searchParam = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(paramData);
                foreach (var item in searchParam)
                {
                    if (item.Key == "ID")
                    {
                        lamadaExtention.GetExpression("ID", item.Value.ToString(), ExpressionType.NotEqual);
                        continue;
                    }
                }

                if (lamadaExtention.GetLambda() != null)
                    query = query.Where(lamadaExtention.GetLambda());

                query = query.OrderBy(LamadaExtention<SmEmployee>.SortLambda<SmEmployee, string>(defaultSorter, "EmployeeName"));

                obj.data = query.ToList();


                total = query.Count();

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

    }
}
