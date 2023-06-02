import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvOtherIn/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvOtherIn/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvOtherIn/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvOtherIn/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvOtherIn/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvOtherIn/BatchDelete', {
        method: 'POST',
        data: params
    });
}
export async function AuditOrder(params) {
    return request('/api/IvOtherIn/AuditOrder', {
        method: 'POST',
        data: params
    });
}