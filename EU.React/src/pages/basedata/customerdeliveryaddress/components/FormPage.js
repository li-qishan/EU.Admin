import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, Switch, Button } from 'antd';
import { connect } from 'umi';

const FormItem = Form.Item;
let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',//从表主键
            moduleCode: "BD_CUSTOMER_CONTACT_MNG",
            IsDefault: 'true'
        };
    }
    // componentWillMount() {
    //     const { dispatch, user, Id } = this.props;
    //     if (dispatch && Id) {
    //         this.setState({ Id })
    //         dispatch({
    //             type: 'customerdeliveryaddress/getById',
    //             payload: { Id },
    //         }).then((result) => {
    //             if (result.response && me.formRef.current)
    //                 me.formRef.current.setFieldsValue(result.response)
    //         })
    //     }

    // }
    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible) {
            const { customerdeliveryaddress: { Id } } = me.props;
            const { dispatch, modalVisible } = nextProps;
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'customerdeliveryaddress/getById',
                    payload: { Id },
                }).then((result) => {
                    if (result.response && me.formRef.current) {
                        me.formRef.current.setFieldsValue(result.response)
                        me.setState({ IsDefault: result.response.IsDefault ? 'true' : 'false' })
                    }
                })
            }
        }
    }
    okHandle = async (value) => {
        const { onSubmit: handleAdd } = this.props;
        const fieldsValue = await me.formRef.current.validateFields();
        // me.formRef.current.resetFields();
        const { customerdeliveryaddress: { Id } } = this.props;

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
            type: 'customerdeliveryaddress/setId',
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
    //         type: 'customerdeliveryaddress/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, customerdeliveryaddress: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel, customerdeliveryaddress: { Id } } = this.props;

        return (
            (<Modal
                destroyOnClose
                title={Id ? '送货地址->编辑' : '送货地址->新增'}
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
                {/* <FormToolbar value={{ me }} disabled={IsView} /> */}
                <Form
                    labelCol={{ span: 8 }}
                    wrapperCol={{ span: 12 }}
                    // onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                // initialValues={{
                //     IsDefault: true
                // }}
                >
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="Contact" label="负责人" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="Phone" label="手机" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>

                        <Col span={12}>
                            <FormItem name="Telephone" label="电话" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="Address" label="地址" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            {Id ? <FormItem name="IsDefault" valuePropName='checked' label="是否默认">
                                <Switch disabled={IsView} />
                            </FormItem> : <FormItem name="IsDefault" valuePropName='checked' label="是否默认" initialValue={true}>
                                <Switch defaultChecked disabled={IsView} />
                            </FormItem>}
                        </Col>
                        <Col span={12}></Col>
                    </Row>
                </Form>
            </Modal >)
        );
    }
}
export default connect(({ customerdeliveryaddress }) => ({
    customerdeliveryaddress,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);