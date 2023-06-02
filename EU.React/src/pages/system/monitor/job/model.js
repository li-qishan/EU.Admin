import { message } from 'antd';
import { SaveAddFormData, SaveEditFormData, Get, Delete } from './service';
import { GetModuleInfo } from '../../../../services/common';

const Model = {
  namespace: 'smjob',
  state: {
    moduleInfo: { columns: [] },
    Id: null,
    detail: {}
  },
  effects: {
    *getModuleInfo({ payload }, { call, put }) {
      const response = yield call(GetModuleInfo, payload);
      yield put({
        type: 'getModuleInfoSuccess',
        payload: response,
      });
    },
    *setTableStatus({ payload }, { put }) {
      yield put({
        type: 'setTableStatusSuccess',
        payload,
      });
    },
    *saveData({ payload }, { call, put }) {
      const response = yield call(payload.ID ? SaveEditFormData : SaveAddFormData, payload);

      yield put({
        type: 'saveSuccess',
        // eslint-disable-next-line no-nested-ternary
        payload: payload.ID ? payload.ID : response.Id ? response.Id : null,
      });
      return { response };
    },
    // eslint-disable-next-line consistent-return
    *get({ payload }, { call, put }) {
      const response = yield call(Get, payload);
      yield put({
        type: 'setDetailSuccess',
        payload: response.Data,
      });
      return response;
    },
    *delete({ payload }, { call }) {
      const response = yield call(Delete, payload);
      if (response.status === "ok")
        message.success(response.message);
      else
        message.error(response.message);

    }
  },
  reducers: {
    getModuleInfoSuccess(state, { payload }) {
      return { ...state, moduleInfo: payload };
    },
    setTableStatusSuccess(state, { payload }) {
      return { ...state, tableParam: payload };
    },
    saveSuccess(state, { payload }) {
      return { ...state, Id: payload };
    },
    setDetailSuccess(state, { payload }) {
      debugger
      return { ...state, detail: payload };
    },
  },
};
export default Model;
