import { LogoutOutlined, SettingOutlined, UserOutlined } from '@ant-design/icons';
import { Avatar, Menu, Spin, Modal } from 'antd';
import React from 'react';
import { history, connect } from 'umi';
import HeaderDropdown from '../HeaderDropdown';
import styles from './index.less';
import { logOut } from '@/services/login';
import defaultPhoto from '../../assets/default.png';
import signalr from '@/utils/signalr';
import Utility from '@/utils/utility';

class AvatarDropdown extends React.Component {
  onMenuClick = async event => {
    const { key } = event;

    if (key === 'logout') {
      const { dispatch } = this.props;

      Modal.confirm({
        title: '退出系统',
        icon: Utility.getIcon('ExclamationCircleOutlined'),
        content: '确认退出系统？',
        okText: '确定',
        okType: 'danger',
        cancelText: '取消',
        async onOk() {
          if (dispatch) {
            signalr.stop();
            await logOut();//后端停用token
            dispatch({
              type: 'login/logout',
            });
          }
          return;
        },
        onCancel() {
          console.log('Cancel');
        },
      });

    }
    // else history.push(`/account/${key}`);
  };

  render() {
    const { currentUser = {}, menu, } = this.props;
    const menuHeaderDropdown = (
      <Menu className={styles.menu} selectedKeys={[]} onClick={this.onMenuClick}>
        {/* {menu && (
          <Menu.Item key="center">
            <UserOutlined />
            个人中心
          </Menu.Item>
        )} */}
        {menu && (
          <Menu.Item key="settings">
            <SettingOutlined />
            个人设置
          </Menu.Item>
        )}
        {menu && <Menu.Divider />}

        <Menu.Item key="logout">
          <LogoutOutlined />
          退出登录
        </Menu.Item>
      </Menu>
    );
    return currentUser.data && currentUser.data.name ? (
      <HeaderDropdown overlay={menuHeaderDropdown}>
        <span className={`${styles.action} ${styles.account}`}>
          <Avatar size="small" className={styles.avatar} src={currentUser.data.avatar ? currentUser.data.avatar : defaultPhoto} alt="avatar" />
          <span className={styles.name}>{currentUser.data.name}</span>
        </span>
      </HeaderDropdown>
    ) : (
      <span className={`${styles.action} ${styles.account}`}>
        <Spin
          size="small"
          style={{
            marginLeft: 8,
            marginRight: 8,
          }}
        />
      </span>
    );
  }
}

export default connect(({ user }) => ({
  currentUser: user.currentUser,
}))(AvatarDropdown);
