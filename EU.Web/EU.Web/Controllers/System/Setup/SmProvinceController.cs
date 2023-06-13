using EU.Core.CacheManager;
using EU.Core.Const;
using EU.Core.Entry;
using EU.Core.Enums;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.Setup
{
    /// <summary>
    /// 省份
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmProvinceController : BaseController1<SmProvince>
    {
        private readonly IConfiguration Configuration;
        public SmProvinceController(IConfiguration configuration, DataContext _context, IBaseCRUDVM<SmProvince> BaseCrud) : base(_context, BaseCrud)
        {
            Configuration = configuration;
        }

        #region 抓取省份城市数据
        /// <summary>
        /// 抓取省份城市数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ServiceResult<List<ProvinceCityData>>> GetProvinceCityData()
        {
            RedisCacheService Redis = new RedisCacheService(3);
            var list = Redis.Get<List<ProvinceCityData>>(CacheKeys.ProvinceCity.ToString());
            if (list == null)
            {
                var citys = await _context.SmCity
                    .Where(o => o.IsActive == true && o.IsDeleted == false)
                    .OrderBy(o => o.TaxisNo).ToListAsync();
                var countys = await _context.SmCounty
                    .Where(o => o.IsActive == true && o.IsDeleted == false)
                    .OrderBy(o => o.TaxisNo).ToListAsync();
                list = await _context.SmProvince
                    .Where(o => o.IsActive == true && o.IsDeleted == false)
                    .OrderBy(o => o.TaxisNo)
                    .Select(o => new ProvinceCityData
                    {
                        value = o.ID,
                        label = o.ProvinceNameZh
                    }).ToListAsync();
                list.ForEach(p =>
                {
                    p.children = citys.Where(c => c.ProvinceId == p.value).Select(o => new ProvinceCityData
                    {
                        value = o.ID,
                        label = o.CityNameZh
                    }).ToList();
                    p.children?.ForEach(c =>
                    {
                        c.children = countys.Where(county => county.CityId == c.value).Select(o => new ProvinceCityData
                        {
                            value = o.ID,
                            label = o.CountyNameZh
                        }).ToList();
                    });
                });

                Redis.AddObject(CacheKeys.ProvinceCity.ToString(), list);
            }

            return ServiceResult<List<ProvinceCityData>>.OprateSuccess(list, list.Count, ResponseText.QUERY_SUCCESS);
        }
        #endregion
    }

    public class ProvinceCityData
    {
        public string label { get; set; }
        public Guid value { get; set; }
        public List<ProvinceCityData> children { get; set; }
    }
}
