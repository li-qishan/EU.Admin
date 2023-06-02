import { history } from 'umi';
import { message } from 'antd';
import { SaveAddFormData, SaveEditFormData, query, GetById, Delete, GetAllMaterialType } from './service';
import { GetModuleInfo } from '../../../services/common';

const Model = {
    namespace: 'materialtype',
    state: {
        moduleInfo: { columns: [] },
        Id: null,
        treeData: []
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
            yield put({
                type: 'saveSuccess',
                payload: payload.ID ? payload.ID : response.Id ? response.Id : null,
            });
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
        },
        *getAllMaterialType({ payload }, { call, put }) {
            const response = yield call(GetAllMaterialType, payload);
            yield put({
                type: 'getAllMaterialTypeSuccess',
                payload: response
            });
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
        getAllMaterialTypeSuccess(state, { payload }) {
            return { ...state, treeData: [payload.data] };
        },
    },
};
export default Model;
