import React, { Component } from 'react';
import { connect } from 'umi';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import Index from './components/Index'

let me;
class paramconfig extends Component {
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            current: <Index />
        };
    }
    componentWillUnmount() {
        const { dispatch } = this.props;
        dispatch({
            type: 'paramconfig/setTableStatus',
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

export default connect(({ paramconfig, loading }) => ({
    paramconfig,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(paramconfig);