import request from '@/utils/request';

let url = '/api/ApPaymentWriteOff/';
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

export async function GetSourceList(params) {
    return request(url + 'GetSourceList', {
        params,
    });
}
export async function BatchSaveData(params) {
    return request(url + 'BatchAdd', {
        method: 'POST',
        data: params
    });
}