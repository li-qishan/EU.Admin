import React, { Component } from 'react';
import { connect } from 'umi';
import SmProTable from '@/components/SysComponents/SmProTable';
import DetailForm from './DetailForm';
import { queryDetail, BatchDetailDelete, DeleteDetail } from '../service'

let moduleCode = "SM_MODULE_COLUMN_MNG";
let me;
let mainId;
class ColumnTable extends Component {
    detailActionRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            DetailId: '',//从表主键
            detailVisible: false,//表单是否显示
        };
    }
    componentWillMount() {
        const { dispatch } = this.props;
        dispatch({
            type: 'module/getDetailModuleInfo',
            payload: { moduleCode },
        })
    }
    componentDidMount() {
        const { dispatch, Id } = this.props;
        mainId = Id;
    }
    componentWillReceiveProps(nextProps) {
        const { dispatch, Id } = nextProps;
        mainId = Id;
    }
    render() {
        const { dispatch, module: { detailModuleInfo } } = this.props;
        const { DetailId, detailVisible, detailView } = this.state;

        //#region 操作栏按钮方法
        const action = {}
        //#endregion

        // const Columns = [
        //     {
        //         title: '排序号',
        //         dataIndex: 'TaxisNo',
        //         hideInSearch: true,
        //         valueType: 'digit',
        //         sorter: true
        //     },
        //     {
        //         title: '列名',
        //         dataIndex: 'title'
        //     },
        //     {
        //         title: '数据列',
        //         dataIndex: 'dataIndex'
        //     },
        //     {
        //         title: '是否隐藏',
        //         dataIndex: 'hideInTable',
        //         filters: false,
        //         valueEnum: {
        //             true: {
        //                 text: '是',
        //                 status: true,
        //             },
        //             false: {
        //                 text: '否',
        //                 status: false,
        //             },
        //         },
        //     },
        // ]

        return (
            <div>
                <SmProTable
                    search={false}
                    columns={detailModuleInfo.columns}
                    actionRef={this.detailActionRef}
                    moduleInfo={detailModuleInfo}
                    {...action}
                    delete={DeleteDetail}
                    batchDelete={(selectedRows) => BatchDetailDelete(selectedRows)}
                    addPage={() => this.setState({ detailVisible: true, DetailId: '', detailView: false })}
                    editPage={(record) => this.setState({ detailVisible: true, DetailId: record.ID, detailView: false })}
                    viewPage={(record) => this.setState({ detailVisible: true, DetailId: record.ID, detailView: true })}
                    request={(params, sorter, filter) =>
                        queryDetail({
                            paramData: JSON.stringify(params),
                            sorter: JSON.stringify(sorter),
                            filter: JSON.stringify(filter),
                            moduleCode,
                            parentColumn: 'SmModuleId', parentId: mainId
                        })}
                />
                <DetailForm
                    modalVisible={detailVisible}
                    Id={DetailId}
                    IsView={detailView}
                    onSubmit={async values => {
                        const { dispatch } = this.props;
                        if (DetailId) {
                            values.ID = DetailId;
                        }
                        if (mainId) {
                            values.SmModuleId = mainId;
                        }
                        dispatch({
                            type: 'module/saveDetailData',
                            payload: values,
                        }).then(() => {
                            const { dispatch, module: { DetailId } } = me.props;
                            this.setState({ DetailId })
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
export default connect(({ module, loading }) => ({
    module,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(ColumnTable);