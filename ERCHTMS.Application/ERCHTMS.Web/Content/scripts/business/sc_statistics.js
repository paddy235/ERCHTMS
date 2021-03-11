$(function () {
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
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %',
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
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %',
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
function LoadContainer4(xdata, sdata) {

    $('#container4').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: '各等级隐患统计'
        },
        xAxis: {
            categories: xdata  //['安环部', '计划部', '生技部', '运行部', '检修部', '综合部', '燃管部', '除灰部', '监审部', '政工部', '商务部', '财务部']
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
function GetGridTable1() {
    var selectedRowIndex = 0;
    var $gridTable = $('#gridTable1');
    var queryJson = { deptCode: $("#DepartmentCode").val(), year: $("#TimeScope").attr("data-value"), hidPoint: $("#HidPoint").val(), hidRank: $("button[data-id='HidRank']").attr("title") };
    //隐患基本信息
    $gridTable.jqGrid({
        autowidth: true,
        height: parseFloat($(window).height() / 2 - 100),
        url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberList",
        postData: { queryJson: JSON.stringify(queryJson) },
        datatype: "json",
        colModel: [
           { label: '主键', name: 'hidpoint', index: 'hidpoint', width: 100, align: 'center', hidden: true },
            { label: '区域名称', name: 'hidpointname', index: 'hidpointname', width: 150, align: 'center', sortable: true },
            { label: '一般隐患', name: 'ordinaryhid', index: 'ordinaryhid', width: 150, align: 'center', sortable: true },
            { label: '重大隐患', name: 'importanhid', index: 'importanhid', width: 150, align: 'center', sortable: true },
            { label: '合计', name: 'total', index: 'total', width: 150, align: 'center', sortable: true }
        ],
        sortname: 'hidpoint',
        rownumbers: true,
        shrinkToFit: false,
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
    //隐患基本信息
    $gridTable.jqGrid({
        autowidth: true,
        height: parseFloat($(window).height() / 2 - 100),
        datatype: "local",
        data: tdata,
        colModel: [
           { label: '主键', name: 'changedutydepartcode', index: 'changedutydepartcode', width: 100, align: 'center', hidden: true },
            { label: '部门名称', name: 'fullname', index: 'fullname', width: 150, align: 'center', sortable: true },
            { label: '一般隐患', name: 'ordinaryhid', index: 'ordinaryhid', width: 150, align: 'center', sortable: true },
            { label: '重大隐患', name: 'importanhid', index: 'importanhid', width: 150, align: 'center', sortable: true },
            { label: '合计', name: 'total', index: 'total', width: 150, align: 'center', sortable: true }
        ],
        sortname: 'changedutydepartcode',
        rownumbers: true,
        shrinkToFit: false,
        gridview: true,
        onSelectRow: function () {
            selectedRowIndex = $("#" + this.id).getGridParam('selrow');
        },
        gridComplete: function () {
            $("#" + this.id).setSelection(selectedRowIndex, false);
        }
    });
}

//初始化页面
function InitialPage() {

    //默认设置隐患数量统计图及列表
    var oneValue = (parseFloat($(top.window).width() / 2) - 200) + "px";
    //var twoValue = ($(top.window).width() - parseFloat($(top.window).width() / 3) - 100) + "px";
    var widthValue = ($(top.window).width() - 300) + "px";
    var heightValue = "500px";

    /*统计图*/
    $("#container1").css("min-width", oneValue);
    $("#container1").css("height", heightValue);
    $("#container1").css("float", "left");

    $("#container2").css("margin-left", "100px");
    $("#container2").css("min-width", oneValue);
    $("#container2").css("height", heightValue);
    $("#container2").css("float", "left");

    /*趋势图*/
    $("#container3").css("min-width", widthValue);
    $("#container3").css("height", heightValue);

    /*对比图*/
    $("#container4").css("min-width", widthValue);
    $("#container4").css("height", heightValue);


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
        var queryJson = { deptCode: $("#DepartmentCode").val(), year: $("#TimeScope").attr("data-value"), hidPoint: $("#HidPoint").val(), hidRank: $("button[data-id='HidRank']").attr("title") };

        switch (curStatistics) {
            case "统计图":
                //加载列表
                $('#gridTable1').jqGrid('setGridParam', {
                    postData: { queryJson: JSON.stringify(queryJson) }, page: 1
                }).trigger('reloadGrid');
                //加载图表
                $.ajax({
                    type: "get",
                    url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberPie", //饼图
                    data: { queryJson: JSON.stringify(queryJson) },
                    success: function (data) {
                        if (!!data) {
                            var nData = eval("(" + data + ")");
                            $("#container1").highcharts().series[0].setData(nData.hidData);
                            $("#container2").highcharts().series[0].setData(nData.areaData);
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
                            LoadContainer4(nData.xdata, nData.sdata);
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
    var queryJson = { deptCode: $("#DepartmentCode").val(), year: $("#TimeScope").attr("data-value"), hidPoint: $("#HidPoint").val(), hidRank: $("button[data-id='HidRank']").attr("title") };

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

            $("#statisticsList").removeAttr("style");

            $("#gbox_gridTable1").css("display", "block");
            $("#gridTable1").css("display", "block");

            $("#gbox_gridTable2").css("display", "none");
            $("#gridTable2").css("display", "none");

            $("#container1").css("display", "block");
            $("#container2").css("display", "block");
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
            /*隐藏筛选条件*/
            $(".queryform th:eq(2)").css("display", "none");
            $(".queryform th:eq(3)").css("display", "none");
            $(".queryform td:eq(2)").css("display", "none");
            $(".queryform td:eq(3)").css("display", "none");

            //读取列表 (由于初始化的列表是gridTable1)
            $('#gridTable1').jqGrid('setGridParam', {
                postData: { queryJson: JSON.stringify(queryJson) }, page: 1
            }).trigger('reloadGrid');

            //获取统计图
            $.ajax({
                type: "get",
                url: "../../HiddenTroubleManage/HTStatistics/QueryHidNumberPie",  //获取饼图
                data: { queryJson: JSON.stringify(queryJson) },
                success: function (data) {
                    if (!!data) {
                        var nData = eval("(" + data + ")");
                        LoadContainer1(nData.hidData);
                        LoadContainer2(nData.areaData);
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

            $("#statisticsList").css("display", "none");

            $("#container1").css("display", "none");
            $("#container2").css("display", "none");
            $("#container4").css("display", "none");
            $("#container3").css("display", "block");

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

            $("#statisticsList").removeAttr("style");

            $("#gbox_gridTable1").css("display", "none");
            $("#gridTable1").css("display", "none");

            $("#gbox_gridTable2").css("display", "block");
            $("#gridTable2").css("display", "block");

            $("#container1").css("display", "none");
            $("#container2").css("display", "none");
            $("#container3").css("display", "none");

            $("#container4").css("display", "block");
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
                        LoadContainer4(nData.xdata, nData.sdata);
                    }
                }
            });
            break;
    }
    $("#CurPanel").val(sType); //标记当前活动的统计图
}


