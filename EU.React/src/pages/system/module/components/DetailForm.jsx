import React, { Component } from 'react';
import { connect } from 'umi';
import { Modal, Input, Form, Row, Col, Switch, InputNumber, Select } from 'antd';
const { Option } = Select;
const FormItem = Form.Item;

let me;
class DetailForm extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: ''
        };
    }

    componentWillReceiveProps(nextProps) {
        const { dispatch, Id, modalVisible } = nextProps;
        if (dispatch && Id && modalVisible)
            dispatch({
                type: 'module/getDetailById',
                payload: { Id },
            }).then((result) => {
                if (result.response && me.formRef.current)
                    me.formRef.current.setFieldsValue(result.response)
            })
        else {
            // me.formRef.current.setFieldsValue({
            //     TableAlias: 'A'
            // })
            me.setState({ Id: '' })
        }
    }

    okHandle = async () => {
        const { onSubmit: handleAdd } = this.props;
        const { Id } = this.state;
        const fieldsValue = await me.formRef.current.validateFields();
        // me.formRef.current.resetFields();
        if (Id)
            fieldsValue.ID = Id;
        handleAdd(fieldsValue);
    };
    handleDataTypeChange(value) {
        if (value == "Date")
            me.formRef.current.setFieldsValue({ DataFormate: 'Y/m/d H:i:s' })
        else
            me.formRef.current.setFieldsValue({ DataFormate: null })
    }
    render() {
        const { modalVisible, IsView, onCancel } = this.props;
        return (<>
            <Modal
                destroyOnClose
                title="模块列"
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
                    onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                >
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="TaxisNo" label="排序号" rules={[{ required: true }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="title" label="列名称" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="dataIndex" label="数据列" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="TableAlias" label="表别名">
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="valueType" label="数据类型" >
                                {/* <Input placeholder="请输入" disabled={IsView} /> */}
                                <Select>
                                    <Option value=""></Option>
                                    <Option value="date">日期</Option>
                                    <Option value="dateTime">日期和时间</Option>
                                    <Option value="digit">数字</Option>
                                    <Option value="money">金额</Option>
                                    <Option value="dateRange">日期区间</Option>
                                    <Option value="dateTimeRange">日期和时间区间</Option>
                                    <Option value="time">时间</Option>
                                    <Option value="option">操作项</Option>
                                    <Option value="select">选择</Option>
                                    <Option value="textarea">textarea</Option>
                                    <Option value="index">序号列</Option>
                                    <Option value="indexBorder">带border的序号列</Option>
                                    <Option value="progress">进度条</Option>
                                    <Option value="percent">百分比</Option>
                                    <Option value="code">代码块</Option>
                                    <Option value="avatar">头像</Option>
                                    <Option value="password">密码框</Option>
                                </Select>
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="QueryValueType" label="查询数据类型" >
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="width" label="列宽">
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="align" label="对齐方式">
                                <Select defaultValue="left">
                                    <Option value="left">left</Option>
                                    <Option value="right">right</Option>
                                    <Option value="center">center</Option>
                                </Select>
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        {/* <Col span={12}>
                            <FormItem name="DataType" label="数据类型">
                                <Select style={{ width: '100%' }} onChange={this.handleDataTypeChange}>
                                    <Option value="Date">日期类型</Option>
                                    <Option value="Number">数字</Option>
                                    <Option value="Money">货币</Option>
                                </Select>
                            </FormItem>
                        </Col> */}
                        <Col span={12}>
                            <FormItem name="DataFormate" label="格式">
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="QueryValue" label="查询字段">
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem valuePropName='checked' name="IsBool" label="是否bool">
                                <Switch disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem valuePropName='checked' name="IsLovCode" label="是否参数">
                                <Switch disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem valuePropName='checked' name="hideInTable" label="列表中隐藏">
                                <Switch disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem valuePropName='checked' name="hideInSearch" label="查询中隐藏">
                                <Switch disabled={IsView} />
                            </FormItem>
                        </Col>

                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem valuePropName='checked' name="sorter" label="是否排序">
                                <Switch disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem initialValue={true} valuePropName='checked' name="IsExport" label="是否导出">
                                <Switch disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem valuePropName='checked' name="IsSum" label="是否合计">
                                <Switch disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                        </Col>
                    </Row>
                </Form>
            </Modal>
        </>);
    }
}
export default connect(({ module }) => ({
    module,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(DetailForm);