import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Spin, Skeleton, DatePicker } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComboGrid from '@/components/SysComponents/ComboGrid';
// import DetailTable from '../../indetail/components/TableList';
import dayjs from 'dayjs';
import Utility from '@/utils/utility';

const FormItem = Form.Item;
let me;

class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleCode: "PD_PLAN_ORDER_MNG",
            tabkey: 1,
            isLoading: true,
            StockId: null,
            detailCount: 0,
            IsView: false,
            modelName: 'pdplan'
        };
    }
    componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id, disabled: IsView ? true : false })
            dispatch({
                type: 'pdplan/getById',
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
                PlanStartDate: dayjs(),
                OrderDate: dayjs()
            });

    }
    onFinish(values) {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            values.ID = Id;
        dispatch({
            type: 'pdplan/saveData',
            payload: values,
        }).then((result) => {
            const { pdplan: { Id } } = me.props;
            if (Id)
                this.setState({ AuditStatus: "Add", Id })
            // DetailTable.reloadData()
        });
    }
    // async onConfirmOut() {
    //     let { IsView, Id, AuditStatus, detailCount } = this.state;

    //     Modal.confirm({
    //         title: '是否确认出库？?',
    //         icon: Utility.getIcon('ExclamationCircleOutlined'),
    //         okText: '确定',
    //         okType: 'danger',
    //         cancelText: '取消',
    //         async onOk() {
    //             const filehide = message.loading('数据提交中..', 0)
    //             let result = await request('/api/OutOrder/ConfirmOut', {
    //                 method: 'POST',
    //                 params: {
    //                     Id
    //                 }
    //             });
    //             setTimeout(filehide);
    //             if (result && result.status == "ok") {
    //                 me.setState({ AuditStatus: 'CompleteOut' });
    //                 Modal.success({
    //                     content: result.message
    //                 });
    //             } else Modal.error({
    //                 content: result.message
    //             });
    //         },
    //         onCancel() {
    //             console.log('Cancel');
    //         },
    //     });
    // }
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
                                <FormItem name="CustomerId" label="客户" rules={[{ required: true }]}>
                                    <ComboGrid
                                        api="/api/Customer/GetPageList"
                                        itemkey="ID"
                                        itemvalue={['CustomerNo', 'CustomerName']}
                                        disabled={IsView}
                                        param={{
                                            IsActive: true
                                        }}
                                    />
                                </FormItem>
                            </Col>
                        </Row>

                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="Remark" label="备注">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}></Col>
                            <Col span={8}></Col>
                        </Row>

                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                            {/* {Id && AuditStatus == "CompleteAudit" ? <Button
                                type="primary"
                                onClick={() => {
                                    this.onConfirmOut()
                                }} danger>确认出库</Button> : ''} */}
                        </Space>

                    </Card>

                    <div style={{ height: 10 }}></div>
                    {/* {Id ? <Card>
                        <Tabs>
                            <TabPane
                                tab='基本信息'
                                key='1'
                            >
                                <DetailTable Id={Id} outdetailme={me} IsView={IsView} />
                            </TabPane>
                        </Tabs>
                    </Card> : null} */}
                </div>
                }
            </Form>
        )
    }
}
export default connect(({ pdplan }) => ({
    pdplan
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);
