import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';

let moduleCode = "SM_COMPANY_MNG";
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
            type: 'company/getModuleInfo',
            payload: { moduleCode },
        })
    }
    render() {
        const { dispatch, company: { moduleInfo, tableParam } } = this.props;
        const columns = [
            {
                title: '组织编码',
                dataIndex: 'CompanyCode'
            },
            {
                title: '组织名称',
                dataIndex: 'CompanyName'
            },
            {
                title: '组织简称',
                dataIndex: 'CompanyShortName'
            }
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
                            type: 'company/setTableStatus',
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
                            type: 'company/setTableStatus',
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
export default connect(({ company, loading }) => ({
    company,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);