import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvInit/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvInit/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvInit/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvInit/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvInit/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvInit/BatchDelete', {
        method: 'POST',
        data: params
    });
}
export async function AuditOrder(params) {
    return request('/api/IvInit/AuditOrder', {
        method: 'POST',
        data: params
    });
}