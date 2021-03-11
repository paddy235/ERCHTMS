$(function () {
    //var mySwiper1 = new Swiper ('.swiper-container1', {
    //    loop: true,
    //    // 如果需要前进后退按钮
    //    nextButton: '.swiper-button-next1',
    //    prevButton: '.swiper-button-prev1',
    //})
    //var mySwiper2 = new Swiper ('.swiper-container2', {
    //    loop: true,
    //    // 如果需要分页器
    //    nextButton: '.swiper-button-next2',
    //    prevButton: '.swiper-button-prev2',
    //})
    var mySwiper3 = new Swiper('.swiper-container3', {
        loop: true,
        // 如果需要分页器
        pagination: '.swiper-pagination',
        paginationClickable: true
    })
    $('.sec4_list li').on('click', function () {
        var index = $(this).index();
        $(this).addClass('active').siblings().removeClass('active');
    })
    $('.secTit_list li').on('click', function () {
        var index = $(this).index();
        $(this).addClass('active').siblings().removeClass('active');
        for (var i = 0; i < 3; i++) {
            if (i == index) {
                $("#TD" + i).show();
                $("#ZNum" + i).show();
            } else {
                $("#TD" + i).hide();
                $("#ZNum" + i).hide();
            }
        }
    })
    $('.secTitChar_list li').on('click', function () {
        var index = $(this).index();
        $(this).addClass('active').siblings().removeClass('active');
        if (index == 0) {
            Station();
        } else if (index == 1) {
            Camera();
        } else {
            Device();
        }
    })

    var scrollParam1 = {
        autohidemode: true,
        // cursorcolor:"#3693f1",
        // cursorwidth:"10px",
        // background:"#113d63",
        // cursorborder:"none",
        // cursorborderradius:"0"
    }
    // $('.rangeList').niceScroll(scrollParam1);
})
$(function () {
    Station();
});


function Station() {
   
    //基站离线数量
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Offlinedevice/GetOffinedeviceImage?type=1",
        type: "get",
        dataType: "text",
        async: false,
        success: function (data) {

            var json = eval("(" + data + ")");
            var option1 = {
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'shadow'
                    }
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '8%',
                    top: '15%',
                    containLabel: true
                },
                xAxis: [
                    {
                        type: 'category',
                        data: ['01月', '02月', '03月', '04月', '05月', '06月', '07月', '08月', '09月', '10月', '11月', '12月'],
                        axisTick: {
                            alignWithLabel: true
                        },
                        axisLabel: {
                            textStyle: {
                                color: '#333',
                                fontSize: 12
                            }
                        },
                        axisLine: {
                            lineStyle: {
                                color: '#2e99d4'
                            }
                        }

                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        axisLabel: {
                            textStyle: {
                                color: '#333',
                                fontSize: 12
                            }
                        },
                        axisLine: {
                            lineStyle: {
                                color: '#2e99d4'
                            }
                        },
                        splitLine: {
                            lineStyle: {
                                color: '#b8d7ff',
                                type: 'dashed'
                            }
                        }
                    }
                ],
                dataZoom: [
                    {
                        type: 'slider',
                        show: true,
                        start: 0,
                        end: 60,
                        handleSize: 8,
                        bottom: 0,
                        height: 8,
                        textStyle: false
                    }
                ],
                series: [
                    {
                        name: '离线次数',
                        type: 'bar',
                        barWidth: '15%',
                        data: json.x,
                        itemStyle: {
                            normal: {
                                color: '#3aa0ff'
                            }
                        },
                        label: {
                            show: true,
                            position: 'top',
                            textStyle: {
                                fontSize: 12,
                                color: '#333'
                            }
                        }
                    }
                ]
            };
            var chart1 = echarts.init(document.getElementById('chart1'));
            chart1.setOption(option1);

        }
    });
    StationTop();
}

function StationTop() {
    selectTop = 1;
    var time = $("#daytime option:selected").val();
    //基站离线数量
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Offlinedevice/GetOffTop?type=1&Time=" + time + "&topNum=6",
        type: "get",
        dataType: "text",
        async: false,
        success: function (data) {

            var json = eval("(" + data + ")");
            html = "<ul class=\"\" tabindex=\"1\" style=\"overflow: hidden;outline: none;\">";
            for (var i = 0; i < json.length; i++) {
                html += "<li>" +
                    "<span class=\"titleNum active\">" + (i + 1) + "</span>" +
                    "<span>基站" + json[i].deviceid + "</span>" +
                    "<span class=\"buleColor3\">" + json[i].offcount + "</span>" +
                    "</li>" +
                    "<li>";
            }
            html += "</ul>";

            $("#ChartTable").html(html);
        }
    });
}


function Camera() {
   
    //摄像头离线数量
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Offlinedevice/GetOffinedeviceImage?type=3",
        type: "get",
        dataType: "text",
        async: false,
        success: function (data) {

            var json = eval("(" + data + ")");
            var option1 = {
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'shadow'
                    }
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '8%',
                    top: '15%',
                    containLabel: true
                },
                xAxis: [
                    {
                        type: 'category',
                        data: ['01月', '02月', '03月', '04月', '05月', '06月', '07月', '08月', '09月', '10月', '11月', '12月'],
                        axisTick: {
                            alignWithLabel: true
                        },
                        axisLabel: {
                            textStyle: {
                                color: '#333',
                                fontSize: 12
                            }
                        },
                        axisLine: {
                            lineStyle: {
                                color: '#2e99d4'
                            }
                        }

                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        axisLabel: {
                            textStyle: {
                                color: '#333',
                                fontSize: 12
                            }
                        },
                        axisLine: {
                            lineStyle: {
                                color: '#2e99d4'
                            }
                        },
                        splitLine: {
                            lineStyle: {
                                color: '#b8d7ff',
                                type: 'dashed'
                            }
                        }
                    }
                ],
                dataZoom: [
                    {
                        type: 'slider',
                        show: true,
                        start: 0,
                        end: 60,
                        handleSize: 8,
                        bottom: 0,
                        height: 8,
                        textStyle: false
                    }
                ],
                series: [
                    {
                        name: '离线次数',
                        type: 'bar',
                        barWidth: '15%',
                        data: json.x,
                        itemStyle: {
                            normal: {
                                color: '#3aa0ff'
                            }
                        },
                        label: {
                            show: true,
                            position: 'top',
                            textStyle: {
                                fontSize: 12,
                                color: '#333'
                            }
                        }
                    }
                ]
            };
            var chart1 = echarts.init(document.getElementById('chart1'));
            chart1.setOption(option1);
        }
    });
    CameraTop();
}

function CameraTop() {
    selectTop = 2;
    var time = $("#daytime option:selected").val();
    //基站离线数量
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Offlinedevice/GetOffTop?type=3&Time=" + time + "&topNum=6",
        type: "get",
        dataType: "text",
        async: false,
        success: function (data) {

            var json = eval("(" + data + ")");
            html = "<ul class=\"\" tabindex=\"1\" style=\"overflow: hidden;outline: none;\">";
            for (var i = 0; i < json.length; i++) {
                html += "<li>" +
                    "<span class=\"titleNum active\">" +
                    (i + 1) +
                    "</span>" +
                    "<span>摄像头" +
                    json[i].deviceid +
                    "</span>" +
                    "<span class=\"buleColor3\">" +
                    json[i].offcount +
                    "</span>" +
                    "</li>" +
                    "<li>";
            }
            html += "</ul>";
            //html = "<ul class=\"\" tabindex=\"1\" style=\"overflow: hidden;outline: none;\">" +
            //    "<li>" +
            //    "<span class=\"titleNum active\">1</span>" +
            //    "<span>摄像头00154121E10</span>" +
            //    "<span class=\"buleColor3\">10</span>" +
            //    "</li>" +
            //    "<li>" +
            //    "<span class=\"titleNum active\">2</span>" +
            //    "<span>摄像头00154121E03</span>" +
            //    "<span class=\"buleColor3\">8</span>" +
            //    "</li>" +
            //    "<li>" +
            //    "<span class=\"titleNum active\">3</span>" +
            //    "<span>摄像头00154121E17</span>" +
            //    "<span class=\"buleColor3\">7</span>" +
            //    "</li>" +
            //    "<li>" +
            //    "<span class=\"titleNum active\">4</span>" +
            //    "<span>摄像头00154121B22</span>" +
            //    "<span class=\"buleColor3\">5</span>" +
            //    "</li>" +
            //    "<li>" +
            //    "<span class=\"titleNum active\">5</span>" +
            //    "<span>摄像头00154121A13</span>" +
            //    "<span class=\"buleColor3\">4</span>" +
            //    "</li>" +
            //    "<li>" +
            //    "<span class=\"titleNum active\">6</span>" +
            //    "<span>摄像头00154121E17</span>" +
            //    "<span class=\"buleColor3\">1</span>" +
            //    "</li>" +
            //    "</ul>";

            $("#ChartTable").html(html);
        }
    });
}

function Device() {
   
    //门禁离线数量
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Offlinedevice/GetOffinedeviceImage?type=2",
        type: "get",
        dataType: "text",
        async: false,
        success: function (data) {

            var json = eval("(" + data + ")");
            var option1 = {
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'shadow'
                    }
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '8%',
                    top: '15%',
                    containLabel: true
                },
                xAxis: [
                    {
                        type: 'category',
                        data: ['01月', '02月', '03月', '04月', '05月', '06月', '07月', '08月', '09月', '10月', '11月', '12月'],
                        axisTick: {
                            alignWithLabel: true
                        },
                        axisLabel: {
                            textStyle: {
                                color: '#333',
                                fontSize: 12
                            }
                        },
                        axisLine: {
                            lineStyle: {
                                color: '#2e99d4'
                            }
                        }

                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        axisLabel: {
                            textStyle: {
                                color: '#333',
                                fontSize: 12
                            }
                        },
                        axisLine: {
                            lineStyle: {
                                color: '#2e99d4'
                            }
                        },
                        splitLine: {
                            lineStyle: {
                                color: '#b8d7ff',
                                type: 'dashed'
                            }
                        }
                    }
                ],
                dataZoom: [
                    {
                        type: 'slider',
                        show: true,
                        start: 0,
                        end: 60,
                        handleSize: 8,
                        bottom: 0,
                        height: 8,
                        textStyle: false
                    }
                ],
                series: [
                    {
                        name: '离线次数',
                        type: 'bar',
                        barWidth: '15%',
                        data: json.x,
                        itemStyle: {
                            normal: {
                                color: '#3aa0ff'
                            }
                        },
                        label: {
                            show: true,
                            position: 'top',
                            textStyle: {
                                fontSize: 12,
                                color: '#333'
                            }
                        }
                    }
                ]
            };
            var chart1 = echarts.init(document.getElementById('chart1'));
            chart1.setOption(option1);
        }
    });
    DeviceTop();
}

function DeviceTop() {
    selectTop = 3;
    var time = $("#daytime option:selected").val();
    //基站离线数量
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Offlinedevice/GetOffTop?type=2&Time=" + time + "&topNum=6",
        type: "get",
        dataType: "text",
        async: false,
        success: function (data) {

            var json = eval("(" + data + ")");
            html = "<ul class=\"\" tabindex=\"1\" style=\"overflow: hidden;outline: none;\">";
            for (var i = 0; i < json.length; i++) {
                html += "<li>" +
                    "<span class=\"titleNum active\">" +
                    (i + 1) +
                    "</span>" +
                    "<span>门禁" +
                    json[i].deviceid +
                    "</span>" +
                    "<span class=\"buleColor3\">" +
                    json[i].offcount +
                    "</span>" +
                    "</li>" +
                    "<li>";
            }
            html += "</ul>";
            //html = "<ul class=\"\" tabindex=\"1\" style=\"overflow: hidden;outline: none;\">" +
            //    "<li>" +
            //    "<span class=\"titleNum active\">1</span>" +
            //    "<span>门禁141154E060B</span>" +
            //    "<span class=\"buleColor3\">13</span>" +
            //    "</li>" +
            //    "<li>" +
            //    "<span class=\"titleNum active\">2</span>" +
            //    "<span>门禁141154E010B</span>" +
            //    "<span class=\"buleColor3\">12</span>" +
            //    "</li>" +
            //    "<li>" +
            //    "<span class=\"titleNum active\">3</span>" +
            //    "<span>门禁141154E044B</span>" +
            //    "<span class=\"buleColor3\">12</span>" +
            //    "</li>" +
            //    "<li>" +
            //    "<span class=\"titleNum active\">4</span>" +
            //    "<span>门禁141154E0560F</span>" +
            //    "<span class=\"buleColor3\">7</span>" +
            //    "</li>" +
            //    "<li>" +
            //    "<span class=\"titleNum active\">5</span>" +
            //    "<span>门禁141154E711A</span>" +
            //    "<span class=\"buleColor3\">4</span>" +
            //    "</li>" +
            //    "<li>" +
            //    "<span class=\"titleNum active\">6</span>" +
            //    "<span>门禁141154E211C</span>" +
            //    "<span class=\"buleColor3\">2</span>" +
            //    "</li>" +
            //    "</ul>";

            $("#ChartTable").html(html);
        }
    });
}
