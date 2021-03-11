﻿
//回车键
document.onkeydown = function (e) {
    if (!e) e = window.event;
    if ((e.keyCode || e.which) == 13) {
        $("#btnlogin").trigger("click");
    }
}
//生成二维码
function buildCode() {
    if (document.getElementById("imgQR") != undefined) {
        var guid = newGuid();
        guid = uuid + "_" + guid;
        $("#imgQR").attr("src", contentPath + "/Utility/BuilderImage?mode=1&keyValue=" + guid + "|扫码登录");
    }
   
}
//初始化
$(function () {
    buildCode();
    $('#imgrefresh').click(function () {
        buildCode();
    });
    $('.lg-title img').click(function () {
        var $this = $(this);
        var $p = $this.parents('.login_form');
        $p.hide().siblings('.login_form').show();
    });

    $('.lg-title img').hover(function () {
        var title = document.getElementById("divqr").style.display == "none" ? "账号登录" : "扫码登录";
        $("#title1").html(title);
        $('.lg-down').css("display", "none");
    }, function () {
        $('.lg-down').css("display", "none");
    });
    if (hasCode=="true") {
        $("#divCode").show();
    }
    if (!$.support.leadingWhitespace) {
        var html = "您使用的浏览器版本过低,为了达到最佳使用体验，推荐使用谷歌，火狐，360极速模式或IE8以上浏览器。<br />强烈建议您升级到最新版本。<a href='" + top.contentPath + "/Error/Browsers' target='_blank' style='text-decoration:underline;'>点击下载最新浏览器</a><br />";
        var idx = top.layer.open({
            id: "win",
            type: 0,
            title: '系统提示',
            fix: false,
            area: ['700px', '200px'],
            content: html,
            btn: ['关闭'],
            btns: 1,
            closeBtn: false,
            yes: function () {
                top.layer.close(idx);
            }
        });
    }
    //$(".wrap").css("margin-top", ($(window).height() - $(".wrap").height()) / 2 - 35);
    //$(window).resize(function (e) {
    //    $(".wrap").css("margin-top", ($(window).height() - $(".wrap").height()) / 2 - 35);
    //    e.stopPropagation();
    //});
    //错误提示
    if (top.$.cookie('login_error') != null) {
        switch (top.$.cookie('login_error')) {
            case "Overdue":
                top.window.location.href = window.location.href+"?Login_Error=1";
                formMessage('身份票据已失效,请重新登录');
                break;
            case "OnLine":
                top.window.location.href = window.location.href + "?Login_Error=2";
                formMessage('您的帐号已在其它地方登录,请重新登录');
                break;
            default:
                break;
        }
        top.$.cookie('login_error', '', { path: "/", expires: -1 });
    }
    if (window.location.href.indexOf("Login_Error=1") >= 0) {
        formMessage('身份票据已失效,请重新登录');
    }
    if (window.location.href.indexOf("Login_Error=2") >= 0) {
        formMessage('您的帐号已在其它地方登录,请重新登录。请注意保管你的密码'); 
    }
    //记住密码
    if (top.$.cookie('autologin') == 1) {
        $("#autologin").attr("checked", 'true');
        $("#username").val(top.$.cookie('username'));
        $("#password").val(top.$.cookie('password'));
       // CheckLogin(1);
    }
    //设置下次自动登录
    $("#autologin").click(function () {
        if ($(this).attr('checked')=="checked") {
            top.$.cookie('autologin', 1, { path: "/", expires: 7 });
        } else {
            top.$.cookie('autologin', '', { path: "/", expires: -1 });
            top.$.cookie('username', '', { path: "/", expires: -1 });
            top.$.cookie('password', '', { path: "/", expires: -1 });
        }
    });
    //主题风格
    var UItheme = top.$.cookie('UItheme')
    if (UItheme) {
        $("#UItheme").val(UItheme);
    }
    //登录按钮事件
    $("#btnlogin").click(function () {
        var $username = $("#username");
        var $password = $("#password");
        var $verifycode = $("#verifycode");
        if ($username.val() == "") {
            $username.focus();
            formMessage('请输入账户或手机号或邮箱。');
            return false;
        } else if ($password.val() == "") {
            $password.focus();
            formMessage('请输入密码。');
            return false;
        }
        else if ($verifycode.val() == "" && hasCode=="true") {
            $verifycode.focus();
            formMessage('请输入验证码。');
            return false;
        }
        else {
            var pwd1 = $.trim($("#password").val());
            if (pwd1.length > 16) {
                dialogAlert("密码长度不能大于16位！", 0);
                return false;
            }
            $("#btnlogin").attr('disabled', 'disabled');
            $("#btnlogin").val("正在登录……");
            CheckLogin(0);
        }
    })
    //点击切换验证码
    $("#login_verifycode").click(function () {
        $("#verifycode").val('');
        $("#login_verifycode").attr("src", top.contentPath + "/login/VerifyCode?time=" + Math.random());
    });
    //切换注册表单
    $("#a_register").click(function () {
        $(".login_tips").hide();
        $("#loginform").hide();
        $("#registerform").show();
        $("#register_verifycode").attr("src", top.contentPath + "/login/VerifyCode?time=" + Math.random());
        $(".wrap").css("margin-top", ($(window).height() - $(".wrap").height()) / 2 - 35);
    });
    //切换登录表单
    $("#a_login").click(function () {
        $(".login_tips").hide();
        $("#loginform").show();
        $("#registerform").hide();
        $("#login_verifycode").attr("src", top.contentPath + "/login/VerifyCode?time=" + Math.random());
        $(".wrap").css("margin-top", ($(window).height() - $(".wrap").height()) / 2 - 35);
    });
})
//登录验证
function CheckLogin(autologin) {
  
    var username = $.trim($("#username").val());
    var password = $.trim($("#password").val());
    var verifycode = $.trim($("#verifycode").val());
    var pwd1 = "";
    try {
        var str = CryptoJS.DES.encrypt(password, CryptoJS.enc.Utf8.parse(strKey), {
            iv: CryptoJS.enc.Utf8.parse(strIV),
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });
        pwd1 = str.ciphertext.toString();
    } catch(ex){
        pwd1 = $.trim($("#password").val());
    }
    var shapassword = SHA256(password);
    if (password.length<32) {
        password = $.md5(password);
    }
   
    $.ajax({
        url: contentPath+ "/Login/CheckLogin",
        data: { username: $.trim(username), password: $.trim(password), verifycode: verifycode, autologin: autologin, shapassword: shapassword, password1: pwd1 },
        type: "post",
        dataType: "json",
        async:false,
        success: function (data) {
            if (document.getElementById("autologin").checked) {
                top.$.cookie('autologin', 1, { path: "/", expires: 7 });
            } else {
                top.$.cookie('autologin', '', { path: "/", expires: -1 });
            }
            if (data.type == 1) {
                $("#btnlogin").removeAttr('disabled');
                $("#btnlogin").val("登录");
                if (data.resultdata == 0) {
                    dlg = dialogOpen({
                        id: 'UpdatePwd',
                        title: "修改密码",
                        url: "/PersonCenter/UpdatePwd",
                        width: "1000px",
                        height: "400px",
                        //btn: null,
                        //closeBtn: false,
                        callBack: function (iframeId) {
                            top.document.getElementById(iframeId).contentWindow.AcceptClick();
                        }
                    });
                    return false;
                } else {
                    if (top.$.cookie('autologin') == 1) {
                        top.$.cookie('username', $.trim(username), { path: "/", expires: 7 });
                        top.$.cookie('password', $.trim($("#password").val()), { path: "/", expires: 7 });
                    }
                    top.$.cookie('UItheme', $("#UItheme").val(), { path: "/", expires: 30 });
                    var theme = $("#UItheme").val();
                    if (theme == 1) {
                        window.location.href = contentPath + '/Home/AdminDefault';
                    }
                    else if (theme == 2) {
                        window.location.href = contentPath + '/Home/AdminLTE';
                    }
                    else if (theme == 3) {
                        window.location.href = contentPath + '/Home/AdminWindos';
                    }
                    else {
                        window.location.href = contentPath + '/Home/AdminPretty';
                    }
               }
            } else {
                if (data.message.length >= 30) {
                    dialogAlert(data.message, 0);
                } else {
                    formMessage(data.message);
                }
                $("#btnlogin").removeAttr('disabled');
                $("#btnlogin").val("登录");
                if(hasCode=="true"){
                    $("#login_verifycode").trigger("click");
                }
            }
        }
    });
}
//提示信息
function formMessage(msg, type) {
    //$('.login_tips').parents('dt').remove();
    //var _class = "login_tips";
    //if (type == 1) {
    //    _class = "login_tips-succeed";
    //}
    //$('.form').prepend('<dt><div class="' + _class + '"><i class="fa fa-exclamation-circle"></i>' + msg + '</div></dt>');
    dialogMsg(msg,2);
}



/*=========注册账户（begin）================================================================*/
$(function () {
    //手机号离开事件
    $("#txt_register_account").blur(function () {
        if ($(this).val() != "" && !isMobile($(this).val())) {
            $(this).focus();
            formMessage('手机格式不正确,请输入正确格式的手机号码。');
        }
        function isMobile(obj) {
            reg = /^(\+\d{2,3}\-)?\d{11}$/;
            if (!reg.test(obj)) {
                return false;
            } else {
                return true;
            }
        }
    });
    //密码输入事件
    $("#txt_register_password").keyup(function () {
        $(".passlevel").find('em').removeClass('bar');
        var length = $(this).val().length;
        if (length <= 8) {
            $(".passlevel").find('em:eq(0)').addClass('bar');
        } else if (length > 8 && length <= 12) {
            $(".passlevel").find('em:eq(0)').addClass('bar');
            $(".passlevel").find('em:eq(1)').addClass('bar');
        } else if (length > 12) {
            $(".passlevel").find('em').addClass('bar');
        }
    })
    //注册按钮事件
    $("#btnregister").click(function () {
        var $account = $("#txt_register_account");
        var $code = $("#txt_register_code");
        var $name = $("#txt_register_name");
        var $password = $("#txt_register_password");
        var $verifycode = $("#txt_register_verifycode");
        if ($account.val() == "") {
            $account.focus();
            formMessage('请输入手机号。');
            return false;
        } else if ($code.val() == "") {
            $code.focus();
            formMessage('请输入短信验证码。');
            return false;
        } else if ($name.val() == "") {
            $name.focus();
            formMessage('请输入姓名。');
            return false;
        } else if ($password.val() == "") {
            $password.focus();
            formMessage('请输入密码。');
            return false;
        } else if ($verifycode.val() == "") {
            $verifycode.focus();
            formMessage('请输入图片验证码。');
            return false;
        } else {
            $("#btnregister").addClass('active').attr('disabled', 'disabled');
            $("#btnregister").find('span').hide();
            $.ajax({
                url: contentPath + "/Login/Register",
                data: { mobileCode: $account.val(), securityCode: $code.val(), fullName: $name.val(), password: $.md5($password.val()), verifycode: $verifycode.val() },
                type: "post",
                dataType: "json",
                success: function (data) {
                    if (data.type == 1) {
                        alert('注册成功');
                        window.location.href = contentPath + '/Login/Index';
                    } else {
                        formMessage(data.message);
                        $("#btnregister").removeClass('active').removeAttr('disabled');
                        $("#btnregister").find('span').show();
                        $("#register_verifycode").trigger("click");
                    }
                }
            });
        }
    })
    //获取验证码
    $("#register_getcode").click(function () {
        var $this = $(this);
        $this.attr('disabled', 'disabled');
        $.ajax({
            url: contentPath + "/Login/GetSecurityCode",
            data: { mobileCode: $("#txt_register_account").val() },
            type: "get",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.type == 1) {
                    formMessage('已向您的手机' + $("#txt_register_account").val() + '发送了一条验证短信。', 1);
                } else {
                    $this.removeAttr('disabled');
                    formMessage(data.message);
                }
            }
        });
    });
})
/*=========注册账户（end）================================================================*/