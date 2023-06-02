import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, Select, Button, InputNumber } from 'antd';
import { connect } from 'umi';
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
            moduleCode: "PD_RETURN_ORDER_DETAIL_MNG"
        };
    }
    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible) {
            const { dispatch, modalVisible } = nextProps;
            const { pdreturndetail: { Id } } = me.props;
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'pdreturndetail/getById',
                    payload: { Id },
                }).then((result) => {
                    if (result && me.formRef.current) {
                        Utility.setFormFormat(result);
                        me.setState({ StockId: result.StockId })
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
        const { pdreturndetail: { Id } } = me.props;
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
            type: 'pdreturndetail/setId',
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
    //         type: 'pdreturndetail/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, pdreturndetail: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel, pdreturndetail: { Id } } = this.props;

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
                            <FormItem name="ReturnQTY" label="补发数量" rules={[{ required: true }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="StockId" label="仓库">
                                <ComboGrid
                                    api="/api/Stock/GetPageList"
                                    itemkey="ID"
                                    itemvalue="StockNames"
                                    disabled={IsView}
                                    onChange={(value) => {
                                        if (value != this.state.StockId) 
                                            me.formRef.current.setFieldsValue({ 'GoodsLocationId': null });
                                        
                                        this.setState({ StockId: value })
                                    }}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="GoodsLocationId" label="货位">
                                <ComboGrid
                                    api="/api/GoodsLocation/GetPageList"
                                    itemkey="ID"
                                    itemvalue="LocationNames"
                                    disabled={!this.state.StockId ? true : IsView}
                                    parentId={{ 'StockId': this.state.StockId }}
                                // onChange={(value) => {
                                // }}
                                />
                            </FormItem>
                        </Col>
                    </Row>
                </Form>
            </Modal>)
        );
    }
}
export default connect(({ pdreturndetail }) => ({
    pdreturndetail,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);