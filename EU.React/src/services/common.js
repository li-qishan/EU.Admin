import request from '@/utils/request';

export async function GetModuleLogInfo(params) {
  return request('/api/SmModule/GetModuleLogInfo', {
      params
  });
}
export async function ExportExcel(params) {
  return request('/api/Common/ExportExcel', {
      params
  });
}
export async function ClearCache(params) {
  return request('/api/Common/ClearCache', {
      params
  });
}
export async function GetModuleInfo(params) {
  return request('/api/SmModule/GetModuleInfo', {
      params,
  });
}