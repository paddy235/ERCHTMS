layui.use('layim', function (layim) {
    $.connection.hub.url = signalRUrl+"/hubs";
    $.connection.hub.qs = { "userId": uId };
    var chat = $.connection.ChatsHub;
    chat.client.RevMessage = function (userId, message, dateTime, username, toUser) {
        if (uId != userId) {
            var obj = {
                username: username
              , avatar: ""
              , id: userId
              , type: "friend"
              , content: message
            }
            layim.getMessage(obj);
        }
    };
    chat.client.RevGroupMessage = function (userId, toGroup, message, dateTime, username) {
        if (uId != userId) {
            var obj = {
                username: username
              , avatar: ""
              , id: toGroup
              , type: "group"
              , content: message
            }
            layim.getMessage(obj);
        }
    };
    chat.client.UpdateUserStatus = function (userId, isOnLine) {
        var li = $(".layim-list-friend").find("li[class='layim-friend" + userId + " ']");
        if(li!=undefined){
            var src = li.find("img").attr("src"); 
            li.find("img").attr("src", src.replace("off-line", "on-line"));
        }
    };
    chat.client.GetUserList = function (res) {
        var json = eval("(" + res + ")");
        //基础配置
        layim.config({
                   
            //初始化接口
            //init: {
            //    url: 'json/getList.json'
            //  , data: {}
            //}
            //或采用以下方式初始化接口
            init: {
                mine: json.data.mine
              , friend: json.data.friend
              , group: json.data.group
            }


            //查看群员接口
          , members: {
              url: apiUrl+'/api/webIM/getmembers'
            , data: { ownerId: uId }
          }

            //上传图片接口
          , uploadImage: {
              url: apiUrl+'/api/webIM/UploadFile' //（返回的数据格式见下文）
            , type: 'post' //默认post
          }

            //上传文件接口
          , uploadFile: {
              url: apiUrl+'/api/webIM/UploadFile' //（返回的数据格式见下文）
            , type: 'post' //默认post
          }

            //扩展工具栏
          , tool: [{
              alias: 'code'
            , title: '代码'
            , icon: '&#xe64e;'
          }, {
              alias: 'screen'
            , title: '截图'
            , icon: '<i class="fa fa-cut"></i>'
          }]

            //,brief: true //是否简约模式（若开启则不显示主面板）
            ,title: 'WebIM' //自定义主面板最小化时的标题
            //,right: '100px' //主面板相对浏览器右侧距离
            //,minRight: '90px' //聊天面板最小化时相对浏览器右侧距离
          , initSkin: '2.jpg' //1-5 设置初始背景
            //,skin: ['aaa.jpg'] //新增皮肤
            //,isfriend: false //是否开启好友
            //,isgroup: false //是否开启群组
            //,min: true //是否始终最小化主面板，默认false
          , notice: true //是否开启桌面消息提醒，默认false
            //,voice: false //声音提醒，默认开启，声音文件为：default.wav

          , msgbox: layui.cache.dir + 'css/modules/layim/html/msgbox.html' //消息盒子页面地址，若不开启，剔除该项即可
          , find: layui.cache.dir + 'css/modules/layim/html/find.html' //发现页面地址，若不开启，剔除该项即可
          , chatLog: layui.cache.dir + 'css/modules/layim/html/chatLog.html' //聊天记录页面地址，若不开启，剔除该项即可

        });
    }
    // 连接成功后注册服务器方法
    $.connection.hub.start().done(function () {
        //var res = chat.server.getUserList();
        console.log("成功连接到服务器");
    });
    //断开连接后
    $.connection.hub.disconnected(function () {
        console.log("无连接到服务器");
        $(".layui-layim-min").hide();
        $(".layui-layim").hide();
        $(".layui-layim-chat").hide();

       
    });
    //重新尝试连接服务器
    $.connection.hub.reconnecting(function () {
        console.log("正在尝试重新连接服务器");
    });
    //已重新连接到服务器
    $.connection.hub.reconnected(function () {
        console.log("已重新成功连接到服务器");
        $(".layui-layim-min").show();
    });
    //监听在线状态的切换事件
    layim.on('online', function (data) {
       
    });

    //监听签名修改
    layim.on('sign', function (value) {
       
    });
    //查看群员
    layim.on("members", function (data) {
        
    });
    //监听自定义工具栏点击，以添加代码为例
    layim.on('tool(code)', function (insert) {
        layer.prompt({
            title: '插入代码'
          , formType: 2
          , shade: 0
        }, function (text, index) {
            layer.close(index);
            insert('[pre class=layui-code]' + text + '[/pre]'); //将内容插入到编辑器
        });
    });
    layim.on('tool(screen)', function (insert) {
        chatObj.insert = function (msg) {
            insert(msg);
        }
        StartCapture();
    });
    //监听layim建立就绪
    layim.on('ready', function (res) {   
        Init();
    });

    //监听发送消息
    layim.on('sendMessage', function (data) {
        var To = toUser = data.to;
        //console.log(data);

        if (To.type === 'friend') {
            layim.setChatStatus('<span style="color:#FF5722;">对方正在输入。。。</span>');
            chat.server.imSendToOne(To.id, data.mine.content);
            layim.setChatStatus('<span style="color:#FF5722;">在线</span>');
        }
        if (To.type === 'group') {
            chat.server.imSendToGroup(To.id, data.mine.content);
        }
    });

    //监听聊天窗口的切换
    layim.on('chatChange', function (res) {
        var type = res.data.type;
        console.log(res.data.id)
        if (type === 'friend') {
            //模拟标注好友状态
            //layim.setChatStatus('<span style="color:#FF5722;">在线</span>');
        } else if (type === 'group') {
            //模拟系统消息
            //layim.getMessage({
            //  system: true
            //  ,id: res.data.id
            //  ,type: "group"
            //  ,content: '模拟群员'+(Math.random()*100|0) + '加入群聊'
            //});
        }
    });

          

});