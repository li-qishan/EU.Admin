import React, { Component, useState } from 'react';
import { Modal, Upload, Space, message } from 'antd';
import { connect } from 'umi';
import request from '@/utils/request';
import styles from '../index.less';
import { PlusOutlined, LoadingOutlined } from '@ant-design/icons';

// const [previewVisible, setPreviewVisible] = useState(false);
// const [previewImage, setPreviewImage] = useState('');
// const [previewTitle, setPreviewTitle] = useState('');
// const handlePreview = async (file) => {
//     if (!file.url && !file.preview) {
//         file.preview = await getBase64(file.originFileObj);
//     }

//     setPreviewImage(file.url || file.preview);
//     setPreviewVisible(true);
//     setPreviewTitle(file.name || file.url.substring(file.url.lastIndexOf('/') + 1));
// };

let me;
let flag = true;
class UploadImage extends Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        me = this;
        me.state = {
            loading: false,
            ImageUrl: null,
            MasterId: "",
            files: []
        };
    }
    // componentWillMount() {
    //     const { dispatch, Id, ImageUrl } = this.props;
    //     MasterId = Id;
    //     debugger
    //     if (ImageUrl)
    //         this.setState({
    //             ImageUrl,
    //         })
    // }
    componentDidMount() {
        const { Id, ImageUrl, isUnique } = this.props;

        if (ImageUrl)
            this.setState({
                MasterId: Id,
                ImageUrl: '/api/File/GetByUrl?url=' + ImageUrl,
            })
        else
            this.setState({
                MasterId: Id
            })
        this.getImageData();
    }
    componentWillReceiveProps(nextProps) {
        const { Id, ImageUrl } = nextProps;

        if (ImageUrl)
            this.setState({
                MasterId: Id,
                ImageUrl: '/api/File/GetByUrl?url=' + ImageUrl,
            })
    }
    getBase64(img, callback) {
        const reader = new FileReader();
        reader.addEventListener('load', () => callback(reader.result));
        reader.readAsDataURL(img);
    }
    async getImageData() {
        const { Id, filePath, isUnique, imageType } = this.props;
        if (!isUnique) {
            let result = await request('/api/File/GetFileList?masterId=' + Id + '&imageType=' + (imageType ?? filePath), {
            });
            if (result.status == 'ok') {
                this.setState({
                    files: result.data
                })
            }
        }
    }
    async uploadFileAttachment(file) {
        if (file.file.status == "removed")
            return;
        const { loading, ImageUrl, MasterId } = this.state;
        let { filePath, isUnique, masterTable, masterColumn, imageType } = this.props;
        if (!filePath)
            filePath = 'material';
        if (!isUnique)
            isUnique = false;
        if (flag)
            flag = false;
        else
            return false;

        me.getBase64(file.file.originFileObj, ImageUrl =>
            this.setState({
                ImageUrl,
                loading: true
            })
        );
        //附件上传
        const filehide = message.loading('上传中..', 0)
        const formData = new FormData();

        formData.append('fileList', file.file.originFileObj);
        let result = await request('/api/File/UploadImage?masterId=' + MasterId + "&filePath=" + filePath + "&imageType=" + (imageType ?? '') + "&masterTable=" + (masterTable ?? '') + "&masterColumn=" + (masterColumn ?? '') + "&isUnique=true", {
            data: formData,
            method: 'POST'
        });
        setTimeout(filehide);
        flag = true;
        if (result.status == "ok") {
            message.success("上传成功！");
            me.getImageData();
            // dispatch({
            //     type: 'threebelow/setFileList',
            //     payload: [],
            // })
        }
        else
            message.error(result.message);

        this.setState({
            loading: false
        })
        // }
    }
    async handlePreview(file) {
        this.setState({
            previewVisible: true,
            previewTitle: file.name,
            previewImage: file.url
        })
    }
    async onRemove(file) {
        console.log(file)
        request('/api/File/Delete?Id=' + file.uid, {
        });
        let { files } = this.state;
        let tempList = [...files];
        let index = tempList.findIndex(item => item.ID == file.uid);
        if (index > -1) {
            tempList.splice(index, 1);
            this.setState({
                files: tempList
            })
        }
        return false;
    };
    render() {
        let { accept, isUnique, filePath } = this.props;
        const { loading, ImageUrl, MasterId, files, previewVisible, previewTitle, previewImage } = this.state;
        if (!accept)
            accept = '.png,.jpeg,.jpg';
        if (!filePath)
            filePath = 'material';
        const fileList = [];
        files.map((item, index) => {
            fileList.push({
                uid: item.ID,
                name: item.OriginalFileName,
                status: 'done',
                url: '/api/File/GetByUrl?url=' + item.FileName + '.' + item.FileExt,
            })
        });


        return (
            (<div className='1122wwww'>
                {isUnique ? <>
                    {MasterId ? <>
                        <Space style={{ justifyContent: 'flex-start', float: 'left' }}>
                            {MasterId ? <Upload
                                accept={accept}
                                listType="picture-card"
                                showUploadList={false}
                                className={styles.uploader}
                                onChange={(file) => { this.uploadFileAttachment(file) }}
                            >
                                {ImageUrl ? <img src={ImageUrl} alt={ImageUrl} style={{ width: '100%' }} /> : <div>
                                    {loading ? <LoadingOutlined style={{ fontSize: 24 }} /> : <PlusOutlined style={{ fontSize: 24 }} />}
                                    <div className="ant-upload-text">图片上传</div>
                                </div>}
                            </Upload> : null}
                        </Space>
                    </> : null
                    }
                </> : <>
                    {MasterId ? <>
                        <div >

                            {/* {
                                files.map((item, index) => {
                                    return (
                                        <>
                                            <div
                                                className={styles.uploader}
                                                style={{ justifyContent: 'flex-start', float: 'left' }}
                                            >
                                                <img src={'/api/File/GetByUrl?url=' + item.FileName + '.' + item.FileExt} alt={ImageUrl} style={{ width: '100%' }} />
                                            </div>
                                        </>
                                    )
                                })
                            } */}
                            {MasterId ? <div

                            ><Upload
                                accept={accept}
                                listType="picture-card"
                                // showUploadList={false}
                                fileList={fileList}
                                onChange={(file) => { this.uploadFileAttachment(file) }}
                                //   onPreview={handlePreview}
                                onPreview={(file) => { this.handlePreview(file) }}
                                onRemove={(file) => { this.onRemove(file) }}
                            >
                                    <div>
                                        {loading ? <LoadingOutlined style={{ fontSize: 24 }} /> : <PlusOutlined style={{ fontSize: 24 }} />}
                                        <div className="ant-upload-text">图片上传</div>
                                    </div>
                                </Upload></div> : null}
                        </div>
                        <Modal
                            open={previewVisible}
                            title={previewTitle}
                            footer={null}
                            onCancel={(file) => {
                                this.setState({
                                    previewVisible: false
                                })
                            }}
                        >
                            <img
                                alt="example"
                                style={{
                                    width: '100%',
                                }}
                                src={previewImage}
                            />
                        </Modal>
                    </> : null
                    }
                </>}
            </div>)
        );
    }
}

export default connect(({ uploadimage }) => ({
    uploadimage,
    // loading: loading.effects['dashboardAndanalysis/fetch'],
}))(UploadImage);
