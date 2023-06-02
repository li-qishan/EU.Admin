import React, { Component, Suspense } from 'react';
import { connect } from 'umi';
import { Button, Modal, Tree, Dropdown, Menu, message, DatePicker, Input, Card, Form, Row, Col, Select, Space, Switch, InputNumber, Tabs, Tag, Popconfirm } from 'antd';

const FormItem = Form.Item;

let me;
class DetailForm extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: ''
        };
    }
    componentWillMount() {

    }

    componentWillReceiveProps(nextProps) {
        const { dispatch, user, Id, modalVisible } = nextProps;
        if (dispatch && Id && modalVisible) {
            this.setState({ Id })
            dispatch({
                type: 'setparam/getDetailById',
                payload: { Id },
            }).then((result) => {
                if (result.response && me.formRef.current)
                    me.formRef.current.setFieldsValue(result.response)
            })
        }
    }

    okHandle = async () => {
        const { modalVisible, onSubmit: handleAdd } = this.props;
        const fieldsValue = await me.formRef.current.validateFields();
        // me.formRef.current.resetFields();
        handleAdd(fieldsValue);
    };

    render() {
        const { modalVisible, IsView, onCancel } = this.props;
        return (<>
            <Modal
                destroyOnClose
                title="参数明细"
                open={modalVisible}
                onOk={this.okHandle}
                maskClosable={false}
                width={1000}
                onCancel={() => onCancel()}
            >
                <Form
                    labelCol={{ span: 8 }}
                    wrapperCol={{ span: 12 }}
                    onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                >
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="TaxisNo" label="排序号" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="Value" label="参数值" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="Text" label="参数名称" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                        </Col>
                    </Row>
                </Form>
            </Modal>
        </>);
    }
}
export default connect(({ setparam, loading }) => ({
    setparam,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(DetailForm);