import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space } from 'antd';
import { connect } from 'umi';
import TableList from './Index';
import Index from '../index';

const FormItem = Form.Item;
const { TextArea } = Input;

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
    componentWillMount() {
        const { dispatch, user, Id } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'paramconfig/getById',
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
            type: 'paramconfig/saveData',
            payload: values,
        }).then(() => {
            const { dispatch, paramconfig: { Id } } = me.props;
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
                            <FormItem name="ConfigCode" label="参数代码" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="ConfigValue" label="参数值" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24}>
                        <Col span={24}>
                            <FormItem name="Remark" label="备注" rules={[{ required: true }]}>
                                <TextArea placeholder="请输入" autoSize={{ minRows: 6 }} disabled={IsView} />
                            </FormItem>
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
export default connect(({ paramconfig, loading }) => ({
    paramconfig,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);