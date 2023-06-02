import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton, Switch, message } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import ComBoBox from '@/components/SysComponents/ComboBox';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import DetailTable from '../../goodslocation/components/TableList';
import FormToolbar from '@/components/SysComponents/FormToolbar';

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
            isLoading: true,
            moduleCode: "BD_STOCK_MNG",
            disabled: true
        };
    }
    componentWillMount() {
        const { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'stock/getById',
                payload: { Id },
            }).then((result) => {
                if (result.response && me.formRef.current) {
                    me.setState({ isLoading: false, disabled: IsView ? true : false })
                    me.formRef.current.setFieldsValue(result.response)

                }
            })
        } else me.setState({ isLoading: false })

    }
    // onFinish(values) {
    //     const { dispatch } = this.props;
    //     const { Id } = this.state;
    //     if (Id) {
    //         values.ID = Id;
    //     }
    //     dispatch({
    //         type: 'stock/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { stock: { Id } } = me.props;
    //         if (Id)
    //             this.setState({ Id })
    //     });
    // }
    onFinish(data, type = 'Save') {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            data.ID = Id;

        dispatch({
            type: 'stock/saveData',
            payload: data
        }).then((result) => {
            let response = result.response;
            if (response.status === "ok") {
                if (type == 'SaveAdd') {
                    me.setState({
                        Id: ''
                    });
                    me.formRef.current.resetFields();
                }
                else if (!Id)
                    this.setState({ Id: response.Id });
                message.success(response.message);
            }
            else
                message.error(response.message);
        });
    }
    onFinishAdd() {
        me.formRef.current.validateFields()
            .then(values => {
                me.onFinish(values, 'SaveAdd')
            });
    }
    render() {
        const { IsView, dispatch, moduleInfo } = this.props;
        const { Id, isLoading } = this.state;
        return (
            <>
                <Form
                    labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                    wrapperCol={{ span: 16 }}
                    onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                >
                    <FormToolbar value={{ me, dispatch }}
                        moduleInfo={moduleInfo}
                    />
                    {isLoading ? <Card>
                        <Skeleton active />
                        <div style={{ textAlign: 'center', padding: 20 }}>
                            <Spin size="large" />
                        </div>
                    </Card> : <Card>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="StockNo" label="仓库编号" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="StockNames" label="仓库名称" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="StockCategory" label="仓库类别" rules={[{ required: true }]}>
                                    <ComBoBox disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="StockKeeperId" label="仓管员" >
                                    <ComboGrid
                                        api="/api/Account/GetPageList"
                                        itemkey="ID"
                                        itemvalue="UserName"
                                        onChange={(value) => {
                                            this.setState({ StockKeeperId: value })
                                        }}
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem valuePropName='checked' name="IsVirtual" label="是否虚拟仓">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                            </Col>
                        </Row>
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            {!IsView ? <Button type="primary" onClick={() => this.onFinishAdd()}>保存并新建</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                        </Space>
                    </Card>
                    }
                </Form>
                <div style={{ height: 10 }}></div>
                {Id ? <Card>
                    <Tabs>
                        <TabPane
                            tab='货位管理'
                            key='1'
                        >
                            <DetailTable Id={Id} IsView={IsView} />
                        </TabPane>
                    </Tabs>
                </Card> : null}
            </>
        )
    }
}
export default connect(({ stock }) => ({
    stock,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);