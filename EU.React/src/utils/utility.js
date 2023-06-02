/* eslint-disable no-unused-vars */
import dayjs, { isMoment, isDate } from 'dayjs'
import { GenerateCode } from '@/services/GenerateCode'
import {
  SearchOutlined, ExclamationCircleOutlined, CloudDownloadOutlined, RedoOutlined,
  RollbackOutlined, PlusOutlined, BarsOutlined, BookOutlined, SmileOutlined, HeartOutlined,
  AppstoreOutlined, AppstoreAddOutlined, TeamOutlined, FileAddOutlined, FileOutlined, UserSwitchOutlined,
  UserOutlined, SettingOutlined, LoadingOutlined, HomeOutlined
} from '@ant-design/icons';

const IconMap = {
  home: <HomeOutlined />,
  smile: <SmileOutlined />,
  heart: <HeartOutlined />,
  appstore: <AppstoreOutlined />,
  appstoreadd: <AppstoreAddOutlined />,
  team: <TeamOutlined />,
  fileadd: <FileAddOutlined />,
  file: <FileOutlined />,
  userswitch: <UserSwitchOutlined />,
  user: <UserOutlined />,
  setting: <SettingOutlined />,
  book: <BookOutlined />,
  bars: <BarsOutlined />,
  RedoOutlined: <RedoOutlined />,
  PlusOutlined: <PlusOutlined />,
  RollbackOutlined: <RollbackOutlined />,
  LoadingOutlined: <LoadingOutlined />,
  CloudDownloadOutlined: <CloudDownloadOutlined />,
  ExclamationCircleOutlined: <ExclamationCircleOutlined />,
  SearchOutlined: <SearchOutlined />
};

export default class Utility {
  static async getGenerateCode(code) {
    const result = await GenerateCode(code);
    if (result.status === "ok") {
      return result.number;
    }
    return result.message;

  }

  static FormDateFormat(formValue) {
    // eslint-disable-next-line no-plusplus
    for (let i = 0; i < Object.keys(formValue).length; i++) {
      if (isMoment(Object.values(formValue)[i])) {
        // eslint-disable-next-line no-param-reassign
        formValue[Object.keys(formValue)[i]] = dayjs(Object.values(formValue)[i]).format();
      }
    }
  }

  static setFormFormat(formValue) {
    // eslint-disable-next-line no-plusplus
    for (let i = 0; i < Object.keys(formValue).length; i++) {
      const data = Object.values(formValue)[i];
      // eslint-disable-next-line no-restricted-globals
      if (isNaN(data) && !isNaN(Date.parse(data))) {
        // eslint-disable-next-line no-param-reassign
        formValue[Object.keys(formValue)[i]] = dayjs(Object.values(formValue)[i]);
      }
    }
  }

  static getIcon(name) {
    if (IconMap[name])
      return IconMap[name]
    return null
  }

  static downloadFileById(fileId, fileName) {
    // const url = window.URL.createObjectURL(`/api/File/Download?id=${ fileId}`);
    const url = `/api/File/Download?id=${fileId}`;
    const link = document.createElement('a')
    link.style.display = 'none'
    link.href = url
    link.setAttribute('download', fileName)

    document.body.appendChild(link)
    link.click()
    // 释放URL对象所占资源
    window.URL.revokeObjectURL(url)
    // 用完即删
    document.body.removeChild(link)
  }

  static GetQueryString(name) {
    const reg = new RegExp(`(^|&)${name}=([^&]*)(&|$)`);
    const r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
  }

  static addCache(key, value) {
    localStorage.setItem(key, value);
  }
  static getCache(key) {
    return localStorage.getItem(key);
  }
}
