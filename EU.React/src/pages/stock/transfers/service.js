import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvTransfers/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvTransfers/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvTransfers/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvTransfers/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvTransfers/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvTransfers/BatchDelete', {
        method: 'POST',
        data: params
    });
}
export async function AuditOrder(params) {
    return request('/api/IvTransfers/AuditOrder', {
        method: 'POST',
        data: params
    });
}