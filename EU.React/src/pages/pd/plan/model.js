import { message } from 'antd';
import { SaveAddFormData, SaveEditFormData, GetById, Delete, AuditOrder } from './service';
import { GetModuleInfo } from '../../../services/common';

const Model = {
    namespace: 'pdplan',
    state: {
        moduleInfo: { columns: [] },
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
                payload: payload.ID ? payload.ID : response.Id ? response.Id : null,
            });
            if (response.status === "ok")
                message.success(response.message);
            else
                message.error(response.message);
        },
        *getById({ payload }, { call }) {
            const response = yield call(GetById, payload);
            if (response)
                return { response };
            return null;

        },
        *delete({ payload }, { call }) {
            const response = yield call(Delete, payload);
            if (response.status === "ok") {
                message.success(response.message);
            }
            else {
                message.error(response.message);
            }
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
            const moduleInfo = payload;
            return { ...state, moduleInfo };
        },
        setTableStatusSuccess(state, { payload }) {
            return { ...state, tableParam: payload };
        },
        saveSuccess(state, { payload }) {
            return { ...state, Id: payload };
        }
    },
};
export default Model;
