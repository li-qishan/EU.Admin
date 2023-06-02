
import { GetModuleInfo } from '../../../services/common';

const Model = {
    namespace: 'ivquery',
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
        }
    },
    reducers: {
        getModuleInfoSuccess(state, { payload }) {
            return { ...state, moduleInfo: payload };
        },
        setTableStatusSuccess(state, { payload }) {
            return { ...state, tableParam: payload };
        },
    },
};
export default Model;
