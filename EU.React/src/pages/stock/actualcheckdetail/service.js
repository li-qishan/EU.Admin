import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/IvActualCheckDetail/Add', {
        method: 'POST',
        data: params
    });
}
export async function BatchSaveAddFormData(params) {
    return request('/api/IvActualCheckDetail/BatchAdd', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/IvActualCheckDetail/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/IvActualCheckDetail/GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/IvActualCheckDetail/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/IvActualCheckDetail/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/IvActualCheckDetail/BatchDelete', {
        method: 'POST',
        data: params
    });
}
export async function queryCheckList(params) {
    return request('/api/IvActualCheck/GetCheckList', {
        params,
    });
}