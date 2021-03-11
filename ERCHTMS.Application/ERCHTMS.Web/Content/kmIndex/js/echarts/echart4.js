

//初始化加载
$(function () {
    LllegalChangeForLeaderCockpit();
    NoCloseLoopLllegalStatistics();
});


//各部门违章整改率统计
function LllegalChangeForLeaderCockpit() {
    var json;
    $.post("../Desktop/GetLllegalChangeForLeaderCockpit", {}, function (data) {
        json = eval("(" + data + ")");
        var resultdata = JSON.parse(json.resultdata);

        if (!!resultdata) {
            var deptdata = new Array();
            var ybdata = new Array();
            var jyzdata = new Array();
            var yzdata = new Array();
            $(resultdata).each(function (index, ele) {
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
                    }
                },
                legend: {
                    data: ['一般违章', '较严重违章', '严重违章'],
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
                    containLabel: true,
                    x: 30,
                    x2: 20,
                },
                xAxis: [
                    {
                        type: 'category',
                        axisLabel: {
                             interval: 0,
                            rotate: 18,
                            textStyle: {
                                color: '#ccc',
                                fontSize: 12
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
                        axisLabel: {
                            formatter: '{value} %',
                            textStyle: {
                                color: '#c7e6fa'
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
                        data: ybdata
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
                        data: jyzdata
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
                        data: yzdata
                    }
                ]
            };
            var chart = echarts.init(document.querySelector('.charts4'));
            chart.setOption(option);
        }
    });
}

//各部门未闭环违章统计
function NoCloseLoopLllegalStatistics() {
    $.post("../Desktop/GetNoCloseLoopLllegalStatistics", {}, function (data) {
        var json = "";
        var sHtml1 = "";
        var sHtml2 = "";
        var sHtml3 = "";
        $("#llegalTable").find(".row2").remove();
        right = true;
        json = eval("(" + data + ")");
        var resultdata = JSON.parse(json.resultdata);
        $("#llegalTable").append("");
        if (!!resultdata) {
            var array = resultdata;
            var curIndex = 0;
            $(array).each(function (index, ele) {
                curIndex = index + 1;

                if (ele.s1 != "0(0%)") {
                    sHtml1 = "<a style=\"color:red;\" onclick=\"viewLllegal('" + ele.code + "','一般违章')\">" + ele.s1 + "</a>";
                }
                else {
                    sHtml1 = ele.s1;
                }

                if (ele.s2 != "0(0%)") {
                    sHtml2 = "<a style=\"color:red;\" onclick=\"viewLllegal('" + ele.code + "','较严重违章')\">" + ele.s2 + "</a>";
                }
                else {
                    sHtml2 = ele.s2;
                }

                if (ele.s3 != "0(0%)") {
                    sHtml3 = "<a style=\"color:red;\" onclick=\"viewLllegal('" + ele.code + "','严重违章')\">" + ele.s3 + "</a>";
                }
                else {
                    sHtml3 = ele.s3;
                }

                var trText = "<tr class='row1'><td>" + curIndex + "</td><td>" + ele.name + "</td><td>" + sHtml1 + "</td><td>" + sHtml2 + "</td><td>" + sHtml3 + "</td></tr>";
                $("#llegalTable").append(trText);
                //$("#llegalTable").css({ "overflow": "auto", "outline": "none" });

                $('#llegalTable').niceScroll({
                    autohidemode: false,
                    cursorcolor: "#68ceed",
                    cursorborderradius: "0px",
                    cursorwidth: "5px",
                    background: "#204758",
                    cursorborder: "none",
                });
            });
        }
    });
}

///查看违章
function viewLllegal(code, levelname) {
    var url = "";
    var title = "未闭环" + levelname + "信息";
    url = "../LllegalManage/LllegalRegister/SdIndex?action=NotClose&lllegallevelname=" + levelname + "&qdeptcode=" + code;
    top.openTab("109d0e86-d8d8-4794-a15e-7308d1646344", url, title);
}