import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, Button, InputNumber, DatePicker } from 'antd';
import { connect } from 'umi';
import Utility from '@/utils/utility';
const FormItem = Form.Item;
let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            moduleCode: "PO_ORDER_PREPAYMENT_MNG"
        };
    }
    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible) {
            const { dispatch, modalVisible } = nextProps;
            const { poprepayment: { Id } } = me.props;
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'poprepayment/getById',
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
        const { poprepayment: { Id } } = me.props;
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
            type: 'poprepayment/setId',
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
    //         type: 'poprepayment/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, poprepayment: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel, poprepayment: { Id } } = this.props;

        return (
            (<Modal
                destroyOnClose
                title={Id ? '工序人员->编辑' : '工序人员->新增'}
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
                            <FormItem name="Percent" label="预付比例" rules={[{
                                required: true
                            }, {
                                type: 'number', min: 0, message: '预付比例必须大于0 !'
                            }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="Amount" label="预付金额" rules={[{
                                required: true
                            }, {
                                type: 'number', min: 0, message: '预付金额必须大于0 !'
                            }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="PayTime" label="预付时间" rules={[{ required: true }]}>
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
                        <Col span={16}>
                        </Col>
                    </Row>
                </Form>
            </Modal>)
        );
    }
}
export default connect(({ poprepayment }) => ({
    poprepayment
}))(FormPage);