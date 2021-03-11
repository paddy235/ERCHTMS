




$(function () {

    getHTChangeStat();
    SafeNorm();

});


//安全指标
function SafeNorm() {
    //实时监控数据统计
    $.post("../Desktop/PowerPlantSafetyHomePage", { itemType: "SSJK", mode: 0 }, function (data) {
        if (data.length > 2) {
            var count = 0;
            var wbHtml = "";
            var json = eval("(" + data + ")");
            //指标
            $(json.resultdata).each(function (index, item) {
                if (!!item.TypeCode) {
                    //var moduleid = "#" + item.TypeCode;
                    //外包工程管理
                    if (item.TypeCode == "WBWFMG-01") {
                        //itemflowManager(item);
                    }
                    switch (item.TypeCode) {
                        case "CJLDJSC-002"://安全风险管理
                            if (item.itemname == "重大风险数") {
                                $(".SafeNormRisk1").html(item.Num);
                            }
                            else if (item.itemname == "较大风险数量") {
                                $(".SafeNormRisk2").html(item.Num);
                            }
                            else {
                                $(".SafeNormRisk3").html(item.Num);
                            }
                            break;
                        case "CJLDJSC-003"://隐患排查治理
                            if (item.itemname == "安全检查次数") {
                                $(".SafeNormHidden1").html(item.Num);
                            }
                            else if (item.itemname == "一般隐患数量") {
                                $(".SafeNormHidden2").html(item.Num);
                            }
                            else if (item.itemname == "重大隐患数") {
                                $(".SafeNormHidden3").html(item.Num);
                            }
                            else if (item.itemname == "未整改隐患数") {
                                $(".SafeNormHidden4").html(item.Num);
                            }
                            else if (item.itemname == "一般隐患整改率") {
                                $(".SafeNormHidden5").html(item.Num + "%");
                            }
                            else {
                                $(".SafeNormHidden6").html(item.Num + "%");
                            }
                            break;
                        case "CJLDJSC-004"://反违章管理
                            if (item.itemname == "违章数量") {
                                $(".SafeNormBreak1").html(item.Num);
                            }
                            else if (item.itemname == "未整改违章数") {
                                $(".SafeNormBreak2").html(item.Num);
                            }
                            else {
                                $(".SafeNormBreak3").html(item.Num + "%");
                            }
                            break;
                        case "CJLDJSC-005"://外包管控
                            if (item.itemname == "外包工程总数") {
                                $(".SafeNormDanger1").html(item.Num);
                            }
                            else if (item.itemname == "在场外包单位数") {
                                $(".SafeNormDanger2").html(item.Num);
                            }
                            else if (item.itemname == "外包人员违章数") {
                                $(".SafeNormDanger3").html(item.Num);
                            }
                            else if (item.itemname == "本月新进外包人数") {
                                $(".SafeNormDanger4").html(item.Num);
                            }
                            else if (item.itemname == "在建外包工程数") {
                                $(".SafeNormDanger5").html(item.Num);
                            }
                            else if (item.itemname == "外包工程在厂人数") {
                                $(".SafeNormDanger6").html(item.Num);
                            }
                            break;
                        case "CJLDJSC-006":
                            if (item.itemname == "重大危险源数") {
                                $(".SafeNormRisk4").html(item.Num);
                            }
                            break;
                        case "CJLDJSC-007"://特种设备及人员管理
                            if (item.itemname == "全厂特种设备信息") {
                                $(".SafeNormEqumet1").html(item.Num);
                            }
                            else if (item.itemname == "特种作业人员") {
                                $(".SafeNormEqumet2").html(item.Num);
                            }
                            else {
                                $(".SafeNormEqumet3").html(item.Num);
                            }
                            break;
                        case ""://教育培训

                            break;
                        case "CJLDJSC-009"://事故事件
                            if (item.itemname == "未遂事件起数") {
                                $(".safeMalfunction1").html(item.Num);
                            }
                            else if (item.itemname == "事故事件起数") {
                                $(".safeMalfunction2").html(item.Num);
                            }
                            else {
                                $(".safeMalfunction3").html(item.Num);
                            }
                            break;
                        default:
                    }
                }
            });
        }
    });
}

//隐患整改情况统计
function getHTChangeStat() {
 
    $.ajax({
        dataType: "json",
        type: "post",
        url: "../Desktop/GetHiddenChangeForLeaderCockpit",
        success: function (json) {
            if (json.type == 1) {
                var deptdata = new Array();
                var ybdata = new Array();
                var zddata = new Array();
                var zgldata = new Array();
                var data = $.parseJSON(json.resultdata);
                $(data).each(function (index, ele) {
                    //if (index < 9) {
                        if (!!ele) {
                            deptdata.push(ele.fullname);
                            ybdata.push(ele.ybcount);
                            zddata.push(ele.zdcount);
                            zgldata.push(ele.zgl);
                        }
                    //}

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
                        formatter: function (params) {
                            console.log(params)

                            var relVal = params[0].name;
                            for (var i = 0, l = params.length; i < l; i++) {
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
                        top: '18%',
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
                                rotate:30,
                                interval: 0,
                                //formatter: function (params) {
                                //    var newParamsName = '';
                                //    var paramsNameNumber = params.length;
                                //    var providerNumer = 8;
                                //    var rowNumber = Math.ceil(paramsNameNumber / providerNumer)
                                //    if (paramsNameNumber > providerNumer) {
                                //        for (var p = 0; p < rowNumber; p++) {
                                //            var tempStr = '';
                                //            var start = p * providerNumer;
                                //            var end = start + providerNumer;
                                //            if (p == rowNumber - 1) {
                                //                tempStr = params.substring(start, paramsNameNumber)
                                //            } else {
                                //                tempStr = params.substring(start, end) + '\n'
                                //            }
                                //            newParamsName += tempStr
                                //        }
                                //    } else {
                                //        newParamsName = params;
                                //    }

                                //    return newParamsName

                                //},
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
                                    var providerNumer = 8;
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
                            min: 0, max: 100,
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
                    ],
                    dataZoom: [
                        {                            
                            show: true,
                            backgroundColor:"rgba(47,69,84,0)",
                            type: 'inside',
                            fillerColor: "rgba(167,183,204,0.4)",
                            borderColor:"#ddd",
                            filterMode:'filter',
                            start: 0,
                            end: 5,
                            xAxisIndex: 0
                        }
                    ]
                }
                var chart = echarts.init(document.getElementById('chart2'));
                chart.setOption(option);
            }
        }
    });

};






















