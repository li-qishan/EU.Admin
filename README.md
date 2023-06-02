
<div align="center"><h1>EU-Admin</h1></div>
<div align="center"><h3>简单设计，力求精简</h3></div>

## 前言

坐标苏州，2014年7月开始入行，学习.NET,最开始就学了webform，工作之后学了MVC、Sencha Touch（已抛弃技术）、JS、React、ReactNative、VUE，从.NetFramework 转到.NET CORE
从去年开始， 希望把前几年工作经历，以及想法做一个沉淀，故而有了这个项目。
新开的这个项目，期望实现这样的能力：业务人员只需关注实体的构建，业务服务的编写，以及路由的配置，未来的计划是所以基础代码一键生产。

让业务的开发，变成简单的三步走：创建实体 >> 业务开发 >> 路由配置。

后期还想做一个APP 基于RN

## 项目概述

前后端分离，使用 JWT 认证。

后端：基于 .NET6 和 EF Core，集成常用组件。

前端：基于Ant Design Pro，主技术栈：React、Ant Design

* 前端采用React 17、Ant Design Pro 5、TypeScript、umi3。
* 后端采用.NET6、Redis & Jwt。
* 权限认证使用Jwt，支持多终端认证系统。
* 支持加载动态权限菜单，多方式轻松权限控制。
* 高效率开发，使用代码生成器可以一键生成前后端代码。
* 数据库：SQL Server2014


## 内置功能(部分功能未实现)

1.  用户管理：用户是系统操作者，该功能主要完成系统用户配置。
2.  部门管理：配置系统组织机构（公司、部门、小组），树结构展现支持数据权限。
3.  岗位管理：配置系统用户所属担任职务。
4.  菜单管理：配置系统菜单，操作权限，按钮权限标识等。
5.  角色管理：角色菜单权限分配、设置角色按机构进行数据范围权限划分。
6.  字典管理：对系统中经常使用的一些较为固定的数据进行维护。
7.  参数管理：对系统动态配置常用参数。
8.  通知公告：系统通知公告信息发布维护。
9.  操作日志：系统正常操作日志记录和查询；系统异常信息日志记录和查询。
10. 登录日志：系统登录日志记录查询包含登录异常。
11. 在线用户：当前系统中活跃用户状态监控。
12. 定时任务：在线（添加、修改、删除)任务调度包含执行结果日志。
13. 代码生成：前后端代码的生成（html、xml、sql）支持CRUD下载 。
14. 系统接口：根据业务代码自动生成相关的api接口文档。
15. 服务监控：监视当前系统CPU、内存、磁盘、堆栈等相关信息。
16. 缓存监控：对系统的缓存信息查询，命令统计等。
17. 在线构建器：拖动表单元素生成相应的HTML代码。
18. 连接池监视：监视当前系统数据库连接池状态，可进行分析SQL找出系统性能瓶颈。

## 在线体验

http://124.221.9.198:8005/

管理员： Admin  密码：1

 
## 前端开发注意事项

Node：建议v16或以上

安装依赖请支行：yarn

正常启动请运行: npm run dev

Mock测试模式请运行: npm run start

发布打包请运行: npm run build

```bash
├─EU.React                    # 前端
├─doc                         # 项目文档
├─EU.Web                      # 后端
   ├─EU.Common                # 公共方法
   ├─EU.Core                  # 基础设施
   ├─EU.DataAccess            # 仓储层
   ├─EU.Model                 # 实体层
   ├─EU.TaskHelper            # 任务方法类
   └─EU.Web                   # 服务层/表现层
   └─EU.Web.BackgroundJobs    # 任务
   └─EU.WeixinService         # 微信接口（微信公众号、微信支付、企业微信）
```

## 相关技术文档

### TypeScript
https://www.tslang.cn/docs/home.html

### React Js
https://react.docschina.org/docs/getting-started.html

### Ant Design 
https://ant.design/components/overview-cn/

### Ant Design Pro
https://pro.ant.design/zh-CN/docs/overview

### Ant Design Chart
https://charts.ant.design/zh

### Umi Js
https://umijs.org/docs/introduce/introduce

### Senparc
https://github.com/JeffreySu/WeiXinMPSDK

https://sdk.weixin.senparc.com/

感谢这些优秀的开源项目！

## 部署

前端利用Nginx部署，后端是用IIS
（知道现在比较流行容器化部署，但是我还是很喜欢IIS,后期可以尝试使用docker部署）

## 系统能力

- 认证：集成Cookies、JWT；默认启用 JWT
- 授权：[基于策略（Policy）的授权](https://docs.microsoft.com/zh-cn/aspnet/core/security/authorization/policies?view=aspnetcore-6.0)
- ORM：[EF Core](https://docs.microsoft.com/zh-cn/ef/core/) 的 [Code First 模式](https://docs.microsoft.com/zh-cn/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)
- 依赖注入：默认 DI 容器，实现自动注入
- 缓存：[IDistributedCache](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.caching.distributed.idistributedcache)，默认注入 Memory Cache，可替换 Redis
- 日志：[NLog](https://nlog-project.org/)
- 事件总线：[默认启用 BackgroupService](https://docs.microsoft.com/zh-cn/dotnet/core/extensions/queue-service?source=recommendations)，基于[Channel](https://docs.microsoft.com/zh-cn/dotnet/api/system.threading.channels.channel-1) 实现的单机版发布订阅；可替换为 Redis 的发布订阅（可用于分布式）；也可替换为 RabbitMQ 的发布订阅（可用于分布式）
- 定时任务：Quartz
- 对象映射：AutoMapper

## 一些Q&A

#### 为什么把 IServices 这些接口层给干掉了，仅留下实现层？

答：一般项目中会如有 IRepository 和 IServices 这些个抽象层，主要是为了控制反转（IoC），实现项目各层之间解耦，最终目的就是为了“高内聚，低耦合”。

个人认为，对于单体项目来说，做到高内聚即可，再追求完全的低耦合，会增加成本和困扰（举个简单的栗子：项目初期，业务大改是常有的事，改服务类的接口的事并不少见。除非说业务主体明确，需要修改的，并不是业务的接口，而是业务的具体实现）。

最后是这个项目，本就是为了追求最简三层单体。

#### 为什么不对仓储额外封装一层？

答：简单的项目基本上是单数据库的，且 EF Core 已经实现了工作单元和仓储模式，可以不用再封装一层。

当然，个人还是建议跟ABP框架那样再封装一层仓储，可以避免一些后续的开发运维问题（比如：系统迁移、重构等）。
## 贡献

- 提 Issue 请到 github

## 联系我

如果这个项对您觉得还不错，可以和我一起努力。

邮箱：xiaochanghai@foxmail.com

也可以加我微信

![image-1e16e3f03796e42ff34239ccdfa509a](./doc/images/1e16e3f03796e42ff34239ccdfa509a.jpg)

部分内容来源与其他开源坐着，谢谢