import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Select, Space, InputNumber } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';

const FormItem = Form.Item;
const { Option } = Select;

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
                type: 'autocode/getById',
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
            type: 'autocode/saveData',
            payload: values,
        }).then(() => {
            const { dispatch, autocode: { Id } } = me.props;
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
                            <FormItem name="NumberCode" label="编号代码" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="Prefix" label="前缀" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="DateFormatType" label="日期格式" rules={[{ required: true }]}>
                                <Select allowClear>
                                    <Option key='YYYYMMDDHHMM'>YYYYMMDDHHMM</Option>
                                    <Option key='YYYYMMDDHH'>YYYYMMDDHH</Option>
                                    <Option key='YYYYMMDD'>YYYYMMDD</Option>
                                    <Option key='YYYYMM'>YYYYMM</Option>
                                    <Option key='YYYY'>YYYY</Option>
                                </Select>
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="NumberLength" label="长度" rules={[{ required: true }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="TableName" label="表名" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="ColumnName" label="列名" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
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
export default connect(({ autocode, loading }) => ({
    autocode,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);