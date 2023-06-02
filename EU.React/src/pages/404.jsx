import { Button, Result,Card } from 'antd';
import React from 'react';
import { history } from 'umi';

const NoFoundPage = () => (
  <Card>
  <Result
    status="404"
    title="404"
    subTitle="抱歉, 您访问的页面不存在."
    extra={
      <Button type="primary" onClick={() => history.push('/')}>
        回到首页
      </Button>
    }
  /></Card>
);

export default NoFoundPage;
