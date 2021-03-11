$(function () {
    InitialPage();
    stat();
})
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


//模块统计
function stat() {
    //基站饼图统计
    $.get("GetLableChart", {}, function (data) {
        var count = 0;
        var list = eval(data);
        var datalist = new Array();
        for (var i = 0; i < list.length; i++) {
            datalist.push({ value: list[i][1], name: list[i][0] });
            count += list[i][1];
        }
        var option3 = {
            title: {
                text: '基站统计',
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
                                var colorList1 = ['#3aa0ff', '#36cbcb', '#4dcb73', '#fad337', '#f2637b', '#975fe4']
                                return colorList1[params.dataIndex]
                            },
                        },
                    }
                }
            ]
        };

        var chart3 = echarts.init(document.getElementById('chart4'));
        chart3.setOption(option3);


    });


    //基站表格统计
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/BaseStation/GetLableStatistics",
        type: "post",
        dataType: "json",
        async: false,
        success: function (data) {

            var html = " <table class=\"table1\">";
            html += "<thead><tr><th width=\"25%\">区域<span class=\"\"></span></th><th width=\"25%\">基站数量<span class=\"active\"></span></th><th width=\"25%\">占比<span class=\"\"></span></th><th width=\"25%\">在线数量</th></tr> </thead>   <tbody>";
            for (var i = 0; i < data.length; i++) {
                html += "<tr><td class=\"group_interal\">" + data[i].Name + "</td><td class=\"group_interal group_underline\">" + data[i].Num + "</td><td class=\"group_interal group_underline\">" + data[i].Proportion + "</td><td class=\"group_interal group_underline\">" + data[i].Num2 + "</td></tr>";
            }
            html += "</tbody></table>";
            $("#tabcontent").html(html);
        }
    });
    //基本总数量统计
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/BaseStation/GetCount",
        type: "post",
        dataType: "json",
        async: false,
        success: function (data) {
          //  $("#str").html("&nbsp;&nbsp;统计图表&nbsp;&nbsp;总数<a style=\"color: rgb(46, 153, 212);\">" + data + "</a>个  " + "在线<a style=\"color: rgb(46, 153, 212);\">" + data + "</a>个");
            $("#ZNum").html(data);
            $("#OnlineNum").html(data);
        }
    });
    
}

//首页基站统计
function Indexstat() {
    //表格统计
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/BaseStation/GetIndexBaseStation",
        type: "post",
        dataType: "json",
        async: false,
        success: function (data) {
            var html = "";
            var count = 0;
            for (var i = 0; i < data.length; i++) {
                if (i % 2 == 0) {
                    html += "<tr><td>" + data[i].Name + " </td><td> <a  href=\"javascript:ShowBaseStation('" + encodeURIComponent(data[i].DistrictCode) + "',0)\">" + data[i].OnNum + "</a></td><td>" + data[i].OnProportion + "</td><td> <a  href=\"javascript:ShowBaseStation('" + encodeURIComponent(data[i].DistrictCode) + "',1)\">" + data[i].OffNum + "</a></td><td>" + data[i].OfflineProportion + "</td></tr>";
                }
                else {
                    html += "<tr><td class=\"group_interal\">" + data[i].Name + "</td> <td class=\"group_interal\"><a href=\"javascript:ShowBaseStation('" + encodeURIComponent(data[i].DistrictCode) + "',0)\">" + data[i].OnNum + "</a></td> <td class=\"group_interal\">" + data[i].OnProportion + "</td> <td class=\"group_interal\"><a  href=\"javascript:ShowBaseStation('" + encodeURIComponent(data[i].DistrictCode) + "',1)\">" + data[i].OffNum + "</a></td> <td class=\"group_interal\">" + data[i].OfflineProportion + "</td></tr>";
                }
                count = data[i].Count;;
            }
            $("#ZNum0").html("总数：" + count);
            $("#BaseStationbody").html(html);
        }
    });




     
}

