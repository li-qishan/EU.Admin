using System;
using System.Dynamic;
using EU.Core.Entry;
using System.Threading.Tasks;
using EU.Core.Enums;
using EU.Core.UserManager;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;
using EU.TaskHelper;
using EU.Core.Const;
using System.Linq;

namespace EU.Web.Controllers.SM
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.Base)]
    public class SmApiLogController : BaseController<SmApiLog>
    {

        public SmApiLogController(DataContext _context, IBaseCRUDVM<SmApiLog> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override async Task<ServiceResult<string>> Add(SmApiLog Model)
        {
            try
            {
                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdSmApiLog", "SmApiLogNo", Model.SmApiLogNo, ModifyType.Add, null, "材质编号");
                //#endregion

                DoAddPrepare(Model);
                await _BaseCrud.DoAddAsync(Model);

                var Id = Model.GetType().GetProperties().Where(x => x.Name.ToLower() == "id").FirstOrDefault()
                    ?.GetValue(Model).ToString();

                return ServiceResult<string>.OprateSuccess(Id, ResponseText.INSERT_SUCCESS);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 更新重写
        [HttpPost]
        public override async Task<ServiceResult> Update(dynamic modelModify)
        {

            try
            {
                Update<SmApiLog>(modelModify);
                await _context.SaveChangesAsync();
                return ServiceResult.OprateSuccess(ResponseText.UPDATE_SUCCESS);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
