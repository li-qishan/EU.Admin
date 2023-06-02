import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, InputNumber, Skeleton, Spin, message } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';

const FormItem = Form.Item;

let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleCode: "BD_MEASUREMENT_UNIT_MNG",
            isLoading: true,
            disabled: true
        };
    }
    componentWillMount() {
        const { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'unit/getById',
                payload: { Id }
            }).then((result) => {
                me.setState({ isLoading: false, disabled: IsView ?? false })
                if (result.Success && me.formRef.current)
                    me.formRef.current.setFieldsValue(result.Data)
            })
        } else me.setState({ isLoading: false })

    }
    // onFinish(values) {
    //     const { dispatch } = this.props;
    //     const { Id } = this.state;
    //     if (Id) {
    //         values.ID = Id;
    //     }
    //     dispatch({
    //         type: 'unit/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { unit: { Id } } = me.props;
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
            type: 'unit/saveData',
            payload: data
        }).then((result) => {
            let response = result.response;
            if (response.status === "ok") {
                if (type == 'SaveAdd') {
                    me.setState({
                        Id: ''
                    });
                    me.formRef.current.resetFields();
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
        const { isLoading } = this.state;

        return (
            <Form
                labelCol={{ span: 4 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
            >
                <FormToolbar value={{ me, dispatch }}
                    moduleInfo={moduleInfo}
                />
                {isLoading ? <Card>
                    <Skeleton active />
                    <Skeleton active />
                    <div style={{ textAlign: 'center', padding: 20 }}>
                        <Spin size="large" />
                    </div>
                </Card> : <Card>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="UnitNo" label="单位编号" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="UnitNames" label="单位名称" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="DecimalPlaces" label="小数位数" rules={[{ required: true }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>

                        </Col>
                    </Row>
                    <Space style={{ display: 'flex', justifyContent: 'center' }}>
                        {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                        {!IsView ? <Button type="primary" onClick={() => this.onFinishAdd()}>保存并新建</Button> : ''}
                        <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                    </Space>
                </Card>}
            </Form>
        )
    }
}
export default connect(({ unit }) => ({
    unit,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);