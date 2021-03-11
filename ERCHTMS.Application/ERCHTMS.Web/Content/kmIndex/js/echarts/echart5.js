//var obj5 = document.getElementById('chart5')
//var option5 = {
//    title: {
//    },
//    tooltip: {
//        trigger: 'axis',
//        formatter:function (params) {
//            var relVal = params[0].name;
//            for (var i =0, l = params.length; i < l; i++){
//                i === 2 ? relVal += '<br/>' + params[i].marker + params[i].seriesName + params[i].value + '%' : relVal += '<br/>' + params[i].marker + params[i].seriesName + params[i].value
//            }
//            return relVal;
//        }
//    },
//    legend: {
//        data:['发现违章数','未闭环违章数','未违章整改率'],
//        textStyle:{
//            color:'#ffffff',
//            fontSize:14
//        },
//        itemGap:50
//    },
//    grid: {
//        containLabel: true,
//        x:0,
//        x2:40,
//        bottom:10
//    },
//    xAxis: {
//        type: 'category',
//        boundaryGap: false,
//        axisLine:{
//            lineStyle:{
//                color:'#242668'
//            }
//        },
//        splitLine:{
//            show:true,
//            lineStyle:{
//                color:'#242668'
//            }
//        },
//        axisLabel: {
//            textStyle:{
//                color:'rgba(255,255,255,.6)',
//                fontSize:14
//            }
//        },
//        data: ['1月','2月','3月','4月','5月','6月','7月','8月','9月','10月','11月','12月']
//    },
//    yAxis: [
//        {
//            axisLine:{
//                lineStyle:{
//                    color:'#242668'
//                }
//            },
//            splitLine:{
//                show:false,
//                lineStyle:{
//                    color:'#242668'
//                }
//            },
//            axisLabel: {
//                textStyle:{
//                    color:'rgba(255,255,255,.6)',
//                    fontSize:14
//                }
//            },
//            type: 'value'
//        },
//        {
//            type:'value',
//            name:'整改率',
//            max:100,
//            nameTextStyle:{
//                color:'rgba(255,255,255,.6)'
//            },
//            axisLine:{
//                lineStyle:{
//                    color:'#242668'
//                }
//            },
//            splitLine:{
//                lineStyle:{
//                    color:'#242668'
//                }
//            },
//            axisLabel: {
//                textStyle:{
//                    color:'rgba(255,255,255,.6)',
//                    fontSize:14
//                },
//                formatter:'{value}%'
//            }
//        }
//    ],
//    series: [
//        {
//            name:'发现违章数',
//            type:'line',
//            yAxisIndex:0,
//            // stack: '总量',
//            data:[28, 28, 17, 34, 28, 36, 20,38, 29, 40, 40, 40],
//            color:'#3879e7'
//        },
//        {
//            name:'未闭环违章数',
//            type:'line',
//            yAxisIndex:0,
//            // stack: '总量',
//            data:[19, 8, 9, 9, 9, 13, 15,8, 8, 8, 12, 14],
//            color:'#eaa309'
//        },
//        {
//            name:'未违章整改率',
//            type:'line',
//            yAxisIndex:1,
//            // stack: '总量',
//            data:[18, 6, 6, 7, 7, 9, 10,7, 7, 7, 9, 10],
//            color:'#d42129'
//        }
//    ]
//};
//function count(obj,option){
//    var Chart5 = echarts.init(obj);
//    Chart5.setOption(option);
//}
//count(obj5,option5)