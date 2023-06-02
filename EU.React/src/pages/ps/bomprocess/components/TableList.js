import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import SmProTable from '@/components/SysComponents/SmProTable';
import DetailForm from './FormPage';
import { message } from 'antd';
import FormEdit from './FormEdit';

let moduleCode = "PS_PROCESS_BOM_PROCESS_MNG";
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
            type: 'psbomprocess/getModuleInfo',
            payload: { moduleCode }
        });
        this.setState({ masterId: Id })
    }
    static reloadData(Id) {
        if (me && me.detailActionRef)
        me.setState({
            masterId: Id
        }, () => {
            me.detailActionRef.current.reload();
        });
    }
    setDetailId(editVisible, id, detailView) {
        const { dispatch } = this.props;
        dispatch({
            type: 'psbomprocess/setId',
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
        const { dispatch, psbomprocess: { moduleInfo, tableParam }, IsView } = this.props;
        const { masterId, DetailId, detailVisible, detailView, editVisible } = this.state;

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
                    // addPage={() => this.setState({ detailVisible: true, DetailId: '', detailView: false })}
                    addPage={() => {
                        if (!masterId) {
                            message.error('请先保存主表数据！');
                            return false;
                        }
                        me.setState({ detailVisible: true, detailView: false });
                    }}
                    editPage={(record) => {
                        this.setDetailId(true, record.ID, IsView == true ? true : false)
                    }}
                    viewPage={(record) => {
                        this.setDetailId(true, record.ID, true)
                    }}
                    onReset={() => {
                        dispatch({
                            type: 'psbomprocess/setTableStatus',
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
                            type: 'psbomprocess/setTableStatus',
                            payload: { params, sorter },
                        });
                        filter.BOMId = masterId;
                        if (masterId)
                            return query({
                                paramData: JSON.stringify(params),
                                sorter: JSON.stringify(sorter),
                                filter: JSON.stringify(filter),
                                moduleCode, BOMId: masterId
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
                            values[i].BOMId = masterId;
                        dispatch({
                            type: 'psbomprocess/batchSaveData',
                            payload: values,
                        }).then(() => {
                            const { psbomprocess: { Id } } = me.props;
                            // this.setState({ DetailId: Id })
                            this.setState({ detailVisible: false })
                            // arrivalme.setState({ detailCount: 1 })
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
                        if (masterId)
                            values.BOMId = masterId;
                        dispatch({
                            type: 'psbomprocess/saveData',
                            payload: values,
                        }).then(() => {
                            if (result && result.status === "ok") {
                                if (calback)
                                    calback();
                                const { psbomprocess: { Id } } = me.props;
                                this.setState({ DetailId: Id })
                                // arrivalme.setState({ detailCount: 1 })
                                if (me.detailActionRef.current)
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
export default connect(({ psbomprocess }) => ({
    psbomprocess
}))(TableList);