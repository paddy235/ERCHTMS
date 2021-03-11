



//初始化加载
$(function () {

    NoCloseLoopHidStatistics();
    HiddenChangeForLeaderCockpit();
});

//未闭环隐患统计
function NoCloseLoopHidStatistics() {
    $.post("../Desktop/GetNoCloseLoopHidStatistics", {}, function (data) {
        var json = "";
        var sHtml1 = "";
        var sHtml2 = "";
        json = eval("(" + data + ")");
        var resultdata = JSON.parse(json.resultdata);
        //按责任单位统计
        if (!!resultdata.unitdata) {
            var array = resultdata.unitdata;
            var curIndex = 0;
            var ptrText1 = "";
            $("#unitTable").find(".row2").remove();//去掉加载图片
            $(array).each(function (index, ele) {
                curIndex = index + 1;
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
                ptrText1 += "<tr class='row1'><td>" + curIndex + "</td><td>" + ele.name + "</td><td>" + sHtml1 + "</td><td>" + sHtml2 + "</td></tr>";
                // $("#unitTable").append(trText);
            });
            $("#unitTable").empty().append(ptrText1);
            $('.felllist2').niceScroll({
                autohidemode: false,
                cursorcolor: "#68ceed",
                cursorborderradius: "0px",
                cursorwidth: "5px",
                background: "#204758",
                cursorborder: "none",
            });
        }

        //按区域统计
        if (!!resultdata.areadata) {
            var array = resultdata.areadata;
            var curIndex = 0;
            var ptrText2 = "";
            $(array).each(function (index, ele) {
                curIndex = index + 1;
                if (ele.s1 != "0(0%)") {
                    sHtml1 = "<a style=\"color:red;\" onclick=\"viewHid(1,'" + ele.code + "','一般隐患')\">" + ele.s1 + "</a>";
                }
                else {
                    sHtml1 = ele.s1;
                }

                if (ele.s2 != "0(0%)") {
                    sHtml2 = "<a style=\"color:red;\" onclick=\"viewHid(1,'" + ele.code + "','重大隐患')\">" + ele.s2 + "</a>";
                }
                else {
                    sHtml2 = ele.s2;
                }
                ptrText2 += "<tr><td class='row1'>" + curIndex + "</td><td>" + ele.name + "</td><td>" + sHtml1 + "</td><td>" + sHtml2 + "</td></tr>";
            });
            $("#areaTable").empty().append(ptrText2);
        }
        //按专业统计
        if (!!resultdata.majordata) {
            var array = resultdata.majordata;
            var curIndex = 0;
            $("#majorTable").append("");
            var ptrText3 = "";
            $(array).each(function (index, ele) {
                curIndex = index + 1;
                if (ele.s1 != "0(0%)") {
                    sHtml1 = "<a style=\"color:red;\" onclick=\"viewHid(2,'" + ele.id + "','一般隐患')\">" + ele.s1 + "</a>";
                }
                else {
                    sHtml1 = ele.s1;
                }

                if (ele.s2 != "0(0%)") {
                    sHtml2 = "<a style=\"color:red;\" onclick=\"viewHid(2,'" + ele.id + "','重大隐患')\">" + ele.s2 + "</a>";
                }
                else {
                    sHtml2 = ele.s2;
                }
                ptrText3 += "<tr class='row1'><td>" + curIndex + "</td><td>" + ele.name + "</td><td>" + sHtml1 + "</td><td>" + sHtml2 + "</td></tr>";

            });
            $("#majorTable").empty().append(ptrText3);
        }
    });

}

///查看隐患
function viewHid(mode, code, rankname) {
    var url = "";
    var title = "未闭环" + rankname + "信息";
    //按责任单位
    if (mode == 0) {
        url = "../HiddenTroubleManage/HTBaseInfo/Index?ChangeStatus=未整改结束&mode=wbhtjcx&qrankname=" + rankname + "&qdeptcode=" + code;
    }
    else if (mode == 1)  //按区域
    {
        url = "../HiddenTroubleManage/HTBaseInfo/Index?ChangeStatus=未整改结束&mode=wbhtjcx&qrankname=" + rankname + "&code=" + code;
    }
    else if (mode == 2)//按专业
    {
        url = "../HiddenTroubleManage/HTBaseInfo/Index?ChangeStatus=未整改结束&mode=wbhtjcx&qrankname=" + rankname + "&majorClassify=" + code;
    }
    top.openTab("67fdd0b7-6629-48dc-a70f-3eb7ba2df1f0", url, title);
}


//违章整改率统计
function HiddenChangeForLeaderCockpit() {
    var json;
    $.post("../Desktop/GetLllegalChangeForLeaderCockpit", {}, function (data) {
        //json = eval("(" + data + ")");
        //var resultdata = JSON.parse(json.resultdata);
        ////按责任单位统计
        //if (!!resultdata) {
        //    var deptdata = new Array();
        //    var ybdata = new Array();
        //    var zddata = new Array();

        //    $(resultdata).each(function (index, ele) {
        //        if (!!ele) {
        //            deptdata.push(ele.fullname);
        //            ybdata.push(ele.ybzgl);
        //            zddata.push(ele.zdzgl);
        //        }
        //    });

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
                            rotate: 30,
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
                                color: '#ff7f50',
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
                        barWidth: '35%',
                        itemStyle: {
                            normal: {
                                color: '#87cefa',
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
                        barWidth: '35%',
                        itemStyle: {
                            normal: {
                                color: '#32d03b',
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
                ],
                 dataZoom: [
                    {
                        show: true,
                        backgroundColor: "rgba(47,69,84,0)",
                        type: 'inside',
                        fillerColor: "rgba(167,183,204,0.4)",
                        borderColor: "#ddd",
                        filterMode: 'filter',
                        start: 0,
                        end: 5,
                        xAxisIndex: 0
                    }
                ]
            }
            var chart = echarts.init(document.getElementById('chart3'));
            chart.setOption(option);

        }
    });
    
    //var json;
    //$.post("../Desktop/GetHiddenChangeForLeaderCockpit", {}, function (data) {
    //    json = eval("(" + data + ")");
    //    var resultdata = JSON.parse(json.resultdata);

    //    if (!!resultdata) {
    //        var deptdata = new Array();
    //        var ybdata = new Array();
    //        var jyzdata = new Array();
    //        var yzdata = new Array();
    //        $(resultdata).each(function (index, ele) {
    //            if (!!ele) {
    //                deptdata.push(ele.fullname);
    //                ybdata.push(ele.ybzgl);
    //                jyzdata.push(ele.jyzzgl);
    //                yzdata.push(ele.yzzgl);
    //            }
    //        });

    //        var obj = document.getElementById('chart3');

    //        var option = {
    //            color: ['#f6b327', '#32d03b', '#333'],
    //            title: {
    //                text: '违章整改率统计',
    //                x: 'center',
    //                textStyle: {
    //                    fontSize: 14,
    //                    color: '#fff'
    //                }
    //            },
    //            legend: {
    //                data: ['一般违章', '较严重违章', '严重违章'],
    //                textStyle: {
    //                    fontSize: 13,
    //                    color: '#fff'
    //                },
    //                top: 30
    //            },
    //            tooltip: {
    //                trigger: 'axis',
    //                formatter: function (params) {
    //                    var res = params[0].name + '的整改率<br/>';
    //                    res += params[0].seriesName + ':' + params[0].value + '%<br/>';
    //                    res += params[1].seriesName + ':' + params[1].value + '%<br/>';
    //                    res += params[2].seriesName + ':' + params[2].value + '%<br/>';
    //                    return res;
    //                }
    //            },
    //            xAxis: [
    //        {
    //            type: 'category',
    //            data: deptdata,
    //            axisLabel: {
    //                interval: 0,
    //                rotate: 18,
    //                textStyle: { color: '#fff' }
    //            },
    //            axisLine: {
    //                lineStyle: {
    //                    color: '#fff'
    //                }
    //            }
    //        }
    //            ],
    //            yAxis: [
    //        {
    //            type: 'value',
    //            name: '整改率占比',
    //            min: 0,
    //            axisLabel: {
    //                formatter: '{value}%',
    //                textStyle: {
    //                    color: '#fff'
    //                }
    //            },
    //            axisLine: {
    //                lineStyle: {
    //                    color: '#fff'
    //                }
    //            }
    //        }
    //            ],
    //            series: [
    //                {
    //                    name: '一般违章',
    //                    type: 'bar',
    //                    barWidth: '35%',
    //                    data: ybdata,
    //                    color: '#ff7f50'
    //                },
    //        {
    //            name: '较严重违章',
    //            type: 'bar',
    //            barWidth: '35%',
    //            data: jyzdata,
    //            color: '#87cefa'
    //        },
    //        {
    //            name: '严重违章',
    //            type: 'bar',
    //            barWidth: '35%',
    //            data: yzdata,
    //            color: '#32d03b'
    //        }
    //            ]
    //        };
    //        var Chart = echarts.init(obj)
    //        Chart.setOption(option);
    //    }
    //});
}
