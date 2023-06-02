import React, { Component } from 'react';
import { Button, Tree, Input, Card, Form, Row, Col, Space, Tabs } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import { GetRoleList } from '../service'
import ComBoBox from '@/components/SysComponents/ComboBox';

const FormItem = Form.Item;
const { TabPane } = Tabs;

let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            treeData: [],
            checkedRoleKeys: []
        };
    }
    async componentWillMount() {
        const { dispatch, Id } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'smuser/getById',
                payload: { Id },
            }).then((result) => {
                if (result.response && me.formRef.current)
                    me.formRef.current.setFieldsValue(result.response)
                dispatch({
                    type: 'smuser/getUserRole',
                    payload: { UserId: Id },
                }).then((result) => {
                    if (result && result.length > 0) {
                        var checkedRoleKeys = [];
                        for (var i = 0; i < result.length; i++) {
                            checkedRoleKeys.push(result[i].SmRoleId);
                        }
                        this.setState({ checkedRoleKeys: checkedRoleKeys });
                    }
                })
            })
        }
        var response = await GetRoleList()
        if (response.status == 'ok' && response.data) {
            this.setState({ treeData: [response.data] })
        }
    }
    onFinish(values) {
        const { dispatch } = this.props;
        const { Id, checkedRoleKeys } = this.state;
        if (Id) {
            values.ID = Id;
        }
        dispatch({
            type: 'smuser/saveData',
            payload: values,
        }).then(() => {
            const { dispatch, smuser: { Id } } = me.props;
            this.setState({ Id })
            dispatch({
                type: 'smuser/saveUserRole',
                payload: { roleList: checkedRoleKeys, UserId: Id },
            })
        });
    }
    onRoleCheck(checkedRoleKeys) {
        me.setState({ checkedRoleKeys });
    }
    render() {
        const { IsView } = this.props;
        const { Id, checkedRoleKeys } = this.state;
        return (
            <Form
                labelCol={{ span: 4 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
            >
                <Card>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="UserAccount" label="账号" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="UserName" label="用户名" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="EmployeeId" label="所属员工">
                                <ComboGrid
                                    api="/api/SmEmployee/GetPageList"
                                    itemkey="ID"
                                    itemvalue="EmployeeCode,EmployeeName"
                                    disabled={IsView}
                                // onChange={() => {
                                // }}
                                />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="UserType" label="用户类型" rules={[{ required: true }]}>
                                <ComBoBox disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            {
                                !Id ? <FormItem name="PassWord" label="密码" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem> : ''
                            }

                        </Col>

                        <Col span={8}>

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
                            tab={<span>功能角色</span>}
                            key="1"
                        >
                            <Tree expandedKeys={['All']} checkedKeys={checkedRoleKeys} onCheck={this.onRoleCheck} checkable treeData={this.state.treeData} />
                        </TabPane>
                    </Tabs>
                </Card>
            </Form>
        )
    }
}
export default connect(({ smuser, user }) => ({
    smuser,
    user,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);