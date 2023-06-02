import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvTransfersDetail/Add', {
        method: 'POST',
        data: params
    });
}
export async function BatchSaveAddFormData(params) {
    return request('/api/IvTransfersDetail/BatchAdd', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvTransfersDetail/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvTransfersDetail/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvTransfersDetail/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvTransfersDetail/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvTransfersDetail/BatchDelete', {
        method: 'POST',
        data: params
    });
}