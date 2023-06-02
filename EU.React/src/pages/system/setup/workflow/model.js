import { history } from 'umi';
import { message } from 'antd';
import { SaveAddFormData, SaveEditFormData, query, GetById, Delete, SaveFlowData, GetFlowData } from './service';
import { GetModuleInfo } from '../../../../services/common';

const Model = {
    namespace: 'workflow',
    state: {
        moduleInfo: null,
        Id: null,
        structureId: null,
        flowData: []
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
        *setStructure({ payload }, { call, put }) {
            yield put({
                type: 'setStructureSuccess',
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
        *getFlowData({ payload }, { call, put }) {
            const response = yield call(GetFlowData, payload);
            yield put({
                type: 'getFlowDataSuccess',
                payload: response,
            });
        },
        *saveFlowData({ payload }, { call, put }) {
            const response = yield call(SaveFlowData, payload);
            if (response.status == "ok") {
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
        setStructureSuccess(state, { payload }) {
            return { ...state, structureId: payload };
        },
        getFlowDataSuccess(state, { payload }) {
            var flowData = payload.data;
            return { ...state, flowData: flowData };
        },
        saveSuccess(state, { payload }) {
            return { ...state, Id: payload };
        },
    },
};
export default Model;
