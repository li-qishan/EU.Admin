import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';

let moduleCode = "SM_ROLE_MNG";
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
    const { dispatch, smrole: { moduleInfo } } = this.props;
    if (moduleInfo == null)
      dispatch({
        type: 'smrole/getModuleInfo',
        payload: { moduleCode },
      })
  }
  render() {
    const { dispatch, smrole: { moduleInfo, tableParam } } = this.props;

    const columns = [
      {
        title: '创建时间',
        dataIndex: 'CreatedTime',
        valueType: 'dateTime',
        hideInSearch: true,
        sorter: true
      },
      {
        title: '角色代码',
        dataIndex: 'RoleCode'
      },
      {
        title: '角色名称',
        dataIndex: 'RoleName'
      },
      {
        title: '是否启用',
        dataIndex: 'IsActive',
        hideInSearch: true,
        filters: false,
        valueEnum: {
          true: {
            text: '是',
          },
          false: {
            text: '否',
          },
        },
      },
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
              type: 'smrole/setTableStatus',
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
              type: 'smrole/setTableStatus',
              payload: { params, sorter },
            })
            return query({
              paramData: JSON.stringify(params),
              sorter: JSON.stringify(sorter),
              filter: JSON.stringify(filter),
              moduleCode
            })
          }}
        />
      </div>
    )
  }
}
export default connect(({ smrole, loading }) => ({
  smrole,
  // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);
