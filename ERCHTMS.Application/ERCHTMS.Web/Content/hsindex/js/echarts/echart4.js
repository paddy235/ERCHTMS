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
                var data = $.parseJSON(json.resultdata);
                $(data).each(function (index, ele) {
                    if (!!ele) {
                        deptdata.push(ele.fullname);
                        ybdata.push(ele.ybzgl);
                        jyzdata.push(ele.jyzzgl);
                        yzdata.push(ele.yzzgl);
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
                                i === 3 ? relVal += '<br/>' + params[i].marker + params[i].seriesName + params[i].value + '%' : relVal += '<br/>' + params[i].marker + params[i].seriesName + params[i].value
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
                            //min: 0,
                            //minInterval:1,
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
                            yAxisIndex: 1,
                            symbol: 'circle',
                            symbolSize: 6,
                            itemStyle: {
                                normal: {
                                    color: '#00b286'
                                }
                            },
                            data: [75, 0, 75, 90, 40]
                        }
                    ]
                };

                var chart = echarts.init(document.querySelector('.charts4'));
                chart.setOption(option);

            }
        }
    });

}