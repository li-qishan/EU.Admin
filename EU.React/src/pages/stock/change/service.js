import request from '@/utils/request';

export async function query(params) {
    return request('/api/IvAdjust/GetGridList', {
        params,
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