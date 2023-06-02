import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, InputNumber, Tabs, Switch, TreeSelect, Skeleton, Spin, message } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComBoBox from '@/components/SysComponents/ComboBox';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import Attachment from '../../../attachment/index';
import UploadImage from '../../../upload/image/index';

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
            moduleCode: "BD_MATERIAL_MNG",
            MaterialTypeId: undefined,
            tabkey: 1,
            isLoading: true
        };
    }
    componentWillMount() {
        const { dispatch, Id, IsView } = this.props;
        if (dispatch && Id) {
            this.setState({ Id, disabled: IsView ? true : false })
            dispatch({
                type: 'material/getById',
                payload: { Id },
            }).then((result) => {
                if (result.response && me.formRef.current) {
                    me.setState({ isLoading: false })

                    me.formRef.current.setFieldsValue(result.response);
                    this.setState({
                        ImageUrl: result.response.ImageUrl ? result.response.ImageUrl : null,
                        MaterialTypeId: result.response.MaterialTypeId ? result.response.MaterialTypeId : null,
                        isLoading: false
                    })
                } else me.setState({ isLoading: false })
            })
        } else me.setState({ isLoading: false })
        dispatch({
            type: 'materialtype/getAllMaterialType'
        });

    }
    // onFinish(values) {
    //     const { dispatch } = this.props;
    //     const { Id } = this.state;
    //     if (Id)
    //         values.ID = Id;
    //     dispatch({
    //         type: 'material/saveData',
    //         payload: values,
    //     }).then(() => {
    //         const { material: { Id } } = me.props;
    //         if (Id)
    //             this.setState({ Id })
    //     });
    // }
    onFinish(data, type = 'Save') {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id)
            data.ID = Id;

        dispatch({
            type: 'material/saveData',
            payload: data
        }).then((result) => {
            let response = result.response;
            if (response.status === "ok") {
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
    onTabClick(key) {
        me.setState({ tabkey: key });
    }
    render() {
        const { IsView, materialtype: { treeData }, dispatch, moduleInfo } = this.props;
        const { Id, tabkey, isLoading, ImageUrl } = this.state;
        return (
            <Form
                labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
            >
                <FormToolbar value={{ me, dispatch }}
                    moduleInfo={moduleInfo}
                />
                {isLoading ? <Card>
                    <Skeleton active />
                    <Skeleton active />
                    <div style={{ textAlign: 'center', padding: 20 }}>
                        <Spin size="large" />
                    </div>
                </Card> : <> <Card>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="MaterialNo" label="物料编号" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="MaterialNames" label="物料名称" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="Specifications" label="规格">
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={8}>
                            <FormItem name="TextureId" label={<a href="/basedata/texture" target="_blank">材质</a>}>
                                <ComboGrid
                                    api="/api/Texture/GetPageList"
                                    itemkey="ID"
                                    itemvalue="TextureNames"
                                    // onChange={(value) => {
                                    //     this.setState({ StockKeeperId: value })
                                    // }}
                                    disabled={IsView}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="MaterialTypeId" label={<a href="/basedata/materialtype" target="_blank">物料类型</a>} rules={[{ required: true }]}>
                                {/* <ComboGrid
                                    api="/api/MaterialType/GetPageList"
                                    itemkey="ID"
                                    itemvalue="MaterialTypeNames"
                                    // onChange={(value) => {
                                    //     this.setState({ StockKeeperId: value })
                                    // }}
                                    disabled={IsView}
                                /> */}
                                <TreeSelect
                                    // value={this.state.MaterialTypeId}
                                    dropdownStyle={{ maxHeight: 400, overflow: 'auto' }}
                                    treeData={treeData}
                                    placeholder="请选择物料类型"
                                    treeDefaultExpandAll
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="InventoryValuation" label="存货计价">
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24}>
                        <Col span={8}>
                            <FormItem name="UnitId" label={<a href="/basedata/unit" target="_blank">单位</a>} rules={[{ required: true }]}>
                                <ComboGrid
                                    api="/api/Unit/GetPageList"
                                    itemkey="ID"
                                    itemvalue="UnitNames"
                                    // onChange={(value) => {
                                    //     this.setState({ StockKeeperId: value })
                                    // }}
                                    disabled={IsView}
                                />
                            </FormItem>
                        </Col>
                        <Col span={8}>
                            <FormItem name="Description" label="描述">
                                <Input placeholder="请输入" disabled={IsView} />
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
                    </Space>

                </Card>

                    <div style={{ height: 10 }}></div>

                    <Card>
                        <Tabs onTabClick={this.onTabClick}>

                            <TabPane
                                tab={<span>基本信息</span>}
                                key="1"
                            >
                                <Row gutter={24} justify={"center"}>
                                    <Col span={8}>
                                        <FormItem name="Length" label="长">
                                            <InputNumber placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="Width" label="宽">
                                            <InputNumber placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="Height" label="高">
                                            <InputNumber placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                </Row>

                                <Row gutter={24} justify={"center"}>

                                    <Col span={8}>
                                        <FormItem name="ColorId" label={<a href="/basedata/color" target="_blank">颜色</a>}>
                                            <ComboGrid
                                                api="/api/Color/GetPageList"
                                                itemkey="ID"
                                                itemvalue="ColorNames"
                                                // onChange={(value) => {
                                                //     this.setState({ StockKeeperId: value })
                                                // }}
                                                disabled={IsView}
                                            />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="DrawingNo" label="图号">
                                            <Input placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                    </Col>
                                </Row>
                            </TabPane>
                            <TabPane
                                tab={<span>管控设置</span>}
                                key="2"
                            >
                                <Row gutter={24} justify={"center"}>
                                    <Col span={8}>
                                        <FormItem name="MinOrder" label="最小起订量">
                                            <InputNumber placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="SafetStock" label="安全库存量">
                                            <InputNumber placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="MinPurchase" label="最小采购量">
                                            <InputNumber placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                </Row>
                                <Row gutter={24} justify={"center"}>
                                    <Col span={8}>
                                        <FormItem name="ExpirationDate" label="保质期">
                                            <InputNumber placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem valuePropName='checked' name="IsBatchControl" label="批号管控">
                                            <Switch disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="CheckMethod" label="检验方式">
                                            <ComBoBox disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                </Row>
                                <Row gutter={24} justify={"center"}>

                                    <Col span={8}>
                                        <FormItem name="PurchasePrice" label="采购价">
                                            <InputNumber placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="MinSalesPrice" label="最低销售价">
                                            <InputNumber placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>
                                        <FormItem name="ProductionPurchasePreDays" label="生产采购前置天数">
                                            <InputNumber placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>

                                </Row>
                                <Row gutter={24} justify={"center"}>

                                    <Col span={8}>
                                        <FormItem name="ProductionPurchasePeriod" label="生产采购周期">
                                            <InputNumber placeholder="请输入" disabled={IsView} />
                                        </FormItem>
                                    </Col>
                                    <Col span={8}>

                                    </Col>
                                    <Col span={8}>

                                    </Col>
                                </Row>
                            </TabPane>
                            {Id ? <TabPane
                                tab={<span>产品图片</span>}
                                key="3"
                            >
                                <UploadImage Id={Id} isUnique={true} filePath='product' masterTable='BdMaterial' masterColumn='ImageUrl' ImageUrl={ImageUrl} />
                            </TabPane> : null}
                            {Id ? <TabPane
                                tab={<span>附件</span>}
                                key="4"
                            >
                                <Attachment Id={Id} />
                            </TabPane> : null}
                        </Tabs>
                        {(tabkey == 1 || tabkey == 2) ? <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                            {!IsView ? <Button type="primary" onClick={() => this.onFinishAdd()}>保存并新建</Button> : ''}
                            <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                        </Space> : null}
                    </Card></>}
            </Form>
        )
    }
}
export default connect(({ material, materialtype }) => ({
    material, materialtype
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);