import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvAdjustDetail/Add', {
        method: 'POST',
        data: params
    });
}
export async function BatchSaveAddFormData(params) {
    return request('/api/IvAdjustDetail/BatchAdd', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvAdjustDetail/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvAdjustDetail/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvAdjustDetail/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvAdjustDetail/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvAdjustDetail/BatchDelete', {
        method: 'POST',
        data: params
    });
}