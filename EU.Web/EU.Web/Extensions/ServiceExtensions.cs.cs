
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EU.Core.Const;
using EU.Core;
using EU.TaskHelper;
using EU.Web.Controllers;
using EU.Core.Services;
using EU.Core.Utilities;

namespace JianLian.HDIS.HttpApi.Hosting.Extensions
{
    public static class ServiceExtensions
    {
        public static void Init()
        {
            //TaskCallback消息订阅
            Logger.WriteLog("[TaskCallback]启动消息订阅");
            RabbitMQHelper.ConsumeMsg<TaskCallbackMsg>(RabbitMQConsts.CLIENT_ID_TASK_CALLBACK, msg =>
            {
                 ThreadPool.QueueUserWorkItem(TaskHelper.TaskCallback, msg);
                return ConsumeAction.Accept;
            });
            //启动内存监控
            //new Thread(MemMonitor).Start();



        }


        #region 内存监控
        /// <summary>
        /// 内存监控
        /// </summary>
        //private static void MemMonitor()
        //{
        //    while (true)
        //    {
        //        var p = System.Diagnostics.Process.GetCurrentProcess();

        //        long mem = p.WorkingSet64 / 1024 / 1024;

        //        if (Utility.GetSysDateTime().Hour == 3 && mem > 1024)
        //        {
        //            Logger.WriteLog($"{DateTimeHelper.Now():HH:mm:ss}  内存超限 {mem}，程序退出");
        //            Environment.Exit(-1);
        //            break;
        //        }
        //        Thread.Sleep(5000);
        //    }
        //}
        #endregion
    }
}
