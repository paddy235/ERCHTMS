//$(function () {
var dlg = null;
var userid = "88888";
if (signalRUrl.length > 0) {
    var status = false;
    var st = null;
    var defaults = {
        url: signalRUrl //服务地址
    };
    var options = {};
    var options = $.extend(defaults, options);
    //Set the hubs URL for the connection
    $.connection.hub.url = options.url;
    $.connection.hub.qs = { "userId": userid };//传递参数(做为客户端接收标识)
    // Declare a proxy to reference the hub.
    var chat = $.connection.ChatsHub;
    //定义客户端方法供服务端调用。服务端向客户端推送消息
    try {

        chat.client.revMessage = function (str, msg, EqId) {
            $("#ContentStr").html(msg);

            BindUsserGps(msg);


        };

    } catch (ex) {

    }
   
    // 连接成功后注册服务器方法
    $.connection.hub.start().done(function () {
    
        chat.server.createGroup(userid);
        chat.server.sendMsg(userid, "");
       
        chat.server.printMsg(userid + "(" + userid + ")成功连接到SignalR服务器");
    });
  
   
    //断开连接后
    $.connection.hub.disconnected(function () {
           
    });
    //重连服务器
    $.connection.hub.reconnecting(function () {
       
       
    });
    //连接错误
    $.connection.hub.error(function () {
        
    });
    
   
}


   
//});
