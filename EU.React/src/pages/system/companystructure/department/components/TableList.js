import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';

let moduleCode = "SM_DEPARTMENT_MNG";
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
            type: 'department/getModuleInfo',
            payload: { moduleCode },
        })
    }
    render() {
        const { dispatch, department: { moduleInfo, tableParam } } = this.props;

        const columns = [
            {
                title: '部门编码',
                dataIndex: 'DepartmentCode'
            },
            {
                title: '部门名称',
                dataIndex: 'DepartmentName'
            },
            {
                title: '所属组织',
                dataIndex: ['Company', 'CompanyName']
            },
            {
                title: '上级部门',
                dataIndex: ['Department', 'DepartmentName']
            },
        ];

        //#region 操作栏按钮方法
        const action = {}
        //#endregion

        return (
            <div>
                <SmProTable
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
                            type: 'department/setTableStatus',
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
                            type: 'department/setTableStatus',
                            payload: { params, sorter },
                        })
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
export default connect(({ department, loading }) => ({
    department,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);