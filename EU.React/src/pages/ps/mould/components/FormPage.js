import React, { Component } from 'react';
import { Modal, Input, Form, Row, Col, Select, Button, InputNumber } from 'antd';
import { connect } from 'umi';
import ComBoBox from '@/components/SysComponents/ComboBox';
import Utility from '@/utils/utility';
import ComboGrid from '@/components/SysComponents/ComboGrid';
const FormItem = Form.Item;
const { Option } = Select;
let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            moduleCode: "PS_PROCESS_BOM_MOULD_MNG"
        };
    }
    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible) {
            const { dispatch, modalVisible } = nextProps;
            const { psmould: { Id } } = me.props;
            if (dispatch && Id && modalVisible) {
                dispatch({
                    type: 'psmould/getById',
                    payload: { Id },
                }).then((result) => {
                    if (result && me.formRef.current) {
                        Utility.setFormFormat(result);
                        me.formRef.current.setFieldsValue(result)
                    }
                })
            }
        }
    }

    okHandle = async (value) => {
        const { onSubmit: handleAdd } = this.props;
        const fieldsValue = await me.formRef.current.validateFields();
        // me.formRef.current.resetFields();
        const { psmould: { Id } } = me.props;
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
            type: 'psmould/setId',
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
    //         type: 'psmould/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, psmould: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel, psmould: { Id } } = this.props;

        return (
            (<Modal
                destroyOnClose
                title={Id ? '模具->编辑' : '模具->新增'}
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
                    labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                    wrapperCol={{ span: 16 }}
                    // onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                >
                    <Row gutter={24} justify={"center"}>

                        <Col span={8}>
                            <FormItem name="MouldId" label="模具" rules={[{ required: true }]}>
                                <ComboGrid
                                    api="/api/Mould/GetPageList"
                                    itemkey="ID"
                                    itemvalue={['MouldNo', 'MouldName']}
                                    disabled={IsView}
                                />
                            </FormItem>

                        </Col>
                        <Col span={8}>
                            <FormItem name="ProcessId" label="工序" rules={[{ required: true }]}>
                                <ComboGrid
                                    api="/api/Process/GetPageList"
                                    itemkey="ID"
                                    param={{
                                        IsActive: true
                                    }}
                                    itemvalue={['ProcessNo', 'ProcessName']}
                                    disabled={IsView}
                                />
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
export default connect(({ psmould }) => ({
    psmould,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);