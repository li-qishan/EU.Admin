using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EU.Core;
using EU.Core.Configuration;
using EU.Core.Const;
using EU.TaskHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Azure.Core.HttpHeader;
using EU.EventBus;
using DotNetCore.CAP;
using EU.Model.System;

namespace EU.Web.BackgroundJobs
{
    class Program
    {
        static async Task Main(string[] args)
        {

            // 注入事件服务
            var container = new ServiceCollection();
            container.AddLogging();
            container.AddJobSetup();
            var sp = container.BuildServiceProvider();
            //await sp.GetService<IBootstrapper>().BootstrapAsync();
            var schedulerCenter = sp.GetService<ISchedulerCenter>();

            // 任务处理中心
            TaskCenter taskCenter = new TaskCenter(schedulerCenter);
            taskCenter.Start();
            container.AddEventBusSetup();

            #region EventBus 测试代码
            sp = container.BuildServiceProvider();
            Thread.Sleep(5000);
            await sp.GetService<IBootstrapper>().BootstrapAsync();
            var _capPublisher = sp.GetService<ICapPublisher>();


            for (int i = 0; i < 100; i++)
            {
                var sm = new testmodel()
                {
                    ID = Guid.NewGuid(),
                    Name = i.ToString(),
                };

                _capPublisher.Publish("bus.app.test", sm);

            }
            #endregion

            Thread.Sleep(Timeout.Infinite);

        }
        public static bool IsPortOpen(string host, int port, TimeSpan timeout)
        {
            try
            {
                using var client = new System.Net.Sockets.TcpClient();
                var result = client.BeginConnect(host, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(timeout);
                client.EndConnect(result);
                return success;
            }
            catch
            {
                return false;
            }
        }
    }
}
