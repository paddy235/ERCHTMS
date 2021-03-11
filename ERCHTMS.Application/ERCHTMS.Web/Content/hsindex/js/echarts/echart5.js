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
                                i === 0 || i === 4 ? relVal += '<br/>' + params[i].marker + params[i].seriesName + params[i].value + '%' : relVal += '<br/>' + params[i].marker + params[i].seriesName + params[i].value
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
                        data: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
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
                            type: 'value'
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
                            yAxisIndex: 0,
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
                            yAxisIndex: 1,
                            // stack: '总量',
                            data: json.resultdata.yhfx,
                            color: '#d42129'
                        },
                        {
                            name: '发现违章数',
                            type: 'line',
                            yAxisIndex: 1,
                            // stack: '总量',
                            data: json.resultdata.wzfx,
                            color: '#5de7b7'
                        },
                        {
                            name: '违章整改率',
                            type: 'line',
                            yAxisIndex: 0,
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