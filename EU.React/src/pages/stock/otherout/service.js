import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvOtherOut/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvOtherOut/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvOtherOut/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvOtherOut/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvOtherOut/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvOtherOut/BatchDelete', {
        method: 'POST',
        data: params
    });
}
export async function AuditOrder(params) {
    return request('/api/IvOtherOut/AuditOrder', {
        method: 'POST',
        data: params
    });
}