import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Spin, Skeleton, InputNumber, message } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import Utility from '@/utils/utility';
import ComBoBox from '@/components/SysComponents/ComboBox';
import ComboGrid from '@/components/SysComponents/ComboGrid';

const FormItem = Form.Item;
let me;

class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleCode: "EM_MACHIN_INFO_MNG",
            // tabkey: 1,
            isLoading: true,
            disabled: false,
            modelName: 'machine',
            MachineStatus: '',
        };
    }
    componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'machine/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false, disabled: IsView ? true : false });
                let data = result.response;
                if (data && me.formRef.current) {
                    Utility.setFormFormat(data);
                    dispatch({
                        type: 'machine/setDetail',
                        payload: data
                    });
                    me.formRef.current.setFieldsValue(data);
                    if (data.AuditStatus == 'CompleteAudit' || data.AuditStatus == 'CompleteAdjust')
                        IsView = true;
                    me.setState({ AuditStatus: data.AuditStatus, IsView, MachineStatus: data.MachineStatus })
                }
            })
        } else me.setState({ isLoading: false })
    }
    componentDidMount() {
        const { Id } = this.props;
        if (!Id) {
            me.setState({ MachineStatus: 'Freeing' })
            me.formRef.current.setFieldsValue({
                MachineStatus: 'Freeing'
            });
        }

    }
    onFinish(values, type = 'Save') {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            values.ID = Id;
        dispatch({
            type: 'machine/saveData',
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
        let { IsView, AuditStatus, isLoading, MachineStatus } = this.state;
        if (AuditStatus == 'CompleteAudit' || AuditStatus == 'CompleteAdjust')
            IsView = true;
        let machineStatusIsFreeing = MachineStatus == 'Freeing' ? true : false;

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
                                <FormItem name="MachineNo" label="设备编号" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="MachineName" label="设备名称" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="MachineType" label="设备类型" rules={[{ required: true }]}>
                                    <ComboGrid
                                        api="/api/MachineType/GetPageList"
                                        itemkey="ID"
                                        param={{
                                            IsActive: true
                                        }}
                                        itemvalue={['TypeNo', 'TypeName']}
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="MachineStatus" label="设备状态">
                                    <ComBoBox disabled={IsView} onChange={(value) => {
                                        this.setState({ MachineStatus: value })
                                    }} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="StandardMachineTime" label="标准加工时间" rules={[{ required: !machineStatusIsFreeing }]}>
                                    <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="MaxMachineTime" label="最大加工时间" rules={[{ required: !machineStatusIsFreeing }]}>
                                    <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="TimeUnit" label="时间单位" rules={[{ required: !machineStatusIsFreeing }]}>
                                    <ComBoBox disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Location" label="位置" rules={[{ required: MachineStatus == 'Freeing' ? false : true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="ResponsibleId" label="责任人" rules={[{ required: MachineStatus == 'Freeing' ? false : true }]}>
                                    <ComboGrid
                                        api="/api/SmEmployee/GetPageList"
                                        itemkey="ID"
                                        itemvalue={['EmployeeCode', 'EmployeeName']}
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            {!IsView ? <Button type="primary" onClick={() => this.onFinishAdd()}>保存并新建</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                        </Space>
                    </Card>
                </>
                }
            </Form>
        )
    }
}
export default connect(({ machine }) => ({
    machine
}))(FormPage);