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


//标签统计
function stat() {
    $.get("GetLableChart", {}, function (data) {
        var count = 0;
        var list=eval(data);
        var datalist = new Array();
        for (var i = 0; i < list.length; i++) {
            datalist.push({ value: list[i][1], name: list[i][0] });
            count += list[i][1];
        }

        var option3 = {
            title: {
                text: '标签统计',
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


    //标签统计列表
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Lablemanage/GetLableStatistics",
        type: "post",
        dataType: "json",
        async: false,
        success: function (data) {

            var num = 0;
            var html = " <table class=\"table1\">";
            html += "<thead><tr><th width=\"25%\">标签类型<span class=\"\"></span></th><th width=\"25%\">标签数量<span class=\"active\"></span></th><th width=\"25%\">标签占比<span class=\"\"></span></th><th width=\"25%\">在线数量</th></tr> </thead>   <tbody>";
            for (var i = 0; i < data.length; i++) {
                html += "<tr><td class=\"group_interal\">" + data[i].Name + "</td><td class=\"group_interal group_underline\">" + data[i].Num + "</td><td class=\"group_interal group_underline\">" + data[i].Proportion + "</td><td class=\"group_interal group_underline\">" + data[i].Num2 + "</td></tr>";
                num += Number(data[i].Num2);
            }
            html+="</tbody></table>";
            $("#tabcontent").html(html);
            $("#OnlineNum").html(num);
        }
    });
    //标签总数
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Lablemanage/GetCount",
        type: "post",
        dataType: "json",
        async: false,
        success: function (data) {
            $("#ZNum").html(data);
            //$("#OnlineNum").html(data);
        }
    });
    
}


//摄像头统计
function Camerastat() {
    $.get("GetCameraChart", {}, function (data) {

        var count = 0;
        var list = eval(data);
        var datalist = new Array();
        for (var i = 0; i < list.length; i++) {
            datalist.push({ value: list[i][1], name: list[i][0] });
            count += list[i][1];
        }

        var option3 = {
            title: {
                text: '摄像头数量统计',
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


    //摄像头统计列表
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Kbscameramanage/GetCameraStatistics",
        type: "post",
        dataType: "json",
        async: false,
        success: function (data) {
            var html = " <table class=\"table1\">";
            html += "<thead><tr><th width=\"25%\">区域<span class=\"\"></span></th><th width=\"25%\">摄像头数量<span class=\"active\"></span></th><th width=\"25%\">占比<span class=\"\"></span></th><th width=\"25%\">在线数量</th></tr> </thead>   <tbody>";
            for (var i = 0; i < data.length; i++) {
                html += "<tr><td class=\"group_interal\">" + data[i].Name + "</td><td class=\"group_interal group_underline\">" + data[i].Num + "</td><td class=\"group_interal group_underline\">" + data[i].Proportion + "</td><td class=\"group_interal group_underline\">" + data[i].OnNum + "</td></tr>";

            }
            html += "</tbody></table>";
            $("#tabcontent").html(html);
        }
    });
    //摄像头总数
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Kbscameramanage/GetCount",
        type: "post",
        dataType: "json",
        async: false,
        success: function (data) {
            $("#ZNum").html(data);
            $("#OnlineNum").html(data);
        }
    });

}

function Devicestat() {
    $.get("GetDeviceChart", {}, function (data) {
        var count = 0;
        var list = eval(data);
        var datalist = new Array();
        for (var i = 0; i < list.length; i++) {
            datalist.push({ value: list[i][1], name: list[i][0] });
            count += list[i][1];
        }

        var option3 = {
            title: {
                text: '门禁数量统计',
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


    //统计列表
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Kbsdevice/GetDeviceStatistics",
        type: "post",
        dataType: "json",
        async: false,
        success: function (data) {

            var html = " <table class=\"table1\">";
            html += "<thead><tr><th width=\"25%\">区域<span class=\"\"></span></th><th width=\"25%\">门禁数量<span class=\"active\"></span></th><th width=\"25%\">占比<span class=\"\"></span></th><th width=\"25%\">在线数量</th></tr> </thead>   <tbody>";
            for (var i = 0; i < data.length; i++) {
                html += "<tr><td class=\"group_interal\">" + data[i].Name + "</td><td class=\"group_interal group_underline\">" + data[i].Num + "</td><td class=\"group_interal group_underline\">" + data[i].Proportion + "</td><td class=\"group_interal group_underline\">" + data[i].Num2 + "</td></tr>";

            }
            html += "</tbody></table>";
            $("#tabcontent").html(html);
        }
    });
    //门禁总数
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Kbsdevice/GetCount",
        type: "post",
        dataType: "json",
        async: false,
        success: function (data) {
            $("#ZNum").html(data);
            $("#OnlineNum").html(data);
        }
    });
}


//首页标签
function IndexLable() {
    $.get(top.contentPath + "/KbsDeviceManage/Lablemanage/GetLableChart", {}, function (data) {
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
            url: top.contentPath + "/KbsDeviceManage/Lablemanage/GetLableStatistics",
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
                text: '绑定总数',
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
 //摄像头统计列表
function IndexCamera() {
    //摄像头统计列表
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Kbscameramanage/GetCameraStatistics",
        type: "post",
        dataType: "json",
        async: false,
        success: function (data) {
            var flag = true;
            var html = " <table class=\"table\">";
            html += "<thead><tr><th width=\"28%\">区域<span class=\"\"></span></th><th width=\"18%\">在线数量<span class=\"active\"></span></th><th width=\"18%\">占比<span class=\"\"></span></th><th width=\"18%\">离线数量</th><th width=\"18%\">占比<span class=\"\"></span></th></tr></thead><tbody>";
            for (var i = 0; i < data.length; i++) {
                if (flag) {
                    html += "<tr><td>" +
                        data[i].Name +
                        "</td><td> <a  href=\"javascript:showKbscameramanage('" + encodeURIComponent(data[i].DistrictCode) + "',0)\">" +
                        data[i].OnNum +
                        "</a></td><td>" +
                        data[i].OnProportion +
                        "</td><td> <a  href=\"javascript:showKbscameramanage('" + encodeURIComponent(data[i].DistrictCode) + "',1)\">" +
                        data[i].OffNum +
                        "</a></td><td>" +
                        data[i].OfflineProportion +
                        "</td></tr>";
                    flag = false;
                } else {
                    html += "<tr><td class=\"group_interal\">" +
                        data[i].Name +
                        "</td><td class=\"group_interal\"> <a  href=\"javascript:showKbscameramanage('" + encodeURIComponent(data[i].DistrictCode) + "',0)\">" +
                        data[i].OnNum +
                        "</a></td><td class=\"group_interal\">" +
                        data[i].OnProportion +
                        "</td><td class=\"group_interal\"><a  href=\"javascript:showKbscameramanage('" + encodeURIComponent(data[i].DistrictCode) + "',1)\">" +
                        data[i].OffNum +
                        "</a></td><td class=\"group_interal\">" +
                        data[i].OfflineProportion +
                        "</td></tr>";
                    flag = true;
                }
                
            }
            html += "</tbody></table>";
            $("#TD1").html(html);
            //摄像头总数
            $.ajax({
                url: top.contentPath + "/KbsDeviceManage/Kbscameramanage/GetCount",
                type: "post",
                dataType: "json",
                async: false,
                success: function (data) {
                    $("#ZNum1").html("总数："+data);
                }
            });
        }
    });
}


function IndexDevice() {
    //门禁统计列表
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Kbsdevice/GetDeviceStatistics",
        type: "post",
        dataType: "json",
        async: false,
        success: function (data) {

            var flag = true;
            var html = " <table class=\"table\">";
            html += "<thead><tr><th width=\"28%\">区域<span class=\"\"></span></th><th width=\"18%\">在线数量<span class=\"active\"></span></th><th width=\"18%\">占比<span class=\"\"></span></th><th width=\"18%\">离线数量</th><th width=\"18%\">占比<span class=\"\"></span></th></tr></thead><tbody>";
            for (var i = 0; i < data.length; i++) {
                if (flag) {
                    html += "<tr><td>" +
                        data[i].Name +
                        "</td><td> <a  href=\"javascript:showKbsdevice('" + encodeURIComponent(data[i].DistrictCode) + "',0)\">" +
                        data[i].OnNum +
                        "</a></td><td>" +
                        data[i].OnProportion +
                        "</td><td><a  href=\"javascript:showKbsdevice('" + encodeURIComponent(data[i].DistrictCode) + "',1)\">" +
                        data[i].OffNum +
                        "</a></td><td>" +
                        data[i].OfflineProportion +
                        "</td></tr>";
                    flag = false;
                } else {
                    html += "<tr><td class=\"group_interal\">" +
                        data[i].Name +
                        "</td><td class=\"group_interal\"> <a  href=\"javascript:showKbsdevice('" + encodeURIComponent(data[i].DistrictCode) + "',0)\">" +
                        data[i].OnNum +
                        "</a></td><td class=\"group_interal\">" +
                        data[i].OnProportion +
                        "</td><td class=\"group_interal\"><a  href=\"javascript:showKbsdevice('" + encodeURIComponent(data[i].DistrictCode) + "',1)\">" +
                        data[i].OffNum +
                        "</a></td><td class=\"group_interal\">" +
                        data[i].OfflineProportion +
                        "</td></tr>";
                    flag = true;
                }

            }
            html += "</tbody></table>";
            $("#TD2").html(html);
        }
    });

    //门禁总数
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Kbsdevice/GetCount",
        type: "post",
        dataType: "json",
        async: false,
        success: function (data) {
            $("#ZNum2").html("总数：" + data);
        }
    });
}