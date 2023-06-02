import React, { Component } from 'react';
import { Button, Tree, Input, Card, Form, Row, Col, Select, Space, Tabs } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import DetailTable from './DetailTable';

const FormItem = Form.Item;
const { TabPane } = Tabs;

let me;
class FormPage extends Component {
    formRef = React.createRef();
    actionRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            modalVisible: false,
            IsDetailView: false
        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'setparam/getById',
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
            type: 'setparam/saveData',
            payload: values,
        }).then(() => {
            const { setparam: { Id } } = me.props;
            this.setState({ Id })
        });
    }
    render() {
        const { IsView, setparam: { moduleInfo } } = this.props;
        const { Id } = this.state;

        return (
            <>
                <Form
                    labelCol={{ span: 4 }}
                    wrapperCol={{ span: 16 }}
                    onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                >
                    <Card>
                        <Row gutter={24} justify={"center"}>
                            <Col span={12}>
                                <FormItem name="LovCode" label="参数代码" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={12}>
                                <FormItem name="LovName" label="参数名称" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                        </Space>
                    </Card>
                    <div style={{ height: 20 }}></div>
                </Form>
                <Card>
                    <Tabs>
                        <TabPane
                            tab='参数明细'
                        >
                            <DetailTable Id={Id} />
                        </TabPane>
                    </Tabs>
                </Card>
            </>
        )
    }
}
export default connect(({ setparam }) => ({
    setparam,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);