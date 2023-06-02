import { message } from 'antd';
import { SaveAddFormData, SaveEditFormData, GetById, Delete, AuditOrder } from './service';
import { GetModuleInfo } from '../../../services/common';

const Model = {
    namespace: 'ivcheck',
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
            if (response.status === "ok")
                message.success(response.message);
            else
                message.error(response.message);
        },
        // eslint-disable-next-line consistent-return
        *getById({ payload }, { call }) {
            const response = yield call(GetById, payload);
            if (response) 
                return { response };
        },
        *delete({ payload }, { call }) {
            const response = yield call(Delete, payload);
            if (response.status === "ok") 
                message.success(response.message);
            else 
                message.error(response.message);
        },
        *setDetail({ payload }, { put }) {
            yield put({
                type: 'setDetailSuccess',
                payload,
            });
        },
        *auditOrder({ payload }, { call }) {
            const response = yield call(AuditOrder, payload);
            if (response)
                return { response };
            return null;
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
            return { ...state, detail: payload };
        },
    },
};
export default Model;
