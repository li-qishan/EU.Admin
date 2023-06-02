import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton, DatePicker, Switch, Modal, message } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import DetailTable from '../../checkdetail/components/TableList';
import dayjs from 'dayjs';
import Utility from '@/utils/utility';
import request from "@/utils/request";

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
            moduleCode: "IV_STOCK_CHECK_MNG",
            // tabkey: 1,
            isLoading: true,
            StockId: null,
            disabled: true,
            modelName: 'ivcheck'
        };
    }
    componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'ivcheck/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false, disabled: IsView ? true : false });
                let data = result.response;
                if (data && me.formRef.current) {
                    Utility.setFormFormat(data);
                    dispatch({
                        type: 'ivcheck/setDetail',
                        payload: data
                    });
                    me.formRef.current.setFieldsValue(data);
                    if (data.AuditStatus == 'CompleteAudit' || data.AuditStatus == 'CompleteOut')
                        IsView = true;
                    me.setState({ AuditStatus: data.AuditStatus, StockId: data.StockId, IsView })
                }
            })
        } else me.setState({ isLoading: false })
    }
    componentDidMount() {
        const { Id } = this.props;
        if (!Id)
            me.formRef.current.setFieldsValue({
                OrderDate: dayjs()
            });
    }
    onFinish(values) {
        const { dispatch, user: { currentUser } } = this.props;
        const { Id } = this.state;
        if (Id)
            values.ID = Id;
        else {
            values.UserId = currentUser.data.userid;
        }
        dispatch({
            type: 'ivcheck/saveData',
            payload: values,
        }).then(() => {
            const { ivcheck: { Id } } = me.props;
            if (Id)
                this.setState({ Id, disabled: false, AuditStatus: "Add" })
            DetailTable.reloadData();
        });
    }
    async onConfirmCheck() {
        let { Id } = this.state;

        Modal.confirm({
            title: '是否盘点确认？？',
            icon: Utility.getIcon('ExclamationCircleOutlined'),
            okText: '确定',
            okType: 'danger',
            cancelText: '取消',
            async onOk() {
                const filehide = message.loading('数据提交中..', 0)
                let result = await request('/api/IvCheck/ConfirmCheck', {
                    method: 'POST',
                    params: {
                        Id
                    }
                });
                setTimeout(filehide);
                if (result && result.status == "ok") {
                    me.setState({ AuditStatus: 'CompleteCheck' });
                    Modal.success({
                        content: result.message
                    });
                } else Modal.error({
                    content: result.message
                });
            },
            onCancel() {
                // console.log('Cancel');
            },
        });
    }

    async generateCheckDetail() {
        let { Id } = this.state;

        Modal.confirm({
            title: '是否确定重新生成盘点单数据？？',
            icon: Utility.getIcon('ExclamationCircleOutlined'),
            okText: '确定',
            okType: 'danger',
            cancelText: '取消',
            async onOk() {
                const filehide = message.loading('数据提交中..', 0)
                let result = await request('/api/IvCheck/GenerateCheckDetail', {
                    method: 'POST',
                    params: {
                        Id
                    }
                });
                setTimeout(filehide);
                if (result && result.status == "ok") {
                    DetailTable.reloadData();
                    Modal.success({
                        content: result.message
                    });
                } else Modal.error({
                    content: result.message
                });
            },
            onCancel() {
                // console.log('Cancel');
            },
        });
    }
    render() {
        let { dispatch, moduleInfo } = this.props;
        let { IsView, Id, AuditStatus, StockId, isLoading } = this.state;
        if (AuditStatus == 'CompleteAudit' || AuditStatus == 'CompleteCheck')
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
                    // onAuditSubmit={(status) => {
                    //     if (status == 'Add') {

                    //     } else if (status == 'CompleteAudit') {

                    //     }
                    // }}
                    />
                    <Card>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="OrderNo" label="单号">
                                    <Input placeholder="请输入" disabled defaultValue="自动" />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="OrderDate" label="盘点日期" rules={[{ required: true }]}>
                                    <DatePicker
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="CheckName" label="盘点名称" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>

                            <Col span={8}>
                                <FormItem name="StockId" label="仓库">
                                    <ComboGrid
                                        api="/api/Stock/GetPageList"
                                        itemkey="ID"
                                        itemvalue="StockNames"
                                        disabled={IsView}
                                        onChange={(value) => {
                                            this.setState({ StockId: value });
                                            me.formRef.current.setFieldsValue({
                                                GoodsLocationId: null
                                            });
                                        }}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="GoodsLocationId" label="货位">
                                    <ComboGrid
                                        api="/api/GoodsLocation/GetPageList"
                                        itemkey="ID"
                                        itemvalue="LocationNames"
                                        disabled={!StockId ? true : IsView}
                                        parentId={{ 'StockId': StockId }}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="MaterialId" label="物料">
                                    <ComboGrid
                                        api="/api/Material/GetPageList"
                                        itemkey="ID"
                                        param={{
                                            IsActive: true
                                        }}
                                        itemvalue={['MaterialNo', 'MaterialNames']}
                                        onChange={(value, Option, data) => {
                                            me.formRef.current.setFieldsValue({
                                                'MaterialName': data.length > 0 ? data[0].MaterialNames : ""
                                            });
                                        }}
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>

                            <Col span={8}>
                                <FormItem valuePropName='checked' name="IsZero" label="包含0库存">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Remark" label="备注">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                            </Col>
                        </Row>
                        <Space style={{ display: 'none' }}>
                            <FormItem name="MaterialName">
                                <Input />
                            </FormItem>
                        </Space>
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                            {!IsView ? <Button type="primary" onClick={() => {
                                this.generateCheckDetail()
                            }}>生成盘点单</Button> : null}
                            {AuditStatus == 'CompleteAudit' ? <Button type="primary" onClick={() => {
                                this.onConfirmCheck()
                            }}>盘点确认</Button> : null}
                        </Space>
                    </Card>

                    <div style={{ height: 10 }}></div>
                    {Id ? <Card>
                        <Tabs>
                            <TabPane
                                tab='基本信息'
                                key='1'
                            >
                                <DetailTable Id={Id} IsView={IsView} />
                            </TabPane>
                        </Tabs>
                    </Card> : null}
                </>
                }
            </Form>
        )
    }
}
export default connect(({ ivcheck, user }) => ({
    ivcheck, user
}))(FormPage);
