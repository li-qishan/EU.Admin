import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, InputNumber, Spin, Skeleton, Card } from 'antd';
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
            Id: '',//从表主键
            isLoading: true
        };
    }
    // componentWillMount() {
    //     const { dispatch, user, Id } = this.props;
    //     if (dispatch && Id) {
    //         this.setState({ Id })
    //         dispatch({
    //             type: 'salesorderdetail/getById',
    //             payload: { Id },
    //         }).then((result) => {
    //             if (result.response && me.formRef.current)
    //                 me.formRef.current.setFieldsValue(result.response)
    //         })
    //     }

    // }
    componentWillReceiveProps(nextProps) {
        me.setState({ isLoading: true })
        const { dispatch, Id, modalVisible } = nextProps;
        this.setState({ Id: Id ? Id : null })
        if (dispatch && Id && modalVisible) {
            dispatch({
                type: 'salesorderdetail/getById',
                payload: { Id },
            }).then((result) => {
                me.setState({ isLoading: false })
                if (result.response && me.formRef.current)
                    me.formRef.current.setFieldsValue(result.response)
            })
        } else
            me.setState({ isLoading: false })
    }

    okHandle = async () => {
        const { onSubmit: handleAdd } = this.props;
        // me.formRef.current.resetFields();
        const { Id } = this.state;
        if (Id) {
            const fieldsValue = await me.formRef.current.validateFields();
            fieldsValue.ID = Id;
            handleAdd(fieldsValue, Id);
        }
    };
    // onFinish(values) {
    //     const { dispatch } = this.props;
    //     const { Id } = this.state;
    //     if (Id) {
    //         values.ID = Id;
    //     }
    //     dispatch({
    //         type: 'salesorderdetail/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, salesorderdetail: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel } = this.props;
        const { Id, isLoading } = this.state;

        return (
            (<Modal
                destroyOnClose
                title={Id ? '单据明细->编辑' : '单据明细->新增'}
                open={modalVisible}
                onOk={this.okHandle}
                maskClosable={false}
                width={1000}
                onCancel={() => onCancel()}
                okButtonProps={{ disabled: IsView }}
            >
                <Form
                    labelCol={{ span: 8 }}
                    wrapperCol={{ span: 12 }}
                    // onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                >
                    {isLoading ? <Card>
                        <Skeleton active />
                        <div style={{ textAlign: 'center', padding: 20 }}>
                            <Spin size="large" />
                        </div>
                    </Card> :
                        <>
                            <Row gutter={24} justify={"center"}>
                                <Col span={12}>
                                    <FormItem name="MaterialId" label="货品编号" rules={[{ required: true }]}>
                                        <ComboGrid
                                            api="/api/Material/GetPageList"
                                            itemkey="ID"
                                            param={{
                                                IsActive: true
                                            }}
                                            // itemvalue="MaterialNo,MaterialNames"
                                            itemvalue={['MaterialNo', 'MaterialNames']}
                                            // onChange={(value) => {
                                            //     this.setState({ StockKeeperId: value })
                                            // }}
                                            disabled={IsView}
                                        />
                                    </FormItem>
                                </Col>
                                <Col span={12}>
                                    <FormItem name="QTY" label="数量" rules={[{
                                        required: true
                                    }, {
                                        type: 'number', min: 0, message: '数量最小值为0 !'
                                    }]}>
                                        <InputNumber placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                            </Row>
                            <Row gutter={24} justify={"center"}>
                                <Col span={12}>
                                    <FormItem name="Price" label="单价" rules={[{ required: true }, { type: 'number', min: 0, message: '单价最小值为0 !' }]}>
                                        <InputNumber placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={12}>
                                    <FormItem name="CustomerMaterialCode" label="客户物料编码">
                                        <Input placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                            </Row>
                        </>
                    }
                </Form>
            </Modal>)
        );
    }
}
export default connect(({ salesorderdetail }) => ({
    salesorderdetail,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);