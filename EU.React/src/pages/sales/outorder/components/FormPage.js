import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton, DatePicker, Modal, message } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import DetailTable from '../../outorderdetail/components/TableList';
import dayjs from 'dayjs';
import Utility from '@/utils/utility';
import request from "@/utils/request";

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
            moduleCode: "SD_OUT_ORDER_MNG",
            tabkey: 1,
            isLoading: true,
            StockId: null,
            detailCount: 0,
            IsView: false,
            modelName: 'outorder'
        };
    }
    componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id, disabled: IsView ? true : false })
            dispatch({
                type: 'outorder/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false })
                let data = result.response.data;
                let detailCount = result.response.count;
                if (data && me.formRef.current) {
                    Utility.setFormFormat(data);
                    me.formRef.current.setFieldsValue(data);

                    if (data.AuditStatus == 'CompleteAudit' || data.AuditStatus == 'CompleteOut')
                        IsView = true;

                    me.setState({ AuditStatus: data.AuditStatus, StockId: data.StockId, detailCount, IsView });
                }
            })
        } else me.setState({ isLoading: false })

    }
    componentDidMount() {
        const { Id } = this.props;
        if (!Id)
            me.formRef.current.setFieldsValue({
                OrderDate: dayjs(),
                OutDate: dayjs()
            });

    }
    onFinish(values) {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            values.ID = Id;
        dispatch({
            type: 'outorder/saveData',
            payload: values,
        }).then((result) => {
            const { outorder: { Id } } = me.props;
            if (Id)
                this.setState({ AuditStatus: "Add", Id })
            // DetailTable.reloadData()
        });
    }
    async onConfirmOut() {
        let { IsView, Id, AuditStatus, detailCount } = this.state;

        Modal.confirm({
            title: '是否确认出库？?',
            icon: Utility.getIcon('ExclamationCircleOutlined'),
            okText: '确定',
            okType: 'danger',
            cancelText: '取消',
            async onOk() {
                const filehide = message.loading('数据提交中..', 0)
                let result = await request('/api/OutOrder/ConfirmOut', {
                    method: 'POST',
                    params: {
                        Id
                    }
                });
                setTimeout(filehide);
                if (result && result.status == "ok") {
                    me.setState({ AuditStatus: 'CompleteOut' });
                    Modal.success({
                        content: result.message
                    });
                } else Modal.error({
                    content: result.message
                });
            },
            onCancel() {
                console.log('Cancel');
            },
        });
    }
    render() {
        // const { IsView, dispatch } = this.props;
        // const { Id, detailCount } = this.state;

        let { dispatch, moduleInfo } = this.props;
        let { IsView, Id, AuditStatus, detailCount } = this.state;

        if (AuditStatus == 'CompleteAudit' || AuditStatus == 'CompleteOut')
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
                    <FormToolbar value={{ me, dispatch }}
                        moduleInfo={moduleInfo}
                    // onAuditSubmit={(status) => {
                    //     if (status == 'Add') {

                    //     } else if (status == 'CompleteAudit') {

                    //     }
                    // }}
                    />
                    <Card>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="OrderNo" label="出货单号">
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
                                        disabled={IsView || (detailCount > 0 ? true : false)}
                                        onChange={(value) => {
                                            // if (value != this.state.StockId) {
                                            //     me.formRef.current.setFieldsValue({ 'DepartmentId': '' });
                                            // }
                                            this.setState({ StockId: value })
                                        }}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="GoodsLocationId" label="发货货位">
                                    <ComboGrid
                                        api="/api/GoodsLocation/GetPageList"
                                        itemkey="ID"
                                        itemvalue="LocationNames"
                                        disabled={!this.state.StockId ? true : (IsView || (detailCount > 0 ? true : false))}
                                        parentId={{ 'StockId': this.state.StockId }}
                                    // onChange={(value) => {
                                    // }}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="OutDate" label="出库日期" rules={[{ required: true }]}>
                                    <DatePicker
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="Contact" label="收货人">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
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

                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="Remark" label="备注">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={16}>

                            </Col>
                        </Row>

                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                            {Id && AuditStatus == "CompleteAudit" ? <Button
                                type="primary"
                                onClick={() => {
                                    this.onConfirmOut()
                                }} danger>确认出库</Button> : ''}
                        </Space>

                    </Card>

                    <div style={{ height: 10 }}></div>
                    {Id ? <Card>
                        <Tabs>
                            <TabPane
                                tab='基本信息'
                                key='1'
                            >
                                <DetailTable Id={Id} outdetailme={me} IsView={IsView} />
                            </TabPane>
                        </Tabs>
                    </Card> : null}
                </div>
                }
            </Form>
        )
    }
}
export default connect(({ outorder, employee }) => ({
    outorder, employee
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);
