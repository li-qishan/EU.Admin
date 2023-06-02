import React, { Component } from 'react';
import { Button, Upload, Space, message } from 'antd';
import { connect } from 'umi';
import { UploadOutlined } from '@ant-design/icons';
import { query, BatchDelete, Delete } from './service';
import SmProTable from '@/components/SysComponents/SmProTable';
import request from '@/utils/request';
import defaultSettings from '../../../config/defaultSettings';
import Utility from '@/utils/utility';

let moduleCode = "SM_FILE_ATTACHMENT";
let me;
let MasterId = "";
let flag = true;
class AttachmentList extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {

        };
    }
    componentWillMount() {
        const { dispatch, Id } = this.props;
        MasterId = Id;
        dispatch({
            type: 'attachment/getModuleInfo',
            payload: { moduleCode },
        })
    }
    componentDidMount() {
        const { Id } = this.props;
        MasterId = Id;
    }
    componentWillReceiveProps(nextProps) {
        const { Id } = nextProps;
        MasterId = Id;
    }
    async uploadFileAttachment(file, action) {
        let { filePath, isUnique } = this.props;
        if (!filePath)
            filePath = 'material';
        if (!isUnique)
            isUnique = false;
        if (flag)
            flag = false;
        else
            return false;

        //附件上传
        const filehide = message.loading('附件上传中..', 0)
        const formData = new FormData();
        formData.append('fileList', file.file.originFileObj);
        let result = await request('/api/File/Upload?masterId=' + MasterId + "&filePath=" + filePath + "&isUnique=" + isUnique, {
            data: formData,
            method: 'POST'
        });
        setTimeout(filehide);
        flag = true;
        if (result.status == "ok") {
            action.reload();
            message.success("附件上传成功！")
            // dispatch({
            //     type: 'threebelow/setFileList',
            //     payload: [],
            // })
        }
        else {
            message.error(result.message);
        }
        // }
    }
    onDownload(item) {
        Utility.downloadFileById(item.ID, item.FileName);
    }
    render() {
        let { dispatch, attachment: { moduleInfo, tableParam }, accept, filePath, IsView } = this.props;
        if (!accept)
            accept = '.png,.jpeg';
        if (!filePath)
            filePath = 'material';
        let { webUrl } = defaultSettings;

        const columns = [
            {
                title: '创建时间',
                hideInSearch: true,
                dataIndex: 'CreatedTime',
                width: 180
            }, {
                title: '文件名',
                width: 180,
                // hideInSearch: true,
                // filters: false,
                hideInSearch: true,
                dataIndex: 'OriginalFileName',
                // render: (_, item) => {
                //     return <Badge status={item.status} text={item.ip} />;
                //   },
                //render: (reload, item) => [<a href={webUrl + item.Path + "/" + item.FileName + "." + item.FileExt} target="_blank" key="link">{reload}</a>]
                render: (reload, item) => [<a onClick={() => this.onDownload(item)} target="_blank" key="link">{reload}</a>]
                // }, {
                //     title: '排序号',
                //     width: 180,
                //     hideInSearch: true,
                //     dataIndex: 'TaxisNo'
            }
        ];
        const ViewFileAttachment = (Id) => {
            Index.changePage(<SqlEdit Id={Id} />)
        }
        //#region 操作栏按钮方法
        const action = {
            ViewFileAttachment
        }
        //#endregion

        return (
            <div>

                <SmProTable
                    columns={columns}
                    delete={Delete}
                    batchDelete={(selectedRows) => BatchDelete(selectedRows)}
                    moduleInfo={moduleInfo}
                    {...action}
                    search={false}
                    toolBarRender={(action, { }) => [
                        <Space style={{ display: 'flex', justifyContent: 'center' }}>
                            {MasterId && IsView != true ? <Upload
                                accept={accept}
                                showUploadList={false}
                                onChange={(file) => { this.uploadFileAttachment(file, action) }}
                            >
                                <Button type="primary"><UploadOutlined /> 上传附件</Button>
                            </Upload> : null}
                        </Space>
                    ]}
                    addPage={() => this.setState({ detailVisible: true, DetailId: '', detailView: false })}
                    // changePage={Index.changePage}
                    // formPage={FormPage}
                    formRef={this.formRef}
                    form={{ labelCol: { span: 6 } }}
                    onReset={() => {
                        dispatch({
                            type: 'attachment/setTableStatus',
                            payload: {},
                        })
                    }}
                    onLoad={() => {
                        if (tableParam && tableParam.params && this.formRef.current) {
                            this.formRef.current.setFieldsValue({ ...tableParam.params })
                        }
                    }}
                    pagination={tableParam && tableParam.params ? { current: tableParam.params.current } : {}}
                    request={(params, sorter, filter) => {
                        if (tableParam && tableParam.params && !params._timestamp) {
                            params = tableParam.params;
                            //sorter = tableParam.sorter
                        }
                        dispatch({
                            type: 'attachment/setTableStatus',
                            payload: { params, sorter },
                        })
                        filter.MasterId = MasterId;
                        return query({
                            paramData: JSON.stringify(params),
                            sorter: JSON.stringify(sorter),
                            filter: JSON.stringify(filter),
                            moduleCode,
                            parentColumn: 'ImageType', parentId: filePath

                        })
                    }}
                />
            </div>
        )
    }
}

export default connect(({ attachment }) => ({
    attachment,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(AttachmentList);