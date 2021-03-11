




//初始化加载
$(function () {
   
    SafetyStatus();
    SafetyStatusOper();
    SafeNorm();
});

var num1 = "";var num2="";
var nums1 = ""; var nums2 = "";

//电子安全状态
function SafetyStatus() {

    //预警指数统计
    $.get("../home/GetIndexWarnValue", {}, function (data) {
        var json = eval("(" + data + ")");
        var score = 0;
        var scorearry = [];
        var firstnum = 0;
        var secondnum = 0;
        var thirdnum = 0;
        var fourthnum = 0;
        var curindex = 0;
        if (!!json.score) {
            score = json.score;
            curindex = json.index;
            scorearry = json.scorearry;
            if (!!scorearry) {
                firstnum = parseFloat(scorearry[0] / 100).toFixed(1);
                secondnum = parseFloat(scorearry[1] / 100).toFixed(1);
                thirdnum = parseFloat(scorearry[2] / 100).toFixed(1);
                fourthnum = parseFloat(scorearry[3] / 100).toFixed(1);
            }
        }
        switch (curindex) {
            case 0:
                $("#curSafetyStatus").html("目前处于危险状态");
                break;
            case 1:
                $("#curSafetyStatus").html("目前处于较危险状态");
                break;
            case 2:
                $("#curSafetyStatus").html("目前处于较安全状态");
                break;
            case 3:
                $("#curSafetyStatus").html("目前处于安全状态");
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
                x: 210,
                bottom: '4%',
                icon: 'rect',
                data: ['危险', '较危险', '较安全', '安全'],
                textStyle: {
                    color: '#8492af',
                    fontSize: 18
                },
                itemGap: 48,
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
                        interval: 'auto',
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
                    center: ['50%', '60%'], // 默认全局居中
                    radius: '90%',
                    startAngle: 180,
                    endAngle: 0,
                    axisLine: {
                        show: true,
                        lineStyle: { // 属性lineStyle控制线条样式
                            width: 46,
                            //color: [[0.6, '#ff4157'], [0.7, '#fba73a'], [0.9, '#90eaf9'], [1, '#5aeb6']]
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
                        distance: -90
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
                        value: score,
                        label: {
                            textStyle: {
                                fontSize: 12
                            }
                        }
                    }]
                }
            ]
        };

        var colors = ['#f6b327', '#32d03b', '#ccc'];
        var chart = echarts.init(document.querySelector('.charts1'));
        chart.setOption(option);

    });
}


//电子安全状态详情（10个扣分项）
function SafetyStatusOper() {

    $.post("../home/GetWarnData", { startDate: "" }, function (data) {
        if (!!data) {
            var obj = eval("(" + data + ")");
            var resultdata = obj.resultdata;

            var Csum = 0;
            $(resultdata).each(function (index, ele) {
                var serialnum = index + 1;
                var tabhtml = "<tr><td>" + serialnum + "</td><td>" + ele.classificationindex + "</td> <td>" + ele.classificationscore + "</td></tr>";
                Csum = Number(Csum) + Number(ele.classificationscore);
                if (!!tabhtml) {
                    $("#SafetyStatusOper").append(tabhtml);
                }
            });
            var num = Number(Csum);
            $(".CsumScore").html(num.toFixed(1));
            //$(".CsumScore").html(Csum);

        }
    });


}

//安全指标
function SafeNorm() {

    //实时监控数据统计
    $.post("../Desktop/PowerPlantSafetyHomePage", { itemType: "SSJK", mode: 0 }, function (data) {
        alert(1);
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
                        itemflowManager(item);
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

//外包工程流程管理
function itemflowManager(item) {
    switch (item.itemname) {
        case "单位资质审查":
            $(".ItemFlowManager1").html(item.Num);
            if (item.Num != "0") { $(".ItemFlowManager1").attr("style", "color:red"); }
            break;
        case "人员资质审查":
            $(".ItemFlowManager2").html(item.Num);
            break;
        case "安全质量保证金":
            $(".ItemFlowManager3").html(item.Num);
            if (item.Num != "0") { $(".ItemFlowManager3").attr("style", "color:red"); }
            break;
        case "合同协议":
            num1 = item.Num
            break;
        case "合同协议_":
            num2 = item.Num
            break;
        case "入厂许可":
            $(".ItemFlowManager5").html(item.Num);
            if (item.Num != "0") { $(".ItemFlowManager5").attr("style", "color:red"); }
            break;
        case "工器具验收":
            nums1 = item.Num;
            break;
        case "工器具验收_":
            nums2 = item.Num;
            break;
        case "三措两案":
            $(".ItemFlowManager7").html(item.Num);
            if (item.Num != "0") { $(".ItemFlowManager7").attr("style", "color:red"); }
            break;
        case "安全技术交底":
            $(".ItemFlowManager8").html(item.Num);
            if (item.Num != "0") { $(".ItemFlowManager8").attr("style", "color:red"); }
            break;
        case "开工申请":
            $(".ItemFlowManager9").html(item.Num);
            if (item.Num != "0") { $(".ItemFlowManager9").attr("style", "color:red"); }
            break;
        case "开(收)工会":
            $(".ItemFlowManager10").html(item.Num);
            if (item.Num != "0") { $(".ItemFlowManager10").attr("style", "color:red"); }
            break;
        case "安全评价":
            $(".ItemFlowManager11").html(item.Num);
            if (item.Num != "0") { $(".ItemFlowManager11").attr("style", "color:red"); }
            break;
        default:
    }

    var html = ''; var htmls = "";
    if (num1 != "" && num2 != "") {
        html = num1 == "0" ? "<span>0</span>个," : "<span style='color:red;'>" + num1 + "</span>个,";
        html += num2 == "0" ? "<span>0</span>个" : "<span style='color:red;'>" + num2 + "</span>个";
    }
    if (nums1 != "" && nums2 != "") {
        htmls = nums1 == "0" ? "<span>0</span>个," : "<span style='color:red;'>" + nums1 + "</span>个,";
        htmls += nums2 == "0" ? "<span>0</span>个" : "<span style='color:red;'>" + nums2 + "</span>个";
    }
    $(".ItemFlowManager4").html(html);//合同协议
    $(".ItemFlowManager6").html(htmls);//工器具验收
}





