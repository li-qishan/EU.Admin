import React, { Component } from 'react';
import { Modal, Input, InputNumber, Badge, Tabs, Button, message,Switch } from 'antd';
import { connect } from 'umi';
import ProTable from '@ant-design/pro-table';
import { query as queryMaterial } from '../../process/service';
import ComboGrid from '@/components/SysComponents/ComboGrid';
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
                activeKey:'1',
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
            record.ProcessId = record.ID;
        });
        // let index = selectList.findIndex(item => !item.Dosage);
        // if (index > -1) {
        //     message.error("请输入第" + (index + 1) + "行数据【用量】！");
        //     me.setState({
        //         activeKey: '2'
        //     })
        //     return false;
        // }
        // index = selectList.findIndex(item => !item.DosageBase);
        // if (index > -1) {
        //     message.error("请选择第" + (index + 1) + "行数据【用量基数】！");
        //     me.setState({
        //         activeKey: '2'
        //     })
        //     return false;
        // }
        // index = selectList.findIndex(item => !item.WastageRate);
        // if (index > -1) {
        //     message.error("请选择第" + (index + 1) + "行数据【损耗率】！");
        //     me.setState({
        //         activeKey: '2'
        //     })
        //     return false;
        // }

        handleAdd(selectList, '');
        me.setState({
            activeKey: '1',
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
            moduleCode: 'PS_PROCESS_MNG'
        });
        let { selectList, selectedRowKeys } = this.state;
        let tempList = [...selectList];
        let tempSelectedRowKeys = [...selectedRowKeys];
        result.data.forEach((record) => {
            tempSelectedRowKeys.push(record.ID);
            record.DosageBase = 1;
            record.Reason = '';
            tempList.push(record);
        });
        me.setState({
            selectList: tempList,
            selectedRowKeys: tempSelectedRowKeys
        })
    }
    render() {
        const { modalVisible, onCancel, masterId } = this.props;
        let { selectList, selectedRowKeys, activeKey } = this.state;
        let tableParam = {};
        const columns = [{
            title: '工序编号',
            dataIndex: 'ProcessNo'
        }, {
            title: '工序名称',
            dataIndex: 'ProcessName'
        }, {
            title: '车间',
            dataIndex: 'WorkShopName'
        }, {
            title: '加工类型',
            dataIndex: 'MachiningType',
            valueEnum: {
                OutSource: {
                    text: '外协',
                },
                QualityTesting: {
                    text: '质检',
                },
                Self: {
                    text: '自制',
                },
                QualityTesting: {
                    text: '质检',
                }
            }
        }, {
            title: '工序类型',
            dataIndex: 'ProcessType',
            valueEnum: {
                MachineProcessing: {
                    text: '机台加工',
                },
                NonMachineProcessing: {
                    text: '非机台加工',
                }
            }
        }, {
            title: '外协定价',
            dataIndex: 'PricingType',
            valueEnum: {
                Weight: {
                    text: '重量',
                },
                WorkHours: {
                    text: '工时',
                }
            }
        }];
        let hasColumns = [...columns];
        hasColumns.push({
            title: '机台',
            dataIndex: 'MachineId',
            width: 180,
            render: (text, record) => (
                <>
                    <ComboGrid
                        api="/api/Process/GetMachineList"
                        itemkey="MachineId"
                        param={{
                            IsActive: true,
                            moduleCode: 'PS_PROCESS_MACHINE_MNG',
                            pageSize: 1000000
                        }}
                        itemvalue={['MachineNo', 'MachineName']}
                        disabled={!record.ID ? true : false}
                        parentId={{ 'ProcessId': record.ID }}
                        value={text}
                        onChange={value => { me.onQTYChange(value, record, 'MachineId') }}
                    />
                </>)
        }, {
            title: '单重单位',
            dataIndex: 'WeightUnit',
            width: 120,
            render: (text, record) => (
                <>
                    <ComBoBox size='small' id='WeightUnit' value={text} onChange={value => me.onQTYChange(value, record, 'WeightUnit')} />
                    {/* <InputNumber size='small' value={text} min='0' onChange={value => me.onQTYChange(value, record, 'StandardMachineTime')} /> */}
                </>)
        }, {
            title: '单重',
            dataIndex: 'SingleWeight',
            width: 120,
            render: (text, record) => (
                <>
                    <InputNumber size='small' value={text} min='0' onChange={value => me.onQTYChange(value, record, 'SingleWeight')} />
                </>)
        }, {
            title: '加工天数',
            dataIndex: 'ProcessingDays',
            width: 120,
            render: (text, record) => (
                <>
                    <InputNumber size='small' value={text} min='0' onChange={value => me.onQTYChange(value, record, 'ProcessingDays')} />
                </>)
        }, {
            title: '调机时间',
            dataIndex: 'SetupTime',
            width: 120,
            render: (text, record) => (
                <>
                    <InputNumber size='small' value={text} min='0' onChange={value => me.onQTYChange(value, record, 'SetupTime')} />
                </>)
        }, {
            title: '工时单位',
            dataIndex: 'TimeUnit',
            width: 120,
            render: (text, record) => (
                <>
                    <ComBoBox size='small' id='TimeUnit' value={text} onChange={value => me.onQTYChange(value, record, 'TimeUnit')} />
                    {/* <InputNumber size='small' value={text} min='0' onChange={value => me.onQTYChange(value, record, 'StandardMachineTime')} /> */}
                </>)
        }, {
            title: '标准工时',
            dataIndex: 'StandardHours',
            width: 120,
            render: (text, record) => (
                <>
                    <InputNumber size='small' value={text} min='0' onChange={value => me.onQTYChange(value, record, 'StandardHours')} />
                </>)
        }, {
            title: '标准工价',
            dataIndex: 'StandardWages',
            width: 120,
            render: (text, record) => (
                <>
                    <InputNumber size='small' value={text} min='0' onChange={value => me.onQTYChange(value, record, 'StandardWages')} />
                </>)
        }, {
            title: '检验后转移',
            dataIndex: 'IsTransfer',
            width: 120,
            render: (text, record) => (
                <>
                    <Switch size='small' checked={text} onChange={value => me.onQTYChange(value, record, 'IsTransfer')} />
                </>)
        }, {
            title: '工艺不良率%',
            dataIndex: 'RejectRate',
            width: 120,
            render: (text, record) => (
                <>
                    <InputNumber size='small' value={text} min='0' onChange={value => me.onQTYChange(value, record, 'RejectRate')} />
                </>)
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
                                        record.DosageBase = 1;
                                        record.Reason = '';
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
                                            record.DosageBase = 1;
                                            record.Reason = '';
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
                                    moduleCode: 'PS_PROCESS_MNG',
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