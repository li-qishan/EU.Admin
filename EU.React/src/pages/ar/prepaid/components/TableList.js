import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';

let moduleCode = "AR_PREPAID_ORDER_MNG";
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
        const { dispatch } = this.props;
        dispatch({
            type: 'arprepaid/getModuleInfo',
            payload: { moduleCode },
        })
    }
    render() {
        const { dispatch, arprepaid: { moduleInfo, tableParam } } = this.props;

        //#region 操作栏按钮方法
        const action = {}
        //#endregion

        moduleInfo.columns && moduleInfo.columns.map((item, index) => {
            if (item.dataIndex == 'Source') {
                moduleInfo.columns[index].valueEnum = {
                    'Material': {
                        text: '基础物料',
                    },
                    'Sales': {
                        text: '销售单',
                    },
                    'PdPlan': {
                        text: '生产计划单',
                    }
                };
            }
        })

        return (
            <div>
                <SmProTable
                    columns={moduleInfo.columns}
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
                            type: 'arprepaid/setTableStatus',
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
                            type: 'arprepaid/setTableStatus',
                            payload: { params, sorter },
                        })
                        return query({
                            paramData: JSON.stringify(params),
                            sorter: JSON.stringify(sorter),
                            filter: JSON.stringify(filter),
                            moduleCode
                        })
                    }}
                />
            </div>
        )
    }
}
export default connect(({ arprepaid }) => ({
    arprepaid,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);