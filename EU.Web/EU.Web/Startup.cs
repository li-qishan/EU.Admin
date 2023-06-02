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
               options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // ����ʱ��Ϊ UTC
           });

            services.AddHttpContextAccessor();
            services.AddSenparcWeixinServices(Configuration);//ע��ȫ��΢�ŷ���
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

            #region JWT��֤

            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            JwtSettings setting = new JwtSettings();
            //�������ļ���Ϣ��ʵ��
            Configuration.Bind("JwtSettings", setting);

            // �����֤����Ȩ
            services.AddAuthenticationAndAuthorization(setting);

            services.AddSingleton<IAuthorizationHandler, PolicyHandler>();
            #endregion

            #region ����IHttpContextAccessorʵ��ϵͳ������ݱ�ʶ
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPrincipalAccessor, PrincipalAccessor>();
            services.AddSingleton<IClaimsAccessor, ClaimsAccessor>();
            #endregion

            #region cors��������

            string corsUrls = Configuration["CorsUrls"];
            if (string.IsNullOrEmpty(corsUrls))
            {
                throw new Exception("�����ÿ������ǰ��Url");
            }
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(corsUrls.Split(","))
                        //���Ԥ���������ʱ��
                         .SetPreflightMaxAge(TimeSpan.FromSeconds(2520))
                        .AllowCredentials()
                        .AllowAnyHeader().AllowAnyMethod();
                    });
            });
            #endregion

            services.AddSingleton<IJwtAppService, JwtAppService>();
            services.AddSingleton<WxConfigContainer>();
            var basePath = AppContext.BaseDirectory;

            #region ע��Swagger����
            services.AddSwagger();
            //services.AddSwaggerGen(c =>
            //{
            //    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "��̨Api", Version = "v1" });
            //    c.SwaggerDoc("v2", new OpenApiInfo { Title = "���̹�ԢAPI�ӿ�", Version = "v2" });
            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            //    {
            //        Description = "JWT��Ȩtokenǰ����Ҫ�����ֶ�Bearer��һ���ո�,��Bearer token",
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
            //                c.IncludeXmlComments(item, true);//Ĭ�ϵĵڶ���������false�������controller��ע�ͣ��ǵ��޸�
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

            ////��ʼ������
            //var builder = new ContainerBuilder();
            ////�ܵ��ľ�
            //builder.Populate(services);
            ////ע��ҵ��
            //builder.RegisterAssemblyTypes(Assembly.Load("NetCoreWebApi.Repository"), Assembly.Load("NetCoreWebApi.Repository"))
            //    .Where(t => t.Name.EndsWith("Repository"))
            //    .AsImplementedInterfaces();
            ////ע��ִ�������IRepository�ӿڵ�Repository��ӳ��
            //builder.RegisterGeneric(typeof(BaseCRUDVM<>))
            //    //InstancePerDependency��Ĭ��ģʽ��ÿ�ε��ã���������ʵ��������ÿ�����󶼴���һ���µĶ���
            //    .As(typeof(IBaseCRUDVM<>)).InstancePerDependency();
            ////����
            //ApplicationContainer = builder.Build();
            ////��AutoFac�������ܵ���
            //return new AutofacServiceProvider(ApplicationContainer);

            //Senparc.CO2NET ȫ��ע�ᣨ���룩
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
            //ҵ���߼������ڳ��������ռ�
            Assembly service = Assembly.Load("EU.Web");
            //�ӿڲ����ڳ��������ռ�
            Assembly repository = Assembly.Load("EU.Web");
            //�Զ�ע��
            builder.RegisterAssemblyTypes(service, repository)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
            //ע��ִ�������IRepository�ӿڵ�Repository��ӳ��
            builder.RegisterGeneric(typeof(BaseCRUDVM<>))
                //InstancePerDependency��Ĭ��ģʽ��ÿ�ε��ã���������ʵ��������ÿ�����󶼴���һ���µĶ���
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
            // �鿴ע������з���
            app.UseAllServicesMiddle(_services);
            //app.UseMiddleware<ExceptionHandlerMiddleWare>();
            app.UseDefaultFiles();
            app.Use(HttpRequestMiddleware.Context);
            //����HttpContext
            app.UseStaticHttpContext();

            //����Swagger����
            app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    //c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            //    c.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
            //    c.DefaultModelsExpandDepth(-1); //����Ϊ - 1 �ɲ���ʾmodels
            //    c.DocExpansion(DocExpansion.None); //����Ϊnone���۵����з���
            //});
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseStaticHttpContext();

            app.UseCors();

            //ʹ����֤����Ȩ
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

            #region ΢������
            // ���� CO2NET ȫ��ע�ᣬ���룡
            // ���� UseSenparcGlobal() �ĸ����÷��� CO2NET Demo��https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore3/Startup.cs
            app.UseSenparcGlobal(env, senparcSetting.Value, globalRegister =>
             {
                 //��ͬһ���ֲ�ʽ����ͬʱ�����ڶ����վ��Ӧ�ó���أ�ʱ������ʹ�������ռ佫����루�Ǳ��룩
                 globalRegister.ChangeDefaultCacheNamespace("DefaultCO2NETCache");
                 if (UseRedis(senparcSetting.Value, out string redisConfigurationStr))//����Ϊ�˷��㲻ͬ�����Ŀ����߽������ã��������жϵķ�ʽ��ʵ�ʿ�������һ����ȷ���ģ������if�������Ժ���
                 {
                     /* ˵����
                      * 1��Redis �������ַ�����Ϣ��� Config.SenparcSetting.Cache_Redis_Configuration �Զ���ȡ��ע�ᣬ�粻��Ҫ�޸ģ��·��������Ժ���
                     /* 2�������ֶ��޸ģ�����ͨ���·� SetConfigurationOption �����ֶ����� Redis ������Ϣ�����޸����ã����������ã�
                      */
                     Senparc.CO2NET.Cache.CsRedis.Register.SetConfigurationOption(redisConfigurationStr);

                     //���»�������ȫ�ֻ�������Ϊ Redis
                     Senparc.CO2NET.Cache.CsRedis.Register.UseKeyValueRedisNow();//��ֵ�Ի�����ԣ��Ƽ���
                                                                                 //Senparc.CO2NET.Cache.CsRedis.Register.UseHashRedisNow();//HashSet�����ʽ�Ļ������

                     //Ҳ����ͨ�����·�ʽ�Զ��嵱ǰ��Ҫ���õĻ������
                     //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisObjectCacheStrategy.Instance);//��ֵ��
                     //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisHashSetObjectCacheStrategy.Instance);//HashSet

                     //wxConfigContainer.Value.Use();

                     #region ע�� StackExchange.Redis

                     /* �����Ҫʹ�� StackExchange.Redis�������ʹ�� Senparc.CO2NET.Cache.Redis ��
                      * ע�⣺��һ��ע������� CsRedis ����ѡһ���ɣ��� Sample ��Ҫͬʱ��ʾ�����⣬��˲Ŷ�����ע��
                      */

                     //Senparc.CO2NET.Cache.Redis.Register.SetConfigurationOption(redisConfigurationStr);
                     //Senparc.CO2NET.Cache.Redis.Register.UseKeyValueRedisNow();//��ֵ�Ի�����ԣ��Ƽ���

                     #endregion
                 }
             }, true)
                .UseSenparcWeixin(senparcWeixinSetting.Value, (weixinRegister, weixinSetting) =>
                {
                    if (UseRedis(senparcSetting.Value, out _))
                    {
                        weixinRegister.UseSenparcWeixinCacheCsRedis();//CsRedis����ѡһ
                                                                      //weixinRegister.UseSenparcWeixinCacheRedis();//StackExchange.Redis����ѡһ
                    }
                });
            //.UseSenparcWeixin(senparcWeixinSetting.Value, weixinRegister =>
            //{
            //    #region ΢���������
            //    /* ΢�����ÿ�ʼ
            //    * 
            //    * ���鰴������˳�����ע�ᣬ�����뽫������ڵ�һλ��
            //    */
            //    #region ΢�Ż��棨���裬����������ÿ�ͷ����ȷ���������������������ע�����ʹ����ȷ�����ã�
            //    //ע�⣺���ʹ�÷Ǳ��ػ��棬����ִ�б���ע����룬�����յ�����ǰ��չ�������û�н���ע�ᡱ���쳣
            //    //΢�ŵ� Redis ���棬�����ʹ����ע�͵�������ǰ���뱣֤������Ч��������״�         -- DPBMARK Redis
            //    if (UseRedis(senparcSetting.Value, out _))
            //    {
            //        weixinRegister.UseSenparcWeixinCacheCsRedis();//CsRedis����ѡһ
            //                                                      //weixinRegister.UseSenparcWeixinCacheRedis();//StackExchange.Redis����ѡһ
            //    }                                                                                       // DPBMARK_END
            //   #endregion
            //   #endregion
            //});
            #region ʹ�� MessageHadler �м��������ȡ������������ Controller
            //MessageHandler �м�����ܣ�https://www.cnblogs.com/szw/p/Wechat-MessageHandler-Middleware.html

            //ʹ�ù��ںŵ� MessageHandler �м����������Ҫ���� Controller��                       --DPBMARK MP
            app.UseMessageHandlerForMp("/WeixinAsync", CustomMessageHandler.GenerateMessageHandler, options =>
            {
                //˵�����˴��������ʾ�˽�Ϊȫ��Ĺ��ܵ㣬�򻯵�ʹ�ÿ��Բο�����С�������ҵ΢��

                #region ���� SenparcWeixinSetting ���������Զ��ṩ Token��EncodingAESKey �Ȳ���

                //�˴�Ϊί�У����Ը���������̬�ж��������������룩
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

                //�� MessageHandler ���첽����δ�ṩ��дʱ������ͬ�����������裩
                options.DefaultMessageHandlerAsyncEvent = DefaultMessageHandlerAsyncEvent.SelfSynicMethod;

                //�Է����쳣���д�����ѡ��
                options.AggregateExceptionCatch = ex =>
                {
                    //�߼�����...
                    return false;//ϵͳ�����׳��쳣
                };
            });                                                                                   // DPBMARK_END
            //ʹ�� С���� MessageHandler �м��                                                   // -- DPBMARK MiniProgram
            app.UseMessageHandlerForWxOpen("/WxOpenAsync", CustomWxOpenMessageHandler.GenerateMessageHandler, options =>
            {
                options.DefaultMessageHandlerAsyncEvent = DefaultMessageHandlerAsyncEvent.SelfSynicMethod;
                options.AccountSettingFunc = context => senparcWeixinSetting.Value;
            }
            );                                                                                    // DPBMARK_END

            //ʹ�� ��ҵ΢�� MessageHandler �м��                                                 // -- DPBMARK Work
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


                //�Է����쳣���д�����ѡ��
                option.AggregateExceptionCatch = ex =>
                {
                    //Logger.WriteLog("Weixin", "ggregateExceptio:" + ex.Message);
                    //�߼�����...
                    return false;//ϵͳ�����׳��쳣
                };
            });

            #endregion
            #endregion
        }
        // -- DPBMARK Redis
        /// <summary>
        /// �жϵ�ǰ�����Ƿ�����ʹ�� Redis�������Ƿ��Ѿ��޸���Ĭ�������ַ����жϣ�
        /// </summary>
        /// <param name="senparcSetting"></param>
        /// <returns></returns>
        private bool UseRedis(SenparcSetting senparcSetting, out string redisConfigurationStr)
        {
            redisConfigurationStr = senparcSetting.Cache_Redis_Configuration;
            var useRedis = !string.IsNullOrEmpty(redisConfigurationStr) && redisConfigurationStr != "#{Cache_Redis_Configuration}#"/*Ĭ��ֵ��������*/;
            return useRedis;
        }
    }
}
