import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvOtherOutDetail/Add', {
        method: 'POST',
        data: params
    });
}
export async function BatchSaveAddFormData(params) {
    return request('/api/IvOtherOutDetail/BatchAdd', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvOtherOutDetail/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvOtherOutDetail/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvOtherOutDetail/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvOtherOutDetail/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvOtherOutDetail/BatchDelete', {
        method: 'POST',
        data: params
    });
}