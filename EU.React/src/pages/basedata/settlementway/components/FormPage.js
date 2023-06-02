import React, { Component } from 'react';
import { Button, Spin, Input, Card, Form, Row, Col, Space, InputNumber, Skeleton, message } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComBoBox from '@/components/SysComponents/ComboBox';
const FormItem = Form.Item;

let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleCode: "BD_SETTLEMENTWAY_MNG",
            isLoading: true,
            disabled: true
        };
    }
    componentWillMount() {
        const { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'settlementway/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false, disabled: IsView ? true : false })
                if (result.response && me.formRef.current)
                    me.formRef.current.setFieldsValue(result.response)
            })
        } else
            this.setState({ isLoading: false })

    }
    componentDidMount() {
        const { Id } = this.props;
        if (!Id)
            me.formRef.current.setFieldsValue({
                Days: 0
            });

    }
    // onFinish(values) {
    //     const { dispatch } = this.props;
    //     const { Id } = this.state;
    //     if (Id) {
    //         values.ID = Id;
    //     }
    //     dispatch({
    //         type: 'settlementway/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { settlementway: { Id } } = me.props;
    //         if (Id)
    //             this.setState({ Id })
    //     });
    // }
    onFinish(data, type = 'Save') {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            data.ID = Id;

        dispatch({
            type: 'settlementway/saveData',
            payload: data
        }).then((result) => {
            let response = result.response;
            if (response.status === "ok") {
                if (type == 'SaveAdd') {
                    me.setState({
                        Id: ''
                    });
                    me.formRef.current.resetFields();
                    me.formRef.current.setFieldsValue({
                        Days: 0
                    });
                }
                else if (!Id)
                    this.setState({ Id: response.Id });
                message.success(response.message);
            }
            else
                message.error(response.message);
        });
    }
    onFinishAdd() {
        me.formRef.current.validateFields()
            .then(values => {
                me.onFinish(values, 'SaveAdd')
            });
    }
    render() {
        const { IsView, dispatch, moduleInfo } = this.props;
        return (
            <Form
                labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
            >
                <FormToolbar value={{ me, dispatch }}
                    moduleInfo={moduleInfo}
                />
                {this.state.isLoading ? <Card>
                    <Skeleton active />
                    <Skeleton active />
                    <div style={{ textAlign: 'center', padding: 20 }}>
                        <Spin size="large" />
                    </div>
                </Card> :
                    <Card>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="SettlementNo" label="编号" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Days" label="账期天数" rules={[{ required: true }, { type: 'number', min: 0, message: '账期天数最小值为0 !' }]}>
                                    <InputNumber placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="SettlementAccountType" label="账款类型" rules={[{ required: true }]}>
                                    <ComBoBox disabled={IsView}
                                        onChange={(value, Option, data) => {
                                            if (value == 'ImmediatePay')
                                                me.formRef.current.setFieldsValue({
                                                    Days: 0
                                                });
                                            else if (value == 'MonthlyPay')
                                                me.formRef.current.setFieldsValue({
                                                    Days: 30
                                                });
                                            else if (value == 'NextMonthlyPay')
                                                me.formRef.current.setFieldsValue({
                                                    Days: 60
                                                });
                                        }}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="SettlementBillType" label="收付款" rules={[{ required: true }]}>
                                    <ComBoBox disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                {/* <FormItem name="SettlementName" label="付款说明">
                                    <Input placeholder="请输入" disabled />
                                </FormItem> */}
                                <FormItem name="Remark" label="备注">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>

                            </Col>
                        </Row>
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            {!IsView ? <Button type="primary" onClick={() => this.onFinishAdd()}>保存并新建</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                        </Space>
                    </Card>

                }
            </Form>
        )
    }
}
export default connect(({ settlementway }) => ({
    settlementway,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);