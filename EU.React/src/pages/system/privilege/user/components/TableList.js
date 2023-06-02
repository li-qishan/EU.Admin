import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete, ResetPassword } from '../service';
import Index from '../index';
import { Modal, Button, Row, Col, Form, Input, message } from 'antd';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';
const FormItem = Form.Item;

let moduleCode = "SM_USER_MNG";
let me;
class TableList extends Component {
  formRef = React.createRef();
  constructor(props) {
    super(props);
    me = this;
    me.state = {
      modalVisible: false
    };
  }
  componentWillMount() {
    const { dispatch, smuser: { moduleInfo } } = this.props;

    if (moduleInfo == null)
      dispatch({
        type: 'smuser/getModuleInfo',
        payload: { moduleCode },
      })
  }

  resetPassword = async () => {
    const { passWord, userId } = this.state;
    if (!passWord) {
      message.error("请输入密码!");
      return false;
    }
    message.loading('数据提交中...', 0);
    let response = await ResetPassword({ Id: userId, Password: passWord });
    message.destroy();
    message.success(response.Message);
    this.setState({ modalVisible: false, passWord: null })

  };
  render() {
    const { dispatch, smuser: { moduleInfo, tableParam } } = this.props;
    const { modalVisible } = this.state;
    const columns = [
      {
        title: '创建时间',
        dataIndex: 'CreatedTime',
        hideInSearch: true,
        sorter: true,
        width: 200
      },
      {
        title: '账号',
        dataIndex: 'UserAccount'
      },
      {
        title: '用户名',
        dataIndex: 'UserName'
      }
    ];

    //#region 操作栏按钮方法
    const ResetPassword = (Id) => {
      // Index.changePage(<SqlEdit Id={Id} />)
      this.setState({ modalVisible: true, userId: Id })
    }
    const action = {
      ResetPassword
    }
    //#endregion

    return (
      <>
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
              type: 'smuser/setTableStatus',
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
              type: 'smuser/setTableStatus',
              payload: { params, sorter },
            })
            return query({
              paramData: JSON.stringify(params),
              sorter: JSON.stringify(sorter),
              filter: JSON.stringify(filter)
            })
          }}
        />

        <Modal
          destroyOnClose
          title='密码重置'
          open={modalVisible}
          // onOk={this.okHandle}
          maskClosable={false}
          width={1000}
          onCancel={() => {
            this.setState({ modalVisible: false })
          }}
          footer={null}
        >
          <Row gutter={24} justify={"center"}>
            <Col span={8}>
            </Col>
            <Col span={8}>
              <FormItem name="passWord" label="密码" rules={[{ required: true }]}>
                <Input type='password' placeholder="请输入" onChange={(passWord, Option, data) => {
                  this.setState({ passWord: passWord.target.value })
                }} />
              </FormItem>
            </Col>
            <Col span={8}>
            </Col>
          </Row>
          <Row gutter={24} justify={"center"}>
            <Col span={8}>
            </Col>
            <Col span={8} >
              <Button type="primary" onClick={this.resetPassword}>保存</Button>
            </Col>
            <Col span={6}>
            </Col>
          </Row>
        </Modal>
      </>
    )
  }
}
export default connect(({ smuser, loading }) => ({
  smuser,
  // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);
