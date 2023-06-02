import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, InputNumber } from 'antd';
import { connect } from 'umi';

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
    // componentWillMount() {
    //     const { dispatch, Id } = this.props;
    //     
    //     if (dispatch && Id) {
    //         this.setState({ Id })
    //         dispatch({
    //             type: 'materialtype/getById',
    //             payload: { Id },
    //         }).then((result) => {
    //             if (result.response && me.formRef.current)
    //                 me.formRef.current.setFieldsValue(result.response)
    //         })
    //     }

    // }
    componentWillReceiveProps(nextProps) {
        const { dispatch, Id, } = nextProps;
        this.setState({ Id: Id ? Id : null })
        me.formRef.current.resetFields();
        if (dispatch && Id) {
            dispatch({
                type: 'materialtype/getById',
                payload: { Id },
            }).then((result) => {
                if (result.response && me.formRef.current)
                    me.formRef.current.setFieldsValue(result.response)
            })
        }
    }

    onFinish(values) {
        const { dispatch, parentTypeId } = this.props;
        const { Id } = this.state;
        if (Id) {
            values.ID = Id;
        }
        if (parentTypeId)
            values.ParentTypeId = parentTypeId;

        dispatch({
            type: 'materialtype/saveData',
            payload: values,
        }).then(() => {
            const { materialtype: { Id } } = me.props;
            this.setState({ Id })
            dispatch({
                type: 'materialtype/getAllMaterialType'
            });
        });
    }
    render() {
        const { IsView } = this.props;
        return (
            <Form
                labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
            >
                {/* <FormToolbar value={{ me }} /> */}
                <Card>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="MaterialTypeNo" label="分类编号" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="MaterialTypeNames" label="分类名称" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="TaxisNo" label="排序号">
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>

                        </Col>
                    </Row>
                    <Space style={{ display: 'flex', justifyContent: 'center' }}>
                        {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                        {/* <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button> */}
                    </Space>
                </Card>
            </Form>
        )
    }
}
export default connect(({ materialtype }) => ({
    materialtype,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);