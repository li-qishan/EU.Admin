import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvOtherInDetail/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvOtherInDetail/Update', {
        method: 'POST',
        data: params
    });
}
export async function BatchSaveAddFormData(params) {
    return request('/api/IvOtherInDetail/BatchAdd', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvOtherInDetail/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvOtherInDetail/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvOtherInDetail/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvOtherInDetail/BatchDelete', {
        method: 'POST',
        data: params
    });
}