import React, { Component, Suspense } from 'react';
import { connect } from 'umi';
import SmProTable from '@/components/SysComponents/SmProTable';
import DetailForm from './DetailForm';
import { queryDetail, BatchDetailDelete, DeleteDetail } from '../service'

let moduleCode = "SM_SETPARAM_DETAIL_MNG";
let me;
let mainId;
class DetailTableList extends Component {
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
            type: 'setparam/getDetailModuleInfo',
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
        const { dispatch, setparam: { detailModuleInfo } } = this.props;
        const { DetailId, detailVisible, detailView } = this.state;

        //#region 操作栏按钮方法
        const action = {}
        //#endregion

        const Columns = [
            {
                title: '排序号',
                dataIndex: 'TaxisNo',
                hideInSearch: true,
                valueType: 'digit',
                sorter: true
            },
            {
                title: '参数值',
                dataIndex: 'Value'
            },
            {
                title: '参数名称',
                dataIndex: 'Text'
            },
        ]

        return (
            <div>
                <SmProTable
                    search={false}
                    columns={Columns}
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
                            moduleCode,
                            paramData: JSON.stringify(params),
                            sorter: JSON.stringify(sorter),
                            filter: JSON.stringify(filter),
                            parentColumn: 'SmLovId', parentId: mainId
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
                            values.SmLovId = mainId;
                        }
                        dispatch({
                            type: 'setparam/saveDetailData',
                            payload: values,
                        }).then(() => {
                            const { dispatch, setparam: { DetailId } } = me.props;
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
export default connect(({ setparam, loading }) => ({
    setparam,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(DetailTableList);