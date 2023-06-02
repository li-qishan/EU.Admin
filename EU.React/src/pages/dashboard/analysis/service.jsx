// import request from 'umi-request';

// export async function fakeChartData() {
//   return request('/api/fake_chart_data');
// }

import _mock from './_mock';

export function fakeChartData() {
  return _mock;
}

