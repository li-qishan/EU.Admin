import { history } from 'umi';
import { message } from 'antd';
import { SaveAddFormData, SaveEditFormData, GetById, Delete, DeleteDetail, SaveAddDetailFormData, SaveEditDetailFormData, GetDetailById } from './service';
import { GetModuleInfo } from '../../../../services/common';

const Model = {
    namespace: 'setparam',
    state: {
        moduleInfo: null,
        Id: null,
        DetailId: null
    },
    effects: {
        *getModuleInfo({ payload }, { call, put }) {
            const response = yield call(GetModuleInfo, payload);
            yield put({
                type: 'getModuleInfoSuccess',
                payload: response,
            });
        },
        *getDetailModuleInfo({ payload }, { call, put }) {
            const response = yield call(GetModuleInfo, payload);
            yield put({
                type: 'getDetailModuleInfoSuccess',
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
        *saveDetailData({ payload }, { call, put }) {
            const response = yield call(payload.ID ? SaveEditDetailFormData : SaveAddDetailFormData, payload);
            if (!payload.SmLovId) {
                message.error('请先保存主表数据！');
                return false;
            }
            if (response.status == "ok") {
                yield put({
                    type: 'saveDetailSuccess',
                    payload: payload.ID ? payload.ID : response.Id,
                });
                message.success(response.message);
            }
            else {
                message.error(response.message);
            }
        },
        *getDetailById({ payload }, { call, put }) {
            const response = yield call(GetDetailById, payload);
            if (response) {
                return { response };
            }
        },
        *deleteDetail({ payload }, { call, put }) {
            const response = yield call(DeleteDetail, payload);
            if (response.status == "ok") {
                message.success(response.message);
            }
        }
    },
    reducers: {
        getModuleInfoSuccess(state, { payload }) {
            var moduleInfo = payload;
            return { ...state, moduleInfo };
        },
        getDetailModuleInfoSuccess(state, { payload }) {
            var detailModuleInfo = payload;
            return { ...state, detailModuleInfo };
        },
        setTableStatusSuccess(state, { payload }) {
            return { ...state, tableParam: payload };
        },
        saveSuccess(state, { payload }) {
            return { ...state, Id: payload };
        },
        saveDetailSuccess(state, { payload }) {
            return { ...state, DetailId: payload };
        },
    },
};
export default Model;
