import React, { Component } from 'react';
import { Button, Input, Card, Form, Row, Col, Space, InputNumber, Tabs, Spin, Skeleton, DatePicker } from 'antd';
import { connect } from 'umi';
import { history } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import FormStatus from '@/components/SysComponents/FormStatus';
import ComBoBox from '@/components/SysComponents/ComboBox';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import DetailTable from '../../salesorderdetail/components/TableList';
import Prepayment from '../../salesorderprepayment/components/TableList';
import dayjs from 'dayjs';
import Utility from '@/utils/utility';

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
      moduleCode: "SD_SALES_ORDER_MNG",
      tabkey: 1,
      disabled: false,
      isLoading: true,
      TaxType: '',
      modelName: 'salesorder'
    };
  }
  componentWillMount() {
    const { IsView, dispatch, Id } = this.props;
    if (dispatch && Id) {
      me.setState({ Id, disabled: IsView ? true : false })
      dispatch({
        type: 'salesorder/getById',
        payload: { Id },
      }).then((result) => {
        me.setState({ isLoading: false })
        if (result.response && me.formRef.current) {
          Utility.setFormFormat(result.response);
          me.formRef.current.setFieldsValue(result.response);
          me.setState({ AuditStatus: result.response.AuditStatus, TaxType: result.response.TaxType })
        }
      })
    } else me.setState({ isLoading: false })
  }
  componentDidMount() {
    const { Id } = this.props;
    debugger
    if (!Id)
      me.formRef.current.setFieldsValue({
        OrderCategory: 'Official',
        OrderDate: dayjs(),
        DeliveryrDate: dayjs()
      });

  }
  onFinish(values) {
    const { dispatch } = this.props;
    const { Id } = this.state;
    if (Id)
      values.ID = Id;
    dispatch({
      type: 'salesorder/saveData',
      payload: values,
    }).then(() => {
      const { salesorder: { Id } } = me.props;
      if (Id)
        this.setState({ Id, AuditStatus: 'Add' })
      DetailTable.reloadData()
    });
  }
  render() {
    let { IsView, dispatch, moduleInfo } = this.props;
    const { Id, AuditStatus, isLoading, TaxType } = this.state;
    if (AuditStatus == 'CompleteAudit')
      IsView = true;
    return (
      <Form
        labelCol={{ span: 6, xl: 6, md: 12, sm: 12 }}
        wrapperCol={{ span: 16 }}
        onFinish={(values) => { this.onFinish(values) }}
        ref={this.formRef}
      >
        <FormToolbar value={{ me, dispatch }}
          moduleInfo={moduleInfo}
          onAuditSubmit={(status) => {
            if (status == 'Add') {

            } else if (status == 'CompleteAudit') {

            }
          }} />
        {isLoading ? <Card>
          <Skeleton active />
          <Skeleton active />
          <div style={{ textAlign: 'center', padding: 20 }}>
            <Spin size="large" />
          </div>
        </Card> : <div>
          <Card>
            <FormStatus AuditStatus={AuditStatus} />
            <Row gutter={24} justify={"center"}>
              <Col span={8}>
                <FormItem name="OrderNo" label="销售单号">
                  <Input placeholder="请输入" disabled='trues' defaultValue="自动" />
                </FormItem>
              </Col>
              <Col span={8}>
                <FormItem name="OrderDate" label="订单日期" rules={[{ required: true }]}>
                  <DatePicker
                    disabled={IsView}
                  />
                </FormItem>
              </Col>
              <Col span={8}>
                <FormItem name="DeliveryrDate" label="交货日期" rules={[{ required: true }]}>
                  <DatePicker
                    disabled={IsView}
                  />
                </FormItem>
              </Col>
            </Row>
            <Row gutter={24} justify={"center"}>
              <Col span={8}>
                <FormItem name="OrderCategory" label="订单类别" rules={[{ required: true }]}>
                  <ComBoBox disabled={IsView} defaultValue="Official" />
                </FormItem>
              </Col>
              <Col span={8}>
                <FormItem name="CustomerId" label={<a onClick={(value) => {
                  history.push(`/basedata/customer`)
                }}>客户</a>} rules={[{ required: true }]}>
                  <ComboGrid
                    api="/api/Customer/GetPageList"
                    itemkey="ID"
                    itemvalue="CustomerName"
                    disabled={IsView}
                    param={{
                      IsActive: true
                    }}
                    onChange={(value) => {
                      dispatch({
                        type: 'customer/getById',
                        payload: { Id: value },
                      }).then((result) => {
                        if (result.response && me.formRef.current) {
                          let data = result.response;
                          me.formRef.current.setFieldsValue({
                            TaxType: data.TaxType,
                            TaxRate: data.TaxRate,
                            CurrencyId: data.CurrencyId,
                            SettlementWayId: data.SettlementWayId
                          })

                        }
                      })
                      dispatch({
                        type: 'customerdeliveryaddress/getDefaultData',
                        payload: { masterId: value },
                      }).then((result) => {
                        if (result.response && me.formRef.current) {
                          let data = result.response.data;
                          if (data)
                            me.formRef.current.setFieldsValue({
                              Contact: data.Contact,
                              Phone: data.Phone,
                              Address: data.Address
                            })

                        }
                      })
                      // this.setState({ StockKeeperId: value })
                    }}
                  />
                </FormItem>
              </Col>
              <Col span={8}>
                <FormItem name="CustomerOrderNo" label="客户单号">
                  <Input placeholder="请输入" disabled={IsView} />
                </FormItem>
              </Col>
            </Row>
            <Row gutter={24} justify={"center"}>
              <Col span={8}>
                <FormItem name="SalesmanId" label="业务员">
                  <ComboGrid
                    api="/api/SmEmployee/GetPageList"
                    itemkey="ID"
                    itemvalue="EmployeeName"
                    disabled={IsView}
                    param={{
                      IsActive: true
                    }}
                  />
                </FormItem>
              </Col>
              <Col span={8}>
                <FormItem name="TaxType" label="税别" rules={[{ required: true }]}>
                  <ComBoBox disabled={IsView}
                    onChange={(value) => {
                      if (value == 'ZeroTax')
                        me.formRef.current.setFieldsValue({
                          TaxRate: 0
                        });

                      this.setState({ TaxType: value })
                    }}
                  />
                </FormItem>
              </Col>
              <Col span={8}>
                <FormItem name="TaxRate" label="税率" rules={[{ required: true }, { type: 'number', min: 0, message: '税率最小值为0 !' }]}>
                  <InputNumber placeholder="请输入" disabled={TaxType == 'ZeroTax' ? true : IsView} />
                </FormItem>
              </Col>
            </Row>
            <Row gutter={24} justify={"center"}>
              <Col span={8}>
                <FormItem name="SettlementWayId" label={<a onClick={(value) => {
                  history.push(`/basedata/settlementway`)
                }}>结算方式</a>}>
                  <ComboGrid
                    api="/api/SettlementWay/GetPageList"
                    itemkey="ID"
                    itemvalue="SettlementName"
                    // onChange={(value) => {
                    //     this.setState({ StockKeeperId: value })
                    // }}
                    disabled={IsView}
                  />
                </FormItem>
              </Col>
              <Col span={8}>
                <FormItem name="CurrencyId" label={<a onClick={(value) => {
                  history.push(`/basedata/currency`)
                }}>币别</a>} rules={[{ required: true }]}>
                  <ComboGrid
                    api="/api/Currency/GetPageList"
                    itemkey="ID"
                    itemvalue="CurrencyName"
                    // onChange={(value) => {
                    //     this.setState({ StockKeeperId: value })
                    // }}
                    disabled={IsView}
                  />
                </FormItem>
              </Col>
              <Col span={8}>
                <FormItem name="Contact" label="收货人">
                  <Input placeholder="请输入" disabled={IsView} />
                </FormItem>
              </Col>
              {/* <Col span={8}>
                            <FormItem name="TaxIncludedAmount" label="含税金额" rules={[{ required: true }]}>
                                <InputNumber placeholder="请输入" disabled={IsView} style={{ width: '100%' }} />
                            </FormItem>
                        </Col> */}

            </Row>
            <Row gutter={24} justify={"center"}>
              <Col span={8}>
                <FormItem name="Phone" label="收货电话">
                  <Input placeholder="请输入" disabled={IsView} />
                </FormItem>
              </Col>
              <Col span={8}>
                <FormItem name="Address" label="收货地址">
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
              <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
            </Space>

          </Card>

          <div style={{ height: 10 }}></div>
          {Id ? <Card>
            <Tabs>
              <TabPane
                tab='基本信息'
                key='1'
              >
                <DetailTable Id={Id} IsView={IsView} />
              </TabPane>
              <TabPane
                tab='预付账款'
                key='2'
              >
                <Prepayment Id={Id} IsView={IsView} />
              </TabPane>
            </Tabs>
          </Card> : null}
        </div>
        }
      </Form>
    )
  }
}
export default connect(({ salesorder, employee }) => ({
  salesorder, employee
  // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);
