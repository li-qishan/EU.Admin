// eslint-disable-next-line no-unused-vars
import React, { Component } from 'react';
import { connect } from 'umi';
// eslint-disable-next-line no-unused-vars
import { PageHeaderWrapper } from '@ant-design/pro-layout';
// eslint-disable-next-line no-unused-vars
import TableList from './components/TableList'
// eslint-disable-next-line no-unused-vars
import FormPage from './components/FormPage';

let me;
class apcheck extends Component {
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
            type: 'apcheck/setTableStatus',
            payload: {},
        })
    }

    componentWillMount() {
        const tempRowId = localStorage.getItem("tempRowId");
        if (tempRowId) {
            localStorage.removeItem('tempRowId');
            me.setState({ current: <FormPage Id={tempRowId} IsView={true} /> })
        }
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

// eslint-disable-next-line no-shadow
export default connect(({ apcheck }) => ({
    apcheck
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(apcheck);