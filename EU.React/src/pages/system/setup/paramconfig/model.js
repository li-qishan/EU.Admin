import { history } from 'umi';
import { message } from 'antd';
import { SaveAddFormData, SaveEditFormData, query, GetById, Delete, queryByGroup } from './service';
import { GetModuleInfo } from '../../../../services/common';

const Model = {
    namespace: 'paramconfig',
    state: {
        moduleInfo: { columns: [] },
        Id: null,
        groups: []
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
            else
                message.error(response.message);

        },
        *getById({ payload }, { call, put }) {
            const response = yield call(GetById, payload);
            if (response)
                return { response };
        },
        *delete({ payload }, { call, put }) {
            const response = yield call(Delete, payload);
            if (response.status == "ok")
                message.success(response.message);
            else
                message.error(response.message);
        },
        *queryByGroup({ payload }, { call, put }) {
            const response = yield call(queryByGroup, payload);
            if (response.Code == 10000)
                yield put({
                    type: 'queryByGroupSuccess',
                    payload: response.Data,
                });
            else
                message.error(response.Message);
        },
        *updateGroup({ payload }, { call, put }) {
            yield put({
                type: 'queryByGroupSuccess',
                payload: payload
            });
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
        queryByGroupSuccess(state, { payload }) {
            return { ...state, groups: payload };
        }
    },
};
export default Model;
