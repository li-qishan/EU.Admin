import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, InputNumber, Spin, Card, Skeleton, Button } from 'antd';
import { connect } from 'umi';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import Utility from '@/utils/utility';

const FormItem = Form.Item;

let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            isLoading: true,
            disabled: true
        };
    }
    // componentWillMount() {
    //     const { dispatch, user, Id } = this.props;
    //     if (dispatch && Id) {
    //         this.setState({ Id })
    //         dispatch({
    //             type: 'ivinitdetail/getById',
    //             payload: { Id },
    //         }).then((result) => {
    //             if (result && me.formRef.current)
    //                 me.formRef.current.setFieldsValue(result)
    //         })
    //     }

    // }
    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible) {
            const { ivinitdetail: { Id } } = me.props;

            const { dispatch, modalVisible, IsView } = nextProps;
            me.setState({ disabled: IsView ? true : false, isLoading: true })
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'ivinitdetail/getById',
                    payload: { Id },
                }).then((result) => {
                    // if (result && me.formRef.current)
                    //     me.formRef.current.setFieldsValue(result)

                    if (result && me.formRef.current) {
                        Utility.setFormFormat(result);
                        me.formRef.current.setFieldsValue(result);
                        me.setState({ isLoading: false, StockId: result.StockId })
                    }
                })
            } else me.setState({ isLoading: false })
        }
    }

    okHandle = async (value) => {
        const { onSubmit: handleAdd } = this.props;
        const fieldsValue = await me.formRef.current.validateFields();
        // me.formRef.current.resetFields();
        const { ivinitdetail: { Id } } = this.props;
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
        me.setState({ StockId: null });
        dispatch({
            type: 'ivinitdetail/setId',
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
    //         type: 'ivinitdetail/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, ivinitdetail: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel, ivinitdetail: { Id } } = this.props;
        const { isLoading, StockId } = this.state;

        return (
            (<Modal
                destroyOnClose
                title={Id ? '单据明细->编辑' : '单据明细->新增'}
                open={modalVisible}
                // onOk={this.okHandle}
                maskClosable={false}
                width={1200}
                onCancel={() => onCancel()}
                footer={[
                    <Button key="submit" disabled={IsView} type="primary" onClick={this.okHandle}>
                        保存
                    </Button>,
                    // <Button
                    //     key="submit1"
                    //     type="primary" disabled={IsView}
                    //     onClick={() => {
                    //         me.okHandle('newSave');
                    //     }}
                    // >
                    //     保存并新建
                    // </Button>,
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
                    {isLoading ? <Card>
                        <Skeleton active />
                        <div style={{ textAlign: 'center', padding: 20 }}>
                            <Spin size="large" />
                        </div>
                    </Card> : <>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="MaterialId" label="货品" rules={[{ required: true }]}>
                                    <ComboGrid
                                        api="/api/Material/GetPageList"
                                        itemkey="ID"
                                        // itemvalue="MaterialNames"
                                        itemvalue={['MaterialNo', 'MaterialNames']}
                                        onChange={(value, Option, data) => {
                                            me.formRef.current.setFieldsValue({
                                                'MaterialName': data.length > 0 ? data[0].MaterialNames : null,
                                                'UnitId': data.length > 0 ? data[0].UnitId : null,
                                                'Specifications': data.length > 0 ? data[0].Specifications : null
                                            });
                                        }}
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="QTY" label="数量" rules={[{
                                    required: true
                                }, {
                                    type: 'number', min: 0, message: '数量必须大于0 !'
                                }]}>
                                    <InputNumber placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="BatchNo" label="批次号">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>

                            <Col span={8}>
                                <FormItem name="StockId" label="仓库" rules={[{ required: true }]}>
                                    <ComboGrid
                                        api="/api/Stock/GetPageList"
                                        itemkey="ID"
                                        itemvalue="StockNames"
                                        disabled={IsView}
                                        onChange={(value) => {
                                            this.setState({ StockId: value })
                                            me.formRef.current.setFieldsValue({
                                                GoodsLocationId: null
                                            });
                                        }}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="GoodsLocationId" label="货位" rules={[{ required: true }]}>
                                    <ComboGrid
                                        api="/api/GoodsLocation/GetPageList"
                                        itemkey="ID"
                                        itemvalue="LocationNames"
                                        disabled={!StockId ? true : IsView}
                                        parentId={{ 'StockId': StockId }}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                            </Col>
                        </Row>
                    </>}
                </Form>
            </Modal>)
        );
    }
}
export default connect(({ ivinitdetail }) => ({
    ivinitdetail
}))(FormPage);