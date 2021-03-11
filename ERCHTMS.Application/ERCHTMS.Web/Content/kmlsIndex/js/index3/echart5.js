var chart5

//var data7 = [{
//    name: '临时外包',
//    value: 0
//}, {
//    name: '长协外包',
//    value: 1
//}]




$(function () {

    var data7 = new Array();
    var names = [];
    var date = new Date();
    $.get("../OutsourcingProject/Outsouringengineer/OutEngineerStat", { deptid: "", year: date.getFullYear() }, function (data) {
         var list = eval("(" + data + ")");
        var color = ['#ff7f50', '#da70d6']
        for (var i = 0; i < list.length; i++) {
            var html = { name: list[i][0], value: list[0][1] };
            //data7.push(html);
            data7.push({ value: list[i][1], name: list[i][0], itemStyle: { color: color[i] } });
            names.push(list[i][0]);
        }

       
        var option = {
            legend: {
                selectedMode: true,
                bottom: 0,
                itemGap: 20,
                itemWidth: 20,
                itemHeight: 14,
                width: 700,
                x: 'center',
                textStyle: {
                    fontSize: 12,
                    color: '#cccccc',
                    textAlign: 'left'

                },
                data: names,
            },
            title: {
                x: 'center',
                top: 100
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            series: [{
                name: '外包工程建设情况',
                type: 'pie',
                hoverAnimation: true,
                radius: '60%',
                center: ['50%', '45%'],
                roseType: 'radius',
                color: color,
                label: {
                    normal: {
                        show: true,
                        position: 'absolute',
                        color: '#cccccc',
                        fontSize: 14
                    }
                },
                labelLine: {
                    normal: {
                        show: true,
                        length: 10,
                        length2: 5,
                        lineStyle: {

                        }
                    }
                },
                data: data7
            }]
        }
        chart5 = echarts.init(document.querySelector('.charts2'));
        chart5.setOption(option);



       // data7 = [{ name: '临时外包', value: 0 }, { name: '长协外包', value: 1 }];


    });


});





