import React from "react";
import already from '../../assets/already.png';

class FormStatus extends React.Component {
    constructor(props) {
        super(props);
        this.state = {

        };
    }

    render() {
        const { AuditStatus } = this.props;

        return (
            <>
                {AuditStatus === 'CompleteAudit' ? <img src={already} style={{ position: 'absolute', width: 80, left: '5px', top: '5px' }} /> : ''}
            </>
        );
    }
}
export default FormStatus;