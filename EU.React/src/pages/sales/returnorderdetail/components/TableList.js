import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import SmProTable from '@/components/SysComponents/SmProTable';
import DetailForm from './FormPage';
import FormEdit from './FormEdit';

let moduleCode = "SD_RETURN_ORDER_DETAIL_MNG";
let me;
class TableList extends Component {
    formRef = React.createRef();
    detailActionRef = React.createRef();

    constructor(props) {
        super(props);
        me = this;
        me.state = {
            masterId: '',
            DetailId: '',
            detailVisible: false,//表单是否显示
            editVisible: false,//表单是否显示
        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        dispatch({
            type: 'returnorderdetail/getModuleInfo',
            payload: { moduleCode }
        });
        this.setState({ masterId: Id })
    }
    static reloadData() {
        me.detailActionRef.current.reload();
    }
    // componentDidMount() {
    //     const { dispatch, Id } = this.props;
    //     mainId = Id;
    // }
    // componentWillReceiveProps(nextProps) {
    //     const { dispatch, Id } = nextProps;
    //     mainId = Id;
    // }
    render() {
        const { dispatch, returnorderdetail: { moduleInfo, tableParam }, IsView } = this.props;
        const { masterId, DetailId, detailVisible, detailView, editVisible } = this.state;

        moduleInfo.columns && moduleInfo.columns.map((item, index) => {
            if (item.dataIndex == 'OutOrderNo' || item.dataIndex == 'SalesOrderNo' || item.dataIndex == 'MaterialNo' || item.dataIndex == 'MaterialName') {
                moduleInfo.columns[index].render = (text, record) => (
                    <a onClick={() => {
                        let tempRowId = null;
                        let url = null;
                        if (item.dataIndex == 'OutOrderNo'){
                            tempRowId = record.OutOrderId;
                            url='/sales/outorder';
                        }
                        else if (item.dataIndex == 'MaterialNo' || item.dataIndex == 'MaterialName'){
                            tempRowId = record.MaterialId;
                            url='/basedata/material';
                        }
                        else if (item.dataIndex == 'SalesOrderNo'){
                            tempRowId = record.SalesOrderId;
                            url='/sales/salesorder';
                        }

                        if (tempRowId)
                            localStorage.setItem("tempRowId", tempRowId)
                        if (url)
                            window.open(url);
                            
                    }}>{text}</a>
                )
                // } else if (item.dataIndex == 'MaterialNo' || item.dataIndex == 'MaterialName') {
                //     moduleInfo.columns[index].render = (text, record) => (
                //         <a onClick={() => {
                //             localStorage.setItem("tempRowId", record.MaterialId)
                //             window.open("/basedata/material");
                //         }}>{text}</a>
                //     )
                // }else if (item.dataIndex == 'SalesOrderNo') {
                //     moduleInfo.columns[index].render = (text, record) => (
                //         <a onClick={() => {
                //             localStorage.setItem("tempRowId", record.SalesOrderId)
                //             window.open("/sales/salesorder");
                //         }}>{text}</a>
                //     )
            }
        })

        //#region 操作栏按钮方法
        const action = {}
        //#endregion

        return (
            <div>
                <SmProTable
                    columns={moduleInfo.columns}
                    actionRef={this.detailActionRef}
                    delete={Delete}
                    batchDelete={(selectedRows) => BatchDelete(selectedRows)}
                    moduleInfo={moduleInfo}
                    IsView={IsView}
                    {...action}
                    changePage={Index.changePage}
                    // formPage={FormPage}
                    // formRef={this.formRef}
                    // form={{ labelCol: { span: 6 } }}
                    addPage={() => this.setState({ detailVisible: true, DetailId: '', detailView: false })}
                    editPage={(record) => this.setState({ editVisible: true, DetailId: record.ID, detailView: false })}
                    viewPage={(record) => this.setState({ editVisible: true, DetailId: record.ID, detailView: true })}
                    onReset={() => {
                        dispatch({
                            type: 'returnorderdetail/setTableStatus',
                            payload: {},
                        })
                    }}
                    onLoad={() => {
                        if (tableParam && tableParam.params && this.formRef.current) {
                            this.formRef.current.setFieldsValue({ ...tableParam.params })
                        }
                    }}
                    search={false}
                    pagination={tableParam && tableParam.params ? { current: tableParam.params.current } : {}}
                    request={(params, sorter, filter) => {
                        if (tableParam && tableParam.params && !params._timestamp)
                            params = Object.assign(tableParam.params, params);
                        if (tableParam && tableParam.sorter)
                            sorter = Object.assign(tableParam.sorter, sorter);
                        dispatch({
                            type: 'returnorderdetail/setTableStatus',
                            payload: { params, sorter },
                        });
                        filter.OrderId = masterId;
                        return query({
                            paramData: JSON.stringify(params),
                            sorter: JSON.stringify(sorter),
                            filter: JSON.stringify(filter),
                            moduleCode, OrderId: masterId
                        })
                    }}
                />
                <DetailForm
                    modalVisible={detailVisible}
                    Id={DetailId}
                    masterId={masterId}
                    IsView={detailView}
                    onSubmit={async values => {
                        const { dispatch } = this.props;
                        // if (DetailId) {
                        //     values.ID = DetailId;
                        // }
                        // if (masterId) {
                        //     values.OrderId = masterId;
                        // }
                        for (let i = 0; i < values.length; i++)
                            values[i].OrderId = masterId;
                        dispatch({
                            type: 'returnorderdetail/batchSaveData',
                            payload: values,
                        }).then(() => {
                            const { returnorderdetail: { Id } } = me.props;
                            // this.setState({ DetailId: Id })
                            this.setState({ detailVisible: false })
                            if (me.detailActionRef.current) {
                                me.detailActionRef.current.reload();
                            }
                        });
                    }}
                    onCancel={() => { this.setState({ detailVisible: false }) }}
                />
                <FormEdit
                    modalVisible={editVisible}
                    Id={DetailId}
                    IsView={detailView}
                    onSubmit={async values => {
                        const { dispatch } = this.props;
                        // if (DetailId) {
                        //     values.ID = DetailId;
                        // }
                        if (masterId) {
                            values.OrderId = masterId;
                        }
                        dispatch({
                            type: 'returnorderdetail/saveData',
                            payload: values,
                        }).then(() => {
                            const { returnorderdetail: { Id } } = me.props;
                            this.setState({ DetailId: Id })
                            if (me.detailActionRef.current) {
                                me.detailActionRef.current.reload();
                            }
                        });
                    }}
                    onCancel={() => { this.setState({ editVisible: false }) }}
                />
            </div>
        )
    }
}
export default connect(({ returnorderdetail }) => ({
    returnorderdetail
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);