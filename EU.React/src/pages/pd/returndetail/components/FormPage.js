import React, { Component } from 'react';
import { Modal, InputNumber, Badge, Tabs, Select, Button } from 'antd';
import { connect } from 'umi';
import ProTable from '@ant-design/pro-table';
import { queryAnalysis } from '../service';
import { query as queryMaterial } from '../../../basedata/material/service';
import { query as queryStock } from '../../../basedata/stock/service';
import { query as queryGoodsGocation } from '../../../basedata/goodslocation/service';
import { ExclamationOutlined } from '@ant-design/icons';

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
            selectedRowKeys: [],
            materialList: [],
            stockList: []
        };
    }

    static async getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible && nextProps.modalVisible) {
            me.setState({
                selectList: [],
                selectedRowKeys: [],
            });
            queryStock({
                paramData: { current: 1, pageSize: 200000 },
                sorter: {},
                filter: {},
                moduleCode: 'BD_STOCK_MNG'
            }).then((result) => {
                me.setState({
                    stockList: result.data
                });
            });
        }
    }
    onTabsChange(key) {
        // console.log(key);
    }
    onDelList(record) {
        let { selectList, selectedRowKeys } = this.state;
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
                // }, () => {
            })
        }
    }
    okHandle = async () => {
        const { onSubmit: handleAdd } = this.props;
        // me.formRef.current.resetFields();
        let { selectList } = this.state;
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
        let result = await queryAnalysis({
            paramData: JSON.stringify(params),
            moduleCode: 'BD_MATERIAL_MNG'
        });
        let { selectList, selectedRowKeys } = this.state;
        let tempList = [...selectList];
        let tempSelectedRowKeys = [...selectedRowKeys];
        result.data.forEach((record) => {
            let flag = tempList.findIndex(item => item.ID === record.ID);
            if (flag < 0) {
                tempSelectedRowKeys.push(record.ID);
                tempList.push(record);
            }

        });
        me.setState({
            selectList: tempList,
            selectedRowKeys: tempSelectedRowKeys
        })
    }
    onChangeReturnQTY(values, record) {
        let { selectList } = me.state;
        let tempList = [...selectList];
        let flag = tempList.findIndex(item => item.ID === record.ID);

        if (flag > -1) {
            tempList[flag].ReturnQTY = values;
            me.setState({
                selectList: tempList
            });
        }
    }
    onChangeStockId(values, record) {
        let { selectList } = me.state;
        let tempList = [...selectList];
        let flag = tempList.findIndex(item => item.ID === record.ID);

        if (flag > -1) {
            tempList[flag].StockId = values;
            me.setState({
                selectList: tempList
            });
            queryGoodsGocation({
                paramData: { current: 1, pageSize: 200000 },
                sorter: {},
                filter: { StockId: values },
                moduleCode: 'BD_GOOD_LOCATION_MNG'
            }).then((result) => {
                tempList[flag].GoodsLocationList = result.data;
                me.setState({
                    selectList: tempList
                });
            });
        }
    }
    onChangeGoodsLocationId(values, record) {
        let { selectList } = me.state;
        let tempList = [...selectList];
        let flag = tempList.findIndex(item => item.ID === record.ID);

        if (flag > -1) {
            tempList[flag].GoodsLocationId = values;
            me.setState({
                selectList: tempList
            });
        }
    }

    render() {
        const { modalVisible, onCancel, masterId } = this.props;
        let { selectList, selectedRowKeys, stockList } = this.state;

        let tableParam = {};
        const columns = [{
            title: '生产工单号',
            dataIndex: 'PdOrderNo',
            width: 130
        }, {
            title: '项次',
            dataIndex: 'SourceSerialNumber',
            width: 50
        }, {
            title: '生产产品编号',
            dataIndex: 'PdMaterialNo',
            width: 160
        }, {
            title: '生产产品名称',
            dataIndex: 'PdMaterialName'
        }, {
            title: '生产产品规格',
            dataIndex: 'PdSpecifications'
        }, {
            title: '生产产品单位',
            dataIndex: 'PdUnitName'
        }, {
            title: '生产产品配方',
            dataIndex: 'PdFormula'
        }, {
            title: '生产数量',
            dataIndex: 'OrderQTY'
        }, {
            title: '应发材料编号',
            dataIndex: 'MaterialNo',
            width: 160
        }, {
            title: '应发材料名称',
            dataIndex: 'MaterialName'
        }, {
            title: '应发材料规格',
            dataIndex: 'Specifications'
        }, {
            title: '应发材料单位',
            dataIndex: 'UnitName'
        }, {
            title: '应发数量',
            dataIndex: 'ShouldQTY'
        }, {
            title: '实发数量',
            dataIndex: 'ActualQTY'
        }, {
            title: '关联订单号',
            dataIndex: 'LinkOrderNo'
        }];
        const hasColumns = columns.concat([{
            title: '退库数量',
            dataIndex: 'ReturnQTY',
            width: 120,
            render: (text, record) => (
                <>
                    <InputNumber size='small' value={text} min='0' onChange={value => me.onChangeReturnQTY(value, record)} />
                </>)
        }, {
            title: '仓库',
            dataIndex: 'StockId',
            width: 160,
            render: (text, record) => (
                <Select
                    allowClear
                    showSearch
                    size='small'
                    // filterOption={false}
                    optionFilterProp="children"
                    value={text}
                    onChange={(value, Option) => me.onChangeStockId(value, record)}
                >
                    {stockList && stockList.length > 0 && stockList.map(item => {
                        return (
                            <Option key={item['ID']}>{item['StockNo'] + ' - ' + item['StockNames']}</Option>
                        )
                    })}
                </Select>
            ),
        }, {
            title: '货位',
            dataIndex: 'GoodsLocationId',
            width: 160,
            render: (text, record) => (
                <Select
                    allowClear
                    showSearch
                    size='small'
                    // filterOption={false}
                    optionFilterProp="children"
                    value={text}
                    onChange={(value, Option) => me.onChangeGoodsLocationId(value, record)}
                >
                    {record.GoodsLocationList && record.GoodsLocationList.length > 0 && record.GoodsLocationList.map(item => {
                        return (
                            <Option key={item['ID']}>{item['LocationNo'] + ' - ' + item['LocationNames']}</Option>
                        )
                    })}
                </Select>
            ),
        }, {
            title: '操作',
            key: 'operation',
            fixed: 'right',
            width: 60,
            render: (text, record) => <a onClick={() => me.onDelList(record)}>删除</a>,
        }]);

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

                                return queryAnalysis({
                                    paramData: JSON.stringify(params),
                                    sorter: JSON.stringify(sorter),
                                    filter: JSON.stringify(filter),
                                    masterId
                                })
                            }}
                            options={{
                                fullScreen: false,
                                reload: false,
                                setting: false,
                                density: false,
                                // search: {
                                //     name: 'keyWord',
                                // },
                            }}
                        // toolBarRender={false}
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
export default connect(({ pdrequire }) => ({
    pdrequire
}))(FormPage);