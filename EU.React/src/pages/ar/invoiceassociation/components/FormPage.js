import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, Button, InputNumber, DatePicker } from 'antd';
import { connect } from 'umi';
import Utility from '@/utils/utility';
import dayjs from 'dayjs';
const FormItem = Form.Item;
let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            moduleCode: "AR_INVOICE_ASSOCIATION_MNG"
        };
    }
    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible) {
            const { dispatch, modalVisible } = nextProps;
            const { arinvoiceassociation: { Id } } = me.props;
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'arinvoiceassociation/getById',
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
        const { arinvoiceassociation: { Id } } = me.props;
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
            type: 'arinvoiceassociation/setId',
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
    //         type: 'arinvoiceassociation/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, arinvoiceassociation: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel, arinvoiceassociation: { Id } } = this.props;

        return (
            (<Modal
                destroyOnClose
                title={Id ? '对应发票->编辑' : '对应发票->新增'}
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
                            <FormItem name="InvoiceNo" label="发票号码" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="InvoiceDate" label="发票日期">
                                <DatePicker
                                    disabled={IsView} defaultValue={dayjs()}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="TaxRate" label="发票税率">
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="NoTaxAmount" label="未税金额">
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="TaxAmount" label="税额">
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="TaxIncludedAmount" label="含税金额">
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
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
                </Form>
            </Modal>)
        );
    }
}
export default connect(({ arinvoiceassociation }) => ({
    arinvoiceassociation
}))(FormPage);
