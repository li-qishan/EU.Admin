using Senparc.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU.WeixinService
{
    public class WxOpenService
    {
        /// <summary>
        /// 客服Socket 客服userid/客服的socket
        /// </summary>
        public static Dictionary<string, WebSocketHelper> kfSocket = new Dictionary<string, WebSocketHelper>();
        /// <summary>
        /// //小程序客户聊天指派小程序客户 opendId/客服userid
        /// </summary>
        public static Dictionary<string, string> assignKF = new Dictionary<string, string>();
    }
}
