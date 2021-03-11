//安全状态
function getSafeStatus() {
    $.ajax({
        dataType: "json",
        type: "get",
        url: "../../home/GetIndexWarnValue",
        success: function (json) {
            if (!!json.score) {
                var curindex = json.index;
                var scorearry = json.scorearry;
                var firstnum = 0;
                var secondnum = 0;
                var thirdnum = 0;
                var fourthnum = 0;
                if (!!scorearry) {
                    firstnum = parseFloat(scorearry[0] / 100).toFixed(1);
                    secondnum = parseFloat(scorearry[1] / 100).toFixed(1);
                    thirdnum = parseFloat(scorearry[2] / 100).toFixed(1);
                    fourthnum = parseFloat(scorearry[3] / 100).toFixed(1);
                }

                switch (curindex) {
                    case 0:
                        $("#warnLevel").html("<p style='color:#fdb381'>目前处于危险状态</p>");
                        break;
                    case 1:
                        $("#warnLevel").html("<p style='color:#f8df76'>目前处于较危险状态</p>");
                        break;
                    case 2:
                        $("#warnLevel").html("<p style='color:#96e8f6'>目前处于较安全状态</p>");
                        break;
                    case 3:
                        $("#warnLevel").html("<p style='color:#5ae6b6'>目前处于安全状态</p>");
                        break;
                }
                var option = {
                    tooltip: {
                        formatter: "{a}<br/>{c}{b}分",
                        backgroundColor: 'rgba(255,255,255,0.8)',
                        textStyle: {
                            color: 'black',
                        }

                    },
                    legend: {
                        x: 'left',
                        bottom: '14%',
                        left: '3%',
                        icon: 'rect',
                        data: ['危险', '较危险', '较安全', '安全'],
                        textStyle: {
                            color: '#8492af',
                            fontSize: 18
                        },
                        itemGap: 30,
                        color: ['#fdb381', '#f8df76', '#96e8f6', '#5ae6b6'],

                    },
                    toolbox: {
                        show: false,
                        feature: {
                            mark: {
                                show: true
                            },
                            restore: {
                                show: true
                            },
                            saveAsImage: {
                                show: true
                            }
                        },
                        extraCssText: 'width:160px;height:50px;background:#fff;'
                    },

                    xAxis: [ //这里有很多的show，必须都设置成不显示
                        {
                            type: 'category',
                            data: [],
                            axisLine: {
                                show: false
                            },
                            splitLine: {
                                show: false
                            },
                            splitArea: {
                                // interval: 'auto',
                                show: false
                            }
                        }
                    ],
                    yAxis: [ //这里有很多的show，必须都设置成不显示
                        {
                            type: 'value',
                            axisLine: {
                                show: false
                            },
                            splitLine: {
                                show: false
                            },
                        }
                    ],
                    series: [{
                        name: '危险',
                        type: 'bar',
                        itemStyle: {
                            normal: {
                                color: '#ff4157'
                            }
                        }
                    },
                        {
                            name: '较危险',
                            type: 'bar',
                            itemStyle: {
                                normal: {
                                    color: '#f8df76'
                                }
                            }

                        },
                        {
                            name: '较安全',
                            type: 'bar',
                            itemStyle: {
                                normal: {
                                    color: '#96e8f6'
                                }
                            }

                        },
                        {
                            name: '安全',
                            type: 'bar',
                            itemStyle: {
                                normal: {
                                    color: '#5ae6b6'
                                }
                            }

                        },
                        {
                            name: '预警指数',
                            type: 'gauge',
                            center: ['50%', '50%'], // 默认全局居中
                            radius: '75%',
                            startAngle: 180,
                            endAngle: 0,
                            axisLine: {
                                show: true,
                                lineStyle: { // 属性lineStyle控制线条样式
                                    width: 36,
                                    color: [[firstnum, '#ff4157'], [secondnum, '#fba73a'], [thirdnum, '#90eaf9'], [fourthnum, '#5ae6b6']]
                                },

                            },
                            splitLine: { // 分隔线
                                length: 46, // 属性length控制线长
                                lineStyle: { // 属性lineStyle（详见lineStyle）控制线条样式
                                    width: 3
                                }
                            },
                            axisTick: { // 坐标轴小标记
                                length: 15, // 属性length控制线长

                            },
                            axisLabel: {
                                color: '#fff',
                                fontSize: 18,
                                distance: -80
                            },
                            pointer: {
                                width: 2
                            },
                            detail: {
                                // 其余属性默认使用全局文本样式，详见TEXTSTYLE
                                fontWeight: 'bolder',
                                formatter: "{score|{value}分}",
                                offsetCenter: [0, '-40%'],
                                height: 30,
                                rich: {
                                    score: {
                                        color: '#ff9937',
                                        fontFamily: '微软雅黑',
                                        fontSize: 42,
                                    }
                                }

                            },
                            data: [{
                                value: json.score,
                                label: {
                                    textStyle: {
                                        fontSize: 12
                                    }
                                }
                            }]
                        }
                    ]
                };
                var chart = echarts.init(document.querySelector('.charts1'));
                chart.setOption(option);
            }
        }
    });
}
//未闭环隐患统计
function getNoCloseHTInfo() {
    $.ajax({
        dataType: "json",
        type: "post",
        url: "../../Desktop/GetNoCloseLoopHidStatistics",
        success: function (json) {
            if (json.type == 1) {
                var html = "";
                $("#tabStat").html("");
                var data = $.parseJSON(json.resultdata);
                $(data.unitdata).each(function (index, ele) {
                    var curIndex = index + 1;
                    if (ele.s1 != "0(0%)") {
                        sHtml1 = "<a style=\"color:red;\" onclick=\"viewHid(0,'" + ele.code + "','一般隐患')\">" + ele.s1 + "</a>";
                    }
                    else {
                        sHtml1 = ele.s1;
                    }
                    if (ele.s2 != "0(0%)") {
                        sHtml2 = "<a style=\"color:red;\" onclick=\"viewHid(0,'" + ele.code + "','重大隐患')\">" + ele.s2 + "</a>";
                    }
                    else {
                        sHtml2 = ele.s2;
                    }
                    var trText = "<tr class='row1'><td>" + ele.name + "</td><td>" + sHtml1 + "</td><td>" + sHtml2 + "</td></tr>";
                    $("#tabStat").append(trText);
                });
            }
        }
    });
}
//高风险作业统计
function getWorkStat() {
    $.ajax({
        dataType: "json",
        type: "get",
        url: "../../DangerjobStatistics/GetDangerousJobCount",
        success: function (json) {
            var data = [];
            $(json).each(function (j, item) {
                data.push({
                    name: item.name,
                    value: item.y
                });
            });
            //data[6].value = 10;
            var color = ['#ff7f50', '#da70d6', '#32cd32', '#6495ed', '#ff69b4', '#ba55d3', '#cd5c5c', '#ffa500', '#ffa500', '#54b1f9']
            var option = {
                legend: {
                    selectedMode: true,
                    bottom: 15,
                    left: 0,
                    itemGap: 15,
                    itemWidth: 13,
                    itemHeight: 8,
                    width: 650,
                    x: 'center',
                    textStyle: {
                        fontSize: 12,
                        color: '#cccccc',
                        textAlign: 'left'

                    }
                    // data: ['有限空间作业', '高处作业', '电除尘拆作业', '吸收塔防腐作业', '夜间作业', '有毒有害作业', '起重吊装作业', '脚手架搭设作业', '脚手架拆除作业', '安全设施变动'],
                },

                series: [{
                    type: 'pie',
                    hoverAnimation: true,
                    radius: '45%',
                    center: ['50%', '45%'],
                    //roseType: 'radius',
                    //color: color,
                    label: {
                        normal: {
                            show: true,
                            position: 'absolute',
                            color: '#cccccc',
                            fontSize: 14
                        }
                    },
                    itemStyle: {
                        normal: {
                            label: {
                                show: true,
                                formatter: '{b}:{c}  {d}%'
                            }
                        }
                    },
                    labelLine: {
                        normal: {
                            // formatter:'{c}% \n {b} \n\n',

                            show: true,
                            length: 20,
                            length2: 15,
                            lineStyle: {

                            }
                        }
                    },
                    data: data
                }]
            }
            var chart = echarts.init(document.querySelector('.charts2'));
            chart.setOption(option);
        }
    });
}
//隐患整改情况统计
function getHTChangeStat() {
    $.ajax({
        dataType: "json",
        type: "post",
        url: "../../Desktop/GetHiddenChangeForLeaderCockpit",
        success: function (json) {
            if (json.type == 1) {
                var deptdata = new Array();
                var ybdata = new Array();
                var zddata = new Array();
                var zgldata = new Array();
                var data = $.parseJSON(json.resultdata);
                $(data).each(function (index, ele) {
                    if (index < 9) {
                        if (!!ele) {
                            deptdata.push(ele.fullname);
                            ybdata.push(ele.ybcount);
                            zddata.push(ele.zdcount);
                            zgldata.push(ele.zgl);
                        }
                    }

                });
                var option = {
                    title: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'shadow',
                            shadowStyle: {
                                color: 'rgba(255,255,255,.1)'
                            }
                        },
                        //   text:'隐患整改率',
                        textStyle: {
                            fontSize: 14,
                            color: '#fff'
                        }
                    },
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'shadow',
                            shadowStyle: {
                                color: 'rgba(255,255,255,.1)'
                            }
                        },
                         formatter:function (params) {
                             console.log(params)

                             var relVal = params[0].name;
                             for (var i =0, l = params.length; i < l; i++){
                                 i === 1 ? relVal += '<br/>' + params[i].marker + params[i].seriesName + "：" + params[i].value + '%' : relVal += '<br/>' + params[i].marker + params[i].seriesName + "：" + params[i].value
                             }
                             return relVal;

                         }
                    },
                    legend: {
                        data: ['一般隐患', '重大隐患', '整改率'],
                        x: 'center',
                        itemGap: 30,
                        textStyle: {
                            fontSize: 12,
                            color: '#ccc'
                        }
                    },
                    toolbox: {
                    },
                    calculable: true,
                    grid: {
                        left: '1%',
                        right: '1%',
                        bottom: '1%',
                        top: '15%',
                        containLabel: true
                    },
                    xAxis: [
                        {
                            type: 'category',
                            axisLabel: {
                                textStyle: {
                                    color: '#ccc',
                                    fontSize: 12
                                },
                                interval: 0,
                                formatter: function (params) {
                                    var newParamsName = '';
                                    var paramsNameNumber = params.length;
                                    var providerNumer = 4;
                                    var rowNumber = Math.ceil(paramsNameNumber / providerNumer)
                                    if (paramsNameNumber > providerNumer) {
                                        for (var p = 0; p < rowNumber; p++) {
                                            var tempStr = '';
                                            var start = p * providerNumer;
                                            var end = start + providerNumer;
                                            if (p == rowNumber - 1) {
                                                tempStr = params.substring(start, paramsNameNumber)
                                            } else {
                                                tempStr = params.substring(start, end) + '\n'
                                            }
                                            newParamsName += tempStr
                                        }
                                    } else {
                                        newParamsName = params;
                                    }

                                    return newParamsName

                                },
                            },
                            axisLine: {
                                lineStyle: {
                                    color: "rgba(241,245,248,.2)"
                                }
                            },
                            data: deptdata
                        },
                        {
                            type: 'category',
                            axisLine: { show: false },
                            axisTick: { show: false },
                            axisLabel: {
                                show: false,
                                interval: 0,
                                formatter: function (params) {
                                    var newParamsName = '';
                                    var paramsNameNumber = params.length;
                                    var providerNumer = 4;
                                    var rowNumber = Math.ceil(paramsNameNumber / providerNumer)
                                    if (paramsNameNumber > providerNumer) {
                                        for (var p = 0; p < rowNumber; p++) {
                                            var tempStr = '';
                                            var start = p * providerNumer;
                                            var end = start + providerNumer;
                                            if (p == rowNumber - 1) {
                                                tempStr = params.substring(start, paramsNameNumber)
                                            } else {
                                                tempStr = params.substring(start, end) + '\n'
                                            }
                                            newParamsName += tempStr
                                        }
                                    } else {
                                        newParamsName = params;
                                    }

                                    return newParamsName

                                }
                            },
                            splitArea: { show: false },
                            splitLine: { show: false, },
                            data: deptdata
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value',
                            min: 0,
                            minInterval: 1,
                            name: '数量',
                            nameTextStyle: {
                                color: 'rgba(255,255,255,.6)',
                                fontSize: 12,
                            },
                            nameGap: 30,
                            axisLabel: {
                                formatter: '{value}',
                                textStyle: {
                                    color: 'rgba(255,255,255,.6)'
                                }
                            },
                            axisLine: {
                                lineStyle: {
                                    color: "rgba(85,113,255,.3)"
                                }
                            },
                            splitLine: {
                                lineStyle: {
                                    color: "rgba(85,113,255,.3)"
                                }
                            }
                        },
                        {
                            type: 'value',
                            name: '整改率',
                            nameTextStyle: {
                                color: 'rgba(255,255,255,.6)',
                                fontSize: 12
                            },
                            min:0,max:100,
                            nameGap: 30,
                            axisLabel: {
                                formatter: '{value} %',
                                textStyle: {
                                    color: '#c7e6fa'
                                }
                            },
                            axisLine: {
                                lineStyle: {
                                    color: "rgba(241,245,248,.2)"
                                }
                            },
                            splitLine: {
                                lineStyle: {
                                    color: "rgba(241,245,248,.2)"
                                }
                            }
                        },

                    ],
                    series: [
                        {
                            name: '一般隐患',
                            type: 'bar',
                            stack: "数量",
                            barWidth: '35%',
                            itemStyle: {
                                normal: {
                                    color: new echarts.graphic.LinearGradient(
                                        0, 0, 0, 1,
                                        [{ offset: 0, color: '#3a7be7' },
                                            { offset: 0.5, color: '#6b9cef' },

                                            { offset: 1, color: '#91b7f6' },

                                        ]
                                    ),
                                    label: {
                                        show: true,
                                        position: 'insideRight',
                                        textStyle: {
                                            color: '#ccc'
                                        }
                                    }
                                }
                            },
                            data: ybdata
                        },
                        {
                            name: '重大隐患',
                            stack: "数量",
                            type: 'bar',
                            xAxisIndex: 1,
                            barWidth: '35%',
                            itemStyle: {
                                normal: {
                                    color: '#d89502',
                                    label: {
                                        //    formatter:'{c} %',

                                        show: true,
                                        position: 'insideRight',
                                        textStyle: {
                                            color: '#ccc'
                                        }
                                    }
                                }
                            },
                            data: zddata
                        },
                        {
                            name: '整改率',
                            type: 'line',
                            yAxisIndex: 1,
                            symbol: 'circle',
                            symbolSize: 6,
                            itemStyle: {
                                normal: {
                                    color: '#00b286'
                                }
                            },
                            data: zgldata
                        }
                    ]

                }
                var chart = echarts.init(document.querySelector('.charts3'));
                chart.setOption(option);
            }
        }
    });

};
//违章统计
function getWZStat() {
    $.ajax({
        dataType: "json",
        type: "post",
        url: "../../Desktop/GetLllegalChangeForLeaderCockpit",
        success: function (json) {
            if (json.type == 1) {
                var deptdata = new Array();
                var ybdata = new Array();
                var jyzdata = new Array();
                var yzdata = new Array();
                var zgldata = new Array();
                var data = $.parseJSON(json.resultdata);
                $(data).each(function (index, ele) {
                    if (!!ele) {
                        deptdata.push(ele.fullname);
                        ybdata.push(ele.ybcount);
                        jyzdata.push(ele.jyzcount);
                        yzdata.push(ele.zdcount);
                        zgldata.push(ele.zgl);
                    }
                });
                var option = {
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'shadow',
                            shadowStyle: {
                                color: 'rgba(255,255,255,.1)'
                            }
                        },
                        formatter: function (params) {
                            var relVal = params[0].name;
                            for (var i = 0, l = params.length; i < l; i++) {
                                i === 3 ? relVal += '<br/>' + params[i].marker + params[i].seriesName + "：" + params[i].value + '%' : relVal += '<br/>' + params[i].marker + params[i].seriesName + "：" + params[i].value
                            }
                            return relVal;
                        }
                    },
                    legend: {
                        data: ['一般违章', '较严重违章', '严重违章', '整改率'],
                        itemGap: 30,
                        left: 'center',
                        textStyle: {
                            fontSize: 12,
                            color: '#ccc'
                        }
                    },
                    calculable: true,
                    grid: {
                        containLabel: true,
                        x: 30,
                        x2: 20,
                    },
                    xAxis: [
                        {
                            type: 'category',
                            axisLabel: {
                                textStyle: {
                                    color: '#ccc',
                                    fontSize: 12
                                },
                                interval: 0,
                                formatter: function (params) {
                                    var newParamsName = '';
                                    var paramsNameNumber = params.length;
                                    var providerNumer = 4;
                                    var rowNumber = Math.ceil(paramsNameNumber / providerNumer)
                                    if (paramsNameNumber > providerNumer) {
                                        for (var p = 0; p < rowNumber; p++) {
                                            var tempStr = '';
                                            var start = p * providerNumer;
                                            var end = start + providerNumer;
                                            if (p == rowNumber - 1) {
                                                tempStr = params.substring(start, paramsNameNumber)
                                            } else {
                                                tempStr = params.substring(start, end) + '\n'
                                            }
                                            newParamsName += tempStr
                                        }
                                    } else {
                                        newParamsName = params;
                                    }

                                    return newParamsName

                                }
                            },
                            axisLine: {
                                lineStyle: {
                                    color: "rgba(85,113,255,.3)"
                                }
                            },
                            data: deptdata

                        },
                        {
                            type: 'category',
                            axisLine: { show: false },
                            axisTick: { show: false },
                            axisLabel: {
                                show: false
                            },
                            splitArea: { show: false },
                            splitLine: { show: false, },
                            data: deptdata
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value',
                            name: '违章数量',
                            min: 0,
                            minInterval:1,
                            nameTextStyle: {
                                color: 'rgba(255,255,255,.6)',
                                fontSize: 16
                            },
                            nameGap: 30,
                            axisLabel: {
                                formatter: '{value}',
                                textStyle: {
                                    color: 'rgba(255,255,255,.6)'
                                }
                            },
                            axisLine: {
                                lineStyle: {
                                    color: "rgba(85,113,255,.3)"
                                }
                            },
                            splitLine: {
                                lineStyle: {
                                    color: "rgba(85,113,255,.3)"
                                }
                            }
                        },
                        {
                            type: 'value',
                            name: '整改率',
                            nameTextStyle: {
                                color: 'rgba(255,255,255,.6)',
                                fontSize: 16
                            },
                            min: 0,
                            max:100,
                            nameGap: 30,
                            axisLabel: {
                                formatter: '{value} %',
                                textStyle: {
                                    color: 'rgba(255,255,255,.6)'
                                }
                            },
                            axisLine: {
                                lineStyle: {
                                    color: "rgba(85,113,255,.3)"
                                }
                            },
                            splitLine: {
                                lineStyle: {
                                    color: "rgba(85,113,255,.3)"
                                }
                            }
                        }
                    ],
                    series: [
                        {
                            name: '一般违章',
                            type: 'bar',
                            barWidth: '15%',
                            itemStyle: {
                                normal: {
                                    color: '#346fd6',
                                    label: {
                                        show: true,
                                        position: 'top',
                                        textStyle: {
                                            color: '#ccc'
                                        }
                                    }
                                }
                            },
                            data: ybdata
                        },
                        {
                            name: '较严重违章',
                            type: 'bar',
                            // xAxisIndex:1,
                            barWidth: '15%',
                            itemStyle: {
                                normal: {
                                    color: '#cab335',
                                    label: {
                                        show: true,
                                        position: 'top',
                                        textStyle: {
                                            color: '#ccc'
                                        }
                                    }
                                }
                            },
                            barGap: 0,
                            data: jyzdata
                        },
                        {
                            name: '严重违章',
                            type: 'bar',
                            // xAxisIndex:1,
                            barWidth: '15%',
                            itemStyle: {
                                normal: {
                                    color: '#d42129',
                                    label: {
                                        show: true,
                                        position: 'top',
                                        textStyle: {
                                            color: '#ccc'
                                        }
                                    }
                                }
                            },
                            data: yzdata
                        },
                        {
                            name: '整改率',
                            type: 'line',
                            max: 100,
                            yAxisIndex: 1,
                            symbol: 'circle',
                            symbolSize: 6,
                            itemStyle: {
                                normal: {
                                    color: '#00b286'
                                }
                            },
                            data: zgldata
                        }
                    ]
                };

                var chart = echarts.init(document.querySelector('.charts4'));
                chart.setOption(option);

            }
        }
    });
}
//隐患月度趋势统计分析
function GetMonthHTStat() {
    $.ajax({
        dataType: "json",
        type: "get",
        url: "../../desktop/GetMonthHTStat",
        success: function (json) {
            if (json.type == 1) {
                var obj = document.querySelector('#charts5')
                var option5 = {
                    title: {
                    },
                    tooltip: {
                        trigger: 'axis',
                        formatter: function (params) {
                            var relVal = params[0].name;
                            for (var i = 0, l = params.length; i < l; i++) {
                                i === 0 || i === 4 ? relVal += '<br/>' + params[i].marker + params[i].seriesName + "：" + params[i].value + '%' : relVal += '<br/>' + params[i].marker + params[i].seriesName + "：" + params[i].value
                            }
                            return relVal;
                        }
                    },
                    legend: {
                        data: ['隐患整改率', '安全检查次数', '发现隐患数', '发现违章数', '违章整改率'],
                        textStyle: {
                            color: '#ffffff',
                            fontSize: 14
                        },
                        itemGap: 40
                    },
                    grid: {
                        containLabel: true,
                        x: 0,
                        x2: 40,
                        bottom: 10
                    },
                    xAxis: {
                        type: 'category',
                        boundaryGap: false,
                        axisLine: {
                            lineStyle: {
                                color: '#242668'
                            }
                        },
                        splitLine: {
                            show: true,
                            lineStyle: {
                                color: '#242668'
                            }
                        },
                        axisLabel: {
                            textStyle: {
                                color: 'rgba(255,255,255,.6)',
                                fontSize: 14
                            }
                        },
                        data: json.resultdata.months
                    },
                    yAxis: [
                        {
                            axisLine: {
                                lineStyle: {
                                    color: '#242668'
                                }
                            },
                            splitLine: {
                                show: false,
                                lineStyle: {
                                    color: '#242668'
                                }
                            },
                            axisLabel: {
                                textStyle: {
                                    color: 'rgba(255,255,255,.6)',
                                    fontSize: 14
                                }
                            },
                            type: 'value',
                            min: 0,
                            minInterval: 1
                        },
                        {
                            type: 'value',
                            name: '整改率',
                            max: 100,
                            nameGap: 30,
                            nameTextStyle: {
                                color: 'rgba(255,255,255,.6)',
                                fontSize: 16
                            },
                            axisLine: {
                                lineStyle: {
                                    color: '#242668'
                                }
                            },
                            splitLine: {
                                lineStyle: {
                                    color: '#242668'
                                }
                            },
                            axisLabel: {
                                textStyle: {
                                    color: 'rgba(255,255,255,.6)',
                                    fontSize: 14
                                },
                                formatter: '{value}%'
                            }
                        }
                    ],
                    series: [
                        {
                            name: '隐患整改率',
                            type: 'line',
                            yAxisIndex: 1,
                            // stack: '总量',
                            data: json.resultdata.yhzgl,
                            color: '#3879e7'
                        },
                        {
                            name: '安全检查次数',
                            type: 'line',
                            yAxisIndex: 0,
                            // stack: '总量',
                            data: json.resultdata.jc,
                            color: '#eaa309'
                        },
                        {
                            name: '发现隐患数',
                            type: 'line',
                            yAxisIndex: 0,
                            // stack: '总量',
                            data: json.resultdata.yhfx,
                            color: '#d42129'
                        },
                        {
                            name: '发现违章数',
                            type: 'line',
                            yAxisIndex: 0,
                            // stack: '总量',
                            data: json.resultdata.wzfx,
                            color: '#5de7b7'
                        },
                        {
                            name: '违章整改率',
                            type: 'line',
                            yAxisIndex: 1,
                            // stack: '总量',
                            data: json.resultdata.wzzgl,
                            color: '#f0ff00'
                        },
                    ]
                };
                var Chart5 = echarts.init(obj);
                Chart5.setOption(option5);
            }
        }
    });
}
