import React, { Component } from 'react';
import { connect } from 'umi';
import { query, BatchDelete, Delete } from '../service';
import { message, Modal } from 'antd';
import Index from '../index';
import SmProTable from '@/components/SysComponents/SmProTable';
import DetailForm from './FormPage';
import FormEdit from './FormEdit';
import InvoiceFormPage from '../../invoice/components/FormPage';
import Utility from '@/utils/utility';

let moduleCode = "AR_SALES_COLLECTION_WRITE_OFF_MNG";
let me;
const { confirm } = Modal;

class TableList extends Component {
    formRef = React.createRef();
    detailActionRef = React.createRef();

    constructor(props) {
        super(props);
        me = this;
        me.state = {
            masterId: '',
            DetailId: '',
            detailVisible: false,//表单是否显示
            editVisible: false,//表单是否显示
        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        dispatch({
            type: 'arcollectionwriteoff/getModuleInfo',
            payload: { moduleCode }
        });
        this.setState({ masterId: Id })
    }
    static reloadData() {
        me.detailActionRef.current.reload();
    }
    // componentDidMount() {
    //     const { dispatch, Id } = this.props;
    //     mainId = Id;
    // }
    // componentWillReceiveProps(nextProps) {
    //     const { dispatch, Id } = nextProps;
    //     mainId = Id;
    // }
    render() {
        const { dispatch, arcollectionwriteoff: { moduleInfo, tableParam }, IsView } = this.props;
        const { masterId, DetailId, detailVisible, detailView, editVisible } = this.state;

        //#region 操作栏按钮方法
        const action = {}
        //#endregion

        moduleInfo.columns && moduleInfo.columns.map((item, index) => {
            if (item.dataIndex == 'OrderSource') {
                moduleInfo.columns[index].valueEnum = {
                    'ArInvoiceOrder': {
                        text: '应收发票',
                    },
                    'ArPrepaidOrder': {
                        text: '销售预收款',
                    }
                };
            }
        })

        return (
            <div>
                <SmProTable
                    columns={moduleInfo.columns}
                    actionRef={this.detailActionRef}
                    delete={Delete}
                    batchDelete={(selectedRows) => {
                        BatchDelete(selectedRows)
                        if (InvoiceFormPage)
                            InvoiceFormPage.reloadData();
                    }}
                    // deleteConfirm={async (action, data) => {
                    //     confirm({
                    //         title: '是否确定删除记录?',
                    //         icon: Utility.getIcon('ExclamationCircleOutlined'),
                    //         okText: '确定',
                    //         okType: 'danger',
                    //         cancelText: '取消',
                    //         onOk() {
                    //             Delete(data).then((response) => {
                    //                 if (response.status == "ok") {
                    //                     action.reload();
                    //                     if (InvoiceFormPage)
                    //                         InvoiceFormPage.reloadData();
                    //                     message.success(response.message);
                    //                 }
                    //                 else
                    //                     message.error(response.message);
                    //             });
                    //         },
                    //         onCancel() {
                    //             console.log('Cancel');
                    //         },
                    //     });
                    // }}
                    moduleInfo={moduleInfo}
                    IsView={IsView}
                    {...action}
                    changePage={Index.changePage}
                    // formPage={FormPage}
                    // formRef={this.formRef}
                    // form={{ labelCol: { span: 6 } }}
                    addPage={() => this.setState({ detailVisible: true, DetailId: '', detailView: false })}
                    editPage={(record) => this.setState({ editVisible: true, DetailId: record.ID, detailView: false })}
                    viewPage={(record) => this.setState({ editVisible: true, DetailId: record.ID, detailView: true })}
                    onReset={() => {
                        dispatch({
                            type: 'arcollectionwriteoff/setTableStatus',
                            payload: {},
                        })
                    }}
                    onLoad={() => {
                        if (tableParam && tableParam.params && this.formRef.current) {
                            this.formRef.current.setFieldsValue({ ...tableParam.params })
                        }
                    }}
                    search={false}
                    pagination={tableParam && tableParam.params ? { current: tableParam.params.current } : {}}
                    request={(params, sorter, filter) => {
                        if (tableParam && tableParam.params && !params._timestamp)
                            params = Object.assign(tableParam.params, params);
                        if (tableParam && tableParam.sorter)
                            sorter = Object.assign(tableParam.sorter, sorter);
                        dispatch({
                            type: 'arcollectionwriteoff/setTableStatus',
                            payload: { params, sorter },
                        });
                        filter.OrderId = masterId;
                        return query({
                            paramData: JSON.stringify(params),
                            sorter: JSON.stringify(sorter),
                            filter: JSON.stringify(filter),
                            moduleCode, OrderId: masterId
                        })
                    }}
                />
                <DetailForm
                    modalVisible={detailVisible}
                    Id={DetailId}
                    masterId={masterId}
                    IsView={detailView}
                    onSubmit={async values => {
                        const { dispatch } = this.props;
                        // if (DetailId) {
                        //     values.ID = DetailId;
                        // }
                        // if (masterId) {
                        //     values.OrderId = masterId;
                        // }
                        for (let i = 0; i < values.length; i++)
                            values[i].OrderId = masterId;
                        dispatch({
                            type: 'arcollectionwriteoff/batchSaveData',
                            payload: values,
                        }).then(() => {
                            const { arcollectionwriteoff: { Id } } = me.props;
                            // this.setState({ DetailId: Id })
                            this.setState({ detailVisible: false })
                            if (me.detailActionRef.current) {
                                me.detailActionRef.current.reload();
                            }
                            if (InvoiceFormPage)
                                InvoiceFormPage.reloadData();
                        });
                    }}
                    onCancel={() => { this.setState({ detailVisible: false }) }}
                />
                <FormEdit
                    modalVisible={editVisible}
                    Id={DetailId}
                    IsView={detailView}
                    onSubmit={async values => {
                        const { dispatch } = this.props;
                        // if (DetailId) {
                        //     values.ID = DetailId;
                        // }
                        if (masterId) {
                            values.OrderId = masterId;
                        }
                        dispatch({
                            type: 'arcollectionwriteoff/saveData',
                            payload: values,
                        }).then(() => {
                            const { arcollectionwriteoff: { Id } } = me.props;
                            this.setState({ DetailId: Id })
                            if (me.detailActionRef.current) {
                                me.detailActionRef.current.reload();
                            }
                        });
                    }}
                    onCancel={() => { this.setState({ editVisible: false }) }}
                />
            </div>
        )
    }
}
export default connect(({ arcollectionwriteoff }) => ({
    arcollectionwriteoff
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(TableList);