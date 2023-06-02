import { message } from 'antd';
import { SaveAddFormData, SaveEditFormData, GetById, Delete, getByModuleCode } from './service';
import { GetModuleInfo } from '../../../services/common';

const Model = {
    namespace: 'importconfig',
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
                payload: payload,
            });
        },
        *saveData({ payload }, { call, put }) {
            const response = yield call(payload.ID ? SaveEditFormData : SaveAddFormData, payload);

            yield put({
                type: 'saveSuccess',
                payload: payload.ID ? payload.ID : response.Id ? response.Id : null,
            });
            if (response.status == "ok")
                message.success(response.message);
            else
                message.error(response.message);
        },
        *getById({ payload }, { call }) {
            const response = yield call(GetById, payload);
            if (response) {
                return { response };
            }
        },
        *delete({ payload }, { call }) {
            const response = yield call(Delete, payload);
            if (response.status == "ok") {
                message.success(response.message);
            }
            else {
                message.error(response.message);
            }
        },
        *setDetail({ payload }, { put }) {
            yield put({
                type: 'setDetailSuccess',
                payload: payload,
            });
        },
        *getByModuleCode({ payload }, { call }) {
            const response = yield call(getByModuleCode, payload);
            if (response) {
                return { response };
            }
        }
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
        setDetailSuccess(state, { payload }) {
            return { ...state, detail: payload };
        },
    },
};
export default Model;
