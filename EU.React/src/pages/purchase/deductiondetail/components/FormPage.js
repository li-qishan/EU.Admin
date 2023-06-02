import React, { Component } from 'react';
import { Modal, Input, InputNumber, Badge, Form, Tabs, Button, Select, Row, Col } from 'antd';
import { connect } from 'umi';
import { ExclamationOutlined } from '@ant-design/icons';
import { queryFeeSource } from '../service';
import dayjs from 'dayjs';
import ProTable from '@ant-design/pro-table';
const { TabPane } = Tabs;
const FormItem = Form.Item;
import ComboGrid from '@/components/SysComponents/ComboGrid';

let me;
class FormPage extends Component {
    ref = React.createRef();
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',//从表主键
            isLoading: true,
            selectList: [],
            selectedRowKeys: [],
            Source: null
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
        let { selectList, Source } = this.state;

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
            else if (type == 'Remark' || type == 'Reason')
                tempList[index][type] = values.target.value
            else
                tempList[index][type] = values;

            let taxRate = tempList[index].TaxRate ?? 0;
            let TaxAmount = ((tempList[index].NoTaxAmount ?? 0) * taxRate) / 100;
            TaxAmount = Number(TaxAmount).toFixed(2)
            let TaxIncludedAmount = tempList[index].NoTaxAmount + TaxAmount;
            TaxIncludedAmount = Number(TaxIncludedAmount).toFixed(2);
            tempList[index].TaxAmount = Number(TaxAmount);
            tempList[index].TaxIncludedAmount = Number(TaxIncludedAmount);
        }
        this.setState({ selectList: tempList })
    }
    okFromHandle = async () => {
        const { onSubmit: handleAdd, pofeeorder: { detail: { TaxRate } } } = this.props;
        const fieldsValue = await me.formRef.current.validateFields();
        // me.formRef.current.resetFields();
        const { Id } = this.state;
        if (Id)
            fieldsValue.ID = Id;
        if (fieldsValue.TaxRate == null)
            fieldsValue.TaxRate = TaxRate;
        handleAdd([fieldsValue]);
    };
    render() {
        const { modalVisible, onCancel, masterId, pofeeorder: { detail: { TaxRate } } } = this.props;
        let { selectList, selectedRowKeys, Source } = this.state;

        let tableParam = {};
        let columns = [{
            title: '单据来源',
            dataIndex: 'OrderSource',
            filters: false,
            width: 120,
            valueEnum: {
                null: {
                    text: '无',
                },
                'Material': {
                    text: '物料',
                },
                'POOrder': {
                    text: '采购单',
                },
                'POInOrder': {
                    text: '采购入库单',
                }
            }
        }, {
            title: '来源单号',
            hideInTable: Source == 'Material' ? true : false,
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
            title: '数量',
            hideInTable: Source == 'Material' ? true : false,
            dataIndex: 'QTY',
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
            title: '数量',
            dataIndex: 'QTY',
            render: (text, record) => (
                <InputNumber
                    placeholder="请输入"
                    size='small'
                    value={text}
                    min='0'
                    max={record.MaxReturnQTY}
                    onChange={(value) => me.onCommonChange(value, record, 'QTY')}
                />
            )

        }, {
            title: '单价',
            dataIndex: 'Price',
            render: (text, record) => (
                <InputNumber
                    placeholder="请输入"
                    size='small'
                    value={text}
                    min='0'
                    max={record.MaxReturnQTY}
                    onChange={(value) => me.onCommonChange(value, record, 'Price')}
                />
            )
        }, {
            title: '税率',
            dataIndex: 'TaxRate',
            render: (text, record) => (
                <InputNumber
                    placeholder="请输入"
                    size='small'
                    value={text}
                    min='0'
                    max={record.MaxReturnQTY}
                    onChange={(value) => me.onCommonChange(value, record, 'TaxRate')}
                />
            )
        }, {
            title: '未税金额',
            dataIndex: 'NoTaxAmount',
            render: (text, record) => (
                <InputNumber
                    placeholder="请输入"
                    size='small'
                    value={text}
                    min='0'
                    max={record.MaxReturnQTY}
                    onChange={(value) => me.onCommonChange(value, record, 'NoTaxAmount')}
                />
            )
        }, {
            title: '税额',
            dataIndex: 'TaxAmount',
            valueType: 'digit'
        }, {
            title: '含税金额',
            dataIndex: 'TaxIncludedAmount',
            valueType: 'digit'
        }, {
            title: '费用原因',
            dataIndex: 'Reason',
            width: 150,
            render: (text, record) => (
                <Input size='small' defaultValue={text} onChange={(value) => me.onCommonChange(value, record, 'Reason')} />
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
        return (<>
            {Source == null ? <Modal
                destroyOnClose
                title='其他费用'
                open={modalVisible}
                // onOk={this.okHandle}
                maskClosable={false}
                width={1100}
                onCancel={() => onCancel()}
                onOk={this.okFromHandle}
            >
                <Form
                    labelCol={{ span: 8 }}
                    wrapperCol={{ span: 12 }}
                    // onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                >
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="Source" label="来源" >
                                <Select
                                    value={Source}
                                    defaultValue={Source}
                                    // style={{ width: '100%' }}
                                    onChange={(value) => {
                                        this.setState({ Source: value });
                                    }}
                                >
                                    <Select.Option>无</Select.Option>
                                    <Select.Option value="Material">物料</Select.Option>
                                    <Select.Option value="POOrder">采购单</Select.Option>
                                    <Select.Option value="POInOrder">采购入库单</Select.Option>
                                </Select>
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="MaterialId" label="货品编号">
                                <ComboGrid
                                    api="/api/Material/GetPageList"
                                    itemkey="ID"
                                    param={{
                                        IsActive: true
                                    }}
                                    // itemvalue="MaterialNo,MaterialNames"
                                    itemvalue={['MaterialNo', 'MaterialNames']}
                                // onChange={(value) => {
                                //     this.setState({ StockKeeperId: value })
                                // }}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="QTY" label="数量" rules={[{
                                type: 'number', min: 0, message: '数量必须大于0 !'
                            }]}>
                                <InputNumber placeholder="请输入" />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="Price" label="单价">
                                <InputNumber placeholder="请输入" />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="TaxRate" label="税率">
                                <InputNumber placeholder="请输入" defaultValue={TaxRate} rules={[{
                                    type: 'number', min: 0, message: '税率最小值为0 !'
                                }]} onChange={async (value) => {
                                    let NoTaxAmount = await me.formRef.current.getFieldValue("NoTaxAmount");
                                    if (NoTaxAmount || NoTaxAmount == 0) {
                                        let TaxAmount = (value * NoTaxAmount) / 100;
                                        TaxAmount = Number(TaxAmount).toFixed(2)
                                        let TaxIncludedAmount = value + TaxAmount;
                                        TaxIncludedAmount = Number(TaxIncludedAmount).toFixed(2)
                                        me.formRef.current.setFieldsValue({ TaxAmount: Number(TaxAmount), TaxIncludedAmount: Number(TaxIncludedAmount) });
                                    }
                                }} />
                            </FormItem>

                        </Col>
                        <Col span={8}>
                            <FormItem name="NoTaxAmount" label="未税金额" rules={[{ required: true }, { type: 'number', min: 0, message: '未税金额最小值为0 !' }]}>
                                <InputNumber placeholder="请输入" onChange={async (value) => {
                                    let taxRate = (await me.formRef.current.getFieldValue("TaxRate")) ?? TaxRate;
                                    let TaxAmount = (value * taxRate) / 100;
                                    TaxAmount = Number(TaxAmount).toFixed(2)
                                    let TaxIncludedAmount = value + TaxAmount;
                                    TaxIncludedAmount = Number(TaxIncludedAmount).toFixed(2);
                                    me.formRef.current.setFieldsValue({ TaxAmount: Number(TaxAmount), TaxIncludedAmount: Number(TaxIncludedAmount) });
                                }} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="TaxAmount" label="税额" rules={[{ required: true }, { type: 'number', min: 0, message: '税额最小值为0 !' }]}>
                                <InputNumber placeholder="请输入" />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="TaxIncludedAmount" label="含税金额">
                                <InputNumber placeholder="请输入" rules={[{
                                    type: 'number', min: 0, message: '税率最小值为0 !'
                                }]} />
                            </FormItem>

                        </Col>
                        <Col span={8}>
                            <FormItem name="Reason" label="费用原因"  >
                                <Input placeholder="请输入" />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="Remark" label="备注"  >
                                <Input placeholder="请输入" />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                        </Col>
                        <Col span={8}>
                        </Col>
                    </Row>
                </Form>
            </Modal>
                : <Modal
                    destroyOnClose
                    title=''
                    open={modalVisible}
                    // onOk={this.okHandle}
                    maskClosable={false}
                    width={1100}
                    // closable={false}
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
                ><Tabs defaultActiveKey="1" onChange={this.onTabsChange}>
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
                                    return queryFeeSource({
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
                                        <Select.Option>无</Select.Option>
                                        <Select.Option value="Material">物料</Select.Option>
                                        <Select.Option value="POOrder">采购单</Select.Option>
                                        <Select.Option value="POInOrder">采购入库单</Select.Option>
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
                </Modal>}
        </>);
    }
}
export default connect(({ pofeeorder }) => ({
    pofeeorder
}))(FormPage);
