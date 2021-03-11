function gridqx($gridTable) {
    var rows = $gridTable.jqGrid("getRowData");//获取当前页记录行数据
    //查询用户对该模块的数据操作权限
    $.post(top.contentPath + "/AuthorizeManage/PermissionJob/GetDataAuthority", { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (data) {
        var jsonArr = eval("(" + data + ")");
        $(rows).each(function (i, item) {
            var uId = item.userid;
            var dCode = item.departmentcode; //获取记录创建人的所属部门Code
            var oCode = item.organizecode;  //获取记录创建人的所属机构Code
            var btns = $("td[aria-describedby='gridTable_Oper']").eq(i).children();//获取操作列中定义的操作按钮
            var html = "";
            //如果操作列中没有定义任何按钮则根据系统权限设置自动绑定操作按钮
            if (btns.length == 0) {
                $(jsonArr).each(function (j, item1) {
                    var authType = parseInt(item1.authorizetype);//获取数据操作权限范围.1：本人,2：本部门，3：本部门及下属部门，4：本机构，5：全部
                    switch (authType) {
                        //本用户
                        case 1:
                            html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                            //本部门
                        case 2:
                            html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                            //本子部门
                        case 3:
                            html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                            //本机构
                        case 4:
                            html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                        case 5:
                            html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
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
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本部门
                        case 2:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本子部门
                        case 3:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本机构
                        case 4:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
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
//康巴什基础管理用
function kbsgridqx($gridTable) {
    var rows = $gridTable.jqGrid("getRowData");//获取当前页记录行数据
    //查询用户对该模块的数据操作权限
    $.post(top.contentPath + "/AuthorizeManage/PermissionJob/GetDataAuthority", { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (data) {
        var jsonArr = eval("(" + data + ")");
        $(rows).each(function (i, item) {
            var uId = item.userid;
            var dCode = item.departmentcode; //获取记录创建人的所属部门Code
            var oCode = item.organizecode;  //获取记录创建人的所属机构Code
            var btns = $("td[aria-describedby='gridTable_Oper']").eq(i).children();//获取操作列中定义的操作按钮
            var html = "";
            //如果操作列中没有定义任何按钮则根据系统权限设置自动绑定操作按钮
            if (btns.length == 0) {
                $(jsonArr).each(function (j, item1) {
                    var authType = parseInt(item1.authorizetype);//获取数据操作权限范围.1：本人,2：本部门，3：本部门及下属部门，4：本机构，5：全部
                    switch (authType) {
                        //本用户
                        case 1:
                            html += "<a href=\"javascript:" + item1.actionname + "('" + rows[i].ID + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                            //本部门
                        case 2:
                            html += "<a href=\"javascript:" + item1.actionname + "('" + rows[i].ID + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                            //本子部门
                        case 3:
                            html += "<a href=\"javascript:" + item1.actionname + "('" + rows[i].ID + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                            //本机构
                        case 4:
                            html += "<a href=\"javascript:" + item1.actionname + "('" + rows[i].ID + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                        case 5:
                            html += "<a href=\"javascript:" + item1.actionname + "('" + rows[i].ID + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
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
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本部门
                        case 2:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本子部门
                        case 3:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本机构
                        case 4:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
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

//监测记录用
function gridqxyh($gridTable, userid) {
    var rows = $gridTable.jqGrid("getRowData");//获取当前页记录行数据
    //查询用户对该模块的数据操作权限
    $.post(top.contentPath + "/AuthorizeManage/PermissionJob/GetDataAuthority", { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (data) {
        var jsonArr = eval("(" + data + ")");
        $(rows).each(function (i, item) {
            id = item.hid;
            var uId = item.createuserid;//获取记录创建人的Id
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
                                if (btns.find("a[name='" + item1.encode + "']").length == 0) {
                                    html += "<a href=\"javascript:" + item1.actionname + "('" + id + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                }
                            } else {
                                if (btns.find("a[name='" + item1.encode + "']").length > 0) {
                                    btns.find("a[name='" + item1.encode + "']").remove();
                                }
                            }
                            break;
                            //本部门
                        case 2:
                            if (top.currUserDeptCode == dCode) {
                                if (btns.find("a[name='" + item1.encode + "']").length == 0) {
                                    html += "<a href=\"javascript:" + item1.actionname + "('" + id + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                }
                            } else {
                                if (btns.find("a[name='" + item1.encode + "']").length > 0) {
                                    btns.find("a[name='" + item1.encode + "']").remove();
                                }
                            }
                            break;
                            //本子部门
                        case 3:
                            if (dCode.indexOf(top.currUserDeptCode) >= 0) {
                                if (btns.find("a[name='" + item1.encode + "']").length == 0) {
                                    html += "<a href=\"javascript:" + item1.actionname + "('" + id + "')\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                }
                            } else {
                                if (btns.find("a[name='" + item1.encode + "']").length > 0) {
                                    btns.find("a[name='" + item1.encode + "']").remove();
                                }
                            }
                            break;
                            //本机构
                        case 4:
                            if (oCode == top.currUserOrgCode) {
                                if (item1.encode == "addyh" || item1.encode == "selecthid") {//是否是隐患按钮
                                    if (rows[i].isexcessive == "是") {
                                        html += "<a style='margin-left: 5px;' href=\"javascript:" + item1.actionname + "('" + rows[i].hid + "','hazard')\" title=\"" + item1.fullname + "\"  class='btn btn-default' ><i class=\"" + item1.faimage + "\"></i>" + item1.fullname + "</a>";
                                    } else {
                                        html += "<a style='opacity:0.2;margin-left: 5px;' title=\"" + item1.fullname + "\"  class='btn btn-default' ><i class=\"" + item1.faimage + "\"></i>" + item1.fullname + "</a>";
                                    }
                                } else {
                                    html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                }
                            } else {
                                if (btns.find("a[name='" + item1.encode + "']").length > 0) {
                                    btns.find("a[name='" + item1.encode + "']").remove();
                                }
                            }

                            break;
                        case 5:
                            if (btns.find("a[name='" + item1.encode + "']").length == 0) {
                                if (item1.encode == "addyh" || item1.encode == "selecthid") {//是否是隐患按钮
                                    if (rows[i].isexcessive == "是") {
                                        html += "<a style='margin-left: 5px;' href=\"javascript:" + item1.actionname + "('" + rows[i].hid + "','hazard')\" title=\"" + item1.fullname + "\"  class='btn btn-default' ><i class=\"" + item1.faimage + "\"></i>" + item1.fullname + "</a>";
                                    } else {
                                        html += "<a style='opacity:0.2;margin-left: 5px;' title=\"" + item1.fullname + "\"  class='btn btn-default' ><i class=\"" + item1.faimage + "\"></i>" + item1.fullname + "</a>";
                                    }
                                } else {
                                    html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                }
                            }
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
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本部门
                        case 2:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本子部门
                        case 3:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本机构
                        case 4:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
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

//物料系统入场开票专用
function gridqxrc($gridTable) {
    var rows = $gridTable.jqGrid("getRowData");//获取当前页记录行数据
    //查询用户对该模块的数据操作权限
    $.post(top.contentPath + "/AuthorizeManage/PermissionJob/GetDataAuthority", { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (data) {
        var jsonArr = eval("(" + data + ")");
        $(rows).each(function (i, item) {
            var uId = item.userid;
            var dCode = item.departmentcode; //获取记录创建人的所属部门Code
            var oCode = item.organizecode;  //获取记录创建人的所属机构Code
            var btns = $("td[aria-describedby='gridTable_Oper']").eq(i).children();//获取操作列中定义的操作按钮
            var html = "";
            //如果操作列中没有定义任何按钮则根据系统权限设置自动绑定操作按钮
            if (btns.length == 0) {
                $(jsonArr).each(function (j, item1) {
                    var authType = parseInt(item1.authorizetype);//获取数据操作权限范围.1：本人,2：本部门，3：本部门及下属部门，4：本机构，5：全部
                    switch (authType) {
                        //本用户
                        case 1:
                            html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                            //本部门
                        case 2:
                            html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                            //本子部门
                        case 3:
                            html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                            //本机构
                        case 4:
                            //html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            if (item1.encode == "edit" || item1.encode == "delete") {//是否显示编辑按钮
                                if (rows[i].getstamptime == "" && rows[i].outcu != "1") {
                                    html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                }
                            } else {
                                html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            }
                            break;
                        case 5:
                            //html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            if (item1.encode == "edit" || item1.encode == "delete") {//是否显示编辑按钮
                                if (rows[i].getstamptime == "" && rows[i].outcu != "1") {
                                    html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                }
                            } else {
                                html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            }
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
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本部门
                        case 2:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本子部门
                        case 3:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本机构
                        case 4:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
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


//物料系统称重计量专用
function gridqxcz($gridTable) {
    var rows = $gridTable.jqGrid("getRowData");//获取当前页记录行数据
    //查询用户对该模块的数据操作权限
    $.post(top.contentPath + "/AuthorizeManage/PermissionJob/GetDataAuthority", { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }, function (data) {
        var jsonArr = eval("(" + data + ")");
        $(rows).each(function (i, item) {
            var uId = item.userid;
            var dCode = item.departmentcode; //获取记录创建人的所属部门Code
            var oCode = item.organizecode;  //获取记录创建人的所属机构Code
            var btns = $("td[aria-describedby='gridTable_Oper']").eq(i).children();//获取操作列中定义的操作按钮
            var html = "";
            //如果操作列中没有定义任何按钮则根据系统权限设置自动绑定操作按钮
            if (btns.length == 0) {
                $(jsonArr).each(function (j, item1) {
                    var authType = parseInt(item1.authorizetype);//获取数据操作权限范围.1：本人,2：本部门，3：本部门及下属部门，4：本机构，5：全部
                    switch (authType) {
                        //本用户
                        case 1:
                            html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                            //本部门
                        case 2:
                            html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                            //本子部门
                        case 3:
                            html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            break;
                            //本机构
                        case 4:
                            //html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            if (item1.encode == "delete") {//是否显示编辑按钮
                                if (rows[i].isout != "1") {
                                    html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                }
                            } else {
                                html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            }
                            break;
                        case 5:
                            //html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            if (item1.encode == "delete") {//是否显示编辑按钮
                                if (rows[i].isout != "1") {
                                    html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                                }
                            } else {
                                html += "<a href=\"javascript:" + item1.actionname + "()\" title=\"" + item1.fullname + "\"><i class=\"" + item1.faimage + "\"></i></a>";
                            }
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
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本部门
                        case 2:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本子部门
                        case 3:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
                            break;
                            //本机构
                        case 4:
                            $(btns).find("a[name='" + item1.itemcode + "']").remove();
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