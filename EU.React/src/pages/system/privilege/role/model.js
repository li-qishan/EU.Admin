import { message } from 'antd';
import { SaveAddFormData, SaveEditFormData, GetRoleModule, Get, Delete, BatchInsertRoleModule, GetModuleFunction, SaveRoleFunction, SaveRoleFuncPriv, GetRoleFuncPriv } from './service';
import { GetModuleInfo } from '../../../../services/common';

let noAction = [];
let addAction = [];
const Model = {
    namespace: 'smrole',
    state: {
        moduleInfo: null,
        Id: null,
        functionData: []
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
          const response = yield call(Get, payload);
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
        //#region 模块树
        *saveRoleModule({ payload }, { call, put }) {
            const response = yield call(BatchInsertRoleModule, payload);
            if (response.status == "ok") {
                message.success(response.message);
            }
            else {
                message.error(response.message);
            }
        },
        *getRoleModule({ payload }, { call, put }) {
            const response = yield call(GetRoleModule, payload);
            if (response.Success) {
                return response.Data;
            }
            else {
                message.error(response.message);
            }
        },
        //#endregion

        //#region 基础功能权限

        //设置添加删除权限数据
        *setNoAction({ payload }, { call, put }) {
            yield put({
                type: 'setNoActionSuccess',
                payload: payload
            });
        },
        //获取按钮组
        *getModuleFunction({ payload }, { call, put }) {
            let response;
            if (payload) {
                response = yield call(GetModuleFunction, payload);
            }
            yield put({
                type: 'getModuleFunctionSuccess',
                payload: response ? response : ''
            });
        },
        //设置选择数据
        *setCheckFunction({ payload }, { call, put }) {
            yield put({
                type: 'setCheckFunctionSuccess',
                payload: payload
            });
        },
        //保存
        *saveRoleFunction({ payload }, { call, put }) {
            const response = yield call(SaveRoleFunction, payload);
            if (response.status == "ok") {
                return response.data;
            }
            else {
                message.error(response.message);
            }
        },

        //#endregion

        //#region 功能定义树
        *saveRoleFuncPriv({ payload }, { call, put }) {
            const response = yield call(SaveRoleFuncPriv, payload);
            if (response.status == "ok") {
                message.success(response.message);
            }
            else {
                message.error(response.message);
            }
        },
        *getRoleFuncPriv({ payload }, { call, put }) {
            const response = yield call(GetRoleFuncPriv, payload);
            if (response.status == "ok") {
                return response.data;
            }
            else {
                message.error(response.message);
            }
        },
        //#endregion
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
        //#region 基础功能权限
        getModuleFunctionSuccess(state, { payload }) {
            var functionData = [];
            var checkFunction = [];
            if (payload) {
                if (payload.functionData)
                    functionData = payload.functionData;
                if (payload.checkValue) {
                    checkFunction = payload.checkValue;
                }
            }
            let oldCheckFunction = [...checkFunction];
            oldCheckFunction.filter((item, index) => {
                if (noAction.includes(item)) {
                    checkFunction.splice(checkFunction.indexOf(item), 1);
                }
            })
            return { ...state, functionData: functionData, checkFunction };
        },
        setNoActionSuccess(state, { payload }) {
            if (payload) {
                if (payload.noAction && payload.noAction.length > 0) {
                    //如果删除数组不存在则添加
                    if (!noAction.includes(payload.noAction[0]))
                        noAction.push(payload.noAction[0]);
                    //判断是否在添加的数组中，有则删除
                    if (addAction.includes(payload.noAction[0]))
                        addAction.splice(addAction.indexOf(payload.noAction[0]), 1)
                }
                if (payload.addAction && payload.addAction.length > 0) {
                    //判断添加的值是否在删除数组中
                    if (noAction.includes(payload.addAction[0]))
                        noAction.splice(noAction.indexOf(payload.addAction[0]), 1);
                    else
                        addAction.push(payload.addAction[0]);
                }
            }
            return { ...state, noAction, addAction };
        },
        setCheckFunctionSuccess(state, { payload }) {
            let checkFunction = [];
            if (payload) {
                checkFunction = payload;
            }
            return { ...state, checkFunction };
        },
        //#endregion

        //#region 功能定义树
        //#endregion
    },
};
export default Model;
