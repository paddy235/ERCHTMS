var chart2
(function () {
    var data6 = [10, 20, 30, 40, 50, 60, 70, 80, 90, 100]
    var option = {
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow',
                shadowStyle: {
                    color: 'rgba(255,255,255,.1)'
                }
            }

        },
        legend: {
            data: ['一般违章', '较严重违章', '严重违章'],
            itemGap: 30,
            textStyle: {
                fontSize: 12,
                color: '#ccc'
            },
            top: 30
        },
        toolbox: {
        },
        calculable: true,
        grid: {
            containLabel: false,
            top: 80,
            left: 40,
            right: 0,
            bottom: 50
        },
        xAxis: [
            {
                type: 'category',
                axisLabel: {
                    textStyle: {
                        color: '#ccc',
                        fontSize: 12
                    }
                },
                axisLine: {
                    lineStyle: {
                        color: "rgba(153,153,153,.3)"
                    }
                },
                data: ['外包单位', '生技部', '安监部', '燃料部', '质检部', '发电运行部', '设备维修部']
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
                data: ['外包单位', '生技部', '安监部', '燃料部', '质检部', '发电运行部', '设备维修部']
            }
        ],
        yAxis: [
            {
                type: 'value',
                axisLabel: {
                    formatter: '{value} %',
                    textStyle: {
                        color: '#c7e6fa'
                    }
                },
                axisLine: {
                    lineStyle: {
                        color: "rgba(153,153,153,.3)"
                    }
                },
                splitLine: {
                    lineStyle: {
                        color: "rgba(153,153,153,.3)"
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
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [

                                { offset: 0, color: '#3879e7' },
                                { offset: 0.5, color: '#6194ec' },
                                { offset: 1, color: '#afc9f5' }
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
                data: [96, 88, 95, 99, 100, 95, 100]
            },
            {
                name: '较严重违章',
                type: 'bar',
                // xAxisIndex:1,
                barWidth: '15%',
                itemStyle: {
                    normal: {
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [

                                { offset: 0, color: '#e7a20d' },
                                { offset: 0.5, color: '#dbb662' },
                                { offset: 1, color: '#efd397' },
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
                data: [40, 15, 0, 75, 0, 75, 90]
            },
            {
                name: '严重违章',
                type: 'bar',
                // xAxisIndex:1,
                barWidth: '15%',
                itemStyle: {
                    normal: {
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [

                                { offset: 0, color: '#cf2531' },
                                { offset: 0.5, color: '#d06b7b' },
                                { offset: 1, color: '#eda4a8' },
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
                data: [75, 0, 75, 90, 40, 0, 0]
            }
        ]
    };
    let obj = document.getElementById('chart2')
    chart2 = echarts.init(obj);
    chart2.setOption(option);
})()