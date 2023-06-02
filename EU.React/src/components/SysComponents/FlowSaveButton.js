import React from "react";
import { withPropsAPI } from "gg-editor";
import { Button } from "antd";

class Save extends React.Component {
    render() {
        return (
            <div style={{ padding: 8 }}>
                <Button
                    {...this.props}
                >保存</Button>
            </div>
        );
    }
}

export default withPropsAPI(Save);
