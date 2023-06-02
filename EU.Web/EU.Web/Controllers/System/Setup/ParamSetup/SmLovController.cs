using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.Core.Const;
using EU.Core.Entry;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NPOI.SS.Formula.Functions;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmLovController : BaseController<SmLov>
    {
        public SmLovController(DataContext _context, IBaseCRUDVM<SmLov> BaseCrud) : base(_context, BaseCrud)
        {
        }

        [HttpGet]
        public async Task<ServiceResult<IEnumerable<KeyValue>>> GetByCode(string code)
        {
            try
            {
                var lsit = await LOVHelper.GetLovListAsync(code);
                var data = lsit.Select(x => new KeyValue() { key = x.Value, value = x.Text });

                return ServiceResult<IEnumerable<KeyValue>>.OprateSuccess(data, ResponseText.QUERY_SUCCESS);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class KeyValue
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
