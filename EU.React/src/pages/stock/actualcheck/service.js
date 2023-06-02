import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvActualCheck/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvActualCheck/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvActualCheck/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvActualCheck/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvActualCheck/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvActualCheck/BatchDelete', {
        method: 'POST',
        data: params
    });
}
export async function AuditOrder(params) {
    return request('/api/IvActualCheck/AuditOrder', {
        method: 'POST',
        data: params
    });
}
