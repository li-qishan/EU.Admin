import request from '@/utils/request';

export async function SaveAddFormData(params) {
    return request('/api/SmLov/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/SmLov/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/SmLov/GetPageList', {
        params,
    });
}
export async function queryDetail(params) {
    if (!params.parentId)
        return;
    return request('/api/SmLovDetail/GetPageList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/SmLov/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/SmLov/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/SmLov/BatchDelete', {
        method: 'POST',
        data: params
    });
}
export async function SaveAddDetailFormData(params) {
    return request('/api/SmLovDetail/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditDetailFormData(params) {
    return request('/api/SmLovDetail/Update', {
        method: 'POST',
        data: params
    });
}
export async function GetDetailById(params) {
    return request('/api/SmLovDetail/GetById', {
        params
    });
}
export async function DeleteDetail(params) {
    return request('/api/SmLovDetail/Delete', {
        params
    });
} 
export async function BatchDetailDelete(params) {
    return request('/api/SmLovDetail/BatchDelete', {
        method: 'POST',
        data: params
    });
}