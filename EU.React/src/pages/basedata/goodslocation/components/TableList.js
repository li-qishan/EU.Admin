import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import SmProTable from '@/components/SysComponents/SmProTable';
import DetailForm from './FormPage';

let moduleCode = "BD_GOOD_LOCATION_MNG";
let me;
class TableList extends Component {
    formRef = React.createRef();
    detailActionRef = React.createRef();

    constructor(props) {
        super(props);
        me = this;
        me.state = {
            masterId: '',
            detailVisible: false,//表单是否显示
        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        dispatch({
            type: 'goodslocation/getModuleInfo',
            payload: { moduleCode }
        });
        this.setState({ masterId: Id })
    }
    // componentDidMount() {
    //     const { dispatch, Id } = this.props;
    //     mainId = Id;
    // }
    // componentWillReceiveProps(nextProps) {
    //     const { dispatch, Id } = nextProps;
    //     mainId = Id;
    // }
    setDetailId(detailVisible, id, detailView) {
        const { dispatch } = this.props;
        dispatch({
            type: 'goodslocation/setId',
            payload: id
        }).then(() => {
            me.setState({ detailVisible, detailView });
        });
    }
    render() {
        const { dispatch, goodslocation: { moduleInfo, tableParam }, IsView } = this.props;
        const { masterId, detailVisible, detailView } = this.state;

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
                        //this.setState({ detailVisible: true, DetailId: '', detailView: false });
                        this.setDetailId(true, '', false)
                    }}
                    editPage={(record) => {
                        //this.setState({ detailVisible: true, DetailId: record.ID, detailView: false })
                        this.setDetailId(true, record.ID, false)
                    }}
                    viewPage={(record) => {
                        //this.setState({ detailVisible: true, DetailId: record.ID, detailView: true })
                        this.setDetailId(true, record.ID, true)
                    }}
                    onReset={() => {
                        dispatch({
                            type: 'goodslocation/setTableStatus',
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
                        dispatch({
                            type: 'goodslocation/setTableStatus',
                            payload: { params, sorter },
                        });
                        filter.StockId = masterId;
                        return query({
                            paramData: JSON.stringify(params),
                            sorter: JSON.stringify(sorter),
                            filter: JSON.stringify(filter),
                            moduleCode, StockId: masterId
                        })
                    }}
                />
                <DetailForm
                    modalVisible={detailVisible}
                    IsView={detailView}
                    onSubmit={async (values, calback) => {
                        const { dispatch } = this.props;
                        if (masterId)
                            values.StockId = masterId;

                        dispatch({
                            type: 'goodslocation/saveData',
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
export default connect(({ goodslocation }) => ({
    goodslocation
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);