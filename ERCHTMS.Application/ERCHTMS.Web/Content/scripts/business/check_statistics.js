$(function () {
    InitialPage();
});


//加载统计图一
function LoadContainer1(data) {
    //图形
    $('#container1').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: '各等级隐患统计'
        },
        xAxis: {
            categories: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
        },
        yAxis: {
            min: 0,
            allowDecimal: false,
            title: {
                text: '数量'
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                }
            }
        },
        legend: {
            align: 'right',
            x: -30,
            verticalAlign: 'top',
            y: 25,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
            borderColor: '#CCC',
            borderWidth: 1,
            shadow: false
        },
        tooltip: {
            headerFormat: '<b>{point.x}</b><br/>',
            pointFormat: '{series.name}: {point.y}<br/>合计: {point.stackTotal}'
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true,
                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                    style: {
                        textShadow: '0 0 3px black'
                    }
                }
            }
        },
        series: data,
        lang: { noData: "暂无数据可显示!" },
        noData: {
            position: {
                align: 'center',
                verticalAlign: 'middle'
            },
            attr: {
                'stroke-width': 1
            },
            style: {
                fontWeight: 'bold',
                fontSize: '15px',
                color: '#202030'
            }
        }
    });
}

//加载趋势图
function LoadContainer2(data) {

    $('#container2').highcharts({
        chart: {
            type: 'spline'
        },
        title: {
            text: '隐患数量变化趋势图'
        },
        subtitle: {
            text: ''
        },
        xAxis: {
            categories: ['1月', '2月', '3月', '4月', '5月', '6月',
                '7月', '8月', '9月', '10月', '11月', '12月']
        },
        yAxis: {
            min: 0,
            allowDecimal: false,
            title: {
                text: '数量'
            },
            labels: {
                formatter: function () {
                    return this.value;
                }
            }
        },
        tooltip: {
            crosshairs: true,
            shared: true
        },
        plotOptions: {
            spline: {
                marker: {
                    radius: 4,
                    lineColor: '#666666',
                    lineWidth: 1
                },
                dataLabels: {
                    enabled: true,  //在数据点上显示对应的数据值
                    formatter: function () { //格式化提示信息
                        return '<b>' + this.series.name + '</b>' + this.x + ':' + this.y + '条';
                    }
                }
            }
        },
        series: data,
        lang: { noData: "暂无数据可显示!" },
        noData: {
            position: {
                align: 'center',
                verticalAlign: 'middle'
            },
            attr: {
                'stroke-width': 1
            },
            style: {
                fontWeight: 'bold',
                fontSize: '15px',
                color: '#202030'
            }
        }
    });
}

//加载对比图
function LoadContainer3(xdata, sdata) {

    $('#container3').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: '各等级隐患统计'
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            min: 0,
            title: {
                text: '数量'
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                }
            }
        },
        legend: {
            align: 'right',
            x: -30,
            verticalAlign: 'top',
            y: 25,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
            borderColor: '#CCC',
            borderWidth: 1,
            shadow: false
        },
        tooltip: {
            pointFormat: '{series.name}: {point.y}<br/>合计: {point.stackTotal}'
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true,
                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                }
            }
        },
        series: xdata,
        drilldown: { series: sdata },
        lang: { noData: "暂无数据可显示!" },
        noData: {
            position: {
                align: 'center',
                verticalAlign: 'middle'
            },
            attr: {
                'stroke-width': 1
            },
            style: {
                fontWeight: 'bold',
                fontSize: '15px',
                color: '#202030'
            }
        }
    });
}

//加载表格1
function GetGridTable1(tdata) {

    var selectedRowIndex = 0;
    var $gridTable = $('#gridTable1');
    //隐患基本信息
    $gridTable.jqGrid({
        autowidth: true,
        height: parseFloat($(window).height() / 2 - 100),
        datatype: "local",
        data: tdata,
        colModel: [
            { label: '月份', name: 'month', index: 'month', width: 150, align: 'center', sortable: true },
            {
                label: '一般隐患', name: 'ordinaryhid', index: 'ordinaryhid', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.ordinaryhid;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(0,'" + rowObject.month + "','一般隐患')>" + rowObject.ordinaryhid + "</a>";
                    }
                    return rval;
                }
            },
            {
                label: '重大隐患', name: 'importanhid', index: 'importanhid', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.importanhid;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(0,'" + rowObject.month + "','重大隐患')>" + rowObject.importanhid + "</a>";
                    }
                    return rval;
                }
            },
            {
                label: '合计', name: 'total', index: 'total', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.total;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(0,'" + rowObject.month + "','')>" + rowObject.total + "</a>";
                    }
                    return rval;
                }
            }
        ],
        sortname: 'month',
        rownumbers: true,
        shrinkToFit: true,
        gridview: true,
        onSelectRow: function () {
            selectedRowIndex = $("#" + this.id).getGridParam('selrow');
        },
        gridComplete: function () {
            $("#" + this.id).setSelection(selectedRowIndex, false);
        }
    });
}

//加载表格2
function GetGridTable2(tdata) {
    var selectedRowIndex = 0;
    var $gridTable = $('#gridTable2');
    var p = $gridTable.jqGrid('getGridParam');
    if (!!p && !!p.data) {
        //按条件加载各等级隐患对比图
        $('#gridTable2').jqGrid('clearGridData');
        $('#gridTable2').jqGrid('setGridParam', { dataType: 'local', data: tdata }).trigger('reloadGrid');
    }
    else {
        //隐患基本信息
        $gridTable.jqGrid({
            autowidth: true,
            height: parseFloat($(window).height() / 2 - 100),
            datatype: "local",
            data: tdata,
            colModel: [
               { label: '主键', name: 'createuserdeptcode', index: 'createuserdeptcode', width: 100, align: 'center', hidden: true },
                {
                    label: '部门名称', name: 'fullname', index: 'fullname', width: 150, align: 'center', sortable: true
                },
                {
                    label: '一般隐患', name: 'ordinaryhid', index: 'ordinaryhid', width: 150, align: 'center', sortable: true,
                    formatter: function (cellvalue, options, rowObject) {
                        var rval = rowObject.ordinaryhid;
                        if (rval > 0) {
                            rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(1,'" + rowObject.createuserdeptcode + "','一般隐患')>" + rowObject.ordinaryhid + "</a>";
                        }
                        return rval;
                    }
                },
                {
                    label: '重大隐患', name: 'importanhid', index: 'importanhid', width: 150, align: 'center', sortable: true,
                    formatter: function (cellvalue, options, rowObject) {
                        var rval = rowObject.importanhid;
                        if (rval > 0) {
                            rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(1,'" + rowObject.createuserdeptcode + "','重大隐患')>" + rowObject.importanhid + "</a>";
                        }
                        return rval;
                    }
                },
                {
                    label: '合计', name: 'total', index: 'total', width: 150, align: 'center', sortable: true,
                    formatter: function (cellvalue, options, rowObject) {
                        var rval = rowObject.total;
                        if (rval > 0) {
                            rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(1,'" + rowObject.createuserdeptcode + "','')>" + rowObject.total + "</a>";
                        }
                        return rval;
                    }
                }
            ],
            sortname: 'sortcode',
            rownumbers: true,
            shrinkToFit: true,
            gridview: true,
            onSelectRow: function () {
                selectedRowIndex = $("#" + this.id).getGridParam('selrow');
            },
            gridComplete: function () {
                $("#" + this.id).setSelection(selectedRowIndex, false);
            }
        });
    }
}


//跳转到隐患列表页面
function GoHiddenList(qtype, qargs, qval) {

    var url = "";
    //选择的年度
    var qyear = $("#TimeScope").attr("data-text"); //年度()

    var qdeptcode = $("#DepartmentCode").val(); //单位

    var qhidpoint = $("#HidPoint").val();  //区域

    var qchecktype = $("#CheckDataType").attr("data-text") == "请选择" ? "" : $("#CheckDataType").attr("data-text"); //检查类型

    //统计条件构成:单位、年度、区域、隐患级别、检查类型，月份

    //按月份
    if (qtype == 0) {

        var qyearmonth = qyear + "-" + qargs; //年-月 2018-01

        url = '/HiddenTroubleManage/HTBaseInfo/Index?mode=yhtjinfo&pMode=1&qdeptcode=' + qdeptcode + '&qrankname=' + qval + '&qyearmonth=' + qyearmonth + '&code=' + qhidpoint + '&qchecktype=' + qchecktype;
    }
    else  //按部门
    {
        url = '/HiddenTroubleManage/HTBaseInfo/Index?mode=yhdjinfo&pMode=1&qdeptcode=' + qargs + '&qrankname=' + qval + '&qyear=' + qyear + '&code=' + qhidpoint + '&qchecktype=' + qchecktype;
    }
    url += "&layerId=HTWindow";
    var idx = dialogOpen({
        id: 'HTWindow',
        title: '隐患列表',
        url: url,
        btns: 1,
        btn: ["关闭"],
        width: ($(top.window).width() - 200) + "px",
        height: ($(top.window).height() - 100) + "px",
        callBack: function (iframeId) {
            top.layer.close(idx);
        }
    });
}


//初始化页面
function InitialPage() {

    //查询事件
    $("#btn_Search").click(function () {
        var curStatistics = $("#CurPanel").val();
        var queryJson = {
            deptCode: $("#DepartmentCode").val(),
            year: $("#TimeScope").attr("data-value"),
            hidPoint: $("#HidPoint").val(),
            hidRank: $("button[data-id='HidRank']").attr("title"),
            ischeck: ischeck,
            checkType: $("#CheckDataType").attr("data-value")
        };

        switch (curStatistics) {
            case "统计图":
                //加载图表
                $.ajax({
                    type: "get",
                    url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberColumn", //饼图
                    data: { queryJson: JSON.stringify(queryJson) },
                    success: function (data) {
                        if (!!data) {
                            var nData = eval("(" + data + ")");

                            LoadContainer1(nData.sdata);

                            $('#gridTable1').jqGrid('clearGridData');
                            $('#gridTable1').jqGrid('setGridParam', { dataType: 'local', data: nData.tdata }).trigger('reloadGrid');
                        }
                    }
                });
                break;
            case "趋势图":
                $.ajax({
                    type: "get",
                    url: "../../HiddenTroubleManage/HTStatistics/QueryCheckHidTendency",
                    data: { queryJson: JSON.stringify(queryJson) },
                    success: function (data) {
                        if (!!data) {
                            var nData = eval("(" + data + ")");

                            LoadContainer2(nData.sdata);

                            $('#gridTable1').jqGrid('clearGridData');
                            $('#gridTable1').jqGrid('setGridParam', { dataType: 'local', data: nData.tdata }).trigger('reloadGrid');
                        }
                    }
                });
                break;
            case "对比图":
                $.ajax({
                    type: "get",
                    url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberComparison",  //获取对比图
                    data: { queryJson: JSON.stringify(queryJson) },
                    success: function (data) {
                        if (!!data) {
                            var nData = eval("(" + data + ")");

                            //按条件加载各等级隐患对比图
                            $('#gridTable2').jqGrid('clearGridData');
                            $('#gridTable2').jqGrid('setGridParam', { dataType: 'local', data: nData.tdata }).trigger('reloadGrid');

                            //按条件加载各等级隐患列表
                            LoadContainer3(nData.xdata, nData.sdata);
                        }
                    }
                });
                break;
        }
    });


    //查询回车
    $('#txt_Keyword').bind('keypress', function (event) {
        if (event.keyCode == "13") {
            $('#btn_Search').trigger("click");
        }
    });
}

function showChart(obj, sType) {
    var queryJson = {
        deptCode: $("#DepartmentCode").val(),
        year: $("#TimeScope").attr("data-value"),
        hidPoint: $("#HidPoint").val(),
        hidRank: $("button[data-id='HidRank']").attr("title"),
        ischeck: ischeck,
        checkType: $("#CheckDataType").attr("data-value")
    };

    switch (sType) {
        case "统计图":
            //改变当前按钮样式
            $(obj).css("background-color", "#337ab7");
            $(obj).css("color", "#fff");
            $(obj).find("i:eq(0)").css("color", "#fff");
            if ($("#CurPanel").val() == "趋势图") {
                $("#lr-panel-two").css("background-color", "#fff");
                $("#lr-panel-two").css("color", "#333");
                $("#lr-panel-two i").css("color", "#666666");
            }
            if ($("#CurPanel").val() == "对比图") {
                $("#lr-panel-three").css("background-color", "#fff");
                $("#lr-panel-three").css("color", "#333");
                $("#lr-panel-three i").css("color", "#666666");
            }

            $("#gbox_gridTable1").css("display", "block");
            $("#gridTable1").css("display", "block");

            $("#gbox_gridTable2").css("display", "none");
            $("#gridTable2").css("display", "none");

            $("#container1").css("display", "block");
            $("#container2").css("display", "none");
            $("#container3").css("display", "none");


            //获取统计图
            $.ajax({
                type: "get",
                url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberColumn",  //获取饼图
                data: { queryJson: JSON.stringify(queryJson) },
                success: function (data) {
                    if (!!data) {
                        var nData = eval("(" + data + ")");

                        LoadContainer1(nData.sdata);

                        $('#gridTable1').jqGrid('clearGridData');
                        $('#gridTable1').jqGrid('setGridParam', { dataType: 'local', data: nData.tdata }).trigger('reloadGrid');
                    }
                }
            });
            break;
        case "趋势图":
            ////改变当前按钮样式
            $(obj).css("background-color", "#337ab7");
            $(obj).css("color", "#fff");
            $(obj).find("i:eq(0)").css("color", "#fff");
            if ($("#CurPanel").val() == "统计图") {
                $("#lr-panel-one").css("background-color", "#fff");
                $("#lr-panel-one").css("color", "#333");
                $("#lr-panel-one i").css("color", "#666666");
            }
            if ($("#CurPanel").val() == "对比图") {
                $("#lr-panel-three").css("background-color", "#fff");
                $("#lr-panel-three").css("color", "#333");
                $("#lr-panel-three i").css("color", "#666666");
            }

            $("#gbox_gridTable1").css("display", "block");
            $("#gridTable1").css("display", "block");

            $("#gbox_gridTable2").css("display", "none");
            $("#gridTable2").css("display", "none");


            $("#container1").css("display", "none");
            $("#container2").css("display", "block");
            $("#container3").css("display", "none");


            $.ajax({
                type: "get",
                url: "../../HiddenTroubleManage/HTStatistics/QueryCheckHidTendency",
                data: { queryJson: JSON.stringify(queryJson) },
                success: function (data) {
                    if (!!data) {
                        var nData = eval("(" + data + ")");

                        LoadContainer2(nData.sdata);

                        $('#gridTable1').jqGrid('clearGridData');
                        $('#gridTable1').jqGrid('setGridParam', { dataType: 'local', data: nData.tdata }).trigger('reloadGrid');
                    }
                }
            });

            break;
        case "对比图":
            $(obj).css("background-color", "#337ab7");
            $(obj).css("color", "#fff");
            $(obj).find("i:eq(0)").css("color", "#fff");
            if ($("#CurPanel").val() == "统计图") {
                $("#lr-panel-one").css("background-color", "#fff");
                $("#lr-panel-one").css("color", "#333");
                $("#lr-panel-one i").css("color", "#666666");
            }
            if ($("#CurPanel").val() == "趋势图") {
                $("#lr-panel-two").css("background-color", "#fff");
                $("#lr-panel-two").css("color", "#333");
                $("#lr-panel-two i").css("color", "#666666");
            }

            $("#gbox_gridTable1").css("display", "none");
            $("#gridTable1").css("display", "none");

            $("#gbox_gridTable2").css("display", "block");
            $("#gridTable2").css("display", "block");

            $("#container1").css("display", "none");
            $("#container2").css("display", "none");
            $("#container3").css("display", "block");


            $.ajax({
                type: "get",
                url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberComparison",  //获取对比图
                data: { queryJson: JSON.stringify(queryJson) },
                success: function (data) {
                    if (!!data) {
                        var nData = eval("(" + data + ")");

                        //按条件加载各等级隐患对比图
                        GetGridTable2(nData.tdata);
                        //按条件加载各等级隐患列表
                        LoadContainer3(nData.xdata, nData.sdata);
                    }
                }
            });
            break;
    }
    $("#CurPanel").val(sType); //标记当前活动的统计图
}


