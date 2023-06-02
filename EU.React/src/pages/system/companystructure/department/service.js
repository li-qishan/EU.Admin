import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/SmDepartment/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/SmDepartment/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/SmDepartment/GetPageList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/SmDepartment/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/SmDepartment/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/SmDepartment/BatchDelete', {
        method: 'POST',
        data: params
    });
}