//======================
//列表页面代理类options
//{
//    gridId: "gridTable",
//    gridHeight: winHeight - 136,
//    gridOptions: {
//        height: winHeight - 136,
//        url: "../../StandardSystem/StandardApply/GetListJson",
//        datatype: "json",
//        pager: "#gridPager",                
//        sortname: 'num',
//        sortorder: 'asc,createdate desc'
//    },
//    gridColumns: [
//        { label: 'createuserid', name: 'createuserid', hidden: true },
//        { label: 'createuserdeptcode', name: 'createuserdeptcode', hidden: true },
//        { label: 'createuserorgcode', name: 'createuserorgcode', hidden: true },
//        { label: '主键', name: 'id', hidden: true },
//        {
//            label: '操作', name: 'Oper', width: 100, align: 'center', sortable: false,
//            formatter: function (cellvalue, options, rowObject) {
//                return buildOper(rowObject);
//            }
//        },
//        { label: '文件名称', name: 'filename', index: 'filename', width: 300, align: 'center', sortable: true },
//        {...}
//    ],
//    completeCallback:function(){},
//    hasPowerOper: false,
//    userInfo: {
//        userId: userId,
//        deptCode: deptCode,
//        orgCode: orgCode
//    }
//}
//======================
var selectedRowIndex = 0;
function listAgency(options) {
    var agc = this;
    options = options || {};
    //====================
    //初始参数
    //====================
    var settings = {
        hasPowerOper: false,
        userInfo: {
            userId: "",
            deptCode: "",
            orgCode: ""
        },
        pageControls: {
            queryId:"queryArea",
            searchId: "btn_Search",
            resetId: "btn_Reset",
            refreshId: "refresh",
            addId: "add",
            impId: "imp",
            expId: "exp",
            btns:[]
        },
        gridId: "gridTable",
        gridHeight: "100%",
        gridOptions: null,
        gridColumns: [],     
        completeCallback: function () { }
    };    
    //====================
    //事件
    //====================
    var events = {
        //====================
        //窗口大小改变
        //====================
        onResize: function () { resize(); },
        //====================
        //构造查询条件
        //====================
        onBuildQuery: function () {return buildQuery();},
        //====================
        //查询
        //====================
        onQueryData : function(){agc.queryData();},
        //====================
        //重置
        //====================
        onReset: function () {reset();},
        //====================
        //刷新
        //====================
        onReload: function () { reload() },
        //====================
        //添加
        //====================
        onAdd: function () { add();},
        //====================
        //导入
        //====================
        onImp: function () { imp(); },
        //====================
        //导出
        //====================
        onExp: function () { exp(); },
        //====================
        //权限
        //====================
        onSetPowerOper: function () { setPowerOper();}
    };
    //====================
    //事件参数
    //====================
    var evtArgs = {
        addArg: null,
        editArg: null,
        delArg: null,
        detailArg: null,
        impArg: null,
        expArg: null
    };
    $.extend(true,settings, options);
    $.extend(true,events, options.events);
    $.extend(true,evtArgs, options.evtArgs);
    //表格对象
    var grid = $("#" + settings.gridId);
    //====================
    //绑定页面控件ctrls
    //{searchId:'',addId:'',refreshId:'',resetId:'',impId:''}
    //====================
    this.bindPageControls = function (ctrls) {
        var it = this;
        $.extend(settings.pageControls, ctrls);
        var searcId = settings.pageControls.searchId;
        if (!!searcId) {//查询
            $("#" + searcId).click(function () {
                events.onQueryData();
            })
        }
        var resetId = settings.pageControls.resetId;
        if (!!resetId) {//重置
            $("#" + resetId).click(function () {
                events.onReset();
            })
        }
        var refreshId = settings.pageControls.refreshId;
        if (!!refreshId) {//刷新
            $("#" + refreshId).click(function () {
                events.onReload();
            })
        }
        var addId = settings.pageControls.addId;
        if (!!addId) {//添加
            $("#" + addId).click(function () {
                events.onAdd();
            })
        }
        var impId = settings.pageControls.impId;
        if (!!impId) {//导入
            $("#" + impId).click(function () {
                events.onImp();
            })
        }
        var expId = settings.pageControls.expId;
        if (!!expId) {//导出
            $("#" + expId).click(function () {
                events.onExp();
            })
        }
        //按钮事件
        for (var i = 0; i < settings.pageControls.btns.length; i++) {
            var btn = settings.pageControls.btns[i];
            if (!!btn.id) {
                var evt = new pageEvent({ id: btn.id, url: btn.url, onClick: btn.onClick, args: btn.args });
                $("#" + btn.id).bind("click", evt.touch);
            }
        }
    };
    //====================
    //绑定页面事件evts
    //{onBuildQuery:fn,onReset:fn,onQueryData:fn,onAdd:fn,onImp:fn,onExp:fn}
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
    //绑定页面跳转地址urlArgs
    //{
    //    addArg: {
    //            title: '添加标准修编申请',
    //            url: '/StandardSystem/StandardApply/Form',
    //            btn: null
    //    },
    //    editArg: {
    //            title: '编辑标准修编申请',
    //            url: '/StandardSystem/StandardApply/Form',
    //            btn: null
    //    },
    //    delArg: { url: '../../StandardSystem/StandardApply/RemoveForm' },
    //    detailArg: {
    //            title: '查看标准修编申请',
    //            url: '/StandardSystem/StandardApply/Detail',
    //            btn: null
    //    }
    //}
    //====================
    this.addPageGotoUrl = function (urlArgs) {
        $.extend(evtArgs, urlArgs);
    };
    //====================
    //初始化args=
    //([
    //{
    //  url:'',
    //  callBack:fn, 
    //  ctrls:[{Id:'',memberId:'',memberText:'',description:'',dataProName:''},{...}]
    //},
    //{
    //  data:[{id:'',name:''},{...}],
    //  ctrl:{Id:'',memberId:'',memberText:'',description:''}
    //},
    //{...}
    //])
    //====================
    this.initialPage = function (args) {
        args = args || {};
        //resize重设布局;
        $(window).resize(function (e) {
            window.setTimeout(function () {
                if(settings.trgResize!=false)
                    events.onResize();                    
            }, 200);
            e.stopPropagation();
        });
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
                    success: function (data) {
                        for (var j = 0; j < arg.ctrls.length; j++) {
                            var ctrl = arg.ctrls[j];
                            $("#" + ctrl.Id).bindCombobox(ctrl.memberId, ctrl.memberText, ctrl.description, eval(ctrl.dataProName));
                        }
                        if (arg.callBack && typeof arg.callBack == "function") {
                            arg.callBack(data);
                        }
                    }
                });
            }
            else {
                //回调
                if (!!arg.callBack && typeof arg.callBack == "function") {
                    arg.callBack();
                }
            }
            if (!!arg.conditionData) {
                $("#" + settings.pageControls.queryId).formDeserialize(arg.conditionData);
            }
        }
        searchData();
    };        
    //====================
    //首次加载查询
    //====================
    var searchData = function () {        
        var opts = {
            autowidth: true,
            height: 'auto',
            postData: { queryJson: events.onBuildQuery() },
            datatype: "json",
            colModel: settings.gridColumns,
            viewrecords: true,
            rowNum: 15,
            rowList: [10, 15, 20, 50, 100],
            rownumbers: true,
            shrinkToFit: true,
            gridview: true,
            onSelectRow: function () {
                selectedRowIndex = $(this).getGridParam('selrow');
            },
            gridComplete: function () {
                $(this).setSelection(selectedRowIndex, false);
                if (settings.hasPowerOper == true)
                    events.onSetPowerOper();
                if (settings.completeCallback) {
                    var data = $(this).jqGrid("getRowData");
                    settings.completeCallback(data);
                }
            }
        };
        $.extend(opts, settings.gridOptions);
        grid.jqGrid(opts);
    };
    //====================
    //窗口大小改变
    //====================
    var resize = function () {
        var winHeight = $(window).height();
        grid.setGridWidth(($('.gridPanel').width()));
        if(!!settings.gridJustHeight)
            grid.setGridHeight(winHeight - settings.gridJustHeight);
        else
            grid.setGridHeight(winHeight - 136);
    }
    //====================
    //权限设置
    //====================
    var setPowerOper = function () {
        var $gridTable = grid;
        var rows = $gridTable.jqGrid("getRowData");//获取当前页记录行数据
        //查询用户的操作权限
        $.post(top.contentPath + "/AuthorizeManage/PermissionJob/GetOperAuthority", { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (data) {
            if (!data) data = "[]";
            var jsonArr = eval("(" + data + ")");
            //新增权限
            var hasAdd = false;
            $(jsonArr).each(function (j, item1) {
                if (item1.encode == "add") {
                    hasAdd = true;
                    return;
                }
            });
            if (!hasAdd) {//增加、导入权限
                $("#" + settings.pageControls.addId + ",#" + settings.pageControls.impId).remove();
            }
        });
        //查询用户对该模块的数据操作权限
        $.post(top.contentPath + "/AuthorizeManage/PermissionJob/GetDataAuthority", { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (data) {
            var currUserId = settings.userInfo.currUserId;
            var deptCode = settings.userInfo.deptCode;
            var orgCode = settings.userInfo.orgCode;
            var jsonArr = eval("(" + data + ")");
            var colOper = $("td[aria-describedby='gridTable_Oper']");//操作列
            $(rows).each(function (i, item) {
                var uId = item.createuserid;
                var keyValue = item.id;
                var dCode = item.createuserdeptcode; //获取记录创建人的所属部门Code
                var oCode = item.createuserorgcode;  //获取记录创建人的所属机构Code
                var btns = colOper.eq(i).children();//获取操作列中定义的操作按钮
                var html = "";
                //如果操作列中没有定义任何按钮则根据系统权限设置自动绑定操作按钮
                if (btns.length == 0) {
                    var detail = "detail";
                    $(jsonArr).each(function (j, item1) {
                        var authType = parseInt(item1.authorizetype);//获取数据操作权限范围.1：本人,2：本部门，3：本部门及下属部门，4：本机构，5：全部
                        if (item1.actionname == "detail")
                            detail = "";
                        switch (authType) {
                            //本用户
                            case 1:
                                if (currUserId == uId) {
                                    html += "<a href=\"javascript:" + item1.actionname + "('" + keyValue + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                }
                                break;
                                //本部门
                            case 2:
                                if (deptCode == dCode) {
                                    html += "<a href=\"javascript:" + item1.actionname + "('" + keyValue + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                }
                                break;
                                //本子部门
                            case 3:
                                if (dCode.indexOf(deptCode) >= 0) {
                                    html += "<a href=\"javascript:" + item1.actionname + "('" + keyValue + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                }
                                break;
                                //本机构
                            case 4:
                                if (oCode == orgCode) {
                                    html += "<a href=\"javascript:" + item1.actionname + "('" + keyValue + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                }
                                break;
                            case 5:
                                html += "<a href=\"javascript:" + item1.actionname + "('" + keyValue + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                break;
                        }
                    });
                    if (!!detail) {
                        html = "<a href=\"javascript:detail('" + keyValue + "')\" title=\"查看\"><i class=\"fa fa-eye\"></i></a>" + html;
                    }
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
                                if (currUserId != uId) {
                                    $(btns).filter(function (i) { return this.name == item1.encode }).remove();
                                }
                                break;
                                //本部门
                            case 2:
                                if (deptCode != dCode) {
                                    $(btns).filter(function (i) { return this.name == item1.encode }).remove();
                                }
                                break;
                                //本子部门
                            case 3:
                                if (dCode.indexOf(deptCode) < 0) {
                                    $(btns).filter(function (i) { return this.name == item1.encode }).remove();
                                }
                                break;
                                //本机构
                            case 4:
                                if (oCode != orgCode) {
                                    $(btns).filter(function (i) { return this.name == item1.encode }).remove();
                                }
                                break;
                        }
                    });
                    $(btns).each(function (j, dom) {
                        var d = $(dom);
                        if (html.indexOf(d.attr("name")) < 0) {
                            d.remove();
                        }
                    });
                }
            });
        });
    };
    //====================
    //构造查询条件
    //====================
    var buildQuery = function () {
        var data = '';

        var sel = $("#" + settings.pageControls.queryId).find(".ui-select");
        sel.each(function () {
            var it = $(this);
            var pro = it.attr("queryPro");
            pro = !!pro ? pro : it.attr("id");
            var val = it.attr("data-value");
            if (!val)
                val = "";
            data += pro + ":'" + val + "',";
        })

        var ipt = $("#" + settings.pageControls.queryId).find("input");
        ipt.each(function () {
            var it = $(this);
            var pro = it.attr("queryPro");
            pro = !!pro ? pro : it.attr("id");
            var val = it.val();
            if (!val)
                val = "";
            data += pro + ":'" + val + "',";
        })

        if (!!data)
            data = '{' + data.substring(0, data.length - 1) + '}';

        return data;
    };
    //====================
    //根据条件查询
    //====================
    this.queryData = function () {
        var queryJson = events.onBuildQuery();
        var $gridTable = grid;
        $gridTable.jqGrid('setGridParam', {
            postData: { queryJson: queryJson },
            page: 1
        }).trigger('reloadGrid');
    };
    //====================
    //重置
    //====================
    var reset = function () {
        var sel = $("#" + settings.pageControls.queryId).find(".ui-select");
        sel.each(function () {
            $(this).resetCombobox("==全部==", "")
        })

        var ipt = $("#" + settings.pageControls.queryId).find("input:text,input:hidden");
        ipt.each(function () {
            $(this).val("");
        })
        var chk = $("#" + settings.pageControls.queryId).find("input:checkbox");
        chk.each(function () {
            $(this).removeAttr("checked");
        })
    };
    //====================
    //添加
    //====================
    var add = function () {
        if (!!evtArgs.addArg && !!evtArgs.addArg.url) {
            var dlgArg = {
                id: 'Form',
                title: '添加',
                //url: '/LllegalStandard/LllegalStandard/Form',
                width: ($(top.window).width() - 200) + "px",
                height: ($(top.window).height() - 100) + "px",
                callBack: function (iframeId) {
                    debugger;
                    if (top.frames[iframeId].AcceptClick)
                    {
                        top.frames[iframeId].AcceptClick();
                    }
                }
            };
            $.extend(dlgArg, evtArgs.addArg);
            var lnkChar = dlgArg.url.indexOf("?") < 0 ? "?" : "&";
            dlgArg.url += lnkChar + "actiontype=add";
            dialogOpen(dlgArg);
        }
    };
    //====================
    //修改
    //====================
    edit = function () {
        if (!!evtArgs.editArg && !!evtArgs.editArg.url) {
            var keyValue = grid.jqGridRowValue('id');
            if (checkedRow(keyValue)) {
                var dlgArg = {
                    id: 'Form',
                    title: '编辑',
                    width: ($(top.window).width() - 200) + "px",
                    height: ($(top.window).height() - 100) + "px",
                    callBack: function (iframeId) {
                        if (top.frames[iframeId].AcceptClick)
                            top.frames[iframeId].AcceptClick();
                    }
                };
                $.extend(dlgArg, evtArgs.editArg);
                var lnkChar = dlgArg.url.indexOf("?") < 0 ? "?" : "&";
                dlgArg.url += lnkChar + "actiontype=edit&keyValue=" + keyValue;
                dialogOpen(dlgArg);
            }
        } else {
            dialogMsg('无效参数！', 0);
        }
    };
    //====================
    //删除
    //====================
    del = function () {
        if (!!evtArgs.delArg && !!evtArgs.delArg.url) {
            var keyValue = grid.jqGridRowValue('id');
            if (!!keyValue) {
                var delOpt = {
                    param: { keyValue: keyValue },
                    success: function (data) {
                        grid.trigger('reloadGrid');
                    }
                };
                $.extend(delOpt, evtArgs.delArg);
                $.RemoveForm(delOpt);
            } else {
                dialogMsg('请选择需要删除的数据！', 0);
            }
        } else {
            dialogMsg('无效参数！', 0);
        }
    };
    //====================
    //详细
    //====================
    detail = function () {
        if (!!evtArgs.detailArg && !!evtArgs.detailArg.url) {
            var keyValue = grid.jqGridRowValue('id');
            if (checkedRow(keyValue)) {
                var dlgArg = {
                    id: "Detail",
                    title: '详情',
                    width: ($(top.window).width() - 200) + "px",
                    height: ($(top.window).height() - 100) + "px",
                    btn: null
                };
                $.extend(dlgArg, evtArgs.detailArg);
                var lnkChar = dlgArg.url.indexOf("?") < 0 ? "?" : "&";
                dlgArg.url += lnkChar + "actiontype=view&keyValue=" + keyValue;
                dialogOpen(dlgArg);
            }
        } else {
            dialogMsg('无效参数！', 0);
        }
    };
    //====================
    //导入
    //====================
    var imp = function () {
        if (!!evtArgs.impArg && !!evtArgs.impArg.url) {
            var dlgArg = {
                id: "Import",
                title: '导入',
                width: "500px",
                height: "450px",
                callBack: function (iframeId) {
                    if (top.document.getElementById(iframeId))
                        top.document.getElementById(iframeId).contentWindow.AcceptClick();
                }
            };
            $.extend(dlgArg, evtArgs.impArg);
            dialogOpen(dlgArg);
        }
        else {
            dialogMsg('无效参数！', 0);
        }
    };
    //====================
    //导出
    //====================
    var exp = function () {
        if (!!evtArgs.expArg && !!evtArgs.expArg.url) {
            var expArg = {
            };
            $.extend(expArg, evtArgs.expArg);
            //导出数据...
            var url = evtArgs.expArg.url;
            var lnkChar = url.indexOf("?") < 0 ? "?" : "&";
            url += lnkChar + getExpQuery();
            window.location.href = url;
        }
    };
    //====================
    //验证选择的数据
    //====================
    var checkedRow = function (id) {
        var isOK = true;
        if (id == undefined || id == "" || id == 'null' || id == 'undefined') {
            isOK = false;
            dialogMsg('您没有选中任何数据项,请选中后再操作！', 0);
        } else if (id.split(",").length > 1) {
            isOK = false;
            dialogMsg('很抱歉,一次只能选择一条记录！', 0);
        }
        return isOK;
    };
    //
    //获取导出条件
    //
    getExpQuery = function () {        
        return "queryJson=" + events.onBuildQuery() + "&sortname=" + grid.jqGrid("getGridParam", "sortname") + "&sortorder=" + grid.jqGrid("getGridParam", "sortorder");
    };
}
//====================
//页面按钮事件args
//{
//    Id: '',    
//    url: null,
//    onClick: null,
//    args: null
//}
//====================
function pageEvent(args) {
    //规则执行条件及执行事件
    var r = {
        Id: '',
        url: null,
        onClick: null,
        args: null
    };
    $.extend(r, args);
    //规则成立，执行相应方法。
    this.touch = function () {
        var args = "";
        if (!!r.args && r.args.indexOf("javascript") > -1)
            args = eval(r.args);
        else
            args = r.args;
        if (!!r.onClick) {
            if ($.isFunction(r.onClick))
                r.onClick(args);
            else
                eval(r.onClick);
        }
        else if (!!r.url) {
            var url = r.url;
            var lnkChar = url.indexOf("?") < 0 ? "?" : "&";
            url += lnkChar + args;
            window.location.href = url;
        }
    }
}