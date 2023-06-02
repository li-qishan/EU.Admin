using System;
using System.Collections.Generic;
using System.Text;

namespace EU.Model.JWT
{
    public class JwtAuthorizationDto
    {
        public string UserId { get; set; }
        public string Token { get; set; }

        /// <summary>
        /// 授权时间戳
        /// </summary>
        public long Auths { get; set; }

        /// <summary>
        /// 过期时间戳
        /// </summary>
        public long Expires { get; set; }

        /// <summary>
        /// 是否授权成功
        /// </summary>
        public bool Success { get; set; }
    }
}
