import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import DetailTable from '../../tabledetail/components/TableList';
import Utility from '@/utils/utility';

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
            moduleCode: "SM_TABLE_CATALOG_MNG",
            // tabkey: 1,
            isLoading: true,
        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'smtable/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false })
                if (result.response && me.formRef.current) {
                    Utility.setFormFormat(result.response);
                    dispatch({
                        type: 'smtable/setDetail',
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
            type: 'smtable/saveData',
            payload: values,
        }).then(() => {
            const { smtable: { Id } } = me.props;
            if (Id)
                this.setState({ Id })
            // DetailTable.reloadData()
        });
    }
    render() {
        const { IsView, smtable: { detail } } = this.props;
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
                                    <FormItem name="TableCode" label="表代码" rules={[{ required: true }]}>
                                        <Input placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem name="TableName" label="表名" rules={[{ required: true }]}>
                                        <Input placeholder="请输入"
                                            disabled={IsView}
                                        />
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
                        {detail.TableCode ? <Card>
                            <Tabs>
                                <TabPane
                                    tab='表栏位'
                                    key='1'
                                >
                                    <DetailTable Id={detail.TableCode} />
                                </TabPane>
                            </Tabs>
                        </Card> : null}
                    </div>
                }
            </Form>
        )
    }
}
export default connect(({ smtable }) => ({
    smtable
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);
