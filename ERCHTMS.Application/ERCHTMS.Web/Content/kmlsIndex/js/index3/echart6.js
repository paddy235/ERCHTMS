


//初始化加载
$(function () {
    HighRiskWorking();

})


//今日作业风险 //高风险作业统计
function HighRiskWorking() {
    $.post("../Desktop/GetHighRiskWorkingForLeaderCockpit", {}, function (data) {
        var json;
        left = true;
        json = eval("(" + data + ")");
        var resultdata = JSON.parse(json.resultdata);
        //今日作业风险
        //if (!!resultdata.dtdata) {
        //    var trText = "";
        //    var array = resultdata.dtdata;
        //    var curIndex = 0;
        //    $(array).each(function (index, ele) {
        //        curIndex = index + 1;
        //        var risktypename = !!ele.risktypename ? ele.risktypename : "";
        //        trText += "<tr><td>" + curIndex + "</td><td>" + ele.itemname + "</td><td>" + ele.workareaname + "</td><td>" + risktypename + "</td><td>" + ele.workdeptname + "</td></tr>";
        //    });
        //    $("#workingTable").empty().append(trText);
        //}

        //高风险作业统计
        if (!!resultdata.tjdata) {
            var tjdata = resultdata.tjdata;
            var colors = ["#54b1f9", '#40e0d0', '#ffa500', '#cd5c5c', '#ba55d3', '#ff69b4', '#6495ed', '#32cd32', '#00A7FF', '#D7AC6A', '#88D184', '#B119E1', '#FF4300'];
            var seriesdata = new Array();
            var legenddata = new Array();
            $(tjdata).each(function (index, ele) {
                seriesdata.push({ value: ele.nums, name: ele.itemname, itemStyle: { color: colors[index] } });
                legenddata.push(ele.itemname);
            });
            //var obj = document.getElementById('highriskwork');
            //$(obj).height("450");
            var option = {
                title: {
                    // text: '南丁格尔玫瑰图',
                    // subtext: '纯属虚构',
                    x: 'center',
                    top: 100
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                },
                legend: {
                    x: 'left',
                    y: 'bottom',
                    data: legenddata,
                    textStyle: {
                        color: "#fff"
                    }
                },
                calculable: true,
                series: [
                    {
                        name: '高风险作业统计',
                        type: 'pie',
                        radius: [50, 90],
                        center: ['50%', '38%'],
                        roseType: 'area',
                        labelLine: {
                            length: 0,
                            length2: 0
                        },
                        data: seriesdata
                    }
                ]
            };
            var myChart = echarts.init(document.querySelector('.charts3'));
            myChart.setOption(option, true);

        }
    });


}