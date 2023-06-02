import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, InputNumber } from 'antd';
import { connect } from 'umi';
import FormToolbar from '@/components/SysComponents/FormToolbar';
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
            Id: ''//从表主键
        };
    }
    // componentWillMount() {
    //     const { dispatch, user, Id } = this.props;
    //     if (dispatch && Id) {
    //         this.setState({ Id })
    //         dispatch({
    //             type: 'smtabledetail/getById',
    //             payload: { Id },
    //         }).then((result) => {
    //             if (result.response && me.formRef.current)
    //                 me.formRef.current.setFieldsValue(result.response)
    //         })
    //     }

    // }
    componentWillReceiveProps(nextProps) {
        const { dispatch, Id, modalVisible } = nextProps;
        this.setState({ Id: Id ? Id : null })
        if (dispatch && Id && modalVisible) {
            dispatch({
                type: 'smtabledetail/getById',
                payload: { Id },
            }).then((result) => {
                // if (result.response && me.formRef.current)
                //     me.formRef.current.setFieldsValue(result.response)

                if (result.response && me.formRef.current) {
                    Utility.setFormFormat(result.response);
                    me.formRef.current.setFieldsValue(result.response);
                    me.setState({ StockId: result.response.StockId })
                }
            })
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
    //         type: 'smtabledetail/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, smtabledetail: { Id } } = me.props;
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
                width={1200}
                onCancel={() => onCancel()}
            >
                <FormToolbar value={{ me }} />
                <Form
                    labelCol={{ span: 8 }}
                    wrapperCol={{ span: 12 }}
                    // onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                >
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="MaterialId" label="物料" rules={[{ required: true }]}>
                                <ComboGrid
                                    api="/api/Material/GetPageList"
                                    itemkey="ID"
                                    itemvalue="MaterialNames"
                                    onChange={(value, Option, data) => {
                                        me.formRef.current.setFieldsValue({
                                            'MaterialName': data[0].MaterialNames,
                                            'UnitId': data[0].UnitId,
                                            'Specifications': data[0].Specifications
                                        });
                                    }}
                                    disabled={IsView}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="QTY" label="入库数量" rules={[{
                                required: true
                            }, {
                                type: 'number', min: 1, message: '入库数量最小值为0 !'
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
                            <FormItem name="StockId" label="仓库">
                                <ComboGrid
                                    api="/api/Stock/GetPageList"
                                    itemkey="ID"
                                    itemvalue="StockNames"
                                    disabled={IsView}
                                    onChange={(value, Option, data) => {
                                        me.formRef.current.setFieldsValue({
                                            'StockName': data[0].StockNames,
                                            'GoodsLocationId': null
                                        });
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
                                    itemvalue="LocationNames"
                                    disabled={!this.state.StockId ? true : IsView}
                                    parentId={{ 'StockId': this.state.StockId }}
                                    onChange={(value, Option, data) => {
                                        me.formRef.current.setFieldsValue({
                                            'GoodsLocationName': data[0].LocationNames
                                        });
                                    }}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="Remark" label="备注">
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
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
export default connect(({ smtabledetail }) => ({
    smtabledetail,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);