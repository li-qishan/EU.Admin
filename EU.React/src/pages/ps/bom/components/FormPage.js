import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, Spin, Tree, Skeleton, InputNumber, Tabs, message, Drawer, Switch } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import Utility from '@/utils/utility';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import Material from '../../material/components/TableList';
import Mould from '../../mould/components/TableList';
import Process from '../../bomprocess/components/TableList';
import request from "@/utils/request";
import { DownOutlined } from '@ant-design/icons';

const { TabPane } = Tabs;
const FormItem = Form.Item;
let me;

class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleCode: "PS_PROCESS_BOM_MNG",
            // tabkey: 1,
            isLoading: true,
            disabled: false,
            modelName: 'psbom',
            drawerVisible: false,
            treeData: [],
            title: ''
        };
    }
    async componentWillMount() {
        let { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'psbom/getById',
                payload: { Id },
            }).then((result) => {
                this.setState({ isLoading: false, disabled: IsView ? true : false });
                let data = result.response;
                if (data && me.formRef.current) {
                    Utility.setFormFormat(data);
                    dispatch({
                        type: 'psbom/setDetail',
                        payload: data
                    });
                    me.formRef.current.setFieldsValue(data);
                    if (data.AuditStatus == 'CompleteAudit' || data.AuditStatus == 'CompleteAdjust')
                        IsView = true;
                    me.setState({ AuditStatus: data.AuditStatus, IsView })
                }
            })
        } else me.setState({ isLoading: false });
        let result = await request('/api/BOM/GetBOMTree', {
            params: {
                bomId: Id
            }
        });
        me.setState({ treeData: result.data, title: result.title });
    }
    // componentDidMount() {
    //     const { Id } = this.props;
    //     if (!Id)
    //         me.formRef.current.setFieldsValue({
    //             OrderDate: dayjs()
    //         });
    // }
    onFinish(values, type = 'Save') {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            values.ID = Id;
        dispatch({
            type: 'psbom/saveData',
            payload: values,
        }).then((result) => {
            let response = result.response;
            if (response.status === "ok") {
                // DetailTable.reloadData()
                if (type == 'SaveAdd') {
                    me.setState({
                        Id: ''
                    });
                    me.formRef.current.resetFields();
                }
                else if (!Id)
                    this.setState({ Id: response.Id });
                message.success(response.message);
            }
            else
                message.error(response.message);
        });
    }
    onFinishAdd() {
        me.formRef.current.validateFields()
            .then(values => {
                me.onFinish(values, 'SaveAdd')
            });
    }
    async onDrawerOpen() {
        this.setState({
            drawerVisible: true
        });
        const { Id } = this.state;
        let result = await request('/api/BOM/GetBOMTree', {
            params: {
                bomId: Id
            }
        });
        me.setState({ treeData: result.data, title: result.title });
    }
    onDrawerClose() {
        this.setState({
            drawerVisible: false,
        });
    }
    onTreeSelect = (value, info) => {
        if (info.node.children) {
            message.loading('数据刷新中...', 0);
            let Id = info.node.BOMId;
            let { dispatch } = this.props;
            if (dispatch && Id) {
                this.setState({
                    Id
                }, () => {
                    dispatch({
                        type: 'psbom/getById',
                        payload: { Id },
                    }).then((result) => {
                        this.setState({ isLoading: false });
                        Material.reloadData(Id);
                        Mould.reloadData(Id);
                        Process.reloadData(Id);
                        message.destroy();
                        let data = result.response;
                        if (data && me.formRef.current) {
                            Utility.setFormFormat(data);
                            dispatch({
                                type: 'psbom/setDetail',
                                payload: data
                            });
                            me.formRef.current.setFieldsValue(data);
                            if (data.AuditStatus == 'CompleteAudit' || data.AuditStatus == 'CompleteAdjust')
                                IsView = true;
                            me.setState({ AuditStatus: data.AuditStatus })
                        }
                    })
                })

            }
        }
    };
    render() {
        let { dispatch, moduleInfo } = this.props;
        let { IsView, AuditStatus, isLoading, Id, drawerVisible, treeData, title } = this.state;
        if (AuditStatus == 'CompleteAudit' || AuditStatus == 'CompleteAdjust')
            IsView = true;
        return (
            (<Form
                labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
            >
                {isLoading ? <Card>
                    <Skeleton active />
                    <Skeleton active />
                    <div style={{ textAlign: 'center', padding: 20 }}>
                        <Spin size="large" />
                    </div>
                </Card> : <>
                    <FormToolbar value={{ me, dispatch }}
                        moduleInfo={moduleInfo}
                    />
                    <Card>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="MaterialId" label="物料" rules={[{ required: true }]}>
                                    <ComboGrid
                                        api="/api/Material/GetPageList"
                                        itemkey="ID"
                                        itemvalue={['MaterialNo', 'MaterialNames']}
                                        disabled={IsView}
                                    />
                                </FormItem>

                            </Col>
                            <Col span={8}>
                                <FormItem name="WorkShopId" label="车间" rules={[{ required: true }]}>
                                    <ComboGrid
                                        api="/api/WorkShop/GetPageList"
                                        itemkey="ID"
                                        itemvalue={['WorkShopNo', 'WorkShopName']}
                                        disabled={IsView}
                                    />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Version" label="版本" rules={[{ required: true }]}>
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Row gutter={24} justify={"center"}>
                            <Col span={8}>
                                <FormItem name="BulkQty" label="批量" rules={[{ required: true }]}>
                                    <InputNumber placeholder="请输入" disabled={IsView} min="0" />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem valuePropName='checked' name="IsActive" label="是否启用">
                                    <Switch disabled={IsView} />
                                </FormItem>
                            </Col>
                            <Col span={8}>
                                <FormItem name="Remark" label="备注">
                                    <Input placeholder="请输入" disabled={IsView} />
                                </FormItem>
                            </Col>
                        </Row>
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            {!IsView ? <Button type="primary" onClick={() => this.onFinishAdd()}>保存并新建</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                            <Button type="primary" onClick={() => this.onDrawerOpen()}>查看</Button>
                        </Space>
                    </Card>
                    <div style={{ height: 10 }}></div>
                    {Id ? <Card>
                        <Tabs>
                            <TabPane
                                tab='材料组成'
                                key='1'
                            >
                                <Material Id={Id} outdetailme={me} IsView={IsView} />
                            </TabPane>
                            <TabPane
                                tab='工艺路线'
                                key='2'
                            >
                                <Process Id={Id} outdetailme={me} IsView={IsView} />
                            </TabPane>
                            <TabPane
                                tab='工模治具'
                                key='3'
                            >
                                <Mould Id={Id} outdetailme={me} IsView={IsView} />
                            </TabPane>
                            {/*  <TabPane
                                tab='工序单价'
                                key='3'
                            >
                                <Price Id={Id} outdetailme={me} IsView={IsView} />
                            </TabPane>
                            <TabPane
                                tab='工序人员'
                                key='4'
                            >
                                <Employee Id={Id} outdetailme={me} IsView={IsView} />
                            </TabPane> */}
                        </Tabs>
                    </Card> : null}

                    <Drawer
                        title={title}
                        width={640}
                        placement="right"
                        closable={false}
                        onClose={() => this.onDrawerClose()}
                        open={drawerVisible}
                    >
                        <Tree
                            showLine
                            switcherIcon={<DownOutlined />}
                            // defaultExpandParent={true}
                            showIcon
                            defaultExpandAll
                            onSelect={this.onTreeSelect}
                            treeData={treeData}
                        />
                        {/* <Tree
        showLine
        switcherIcon={<DownOutlined />}
        treeData={[
          {
            title: 'parent 1',
            key: '0-0',
            children: treeData
          },
        ]}
      /> */}
                    </Drawer>
                </>
                }
            </Form>)
        );
    }
}
export default connect(({ psbom }) => ({
    psbom
}))(FormPage);
