import React, { Component } from 'react';
import { Button, Tree, Skeleton, Input, Card, Form, Row, Col, Space, Switch, Tabs, Checkbox, Collapse } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import { GetAllModuleList, GetAllFuncPriv, GetById, GetRoleModule, GetRoleFuncPriv } from '../service'

const FormItem = Form.Item;
const { TabPane } = Tabs;
const { Panel } = Collapse;

let me;
class FormPage extends Component {
  formRef = React.createRef();
  constructor(props) {
    super(props);
    me = this;
    me.state = {
      Id: '',
      treeData: [],
      treeFuncPrivData: [],
      expandedKeys: ['All'],
      checkedModuleKeys: [],
      selectModuleId: 'All',
      noAction: [],
      checkedFuncPrivKeys: []
    };
  }
  async componentWillMount() {
    const { dispatch, Id, smrole: { noAction, functionData, addAction } } = this.props;
    if (noAction)
      noAction.length = 0;
    if (functionData)
      functionData.length = 0;
    if (addAction)
      addAction.length = 0;

    if (dispatch && Id) {
      this.setState({ Id })
      // dispatch({
      //       type: 'smrole/getById',
      //       payload: { Id },
      //   }).then((result) => {
      // alert('2')
      var result = await GetById({ Id });
      if (result && me.formRef.current)
        me.formRef.current.setFieldsValue(result);

      result = await GetRoleModule({ RoleId: Id });
      debugger
      if (result && result.Data.length > 0) {
        var checkedModuleKeys = [];
        for (var i = 0; i < result.Data.length; i++) {
          checkedModuleKeys.push(result.Data[i].SmModuleId);
        }
        this.setState({ checkedModuleKeys: checkedModuleKeys });
      }
      result = await GetRoleFuncPriv({ RoleId: Id });
      debugger
      if (result && result.data.length > 0) {
        this.setState({ checkedFuncPrivKeys: result.data });
      }
    }
    var response = await GetAllModuleList()
    if (response.status == 'ok' && response.data) {
      // response.data.children.forEach(element => {

      // });
      this.setState({ treeData: [response.data] })
    }
    var response = await GetAllFuncPriv()
    if (response.status == 'ok' && response.data) {
      this.setState({ treeFuncPrivData: [response.data] })
    }
  }
  onFinish(values) {
    const { dispatch, smrole: { noAction, addAction } } = this.props;
    const { Id, checkedModuleKeys, checkedFuncPrivKeys } = this.state;
    if (Id) {
      values.ID = Id;
    }
    dispatch({
      type: 'smrole/saveData',
      payload: values,
    }).then(() => {
      const { dispatch, smrole: { Id } } = me.props;
      this.setState({ Id })
      dispatch({
        type: 'smrole/saveRoleModule',
        payload: { ModuleList: checkedModuleKeys, RoleId: Id },
      })
      dispatch({
        type: 'smrole/saveRoleFunction',
        payload: { RoleFuncData: noAction ?? [], AddAction: addAction ?? [], RoleId: Id },
      })
      dispatch({
        type: 'smrole/saveRoleFuncPriv',
        payload: { FunctionList: checkedFuncPrivKeys, RoleId: Id },
      })
    });
  }
  //#region 基础功能权限
  onModuleCheck(checkedModuleKeys) {
    me.setState({ checkedModuleKeys });
  }
  selectModule(selectKeys, e) {
    const { dispatch } = me.props;
    const { Id } = me.state;
    if (e.node.isLeaf) {
      me.setState({ panelTitle: e.node.title, selectModuleId: selectKeys[0] });
      dispatch({
        type: 'smrole/getModuleFunction',
        payload: { moduleId: selectKeys.length > 0 ? selectKeys[0] : null, roleId: Id },
      })
    } else {
      dispatch({
        type: 'smrole/getModuleFunction',
        payload: null,
      })
    }
  }
  changeFunc(values) {
    const { dispatch, smrole: { checkFunction } } = this.props;
    let noAction = checkFunction.filter((item) => {
      return !values.includes(item)
    })
    let addAction = values.filter((item) => {
      return !checkFunction.includes(item)
    })
    dispatch({
      type: 'smrole/setNoAction',
      payload: { noAction: noAction, addAction: addAction },
    })
    dispatch({
      type: 'smrole/setCheckFunction',
      payload: values,
    })
  }
  //#endregion

  //#region 异步加载树数据
  // loadModuleData({ key, children }) {
  //     const { treeData, expandedKeys } = me.state;
  //     var newExpandedKeys = expandedKeys;
  //     return new Promise(async resolve => {
  //         if (children) {
  //             resolve();
  //             return;
  //         }
  //         let response = await GetModuleList({ parentKey: key });
  //         if (response.data) {
  //             newExpandedKeys.push(key);
  //             me.setState({ expandedKeys: newExpandedKeys });
  //             var data = me.updateTreeData(treeData, key, response.data)
  //             me.setState({
  //                 treeData: data,
  //             });
  //             resolve();
  //         }
  //     });
  // }
  // updateTreeData(list, key, children) {
  //     return list.map(node => {
  //         if (node.key === key) {
  //             return { ...node, children };
  //         }
  //         if (node.children) {
  //             return { ...node, children: me.updateTreeData(node.children, key, children) };
  //         }

  //         return node;
  //     });
  // }
  //#endregion

  //#region 功能定义树
  onFuncPrivCheck(checkedFuncPrivKeys) {
    me.setState({ checkedFuncPrivKeys });
  }
  //#endregion

  render() {
    const { IsView,  smrole: { functionData, checkFunction } } = this.props;
    const { checkedModuleKeys, panelTitle, checkedFuncPrivKeys, treeData, expandedKeys } = this.state;
    return (
      <Form
        labelCol={{ span: 4 }}
        wrapperCol={{ span: 16 }}
        onFinish={(values) => { this.onFinish(values) }}
        ref={this.formRef}
      >
        <Card>
          <Row gutter={24} justify={"center"}>
            <Col span={12}>
              <FormItem name="RoleCode" label="角色代码" rules={[{ required: true }]}>
                <Input placeholder="请输入" disabled={IsView} />
              </FormItem>
            </Col>
            <Col span={12}>
              <FormItem name="RoleName" label="角色名称" rules={[{ required: true }]}>
                <Input placeholder="请输入" disabled={IsView} />
              </FormItem>
            </Col>
          </Row>
          <Row gutter={24} justify={"center"}>
            <Col span={12}>
              <FormItem valuePropName='checked' name="IsActive" label="是否启用">
                <Switch disabled={IsView} />
              </FormItem>
            </Col>
            <Col span={12}>

            </Col>
          </Row>
          <Space style={{ display: 'flex', justifyContent: 'center' }}>
            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
          </Space>
        </Card>
        <div style={{ height: 20 }}></div>
        <Card>
          <Tabs>
            <TabPane
              tab={<span>角色模块</span>}
              key="1"
            >
              <Space style={{ display: 'flex', flexDirection: 'row', alignItems: 'flex-start', justifyContent: 'space-between' }}>
                {treeData.length > 0 ? <Tree defaultExpandedKeys={expandedKeys} defaultExpandParent1={true} checkedKeys={checkedModuleKeys} onCheck={this.onModuleCheck} checkable onSelect={this.selectModule} treeData={treeData} /> : <Skeleton active />}
                {
                  functionData.length > 0 ?
                    <Collapse
                      defaultActiveKey={['1']}
                      expandIconPosition='right'
                      style={{ width: '600px' }}
                    >
                      <Panel header={panelTitle} key="1">
                        <Checkbox.Group value={checkFunction} options={functionData} onChange={(value) => { this.changeFunc(value) }} />
                      </Panel>
                    </Collapse> : ''
                }
              </Space>
            </TabPane>
            <TabPane
              tab={<span>功能定义</span>}
              key="2"
            >
              <Tree defaultExpandedKeys={expandedKeys} checkedKeys={checkedFuncPrivKeys} onCheck={this.onFuncPrivCheck} checkable treeData={this.state.treeFuncPrivData} />
            </TabPane>
          </Tabs>
        </Card>
      </Form>
    )
  }
}
export default connect(({ smrole }) => ({
  smrole,
  // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);
