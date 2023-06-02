using EU.Core.Utilities;
using EU.Model;
using EU.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU.Core.WeiXin
{

    public class WxConfigContainer
    {
        /// <summary>
        /// 暂存微信config
        /// </summary>
        private Dictionary<string, WxConfig> wxConfigContainer = new Dictionary<string, WxConfig>();
        private Dictionary<string, string> defineKeyToBuildKey = new Dictionary<string, string>();
        public WxConfigContainer()
        {
            List<WxConfig> smConfigs = DBHelper.Instance.QueryList<WxConfig>("SELECT * FROM WxConfig WHERE IsDeleted='false'", null);
            foreach (var item in smConfigs)
            {
                if (!string.IsNullOrEmpty(item.WeixinId))
                {
                    try
                    {
                        wxConfigContainer.Add(item.WeixinId, item);
                        defineKeyToBuildKey.Add(item.OriginId + item.AppId, item.WeixinId);
                        if (item.InterfaceType == "A02")
                        {
                            Senparc.Weixin.Work.Containers.AccessTokenContainer.RegisterAsync(item.OriginId, item.AppSecret).Wait();
                        }
                        else
                        {
                            Senparc.Weixin.MP.Containers.AccessTokenContainer.RegisterAsync(item.AppId, item.AppSecret).Wait();
                        }
                    }
                    catch (Exception)
                    {
                        //Logger.WriteLog("Weixin", e.Message);
                    }
                }
            }
        }

        public string GetToken(string weixinId, bool getNewToken = false)
        {
            //Logger.WriteLog("Weixin", $"后去token{weixinId}");
            WxConfig wxConfig;
            if (wxConfigContainer.ContainsKey(weixinId))
            {
                wxConfig = wxConfigContainer[weixinId];
                if (wxConfig.InterfaceType == "A02")
                {
                    return Senparc.Weixin.Work.Containers.AccessTokenContainer.GetToken(wxConfig.OriginId, wxConfig.AppSecret, getNewToken);
                }
                else
                {
                    return Senparc.Weixin.MP.Containers.AccessTokenContainer.GetAccessToken(wxConfig.AppId, getNewToken);
                }
            }
            else
            {
                return $"{weixinId}不存在";
            }
        }

        public string GetWorkToken(string buildKey)
        {
            string weixinId;
            if (defineKeyToBuildKey.TryGetValue(buildKey, out weixinId))
            {
                return GetToken(weixinId);
            }
            else
            {
                return $"{buildKey}不存在";
            }
        }

        public WxConfig GetConfig(string weixinId)
        {
            WxConfig wxConfig = null;
            if (wxConfigContainer.ContainsKey(weixinId))
            {
                wxConfig = wxConfigContainer[weixinId];
            }
            return wxConfig;
        }

        public void Use()
        {

        }
    }
}
