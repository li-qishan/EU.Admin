import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton, DatePicker } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import DetailTable from '../../requestiondetail/components/TableList';
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
            moduleCode: "PO_REQUESTION_MNG",
            isLoading: true,
            disabled: true,
            modelName: 'requestion'
        };
    }
    componentWillMount() {
        const { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'requestion/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false, disabled: IsView ? true : false })
                if (result.response && me.formRef.current) {
                    Utility.setFormFormat(result.response);
                    me.formRef.current.setFieldsValue(result.response);
                    me.setState({ AuditStatus: result.response.AuditStatus })
                }
            })
        } else me.setState({ isLoading: false })
        if (!Id)
            dispatch({
                type: 'user/getUserDepartment',
                payload: {},
            }).then((result) => {
                if (result.response && result.response.data) {
                    me.setState({ DepartmentId: result.response.data.ID })
                }
            })
    }
    componentDidMount() {
        const { Id } = this.props;
        if (!Id)
            me.formRef.current.setFieldsValue({
                OrderDate: dayjs()
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
            type: 'requestion/saveData',
            payload: values,
        }).then(() => {
            const { requestion: { Id } } = me.props;
            if (Id)
                this.setState({ Id, AuditStatus: 'Add', disabled: false })
            // DetailTable.reloadData()
        });
    }
    render() {
        let { IsView, dispatch, moduleInfo } = this.props;
        const { Id, isLoading, AuditStatus } = this.state;
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
                </Card> : <div>
                    <FormToolbar value={{ me, dispatch }}
                        moduleInfo={moduleInfo}
                    // onAuditSubmit={(status) => {
                    // }}
                    />
                    <Card>
                        <FormStatus AuditStatus={AuditStatus} />

                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="OrderNo" label="请购单号">
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
                                <FormItem name="RequestionDate" label="需求日期" rules={[{ required: true }]}>
                                    <DatePicker
                                        disabled={IsView}
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
                            <Col span={8}>
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
export default connect(({ requestion, user }) => ({
    requestion, user
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);
