import React, { Component } from 'react';
import { connect } from 'umi';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import TableList from './components/TableList'

let me;
class settlementway extends Component {
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
            type: 'settlementway/setTableStatus',
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

export default connect(({ settlementway }) => ({
    settlementway,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(settlementway);