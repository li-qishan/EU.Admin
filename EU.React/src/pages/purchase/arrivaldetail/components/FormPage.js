import React, { Component } from 'react';
import { Modal, Input, InputNumber, Badge, Tabs, Button } from 'antd';
import { connect } from 'umi';
import ProTable from '@ant-design/pro-table';
import { queryWaitArrival } from '../service';
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
        handleAdd(selectList, '');
        me.setState({
            selectList: [],
            selectedRowKeys: []
        })
    };
    onCommonChange(values, record, type) {
        let { selectList, selectedRowKeys } = this.state;
        //let tempList = selectList;
        let tempList = [...selectList];
        const index = tempList.findIndex(item => item.ID === record.ID);
        if (index > -1) {
            if (type == 'ReserveDeliveryTime')
                tempList[index][type] = dayjs(values).format('YYYY/MM/DD');
            else
                tempList[index][type] = values;
            this.setState({ selectList: tempList })
        }
    }
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
    //         tempSelectedRowKeys.push(record.ID);
    //         tempList.push(record);
    //     });
    //     me.setState({
    //         selectList: tempList,
    //         selectedRowKeys: tempSelectedRowKeys
    //     })
    // }
    render() {
        const { modalVisible, onCancel, masterId } = this.props;
        let { selectList, selectedRowKeys, Source } = this.state;

        let tableParam = {};
        const columns = [{
            title: '单据来源',
            dataIndex: 'OrderSource',
            width: 100,
            filters: false,
            valueEnum: {
                'POOrder': {
                    text: '采购单',
                },
                'Sales': {
                    text: '销售单',
                }
            }
        }, {
            title: '来源单号',
            dataIndex: 'SourceOrderNo',
        }, {
            title: '物料名称',
            dataIndex: 'MaterialName'
        }, {
            title: '规格',
            dataIndex: 'Specifications'
        }, {
            title: '采购数量',
            dataIndex: 'PurchaseQTY'
        }, {
            title: '剩余采购数量',
            dataIndex: 'MaxArrivalQTY'
        }, {
            title: '预定交期',
            dataIndex: 'ReserveDeliveryTime',
            render: text => <>{text ? text.substring(0, 10) : null}</>
        }];
        let hasColumns = [...columns];
        hasColumns.push({
            title: '到货数量',
            dataIndex: 'ArrivalQTY',
            render: (text, record) => (
                <InputNumber
                    placeholder="请输入"
                    value={text}
                    min='0'
                    max={record.MaxArrivalQTY}
                    onChange={(value) => me.onCommonChange(value, record, 'ArrivalQTY')}
                />
            )
            // }, {
            //     title: '预定交期',
            //     dataIndex: 'ReserveDeliveryTime',
            //     render: (text, record) => (
            //         <DatePicker defaultValue={dayjs(text, 'YYYY/MM/DD')} format={'YYYY/MM/DD'} onChange={(value) => me.onCommonChange(value, record, 'ReserveDeliveryTime')} />
            //     ),
            //     width: 150
        }, {
            title: '备注',
            dataIndex: 'Remark',
            render: (text, record) => (
                <Input defaultValue={text} onChange={(value) => me.onCommonChange(value, record, 'Remark')} />
            )
        }, {
            title: '操作',
            key: 'operation',
            fixed: 'right',
            width: 60,
            render: (text, record) => <a onClick={() => me.onDelList(record)}>删除</a>
        });
        // const data = [];
        // let i = 0;
        // formList.forEach(element => {
        //     element.key = i;
        //     if (!element.ReserveDeliveryTime)
        //         element.ReserveDeliveryTime = dayjs();
        //     data.push(element);
        //     i++;
        // });
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

                                return queryWaitArrival({
                                    paramData: JSON.stringify(params),
                                    sorter: JSON.stringify(sorter),
                                    filter: JSON.stringify(filter),
                                    source: Source,
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
                                toolBarRender={false}
                                columnEmptyText={''}
                            />
                        }
                    </TabPane>

                </Tabs>
            </Modal>)
        );
    }
}
export default connect(({ poarrivaldetail, poorder }) => ({
    poarrivaldetail, poorder
}))(FormPage);
