$(function () {
    Highcharts.setOptions({
        exporting: {
            url: "../../HiddenTroubleManage/HTStatistics/Export",
            enabled: true,
            filename: 'MyChart',
            width: 1660,
            printMaxWidth: 2248,
            sourceWidth: 1660,
            sourceHeight: 768
        },
        lang: {
            printChart: "打印图表",
            downloadJPEG: "下载JPEG图片",
            downloadPDF: "下载PDF文件",
            downloadPNG: "下载PNG图片",
            downloadSVG: "下载SVG文件"
        }
    });
    InitialPage();
});


//加载统计图一
function LoadContainer1(data) {

    //图形
    $('#container1').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        tooltip: { pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>' },
        exporting: { enabled: false },
        title: { text: '隐患数量统计' },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b><br/>{point.percentage:.1f} %',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    }
                }
            }
        },
        series: [{
            name: '百分比',
            colorByPoint: true,
            data: data
        }],
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

//加载统计图二
function LoadContainer2(data) {

    //图形
    $('#container2').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        tooltip: { pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>' },
        exporting: { enabled: false },
        title: { text: '隐患区域分布统计' },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b><br/>{point.percentage:.1f} %',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    }
                }
            }
        },
        series: [{
            name: '百分比',
            colorByPoint: true,
            data: data
        }],
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
function LoadContainer3(data) {

    $('#container3').highcharts({
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
                        return this.x + ':' + this.y + '条</b>'; // this.series.name + '</b>' +
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

//加载对比图(各部门登记的)
function LoadContainer4(xdata, sdata) {

    $('#container4').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: '各部门登记隐患统计'
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

//加载对比图
function LoadContainer5(xdata, sdata) {
    $('#container5').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: '各区域对比情况统计'
        },
        xAxis: {
            categories: xdata  //['xx区域1', 'xx区域2', 'xx区域3','xx区域4', 'xx区域5','xx区域6']
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
        series: sdata,
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
function GetGridTable1(tdata, iscompany) {
    var isShow = iscompany == "1" ? false : true;
    var selectedRowIndex = 0;
    var $gridTable = $('#gridTable1');
    //隐患基本信息
    $gridTable.jqGrid({
        autowidth: true,
        height: parseFloat($(window).height() / 2 - 100),
        datatype: "local",
        data: tdata,
        colModel: [
           { label: '主键', name: 'hidpoint', index: 'hidpoint', width: 100, align: 'center', hidden: true },
            {
                label: '区域名称', name: 'hidpointname', index: 'hidpointname', width: 150, align: 'center', sortable: true, hidden: isShow
            },
            {
                label: '一般隐患', name: 'ordinaryhid', index: 'ordinaryhid', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var point = !!rowObject.hidpoint ? rowObject.hidpoint : "";
                    var rval = rowObject.ordinaryhid;
                    if (rval > 0) {

                      
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(0,'" + point + "','一般隐患')>" + rowObject.ordinaryhid + "</a>";
                    }
                    return rval;
                }
            },
            {
                label: '重大隐患', name: 'importanhid', index: 'importanhid', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.importanhid;
                    var point = !!rowObject.hidpoint ? rowObject.hidpoint : "";
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(0,'" + point + "','重大隐患')>" + rowObject.importanhid + "</a>";
                    }
                    return rval;
                }
            },
            {
                label: '合计', name: 'total', index: 'total', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.total;
                    var point = !!rowObject.hidpoint ? rowObject.hidpoint : "";
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(0,'" + point + "','')>" + rowObject.total + "</a>";
                    }
                    return rval;
                }
            }
        ],
        sortname: 'hidpoint',
        rowNum: 10000,
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
function GetGridTable2(queryJson) {
    var selectedRowIndex = 0;
    var $gridTable = $('#gridTable2');
    var qtype = 1;
    if (iscompany == "0") {
        qtype = 2;
    }
    $gridTable.jqGrid({
        url: "../../HiddenTroubleManage/HTStatistics/QueryHidNUmberComparisonList",
        datatype: "json",
        treeGrid: true,
        treeGridModel: "nested",
        ExpandColumn: "fullname",
        postData: { queryJson: JSON.stringify(queryJson) },
        height: parseFloat($(window).height() / 2 - 100),
        autowidth: true,
        colModel: [
            { label: '部门名称', name: 'fullname', index: 'fullname', width: 150, align: 'center', sortable: false },
            { label: '主键', name: 'departmentid', index: 'departmentid', width: 100, align: 'center', hidden: true },
            { label: '父节点', name: 'parent', index: 'parent', width: 100, align: 'center', hidden: true },
            { label: '部门编码', name: 'createuserdeptcode', index: 'createuserdeptcode', width: 100, align: 'center', hidden: true },
            { label: 'haschild', name: 'haschild', hidden: true },
            {
                label: '一般隐患', name: 'ordinaryhid', index: 'ordinaryhid', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.ordinaryhid;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(" + qtype + ",'" + rowObject.createuserdeptcode + "','一般隐患')>" + rowObject.ordinaryhid + "</a>";
                    }
                    return rval;
                }
            },
            {
                label: '重大隐患', name: 'importanhid', index: 'importanhid', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.importanhid;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(" + qtype + ",'" + rowObject.createuserdeptcode + "','重大隐患')>" + rowObject.importanhid + "</a>";
                    }
                    return rval;
                }
            },
            {
                label: '合计', name: 'total', index: 'total', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.total;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(" + qtype + ",'" + rowObject.createuserdeptcode + "','')>" + rowObject.total + "</a>";
                    }
                    return rval;
                }
            }
        ],
        treeReader: {
            level_field: "level",
            parent_id_field: "parent",
            leaf_field: "isLeaf",
            expanded_field:"expanded"
        },
        rowNum: "all",
        rownumbers: true,
        rownumWidth:70,
        onSelectRow: function (rowid) {
            selectedRowIndex = $("#" + this.id).getGridParam('selrow');
        },
        gridComplete: function () {
            $("#" + this.id).setSelection(selectedRowIndex, false);
        }
    });
}

//跳转到隐患列表页面
function GoHiddenList(qtype, qargs, qval) {

    var url = "";
    //选择的年度
    var qyear = $("#TimeScope").attr("data-text"); //年度()

    var qhidpoint = $("#HidPoint").val();  //区域

    var qdeptcode = $("#DepartmentCode").val(); //单位

    var startdate = $("#startDate").val(); //起始时间

    var enddate = $("#endDate").val(); //截止时间

    var statType = $("#statType").attr("data-value")

    //统计条件构成:单位、年度、区域、隐患级别

    //按区域(厂级用户)
    if (qtype == 0) {
        url = '/HiddenTroubleManage/HTBaseInfo/Index?mode=yhdjinfo&qdeptcode=' + qdeptcode + '&qrankname=' + qval + '&qyear=' + qyear + '&code=' + qargs + "&ChangeStatus=" + statType;
    }
    //按部门(厂级用户)
    else if (qtype == 1) {
        url = '/HiddenTroubleManage/HTBaseInfo/Index?mode=yhdjinfo&qdeptcode=' + qargs + '&qrankname=' + qval + '&startdate=' + startdate + '&enddate=' + enddate + '&code=' + qhidpoint + "&ChangeStatus=" + statType;
    }
    //按部门(非厂级用户)
    else
    {
        url = '/HiddenTroubleManage/HTBaseInfo/Index?mode=yhdjinfo&bmmark=bmjtj&qdeptcode=' + qargs + '&qrankname=' + qval + '&startdate=' + startdate + '&enddate=' + enddate + '&code=' + qhidpoint + "&ChangeStatus=" + statType;
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

    //时间范围
    $("#TimeScope").ComboBox({
        id: "id",
        text: "text",
        url: "../../HiddenTroubleManage/HTStatistics/QueryTime",
        description: "",
        allowSearch: false
    });

    var curTimeScope = $("#TimeScope-option li:eq(0)").attr("data-value");
    $("#TimeScope").ComboBoxSetValue(curTimeScope);
    $("#TimeScope").attr("data-text", curTimeScope);
    $("#TimeScope").attr("data-value", curTimeScope);

    

    //查询事件
    $("#btn_Search").click(function () {

        var curStatistics = $("#CurPanel").val();
        var queryJson = {
            deptCode: $("#DepartmentCode").val(),
            year: $("#TimeScope").attr("data-value"),
            hidPoint: $("#HidPoint").val(),
            hidRank: $("button[data-id='HidRank']").attr("title"),
            ischeck: "",
            checkType: "",
            statType: $("#statType").attr("data-value")
        };

        switch (curStatistics) {
            case "统计图":
                //加载图表
                $.ajax({
                    type: "get",
                    url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberPie", //饼图
                    data: { queryJson: JSON.stringify(queryJson) },
                    success: function (data) {

                        if (!!data) {
                            var nData = eval("(" + data + ")");

                            $("#container1").highcharts().series[0].setData(nData.hiddata);

                            if (nData.iscompany == "1") {
                                $("#container2").highcharts().series[0].setData(nData.areadata);
                            }

                            $('#gridTable1').jqGrid('clearGridData');
                            $('#gridTable1').jqGrid('setGridParam', { dataType: 'local', data: nData.tdata, isCompany: nData.iscompany }).trigger('reloadGrid');
                        }
                    }
                });
                break;
            case "趋势图":
                $.ajax({
                    type: "get",
                    url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberTendency",
                    data: { queryJson: JSON.stringify(queryJson) },
                    success: function (data) {
                        if (!!data) {
                            var nData = eval("(" + data + ")");
                            LoadContainer3(nData);
                        }
                    }
                });
                break;
            case "对比图":
                var curComparison = $("#ComparisonPanel").val();
                if (curComparison == "按区域对比") {
                    $.ajax({
                        type: "get",
                        url: "../../HiddenTroubleManage/HTStatistics/QueryComparisonForDistrict",  //获取对比图
                        data: { queryJson: JSON.stringify(queryJson) },
                        success: function (data) {
                            if (!!data) {
                                var nData = eval("(" + data + ")");
                                //按条件加载各等级隐患对比图
                                $('#gridTable1').jqGrid('clearGridData');
                                $('#gridTable1').jqGrid('setGridParam', { dataType: 'local', data: nData.tdata }).trigger('reloadGrid');

                                //按条件加载各等级隐患列表
                                LoadContainer5(nData.xdata, nData.sdata);
                            }
                        }
                    });
                }
                else {
                    //按单位对比
                    queryJson = {
                        deptCode: $("#DepartmentCode").val(),
                        startDate: $("#startDate").val(),
                        endDate: $("#endDate").val(),
                        hidPoint: $("#HidPoint").val(),
                        hidRank: $("button[data-id='HidRank']").attr("title"),
                        ischeck: "",
                        checkType: "",
                        statType: $("#statType").attr("data-value")
                    };

                    $.ajax({
                        type: "get",
                        url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberComparison",  //获取对比图
                        data: { queryJson: JSON.stringify(queryJson) },
                        success: function (data) {
                            if (!!data) {
                                var nData = eval("(" + data + ")");
                                //按条件加载各等级隐患对比图
                                $('#gridTable2').jqGrid('clearGridData');
                                //$('#gridTable2').jqGrid('setGridParam', { dataType: 'local', data: nData.tdata }).trigger('reloadGrid');
                                //GetGridTable2(queryJson);
                                $('#gridTable2').jqGrid('setGridParam', { postData: { queryJson: JSON.stringify(queryJson) } }).trigger('reloadGrid');
                                //按条件加载各等级隐患列表
                                LoadContainer4(nData.xdata, nData.sdata);
                            }
                        }
                    });
                }
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

    //切换过程清空条件

    //区域范围
    $("#HidPointName").val("");
    $("#HidPoint").val("");
    //时间范围
    var curTime = $("#TimeScope-option li:eq(0)").attr("data-value");
    $("#TimeScope").ComboBoxSetValue(curTime);
    $("#TimeScope").attr("data-text", curTime);
    $("#TimeScope").attr("data-value", curTime);
    //隐患级别
    $("button[data-id='HidRank']").removeClass().addClass("btn dropdown-toggle btn-default bs-placeholder");
    $("button[data-id='HidRank']").attr("title", "请选择");
    $("button[data-id='HidRank'] span:eq(0)").text("请选择");
    $("button[data-id='HidRank']").next().find("ul li").each(function (index, ele) {
        $(this).removeClass();
    });


    var queryJson = {
        deptCode: $("#DepartmentCode").val(),
        year: $("#TimeScope").attr("data-value"),
        hidPoint: $("#HidPoint").val(),
        hidRank: $("button[data-id='HidRank']").attr("title"),
        ischeck: "",
        checkType: "",
        statType: $("#statType").attr("data-value")
    };

    switch (sType) {
        case "统计图":
            $("#btn_Export").css("display", "");
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

                $("#lr-panel-four").css("background-color", "#fff");
                $("#lr-panel-four").css("color", "#333");
                $("#lr-panel-four i").css("color", "#666666");
            }

            $("#statisticsList").removeAttr("style");

            $("#gbox_gridTable1").css("display", "");
            $("#gridTable1").css("display", "");

            $("#gbox_gridTable2").css("display", "none");
            $("#gridTable2").css("display", "none");

            $("#container1").css("display", "");
            $("#container2").css("display", "");
            $("#container3").css("display", "none");
            $("#container4").css("display", "none");
            $("#container5").css("display", "none");

            $(".queryform th").each(function (index, ele) {
                $(this).removeAttr("Style");
                $(this).css("padding-left", "10px");
            });
            $(".queryform td").each(function (index, ele) {
                $(this).removeAttr("Style");
                $(this).css("padding-left", "5px");
            });
            /*隐藏筛选条件*/
            $(".queryform th:eq(1)").css("display", "none");
            $(".queryform th:eq(2)").css("display", "none");
            $(".queryform td:eq(1)").css("display", "none");
            $(".queryform td:eq(2)").css("display", "none");
            $(".queryform th:eq(4)").css("display", "none");
            $(".queryform td:eq(4)").css("display", "none");

            //获取统计图
            $.ajax({
                type: "get",
                url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberPie",  //获取饼图
                data: { queryJson: JSON.stringify(queryJson) },
                success: function (data) {
                    if (!!data) {
                        var nData = eval("(" + data + ")");

                        $("#container1").highcharts().series[0].setData(nData.hiddata);
                        if (nData.iscompany == "1") {
                            $("#container2").highcharts().series[0].setData(nData.areadata);
                        }

                        $('#gridTable1').jqGrid('clearGridData');
                        $('#gridTable1').jqGrid('setGridParam', { dataType: 'local', data: nData.tdata, isCompany: nData.iscompany }).trigger('reloadGrid');
                    }
                }
            });
            break;
        case "趋势图":
            $("#btn_Export").css("display", "none");
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

                $("#lr-panel-four").css("background-color", "#fff");
                $("#lr-panel-four").css("color", "#333");
                $("#lr-panel-four i").css("color", "#666666");
            }

            $("#statisticsList").css("display", "none");

            $("#container1").css("display", "none");
            $("#container2").css("display", "none");
            $("#container4").css("display", "none");
            $("#container5").css("display", "none");
            $("#container3").css("display", "");

            $(".queryform th").each(function (index, ele) {
                $(this).removeAttr("Style");
                $(this).css("padding-left", "10px");
            });
            $(".queryform td").each(function (index, ele) {
                $(this).removeAttr("Style");
                $(this).css("padding-left", "5px");
            });

            $.ajax({
                type: "get",
                url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberTendency",
                data: { queryJson: JSON.stringify(queryJson) },
                success: function (data) {
                    if (!!data) {
                        var nData = eval("(" + data + ")");
                        LoadContainer3(nData);
                    }
                }
            });

            $(".queryform th:eq(4)").css("display", "none");
            $(".queryform td:eq(4)").css("display", "none");
            break;
        case "对比图":
            $("#btn_Export").css("display", "");

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

            $("#statisticsList").removeAttr("style");

            $("#gbox_gridTable1").css("display", "none");
            $("#gridTable1").css("display", "none");

            $("#gbox_gridTable2").css("display", "");
            $("#gridTable2").css("display", "");

            $("#container1").css("display", "none");
            $("#container2").css("display", "none");
            $("#container3").css("display", "none");
            $("#container5").css("display", "none");

            $("#container4").css("display", "");
            $(".queryform th").each(function (index, ele) {
                $(this).removeAttr("Style");
                $(this).css("padding-left", "10px");
            });
            $(".queryform td").each(function (index, ele) {
                $(this).removeAttr("Style");
                $(this).css("padding-left", "5px");
            });
            $(".queryform th:eq(3)").css("display", "none");
            $(".queryform td:eq(3)").css("display", "none");
            $(".queryform td:eq(4)").css("line-height", "28px");

            queryJson = {
                deptCode: $("#DepartmentCode").val(),
                startDate: $("#startDate").val(),
                endDate: $("#endDate").val(),
                hidPoint: $("#HidPoint").val(),
                hidRank: $("button[data-id='HidRank']").attr("title"),
                ischeck: "",
                checkType: "",
                statType: $("#statType").attr("data-value")
            };

            $.ajax({
                type: "get",
                url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberComparison",  //获取对比图
                data: { queryJson: JSON.stringify(queryJson) },
                success: function (data) {
                    if (!!data) {
                        var nData = eval("(" + data + ")");
                        //按条件加载各等级隐患对比图
                        GetGridTable2(queryJson);
                        //按条件加载各等级隐患列表
                        LoadContainer4(nData.xdata, nData.sdata);
                    }
                }
            });
            break;
    }

    if (sType == "统计图" || sType == "趋势图" || sType == "对比图") {
        $("#CurPanel").val(sType); //标记当前活动的统计图
        if (sType == "对比图") {
            $("#ComparisonPanel").val("按单位对比");
        }
    }

}


//按单位进行对比
function dwQuery() {
    //切换过程清空条件
    $("#btn_Export").css("display", "");
    //区域范围
    $("#HidPointName").val("");
    $("#HidPoint").val("");
    //时间范围
    var curTime = $("#TimeScope-option li:eq(0)").attr("data-value");
    $("#TimeScope").ComboBoxSetValue(curTime);
    $("#TimeScope").attr("data-text", curTime);
    $("#TimeScope").attr("data-value", curTime);
    //隐患级别
    $("button[data-id='HidRank']").removeClass().addClass("btn dropdown-toggle btn-default bs-placeholder");
    $("button[data-id='HidRank']").attr("title", "请选择");
    $("button[data-id='HidRank'] span:eq(0)").text("请选择");
    $("button[data-id='HidRank']").next().find("ul li").each(function (index, ele) {
        $(this).removeClass();
    });


    $("#lr-panel-four").css("background-color", "#337ab7");
    $("#lr-panel-four").css("color", "#fff");
    $("#lr-panel-four").find("i:eq(0)").css("color", "#fff");
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

    $("#statisticsList").removeAttr("style");

    $("#container1").css("display", "none");
    $("#container2").css("display", "none");
    $("#container3").css("display", "none");

    $(".queryform th").each(function (index, ele) {
        $(this).removeAttr("Style");
        $(this).css("padding-left", "10px");
    });
    $(".queryform td").each(function (index, ele) {
        $(this).removeAttr("Style");
        $(this).css("padding-left", "5px");
    });
    $(".queryform th:eq(2)").css("display", "none");
    $(".queryform td:eq(2)").css("display", "none");
    $(".queryform th:eq(3)").css("display", "none");
    $(".queryform td:eq(3)").css("display", "none");
    $(".queryform td:eq(4)").css("line-height", "28px");


    $("#container4").css("display", "");
    $("#container5").css("display", "none");

    $("#gbox_gridTable1").css("display", "none");
    $("#gridTable1").css("display", "none");

    $("#gbox_gridTable2").css("display", "");
    $("#gridTable2").css("display", "");

    $("#CurPanel").val("对比图");
    $("#ComparisonPanel").val("按单位对比");

    var queryJson = {
        deptCode: $("#DepartmentCode").val(),
        startDate: $("#startDate").val(),
        endDate: $("#endDate").val(),
        hidPoint: $("#HidPoint").val(),
        hidRank: $("button[data-id='HidRank']").attr("title"),
        ischeck: "",
        checkType: "",
        statType: $("#statType").attr("data-value")
    };

    $.ajax({
        type: "get",
        url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberComparison",  //获取对比图
        data: { queryJson: JSON.stringify(queryJson) },
        success: function (data) {
            if (!!data) {
                var nData = eval("(" + data + ")");
                //按条件加载各等级隐患对比图
                GetGridTable2(queryJson);
                //按条件加载各等级隐患列表
                LoadContainer4(nData.xdata, nData.sdata);
            }
        }
    });

}

//按区域进行对比
function qyQuery() {

    //切换过程清空条件
    $("#btn_Export").css("display", "");
    //区域范围
    $("#HidPointName").val("");
    $("#HidPoint").val("");
    //时间范围
    var curTime = $("#TimeScope-option li:eq(0)").attr("data-value");
    $("#TimeScope").ComboBoxSetValue(curTime);
    $("#TimeScope").attr("data-text", curTime);
    $("#TimeScope").attr("data-value", curTime);
    //隐患级别
    $("button[data-id='HidRank']").removeClass().addClass("btn dropdown-toggle btn-default bs-placeholder");
    $("button[data-id='HidRank']").attr("title", "请选择");
    $("button[data-id='HidRank'] span:eq(0)").text("请选择");
    $("button[data-id='HidRank']").next().find("ul li").each(function (index, ele) {
        $(this).removeClass();
    });

    $("#lr-panel-four").css("background-color", "#337ab7");
    $("#lr-panel-four").css("color", "#fff");
    $("#lr-panel-four").find("i:eq(0)").css("color", "#fff");

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

    $("#statisticsList").removeAttr("style");

    $("#container1").css("display", "none");
    $("#container2").css("display", "none");
    $("#container3").css("display", "none");

    $(".queryform th").each(function (index, ele) {
        $(this).removeAttr("Style");
        $(this).css("padding-left", "10px");
    });
    $(".queryform td").each(function (index, ele) {
        $(this).removeAttr("Style");
        $(this).css("padding-left", "5px");
    });
    $(".queryform th:eq(2)").css("display", "none");
    $(".queryform td:eq(2)").css("display", "none");
    $(".queryform th:eq(4)").css("display", "none");
    $(".queryform td:eq(4)").css("display", "none");


    $("#container5").css("display", "");
    $("#container4").css("display", "none");

    $("#gbox_gridTable1").css("display", "");
    $("#gridTable1").css("display", "");

    $("#gbox_gridTable2").css("display", "none");
    $("#gridTable2").css("display", "none");


    $("#CurPanel").val("对比图");
    $("#ComparisonPanel").val("按区域对比");

    var queryJson = {
        deptCode: $("#DepartmentCode").val(),
        year: $("#TimeScope").attr("data-value"),
        hidPoint: $("#HidPoint").val(),
        hidRank: $("button[data-id='HidRank']").attr("title"),
        ischeck: "",
        checkType: "",
        statType: $("#statType").attr("data-value")
    };

    $.ajax({
        type: "get",
        url: "../../HiddenTroubleManage/HTStatistics/QueryComparisonForDistrict",  //获取对比图
        data: { queryJson: JSON.stringify(queryJson) },
        success: function (data) {
            if (!!data) {
                var nData = eval("(" + data + ")");
                //按条件加载各等级隐患对比图
                GetGridTable1(nData.tdata);
                //按条件加载各等级隐患列表
                LoadContainer5(nData.xdata, nData.sdata);
            }
        }
    });

}

