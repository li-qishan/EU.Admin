using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.PD
{
    /// <summary>
    /// 生产工单-工艺路线
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.PD)]
    public class PdOrderProcessController : BaseController1<PdOrderProcess>
    {
        /// <summary>
        /// 生产工单-工艺路线
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="BaseCrud"></param>
        public PdOrderProcessController(DataContext _context, IBaseCRUDVM<PdOrderProcess> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
