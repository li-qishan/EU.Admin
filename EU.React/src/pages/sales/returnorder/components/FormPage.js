import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton, DatePicker, message, Modal } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import DetailTable from '../../returnorderdetail/components/TableList';
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
            moduleCode: "SD_RETURN_ORDER_MNG",
            tabkey: 1,
            isLoading: true,
            StockId: null,
            IsView: false,
            modelName: 'returnorder'
        };
    }
    componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'returnorder/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false, disabled: IsView ? true : false })
                if (result.response && me.formRef.current) {
                    Utility.setFormFormat(result.response);
                    me.formRef.current.setFieldsValue(result.response);
                    if (result.response.AuditStatus == 'CompleteAudit' || result.response.AuditStatus == "CompleteReturn")
                        IsView = true;
                    me.setState({ AuditStatus: result.response.AuditStatus, StockId: result.response.StockId, ReturnStatus: result.response.ReturnStatus, IsView })
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
        if (!values.ReturnStatus)
            values.ReturnStatus = "WaitReturn";
        dispatch({
            type: 'returnorder/saveData',
            payload: values,
        }).then(() => {
            const { returnorder: { Id } } = me.props;
            if (Id)
                this.setState({ AuditStatus: "Add", Id, ReturnStatus: "WaitReturn" })
            // DetailTable.reloadData()
        });
    }
    onReturnCheckIn() {
        const { dispatch } = this.props;
        const { Id } = this.state;
        Modal.confirm({
            title: '确认退回入库？?',
            icon: Utility.getIcon('ExclamationCircleOutlined'),
            // content: '是否确定删除记录？',
            okText: '确定',
            okType: 'danger',
            cancelText: '取消',
            async onOk() {
                const filehide = message.loading('数据提交中..', 0)
                let result = await request('/api/ReturnOrder/ConfirmReturn', {
                    method: 'POST',
                    params: {
                        Id
                    }
                });
                setTimeout(filehide);
                if (result && result.status == "ok") {
                    me.setState({ ReturnStatus: 'CompleteReturn', AuditStatus: "CompleteReturn" });
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
        let { dispatch, moduleInfo } = this.props;
        let { IsView, Id, AuditStatus, detailCount, ReturnStatus } = this.state;
        if (AuditStatus == 'CompleteAudit' || AuditStatus == 'CompleteReturn')
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
                                <FormItem name="OrderNo" label="退货单号">
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
                                <FormItem name="CustomerId" label="退货客户" rules={[{ required: true }]}>
                                    <ComboGrid
                                        api="/api/Customer/GetPageList"
                                        itemkey="ID"
                                        itemvalue="CustomerName"
                                        disabled={IsView}
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
                                        }}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="StockId" label="退货仓库">
                                    <ComboGrid
                                        api="/api/Stock/GetPageList"
                                        itemkey="ID"
                                        itemvalue="StockNames"
                                        disabled={IsView}
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
                                <FormItem name="GoodsLocationId" label="退货货位">
                                    <ComboGrid
                                        api="/api/GoodsLocation/GetPageList"
                                        itemkey="ID"
                                        itemvalue="LocationNames"
                                        disabled={!this.state.StockId ? true : IsView}
                                        parentId={{ 'StockId': this.state.StockId }}
                                    // onChange={(value) => {
                                    // }}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="ReturnDate" label="退货日期" rules={[{ required: true }]}>
                                    <DatePicker
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="ReturnReason" label="退回原因">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Remark" label="备注">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                {/* <FormItem name="ReturnStatus" label="退货状态">
                                        <ComBoBox disabled={IsView} />
                                    </FormItem> */}
                            </Col>
                        </Row>
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                            {AuditStatus == 'CompleteAudit' && Id && ReturnStatus == "WaitReturn" ? <Button
                                type="primary"
                                onClick={() => { this.onReturnCheckIn() }} danger>退回入库</Button> : ''}
                        </Space>
                    </Card>

                    <div style={{ height: 10 }}></div>
                    {Id ? <Card>
                        <Tabs>
                            <TabPane
                                tab='基本信息'
                                key='1'
                            >
                                <DetailTable Id={Id} IsView={IsView} />
                            </TabPane>
                        </Tabs>
                    </Card> : null}
                </div>
                }
            </Form>
        )
    }
}
export default connect(({ returnorder, employee }) => ({
    returnorder, employee
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);
