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
                            ybdata.push(ele.ybzgl);
                            zddata.push(ele.zdzgl);
                            zgldata.push(index);
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
                        // formatter:function (params) {
                        //     console.log(params)

                        //     var relVal = params[0].name;
                        //     for (var i =0, l = params.length; i < l; i++){
                        //         i === 2? relVal += '<br/>' + params[i].marker + params[i].seriesName + params[i].value + '%' : relVal += '<br/>' + params[i].marker + params[i].seriesName + params[i].value
                        //     }
                        //     return relVal;

                        // }
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
                            name: '隐患数量',
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
                            name: '重大隐患',
                            type: 'bar',
                            xAxisIndex: 1,
                            barWidth: '35%',
                            itemStyle: {
                                normal: {
                                    color: '#d89502',
                                    label: {
                                        //    formatter:'{c} %',

                                        show: true,
                                        position: 'top',
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

}

