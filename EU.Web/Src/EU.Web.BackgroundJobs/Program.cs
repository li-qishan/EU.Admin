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

namespace EU.Web.BackgroundJobs
{
    class Program
    {
        static void Main(string[] args)
        {

            // 注入事件服务
            var container = new ServiceCollection();
            container.AddJobSetup();

            var sp = container.BuildServiceProvider();
            var schedulerCenter = sp.GetService<ISchedulerCenter>();

            // 任务处理中心
            TaskCenter taskCenter = new TaskCenter(schedulerCenter);
            taskCenter.Start();

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
