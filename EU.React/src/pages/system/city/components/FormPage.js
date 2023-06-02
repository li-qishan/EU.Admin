import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton, InputNumber } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import Utility from '@/utils/utility';
import DetailTable from '../../citydetail/components/TableList';

const FormItem = Form.Item;
const { TabPane } = Tabs;
let me;

class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleCode: "SM_CITY_MNG",
            // tabkey: 1,
            isLoading: true,
        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'city/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false })
                if (result.response && me.formRef.current) {
                    Utility.setFormFormat(result.response);
                    dispatch({
                        type: 'city/setDetail',
                        payload: result.response
                    });
                    me.formRef.current.setFieldsValue(result.response);
                }
            })
        } else me.setState({ isLoading: false })
    }
    componentDidMount() {
        // const { Id, user: { currentUser } } = this.props;
        // if (!Id)
        //     me.formRef.current.setFieldsValue({
        //         OrderDate: dayjs()
        //     });
        // me.formRef.current.setFieldsValue({
        //     'CustomerId1': currentUser.data.name
        // });
    }
    onFinish(values) {
        const { dispatch } = this.props;
        const { Id, } = this.state;
        if (Id)
            values.ID = Id;
        dispatch({
            type: 'city/saveData',
            payload: values,
        }).then(() => {
            const { city: { Id } } = me.props;
            if (Id)
                this.setState({ Id })
            // DetailTable.reloadData()
        });
    }
    render() {
        const { IsView, city: { detail } } = this.props;
        const { Id } = this.state;
        return (
            <Form
                labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
            >
                {this.state.isLoading ? <Card>
                    <Skeleton active />
                    <Skeleton active />
                    <div style={{ textAlign: 'center', padding: 20 }}>
                        <Spin size="large" />
                    </div>
                </Card> : <div>
                    <Card>
                        <FormToolbar value={{ me }} />

                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="CityCode" label="城市代码" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="CityNameZh" label="城市" rules={[{ required: true }]}>
                                    <Input placeholder="请输入"
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="CityNameEn" label="城市(英文)">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="CityNo" label="城市编号">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="TaxisNo" label="排序号">
                                    <InputNumber placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Remark" label="备注">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                        </Space>
                    </Card>

                    <div style={{ height: 10 }}></div>
                    {detail.ID ? <Card>
                        <Tabs>
                            <TabPane
                                tab='城市'
                                key='1'
                            >
                                <DetailTable Id={detail.ID} />
                            </TabPane>
                        </Tabs>
                    </Card> : null}
                </div>
                }
            </Form>
        )
    }
}
export default connect(({ city }) => ({
    city
}))(FormPage);
