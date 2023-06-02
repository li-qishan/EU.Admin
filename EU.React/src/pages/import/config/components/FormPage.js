import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Tabs, Spin, Skeleton, InputNumber, Switch } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import ComBoBox from '@/components/SysComponents/ComboBox';
import DetailTable from '../../configdetail/components/TableList';
import Utility from '@/utils/utility';
import Attachment from '../../../attachment/index';

const FormItem = Form.Item;
const { TabPane } = Tabs;
let me;

class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleCode: "SM_IMPORT_TEMPLATE_MNG",
            // tabkey: 1,
            isLoading: true
        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'importconfig/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false })
                if (result.response && me.formRef.current) {
                    Utility.setFormFormat(result.response);
                    dispatch({
                        type: 'importconfig/setDetail',
                        payload: result.response
                    });
                    me.formRef.current.setFieldsValue(result.response);
                }
            })
        } else me.setState({ isLoading: false })
    }
    componentDidMount() {
        // const { Id, user: { currentUser } } = this.props;
        // if (!Id)
        //     me.formRef.current.setFieldsValue({
        //         OrderDate: dayjs()
        //     });
        // me.formRef.current.setFieldsValue({
        //     'CustomerId1': currentUser.data.name
        // });
    }
    onFinish(values) {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            values.ID = Id;
        // else {
        //     values.UserId = currentUser.data.userid;
        //     values.DepartmentId = DepartmentId;
        // }
        dispatch({
            type: 'importconfig/saveData',
            payload: values,
        }).then(() => {
            const { importconfig: { Id } } = me.props;
            if (Id)
                this.setState({ Id })
            // DetailTable.reloadData()
        });
    }
    render() {
        const { IsView } = this.props;
        const { Id } = this.state;
        return (
            <Form
                labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
            >
                {this.state.isLoading ? <Card>
                    <Skeleton active />
                    <Skeleton active />
                    <div style={{ textAlign: 'center', padding: 20 }}>
                        <Spin size="large" />
                    </div>
                </Card> : <div>
                        <Card>
                            <FormToolbar value={{ me }} />

                            <Row gutter={24} justify={"center"}>
                                <Col span={8}>
                                    <FormItem name="TemplateCode" label="模板代码" rules={[{ required: true }]}>
                                        <Input placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem name="TemplateName" label="模板名称" rules={[{ required: true }]}>
                                        <Input placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem name="TableCode" label="表代码" rules={[{ required: true }]}>
                                        <ComboGrid
                                            api="/api/SmTableCatalog/GetPageList"
                                            itemkey="TableCode"
                                            itemvalue="TableCode"
                                        // onChange={(tableCode, Option, data) => {
                                        //     debugger
                                        //     this.setState({ tableCode })
                                        // }}
                                        />
                                    </FormItem>
                                </Col>
                            </Row>
                            <Row gutter={24} justify={"center"}>
                                <Col span={8}>
                                    <FormItem name="ModuleId" label="模块代码" rules={[{ required: true }]}>
                                        <ComboGrid
                                            api="/api/SmModule/GetPageList?IsParent=false"
                                            itemkey="ID"
                                            itemvalue="ModuleName"
                                            disabled={IsView}
                                            onChange={(ModuleId, Option, data) => {
                                                me.formRef.current.setFieldsValue({
                                                    'ModuleCode': data[0].ModuleCode
                                                });
                                            }}
                                        />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem name="SheetName" label="Sheet名" rules={[{ required: true }]}>
                                        <Input placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem name="StartRow" label="数据起始行" rules={[{ required: true }]}>
                                        <Input placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                            </Row>
                            <Row gutter={24} justify={"center"}>
                                <Col span={8}>
                                    <FormItem name="Label" label="标签名">
                                        <Input placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem name="TaxisNo" label="排序号">
                                        <InputNumber placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem name="TransferMode" label="转换时机" rules={[{ required: true }]}>
                                        <ComBoBox disabled={IsView} />
                                    </FormItem>
                                </Col>
                            </Row>
                            <Row gutter={24} justify={"center"}>
                                <Col span={8}>
                                    <FormItem valuePropName='checked' name="IsDisplayProgress" label="显示进度条">
                                        <Switch disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem valuePropName='checked' name="IsLoadData" label="加载数据">
                                        <Switch disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem valuePropName='checked' name="IsDisplay" label="显示">
                                        <Switch disabled={IsView} />
                                    </FormItem>
                                </Col>
                            </Row>
                            <Row gutter={24} justify={"center"}>
                                <Col span={8}>
                                    <FormItem valuePropName='checked' name="IsAllowOverride" label="是否允许覆盖导入">
                                        <Switch disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem name="ExcludeLastRow" label="排除最后行数">
                                        <InputNumber placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                                <Col span={8}>
                                    <FormItem name="Remark" label="备注">
                                        <Input placeholder="请输入" disabled={IsView} />
                                    </FormItem>
                                </Col>
                            </Row>
                            <Space style={{ display: 'none' }}>
                                <FormItem name="ModuleCode">
                                    <Input />
                                </FormItem>

                            </Space>
                            <Space style={{ display: 'flex', justifyContent: 'center' }}>
                                {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                                <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                            </Space>
                        </Card>

                        <div style={{ height: 10 }}></div>
                        {Id ? <Card>
                            <Tabs>
                                <TabPane
                                    tab='模板明细'
                                    key='1'
                                >
                                    <DetailTable Id={Id} />
                                </TabPane>
                                <TabPane
                                    tab={<span>模板</span>}
                                    key="2"
                                >
                                    <Attachment Id={Id} accept=".xlsx,.xls" filePath='importtemplate' isUnique={true} />
                                </TabPane>
                            </Tabs>
                        </Card> : null}
                    </div>
                }
            </Form>
        )
    }
}
export default connect(({ importconfig }) => ({
    importconfig
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);
