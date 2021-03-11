




//初始化加载
$(function () {
    LoadContainer();
    loadbreakYear();
});

//获取隐患月度趋势
function LoadContainer() {
    var queryJson = { deptCode: "", year: "", hidPoint: "", hidRank: "请选择" };

    $.ajax({
        type: "get",
        url: "../HiddenTroubleManage/HTStatistics/QueryChangeSituationTendencyKm",
        data: { queryJson: JSON.stringify(queryJson) },
        success: function (data) {
            if (!!data) {
                var nData = eval("(" + data + ")");
                //加载图表

                var obj = document.getElementById('chart4');
                var option = {
                    title: {
                    },
                    tooltip: {
                        trigger: 'axis',
                        formatter: function (params) {
                            var relVal = params[0].name;
                            for (var i = 0, l = params.length; i < l; i++) {
                                i === 0 ? relVal += '<br/>' + params[i].marker + params[i].seriesName + params[i].value + '%' : relVal += '<br/>' + params[i].marker + params[i].seriesName + params[i].value
                            }
                            return relVal;
                        }
                    },
                    legend: {
                        data: ['隐患整改率', '安全检查次数', '发现隐患数', '未闭环隐患数'],
                        textStyle: {
                            color: '#ffffff',
                            fontSize: 14
                        },
                        itemGap: 50
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
                            nameTextStyle: {
                                color: 'rgba(255,255,255,.6)'
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
                            data: nData[0].data,
                            color: '#3879e7'
                        },
                        {
                            name: '安全检查次数',
                            type: 'line',
                            yAxisIndex: 0,
                            // stack: '总量',
                            data: nData[1].data,
                            color: '#eaa309'
                        },
                        {
                            name: '发现隐患数',
                            type: 'line',
                            yAxisIndex: 0,
                            // stack: '总量',
                            data: nData[2].data,
                            color: '#d42129'
                        },
                        {
                            name: '未闭环隐患数',
                            type: 'line',
                            yAxisIndex: 0,
                            // stack: '总量',
                            data: nData[3].data,
                            color: '#5ae6b6'
                        }
                    ]
                };

                var Chart6 = echarts.init(obj);
                Chart6.setOption(option);

            }
        }
    });
}


//获取违章月度趋势
function loadbreakYear() {
    var queryJson = {
        deptCode: '',
        year: '',
        levelGroups: "一般违章,较严重违章,严重违章",
        DepartmentName: ""
    };
    $.ajax({
        type: "post",
        url: '../LllegalManage/LllegalStatistics/QueryLllegalNumberLineKm',
        data: { queryJson: JSON.stringify(queryJson) },
        success: function (data) {
            if (!!data) {
                var nData = eval("(" + data + ")");
                //加载图表
                var option = {
                    title: {
                    },
                    tooltip: {
                        trigger: 'axis',
                        formatter: function (params) {
                            var relVal = params[0].name;
                            for (var i = 0, l = params.length; i < l; i++) {
                                i === 2 ? relVal += '<br/>' + params[i].marker + params[i].seriesName + params[i].value + '%' : relVal += '<br/>' + params[i].marker + params[i].seriesName + params[i].value
                            }
                            return relVal;
                        }
                    },
                    legend: {
                        data: ['发现违章数', '未闭环违章数', '违章整改率'],
                        textStyle: {
                            color: '#ffffff',
                            fontSize: 14
                        },
                        itemGap: 50
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
                            nameTextStyle: {
                                color: 'rgba(255,255,255,.6)'
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
                            name: '发现违章数',
                            type: 'line',
                            yAxisIndex: 0,
                            // stack: '总量',
                            data: nData[0].data,
                            color: '#3879e7'
                        },
                        {
                            name: '未闭环违章数',
                            type: 'line',
                            yAxisIndex: 0,
                            // stack: '总量',
                            data: nData[1].data,
                            color: '#eaa309'
                        },
                        {
                            name: '违章整改率',
                            type: 'line',
                            yAxisIndex: 1,
                            // stack: '总量',
                            data: nData[2].data,
                            color: '#d42129'
                        }
                    ]
                };
                var obj = document.getElementById('chart1');
                var Chart5 = echarts.init(obj);
                Chart5.setOption(option);
            }
        }
    });
}

