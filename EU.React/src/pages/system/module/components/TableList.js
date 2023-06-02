import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete, ExportModuleSqlScript } from '../service';
import Index from '../index';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';
import ComboGrid from '@/components/SysComponents/ComboGrid'
import SqlEdit from './SqlEdit'
import Utility from '@/utils/utility';
import { message } from 'antd';
let moduleCode = "SM_MODULE_MNG";
let me;
class TableList extends Component {
  formRef = React.createRef();

  constructor(props) {
    super(props);
    me = this;
    me.state = {
      begin: true
    };
  }
  componentWillMount() {
    const { dispatch, module: { moduleInfo } } = this.props;
    if (moduleInfo == null)
      dispatch({
        type: 'module/getModuleInfo',
        payload: { moduleCode }
      })
  }
  async exportModuleSqlScript(action, selectedRows) {
    if (selectedRows.length > 0) {
      let response = await ExportModuleSqlScript(selectedRows);
      if (response.status == "ok") {
        // webUrl + response.data.filePath
        Utility.downloadFileById(response.data.fileId, 'qazwsx')
      } else {
        message.error(response.message);
      }
      action.clearSelected();
    }
  }
  render() {
    const { dispatch, module: { moduleInfo, tableParam } } = this.props;

    if (moduleInfo == null)
      return null;
    moduleInfo.columns && moduleInfo.columns.map((item, index) => {
      if (item.dataIndex == 'ParentId') {
        moduleInfo.columns[index].renderFormItem = () => (
          // <ComboGrid api="/api/SmModule/GetPageList" itemkey="ID" itemvalue="ModuleName" />
          <ComboGrid
            api="/api/SmModule/GetPageList"
            itemkey="ID"
            param={{
              IsParent: true,
              IsActive: true
            }}
            itemvalue="ModuleName"
          />
        )
      }
    })

    //#region 操作栏按钮方法

    const SysModuleCollocate = (Id) => {
      Index.changePage(<SqlEdit Id={Id} />)
    }
    const ExportModuleSqlScript = (action, selectedRows) => {
      me.exportModuleSqlScript(action, selectedRows);
    }

    //#region 操作栏按钮方法
    const action = {
      SysModuleCollocate, ExportModuleSqlScript
    }
    //#endregion
    //#endregion

    return (
      <div>
        <SmProTable
          columns={moduleInfo.columns}
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
              type: 'module/setTableStatus',
              payload: {},
            })
          }}
          onLoad={() => {
            if (tableParam && tableParam.params && this.formRef.current) {
              this.formRef.current.setFieldsValue({ ...tableParam.params })
            }
            this.setState({ begin: false })
          }}
          pagination={tableParam && tableParam.params ? { current: tableParam.params.current } : {}}
          request={(params, sorter, filter) => {
            if (tableParam && tableParam.params && !params._timestamp)
              params = Object.assign(tableParam.params, params);
            if (tableParam && tableParam.sorter)
              sorter = Object.assign(tableParam.sorter, sorter);
            dispatch({
              type: 'module/setTableStatus',
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
export default connect(({ module }) => ({
  module,
  // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);
