import request from '@/utils/request';

let url = '/api/ArInitAccountOrder/'
export async function SaveAddFormData(params) {
    return request(url + 'Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request(url + 'Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request(url + 'GetGridList', {
        params,
    });
}
export async function GetById(params) {
    return request(url + 'GetById', {
        params
    });
}
export async function Delete(params) {
    return request(url + 'Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request(url + 'BatchDelete', {
        method: 'POST',
        data: params
    });
}
export async function AuditOrder(params) {
    return request(url + 'AuditOrder', {
        method: 'POST',
        data: params
    });
}
export async function querySourceList(params) {
    return request(url + 'GetSourceList', {
        params,
    });
}