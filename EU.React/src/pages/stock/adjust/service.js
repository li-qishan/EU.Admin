import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvAdjust/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvAdjust/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvAdjust/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvAdjust/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvAdjust/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvAdjust/BatchDelete', {
        method: 'POST',
        data: params
    });
}
export async function AuditOrder(params) {
    return request('/api/IvAdjust/AuditOrder', {
        method: 'POST',
        data: params
    });
}