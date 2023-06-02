using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EU.Core.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EU.DataAccess
{
    /// <summary>
    /// 直接创建 HDISContext
    /// </summary>
    public class ContextFactory
    {
        /// <summary>
        /// 创建DbContext
        /// </summary>
        /// <returns></returns>
        public static DataContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();

            builder.UseSqlServer(AppSetting.DbConnectionString);
            return new DataContext(builder.Options);
        }
    }
}
