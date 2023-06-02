import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';

let moduleCode = "SM_SCHEDULE_MNG";
let me;
class TableList extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {

        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        dispatch({
            type: 'smschedule/getModuleInfo',
            payload: { moduleCode },
        })
    }
    render() {
        const { dispatch, smschedule: { moduleInfo, tableParam }, user: { currentUser } } = this.props;
        const columns = [
            {
                title: '标题',
                dataIndex: 'Title'
            },
            {
                title: '内容',
                dataIndex: 'Content'
            },
            {
                title: '状态',
                dataIndex: 'Status',
                filters: false,
                valueEnum: {
                    'Add': {
                        text: '新增',
                    },
                    'Auditing': {
                        text: '审批中',
                    },
                    'ComplateAudit': {
                        text: '已审批',
                    },
                    'Goback': {
                        text: '退回',
                    },
                }
            },
        ];

        //#region 操作栏按钮方法
        const action = {}
        //#endregion

        return (
            <div>
                <SmProTable
                    search={false}
                    columns={columns}
                    delete={Delete}
                    batchDelete={(selectedRows) => BatchDelete(selectedRows)}
                    moduleInfo={moduleInfo}
                    {...action}
                    changePage={Index.changePage}
                    formPage={FormPage}
                    formRef={this.formRef}
                    form={{ labelCol: { span: 6 } }}
                    onReset={() => {
                        dispatch({
                            type: 'smschedule/setTableStatus',
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
                        if (tableParam && tableParam.params && !params._timestamp) {
                            params = tableParam.params;
                            //sorter = tableParam.sorter
                        }
                        dispatch({
                            type: 'smschedule/setTableStatus',
                            payload: { params, sorter },
                        })
                        filter.UserId = currentUser.data.userid;
                        return query({
                            paramData: JSON.stringify(params),
                            sorter: JSON.stringify(sorter),
                            filter: JSON.stringify(filter)
                        })
                    }}
                />
            </div>
        )
    }
}
export default connect(({ smschedule, user, loading }) => ({
    smschedule,
    user
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);