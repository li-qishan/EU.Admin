
using EU.Core.HttpRestSharp.Impl;
using EU.Core.Services;
using EU.Core.Utilities;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 这里要注意下，命名空间和程序集是一样的，不然反射不到
/// </summary>
namespace EU.TaskHelper
{
    public class Job_Blogs_Quartz : JobBase, IJob
    {
        //private readonly IBlogArticleServices _blogArticleServices;

        //public Job_Blogs_Quartz(IBlogArticleServices blogArticleServices, ITasksQzServices tasksQzServices)
        //{
        //    _blogArticleServices = blogArticleServices;
        //    _tasksQzServices = tasksQzServices;
        //}

        public Job_Blogs_Quartz(string code = "RedisMonitor", string name = "Redis状态监控") : base(code, name)
        {
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var executeLog = await ExecuteJob(context, async () => await Run(context));
        }
        public async Task Run(IJobExecutionContext context)
        {
            //var list = await _blogArticleServices.Query();
            // 也可以通过数据库配置，获取传递过来的参数
            JobDataMap data = context.JobDetail.JobDataMap;
            //var tenantId = data.GetNullableGuid("TenantId");
            //Random a = new Random();
            //int a1 = a.Next(5, 15) * 1000;
            //Thread.Sleep(a1);
            SendLog($"Job_Blogs_Quartz {Core.Utilities.Utility.GetSysDate()}");

            var client = new FluentClient();
            var request = client
                  //.AddCookie(string name, string value, string path, string domain)//添加cookie
                  //.AddDefaultHeader(string name, string value)//添加默认请求头
                  //.AddDefaultParameter(string name, string value)//添加默认form参数
                  //.AddDefaultQueryParameter(string name, string value)//添加默认query参数
                  //.AddDefaultUrlSegment(string name, string value)//添加请求路径默认占位符参数
                  //.UseAuthenticator(IAuthenticator authenticator)//添加认证器
                  .BuildClient("http://localhost:8015/api/Common/GenerateTableModel/RmType");//client构造完毕,转到request请求构造
            var result1 = request
                        .Get().BuildRequest()
                .GetContent();

            //var result = Core.Utilities.Utility.GetHttpResult($"http://{Ip}:60010/swagger/{itemtype}/swagger.json", "");
            //Logger.WriteLog($"Job_Blogs_Quartz {Core.Utilities.Utility.GetSysDate()}--{result1} ！");
        }
    }
    }
