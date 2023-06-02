import request from '@/utils/request';
let url = '/api/SmQuartzJob/';
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
export async function Query(params) {
  return request(url + 'GetGridList', {
    params,
  });
}
export async function Get(id) {
  return request(url + 'Get/' + id);
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
