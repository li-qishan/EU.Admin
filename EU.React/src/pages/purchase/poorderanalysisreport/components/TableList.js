import React, { Component } from 'react';
import { connect } from 'umi';
import { query } from '../service';
import { DatePicker, Space, Tooltip, Tag } from 'antd';
import Index from '../index';
import SmProTable from '@/components/SysComponents/SmProTable';
import ComboGrid from '@/components/SysComponents/ComboGrid';

const { RangePicker } = DatePicker;
let moduleCode = "PO_ORDER_ANALYSIS_REPORT_MNG";
let me;
class TableList extends Component {
    formRef = React.createRef();
    formRef1 = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {

        };
    }
    componentWillMount() {
        const { dispatch } = this.props;
        dispatch({
            type: 'poorderanalysisreport/getModuleInfo',
            payload: { moduleCode },
        })
    }
    async onConfirmIn() {
        this.formRef.current.submit();
    }
    render() {
        const { dispatch, poorderanalysisreport: { moduleInfo, tableParam } } = this.props;

        moduleInfo.columns && moduleInfo.columns.map((item, index) => {
            if (item.dataIndex == 'MaterialId') {
                moduleInfo.columns[index].renderFormItem = () => (
                    <ComboGrid
                        api="/api/Material/GetPageList"
                        itemkey="ID"
                        param={{
                            IsActive: true
                        }}
                        itemvalue={['MaterialNo', 'MaterialNames']}
                        onClear={() => {
                            if (tableParam.params.MaterialId) {
                                delete tableParam.params.MaterialId
                            }
                        }}
                    />
                )
            } else if (item.dataIndex == 'OrderDate') {
                moduleInfo.columns[index].renderFormItem = () => (
                    <RangePicker />
                )
            // } else if (item.dataIndex == 'OrderNo') {
            //     moduleInfo.columns[index].render = (text, record) => (
            //         <Space>{record.IsDelay == 'Y' ? <Tooltip placement="top" title={'延期'}><Tag color="#f50" key={text}>
            //             {text}
            //         </Tag></Tooltip> : <Tooltip placement="top" title={'正常'}> <Tag color="#87d068" key={text}>
            //             {text}
            //         </Tag></Tooltip>} </Space>
            //     )
            // } else if (item.dataIndex == 'IsDelay') {
            //     moduleInfo.columns[index].render = (text, record) => (
            //         <Space>{record.IsDelay == 'Y' ? <Tag color="#f50" key={text}>延期</Tag> : <Tag color="#87d068" key={text}>正常</Tag>} </Space>
            //     )
            }
        })

        //#region 操作栏按钮方法
        const action = {}
        //#endregion

        return (
            <div>
                <SmProTable
                    columns={moduleInfo.columns}
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
                            type: 'poorderanalysisreport/setTableStatus',
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

                        if (params.OrderDate) {
                            params = Object.assign(params, {
                                StartDate: params.OrderDate[0],
                                EndDate: params.OrderDate[1]
                            });
                            delete tableParam.params.OrderDate;
                        } else {
                            delete params.StartDate;
                            delete params.EndDate;
                        }
                        dispatch({
                            type: 'poorderanalysisreport/setTableStatus',
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
export default connect(({ poorderanalysisreport }) => ({
    poorderanalysisreport,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);