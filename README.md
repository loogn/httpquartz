# HttpQuartz

### 项目说明
基于Quartz.Net、Polly的Http远程调度系统，netcore3.1+mysql数据库。
在服务端有HttpJob提供http远程调度，客户端通过对trigger的操作来实现调度任务的管理，
通过trigger的JobData传输http请求的相关数据。


#### 功能介绍
- 添加任务
- 编辑任务
- 删除任务
- 暂停任务
- 恢复任务
- 日志查看
- API客户端
- IU管理


#### 服务端部署

1. 新建mysql数据库httpquartz  
2. 执行表结构和数据文件/src/HttpQuartz.Server/database/httpquartz.sql    
3. 修改appsettings.json和quartz.config中数据库连接信息  
4. 配置appsettings.json中SafeClients节点，使信任客户端可以使用HttpAPI 
5. 部署HttpQuartz.Server项目(asp.netcore 3.1项目)  
6. 用户名是：admin，密码是：123456  


#### API客户端

安装：
```c#
Install-Package HttpQuartz.Client -Version 0.0.1
```

使用：
```c#
    //实例化HttpClient，或者通过容器获取，并设置BaseAddress为HttpQuartz.Server的部署地址
    using var client = new HttpClient {BaseAddress = new Uri("http://localhost:5000")};
    var httpQuartzClient = new HttpQuartzClient(client);

    //添加新任务(添加触发器)，成功返回success字符串
    var result = await httpQuartzClient.ScheduleJob(new TriggerModel()
    {
        Key = new TriggerKeyModel("test", "test"),
        StartTime = DateTimeOffset.Now.AddSeconds(30),
        JobData = new JobDataInfo()
        {
            url = "http://www.baidu.com",
        },
        SimpleTrigger = new SimpleTriggerInfo()
        {
            RepeatInterval = TimeSpan.FromSeconds(10)
        },
    });
```

#### UI管理截图
![任务列表](https://gitee.com/loogn/httpquartz/raw/master/img/tasklist.png)

![编辑任务](https://gitee.com/loogn/httpquartz/raw/master/img/addtask.png)


#### 相关

[Quartz.Net项目](https://www.quartz-scheduler.net/)  
 
[Polly项目](http://www.thepollyproject.org/)

[LayUI项目](https://www.layui.com/)

