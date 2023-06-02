import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, InputNumber, Button } from 'antd';
import { connect } from 'umi';

const FormItem = Form.Item;
let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
    }

    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible) {
            const { goodslocation: { Id } } = me.props;
            const { dispatch, modalVisible } = nextProps;
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'goodslocation/getById',
                    payload: { Id }
                }).then((result) => {
                    if (result.response && me.formRef.current)
                        me.formRef.current.setFieldsValue(result.response.data)
                })
            }
        }
    }

    okHandle = async (value) => {
        const { onSubmit: handleAdd } = this.props;
        const fieldsValue = await me.formRef.current.validateFields();
        // me.formRef.current.resetFields();
        const { goodslocation: { Id } } = this.props;
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
            type: 'goodslocation/setId',
            payload: ''
            // }).then(() => {
            //     me.setState({ detailVisible, DetailId, detailView });
        });
    }
    render() {
        const { modalVisible, IsView, onCancel, goodslocation: { Id } } = this.props;

        return (
            (<Modal
                destroyOnClose
                title={Id ? '货位->编辑' : '货位->新增'}
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
                    labelCol={{ span: 8 }}
                    wrapperCol={{ span: 12 }}
                    // onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                >
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="LocationNo" label="货位编号" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="LocationNames" label="货位名称" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="LocationCapacity" label="货位容量">
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}></Col>
                    </Row>
                </Form>
            </Modal>)
        );
    }
}
export default connect(({ goodslocation }) => ({
    goodslocation
}))(FormPage);