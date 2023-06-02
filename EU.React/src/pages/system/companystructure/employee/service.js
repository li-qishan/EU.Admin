import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/SmEmployee/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/SmEmployee/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/SmEmployee/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/SmEmployee/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/SmEmployee/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/SmEmployee/BatchDelete', {
        method: 'POST',
        data: params
    });
}