import React, { Component } from 'react';
import { Modal, Input, InputNumber, Badge, Tabs, Button, message, Select } from 'antd';
import { connect } from 'umi';
import ProTable from '@ant-design/pro-table';
import { query as queryMaterial } from '../../machine/service';
import { ExclamationOutlined } from '@ant-design/icons';
import ComBoBox from '@/components/SysComponents/ComboBox';

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
            activeKey: '1'
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

    onQTYChange(value, record, column) {
        let { selectList } = me.state;
        let tempList = [...selectList];
        const i = tempList.findIndex(item => item.ID === record.ID);
        if (i > -1) {
            tempList[i][column] = value;
            me.setState({ selectList: tempList })
        }
    }
    onCodeChange(value, record) {
        let { selectList } = me.state;
        let tempList = [...selectList];
        const i = tempList.findIndex(item => item.ID === record.ID);
        if (i > -1) {
            tempList[i].Remark = value;
            me.setState({ selectList: tempList })
        }
    }
    onTabsChange(key) {
        me.setState({
            activeKey: key
        })
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
        selectList.forEach(record => {
            record.MachineId = record.ID;
        });
        let index = selectList.findIndex(item => !item.QTY);
        if (index > -1) {
            message.error("请输入第" + (index + 1) + "行数据【机台数量】！");
            me.setState({
                activeKey: '2'
            })
            return false;
        }

        handleAdd(selectList, '');
        me.setState({
            activeKey: '1',
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
    //         moduleCode: 'EM_MACHIN_INFO_MNG'
    //     });
    //     let { selectList, selectedRowKeys } = this.state;
    //     let tempList = [...selectList];
    //     let tempSelectedRowKeys = [...selectedRowKeys];
    //     result.data.forEach((record) => {
    //         tempSelectedRowKeys.push(record.ID);
    //         record.QTY = 1;
    //         record.Priority = 1;
    //         record.Reason = '';
    //         tempList.push(record);
    //     });
    //     me.setState({
    //         selectList: tempList,
    //         selectedRowKeys: tempSelectedRowKeys
    //     })
    // }
    render() {
        const { modalVisible, onCancel, masterId } = this.props;
        let { selectList, selectedRowKeys, activeKey } = this.state;
        let PriorityList = [];
        for (let i = 1; i < 21; i++) {
            PriorityList.push(i)
        }
        let tableParam = {};
        const columns = [{
            title: '设备编号',
            dataIndex: 'MachineNo',
            width: 160
        }, {
            title: '设备名称',
            dataIndex: 'MachineName'
        }, {
            title: '产品分类',
            dataIndex: 'TypeName'
        }];
        let hasColumns = [...columns];
        hasColumns.push({
            title: '机台数量',
            dataIndex: 'QTY',
            width: 120,
            render: (text, record) => (
                <>
                    <InputNumber size='small' value={text} min='0' onChange={value => me.onQTYChange(value, record, 'QTY')} />
                </>)
        }, {
            title: '优先级',
            dataIndex: 'Priority',
            width: 120,
            render: (text, record) => (
                <>
                    <Select
                        allowClear
                        size='small'
                        showSearch={true}
                        filterOption={false}
                        defaultValue={1}
                        value={text}
                        onChange={value => me.onQTYChange(value, record, 'Priority')}
                    >
                        {PriorityList.map(item => {
                            return (
                                <Option key={item}>{item}</Option>
                            )
                        })}
                    </Select>
                </>)
        }, {
            title: '标准加工时间',
            dataIndex: 'StandardMachineTime',
            width: 120,
            render: (text, record) => (
                <>
                    <InputNumber size='small' value={text} min='0' onChange={value => me.onQTYChange(value, record, 'StandardMachineTime')} />
                </>)
        }, {
            title: '最大加工时间',
            dataIndex: 'MaxMachineTime',
            width: 120,
            render: (text, record) => (
                <>
                    <InputNumber size='small' value={text} min='0' onChange={value => me.onQTYChange(value, record, 'MaxMachineTime')} />
                </>)
        }, {
            title: '时间单位',
            dataIndex: 'TimeUnit',
            width: 120,
            render: (text, record) => (
                <>
                    <ComBoBox size='small' id='TimeUnit' value={text} onChange={value => me.onQTYChange(value, record, 'TimeUnit')} />
                    {/* <InputNumber size='small' value={text} min='0' onChange={value => me.onQTYChange(value, record, 'StandardMachineTime')} /> */}
                </>)
        }, {
            title: '加工精度',
            dataIndex: 'MachineAccuracy',
            width: 160,
            render: (text, record) => (
                <Input size='small' value={text} onChange={e => me.onQTYChange(e.target.value, record, 'MachineAccuracy')} />
            )
        }, {
            title: '加工说明',
            dataIndex: 'Explantion',
            width: 160,
            render: (text, record) => (
                <Input size='small' value={text} onChange={e => me.onQTYChange(e.target.value, record, 'Explantion')} />
            )
        }, {
            title: '备注',
            dataIndex: 'Remark',
            width: 160,
            render: (text, record) => (
                <Input size='small' value={text} onChange={e => me.onCodeChange(e.target.value, record)} />
            )
        }, {
            title: '操作',
            key: 'operation',
            fixed: 'right',
            width: 60,
            render: (text, record) => <a onClick={() => me.onDelList(record)}>删除</a>
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
                    // <Button
                    //     key="submit1"
                    //     onClick={() => {
                    //         localStorage.setItem("tempRowId", "Y")
                    //         window.open("/basedata/material");
                    //     }}
                    // >
                    //     新建物料
                    // </Button>,
                    <Button key="submit" disabled={selectList.length == 0 ? true : false} type="primary" onClick={this.okHandle}>
                        确认
                    </Button>
                ]}
            >
                <Tabs activeKey={activeKey} onChange={this.onTabsChange}>
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
                                        record.QTY = 1;
                                        record.Priority = 1;
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
                                            record.QTY = 1;
                                            record.Priority = 1;
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
                                    moduleCode: 'EM_MACHIN_INFO_MNG',
                                    Id: masterId
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
export default connect(({ psmaterial }) => ({
    psmaterial
}))(FormPage);