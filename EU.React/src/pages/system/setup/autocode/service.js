import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/SmAutoCode/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/SmAutoCode/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/SmAutoCode/GetPageList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/SmAutoCode/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/SmAutoCode/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/SmAutoCode/BatchDelete', {
        method: 'POST',
        data: params
    });
}