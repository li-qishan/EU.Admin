import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, InputNumber, Button } from 'antd';
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
            StockId: null,
        };
    }
    // componentWillMount() {
    //     const { dispatch, user, Id } = this.props;
    //     if (dispatch && Id) {
    //         this.setState({ Id })
    //         dispatch({
    //             type: 'ivotherindetail/getById',
    //             payload: { Id },
    //         }).then((result) => {
    //             if (result.response && me.formRef.current)
    //                 me.formRef.current.setFieldsValue(result.response)
    //         })
    //     }

    // }
    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible) {
            const { ivotherindetail: { Id } } = me.props;
            const { dispatch, modalVisible } = nextProps;
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'ivotherindetail/getById',
                    payload: { Id },
                }).then((result) => {
                    // if (result.response && me.formRef.current)
                    //     me.formRef.current.setFieldsValue(result.response)
                    let GoodsLocationName = result.GoodsLocationName;
                    if (result && me.formRef.current) {
                        Utility.setFormFormat(result);
                        result.GoodsLocationName = GoodsLocationName;
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
        const { ivotherindetail: { Id } } = me.props;
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
            type: 'ivotherindetail/setId',
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
    //         type: 'ivotherindetail/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, ivotherindetail: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel, ivotherindetail: { Id } } = this.props;
        const { StockId } = this.state;

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
                                    itemvalue={['MaterialNo', 'MaterialNames']}
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
                                type: 'number', min: 0, message: '入库数量最小值为0 !'
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
                                            'StockName': data && data.length > 0 ? data[0].StockNames : null,
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
                                    disabled={!StockId ? true : IsView}
                                    parentId={{ 'StockId': StockId }}
                                    onChange={(value, Option, data) => {
                                        me.formRef.current.setFieldsValue({
                                            'GoodsLocationName': data && data.length > 0 ? data[0].LocationNames : null
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
export default connect(({ ivotherindetail }) => ({
    ivotherindetail,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);