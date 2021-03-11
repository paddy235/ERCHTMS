
//统计图
function statdata(GetData, year, type, text, mode) {
    var multiselect = false;
    if (mode === 2) {
        multiselect = true;
    }
    $.get(GetData, { year: year, mode: mode }, function (data) {
        var json = eval("(" + data + ")");
        $("#c" + mode).val();
        $("#c" + mode).highcharts({
            chart: {
                type: type
            },
            title: {
                text: text,
                x: -20 //center
            },
            xAxis: {
                categories: json.x
            },
            exporting:{
                url: "../../PowerPlantInside/PowerPlantInside/Export",
                enabled: true,
                filename: text,
                width: 1200
            },
            lang: {
                printChart: "打印图表",
                downloadJPEG: "下载JPEG图片",
                downloadPDF: "下载PDF文件",
                downloadPNG: "下载PNG图片",
                downloadSVG: "下载SVG文件"
            },
            yAxis: {
                min: 0,
                title: {
                    text: '次数'
                },
                labels: {
                    formatter: function () {
                        return this.value + '次';
                    }
                },
                stackLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                    }
                },
                tickInterval: 1
            },
            legend: {
                enabled: multiselect,
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            tooltip: {
                headerFormat: '<b>{point.x}</b><br/>',
                pointFormat: '{point.y}次'
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
            series: json.y
        });
    });
};

//事故事件类型统计
function binggrid0(year, type) {
    statdata("GetStatisticsHighchart", year, "column", year+"年事故事件类型统计", type);
    $("#divgrid").html("");
    $("#divgrid").append("<table id='gridTable'></table>");
    $("#gridTable").jqGrid({
        url: "../../PowerPlantInside/PowerPlantInside/GetStatisticsList",
        postData: { year: year, mode: type },
        datatype: "json",
        mtype: "post",
        autoheight: true,
        width: $(window).width() - 100,
        colModel: [
            { label: '类型', name: 'type', align: 'center', sortable: false },
            { label: '人身', name: 'num1', align: 'center', sortable: false },
            { label: '设备', name: 'num2', align: 'center', sortable: false },
            { label: '消防', name: 'num3', align: 'center', sortable: false },
            { label: '交通', name: 'num4', align: 'center', sortable: false },
            { label: '环保', name: 'num5', align: 'center', sortable: false },
            { label: '职业健康', name: 'num6', align: 'center', sortable: false },
            { label: '总计', name: 'Total', align: 'center', sortable: false }

        ],
        rowNum: 100,
        rownumbers: true
        , gridComplete: function () {

        }
    });
}

//事故事件性质统计
function binggrid1(year, type) {
    statdata("GetStatisticsHighchart", year, "column", year + "年事故事件性质统计", type);
    $("#divgrid").html("");
    $("#divgrid").append("<table id='gridTable'></table>");
    $("#gridTable").jqGrid({
        url: "../../PowerPlantInside/PowerPlantInside/GetStatisticsList",
        postData: { year: year, mode: type },
        datatype: "json",
        mtype: "post",
        autoheight: true,
        width: $(window).width() - 100,
        colModel: [
            { label: '性质', name: 'type', align: 'center', sortable: false },
            { label: '二类障碍', name: 'num1', align: 'center', sortable: false },
            { label: '异常', name: 'num2', align: 'center', sortable: false },
            { label: '未遂', name: 'num3', align: 'center', sortable: false },
            { label: '小微事件', name: 'num4', align: 'center', sortable: false },
            { label: '一类障碍', name: 'num5', align: 'center', sortable: false },
            { label: '一般', name: 'num6', align: 'center', sortable: false },
            { label: '工余事件', name: 'num6', align: 'center', sortable: false },
            { label: '总计', name: 'Total', align: 'center', sortable: false }

        ],
        rowNum: 100,
        rownumbers: true
        , gridComplete: function () {

        }
    });
}

//影响事故事件因素统计
function binggrid2(year, type) {
    statdata("GetStatisticsHighchart", year, "line", year + "年影响事故事件因素统计", type);
    $("#divgrid").html("");
    $("#divgrid").append("<table id='gridTable'></table>");
    $("#gridTable").jqGrid({
        url: "../../PowerPlantInside/PowerPlantInside/GetStatisticsList",
        postData: { year: year, mode: type },
        datatype: "json",
        mtype: "post",
        autoheight: true,
        width: $(window).width() - 100,
        colModel: [
            { label: '影响因素', name: 'TypeName', align: 'center', sortable: false },
            { label: '1月', name: 'num1', align: 'center', sortable: false },
            { label: '2月', name: 'num2', align: 'center', sortable: false },
            { label: '3月', name: 'num3', align: 'center', sortable: false },
            { label: '4月', name: 'num4', align: 'center', sortable: false },
            { label: '5月', name: 'num5', align: 'center', sortable: false },
            { label: '6月', name: 'num6', align: 'center', sortable: false },
            { label: '7月', name: 'num7', align: 'center', sortable: false },
            { label: '8月', name: 'num8', align: 'center', sortable: false },
            { label: '9月', name: 'num9', align: 'center', sortable: false },
            { label: '10月', name: 'num10', align: 'center', sortable: false },
            { label: '11月', name: 'num11', align: 'center', sortable: false },
            { label: '12月', name: 'num12', align: 'center', sortable: false },
            { label: '总计', name: 'Total', align: 'center', sortable: false }

        ],
        rowNum: 100,
        rownumbers: true
        , gridComplete: function () {

        }
    });
}

//事故事件发生的部门统计
function binggrid3(year, type) {
    statdata("GetStatisticsHighchart", year, "column", year + "年事故事件发生的部门统计", type);
    $("#divgrid").html("");
    $("#divgrid").append("<table id='gridTable'></table>");
    $("#gridTable").jqGrid({
        url: "../../PowerPlantInside/PowerPlantInside/GetStatisticsList",
        postData: { year: year, mode: type },
        datatype: "json",
        mtype: "post",
        autoheight: true,
        width: $(window).width() - 100,
        colModel: [
            { label: '各个部门', name: 'dept', align: 'center', sortable: false },
            { label: '营销部', name: 'num1', align: 'center', sortable: false },
            { label: '技术支持部', name: 'num2', align: 'center', sortable: false },
            { label: '发电部', name: 'num3', align: 'center', sortable: false },
            { label: '办公室', name: 'num4', align: 'center', sortable: false },
            { label: 'EHS部', name: 'num5', align: 'center', sortable: false },
            { label: '相关方', name: 'num6', align: 'center', sortable: false },
            { label: '全厂', name: 'Total', align: 'center', sortable: false }
        ],
        rowNum: 100,
        rownumbers: true
        , gridComplete: function () {

        }
    });
}





//事故事件所属专业统计
function binggrid4(year, type) {
    statdata("GetStatisticsHighchart", year, "column", year + "年事故事件所属专业统计", type);
    $("#divgrid").html("");
    $("#divgrid").append("<table id='gridTable'></table>");
    $("#gridTable").jqGrid({
        url: "../../PowerPlantInside/PowerPlantInside/GetStatisticsList",
        postData: { year: year, mode: type },
        datatype: "json",
        mtype: "post",
        autoheight: true,
        width: $(window).width() - 100,
        colModel: [
            { label: '所属专业', name: 'type', align: 'center', sortable: false },
            { label: '汽机专业', name: 'num1', align: 'center', sortable: false },
            { label: '锅炉专业', name: 'num2', align: 'center', sortable: false },
            { label: '电气专业', name: 'num3', align: 'center', sortable: false },
            { label: '灰硫专业', name: 'num4', align: 'center', sortable: false },
            { label: '化学专业', name: 'num5', align: 'center', sortable: false },
            { label: '燃料专业', name: 'num6', align: 'center', sortable: false },
            { label: '热控专业', name: 'num7', align: 'center', sortable: false },
            { label: '总计', name: 'Total', align: 'center', sortable: false }

        ],
        rowNum: 100,
        rownumbers: true
        , gridComplete: function () {

        }
    });
}

//事故事件所属机组统计
function binggrid5(year, type) {
    statdata("GetStatisticsHighchart", year, "column", year + "年事故事件所属系统统计", type);
    $("#divgrid").html("");
    $("#divgrid").append("<table id='gridTable'></table>");
    $("#gridTable").jqGrid({
        url: "../../PowerPlantInside/PowerPlantInside/GetStatisticsList",
        postData: { year: year, mode: type },
        datatype: "json",
        mtype: "post",
        autoheight: true,
        width: $(window).width() - 100,
        colModel: [
            { label: '所属系统', name: 'type', align: 'center', sortable: false },
            { label: '#1机组', name: 'num1', align: 'center', sortable: false },
            { label: '#2机组', name: 'num2', align: 'center', sortable: false },
            { label: '#3机组', name: 'num3', align: 'center', sortable: false },
            { label: '#4机组', name: 'num4', align: 'center', sortable: false },
            { label: '公用系统', name: 'num5', align: 'center', sortable: false },
            { label: '输煤系统', name: 'num6', align: 'center', sortable: false },
            { label: '灰硫化系统', name: 'num7', align: 'center', sortable: false },
            { label: '总计', name: 'Total', align: 'center', sortable: false }

        ],
        rowNum: 100,
        rownumbers: true
        , gridComplete: function () {

        }
    });
}
//月度变化统计列表
function binggrid6(year, type) {
    statdata("GetStatisticsHighchart", year, "line", year + "年月度变化统计", type);
    $("#divgrid").html("");
    $("#divgrid").append("<table id='gridTable'></table>");
    $("#gridTable").jqGrid({
        url: "../../PowerPlantInside/PowerPlantInside/GetStatisticsList",
        postData: { year: year, mode: type },
        datatype: "json",
        mtype: "post",
        autoheight: true,
        width: $(window).width() - 100,
        colModel: [
            { label: '月份', name: 'cs', align: 'center', sortable: false },
            { label: '1月', name: 'num1', align: 'center', sortable: false },
            { label: '2月', name: 'num2', align: 'center', sortable: false },
            { label: '3月', name: 'num3', align: 'center', sortable: false },
            { label: '4月', name: 'num4', align: 'center', sortable: false },
            { label: '5月', name: 'num5', align: 'center', sortable: false },
            { label: '6月', name: 'num6', align: 'center', sortable: false },
            { label: '7月', name: 'num7', align: 'center', sortable: false },
            { label: '8月', name: 'num8', align: 'center', sortable: false },
            { label: '9月', name: 'num9', align: 'center', sortable: false },
            { label: '10月', name: 'num10', align: 'center', sortable: false },
            { label: '11月', name: 'num11', align: 'center', sortable: false },
            { label: '12月', name: 'num12', align: 'center', sortable: false },
            { label: '总计', name: 'Total', align: 'center', sortable: false }

        ],
        rowNum: 100,
        rownumbers: true
        , gridComplete: function () {

        }
    });
}

//年度变化统计列表
function binggrid7(year, selt, type) {
    var oldyear = year - 5;
    var colmodel = [{ label: '年份', name: 'cs', align: 'center', sortable: false }];
    if (selt === "2") {
        oldyear = year - 10;
    }
    statdata("GetStatisticsHighchart", oldyear, "column", oldyear + 1 + "-" + year + "年度变化统计", type);
    for (var i = oldyear + 1; i <= year; i++) {
        colmodel.push({ label: i, name: 'num' + i, align: 'center', sortable: false });
    }
    $("#divgrid").html("");
    $("#divgrid").append("<table id='gridTable'></table>");
    $("#gridTable").jqGrid({
        url: "../../PowerPlantInside/PowerPlantInside/GetStatisticsList",
        postData: { year: oldyear,mode:type },
        datatype: "json",
        mtype: "post",
        autoheight: true,
        height: 300,
        width: $(window).width() - 100,
        colModel: colmodel,
        rowNum: 100,
        rownumbers: true
        , gridComplete: function () {

        }
    });
}
