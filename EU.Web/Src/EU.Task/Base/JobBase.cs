
using EU.Core.Utilities;
using EU.Core;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EU.Core.Const;
using Microsoft.AspNetCore.Components.Forms;
using EU.Model;
using EU.Core.LogHelper;

namespace EU.TaskHelper
{
    public abstract class JobBase
    {

        public bool m_Suc = false;
        public string m_Remark = string.Empty;
        public string m_FailReason = string.Empty;
        /// <summary>
        ///  任务编码
        /// </summary>
        public string m_Code { get; set; }

        /// <summary>
        ///  任务名称
        /// </summary>
        public string m_Name { get; set; }

        public JobBase(string code, string name)
        {
            m_Code = code;
            m_Name = name;

            //加载启用状态
            lock (TaskHelper.m_ClientQuartzs)
            {
                if (!TaskHelper.m_ClientQuartzs.Any(o => o.m_Code == code))
                    TaskHelper.m_ClientQuartzs.Add(this);
            }
        }

        //public ITasksQzServices _tasksQzServices;
        /// <summary>
        /// 执行指定任务
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        public async Task<string> ExecuteJob(IJobExecutionContext context, Func<System.Threading.Tasks.Task> func)
        {
            //记录Job时间
            Stopwatch stopwatch = new Stopwatch();
            //JOBID
            var jobid = context.JobDetail.Key.Name;
            //JOB组名
            string groupName = context.JobDetail.Key.Group;
            //任务
            var task = TaskHelper.GetSmQuartzJob(Guid.Parse(jobid));

            //日志
            string jobHistory = $"【{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}】【执行开始】【任务：{task.JobName}】";
            //耗时
            double taskSeconds = 0;
            string lastResult = JobConsts.TASK_EXEC_RESULT_FAIL;
            //DateTime curTime = Utility.GetSysDate()
            var lastTime = DateTimeHelper.Now();
            DateTime? m_NextTime = lastTime;

            try
            {
                stopwatch.Start();
                await func();//执行任务
                stopwatch.Stop();
                jobHistory += $"，【{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}】【执行成功】";
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new JobExecutionException(ex);
                //true  是立即重新执行任务 
                e2.RefireImmediately = false;
                jobHistory += $"，【{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}】【执行失败:{ex.Message}】";

                taskSeconds = Math.Round(stopwatch.Elapsed.TotalSeconds, 3);
            }
            finally
            {
                taskSeconds = Math.Round(stopwatch.Elapsed.TotalSeconds, 3);
                lastResult = JobConsts.TASK_EXEC_RESULT_SUCCESS;
                jobHistory += $"，【{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}】【执行结束】(耗时:{taskSeconds}秒)";

                var curTime = DateTime.UtcNow.AddHours(8);
                CronExpression expression = new CronExpression(task.ScheduleRule);
                expression.TimeZone = TimeZoneInfo.Utc;
                m_NextTime = expression?.GetNextValidTimeAfter(curTime).Value.DateTime;

            }

            _ = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                var curTime = Utility.GetSysDate();
                DbUpdate du = new DbUpdate("SmQuartzJob");
                du.IsInitDefaultValue = false;
                du.Set("LastResult", lastResult);
                du.Set("LastExecuteTime", curTime);
                du.Set("NextExecuteTime", m_NextTime);
                du.Set("LastCost", Convert.ToInt32(taskSeconds));
                du.Where("ID", "=", jobid);
                DBHelper.Instance.ExecuteScalar(du.GetSql());

                DbInsert di = new DbInsert("SmQuartzJobLog");
                di.IsInitDefaultValue = false;
                di.Values("SmQuartzJobId", task.ID);
                di.Values("JobCode", task.JobCode);
                di.Values("ExecuteTime", curTime);
                DBHelper.Instance.ExecuteDML(di.GetSql());
            });

            return jobHistory;
        }

        #region 当前日志
        /// <summary>
        /// 提供最新的200条日志记录
        /// </summary>
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="msg"></param>
        public void SendLog(string msg)
        {
            TaskHelper.AddQuartzLog(m_Code, msg, false);
            Logger.WriteLog($"[{m_Name}] {msg}");
        }
        #endregion
    }
}
