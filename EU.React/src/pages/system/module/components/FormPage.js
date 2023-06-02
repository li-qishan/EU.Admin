import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Switch, InputNumber, Spin, Skeleton } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import ComboGrid from '@/components/SysComponents/ComboGrid';

const FormItem = Form.Item;

let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            isLoading: true
        };
    }
    componentDidMount() {
        const { dispatch, Id } = this.props;
        if (Id) {
            this.setState({ Id })
            dispatch({
                type: 'module/getById',
                payload: { Id },
            }).then((result) => {
                if (result.response && me.formRef.current) {
                    this.setState({ isLoading: false })
                    me.formRef.current.setFieldsValue(result.response)
                }
                else {
                    me.formRef.current.setFieldsValue({ IsExecQuery: true });
                }
            })
        }
        else {
            me.setState({ isLoading: false })

            this.formRef.current.setFieldsValue({
                IsShowAdd: true,
                IsShowUpdate: true,
                IsShowView: true,
                IsShowDelete: true,
                IsShowBatchDelete: true,
                IsExecQuery: true
            })
        }

    }
    onFinish(values) {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id) {
            values.ID = Id;
        }
        dispatch({
            type: 'module/saveData',
            payload: values,
        }).then(() => {
            const { module: { Id } } = me.props;
            if (Id)
                this.setState({ Id })
        });
    }
    render() {
        const { IsView } = this.props;
        return (
            <>
                <Form
                    labelCol={{ span: 4 }}
                    wrapperCol={{ span: 16 }}
                    onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                >
                    {this.state.isLoading ? <Card>
                        <Skeleton active />
                        <Skeleton active />
                        <div style={{ textAlign: 'center', padding: 20 }}>
                            <Spin size="large" />
                        </div>
                    </Card> : <Card>
                        <Row gutter={24} justify={"center"}>
                            <Col span={12}>
                                <FormItem name="ModuleCode" label="模块代码" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={12}>
                                <FormItem name="ModuleName" label="模块名称" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={12}>
                                <FormItem name="TaxisNo" label="排序号" rules={[{ required: true }]}>
                                    <InputNumber placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={12}>
                                <FormItem name="Icon" label="图标">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={12}>
                                <FormItem name="RoutePath" label="路由">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={12}>
                                <FormItem name="ParentId" label="上级模块">
                                    {/* <ComboGrid api="/api/SmModule/GetPageList" itemkey="ID" itemvalue="ModuleName" disabled={IsView} /> */}
                                    <ComboGrid
                                        api="/api/SmModule/GetPageList"
                                        itemkey="ID"
                                        param={{
                                            IsParent: true,
                                            IsActive: true
                                        }}
                                        itemvalue="ModuleName"
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={12}>
                                <FormItem valuePropName='checked' name="IsParent" label="是否目录">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={12}>
                                <FormItem valuePropName='checked' name="IsActive" label="是否显示">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={12}>
                                <FormItem valuePropName='checked' name="IsDetail" label="是否从表">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={12}>
                                <FormItem name="BelongModuleId" label="所属模块">
                                    <ComboGrid api="/api/SmModule/GetPageList" itemkey="ID" itemvalue="ModuleName" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={12}>
                                <FormItem valuePropName='checked' name="IsShowAdd" label="新建">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={12}>
                                <FormItem valuePropName='checked' name="IsShowUpdate" label="修改">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={12}>
                                <FormItem valuePropName='checked' name="IsShowView" label="查看">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={12}>
                                <FormItem valuePropName='checked' name="IsShowDelete" label="删除">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={12}>
                                <FormItem valuePropName='checked' name="IsShowSubmit" label="提交">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={12}>
                                <FormItem valuePropName='checked' name="IsShowBatchDelete" label="批量删除">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={12}>
                                <FormItem valuePropName='checked' name="IsExecQuery" label="是否执行查询">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={12}>
                                <FormItem valuePropName='checked' name="IsShowAudit" label="是否显示审核">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={12}>
                                <FormItem name="DefaultSort" label="默认排序字段">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={12}>
                                <FormItem name="DefaultSortOrder" label="默认排序顺序">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={12}>
                                <FormItem valuePropName='checked' name="IsSum" label="是否合计">
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
                    }
                </Form>
                {/* <Card>
                    <Tabs>
                        <TabPane
                            tab='模块列'
                            key='1'
                        >
                            <DetailTable Id={this.state.Id} />
                        </TabPane>
                    </Tabs>
                </Card> */}
            </>
        )
    }
}
export default connect(({ module }) => ({
    module,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);