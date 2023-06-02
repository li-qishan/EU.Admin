
using EU.Core.Services;
using EU.Core.Utilities;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System.Data.Common;
using System;
using System.Threading.Tasks;

/// <summary>
/// 这里要注意下，命名空间和程序集是一样的，不然反射不到
/// </summary>
namespace EU.TaskHelper
{
    public class DbBackup : JobBase, IJob
    {
        //private readonly IBlogArticleServices _blogArticleServices;

        //public Job_Blogs_Quartz(IBlogArticleServices blogArticleServices, ITasksQzServices tasksQzServices)
        //{
        //    _blogArticleServices = blogArticleServices;
        //    _tasksQzServices = tasksQzServices;
        //}

        public DbBackup(string code = "DbBackup", string name = "数据库备份") : base(code, name)
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
            Logger.WriteLog($"开始执行数据库备份！");
            BackupDatabase();
            Logger.WriteLog($"结束执行数据库备份！");
        }

        #region 数据库备份
        private static string DbBackupPath
        {
            get
            {
                try
                {
                    //string backupPath = Utility.GetAppSettingKey("DbBackupPath");
                    string backupPath = null;
                    if (string.IsNullOrEmpty(backupPath))
                    {
                        backupPath = @"C:\Backup\";
                    }
                    return backupPath;
                }
                catch (Exception E)
                {
                    return @"C:\Backup\";
                }
            }
        }
        private static void BackupDatabase()
        {
            try
            {
                string databaseName = DBHelper.DatabaseName;
                FileHelper.CreateDirectory(DbBackupPath + databaseName + "/");
                string saveAway = DbBackupPath + databaseName + "\\" + databaseName + "_" + Core.Utilities.Utility.GetSysDateTimeString().Replace("/", "").Replace(" ", "").Replace(":", "") + ".bak";
                string cmdText = @"BACKUP DATABASE " + databaseName + " TO DISK='" + saveAway + "'";
                DBHelper.Instance.ExcuteNonQuery(cmdText);
            }
            catch (Exception E)
            {
                throw;
            }
        }

        #endregion
    }
}
