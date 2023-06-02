import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import { Input, Card, Form, Row, Col, Modal, message } from 'antd';
import Index from '../index';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import request from "@/utils/request";

let moduleCode = "PS_PROCESS_BOM_MNG";
let me;
let modelName = 'psbom';
const FormItem = Form.Item;

class TableList extends Component {
    formRef = React.createRef();
    formRef1 = React.createRef();
    actionRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            isModalVisible: false,
            Id: ''
        };
    }
    componentWillMount() {
        const { dispatch } = this.props;
        dispatch({
            type: modelName + '/getModuleInfo',
            payload: { moduleCode },
        })
    }
    handleOk() {
        let { Id } = this.state;
        me.formRef1.current.validateFields()
            .then(async (values) => {
                values.ID = Id;
                message.loading('数据处理中...', 0);
                let result = await request('/api/BOM/Copy', {
                    data: values,
                    method: 'POST'
                });
                if (result.status === "ok") {
                    message.destroy();
                    me.setState({ Id: '', isModalVisible: false, version: '' });
                    me.actionRef.current.reload();
                    me.formRef1.current.resetFields();
                    message.success(result.message);
                }
                else
                    message.error(result.message);

            });
    }
    render() {
        let { dispatch, psbom: { moduleInfo, tableParam } } = this.props;
        let { isModalVisible, version } = this.state;
        moduleInfo.modelName = modelName;

        moduleInfo.columns && moduleInfo.columns.map((item, index) => {
            if (item.dataIndex == 'MaterialId') {
                moduleInfo.columns[index].renderFormItem = () => (
                    <ComboGrid api="/api/Material/GetPageList"
                        itemkey="ID"
                        itemvalue={['MaterialNo', 'MaterialNames']}
                        onClear={(value) => {
                            if (tableParam.params.MaterialId) {
                                delete tableParam.params.MaterialId
                            }
                        }}
                    />
                )
                // } else if (item.dataIndex == 'StockId') {
                //     moduleInfo.columns[index].renderFormItem = () => (
                //         <ComboGrid api="/api/Stock/GetPageList"
                //             itemkey="ID"
                //             itemvalue="StockNames"
                //             onClear={(value) => {
                //                 if (tableParam.params.StockId) {
                //                     delete tableParam.params.StockId
                //                 }
                //             }}
                //         />
                //     )
            }
        })

        const CopyBOM = (Id) => {
            me.setState({ Id, isModalVisible: true })
        }

        //#region 操作栏按钮方法
        const action = {
            CopyBOM
        }
        //#endregion
        const onChange = e => {
            me.setState({ version: e.target.value, version: '' })
            console.log(e.target.value);
        };
        return (
            (<div>
                <SmProTable
                    columns={moduleInfo.columns}
                    delete={Delete}
                    batchDelete={(selectedRows) => BatchDelete(selectedRows)}
                    moduleInfo={moduleInfo}
                    {...action}
                    changePage={Index.changePage}
                    formPage={FormPage}
                    actionRef={this.actionRef}
                    formRef={this.formRef}
                    form={{ labelCol: { span: 6 } }}
                    // onReset={() => {
                    //     dispatch({
                    //         type: 'psbom/setTableStatus',
                    //         payload: {},
                    //     })
                    // }}
                    onLoad={() => {
                        if (tableParam && tableParam.params && this.formRef.current) {
                            this.formRef.current.setFieldsValue({ ...tableParam.params })
                        }
                    }}
                    pagination={tableParam && tableParam.params ? { current: tableParam.params.current } : {}}
                    request={(params, sorter, filter) => {
                        if (tableParam && tableParam.params && !params._timestamp)
                            params = Object.assign(tableParam.params, params);
                        if (tableParam && tableParam.sorter)
                            sorter = Object.assign(tableParam.sorter, sorter);
                        dispatch({
                            type: modelName + '/setTableStatus',
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
                <Modal title="复制BOM" open={isModalVisible}
                    onOk={() => me.handleOk()}
                    onCancel={() => me.setState({ Id: '', isModalVisible: false })}>
                    <Form
                        labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                        wrapperCol={{ span: 16 }}
                        ref={this.formRef1}
                    >
                        <Row gutter={24} justify={"center"}>
                            <Col span={24}>
                                <FormItem name="Version" label="版本" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" onChange={onChange} />
                                </FormItem>
                            </Col>
                        </Row>
                    </Form>
                </Modal>
            </div>)
        );
    }
}
export default connect(({ psbom }) => ({
    psbom,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);