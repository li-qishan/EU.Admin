import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, InputNumber, Switch } from 'antd';
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
    //             type: 'importconfigdetail/getById',
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
                type: 'importconfigdetail/getById',
                payload: { Id },
            }).then((result) => {
                // if (result.response && me.formRef.current)
                //     me.formRef.current.setFieldsValue(result.response)

                if (result.response && me.formRef.current) {
                    Utility.setFormFormat(result.response);
                    me.formRef.current.setFieldsValue(result.response);
                    me.setState({ TableCode: result.response.CorresTableCode })
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
    //         type: 'importconfigdetail/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, importconfigdetail: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel } = this.props;
        const { Id, TableCode } = this.state;

        return (
            (<Modal
                destroyOnClose
                title={Id ? '模板明细->编辑' : '模板明细->新增'}
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
                            <FormItem name="ColumnNo" label="Execl列号" rules={[{ required: true }]}>
                                <ComBoBox disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="ColumnCode" label="列名称" rules={[{
                                required: true
                            }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem valuePropName='checked' name="IsUnique" label="是否唯一">
                                <Switch disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>

                        <Col span={8}>
                            <FormItem valuePropName='checked' name="IsInsert" label="是否插入">
                                <Switch disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="DateFormate" label="格式">
                                <ComBoBox disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="MaxLength" label="最大长度">
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>

                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem valuePropName='checked' name="IsAllowNull" label="允许为空">
                                <Switch disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem valuePropName='checked' name="IsEncrypt" label="加密">
                                <Switch disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="LovCode" label="参数代码">
                                <ComboGrid
                                    api="/api/SmLov/GetPageList"
                                    itemkey="LovCode"
                                    itemvalue="LovCode"
                                    // onChange={(value, Option, data) => {
                                    //     me.formRef.current.setFieldsValue({
                                    //         'MaterialName': data[0].MaterialNames,
                                    //         'UnitId': data[0].UnitId,
                                    //         'Specifications': data[0].Specifications
                                    //     });
                                    // }}
                                    disabled={IsView}
                                />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="CorresTableCode" label="映射表">
                                <ComboGrid
                                    api="/api/SmTableCatalog/GetPageList"
                                    itemkey="TableCode"
                                    itemvalue="TableCode"
                                    onChange={(TableCode, Option, data) => {
                                        this.setState({ TableCode })
                                        me.formRef.current.setFieldsValue({
                                            'CorresColumnCode': null,
                                            'TransColumnCode': null
                                        });
                                    }}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="CorresColumnCode" label="映射字段">
                                <ComboGrid
                                    api="/api/SmFieldCatalog/GetPageList"
                                    itemkey="ColumnCode"
                                    disabled={!TableCode ? true : IsView}
                                    itemvalue="ColumnCode"
                                    parentId={{ 'TableCode': TableCode }}
                                // onChange={(tableCode, Option, data) => {
                                //     this.setState({ tableCode })
                                // }}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="TransColumnCode" label="转换字段">
                                <ComboGrid
                                    api="/api/SmFieldCatalog/GetPageList"
                                    itemkey="ColumnCode"
                                    itemvalue="ColumnCode"
                                    disabled={!TableCode ? true : IsView}
                                    parentId={{ 'TableCode': TableCode }}
                                // onChange={(tableCode, Option, data) => {
                                //     this.setState({ tableCode })
                                // }}
                                />
                            </FormItem>
                        </Col>

                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="Remark" label="备注">
                                <Input disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                        </Col>
                        <Col span={8}>
                        </Col>
                    </Row>
                </Form>
            </Modal>)
        );
    }
}
export default connect(({ importconfigdetail }) => ({
    importconfigdetail,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);