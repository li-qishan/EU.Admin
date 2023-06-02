import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import SmProTable from '@/components/SysComponents/SmProTable';
import DetailForm from './FormPage';

let moduleCode = "SM_TABLE_CATALOG_DETAIL_MNG";
let me;
let modelName = 'smtabledetail';

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
            type: modelName + '/getModuleInfo',
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
        let { dispatch, smtabledetail: { moduleInfo, tableParam }, IsView } = this.props;
        const { masterId, DetailId, detailVisible, detailView } = this.state;
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
                    addPage={() => this.setState({ detailVisible: true, DetailId: '', detailView: false })}
                    editPage={(record) => this.setState({ detailVisible: true, DetailId: record.ID, detailView: IsView == true ? true : false })}
                    viewPage={(record) => this.setState({ detailVisible: true, DetailId: record.ID, detailView: true })}
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
                            type: modelName + '/setTableStatus',
                            payload: { params, sorter },
                        });
                        // params.TableCode = masterId;
                        return query({
                            paramData: JSON.stringify(params),
                            sorter: JSON.stringify(sorter),
                            filter: JSON.stringify(filter),
                            moduleCode,
                            parentColumn: 'TableCode',
                            parentId: masterId
                        })
                    }}
                    search={false}

                />
                <DetailForm
                    modalVisible={detailVisible}
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
                            type: 'smtabledetail/saveData',
                            payload: values,
                        }).then(() => {
                            const { smtabledetail: { Id } } = me.props;
                            this.setState({ DetailId: Id })
                            if (me.detailActionRef.current) {
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
export default connect(({ smtabledetail }) => ({
    smtabledetail
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);