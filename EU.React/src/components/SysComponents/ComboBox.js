import React, {  } from "react";
import { Select } from "antd";
import request from "@/utils/request";

// eslint-disable-next-line no-unused-vars
const { Option } = Select;

class ComBoBox extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: []
        };
    }

    async componentWillMount() {
        const { id } = this.props;
        const result = await request('/api/SmLov/GetByCode', {
            method: 'GET',
            params: { code: id },
        });
        if (result && result.Success && result.Data && result.Data.length > 0) {
            this.setState({ data: result.Data });
        }
    }
    
    componentWillReceiveProps(nextProps) {
        const { value } = nextProps;
        this.setState({ comboValue: value })
    }
    // onChangeValue(e) {
    //     const { onChange, value } = this.props;// 有默认传来的 chang事件，和 value值
    //     this.setState({ comboValue: value })
    //     onChange(e);
    // }
    
    render() {
        const { onChange } = this.props;
        const { comboValue, data } = this.state;
        return (
            <Select
                allowClear
                // onChange={(e) => {
                //     this.onChangeValue(e)
                // }}
                value={comboValue}
                {...this.props}
                // eslint-disable-next-line no-shadow
                onChange={(value, Option) => {
                    let r = null;
                    if (data && data.length > 0)
                        // eslint-disable-next-line func-names
                        r = data.filter(function (s) {
                            return s.ID === value; // 注意：IE9以下的版本没有trim()方法
                        });
                    onChange(value, Option, r);
                }}
            >
                {data && data.length > 0 && data.map((item) => {
                    return (
                        <Option key={item.key}>{item.value}</Option>
                    )
                })}
            </Select>
        )
    }
}

export default ComBoBox