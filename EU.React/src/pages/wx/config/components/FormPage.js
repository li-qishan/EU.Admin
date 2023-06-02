import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Spin, Skeleton } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import Utility from '@/utils/utility';
import ComBoBox from '@/components/SysComponents/ComboBox';

const { TextArea } = Input;
const FormItem = Form.Item;
let me;

class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleCode: "SM_WX_CONFIG_MNG",
            tabkey: 1,
            isLoading: true,
            detailCount: 0,
            IsView: false,
            modelName: 'wxconfig'
        };
    }
    componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id, disabled: IsView ? true : false })
            dispatch({
                type: 'wxconfig/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false })
                let data = result.response;
                // let detailCount = result.response.count;
                if (data && me.formRef.current) {
                    Utility.setFormFormat(data);
                    me.formRef.current.setFieldsValue(data);
                    dispatch({
                        type: 'wxconfig/setDetail',
                        payload: data
                    });
                    // if (data.AuditStatus == 'CompleteAudit')
                    //     IsView = true;

                    // me.setState({ AuditStatus: data.AuditStatus, IsView });
                }
            })
        } else me.setState({ isLoading: false })

    }
    // componentDidMount() {
    //     const { Id } = this.props;
    //     if (!Id)
    //         me.formRef.current.setFieldsValue({
    //             YearMonth: dayjs(),
    //             OrderDate: dayjs()
    //         });

    // }
    onFinish(values) {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            values.ID = Id;
        dispatch({
            type: 'wxconfig/saveData',
            payload: values,
        }).then((result) => {
            const { wxconfig: { Id } } = me.props;
            if (Id)
                this.setState({ AuditStatus: "Add", Id })
            // DetailTable.reloadData()
        });
    }
    // async onGenerateDetail() {
    //     let { IsView, Id, AuditStatus, detailCount } = this.state;

    //     Modal.confirm({
    //         title: '是否生成明细？?',
    //         icon: Utility.getIcon('ExclamationCircleOutlined'),
    //         okText: '确定',
    //         okType: 'danger',
    //         cancelText: '取消',
    //         async onOk() {
    //             const filehide = message.loading('数据处理中..', 0)
    //             let result = await request('/api/wxconfigDetail/GenerateDetail/' + Id, {
    //                 method: 'POST'
    //             });
    //             setTimeout(filehide);

    //             if (result && result.status == "ok") {
    //                 DetailTable.reloadData()
    //                 Modal.success({
    //                     content: result.message
    //                 });
    //             } else Modal.error({
    //                 content: result.message
    //             });
    //         },
    //         onCancel() {
    //             console.log('Cancel');
    //         },
    //     });
    // }
    render() {
        // const { IsView, dispatch } = this.props;
        // const { Id, detailCount } = this.state;

        let { dispatch, moduleInfo } = this.props;
        let { IsView, Id, detailCount } = this.state;

        // if (AuditStatus == 'CompleteAudit' || AuditStatus == 'CompleteIn')
        //     IsView = true;

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
                    <FormToolbar value={{ me, dispatch }}
                        moduleInfo={moduleInfo}
                    // onAuditSubmit={(status) => {
                    //     if (status == 'Add') {

                    //     } else if (status == 'CompleteAudit') {

                    //     }
                    // }}
                    />
                    <Card>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="WeixinId" label="微信号" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="InterfaceType" label="接口类型" rules={[{ required: true }]}>
                                    <ComBoBox disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="WeixinName" label="应用名称" rules={[{
                                    required: true
                                }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="AppId" label="应用ID" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="AESKey" label="加密字符">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Token" label="令牌">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="OriginId" label="原始ID" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="AppSecret" label="应用密钥">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Remark" label="备注">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24}>
                            <Col span={24} >
                                <FormItem name="SubscribeContent" labelCol={{ span: 2 }} wrapperCol={{ span: 20 }} label="关注提醒内容" rules={[{ required: true }]}>
                                    <TextArea placeholder="请输入" autoSize={{ minRows: 3 }} disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24}>
                            <Col span={24}>
                                <FormItem name="AutoReplyContent" labelCol={{ span: 2 }} wrapperCol={{ span: 20 }} label="自动回复内容" rules={[{ required: true }]}>
                                    <TextArea placeholder="请输入" autoSize={{ minRows: 3 }} disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                            {/* {Id && AuditStatus == "Add" ? <Button
                                type="primary"
                                onClick={() => {
                                    this.onGenerateDetail()
                                }} danger>生成明细</Button> : ''} */}
                        </Space>

                    </Card>
                    {/*
                    <div style={{ height: 10 }}></div>
                    {Id ? <Card>
                        <Tabs>
                            <TabPane
                                tab='基本信息'
                                key='1'
                            >
                                <DetailTable Id={Id} outdetailme={me} IsView={IsView} />
                            </TabPane>
                        </Tabs>
                    </Card> : null} */}
                </div>
                }
            </Form>
        )
    }
}
export default connect(({ wxconfig, user }) => ({
    wxconfig, user
}))(FormPage);
