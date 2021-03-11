//高风险作业统计
function getWorkStat() {
    $.ajax({
        dataType: "json",
        type: "post",
        url: "../../Desktop/GetHighRiskWorkingForLeaderCockpit",
        success: function (json) {
            if (json.type == 1) {
                var data = [];
                var rows = $.parseJSON(json.resultdata);
                $(rows.tjdata).each(function (j, item) {
                    data.push({
                        name: item.itemname,
                        value: item.nums
                    });
                    var color = ['#ff7f50', '#da70d6', '#32cd32', '#6495ed', '#ff69b4', '#ba55d3', '#cd5c5c', '#ffa500', '#ffa500', '#54b1f9']
                    var option = {
                        legend: {
                            selectedMode: true,
                            bottom: 15,
                            left: 0,
                            itemGap: 15,
                            itemWidth: 13,
                            itemHeight: 8,
                            width: 650,
                            x: 'center',
                            textStyle: {
                                fontSize: 12,
                                color: '#cccccc',
                                textAlign: 'left'

                            }
                            // data: ['有限空间作业', '高处作业', '电除尘拆作业', '吸收塔防腐作业', '夜间作业', '有毒有害作业', '起重吊装作业', '脚手架搭设作业', '脚手架拆除作业', '安全设施变动'],
                        },

                        series: [{
                            type: 'pie',
                            hoverAnimation: true,
                            radius: '45%',
                            center: ['50%', '45%'],
                            roseType: 'radius',
                            //color: color,
                            label: {
                                normal: {
                                    show: true,
                                    position: 'absolute',
                                    color: '#cccccc',
                                    fontSize: 14
                                }
                            },
                            itemStyle: {
                                normal: {
                                    label: {
                                        show: true,
                                        formatter: '{b}:{c}  {d}%'
                                    }
                                }
                            },
                            labelLine: {
                                normal: {
                                    // formatter:'{c}% \n {b} \n\n',

                                    show: true,
                                    length: 20,
                                    length2: 15,
                                    lineStyle: {

                                    }
                                }
                            },
                            data: data
                        }]
                    }
                    var chart = echarts.init(document.querySelector('.charts2'));
                    chart.setOption(option);
                });
            }
        }
    });
}