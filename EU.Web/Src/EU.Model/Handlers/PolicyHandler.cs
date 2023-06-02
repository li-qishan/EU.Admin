using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EU.Model.JWT;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EU.Model.Handlers
{
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {
        /// <summary>
        /// 授权方式（cookie, bearer, oauth, openid）
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        /// <summary>
        /// jwt 服务
        /// </summary>
        private readonly IJwtAppService _jwtApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="jwtApp"></param>
        public PolicyHandler(IAuthenticationSchemeProvider schemes, IJwtAppService jwtApp)
        {
            Schemes = schemes;
            _jwtApp = jwtApp;
        }

        //授权处理
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            //var http = (context.Resource as Microsoft.AspNetCore.Routing.RouteEndpoint);
            var isAuthenticated = context.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                //判断是否为已停用的 Token
                if (!await _jwtApp.IsCurrentActiveTokenAsync())
                {
                    context.Fail();
                    return;
                }

                //判断是否过期
                if (DateTime.Parse(context.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration).Value) >= DateTime.UtcNow)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
                return;
            }
            context.Fail();
        }
    }
}
