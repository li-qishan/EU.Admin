export default
  [
    {
      path: '/',
      component: '../layouts/BlankLayout',
      routes: [
        {
          path: '/user',
          component: '../layouts/UserLayout',
          routes: [
            {
              path: '/user',
              redirect: '/user/login',
            },
            {
              name: 'login',
              icon: 'smile',
              path: '/user/login',
              component: './user/login',
            },
            {
              name: 'register-result',
              icon: 'smile',
              path: '/user/register-result',
              component: './user/register-result',
            },
            {
              name: 'register',
              icon: 'smile',
              path: '/user/register',
              component: './user/register',

            },
            {
              component: './attachment',
            }
          ],
        },
        {
          path: '/',
          component: '../layouts/BasicLayout',
          Routes: ['src/pages/Authorized'],
          authority: ['admin', 'user'],
          routes: [{
            path: '/',
            name: "首页",
            redirect: '/welcome', },
            {
              component: '404',
              name: '404',
              path: '/404',
          },
          {
            path: '/welcome',
            name: '首页',
            icon: 'smile',
            component: './dashboard/analysis',
          },
          {
            path: '/dashboard/analysis',
            name: '分析',
            icon: 'smile',
            component: './dashboard/analysis',
          },
          {
            path: '/dashboard/monitor',
            name: 'monitor',
            icon: 'smile',
            component: './dashboard/monitor',
          },
          {
            path: '/dashboard/workplace',
            name: 'workplace',
            icon: 'smile',
            component: './dashboard/workplace',
          },
          {
            path: "/ap/check",
            name: "应付对账单",
            icon: null,
            component: "./ap/check",
          }, {
            path: "/ap/initaccount",
            name: "应付期初建账",
            icon: null,
            component: "./ap/initaccount"
          }, {
            path: "/ap/invoice",
            name: "应付发票维护",
            icon: "",
            component: "./ap/invoice"
          }, {
            path: "/ap/payment",
            name: "应付付款单",
            icon: null,
            component: "./ap/payment"
          }, {
            path: "/ap/prepaid",
            name: "应付预付单",
            icon: null,
            component: "./ap/prepaid",
          }, {
            id: "5d637eeb-4e45-49e1-ba6a-51b18ff8fc74",
            path: "/ar/check",
            name: "应收对账单",
            icon: null,
            component: "./ar/check",
          }, {
            path: "/ar/initaccount",
            name: "应收期初建账",
            icon: null,
            component: "./ar/initaccount",
          }, {
            path: "/ar/invoice",
            name: "销售发票开立",
            icon: "",
            component: "./ar/invoice",
          }, {
            path: "/ar/prepaid",
            name: "销售预收款",
            icon: null,
            component: "./ar/prepaid",
          }, {
            path: "/ar/collection",
            name: "销售收款单",
            icon: null,
            component: "./ar/collection",
          }, {
            path: "/basedata/color",
            name: "颜色资料",
            icon: null,
            component: "./basedata/color",
          }, {
            path: "/basedata/currency",
            name: "币别设置",
            icon: "",
            component: "./basedata/currency",
          }, {
            id: "653cdd17-21d5-4d5a-b216-6e31cb6131e9",
            path: "/basedata/customer",
            name: "客户管理",
            icon: null,
            component: "./basedata/customer",

            moduleCode: "BD_CUSTOMER_MNG"
          }, {
            id: "4936502a-3166-4946-96d5-2f3b135b6c48",
            path: "/basedata/deliveryway",
            name: "送货方式",
            icon: null,
            component: "./basedata/deliveryway",

            moduleCode: "BD_DELIVERY_WAY_MNG"
          }, {
            id: "dfb9dca2-550f-4cfb-8ad7-a7edf5088f77",
            path: "/basedata/material",
            name: "物料管理",
            icon: null,
            component: "./basedata/material",

            moduleCode: "BD_MATERIAL_MNG"
          }, {
            id: "504d9703-2b8d-4c76-aba2-8663fb20a758",
            path: "/basedata/materialtype",
            name: "物料类型",
            icon: null,
            component: "./basedata/materialtype",

            moduleCode: "BD_MATERIAL_TYPE_MNG"
          }, {
            id: "c61b9cd0-491d-4a3d-9508-0ab2ae325bc5",
            path: "/basedata/unit",
            name: "计量单位",
            icon: null,
            component: "./basedata/unit",

            moduleCode: "BD_MEASUREMENT_UNIT_MNG"
          }, {
            id: "d5f53c9e-88f5-4915-b551-2b2675354b8e",
            path: "/basedata/settlementway",
            name: "结算方式",
            icon: null,
            component: "./basedata/settlementway",

            moduleCode: "BD_SETTLEMENTWAY_MNG"
          }, {
            id: "df18f51a-4c46-47bc-bf85-2c33d8f35324",
            path: "/basedata/stock",
            name: "仓库管理",
            icon: null,
            component: "./basedata/stock",

            moduleCode: "BD_STOCK_MNG"
          }, {
            id: "ce4d0dec-4720-4cc4-ac61-4d41806e934e",
            path: "/basedata/supplier",
            name: "供应商管理",
            icon: null,
            component: "./basedata/supplier",

            moduleCode: "BD_SUPPLIER_MNG"
          }, {
            id: "2cadd09b-f21e-4aea-b475-db2f116c3867",
            path: "/basedata/texture",
            name: "材质资料",
            icon: null,
            component: "./basedata/texture",

            moduleCode: "BD_TEXTURE_MNG"
          }, {
            id: "ac9e59fe-6240-4428-8043-cf58e5a39637",
            path: "/machine/machine",
            name: "设备基础资料",
            icon: null,
            component: "./machine/machine",

            moduleCode: "EM_MACHIN_INFO_MNG"
          }, {
            id: "d29f481d-379f-4e3f-861e-1ec98f029b45",
            path: "/machine/type",
            name: "设备分类",
            icon: null,
            component: "./machine/type",

            moduleCode: "EM_MACHIN_TYPE_MNG"
          }, {
            id: "214ce3f9-3b23-4203-8d12-2feb462866ac",
            path: "/stock/ivavailableanalysisreport",
            name: "可用库存分析",
            icon: null,
            component: "./stock/ivavailableanalysisreport",

            moduleCode: "IV_AVAILABLE_INVENTORY_ANALYSIS_REPORT_MNG"
          }, {
            id: "5edc0c1a-c00c-4760-8dc1-9282363c7f42",
            path: "/stock/ivsafeinventorywarningreport",
            name: "安全库存预警",
            icon: null,
            component: "./stock/ivsafeinventorywarningreport",

            moduleCode: "IV_SAFE_INVENTORY_WARNING_REPORT_MNG"
          }, {
            id: "7abd7267-06b6-4ee9-a736-7375082b9a21",
            path: "/stock/actualcheck",
            name: "实际盘点录入",
            icon: null,
            component: "./stock/actualcheck",

            moduleCode: "IV_STOCK_ACTUAL_CHECK_MNG"
          }, {
            id: "32bebe7f-547b-467d-a1c0-f02a82b41a0e",
            path: "/stock/adjust",
            name: "库存调整单",
            icon: null,
            component: "./stock/adjust",

            moduleCode: "IV_STOCK_ADJUST_MNG"
          }, {
            id: "d72c8129-e6d5-4c43-98d6-71dda0c13356",
            path: "/stock/check",
            name: "存货盘点建立",
            icon: null,
            component: "./stock/check",

            moduleCode: "IV_STOCK_CHECK_MNG"
          }, {
            id: "85b9392e-159f-4fd1-a156-b585a100a68e",
            path: "/stock/init",
            name: "库存初始化",
            icon: null,
            component: "./stock/init",

            moduleCode: "IV_STOCK_INIT_MNG"
          }, {
            id: "04e89967-cb72-4c38-8557-b3f99dacbda4",
            path: "/stock/otherin",
            name: "其他入库单",
            icon: null,
            component: "./stock/otherin",

            moduleCode: "IV_STOCK_OTHER_IN_MNG"
          }, {
            id: "2b692838-60cc-4cc0-a410-d97a03498d9d",
            path: "/stock/otherout",
            name: "其他出库单",
            icon: null,
            component: "./stock/otherout",

            moduleCode: "IV_STOCK_OTHER_OUT_MNG"
          }, {
            id: "9e798243-04c0-44c3-ba8f-e13feb7551f8",
            path: "/stock/change",
            name: "库存交易明细",
            icon: null,
            component: "./stock/change",

            moduleCode: "IV_STOCK_QTY_CHANGE_MNG"
          }, {
            id: "07e9dc79-2cda-4f61-b521-459de7c16dcf",
            path: "/stock/query",
            name: "库存查询",
            icon: null,
            component: "./stock/query",

            moduleCode: "IV_STOCK_QUERY_MNG"
          }, {
            id: "b1f357e1-2937-4a75-a2af-335624655132",
            path: "/stock/transfers",
            name: "库存调拨单",
            icon: null,
            component: "./stock/transfers",

            moduleCode: "IV_STOCK_TRANSFERS_MNG"
          }, {
            id: "b2d02ba1-22f8-41f1-b56a-b86bcd4fbb7f",
            path: "/mf/in",
            name: "工模治具入账",
            icon: null,
            component: "./mf/in",

            moduleCode: "MF_IN_ORDER_MNG"
          }, {
            id: "c54e0cf2-e485-492a-9f3a-19c1bb6b594b",
            path: "/mf/mould",
            name: "工模治具",
            icon: null,
            component: "./mf/mould",

            moduleCode: "MF_MOULD_MNG"
          }, {
            id: "f611271f-f526-4fd5-b64c-36f9a2e7022f",
            path: "/mf/type",
            name: "工模治具类别",
            icon: null,
            component: "./mf/type",

            moduleCode: "MF_MOULD_TYPE_MNG"
          }, {
            id: "1bfc06cf-ebb3-42c2-a2a3-b854fc25127c",
            path: "/pd/close",
            name: "工单批量结清",
            icon: null,
            component: "./pd/close",

            moduleCode: "PD_CLOSE_ORDER_MNG"
          }, {
            id: "96384574-a005-45e8-8a9c-d09b8e9ff163",
            path: "/pd/complete",
            name: "产品完工入库",
            icon: null,
            component: "./pd/complete",

            moduleCode: "PD_COMPLETE_ORDER_MNG"
          }, {
            id: "ade2723c-3fb8-471b-a751-75caf2631f1e",
            path: "/pd/order",
            name: "生产工单",
            icon: null,
            component: "./pd/order",

            moduleCode: "PD_ORDER_MNG"
          }, {
            id: "d1e0bb6e-e57e-41b2-b092-006e0d438106",
            path: "/pd/out",
            name: "材料出库工单",
            icon: null,
            component: "./pd/out",

            moduleCode: "PD_OUT_ORDER_MNG"
          }, {
            id: "15b44038-d70e-45c6-af8d-db96f194cb59",
            path: "/pd/plan",
            name: "生产计划工单",
            icon: "smile",
            component: "./pd/plan",

            moduleCode: "PD_PLAN_ORDER_MNG"
          }, {
            id: "3faea5d1-6a4f-4491-b7e1-9af897fa8334",
            path: "/pd/reissue",
            name: "材料补发工单",
            icon: null,
            component: "./pd/reissue",

            moduleCode: "PD_REISSUE_ORDER_MNG"
          }, {
            id: "f1ac5c7b-c9a0-4c89-a24b-454581b7616b",
            path: "/pd/require",
            name: "需求分析工单",
            icon: null,
            component: "./pd/require",

            moduleCode: "PD_REQUIRE_ORDER_MNG"
          }, {
            id: "d680e8e1-d68b-4275-8916-e178448e184f",
            path: "/pd/return",
            name: "材料退库作业",
            icon: null,
            component: "./pd/return",

            moduleCode: "PD_RETURN_ORDER_MNG"
          }, {
            id: "533bcf3c-a736-4c84-93f7-d21d2e5e8afb",
            path: "/purchase/arrival",
            name: "采购到货通知",
            icon: null,
            component: "./purchase/arrival",

            moduleCode: "PO_ARRIVAL_ORDER_MNG"
          }, {
            id: "3f0c24c5-9eec-457b-a34b-97f4e88aa6ae",
            path: "/purchase/deduction",
            name: "采购扣款单",
            icon: null,
            component: "./purchase/deduction",

            moduleCode: "PO_DEDUCTION_ORDER_MNG"
          }, {
            id: "39617a55-86ba-4675-8a3a-ef92f1a7ecee",
            path: "/purchase/fee",
            name: "采购费用单",
            icon: null,
            component: "./purchase/fee",

            moduleCode: "PO_FEE_ORDER_MNG"
          }, {
            id: "12cb7331-8857-4a75-99ae-fff4328ea640",
            path: "/purchase/in",
            name: "采购入库单",
            icon: null,
            component: "./purchase/in",

            moduleCode: "PO_IN_ORDER_MNG"
          }, {
            id: "21df2d53-9b4f-4034-9f6e-303336e4b441",
            path: "/purchase/poorderanalysisreport",
            name: "采购单分析",
            icon: null,
            component: "./purchase/poorderanalysisreport",

            moduleCode: "PO_ORDER_ANALYSIS_REPORT_MNG"
          }, {
            id: "d001e121-b967-421d-b9a8-bde2e1e2b780",
            path: "/purchase/order",
            name: "采购单建立",
            icon: null,
            component: "./purchase/order",

            moduleCode: "PO_ORDER_MNG"
          }, {
            id: "51d48c39-133f-4b96-8226-054089a70a21",
            path: "/purchase/porequestiondetailreport",
            name: "请购单明细表",
            icon: null,
            component: "./purchase/porequestiondetailreport",

            moduleCode: "PO_REQUESTION_DETAIL_REPORT_MNG"
          }, {
            id: "174d10d9-16a7-4690-8c41-7ca3d66c8913",
            path: "/purchase/requestion",
            name: "请购单作业",
            icon: null,
            component: "./purchase/requestion",

            moduleCode: "PO_REQUESTION_MNG"
          }, {
            id: "37d7e75e-d6af-443a-9c59-b088adec09f8",
            path: "/purchase/return",
            name: "采购退货单",
            icon: null,
            component: "./purchase/return",

            moduleCode: "PO_RETURN_ORDER_MNG"
          }, {
            id: "3dd1c561-f739-4dde-9784-ff91c77fc735",
            path: "/purchase/pounarrivalanalysisreport",
            name: "采购未到货分析",
            icon: null,
            component: "./purchase/pounarrivalanalysisreport",

            moduleCode: "PO_UN_ARRIVAL_ANALYSIS_REPORT_MNG"
          }, {
            id: "d13131c3-e755-47ea-9a07-1ddcb7a06b79",
            path: "/ps/reason",
            name: "工序不良原因",
            icon: null,
            component: "./ps/reason",

            moduleCode: "PS_PROCESS_BAD_REASON_MNG"
          }, {
            id: "93ef2137-ecfd-4afb-aa88-a47459f0a3ee",
            path: "/ps/bom",
            name: "工程BOM建立",
            icon: null,
            component: "./ps/bom",

            moduleCode: "PS_PROCESS_BOM_MNG"
          }, {
            id: "4695b483-c7ab-4571-8e6e-89fd18495487",
            path: "/ps/process",
            name: "工序建立",
            icon: null,
            component: "./ps/process",

            moduleCode: "PS_PROCESS_MNG"
          }, {
            id: "58600ffd-d278-47b2-bfd5-51ab56316de3",
            path: "/ps/template",
            name: "工序模板",
            icon: null,
            component: "./ps/template",

            moduleCode: "PS_PROCESS_TEMPLATE_MNG"
          }, {
            id: "8d633ba6-f895-49bb-ba6e-719fdfec109e",
            path: "/ps/workshop",
            name: "制造车间建立",
            icon: null,
            component: "./ps/workshop",

            moduleCode: "PS_WORK_SHOP_MNG"
          }, {
            id: "6af660f5-ee50-429f-9bd0-f5ba101f79e3",
            path: "/sales/outorder",
            name: "销售出库单",
            icon: null,
            component: "./sales/outorder",

            moduleCode: "SD_OUT_ORDER_MNG"
          }, {
            id: "1f52af4e-bba7-47ee-80cb-99968ae2f60f",
            path: "/sales/returnorder",
            name: "销售退货单",
            icon: null,
            component: "./sales/returnorder",

            moduleCode: "SD_RETURN_ORDER_MNG"
          }, {
            id: "c319a9a2-8e5f-4284-b493-f49af11973c3",
            path: "/sales/salesunoutanalysisreport",
            name: "订单未出货分析",
            icon: null,
            component: "./sales/salesunoutanalysisreport",

            moduleCode: "SD_SALES_NO_OUT_ANALYSIS_REPORT_MNG"
          }, {
            id: "3eb4c637-34a4-459b-ba61-cb1373125b6a",
            path: "/sales/salesorder",
            name: "销售订单建立",
            icon: null,
            component: "./sales/salesorder",

            moduleCode: "SD_SALES_ORDER_MNG"
          }, {
            id: "a403f70a-7b7d-4fc9-b5b3-31b8fe93dde2",
            path: "/sales/salesreport",
            name: "销售订单分析",
            icon: null,
            component: "./sales/salesreport",

            moduleCode: "SD_SALES_ORDER_REPORT_MNG"
          }, {
            id: "54c62a45-1004-44d5-84ff-bf1d14340067",
            path: "/sales/shiporder",
            name: "计划发货作业",
            icon: null,
            component: "./sales/shiporder",

            moduleCode: "SD_SHIP_ORDER_MNG"
          }, {
            id: "793541da-a542-4532-a9be-65fae0be46c9",
            path: "/system/setup/autocode",
            name: "自动编号",
            icon: null,
            component: "./system/setup/autocode",

            moduleCode: "SM_AUTOCODE_MNG"
          }, {
            id: "18b69f6e-1bde-42c5-849d-90305c862649",
            path: "/system/city",
            name: "城市维护",
            icon: "smile",
            component: "./system/city",

            moduleCode: "SM_CITY_MNG"
          }, {
            id: "6575292a-9ca4-4c71-8051-39c0ba98cc48",
            path: "/system/clear",
            name: "清空缓存",
            icon: null,
            component: "./system/clear",

            moduleCode: "SM_CLEAR_MNG"
          }, {
            id: "fa9b87fd-78be-4aa2-b7c4-70709443f8e3",
            path: "/system/companystructure/company",
            name: "组织管理",
            icon: "smile",
            component: "./system/companystructure/company",

            moduleCode: "SM_COMPANY_MNG"
          }, {
            id: "a9481c84-c351-4af1-a71f-e29d51ad3258",
            path: "/system/setup/paramconfig",
            name: "系统参数配置",
            icon: "user",
            component: "./system/setup/paramconfig",

            moduleCode: "SM_CONFIG_MNG"
          }, {
            id: "ed799b89-0838-474c-95ad-5d0645a59fb5",
            path: "/system/companystructure/department",
            name: "部门管理",
            icon: "smile",
            component: "./system/companystructure/department",

            moduleCode: "SM_DEPARTMENT_MNG"
          }, {
            id: "b0857be2-b1c2-46e7-9c4e-f855cbfd0f81",
            path: "/system/companystructure/employee",
            name: "员工管理",
            icon: "smile",
            component: "./system/companystructure/employee",

            moduleCode: "SM_EMPLOYEE_MNG"
          }, {
            id: "543f7a18-9ca6-4a70-b51a-5294f7c71217",
            path: "/system/privilege/functionpriv",
            name: "功能权限定义",
            icon: "smile",
            component: "./system/privilege/functionpriv",

            moduleCode: "SM_FUNCTION_PRIVILEGE_MNG"
          }, {
            id: "810689ce-dbbd-4705-8111-3a4f9dda07d6",
            path: "/import/config",
            name: "导入模板定义",
            icon: null,
            component: "./import/config",

            moduleCode: "SM_IMPORT_TEMPLATE_MNG"
          }, {
            id: "f05080b6-c4d2-46f9-93e6-3a378f002495",
            path: "/system/setup/setparam",
            name: "参数管理",
            icon: null,
            component: "./system/setup/setparam",

            moduleCode: "SM_LOV_MNG"
          }, {
            id: "891955ee-067a-4110-85df-21f6f72d0ba0",
            path: "/system/module",
            name: "系统模块",
            icon: "appstoreadd",
            component: "./system/module",

            moduleCode: "SM_MODULE_MNG"
          }, {
            id: "ff5eb00c-1532-47d7-875a-99896b3281a9",
            path: "/system/province",
            name: "省份维护",
            icon: "smile",
            component: "./system/province",

            moduleCode: "SM_PROVINCE_MNG"
          }, {
            id: "259e91c0-e381-41c0-9ed7-c5ec8bfac81f",
            path: "/system/privilege/role",
            name: "系统功能角色",
            icon: "userswitch",
            component: "./system/privilege/role",

            moduleCode: "SM_ROLE_MNG"
          }, {
            id: "07fb2ba5-d3ea-42bc-bba5-afd4fdc555b3",
            path: "/system/table",
            name: "系统表字典",
            icon: null,
            component: "./system/table",

            moduleCode: "SM_TABLE_CATALOG_MNG"
          }, {
            id: "17d33b00-d146-4904-877e-728e5eaa132d",
            path: "/system/privilege/user",
            name: "用户管理",
            icon: "user",
            component: "./system/privilege/user",
          }, {
            path: "/system/setup/workflow",
            name: "流程设计",
            icon: null,
            component: "./system/setup/workflow",
          }, {
            path: "/wx/config",
            name: "账号配置",
            icon: null,
            component: "./wx/config",
          }, {
            path: "/account/settings",
            name: "个人设置",
            icon: "",
            component: "./account/settings",
          }, {
            path: "/system/job",
            name: "任务管理",
            icon: "",
            component: "./system/monitor/job"
          }],
        },
      ],
    },
  ];
