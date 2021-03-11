var chart3
(function () {
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
            data: ['一般违章', '重大隐患'],
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
                        color: "rgba(241,245,248,.2)"
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
                        color: "rgba(241,245,248,.2)"
                    }
                },
                splitLine: {
                    lineStyle: {
                        color: "rgba(241,245,248,.2)"
                    }
                }
            }
        ],
        series: [
            {
                name: '一般违章',
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
                data: [96, 88, 95, 99, 100, 95, 100]
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
                            show: true,
                            position: 'top',
                            textStyle: {
                                color: '#ccc'
                            }
                        }
                    }
                },
                data: [40, 0, 0, 75, 0, 75, 90]
            }
        ]

    }
    chart3 = echarts.init(document.getElementById('chart3'));
    chart3.setOption(option);
})()