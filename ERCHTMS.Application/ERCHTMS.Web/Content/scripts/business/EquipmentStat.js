$(function () {
    /**/Highcharts.setOptions({
        lang: {
            printChart: "打印图表",
            downloadJPEG: "下载JPEG图片",
            downloadPDF: "下载PDF文件",
            downloadPNG: "下载PNG图片",
            downloadSVG: "下载SVG文件"
        }
    });
    InitialPage();

    //查询事件
    $("#btn_Search").click(function () {
        var StartTime = $("#StartTime").val();
        var EndTime = $("#EndTime").val();
        if (EndTime.length > 0 && StartTime.length == 0) {
            alert("请选择开始时间");
            return;
        }
        if (new Date(StartTime.replace("-", "/").replace("-", "/")) > new Date(EndTime.replace("-", "/").replace("-", "/"))) {
            alert("开始时间不可大于结束时间！");
            return;
        }
        var queryJson = {
            StartTime: StartTime,
            EndTime: EndTime
        };
        if ($("#CurPanel").val() == "3") {
            $.ajax({
                url: "../../EquipmentManage/SpecialEquipment/GetEquipmentHidStat",
                data: { queryJson: JSON.stringify(queryJson) },
                success: function (data) {
                    if (!!data) {
                        //绑定统计图和表格
                        GetBangTable3(data);
                    }
                }
            });
        } else {
            //获取统计图
            $.ajax({
                url: "../../EquipmentManage/SpecialEquipment/GetEquipmentTypeStat",
                data: { queryJson: JSON.stringify(queryJson) },
                success: function (data) {
                    if (!!data) {
                        GetBangTable1(data);
                    }
                }
            });
        }
        
    });
});


//加载统计图一
function LoadContainer1(EquipmentType, OwnEquipment, ExternalEquipment) {
    //柱形图
    $('#container1').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: ''
        },
        xAxis: {
            categories: EquipmentType
        },
        exporting: { enabled: false },
        yAxis: {
            min: 0,
            title: {
                text: ''
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                }
            }, tickInterval: 1
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
            pointFormat: '{series.name}: {point.y}<br/>共计： {point.stackTotal}'
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
        series: [{
            name: '本单位自有设备',
            data: OwnEquipment
        }, {
            name: '外包单位所有设备',
            data: ExternalEquipment
        }]
    });
}

//加载统计图二
function LoadContainer2(y, OwnEquipment, ExternalEquipment, SumNum) {
    $('#container2').highcharts({
        title: {
            text: '',
            x: -20 //center
        },
        subtitle: {
            text: '',
            x: -20
        },
        xAxis: {
            categories: y
        },
        exporting: { enabled: false },
        yAxis: {
            min: 0,
            title: {
                text: ''
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }], tickInterval: 1
        },
        tooltip: {
            valueSuffix: ''
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: [{
            name: '本单位自有设备',
            data: OwnEquipment
        }, {
            name: '外包单位所有设备',
            data: ExternalEquipment
        }, {
            name: '共计',
            data: SumNum
        }]
    });

}

//加载统计图三
function LoadContainer3(EquipmentType, OwnEquipment, ExternalEquipment) {
    //柱形图
    $('#container3').highcharts({
        chart: {
            type: 'column'
        },
        title: {
            text: ''
        },
        xAxis: {
            categories: EquipmentType
        },
        exporting: { enabled: false },
        yAxis: {
            min: 0,
            title: {
                text: ''
            },
            stackLabels: {
                enabled: true,
                style: {
                    fontWeight: 'bold',
                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                }
            }, tickInterval: 1
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
            pointFormat: '{series.name}: {point.y}<br/>共计： {point.stackTotal}'
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
        series: [{
            name: '本单位自有设备隐患',
            data: OwnEquipment
        }, {
            name: '外包单位所有设备隐患',
            data: ExternalEquipment
        }]
    });
}


//加载表格1
function GetGridTable1(EquipmentType, OwnEquipment, ExternalEquipment, EquipmentTypeValue) {
    $("#gridTable1").html("");
    var strType = "<tr style='font-weight: bold;'><td>序号</td><td>所属关系</td>";
    var strOwnEquipment = "<tr><td>1</td><td>本单位自有设备</td>";
    var strExternalEquipment = "<tr><td>2</td><td>外包单位所有设备</td>";
    var Sum = 0;
    var strSum = "<tr><td>3</td><td>共计</td>";
    for (var i = 0; i < EquipmentType.length; i++) {
        strType += "<td>" + EquipmentType[i] + "</td>";
        strOwnEquipment += "<td>" + genHtml("1", EquipmentTypeValue[i], OwnEquipment[i]) + "</td>";
        strExternalEquipment += "<td>" + genHtml("2", EquipmentTypeValue[i], ExternalEquipment[i]) + "</td>";
        Sum = parseInt(OwnEquipment[i]) + parseInt(ExternalEquipment[i]);
        strSum += "<td>" + genHtml("", EquipmentTypeValue[i], Sum) + "</td>";
    }
    strType += "</tr>";
    strOwnEquipment += "</tr>";
    strExternalEquipment += "</tr>";
    strSum += "</tr>";
    $("#gridTable1").append(strType);
    $("#gridTable1").append(strOwnEquipment);
    $("#gridTable1").append(strExternalEquipment);
    $("#gridTable1").append(strSum);
}

//加载表格2
function GetGridTable2(y, OwnEquipment, ExternalEquipment) {
    var b = false;
    var title = "月份";
    if (y.length < 12) {
        b = true;
        $("#YearStat").html("");
        var $_html = $('<ul></ul>');
        title = "年份";
    }
    $("#gridTable2").html("");
    var str = "<tr style='font-weight: bold;'><td>"+title+"</td><td>本单位自有设备运行故障（起）</td><td>外包单位所有设备运行故障（起）</td><td>共计</td></tr>";
    var Sum = 0;
    for (var i = 0; i < y.length; i++) {
        if (b) {
            $_html.append('<li data-value=' + y[i] + '>' + y[i] + '</li>');
        }
        Sum = parseInt(OwnEquipment[i]) + parseInt(ExternalEquipment[i]);
        str += "<tr><td>" + y[i] + "</td><td>" + OwnEquipment[i] + "</td><td>" + ExternalEquipment[i] + "</td><td>" + Sum + "</td></tr>";
    }
    if (b) {
        $("#YearStat").html($_html);
        $("#YearStat").ComboBox({
            description: "==请选择==",
        });
        $("#YearStat").ComboBoxSetValue("" + new Date().getFullYear() + "");
        $("#YearStat").attr("data-text", "" + new Date().getFullYear() + "");
        $("#YearStat").attr("data-value", "" + new Date().getFullYear() + "");
    }
    
    $("#gridTable2").append(str);
}

//加载表格3
function GetGridTable3(EquipmentType, OwnEquipment, ExternalEquipment, EquipmentTypeValue) {
    $("#gridTable3").html("");
    var strType = "<tr style='font-weight: bold;'><td>序号</td><td></td>";
    var strOwnEquipment = "<tr><td>1</td><td>本单位设备隐患(起)</td>";
    var strExternalEquipment = "<tr><td>2</td><td>外包单位设备隐患(起)</td>";
    var Sum = 0;
    var strSum = "<tr><td>3</td><td>共计(起)</td>";
    for (var i = 0; i < EquipmentType.length; i++) {
        strType += "<td>" + EquipmentType[i] + "</td>";
        strOwnEquipment += "<td>" + genHtml("1", EquipmentTypeValue[i], OwnEquipment[i], "hiddBaseIndex") + "</td>";
        strExternalEquipment += "<td>" + genHtml("2", EquipmentTypeValue[i], ExternalEquipment[i], "hiddBaseIndex") + "</td>";
        Sum = parseInt(OwnEquipment[i]) + parseInt(ExternalEquipment[i]);
        strSum += "<td>" + genHtml("",EquipmentTypeValue[i],Sum,"hiddBaseIndex") + "</td>";
    }
    strType += "</tr>";
    strOwnEquipment += "</tr>";
    strExternalEquipment += "</tr>";
    strSum += "</tr>";
    $("#gridTable3").append(strType);
    $("#gridTable3").append(strOwnEquipment);
    $("#gridTable3").append(strExternalEquipment);
    $("#gridTable3").append(strSum);
}
function genHtml(aff, equType, num,goType) {
    var html = num;
    if (num > 0 || num != "0") {
        if (goType == "hiddBaseIndex")
            html = "<a href=javascript:openHiddBaseIndex('" + aff + "','" + equType + "')>" + num + "</a>";
        else
            html = "<a href=javascript:openSpecialEquipment('" + aff + "','" + equType + "')>" + num + "</a>";
    }
    return html;
}
function openSpecialEquipment(affiliation, equipmenttype) {
    var StartTime = $("#StartTime").val();
    var EndTime = $("#EndTime").val();
    var url = '/EquipmentManage/SpecialEquipment/IndexList?st=' + StartTime + "&et=" + EndTime + "&aff=" + affiliation + "&equtype=" + equipmenttype;
    var idx = dialogOpen({
        id: 'speEquIndex',
        title: '特种设备列表',
        url: url,
        btns: 1,
        btn: ["关闭"],
        width: ($(top.window).width() - 50) + "px",
        height: ($(top.window).height() - 50) + "px",
        callBack: function (iframeId) {
            top.layer.close(idx);
        }
    });
}
function openHiddBaseIndex(affiliation, equipmenttype) {
    var StartTime = $("#StartTime").val();
    var EndTime = $("#EndTime").val();
    var url = '/HiddenTroubleManage/HTBaseInfo/Index?mode=sbtjyh&st=' + StartTime + "&et=" + EndTime + "&aff=" + affiliation + "&equtype=" + equipmenttype;
    var idx = dialogOpen({
        id: 'hiddBaseIndex',
        title: '隐患台帐列表',
        url: url,
        btns: 1,
        btn: ["关闭"],
        width: ($(top.window).width() - 50) + "px",
        height: ($(top.window).height() - 50) + "px",
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
            ischeck: "",
            checkType: ""
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
    $("#btn_Export").click(function () {
        var num = $("#CurPanel").val();
        var filename;
        switch (num) {
            case "1":
                filename = "不同类型特种设备数量统计";
                break;
            case "2":
                filename = "运行故障统计";
                break;
            case "3":
                filename = "隐患数量统计";
                break;
            default:
                filename = "统计表";
        }
        var table = "<table  style='text-align: center; ' border='1'  cellpadding='0' cellspacing='0'>";
        var tabHtml = $($("#gridTable" + num).html());
        tabHtml.find("a").each(function (j, dom) {
            $(dom).removeAttr("href")
          
        });
        table += tabHtml.html();
        table += "</table>";
      
        //encodeURIComponent(table)
        //window.location.href = "../../EquipmentManage/SpecialEquipment/ExportEquipmentStat?TableHtml=" + table;
        $.post("../../EquipmentManage/SpecialEquipment/SaveEquipmentStat",
             { TableHtml: encodeURIComponent(table) }
             , function (data) {
                 window.location.href = "../../EquipmentManage/SpecialEquipment/ExportEquipmentStat?PID=" + data + "&filename=" + encodeURIComponent(filename);
             });
    });
}

//切换统计图
function showChart(obj, sType) {
    //时间范围
    $("#StartTime").val("");
    $("#EndTime").val("");
    var queryJson = {
        deptCode: $("#DepartmentCode").val(),
        year: $("#TimeScope").attr("data-value"),
        hidPoint: $("#HidPoint").val(),
        hidRank: $("button[data-id='HidRank']").attr("title"),
        ischeck: "",
        checkType: ""
    };

    switch (sType) {
        case "1":
            $("#searchTab").css("display", "");
            $("#searchTab1").css("display", "none");
            //改变当前按钮样式
            $(obj).css("background-color", "#337ab7");
            $(obj).css("color", "#fff");
            $(obj).find("i:eq(0)").css("color", "#fff");

            if ($("#CurPanel").val() == "2") {
                $("#lr-panel-two").css("background-color", "#fff");
                $("#lr-panel-two").css("color", "#333");
                $("#lr-panel-two i").css("color", "#666666");
            }
            if ($("#CurPanel").val() == "3") {
                $("#lr-panel-three").css("background-color", "#fff");
                $("#lr-panel-three").css("color", "#333");
                $("#lr-panel-three i").css("color", "#666666");

                $("#lr-panel-four").css("background-color", "#fff");
                $("#lr-panel-four").css("color", "#333");
                $("#lr-panel-four i").css("color", "#666666");
            }

            $("#statisticsList").removeAttr("style");

            $("#gbox_gridTable1").css("display", "block");
            //$("#gridTable1").css("display", "block");
            $("#gridTable1").css("display", "");

            $("#gbox_gridTable2").css("display", "none");
            $("#gridTable2").css("display", "none");

            $("#gbox_gridTable3").css("display", "none");
            $("#gridTable3").css("display", "none");

            $("#container1").css("display", "block");
            $("#container2").css("display", "none");
            $("#container3").css("display", "none");

            //获取统计图
            $.ajax({
                url: "../../EquipmentManage/SpecialEquipment/GetEquipmentTypeStat",
                success: function (data) {
                    if (!!data) {
                        //绑定统计图和表格
                        GetBangTable1(data);
                    }
                }
            });
            break;
        case "2":
            $("#yStat").css("background-color", "#337ab7");
            $("#yStat").css("color", "#fff");
            $("#yStat").find("i:eq(0)").css("color", "#fff");
            $("#mStat").css("background-color", "#fff");
            $("#mStat").css("color", "#333");
            $("#mStat i").css("color", "#666666");
            $("#YearStat").css("display", "none");
            //改变当前按钮样式
            $("#searchTab").css("display", "none");
            $("#searchTab1").css("display", "");
            $(obj).css("background-color", "#337ab7");
            $(obj).css("color", "#fff");
            $(obj).find("i:eq(0)").css("color", "#fff");
            if ($("#CurPanel").val() == "1") {
                $("#lr-panel-one").css("background-color", "#fff");
                $("#lr-panel-one").css("color", "#333");
                $("#lr-panel-one i").css("color", "#666666");
            }
            if ($("#CurPanel").val() == "3") {
                $("#lr-panel-three").css("background-color", "#fff");
                $("#lr-panel-three").css("color", "#333");
                $("#lr-panel-three i").css("color", "#666666");

                $("#lr-panel-four").css("background-color", "#fff");
                $("#lr-panel-four").css("color", "#333");
                $("#lr-panel-four i").css("color", "#666666");
            }

            $("#statisticsList").removeAttr("style");

            $("#gbox_gridTable1").css("display", "none");
            $("#gridTable1").css("display", "none");

            $("#gbox_gridTable2").css("display", "block");
            //$("#gridTable2").css("display", "block");
            $("#gridTable2").css("display", "");

            $("#gbox_gridTable3").css("display", "none");
            $("#gridTable3").css("display", "none");


            $("#container1").css("display", "none");
            $("#container2").css("display", "block");
            $("#container3").css("display", "none");

            $.ajax({
                url: "../../EquipmentManage/SpecialEquipment/GetOperationFailureStat",
                success: function (data) {
                    if (!!data) {
                        //绑定统计图和表格
                        GetBangTable2(data);

                    }
                }
            });
            break;
        case "3":
            $("#searchTab").css("display", "");
            $("#searchTab1").css("display", "none");
            $(obj).css("background-color", "#337ab7");
            $(obj).css("color", "#fff");
            $(obj).find("i:eq(0)").css("color", "#fff");
            if ($("#CurPanel").val() == "1") {
                $("#lr-panel-one").css("background-color", "#fff");
                $("#lr-panel-one").css("color", "#333");
                $("#lr-panel-one i").css("color", "#666666");
            }
            if ($("#CurPanel").val() == "2") {
                $("#lr-panel-two").css("background-color", "#fff");
                $("#lr-panel-two").css("color", "#333");
                $("#lr-panel-two i").css("color", "#666666");
            }

            $("#statisticsList").removeAttr("style");

            $("#gbox_gridTable1").css("display", "none");
            $("#gridTable1").css("display", "none");

            $("#gbox_gridTable2").css("display", "none");
            $("#gridTable2").css("display", "none");

            $("#gbox_gridTable3").css("display", "block");
            $("#gridTable3").css("display", "");

            $("#container1").css("display", "none");
            $("#container2").css("display", "none");
            $("#container3").css("display", "block");

            $.ajax({
                url: "../../EquipmentManage/SpecialEquipment/GetEquipmentHidStat",
                data: { queryJson: JSON.stringify(queryJson) },
                success: function (data) {
                    if (!!data) {
                        //绑定统计图和表格
                        GetBangTable3(data);
                    }
                }
            });
            break;
    }

    if (sType == "1" || sType == "2" || sType == "3") {
        $("#CurPanel").val(sType); //标记当前活动的统计图
    }

}

//切换统计方式
function showStatType(obj, type) {
    $(obj).css("background-color", "#337ab7");
    $(obj).css("color", "#fff");
    $(obj).find("i:eq(0)").css("color", "#fff");
    //月度统计
    if (type == "2") {
        $("#YearStat").css("display", "");
        $("#yStat").css("background-color", "#fff");
        $("#yStat").css("color", "#333");
        $("#yStat i").css("color", "#666666");
        var queryJson = {
            year: $("#YearStat").attr("data-value"),
            type: type
        };
        $.ajax({
            url: "../../EquipmentManage/SpecialEquipment/GetOperationFailureStat",
            data: { queryJson: JSON.stringify(queryJson) },
            success: function (data) {
                if (!!data) {
                    //绑定统计图和表格
                    GetBangTable2(data);

                }
            }
        });
    } else {
        $("#mStat").css("background-color", "#fff");
        $("#mStat").css("color", "#333");
        $("#mStat i").css("color", "#666666");
        $("#YearStat").css("display", "none");
        $.ajax({
            url: "../../EquipmentManage/SpecialEquipment/GetOperationFailureStat",
            success: function (data) {
                if (!!data) {
                    //绑定统计图和表格
                    GetBangTable2(data);

                }
            }
        });
    }
}

function GetBangTable1(data) {
    var ndata = eval(data);
    var EquipmentType = []
    var EquipmentTypeValue = [];;
    var OwnEquipment = [];
    var ExternalEquipment = [];
    $.each(ndata, function (key, val) {
        EquipmentType.push(val.itemname);
        EquipmentTypeValue.push(val.itemvalue);
        OwnEquipment.push(val.ownequipment);
        ExternalEquipment.push(val.externalequipment);
    })
    //统计图
    LoadContainer1(EquipmentType, OwnEquipment, ExternalEquipment);
    //统计表格
    GetGridTable1(EquipmentType, OwnEquipment, ExternalEquipment, EquipmentTypeValue);
}

function GetBangTable2(data) {
    var ndata = eval("(" + data + ")");
    var y = eval(ndata.y);
    var OwnEquipment = eval(ndata.OwnEquipment);
    var ExternalEquipment = eval(ndata.ExternalEquipment);
    var SumNum = eval(ndata.SumNum);
    //统计图
    LoadContainer2(y, OwnEquipment, ExternalEquipment,SumNum);
    //统计表格
    GetGridTable2(y, OwnEquipment, ExternalEquipment);
}

function GetBangTable3(data) {
    var ndata = eval(data);
    var EquipmentType = [];
    var EquipmentTypeValue = [];
    var OwnEquipment = [];
    var ExternalEquipment = [];
    $.each(ndata, function (key, val) {
        EquipmentType.push(val.itemname);
        EquipmentTypeValue.push(val.itemvalue);
        OwnEquipment.push(val.ownequipment);
        ExternalEquipment.push(val.externalequipment);
    })
    //统计图
    LoadContainer3(EquipmentType, OwnEquipment, ExternalEquipment);
    //统计表格
    GetGridTable3(EquipmentType, OwnEquipment, ExternalEquipment, EquipmentTypeValue);
}

//切换年度
function changeYearStat() {
    var queryJson = {
        year: $("#YearStat").attr("data-value"),
        type: "2"
    };
    $.ajax({
        url: "../../EquipmentManage/SpecialEquipment/GetOperationFailureStat",
        data: { queryJson: JSON.stringify(queryJson) },
        success: function (data) {
            if (!!data) {
                //绑定统计图和表格
                GetBangTable2(data);

            }
        }
    });
}



