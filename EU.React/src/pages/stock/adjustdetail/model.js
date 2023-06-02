import { message } from 'antd';
import { SaveAddFormData, SaveEditFormData, GetById, Delete, BatchSaveAddFormData } from './service';
import { GetModuleInfo } from '../../../services/common';

const Model = {
    namespace: 'ivadjustdetail',
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
            if (response.status === "ok") {
                yield put({
                    type: 'saveSuccess',
                    // eslint-disable-next-line no-nested-ternary
                    payload: payload.ID ? payload.ID : response.Id ? response.Id : null,
                });
                message.success(response.message);
            }
            else
                message.error(response.message);

            return response;
        },
        // eslint-disable-next-line consistent-return
        *getById({ payload }, { call }) {
            const response = yield call(GetById, payload);
            if (response)
                return response;
        },
        *delete({ payload }, { call }) {
            const response = yield call(Delete, payload);
            if (response.status === "ok")
                message.success(response.message);
            else
                message.error(response.message);
        },
        *setId({ payload }, { put }) {
            yield put({
                type: 'setIdSuccess',
                payload
            });
        },
        *batchSaveData({ payload }, { call }) {
            const response = yield call(BatchSaveAddFormData, payload);
            if (response.status === "ok")
                message.success(response.message);
            else
                message.error(response.message);
            return response;
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
        setIdSuccess(state, { payload }) {
            return { ...state, Id: payload };
        }
    },
};
export default Model;
