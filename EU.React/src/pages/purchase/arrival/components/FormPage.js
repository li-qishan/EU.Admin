import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton, DatePicker } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import DetailTable from '../../arrivaldetail/components/TableList';
import dayjs from 'dayjs';
import Utility from '@/utils/utility';
import FormStatus from '@/components/SysComponents/FormStatus';

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
            moduleCode: "PO_ARRIVAL_ORDER_MNG",
            isLoading: true,
            disabled: true,
            modelName: 'poarrival'
        };
    }
    componentWillMount() {
        const { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'poarrival/getById',
                payload: { Id },
            }).then((result) => {
                let data = result.response.data;
                let detailCount = result.response.count;
                this.setState({ isLoading: false, disabled: IsView ? true : false })
                if (data && me.formRef.current) {
                    Utility.setFormFormat(data);
                    dispatch({
                        type: 'poarrival/setDetail',
                        payload: data
                    });
                    me.formRef.current.setFieldsValue(data);
                    me.setState({ detailCount, AuditStatus: data.AuditStatus })
                }
            })
        } else me.setState({ isLoading: false })

        if (!Id)
            dispatch({
                type: 'user/getUserDepartment',
                payload: {},
            }).then((result) => {
                if (result.response && result.response.data)
                    me.setState({ DepartmentId: result.response.data.ID })

            })
    }
    componentDidMount() {
        const { Id } = this.props;
        if (!Id)
            me.formRef.current.setFieldsValue({
                OrderDate: dayjs(),
                ArrivalTime: dayjs()
            });
    }
    onFinish(values) {
        const { dispatch, user: { currentUser } } = this.props;
        const { Id, DepartmentId } = this.state;
        if (Id)
            values.ID = Id;
        else {
            values.UserId = currentUser.data.userid;
            values.DepartmentId = DepartmentId;
        }
        dispatch({
            type: 'poarrival/saveData',
            payload: values,
        }).then(() => {
            const { poarrival: { Id } } = me.props;
            if (Id)
                this.setState({ Id, AuditStatus: 'Add', disabled: false })
            // DetailTable.reloadData()
        });
    }

    render() {
        let { IsView, dispatch, moduleInfo } = this.props;
        const { Id, isLoading, AuditStatus, detailCount } = this.state;
        if (AuditStatus == 'CompleteAudit')
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
                    // onAuditSubmit={(status) => {
                    // }}
                    />
                    <Card>
                        <FormStatus AuditStatus={AuditStatus} />
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="OrderNo" label="采购单号">
                                    <Input placeholder="请输入" disabled defaultValue="自动" />
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
                                <FormItem name="ArrivalTime" label="到货日期" rules={[{ required: true }]}>
                                    <DatePicker
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="SupplierId" label="供应商" rules={[{ required: true }]}>
                                    <ComboGrid
                                        api="/api/Supplier/GetPageList"
                                        itemkey="ID"
                                        itemvalue="FullName"
                                        disabled={IsView || (detailCount > 0 ? true : false)}
                                    />
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
                        </Space>
                    </Card>

                    <div style={{ height: 10 }}></div>
                    {Id ? <Card>
                        <Tabs>
                            <TabPane
                                tab='基本信息'
                                key='1'
                            >
                                <DetailTable Id={Id} IsView={IsView} arrivalme={me} />
                            </TabPane>
                        </Tabs>
                    </Card> : null}
                </>
                }
            </Form>
        )
    }
}
export default connect(({ poarrival, user }) => ({
    poarrival, user
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);
