import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvCheck/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvCheck/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvCheck/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvCheck/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvCheck/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvCheck/BatchDelete', {
        method: 'POST',
        data: params
    });
}
export async function AuditOrder(params) {
    return request('/api/IvCheck/AuditOrder', {
        method: 'POST',
        data: params
    });
}