//var data7 = [{
//	name: '一级风险',
//	value: 5
//}, {
//	name: '二级风险',
//	value: 20
//}, {
//	name: '三级风险',
//	value: 45
//}]
//var color = ['#d42129', '#eaa309', '#0060e1']
//var option = {
//	legend: {
//		selectedMode: true,
//		top: 40,
//		left:100,
//		orient: 'vertical',
//		x:'left',
//		itemGap:20,
//		textStyle: {
//			fontSize: 14,
//			color: '#ffffff',
//			textAlign: 'left'

//		},
//		data: ['一级风险', '二级风险', '三级风险'],

//	},

//	series: [{
//		type: 'pie',
//		hoverAnimation: true,
//		color: color,
//		label: {
//			normal: {
//				show: true,
//				position: 'absolute',

//				color: '#ffffff'
//			}
//		},
//		labelLine: {
//			normal: {
//				show: true,
//				length: 30,
//				length2: 15,
//				lineStyle: {

//				}
//			}
//		},
//		data: data7
//	}]
//}
//var chart = echarts.init(document.getElementById('chart7'));
//chart.setOption(option);




//初始化加载
$(function () {
    OutsourcingForLeaderCockpit();
});

//外包工程统计
function OutsourcingForLeaderCockpit() {

    //外包工程类型统计
    var data = [];
    $.post("../Desktop/GetProjectChart3", {}, function (riskVal) {
        json = eval("(" + riskVal + ")");

        var colors = ['#fdb45c', '#f7464a', '#46bfbd', '#008EF0', '#57A64A'];
        if (!!json.resultdata) {
            var xdata = new Array();
            var resultdata = JSON.parse(json.resultdata);
            for (var i = 0; i < resultdata.xValues.length; i++) {
                var text = resultdata.xValues[i];
                var value = resultdata.yValues[i];
                data.push({ name: text, value: value, itemStyle: { color: colors[i] } });
                xdata.push(text);
            };

            var option = {
                title: {
                    text: '外包工程风险占比统计',
                    textStyle: {
                        color: '#fff',
                        fontSize: 14,
                    },
                    x: 'center'
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                },
                legend: {
                    orient: 'vertical',
                    left: 'left',
                    data: xdata,
                    textStyle: {
                        color: '#fff'
                    }
                },
                series: [
                    {
                        name: '外包工程风险占比',
                        type: 'pie',
                        radius: '55%',
                        center: ['50%', '60%'],
                        data: data,
                        itemStyle: {
                            emphasis: {
                                shadowBlur: 10,
                                shadowOffsetX: 0,
                                shadowColor: '#fff'
                            }
                        }
                    }
                ]
            };
            var Chart = echarts.init(document.getElementById('chart7'));
            //var Chart = echarts.init(obj)
            Chart.setOption(option, true);
            //外包工程
            Chart.on('click', function (params) {
                top.tablist.newTab({ id: 'c593e2a1-f391-4c59-ba41-d3b82f16536c', url: '../OutsourcingProject/Outsouringengineer/Index?pMode=0&englevelname=' + params.name, title: params.name });
            });
        } else {
            $(obj).html("<p style='color:white;'>暂无数据显示</p>");
        }

    });
}
