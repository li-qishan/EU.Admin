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
    public class SettlementWayController : BaseController<SettlementWay>
    {
        public SettlementWayController(DataContext _context, IBaseCRUDVM<SettlementWay> BaseCrud) : base(_context, BaseCrud)
        {

        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(SettlementWay Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                #region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "BdSettlementWay", "SettlementNo", Model.SettlementNo, ModifyType.Add, null, "结算编号");
                #endregion

                List<LovInfo> enumData = LOVHelper.GetLovList("SettlementAccountType").ToList();

                if (enumData.Any())
                {
                    string SettlementName = string.Empty;
                    LovInfo info = enumData.Where(x => x.Value == Model.SettlementAccountType).SingleOrDefault();
                    if (info != null)
                        SettlementName = info.Text;
                    if (Model.Days > 0)
                        SettlementName += ",付款天数为" + Model.Days + "天";
                    Model.SettlementName = SettlementName;
                }

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
                Utility.CheckCodeExist("", "BdSettlementWay", "SettlementNo", modelModify.SettlementNo.Value, ModifyType.Edit, modelModify.ID.Value, "结算编号");
                #endregion

                List<LovInfo> enumData = LOVHelper.GetLovList("SettlementAccountType").ToList();

                if (enumData.Any())
                {
                    string SettlementName = string.Empty;
                    LovInfo info = enumData.Where(x => x.Value == modelModify.SettlementAccountType.Value).SingleOrDefault();
                    if (info != null)
                        SettlementName = info.Text;
                    int days = Convert.ToInt32(modelModify.Days.Value);
                    if (days > 0)
                        SettlementName += ",付款天数为" + days + "天";
                    modelModify.SettlementName = SettlementName;
                }

                Update<SettlementWay>(modelModify);
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

    }
}
