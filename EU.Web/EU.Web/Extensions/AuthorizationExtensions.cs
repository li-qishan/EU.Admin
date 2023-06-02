using EU.Core.Entry;
using EU.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU.Web.Extensions
{
    public static class AuthorizationExtensions
    {
        /// <summary>
        /// 添加验证和授权
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, JwtSettings setting)
        {
            // 添加 JWT 验证

            services.AddAuthorization(options =>
            {
                //1、Definition authorization policy
                options.AddPolicy("Permission",
                    policy => policy.Requirements.Add(new PolicyRequirement()));
            }).AddAuthentication(option =>
            {
                //2、Authentication
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // 是否验证颁发者
                    ValidateIssuer = true,
                    // 是否验证访问群体
                    ValidateAudience = true,
                    // 是否验证生存期
                    ValidateLifetime = true,
                    // 验证Token的时间偏移量
                    ClockSkew = TimeSpan.FromSeconds(30),
                    // 是否验证安全密钥
                    ValidateIssuerSigningKey = true,
                    // 访问群体
                    ValidAudience = setting.Audience,
                    // 颁发者
                    ValidIssuer = setting.Audience,
                    // 安全密钥
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.SecretKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(ServiceResult.OprateFailed("未登录或登录信息已失效，请重新登录", ServiceResultCode.Unauthorized)));
                        return Task.FromResult(0);
                    }
                };
            });

           //.AddJwtBearer(config =>
           // {
           //     //3、Use Jwt bearer 
           //     config.TokenValidationParameters = new TokenValidationParameters
           //     {
           //         ValidAudience = setting.Audience,
           //         ValidIssuer = setting.Issuer,
           //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.SecretKey))
           //     };
           //     config.Events = new JwtBearerEvents
           //     {
           //         OnAuthenticationFailed = context =>
           //         {
           //             //Token expired
           //             if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
           //             {
           //                 context.Response.Headers.Add("Token-Expired", "true");
           //             }
           //             return Task.CompletedTask;
           //         }
           //     };
           // });


            return services;
        }

        /// <summary>
        /// 使用验证和授权
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuthenticationAndAuthorization(this IApplicationBuilder app)
        {
            // 使用验证
            app.UseAuthentication();
            // 使用授权
            app.UseAuthorization();
            return app;
        }
    }
}
