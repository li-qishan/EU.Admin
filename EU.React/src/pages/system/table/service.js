import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/SmTableCatalog/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/SmTableCatalog/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/SmTableCatalog/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/SmTableCatalog/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/SmTableCatalog/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/SmTableCatalog/BatchDelete', {
        method: 'POST',
        data: params
    });
}
export async function InitAssignmentTable(params) {
    return request('/api/SmTableCatalog/InitAssignmentTable', {
        method: 'POST',
        data: params
    });
}
export async function InitAllTable(params) {
    return request('/api/SmTableCatalog/InitAllTable', {
        method: 'POST',
        data: params
    });
}