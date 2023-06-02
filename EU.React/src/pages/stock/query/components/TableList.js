import React, { Component } from 'react';
import { connect } from 'umi';
import { query } from '../service';
import Index from '../index';
// import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';
import ComboGrid from '@/components/SysComponents/ComboGrid'

let moduleCode = "IV_STOCK_QUERY_MNG";
let me;
class TableList extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            StockId: ''
        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        dispatch({
            type: 'ivquery/getModuleInfo',
            payload: { moduleCode },
        })
    }
    render() {
        const { dispatch, ivquery: { moduleInfo, tableParam } } = this.props;
        let disabled = !me.state.StockId ? true : false;
        moduleInfo.columns && moduleInfo.columns.map((item, index) => {
            if (item.dataIndex == 'MaterialId') {
                moduleInfo.columns[index].renderFormItem = () => (
                    <ComboGrid
                        api="/api/Material/GetPageList"
                        itemkey="ID"
                        param={{
                            IsActive: true
                        }}
                        // itemvalue="MaterialNo,MaterialNames"
                        itemvalue={['MaterialNo', 'MaterialNames']}
                    />
                )
            } else if (item.dataIndex == 'StockId') {
                moduleInfo.columns[index].renderFormItem = () => (
                    <ComboGrid api="/api/Stock/GetPageList"
                        itemkey="ID"
                        param={{
                            IsActive: true
                        }}
                        itemvalue={['StockNo', 'StockNames']}
                        onChange={(value) => {
                            if (value != me.state.StockId)
                                if (tableParam && tableParam.params.GoodsLocationId)
                                    delete tableParam.params.GoodsLocationId
                            me.setState({ StockId: value })
                        }}
                        onClear={(value) => {
                            if (tableParam.params.StockId)
                                delete tableParam.params.StockId
                        }}
                    />
                )
            } else if (item.dataIndex == 'GoodsLocationId') {
                moduleInfo.columns[index].renderFormItem = null;
                if (me.state.StockId)
                    moduleInfo.columns[index].renderFormItem = () => (
                        <ComboGrid api="/api/GoodsLocation/GetPageList"
                            itemkey="ID"
                            param={{
                                IsActive: true,
                                StockId: me.state.StockId ?? ''
                            }}
                            itemvalue={['LocationNo', 'LocationNames']}
                            // disabled={disabled}
                            onClear={(value) => {
                                if (tableParam.params.GoodsLocationId)
                                    delete tableParam.params.GoodsLocationId
                            }}
                        />
                    );
                // else moduleInfo.columns[index].renderFormItem = () => (
                //     <ComboGrid api="/api/GoodsLocation/GetPageList"
                //         itemkey="ID"
                //         param={{
                //             IsActive: true,
                //         }}
                //         itemvalue={['LocationNo', 'LocationNames']}
                //         // disabled={disabled}
                //         onClear={(value) => {
                //             if (tableParam.params.GoodsLocationId)
                //                 delete tableParam.params.GoodsLocationId
                //         }}
                //     />
                // );
            }
        })

        //#region 操作栏按钮方法
        const action = {}
        //#endregion

        return (
            <div>
                <SmProTable
                    // columns={moduleInfo.columns}
                    // delete={Delete}
                    // batchDelete={(selectedRows) => BatchDelete(selectedRows)}
                    moduleInfo={moduleInfo}
                    {...action}
                    changePage={Index.changePage}
                    // formPage={FormPage}
                    formRef={this.formRef}
                    form={{ labelCol: { span: 6 } }}
                    onReset={() => {
                        dispatch({
                            type: 'ivquery/setTableStatus',
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
                            type: 'ivquery/setTableStatus',
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
export default connect(({ ivquery, loading }) => ({
    ivquery,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);