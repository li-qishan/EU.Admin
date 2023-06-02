import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/SmCompany/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/SmCompany/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/SmCompany/GetPageList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/SmCompany/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/SmCompany/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/SmCompany/BatchDelete', {
        method: 'POST',
        data: params
    });
}