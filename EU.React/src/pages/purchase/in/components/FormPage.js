import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton, DatePicker, Modal, message } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import DetailTable from '../../indetail/components/TableList';
import dayjs from 'dayjs';
import Utility from '@/utils/utility';
import FormStatus from '@/components/SysComponents/FormStatus';
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
            moduleCode: "PO_IN_ORDER_MNG",
            tabkey: 1,
            isLoading: true,
            StockId: null,
            disabled: false,
            modelName: 'inorder'
        };
    }
    componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'inorder/getById',
                payload: { Id },
            }).then((result) => {
                let data = result.response.data;
                let detailCount = result.response.count;
                this.setState({ isLoading: false, disabled: IsView ? true : false })
                if (data && me.formRef.current) {
                    Utility.setFormFormat(data);
                    dispatch({
                        type: 'inorder/setDetail',
                        payload: data
                    });
                    if (data.AuditStatus == 'CompleteAudit' || data.AuditStatus == 'CompleteIn')
                        IsView = true;
                    me.formRef.current.setFieldsValue(data);
                    me.setState({ detailCount, StockId: data.StockId, AuditStatus: data.AuditStatus, IsView })
                }
            })
        } else me.setState({ isLoading: false })
        dispatch({
            type: 'poarrival/getConfigByCode',
            payload: 'IsPOArrival'
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
            type: 'inorder/saveData',
            payload: values,
        }).then(() => {
            const { inorder: { Id } } = me.props;
            if (Id)
                this.setState({ Id, AuditStatus: 'Add' })
            // DetailTable.reloadData()
        });
    }
    async onConfirmIn() {
        let { IsView, Id, AuditStatus, detailCount } = this.state;

        Modal.confirm({
            title: '是否确认入库？?',
            icon: Utility.getIcon('ExclamationCircleOutlined'),
            okText: '确定',
            okType: 'danger',
            cancelText: '取消',
            async onOk() {
                const filehide = message.loading('数据提交中..', 0)
                let result = await request('/api/InOrder/ConfirmIn', {
                    method: 'POST',
                    params: {
                        Id
                    }
                });
                setTimeout(filehide);
                if (result && result.status == "ok") {
                    me.setState({ AuditStatus: 'CompleteIn' });
                    Modal.success({
                        content: result.message
                    });
                } else Modal.error({
                    content: result.message
                });
            },
            onCancel() {
                console.log('Cancel');
            },
        });
    }
    render() {

        let { IsView, dispatch, moduleInfo } = this.props;
        const { Id, isLoading, StockId, AuditStatus, detailCount } = this.state;
        if (AuditStatus == 'CompleteAudit' || AuditStatus == 'CompleteIn')
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
                    // }}
                    />
                    <Card>
                        <FormStatus AuditStatus={AuditStatus} />
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="OrderNo" label="入库单号">
                                    <Input placeholder="请输入" disabled defaultValue="自动" />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="OrderDate" label="入库日期" rules={[{ required: true }]}>
                                    <DatePicker
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="SupplierId" label="供应商" rules={[{ required: true }]}>
                                    <ComboGrid
                                        api="/api/Supplier/GetPageList"
                                        itemkey="ID"
                                        itemvalue="FullName"
                                        disabled={IsView || (detailCount > 0 ? true : false)}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="StockId" label="入库仓库">
                                    <ComboGrid
                                        api="/api/Stock/GetPageList"
                                        itemkey="ID"
                                        itemvalue="StockNames"
                                        disabled={IsView}
                                        onChange={(value) => {
                                            // if (value != this.state.StockId) {
                                            //     me.formRef.current.setFieldsValue({ 'DepartmentId': '' });
                                            // }
                                            me.setState({ StockId: value })
                                        }}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="GoodsLocationId" label="入库货位">
                                    <ComboGrid
                                        api="/api/GoodsLocation/GetPageList"
                                        itemkey="ID"
                                        itemvalue="LocationNames"
                                        disabled={!StockId ? true : IsView}
                                        parentId={{ 'StockId': StockId }}
                                    // onChange={(value) => {
                                    // }}
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
                            {Id && AuditStatus == "CompleteAudit" ? <Button
                                type="primary"
                                onClick={() => {
                                    this.onConfirmIn()
                                }} danger>确认入库</Button> : ''}
                        </Space>
                    </Card>

                    <div style={{ height: 10 }}></div>
                    {Id ? <Card>
                        <Tabs>
                            <TabPane
                                tab='基本信息'
                                key='1'
                            >
                                <DetailTable Id={Id} IsView={IsView} inme={me} />
                            </TabPane>
                        </Tabs>
                    </Card> : null}
                </>
                }
            </Form>
        )
    }
}
export default connect(({ inorder, user }) => ({
    inorder, user
}))(FormPage);
