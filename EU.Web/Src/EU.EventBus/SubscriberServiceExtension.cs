using System;
using System.Linq;
using EU.Core.Configuration;
using EU.Core.Services;
using EU.EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace EU.EventBus
{
    public static class SubscriberServiceExtension
    {
        public static void AddEventBusSetup(this IServiceCollection services)
        {

            AddService(services, true);

        }
        public static void AddEventBusSetup(this ServiceCollection services)
        {
            AddService(services, true);
        }

        public static void AddService(IServiceCollection services, bool enable)
        {
            Logger.WriteLog("[Task]EventBus 开启");
            if (enable)
            {
                var assembly = typeof(ISubscriberService).Assembly;

                var subscriberServiceTypes = assembly.GetExportedTypes()
                    .Where(type => type.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

                foreach (var subscriberServiceType in subscriberServiceTypes)
                {
                    services.AddTransient(subscriberServiceType);
                }
            }

            services.AddCap(x =>
            {
                var rabbitMQ = AppSetting.RabbitMQConfiguration;
                x.DefaultGroupName = "cap.event.bus";

                x.UseDashboard();

                x.UseRabbitMQ(x =>
                {
                    x.HostName = rabbitMQ.HostName;
                    x.Port = rabbitMQ.Port;
                    x.UserName = rabbitMQ.UserName;
                    x.Password = rabbitMQ.Password;
                });
                var dbType = AppSetting.DBType;
                if (dbType == "MsSql")
                    x.UseSqlServer(AppSetting.DbConnectionString);
                else if (dbType == "MySql")
                    x.UseMySql(AppSetting.DbConnectionString);
                else if (dbType == "PostgreSql")
                    x.UsePostgreSql(AppSetting.DbConnectionString);
            });
        }


    }


}
