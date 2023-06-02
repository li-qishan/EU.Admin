import React, { Component } from 'react';
import { Button, message, Tabs, Input, Card, Form, Row, Col, Space, Modal, Skeleton } from 'antd';
import { connect } from 'umi';
import { SaveOutlined, RollbackOutlined, InfoCircleOutlined } from '@ant-design/icons';
import TableList from './TableList';
import Index from '../index';
import DetailTable from './DetailTable';
import request from "@/utils/request";

const { TextArea } = Input;
const FormItem = Form.Item;
const { TabPane } = Tabs;
let me;
class SqlEdit extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleSqlId: '',
            tabkey: '1',
            iShowFullSql: false,
            fullSql: null
        };
    }
    componentDidMount() {
        const { dispatch, Id } = this.props;
        if (Id) {
            message.loading({ content: '数据加载...' });

            dispatch({
                type: 'module/GetModuleSqlInfo',
                payload: { moduleId: Id },
            }).then((result) => {
                message.destroy();
                if (result.Success && me.formRef.current) {
                    let data = result.Data;
                    if (data.module) {
                        if (!data.moduleSql) {
                            data.moduleSql = {
                                TableAliasNames: "A",
                                SqlDefaultCondition: "A.IsActive = 'true' AND A.IsDeleted = 'false'",
                                SqlRecycleCondition: "A.IsActive = 'true' AND A.IsDeleted = 'true'",
                                SqlSelect: "SELECT A.*,A.ID AS DELETE_CONFIRM_MSG"
                            };
                        }
                        me.setState({ Id, moduleSqlId: data.moduleSql.ID });
                        Object.assign(data.moduleSql, data.module);

                        me.formRef.current.setFieldsValue(data.moduleSql)
                    }
                }
            })
        }
        // else {
        //     this.formRef.current.setFieldsValue({
        //         IsShowAdd: true,
        //         IsShowUpdate: true,
        //         IsShowView: true,
        //         IsShowDelete: true,
        //         IsShowBatchDelete: true,
        //     })
        // }
    }
    onFinish(values) {
        const { dispatch } = me.props;
        const { moduleSqlId, Id } = me.state;
        if (moduleSqlId) {
            values.ID = moduleSqlId;
        }
        values.ModuleId = Id;
        dispatch({
            type: 'module/saveModuleSqlData',
            payload: values,
        }).then(() => {
            const { module: { moduleSqlId } } = me.props;
            this.setState({ moduleSqlId });
        });
    }
    onTabClick(key) {
        me.setState({ tabkey: key });
    }
    showLogRecordCancel = e => {
        let me = this;
        me.setState({
            iShowFullSql: false
        });
    };
    async getFullSql() {
        let { Id } = this.state;

        let result = await request('/api/SmModuleSql/GetModuleFullSql/' + Id, {
            method: 'POST'
        });
        if (result && result.Success) me.setState({ iShowFullSql: true, fullSql: result.Data });

    }
    render() {
        const { IsView } = this.props;
        const { iShowFullSql, fullSql } = this.state;
        let legendCss = { width: 'auto', fontSize: 14, border: 0, paddingLeft: 10, paddingRight: 10, color: '#333' };
        return (<>
            {iShowFullSql ? <Modal
                title="完整SQL"
                open={iShowFullSql}
                width={800}
                footer={null}
                onCancel={this.showLogRecordCancel}

            >
                {iShowFullSql ? <TextArea rows={4} value={fullSql} disabled={true} />
                    : <Skeleton active />}
            </Modal> : null}
            <Form
                labelCol={{ span: 4 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
            >
                <Space style={{ display: 'flex', justifyContent: 'flex-end' }}>
                    {/* <Button type="default" onClick={() => Index.changePage(<TableList />)}>读取XML</Button>
                <Button type="default" onClick={() => Index.changePage(<TableList />)}>生成当前XML</Button>
                <Button type="default" onClick={() => Index.changePage(<TableList />)}>生成全部XML</Button> */}
                    <Button type="default" onClick={() => this.getFullSql()}><InfoCircleOutlined />查看完整SQL</Button>
                    <Button type="default" onClick={() => Index.changePage(<TableList />)}><RollbackOutlined /></Button>
                </Space>
                <div style={{ height: 10 }}></div>
                <Card>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="ModuleCode" label="模块代码" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={true} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="ModuleName" label="模块名称" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={true} />
                            </FormItem>
                        </Col>
                    </Row>

                </Card>
                <div style={{ height: 10 }}></div>
                <Card>
                    <Tabs onTabClick={this.onTabClick}>
                        <TabPane
                            tab={<span>模块SQL</span>}
                            key="1"
                        >
                            <fieldset className="x-fieldset">
                                <legend style={legendCss}>表信息</legend>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={12}>
                                        <FormItem name="PrimaryTableName" label="主表名" rules={[{ required: true }]}>
                                            <Input placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={12}>
                                        <FormItem name="TableAliasNames" label="全部表别名" rules={[{ required: true }]}>
                                            <Input placeholder="请输入" disabled={IsView} rules={[{ required: true }]} />
                                        </FormItem>
                                    </Col>
                                </Row>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={12}>
                                        <FormItem name="TableNames" label="全部表名" rules={[{ required: true }]}>
                                            <Input placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={12}>
                                        <FormItem name="PrimaryKey" label="主键">
                                            <Input placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                </Row>
                            </fieldset>
                            <fieldset className="x-fieldset">
                                <legend style={legendCss}>SQL信息</legend>
                                <Row gutter={24}>
                                    <Col span={24}>
                                        <FormItem name="SqlSelect" label="Select语句" rules={[{ required: true }]}>
                                            <TextArea placeholder="请输入" autoSize={{ minRows: 6 }} disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                </Row>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={24}>
                                        <FormItem name="SqlSelectBrw" label="首页Select语句">
                                            <TextArea placeholder="请输入" autoSize={{ minRows: 6 }} disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                </Row>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={12}>
                                        <FormItem name="JoinType" label="关联类型">
                                            <Input placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={12}>
                                        {/* <FormItem name="ParentId" label="导出Excel隐藏列">
                                        <ComboGrid api="/api/SmModule/GetPageList" itemkey="ID" itemvalue="ModuleName" />
                                    </FormItem> */}
                                    </Col>
                                </Row>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={12}>
                                        <FormItem name="SqlJoinTable" label="关联表">
                                            <Input placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={12}>
                                        {/* <FormItem name="ParentId" label="默认查询列索引">
                                        <ComboGrid api="/api/SmModule/GetPageList" itemkey="ID" itemvalue="ModuleName" />
                                    </FormItem> */}
                                    </Col>
                                </Row>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={12}>
                                        <FormItem name="SqlJoinTableAlias" label="关联表别名">
                                            <Input placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={12}>

                                    </Col>
                                </Row>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={24}>
                                        <FormItem name="SqlJoinCondition" label="关联条件">
                                            <TextArea placeholder="请输入" autoSize={{ minRows: 6 }} disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                </Row>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={24}>
                                        <FormItem name="SqlDefaultCondition" label="默认条件*" rules={[{ required: true }]}>
                                            <TextArea placeholder="请输入" autoSize={{ minRows: 6 }} disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                </Row>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={24}>
                                        <FormItem name="SqlRecycleCondition" label="回收站条件" rules={[{ required: true }]}>
                                            <TextArea placeholder="请输入" autoSize={{ minRows: 6 }} disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                </Row>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={24}>
                                        <FormItem name="SqlQueryCondition" label="初始查询条件">
                                            <Input placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                </Row>
                                {/* <Row gutter={24} justify={"center"}>
                                <Col span={24}>
                                    <FormItem name="RoutePath" label="合并表头">
                                        <Input placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                            </Row> */}
                            </fieldset>
                            <fieldset className="x-fieldset">
                                <legend style={legendCss}>排序信息</legend>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={12}>
                                        <FormItem name="DefaultSortField" labelCol={{ span: 6 }} label="主表默认排序列名" rules={[{ required: true }]}>
                                            <Input placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={12}>
                                        <FormItem name="DefaultSortDirection" labelCol={{ span: 6 }} label="主表默认排序方向" rules={[{ required: true }]}>
                                            <Input placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                </Row>
                                {/* <Row gutter={24} justify={"center"}>
                                <Col span={12}>
                                    <FormItem name="RoutePath" label="从表默认排序列名">
                                        <Input placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={12}>
                                    <FormItem name="ParentId" label="从表默认排序方向">
                                        <ComboGrid api="/api/SmModule/GetPageList" itemkey="ID" itemvalue="ModuleName" />
                                    </FormItem>
                                </Col>
                            </Row> */}
                            </fieldset>
                            <fieldset className="x-fieldset">
                                <legend style={legendCss}>描述信息</legend>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={24}>
                                        <FormItem name="Description" label="描述">
                                            <TextArea placeholder="请输入" autoSize={{ minRows: 6 }} disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                </Row>
                            </fieldset>
                        </TabPane>
                        <TabPane
                            tab={<span>完整SQL</span>}
                            key="2"
                        >
                            {/* <Space style={{ display: 'flex', flexDirection: 'row', alignItems: 'flex-start', justifyContent: 'space-between' }}> */}
                            <Row gutter={24} justify={"center"}>
                                <Col span={24}>
                                    <FormItem name="FullSql" labelCol={{ span: 0 }} wrapperCol={{ span: 24 }}>
                                        <TextArea placeholder="请输入" autoSize={{ minRows: 14 }} disabled={IsView} />
                                    </FormItem>
                                </Col>
                            </Row>
                            {/* </Space> */}
                        </TabPane>
                        <TabPane
                            tab={<span>模块列</span>}
                            key="3"
                        >
                            {this.state.Id ? <DetailTable Id={this.state.Id} /> : null}
                        </TabPane>
                    </Tabs>
                    {this.state.tabkey != '3' ? <Space style={{ display: 'flex', justifyContent: 'center' }}>
                        {!IsView ? <Button type="primary" htmlType="submit"><SaveOutlined />保存</Button> : ''}
                        <Button type="default" onClick={() => Index.changePage(<TableList />)}><RollbackOutlined />返回</Button>
                    </Space> : ''}
                </Card>
            </Form>
        </>);
    }
}
export default connect(({ module }) => ({
    module,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(SqlEdit);