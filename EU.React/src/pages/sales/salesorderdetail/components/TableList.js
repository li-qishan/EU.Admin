import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import SmProTable from '@/components/SysComponents/SmProTable';
import DetailForm from './FormPage';
import FormEdit from './FormEdit';

let moduleCode = "SD_SALES_ORDER_DETAIL_MNG";
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
            type: 'salesorderdetail/getModuleInfo',
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
        const { dispatch, salesorderdetail: { moduleInfo, tableParam }, IsView } = this.props;
        const { masterId, DetailId, detailVisible, editVisible, detailView } = this.state;

        moduleInfo.columns && moduleInfo.columns.map((item, index) => {
            if (item.dataIndex == 'MaterialNo') {
                moduleInfo.columns[index].render = (text, record) => (
                    <a onClick={() => {
                        localStorage.setItem("tempRowId", record.MaterialId)
                        window.open("/basedata/material");
                    }}>{text}</a>
                )
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
                    {...action}
                    changePage={Index.changePage}
                    IsView={IsView}
                    // formPage={FormPage}
                    // formRef={this.formRef}
                    // form={{ labelCol: { span: 6 } }}
                    addPage={() => this.setState({ detailVisible: true, DetailId: '', detailView: false })}
                    editPage={(record) => this.setState({ editVisible: true, DetailId: record.ID, detailView: IsView == true ? true : false })}
                    viewPage={(record) => this.setState({ editVisible: true, DetailId: record.ID, detailView: true })}
                    onReset={() => {
                        dispatch({
                            type: 'salesorderdetail/setTableStatus',
                            payload: {},
                        })
                    }}
                    onLoad={() => {
                        if (tableParam && tableParam.params && this.formRef.current)
                            this.formRef.current.setFieldsValue({ ...tableParam.params })
                    }}
                    search={false}
                    pagination={tableParam && tableParam.params ? { current: tableParam.params.current } : {}}
                    request={(params, sorter, filter) => {
                        if (tableParam && tableParam.params && !params._timestamp)
                            params = Object.assign(tableParam.params, params);
                        if (tableParam && tableParam.sorter)
                            sorter = Object.assign(tableParam.sorter, sorter);
                        dispatch({
                            type: 'salesorderdetail/setTableStatus',
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
                    moduleInfo={moduleInfo}
                    Id={DetailId}
                    IsView={detailView}
                    onSubmit={async (values, Id) => {
                        const { dispatch } = this.props;
                        // if (DetailId) {
                        //     values.ID = DetailId;
                        // }

                        if (Id) {
                            if (masterId)
                                values.OrderId = masterId;
                        }
                        else
                            for (let i = 0; i < values.length; i++)
                                values[i].OrderId = masterId;

                        dispatch({
                            type: 'salesorderdetail/batchSaveData',
                            payload: values,
                        }).then(() => {
                            if (Id) {
                                const { salesorderdetail: { Id } } = me.props;
                                this.setState({ DetailId: Id })
                            }
                            this.setState({ detailVisible: false })

                            if (me.detailActionRef.current)
                                me.detailActionRef.current.reload();

                        });
                    }}
                    onCancel={() => { this.setState({ detailVisible: false }) }}
                />

                <FormEdit
                    modalVisible={editVisible}
                    moduleInfo={moduleInfo}
                    Id={DetailId}
                    IsView={detailView}
                    onSubmit={async (values, Id) => {
                        const { dispatch } = this.props;
                        // if (DetailId) {
                        //     values.ID = DetailId;
                        // }

                        if (Id) {
                            if (masterId) {
                                values.OrderId = masterId;
                            }
                        }
                        else
                            for (let i = 0; i < values.length; i++)
                                values[i].OrderId = masterId;

                        dispatch({
                            type: 'salesorderdetail/saveData',
                            payload: values,
                        }).then(() => {
                            if (Id) {
                                const { salesorderdetail: { Id } } = me.props;
                                this.setState({ DetailId: Id })
                            }
                            this.setState({ editVisible: false })

                            if (me.detailActionRef.current)
                                me.detailActionRef.current.reload();

                        });
                    }}
                    onCancel={() => { this.setState({ editVisible: false }) }}
                />
            </div>
        )
    }
}
export default connect(({ salesorderdetail, global }) => ({
    salesorderdetail, global
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);