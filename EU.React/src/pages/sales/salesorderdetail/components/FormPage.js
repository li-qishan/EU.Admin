import React, { Component } from 'react';
import { Modal, Input, InputNumber, Badge, Tabs, Button, message } from 'antd';
import { connect } from 'umi';
import ProTable from '@ant-design/pro-table';
import { query as queryMaterial } from '../../../basedata/material/service';
import { ExclamationOutlined } from '@ant-design/icons';

const { TabPane } = Tabs;

let me;
class FormPage extends Component {
    formRef = React.createRef();
    ref = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',//从表主键
            isLoading: true,
            selectList: [],
            QTYObject: {},
            PriceObject: {},
            CodeObject: {},
            selectedRowKeys: []
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
    static getDerivedStateFromProps(nextProps) {
        if (me.props.modalVisible !== nextProps.modalVisible) {
            me.setState({
                selectList: [],
                selectedRowKeys: [],
                QTYObject: {},
                PriceObject: {},
                CodeObject: {}
            })
        }
    }

    onQTYChange(value, record) {
        let { QTYObject } = this.state;
        QTYObject[record.ID] = value;
        this.setState({ QTYObject })

    }
    onPriceChange(value, record) {
        let { PriceObject } = this.state;
        PriceObject[record.ID] = value;
        this.setState({ PriceObject })
    }
    onCodeChange(value, record) {
        let { CodeObject } = this.state;
        CodeObject[record.ID] = value;
        this.setState({ CodeObject })
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
        let { selectList, QTYObject, PriceObject, CodeObject } = this.state;
        let flag = -1;
        selectList.forEach(record => {
            record.MaterialId = record.ID;
            record.MaterialName = record.MaterialNames;
            record.MaterialSpecifications = record.Specifications;
            record.MaterialUnitId = record.UnitId;
            record.QTY = QTYObject[record.ID] ?? null;
            record.Price = PriceObject[record.ID] ?? null;
            record.CustomerMaterialCode = CodeObject[record.ID] ?? null;

            if (!record.QTY || !record.Price)
                flag = 1;
        });

        if (flag > -1) {
            message.error("请输入数量或单价！");
            return false;
        }

        handleAdd(selectList, '');
        me.setState({
            selectList: [],
            selectedRowKeys: [],
            QTYObject: {},
            PriceObject: {},
            CodeObject: {}
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
        let { selectList, selectedRowKeys } = this.state;
        let tempList = [...selectList];
        let tempSelectedRowKeys = [...selectedRowKeys];
        result.data.forEach((record) => {
            tempSelectedRowKeys.push(record.ID);
            tempList.push(record);
        });
        me.setState({
            selectList: tempList,
            selectedRowKeys: tempSelectedRowKeys
        })
    }
    render() {
        const { modalVisible, onCancel } = this.props;
        let { QTYObject, selectList, PriceObject, CodeObject, selectedRowKeys } = this.state;

        let tableParam = {};
        const columns = [{
            title: '物料编号',
            dataIndex: 'MaterialNo',
            width: 160
        }, {
            title: '物料名称',
            dataIndex: 'MaterialNames'
        }, {
            title: '规格',
            dataIndex: 'Specifications'
        }, {
            title: '描述',
            dataIndex: 'Description'
        }, {
            title: '数量',
            dataIndex: 'QTY',
            width: 160,
            render: (text, record) => (
                <>
                    <InputNumber size='small' value={QTYObject[record.ID] ?? null} min='0' onChange={value => me.onQTYChange(value, record)} />
                </>)
        }, {
            title: '单价',
            dataIndex: 'Price',
            width: 160,
            render: (text, record) => (
                <InputNumber size='small' value={PriceObject[record.ID] ?? null} min='0' onChange={value => me.onPriceChange(value, record)} />
            )
        }, {
            title: '客户物料编码',
            dataIndex: 'CustomerMaterialCode',
            width: 160,
            render: (text, record) => (
                <Input size='small' value={CodeObject[record.ID] ?? null} onChange={e => me.onCodeChange(e.target.value, record)} />
            )
        }];
        const hasColumns = [{
            title: '物料编号',
            dataIndex: 'MaterialNo',
            width: 160
        }, {
            title: '物料名称',
            dataIndex: 'MaterialNames'
        }, {
            title: '规格',
            dataIndex: 'Specifications'
        }, {
            title: '描述',
            dataIndex: 'Description'
        }, {
            title: '数量',
            dataIndex: 'QTY',
            width: 160,
            render: (text, record) => (
                <>
                    <InputNumber size='small' value={QTYObject[record.ID] ?? null} min='0' onChange={value => me.onQTYChange(value, record)} />
                </>)
        }, {
            title: '单价',
            dataIndex: 'Price',
            width: 160,
            render: (text, record) => (
                <InputNumber size='small' value={PriceObject[record.ID] ?? null} min='0' step={record.Step} onChange={value => me.onPriceChange(value, record)} />
            )
        }, {
            title: '客户物料编码',
            dataIndex: 'CustomerMaterialCode',
            width: 160,
            render: (text, record) => (
                <Input size='small' value={CodeObject[record.ID] ?? null} onChange={e => me.onCodeChange(e.target.value, record)} />
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
                                toolBarRender={false}
                            />
                        }
                    </TabPane>

                </Tabs>
            </Modal>)
        );
    }
}
export default connect(({ salesorderdetail }) => ({
    salesorderdetail,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);