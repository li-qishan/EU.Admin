import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Spin, Skeleton, InputNumber, Tabs, message } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import dayjs from 'dayjs';
import Utility from '@/utils/utility';
import ComBoBox from '@/components/SysComponents/ComboBox';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import DetailTable from '../../machine/components/TableList';
import Supplier from '../../supplier/components/TableList';
import Price from '../../price/components/TableList';
import Employee from '../../employee/components/TableList';
const { TabPane } = Tabs;
const FormItem = Form.Item;
let me;

class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleCode: "PS_PROCESS_MNG",
            // tabkey: 1,
            isLoading: true,
            disabled: false,
            modelName: 'process'
        };
    }
    componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'process/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false, disabled: IsView ? true : false });
                let data = result.response;
                if (data && me.formRef.current) {
                    Utility.setFormFormat(data);
                    dispatch({
                        type: 'process/setDetail',
                        payload: data
                    });
                    me.formRef.current.setFieldsValue(data);
                    if (data.AuditStatus == 'CompleteAudit' || data.AuditStatus == 'CompleteAdjust')
                        IsView = true;
                    me.setState({ AuditStatus: data.AuditStatus, IsView })
                }
            })
        } else me.setState({ isLoading: false })
    }
    // componentDidMount() {
    //     const { Id } = this.props;
    //     if (!Id)
    //         me.formRef.current.setFieldsValue({
    //             OrderDate: dayjs()
    //         });
    // }
    onFinish(values, type = 'Save') {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            values.ID = Id;
        dispatch({
            type: 'process/saveData',
            payload: values,
        }).then((result) => {
            let response = result.response;
            if (response.status === "ok") {
                // DetailTable.reloadData()
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
        let { dispatch, moduleInfo } = this.props;
        let { IsView, AuditStatus, isLoading, Id } = this.state;
        if (AuditStatus == 'CompleteAudit' || AuditStatus == 'CompleteAdjust')
            IsView = true;

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
                                    <FormItem name="ProcessNo" label="工序编号" rules={[{ required: true }]}>
                                        <Input placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem name="ProcessName" label="工序名称" rules={[{ required: true }]}>
                                        <Input placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem name="WorkShopId" label="车间" rules={[{ required: true }]}>
                                        <ComboGrid
                                            api="/api/WorkShop/GetPageList"
                                            itemkey="ID"
                                            itemvalue={['WorkShopNo', 'WorkShopName']}
                                            disabled={IsView}
                                        />
                                    </FormItem>
                                </Col>
                            </Row>
                            <Row gutter={24} justify={"center"}>
                                <Col span={8}>
                                    <FormItem name="MachiningType" label="加工类型" rules={[{ required: true }]}>
                                        <ComBoBox disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem name="ProcessType" label="工序类型" rules={[{ required: true }]}>
                                        <ComBoBox disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem name="PricingType" label="外协定价" rules={[{ required: true }]}>
                                        <ComBoBox disabled={IsView} />
                                    </FormItem>
                                </Col>
                            </Row>
                            <Space style={{ display: 'flex', justifyContent: 'center' }}>
                                {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                                {!IsView ? <Button type="primary" onClick={() => this.onFinishAdd()}>保存并新建</Button> : ''}
                                <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                            </Space>
                        </Card>
                        <div style={{ height: 10 }}></div>
                        {Id ? <Card>
                            <Tabs>
                                <TabPane
                                    tab='机台设置'
                                    key='1'
                                >
                                    <DetailTable Id={Id} outdetailme={me} IsView={IsView} />
                                </TabPane>
                                <TabPane
                                    tab='外协厂商'
                                    key='2'
                                >
                                    <Supplier Id={Id} outdetailme={me} IsView={IsView} />
                                </TabPane>
                                <TabPane
                                    tab='工序单价'
                                    key='3'
                                >
                                    <Price Id={Id} outdetailme={me} IsView={IsView} />
                                </TabPane>
                                <TabPane
                                    tab='工序人员'
                                    key='4'
                                >
                                    <Employee Id={Id} outdetailme={me} IsView={IsView} />
                                </TabPane>
                            </Tabs>
                        </Card> : null}
                    </>
                }
            </Form>
        )
    }
}
export default connect(({ process }) => ({
    process
}))(FormPage);
