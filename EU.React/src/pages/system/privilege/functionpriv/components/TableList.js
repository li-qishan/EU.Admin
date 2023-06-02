import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';

let moduleCode = "SM_FUNCTION_PRIVILEGE_MNG";
let me;
class TableList extends Component {
  formRef = React.createRef();
  constructor(props) {
    super(props);
    me = this;
    me.state = {

    };
  }
  componentWillMount() {
    const { dispatch, functionpriv: { moduleInfo } } = this.props;
    if (moduleInfo == null)
      dispatch({
        type: 'functionpriv/getModuleInfo',
        payload: { moduleCode },
      })
  }
  render() {
    const { dispatch, functionpriv: { moduleInfo, tableParam } } = this.props;

    const columns = [
      {
        title: '排序号',
        hideInSearch: true,
        dataIndex: 'TaxisNo',
      },
      {
        title: '功能代码',
        dataIndex: 'FunctionCode'
      },
      {
        title: '功能名称',
        dataIndex: 'FunctionName'
      },
      {
        title: '显示位置',
        hideInSearch: true,
        dataIndex: 'DisplayPosition',
        filters: false,
        valueEnum: {
          0: {
            text: '操作',
          },
          1: {
            text: '菜单',
          },
          2: {
            text: '隐藏菜单',
          }
        },
      },
      {
        title: '关联模块',
        dataIndex: ['SmModule', 'ModuleName'],
        hideInSearch: true,
        width: 180
        // },
        // {
        //     title: '关联模块',
        //     dataIndex: 'SmModuleId',
        //     hideInTable: true,
        //     renderFormItem: () => (
        //         <ComboGrid api="/api/SmModule/GetPageList" itemkey="ID" itemvalue="ModuleName" />
        //     )
      }
    ];

    //#region 操作栏按钮方法
    const action = {}
    //#endregion

    return (
      <div>
        <SmProTable
          columns={columns}
          delete={Delete}
          batchDelete={(selectedRows) => BatchDelete(selectedRows)}
          moduleInfo={moduleInfo}
          {...action}
          changePage={Index.changePage}
          formPage={FormPage}
          formRef={this.formRef}
          form={{ labelCol: { span: 6 } }}
          onReset={() => {
            dispatch({
              type: 'functionpriv/setTableStatus',
              payload: {},
            })
          }}
          onLoad={() => {
            if (tableParam && tableParam.params && this.formRef.current) {
              this.formRef.current.setFieldsValue({ ...tableParam.params })
            }
          }}
          pagination={tableParam && tableParam.params ? { current: tableParam.params.current } : {}}
          request={(params, sorter, filter) => {
            if (tableParam && tableParam.params && !params._timestamp) {
              params = tableParam.params;
              //sorter = tableParam.sorter
            }
            dispatch({
              type: 'functionpriv/setTableStatus',
              payload: { params, sorter },
            })
            return query({
              paramData: JSON.stringify(params),
              sorter: JSON.stringify(sorter),
              filter: JSON.stringify(filter)
            })
          }}
        />
      </div>
    )
  }
}
export default connect(({ functionpriv, loading }) => ({
  functionpriv,
  // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);
