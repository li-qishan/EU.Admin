import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, Button, InputNumber, DatePicker } from 'antd';
import { connect } from 'umi';
import Utility from '@/utils/utility';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import dayjs from 'dayjs';

const FormItem = Form.Item;
let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            moduleCode: "AP_INIT_ACCOUNT_ORDER_DETAIL_MNG"
        };
    }
    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible && nextProps.modalVisible) {
            const { dispatch, modalVisible } = nextProps;
            const { apinitaccountdetail: { Id } } = me.props;
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'apinitaccountdetail/getById',
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
        const { apinitaccountdetail: { Id }, apinitaccount: { detail: { extend: { TaxRate } } } } = me.props;
        if (Id)
            fieldsValue.ID = Id;
        if (!fieldsValue.TaxRate && fieldsValue.TaxRate != 0)
            fieldsValue.TaxRate = TaxRate;
        if (value == "newSave")
            handleAdd(fieldsValue, me.onSubmitSuccess);
        else
            handleAdd(fieldsValue);
    };
    onSubmitSuccess() {
        const { dispatch } = me.props;
        me.formRef.current.resetFields();
        dispatch({
            type: 'apinitaccountdetail/setId',
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
    //         type: 'apinitaccountdetail/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, apinitaccountdetail: { Id } } = me.props;
    //         this.setState({ Id })
    //     });apinitaccount detail:{extend:TaxRate }
    // }
    render() {
        const { modalVisible, IsView, onCancel, apinitaccountdetail: { Id }, apinitaccount: { detail: { extend: { TaxRate } } } } = this.props;

        return (
            (<Modal
                destroyOnClose
                title={Id ? '材料->编辑' : '材料->新增'}
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
                            <FormItem name="OrderNo" label="单据编号">
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="OrderName" label="单据名称">
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="OrderDate" label="单据日期">
                                <DatePicker defaultValue={dayjs()}
                                    disabled={IsView}
                                />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="MaterialId" label="物料" >
                                <ComboGrid
                                    api="/api/Material/GetPageList"
                                    itemkey="ID"
                                    param={{
                                        IsActive: true
                                    }}
                                    itemvalue={['MaterialNo', 'MaterialNames']}
                                    disabled={IsView}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="QTY" label="数量" rules={[{ required: true }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="Price" label="单价" rules={[{ required: true }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="TaxRate" label="税率">
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} defaultValue={TaxRate} />
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
                </Form>
            </Modal>)
        );
    }
}
export default connect(({ apinitaccountdetail, apinitaccount }) => ({
    apinitaccountdetail, apinitaccount
}))(FormPage);
