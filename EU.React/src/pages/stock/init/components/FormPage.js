import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton, DatePicker, Modal, message } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import DetailTable from '../../initdetail/components/TableList';
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
            moduleCode: "IV_STOCK_INIT_MNG",
            isLoading: true,
            StockId: null,
            disabled: false,
            modelName: 'ivadjust'
        };
    }
    componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'ivinit/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false, disabled: IsView ? true : false });
                let data = result.response;
                if (data && me.formRef.current) {
                    Utility.setFormFormat(data);
                    dispatch({
                        type: 'ivinit/setDetail',
                        payload: data
                    });
                    me.formRef.current.setFieldsValue(data);
                    if (data.AuditStatus == 'CompleteAudit' || data.AuditStatus == 'CompleteInit')
                        IsView = true;
                    me.setState({
                        AuditStatus: data.AuditStatus,
                        StockId: data.StockId,
                        IsView
                    })
                }
            })
        } else me.setState({ isLoading: false })
        if (!Id)
            dispatch({
                type: 'user/getUserDepartment',
                payload: {},
            }).then((result) => {
                if (result.response && result.response.data)
                    me.setState({ DepartmentId: result.response.data.ID })
            })
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
        const { Id, DepartmentId } = this.state;

        if (Id)
            values.ID = Id;
        else {
            values.UserId = currentUser.data.userid;
            values.DepartmentId = DepartmentId;
        }
        dispatch({
            type: 'ivinit/saveData',
            payload: values
        }).then(() => {
            const { ivinit: { Id } } = me.props;
            if (Id)
                this.setState({ Id, AuditStatus: 'Add' })
            // DetailTable.reloadData()
        });
    }
    async onInit() {
        let { Id } = this.state;

        Modal.confirm({
            title: '确认执行数据初始化？？',
            icon: Utility.getIcon('ExclamationCircleOutlined'),
            // content: '是否确定删除记录？',
            okText: '确定',
            okType: 'danger',
            cancelText: '取消',
            async onOk() {
                const filehide = message.loading('数据初始化中..', 0)
                let result = await request('/api/IvInit/Init', {
                    method: 'POST',
                    params: {
                        Id
                    }
                });
                setTimeout(filehide);
                if (result.status == "ok") {
                    message.success(result.message);
                    me.setState({ AuditStatus: 'CompleteInit' });
                }
                else
                    message.error(result.message);
            },
            onCancel() {
                console.log('Cancel');
            },
        });
    }
    async initData() {

        const { Id } = this.state;
        Modal.confirm({
            title: '确认执行数据初始化？？',
            icon: Utility.getIcon('ExclamationCircleOutlined'),
            // content: '是否确定删除记录？',
            okText: '确定',
            okType: 'danger',
            cancelText: '取消',
            async onOk() {
                const filehide = message.loading('数据初始化中..', 0)
                let result = await request('/api/IvInit/Init', {
                    method: 'POST',
                    data: [{ ID: Id }]
                });
                setTimeout(filehide);
                if (result.status == "ok") {
                    message.success(result.message)
                    me.setState({ InitTime: 1 })
                }
                else
                    message.error(result.message);
            },
            onCancel() {
                console.log('Cancel');
            },
        });

    }
    render() {
        let { dispatch, moduleInfo } = this.props;
        let { IsView, Id, AuditStatus, StockId, isLoading } = this.state;
        if (AuditStatus == 'CompleteAudit' || AuditStatus == 'CompleteInit')
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
                                <FormItem name="OrderDate" label="调整日期" rules={[{ required: true }]}>
                                    <DatePicker disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Remark" label="备注">
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
                                            this.setState({ StockId: value })
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
                            <Col span={8}> </Col>
                        </Row>
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                            {AuditStatus == 'CompleteAudit' ? <Button type="primary" onClick={() => {
                                this.onInit()
                            }}>初始化</Button> : null}
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
export default connect(({ ivinit, user }) => ({
    ivinit, user
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);
