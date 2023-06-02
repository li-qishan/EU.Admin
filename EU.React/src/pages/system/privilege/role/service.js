import request from '@/utils/request';

//#region 基础配置
export async function SaveAddFormData(params) {
    return request('/api/SmRole/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/SmRole/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/SmRole/GetPageList', {
        params,
    });
}
export async function Get(id) {
    return request('/api/SmRole/Get/'+id);
}
export async function Delete(params) {
    return request('/api/SmRole/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/SmRole/BatchDelete', {
        method: 'POST',
        data: params
    });
}
//#endregion

//#region 模块树
export async function GetModuleList(params) {
    return request('/api/SmRoleModule/GetModuleList', {
        params
    });
}
export async function GetAllModuleList(params) {
    return request('/api/SmRoleModule/GetAllModuleList', {
        params
    });
}
export async function BatchInsertRoleModule(params) {
    return request('/api/SmRoleModule/BatchInsertRoleModule', {
        method: 'POST',
        data: params
    });
}
export async function GetRoleModule(params) {
    return request('/api/SmRoleModule/GetRoleModule', {
        params
    });
}
//#endregion

//#region 基础功能权限
export async function GetModuleFunction(params) {
    return request('/api/SmRoleFunction/GetModuleFunction', {
        params
    });
}
export async function SaveRoleFunction(params) {
    return request('/api/SmRoleFunction/SaveRoleFunction', {
        method: 'POST',
        data: params
    });
}
export async function GetRoleFunction(params) {
    return request('/api/SmRoleFunction/GetRoleFunction', {
        params
    });
}
//#endregion

//#region 功能定义
export async function GetAllFuncPriv(params) {
    return request('/api/SmRoleFunction/GetAllFuncPriv', {
        params
    });
}
export async function SaveRoleFuncPriv(params) {
    return request('/api/SmRoleFunction/SaveRoleFuncPriv', {
        method: 'POST',
        data: params
    });
}
export async function GetRoleFuncPriv(params) {
    return request('/api/SmRoleFunction/GetRoleFuncPriv', {
        params
    });
}
//#endregion

