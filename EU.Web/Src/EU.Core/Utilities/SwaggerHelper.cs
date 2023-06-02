using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using static EU.Core.Const.Consts;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace EU.Core.Utilities
{
    /// <summary>
    /// Swagger方法类
    /// </summary>
    public static class SwaggerHelper
    {
        /// <summary>
        /// 当前API版本，从appsettings.json获取
        /// </summary>
        private static readonly string version = $"v1.1.4";

        /// <summary>
        /// Swagger分组信息，将进行遍历使用
        /// </summary>
        private static readonly List<SwaggerApiInfo> ApiInfos = new List<SwaggerApiInfo>()
        {
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.Auth,
                Name = "认证授权",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "认证授权",
                    Description = "登录/注销",
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.System,
                Name = "系统模块",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "系统模块",
                    Description = "用户/角色/权限...",
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.Base,
                Name = "基础模块",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "基础模块",
                    Description = "物料/单位/客户...",
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.EM,
                Name = "设备模块",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "设备模块",
                    Description = "设备...",
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.IV,
                Name = "库存模块",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "库存模块",
                    Description = "库存...",
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.MF,
                Name = "工模模块",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "工模模块",
                    Description = "工模...",
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.PD,
                Name = "生产模块",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "生产模块",
                    Description = "生产...",
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.PO,
                Name = "采购模块",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "采购模块",
                    Description = "采购...",
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.PS,
                Name = "产品结构模块",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "产品结构模块",
                    Description = "采购...",
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.SD,
                Name = "销售模块",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "销售模块",
                    Description = "采购...",
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.AP,
                Name = "应付模块",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "应付模块",
                    Description = "应付期初建账、应付对账单...",
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.Procedure,
                Name = "流程模块",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "流程模块",
                    Description = "透析流程相关...",
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.Report,
                Name = "报表模块",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "报表模块",
                    Description = "报表相关...",
                }
            },
            new SwaggerApiInfo
            {
                UrlPrefix = Grouping.Assistant,
                Name = "工具模块",
                OpenApiInfo = new OpenApiInfo
                {
                    Version = version,
                    Title = "工具模块",
                    Description = "自助测量程序/中央站...",
                }
            },
        };

        /// <summary>
        /// AddSwagger
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                // 遍历并应用Swagger分组信息
                ApiInfos.ForEach(x =>
                {
                    options.SwaggerDoc(x.UrlPrefix, x.OpenApiInfo);
                });

                // API注释所需XML文件

                try
                {
                    var basePath = AppContext.BaseDirectory;
                    string[] Files = Directory.GetFiles(basePath);
                    foreach (var item in Files)
                    {
                        if (Path.GetExtension(item).Equals(".xml"))
                            options.IncludeXmlComments(item, true);//默认的第二个参数是false，这个是controller的注释，记得修改
                    }
                }
                catch (Exception)
                {

                }
                #region JWT身份认证配置

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT授权token前面需要加上字段Bearer与一个空格,如Bearer token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });

                #endregion
            });
        }

        /// <summary>
        /// UseSwaggerUI
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
        {
            SwaggerBuilderExtensions.UseSwagger(app);
            app.UseSwaggerUI(options =>
            {
                // 遍历分组信息，生成Json
                ApiInfos.ForEach(x =>
                {
                    options.SwaggerEndpoint($"/swagger/{x.UrlPrefix}/swagger.json", x.Name);
                });
                // 模型的默认扩展深度，设置为 -1 完全隐藏模型
                options.DefaultModelsExpandDepth(-1);
                // API文档仅展开标记
                options.DocExpansion(DocExpansion.List);
                // API前缀设置为空
                //options.RoutePrefix = string.Empty;
                //// API页面Title
                options.DocumentTitle = "优智云接口文档";
            });
            return app;
        }
        private class SwaggerApiInfo
        {
            /// <summary>
            /// URL前缀
            /// </summary>
            public string UrlPrefix { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// OpenApiInfo
            /// </summary>
            public OpenApiInfo OpenApiInfo { get; set; }
        }
    }
}
