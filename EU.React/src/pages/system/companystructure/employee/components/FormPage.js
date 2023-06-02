import React, { Component } from 'react';
import { Button, Tree, Input, Card, Form, Row, Col, DatePicker, Space, InputNumber, Tabs } from 'antd';
import { connect } from 'umi';
import TableList from './TableList';
import Index from '../index';
import FormToolbar from '@/components/SysComponents/FormToolbar';
import ComboGrid from '@/components/SysComponents/ComboGrid';
import ComBoBox from '@/components/SysComponents/ComboBox';
import Utility from '@/utils/utility';

const FormItem = Form.Item;

let me;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',
            moduleCode: "SM_EMPLOYEE_MNG"
        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        if (dispatch && Id) {
            this.setState({ Id })
            dispatch({
                type: 'employee/getById',
                payload: { Id },
            }).then((result) => {
                if (result.response && me.formRef.current)
                    Utility.setFormFormat(result.response);
                me.formRef.current.setFieldsValue(result.response)

            })
        }

    }
    onFinish(values) {
        const { dispatch } = this.props;
        const { Id } = this.state;
        if (Id) {
            values.ID = Id;
        }
        dispatch({
            type: 'employee/saveData',
            payload: values,
        }).then(() => {
            const { employee: { Id } } = me.props;
            if (Id)
                this.setState({ Id })
        });
    }
    render() {
        const { IsView } = this.props;
        const { Id } = this.state;
        return (
            <Form
                labelCol={{ span: 4 }}
                wrapperCol={{ span: 16 }}
                onFinish={(values) => { this.onFinish(values) }}
                ref={this.formRef}
            >
                <FormToolbar value={{ me }} />
                <Card>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="EmployeeCode" label="员工代码" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="EmployeeName" label="员工姓名" rules={[{ required: true }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="EName" label="英文名" >
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="Sex" label="性别">
                                <ComBoBox disabled={IsView} />
                            </FormItem>
                        </Col>

                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="NickName" label="昵称">
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="Phone" label="电话" rules={[{ pattern: /^1(3|4|5|7|8)\d{9}$/, message: '格式不正确' }]}>
                                <Input placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>

                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="HireDate" label="入职日期">
                                <DatePicker
                                    disabled={IsView}
                                />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="TermDate" label="离职日期">
                                <DatePicker
                                    disabled={IsView}
                                />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        {/* <Col span={12}>
                            <FormItem name="CompanyId" label="所属组织">
                                <ComboGrid
                                    api="/api/SmCompany/GetPageList"
                                    itemkey="ID"
                                    itemvalue="CompanyName"
                                    onChange={(value) => {
                                        if (value != this.state.CompanyId) {
                                            me.formRef.current.setFieldsValue({ 'DepartmentId': '' });
                                        }
                                        this.setState({ CompanyId: value })
                                    }}
                                    disabled={IsView}
                                />
                            </FormItem> 
                        </Col>*/}
                        <Col span={12}>
                            <FormItem name="DepartmentId" label="上级部门" >
                                <ComboGrid
                                    api="/api/SmDepartment/GetPageList"
                                    itemkey="ID"
                                    itemvalue="DepartmentName"
                                    disabled={IsView}
                                //  parentId={{ 'CompanyId': this.state.CompanyId }}
                                />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                            <FormItem name="ParentEmployeeId" label="上级" >
                                <ComboGrid
                                    api="/api/SmEmployee/GetParentList"
                                    itemkey="ID"
                                    itemvalue="EmployeeName"
                                    disabled={IsView}
                                    parentId={{ 'ID': Id }}
                                />
                            </FormItem>
                        </Col>
                    </Row>
                    <Row gutter={24} justify={"center"}>
                        <Col span={12}>
                            <FormItem name="MonthsSalesAmount" label="月销售目标">
                                <InputNumber placeholder="请输入" disabled={IsView} />
                            </FormItem>
                        </Col>
                        <Col span={12}>
                        </Col>
                    </Row>
                    <Space style={{ display: 'flex', justifyContent: 'center' }}>
                        {!IsView ? <Button type="primary" htmlType="submit">保存</Button> : ''}
                        <Button type="default" onClick={() => Index.changePage(<TableList />)}>返回</Button>
                    </Space>
                </Card>
            </Form>
        )
    }
}
export default connect(({ employee }) => ({
    employee,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);