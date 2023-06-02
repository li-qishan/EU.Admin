import React, { Component } from 'react';
import { Modal, Form, Row, Col, InputNumber, Input } from 'antd';
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
            MaxCheckQTY: 0
        };
    }
    // componentWillMount() {
    //     const { dispatch, user, Id } = this.props;
    //     if (dispatch && Id) {
    //         this.setState({ Id })
    //         dispatch({
    //             type: 'archeckdetail/getById',
    //             payload: { Id },
    //         }).then((result) => {
    //             if (result.response && me.formRef.current)
    //                 me.formRef.current.setFieldsValue(result.response)
    //         })
    //     }
    // }
    static async getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible && nextProps.modalVisible) {
            const { dispatch, Id, modalVisible } = nextProps;
            me.setState({ Id: Id ? Id : null })
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'archeckdetail/getById',
                    payload: { Id },
                }).then((result) => {
                    if (result.response && me.formRef.current) {
                        me.formRef.current.setFieldsValue(result.response.data);
                        me.setState({ MaxCheckQTY: result.response.extend.MaxCheckQTY })
                    }
                })
            }
        }
    }

    okHandle = async () => {
        const { onSubmit: handleAdd } = this.props;
        const fieldsValue = await me.formRef.current.validateFields();
        // me.formRef.current.resetFields();
        const { Id } = this.state;
        if (Id)
            fieldsValue.ID = Id;
        handleAdd(fieldsValue);
    };
    // onFinish(values) {
    //     const { dispatch } = this.props;
    //     const { Id } = this.state;
    //     if (Id) {
    //         values.ID = Id;
    //     }
    //     dispatch({
    //         type: 'archeckdetail/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, archeckdetail: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel } = this.props;
        const { Id, MaxCheckQTY } = this.state;

        return (
            (<Modal
                destroyOnClose
                title={Id ? '单据明细->编辑' : '单据明细->新增'}
                open={modalVisible}
                onOk={this.okHandle}
                maskClosable={false}
                width={1000}
                onCancel={() => onCancel()}
            >
                {/* <FormToolbar value={{ me }} /> */}
                <Form
                    labelCol={{ span: 8 }}
                    wrapperCol={{ span: 12 }}
                    // onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                >
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="Price" label="单价" rules={[{
                                required: true
                            }, {
                                type: 'number', min: 0, message: '单价必须大于0 !'
                            }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="CheckQTY" label="对账数量" rules={[{
                                required: true
                            }, {
                                type: 'number', min: 0, message: '对账数量必须大于0 !'
                            }, {
                                type: 'number', max: MaxCheckQTY, message: '对账数量必须小于' + MaxCheckQTY + ' !'
                            }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} />
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
export default connect(({ archeckdetail }) => ({
    archeckdetail,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);