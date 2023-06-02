import React from "react";
import { Button, Space, Modal, Descriptions, Skeleton, message } from "antd";
import { PlusOutlined, UnorderedListOutlined, CheckOutlined, RollbackOutlined } from '@ant-design/icons';
import { GetModuleLogInfo } from '../../services/common';
import Utility from '@/utils/utility';

class FormToolbar extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            recordLogVisible: false,
            recordLogData: null
        };
    }
    async showLogRecord() {
        const { value: { me: { state: { Id, moduleCode } } } } = this.props;

        let response = await GetModuleLogInfo({ moduleCode, id: Id });
        if (response.Success) {
            this.setState({ recordLogData: response.Data, recordLogVisible: true })
        } else {
            message.error(response.message);
        }
    }
    showLogRecordCancel = e => {
        this.setState({
            recordLogData: null,
            recordLogVisible: false
        });
    };
    async onAuditStatus(auditStatus) {
        const { value: { me, me: { state: { Id, moduleCode, AuditStatus, modelName } }, dispatch }, onAuditSubmit: onAuditSubmit } = this.props;

        if (auditStatus == 'Add') {
            message.loading('数据提交中...', 0);
            dispatch({
                type: modelName + '/auditOrder',
                payload: { orderId: Id, auditStatus },
            }).then((result) => {
                result = result.response;
                if (onAuditSubmit)
                    onAuditSubmit(AuditStatus, result.auditStatus, result);
                message.destroy();
                if (result.status == "ok") {
                    me.setState({ AuditStatus: result.auditStatus, IsView: true });
                    message.success(result.message);
                }
                else
                    message.error(result.message);
            });
        } else if (auditStatus == 'CompleteAudit') {
            Modal.confirm({
                title: '确认执行变更数据？?',
                icon: Utility.getIcon('ExclamationCircleOutlined'),
                // content: '是否确定删除记录？',
                okText: '确定',
                okType: 'danger',
                cancelText: '取消',
                onOk() {
                    message.loading('数据提交中...', 0);
                    dispatch({
                        type: modelName + '/auditOrder',
                        payload: { orderId: Id, auditStatus },
                    }).then((result) => {
                        result = result.response;
                        if (onAuditSubmit)
                            onAuditSubmit(AuditStatus, result.auditStatus, result);

                        message.destroy();
                        if (result.status == "ok") {
                            me.setState({ AuditStatus: result.auditStatus, IsView: false })
                            message.success(result.message);
                        }
                        else
                            message.error(result.message);
                    })
                },
                onCancel() {
                    console.log('Cancel');
                },
            });
        }
    }
    render() {
        let { recordLogVisible, recordLogData } = this.state;
        const { value: { me, me: { state: { Id, moduleCode, AuditStatus, modelName, disabled } } }, onAuditSubmit: onAuditSubmit, moduleInfo } = this.props;
        return (<>
            {me && Id && !disabled ? <> <Space style={{ display: 'flex', justifyContent: 'flex-end' }}>
                <Button type="default" onClick={() => {
                    me.formRef.current.resetFields();
                    me.setState({ Id: null })
                }}><PlusOutlined /></Button>
                {/* {AuditStatus && AuditStatus == 'Add' ? <Button
                    onClick={() => {
                        onAuditSubmit(AuditStatus)
                    }} type="primary"><CheckOutlined /> 审核</Button> : null}
                {AuditStatus && AuditStatus == 'CompleteAudit' ? <Button
                    onClick={() => {
                        onAuditSubmit(AuditStatus)
                    }}><RollbackOutlined /> 撤销</Button> : null} */}
                {moduleInfo && moduleInfo.IsShowAudit && AuditStatus ? <>
                    {AuditStatus == 'Add' ? <Button
                        onClick={() => {
                            if (modelName)
                                this.onAuditStatus(AuditStatus)
                            else
                                onAuditSubmit(AuditStatus);
                        }} type="primary"><CheckOutlined /> 审核</Button> : null}
                    {AuditStatus == 'CompleteAudit' ? <Button
                        onClick={() => {
                            if (modelName)
                                this.onAuditStatus(AuditStatus);
                            else
                                onAuditSubmit(AuditStatus);
                        }}><RollbackOutlined /> 撤销</Button> : null}
                </> : null}
                {moduleCode ? <Button
                    onClick={() => {
                        this.showLogRecord()
                    }}
                ><UnorderedListOutlined /> 日志</Button> : null}

            </Space><div style={{ height: 10 }}></div></> : null}
            <Modal
                title="日志"
                open={recordLogVisible}
                width={1000}
                footer={null}
                onCancel={this.showLogRecordCancel}
            >
                {recordLogData ? <Descriptions title="表信息" bordered>
                    <Descriptions.Item label="表名称">{recordLogData.tableName}</Descriptions.Item>
                    <Descriptions.Item label="表主键" span={2}>{recordLogData.ID} </Descriptions.Item>
                    <Descriptions.Item label="创建人">{recordLogData.CreatedBy} </Descriptions.Item>
                    <Descriptions.Item label="最后修改人" span={2}>{recordLogData.UpdateBy} </Descriptions.Item>
                    <Descriptions.Item label="创建时间">{recordLogData.CreatedTime} </Descriptions.Item>
                    <Descriptions.Item label="最后修改时间" span={2}>{recordLogData.UpdateTime} </Descriptions.Item>
                </Descriptions> : <Skeleton active />}
            </Modal>
        </>);
    }
}
export default FormToolbar;
