import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';

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
        const { dispatch, Id } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'company/getById',
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
            type: 'company/saveData',
            payload: values,
        }).then(() => {
            const { company: { Id } } = me.props;
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
                            <FormItem name="CompanyCode" label="组织编码" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="CompanyName" label="组织名称" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="CompanyShortName" label="组织简称" >
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
export default connect(({ company }) => ({
    company,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);