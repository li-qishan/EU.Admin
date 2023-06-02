using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AgileObjects.AgileMapper;
using EU.Core.Const;
using EU.Core.Entry;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using EU.Model.System;
using EU.Model.System.Setup;
using EU.Model.System.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.Setup
{
    /// <summary>
    /// 系统参数配置
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmConfigController : BaseController<SmConfig>
    {
        private readonly IConfiguration Configuration;
        public SmConfigController(IConfiguration configuration, DataContext _context, IBaseCRUDVM<SmConfig> BaseCrud) : base(_context, BaseCrud)
        {
            Configuration = configuration;
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(SmConfig Model)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //#region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "SmConfig", "ConfigCode", Model.ConfigCode, ModifyType.Add, null, "参数代码");
                //#endregion
                ConfigCache.Add(Model.ConfigCode, Model);

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
        /// <summary>
        /// 更新重写
        /// </summary>
        /// <param name="modelModify"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Update(dynamic modelModify)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {

                //#region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "SmConfig", "ConfigCode", modelModify.ConfigCode.Value, ModifyType.Edit, modelModify.ID.Value, "参数代码");
                //#endregion

                Update<SmConfig>(modelModify);
                _context.SaveChanges();

                ConfigCache.Init();

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

        #region 获取值
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="code">参数代码</param>
        /// <returns></returns>
        [HttpGet, Route("{code}")]
        public IActionResult GetByCode(string code)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                string value = ConfigCache.GetValue(code);

                status = "ok";
                obj.value = value;
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

        /// <summary>
        /// 系统参数明细 -- 根据参数分组查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ServiceResult<List<SmConfigView>>> GetListByGroup()
        {
            dynamic obj = new ExpandoObject();
            var groups = await _context.SmConfigGroup.OrderBy(o => o.Sequence).Select(o => new SmConfigView()
            {
                ID = o.ID,
                ParentId = o.ParentId,
                Name = o.Name,
                Type = o.Type
            }).ToListAsync();
            var configs = await _context.SmConfig.OrderBy(o => o.Sequence).ToListAsync();
            //var views = customerDto.Map().OnTo(customer);
            groups?.ForEach(o =>
            {
                o.detail = configs.Where(x => x.ConfigGroupId == o.ID).ToList();
            });
            return ServiceResult<List<SmConfigView>>.OprateSuccess(groups, ResponseText.QUERY_SUCCESS);
            //return Ok(obj);
            //return await _systemSettingItemService.GetListByGroupAsync(groupId);
        }

    }
}
