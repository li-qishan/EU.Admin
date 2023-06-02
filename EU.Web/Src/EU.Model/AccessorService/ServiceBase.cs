using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace EU.Web
{
    public abstract class ServiceBase
    {
        /// <summary>
        /// 身份信息
        /// </summary>
        protected IClaimsAccessor Claims { get; set; }

        /// <summary>
        /// cotr
        /// </summary>
        protected ServiceBase()
        {
            Claims = ServiceProviderInstance.Instance.GetRequiredService<IClaimsAccessor>();
        }
    }
}
