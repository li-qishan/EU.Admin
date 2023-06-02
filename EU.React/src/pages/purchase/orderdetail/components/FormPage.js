import React, { Component } from 'react';
import { Modal, Input, InputNumber, Badge, Tabs, Button, message, Select, DatePicker } from 'antd';
import { connect } from 'umi';
import ProTable from '@ant-design/pro-table';
import { query as queryMaterial } from '../../../basedata/material/service';
import { ExclamationOutlined } from '@ant-design/icons';
import { queryWaitPurchase } from '../service';
import { GetById as getSupplier } from '../../../basedata/supplier/service';
import dayjs from 'dayjs'
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
                selectedRowKeys: []
            });
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
        let flag = -1;
        selectList.forEach(record => {
            if (!Source)
                record.MaterialId = record.ID;

            if ((!record.PurchaseQTY || !record.Price) && record.Price < 0)
                flag = 1;
        });

        if (flag > -1) {
            message.error("请输入数量或单价！");
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
    render() {
        const { modalVisible, onCancel, masterId } = this.props;
        let { selectList, selectedRowKeys, Source } = this.state;

        let tableParam = {};
        let columns = [{
            title: '单据来源',
            dataIndex: 'OrderSource',
            filters: false,
            valueEnum: {
                'Requestion': {
                    text: '请购单',
                },
                'Sales': {
                    text: '销售单',
                },
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
            title: Source == 'Sales' ? '订单数量' : '请购数量',
            dataIndex: 'QTY',
            valueType: 'digit'
        }, {
            title: Source == 'Sales' ? '订单剩余数量' : '请购剩余数量',
            dataIndex: 'MaxPurchaseQTY',
            valueType: 'digit'
            // }, {
            //     title: '单价',
            //     dataIndex: 'Price'
            // }, {
            //     title: '未税金额',
            //     dataIndex: 'NoTaxAmount',
            // }, {
            //     title: '税额',
            //     dataIndex: 'TaxAmount'
            // }, {
            //     title: '含税金额',
            //     dataIndex: 'TaxIncludedAmount'
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
        let hasColumns = [...columns];
        hasColumns.push({
            title: '数量',
            dataIndex: 'PurchaseQTY',
            render: (text, record) => (
                <InputNumber
                    placeholder="请输入"
                    value={text}
                    min='1'
                    // max={record.MaxPurchaseQTY}
                    onChange={(value) => me.onCommonChange(value, record, 'PurchaseQTY')}
                />

            )
        }, {
            title: '单价',
            dataIndex: 'Price',
            render: (text, record) => (
                <InputNumber
                    placeholder="请输入"
                    value={text}
                    min='0'
                    onChange={(value) => me.onCommonChange(value, record, 'Price')}
                />
            ),
            width: 100
        }, {
            title: '预定交期',
            dataIndex: 'ReserveDeliveryTime',
            //render: text => <>{text ? text.substring(0, 10) : null}</>,
            render: (text, record) => (
                <DatePicker defaultValue={dayjs(text, 'YYYY/MM/DD')} format={'YYYY/MM/DD'} onChange={(value) => me.onCommonChange(value, record, 'ReserveDeliveryTime')} />
            ),
            width: 150
        }, {
            title: '备注',
            dataIndex: 'Remark',
            width: 150,
            render: (text, record) => (
                <Input defaultValue={text} onChange={(value) => me.onCommonChange(value, record, 'Remark')} />
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
                                        tempSelectedRowKeys.push(record.ID);
                                        if (!Source)
                                            record.PurchaseQTY = 1;
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
                                            tempSelectedRowKeys.push(record.ID);
                                            if (!Source)
                                                record.PurchaseQTY = 1;
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
                                    onChange={(value) => {
                                        this.setState({ Source: value });
                                        this.ref.current.reloadAndRest();
                                    }}
                                    style={{ marginLeft: 10, width: 100 }}
                                >
                                    <Select.Option value="">无</Select.Option>
                                    <Select.Option value="Requestion">请购单</Select.Option>
                                    <Select.Option value="Sales">销售单</Select.Option>
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
export default connect(({ poorderdetail, poorder }) => ({
    poorderdetail, poorder
}))(FormPage);
