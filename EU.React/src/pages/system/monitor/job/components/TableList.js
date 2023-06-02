import React, { Component } from 'react';
import { connect } from 'umi';
import { Query, BatchDelete, Delete } from '../service';
import { Input, Card, Form, Row, Col, Modal, message } from 'antd';
import Index from '../index';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import request from "@/utils/request";

let moduleCode = "SM_JOB_MNG";
let me;
let modelName = 'smjob';
const FormItem = Form.Item;

class TableList extends Component {
  formRef = React.createRef();
  formRef1 = React.createRef();
  actionRef = React.createRef();
  constructor(props) {
    super(props);
    me = this;
    me.state = {
      isModalVisible: false,
      logContent: ''
    };
  }
  componentWillMount() {
    const { dispatch } = this.props;
    dispatch({
      type: modelName + '/getModuleInfo',
      payload: { moduleCode },
    })
  }
  async readJobCurrentLog(Id, type = "LOG.CURRENT") {
    message.loading('数据处理中...', 0);

    let result = await request(`/api/SmQuartzJob/Operate/${Id}/${type}`, {
      method: 'GET'
    });
    message.destroy();
    if (result.Success) {
      me.setState({ isModalVisible: true, logContent: result.Data });
      // me.actionRef.current.reload();
      // me.formRef1.current.resetFields();
      message.success(result.Message);
    }
    else
      message.error(result.Message);
  }
  render() {
    let { dispatch, smjob: { moduleInfo, tableParam } } = this.props;
    let { isModalVisible, logContent } = this.state;
    moduleInfo.modelName = modelName;

    //#region 操作栏按钮方法
    const ReadJobCurrentLog = (Id) => {
      me.readJobCurrentLog(Id)
    }
    const action = {
      ReadJobCurrentLog
    }
    //#endregion ‘===

    // const onChange = e => {
    //   me.setState({ version: e.target.value, version: '' })
    //   console.log(e.target.value);
    // };
    const htmlString = "<p>Hello, world!</p>";
    return (
      (<div>
        <SmProTable
          columns={moduleInfo.columns}
          delete={Delete}
          batchDelete={(selectedRows) => BatchDelete(selectedRows)}
          moduleInfo={moduleInfo}
          {...action}
          changePage={Index.changePage}
          formPage={FormPage}
          actionRef={this.actionRef}
          formRef={this.formRef}
          form={{ labelCol: { span: 6 } }}
          // onReset={() => {
          //     dispatch({
          //         type: 'smjob/setTableStatus',
          //         payload: {},
          //     })
          // }}
          onLoad={() => {
            if (tableParam && tableParam.params && this.formRef.current) {
              this.formRef.current.setFieldsValue({ ...tableParam.params })
            }
          }}
          pagination={tableParam && tableParam.params ? { current: tableParam.params.current } : {}}
          request={(params, sorter, filter) => {
            if (tableParam && tableParam.params && !params._timestamp)
              params = Object.assign(tableParam.params, params);
            if (tableParam && tableParam.sorter)
              sorter = Object.assign(tableParam.sorter, sorter);
            dispatch({
              type: modelName + '/setTableStatus',
              payload: { params, sorter },
            })
            return Query({
              paramData: JSON.stringify(params),
              sorter: JSON.stringify(sorter),
              filter: JSON.stringify(filter),
              moduleCode
            })
          }}
        />
        <Modal title="当前日志" open={isModalVisible}
          width={1200}
          destroyOnClose={true}
          onOk={() => me.setState({ logContent: '', isModalVisible: false })}
          onCancel={() => me.setState({ logContent: '', isModalVisible: false })}>
          <div dangerouslySetInnerHTML={{ __html: logContent }} />
        </Modal>
      </div>)
    );
  }
}
export default connect(({ smjob }) => ({
  smjob,
  // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);
