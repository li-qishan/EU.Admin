// import { history } from 'umi'
// import React from 'react'
// import request from '@/utils/request'

// let extraRoutes;

// export function patchRoutes({ routes }) {
//     routes[0].routes[1].routes = extraRoutes;
// }
// export async function render(oldRender) {
//     var token = localStorage.getItem('antd-pro-authority');
//     if (token) {
//         let result = await request('/api/SmModule/GetPatchRoutes');
//         if (result) {
//             if (result.data && result.data.length > 0) {
//                 for (var i = 0; i < result.data.length; i++)
//                     if (result.data[i].path) {
//                         result.data[i].exact = true;
//                         try {
//                             // require('./pages/1/1').default;jiu
//                             require('./pages' + result.data[i].path).default;
//                             result.data[i].component = require('./pages' + result.data[i].path).default;
//                         } catch (error) { }
//                     }

//                 extraRoutes = result.data;
//             }
//         }
//     }
//     oldRender();
// }
