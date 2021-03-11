//<!------------------------------说明-------------------------------------------!>
//<1、在调用此公共js前，请确保数据查询的主键设置为id显示>

/// <summary>
/// 加载公共的列表页面 
/// </summary>
///<param name="url">请求的url地址</param>
/// <param name="arrCol">显示列</param>
/// <param name="sortorder">排序方式</param>
/// <param name="sortname">排序字段</param>
///<param name="queryJson">查询条件</param>
///<param name="isShowGridAction">是否显示列表的操作权限</param>
///<param name="isShowMultiselect">是否可以多选</param>
var selectedRowIndex = 0;

//无分页的绑定grid
function GetGridNoPage(gridTable, arrCol, height) {
    if (height == "")
        height = $(window).height() - 350;
    var selectedRowIndex = 0;
    var $gridTable = gridTable;
    $gridTable.jqGrid({
        datatype: "json",
        url: "",
        height: height,
        autowidth: true,
        colModel: arrCol,
        viewrecords: true,
        //rownumbers: true,
        shrinkToFit: true,
        gridview: true,
        onSelectRow: function () {
            selectedRowIndex = $("#" + this.id).getGridParam('selrow');
        },
        loadError: function (xhr, status, error) {
        },

    });
}

function GetGridNoPageAnUrl(gridTable, arrCol, height, url) {
    if (height == "")
        height = $(window).height() - 350;
    var selectedRowIndex = 0;
    var $gridTable = gridTable;
    $gridTable.jqGrid({
        datatype: "json",
        url: url,
        height: height,
        autowidth: true,
        colModel: arrCol,
        viewrecords: true,
        //rownumbers: true,
        shrinkToFit: true,
        gridview: true,
        onSelectRow: function () {
            selectedRowIndex = $("#" + this.id).getGridParam('selrow');
        },
        loadError: function (xhr, status, error) {
        },

    });
}

function addRowData(index, arr) {

    $("#gridTable").addRowData(i, arr);
}

function checkValue(value) {
    if (value == "" || value == undefined || value == undefined)
        return false;
    return true;

}

function GetValue(value) {
    if (value == "" || value == undefined || value == undefined)
        return "";
    return value;

}


//分页数据绑定gride
function GetGrid(url, arrCol, sortorder, sortname, isShowGridAction, isShowMultiselect, height, rownumWidth, queryJsonPostData) {
    //是否在指定位置插入操作
    arrCol.push(
        { label: '创建人', name: 'createuserid', index: 'createuserid', align: 'center', sortable: true, hidden: true },
        { label: '创建人部门code', name: 'createuserdeptcode', index: 'createuserdeptcode', align: 'center', sortable: true, hidden: true },
        { label: '创建人组织code', name: 'createuserorgcode', index: 'createuserorgcode', align: 'center', sortable: true, hidden: true }
);
    if (isShowGridAction)
        arrCol.splice(0, 0, { label: '操作', name: 'Oper', width: 100, align: 'center', sortable: false});
    var $gridTable = $('#gridTable');
    $gridTable.jqGrid({
        autowidth: true,
        height: height,
        postData: { queryJson: JSON.stringify(queryJsonPostData) },
        url: url,
        datatype: "json",
        colModel: arrCol,
        viewrecords: true,
        rowNum: 20,
        rownumWidth: rownumWidth,
        pager: "#gridPager",
        sortname: sortname,
        sortorder: sortorder,
        rownumbers: true,
        shrinkToFit: true,
        gridview: true,
        multiselect: isShowMultiselect,
        onSelectRow: function () {
            selectedRowIndex = $('#' + this.id).getGridParam('selrow');
        },
        gridComplete: function () {
            $('#' + this.id).setSelection(selectedRowIndex, false);
            if (isShowGridAction) {
                GridActionShow();
            }
        }
    });
    //查询事件
    $("#btn_Search").click(function () {
        var filter = $(".ui-filter-list");
        if (filter.html() != undefined && filter.html() != null) {
            if (filter.attr("style").indexOf("block") >= 0) {
                ////隐藏搜索框
                var title = $(".ui-filter-text");

                title.trigger("click");
            }
        }


        var $gridTable = $('#gridTable');
        var queryJson = GetQueryJson();
        $gridTable.jqGrid('setGridParam', {
            postData: { queryJson: JSON.stringify(queryJson) }, page: 1
        }).trigger('reloadGrid');
    });
}

/// <summary>
/// 列表的操作权限 
/// </summary>
function GridActionShow() {
    $("#" + this.id).setSelection(selectedRowIndex, false);

    var $gridTable = $('#gridTable');
    var rows = $gridTable.jqGrid("getRowData");//获取当前页记录行数据
    //查询用户对该模块的数据操作权限
    $.post(top.contentPath + "/AuthorizeManage/PermissionJob/GetDataAuthority", { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (data) {
        var jsonArr = eval("(" + data + ")");
        $(rows).each(function (i, item) {
            var uId = item.createuserid;
            var keyValue = item.id;
            var dCode = item.createuserdeptcode; //获取记录创建人的所属部门Code
            var oCode = item.createuserorgcode;  //获取记录创建人的所属机构Code
            var btns = $("td[aria-describedby='gridTable_Oper']").eq(i).children();//获取操作列中定义的操作按钮
            var html = "";
            //如果操作列中没有定义任何按钮则根据系统权限设置自动绑定操作按钮
            if (btns.length == 0) {
                $(jsonArr).each(function (j, item1) {
                    var authType = parseInt(item1.authorizetype);//获取数据操作权限范围.1：本人,2：本部门，3：本部门及下属部门，4：本机构，5：全部
                    switch (authType) {
                        //本用户
                        case 1:
                            if (top.currUserId == uId) {
                                html += "<a href=\"javascript:" + item1.actionname + "('" + item1.encode + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            }
                            break;
                            //本部门
                        case 2:
                            if (top.currUserDeptCode == dCode) {
                                html += "<a href=\"javascript:" + item1.actionname + "('" + item1.encode + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            }
                            break;
                            //本子部门
                        case 3:
                            if (dCode.indexOf(top.currUserDeptCode) >= 0) {
                                html += "<a href=\"javascript:" + item1.actionname + "('" + item1.encode + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            }
                            break;
                            //本机构
                        case 4:
                            if (oCode == top.currUserOrgCode) {
                                html += "<a href=\"javascript:" + item1.actionname + "('" + item1.encode + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            }
                            break;
                        case 5:
                            html += "<a href=\"javascript:" + item1.actionname + "('" + item1.encode + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                    }


                });

                $("td[aria-describedby='gridTable_Oper']").eq(i).html(html);
            } else {

                html = "";
                //遍历用户对该模块的数据操作权限
                $(jsonArr).each(function (j, item1) {
                    html += item1.encode + ",";
                    var authType = item1.authorizetype;//获取数据操作权限范围.1：本人,2：本部门，3：本部门及下属部门，4：本机构，5：全部
                    switch (authType) {
                        //本用户
                        case 1:
                            if (top.currUserId != uId) {
                                $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            }
                            break;
                            //本部门
                        case 2:

                            if (deptCode != top.currUserDeptCode) {
                                $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            }
                            break;
                            //本子部门
                        case 3:
                            if (dCode.indexOf(top.currUserDeptCode) < 0) {
                                $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            }
                            break;
                            //本机构
                        case 4:
                            if (oCode != top.currUserOrgCode) {
                                $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            }
                            break;
                    }
                });

                $(btns).each(function (j, dom) {
                    if (html.indexOf(dom.attr("name")) < 0) {
                        $(dom).remove();
                    }
                });
            }

        });
    });
}

/// <summary>
/// 弹框操作 
/// </summary>
///<param name="url">请求的url地址</param>
/// <param name="title">标题</param>
/// <param name="width">宽度</param>
///<param name="height">长度</param>
///<param name="isOnlyClose">判断是查看还是操作弹框</param>
var isImport = false;
function DialogOpenShow(url, title, width, height, isOnlyClose,tabId) {
    if (width == "") width = ($(top.window).width() - 350) + "px";
    if (height == "") height = ($(top.window).height() - 170) + "px";
    if (isOnlyClose) {
        var dlg = dialogOpen({
            id: tabId == "" ? "Form" : tabId,
            title: title,
            url: url,
            width: width,
            height: height,
            btn: ["关闭"],
            callBack: function (iframeId) {
                top.layer.close(dlg);
            }
        });
    }
    else {
        var dlg = dialogOpen({
            id: "Form",
            title: title,
            url: url,
            width: width,
            height: height,
            callBack: function (iframeId) {
                top.document.getElementById(iframeId).contentWindow.AcceptClick();
            }
            ,
            cancel: function (index) {
                if (isImport) {
                    isImport = false;
                    $("#gridTable").jqGrid('setGridParam', {
                        postData: { keyWord: "" }
                    }).trigger('reloadGrid');
                }

            }
        });
    }


}

/// <summary>
/// 页面按钮的操作权限判断 
/// </summary>
///<param name="action">具体的操作方法，跟功能配置的编号必须一致</param>
function Action(action) {
    if (action == undefined || action == "" || action == null)
        action = event.target.id;
    //获取当前目录的路劲
    var str = location.pathname;
    var arr = str.split("/");
    //去掉Index
    delete arr[arr.length - 1];
    //去掉项目目录
    delete arr[1];
    var dir = arr.join("/");
    var rowObject = $('#gridTable').jqGrid("getRowData", selectedRowIndex);
    var keyValue = $('#gridTable').jqGridRowValue('id');
    //链表查询时判断
    var keyValuedeal = rowObject.dealid;
    var title = document.title;
    switch (action) {
        case "add":
            DialogOpenShow(dir + 'Form', '添加' + title, "", "", false);
            break;
        case "delete":
            if (keyValue) {
                //有关联且处理
                if (keyValuedeal != undefined && keyValuedeal != "" && keyValuedeal != null)
                    keyValue = keyValuedeal;
                //有关联但是没有处理
                if (keyValuedeal != undefined && keyValuedeal == "") {
                    dialogMsg('该条数据未做处理！', 0);
                    return;
                }
                $.RemoveForm({
                    url: "../.." + dir + 'RemoveForm',
                    param: { keyValue: keyValue },
                    success: function (data) {
                        $('#gridTable').trigger('reloadGrid');
                    }
                })
            } else {
                dialogMsg('请选择需要删除的' + title, 0);
            }
            break;
        case "edit":
            if (checkedRow(keyValue)) {
                DialogOpenShow(dir + 'Form?keyValue=' + keyValue + "&keyValuedeal=" + keyValuedeal, '编辑' + title, "", "", false);
            }
            break;
        case "Import":
            DialogOpenShow(dir + 'Import', '导入' + title, ($(top.window).width() - 900) + "px", ($(top.window).height() - 440) + "px", false);
            break;
        case "export":
            var queryJson = GetQueryJson();
            location.href = "../.." + dir + "Export?queryJson=" + JSON.stringify(queryJson);
            break;
        case "show":
            if (checkedRow(keyValue)) {

                DialogOpenShow(dir + 'Form?action=show&keyValue=' + keyValue + "&keyValuedeal=" + keyValuedeal, '查看' + title, "", "", true);
            }
            break;
        default:

    }

}

//按钮权限
function LoadToolBar() {
    var html = '<div class="btn-group"> <a class="btn btn-default" onclick="reload()"><i class="fa fa-refresh"></i>刷新</a> </div><script>$(".toolbar").authorizeButton()</script>';
    $(".toolbar").html(html);
}






