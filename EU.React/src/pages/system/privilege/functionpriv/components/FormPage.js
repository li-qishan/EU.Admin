import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, InputNumber } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import ComBoBox from '@/components/SysComponents/ComboBox';
import ComboGrid from '@/components/SysComponents/ComboGrid';

const FormItem = Form.Item;
let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: ''
        };
    }
    componentDidMount() {
        const { dispatch, user, Id } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'functionpriv/getById',
                payload: { Id },
            }).then((result) => {
                if (result.response && me.formRef.current)
                    me.formRef.current.setFieldsValue(result.response)
            })
        }

    }
    onFinish(values) {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id) {
            values.ID = Id;
        }
        dispatch({
            type: 'functionpriv/saveData',
            payload: values,
        }).then(() => {
            const { dispatch, functionpriv: { Id } } = me.props;
            this.setState({ Id })
        });
    }
    render() {
        const { IsView } = this.props;
        const { Id } = this.state;
        return (
            <Form
                labelCol={{ span: 4 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
            >
                <Card>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="FunctionCode" label="功能代码" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="FunctionName" label="功能名称" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="DisplayPosition" label="显示位置" rules={[{ required: true }]}>
                                <ComBoBox disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="SmModuleId" label="关联模块" rules={[{ required: true }]}>
                                <ComboGrid api="/api/SmModule/GetPageList" itemkey="ID" itemvalue="ModuleName" />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="TaxisNo" label="排序号" >
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="Color" label="颜色" >
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="Icon" label="图标" >
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>

                        </Col>
                    </Row>
                    <Space style={{ display: 'flex', justifyContent: 'center' }}>
                        {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                        <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                    </Space>
                </Card>
            </Form>
        )
    }
}
export default connect(({ functionpriv, loading }) => ({
    functionpriv,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);