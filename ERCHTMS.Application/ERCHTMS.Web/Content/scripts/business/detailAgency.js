//======================
//详情页面代理类options
//{
//    keyValue: '',
//    actiontype:!'view',
//    formId: "form1",
//    currIfrmGridId: "gridTable",
//    pageControls: {
//        saveId: "btn_Save",
//        submitId: "btn_Submit",         
//        expId: "btn_Exp"
//    }
//    saveParam: {
//        url: '../../StandardSystem/StandardApply/SaveForm',
//        data: {},
//        loading: '正在保存数据...',
//        refresh: true,
//        successCallback: function (data) { },
//        completeCallback:function(){}
//    },
//    submitParam: {
//        url: '../../StandardSystem/StandardApply/SubmitForm',
//        data: {},     
//        loading: '正在保存数据...',
//        refresh: true,
//        successCallback: function (data) { },
//        completeCallback:function(){}
//    }
//}
//======================
function detailAgency(options) {    
    options = options || {};
    var agc = this;
    var pRules = [];//页面规则
    //
    //页面参数
    //
    var settings = {
        keyValue: '',
        actiontype:'',
        formId: "form1",
        currIfrmGridId: "gridTable",
        pageControls: {
            saveId: "btn_Save",
            submitId: "btn_Submit",         
            expId: "btn_Exp"
        }
    };
    //
    //保存参数
    //
    var saveParam = {
        data: {},
        url: '',
        loading: '正在保存数据...',
        refresh: true,
        successCallback: function (data) { },
        completeCallback:function(){}
    }
    //
    //提交参数
    //
    var submitParam = {
        data: {},
        url: '',
        loading: '正在保存数据...',
        refresh: true,
        successCallback:function(data){},
        completeCallback:function(){}
    }
    //
    //页面事件
    //
    var events = {
        onSave: function () { AcceptClick() },
        onSubmit: function () { SubmitAction() },
        onExp:function(){}
    };
    $.extend(settings, options);
    $.extend(saveParam, options.saveParam);
    $.extend(submitParam, options.submitParam);
    $.extend(events, options.events);
    settings.keyValue = !!settings.keyValue ? settings.keyValue : "";//newGuid()
    //====================
    //绑定页面控件ctrls
    //{saveId:'',submitId:'',expId:''}
    //====================
    this.bindPageControls = function (ctrls) {
        var it = this;
        $.extend(settings.pageControls, ctrls);
        var saveId = settings.pageControls.saveId;
        if (!!saveId) {//保存
            $("#" + saveId).click(function () {
                events.onSave();
            })
        }
        var submitId = settings.pageControls.submitId;
        if (!!submitId) {//提交
            $("#" + submitId).click(function () {
                events.onSubmit();
            })
        }
        var expId = settings.pageControls.expId;
        if (!!expId) {//导出
            $("#" + expId).click(function () {
                events.onExp();
            })
        }
    };
    //====================
    //绑定页面事件evts:
    //{onSave:fn,onSubmit:fn,onExp:fn}
    //====================
    this.addPageEvents = function (evts) {
        $.extend(events, evts);
    };
    //====================
    //更新参数设置
    //====================
    this.setParam = function (param) {
        param = param || {};
        $.extend(true, settings, param);
    }
    //====================
    //页面初始化args=
    //([
    //{
    //    url: '',
    //    param:{keyValue:keyValue},
    //    callBack:fn, 
    //    forms:[{Id:'',dataProName:''},{...}],
    //    ctrls:[{Id:'',memberId:'',memberText:'',description:'',dataProName:''},{...}],
    //    upfiles: [{ Id: '',recIdExp:'', extVal:'', extensions: '', actiontype: !'view',isImage:false,fileDir: '' },{...}],
    //},
    //{
    //    data:[],
    //    ctrl:{Id:'',memberId:'',memberText:'',description:''}
    //},
    //{...}
    //])
    //====================
    this.initialPage = function (args) {
        args = args || {};       
        //URL加载数据
        for (var i = 0; i < args.length; i++) {
            var arg = args[i];
            if (!!arg.data && !!arg.ctrl) {
                var ctrl = arg.ctrl;
                $("#" + ctrl.Id).bindCombobox(ctrl.memberId, ctrl.memberText, ctrl.description, arg.data);
            }            
            if (!!arg.url) {
                $.SetForm({
                    url: arg.url,
                    param: arg.param,
                    success: function (data) {
                        if (!!data) {
                            if (!!arg.ctrls) {//下拉框
                                for (var j = 0; j < arg.ctrls.length; j++) {                                    
                                    var ctrl = arg.ctrls[j];
                                    $("#" + ctrl.Id).bindCombobox(ctrl.memberId, ctrl.memberText, ctrl.description, eval(ctrl.dataProName));
                                }
                            }
                            if (!!arg.forms) {//form页面数据
                                for (var k = 0; k < arg.forms.length; k++) {                                    
                                    var form = arg.forms[k];
                                    if (!!form.extData)
                                        $("#" + form.Id).formDeserialize(form.extData);
                                    $("#" + form.Id).formDeserialize(eval(form.dataProName));
                                }
                            }                            
                            //回调
                            if (!!arg.callBack && typeof arg.callBack == "function") {
                                arg.callBack(data);
                            }
                        }
                        fileLoad(arg.upfiles,data);
                    }
                });
            }
            else {
                fileLoad(arg.upfiles);
                //回调
                if (!!arg.callBack && typeof arg.callBack == "function") {
                    arg.callBack();
                }
            }
        }
        if (settings.actiontype == "view") {
            var saveId = settings.pageControls.saveId;
            var submitId = settings.pageControls.submitId;
            var formId = settings.formId;
            var btn = $("#" + saveId + ",#" + submitId);
            btn.attr("disabled", "disabled");
            $("#" + formId).find("input,textarea,.ui-select,.ui-select-text,.ui-select-option-content").each(function (ele, index) {
                var it = $(this);
                it.attr("disabled", "disabled");
                it.attr("readonly", "readonly");
            });           
        }
    }
    //
    //加载文件
    //
    function fileLoad(upfiles,data) {
        if (!!upfiles) {//上传文件
            for (var l = 0; l < upfiles.length; l++) {
                var upfile = upfiles[l];
                var recId = "";
                if (!!upfile.recIdExp) {
                    recId = eval(upfile.recIdExp);
                    recId = !!recId ? recId : settings.keyValue;
                }
                else
                    recId = settings.keyValue;
                recId += (!!upfile.extVal ? upfile.extVal : "");
                if (!!recId) {
                    var isdelete = upfile.actiontype == "view" ? false : true;
                    if (upfile.actiontype != "view") {
                        var upOpt = {
                            keyValue: recId, el: '#' + upfile.Id
                        };
                        $.extend(upOpt, upfile);
                        file_upload.init(upOpt);
                    }
                    //绑定附件
                    $.ajax({
                        url: '../../PublicInfoManage/ResourceFile/GetFilesByRecId',
                        data: { recId: recId },
                        type: "post",
                        success: function (data) {
                            var objdata = eval("(" + data + ")"); //转化为对象类型
                            file_upload.bind(objdata, isdelete, !!upfile.isImage, recId, upfile.Id);
                        }
                    });
                }
            }
        }
    }
    //====================
    //添加保存参数data={pName1:'',pName2:''}
    //====================
    this.appendSaveData = function (data) {
        $.extend(saveParam.data, data);
        $.extend(submitParam.data, data);
    }
    //====================
    //添加页面规则
    //====================
    this.addRule = function (rule) {
        pRules.push(rule);
    }
    //====================
    //执行页面规则(ruleType=='page')
    //====================
    this.exePageRule = function () {       
        if (pRules.length > 0) {
            for (var i = 0; i < pRules.length; i++)
            {
                var p = pRules[i];
                if(p.ruleType=='page')
                    p.execute();
            }
        }
    }    
    //====================
    //执行验证规则(ruleType=='validate')
    //====================
    function exeValidateRule() {
        var r = true;

        if (pRules.length > 0) {
            for (var i = 0; i < pRules.length; i++) {
                var p = pRules[i];
                if (p.ruleType == 'validate')
                {
                    r = p.execute();
                    if (r == false) break;
                }
            }
        }

        return r;
    }
    //====================
    //绑定页面控件触发事件将要执行页面规则args
    //[
    //    {ctrlId:'',evtName:'',ruleId:''},
    //    {...}
    //]
    //====================
    this.bindTriggerRule = function (args) {
        for (var i = 0; i < args.length; i++) {
            var arg = args[i];
            if (!!arg.ctrlId && !!arg.ruleId) {
                var r = findRule(arg.ruleId);
                if (!!r) {
                    $("#" + arg.ctrlId).bind(arg.evtName, r.execute);
                }
            }
        }
    }
    //
    //根据id查找规则
    //
    function findRule(ruleId) {
        var r = null;

        for (var i = 0; i < pRules.length; i++) {
            var rule = pRules[i];
            if (rule.ruleId == ruleId) {
                r = rule;
                break;
            }
        }

        return r;
    }
    //
    //保存
    //
    AcceptClick = function () {
        var keyValue = settings.keyValue;
        var fmId = settings.formId;
        var gridId = settings.currIfrmGridId;
        var btnId = settings.pageControls.saveId;
        var url = saveParam.url;
        var refresh = saveParam.refresh;
        if (!!url) {
            $("#" + btnId).attr("disabled", "disabled");
            var saveOpt = {
                loading: "正在保存数据...",
                success: function (data) {
                    if (refresh == true) {
                        $.currentIframe().$("#" + gridId).trigger("reloadGrid");
                    }
                    $("#" + btnId).removeAttr("disabled");
                    if (saveParam.successCallback) {
                        saveParam.successCallback(data);
                    }
                },
                complete: function () {
                    $("#" + btnId).removeAttr("disabled");
                    if (saveParam.completeCallback) {
                        saveParam.completeCallback();
                    }
                }
            };
            $.extend(saveOpt, saveParam);
            var lnkChar = url.indexOf("?") < 0 ? "?" : "&";
            url += lnkChar + "keyValue=" + keyValue;
            if (!$("#" + fmId).Validform()||!exeValidateRule()) {
                $("#" + btnId).removeAttr("disabled");
                return false;
            }
            var postData = $("#" + fmId).formSerialize(keyValue);
            debugger;
            $.extend(postData, saveParam.data);
            $.extend(saveOpt, { url: url, param: postData });
            $.SaveForm(saveOpt);
        }
        else {
            dialogMsg('url参数无效！', 0);
        }
    }
    //
    //提交
    //
    SubmitAction = function () {
        var keyValue = settings.keyValue;
        var fmId = settings.formId;
        var gridId = settings.currIfrmGridId;
        var btnId = settings.pageControls.submitId;
        var url = submitParam.url;
        var refresh = submitParam.refresh;
        if (!!url) {                   
            $("#" + btnId).attr("disabled", "disabled");            
            var saveOpt = {
                loading: "正在保存数据...",
                success: function (data) {
                    if (refresh == true) {
                        $.currentIframe().$("#" + gridId).trigger("reloadGrid");
                    }                    
                    $("#" + btnId).removeAttr("disabled");
                    if (submitParam.successCallback) {
                        submitParam.successCallback(data);
                    }
                },
                complete: function () {
                    $("#" + btnId).removeAttr("disabled");
                    if (submitParam.completeCallback) {
                        submitParam.completeCallback();
                    }
                }
            };
            $.extend(saveOpt, submitParam);
            var lnkChar = url.indexOf("?") < 0 ? "?" : "&";
            url += lnkChar + "keyValue=" + keyValue;
            if (!$("#" + fmId).Validform() || !exeValidateRule()) {
                $("#" + btnId).removeAttr("disabled");
                return false;
            }
            var postData = $("#" + fmId).formSerialize(keyValue);
            $.extend(postData, submitParam.data);
            $.extend(saveOpt, {url: url,param: postData});
            $.SaveForm(saveOpt);
        }
        else {
            dialogMsg('url参数无效！', 0);
        }
    }
    //
    //新ID
    //
    function newId(len) {
        var l = !!len ? len : 36;
        var Id = "";
        var char = "0123456789ABCDEF";
        for (var i = 0; i < l; i++) {
            var index = Math.floor(Math.random() * char.length);
            Id += char.substr(index, 1);
        }

        return Id;
    }
}
//======================
//页面规则类{ruleId:'',ruleType:'',execCnd:null,execArgs:{},onExecute:fn}
//======================
function pageRule(ruleArgs) {
    //规则执行条件及执行事件
    var r = {
        ruleId: '',
        ruleType:'page',
        execCnd:null,
        execArgs: null,
        onExecute: null
    };
    $.extend(r, ruleArgs);
    this.ruleId = r.ruleId;
    this.ruleType = r.ruleType;
    //验证条件是否成立
    function validate() {
        var vr = true;
        if (!!r.execCnd)
            vr = eval(r.execCnd);
        return vr;
    }
    //规则成立，执行相应方法。
    this.execute = function (batch) {
        var rtn = true;

        if (validate() === true && typeof r.onExecute=="function") {
            rtn = r.onExecute(r.execArgs);
        }        

        return rtn;
    }
}