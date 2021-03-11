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
        year = $("#StartDate").val() + "|" + $("#EndDate").val() + "";
    }
    $.get("GetLogChart", { deptCode: deptCode, year: year }, function (data) {

        var chart = new Highcharts.Chart({
            chart: {
                renderTo: 'piecontainer',
                plotBackgroundColor: null,
                plotBorderWidth: null,
                defaultSeriesType: 'pie'
            },
            title: {
                text: '车辆统计'
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


    var $gridTable = $("#gridTable");
    $gridTable.jqGrid({
        url: "../../CarManage/Carinlog/GetTableChar",
        postData: { year: year },
        datatype: "json",
        mtype: "post",
        autoheight: true,
        autowidth: true,
        colModel: [
            {
                label: '车辆类型', name: 'type', sortable: false, align: 'center',
                formatter: function (cellvalue, options, rowObject) {
                    switch (cellvalue) {
                        case 0:
                            return "电厂班车";
                            break;
                        case 1:
                            return "私家车";
                            break;
                        case 2:
                            return "商务公车";
                            break;
                        case 3:
                            return "拜访车辆";
                            break;
                        case 4:
                            return "物料车辆";
                            break;
                        case 5:
                            return  "危化品车辆";
                            break;
                    }
                    return "";

                }
            },
            { label: '进出车次', name: 'carnum', sortable: false, align: 'center' },
            {
                label: '比例', name: 'bl', sortable: false, align: 'center',
                formatter: function (cellvalue, options, rowObject) {
                    return cellvalue.toFixed(2) + "%";

                }
            },

        ],
        viewrecords: true,
        rowNum: 100,
        rownumbers: true
        , gridComplete: function () {

        }
        , loadComplete: function (xhr) {
            var data = $gridTable.jqGrid('getRowData');
        }
    });

    //查询事件
    $("#btntj").click(function () {
        if ($("#StartDate").val().length > 0 && $("#EndDate").val().length > 0) {
            year = $("#StartDate").val() + "|" + $("#EndDate").val() + "";
        }
        $gridTable.jqGrid('setGridParam', {
            postData: { year: year }, page: 1
        }).trigger('reloadGrid');
    });
}

function stat1() {
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
        year = $("#StartDate").val() + "|" + $("#EndDate").val() + "";
    }
    var $gridTable = $("#gridTable2");
    $gridTable.jqGrid({
        url: "../../CarManage/Carinlog/GetLogDetail",
        postData: { year: year, CarType: CarType, Status: InLog },
        datatype: "json",
        mtype: "post",
        height: $(window).height() - 220,
        autowidth: true,
        colModel: [
            { label: '驾驶人', name: 'dirver', hidden: true },
            { label: '电话', name: 'phone', hidden: true },
            { label: '驾驶人', name: 'vdirver', hidden: true },
            { label: '电话', name: 'vphone', hidden: true },
            { label: '驾驶人', name: 'drivername', hidden: true },
            { label: '电话', name: 'drivertel', hidden: true },
            { label: '车牌', name: 'carno', sortable: false, align: 'center' },
            {
                label: '车辆类型', name: 'type', align: 'center', sortable: false,
                formatter: function (cellvalue, options, rowObject) {
                    switch (cellvalue) {
                        case 0:
                            return "电厂班车";
                            break;
                        case 1:
                            return "私家车";
                            break;
                        case 2:
                            return "商务公车";
                            break;
                        case 3:
                            return "拜访车辆";
                            break;
                        case 4:
                            return "物料车辆";
                            break;
                        case 5:
                            return "危化品车辆";
                            break;
                    }
                    return "";

                }
            },
            {
                label: '驾驶人', name: 'drivername', sortable: false, align: 'center'
            },
            {
                label: '电话', name: 'phone', sortable: false, align: 'center'
            },
            {
                label: '进出类型', name: 'status', sortable: false, align: 'center',
                formatter: function (cellvalue, options, rowObject) {
                    if (cellvalue == 0) {
                        return "进厂";
                    } else {
                        return "出厂";
                    }

                }
            },
            { label: '经过门岗', name: 'address', sortable: false, align: 'center' },
            { label: '时间', name: 'createdate', sortable: false, align: 'center' }

        ],
        viewrecords: true,
        rowNum: 100,
        rownumbers: true
        , gridComplete: function () {

        }
        , loadComplete: function (xhr) {
            var data = $gridTable.jqGrid('getRowData');
        }
    });

    //查询事件
    $("#btnqs").click(function () {
        year = "";
        if ($("#StartDate").val().length > 0 && $("#EndDate").val().length > 0) {
            year = $("#StartDate").val() + "|" + $("#EndDate").val() + "";
        }
        $gridTable.jqGrid('setGridParam', {
            postData: { year: year, CarType: CarType, Status: InLog }, page: 1
        }).trigger('reloadGrid');
    });
}


function query() {
    CarType = $("#CarType").val();
    InLog = $("#InLog").val();
    if (state == 1) {
        $("#btntj").trigger("click"); stat();
    }
    if (state == 2) {
        $("#btnqs").trigger("click");
    }
}
//重置查询
function reset() {
    $("#StartDate").val(""); $("#EndDate").val("");
    $("#CarType").val(""); $("#CarType").selectpicker("refresh");
    $("#InLog").val(""); $("#InLog").selectpicker("refresh");
    CarType = $("#CarType").val();
    InLog = $("#InLog").val();
    if (state == 1) {
        $("#btntj").trigger("click"); stat();
    }
    if (state == 2) {
        $("#btnqs").trigger("click");
    }
}