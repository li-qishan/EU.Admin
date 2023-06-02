import React, { Component } from 'react';
import { Row, Col, Tree, Tabs, Space, Button } from 'antd';
import { connect } from 'umi';
import { DownOutlined, PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import { query, BatchDelete, Delete } from '../service';
import Index from '../index';
import FormPage from './FormPage'
import SmProTable from '@/components/SysComponents/SmProTable';

let moduleCode = "BD_MATERIAL_TYPE_MNG";
let me;
const { TabPane } = Tabs;
// const treeData = [
//     {
//         title: 'Node1',
//         value: '0-0',
//         selectable:false,
//         children: [
//             {
//                 title: 'Node1-Child Node1',
//                 value: '0-0-1',
//             },
//             {
//                 title: 'Node1-Child Node2',
//                 value: '0-0-2',
//             },
//         ],
//     },
//     {
//         title: 'Node2',
//         value: '0-1',
//         children: []
//     },
// ];

// const options = [
//     {
//         value: 'zhejiang',
//         label: 'Zhejiang',
//         children: [
//             {
//                 value: 'hangzhou',
//                 label: 'Hangzhou',
//                 children: [
//                     {
//                         value: 'xihu',
//                         label: 'West Lake',
//                     },
//                 ],
//             },
//         ],
//     },
//     {
//         value: 'jiangsu',
//         label: 'Jiangsu',
//         children: [
//             {
//                 value: 'nanjing',
//                 label: 'Nanjing',
//                 children: [
//                 ],
//             },
//         ],
//     },
// ];


class TableList extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            value: undefined,
            Id: null,
            selectedKeys: null,
            parentTypeId: null
        };
    }
    componentWillMount() {
        const { dispatch } = this.props;
        dispatch({
            type: 'materialtype/getModuleInfo',
            payload: { moduleCode },
        });
        dispatch({
            type: 'materialtype/getAllMaterialType'
        });
    }
    // onChange = value => {
    //     console.log(value);
    //     this.setState({ value });
    // };
    // onChange(value) {
    //     console.log(value);
    // }
    onSelect = (value, info) => {
        const { Id, selectedKeys } = this.state;
        if (value[0] != "All")
            me.setState({ selectedKeys: value[0], Id: value[0], tabkey: '1' });
        else
            me.setState({ selectedKeys: null, Id: null, tabkey: '1' });
        // console.log('selected', selectedKeys, info);
    };
    onTabClick(key) {
        me.setState({ tabkey: key });
    }
    render() {
        const { dispatch, materialtype: { moduleInfo, tableParam, treeData } } = this.props;
        const { Id, selectedKeys, parentTypeId, tabkey } = this.state;

        //#region 操作栏按钮方法
        const action = {}
        //#endregion

        return (
            <div>
                <Row gutter={[16, 16]} style={{ background: '#fff' }}>
                    <Col span={6}>
                        {treeData.length > 0 ? <>
                            <Space style={{ display: 'flex', paddingTop: 12 }}>
                                <Button type="primary"
                                    onClick={() => {
                                        if (selectedKeys)
                                            me.setState({ parentTypeId: selectedKeys, Id: null, tabkey: '1' });
                                        else
                                            me.setState({ Id: null, tabkey: '1' });
                                    }}
                                ><PlusOutlined /> 新建</Button>
                                {Id ? <Button
                                    onClick={() => {
                                        dispatch({
                                            type: 'materialtype/delete',
                                            payload: { Id },
                                        }).then((result) => {
                                            me.setState({ Id: null });
                                            dispatch({
                                                type: 'materialtype/getAllMaterialType'
                                            });
                                        })
                                    }}
                                ><DeleteOutlined /> 删除</Button>
                                    : null}
                            </Space>
                            <div style={{ height: 10 }}></div>
                            <fieldset className="x-fieldset">
                                <legend style={{ width: 'auto', fontSize: 14, border: 0, paddingLeft: 10, paddingRight: 10, color: '#333' }}>物料类型树</legend>
                                {/* <Cascader style={{ width: '100%' }} options={options} onChange={this.onChange} placeholder="Please select" /> */}
                                {/* <TreeSelect
                            style={{ width: '100%' }}
                            value={this.state.value}
                            dropdownStyle={{ maxHeight: 400, overflow: 'auto' }}
                            treeData={treeData}
                            placeholder="Please select"
                            treeDefaultExpandAll
                            onChange={this.onChange}
                        /> */}
                                <Tree
                                    showLine
                                    switcherIcon={<DownOutlined />}
                                    defaultExpandAll={true}
                                    onSelect={this.onSelect}
                                    treeData={treeData}
                                />
                            </fieldset>
                        </> : null}
                    </Col>
                    <Col span={18} >
                        <Tabs activeKey={tabkey} onTabClick={this.onTabClick}>
                            <TabPane
                                tab={<span>基本资料</span>}
                                key="1"
                            >
                                <FormPage Id={Id} parentTypeId={parentTypeId} />
                            </TabPane>
                            <TabPane
                                tab={<span>数据浏览</span>}
                                key="2"
                            >
                                <SmProTable
                                    columns={moduleInfo.columns}
                                    delete={Delete}
                                    batchDelete={(selectedRows) => BatchDelete(selectedRows)}
                                    moduleInfo={moduleInfo}
                                    {...action}
                                    changePage={Index.changePage}
                                    formPage={FormPage}
                                    formRef={this.formRef}
                                    form={{ labelCol: { span: 6 } }}
                                    onReset={() => {
                                        dispatch({
                                            type: 'materialtype/setTableStatus',
                                            payload: {},
                                        })
                                    }}
                                    onLoad={() => {
                                        if (tableParam && tableParam.params && this.formRef.current)
                                            this.formRef.current.setFieldsValue({ ...tableParam.params })
                                    }}
                                    pagination={tableParam && tableParam.params ? { current: tableParam.params.current } : {}}
                                    request={(params, sorter, filter) => {
                                        if (tableParam && tableParam.params && !params._timestamp)
                                            params = Object.assign(tableParam.params, params);
                                        if (tableParam && tableParam.sorter)
                                            sorter = Object.assign(tableParam.sorter, sorter);
                                        dispatch({
                                            type: 'materialtype/setTableStatus',
                                            payload: { params, sorter },
                                        })
                                        return query({
                                            paramData: JSON.stringify(params),
                                            sorter: JSON.stringify(sorter),
                                            filter: JSON.stringify(filter),
                                            moduleCode
                                        })
                                    }}
                                />
                            </TabPane>
                        </Tabs>

                    </Col>
                </Row>

            </div>
        )
    }
}
export default connect(({ materialtype }) => ({
    materialtype
}))(TableList);