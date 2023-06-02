using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EU.Web
{
    public class ClaimsAccessor : IClaimsAccessor
    {
        protected IPrincipalAccessor PrincipalAccessor { get; }

        public ClaimsAccessor(IPrincipalAccessor principalAccessor)
        {
            PrincipalAccessor = principalAccessor;
        }

        /// <summary>
        /// 登录用户ID
        /// </summary>
        public Guid? UserId
        {
            get
            {
                var userId = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                if (userId != null)
                {
                    return Guid.Parse(userId);
                }
                return null;
            }
        }

        /// <summary>
        /// 用户角色Id
        /// </summary>
        //public string RoleIds
        //{
        //    get
        //    {
        //        var roleIds = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == SystemClaimTypes.RoleIds)?.Value;
        //        if (string.IsNullOrWhiteSpace(roleIds))
        //        {
        //            return string.Empty;
        //        }

        //        return roleIds;
        //    }
        //}
    }
    public interface IClaimsAccessor
    {
        /// <summary>
        /// 登录用户ID
        /// </summary>
        Guid? UserId { get; }

        /// <summary>
        /// 用户角色Id
        /// </summary>
        //string RoleIds { get; }
    }
}
