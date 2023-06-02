import React, { Component } from 'react';
import { connect } from 'umi';
import { query } from '../service';
import { Input, Form, DatePicker } from 'antd';
import Index from '../index';
import SmProTable from '@/components/SysComponents/SmProTable';
import ComboGrid from '@/components/SysComponents/ComboGrid';

const FormItem = Form.Item;
const { RangePicker } = DatePicker;
let moduleCode = "PD_CLOSE_ORDER_MNG";
let me;
class TableList extends Component {
    formRef = React.createRef();
    formRef1 = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {

        };
    }
    componentWillMount() {
        const { dispatch } = this.props;
        dispatch({
            type: 'pdclose/getModuleInfo',
            payload: { moduleCode },
        })
    }
    async onConfirmIn() {
        this.formRef.current.submit();
    }
    render() {
        const { dispatch, pdclose: { moduleInfo, tableParam } } = this.props;

        moduleInfo.columns && moduleInfo.columns.map((item, index) => {
            if (item.dataIndex == 'MaterialId') {
                moduleInfo.columns[index].renderFormItem = () => (
                    <ComboGrid
                        api="/api/Material/GetPageList"
                        itemkey="ID"
                        param={{
                            IsActive: true
                        }}
                        itemvalue={['MaterialNo', 'MaterialNames']}
                        onClear={(value) => {
                            if (tableParam.params.MaterialId) {
                                delete tableParam.params.MaterialId
                            }
                        }}
                    />
                )
            } else if (item.dataIndex == 'OrderDate') {
                moduleInfo.columns[index].renderFormItem = () => (
                    <RangePicker />
                )
            } else if (item.dataIndex == 'OrderNo') {
                moduleInfo.columns[index].renderFormItem = () => (
                    <Input placeholder="请输入" />
                )
            }
        })

        //#region 操作栏按钮方法
        const action = {}
        //#endregion

        return (
            <div>
                {/* <Form
                    labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                    wrapperCol={{ span: 16 }}
                    ref={this.formRef1}
                >
                    <Card>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="OrderDate" label="工单日期">
                                    <RangePicker />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="OrderNo" label="工单号" >
                                    <Input placeholder="请输入" />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="MaterialId" label="物料" >
                                    <ComboGrid
                                        api="/api/Material/GetPageList"
                                        itemkey="ID"
                                        param={{
                                            IsActive: true
                                        }}
                                        itemvalue={['MaterialNo', 'MaterialNames']}
                                    />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="CustomerName" label="客户名称" >
                                    <Input placeholder="请输入" />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="SalesOrderNo" label="销售单号" >
                                    <Input placeholder="请输入" />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                            </Col>
                        </Row>
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            <Button
                                type="primary"
                                onClick={() => {
                                    this.onConfirmIn()
                                }} danger>确认入库</Button>
                        </Space>
                    </Card>
                </Form> */}
                <SmProTable
                    columns={moduleInfo.columns}
                    // delete={Delete}
                    // batchDelete={(selectedRows) => BatchDelete(selectedRows)}
                    moduleInfo={moduleInfo}
                    {...action}
                    changePage={Index.changePage}
                    // formPage={FormPage}
                    formRef={this.formRef}
                    form={{ labelCol: { span: 6 } }}
                    onReset={() => {
                        dispatch({
                            type: 'pdclose/setTableStatus',
                            payload: {},
                        })
                    }}
                    onLoad={() => {
                        if (tableParam && tableParam.params && this.formRef.current)
                            this.formRef.current.setFieldsValue({ ...tableParam.params })
                    }}
                    pagination={tableParam && tableParam.params ? { current: tableParam.params.current } : {}}
                    request={(params, sorter, filter) => {
                        if (tableParam && tableParam.params && !params._timestamp)
                            params = Object.assign(tableParam.params, params);
                        if (tableParam && tableParam.sorter)
                            sorter = Object.assign(tableParam.sorter, sorter);

                        if (params.OrderDate) {
                            params = Object.assign(params, {
                                StartDate: params.OrderDate[0],
                                EndDate: params.OrderDate[1]
                            });
                            delete tableParam.params.OrderDate;
                        } else {
                            delete params.StartDate;
                            delete params.EndDate;
                        }
                        dispatch({
                            type: 'pdclose/setTableStatus',
                            payload: { params, sorter },
                        })
                        return query({
                            paramData: JSON.stringify(params),
                            sorter: JSON.stringify(sorter),
                            filter: JSON.stringify(filter),
                            moduleCode
                        })
                    }}
                />
            </div>
        )
    }
}
export default connect(({ pdclose }) => ({
    pdclose,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);