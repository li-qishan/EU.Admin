import request from '@/utils/request';

let url = '/api/File/';
export async function query(params) {
    return request(url + 'GetPageList', {
        params,
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