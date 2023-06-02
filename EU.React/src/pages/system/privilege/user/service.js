import request from '@/utils/request';
export async function SaveAddFormData(params) {
    return request('/api/Account/Add', {
        method: 'POST',
        data: params
    });
}
export async function SaveEditFormData(params) {
    return request('/api/Account/Update', {
        method: 'POST',
        data: params
    });
}
export async function query(params) {
    return request('/api/Account/GetPageList', {
        params,
    });
}
export async function GetById(params) {
    return request('/api/Account/GetById', {
        params
    });
}
export async function Delete(params) {
    return request('/api/Account/Delete', {
        params
    });
}
export async function BatchDelete(params) {
    return request('/api/Account/BatchDelete', {
        method: 'POST',
        data: params
    });
}
export async function GetRoleList(params) {
    return request('/api/SmUserRole/GetRoleList', {
        params
    });
}
export async function BatchInsertUserRole(params) {
    return request('/api/SmUserRole/BatchInsertUserRole', {
        method: 'POST',
        data: params
    });
}
export async function GetUserRole(params) {
    return request('/api/SmUserRole/GetUserRole', {
        params
    });
}
export async function ResetPassword(params) {
  return request('/api/Account/ResetPassword', {
      method: 'POST',
      data: params
  });
}
