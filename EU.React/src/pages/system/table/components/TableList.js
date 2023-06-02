import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import { Modal, Button, Form, Row, Col, message } from 'antd';
import Index from '../index';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import { InitAssignmentTable, InitAllTable } from '../service';

const FormItem = Form.Item;

let moduleCode = "SM_TABLE_CATALOG_MNG";
let me;
let modelName = 'smtable';

class TableList extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            modalVisible: false,
            tableCode: ''
        };
    }
    componentWillMount() {
        const { dispatch } = this.props;
        dispatch({
            type: modelName + '/getModuleInfo',
            payload: { moduleCode },
        })
    }
    InitAssignmentTable = async () => {
        const { tableCode } = this.state;
        if (!tableCode) {
            message.error("请选择表代码!");
            return false;
        }
        message.loading('数据提交中...', 0);
        let response = await InitAssignmentTable({ tableCode });
        message.destroy();
        if (response.status == "ok") {
            message.success(response.message);
        } else {
            message.error(response.message);
        }
    };
    InitAllTable = async () => {
        message.loading('数据提交中...', 0);
        let response = await InitAllTable({});
        message.destroy();
        if (response.status == "ok") {
            message.success(response.message);
        } else {
            message.error(response.message);
        }
    };
    render() {
        let { dispatch, smtable: { moduleInfo, tableParam } } = this.props;
        moduleInfo.modelName = modelName;
        const { modalVisible } = this.state;

        //#region 操作栏按钮方法
        const InitTableCatalog = (Id) => {
            // Index.changePage(<SqlEdit Id={Id} />)
            this.setState({ modalVisible: true })
        }
        //#region 操作栏按钮方法
        const action = {
            InitTableCatalog
        }
        //#endregion

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
                    formRef={this.formRef}
                    form={{ labelCol: { span: 6 } }}
                    // onReset={() => {
                    //     dispatch({
                    //         type: 'smtable/setTableStatus',
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
                <Modal
                    destroyOnClose
                    title='初始化表'
                    open={modalVisible}
                    // onOk={this.okHandle}
                    maskClosable={false}
                    width={1000}
                    onCancel={() => {
                        this.setState({ modalVisible: false })
                    }}
                    footer={null}
                >
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                        </Col>
                        <Col span={8}>
                            <FormItem name="GoodsLocationId" label="表代码">
                                <ComboGrid
                                    api="/api/SmTableCatalog/GetPageList"
                                    itemkey="TableCode"
                                    itemvalue="TableCode"
                                    onChange={(tableCode, Option, data) => {
                                        this.setState({ tableCode })
                                    }}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={6}>
                        </Col>
                        <Col span={6} style={{ textAlign: 'right' }}>
                            <Button type="primary" onClick={this.InitAllTable}>初始化所有表</Button>
                        </Col>
                        <Col span={6}>
                            <Button type="primary" onClick={this.InitAssignmentTable}>初始化指定表</Button>
                        </Col>
                        <Col span={6}>
                        </Col>
                    </Row>
                </Modal>
            </div>)
        );
    }
}
export default connect(({ smtable }) => ({
    smtable,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);