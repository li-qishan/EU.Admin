using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.CAP;
using EU.Core.Const;
using EU.Core.Services;
using EU.Model.System;

namespace EU.EventBus.Subscriptions
{

    public class TestService : ISubscriberService, ICapSubscribe
    {
        public TestService() { }

        [CapSubscribe("bus.app.test")]
        public async Task Handle(testmodel test)
        {
            try
            {
                Logger.WriteLog($"[测试操作Bus：] {test.ID}");
                Logger.WriteLog($"[测试操作Bus：] {test.Name}");

            }
            catch (Exception e)
            {
                Logger.WriteLog($"[测试操作Bus异常：] {e.Message}");

                throw new Exception(e.Message, e);
            }
        }
    }
}
