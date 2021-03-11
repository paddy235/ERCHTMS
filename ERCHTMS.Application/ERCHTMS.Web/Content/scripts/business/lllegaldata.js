var keyValue = request("keyValue"); //主键 

//查看违章视图
function viewdata(obj, atype, flowstate) {
    var rqUrl = "";
    var title = "";

    //未整改隐患的查看页面
    if (flowstate == "违章登记" || flowstate == "违章举报") {
        title = "查看违章";
        rqUrl = '/LllegalManage/LllegalRegister/Form?keyValue=' + obj + '&actiontype=view';
    }
    else if (flowstate == "违章核准" || flowstate == "违章审核") {
        title = "查看违章";
        rqUrl = '/LllegalManage/LllegalApprove/Form?keyValue=' + obj + '&actiontype=view';
    }
    else if (flowstate == "制定整改计划") {
        title = "查看违章";
        rqUrl = '/LllegalManage/LllegalPlanReform/Form?keyValue=' + obj + '&actiontype=view';
    }
    else if (flowstate == "违章整改") {
        title = "查看违章";
        rqUrl = '/LllegalManage/LllegalReform/Form?keyValue=' + obj + '&actiontype=view';
    }
    else if (flowstate == "违章验收") {
        title = "查看违章";
        rqUrl = '/LllegalManage/LllegalAccept/Form?keyValue=' + obj + '&actiontype=view';
    }
    else if (flowstate == "验收确认") {
        title = "查看违章";
        rqUrl = '/LllegalManage/LllegalConfirm/Form?keyValue=' + obj + '&actiontype=view';
    }
    else {
        if (atype == "0") {
            title = "查看违章";
            rqUrl = '/LllegalManage/LllegalConfirm/Form?keyValue=' + obj + '&actiontype=view';
        }
        else  //已整改的查看页面
        {
            title = "已整改违章查看";
            rqUrl = '/LllegalManage/LllegalRegister/NewForm?keyValue=' + obj + '&actiontype=view';
        }
    }

    dialogOpen({
        id: 'LllegalForm',
        title: title,
        url: rqUrl,
        width: ($(top.window).width() - 100) + "px",
        height: ($(top.window).height() - 100) + "px",
        btn: null
    });
}
//编辑视图
function editdata(obj, atype, flowstate) {
    var rqUrl = "";
    var title = "";

    //未整改隐患的查看页面
    if (flowstate == "违章登记" || flowstate == "违章举报") {
        title = "编辑违章";
        rqUrl = '/LllegalManage/LllegalRegister/Form?keyValue=' + obj + '&actiontype=edit&callFormId=LllegalForm';
    }
    else if (flowstate == "违章核准" || flowstate == "违章审核") {
        title = "编辑违章";
        rqUrl = '/LllegalManage/LllegalApprove/Form?keyValue=' + obj + '&actiontype=edit&callFormId=LllegalForm';
    }
    else if (flowstate == "制定整改计划") {
        title = "编辑违章";
        rqUrl = '/LllegalManage/LllegalPlanReform/Form?keyValue=' + obj + '&actiontype=edit';
    }
    else if (flowstate == "违章整改") {
        title = "编辑违章";
        rqUrl = '/LllegalManage/LllegalReform/Form?keyValue=' + obj + '&actiontype=edit';
    }
    else if (flowstate == "违章验收") {
        title = "编辑违章";
        rqUrl = '/LllegalManage/LllegalAccept/Form?keyValue=' + obj + '&actiontype=edit';
    }
    else {
        title = "编辑违章";
        rqUrl = '/LllegalManage/LllegalAccept/Form?keyValue=' + obj + '&actiontype=edit';
    }

    dialogOpen({
        id: 'LllegalForm',
        title: title,
        url: rqUrl,
        width: ($(top.window).width() - 100) + "px",
        height: ($(top.window).height() - 100) + "px",
        btn: null
    });
}
//查看流程图
function OpenViewFlow(keyValue) {
    var title = "违章流程图";
    var rqUrl = "/SystemManage/WorkFlow/Detail?keyValue=" + keyValue;
    dialogOpen({
        id: 'LllegalFlowForm',
        title: title,
        url: rqUrl,
        width: ($(top.window).width() / 2 + 200) + "px",
        height: ($(top.window).height() / 2 + 300) + "px",
        btn: null
    });
}
//审核
function getApproveGrid() {
    var selectedRowIndex = 0;
    var $gridTable = $('#approveGridTable');
    $gridTable.jqGrid({
        url: "../../LllegalManage/LllegalApprove/GetHistoryListJson?keyValue=" + keyValue,
        datatype: "json",
        height: $(window).height() - 500,
        autowidth: true,
        colModel: [
            {
                label: '操作', name: 'ID', index: 'ID', width: 120, align: 'center',
                formatter: function (cellvalue, options, rowObject) {
                    var html = "<a href=javascript:DetailApprove('" + rowObject.ID + "')  title='查看'><i class='fa fa-eye'></i></a>";
                    return html;
                }
            },
            { label: '审核人', name: 'APPROVEPERSON', index: 'APPROVEPERSON', width: 150, align: 'center' },
            { label: '审核单位', name: 'APPROVEDEPTNAME', index: 'APPROVEDEPTNAME', width: 120, align: 'center' },
            {
                label: '审核结果', name: 'APPROVERESULT', index: 'APPROVERESULT', width: 150, align: 'center',
                formatter: function (cellvalue, options, rowObject) {
                    var html = rowObject.APPROVERESULT == "1" ? "通过" : "不通过";
                    return html;
                }
            },
            {
                label: '审核时间', name: 'APPROVEDATE', index: 'APPROVEDATE', width: 150, align: 'center',
                formatter: function (cellvalue, options, rowObject) {
                    var html = formatDate(cellvalue, 'yyyy-MM-dd');
                    return html;
                }
            }
        ],
        pager: false,
        rowNum: "20",
        sortname: 'approvedate',
        sortorder: 'desc',
        rownumbers: true,
        shrinkToFit: false,
        gridview: true,
        ondblClickRow: function (id) {
        }
    });
}

//审核
function GetApproveGrid() {
    var selectedRowIndex = 0;
    var $gridTable = $('#approveGridTable');
    $gridTable.jqGrid({
        url: "../../LllegalManage/LllegalApprove/GetHistoryListJson?keyValue=" + keyValue,
        datatype: "json",
        height: $(window).height() - 500,
        autowidth: true,
        colModel: [
            {
                label: '操作', name: 'ID', index: 'ID', width: 120, align: 'center',
                formatter: function (cellvalue, options, rowObject) {
                    var html = "<a href=javascript:DetailApprove('" + rowObject.ID + "')  title='查看'><i class='fa fa-eye'></i></a>";
                    return html;
                }
            },
            { label: '审核人', name: 'APPROVEPERSON', index: 'APPROVEPERSON', width: 150, align: 'center' },
            { label: '审核单位', name: 'APPROVEDEPTNAME', index: 'APPROVEDEPTNAME', width: 120, align: 'center' },
            {
                label: '审核结果', name: 'APPROVERESULT', index: 'APPROVERESULT', width: 150, align: 'center',
                formatter: function (cellvalue, options, rowObject) {
                    var html = rowObject.APPROVERESULT == "1" ? "通过" : "不通过";
                    return html;
                }
            },
            {
                label: '审核时间', name: 'APPROVEDATE', index: 'APPROVEDATE', width: 150, align: 'center',
                formatter: function (cellvalue, options, rowObject) {
                    var html = formatDate(cellvalue, 'yyyy-MM-dd');
                    return html;
                }
            }
        ],
        pager: false,
        rowNum: "20",
        sortname: 'approvedate',
        sortorder: 'desc',
        rownumbers: true,
        shrinkToFit: false,
        gridview: true,
        ondblClickRow: function (id) {
        }
    });
}
//查看审核详情
function DetailApprove(id) {
    dialogOpen({
        id: 'DetailForm',
        title: '违章审核详情',
        url: '/LllegalManage/LllegalApprove/Detail?keyValue=' + id,
        width: ($(top.window).width() - 200) + "px",
        height: '680px',
        btn: null
    });
}
//获取历史整改信息
function GetHistoryReform() {
    dialogOpen({
        id: 'HistoryForm',
        title: '违章整改信息',
        url: '/LllegalManage/LllegalReform/DetailList?keyValue=' + keyValue,
        width: ($(top.window).width() - 200) + "px",
        height: ($(top.window).height() - 200) + "px",
        btn: null
    });
}
//获取历史验收信息
function GetHistoryAccetp() {
    var keyCode = $("#HIDCODE").val();
    dialogOpen({
        id: 'HistoryForm',
        title: '违章验收信息',
        url: '/LllegalManage/LllegalAccept/DetailList?keyValue=' + keyValue,
        width: ($(top.window).width() - 200) + "px",
        height: ($(top.window).height() - 200) + "px",
        btn: null
    });
}
