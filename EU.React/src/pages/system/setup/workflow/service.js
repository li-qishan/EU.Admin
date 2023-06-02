import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/SmProjectFlow/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/SmProjectFlow/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/SmProjectFlow/GetPageList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/SmProjectFlow/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/SmProjectFlow/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/SmProjectFlow/BatchDelete', {
        method: 'POST',
        data: params
    });
}
export async function GetStructureTree(params) {
    return request('/api/SmProjectFlow/GetStructureTree', {
        params
    });
}
//流程图
export async function SaveFlowData(params) {
    return request('/api/SmProjectFlow/SaveFlowData', {
        method: 'POST',
        data: params
    });
}
export async function GetFlowData(params) {
    return request('/api/SmProjectFlow/GetFlowData', {
        params
    });
}