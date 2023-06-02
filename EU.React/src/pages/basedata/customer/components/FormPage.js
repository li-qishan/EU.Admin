import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Skeleton, Spin, InputNumber, Tabs, Switch, message } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComBoBox from '@/components/SysComponents/ComboBox';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import DeliveryAddress from '../../customerdeliveryaddress/components/TableList';
import Contact from '../../customercontact/components/TableList';
import Invoice from '../../customerinvoice/components/TableList';

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
            moduleCode: "BD_CUSTOMER_MNG",
            tabkey: 1,
            isLoading: true,
            TaxType: '',
            disabled: true
        };
    }
    componentWillMount() {
        const { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'customer/getById',
                payload: { Id },
            }).then((result) => {
                me.setState({ isLoading: false, disabled: IsView ? true : false })
                if (result.response && me.formRef.current) {
                    me.formRef.current.setFieldsValue(result.response)
                    me.setState({ TaxType: result.response.TaxType })
                }
            })
        } else me.setState({ isLoading: false })

    }
    componentDidMount() {
        const { Id } = this.props;
        if (!Id)
            me.formRef.current.setFieldsValue({
                IsActive: true
            });
    }
    // onFinish(values) {
    //     const { dispatch } = this.props;
    //     const { Id } = this.state;
    //     if (Id) {
    //         values.ID = Id;
    //     }
    //     dispatch({
    //         type: 'customer/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { customer: { Id } } = me.props;
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
            type: 'customer/saveData',
            payload: data
        }).then((result) => {
            let response = result.response;
            if (response.status === "ok") {
                if (type == 'SaveAdd') {
                    me.setState({
                        Id: ''
                    });
                    me.formRef.current.resetFields();
                    me.formRef.current.setFieldsValue({
                        IsActive: true
                    });
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
    onTabClick(key) {
        me.setState({ tabkey: key });
    }
    render() {
        const { IsView, dispatch, moduleInfo } = this.props;
        const { Id, tabkey, isLoading, TaxType } = this.state;

        return (
            <Form
                labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
            >
                {isLoading ? <Card>
                    <Skeleton active />
                    <Skeleton active />
                    <div style={{ textAlign: 'center', padding: 20 }}>
                        <Spin size="large" />
                    </div>
                </Card> : <>
                    <FormToolbar value={{ me, dispatch }}
                        moduleInfo={moduleInfo}
                    />
                    <Card>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="CustomerNo" label="客户编号" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="CustomerName" label="客户名称" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="CustomerLevel" label="客户等级">
                                    <ComBoBox disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24}>
                            <Col span={8}>
                                <FormItem valuePropName='checked' name="IsActive" label="是否启用">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Remark" label="备注">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}></Col>
                        </Row>
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            {!IsView ? <Button type="primary" onClick={() => this.onFinishAdd()}>保存并新建</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                        </Space>
                    </Card>
                    <div style={{ height: 10 }}></div>

                    <Card>
                        <Tabs onTabClick={this.onTabClick}>

                            <TabPane
                                tab={<span>基本信息</span>}
                                key="1"
                            >
                                <Row gutter={24} justify={"center"}>
                                    <Col span={8}>
                                        <FormItem name="Contact" label="联系人">
                                            <Input placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="Phone" label="电话">
                                            <Input placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="EmployeeId" label="业务员" >
                                            <ComboGrid
                                                api="/api/SmEmployee/GetPageList"
                                                itemkey="ID"
                                                itemvalue="EmployeeName"
                                                disabled={IsView}
                                            />
                                        </FormItem>
                                    </Col>
                                </Row>

                                <Row gutter={24} justify={"center"}>
                                    <Col span={8}>
                                        <FormItem name="TaxType" label="税别" rules={[{ required: true }]}>
                                            <ComBoBox disabled={IsView}
                                                onChange={(value) => {
                                                    if (value == 'ZeroTax')
                                                        me.formRef.current.setFieldsValue({
                                                            TaxRate: 0
                                                        });

                                                    this.setState({ TaxType: value })
                                                }}
                                            />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="TaxRate" label="税率" rules={[{ required: true }]}>
                                            <InputNumber placeholder="请输入" disabled={TaxType == 'ZeroTax' ? true : IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="SettlementWayId" label="结算方式" rules={[{ required: true }]}>
                                            <ComboGrid
                                                api="/api/SettlementWay/GetPageList"
                                                itemkey="ID"
                                                itemvalue="SettlementName"
                                                // onChange={(value) => {
                                                //     this.setState({ StockKeeperId: value })
                                                // }}
                                                disabled={IsView}
                                            />
                                        </FormItem>
                                    </Col>
                                </Row>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={8}>
                                        <FormItem name="DeliveryWayId" label="送货方式">
                                            <ComboGrid
                                                api="/api/DeliveryWay/GetPageList"
                                                itemkey="ID"
                                                itemvalue="DeliveryName"
                                                // onChange={(value) => {
                                                //     this.setState({ StockKeeperId: value })
                                                // }}
                                                disabled={IsView}
                                            />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="CurrencyId" label="币别" rules={[{ required: true }]}>
                                            <ComboGrid
                                                api="/api/Currency/GetPageList"
                                                itemkey="ID"
                                                itemvalue="CurrencyName"
                                                // onChange={(value) => {
                                                //     this.setState({ StockKeeperId: value })
                                                // }}
                                                disabled={IsView}
                                            />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="CreditLine" label="信用额度">
                                            <InputNumber placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                </Row>

                                <Row gutter={24} justify={"center"}>
                                    <Col span={8}>
                                        <FormItem name="MainProduct" label="主营产品">
                                            <Input placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>

                                    </Col>
                                    <Col span={8}>

                                    </Col>
                                </Row>
                            </TabPane>
                            <TabPane
                                tab={<span>送货地址</span>}
                                key="2"
                            >
                                <DeliveryAddress Id={Id} IsView={IsView} />
                            </TabPane>
                            <TabPane
                                tab={<span>联系人</span>}
                                key="3"
                            >
                                <Contact Id={Id} IsView={IsView} />
                            </TabPane>
                            <TabPane
                                tab={<span>开票信息</span>}
                                key="4"
                            >
                                <Invoice Id={Id} IsView={IsView} />
                            </TabPane>
                        </Tabs>
                        {/* {tabkey == 1 ? <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            {!IsView ? <Button type="primary" onClick={() => this.onFinishAdd()}>保存并新建</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                        </Space> : null} */}
                    </Card>
                </>
                }
            </Form>
        )
    }
}
export default connect(({ customer }) => ({
    customer,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);