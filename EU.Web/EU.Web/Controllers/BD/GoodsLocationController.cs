using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.Core.Enums;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using EU.Model.System;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.BD
{
    /// <summary>
    /// 货位
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.Base)]
    public class GoodsLocationController : BaseController1<GoodsLocation>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public GoodsLocationController(DataContext _context, IBaseCRUDVM<GoodsLocation> BaseCrud) : base(_context, BaseCrud)
        {

        }

        #region 新增重写
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        public override IActionResult Add(GoodsLocation Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Stock stock = _context.BdStock.Where(x => x.ID == Model.StockId).SingleOrDefault(); ;
                if (stock.IsVirtual.Value)
                {
                    List<GoodsLocation> locationList = _context.BdGoodsLocation.Where(x => x.StockId == Model.StockId && x.IsActive == true && x.IsDeleted == false).ToList();
                    if (locationList.Count > 0)
                        throw new("虚拟仓只能新建一个货位!");
                }
                //List<SmModule> smModules = _context.SmModules.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();


                #region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "BdGoodsLocation", "LocationNo", Model.LocationNo, ModifyType.Add, null, "货位编号", "StockId='" + Model.StockId + "'");
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
        /// <summary>
        /// 更新
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
                #region 检查是否存在相同的编码
                Utility.CheckCodeExist("", "BdGoodsLocation", "LocationNo", modelModify.LocationNo.Value, ModifyType.Edit, modelModify.ID.Value, "货位编号", "StockId='" + modelModify.StockId.Value + "'");
                #endregion

                Update<GoodsLocation>(modelModify);
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

        #region 获取详情
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public override async Task<IActionResult> GetById(Guid Id)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            int count = 0;

            try
            {
                string sql = "SELECT * FROM BdGoodsLocation WHERE ID='{0}'";
                sql = string.Format(sql, Id);
                obj.data = DBHelper.Instance.QueryFirst<GoodsLocation>(sql);
                status = "ok";
            }
            catch (Exception E)
            {
                message = E.Message;
            }
            obj.count = count;
            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }
        #endregion

    }
}
