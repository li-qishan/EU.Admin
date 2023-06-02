import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import SmProTable from '@/components/SysComponents/SmProTable';
import DetailForm from './FormPage';
import { message } from 'antd';
import FormEdit from './FormEdit';

let moduleCode = "PD_REQUIRE_ANALYSIS_MNG";
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
            type: 'pdanalysis/getModuleInfo',
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
    setDetailId(editVisible, id, detailView) {
        const { dispatch } = this.props;
        dispatch({
            type: 'pdanalysis/setId',
            payload: id
        }).then(() => {
            me.setState({ editVisible, detailView });
        });
    }
    static reloadData() {
        me.detailActionRef.current.reload();
    }
    render() {
        const { dispatch, pdanalysis: { moduleInfo, tableParam }, IsView } = this.props;
        const { masterId, DetailId, detailVisible, detailView, editVisible } = this.state;

        //#region 操作栏按钮方法
        const action = {}
        //#endregion

        moduleInfo.columns && moduleInfo.columns.map((item, index) => {
            if (item.dataIndex == 'Source') {
                moduleInfo.columns[index].valueEnum = {
                    'Requestion': {
                        text: '请购单',
                    },
                    'Sales': {
                        text: '销售单',
                    },
                };
            }
        })

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
                        me.setState({ detailVisible: true, detailView: false });
                    }}
                    editPage={(record) => {
                        this.setDetailId(true, record.ID, false)
                    }}
                    viewPage={(record) => {
                        this.setDetailId(true, record.ID, true)
                    }}
                    onReset={() => {
                        dispatch({
                            type: 'pdanalysis/setTableStatus',
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
                            type: 'pdanalysis/setTableStatus',
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
                    masterId={masterId}
                    onSubmit={async (values, calback) => {
                        const { dispatch } = this.props;
                        for (let i = 0; i < values.length; i++) {
                            values[i].OrderId = masterId;
                            values[i].SourceOrderSerialNumber = values[i].SerialNumber;
                        }

                        dispatch({
                            type: 'pdanalysis/batchSaveData',
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
                            type: 'pdanalysis/saveData',
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
export default connect(({ pdanalysis }) => ({
    pdanalysis
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);