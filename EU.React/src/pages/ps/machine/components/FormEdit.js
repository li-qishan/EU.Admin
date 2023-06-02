import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, Select, Button, InputNumber } from 'antd';
import { connect } from 'umi';
import ComBoBox from '@/components/SysComponents/ComboBox';
import Utility from '@/utils/utility';
import ComboGrid from '@/components/SysComponents/ComboGrid';
const FormItem = Form.Item;
const { Option } = Select;
let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            moduleCode: "PS_PROCESS_MACHINE_MNG"
        };
    }
    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible && nextProps.modalVisible) {
            const { dispatch, modalVisible } = nextProps;
            const { processmachine: { Id } } = me.props;
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'processmachine/getById',
                    payload: { Id },
                }).then((result) => {
                    if (result && me.formRef.current) {
                        Utility.setFormFormat(result);
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
        const { processmachine: { Id } } = me.props;
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
            type: 'processmachine/setId',
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
    //         type: 'processmachine/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, processmachine: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel, processmachine: { Id } } = this.props;
        let PriorityList = [];
        for (let i = 1; i < 21; i++) {
            PriorityList.push(i)
        }

        return (
            (<Modal
                destroyOnClose
                title={Id ? '工序机台->编辑' : '工序机台->新增'}
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
                            <FormItem name="MachineId" label="机台" rules={[{ required: true }]}>
                                <ComboGrid
                                    api="/api/Machine/GetPageList"
                                    itemkey="ID"
                                    param={{
                                        IsActive: true
                                    }}
                                    itemvalue={['MachineNo', 'MachineName']}
                                    disabled={IsView}
                                    onChange={(value, Option, data) => {
                                        let data1 = data[0];
                                        me.formRef.current.setFieldsValue({
                                            StandardMachineTime: data1.StandardMachineTime,
                                            MaxMachineTime: data1.MaxMachineTime,
                                            TimeUnit: data1.TimeUnit
                                        })
                                    }}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="QTY" label="机台数量" rules={[{ required: true }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} defaultValue={1} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="Priority" label="优先级" rules={[{ required: true }]}>
                                <Select
                                    allowClear
                                    showSearch={true}
                                    filterOption={false}
                                    defaultValue={1}
                                // onChange={(e) => {
                                //     this.onChangeValue(e)
                                // }}
                                // value={comboValue}
                                // onPopupScroll={(e) => { this.popuploadData(e) }}
                                // notFoundContent={loading ? <Spin size="small" /> : null}
                                // onSearch={(value) => { this.queryLoadData(value, true) }}
                                // eslint-disable-next-line no-shadow
                                // onChange={(value, Option) => {
                                //     let r = null;
                                //     if (dropDownData && dropDownData.length > 0)
                                //         // eslint-disable-next-line func-names
                                //         r = dropDownData.filter(function (s) {
                                //             return s.ID === value; // 注意：IE9以下的版本没有trim()方法
                                //         });
                                //     onChange(value, Option, r);
                                // }}
                                >
                                    {PriorityList.map(item => {
                                        return (
                                            <Option key={item}>{item}</Option>
                                        )
                                    })}
                                </Select>
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="StandardMachineTime" label="标准加工时间">
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="MaxMachineTime" label="最大加工时间">
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="TimeUnit" label="时间单位">
                                <ComBoBox disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="MachineAccuracy" label="加工精度" >
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="Explantion" label="加工说明">
                                <Input placeholder="请输入" disabled={IsView} />
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
export default connect(({ processmachine }) => ({
    processmachine
}))(FormPage);