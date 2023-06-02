import React, { Component, Suspense } from 'react';
import { Button, Divider, Dropdown, Menu, message, Input } from 'antd';
import { connect } from 'umi';
import { DownOutlined, PlusOutlined } from '@ant-design/icons';
import ProTable from '@ant-design/pro-table';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import TableList from './components/TableList'

let me;
class texture extends Component {
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            current: <TableList />
        };
    }
    componentWillUnmount() {
        const { dispatch } = this.props;
        dispatch({
            type: 'texture/setTableStatus',
            payload: {},
        })
    }
    static changePage(current) {
        me.setState({current})
    }
    render() {
        const { current } = this.state;
        const pageComponent = current;
        return (
            <>
                <PageHeaderWrapper title={false}>
                    {pageComponent}
                </PageHeaderWrapper>
            </>
        )

    }
}

export default connect(({ texture, loading }) => ({
    texture,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(texture);