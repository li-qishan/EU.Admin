import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton, DatePicker } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import DetailTable from '../../shiporderdetail/components/TableList';

import dayjs from 'dayjs';
import Utility from '@/utils/utility';

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
            moduleCode: "SD_SHIP_ORDER_MNG",
            tabkey: 1,
            isLoading: true,
            detailCount: 0,
            IsView: false,
            modelName: 'shiporder'
        };
    }
    componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id, disabled: IsView ? true : false })
            dispatch({
                type: 'shiporder/getById',
                payload: { Id },
            }).then((result) => {
                let data = result.response.data;
                let detailCount = result.response.count;
                this.setState({ isLoading: false })
                if (data && me.formRef.current) {
                    Utility.setFormFormat(data);
                    me.formRef.current.setFieldsValue(data);
                    if (data.AuditStatus == 'CompleteAudit')
                        IsView = true;
                    me.setState({ AuditStatus: data.AuditStatus, detailCount, IsView })
                }
            })
        } else me.setState({ isLoading: false })

    }
    componentDidMount() {
        const { Id } = this.props;
        if (!Id)
            me.formRef.current.setFieldsValue({
                OrderDate: dayjs()
            });

    }
    onFinish(values) {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            values.ID = Id;
        dispatch({
            type: 'shiporder/saveData',
            payload: values,
        }).then(() => {
            const { shiporder: { Id } } = me.props;
            if (Id)
                this.setState({ Id, AuditStatus: 'Add' })
            DetailTable.reloadData()
        });
    }
    render() {
        let { dispatch, moduleInfo } = this.props;
        let { IsView, Id, AuditStatus, detailCount } = this.state;

        if (AuditStatus == 'CompleteAudit')
            IsView = true;

        return (
            <Form
                labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
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
                </Card> : <div>
                    <Card>
                        <FormToolbar value={{ me, dispatch }}
                            moduleInfo={moduleInfo}
                            onAuditSubmit={(status) => {
                                if (status == 'Add') {

                                } else if (status == 'CompleteAudit') {

                                }
                            }} />

                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="OrderNo" label="发货单号">
                                    <Input placeholder="请输入" disabled='trues' defaultValue="自动" />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="OrderDate" label="作业日期" rules={[{ required: true }]}>
                                    <DatePicker
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="CustomerId" label="客户" rules={[{ required: true }]}>
                                    <ComboGrid
                                        api="/api/Customer/GetPageList"
                                        itemkey="ID"
                                        itemvalue="CustomerName"
                                        disabled={IsView || (detailCount > 0 ? true : false)}
                                        param={{
                                            IsActive: true
                                        }}
                                        onChange={(value) => {
                                            dispatch({
                                                type: 'customer/getById',
                                                payload: { Id: value },
                                            }).then((result) => {
                                                if (result.response && me.formRef.current) {
                                                    let data = result.response;
                                                    me.formRef.current.setFieldsValue({
                                                        TaxType: data.TaxType,
                                                        TaxRate: data.TaxRate,
                                                        CurrencyId: data.CurrencyId,
                                                        SettlementWayId: data.SettlementWayId
                                                    })

                                                }
                                            })
                                            dispatch({
                                                type: 'customerdeliveryaddress/getDefaultData',
                                                payload: { masterId: value },
                                            }).then((result) => {
                                                if (result.response && me.formRef.current) {
                                                    let data = result.response.data;
                                                    if (data)
                                                        me.formRef.current.setFieldsValue({
                                                            Contact: data.Contact,
                                                            Phone: data.Phone,
                                                            Address: data.Address
                                                        })

                                                }
                                            })
                                        }}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="StockId" label="发货仓库">
                                    <ComboGrid
                                        api="/api/Stock/GetPageList"
                                        itemkey="ID"
                                        itemvalue="StockNames"
                                        disabled={IsView}
                                        onChange={() => {
                                        }}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="ShipDate" label="发货日期" rules={[{ required: true }]}>
                                    <DatePicker
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Contact" label="收货人">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>

                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="Phone" label="收货电话">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Address" label="收货地址">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Remark" label="备注">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                        </Space>

                    </Card>

                    <div style={{ height: 10 }}></div>
                    {Id ? <Card>
                        <Tabs>
                            <TabPane
                                tab='基本信息'
                                key='1'
                            >
                                <DetailTable Id={Id} IsView={IsView} shipdetailme={me} />
                            </TabPane>
                        </Tabs>
                    </Card> : null}
                </div>
                }
            </Form>
        )
    }
}
export default connect(({ shiporder, employee }) => ({
    shiporder, employee
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);
