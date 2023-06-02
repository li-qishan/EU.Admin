/* eslint-disable no-unused-vars */
import React, { Component, useState } from "react";
import { Select, Spin } from "antd";
import request from "@/utils/request";

const { Option } = Select;

let page = 1;
let oldData = [];
let searchValue = '';

class ComBoGrid extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            dropDownData: [],
            parentId: null
        };
    }

    componentDidMount() {
        const { parentId, value } = this.props;
        page = 1;
        oldData = [];
        searchValue = '';
        if (!parentId || value) {// 联动下级不查询
            this.queryLoadData();
        }
    }

    /**
     * 下拉数据查询
     * @param {*} sql
     */
    async queryLoadData(value = '', isSearch = false, parentId) {
        const { api, itemvalue, param } = this.props;
        if (isSearch) {
            page = 1;
            oldData = [];
        }
        if (!parentId)
            parentId = this.props.parentId;

        searchValue = value;// 记住搜索值，下拉事件调用
        if (value) {
            this.setState({ loading: true });// 查询显示加载
        }
        let paramData = {
            current: page,
            pageSize: 100,
        }
        if (value) {
            if (typeof itemvalue === 'string')
                paramData[itemvalue] = value
            else
                itemvalue.forEach(e => {
                    paramData[e] = value
                });
            // eslint-disable-next-line array-callback-return
            // itemvalue.map((a) => {
            //     paramData[a] = value
            // })
        }
        // 联动上级数据
        let newParentId;
        let oldParentId;
        if (parentId) {
            // eslint-disable-next-line prefer-destructuring
            // paramData[Object.keys(parentId)[0]] = Object.values(parentId)[0];
            // eslint-disable-next-line prefer-destructuring
            newParentId = Object.values(parentId)[0];
        }
        else if (this.state.parentId) {
            // eslint-disable-next-line prefer-destructuring
            // paramData[Object.keys(this.state.parentId)[0]] = Object.values(this.state.parentId)[0]
            // eslint-disable-next-line prefer-destructuring
            oldParentId = Object.values(this.state.parentId)[0];
        }
        // eslint-disable-next-line eqeqeq
        if (newParentId != oldParentId) {
            page = 1;
            oldData = [];
            this.setState({ comboValue: '', loading: false, dropDownData: oldData })
        }
        // let filter = {};
        // if (param)
        //     filter = Object.assign(filter, param)
        const result = await request(api, {
            method: 'GET',
            params: {
                paramData,
                filter: param,
                parentColumn: parentId ? Object.keys(parentId)[0] : null,
                parentId: parentId ? Object.values(parentId)[0] : null
            }
        });
        if (result && result.status=='ok' && result.data && result.data.length > 0) {
            this.setState({ loading: false, dropDownData: oldData.concat(result.data) })
        } else
            this.setState({ loading: false, dropDownData: oldData })

    }

    /**
     * 父组件props发生变化时发生
     * @param {*} nextProps
     */
    componentWillReceiveProps(nextProps) {
        const { value, parentId } = nextProps;
        const { dropDownData } = this.state;
        this.setState({ comboValue: value });
        // && (this.props.value !== nextProps.value || !this.props.value)
        // let aa = this.props.value;
        // let bb = nextProps.value;
        if (Object.keys(nextProps).includes('parentId')) {
            if (parentId && Object.values(parentId)[0] != null)
                if (this.props.value !== value || dropDownData.length === 0 || typeof this.props.value === 'undefined' && typeof value === 'undefined') {
                    this.setState({ parentId })
                    this.queryLoadData(null, null, parentId);
                }
                else
                    this.setState({ dropDownData: [] })
        }
    }

    /**
     * 下拉事件
     */
    popuploadData(e) {
        const { target } = e
        // scrollHeight：代表包括当前不可见部分的元素的高度
        // scrollTop：代表当有滚动条时滚动条向下滚动的距离，也就是元素顶部被遮住的高度
        // clientHeight：包括padding但不包括border、水平滚动条、margin的元素的高度
        const rmHeight = target.scrollHeight - target.scrollTop
        const clHeight = target.clientHeight
        // 滚动条到达底部
        if (rmHeight === clHeight) {
            const { dropDownData } = this.state;
            oldData = dropDownData;// 记住原始数据，分页查询时拼接
            // eslint-disable-next-line no-plusplus
            page++;
            this.queryLoadData(searchValue);
        }
    }
    // /**
    //  * 父组件有默认传来的 chang事件，和 value值
    //  * @param {*} e
    //  */
    // onChangeValue(e) {
    //     const { onChange, value } = this.props;
    //     this.setState({ comboValue: value })
    //     onChange(e);
    // }

    render() {
        const { itemkey, itemvalue, onChange } = this.props;
        const { comboValue, loading, dropDownData } = this.state;
        return (
            <Select
                allowClear
                showSearch={true}
                filterOption={false}
                // onChange={(e) => {
                //     this.onChangeValue(e)
                // }}
                value={comboValue}
                // onPopupScroll={(e) => { this.popuploadData(e) }}
                notFoundContent={loading ? <Spin size="small" /> : null}
                onSearch={(value) => { this.queryLoadData(value, true) }}
                {...this.props}
                // eslint-disable-next-line no-shadow
                onChange={(value, Option) => {
                    let r = null;
                    if (dropDownData && dropDownData.length > 0)
                        // eslint-disable-next-line func-names
                        r = dropDownData.filter(function (s) {
                            return s.ID === value; // 注意：IE9以下的版本没有trim()方法
                        });
                    onChange(value, Option, r);
                }}
            >
                {dropDownData && dropDownData.length > 0 && dropDownData.map(item => {
                    return (
                        // <>
                        //     {itemvalue.split(',').length == 2 ?
                        //         <>
                        //             {/* <Option key={item[itemkey]}>{itemvalue.split(',').map(value => {
                        //                 return (item[value])
                        //             })}</Option> */}
                        //             <Option key={item[itemkey]}>{item[itemvalue.split(',')[0]]} - {item[itemvalue.split(',')[1]]}</Option>
                        //         </> :
                        //         <Option key={item[itemkey]}>{item[itemvalue]}</Option>
                        //     }
                        // </>
                        <>{
                            typeof itemvalue === 'string' ?
                                <Option key={item['ID']}>{item[itemvalue]}</Option>
                                :
                                <Option key={item['ID']}>
                                    {itemvalue.map((value, index) => {
                                        if (index === 0)
                                            return (item[value])
                                        return (` - ${item[value]}`)

                                    })}
                                </Option>
                        }
                        </>
                    )
                })}
            </Select>
        )
    }
}

export default ComBoGrid
