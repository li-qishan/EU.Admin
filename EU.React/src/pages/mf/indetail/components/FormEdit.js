import React, { Component } from 'react';
import { Modal, Form, Row, Col, InputNumber } from 'antd';
import { connect } from 'umi';
import ComboGrid from '@/components/SysComponents/ComboGrid';

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
    //             type: 'mfinorderdetail/getById',
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
                    type: 'mfinorderdetail/getById',
                    payload: { Id },
                }).then((result) => {
                    if (result.response && me.formRef.current) {
                        me.formRef.current.setFieldsValue(result.response)
                        me.setState({ StockId: result.response.StockId });
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
    //         type: 'mfinorderdetail/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, mfinorderdetail: { Id } } = me.props;
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
                    wrapperCol={{ span: 16 }}
                    // onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                >
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="MouldId" label="模具">
                                <ComboGrid
                                    api="/api/Mould/GetPageList"
                                    itemkey="ID"
                                    itemvalue={['MouldNo', 'MouldName']}
                                    disabled={IsView}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="CurrencyId" label="币别">
                                <ComboGrid
                                    api="/api/Currency/GetPageList"
                                    itemkey="ID"
                                    itemvalue={['CurrencyNo', 'CurrencyName']}
                                    disabled={IsView}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="QTY" label="数量" >
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>

                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="Price" label="价格" >
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="StockId" label="仓库">
                                <ComboGrid
                                    api="/api/Stock/GetPageList"
                                    itemkey="ID"
                                    itemvalue={['StockNo', 'StockNames']}
                                    disabled={IsView}
                                    onChange={(value) => {
                                        if (value != this.state.StockId) {
                                            me.formRef.current.setFieldsValue({ 'GoodsLocationId': null });
                                        }
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
                                    itemvalue=""
                                    itemvalue={['LocationNo', 'LocationNames']}
                                    disabled={IsView}
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
export default connect(({ mfinorderdetail }) => ({
    mfinorderdetail,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);