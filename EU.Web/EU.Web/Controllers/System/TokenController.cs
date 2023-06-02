using System;
using System.Data;
using System.Dynamic;
using System.Text;
using EU.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using EU.Core.Module;
using EU.Model.System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using EU.Core;
using EU.Core.CacheManager;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EU.Model;
using static EU.Core.Const.Consts;
using EU.Domain.System;
using EU.Core.Entry;
using EU.Core.Const;
using EU.DataAccess;
using EU.Domain;
using Microsoft.EntityFrameworkCore;
using EU.Model.JWT;

namespace EU.Web.Controllers.System
{
    /// <summary>
    /// 公共方法
    /// </summary>
    [ApiController, Authorize(Policy = "Permission"), GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.Auth)]
    public class TokenController : BaseController<SmUser>
    {
        private readonly IJwtAppService _jwtApp;

        /// <summary>
        /// 配置信息
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 公共方法
        /// </summary>
        /// <param name="configuration"></param>

        public TokenController(IConfiguration configuration, IJwtAppService jwtApp, DataContext _context, IBaseCRUDVM<SmUser> BaseCrud) : base(_context, BaseCrud)
        {
            _configuration = configuration;
            _jwtApp = jwtApp;
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public async Task<ServiceResult<JwtAuthorizationDto>> GetFixAccessToken(string uuid, string appId, string appSecret)
        {
            var api = await _context.SmApi.Where(o => o.AppId == appId && o.AppSecret == appSecret).FirstOrDefaultAsync();
            if (api == null)
                throw new Exception("无效的接口授权！");
            SmUser User = new SmUser();
            User.ID = api.ID;
            var result = _jwtApp.Create(User);

            return ServiceResult<JwtAuthorizationDto>.OprateSuccess(result, ResponseText.QUERY_SUCCESS);
        }

        /// <summary>
        /// 当token将要过期时，提前置换一个新的token
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ServiceResult<JwtAuthorizationDto> ReplaceToken()
        {
            //var list = await _context.ApCheckOrder.Where(o => true).FirstOrDefaultAsync();

            SmUser User = new SmUser();
            var result = _jwtApp.Create(User);

            return ServiceResult<JwtAuthorizationDto>.OprateSuccess(result, ResponseText.QUERY_SUCCESS);
        }
    }
}
