//初始化
function InitialPage() {
    $('#desktop').height($(window).height() - 22);
    $(window).resize(function (e) {
        window.setTimeout(function () {
            $('#desktop').height($(window).height() - 22);
        }, 200);
        e.stopPropagation();
    });
}



//首页作业实时分布
function IndexLable() {
    $.get(top.contentPath + "/KbsDeviceManage/Safeworkcontrol/GetLableChart", {}, function (data) {
        var count = 0;
        var list = eval(data);
        var datalist = new Array();
        for (var i = 0; i < list.length; i++) {
            datalist.push({ value: list[i][1], name: list[i][0] });
            count += list[i][1];
        }

        var colorList1 = ['#3aa0ff', '#36cbcb', '#4dcb73', '#fad337', '#f2637b', '#975fe4'];
        var Colorlist = new Array();
        //标签统计列表
        $.ajax({
            url: top.contentPath + "/KbsDeviceManage/Safeworkcontrol/GetWorkRealTimeDistribution",
            type: "post",
            dataType: "json",
            async: false,
            success: function (data) {
                //<img src=\"../../Content/images/kbs/legend_icon"+(i+1)+".png\">
                var html = "  <ul class=\"legendList2\">";
                for (var i = 0; i < data.length; i++) {
                    Colorlist.push({ value: colorList1[i], name: data[i].Name });
                    html += "<li><span style=\"width:14px;height:14px; background:" + colorList1[i] + ";border-radius:7px; margin-right:5px;\"></span> <span class=\"danger\">" +
                        data[i].Name +
                        "</span><span class=\"per_span\">" +
                        data[i].Proportion +
                        "</span>" +
                        data[i].Num +
                        "</li>";
                }
                html += "</ul>";
                $("#LableBox").html(html);
            }
        });

        var option3 = {
            title: {
                text: '作业总数',
                subtext: count,
                subtextStyle: {
                    color: '#333',
                    fontSize: 22,
                    fontWeight: 'bold'

                },
                textStyle: {
                    color: '#333',
                    fontWeight: 'bold',
                    fontSize: 16
                },
                x: 'center',
                y: '41%'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{b}<br/> {c} ({d}%)"
            },
            legend: {
                show: false
            },
            grid: {
                top: '8%',
                containLabel: true
            },

            calculable: true,
            series: [
                {
                    name: '',
                    type: 'pie',
                    radius: ['55%', '85%'],
                    sort: 'ascending',     // for funnel
                    data: datalist,
                    label: {
                        show: false
                    },
                    itemStyle: {
                        normal: {
                            color: function (params) {
                                for (var j = 0; j < Colorlist.length; j++) {
                                    if (params.name == Colorlist[j].name) {
                                        return Colorlist[j].value;
                                    }
                                }
                              
                            },
                        },

                    }
                }
            ]
        };

        var chart3 = echarts.init(document.getElementById('chart3'));
        chart3.setOption(option3);
    });


    

}

