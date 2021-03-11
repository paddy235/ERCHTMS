

//声明公用变量
var initCount = 0;
var pubKey = '';
//var width = 1000;
//var height = 600;


// 创建播放实例
function initPlugin() {
    oWebControl = new WebControl({
        szPluginContainer: "playWnd",                       // 指定容器id
        iServicePortStart: 15900,                           // 指定起止端口号，建议使用该值
        iServicePortEnd: 15909,
        szClassId: "23BF3B0A-2C56-4D97-9C03-0CB103AA8F11",   // 用于IE10使用ActiveX的clsid
        cbConnectSuccess: function () {                     // 创建WebControl实例成功
            oWebControl.JS_StartService("window", {         // WebControl实例创建成功后需要启动服务
                dllPath: "./VideoPluginConnect.dll"         // 值"./VideoPluginConnect.dll"写死
            }).then(function () {                           // 启动插件服务成功
                oWebControl.JS_SetWindowControlCallback({   // 设置消息回调
                    cbIntegrationCallBack: cbIntegrationCallBack
                });

                oWebControl.JS_CreateWnd("playWnd", width, height).then(function () { //JS_CreateWnd创建视频播放窗口，宽高可设定
                    init();  // 创建播放实例成功后初始化
                });
            }, function () { // 启动插件服务失败
            });
        },
        cbConnectError: function () { // 创建WebControl实例失败
            oWebControl = null;
            $("#playWnd").html("插件未启动，正在尝试启动，请稍候...");
            WebControl.JS_WakeUp("VideoWebPlugin://"); // 程序未启动时执行error函数，采用wakeup来启动程序
            initCount++;
            if (initCount < 3) {
                setTimeout(function () {
                    initPlugin();
                }, 3000)
            } else {
                $("#playWnd").html("插件启动失败，请检查插件是否安装！");
            }
        },
        cbConnectClose: function (bNormalClose) {
            // 异常断开：bNormalClose = false
            // JS_Disconnect正常断开：bNormalClose = true
            console.log("cbConnectClose");
            oWebControl = null;
        }
    });
}

// 设置窗口控制回调
function setCallbacks() {
    oWebControl.JS_SetWindowControlCallback({
        cbIntegrationCallBack: cbIntegrationCallBack
    });
}

// 推送消息
function cbIntegrationCallBack(oData) {
    showCBInfo(JSON.stringify(oData.responseMsg));
}

//初始化
function init() {
    getPubKey(function () {

        ////////////////////////////////// 请自行修改以下变量值	////////////////////////////////////
        var appkey = Hkappkey;                           //综合安防管理平台提供的appkey，必填
        var secret = setEncrypt(Hksecret);   //综合安防管理平台提供的secret，必填
        var ip = HikIp;                           //综合安防管理平台IP地址，必填
        var playMode = mode;                                  //初始播放模式：0-预览，1-回放
        var port = HikPort;                                    //综合安防管理平台端口，若启用HTTPS协议，默认443
        var snapDir = "D:\\SnapDir";                       //抓图存储路径
        var videoDir = "D:\\VideoDir";                     //紧急录像或录像剪辑存储路径
        var layout = "1x1";                                //playMode指定模式的布局
        var enableHTTPS = 1;                               //是否启用HTTPS协议与综合安防管理平台交互，这里总是填1
        var encryptedFields = 'secret';					   //加密字段，默认加密领域为secret
        var showToolbar = 1;                               //是否显示工具栏，0-不显示，非0-显示
        var showSmart = 1;                                 //是否显示智能信息（如配置移动侦测后画面上的线框），0-不显示，非0-显示
        var buttonIDs = "0,16,256,257,258,259,260,512,513,514,515,516,517,768,769";  //自定义工具条按钮
        ////////////////////////////////// 请自行修改以上变量值	////////////////////////////////////

        oWebControl.JS_RequestInterface({
            funcName: "init",
            argument: JSON.stringify({
                appkey: appkey,                            //API网关提供的appkey
                secret: secret,                            //API网关提供的secret
                ip: ip,                                    //API网关IP地址
                playMode: playMode,                        //播放模式（决定显示预览还是回放界面）
                port: port,                                //端口
                snapDir: snapDir,                          //抓图存储路径
                videoDir: videoDir,                        //紧急录像或录像剪辑存储路径
                layout: layout,                            //布局
                enableHTTPS: enableHTTPS,                  //是否启用HTTPS协议
                encryptedFields: encryptedFields,          //加密字段
                showToolbar: showToolbar,                  //是否显示工具栏
                showSmart: showSmart,                      //是否显示智能信息
                buttonIDs: buttonIDs                       //自定义工具条按钮
            })
        }).then(function (oData) {
            oWebControl.JS_Resize(width, height);  // 初始化后resize一次，规避firefox下首次显示窗口后插件窗口未与DIV窗口重合问题
        });
    });
}

//获取公钥
function getPubKey(callback) {
    oWebControl.JS_RequestInterface({
        funcName: "getRSAPubKey",
        argument: JSON.stringify({
            keyLength: 1024
        })
    }).then(function (oData) {
        console.log(oData);
        if (oData.responseMsg.data) {
            pubKey = oData.responseMsg.data;
            callback()
        }
    })
}

//RSA加密
function setEncrypt(value) {
    var encrypt = new JSEncrypt();
    encrypt.setPublicKey(pubKey);
    return encrypt.encrypt(value);
}

// 监听resize事件，使插件窗口尺寸跟随DIV窗口变化
$(window).resize(function () {
    if (oWebControl != null) {
        oWebControl.JS_Resize(width, height);
        setWndCover();
    }
});

// 监听滚动条scroll事件，使插件窗口跟随浏览器滚动而移动
$(window).scroll(function () {
    if (oWebControl != null) {
        oWebControl.JS_Resize(width, height);
        setWndCover();
    }
});


// 设置窗口裁剪，当因滚动条滚动导致窗口需要被遮住的情况下需要JS_CuttingPartWindow部分窗口
function setWndCover() {
    var iWidth = $(window).width();
    var iHeight = $(window).height();
    var oDivRect = $("#playWnd").get(0).getBoundingClientRect();

    var iCoverLeft = (oDivRect.left < 0) ? Math.abs(oDivRect.left) : 0;
    var iCoverTop = (oDivRect.top < 0) ? Math.abs(oDivRect.top) : 0;
    var iCoverRight = (oDivRect.right - iWidth > 0) ? Math.round(oDivRect.right - iWidth) : 0;
    var iCoverBottom = (oDivRect.bottom - iHeight > 0) ? Math.round(oDivRect.bottom - iHeight) : 0;

    iCoverLeft = (iCoverLeft > width) ? width : iCoverLeft;
    iCoverTop = (iCoverTop > height) ? height : iCoverTop;
    iCoverRight = (iCoverRight > width) ? width : iCoverRight;
    iCoverBottom = (iCoverBottom > height) ? height : iCoverBottom;

    oWebControl.JS_RepairPartWindow(0, 0, width + 1, height);    // 多1个像素点防止还原后边界缺失一个像素条
    if (iCoverLeft != 0) {
        oWebControl.JS_CuttingPartWindow(0, 0, iCoverLeft, height);
    }
    if (iCoverTop != 0) {
        oWebControl.JS_CuttingPartWindow(0, 0, width + 1, iCoverTop);    // 多剪掉一个像素条，防止出现剪掉一部分窗口后出现一个像素条
    }
    if (iCoverRight != 0) {
        oWebControl.JS_CuttingPartWindow(width - iCoverRight, 0, iCoverRight, height);
    }
    if (iCoverBottom != 0) {
        oWebControl.JS_CuttingPartWindow(0, height - iCoverBottom, width, iCoverBottom);
    }
}

//视频预览功能
function cameraplay() {
    var cameraIndexCode = KbscameraIndexCode;     //获取输入的监控点编号值，必填
    var streamMode = 0;                                     //主子码流标识：0-主码流，1-子码流
    var transMode = 1;                                      //传输协议：0-UDP，1-TCP
    var gpuMode = 0;                                        //是否启用GPU硬解，0-不启用，1-启用
    var wndId = -1;                                         //播放窗口序号（在2x2以上布局下可指定播放窗口）

    cameraIndexCode = cameraIndexCode.replace(/(^\s*)/g, "");
    cameraIndexCode = cameraIndexCode.replace(/(\s*$)/g, "");

    oWebControl.JS_RequestInterface({
        funcName: "startPreview",
        argument: JSON.stringify({
            cameraIndexCode: cameraIndexCode,                //监控点编号
            streamMode: streamMode,                         //主子码流标识
            transMode: transMode,                           //传输协议
            gpuMode: gpuMode,                               //是否开启GPU硬解
            wndId: wndId                                     //可指定播放窗口
        })
    })
};

function cameraReplay() {
    //if ($("#Starttime").val() == "" || $("#Endtime").val() == "") {
    //    dialogMsg('起止时间不能为空！', 0);
    //}
    var cameraIndexCode = KbscameraIndexCode;         //获取输入的监控点编号值，必填
    var datenow = new Date();//当前时间
    var startTimeStamp = Date.parse(new Date(datenow.getFullYear() + "-" + (datenow.getMonth() + 1) + "-" + datenow.getDate() + " 00:00:00"));    //回放开始时间戳，必填
    var endTimeStamp = Date.parse(new Date());  //回放结束时间戳，必填
    var recordLocation = RecordLocation;                                     //录像存储位置：0-中心存储，1-设备存储
    var transMode = 1;                                          //传输协议：0-UDP，1-TCP
    var gpuMode = 0;                                            //是否启用GPU硬解，0-不启用，1-启用
    var wndId = -1;                                             //播放窗口序号（在2x2以上布局下可指定播放窗口）

    oWebControl.JS_RequestInterface({
        funcName: "startPlayback",
        argument: JSON.stringify({
            cameraIndexCode: cameraIndexCode,                   //监控点编号
            startTimeStamp: Math.floor(startTimeStamp / 1000).toString(),  //录像查询开始时间戳，单位：秒
            endTimeStamp: Math.floor(endTimeStamp / 1000).toString(),      //录像结束开始时间戳，单位：秒
            recordLocation: recordLocation,                     //录像存储类型：0-中心存储，1-设备存储
            transMode: transMode,                               //传输协议：0-UDP，1-TCP
            gpuMode: gpuMode,                                   //是否启用GPU硬解，0-不启用，1-启用
            wndId: wndId                                         //可指定播放窗口
        })
    })
}

//停止全部预览
$("#stopAllPreview").click(function () {
    oWebControl.JS_RequestInterface({
        funcName: "stopAllPreview"
    });
});

// 标签关闭
$(window).unload(function () {
    if (oWebControl != null) {
        oWebControl.JS_HideWnd();   // 先让窗口隐藏，规避可能的插件窗口滞后于浏览器消失问题
        oWebControl.JS_Disconnect().then(function () {  // 断开与插件服务连接成功
        },
        function () {  // 断开与插件服务连接失败
        });
    }
});
