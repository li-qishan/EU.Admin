import React, { Component } from 'react';
import { Modal, Upload, Form, Row, Col, Steps, Button, Space, message, Result, Table } from 'antd';
import { connect } from 'umi';
import Utility from '@/utils/utility';
import request from '@/utils/request'
import { UploadOutlined } from '@ant-design/icons';

const { Step } = Steps;
const FormItem = Form.Item;

let me;
let flag = true;
class FormPage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            Id: '',//从表主键,
            stepsCurrent: 0,
            errorList: [],
            importColumns: [],
            importList: [],
            importTemplateInfo: {},
            importDataId: null,
            templateUrl: null
        };
    }
    componentWillReceiveProps(nextProps) {
        // const { dispatch, Id, modalVisible } = nextProps;
        // this.setState({ Id: Id ? Id : null })

        const { dispatch, Id, modalVisible, moduleInfo: { moduleCode } } = nextProps;
        if (dispatch && moduleCode && modalVisible) {
            dispatch({
                type: 'importconfig/getByModuleCode',
                payload: { moduleCode },
            }).then((result) => {
                if (result.response.data && me.formRef.current) {
                    Utility.setFormFormat(result.response.data);
                    if (result.response.fileId)
                        this.setState({ importTemplateInfo: result.response.data, fileId: result.response.fileId })
                    else
                        this.setState({ importTemplateInfo: result.response.data })

                    // dispatch({
                    //     type: 'importconfig/setDetail',
                    //     payload: result.response
                    // });
                }

            })
        }
    }

    okHandle = async () => {
        // const { onSubmit: handleAdd } = this.props;
        // const fieldsValue = await me.formRef.current.validateFields();
        // const { Id } = this.state;
        // if (Id)
        //     fieldsValue.ID = Id;
        // handleAdd(fieldsValue);
    };
    beforeUpload(file) {
        const isJpgOrPng = file.type === 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
        if (!isJpgOrPng) {
            message.error('请选择正确的Excel文件!');
            return;
        }
        const isLt2M = file.size / 1024 / 1024 < 2;
        // if (!isLt2M) {
        //     message.error('Image must smaller than 2MB!');
        // }
        return isJpgOrPng && isLt2M;
    }
    async handleChange(file) {
        const { moduleInfo: { moduleCode } } = this.props;
        const { Id } = this.state;
        if (flag)
            flag = false;
        else
            return false;
        //附件上传
        const filehide = message.loading('上传中..', 0)
        const formData = new FormData();
        formData.append('fileList', file.file.originFileObj);
        let result = await request('/api/Common/ImportExcel?moduleCode=' + moduleCode + "&fileName=" + file.file.name, {
            data: formData,
            method: 'POST'
        });
        setTimeout(filehide);
        flag = true;
        if (result.status == "ok") {
            let importList = result.data.importList;
            for (let index = 0; index < importList.length; index++)
                importList[index].Key = index + 1;
            let importColumns = [{
                title: '序号',
                dataIndex: 'Key',
                key: 'Key',
            }];
            if (result.data.importColumns && result.data.importColumns.length > 0) {
                for (let j = 0; j < result.data.importColumns.length; j++)
                    importColumns.push({
                        title: result.data.importColumns[j],
                        dataIndex: result.data.importColumns[j],
                        key: 'Key_' + j,
                    });
            }

            this.setState({ stepsCurrent: 1, importList, importColumns, importDataId: result.data.importDataId })
            message.success("上传成功！", 3)
            // dispatch({
            //     type: 'threebelow/setFileList',
            //     payload: [],
            // })
        }
        else {
            let errorList = result.data.errorList;
            for (let index = 0; index < errorList.length; index++)
                errorList[index].Key = index + 1;
            if (errorList.length > 0)
                this.setState({ stepsCurrent: 1, errorList })

            message.error(result.message, 3);
        }
        // }
    }
    okTransferData = async (type) => {
        const { moduleInfo: { masterId, moduleCode } } = this.props;

        const { onReload } = this.props;
        const { importTemplateInfo, importDataId } = this.state;
        const filehide = message.loading('数据转换中..', 0)
        let result = await request('/api/Common/TransferExcelData', {
            data: {
                type,
                importDataId,
                importTemplateCode: importTemplateInfo.TemplateCode,
                masterId: masterId ?? null,
                moduleCode: moduleCode ?? null
            },
            method: 'POST'
        });
        setTimeout(filehide);
        // debugger

        if (result.status == "ok") {
            onReload();
            this.setState({ stepsCurrent: 2 });
            message.success(result.message, 3)
        }
        else {
            // this.setState({ importDataId: null });
            message.error(result.message, 3);
        }
    };
    onStepsChange = current => {
        console.log('onChange:', current);
        this.setState({ stepsCurrent: current });
    };
    onDownload(fileId) {
        Utility.downloadFileById(fileId, '');
    }
    render() {
        const { modalVisible, moduleInfo, onCancel } = this.props;
        const { Id, stepsCurrent, errorList, importList, importColumns, importTemplateInfo: { IsAllowOverride }, fileId } = this.state;
        // let { webUrl } = defaultSettings;
        // let IsView = false;

        return (
            (<Modal
                destroyOnClose
                title={moduleInfo.moduleName + '导入'}
                open={modalVisible}
                onOk={this.okHandle}
                maskClosable={false}
                width={1000}
                onCancel={() => { this.setState({ stepsCurrent: 0, errorList: [] }); onCancel() }}
                footer={[
                    <Button key="back" onClick={() => { this.setState({ stepsCurrent: 0, errorList: [] }); onCancel() }}>
                        关闭
                    </Button>
                ]}
            >
                <Steps
                    type="navigation"
                    current={stepsCurrent}
                    size="small"
                    // onChange={this.onStepsChange}
                    className="site-navigation-steps"
                >
                    <Step status="process" title="上传Execl" />
                    <Step status="process" title="数据预览" />
                    <Step status="process" title="导入数据" />
                </Steps>
                {stepsCurrent == 0 ? <Form
                    labelCol={{ span: 8 }}
                    wrapperCol={{ span: 18 }}
                    // onFinish={(values) => { this.onFinish(values) }}
                    ref={this.formRef}
                    style={{ marginTop: 20 }}
                >
                    <Row gutter={24} justify={"center"}>
                        <Col span={24} >
                            <FormItem label="Excel文件：">
                                <Space>
                                    <Upload
                                        accept=".xlsx,.xls"
                                        // listType="picture-card"
                                        // className={styles.uploader}
                                        showUploadList={false}
                                        beforeUpload={(file) => { this.beforeUpload(file) }}
                                        // fileList={fileList}
                                        onChange={(file) => { this.handleChange(file) }}
                                    // onRemove={(file) => { this.deleteFilled(file) }}
                                    >
                                        <Button type="primary"
                                            onClick={() => {

                                            }}
                                        ><UploadOutlined /> 点击上传Excel文件</Button>
                                        {/* <Button><UploadOutlined /> </Button> */}
                                    </Upload>

                                </Space>
                            </FormItem>
                        </Col>
                        <Col span={24} >
                            <FormItem label="导入步骤：">
                                <Row>1、下载导入模板：{fileId ? <a onClick={() => this.onDownload(fileId)} target="_blank" key="link">{moduleInfo.moduleName}导入模板</a> : null}</Row>
                                <Row>2、根据模板中的格式填写内容，不可以调整列的先后顺序。</Row>
                                <Row>3、点击“选择Excel文件”执行上传操作。</Row>
                            </FormItem>
                        </Col>
                        <Col span={24} >
                            <FormItem label="注意事项：">
                                <Row>1、后缀名必须为xlsx或xls。</Row>
                                <Row>2、数据请勿放在合并的单元格中。</Row>
                                <Row>3、第一行红色字体的为必填栏位，同时注意特殊字段的格式是否正确，例如：日期类型，数字类型等。</Row>
                                <Row>4、不可以调整Excel模板中列的顺序。</Row>
                                <Row>5、不可以修改导入模板中的工作簿(Sheet)名称。</Row>
                                <Row>6、导入数据时，系统会将第一行的内容作为标题行，因此导入的内容请从第2行开始填写。</Row>
                            </FormItem>
                        </Col>
                    </Row>
                </Form> : null}
                {stepsCurrent == 1 && errorList.length > 0 ? <>
                    <Result
                        style={{ padding: 20 }}
                        status="error"
                        title="读取失败"
                        subTitle={errorList.length + '条错误信息'}
                        extra={[
                            <Button type="primary" key="console" onClick={() => {
                                this.setState({ stepsCurrent: 0, errorList: [], importList: [] })
                            }}>返回上一页</Button>
                        ]}
                    >
                    </Result>
                    <Table columns={[
                        {
                            title: '序号',
                            dataIndex: 'Key',
                            key: 'Key',
                        },
                        {
                            title: 'Sheet名',
                            dataIndex: 'SheetName',
                            key: 'SheetName',
                        },
                        {
                            title: '错误信息',
                            dataIndex: 'ErrorName',
                            key: 'ErrorName',
                        },
                    ]} dataSource={errorList} />
                </> : ''}
                {stepsCurrent == 1 && importList.length > 0 ? <>
                    <Result
                        style={{ padding: 20 }}
                        status="success"
                        title="读取成功"
                        subTitle={'本次从Excel共读取数据：' + importList.length + '笔，以下只显示了部分数据供用户预览'}
                        extra={[
                            <Button type="primary" key="append" onClick={() => { this.okTransferData('append') }}>{Utility.getIcon('PlusOutlined')}追加导入</Button>,
                            <>{IsAllowOverride ? <Button key="override" danger onClick={() => { this.okTransferData('override') }}>{Utility.getIcon('RollbackOutlined')}覆盖导入</Button> : null}</>,
                            <Button type="primary" key="console" onClick={() => {
                                this.setState({ stepsCurrent: 0, importDataId: null, errorList: [], importList: [] })
                            }}>返回上一页</Button>
                        ]}
                    >
                    </Result>
                    <Table columns={importColumns} dataSource={importList} />
                </> : ''
                }
                {
                    stepsCurrent == 2 ? <>
                        <Result
                            style={{ padding: 20 }}
                            status="success"
                            title="导入成功"
                            // subTitle={'本次从Excel共读取数据：' + importList.length + '笔，以下只显示了部分数据供用户预览'}
                            extra={[
                                <Button type="primary" key="console" onClick={() => {
                                    this.setState({ stepsCurrent: 0, importDataId: null, errorList: [], importList: [] })
                                }}>返回</Button>
                            ]}
                        >
                        </Result>
                    </> : ''
                }
            </Modal >)
        );
    }
}
export default connect(({ importconfig }) => ({
    importconfig,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(FormPage);