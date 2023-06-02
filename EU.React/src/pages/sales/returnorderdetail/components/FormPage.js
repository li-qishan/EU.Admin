import React, { Component } from 'react';
import { Modal, InputNumber, Button, Select, Switch, message } from 'antd';
import { connect } from 'umi';
import { PlusOutlined } from '@ant-design/icons';
import { queryHasOut } from '../service';
import ProTable from '@ant-design/pro-table';
import request from "@/utils/request";

let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',//从表主键
            isSelectModalVisible: false,
            formList: [],
            selectList: []
        };
    }
    // componentWillMount() {
    //     const { dispatch, user, Id } = this.props;
    //     if (dispatch && Id) {
    //         this.setState({ Id })
    //         dispatch({
    //             type: 'returnorderdetail/getById',
    //             payload: { Id },
    //         }).then((result) => {
    //             if (result.response && me.formRef.current)
    //                 me.formRef.current.setFieldsValue(result.response)
    //         })
    //     }

    // }
    componentWillReceiveProps(nextProps) {
        const { dispatch, Id, modalVisible } = nextProps;
        this.setState({ Id: Id ? Id : null })
        // if (dispatch && Id && modalVisible) {
        //     dispatch({
        //         type: 'returnorderdetail/getById',
        //         payload: { Id },
        //     }).then((result) => {
        //         if (result.response && me.formRef.current)
        //             me.formRef.current.setFieldsValue(result.response)
        //     })
        // }
        this.setState({ formList: [], selectList: [] })

    }

    okHandle = async () => {
        const { onSubmit: handleAdd } = this.props;
        // const fieldsValue = await me.formRef.current.validateFields();
        // // me.formRef.current.resetFields();
        // const { Id } = this.state;
        // if (Id)
        //     fieldsValue.ID = Id;
        let { formList } = me.state;
        let flag = -1;
        for (let i = 0; i < formList.length; i++) {
            if (!formList[i].StockId || !formList[i].GoodsLocationId)
                flag = i;
        }
        if (flag == -1) {
            this.setState({ formList: [] })
            handleAdd(formList);
        }
        else
            message.error('请选择序号【' + (flag + 1) + '】出库仓或出库仓位！');
    };
    handleOk = async () => {
        let { selectList, formList } = me.state;

        selectList.forEach(element => {
            let list = formList.filter((value) => {
                return value.ID == element.ID;
            });
            if (list.length == 0)
                formList.push(element);
        });

        // console.log(formList, 'formList')
        // formList = [formList, ...selectList]
        // console.log(selectList, 'selectList')
        // console.log(formList, 'formList1')
        this.setState({ isSelectModalVisible: false, formList })
        this.formRef.current.reload();

    };
    onSelectCancel = async () => {
        this.setState({ isSelectModalVisible: false })
    }
    onChange(values, record) {
        let { formList } = me.state;
        for (let index = 0; index < formList.length; index++)
            if (formList[index].ID == record.ID)
                formList[index].ReturnQTY = values;
        this.setState({ formList })
    }
    onChangeEntity(values, record) {
        let { formList } = me.state;
        for (let i = 0; i < formList.length; i++)
            if (formList[i].ID == record.ID)
                formList[i].IsEntity = values;
        this.setState({ formList })
    }
    async onChangeStock(values, record) {
        let { formList } = me.state;
        let flag = -1;
        for (let i = 0; i < formList.length; i++)
            if (formList[i].ID == record.ID) {
                flag = i;
                formList[i].StockId = values;
            }
        if (flag > -1) {
            let result = await request('/api/OutOrder/GetLocationList', {
                method: 'GET',
                params: {
                    StockId: values,
                    MaterialId: record.MaterialId
                }
            });
            if (result && result.status == "ok" && result.data && result.data.length > 0) {
                formList[flag].GoodsLocationId = result.data[0].ID;
                formList[flag].GoodsLocationList = result.data;
                formList[flag].InventoryQTY = result.InventoryQTY;
            } else {
                formList[flag].GoodsLocationId = null;
                formList[flag].GoodsLocationList = [];
                formList[flag].InventoryQTY = 0;
            }
            me.setState({ formList }, () => {
                me.formRef.current.reload();
            })
        }
    }
    async onChangeLoction(values, record) {
        let { formList } = me.state;
        let flag = -1;
        for (let i = 0; i < formList.length; i++)
            if (formList[i].ID == record.ID) {
                flag = i;
                formList[i].GoodsLocationId = values;
            }
        if (flag > -1) {
            let result = await request('/api/MaterialInventory/GetPageList', {
                method: 'GET',
                params: {
                    paramData: {
                        current: 1,
                        pageSize: 20,
                        StockId: record.StockId,
                        GoodsLocationId: record.GoodsLocationId,
                        MaterialId: record.MaterialId
                    }
                }
            });
            if (result && result.status == "ok" && result.data && result.data.length > 0) {
                formList[flag].InventoryQTY = result.data[0].QTY;
            } else {
                formList[flag].InventoryQTY = 0;
            }
            me.setState({ formList }, () => {
                me.formRef.current.reload();
            })
        }
    }
    onDelList(record) {
        let { formList } = this.state;
        const index = formList.findIndex(item => item.ID === record.ID);
        if (index > -1) {
            formList.splice(index, 1);
            me.setState({ formList }, () => {
                me.formRef.current.reload();
            })
        }
    }
    // onFinish(values) {
    //     const { dispatch } = this.props;
    //     const { Id } = this.state;
    //     if (Id) {
    //         values.ID = Id;
    //     }
    //     dispatch({
    //         type: 'returnorderdetail/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { dispatch, returnorderdetail: { Id } } = me.props;
    //         this.setState({ Id })
    //     });
    // }
    render() {
        const { modalVisible, IsView, onCancel, masterId } = this.props;
        const { Id, isSelectModalVisible, formList, selectList } = this.state;
        const SelectColumns = [{
            title: '出库单号',
            hideInSearch: true,
            dataIndex: 'OutOrderNo',
            copyable: true
        }, {
            title: '销售单号',
            hideInSearch: true,
            dataIndex: 'SalesOrderNo',
            copyable: true
        }, {
            title: '出库单行号',
            hideInSearch: true,
            dataIndex: 'SerialNumber'
        }, {
            title: '产品编码',
            hideInSearch: true,
            dataIndex: 'MaterialNo'
        }, {
            title: '产品名称',
            hideInSearch: true,
            dataIndex: 'MaterialName'
        }, {
            title: '规格',
            hideInSearch: true,
            dataIndex: 'MaterialSpecifications',
            width: 160
        }, {
            title: '单位',
            hideInSearch: true,
            dataIndex: 'UnitName',
        }, {
            title: '出库数量',
            dataIndex: 'OutQTY'
        }, {
            title: '可退货数量',
            dataIndex: 'ReturnQTY',
            width: 100,
            // }, {
            //     title: '退货数量',
            //     dataIndex: 'ReturnQTY'
            // }, {
            //     title: '单价',
            //     dataIndex: 'Price'
            // }, {
            //     title: '未税金额',
            //     dataIndex: 'NoTaxAmount'
            // }, {
            //     title: '税额',
            //     dataIndex: 'TaxAmount'
            // }, {
            //     title: '含税金额',
            //     dataIndex: 'TaxIncludedAmount'
        }, {
            title: '出库时间',
            dataIndex: 'OutTime'
        }, {
            title: '是否实物退回',
            dataIndex: 'IsEntity',
            valueEnum: { true: { text: "是" }, false: { text: "否" } }
        }];
        let tableParam = {};

        const SelectRowSelection = {
            onChange: (selectedRowKeys, selectedRows) => {
                me.setState({ selectList: selectedRows })
                console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
            },
        };
        let columns = [{
            title: '序号',
            dataIndex: 'key',
            render: text => <a>{text + 1}</a>,
        }, {
            title: '出库单号',
            hideInSearch: true,
            dataIndex: 'OutOrderNo',
            copyable: true
        }, {
            title: '销售单号',
            hideInSearch: true,
            dataIndex: 'SalesOrderNo',
            copyable: true
        }, {
            title: '出库单行号',
            hideInSearch: true,
            dataIndex: 'SerialNumber'
        }, {
            title: '产品编码',
            hideInSearch: true,
            dataIndex: 'MaterialNo'
        }, {
            title: '产品名称',
            hideInSearch: true,
            dataIndex: 'MaterialName'
        }, {
            title: '规格',
            hideInSearch: true,
            dataIndex: 'MaterialSpecifications',
            width: 160
        }, {
            title: '单位',
            hideInSearch: true,
            dataIndex: 'UnitName',
        }, {
            title: '出库数量',
            dataIndex: 'OutQTY'
        }, {
            title: '退货数量',
            dataIndex: 'ReturnQTY',
            width: 100,
            render: (text, record) => (
                <InputNumber placeholder="请输入" value={text} disabled={IsView} onChange={(value) => me.onChange(value, record)} />
            ),
        }, {
            title: '退回仓库',
            dataIndex: 'StockId',
            width: 120,
            render: (text, record) => (
                <Select
                    allowClear
                    showSearch={false}
                    filterOption={false}
                    value={text}
                    onChange={(value, Option) => me.onChangeStock(value, record)}
                >
                    {record && record.StockList.length > 0 && record.StockList.map(item => {
                        return (
                            <Option key={item['ID']}>{item['StockNames']}</Option>
                        )
                    })}
                </Select>
            ),
        }, {
            title: '退回货位',
            dataIndex: 'GoodsLocationId',
            width: 120,
            render: (text, record) => (
                <Select
                    allowClear
                    showSearch={false}
                    filterOption={false}
                    value={text}
                    onChange={(value, Option) => me.onChangeLoction(value, record)}
                >
                    {record && record.GoodsLocationList.length > 0 && record.GoodsLocationList.map(item => {
                        return (
                            <Option key={item['ID']}>{item['LocationNames']}</Option>
                        )
                    })}
                </Select>
            )
            // }, {
            //     title: '退货数量',
            //     dataIndex: 'ReturnQTY'
            // }, {
            //     title: '单价',
            //     dataIndex: 'Price'
            // }, {
            //     title: '未税金额',
            //     dataIndex: 'NoTaxAmount'
            // }, {
            //     title: '税额',
            //     dataIndex: 'TaxAmount'
            // }, {
            //     title: '含税金额',
            //     dataIndex: 'TaxIncludedAmount'
        }, {
            title: '出库时间',
            dataIndex: 'OutTime'
        }, {
            title: '是否实物退回',
            dataIndex: 'IsEntity',
            // valueEnum: { true: { text: "是" }, false: { text: "否" } }
            render: (text, record) => (
                (<Switch checked={text} onChange={(value) => me.onChangeEntity(value, record)} />)
                // <InputNumber placeholder="请输入" value={text} disabled={IsView} min={record.Min} step={record.Step} onChange={(value) => me.onChangeQTY(value, record)} style={{ width: '100%' }} />
            ),
        }, {
            title: '操作',
            key: 'operation',
            fixed: 'right',
            width: 60,
            render: (text, record) => <a onClick={() => me.onDelList(record)}>删除</a>
        }];

        const data = [];
        let i = 0;
        formList.forEach(element => {
            element.key = i;
            data.push(element);
            i++;
        });
        return (<>
            <Modal
                destroyOnClose
                title={Id ? '退货单明细->编辑' : '退货单明细->新增'}
                open={modalVisible}
                onOk={this.okHandle}
                maskClosable={false}
                width={1100}
                onCancel={() => onCancel()}
                okButtonProps={{ disabled: IsView || (formList.length == 0 ? true : false) }}
            >
                {/* <Table columns={columns} dataSource={data} /> */}
                <ProTable
                    rowKey="ID"
                    actionRef={this.formRef}
                    scroll={{ x: 'max-content' }}
                    columns={columns}
                    search={false}
                    request={() =>
                        Promise.resolve({
                            data: data,
                            success: true,
                        })
                    }
                    size='small'
                    options={{
                        fullScreen: false,
                        reload: true,
                        setting: false,
                        density: false
                    }}
                    toolBarRender={false}
                />
                <Button
                    type="dashed"
                    style={{ width: '100%', marginTop: 10 }}
                    onClick={() => this.setState({ isSelectModalVisible: true, selectList: [] })}>
                    <PlusOutlined /> 添加一行数据
                </Button>
            </Modal>
            <Modal
                width={1100}
                zIndex={1100}
                destroyOnClose
                title="选择销售订单"
                open={isSelectModalVisible}
                onOk={this.handleOk}
                onCancel={this.onSelectCancel}
                okButtonProps={{ disabled: selectList.length == 0 ? true : false }}>
                <ProTable
                    rowKey="ID"
                    rowSelection={{
                        fixed: 'left',
                        ...SelectRowSelection,
                    }}
                    scroll={{ x: 'max-content' }}
                    columns={SelectColumns}
                    search={false}
                    size='small'
                    pagination={{
                        pageSize: 10
                    }}
                    request={(params, sorter, filter) => {
                        if (tableParam && tableParam.params && !params._timestamp) {
                            params = tableParam.params;
                            //sorter = tableParam.sorter
                        }
                        // dispatch({
                        //     type: 'returnorderdetail/setTableStatus',
                        //     payload: { params, sorter },
                        // });
                        // filter.OrderId = masterId;
                        return queryHasOut({
                            paramData: JSON.stringify(params),
                            sorter: JSON.stringify(sorter),
                            filter: JSON.stringify(filter),
                            masterId
                        })
                    }}
                    options={{
                        fullScreen: false,
                        reload: true,
                        setting: false,
                        density: false
                    }}
                    toolBarRender={false}
                />
            </Modal>
        </>);
    }
}
export default connect(({ returnorderdetail }) => ({
    returnorderdetail,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);