import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';

let moduleCode = "IV_STOCK_INIT_MNG";
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
            type: 'ivinit/getModuleInfo',
            payload: { moduleCode },
        })
    }
    // async initData(action) {
    //     Modal.confirm({
    //         title: '确认执行数据初始化？？',
    //         icon: Utility.getIcon('ExclamationCircleOutlined'),
    //         // content: '是否确定删除记录？',
    //         okText: '确定',
    //         okType: 'danger',
    //         cancelText: '取消',
    //         async onOk() {
    //             const filehide = message.loading('数据初始化中..', 0)
    //             let result = await request('/api/IvInit/Init', {
    //                 method: 'POST',
    //                 data: []
    //             });
    //             setTimeout(filehide);
    //             if (result.status == "ok") {
    //                 message.success(result.message)
    //             }
    //             else {
    //                 message.error(result.message);
    //             }
    //             action.reload();
    //         },
    //         onCancel() {
    //             console.log('Cancel');
    //         },
    //     });

    // }
    render() {
        const { dispatch, ivinit: { moduleInfo, tableParam } } = this.props;
        moduleInfo.modelName = 'ivinit';
        //#region 操作栏按钮方法
        // const InitStock = (action) => {
        //     me.initData(action);
        // }
        //#region 操作栏按钮方法
        const action = {

        }
        //#endregion

        return (
            <div>
                <SmProTable
                    delete={Delete}
                    batchDelete={(selectedRows) => BatchDelete(selectedRows)}
                    moduleInfo={moduleInfo}
                    {...action}
                    changePage={Index.changePage}
                    formPage={FormPage}
                    formRef={this.formRef}
                    form={{ labelCol: { span: 6 } }}
                    // onReset={() => {
                    //     debugger
                    //     dispatch({
                    //         type: 'ivinit/setTableStatus',
                    //         payload: {},
                    //     })
                    // }}
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
                            type: 'ivinit/setTableStatus',
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
export default connect(({ ivinit }) => ({
    ivinit,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);
