import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import SmProTable from '@/components/SysComponents/SmProTable';
import DetailForm from './FormPage';
import FormEdit from './FormEdit';

let moduleCode = "PO_REQUESTION_DETAIL_MNG";
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
            type: 'requestiondetail/getModuleInfo',
            payload: { moduleCode }
        });
        this.setState({ masterId: Id })
    }
    static reloadData() {
        me.detailActionRef.current.reload();
    }
    setDetailId(editVisible, id, detailView) {
        const { dispatch } = this.props;
        dispatch({
            type: 'requestiondetail/setId',
            payload: id
        }).then(() => {
            me.setState({ editVisible, detailView });
        });
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
        let { dispatch, requestiondetail: { moduleInfo, tableParam }, IsView } = this.props;
        const { masterId, detailVisible, editVisible, detailView } = this.state;
        moduleInfo.modelName = 'requestiondetail';

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
                    addPage={() => {
                        me.setState({ detailVisible: true, detailView: false });
                    }}
                    editPage={(record) => {
                        this.setDetailId(true, record.ID, IsView == true ? true : false)
                    }}
                    viewPage={(record) => {
                        this.setDetailId(true, record.ID, true)
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
                            type: 'requestiondetail/setTableStatus',
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
                    search={false}

                />
                <DetailForm
                    modalVisible={detailVisible}
                    IsView={detailView}
                    onSubmit={async (values, calback) => {
                        const { dispatch } = this.props;
                        for (let i = 0; i < values.length; i++)
                            values[i].OrderId = masterId;

                        dispatch({
                            type: 'requestiondetail/batchSaveData',
                            payload: values,
                        }).then((result) => {
                            if (result && result.status === "ok") {
                                if (calback)
                                    calback();
                                this.setState({ detailVisible: false })
                                if (me.detailActionRef.current)
                                    me.detailActionRef.current.reload();
                            }
                        });
                    }}
                    onCancel={() => { this.setState({ detailVisible: false }) }}
                />
                <FormEdit
                    modalVisible={editVisible}
                    moduleInfo={moduleInfo}
                    IsView={detailView}
                    onSubmit={async (values, Id) => {
                        const { dispatch } = this.props;
                        if (Id)
                            if (masterId)
                                values.OrderId = masterId;

                        dispatch({
                            type: 'requestiondetail/saveData',
                            payload: values,
                        }).then(() => {
                            const { requestiondetail: { Id } } = me.props;
                            this.setState({ editVisible: false, DetailId: Id })

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
export default connect(({ requestiondetail }) => ({
    requestiondetail
}))(TableList);