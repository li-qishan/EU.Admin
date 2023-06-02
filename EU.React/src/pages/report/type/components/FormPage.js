import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Spin, Tree, Skeleton, InputNumber, Tabs, message, Drawer, Switch } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import Utility from '@/utils/utility';
import ComboGrid from '@/components/SysComponents/ComboGrid';
// import Material from '../../material/components/TableList';
// import Mould from '../../mould/components/TableList';
// import Process from '../../bomprocess/components/TableList';
// import request from "@/utils/request";
import { DownOutlined } from '@ant-design/icons';

const { TabPane } = Tabs;
const FormItem = Form.Item;
let me;

class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleCode: "RM_TYPE_MNG",
            // tabkey: 1,
            isLoading: true,
            disabled: false,
            modelName: 'rmtype'
        };
    }
    async componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'rmtype/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false, disabled: IsView ? true : false });
                let data = result.response;
                if (data && me.formRef.current) {
                    Utility.setFormFormat(data);
                    dispatch({
                        type: 'rmtype/setDetail',
                        payload: data
                    });
                    me.formRef.current.setFieldsValue(data);
                    if (data.AuditStatus == 'CompleteAudit' || data.AuditStatus == 'CompleteAdjust')
                        IsView = true;
                    me.setState({ AuditStatus: data.AuditStatus, IsView })
                }
            })
        } else me.setState({ isLoading: false });

    }
    // componentDidMount() {
    //     const { Id } = this.props;
    //     if (!Id)
    //         me.formRef.current.setFieldsValue({
    //             OrderDate: dayjs()
    //         });
    // }
    onFinish(values, type = 'Save') {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            values.ID = Id;
        dispatch({
            type: 'rmtype/saveData',
            payload: values,
        }).then((result) => {
            let response = result.response;
            if (response.status === "ok") {
                // DetailTable.reloadData()
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
        let { dispatch, moduleInfo } = this.props;
        let { IsView, AuditStatus, isLoading } = this.state;
        if (AuditStatus == 'CompleteAudit' || AuditStatus == 'CompleteAdjust')
            IsView = true;
        return (
            <Form
                labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
            >
                {isLoading ? <Card>
                    <Skeleton active />
                    <Skeleton active />
                    <div style={{ textAlign: 'center', padding: 20 }}>
                        <Spin size="large" />
                    </div>
                </Card> : <>
                    <FormToolbar value={{ me, dispatch }}
                        moduleInfo={moduleInfo}
                    />
                    <Card>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="TypeName" label="类型名称">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
{/*  */}
                            <Col span={8}>
                                <FormItem name="Remark" label="备注">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}></Col>
                        </Row>

                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            {!IsView ? <Button type="primary" onClick={() => this.onFinishAdd()}>保存并新建</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                        </Space>
                    </Card>
                    <div style={{ height: 10 }}></div>
                </>
                }
            </Form>
        )
    }
}
export default connect(({ rmtype }) => ({
    rmtype
}))(FormPage);
