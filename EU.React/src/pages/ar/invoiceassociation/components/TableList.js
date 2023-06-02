import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import SmProTable from '@/components/SysComponents/SmProTable';
import DetailForm from './FormPage';
import { message } from 'antd';

let moduleCode = "AR_INVOICE_ASSOCIATION_MNG";
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
        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        dispatch({
            type: 'arinvoiceassociation/getModuleInfo',
            payload: { moduleCode }
        });
        this.setState({ masterId: Id })
    }
    componentDidMount() {
        const { Id } = this.props;
        this.setState({ masterId: Id })
    }
    componentWillReceiveProps(nextProps) {
        const { Id } = nextProps;
        this.setState({ masterId: Id })
    }
    setDetailId(detailVisible, id, detailView) {
        const { dispatch } = this.props;
        dispatch({
            type: 'arinvoiceassociation/setId',
            payload: id
        }).then(() => {
            me.setState({ detailVisible, detailView });
        });
    }
    render() {
        const { dispatch, arinvoiceassociation: { moduleInfo, tableParam }, IsView } = this.props;
        const { masterId, DetailId, detailVisible, detailView } = this.state;

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
                    search={false}
                    IsView={IsView}
                    changePage={Index.changePage}
                    // formPage={FormPage}
                    // formRef={this.formRef}
                    // form={{ labelCol: { span: 6 } }}
                    addPage={() => {
                        if (!masterId) {
                            message.error('请先保存主表数据！');
                            return false;
                        }
                        this.setDetailId(true, '', false)
                    }}
                    editPage={(record) => {
                        this.setDetailId(true, record.ID, false)
                    }}
                    viewPage={(record) => {
                        this.setDetailId(true, record.ID, true)
                    }}
                    onReset={() => {
                        dispatch({
                            type: 'arinvoiceassociation/setTableStatus',
                            payload: {},
                        })
                    }}
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
                            type: 'arinvoiceassociation/setTableStatus',
                            payload: { params, sorter },
                        });
                        filter.OrderId = masterId;
                        if (masterId)
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
                    IsView={detailView}
                    onSubmit={async (values, calback) => {
                        const { dispatch } = this.props;
                        if (masterId) 
                            values.OrderId = masterId;
                        dispatch({
                            type: 'arinvoiceassociation/saveData',
                            payload: values,
                        }).then((result) => {
                            if (result && result.status === "ok") {
                                if (calback)
                                    calback();

                                if (me.detailActionRef.current)
                                    me.detailActionRef.current.reload();
                            }
                        });
                    }}
                    onCancel={() => { this.setState({ detailVisible: false }) }}
                />
            </div>
        )
    }
}
export default connect(({ arinvoiceassociation }) => ({
    arinvoiceassociation
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);