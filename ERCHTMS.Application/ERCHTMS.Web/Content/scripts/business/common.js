
//打开新页面
function OpenNewTab(title, url, id) {
    top.tablist.newTab({
        url: top.contentPath + url,
        title: title,
        closed: true,
        id: id
    });
}
//安全风险管理获取配置内容
function GetData(RiskType, ConfigType, ItemType, DivId, DataType) {
    $.ajax({
        url: "../../RiskDataBaseConfig/Riskdatabaseconfig/GetDataByType",
        type: "GET",
        data: { RiskType: RiskType, ConfigType: ConfigType, DataType: DataType, ItemType: ItemType },
        async: false,
        dataType: "json",
        success: function (result) {
            if (result.length > 0) {
                $("#" + DivId + "").append(result[0].configcontent);
            }
        }
    });
}
//获取版本标记
function GetVersion() {
    var IsGdxy = false;
    $.ajax({
        url: "../../SystemManage/DataItemDetail/GetDataItemListJson",
        type: "GET",
        data: { EnCode: "VManager" },
        async: false,
        dataType: "json",
        success: function (result) {
            if (result.length > 0) {
                IsGdxy = true;
            } else {
                IsGdxy = false;
            }
        }
    });
    return IsGdxy;
}
var dlg;
//获取当前用户的签名图片
function getUserSignPic(currUserId, callframeid) {

    var signImg = "";
    $.ajax({
        url: "../../BaseManage/User/GetEntity?keyValue=" + currUserId,
        dataType: "JSON",
        async: false,
        success: function (result) {
            if (result != null) {
                signImg = result.SignImg;
            }
        }
    });
    //$.SetForm({
    //    url: "../../BaseManage/User/GetEntity?keyValue=" + currUserId,
    //    success: function (data) {
    //        if (data != null) {
    //            signImg = data.SignImg;
    //        }
    //    }
    //})
    if (!!signImg) {
        return signImg;
    }
    else {
        var idx = dialogConfirm("请先上传签名图片？", function (isSure) {
            if (isSure) {
                dlg = dialogOpen({
                    id: 'SetForm',
                    title: '个人设置',
                    url: '/PersonCenter/Index?mode=0&callframeid=' + callframeid + "&currUserId=" + currUserId,
                    width: "800px",
                    height: "800px",
                    btn: null
                })
                top.layer.close(idx);
            } else {
                return signImg;
            }
        });
    }
}
//外包流程配置
function OutConfigAjax(rolename) {
    var value;
    if (rolename.indexOf("省级") >= 0 || rolename.indexOf("集团") >= 0) {
        value = -1;
    } else {
        $.ajax({
            url: top.contentPath + '/OutsourcingProject/Outprocessconfig/IsConfigExist',
            dataType: "JSON",
            async: false,
            success: function (result) {
                value = result;
            }
        });
    }
    if (value > 0 || value == -1) {

    } else {
        layer.open({
            id: "win1",
            type: 0,
            title: "系统提醒",
            fix: false,
            closeBtn: false,
            shadeClose: false,
            area: ['400px', '200px'],
            content: "<div style='text-align: center;font-size: 24px; vertical-align: middle; line-height: 100px;'>请联系管理员配置外包流程！</div>",
            btn: null
        });
    }
}

//Ajax公共方法
function Ajax(value) {
    $.ajax({
        url: '../../SaftProductTargetManage/SafeProductDutyBook/GetFiles',
        data: { fileId: value },
        dataType: "JSON",
        async: false,
        success: function (result) {
            value = result;
        }
    });
    return value;
}

function AjaxCommon(url) {
    var value;
    $.ajax({
        url: url,
        dataType: "JSON",
        async: false,
        success: function (result) {
            value = result;
        }
    });
    return value;
}
//获取附件验证是否存在附件
function GetFile(value) {
    $.ajax({
        url: '../../SaftProductTargetManage/SafeProductDutyBook/GetFiles',
        data: { fileId: value },
        dataType: "JSON",
        async: false,
        success: function (result) {
            if (result == "" || result == undefined || result == null) {
                value = "";
            }
        }
    });
    return value;
}
//绑定地区
function ComboxArea(id, url) {
    $("#" + id).ComboBoxTree({
        //url: ?orgID=" + value,
        url: url,
        description: "==请选择==",
        height: "180px",
        allowSearch: true
    });
}

//验证是查看还是编辑
function ShowOrEdit(action, uploaderId, filesId) {
    //调查报告附件
    if (action == "show") {
        bindUpload(uploaderId, filesId);
    }
    else {
        file_upload.init({
            el: '#' + uploaderId, keyValue: filesId, extensions: 'doc,docx,xls,xlsx,zip,rar,jpg,png,gif,bmp,txt,ppt,pptx,pdf,mp3,mp4,avi', isImage: false, fileNumLimit: 100
        });
        file_upload.bindFiles(true, false, filesId, uploaderId, true);
    }
}


function bindUpload(uploaderId, filesId) {
    $.ajax({
        url: '../../SaftProductTargetManage/SafeProductDutyBook/GetFiles',
        data: { fileId: filesId },
        dataType: "JSON",
        async: false,
        success: function (result) {
            file_upload.bind(result, false, false, filesId, uploaderId);
        }
    });

}


////绑定编码值
function ComBoxForData(elId, enCode) {
    //应急预案类型
    $("#" + elId).ComboBox({
        url: "../../SystemManage/DataItemDetail/GetDataItemListJson",
        param: { EnCode: enCode },
        id: "ItemValue",
        text: "ItemName",
        description: "==请选择==",
        height: "150px",
        allowSearch: true
    });
}





/// <summary>
/// 选择部门 
/// </summary>
///<param name="deptId">查询条件，根据mode的值查询方式会不同</param>
/// <param name="checkMode">单选或多选，0:单选，1:多选</param>
/// <param name="mode">查询模式，0:获取机构ID为Ids下所有的部门(即EnCode=Ids)，1:获取部门ParentId为Ids下所有部门(即ParentId=Ids)，2:获取部门的parentId在Ids中的部门(即EnCode in(Ids))，
///   3:获取部门Id在Ids中的部门(即DepartmentId in(Ids)) 4.获取承包商和分包商 5.获取本部门及本部门以下的部门,以及发包部门为此部门的承包商,10获取本部门及本部门以下的数据，不包含所管辖的承包商 
///11.获取本机构的数据,不包含承包商</param>
///<param name="title">弹出层标题</param>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID，多个用逗号分隔，顺序依次为部门名称,部门Code，部门Id,部门负责人姓名,部门负责人ID（多选用逗号分隔）</param>
///<param name="callback">回调函数</param>
///<param name="showDb"></param>
///<param name="type">特殊参数处理</param>
function selectDept(deptId, checkMode, mode, title, winObject, domId, deptIds, callback, showDb, type) {
    return dialogOpen({
        id: "Dept",
        title: title,
        url: '/BaseManage/Department/Select?deptId=' + deptId + "&checkMode=" + checkMode + "&mode=" + mode + "&deptIds=" + deptIds + "&showDb=" + showDb + "&type=" + type,
        width: "700px",
        height: "500px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId, window, callback);
        }
    });
}


/// <summary>
/// 选择模块
/// </summary>
//参数说明参考选择部门
function selectModule(moduleId, checkMode, mode, title, winObject, domId, moduleIds, callback) {

    return dialogOpen({
        id: "Module",
        title: title,
        url: '/AuthorizeManage/ModuleListColumnAuth/Select?moduleId=' + moduleId + "&checkMode=" + checkMode + "&mode=" + mode + "&moduleIds=" + moduleIds,
        width: "700px",
        height: "500px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId, window, callback);
        }
    });
}

function selectDeptByArgs(deptId, checkMode, mode, title, winObject, domId, action, callback) {
    return dialogOpen({
        id: "Dept",
        title: title,
        url: '/BaseManage/Department/Select?deptId=' + deptId + "&checkMode=" + checkMode + "&mode=" + mode,
        width: "700px",
        height: "500px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.PostSubmit(winObject, domId, callback);
        }
    });
}
/// <summary>
/// 选择用户
/// </summary>
///<param name="deptId">查询条件，根据mode的值查询方式会不同</param>
/// <param name="checkMode">单选或多选，0:单选，1:多选</param>
/// <param name="mode">查询模式，0:获取机构ID为Ids下所有的部门(即OrganizeId=Ids)，1:获取部门ParentId为Ids下所有部门(即ParentId=Ids)，2:获取部门的parentId在Ids中的部门(即ParentId in(Ids))，3:获取部门Id在Ids中的部门(即DepartmentId in(Ids))</param>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID，多个用逗号分隔，顺序依次为用户名称,用户账号，用户Id（多选用逗号分隔）</param>
///<param name="">
///<param name="userIds">用户页面带过来的用户ids</param>
function selectUser(options) {
    var deptCode = options.deptCode == undefined ? "" : options.deptCode;
    var url = '/BaseManage/User/Select?deptId=' + options.deptId + "&checkMode=" + options.checkMode + "&mode=" + options.mode + "&deptCode=" + deptCode + "&userIds=" + options.userIds;
    url = options.userKind == undefined ? url : url + "&userKind=" + options.userKind;//userKind：人员类别，1：特种作业人员，2：特种设备操作人员
    url = options.eliminateUserIds == undefined ? url : url + "&eliminateUserIds=" + options.eliminateUserIds;//eliminateUserIds:排除不能选择的人员
    url = options.special == undefined ? url : url + "&special=" + options.special;//special:旁站监督员的特殊处理[排除选过的人]
    url = options.side == undefined ? url : url + "&side=" + options.side;//special:旁站监督员的特殊处理[只要旁站员]
    url = options.rolename == undefined ? url : url + "&rolename=" + options.rolename;//rolename:开工申请特殊处理
    url = options.projectid == undefined ? url : url + "&projectid=" + options.projectid;//rolename:开工申请特殊处理
    url = options.threeperson == undefined ? url : url + "&threeperson=" + options.threeperson; //三种人
    return dialogOpen({
        id: "User",
        title: "选择用户",
        url: url,
        width: ($(top.window).width() - 10) + "px",
        height: ($(top.window).height()) + "px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(options);
        }
    });
}

/// <summary>
/// 选择用户(不包含转岗中人员)
/// </summary>
///<param name="deptId">查询条件，根据mode的值查询方式会不同</param>
/// <param name="checkMode">单选或多选，0:单选，1:多选</param>
/// <param name="mode">查询模式，0:获取机构ID为Ids下所有的部门(即OrganizeId=Ids)，1:获取部门ParentId为Ids下所有部门(即ParentId=Ids)，2:获取部门的parentId在Ids中的部门(即ParentId in(Ids))，3:获取部门Id在Ids中的部门(即DepartmentId in(Ids))</param>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID，多个用逗号分隔，顺序依次为用户名称,用户账号，用户Id（多选用逗号分隔）</param>
///<param name="">
///<param name="userIds">用户页面带过来的用户ids</param>
function NoTransferselectUser(options) {
    var deptCode = options.deptCode == undefined ? "" : options.deptCode;
    var url = '/BaseManage/User/NoTransferSelect?deptId=' + options.deptId + "&checkMode=" + options.checkMode + "&mode=" + options.mode + "&deptCode=" + deptCode + "&userIds=" + options.userIds;
    url = options.userKind == undefined ? url : url + "&userKind=" + options.userKind;//userKind：人员类别，1：特种作业人员，2：特种设备操作人员
    url = options.eliminateUserIds == undefined ? url : url + "&eliminateUserIds=" + options.eliminateUserIds;//eliminateUserIds:排除不能选择的人员
    url = options.special == undefined ? url : url + "&special=" + options.special;//special:旁站监督员的特殊处理[排除选过的人]
    url = options.side == undefined ? url : url + "&side=" + options.side;//special:旁站监督员的特殊处理[只要旁站员]
    url = options.rolename == undefined ? url : url + "&rolename=" + options.rolename;//rolename:开工申请特殊处理
    url = options.projectid == undefined ? url : url + "&projectid=" + options.projectid;//rolename:开工申请特殊处理
    return dialogOpen({
        id: "User",
        title: "选择用户",
        url: url,
        width: ($(top.window).width() - 100) + "px",
        height: ($(top.window).height()) + "px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(options);
        }
    });
}


//选择不同用户
function selectSpecialDiffUser(deptcode, ckmode, mode, winObject, domId, dfferentID, describe) {
    var checkMode = ckmode;
    var chooseDiff = "";
    if (ckmode == 2) {
        checkMode = 1;
        chooseDiff = "&chooseDiff=1";
    }
    if (ckmode == 3) {
        checkMode = 0;
        chooseDiff = "&chooseDiff=1";
    }
    //问题流程专用
    if (ckmode == 4) {
        checkMode = 1; //多选
        chooseDiff = "&chooseDiff=2";
    }
    return dialogOpen({
        id: "User",
        title: "选择用户",
        url: "/BaseManage/User/Select?departmentCode=" + deptcode + "&checkMode=" + checkMode + "&mode=" + mode + chooseDiff,
        width: ($(top.window).width() - 100) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptDifferentClick(winObject, domId, dfferentID, describe);
        }
    });
}



//选择不同用户
function selectDifferentUser(deptId, ckmode, mode, winObject, domId, dfferentID, describe, pfrom) {
    var checkMode = ckmode;
    var chooseDiff = "";
    if (ckmode == 2) {
        checkMode = 1;
        chooseDiff = "&chooseDiff=1";
    }
    if (ckmode == 3) {
        checkMode = 0;
        chooseDiff = "&chooseDiff=1";
    }
    //问题流程专用
    if (ckmode == 4) {
        checkMode = 1; //多选
        chooseDiff = "&chooseDiff=2";
    }
    return dialogOpen({
        id: "User",
        title: "选择用户",
        url: "/BaseManage/User/Select?deptId=" + deptId + "&checkMode=" + checkMode + "&mode=" + mode + chooseDiff + "&pfrom=" + pfrom,
        width: ($(top.window).width() - 100) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptDifferentClick(winObject, domId, dfferentID, describe);
        }
    });
}

//选择用户并进行多个Dom赋值
/// <param name="checkMode">单选或多选，0:单选，1:多选</param>
function selectMuchUser(options) {
    //deptId, checkMode, mode, winObject, domId
    var deptId = options.deptId;
    var checkMode = options.checkMode;
    var mode = options.mode;
    var winObject = options.winObject;
    var domId = options.domId;
    var actionArgs = "";
    var controlObj = "";
    var pfrom = options.pfrom;
    var containdept = "";
    if (!!options.containdept)
    {
        containdept = options.containdept;
    }
    var istree = "1";
    if (!!options.istree) {
        istree = options.istree;
    }
    if (!!options.controlObj) {
        controlObj = options.controlObj; //
    }
    var clearObj = "";
    if (!!options.clearObj) {
        clearObj = options.clearObj; //
    }
    return dialogOpen({
        id: "User",
        title: "选择用户",
        url: '/BaseManage/User/Select?deptId=' + deptId + "&checkMode=" + checkMode + "&mode=" + mode + "&userIds=" + options.userIds + "&rolename=" + options.rolename + "&controlObj=" + controlObj + "&clearObj=" + clearObj + "&pfrom=" + pfrom + "&containdept=" + containdept + "&istree=" + istree,
        width: ($(top.window).width() - 100) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptMuchClick(options);
        }
    });
}




/// <summary>
/// 选择角色
/// </summary>
///<param name="deptId">查询条件，根据mode的值查询方式会不同</param>
/// <param name="checkMode">单选或多选，0:单选，1:多选</param>
/// <param name="mode">查询模式，0:获取机构ID为Ids下所有的部门(即OrganizeId=Ids)，1:获取部门ParentId为Ids下所有部门(即ParentId=Ids)，2:获取部门的parentId在Ids中的部门(即ParentId in(Ids))，3:获取部门Id在Ids中的部门(即DepartmentId in(Ids))</param>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID，多个用逗号分隔，顺序依次为用户名称,用户账号，用户Id（多选用逗号分隔）</param>
function selectRole(roleIDs, deptId, checkMode, mode, winObject, domId, callback) {
    return dialogOpen({
        id: "Role",
        title: "选择角色",
        url: '/BaseManage/Role/Select?roleIDs=' + roleIDs + '&deptId=' + deptId + "&checkMode=" + checkMode + "&mode=" + mode,
        width: "250px",
        height: "500px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId, window, callback);
        }
    });
}
/// <summary>
/// 选择隐患排查标准
/// </summary>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID</param>
function selectStandard(winObject, domId) {
    return dialogOpen({
        id: "Standard",
        title: "选择隐患描述",
        url: '/RiskDatabase/HtStandard/Select',
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId);
        }
    });
}

//选择隐患排查标准
function selectHidStandard(options) {
    var winObject = options.winObject;
    var domId = options.domId;
    return dialogOpen({
        id: "HtStandard",
        title: "选择隐患标准",
        url: '/RiskDatabase/HtStandard/Select?action=hid',
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(options);
        }
    });
}



//选择工程项目
function selectProject(winObject, domId) {
    return dialogOpen({
        id: "Project",
        title: "选择工程项目",
        url: '/BaseManage/Project/Select',
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId);
        }
    });
}


//选择工程项目
function selectSpecialProject(winObject, domId, controlObj) {
    return dialogOpen({
        id: "Project",
        title: "选择工程项目",
        url: '/BaseManage/Project/Select?controlObj=' + controlObj,
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId);
        }
    });
}


//选择工程项目
function sjSelectProject(winObject, domId, orgId) {
    return dialogOpen({
        id: "Project",
        title: "选择工程项目",
        url: '/BaseManage/Project/Select?orgid=' + orgId,
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId);
        }
    });
}

//选择工程项目
function SelectOutProject(winObject, domId, deptid, callback, showtype) {
    return dialogOpen({
        id: "Project",
        title: "选择工程项目",
        url: '/BaseManage/Project/Select?deptid=' + deptid + "&showtype=" + showtype,
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId, callback);
        }
    });
}



/// <summary>
/// 选择区域 
/// </summary>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID</param>
/// <param name="checkMode">单选或多选，0:单选，1:多选</param>
function selectArea(winObject, domId, checkMode) {
    return dialogOpen({
        id: "Area",
        title: "选择区域",
        url: '/BaseManage/District/Select?mode=' + checkMode,
        width: ($(top.window).width() - 200) + "px",
        height: "500px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId);
        }
    });
}

function selectMulArea(winObject, domId, areaIds, callback) {
    return dialogOpen({
        id: "Area",
        title: "选择区域",
        url: '/BaseManage/District/MulSelect?areaIds=' + areaIds,
        width: "600px",
        height: "500px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId, callback);
        }
    });
}
//省级选择区域
function sjSelectArea(winObject, domId, objId, checkMode) {
    return dialogOpen({
        id: "Area",
        title: "选择区域",
        url: '/BaseManage/District/Select?mode=' + checkMode + '&objId=' + objId,
        width: ($(top.window).width() - 200) + "px",
        height: "500px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId);
        }
    });
}


/// <summary>
/// 选择设备
/// </summary>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID</param>
/// <param name="checkMode">单选或多选，0:单选，1:多选</param>
function selectRemoteEqu(winObject, domId, checkMode) { 
    return dialogOpen({
        id: "Equipment",
        title: "选择设备",
        url: '/EquipmentManage/SpecialEquipment/RemoteSelect?checkMode=' + checkMode,
        width: ($(top.window).width()/2 -200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject,domId);
        }
    });
}
/// <summary>
/// 选择设备
/// </summary>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID</param>
/// <param name="checkMode">单选或多选，0:单选，1:多选</param>
function selectEquipment(winObject, domId, checkMode) {
    return dialogOpen({
        id: "Equipment",
        title: "选择设备",
        url: '/EquipmentManage/SpecialEquipment/Select?checkMode=' + checkMode,
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick({ winObject: winObject, domId: domId });
            if ($("#AREAID") != undefined) {
                $("#AREAID").ComboBoxSetValue($("#CertificateNo").val());

            }
        }
    });
}
/// <summary>
/// 选择普通设备
/// </summary>
function selectEquipmentNormal(winObject, domId, checkMode) {
    return dialogOpen({
        id: "Equipment",
        title: "选择设备",
        url: '/EquipmentManage/Equipment/Select?checkMode=' + checkMode,
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick({ "winObject": winObject, "domId": domId });
        }
    });
}

/// <summary>
/// 危险化学品实际存在量q
/// </summary>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID</param>
/// <param name="checkMode">单选或多选，0:单选，1:多选</param>
function selectq(winObject, domId, checkMode, qValue) {
    return dialogOpen({
        id: "sq",
        title: "危险化学品实际存在量q",
        url: '/HazardsourceManage/Hazardsource/Selectq?checkMode=' + checkMode + "&qValue=" + qValue,
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId);
        }
    });
}

function selectCommon(winObject, url, title, domId) {
    return dialogOpen({
        id: newGuid(),
        title: title,
        url: url,
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.frames[iframeId].AcceptClick(winObject, domId);

            //top.frames[top.frames.length - 1]
            //top.layer.close(top.frames[top.frames.length - 1].window.idx);

            //top.frames[top.frames.length - 1].window.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId);
        }
    });
}

//
function selectDrillplan(winObject, domId, checkMode) {
    return dialogOpen({
        id: "Drillplan",
        title: "选择应急演练计划",
        url: '/EmergencyPlatform/Drillplan/Select?checkMode=' + checkMode,
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId);
        }
    });
}

//选择危险源清单
function selectHisrelationhd_qd(winObject, domId, checkMode) {
    return dialogOpen({
        id: "Hisrelationhd_qd",
        title: "选择危险源清单",
        url: '/HazardsourceManage/Hisrelationhd_qd/Select?checkMode=' + checkMode,
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick({ winObject: winObject, domId: domId });
        }
    });
}
/// <summary>
/// 下载导出文件
/// </summary>
function downloadFile(downurl) {
    var $exp = $("#expFrame");
    if ($exp.length == 0) {
        $exp = $("<iFrame id='expFrame' style='display:none;'/>");
        $("body").append($exp);
    }
    $exp.attr({ src: downurl });
}


/// <summary>
/// 选择监督员
/// </summary>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID</param>
function selectSuperviseUser(winObject, domId) {
    return dialogOpen({
        id: "Supervise",
        title: "选择监督员",
        url: '/HighRiskWork/SidePerson/Select',
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId);
        }
    });
}


/// <summary>
/// 选择作业审批完成的作业
/// </summary>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID</param>
function selectWork(winObject, domId, checkType) {
    return dialogOpen({
        id: "works",
        title: "选择作业",
        url: '/HighRiskWork/HighRiskApply/Select?checkType=' + checkType,
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId);
        }
    });
}


/// <summary>
/// 选择高风险通用作业审批通过
/// </summary>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID,params分别为类型,部门id,工程名称</param>
function selectCommonWork(winObject, domId, params, callback) {
    return dialogOpen({
        id: "works",
        title: "选择作业",
        url: '/HighRiskWork/SuperviseWorkInfo/SelectCommon?params=' + params,
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId, callback);
        }
    });
}

/// <summary>
/// 选择高风险安全设施变动审批通过且还没验收审批通过的
/// </summary>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID</param>
function selectChangeWork(winObject, domId, params, callback) {
    return dialogOpen({
        id: "changeworks",
        title: "选择作业",
        url: '/HighRiskWork/SuperviseWorkInfo/SelectChange?params=' + params,
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId, callback);
        }
    });
}

/// <summary>
/// 选择高风险脚手架搭设和脚手架拆除的
/// </summary>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID</param>
function selectScaffoldWork(winObject, domId, params, callback) {
    return dialogOpen({
        id: "scaffoldworks",
        title: "选择作业",
        url: '/HighRiskWork/SuperviseWorkInfo/SelectScaffold?params=' + params,
        width: ($(top.window).width() - 200) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId, callback);
        }
    });
}

//选择作业信息
function selectWorkInfo(options) {
    var url = '/HighRiskWork/TeamsInfo/Select?taskshareid=' + options.taskshareid + "&teamid=" + options.teamid + "&mode=" + options.mode + "&checkmode=" + options.checkmode + "&workids=" + options.workids;
    return dialogOpen({
        id: "WorkInfo",
        title: "选择作业信息",
        url: url,
        width: ($(top.window).width() - 400) + "px",
        height: "600px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(options);
        }
    });
}

/// <summary>
/// 选择作业类型
/// </summary>
///<param name="deptId">查询条件，根据mode的值查询方式会不同</param>
/// <param name="checkMode">单选或多选，0:单选，1:多选</param>
/// <param name="mode">查询模式，0:获取机构ID为Ids下所有的部门(即OrganizeId=Ids)，1:获取部门ParentId为Ids下所有部门(即ParentId=Ids)，2:获取部门的parentId在Ids中的部门(即ParentId in(Ids))，3:获取部门Id在Ids中的部门(即DepartmentId in(Ids))</param>
///<param name="winObject">窗体中需要查找domId的对象，一般可写成window.document.body或this.parentNode</param>
///<param name="domId">接收返回值的dom节点的ID，多个用逗号分隔，顺序依次为用户名称,用户账号，用户Id（多选用逗号分隔）</param>
function SelectWorkType(typeIDs, checkMode, mode, winObject, domId, callback) {
    return dialogOpen({
        id: "type",
        title: "选择作业类别",
        url: '/HighRiskWork/SuperviseWorkInfo/SelectWorkType?typeIDs=' + typeIDs + "&checkMode=" + checkMode + "&mode=" + mode,
        width: "400px",
        height: "500px",
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(winObject, domId, callback);
        }
    });
}

function selectSafetyCheck(type, actiondo) {


    var url = "/SaftyCheck/SaftyCheckDataRecord/ZXIndex?pfrom=0&ctype=" + type;
    if (type == 1) {
        url = "/SaftyCheck/SaftyCheckDataRecord/Index?pfrom=0&ctype=" + type;
    }
    top.dlgIndex = dialogOpen({
        id: 'SafetyCheck',
        title: '选择安全检查信息',
        url: url,
        width: ($(top.window).width() - 200) + "px",
        height: ($(top.window).height() - 100) + "px",
        //btn: ["关闭"],
        callBack: function (iframeId) {
            top.document.getElementById(iframeId).contentWindow.AcceptClick(actiondo);
        }
    });
}





/**************隐患相关*****************/

//时间选择比较
function selectCurDate(obj) {
    var curId = $(obj).attr("id");
    var checkdate = !!$("#CHECKDATE") ? formatDate($("#CHECKDATE").val(),'yyyy-MM-dd') : null; //排查日期 
    var approvaldate = !!$("#APPROVALDATE") ? formatDate($("#APPROVALDATE").val(), 'yyyy-MM-dd') : null; //评估日期
    var changedeadine = !!$("#CHANGEDEADINE") ? formatDate($("#CHANGEDEADINE").val(), 'yyyy-MM-dd') : null; //整改截止时间 
    var changefinishdate = !!$("#CHANGEFINISHDATE") ? formatDate($("#CHANGEFINISHDATE").val(), 'yyyy-MM-dd') : null; //整改完成时间
    var acceptdate = !!$("#ACCEPTDATE") ? formatDate($("#ACCEPTDATE").val(), 'yyyy-MM-dd') : null; //验收日期
    var recheckdate = !!$("#RECHECKDATE") ? formatDate($("#RECHECKDATE").val(), 'yyyy-MM-dd') : null; //复查验证日期
    var estimatedate = !!$("#ESTIMATEDATE") ? formatDate($("#ESTIMATEDATE").val(), 'yyyy-MM-dd') : null; //整改效果评估日期
    var rechangedeadine = !!$("#RECHANGEDEADINE") ? formatDate($("#RECHANGEDEADINE").val(), 'yyyy-MM-dd') : null; //原整改截止时间
    //var REFORMDEADLINE = !!$("#REFORMFINISHDATE") ? $("#REFORMDEADLINE").val() : null; //整改截止时间(违章/问题)
    var accepttime = !!$("#ACCEPTTIME") ? formatDate($("#ACCEPTTIME").val(), 'yyyy-MM-dd') : null; //验收时间(违章)
    var reformfinishdate = !!$("#REFORMFINISHDATE") ? formatDate($("#REFORMFINISHDATE").val(), 'yyyy-MM-dd') : null; //整改完成时间(违章/问题)
    var verifydate = !!$("#VERIFYDATE") ? formatDate($("#VERIFYDATE").val(), 'yyyy-MM-dd') : null; //验证时间(问题)
    var issuccess = true;

    var curValue = $(obj).val();
    if (!!curValue) {
        var curdate = new Date(curValue.replace(/-/g, "\/")); //当前对象时间
        //排查日期
        if (curId == "CHECKDATE") {
            if (!!approvaldate && curdate > new Date(approvaldate.replace(/-/g, "\/"))) {
                dialogMsg('排查日期不能大于评估日期！', 0);
                issuccess = false;
            }
            else if (!!changedeadine && curdate > new Date(changedeadine.replace(/-/g, "\/"))) {
                dialogMsg('排查日期不能大于整改截止时间！', 0);
                issuccess = false;
            }
            else if (!!changefinishdate && curdate > new Date(changefinishdate.replace(/-/g, "\/"))) {
                dialogMsg('排查日期不能大于整改结束时间！', 0);
                issuccess = false;
            }
            else if (!!acceptdate && curdate > new Date(acceptdate.replace(/-/g, "\/"))) {
                dialogMsg('排查日期不能大于验收日期！', 0);
                issuccess = false;
            }
            else if (!!recheckdate && curdate > new Date(recheckdate.replace(/-/g, "\/"))) {
                dialogMsg('排查日期不能大于复查日期！', 0);
                issuccess = false;
            }
            else if (!!estimatedate && curdate > new Date(estimatedate.replace(/-/g, "\/"))) {
                dialogMsg('排查日期不能大于效果评估日期！', 0);
                issuccess = false;
            }
        }
            //评估日期
        else if (curId == "APPROVALDATE") {
            if (!!checkdate && curdate < new Date(checkdate.replace(/-/g, "\/"))) {
                dialogMsg('评估日期不能小于排查日期！', 0);
                issuccess = false;
            }
            else if (!!changedeadine && curdate > new Date(changedeadine.replace(/-/g, "\/"))) {
                dialogMsg('评估日期不能大于整改截止时间！', 0);
                issuccess = false;
            }
            else if (!!changefinishdate && curdate > new Date(changefinishdate.replace(/-/g, "\/"))) {
                dialogMsg('评估日期不能大于整改结束时间！', 0);
                issuccess = false;
            }
            else if (!!acceptdate && curdate > new Date(acceptdate.replace(/-/g, "\/"))) {
                dialogMsg('评估日期不能大于验收日期！', 0);
                issuccess = false;
            }
            else if (!!recheckdate && curdate > new Date(recheckdate.replace(/-/g, "\/"))) {
                dialogMsg('评估日期不能大于复查日期！', 0);
                issuccess = false;
            }
            else if (!!estimatedate && curdate > new Date(estimatedate.replace(/-/g, "\/"))) {
                dialogMsg('评估日期不能大于效果评估日期！', 0);
                issuccess = false;
            }
        }
            //整改截止时间
        else if (curId == "CHANGEDEADINE") {
            if (!!checkdate && curdate < new Date(checkdate.replace(/-/g, "\/"))) {
                dialogMsg('整改截止时间不能小于排查日期！', 0);
                issuccess = false;
            }
            else if (!!approvaldate && curdate < new Date(approvaldate.replace(/-/g, "\/"))) {
                dialogMsg('整改截止时间不能小于评估日期！', 0);
                issuccess = false;
            }
                //else if (!!acceptdate && curdate > new Date(acceptdate.replace(/-/g, "\/"))) {
                //    dialogMsg('整改截止时间不能大于验收日期！', 0);
                //    issuccess = false;
                //}
            else if (!!recheckdate && curdate > new Date(recheckdate.replace(/-/g, "\/"))) {
                dialogMsg('整改截止时间不能大于复查日期！', 0);
                issuccess = false;
            }
            else if (!!estimatedate && curdate > new Date(estimatedate.replace(/-/g, "\/"))) {
                dialogMsg('整改截止时间不能大于效果评估日期！', 0);
                issuccess = false;
            }
                //原整改截止时间
            else if (!!rechangedeadine && curdate > new Date(rechangedeadine.replace(/-/g, "\/"))) {
                dialogMsg('当前整改截止时间不能大于原整改截止时间！', 0);
                issuccess = false;
            }
            if (!!$("#ACCEPTDATE")) { $("#ACCEPTDATE").val(curValue); } //赋值验收时间
        }
            //整改完成时间
        else if (curId == "CHANGEFINISHDATE") {
            if (!!checkdate && curdate < new Date(checkdate.replace(/-/g, "\/"))) {
                dialogMsg('整改完成时间不能小于排查日期！', 0);
                issuccess = false;
            }
            //else if (!!approvaldate && curdate < new Date(approvaldate.replace(/-/g, "\/"))) {
            //    dialogMsg('整改完成时间不能小于评估日期！', 0);
            //    issuccess = false;
            //}
            //else if (!!acceptdate && curdate > new Date(acceptdate.replace(/-/g, "\/"))) {
            //    dialogMsg('整改完成时间不能大于验收日期！', 0);
            //    issuccess = false;
            //}
            //else if (!!recheckdate && curdate > new Date(recheckdate.replace(/-/g, "\/"))) {
            //    dialogMsg('整改完成时间不能大于复查日期！', 0);
            //    issuccess = false;
            //}
            //else if (!!estimatedate && curdate > new Date(estimatedate.replace(/-/g, "\/"))) {
            //    dialogMsg('整改完成时间不能大于效果评估日期！', 0);
            //    issuccess = false;
            //}
        }
            //验收时间
        else if (curId == "ACCEPTDATE") {
            if (!!checkdate && curdate < new Date(checkdate.replace(/-/g, "\/"))) {
                dialogMsg('验收日期时间不能小于排查日期！', 0);
                issuccess = false;
            }
            else if (!!approvaldate && curdate < new Date(approvaldate.replace(/-/g, "\/"))) {
                dialogMsg('验收日期不能小于评估日期！', 0);
                issuccess = false;
            }
            else if (!!changedeadine && curdate < new Date(changedeadine.replace(/-/g, "\/"))) {
                dialogMsg('验收日期不能小于整改截止时间！', 0);
                issuccess = false;
            }
                //else if (!!changefinishdate && curdate < new Date(changefinishdate.replace(/-/g, "\/"))) {
                //    dialogMsg('验收日期不能小于整改完成时间！', 0);
                //    issuccess = false;
                //}
            else if (!!recheckdate && curdate > new Date(recheckdate.replace(/-/g, "\/"))) {
                dialogMsg('验收日期不能大于复查日期！', 0);
                issuccess = false;
            }
            else if (!!estimatedate && curdate > new Date(estimatedate.replace(/-/g, "\/"))) {
                dialogMsg('验收日期不能大于效果评估日期！', 0);
                issuccess = false;
            }
        }

            //复查日期
        else if (curId == "RECHECKDATE") {

            if (!!checkdate && curdate < new Date(checkdate.replace(/-/g, "\/"))) {
                dialogMsg('复查日期不能小于排查日期！', 0);
                issuccess = false;
            }
            else if (!!approvaldate && curdate < new Date(approvaldate.replace(/-/g, "\/"))) {
                dialogMsg('复查日期不能小于评估日期！', 0);
                issuccess = false;
            }
            else if (!!changedeadine && curdate < new Date(changedeadine.replace(/-/g, "\/"))) {
                dialogMsg('复查日期不能小于整改截止时间！', 0);
                issuccess = false;
            }
                //else if (!!changefinishdate && curdate < new Date(changefinishdate.replace(/-/g, "\/"))) {
                //    dialogMsg('复查日期不能小于整改完成时间！', 0);
                //    issuccess = false;
                //}
            else if (!!acceptdate && curdate < new Date(acceptdate.replace(/-/g, "\/"))) {
                dialogMsg('复查日期不能小于验收日期！', 0);
                issuccess = false;
            }
        }

            //效果评估日期
        else if (curId == "ESTIMATEDATE") {

            if (!!checkdate && curdate < new Date(checkdate.replace(/-/g, "\/"))) {
                dialogMsg('效果评估日期不能小于排查日期！', 0);
                issuccess = false;
            }
            else if (!!approvaldate && curdate < new Date(approvaldate.replace(/-/g, "\/"))) {
                dialogMsg('效果评估日期不能小于评估日期！', 0);
                issuccess = false;
            }
            else if (!!changedeadine && curdate < new Date(changedeadine.replace(/-/g, "\/"))) {
                dialogMsg('效果评估日期不能小于整改截止时间！', 0);
                issuccess = false;
            }
                //else if (!!changefinishdate && curdate < new Date(changefinishdate.replace(/-/g, "\/"))) {
                //    dialogMsg('效果评估日期不能小于整改完成时间！', 0);
                //    issuccess = false;
                //}
            else if (!!acceptdate && curdate > new Date(acceptdate.replace(/-/g, "\/"))) {
                dialogMsg('效果评估日期不能小于验收日期！', 0);
                issuccess = false;
            }
        }
            //违章验收时间
        else if (curId == "ACCEPTTIME") {
            if (!!reformfinishdate && curdate < new Date(reformfinishdate.replace(/-/g, "\/"))) {
                dialogMsg('验收日期不能小于整改完成时间！', 0);
                issuccess = false;
            }
        }
            //问题验证时间
        else if (curId == "VERIFYDATE") {
            if (!!reformfinishdate && curdate < new Date(reformfinishdate.replace(/-/g, "\/"))) {
                dialogMsg('问题验证时间不能小于整改完成时间！', 0);
                issuccess = false;
            }
        }
            //违章整改截止时间
        else if (curId == "REFORMDEADLINE") {
            if (!!$("#ACCEPTTIME")) { $("#ACCEPTTIME").val(curValue); } //赋值验收时间
        }

        //如果不满足条件，则重新赋值为空字符串。
        if (!issuccess) {
            $(obj).val("");
        }
    }
}


//查看视图
function viewextension(keyId, obj) {
    var rqUrl = '/HiddenTroubleManage/HTExtension/DetailList?keyCode=' + obj + "&keyValue=" + keyId;
    var title = "查看整改延期审批记录";

    dialogOpen({
        id: 'HTExtensionForm',
        title: title,
        url: rqUrl,
        width: ($(top.window).width() - 100) + "px",
        height: ($(top.window).height() - 100) + "px",
        btn: null
    });
}



//查看视图
function viewlllegalext(obj) {
    var rqUrl = '/LllegalManage/LllegalExtension/DetailList?keyValue=' + obj;
    var title = "查看整改延期审批记录";
    dialogOpen({
        id: 'LllegalExtensionForm',
        title: title,
        url: rqUrl,
        width: ($(top.window).width() - 100) + "px",
        height: ($(top.window).height() - 100) + "px",
        btn: null
    });
}


function UnicodeToString(content) {
    var div = document.createElement("div");
    div.innerHTML = content;
    return div.innerHTML || div.textContent;
}



//整改计划查看
function viewchangeplan(keyValue) {
    dialogOpen({
        id: 'ChangePlanForm',
        title: '整改计划详情',
        url: '/HiddenTroubleManage/HTChangePlan/PlanForm?actiontype=view&keyValue=' + keyValue,
        width: ($(top.window).width() - 200) + "px",
        height: '680px',
        btn: null
    });
}


//加密
function encryptByDES(message, key, iv) {
    var keyHex = CryptoJS.enc.Utf8.parse(key);
    var ivHex = CryptoJS.enc.Utf8.parse(iv);
    encrypted = CryptoJS.DES.encrypt(message, keyHex, {
        iv: ivHex,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    }
    );
    return encrypted.ciphertext.toString();
}

//预览图片
function viewimg(id, objid, epath) {
    var $obj = $(objid);
    $.post("../../PublicInfoManage/ResourceFile/GetFilesByRecId", { recId: id }, function (data) {
        var files = eval("(" + data + ")");
        if (files.length > 0) {
            $(files).each(function (i, file) {
                if (!!file.filepath) {
                    var filepath = file.filepath.substring(15, file.filepath.length);
                    if (!!epath && epath.indexOf(filepath) < 0) {
                        var $li = $('<img title="点击查看大图" alt="点击查看大图" style=\"width:0px;height:0px;\" data-original="' + "/" + file.filepath.substring(1, file.filepath.length) + '" src="' + "/" + file.filepath.substring(1, file.filepath.length) + '"  />');
                        $li.appendTo($obj);
                    }
                }
            });
            $obj.viewer({ url: "data-original" });
        }
    });
}

function LoadTips(recId) {
    //绑定附件
    $.ajax({
        url: '../../PublicInfoManage/ResourceFile/GetFilesByRecId',
        data: { recId: recId },
        type: "post",
        success: function (data) {
            if (!!data) {
                var objdata = eval("(" + data + ")"); //转化为对象类型
                if (objdata.length > 0) {
                    var filepath = objdata[0].filepath;
                    var lastIndex = objdata[0].filename.lastIndexOf(".");
                    var filename = objdata[0].filename.substring(0, lastIndex);
                    var html = "温馨提示：<a href='" + filepath + "' style='text-decoration:none;color:blue;' target='_bank'>" + filename + "</a>";
                    $("#HidMessage").html(html);
                } else {
                    $("#HidMessage").remove();
                }
            } else {
                $("#HidMessage").remove();
            }
        }
    });

}