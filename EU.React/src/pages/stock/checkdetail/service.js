import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvCheckDetail/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvCheckDetail/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvCheckDetail/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvCheckDetail/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvCheckDetail/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvCheckDetail/BatchDelete', {
        method: 'POST',
        data: params
    });
}