import { queryCurrent, query as queryUsers, getMenuData, getUserDepartment } from '@/services/user';

const UserModel = {
  namespace: 'user',
  state: {
    currentUser: {},
    menuData: [],
    departmentData: {}
  },
  effects: {
    *fetch(_, { call, put }) {
      const response = yield call(queryUsers);
      yield put({
        type: 'save',
        payload: response,
      });
    },

    *fetchCurrent(_, { call, put }) {
      const response = yield call(queryCurrent);
      yield put({
        type: 'saveCurrentUser',
        payload: response,
      });
    },
    *getMenuData({ state, payload }, { call, put }) {
      const response = yield call(getMenuData);
      var menuData = [];
      if (response.status == "ok") {
        if (response.data && response.data.length > 0) {
          menuData = response.data;
        }
      }
      yield put({
        type: 'getMenuDataSuccess',
        payload: response,
      });
      return menuData
    },
    *getUserDepartment({ payload }, { call }) {
      const response = yield call(getUserDepartment, payload);
      if (response) {
        return { response };
      }
    }
  },
  reducers: {
    saveCurrentUser(state, action) {
      return { ...state, currentUser: action.payload || {} };
    },

    changeNotifyCount(
      state = {
        currentUser: {},
      },
      action,
    ) {
      return {
        ...state,
        currentUser: {
          ...state.currentUser,
          notifyCount: action.payload.totalCount,
          unreadCount: action.payload.unreadCount,
        },
      };
    },
    getMenuDataSuccess(state, { payload }) {
      state.menuData = payload.data;
      return { ...state };
    }
  }
};
export default UserModel;
