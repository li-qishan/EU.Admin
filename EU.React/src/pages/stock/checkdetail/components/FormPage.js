import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, InputNumber, Button } from 'antd';
import { connect } from 'umi';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import ComBoBox from '@/components/SysComponents/ComboBox';
import Utility from '@/utils/utility';

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
    //             type: 'ivcheckdetail/getById',
    //             payload: { Id },
    //         }).then((result) => {
    //             if (result.response && me.formRef.current)
    //                 me.formRef.current.setFieldsValue(result.response)
    //         })
    //     }

    // }
    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible) {
            const { dispatch, modalVisible } = nextProps;
            const { ivcheckdetail: { Id } } = me.props;
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'ivcheckdetail/getById',
                    payload: { Id },
                }).then((result) => {
                    // if (result.response && me.formRef.current)
                    //     me.formRef.current.setFieldsValue(result.response)

                    if (result && me.formRef.current) {
                        Utility.setFormFormat(result);
                        me.formRef.current.setFieldsValue(result);
                        me.setState({ StockId: result.StockId })
                    }
                })
            }
        }
    }

    okHandle = async (value) => {
        const { onSubmit: handleAdd } = this.props;
        const fieldsValue = await me.formRef.current.validateFields();
        // me.formRef.current.resetFields();
        const { ivcheckdetail: { Id } } = me.props;

        if (Id)
            fieldsValue.ID = Id;
        if (value == "newSave")
            handleAdd(fieldsValue, me.onSubmitSuccess);
        else
            handleAdd(fieldsValue);
    };
    onSubmitSuccess() {
        me.formRef.current.resetFields();
        const { dispatch } = me.props;
        dispatch({
            type: 'ivcheckdetail/setId',
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
    //         type: 'ivcheckdetail/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, ivcheckdetail: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel, ivcheckdetail: { Id } } = this.props;

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
                        <Col span={8}>
                            <FormItem name="MaterialId" label="货品编号" rules={[{ required: true }]}>
                                <ComboGrid
                                    api="/api/Material/GetPageList"
                                    itemkey="ID"
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
                            <FormItem name="ActualQTY" label="实际数量" rules={[{
                                required: true
                            }, {
                                type: 'number', min: 0, message: '实际数量必须大于0 !'

                            }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="Price" label="单价" rules={[{ type: 'number', min: 0, message: '单价必须大于0 !' }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} />
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
                                    onChange={(value, Option, data) => {
                                        this.setState({ StockId: value });
                                        me.formRef.current.setFieldsValue({
                                            GoodsLocationId: null,
                                            StockName: data && data.length > 0 ? data[0].StockNames : null
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
                                    disabled={!this.state.StockId ? true : IsView}
                                    parentId={{ 'StockId': this.state.StockId }}
                                    onChange={(value, Option, data) => {
                                        me.formRef.current.setFieldsValue({
                                            GoodsLocationName: data && data.length > 0 ? data[0].LocationNames : null
                                        });
                                    }}
                                />
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
                            <FormItem name="Remark" label="备注">
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}></Col>
                        <Col span={8}></Col>
                        <Col span={8} style={{ display: 'none' }}>
                            <FormItem name="UnitId">
                                <Input />
                            </FormItem>
                            <FormItem name="MaterialName">
                                <Input />
                            </FormItem>
                            <FormItem name="Specifications">
                                <Input />
                            </FormItem>
                            <FormItem name="StockName">
                                <Input />
                            </FormItem>
                            <FormItem name="GoodsLocationName">
                                <Input />
                            </FormItem>
                        </Col>
                    </Row>
                </Form>
            </Modal>)
        );
    }
}
export default connect(({ ivcheckdetail }) => ({
    ivcheckdetail,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);