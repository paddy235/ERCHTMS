//$(function () {
var dlg = null;
if (signalRUrl.length > 0) {
    var status = false;
    var st = null;
    var defaults = {
        url: signalRUrl //服务地址
    };
    var clientId = uuid;
    var options = {};
    var options = $.extend(defaults, options);
    //Set the hubs URL for the connection
    $.connection.hub.url = options.url;
    $.connection.hub.qs = { "userId": clientId };//传递参数
    // Declare a proxy to reference the hub.
    var chat = $.connection.ChatsHub;
    //定义客户端方法供服务端调用。服务端向客户端推送消息
    try{
        chat.client.pushLogin = function (userId, msg) {
            if(msg.Remark=="1"){
                window.location.href = top.contentPath + "/login/SignIn?args=" + msg.Title;
            } else {
                top.layer.open({
                    type: 0,
                    title: "系统提示",
                    content: msg.Content,
                    //offset: "rb",
                    btn: null,
                    shade: 0,
                    time: 5000
                });
            }
          
        };
    }catch(ex){
        top.layer.open({
            type: 0,
            title: "系统提示",
            content: "无法连接到通信服务器",
            offset: "rb",
            btn: null,
            shade: 0,
            time: 5000
        });
       
    }
   
    // 连接成功后注册服务器方法
    $.connection.hub.start().done(function () {
        top.layer.open({
            type: 0,
            title: "系统提示",
            content: "成功连接到SignalR服务器",
            offset: "rb",
            btn: null,
            shade: 0,
            time: 5000
        });
        chat.server.createGroup(clientId);
        
    });
  
   
    //断开连接后
    $.connection.hub.disconnected(function () {
            top.layer.open({
                type: 0,
                title: "系统提示",
                content: "无法连接到服务器,稍后尝试重新连接",
                offset: "rb",
                btn: null,
                //closeBtn: 0,
                shade: 0,
                time: 10000,
                success: function (layerno, index) {
                    var lay = this;
                    status = true;
                    var seconds = 10;
                    st = setInterval(function () {
                        seconds--;
                        layer.title(seconds+"秒后重新连接", index);
                        if (seconds<=1) {
                            window.clearInterval(st);
                        }
                    }, 1000);
                },
                end: function () {
                    $.connection.hub.start().done(function () {
                        if(st!=null){
                            window.clearInterval(st);
                        }
                        status = true;
                        chat.server.createGroup(clientId);

                    });
                }
            });
    });
    //重连服务器
    $.connection.hub.reconnecting(function () {
       
        top.layer.open({
            type: 0,
            title: "系统提示",
            content: "已断开与通信服务器的连接,正在尝试重新连接服务器",
            offset: "rb",
            btn: null,
            shade: 0,
            time: 10000
        });
    });
    //连接错误
    $.connection.hub.error(function () {
        top.layer.open({
            type: 0,
            title: "系统提示",
            content: "无法连接到通信服务器",
            offset: "rb",
            btn: null,
            shade: 0,
            time: 5000
        });
    });
}
   
//});
