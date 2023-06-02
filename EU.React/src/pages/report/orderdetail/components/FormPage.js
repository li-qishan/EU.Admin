import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, Button, InputNumber } from 'antd';
import { connect } from 'umi';
import Utility from '@/utils/utility';
import ComboGrid from '@/components/SysComponents/ComboGrid';
const FormItem = Form.Item;
let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            moduleCode: "RM_FEE_ORDER_DETAIL_MNG"
        };
    }
    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible && nextProps.modalVisible) {
            const { dispatch, modalVisible } = nextProps;
            const { rmorderdetail: { Id } } = me.props;
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'rmorderdetail/getById',
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
        const { rmorderdetail: { Id } } = me.props;
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
            type: 'rmorderdetail/setId',
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
    //         type: 'rmorderdetail/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, rmorderdetail: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel, rmorderdetail: { Id } } = this.props;
        let { TypeId } = this.state;

        return (
            (<Modal
                destroyOnClose
                title={Id ? '费用明细->编辑' : '费用明细->新增'}
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
                            <FormItem name="TypeId" label="类型">
                                <ComboGrid
                                    api="/api/RmType/GetPageList"
                                    itemkey="ID"
                                    itemvalue="TypeName"
                                    disabled={IsView}
                                    onChange={(value) => {
                                        this.setState({ TypeId: value })
                                        me.formRef.current.setFieldsValue({
                                            ItemId: null
                                        });
                                    }}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="ItemId" label="项目">
                                <ComboGrid
                                    api="/api/RmItem/GetPageList"
                                    itemkey="ID"
                                    // param={{
                                    //     IsActive: true
                                    // }}
                                    itemvalue={['ItemCode', 'ItemName']}
                                    disabled={!TypeId ? true : IsView}
                                    parentId={{ 'TypeId': TypeId }}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>  <FormItem name="Cost" label="成本" rules={[{ required: true }]}>
                            <InputNumber placeholder="请输入" disabled={IsView} min={0} />
                        </FormItem></Col>
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
                </Form>
            </Modal>)
        );
    }
}
export default connect(({ rmorderdetail }) => ({
    rmorderdetail
}))(FormPage);