$(function () {
    InitialPage();
    stat();
})
//初始化
function InitialPage() {
    $('#desktop').height($(window).height() - 22);
    $(window).resize(function (e) {
        window.setTimeout(function () {
            $('#desktop').height($(window).height() - 22);
        }, 200);
        e.stopPropagation();
    });
    if (!(roleNames.indexOf("公司级用户") >= 0 || roleNames.indexOf("厂级部门用户") >= 0)) {
        $(".btn11").hide(); $("#dbt").show();
        $("#div1").css({ width: "100%" }); $("#div2").hide();
        $("#DeptName").val(top.currUserDeptName);
        $("#DeptCode").val(top.currUserDeptCode);
    } else {
        $("#DeptName").val(top.currUserOrgName);
        $("#DeptCode").val(top.currUserOrgCode);
    }
}

function dwSearch() {
    riskGrade = $("#riskGrade").val();
    riskGrade = riskGrade == null || riskGrade == "" ? "低风险,一般风险,较大风险,重大风险" : riskGrade;
    $('.area').show(); $('.area1').hide();
    state = 3; $('#con1').hide(); $('#con2').hide(); $('#con3').show(); $('.btn10').removeClass('btn-primary'); $('.btn10').addClass('btn-default'); $('#btndb').addClass('btn-primary');
    $("#grid1").show(); $("#grid2").hide(); stat2();
    deptCode = $("#DeptCode").val().length == 0 ? top.currUserDeptCode : $("#DeptCode").val();
    var year = "";
    if (($("#StartDate").val().length > 0 && $("#EndDate").val().length == 0) || ($("#StartDate").val().length == 0 && $("#EndDate").val().length > 0)) {
        dialogMsg("请填写完整时间范围！", 2);
        return;
    }
    if ($("#StartDate").val().length > 0 && $("#EndDate").val().length > 0) {
        if (parseInt($("#StartDate").val()) >parseInt($("#EndDate").val())) {
            dialogMsg("开始年份不能大于结束年份！", 2);
            return;
        }
        year = $("#StartDate").val() + "-01-01|" + $("#EndDate").val()+"-12-31";
    }
    $("#gridTable1").jqGrid('setGridParam', {
        postData: { deptCode: deptCode, year: year, riskGrade: riskGrade }
    }).trigger('reloadGrid');
}
function qySearch() {
    riskGrade = $("#riskGrade").val(); $('.area').show(); $('.area1').hide();
    riskGrade = riskGrade == null || riskGrade == "" ? "低风险,一般风险,较大风险,重大风险" : riskGrade;
    state = 4; $('#con1').hide(); $('#con2').hide(); $('#con3').show(); $('.btn10').removeClass('btn-primary'); $('.btn10').addClass('btn-default'); $('#btndb').addClass('btn-primary');
    $("#grid1").hide(); $("#grid2").show(); stat3();
    deptCode = $("#DeptCode").val().length == 0 ? top.currUserDeptCode : $("#DeptCode").val();
    var year = "";
    if (($("#StartDate").val().length > 0 && $("#EndDate").val().length == 0) || ($("#StartDate").val().length == 0 && $("#EndDate").val().length > 0)) {
        dialogMsg("请填写完整时间范围！", 2);
        return;
    }
    if ($("#StartDate").val().length > 0 && $("#EndDate").val().length > 0) {
        if (parseInt($("#StartDate").val()) > parseInt($("#EndDate").val())) {
            dialogMsg("开始年份不能大于结束年份！", 2);
            return;
        }
        year = $("#StartDate").val() + "-01-01|" + $("#EndDate").val() + "-12-31";
    }
    $("#gridTable2").jqGrid('setGridParam', {
        postData: { deptCode: deptCode, year: year, riskGrade: riskGrade }
    }).trigger('reloadGrid');

     

}



//根据区域或者作业类型查询安全风险清单
function SafetyByareaOrwork(type, typeNum) {

    //& nbsp; <a id="btn_Area" class="btn btn-primary" onclick="SafetyByareaOrwork('code','0017001001001001013')"><i class="fa fa-undo"></i>&nbsp;根据区域查询安全风险清单</a>
    //& nbsp; <a id="btn_Job" class="btn btn-primary" onclick="SafetyByareaOrwork('worktype','0017001001001001013')"><i class="fa fa-undo"></i>&nbsp;根据作业查询安全风险清单</a>
    var url = '../RiskDatabase/DangerSource/List?' + type + '=' + typeNum;
    top.openTab('d472030f-3094-4ad8-89cd-90c949fd4d91', url, '220KV升压站');
}




//风险数量统计
function stat() {
    var year = "";
    if (($("#StartDate").val().length > 0 && $("#EndDate").val().length == 0) || ($("#StartDate").val().length == 0 && $("#EndDate").val().length > 0)) {
        dialogMsg("请填写完整时间范围！", 2);
        return;
    }
    if ($("#StartDate").val().length > 0 && $("#EndDate").val().length > 0) {
        if (parseInt($("#StartDate").val()) > parseInt($("#EndDate").val())) {
            dialogMsg("开始年份不能大于结束年份！", 2);
            return;
        }
        year = $("#StartDate").val() + "-01-01|" + $("#EndDate").val() + "-12-31";
    }
    deptCode = $("#DeptCode").val().length == 0 ? top.currUserDeptCode : $("#DeptCode").val();
    $.get("getRiskCount", { deptCode: deptCode, year: year }, function (data) {

        var chart = new Highcharts.Chart({
            chart: {
                renderTo: 'piecontainer',
                plotBackgroundColor: null,
                plotBorderWidth: null,
                defaultSeriesType: 'pie'
            },
            title: {
                text: '风险数量统计'
            },
            exporting: {
                enabled: false
            },
            credits: {
                enabled: false
            },
            tooltip: {
                formatter: function () {
                    return '<b>数量：' + this.y + '个，占比：' + Highcharts.numberFormat(this.percentage, 2) + '%</b> ';
                }
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true, //点击切换
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        formatter: function () {
                            return '<b>' + this.point.name + '</b>: ' + Highcharts.numberFormat(this.percentage, 2) + ' %';
                        }
                    },
                    showInLegend: true
                }
            },
            series: [{
                data: eval(data)
            }]
            , lang: { noData: "暂无数据可显示!" },
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
    });

    //if (roleNames.indexOf("公司级用户") >= 0 || roleNames.indexOf("厂级部门用户") >= 0) {
        $.get("getAreaRiskCount", { deptCode: deptCode, year: year }, function (data) {

            var chart = new Highcharts.Chart({
                chart: {
                    renderTo: 'areacontainer',
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    defaultSeriesType: 'pie'
                },
                title: {
                    text: '区域风险数量统计'
                },
                exporting: {
                    enabled: false
                },
                credits: {
                    enabled: false
                },
                tooltip: {
                    formatter: function () {
                        return '<b>数量：' + this.y + '个，占比：' + Highcharts.numberFormat(this.percentage, 2) + '%</b> ';
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true, //点击切换
                        cursor: 'pointer'
                        ,dataLabels: {
                            enabled: true,
                            formatter: function () {  
                                return '<b>' + this.point.name + '</b>: ' + Highcharts.numberFormat(this.percentage, 2) + ' %';
                            }
                        },
                        showInLegend: true
                    }
                },
                series: [{ data: eval(data) }],
                //series: [{
                //    name: '百分比',
                //    colorByPoint: true,
                //    data: eval(data)
                //}],
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
        });
    //}
    var $gridTable = $("#gridTable");
    $gridTable.jqGrid({
        url: "../../RiskDatabase/RiskAssess/GetAreaRiskList",
        postData: { deptCode: deptCode, year: year },
        datatype: "json",
        mtype: "post",
        autoheight: true,
        autowidth: true,
        shrinkToFit: true,
        colModel: [
            { label: '区域', name: 'name', align: 'center' }, { label: '重大风险', name: 'lev1', align: 'center' }, { label: '较大风险', name: 'lev2', align: 'center' },
            { label: '一般风险', name: 'lev3', align: 'center' }, { label: '低风险', name: 'lev4', align: 'center' }, { label: '风险总数', name: 'sum', align: 'center' }, { label: '占比(%)', name: 'percent', align: 'center' }
        ],
        viewrecords: true,
        rowNum: 100,
        rownumbers: true
        ,gridComplete: function () {
 
        }
        , loadComplete: function (xhr) {
            var data = $gridTable.jqGrid('getRowData');
            var num1 = 0; var num2 = 0; var num3 = 0; var num4 = 0; var num1 = 0;
            $(data).each(function (i,item) {
                num1 += parseInt(item.lev1);
                num2 += parseInt(item.lev2);
                num3 +=parseInt(item.lev3);
                num4 += parseInt(item.lev4);
            });
            $gridTable.addRowData(4, { name: '全部', lev1: num1, lev2: num2, lev3: num3, lev4: num4, sum: (num1 + num2 + num3 + num4), percent: '-' });
            if (!!gxhs) {
                $gridTable.setLabel("lev1", "一级风险");
                $gridTable.setLabel("lev2", "二级风险");
                $gridTable.setLabel("lev3", "三级风险");
                $gridTable.setLabel("lev4", "四级风险");
            }
            
        }
    });
 
    //查询事件
    $("#btntj").click(function () {
        deptCode = $("#DeptCode").val().length == 0 ? top.currUserDeptCode : $("#DeptCode").val();
        if ($("#StartDate").val().length > 0 && $("#EndDate").val().length > 0) {
            year = $("#StartDate").val() + "-01-01|" + $("#EndDate").val() + "-12-31";
        }
        $gridTable.jqGrid('setGridParam', {
            postData: { deptCode: deptCode, year: year }, page: 1
        }).trigger('reloadGrid');
    });
}
//趋势图
function stat1() {
    var areaCode = $("#AreaCode").val();
    var year = "";
    if (($("#StartDate").val().length > 0 && $("#EndDate").val().length == 0) || ($("#StartDate").val().length == 0 && $("#EndDate").val().length > 0)) {
        dialogMsg("请填写完整时间范围！", 2);
        return;
    }
    if ($("#StartDate").val().length > 0 && $("#EndDate").val().length > 0) {
        if (parseInt($("#StartDate").val()) > parseInt($("#EndDate").val())) {
            dialogMsg("开始年份不能大于结束年份！", 2);
            return;
        }
        year = $("#StartDate").val() + "|" + $("#EndDate").val();
    }

    deptCode = $("#DeptCode").val().length == 0 ? top.currUserDeptCode : $("#DeptCode").val();
    $.get("getYearRiskCount", { deptCode: deptCode, year: year, riskGrade: riskGrade.toString(), areaCode: areaCode }, function (data) {
        var json = eval("(" + data + ")");
        $('#qscontainer').highcharts({
            chart: {
                type: 'line'
            },
            title: {
                text: '风险数量变化趋势'
            },
            xAxis: {
                categories: json.y
            },
            yAxis: {
                allowDecimals: false,
                min: 0,
                title: {
                    text: '数量（个）'
                },
                labels: {
                    formatter: function () {
                        return this.value + '个'
                    }
                }
            },
            exporting: {
                enabled: false
            },
            credits: {
                enabled: false
            },
            tooltip: {
                enabled: false,
                crosshairs: false,
                shared: false,
                formatter: function () {
                      return '<b>' + this.x + '</b><br/>风险数量： ' + this.y + '个';
                    }
            },
            plotOptions: {
                line: {
                    marker: {
                        enable: true,
                        radius: 5,
                        //lineColor: '#666666',
                        lineWidth: 1
                    },
                    dataLabels: {
                        enabled: true,
                        formatter: function () {
                            return '风险数量： ' + this.y + '个';
                        }
                    },
                    showInLegend: true
                }
            },
            series: [{
                name: '年份',
                marker: {
                    symbol: 'square'
                },
                data: json.x

            }]
            , lang: { noData: "暂无数据可显示!" },
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
    });

}
//按单位对比
function stat2() {
    var year = "";
    if ($("#StartDate").val().length > 0 && $("#EndDate").val().length > 0) {
        year = $("#StartDate").val() + "-01-01|" + $("#EndDate").val() + "-12-31";
    }
    deptCode = $("#DeptCode").val().length == 0 ? top.currUserDeptCode : $("#DeptCode").val();
    $.get("getRatherRiskCount", { deptCode: deptCode, riskGrade: riskGrade.toString(), year: year, areaCode: $("#AreaCode").val() }, function (data) {
        var json = eval("(" + data + ")");
        $('#dbcontainer').highcharts({
            chart: {
                type: 'column',
                borderWidth: 1
            },
            title: {
                text: '单位风险数量对比图'
            },
            xAxis: {
                categories: json.y
            },
            yAxis: {
                //alternateGridColor: '#FDFFD5',
                //gridLineColor: '#ff0000',
                lineWidth: 1,
                minorGridLineWidth: 0,
                minorTickInterval: 'auto',
                minorTickWidth: 1,
                gridLineDashStyle: 'longdash',
                allowDecimals: false,
                min: 0,
                title: {
                    text: '数量（个）'
                },
                labels: {
                    formatter: function () {
                        return this.value + '个'
                    }
                },
                stackLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                    }
                }
               
            },
            exporting: {
                enabled: false
            },
            credits: {
                enabled: false
            },
            tooltip: {
                crosshairs: true,
                shared: true
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
            series: json.x
            , lang: { noData: "暂无数据可显示!" },
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
    });
    riskGrade = riskGrade.toString();
    var $gridTable = $("#gridTable1");
    $gridTable.jqGrid({
        url: "../../RiskDatabase/RiskAssess/GetDeptRiskList",
        postData: { deptCode: deptCode, riskGrade: riskGrade, year: year },
        datatype: "json",
        mtype: "post",
        autoheight: true,
        shrinkToFit: true,
        autowidth: true,
        colModel: [
            { label: '部门', name: 'name', align: 'center' }, { label: '重大风险', name: 'lev1', align: 'center' }, { label: '较大风险', name: 'lev2', align: 'center' },
            { label: '一般风险', name: 'lev3', align: 'center' }, { label: '低风险', name: 'lev4', align: 'center' }, { label: '风险总数', name: 'sum', align: 'center' }, { label: '占比(%)', name: 'percent', align: 'center' }
        ],
        rowNum: 100,
        rownumbers: true
        , gridComplete: function () {
            if (riskGrade.indexOf("重大风险") < 0) {
                $gridTable.hideCol('lev1');
            } else {
                $gridTable.showCol('lev1');
            }
            if (riskGrade.indexOf("较大风险") < 0) {
                $gridTable.hideCol('lev2');
            } else {
                $gridTable.showCol('lev2');
            }
            if (riskGrade.indexOf("一般风险") < 0) {
                $gridTable.hideCol('lev3');
            } else {
                $gridTable.showCol('lev3');
            }
            if (riskGrade.indexOf("低风险") < 0) {
                $gridTable.hideCol('lev4');
            } else {
                $gridTable.showCol('lev4');
            }
        }
        , loadComplete: function (xhr) {
            if (!!gxhs) {
                $gridTable.setLabel("lev1", "一级风险");
                $gridTable.setLabel("lev2", "二级风险");
                $gridTable.setLabel("lev3", "三级风险");
                $gridTable.setLabel("lev4", "四级风险");
            }

        }
    });
}
//按区域对比
function stat3() {
    var year = "";
    if ($("#StartDate").val().length > 0 && $("#EndDate").val().length > 0) {
        year = $("#StartDate").val() + "-01-01|" + $("#EndDate").val() + "-12-31";
    }
    deptCode = $("#DeptCode").val().length == 0 ? top.currUserDeptCode : $("#DeptCode").val();
    $.get("GetAreaRatherRiskStat", { deptCode: deptCode, riskGrade: riskGrade.toString(), year: year }, function (data) {
        var json = eval("(" + data + ")");
        $('#dbcontainer').highcharts({
            chart: {
                type: 'column'
            },
            title: {
                text: '区域风险数量对比图'
            },
            xAxis: {
                categories: json.y
            },
            yAxis: {
                min: 0,
                title: {
                    text: '数量（个）'
                },
                labels: {
                    formatter: function () {
                        return this.value + '个'
                    }
                },
                stackLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                    }
                }
            },
            exporting: {
                enabled: false
            },
            credits: {
                enabled: false
            },
            tooltip: {
                crosshairs: true,
                shared: true
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
            series: json.x
            , lang: { noData: "暂无数据可显示!" },
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
    });

    var $gridTable = $("#gridTable2");
    $gridTable.jqGrid({
        url: "../../RiskDatabase/RiskAssess/GetAreaRiskList",
        postData: { deptCode: deptCode, riskGrade: riskGrade.toString(), year: year },
        datatype: "json",
        mtype: "post",
        autoheight: true,
        width: $(window).width() - 100,
        colModel: [
            { label: '区域', name: 'name', align: 'center' }, { label: '重大风险', name: 'lev1', align: 'center' }, { label: '较大风险', name: 'lev2', align: 'center' },
            { label: '一般风险', name: 'lev3', align: 'center' }, { label: '低风险', name: 'lev4', align: 'center' }, { label: '风险总数', name: 'sum', align: 'center' }, { label: '占比(%)', name: 'percent', align: 'center' }
        ],
        rowNum: 100,
        rownumbers: true
        , gridComplete: function () {
            if (riskGrade.indexOf("重大风险") < 0) {
                $gridTable.hideCol('lev1');
            } else {
                $gridTable.showCol('lev1');
            }
            if (riskGrade.indexOf("较大风险") < 0) {
                $gridTable.hideCol('lev2');
            } else {
                $gridTable.showCol('lev2');
            }
            if (riskGrade.indexOf("一般风险") < 0) {
                $gridTable.hideCol('lev3');
            } else {
                $gridTable.showCol('lev3');
            }
            if (riskGrade.indexOf("低风险") < 0) {
                $gridTable.hideCol('lev4');
            } else {
                $gridTable.showCol('lev4');
            }
        }, loadComplete: function (xhr) {
            if (!!gxhs) {
                $gridTable.setLabel("lev1", "一级风险");
                $gridTable.setLabel("lev2", "二级风险");
                $gridTable.setLabel("lev3", "三级风险");
                $gridTable.setLabel("lev4", "四级风险");
            }

        }
    });


}
//跳转到指定模块菜单
function OpenNav(Navid) {
    top.$("#nav").find('a#' + Navid).trigger("click");
}
function query() {
    riskGrade = $("#riskGrade").val();
    riskGrade = riskGrade == null || riskGrade == "" ? "低风险,一般风险,较大风险,重大风险" : riskGrade;
    if (state == 1) {
        $("#btntj").trigger("click"); stat();
    }
    if (state == 2) {
        $("#btnqs").trigger("click");
    }
    if (state == 3) {
        dwSearch();
    }
    if (state == 4) {
        qySearch();
    }
}
//重置查询
function reset() {
    $("#DeptCode").val(""); $("#DeptName").val(""); $("#StartDate").val(""); $("#EndDate").val(""); $("#AreaName").val(""); $("#AreaCode").val("");
    $("#riskGrade").val(""); $("#riskGrade").selectpicker("refresh");
    if (state == 1) {
        $("#btntj").trigger("click"); stat();
    }
    if (state == 2) {
        $("#btnqs").trigger("click");
    }
    if (state == 3) {
        dwSearch();
    }
    if (state == 4) {
        qySearch()
    }
}
//选择单位回调
function callBackSelect() {
    $.ajax({
        url: '../../RiskDatabase/RiskAssess/GetDept?deptCode=' + $("#DeptCode").val() ,
        dataType: "JSON",
        success: function (result) {
            if (result.Nature == "厂级") {
                 $(".btn11").show(); $("#dbt").hide();
                $("#div2").show(); $("#div1").css({ "width": "50%" });
                $("#btndb").removeAttr("aria-expanded");
                $(".dropdown-menu").removeAttr("style");
                query();
               
            } else {
                $(".btn11").hide(); $("#dbt").show();
                $("#div2").hide(); $("#div1").css({ "width": "100%" });
                $(".highcharts-container").width("100%"); $("svg").width("100%");
                query();
            }
        }
    });
   
   
}
