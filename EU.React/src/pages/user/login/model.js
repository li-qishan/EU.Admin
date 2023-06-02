import { history } from 'umi';
import { message } from 'antd';
import { fakeAccountLogin, getFakeCaptcha } from './service';
import { getPageQuery, setAuthority } from './utils/utils';
import Utility from '@/utils/utility';
import { string } from 'prop-types';

const Model = {
  namespace: 'userAndlogin',
  state: {
    status: undefined,
  },
  effects: {
    *login({ payload }, { call, put }) {
      const response = yield call(fakeAccountLogin, payload);
      yield put({
        type: 'changeLoginStatus',
        payload: response,
      }); // Login successfully

      if (response.Success) {
        Utility.addCache("userId", response.Data.UserId);
        let modules = response.Data.Modules;
        let tabLocationstr = Utility.getCache("tabLocations");
        if (tabLocationstr != null) {
          let tabLocations = [];
          let tabLocations1 = JSON.parse(tabLocationstr);
          tabLocations1.map((item) => {
            if (modules.findIndex(x => x.RoutePath === item.pathname) > -1 || item.pathname == '/welcome') {
              tabLocations.push(item);
            }
          });
          Utility.addCache("tabLocations", JSON.stringify(tabLocations));
        }
        message.success('登录成功！');
        const urlParams = new URL(window.location.href);
        const params = getPageQuery();
        let { redirect } = params;

        if (redirect) {
          const redirectUrlParams = new URL(redirect);

          if (redirectUrlParams.origin === urlParams.origin) {
            redirect = redirect.substr(urlParams.origin.length);

            if (redirect.match(/^\/.*#/)) {
              redirect = redirect.substr(redirect.indexOf('#') + 1);
            }
          } else {
            window.location.href = redirect;
            return;
          }
        }
        window.location.href = redirect || '/';//跳转并刷新触发umi运行时配置，重新获取路由
        // history.replace(redirect || '/');
      } else {
        message.destroy();
        message.error(response.Message);
      }
    },

    *getCaptcha({ payload }, { call }) {
      yield call(getFakeCaptcha, payload);
    },
  },
  reducers: {
    changeLoginStatus(state, { payload }) {
      setAuthority(payload.Data.Token ? payload.Data.Token : 'guest');
      return { ...state, status: payload.Code, type: payload.type };
    },
  },
};
export default Model;
