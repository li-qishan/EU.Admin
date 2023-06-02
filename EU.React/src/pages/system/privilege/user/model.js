import { history } from 'umi';
import { message } from 'antd';
import { SaveAddFormData, SaveEditFormData, query, GetById, Delete, BatchInsertUserRole, GetUserRole } from './service';
import { GetModuleInfo } from '../../../../services/common';

const Model = {
    namespace: 'smuser',
    state: {
        moduleInfo: null,
        Id: null
    },
    effects: {
        *getModuleInfo({ payload }, { call, put }) {
            const response = yield call(GetModuleInfo, payload);
            yield put({
                type: 'getModuleInfoSuccess',
                payload: response,
            });
        },
        *setTableStatus({ payload }, { call, put }) {
            yield put({
                type: 'setTableStatusSuccess',
                payload: payload,
            });
        },
        *saveData({ payload }, { call, put }) {
            const response = yield call(payload.ID ? SaveEditFormData : SaveAddFormData, payload);
            if (response.status == "ok") {
                yield put({
                    type: 'saveSuccess',
                    payload: payload.ID ? payload.ID : response.Id,
                });
                message.success(response.message);
            }
            else {
                message.error(response.message);
            }
        },
        *getById({ payload }, { call, put }) {
            const response = yield call(GetById, payload);
            if (response) {
                return { response };
            }
        },
        *delete({ payload }, { call, put }) {
            const response = yield call(Delete, payload);
            if (response.status == "ok") {
                message.success(response.message);
            }
            else {
                message.error(response.message);
            }
        },
        *saveUserRole({ payload }, { call, put }) {
            const response = yield call(BatchInsertUserRole, payload);
            if (response.status == "ok") {
                message.success(response.message);
            }
            else {
                message.error(response.message);
            }
        },
        *getUserRole({ payload }, { call, put }) {
            const response = yield call(GetUserRole, payload);
            if (response.status == "ok") {
                return response.data;
            }
            else {
                message.error(response.message);
            }
        },
    },
    reducers: {
        getModuleInfoSuccess(state, { payload }) {
            var moduleInfo = payload;
            return { ...state, moduleInfo };
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
