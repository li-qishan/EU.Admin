import React, { Component } from 'react';
import { connect } from 'umi';
import { Select, Row, Col, List, Tabs, Input, Card, Menu, Switch } from 'antd';
import TableList from './TableList'
import {
    CopyOutlined
} from '@ant-design/icons';

const { TabPane } = Tabs;
let me;
class Index extends Component {
    formRef = React.createRef();

    constructor(props) {
        super(props);
        me = this;
        me.state = {
            index: 0
        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        dispatch({
            type: 'paramconfig/queryByGroup',
            // payload: { moduleCode },
        })
    }
    onTabClick(key) {
        me.setState({ tabkey: key });
    }

    onChange(checked, itemIndex, Id) {
        const { paramconfig: { groups } } = this.props;
        const { index } = this.state;
        groups[index].detail[itemIndex].ConfigValue = checked ? "Y" : "N";

        this.onUpdate({ ID: Id, ConfigCode: groups[index].detail[itemIndex].ConfigCode, ConfigValue: checked ? "Y" : "N" }, groups)
    }
    onInputChange(e, itemIndex, Id) {
        const { paramconfig: { groups } } = this.props;
        const { index } = this.state;
        groups[index].detail[itemIndex].ConfigValue = e.target.value;
        this.onUpdate({ ID: Id, ConfigCode: groups[index].detail[itemIndex].ConfigCode, ConfigValue: e.target.value }, groups)
    }
    onSelectChange(value, itemIndex, Id) {
        const { paramconfig: { groups } } = this.props;
        const { index } = this.state;
        groups[index].detail[itemIndex].ConfigValue = value;
        this.onUpdate({ ID: Id, ConfigCode: groups[index].detail[itemIndex].ConfigCode, ConfigValue: value }, groups)
    }
    async onUpdate(values, groups) {
        // let { IsView, Id, AuditStatus, detailCount } = this.state;
        const { dispatch } = this.props;
        dispatch({
            type: 'paramconfig/updateGroup',
            payload: groups
        });
        dispatch({
            type: 'paramconfig/saveData',
            payload: values,
        }).then(() => {
            // const { dispatch, paramconfig: { Id } } = me.props;
            // this.setState({ Id })
        });

    }
    render() {
        const { dispatch, paramconfig: { groups } } = this.props;
        const { tabkey, index } = this.state;


        //#region 操作栏按钮方法
        //#endregion

        return (
            <>
                <Card>
                    <Tabs activeKey={tabkey} onTabClick={this.onTabClick}>
                        <TabPane
                            tab={<span>基本资料</span>}
                            key="1"
                        >
                            <Row>
                                <Col span={4}>
                                    <Menu
                                        selectedKeys={[groups.length > 0 ? groups[index].ID : null]}
                                        onClick={this.onMenuClick}
                                        mode='inline'
                                        theme='light'
                                    >
                                        {
                                            groups.map((item, index) => {
                                                return (
                                                    <Menu.Item key={item.ID}>
                                                        {item.Name}
                                                    </Menu.Item>
                                                )
                                            })
                                        }

                                    </Menu>
                                </Col>
                                <Col span={20} >
                                    {groups.length > 0 ? <List
                                        itemLayout="horizontal"
                                        dataSource={groups[index].detail}
                                        renderItem={(item, itemIndex) => (
                                            <List.Item>
                                                {/* https://www.csdn.net/tags/MtzacgwsMjgyNy1ibG9n.html */}
                                                <List.Item.Meta
                                                    avatar={<></>}
                                                    title={<a>{item.ConfigName}<CopyOutlined /></a>}
                                                    description={item.Remark}
                                                />
                                                <>
                                                    {item.InputType == 'SWITCH' ? <Switch onChange={(checked) => this.onChange(checked, itemIndex, item.ID)} checked={item.ConfigValue == "Y" ? true : false} /> : null}
                                                    {item.InputType == 'INPUT' ? <Input placeholder="请输入" style={{ width: 200 }} value={item.ConfigValue} onChange={(e) => this.onInputChange(e, itemIndex, item.ID)} /> : null}
                                                    {item.InputType == 'SELECT' ?
                                                        <Select value={item.ConfigValue} onChange={(e) => this.onSelectChange(e, itemIndex, item.ID)} style={{ width: 200 }}
                                                        // onChange={handleChange}
                                                        >
                                                            {item.AvailableValue.split(';').map((availableValue, index) => {
                                                                return (
                                                                    <Option value={availableValue.split(':')[1]}>{availableValue.split(':')[0]}</Option>
                                                                )
                                                            })}
                                                        </Select> : null}
                                                </>
                                            </List.Item>
                                        )}
                                    /> : null
                                    }
                                </Col>
                            </Row>
                        </TabPane>
                        <TabPane
                            tab={<span>数据浏览</span>}
                            key="2"
                        >
                            <TableList />
                        </TabPane>
                    </Tabs>
                </Card>
            </>
        )
    }
}
export default connect(({ paramconfig }) => ({
    paramconfig
}))(Index);