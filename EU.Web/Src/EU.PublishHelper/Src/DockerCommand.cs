using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianLian.HDIS.PublishHelper
{
    /// <summary>
    /// Docker命令封装
    /// </summary>
    public class DockerCommand
    {
        /// <summary>
        /// docker ps
        /// </summary>
        /// <param name="hospital"></param>
        /// <returns></returns>
        public static string GetDockerStatus(string hospital)
        {
            return $"sudo docker ps -a --format \"table {{{{.ID}}}}||{{{{.Names}}}}||{{{{.Image}}}}||{{{{.Command}}}}||{{{{.CreatedAt}}}}||{{{{.RunningFor}}}}||{{{{.Ports}}}}||{{{{.Status}}}}||{{{{.Networks}}}}\" | grep \"{hospital}_\"";
        }
        /// <summary>
        /// docker containerName
        /// </summary>
        /// <param name="hospital"></param>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public static string GetDockerContainerName(string hospital, string modelName)
        {
            return $"sudo docker ps -a --format \"table {{{{.Names}}}}\" | grep \"{hospital}_{modelName}\"";
        }
        /// <summary>
        /// docker logs
        /// </summary>
        /// <param name="hospital"></param>
        /// <returns></returns>
        public static string GetDockerLogs(string containerName)
        {
            return $"sudo docker logs {containerName} --tail=50";
        }
        /// <summary>
        /// docker-compose down
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="hospital"></param>
        /// <returns></returns>
        public static string DockerComposeDown(string userName, string hospital)
        {
            return $"cd /home/{userName}/ihdis/compose/{hospital};sudo docker-compose down";
        }
        /// <summary>
        /// docker-compose up
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="hospital"></param>
        /// <returns></returns>
        public static string DockerComposeUp(string userName, string hospital)
        {
            return $"cd /home/{userName}/ihdis/compose/{hospital};sudo docker-compose up -d";
        }
        /// <summary>
        /// docker-compose restart
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="hospital"></param>
        /// <returns></returns>
        public static string DockerComposeRestart(string userName, string hospital)
        {
            return $"cd /home/{userName}/ihdis/compose/{hospital};sudo docker-compose restart";
        }
        /// <summary>
        /// docker-compose remove
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="hospital"></param>
        /// <returns></returns>
        public static string DockerComposeRemove(string userName, string hospital, string cname)
        {
            return $"cd /home/{userName}/ihdis/compose/{hospital};sudo docker-compose stop {cname};sudo docker-compose rm -f {cname}";
        }
        /// <summary>
        /// docker container restart
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public static string DockeContainerRestart(string containerId)
        {
            return $"sudo docker restart {containerId}";
        }
        /// <summary>
        /// docker-compose restart container
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="hospital"></param>
        /// <returns></returns>
        public static string DockerComposeContainerRestart(string userName, string hospital, string container)
        {
            return $"cd /home/{userName}/ihdis/compose/{hospital};sudo docker-compose restart {container}";
        }
        /// <summary>
        /// docker container log
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public static string DockeContainerLog(string userName, string hospital, string container)
        {
            return $"cd /home/{userName}/ihdis/compose/{hospital};sudo docker-compose logs {container}";
        }
    }
}
