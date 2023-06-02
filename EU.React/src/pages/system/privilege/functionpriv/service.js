import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/SmFunctionPrivilege/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/SmFunctionPrivilege/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/SmFunctionPrivilege/GetPageList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/SmFunctionPrivilege/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/SmFunctionPrivilege/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/SmFunctionPrivilege/BatchDelete', {
        method: 'POST',
        data: params
    });
}