import { Mode } from 'use-switch-tabs';

export default {
  navTheme: "dark",
  // 拂晓蓝
  primaryColor: "#1890ff",
  layout: "side",
  contentWidth: "Fluid",
  fixedHeader: false,
  autoHideHeader: false,
  fixSiderbar: true,
  colorWeak: false,
  menu: {
    locale: true
  },
  title: "优智云",
  webUrl: "http://admin.loan.eu-keji.com:8016/",
  pwa: false,
  iconfontUrl: "",
  fontSizeBase:'14px',
  switchTabs: {
    mode: Mode.Route,
    fixed: true,
    reloadable: true,
    persistent: {
      force: true,
    },
  },
};
