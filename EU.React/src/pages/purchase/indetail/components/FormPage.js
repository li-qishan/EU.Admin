import React, { Component } from 'react';
import { Modal, Input, InputNumber, Badge, Tabs, Button, message, Select, DatePicker } from 'antd';
import { connect } from 'umi';
import { query as queryMaterial } from '../../../basedata/material/service';
import { ExclamationOutlined } from '@ant-design/icons';
import { queryWaitPurchase } from '../service';
import dayjs from 'dayjs';
import ProTable from '@ant-design/pro-table';
import request from "@/utils/request";
const { TabPane } = Tabs;

let me;
class FormPage extends Component {
    ref = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',//从表主键
            isLoading: true,
            selectList: [],
            selectedRowKeys: [],
            Source: ''
        };
    }
    // componentWillMount() {
    //     const { dispatch, user, Id } = this.props;
    //     if (dispatch && Id) {
    //         this.setState({ Id })
    //         dispatch({
    //             type: 'salesorderdetail/getById',
    //             payload: { Id },
    //         }).then((result) => {
    //             if (result.response && me.formRef.current)
    //                 me.formRef.current.setFieldsValue(result.response)
    //         })
    //     }

    // }
    // componentWillReceiveProps(nextProps) {
    //     me.setState({ isLoading: true })
    //     const { dispatch, Id, modalVisible } = nextProps;
    //     this.setState({ Id: Id ? Id : null, list: [] })
    //     if (dispatch && Id && modalVisible) {
    //         dispatch({
    //             type: 'salesorderdetail/getById',
    //             payload: { Id },
    //         }).then((result) => {
    //             me.setState({ list: [], isLoading: false })
    //             if (result.response && me.formRef.current)
    //                 me.formRef.current.setFieldsValue(result.response)
    //         })
    //     } else
    //         me.setState({ list: [], isLoading: false })
    // }
    static async getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible && nextProps.modalVisible) {
            me.setState({
                selectList: [],
                StockList: [],
                selectedRowKeys: [],
                GoodsLocationList: []
            });
            // let response = await queryStock({ moduleCode: "BD_STOCK_MNG", paramData: { "current": 1, "pageSize": 1000000 } });
            let response = await request('/api/Stock/GetPageList', {
                method: 'GET',
                params: {
                    paramData: { "current": 1, "pageSize": 1000000 }
                }
            });
            if (response.status == "ok")
                me.setState({
                    StockList: response.data
                });
            else
                message.error(response.message);
            const { inorder: { detail: { StockId, GoodsLocationId } } } = me.props;
            if (StockId) {
                response = await request('/api/GoodsLocation/GetPageList', {
                    method: 'GET',
                    params: {
                        paramData: { "current": 1, "pageSize": 1000000 },
                        filter: { StockId }
                    }
                });
                if (response.status == "ok")
                    me.setState({
                        GoodsLocationList: response.data
                    });
                else
                    message.error(response.message);
            }
        }
    }
    onTabsChange(key) {
        console.log(key);
    }
    onDelList(record) {
        let { selectList } = this.state;
        //let tempList = selectList;
        let tempList = [...selectList];
        const index = tempList.findIndex(item => item.ID === record.ID);
        if (index > -1) {
            tempList.splice(index, 1);
            let selectedRowKeys1 = [];
            tempList.forEach(element => {
                selectedRowKeys1.push(element.ID);
            });
            me.setState({
                selectList: tempList,
                selectedRowKeys: selectedRowKeys1
            }, () => {
            })
        }
    }
    okHandle = async () => {
        const { onSubmit: handleAdd } = this.props;
        let { selectList, Source } = this.state;
        selectList.forEach(record => {
            if (!Source)
                record.MaterialId = record.ID;
        });

        let index = selectList.findIndex(item => !item.StockId);

        if (index > -1) {
            message.error('请选择序号【' + (index + 1) + '】仓库！');
            return false;
        }
        index = selectList.findIndex(item => !item.GoodsLocationId);

        if (index > -1) {
            message.error('请选择序号【' + (index + 1) + '】货位！');
            return false;
        }
        handleAdd(selectList, '');
        me.setState({
            selectList: [],
            selectedRowKeys: []
        })
    };
    // onMatchAll = async () => {
    //     let params = {
    //         "current": 1,
    //         "pageSize": 100000
    //     };
    //     let result = await queryMaterial({
    //         paramData: JSON.stringify(params),
    //         moduleCode: 'BD_MATERIAL_MNG'
    //     });
    //     let { selectList, selectedRowKeys } = this.state;
    //     let tempList = [...selectList];
    //     let tempSelectedRowKeys = [...selectedRowKeys];
    //     result.data.forEach((record) => {
    //         record.ReserveDeliveryTime = dayjs();
    //         tempSelectedRowKeys.push(record.ID);
    //         tempList.push(record);
    //     });
    //     me.setState({
    //         selectList: tempList,
    //         selectedRowKeys: tempSelectedRowKeys
    //     })
    // }
    onCommonChange(values, record, type) {
        let { selectList } = me.state;
        let tempList = [...selectList];
        const index = tempList.findIndex(item => item.ID === record.ID);
        if (index > -1) {
            if (type == 'ReserveDeliveryTime')
                tempList[index][type] = dayjs(values).format('YYYY/MM/DD');
            else if (type == 'Remark') {
                tempList[index][type] = values.target.value
            }
            else
                tempList[index][type] = values;
        }
        this.setState({ selectList: tempList })
    }
    async onChangeStock(values, record) {
        let { selectList } = me.state;
        let tempList = [...selectList];
        let flag = tempList.findIndex(item => item.ID === record.ID);
        tempList[flag].StockId = values;

        if (flag > -1) {
            let result = await request('/api/OutOrder/GetLocationList', {
                method: 'GET',
                params: {
                    StockId: values,
                    MaterialId: record.MaterialId
                }
            });
            let index = record.StockList.findIndex(item => item.ID === values);
            tempList[flag].StockName = record.StockList[index].StockNames
            tempList[flag].GoodsLocationId = null;
            tempList[flag].GoodsLocationList = [];
            if (result && result.status == "ok" && result.data && result.data.length > 0) {
                tempList[flag].GoodsLocationId = result.data[0].ID;
                tempList[flag].GoodsLocationName = result.data[0].LocationNames;
                tempList[flag].GoodsLocationList = result.data;
            }
            me.setState({
                selectList: tempList
            });
        }
    }
    async onChangeLoction(values, record) {
        let { selectList } = this.state;
        let tempList = [...selectList];
        const i = tempList.findIndex(item => item.ID === record.ID);
        tempList[i].GoodsLocationId = values;
        me.setState({ selectList: tempList });
    }
    render() {
        const { modalVisible, onCancel, masterId, poarrival: { isPOArrival } } = this.props;
        let { selectList, selectedRowKeys, Source, StockList, GoodsLocationList } = this.state;

        let tableParam = {};
        let columns = [{
            title: '单据来源',
            dataIndex: 'OrderSource',
            filters: false,
            width: 120,
            valueEnum: {
                'POOrder': {
                    text: '采购单',
                },
                'ArrivalOrder': {
                    text: '到货通知单',
                }
            }
        }, {
            title: '来源单号',
            dataIndex: 'SourceOrderNo'
        }, {
            title: '物料编号',
            dataIndex: 'MaterialNo'
        }, {
            title: '物料名称',
            dataIndex: 'MaterialName'
        }, {
            title: '规格',
            dataIndex: 'Specifications'
        }, {
            title: '描述',
            dataIndex: 'Description'
        }, {
            title: '单位',
            dataIndex: 'UnitName'
        }, {
            title: Source == 'POOrder' ? '采购数量' : '到货数量',
            dataIndex: Source == 'POOrder' ? 'PurchaseQTY' : 'ArrivalQTY',
            valueType: 'digit'
        }, {
            title: Source == 'POOrder' ? '采购剩余数量' : '到货剩余数量',
            dataIndex: 'MaxInQTY',
            valueType: 'digit'
        }];
        if (!Source)
            columns = [{
                title: '物料编号',
                dataIndex: 'MaterialNo'
            }, {
                title: '物料名称',
                dataIndex: 'MaterialNames'
            }, {
                title: '规格',
                dataIndex: 'Specifications'
            }];
        let hasColumns = [{
            title: '序号',
            width: 60,
            dataIndex: 'Key',
            valueType: 'digit'
        }];
        hasColumns = hasColumns.concat(columns)

        hasColumns.push({
            title: '入库数量',
            dataIndex: 'InQTY',
            render: (text, record) => (
                <InputNumber
                    placeholder="请输入"
                    size='small'
                    value={text}
                    min='1'
                    max={record.MaxInQTY}
                    onChange={(value) => me.onCommonChange(value, record, 'InQTY')}
                />
            )
        }, {
            title: '仓库',
            dataIndex: 'StockId',
            width: 120,
            render: (text, record) => (
                <Select
                    size='small'
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
            title: '货位',
            dataIndex: 'GoodsLocationId',
            width: 120,
            render: (text, record) => (
                <Select
                    size='small'
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
        }, {
            title: '预定交期',
            dataIndex: 'ReserveDeliveryTime',
            //render: text => <>{text ? text.substring(0, 10) : null}</>,
            render: (text, record) => (
                <DatePicker size='small' defaultValue={dayjs(text, 'YYYY/MM/DD')} format={'YYYY/MM/DD'} onChange={(value) => me.onCommonChange(value, record, 'ReserveDeliveryTime')} />
            ),
            width: 150
        }, {
            title: '备注',
            dataIndex: 'Remark',
            width: 150,
            render: (text, record) => (
                <Input size='small' defaultValue={text} onChange={(value) => me.onCommonChange(value, record, 'Remark')} />
            )
        }, {

            title: '操作',
            key: 'operation',
            fixed: 'right',
            width: 60,
            render: (text, record) => <a onClick={() => me.onDelList(record)}>删除</a>,
        });

        return (
            (<Modal
                destroyOnClose
                // title='选择物料'
                open={modalVisible}
                // onOk={this.okHandle}
                maskClosable={false}
                width={1100}
                closable={false}
                onCancel={() => onCancel()}
                // okButtonProps={{ disabled: selectList.length == 0 ? true : false }}
                footer={[
                    <Button key="back" onClick={() => onCancel()}>
                        取消
                    </Button>
                    ,
                    // <Button
                    //     key="submit1"
                    //     onClick={this.onMatchAll}
                    // >
                    //     添加全部
                    // </Button>,
                    <Button key="submit" disabled={selectList.length == 0 ? true : false} type="primary" onClick={this.okHandle}>
                        确认
                    </Button>
                ]}
            >
                <Tabs defaultActiveKey="1" onChange={this.onTabsChange}>
                    <TabPane tab="待选" key="1">
                        <ProTable
                            // onRow={record => {
                            //     return {
                            //         onClick: event => { }, // 点击行
                            //         onDoubleClick: event => {  },
                            //         onContextMenu: event => { },
                            //         onMouseEnter: event => { }, // 鼠标移入行
                            //         onMouseLeave: event => { },
                            //     };
                            // }}
                            actionRef={this.ref}
                            rowKey="ID"
                            rowSelection={{
                                fixed: 'left',
                                selectedRowKeys,
                                onChange: (selectedRowKeys, selectedRows) => {
                                    if (selectedRows.length == 0) {
                                        me.setState({
                                            selectList: [],
                                            selectedRowKeys: []
                                        })
                                    }
                                },
                                onSelect: (record, selected) => {
                                    let tempList = [...selectList];
                                    let tempSelectedRowKeys = [...selectedRowKeys];
                                    if (selected) {
                                        if (!Source)
                                            record.InQTY = 1;
                                        record.Key = tempSelectedRowKeys.length + 1;
                                        tempSelectedRowKeys.push(record.ID);
                                        record.StockList = StockList;
                                        record.GoodsLocationList = GoodsLocationList;
                                        if (!record.ReserveDeliveryTime) record.ReserveDeliveryTime = dayjs();
                                        tempList.push(record);
                                    } else {
                                        let index = tempSelectedRowKeys.findIndex(item => item === record.ID);
                                        if (index > -1)
                                            tempSelectedRowKeys.splice(index, 1);

                                        index = -2;
                                        index = tempList.findIndex(item => item.ID === record.ID);
                                        if (index > -1)
                                            tempList.splice(index, 1);
                                    }
                                    me.setState({
                                        selectList: tempList,
                                        selectedRowKeys: tempSelectedRowKeys
                                    })
                                },
                                onSelectAll: (selected, selectedRows, changeRows) => {
                                    let tempList = [...selectList];
                                    let tempSelectedRowKeys = [...selectedRowKeys];
                                    if (selected) {
                                        changeRows.forEach(record => {
                                            if (!Source)
                                                record.InQTY = 1;
                                            record.Key = tempSelectedRowKeys.length + 1;
                                            tempSelectedRowKeys.push(record.ID);
                                            record.StockList = StockList;
                                            record.GoodsLocationList = GoodsLocationList;
                                            if (!record.ReserveDeliveryTime) record.ReserveDeliveryTime = dayjs();
                                            tempList.push(record);
                                        });
                                    } else {
                                        changeRows.forEach(record => {
                                            let index = tempSelectedRowKeys.findIndex(item => item === record.ID);
                                            if (index > -1)
                                                tempSelectedRowKeys.splice(index, 1);

                                            index = -2;
                                            index = tempList.findIndex(item => item.ID === record.ID);
                                            if (index > -1)
                                                tempList.splice(index, 1);
                                        });
                                    }
                                    me.setState({
                                        selectList: tempList,
                                        selectedRowKeys: tempSelectedRowKeys
                                    })
                                }
                            }}
                            scroll={{ x: 'max-content' }}
                            columns={columns}
                            search={false}
                            size='small'
                            pagination={{
                                pageSize: 10
                            }}
                            request={(params, sorter, filter) => {
                                if (tableParam && tableParam.params && !params._timestamp)
                                    params = tableParam.params;
                                // return queryMaterial({
                                //     paramData: JSON.stringify(params),
                                //     sorter: JSON.stringify(sorter),
                                //     filter: JSON.stringify(filter),
                                //     moduleCode: 'BD_MATERIAL_MNG'
                                // })
                                if (Source)
                                    return queryWaitPurchase({
                                        paramData: JSON.stringify(params),
                                        sorter: JSON.stringify(sorter),
                                        filter: JSON.stringify(filter),
                                        source: Source,
                                        masterId
                                    })
                                else
                                    return queryMaterial({
                                        paramData: JSON.stringify(params),
                                        sorter: JSON.stringify(sorter),
                                        filter: JSON.stringify(filter),
                                        moduleCode: 'BD_MATERIAL_MNG'
                                    })
                            }}
                            options={{
                                fullScreen: false,
                                reload: false,
                                setting: false,
                                density: false,
                                search: {
                                    name: 'keyWord',
                                },
                            }}
                            // toolBarRender={false}
                            toolBarRender={() => [
                                <Select
                                    value={Source}
                                    defaultValue={Source}
                                    style={{ width: 120 }}
                                    onChange={(value) => {
                                        this.setState({ Source: value });
                                        this.ref.current.reloadAndRest();
                                    }}
                                    style={{ marginLeft: 10, width: 120 }}
                                >
                                    <Select.Option value="">无</Select.Option>
                                    {isPOArrival == 'N' ? <Select.Option value="POOrder">采购单</Select.Option> : <Select.Option value="ArrivalOrder">到货通知单</Select.Option>}
                                </Select>
                            ]}
                        />
                    </TabPane>
                    <TabPane tab={selectList.length == 0 ? '已选' : <Badge count={selectList.length} style={{ right: '-10px' }}>已选</Badge>} key="2">
                        {selectList.length == 0 ? <div style={{ textAlign: 'center', padding: 20 }}>
                            <div><ExclamationOutlined style={{ fontSize: 40 }} /></div>
                            <div>暂无数据</div>
                        </div> :
                            <ProTable rowKey="ID"
                                scroll={{ x: 'max-content' }}
                                columns={hasColumns}
                                search={false}
                                dataSource={selectList}
                                pagination={{
                                    pageSize: 10
                                }}
                                columnEmptyText={''}
                                size='small'
                                options={{
                                    fullScreen: false,
                                    reload: true,
                                    setting: false,
                                    density: false
                                }}
                                toolBarRender={false}
                            />
                        }
                    </TabPane>

                </Tabs>
            </Modal>)
        );
    }
}
export default connect(({ inorderdetail, inorder, poarrival }) => ({
    inorderdetail, inorder, poarrival
}))(FormPage);
