# Docker部署

## 前言
使用的是VMware+Centos7安装虚拟机进行测试

## 1 VMware虚拟机安装Linux教程

参考地址如下

https://blog.csdn.net/weixin_52799373/article/details/124324077


## 一些Q&A

#### Windows系统重启后，发现虚拟机上不了网？

答：执行service network restart命令后出现下面的错误
```bash
Restarting network (via systemctl): Job for network.service failed because the control process exited with error code.

    See "systemctl status network.service" and "journalctl -xe" for details.    [FAILED]
```

1、先查看下你电脑有没有禁用了VMware DHCP service、VMware Workstation Service、VMware NAT service 这几个vm服务，如果禁用则开启，务必让这几个服务处于启动状态。[按win+R，输入services.msc] 如下图将其重启即可；

2、如果你改成了静态ip别忘了将BOOTPROTO改为static【此处我没有修改，默认DHCP】（ps：余下可以去了解一下动态ip跟静态ip）

显示ok说明重启网络成功，ip也对应查询出来了

然后会发现问题解决啦

未完待续。。。