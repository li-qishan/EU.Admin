import { message } from 'antd';
import { Delete } from './service';
import { GetModuleInfo } from '../../../services/common';

const Model = {
    namespace: 'ivqtychange',
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
    },
};
export default Model;
