import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import SmProTable from '@/components/SysComponents/SmProTable';
import DetailForm from './FormPage';
import FormEdit from './FormEdit';

let moduleCode = "IV_STOCK_TRANSFERS_DETAIL_MNG";
let me;
let modelName = 'ivtransfersdetail';

class TableList extends Component {
    formRef = React.createRef();
    detailActionRef = React.createRef();

    constructor(props) {
        super(props);
        me = this;
        me.state = {
            masterId: '',
            detailVisible: false,//表单是否显示
            editVisible: false,//表单是否显示
        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        dispatch({
            type: modelName + '/getModuleInfo',
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
            type: modelName + '/setId',
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
        let { dispatch, ivtransfersdetail: { moduleInfo, tableParam }, IsView } = this.props;
        const { masterId, detailVisible, editVisible, detailView } = this.state;
        moduleInfo.modelName = modelName;

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
                            type: modelName + '/setTableStatus',
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
                            type: modelName + '/batchSaveData',
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
                            type: modelName + '/saveData',
                            payload: values,
                        }).then(() => {
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
export default connect(({ ivtransfersdetail }) => ({
    ivtransfersdetail
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);