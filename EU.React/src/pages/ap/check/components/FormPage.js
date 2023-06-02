import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton, DatePicker, Modal, message } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import DetailTable from '../../checkdetail/components/TableList';
import dayjs from 'dayjs';
import Utility from '@/utils/utility';
import request from "@/utils/request";

const FormItem = Form.Item;
const { TabPane } = Tabs;
const { RangePicker } = DatePicker;
let me;

class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleCode: "AP_CHECK_ORDER_MNG",
            tabkey: 1,
            isLoading: true,
            detailCount: 0,
            IsView: false,
            modelName: 'apcheck'
        };
    }
    componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id, disabled: IsView ? true : false })
            dispatch({
                type: 'apcheck/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false })
                let data = result.response;
                // let detailCount = result.response.count;
                if (data && me.formRef.current) {
                    Utility.setFormFormat(data);
                    data.Period = [];
                    data.Period.push(data.PeriodBeginTime);
                    data.Period.push(data.PeriodEndTime);
                    me.formRef.current.setFieldsValue(data);
                    dispatch({
                        type: 'apcheck/setDetail',
                        payload: data
                    });
                    if (data.AuditStatus == 'CompleteAudit')
                        IsView = true;

                    me.setState({ AuditStatus: data.AuditStatus, IsView });
                }
            })
        } else me.setState({ isLoading: false })

    }
    componentDidMount() {
        const { Id } = this.props;
        if (!Id)
            me.formRef.current.setFieldsValue({
                YearMonth: dayjs(),
                OrderDate: dayjs()
            });

    }
    onFinish(values) {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            values.ID = Id;
        values.PeriodBeginTime = values.Period[0];
        values.PeriodEndTime = values.Period[1];
        delete values.Period;
        dispatch({
            type: 'apcheck/saveData',
            payload: values,
        }).then((result) => {
            const { apcheck: { Id } } = me.props;
            if (Id)
                this.setState({ AuditStatus: "Add", Id })
            // DetailTable.reloadData()
        });
    }
    async onGenerateDetail() {
        let { IsView, Id, AuditStatus, detailCount } = this.state;

        Modal.confirm({
            title: '是否生成明细？?',
            icon: Utility.getIcon('ExclamationCircleOutlined'),
            okText: '确定',
            okType: 'danger',
            cancelText: '取消',
            async onOk() {
                const filehide = message.loading('数据处理中..', 0)
                let result = await request('/api/ApCheckDetail/GenerateDetail/' + Id, {
                    method: 'POST'
                });
                setTimeout(filehide);
                if (result && result.Success) {
                    DetailTable.reloadData()
                    Modal.success({
                        content: result.Message
                    });
                } else Modal.error({
                    content: result.Message
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

        if (AuditStatus == 'CompleteAudit' || AuditStatus == 'CompleteIn')
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
                                <FormItem name="OrderNo" label="单号">
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
                                {/* <FormItem name="InDate" label="入库日期" rules={[{
                                    required: true
                                }]}>
                                    <DatePicker
                                    />
                                </FormItem> */}
                                <FormItem name="SupplierId" label="供应商" rules={[{ required: true }]}>
                                    <ComboGrid
                                        api="/api/Supplier/GetPageList"
                                        itemkey="ID"
                                        param={{
                                            IsActive: true
                                        }}
                                        itemvalue={['SupplierNo', 'FullName']}
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="Period" label="对账周期" rules={[{ required: true }]}>
                                    <RangePicker />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Remark" label="备注">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                            </Col>
                        </Row>

                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                            {Id && AuditStatus == "Add" ? <Button
                                type="primary"
                                onClick={() => {
                                    this.onGenerateDetail()
                                }} danger>生成明细</Button> : ''}
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
export default connect(({ apcheck, user }) => ({
    apcheck, user
}))(FormPage);
