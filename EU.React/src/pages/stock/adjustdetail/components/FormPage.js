import React, { Component } from 'react';
import { Modal, Input, InputNumber, Badge, Tabs, Button, Select, message } from 'antd';
import { connect } from 'umi';
import ProTable from '@ant-design/pro-table';
import { query as queryMaterial } from '../../../basedata/material/service';
import { ExclamationOutlined } from '@ant-design/icons';
import request from "@/utils/request";

const { TabPane } = Tabs;
let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',//从表主键
            isLoading: true,
            selectList: [],
            StockList: [],
            selectedRowKeys: []
        };
    }

    static async getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible && nextProps.modalVisible) {
            me.setState({
                selectList: [],
                selectedRowKeys: [],
                StockList: []
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
        }
    }

    onQTYChange(value, record) {
        let { selectList } = me.state;
        let tempList = [...selectList];
        for (let i = 0; i < tempList.length; i++)
            if (tempList[i].ID == record.ID)
                tempList[i].QTY = value;

        me.setState({ selectList: tempList })
    }
    onPriceChange(value, record) {
        let { selectList } = me.state;
        let tempList = [...selectList];
        for (let i = 0; i < tempList.length; i++)
            if (tempList[i].ID == record.ID)
                tempList[i].Price = value;

        me.setState({ selectList: tempList })
    }
    onChangeAdjustType(value, record) {
        let { selectList } = me.state;
        let tempList = [...selectList];
        for (let i = 0; i < tempList.length; i++)
            if (tempList[i].ID == record.ID)
                tempList[i].AdjustType = value;

        me.setState({ selectList: tempList })
    }
    onCodeChange(value, record) {
        let { selectList } = me.state;
        let tempList = [...selectList];
        for (let i = 0; i < tempList.length; i++)
            if (tempList[i].ID == record.ID)
                tempList[i].BatchNo = value;

        me.setState({ selectList: tempList })
    }
    onTabsChange(key) {
        console.log(key);
    }
    onDelList(record) {
        let { selectList, selectedRowKeys } = this.state;
        //let tempList = selectList;
        let tempList = [...selectList];
        const index = tempList.findIndex(item => item.ID === record.ID);
        if (index > -1) {
            tempList.splice(index, 1);
            selectedRowKeys = [];
            tempList.forEach(element => {
                selectedRowKeys.push(element.ID);
            });
            me.setState({
                selectList: tempList,
                selectedRowKeys
            }, () => {
            })
        }
    }
    okHandle = async () => {
        const { onSubmit: handleAdd } = this.props;
        // me.formRef.current.resetFields();
        let { selectList } = this.state;
        let flag = -1;
        let flag1 = -1;
        selectList.forEach(record => {
            record.MaterialId = record.ID;
            record.MaterialName = record.MaterialNames;
            if (!record.StockId || !record.GoodsLocationId)
                flag = 1;
            if (record.QTY <= 0)
                flag1 = 1;
        });
        if (flag > -1) {
            message.error("请选择仓库或货位！");
            return false;
        }
        if (flag1 > -1) {
            message.error("数量必须大于0！");
            return false;
        }

        handleAdd(selectList, '');
        me.setState({
            selectList: [],
            selectedRowKeys: []
        })
    };
    onMatchAll = async () => {
        let params = {
            "current": 1,
            "pageSize": 100000
        };
        let result = await queryMaterial({
            paramData: JSON.stringify(params),
            moduleCode: 'BD_MATERIAL_MNG'
        });
        let { selectList, selectedRowKeys, StockList } = this.state;
        let tempList = [...selectList];
        let tempSelectedRowKeys = [...selectedRowKeys];
        result.data.forEach((record) => {
            tempSelectedRowKeys.push(record.ID);
            record.StockId = "";
            record.StockList = StockList;
            record.GoodsLocationId = "";
            record.GoodsLocationList = [];
            record.QTY = 1;
            record.Price = 1;
            record.AdjustType = "Add";
            tempList.push(record);
        });
        me.setState({
            selectList: tempList,
            selectedRowKeys: tempSelectedRowKeys
        })
    }
    async onChangeStock(values, record) {
        let { selectList } = me.state;
        let tempList = [...selectList];
        let flag = -1;
        for (let i = 0; i < tempList.length; i++)
            if (tempList[i].ID == record.ID) {
                flag = i;
                tempList[i].StockId = values;
            }
        if (flag > -1) {
            let result = await request('/api/OutOrder/GetLocationList', {
                method: 'GET',
                params: {
                    StockId: values,
                    MaterialId: record.MaterialId
                }
            });
            tempList[flag].GoodsLocationId = null;
            tempList[flag].GoodsLocationList = [];
            if (result && result.status == "ok" && result.data && result.data.length > 0) {
                tempList[flag].GoodsLocationId = result.data[0].ID;
                tempList[flag].GoodsLocationList = result.data;
            }
            me.setState({ selectList: tempList }, () => {
            })
        }
    }
    async onChangeLoction(values, record) {
        let { selectList } = this.state;
        let tempList = [...selectList];
        let flag = -1;
        for (let i = 0; i < tempList.length; i++)
            if (tempList[i].ID == record.ID) {
                flag = i;
                tempList[i].GoodsLocationId = values;
            }
        me.setState({ selectList: tempList });
    }
    render() {
        const { modalVisible, onCancel } = this.props;
        let { selectList, selectedRowKeys, StockList } = this.state;

        let tableParam = {};
        const columns = [{
            title: '物料编号',
            hideInSearch: true,
            dataIndex: 'MaterialNo',
            width: 160
        }, {
            title: '物料名称',
            hideInSearch: true,
            dataIndex: 'MaterialNames'
        }, {
            title: '规格',
            hideInSearch: true,
            dataIndex: 'Specifications'
            // }, {
            //     title: '数量',
            //     dataIndex: 'QTY',
            //     width: 160,
            //     render: (text, record) => (
            //         <>
            //             <InputNumber size='small' value={QTYObject[record.ID] ?? null} min='0' onChange={value => me.onQTYChange(value, record)} />
            //         </>)
            // }, {
            //     title: '单价',
            //     dataIndex: 'Price',
            //     width: 160,
            //     render: (text, record) => (
            //         <InputNumber size='small' value={PriceObject[record.ID] ?? null} min='0' onChange={value => me.onPriceChange(value, record)} />
            //     )
            // }, {
            //     title: '客户物料编码',
            //     dataIndex: 'BatchNo',
            //     width: 160,
            //     render: (text, record) => (
            //         <Input size='small' value={CodeObject[record.ID] ?? null} onChange={e => me.onCodeChange(e.target.value, record)} />
            //     )
        }];
        const hasColumns = [{
            title: '调整类型',
            dataIndex: 'AdjustType',
            width: 80,
            render: (text, record) => (
                <>
                    <Select
                        allowClear
                        showSearch={false}
                        filterOption={false}
                        value={text}
                        onChange={(value, Option) => me.onChangeAdjustType(value, record)}>
                        <Option value="Add">调增</Option>
                        <Option value="Reduce">调减</Option>
                    </Select>
                </>)
        }, {
            title: '物料编号',
            hideInSearch: true,
            dataIndex: 'MaterialNo',
            width: 160
        }, {
            title: '物料名称',
            hideInSearch: true,
            dataIndex: 'MaterialNames'
        }, {
            title: '规格',
            hideInSearch: true,
            dataIndex: 'Specifications'
        }, {
            title: '数量',
            dataIndex: 'QTY',
            width: 120,
            render: (text, record) => (
                <>
                    <InputNumber size='small' value={text} min='0' onChange={value => me.onQTYChange(value, record)} />
                </>)
        }, {
            title: '单价',
            dataIndex: 'Price',
            width: 120,
            render: (text, record) => (
                <InputNumber size='small' value={text} min='0' step={record.Step} onChange={value => me.onPriceChange(value, record)} />
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
            title: '批次号',
            dataIndex: 'BatchNo',
            width: 160,
            render: (text, record) => (
                <Input size='small' value={text} onChange={e => me.onCodeChange(e.target.value, record)} />
            )
        }, {
            title: '操作',
            key: 'operation',
            fixed: 'right',
            width: 60,
            render: (text, record) => <a onClick={() => me.onDelList(record)}>删除</a>,
        }];

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
                    <Button
                        key="submit1"
                        onClick={this.onMatchAll}
                    >
                        添加全部
                    </Button>,
                    <Button
                        key="submit1"
                        onClick={() => {
                            localStorage.setItem("tempRowId", "Y")
                            window.open("/basedata/material");
                        }}
                    >
                        新建物料
                    </Button>,
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
                            columnEmptyText={false}
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
                                        tempSelectedRowKeys.push(record.ID);
                                        record.StockId = "";
                                        record.StockList = StockList;
                                        record.GoodsLocationId = "";
                                        record.GoodsLocationList = [];
                                        record.QTY = 1;
                                        record.Price = 1;
                                        record.AdjustType = "Add";
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
                                            record.StockId = "";
                                            record.StockList = StockList;
                                            record.GoodsLocationId = "";
                                            record.GoodsLocationList = [];
                                            record.QTY = 1;
                                            record.Price = 1;
                                            record.AdjustType = "Add";
                                            tempSelectedRowKeys.push(record.ID);
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
                        />
                    </TabPane>
                    <TabPane tab={selectList.length == 0 ? '已选' : <Badge count={selectList.length} style={{ right: '-10px' }}>已选</Badge>} key="2">
                        {selectList.length == 0 ? <div style={{ textAlign: 'center', padding: 20 }}>
                            <div><ExclamationOutlined style={{ fontSize: 40 }} /></div>
                            <div>暂无物料</div>
                        </div> :
                            <ProTable rowKey="ID"
                                scroll={{ x: 'max-content' }}
                                columns={hasColumns}
                                search={false}
                                dataSource={selectList}
                                pagination={{
                                    pageSize: 10
                                }}
                                size='small'
                                options={{
                                    fullScreen: false,
                                    reload: true,
                                    setting: false,
                                    density: false
                                }}
                                columnEmptyText=''
                                toolBarRender={false}
                            />
                        }
                    </TabPane>

                </Tabs>
            </Modal>)
        );
    }
}
export default connect(({ ivadjustdetail }) => ({
    ivadjustdetail
}))(FormPage);