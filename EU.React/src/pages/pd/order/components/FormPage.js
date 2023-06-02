import React, { Component } from 'react';
import { Modal, Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton, DatePicker, message, InputNumber, Select } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import Material from '../../ordermaterial/components/TableList';
import Detail from '../../orderdetail/components/TableList';
import Process from '../../orderprocess/components/TableList';
import Mould from '../../ordermould/components/TableList';
import dayjs from 'dayjs';
import Utility from '@/utils/utility';
import request from "@/utils/request";
import ComBoBox from '@/components/SysComponents/ComboBox';
import ProTable from '@ant-design/pro-table';
import { querySourceList } from '../service';

const FormItem = Form.Item;
const { TabPane } = Tabs;
let me;

class FormPage extends Component {
    formRef = React.createRef();
    ref = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleCode: "PD_ORDER_MNG",
            tabkey: 1,
            isLoading: true,
            MaterialId: null,
            detailCount: 0,
            IsView: false,
            modelName: 'pdorder',
            modalVisible: false,
            tempRecord: null,
            selectedRowKeys: [],
            source: '',
            Formula: '',
            isEdit: 'N'
        };
    }
    componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id, disabled: IsView ? true : false })
            dispatch({
                type: 'pdorder/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false })
                let data = result.response.data;
                let extend = result.response.extend;
                let detailCount = result.response.count;
                if (data && me.formRef.current) {
                    Utility.setFormFormat(data);
                    me.formRef.current.setFieldsValue(data);
                    me.formRef.current.setFieldsValue(extend);
                    let Formula = extend.Formula;
                    if (data.AuditStatus == 'CompleteAudit' || data.AuditStatus == 'CompleteOut')
                        IsView = true;

                    me.setState({ AuditStatus: data.AuditStatus, StockId: data.StockId, detailCount, IsView, Formula });
                }
            })
        } else me.setState({ isLoading: false })

    }
    componentDidMount() {
        const { Id } = this.props;
        if (!Id)
            me.formRef.current.setFieldsValue({
                PlanStartDate: dayjs(),
                OrderDate: dayjs(),
                UrgentType: 'Normal'
            });
    }
    onFinish(values) {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            values.ID = Id;
        dispatch({
            type: 'pdorder/saveData',
            payload: values,
        }).then((result) => {
            if (result.status === "ok")
                message.success(result.message);
            else
                message.error(result.message);
            const { pdorder: { Id } } = me.props;
            if (Id)
                this.setState({ isEdit: 'N', AuditStatus: "Add", Id })
            // DetailTable.reloadData()
        });
    }
    async onGenerateDetail() {
        const { Id, isEdit } = this.state;
        const { dispatch } = this.props;
        if (isEdit == 'Y')
            me.formRef.current.validateFields()
                .then(values => {
                    if (Id)
                        values.ID = Id;
                    dispatch({
                        type: 'pdorder/saveData',
                        payload: values,
                    }).then((result) => {
                        if (result.status === "ok") {
                            me.setState({ isEdit: 'N', AuditStatus: "Add" })
                            me.onGenerateDetailSure()
                        }
                        else
                            message.error(result.message);
                    });
                });
        else
            me.onGenerateDetailSure()
    }
    async onGenerateDetailSure() {
        let { IsView, Id, AuditStatus, detailCount } = this.state;

        Modal.confirm({
            title: '是否生成明细？?',
            icon: Utility.getIcon('ExclamationCircleOutlined'),
            okText: '确定',
            okType: 'danger',
            cancelText: '取消',
            async onOk() {
                const filehide = message.loading('数据提交中..', 0)
                let result = await request('/api/PdOrder/GenerateDetail', {
                    method: 'POST',
                    params: {
                        Id
                    }
                });
                setTimeout(filehide);
                if (result && result.status == "ok") {
                    Material.reloadData();
                    Process.reloadData();
                    Mould.reloadData();
                    // me.setState({ AuditStatus: 'CompleteOut' });
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
    okHandle = async () => {
        let { tempRecord } = this.state;
        let record = tempRecord;
        let Formula = '';
        me.formRef.current.setFieldsValue({
            SourceOrderNo: record.SourceOrderNo,
            Specifications: record.Specifications,
            UnitName: record.UnitName,
            Formula: record.Formula,
            MaterialName: record.MaterialNo + ' - ' + record.MaterialName,
            QTY: record.QTY,
            MaterialId: record.MaterialId,
            Source: record.Source,
            SourceOrderId: record.SourceOrderId,
            SourceOrderDetailId: record.SourceOrderDetailId
        });
        Formula = record.Formula;
        me.setState({
            modalVisible: false,
            Formula,
            isEdit: 'Y'
        })
    };
    render() {
        let { dispatch, moduleInfo } = this.props;
        let { IsView, Id, AuditStatus, detailCount, modalVisible, selectedRowKeys, source, Formula } = this.state;
        if (AuditStatus == 'CompleteAudit' || AuditStatus == 'CompleteOut')
            IsView = true;

        let tableParam = {};
        let columns = [];
        if (source == 'Material')
            columns = [{
                title: '物料编号',
                dataIndex: 'MaterialNo',
                width: 160
            }, {
                title: '物料名称',
                dataIndex: 'MaterialName'
            }, {
                title: '规格',
                dataIndex: 'Specifications'
            }, {
                title: '单位',
                dataIndex: 'UnitName'
            }, {
                title: '配方',
                dataIndex: 'Formula'
            }];
        else
            columns = [{
                title: '来源',
                dataIndex: 'Source',
                width: 100,
                filters: false,
                valueEnum: {
                    'Sales': {
                        text: '销售单',
                    },
                    'Requestion': {
                        text: '请购单',
                    }
                }
            }, {
                title: '来源单号',
                dataIndex: 'SourceOrderNo',
                width: 160
            }, {
                title: '项次',
                dataIndex: 'SerialNumber',
                width: 100
            }, {
                title: '物料编号',
                dataIndex: 'MaterialNo',
                width: 160
            }, {
                title: '物料名称',
                dataIndex: 'MaterialName'
            }, {
                title: '规格',
                dataIndex: 'Specifications'
            }, {
                title: '单位',
                dataIndex: 'UnitName'
            }, {
                title: '配方',
                dataIndex: 'Formula'
            }, {
                title: '生产数量',
                dataIndex: 'QTY'
            }];

        return (
            (<Form
                labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
                onFieldsChange={(changedFields, allFields) => {
                    me.setState({
                        isEdit: 'Y'
                    })
                }}
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
                                <FormItem name="OrderNo" label="单号">
                                    <Input placeholder="请输入" disabled='trues' defaultValue="自动" />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="OrderDate" label="作业日期" rules={[{ required: true }]}>
                                    <DatePicker
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="MaterialName" label="物料" rules={[{ required: true, message: '请选择物料' }]}>
                                    {/* <ComboGrid
                                        api="/api/Material/GetPageList"
                                        itemkey="ID"
                                        itemvalue={['MaterialNo', 'MaterialNames']}
                                        disabled={IsView}
                                    /> */}
                                    <Input
                                        addonAfter={<div
                                            onClick={() => {
                                                me.setState({ modalVisible: true, source: 'Material' });
                                            }}
                                        >{Utility.getIcon('SearchOutlined')}</div>} disabled={true} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="SourceOrderNo" label="来源单号">
                                    <Input addonAfter={<div
                                        onClick={() => {
                                            if (source == 'Material' || !source) source = 'Sales';
                                            me.setState({ modalVisible: true, source });
                                        }}
                                    >{Utility.getIcon('SearchOutlined')}</div>} disabled={true} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Specifications" label="规格" >
                                    <Input disabled={true} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="UnitName" label="单位" >
                                    <Input disabled={true} />
                                </FormItem>
                            </Col>

                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="Formula" label="BOM配方">
                                    <Input disabled={true} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="QTY" label="生产数量" rules={[{
                                    required: true
                                }]}>
                                    <InputNumber placeholder="请输入" min='0' disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="UrgentType" label="紧急程度" rules={[{ required: true }]}>
                                    <ComBoBox disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="PlanStartDate" label="预开工日" rules={[{
                                    required: true
                                }]}>
                                    <DatePicker
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="PlanEndDate" label="预完工日" rules={[{
                                    required: true
                                }]}>
                                    <DatePicker
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="WorkShopId" label="车间" rules={[{ required: true }]}>
                                    <ComboGrid
                                        api="/api/WorkShop/GetPageList"
                                        itemkey="ID"
                                        itemvalue={['WorkShopNo', 'WorkShopName']}
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>

                            <Col span={8}>
                                <FormItem name="RequireDate" label="需求日期" rules={[{
                                    required: true
                                }]}>
                                    <DatePicker
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Remark" label="备注">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}></Col>
                        </Row>

                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                            {Id && AuditStatus == "Add" && Formula ? <Button
                                type="primary"
                                onClick={() => {
                                    this.onGenerateDetail()
                                }} danger>生成明细</Button> : ''}
                        </Space>
                        <div style={{ display: 'none' }}>
                            <FormItem name="MaterialId"><Input /></FormItem>
                            <FormItem name="Source"><Input /></FormItem>
                            <FormItem name="SourceOrderId"><Input /></FormItem>
                            <FormItem name="SourceOrderDetailId"><Input /></FormItem>
                        </div>
                    </Card>

                    <div style={{ height: 10 }}></div>
                    {Id ? <Card>
                        <Tabs>
                            <TabPane
                                tab='材料明细'
                                key='1'
                            >
                                <Material Id={Id} outdetailme={me} IsView={IsView} />
                            </TabPane>
                            <TabPane
                                tab='对应订单'
                                key='2'
                            >
                                <Detail Id={Id} outdetailme={me} IsView={IsView} />
                            </TabPane>
                            <TabPane
                                tab='工艺路线'
                                key='3'
                            >
                                <Process Id={Id} outdetailme={me} IsView={IsView} />
                            </TabPane>
                            <TabPane
                                tab='工模治具'
                                key='4'
                            >
                                <Mould Id={Id} outdetailme={me} IsView={IsView} />
                            </TabPane>
                        </Tabs>
                    </Card> : null}
                    <Modal
                        destroyOnClose
                        title='选择物料'
                        open={modalVisible}
                        onOk={this.okHandle}
                        maskClosable={false}
                        width={1100}
                        closable={false}
                        onCancel={() => { me.setState({ modalVisible: false }); }}
                    // okButtonProps={{ disabled: selectList.length == 0 ? true : false }}
                    >
                        <ProTable
                            actionRef={this.ref}
                            columnEmptyText={false}
                            tableAlertRender={false}
                            rowKey="ID"
                            rowSelection={{
                                fixed: 'left',
                                selectedRowKeys,
                                type: 'radio',
                                onSelect: (record, selected) => {
                                    let tempSelectedRowKeys = [];
                                    if (selected) {
                                        tempSelectedRowKeys.push(record.ID);
                                    } else {
                                        let index = tempSelectedRowKeys.findIndex(item => item === record.ID);
                                        if (index > -1)
                                            tempSelectedRowKeys.splice(index, 1);
                                    }
                                    me.setState({
                                        tempRecord: record,
                                        selectedRowKeys: tempSelectedRowKeys
                                    })
                                }
                            }}
                            scroll={{ x: 'max-content' }}
                            columns={columns}
                            search={false}
                            size='small'
                            pagination={{
                                pageSize: 10
                            }}
                            request={(params, sorter, filter) => {
                                if (tableParam && tableParam.params && !params._timestamp)
                                    params = tableParam.params;

                                return querySourceList({
                                    paramData: JSON.stringify(params),
                                    sorter: JSON.stringify(sorter),
                                    filter: JSON.stringify(filter),
                                    source
                                })
                            }}
                            options={{
                                fullScreen: false,
                                reload: false,
                                setting: false,
                                density: false,
                                search: {
                                    name: 'keyWord',
                                },
                            }}
                            toolBarRender={() => [
                                // { source != 'material' ?
                                <Select
                                    value={source}
                                    defaultValue={source}
                                    onChange={(value) => {
                                        this.setState({ source: value });
                                        this.ref.current.reloadAndRest();
                                    }}
                                    style={{ marginLeft: 10, width: 120 }}
                                >
                                    <Select.Option value="Material">基础物料</Select.Option>
                                    <Select.Option value="Sales">销售单</Select.Option>
                                    <Select.Option value="PdPlan">生产计划单</Select.Option>
                                </Select>
                                // : null}
                            ]}
                        />
                    </Modal>
                </div>
                }
            </Form>)
        );
    }
}
export default connect(({ pdorder, employee }) => ({
    pdorder, employee
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);
