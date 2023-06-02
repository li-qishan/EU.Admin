import React, { Component } from 'react';
import { Modal, Input, InputNumber, Badge, Tabs, Button, DatePicker, message } from 'antd';
import { connect } from 'umi';
import { ExclamationOutlined } from '@ant-design/icons';
import { GetSourceList } from '../service';
import dayjs from 'dayjs';
import ProTable from '@ant-design/pro-table';
import ComBoBox from '@/components/SysComponents/ComboBox';

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
            selectedRowKeys: []
        };
    }
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
        let { selectList } = this.state;
        let flag = -1;
        selectList.forEach(record => {
            if (!record.CollectionAmount)
                flag = 1;
        });
        if (flag > -1) {
            message.error("请输入收款金额！");
            return;
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
            if (type == 'DueDate')
                tempList[index][type] = dayjs(values).format('YYYY/MM/DD');
            else if (type == 'Remark' || type == 'BankName' || type == 'InvoiceNo') {
                tempList[index][type] = values.target.value
            }
            else
                tempList[index][type] = values;
        }
        this.setState({ selectList: tempList })
    }
    render() {
        const { modalVisible, onCancel, masterId } = this.props;
        let { selectList, selectedRowKeys } = this.state;

        let tableParam = {};
        let columns = [{
            title: '来源单号',
            dataIndex: 'SourceOrderNo'
        }, {
            title: '含税金额',
            dataIndex: 'TaxIncludedAmount',
            valueType: 'digit'
        }, {
            title: '预付比例',
            dataIndex: 'Percent',
            valueType: 'digit'
        }, {
            title: '预付金额',
            dataIndex: 'Amount',
            valueType: 'digit'
        }];

        let hasColumns = [{
            title: '序号',
            width: 60,
            dataIndex: 'Key',
            valueType: 'digit'
        }];
        hasColumns = hasColumns.concat(columns)

        hasColumns.push({
            title: '收款方式',
            dataIndex: 'CollectionType',
            width: 90,
            render: (text, record) => (
                <ComBoBox
                    id="CollectionType"
                    placeholder="请输入"
                    size='small'
                    value={text} onChange={(value) => me.onCommonChange(value, record, 'CollectionType')} />
            )
        }, {
            title: '收款金额',
            dataIndex: 'CollectionAmount',
            render: (text, record) => (
                <InputNumber
                    placeholder="请输入"
                    size='small'
                    value={text}
                    min='0'
                    max={record.MaxCollectionAmount}
                    onChange={(value) => me.onCommonChange(value, record, 'CollectionAmount')}
                />
            )
        }, {
            title: '银行名称',
            dataIndex: 'BankName',
            width: 150,
            render: (text, record) => (
                <Input size='small' defaultValue={text} onChange={(value) => me.onCommonChange(value, record, 'BankName')} />
            )
        }, {
            title: '票号',
            dataIndex: 'InvoiceNo',
            width: 150,
            render: (text, record) => (
                <Input size='small' defaultValue={text} onChange={(value) => me.onCommonChange(value, record, 'InvoiceNo')} />
            )
        }, {
            title: '到期日',
            dataIndex: 'DueDate',
            width: 150,
            render: (text, record) => (
                <DatePicker
                    size='small'
                    defaultValue={text}
                    onChange={(value) => me.onCommonChange(value, record, 'DueDate')}
                />
            )
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
                                        record.Key = tempSelectedRowKeys.length + 1;
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
                                            record.Key = tempSelectedRowKeys.length + 1;
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
                                // return queryMaterial({
                                //     paramData: JSON.stringify(params),
                                //     sorter: JSON.stringify(sorter),
                                //     filter: JSON.stringify(filter),
                                //     moduleCode: 'BD_MATERIAL_MNG'
                                // })
                                return GetSourceList({
                                    paramData: JSON.stringify(params),
                                    sorter: JSON.stringify(sorter),
                                    filter: JSON.stringify(filter),
                                    // source: Source,
                                    masterId
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
                            toolBarRender={false}
                        // toolBarRender={() => [
                        //     <Select
                        //         value={Source}
                        //         defaultValue={Source}
                        //         onChange={(value) => {
                        //             this.setState({ Source: value });
                        //             this.ref.current.reloadAndRest();
                        //         }}
                        //         style={{ marginLeft: 10, width: 180 }}
                        //     >
                        //         <Select.Option value="ApCheckOrder">应付对账单</Select.Option>
                        //         <Select.Option value="OtherOrder">采购入库单、采购费用单</Select.Option>
                        //     </Select>
                        // ]}
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
