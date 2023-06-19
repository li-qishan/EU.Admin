using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using EU.Core;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using EU.Model.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static EU.Core.Const.Consts;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.System.Import
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmImpTemplateController : BaseController1<SmImpTemplate>
    {

        public SmImpTemplateController(DataContext _context, IBaseCRUDVM<SmImpTemplate> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 按模块代码获取数据
        [HttpGet]
        public IActionResult GetByModuleCode(string moduleCode)
        {
            dynamic obj = new ExpandoObject();
            dynamic data = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            try
            {
                obj.fileId = "";

                SmImpTemplate Template = _context.SmImpTemplate.Where(x => x.ModuleCode == moduleCode).SingleOrDefault();
                if (Template != null)
                {
                    string sql = @"SELECT *
                        FROM FileAttachment
                        WHERE MasterId = '{0}' AND IsDeleted='false'
                        ORDER BY CreatedTime DESC";
                    sql = string.Format(sql, Template.ID);
                    FileAttachment attachment = DBHelper.Instance.QueryFirst<FileAttachment>(sql);
                    if (attachment != null)
                        obj.fileId = attachment.ID;
                }

                data = Template;
                status = "ok";
            }
            catch (Exception E)
            {
                message = E.Message;
            }
            obj.data = data;
            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }
        #endregion
    }
}
