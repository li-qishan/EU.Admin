import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';

let moduleCode = "SM_AUTOCODE_MNG";
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
            type: 'autocode/getModuleInfo',
            payload: { moduleCode },
        })
    }
    render() {
        const { dispatch, autocode: { moduleInfo, tableParam } } = this.props;

        const columns = [
            {
                title: '编号代码',
                dataIndex: 'NumberCode'
            },
            {
                title: '前缀',
                dataIndex: 'Prefix'
            },
            {
                title: '日期格式',
                dataIndex: 'DateFormatType',
                filterMultiple: false,
                hideInSearch: true,
                valueEnum: {
                    0: {
                        text: 'YYYYMMDDHHMM',
                    },
                    1: {
                        text: 'YYYYMMDDHH',
                    },
                    2: {
                        text: 'YYYYMMDD',
                    },
                    3: {
                        text: 'YYYYMM',
                    },
                    4: {
                        text: 'YYYY',
                    },
                },
            },
            {
                title: '长度',
                hideInSearch: true,
                dataIndex: 'NumberLength'
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
                            type: 'autocode/setTableStatus',
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
                            type: 'autocode/setTableStatus',
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
export default connect(({ autocode, loading }) => ({
    autocode,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);