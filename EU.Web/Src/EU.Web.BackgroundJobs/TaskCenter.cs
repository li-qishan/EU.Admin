//using EU.Core;
//using EU.Core.Const;
//using EU.Core.Services;
using System;
using System.ComponentModel.Design;
using System.Data;
using System.IO;
using System.Linq;
using EU.Core;
using EU.Core.Configuration;
using EU.Core.Const;
using EU.Core.Services;
using EU.Core.Utilities;
using EU.TaskHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;

namespace EU.Web.BackgroundJobs
{
    /// <summary>
    /// 任务处理中心
    /// </summary>
    public class TaskCenter
    {
        private readonly ISchedulerCenter _schedulerCenter;

        public static IConfiguration Configuration { get; set; }
        static TaskCenter()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public TaskCenter(ISchedulerCenter schedulerCenter)
        {
            _schedulerCenter = schedulerCenter;
            Configuration = new ConfigurationBuilder()
           .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
           .Build();
        }

        #region 启动任务服务
        /// <summary>
        /// 启动任务服务
        /// </summary>
        public void Start()
        {
            var container = new ServiceCollection();

            AppSetting.Init(container, Configuration);

            _schedulerCenter.InitJobAsync();
            Logger.WriteLog("[Task]启动消息订阅");
            RabbitMQHelper.ConsumeMsg<TaskMsg>(RabbitMQConsts.CLIENT_ID_TASK_JOB, msg =>
            {
                Logger.WriteLog($"[Task] {RabbitMQConsts.CLIENT_ID_TASK_JOB} msg:{msg}");
                System.Threading.ThreadPool.QueueUserWorkItem(TaskHelper.TaskHelper.TaskHandleAsync, msg);
                return ConsumeAction.Accept;
            });
        }

        #endregion

    }

}
