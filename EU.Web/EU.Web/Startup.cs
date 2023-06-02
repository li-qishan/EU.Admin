using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using EU.Model.Handlers;
using EU.Model.JWT;
using EU.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerUI;
using EU.Core.Configuration;
using System;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.Weixin.Entities;
using EU.WeixinService.CustomMessageHandler;
using Senparc.Weixin.MP.MessageHandlers.Middleware;
using Senparc.CO2NET.AspNet;
using Senparc.Weixin;
using Senparc.NeuChar.MessageHandlers;
using Senparc.Weixin.MP;
using EU.Core.Extensions;
using Senparc.Weixin.Cache.Redis;
using Senparc.CO2NET.RegisterServices;
using Senparc.CO2NET.Cache;
using Senparc.CO2NET.Cache.Memcached;
using Senparc.Weixin.RegisterServices;
using System.IO;
using EU.Core.Utilities;
using EU.Core.Middleware;
using Senparc.Weixin.WxOpen.MessageHandlers.Middleware;
using Senparc.Weixin.Cache.CsRedis;
using EU.WeixinService.WxOpenMessageHandler;
using EU.WeixinService.WorkMessageHandlers;
using Senparc.Weixin.Work.MessageHandlers.Middleware;
using EU.Core.WeiXin;
using Senparc.Weixin.AspNet;
using Google.Protobuf.WellKnownTypes;
using EU.Web.Extensions;
using EU.Core.Middlewares;
using JianLian.HDIS.HttpApi.Hosting.Extensions;
using DotNetify;
using DotNetify.Pulse;

namespace EU.Web
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        private IServiceCollection _services;

        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ApplicationContainer
        /// </summary>
        public static IContainer ApplicationContainer { get; set; }

        //private IServiceCollection Services { get; set; }

        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
           .AddControllers(options =>
           {
               options.Filters.Add(typeof(GlobalExceptionFilter));
           }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            })
           .AddNewtonsoftJson(options =>
           {
               options.SerializerSettings.ContractResolver = new DefaultContractResolver();
               options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
               options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // 设置时区为 UTC
           });

            services.AddHttpContextAccessor();
            services.AddSenparcWeixinServices(Configuration);//注册全局微信服务
            //services.AddMemoryCache();
            AppSetting.Init(services, Configuration);
            ServiceExtensions.Init();

            //services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //}).AddNewtonsoftJson(
            //    options =>
            //    {
            //        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //    });
            //Services = services;

            services.AddDbContext<DataContext>(options =>
                {
                    options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("DbConnectionString"));
                });

            //string DbConnectionString = AppSetting.DbConnectionString;

            //DBServerProvider.SetConnection(DBServerProvider.DefaultConnName, DbConnectionString);

            #region JWT认证

            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            JwtSettings setting = new JwtSettings();
            //绑定配置文件信息到实体
            Configuration.Bind("JwtSettings", setting);

            // 添加验证和授权
            services.AddAuthenticationAndAuthorization(setting);

            services.AddSingleton<IAuthorizationHandler, PolicyHandler>();
            #endregion

            #region 基于IHttpContextAccessor实现系统级别身份标识
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPrincipalAccessor, PrincipalAccessor>();
            services.AddSingleton<IClaimsAccessor, ClaimsAccessor>();
            #endregion

            #region cors跨域请求

            string corsUrls = Configuration["CorsUrls"];
            if (string.IsNullOrEmpty(corsUrls))
            {
                throw new Exception("请配置跨请求的前端Url");
            }
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(corsUrls.Split(","))
                        //添加预检请求过期时间
                         .SetPreflightMaxAge(TimeSpan.FromSeconds(2520))
                        .AllowCredentials()
                        .AllowAnyHeader().AllowAnyMethod();
                    });
            });
            #endregion

            services.AddSingleton<IJwtAppService, JwtAppService>();
            services.AddSingleton<WxConfigContainer>();
            var basePath = AppContext.BaseDirectory;

            #region 注入Swagger服务
            services.AddSwagger();
            //services.AddSwaggerGen(c =>
            //{
            //    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "后台Api", Version = "v1" });
            //    c.SwaggerDoc("v2", new OpenApiInfo { Title = "悦樘公寓API接口", Version = "v2" });
            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            //    {
            //        Description = "JWT授权token前面需要加上字段Bearer与一个空格,如Bearer token",
            //        Name = "Authorization",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.ApiKey,
            //        BearerFormat = "JWT",
            //        Scheme = "Bearer"
            //    });

            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "Bearer"
            //                }
            //            },
            //            new string[] { }
            //        }
            //    });
            //    try
            //    {
            //        string[] Files = Directory.GetFiles(basePath);
            //        foreach (var item in Files)
            //        {
            //            if (Path.GetExtension(item).Equals(".xml"))
            //            {
            //                c.IncludeXmlComments(item, true);//默认的第二个参数是false，这个是controller的注释，记得修改
            //            }
            //        }
            //    }
            //    catch (Exception)
            //    {

            //    }
            //});
            #endregion

            #region 
            //string signalRClientUrl = "http://localhost:8000";
            //services.AddCors(options => options.AddPolicy("CorsPolicy",
            //    builder =>
            //    {
            //        builder.AllowAnyMethod().AllowAnyHeader()
            //               .WithOrigins(signalRClientUrl)
            //               .AllowCredentials();
            //    }));
            services.AddSignalR();
            #endregion

            services.AddDistributedRedisCache(r =>
            {
                r.Configuration = Configuration["ConnectionStrings:RedisConnectionString"];
            });

            ////初始化容器
            //var builder = new ContainerBuilder();
            ////管道寄居
            //builder.Populate(services);
            ////注册业务
            //builder.RegisterAssemblyTypes(Assembly.Load("NetCoreWebApi.Repository"), Assembly.Load("NetCoreWebApi.Repository"))
            //    .Where(t => t.Name.EndsWith("Repository"))
            //    .AsImplementedInterfaces();
            ////注册仓储，所有IRepository接口到Repository的映射
            //builder.RegisterGeneric(typeof(BaseCRUDVM<>))
            //    //InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //    .As(typeof(IBaseCRUDVM<>)).InstancePerDependency();
            ////构造
            //ApplicationContainer = builder.Build();
            ////将AutoFac反馈到管道中
            //return new AutofacServiceProvider(ApplicationContainer);

            //Senparc.CO2NET 全局注册（必须）
            services.AddMvc();
            services.AddSenparcGlobalServices(Configuration);


            #region DotNetify
            //services.AddSignalR();
            //services.AddDotNetify();
            //services.AddDotNetifyPulse();
            #endregion

            _services = services;

        }

        /// <summary>
        /// ConfigureContainer
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //业务逻辑层所在程序集命名空间
            Assembly service = Assembly.Load("EU.Web");
            //接口层所在程序集命名空间
            Assembly repository = Assembly.Load("EU.Web");
            //自动注入
            builder.RegisterAssemblyTypes(service, repository)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
            //注册仓储，所有IRepository接口到Repository的映射
            builder.RegisterGeneric(typeof(BaseCRUDVM<>))
                //InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
                .As(typeof(IBaseCRUDVM<>)).InstancePerDependency();
            //builder.RegisterGeneric(typeof(EU.Domain.Base.Repositories.BaseCRUDVM<>)).As(typeof(EU.Domain.Base.IBaseCRUDVM<>)).InstancePerDependency();

        }
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="senparcSetting"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<SenparcSetting> senparcSetting,
            IOptions<SenparcWeixinSetting> senparcWeixinSetting, IOptions<WxConfigContainer> wxConfigContainer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseErrorHandling();
            // 查看注入的所有服务
            app.UseAllServicesMiddle(_services);
            //app.UseMiddleware<ExceptionHandlerMiddleWare>();
            app.UseDefaultFiles();
            app.Use(HttpRequestMiddleware.Context);
            //配置HttpContext
            app.UseStaticHttpContext();

            //启用Swagger服务
            app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    //c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            //    c.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
            //    c.DefaultModelsExpandDepth(-1); //设置为 - 1 可不显示models
            //    c.DocExpansion(DocExpansion.None); //设置为none可折叠所有方法
            //});
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseStaticHttpContext();

            app.UseCors();

            //使用验证和授权
            app.UseAuthenticationAndAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
            });
            ServiceProviderInstance.Instance = app.ApplicationServices;

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            #region DotNetify
            //app.UseWebSockets();
            //app.UseDotNetify();
            //app.UseDotNetifyPulse();
            //app.UseEndpoints(endpoints => endpoints.MapHub<DotNetifyHub>("/dotnetify"));
            #endregion

            #region 微信配置
            // 启动 CO2NET 全局注册，必须！
            // 关于 UseSenparcGlobal() 的更多用法见 CO2NET Demo：https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore3/Startup.cs
            app.UseSenparcGlobal(env, senparcSetting.Value, globalRegister =>
             {
                 //当同一个分布式缓存同时服务于多个网站（应用程序池）时，可以使用命名空间将其隔离（非必须）
                 globalRegister.ChangeDefaultCacheNamespace("DefaultCO2NETCache");
                 if (UseRedis(senparcSetting.Value, out string redisConfigurationStr))//这里为了方便不同环境的开发者进行配置，做成了判断的方式，实际开发环境一般是确定的，这里的if条件可以忽略
                 {
                     /* 说明：
                      * 1、Redis 的连接字符串信息会从 Config.SenparcSetting.Cache_Redis_Configuration 自动获取并注册，如不需要修改，下方方法可以忽略
                     /* 2、如需手动修改，可以通过下方 SetConfigurationOption 方法手动设置 Redis 链接信息（仅修改配置，不立即启用）
                      */
                     Senparc.CO2NET.Cache.CsRedis.Register.SetConfigurationOption(redisConfigurationStr);

                     //以下会立即将全局缓存设置为 Redis
                     Senparc.CO2NET.Cache.CsRedis.Register.UseKeyValueRedisNow();//键值对缓存策略（推荐）
                                                                                 //Senparc.CO2NET.Cache.CsRedis.Register.UseHashRedisNow();//HashSet储存格式的缓存策略

                     //也可以通过以下方式自定义当前需要启用的缓存策略
                     //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisObjectCacheStrategy.Instance);//键值对
                     //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisHashSetObjectCacheStrategy.Instance);//HashSet

                     //wxConfigContainer.Value.Use();

                     #region 注册 StackExchange.Redis

                     /* 如果需要使用 StackExchange.Redis，则可以使用 Senparc.CO2NET.Cache.Redis 库
                      * 注意：这一步注册和上述 CsRedis 库两选一即可，本 Sample 需要同时演示两个库，因此才都进行注册
                      */

                     //Senparc.CO2NET.Cache.Redis.Register.SetConfigurationOption(redisConfigurationStr);
                     //Senparc.CO2NET.Cache.Redis.Register.UseKeyValueRedisNow();//键值对缓存策略（推荐）

                     #endregion
                 }
             }, true)
                .UseSenparcWeixin(senparcWeixinSetting.Value, (weixinRegister, weixinSetting) =>
                {
                    if (UseRedis(senparcSetting.Value, out _))
                    {
                        weixinRegister.UseSenparcWeixinCacheCsRedis();//CsRedis，两选一
                                                                      //weixinRegister.UseSenparcWeixinCacheRedis();//StackExchange.Redis，两选一
                    }
                });
            //.UseSenparcWeixin(senparcWeixinSetting.Value, weixinRegister =>
            //{
            //    #region 微信相关配置
            //    /* 微信配置开始
            //    * 
            //    * 建议按照以下顺序进行注册，尤其须将缓存放在第一位！
            //    */
            //    #region 微信缓存（按需，必须放在配置开头，以确保其他可能依赖到缓存的注册过程使用正确的配置）
            //    //注意：如果使用非本地缓存，而不执行本块注册代码，将会收到“当前扩展缓存策略没有进行注册”的异常
            //    //微信的 Redis 缓存，如果不使用则注释掉（开启前必须保证配置有效，否则会抛错）         -- DPBMARK Redis
            //    if (UseRedis(senparcSetting.Value, out _))
            //    {
            //        weixinRegister.UseSenparcWeixinCacheCsRedis();//CsRedis，两选一
            //                                                      //weixinRegister.UseSenparcWeixinCacheRedis();//StackExchange.Redis，两选一
            //    }                                                                                       // DPBMARK_END
            //   #endregion
            //   #endregion
            //});
            #region 使用 MessageHadler 中间件，用于取代创建独立的 Controller
            //MessageHandler 中间件介绍：https://www.cnblogs.com/szw/p/Wechat-MessageHandler-Middleware.html

            //使用公众号的 MessageHandler 中间件（不再需要创建 Controller）                       --DPBMARK MP
            app.UseMessageHandlerForMp("/WeixinAsync", CustomMessageHandler.GenerateMessageHandler, options =>
            {
                //说明：此代码块中演示了较为全面的功能点，简化的使用可以参考下面小程序和企业微信

                #region 配置 SenparcWeixinSetting 参数，以自动提供 Token、EncodingAESKey 等参数

                //此处为委托，可以根据条件动态判断输入条件（必须）
                options.AccountSettingFunc = context =>
                {
                    try
                    {
                        var userName = context.Request.Query["userName"];
                        WxConfig wxConfig = wxConfigContainer.Value.GetConfig(userName);
                        if (wxConfig == null) return senparcWeixinSetting.Value;
                        SenparcWeixinSetting weixinSetting = new SenparcWeixinSetting();
                        weixinSetting.Token = wxConfig.Token;
                        weixinSetting.WeixinAppId = wxConfig.AppId;
                        weixinSetting.EncodingAESKey = wxConfig.AESKey;
                        weixinSetting.WeixinAppSecret = wxConfig.AppSecret;
                        return weixinSetting;
                    }
                    catch (Exception)
                    {
                        //Logger.WriteLog("Weixin", e.Message);
                        throw;
                    }
                };

                #endregion

                //对 MessageHandler 内异步方法未提供重写时，调用同步方法（按需）
                options.DefaultMessageHandlerAsyncEvent = DefaultMessageHandlerAsyncEvent.SelfSynicMethod;

                //对发生异常进行处理（可选）
                options.AggregateExceptionCatch = ex =>
                {
                    //逻辑处理...
                    return false;//系统层面抛出异常
                };
            });                                                                                   // DPBMARK_END
            //使用 小程序 MessageHandler 中间件                                                   // -- DPBMARK MiniProgram
            app.UseMessageHandlerForWxOpen("/WxOpenAsync", CustomWxOpenMessageHandler.GenerateMessageHandler, options =>
            {
                options.DefaultMessageHandlerAsyncEvent = DefaultMessageHandlerAsyncEvent.SelfSynicMethod;
                options.AccountSettingFunc = context => senparcWeixinSetting.Value;
            }
            );                                                                                    // DPBMARK_END

            //使用 企业微信 MessageHandler 中间件                                                 // -- DPBMARK Work
            app.UseMessageHandlerForWork("/WorkAsync", WorkCustomMessageHandler.GenerateMessageHandler, option =>
            {
                option.AccountSettingFunc = context =>
                {
                    try
                    {
                        var userName = context.Request.Query["userName"];
                        WxConfig wxConfig = wxConfigContainer.Value.GetConfig(userName);
                        if (wxConfig == null)
                        {
                            return senparcWeixinSetting.Value;
                        }
                        SenparcWeixinSetting weixinSetting = new SenparcWeixinSetting();
                        weixinSetting.WeixinCorpToken = wxConfig.Token;
                        weixinSetting.WeixinCorpAgentId = wxConfig.AppId;
                        weixinSetting.WeixinCorpEncodingAESKey = wxConfig.AESKey;
                        weixinSetting.WeixinCorpId = wxConfig.OriginId;
                        weixinSetting.WeixinCorpSecret = wxConfig.AppSecret;
                        weixinSetting.WeixinAppId = wxConfig.OriginId;
                        weixinSetting.WeixinAppSecret = wxConfig.AppSecret;
                        return weixinSetting;
                    }
                    catch (Exception)
                    {
                        //Logger.WriteLog("Weixin", "exception:" + e.Message);
                        throw;
                    }
                };


                //对发生异常进行处理（可选）
                option.AggregateExceptionCatch = ex =>
                {
                    //Logger.WriteLog("Weixin", "ggregateExceptio:" + ex.Message);
                    //逻辑处理...
                    return false;//系统层面抛出异常
                };
            });

            #endregion
            #endregion
        }
        // -- DPBMARK Redis
        /// <summary>
        /// 判断当前配置是否满足使用 Redis（根据是否已经修改了默认配置字符串判断）
        /// </summary>
        /// <param name="senparcSetting"></param>
        /// <returns></returns>
        private bool UseRedis(SenparcSetting senparcSetting, out string redisConfigurationStr)
        {
            redisConfigurationStr = senparcSetting.Cache_Redis_Configuration;
            var useRedis = !string.IsNullOrEmpty(redisConfigurationStr) && redisConfigurationStr != "#{Cache_Redis_Configuration}#"/*默认值，不启用*/;
            return useRedis;
        }
    }
}
