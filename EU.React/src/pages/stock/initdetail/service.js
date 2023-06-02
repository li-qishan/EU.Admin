import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvInitDetail/Add', {
        method: 'POST',
        data: params
    });
}
export async function BatchSaveAddFormData(params) {
    return request('/api/IvInitDetail/BatchAdd', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvInitDetail/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvInitDetail/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvInitDetail/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvInitDetail/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvInitDetail/BatchDelete', {
        method: 'POST',
        data: params
    });
}