using EU.Model.System;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EU.Model.System.Privilege;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace EU.Model.JWT
{
    public class JwtAppService : IJwtAppService
    {
        /// <summary>
        /// 已授权的 Token 信息集合
        /// </summary>
        private static ISet<JwtAuthorizationDto> _tokens = new HashSet<JwtAuthorizationDto>();

        /// <summary>
        /// 配置信息
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 获取 HTTP 请求上下文
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 分布式缓存
        /// </summary>
        private readonly IDistributedCache _cache;

        public JwtAppService(IDistributedCache cache, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public JwtAuthorizationDto Create(SmUser dto)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));

            DateTime authTime = DateTime.UtcNow;
            DateTime expiresAt = authTime.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpireMinutes"]));

            //将用户信息添加到 Claim 中
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);

            IEnumerable<Claim> claims = new Claim[] {
                new Claim(ClaimTypes.Name,dto.ID.ToString()),
                new Claim(ClaimTypes.Expiration,expiresAt.ToString())
            };
            identity.AddClaims(claims);

            //签发一个加密后的用户信息凭证，用来标识用户的身份
            _httpContextAccessor.HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),//创建声明信息
                Issuer = _configuration["JwtSettings:Issuer"],//Jwt token 的签发者
                Audience = _configuration["JwtSettings:Audience"],//Jwt token 的接收者
                Expires = expiresAt,//过期时间
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)//创建 token
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            //存储 Token 信息
            var jwt = new JwtAuthorizationDto
            {
                UserId = dto.ID.ToString(),
                Token = tokenHandler.WriteToken(token),
                Auths = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                Expires = new DateTimeOffset(expiresAt).ToUnixTimeSeconds(),
                Success = true
            };

            _tokens.Add(jwt);

            return jwt;
        }

        public async Task DeactivateAsync(string token) => await _cache.SetStringAsync(GetKey(token),
        " ", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow =
                TimeSpan.FromMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpireMinutes"]))
        });

        public async Task DeactivateCurrentAsync() => await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)
            => await _cache.GetStringAsync(GetKey(token)) == null;

        public async Task<bool> IsCurrentActiveTokenAsync()
            => await IsActiveAsync(GetCurrentAsync());

        public async Task<JwtAuthorizationDto> RefreshAsync(string token, SmUser dto)
        {
            var jwtOld = GetExistenceToken(token);
            if (jwtOld == null)
            {
                return new JwtAuthorizationDto()
                {
                    Token = "未获取到当前 Token 信息",
                    Success = false
                };
            }

            var jwt = Create(dto);

            //停用修改前的 Token 信息
            //await DeactivateAsync(token);

            return jwt;
        }

        #region Method
        /// <summary>
        /// 设置缓存中过期 Token 值的 key
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        private static string GetKey(string token)
            => $"deactivated token:{token}";

        /// <summary>
        /// 获取 HTTP 请求的 Token 值
        /// </summary>
        /// <returns></returns>
        private string GetCurrentAsync()
        {
            //http header
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            //token
            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();// bearer tokenvalue
        }

        /// <summary>
        /// 判断是否存在当前 Token
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        private JwtAuthorizationDto GetExistenceToken(string token)
            => _tokens.SingleOrDefault(x => x.Token == token.Split(" ").Last());
        #endregion
    }
}
