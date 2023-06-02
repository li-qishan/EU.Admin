import request from '@/utils/request';

export async function query() {
  return request('/api/users');
}
export async function queryCurrent() {
  return request('/api/Account/CurrentUser');
}
export async function queryNotices() {
  return request('/api/notices');
}
export async function getMenuData() {
  return request('/api/SmModule/GetMenuData');
}
export async function getUserDepartment() {
  return request('/api/Account/GetUserDepartment');
}
