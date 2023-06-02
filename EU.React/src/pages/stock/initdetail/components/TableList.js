import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import SmProTable from '@/components/SysComponents/SmProTable';
import DetailForm from './FormPage';

let moduleCode = "IV_STOCK_INIT_DETAIL_MNG";
let me;
class TableList extends Component {
    formRef = React.createRef();
    detailActionRef = React.createRef();

    constructor(props) {
        super(props);
        me = this;
        me.state = {
            masterId: '',
            detailVisible: false//表单是否显示
        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        dispatch({
            type: 'ivinitdetail/getModuleInfo',
            payload: { moduleCode }
        });
        this.setState({ masterId: Id })
    }
    static reloadData() {
        me.detailActionRef.current.reload();
    }
    setDetailId(detailVisible, id, detailView) {
        const { dispatch } = this.props;
        dispatch({
            type: 'ivinitdetail/setId',
            payload: id
        }).then(() => {
            me.setState({ detailVisible, detailView });
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
        let { dispatch, ivinitdetail: { moduleInfo, tableParam }, IsView } = this.props;
        const { masterId, detailVisible, detailView } = this.state;
        moduleInfo.modelName = 'ivinitdetail';
        moduleInfo.masterId = masterId;

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
                        this.setDetailId(true, null, IsView == true ? true : false)
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
                            type: 'ivinitdetail/setTableStatus',
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
                        if (masterId)
                            values.OrderId = masterId;

                        dispatch({
                            type: 'ivinitdetail/saveData',
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
            </div>
        )
    }
}
export default connect(({ ivinitdetail }) => ({
    ivinitdetail
}))(TableList);