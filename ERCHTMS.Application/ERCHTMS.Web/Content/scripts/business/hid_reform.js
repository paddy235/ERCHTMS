$(function () {
    Highcharts.setOptions({
        exporting: {
            url: "../../HiddenTroubleManage/HTStatistics/Export",
            enabled: true,
            filename: 'MyChart',
            width: 1200
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
    $('#container1').highcharts({
        chart: {
            type: 'column'
        },
        title: { 
            text: '隐患整改情况统计'
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
            text: '隐患整改变化趋势统计'
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
                text: '百分比/%'
            },
            labels: {
                formatter: function () {
                    return this.value + "%";
                }
            }
        },
        tooltip: {
            crosshairs: true,
            shared: true,
            valueSuffix: '%'
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
                        return '</b>' + this.x + ':' + this.y + '%'; //'<b>' + this.series.name + 
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

//加载部门对比图
function LoadContainer3(xdata, sdata) {

    $('#container3').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: '各部门隐患整改状态统计'
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

//加载区域对比图
function LoadContainer4(xdata, sdata) {

    $('#container4').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: '隐患整改状态统计'
        },
        xAxis: {
            categories: xdata
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
function GetGridTable1(data) {
    var selectedRowIndex = 0;
    var $gridTable = $('#gridTable1');
    //隐患基本信息
    $gridTable.jqGrid({
        autowidth: true,
        height: parseFloat($(window).height() / 2 - 100),
        datatype: "local",
        data: data,
        colModel: [
           { label: '主键', name: 'month', index: 'month', width: 100, align: 'center', hidden: true },
            {
                label: '月份', name: 'month', index: 'month', width: 150, align: 'center', sortable: true
            },
            {
                label: '已整改隐患数量', name: 'yvalue', index: 'yvalue', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.yvalue;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(0,'" + rowObject.month + "','已整改')>" + rowObject.yvalue + "</a>";
                    }
                    return rval;
                }
            },
            {
                label: '未整改隐患数量', name: 'wvalue', index: 'wvalue', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.wvalue;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(0,'" + rowObject.month + "','未整改')>" + rowObject.wvalue + "</a>";
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
function GetGridTable2(queryJson) {
    var selectedRowIndex = 0;
    var $gridTable = $('#gridTable2');
    var qtype = 1;
    if (iscompany == "0") {
        qtype = 2;
    }
    //隐患基本信息
    $gridTable.jqGrid({
        url: "../../HiddenTroubleManage/HTStatistics/QueryChangeSituationComparisionList",
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
           { label: '主键', name: 'changedutydepartcode', index: 'changedutydepartcode', width: 100, align: 'center', hidden: true },
           { label: 'haschild', name: 'haschild', hidden: true },
           {
                label: '未整改隐患数量', name: 'nonchange', index: 'nonchange', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.nonchange;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(" + qtype + ",'" + rowObject.changedutydepartcode + "','未整改')>" + rowObject.nonchange + "</a>";
                    }
                    return rval;
                }
            },
            {
                label: '已整改隐患数量', name: 'thenchange', index: 'thenchange', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.thenchange;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(" + qtype + ",'" + rowObject.changedutydepartcode + "','已整改')>" + rowObject.thenchange + "</a>";
                    }
                    return rval;
                }
            },
            {
                label: '合计', name: 'total', index: 'total', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.total;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(" + qtype + ",'" + rowObject.changedutydepartcode + "','')>" + rowObject.total + "</a>";
                    }
                    return rval;
                }
            }
        ],
        treeReader: {
            level_field: "level",
            parent_id_field: "parent",
            leaf_field: "isLeaf",
            expanded_field: "expanded"
        },
        rowNum: "all",
        rownumbers: true,
        rownumWidth: 70,
        onSelectRow: function (rowid) {
            selectedRowIndex = $("#" + this.id).getGridParam('selrow');
        },
        gridComplete: function () {
            $("#" + this.id).setSelection(selectedRowIndex, false);
        }
    });
}

//加载表格3
function GetGridTable3(data) {
    var selectedRowIndex = 0;
    var $gridTable = $('#gridTable3');
    //隐患基本信息
    $gridTable.jqGrid({
        autowidth: true,
        height: parseFloat($(window).height() / 2 - 100),
        datatype: "local",
        data: data,
        colModel: [
           { label: '主键', name: 'hidpoint', index: 'hidpoint', width: 100, align: 'center', hidden: true },
            { label: '区域名称', name: 'hidpointname', index: 'hidpointname', width: 150, align: 'center', sortable: true },
            {
                label: '未整改隐患数量', name: 'nonchange', index: 'nonchange', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.nonchange;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(3,'" + rowObject.hidpoint + "','未整改')>" + rowObject.nonchange + "</a>";
                    }
                    return rval;
                }
            },
            {
                label: '已整改隐患数量', name: 'thenchange', index: 'thenchange', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.thenchange;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(3,'" + rowObject.hidpoint + "','已整改')>" + rowObject.thenchange + "</a>";
                    }
                    return rval;
                }
            },
            {
                label: '合计', name: 'total', index: 'total', width: 150, align: 'center', sortable: true,
                formatter: function (cellvalue, options, rowObject) {
                    var rval = rowObject.total;
                    if (rval > 0) {
                        rval = "<a style='color:#1688f5;text-decoration:underline;' href=javascript:GoHiddenList(3,'" + rowObject.hidpoint + "','')>" + rowObject.total + "</a>";
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

//跳转到隐患列表页面
function GoHiddenList(qtype, qargs, qval) {

    var url = "";
    //选择的年度
    var qyear = $("#TimeScope").attr("data-text"); //年度

    var qhidrank = $("#HidRank").val(); //隐患级别

    var qhidpoint = $("#HidPoint").val();  //区域

    var qdeptcode = $("#DepartmentCode").val(); //单位

    var startdate = $("#startDate").val(); //起始时间

    var enddate = $("#endDate").val(); //截止时间

    //统计条件构成:单位、年度、区域、隐患级别、整改状态    ChangeStatus
    var qrankname = "";

    if (!!qhidrank)
    {
        if (qhidrank.length == 1)
        {
            qrankname = qhidrank[0];
        }
    }
    //按月份
    if (qtype == 0)
    {
        var qyearmonth = qyear + "-" + qargs; //年-月 2018-01
        url = '/HiddenTroubleManage/HTBaseInfo/Index?mode=yhtjinfo&qdeptcode=' + qdeptcode + '&qrankname=' + qrankname + '&qyearmonth=' + qyearmonth + '&code=' + qhidpoint + '&qchangestatus=' + qval;
    }
    //按部门(厂级用户)
    else if (qtype == 1) //按单位
    {
        url = '/HiddenTroubleManage/HTBaseInfo/Index?mode=yhtjinfo&qdeptcode=' + qargs + '&qrankname=' + qrankname + '&startdate=' + startdate + '&enddate=' + enddate + '&qchangestatus=' + qval;
    }
    //按部门(非厂级用户)
    else if (qtype == 2) {
        url = '/HiddenTroubleManage/HTBaseInfo/Index?mode=yhtjinfo&bmmark=bmjtj&qdeptcode=' + qargs + '&qrankname=' + qrankname + '&startdate=' + startdate + '&enddate=' + enddate + '&qchangestatus=' + qval;
    }
    else if (qtype == 3) //按区域
    {
        url = '/HiddenTroubleManage/HTBaseInfo/Index?mode=yhtjinfo&qdeptcode=' + qdeptcode + '&qrankname=' + qrankname + '&qyear=' + qyear + '&code=' + qargs + '&qchangestatus=' + qval;
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

    //默认设置隐患数量统计图及列表
    var widthValue = ($(top.window).width() - 300) + "px";
    var heightValue = "460px";

    /*统计图*/
    //$("#container1").css("min-width", widthValue);
    $("#container1").css("height", heightValue);

    /*趋势图*/
    //$("#container2").css("min-width", widthValue);
    $("#container2").css("height", heightValue);

    /*对比图*/
    //$("#container3").css("min-width", widthValue);
    $("#container3").css("height", heightValue);


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
            hidRank: $("button[data-id='HidRank']").attr("title")
        };
        switch (curStatistics) {
            case "统计图":
                $.ajax({
                    type: "get",
                    url: "../../HiddenTroubleManage/HTStatistics/QueryChangeSituationForMonth",
                    data: { queryJson: JSON.stringify(queryJson) },
                    success: function (data) {
                        if (!!data) {
                            var nData = eval("(" + data + ")");
                            //绑定列表
                            $('#gridTable1').jqGrid('clearGridData');
                            $('#gridTable1').jqGrid('setGridParam', { dataType: 'local', data: nData.tdata }).trigger('reloadGrid');
                            //加载图表
                            LoadContainer1(nData.sdata);
                        }
                    }
                });
                break;
            case "趋势图":
                $.ajax({
                    type: "get",
                    url: "../../HiddenTroubleManage/HTStatistics/QueryChangeSituationTendency",
                    data: { queryJson: JSON.stringify(queryJson) },
                    success: function (data) {
                        if (!!data) {
                            var nData = eval("(" + data + ")");
                            //加载图表
                            LoadContainer2(nData);
                        }
                    }
                });
                break;
            case "对比图":

                var curComparison = $("#ComparisonPanel").val();
                if (curComparison == "按区域对比") {
                    $.ajax({
                        type: "get",
                        url: "../../HiddenTroubleManage/HTStatistics/QueryChangeComparisionForDistrict",  //获取对比图
                        data: { queryJson: JSON.stringify(queryJson) },
                        success: function (data) {
                            if (!!data) {
                                var nData = eval("(" + data + ")");
                                //按条件加载各等级隐患对比图
                                $('#gridTable3').jqGrid('clearGridData');
                                $('#gridTable3').jqGrid('setGridParam', { dataType: 'local', data: nData.tdata }).trigger('reloadGrid');
                                //按条件加载各等级隐患列表
                                LoadContainer4(nData.xdata, nData.sdata);
                            }
                        }
                    });
                }
                else {
                    //按单位对比参数
                    queryJson = {
                        deptCode: $("#DepartmentCode").val(),
                        startDate: $("#startDate").val(),
                        endDate: $("#endDate").val(),
                        hidPoint: $("#HidPoint").val(),
                        hidRank: $("button[data-id='HidRank']").attr("title")
                    };

                    $.ajax({
                        type: "get",
                        url: "../../HiddenTroubleManage/HTStatistics/QueryChangeSituationComparision",
                        data: { queryJson: JSON.stringify(queryJson) },
                        success: function (data) {
                            if (!!data) {
                                var nData = eval("(" + data + ")");
                                //绑定列表
                                $('#gridTable2').jqGrid('clearGridData');
                                //$('#gridTable2').jqGrid('setGridParam', { dataType: 'local', data: nData.tdata }).trigger('reloadGrid');
                                $('#gridTable2').jqGrid('setGridParam', { postData: { queryJson: JSON.stringify(queryJson) } }).trigger('reloadGrid');
                                //加载图表
                                LoadContainer3(nData.xdata, nData.sdata);
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

//切换当前Chart图
function showChart(obj, sType) {

    //切换清空查询条件

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
        hidRank: $("button[data-id='HidRank']").attr("title")
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

            //移除列表面板隐藏样式
            $("#statisticsList").removeAttr("style");

            $("#gbox_gridTable1").css("display", "");
            $("#gridTable1").css("display", "");

            $("#gbox_gridTable2").css("display", "none");
            $("#gridTable2").css("display", "none");

            $("#gbox_gridTable3").css("display", "none");
            $("#gridTable3").css("display", "none");

            $("#container1").css("display", "");
            $("#container2").css("display", "none");
            $("#container3").css("display", "none");
            $("#container4").css("display", "none");


            $(".queryform th").each(function (index, ele) {
                $(this).removeAttr("Style");
                $(this).css("padding-left", "10px");
            });
            $(".queryform td").each(function (index, ele) {
                $(this).removeAttr("Style");
                $(this).css("padding-left", "5px");
            });

            $(".queryform th:eq(4)").css("display", "none");
            $(".queryform td:eq(4)").css("display", "none");


            //获取统计图
            $.ajax({
                type: "get",
                url: "../../HiddenTroubleManage/HTStatistics/QueryChangeSituationForMonth",
                data: { queryJson: JSON.stringify(queryJson) },
                success: function (data) {
                    if (!!data) {
                        var nData = eval("(" + data + ")");
                        //绑定列表
                        $('#gridTable1').jqGrid('clearGridData');
                        $('#gridTable1').jqGrid('setGridParam', { dataType: 'local', data: nData.tdata }).trigger('reloadGrid');
                        //加载图表
                        LoadContainer1(nData.sdata);
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

            //隐藏列表面板(包含了列表)
            $("#statisticsList").css("display", "none");

            $("#container1").css("display", "none");
            $("#container2").css("display", "");
            $("#container3").css("display", "none");
            $("#container4").css("display", "none");

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

            //加载趋势图
            $.ajax({
                type: "get",
                url: "../../HiddenTroubleManage/HTStatistics/QueryChangeSituationTendency",
                data: { queryJson: JSON.stringify(queryJson) },
                success: function (data) {
                    if (!!data) {
                        var nData = eval("(" + data + ")");
                        //加载图表
                        LoadContainer2(nData);
                    }
                }
            });
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

            //移除列表面板隐藏样式
            $("#statisticsList").removeAttr("style");

            $("#gbox_gridTable1").css("display", "none");
            $("#gridTable1").css("display", "none");

            $("#gbox_gridTable2").css("display", "");
            $("#gridTable2").css("display", "");

            $("#gbox_gridTable3").css("display", "none");
            $("#gridTable3").css("display", "none");

            $("#container1").css("display", "none");
            $("#container2").css("display", "none");
            $("#container3").css("display", "");
            $("#container4").css("display", "none");

            $(".queryform th").each(function (index, ele) {
                $(this).removeAttr("Style");
                $(this).css("padding-left", "10px");
            });
            $(".queryform td").each(function (index, ele) {
                $(this).removeAttr("Style");
                $(this).css("padding-left", "5px");
            });
            $(".queryform th:eq(1)").css("display", "none");
            $(".queryform td:eq(1)").css("display", "none");
            $(".queryform th:eq(3)").css("display", "none");
            $(".queryform td:eq(3)").css("display", "none");
            $(".queryform th:eq(4)").css("display", "");
            $(".queryform td:eq(4)").css("display", "");
            $(".queryform td:eq(4)").css("line-height", "28px");


            queryJson = {
                deptCode: $("#DepartmentCode").val(),
                startDate: $("#startDate").val(),
                endDate: $("#endDate").val(),
                hidPoint: $("#HidPoint").val(),
                hidRank: $("button[data-id='HidRank']").attr("title")
            };

            //获取对比图
            $.ajax({
                type: "get",
                url: "../../HiddenTroubleManage/HTStatistics/QueryChangeSituationComparision",
                data: { queryJson: JSON.stringify(queryJson) },
                success: function (data) {
                    if (!!data) {
                        var nData = eval("(" + data + ")");
                        //绑定列表
                        GetGridTable2(queryJson);
                        //加载图表
                        LoadContainer3(nData.xdata, nData.sdata);
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
    //切换清空查询条件
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

    //移除列表面板隐藏样式
    $("#statisticsList").removeAttr("style");

    $("#gbox_gridTable1").css("display", "none");
    $("#gridTable1").css("display", "none");

    $("#gbox_gridTable2").css("display", "");
    $("#gridTable2").css("display", "");

    $("#gbox_gridTable3").css("display", "none");
    $("#gridTable3").css("display", "none");

    $("#container1").css("display", "none");
    $("#container2").css("display", "none");
    $("#container3").css("display", "");
    $("#container4").css("display", "none");

    $(".queryform th").each(function (index, ele) {
        $(this).removeAttr("Style");
        $(this).css("padding-left", "10px");
    });
    $(".queryform td").each(function (index, ele) {
        $(this).removeAttr("Style");
        $(this).css("padding-left", "5px");
    });
    $(".queryform th:eq(1)").css("display", "none");
    $(".queryform td:eq(1)").css("display", "none");
    $(".queryform th:eq(3)").css("display", "none");
    $(".queryform td:eq(3)").css("display", "none");
    $(".queryform th:eq(4)").css("display", "");
    $(".queryform td:eq(4)").css("display", "");
    $(".queryform td:eq(4)").css("line-height", "28px");

    $("#CurPanel").val("对比图");
    $("#ComparisonPanel").val("按单位对比");

    var queryJson = {
                deptCode: $("#DepartmentCode").val(),
                startDate: $("#startDate").val(),
                endDate: $("#endDate").val(),
                hidPoint: $("#HidPoint").val(),
                hidRank: $("button[data-id='HidRank']").attr("title")
            };

    $.ajax({
        type: "get",
        url: "../../HiddenTroubleManage/HTStatistics/QueryChangeSituationComparision",
        data: { queryJson: JSON.stringify(queryJson) },
        success: function (data) {
            if (!!data) {
                var nData = eval("(" + data + ")");
                //绑定列表
                GetGridTable2(queryJson);
                //加载图表
                LoadContainer3(nData.xdata, nData.sdata);
            }
        }
    });
}

//按区域进行对比
function qyQuery() {
    //切换清空查询条件
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

    //移除列表面板隐藏样式
    $("#statisticsList").removeAttr("style");

    $("#gbox_gridTable1").css("display", "none");
    $("#gridTable1").css("display", "none");

    $("#gbox_gridTable2").css("display", "none");
    $("#gridTable2").css("display", "none");

    $("#gbox_gridTable3").css("display", "");
    $("#gridTable3").css("display", "");


    $("#container1").css("display", "none");
    $("#container2").css("display", "none");
    $("#container3").css("display", "none");
    $("#container4").css("display", "");

    $(".queryform th").each(function (index, ele) {
        $(this).removeAttr("Style");
        $(this).css("padding-left", "10px");
    });
    $(".queryform td").each(function (index, ele) {
        $(this).removeAttr("Style");
        $(this).css("padding-left", "5px");
    });
    $(".queryform th:eq(1)").css("display", "none");
    $(".queryform td:eq(1)").css("display", "none");
    $(".queryform th:eq(4)").css("display", "none");
    $(".queryform td:eq(4)").css("display", "none");

    $("#CurPanel").val("对比图");
    $("#ComparisonPanel").val("按区域对比");

    var queryJson = {
        deptCode: $("#DepartmentCode").val(),
        year: $("#TimeScope").attr("data-value"),
        hidPoint: $("#HidPoint").val(),
        hidRank: $("button[data-id='HidRank']").attr("title")
    };

    $.ajax({
        type: "get",
        url: "../../HiddenTroubleManage/HTStatistics/QueryChangeComparisionForDistrict",  //获取对比图
        data: { queryJson: JSON.stringify(queryJson) },
        success: function (data) {
            if (!!data) {
                var nData = eval("(" + data + ")");
                //按条件加载各等级隐患对比图
                GetGridTable3(nData.tdata);
                //按条件加载各等级隐患列表
                LoadContainer4(nData.xdata, nData.sdata);
            }
        }
    });
}
