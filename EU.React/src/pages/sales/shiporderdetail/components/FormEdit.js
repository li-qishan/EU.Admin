import React, { Component } from 'react';
import { Modal, Form, Row, Col, InputNumber } from 'antd';
import { connect } from 'umi';

const FormItem = Form.Item;

let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: ''//从表主键
        };
    }
    // componentWillMount() {
    //     const { dispatch, user, Id } = this.props;
    //     if (dispatch && Id) {
    //         this.setState({ Id })
    //         dispatch({
    //             type: 'shiporderdetail/getById',
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
                    type: 'shiporderdetail/getById',
                    payload: { Id },
                }).then((result) => {
                    if (result.response && me.formRef.current)
                        me.formRef.current.setFieldsValue(result.response)
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
    //         type: 'shiporderdetail/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, shiporderdetail: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel } = this.props;
        const { Id } = this.state;

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
                        <Col span={12}>
                            <FormItem name="ShipQTY" label="发货数量" rules={[{
                                required: true
                            }, {
                                type: 'number', min: 0, message: '发货数量最小值为0 !'
                            }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            {/* <FormItem name="CustomerMaterialCode" label="客户物料编码">
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem> */}
                        </Col>
                    </Row>
                </Form>
            </Modal>)
        );
    }
}
export default connect(({ shiporderdetail }) => ({
    shiporderdetail,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);