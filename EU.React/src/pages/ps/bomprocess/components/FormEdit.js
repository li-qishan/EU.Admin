import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, Button, InputNumber, Switch } from 'antd';
import { connect } from 'umi';
import ComBoBox from '@/components/SysComponents/ComboBox';
import Utility from '@/utils/utility';
import ComboGrid from '@/components/SysComponents/ComboGrid';
const FormItem = Form.Item;
let me;
class FormEdit extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            moduleCode: "PS_PROCESS_BOM_PROCESS_MNG"
        };
    }
    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible && nextProps.modalVisible) {
            const { dispatch, modalVisible } = nextProps;
            const { psbomprocess: { Id } } = me.props;
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'psbomprocess/getById',
                    payload: { Id },
                }).then((result) => {
                    if (result && me.formRef.current) {
                        Utility.setFormFormat(result);
                        me.setState({ ProcessId: result.ProcessId })
                        me.formRef.current.setFieldsValue(result)
                    }
                })
            }
        }
    }

    okHandle = async (value) => {
        const { onSubmit: handleAdd } = this.props;
        const fieldsValue = await me.formRef.current.validateFields();
        // me.formRef.current.resetFields();
        const { psbomprocess: { Id } } = me.props;
        if (Id)
            fieldsValue.ID = Id;
        if (value == "newSave")
            handleAdd(fieldsValue, me.onSubmitSuccess);
        else
            handleAdd(fieldsValue);
    };
    onSubmitSuccess() {
        const { dispatch } = me.props;
        me.formRef.current.resetFields();
        dispatch({
            type: 'psbomprocess/setId',
            payload: ''
            // }).then(() => {
            //     me.setState({ detailVisible, DetailId, detailView });
        });
    }
    // onFinish(values) {
    //     const { dispatch } = this.props;
    //     const { Id } = this.state;
    //     if (Id) {
    //         values.ID = Id;
    //     }
    //     dispatch({
    //         type: 'psbomprocess/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, psbomprocess: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel, psbomprocess: { Id } } = this.props;

        return (
            (<Modal
                destroyOnClose
                title={Id ? '工艺路线->编辑' : '工艺路线->新增'}
                open={modalVisible}
                // onOk={this.okHandle}
                maskClosable={false}
                width={1200}
                onCancel={() => onCancel()}
                footer={[
                    <Button key="submit" disabled={IsView} type="primary" onClick={this.okHandle}>
                        保存
                    </Button>,
                    <Button
                        key="submit1"
                        type="primary" disabled={IsView}
                        onClick={() => {
                            me.okHandle('newSave');
                        }}
                    >
                        保存并新建
                    </Button>,
                    <Button key="back" onClick={() => onCancel()}>
                        返回
                    </Button>
                ]}
            >
                <Form
                    labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                    wrapperCol={{ span: 16 }}
                    // onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                >
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="ProcessId" label="工序" rules={[{ required: true }]}>
                                <ComboGrid
                                    api="/api/Process/GetPageList"
                                    itemkey="ID"
                                    param={{
                                        IsActive: true
                                    }}
                                    itemvalue={['ProcessNo', 'ProcessName']}
                                    disabled={IsView}
                                    onChange={(value) => {
                                        if (value != this.state.ProcessId) {
                                            me.formRef.current.setFieldsValue({ 'MachineId': null });
                                        }
                                        this.setState({ ProcessId: value })
                                    }}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="MachineId" label="机台">
                                <ComboGrid
                                    api="/api/Process/GetMachineList"
                                    itemkey="MachineId"
                                    param={{
                                        IsActive: true,
                                        moduleCode: 'PS_PROCESS_MACHINE_MNG',
                                        pageSize: 1000000
                                    }}
                                    itemvalue={['MachineNo', 'MachineName']}
                                    disabled={!this.state.ProcessId ? true : (IsView || false)}
                                    parentId={{ 'ProcessId': this.state.ProcessId }}
                                // onChange={(value) => {
                                // }}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="WeightUnit" label="重量单位">
                                <ComBoBox disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="SingleWeight" label="单重">
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="ProcessingDays" label="加工天数">
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="SetupTime" label="调机时间">
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="TimeUnit" label="工时单位">
                                <ComBoBox disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="StandardHours" label="标准工时">
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="StandardWages" label="标准工价">
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem valuePropName='checked' name="IsTransfer" label="检验后转移">
                                <Switch disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="RejectRate" label="工艺不良率">
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="Remark" label="备注">
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                </Form>
            </Modal>)
        );
    }
}
export default connect(({ psbomprocess }) => ({
    psbomprocess,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormEdit);