import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/SmFieldCatalog/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/SmFieldCatalog/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/SmFieldCatalog/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/SmFieldCatalog/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/SmFieldCatalog/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/SmFieldCatalog/BatchDelete', {
        method: 'POST',
        data: params
    });
}