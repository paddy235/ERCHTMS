$(function(){
    //var mySwiper1 = new Swiper ('.swiper-container1', {
    //    loop: true,
    //    // 如果需要前进后退按钮
    //    nextButton: '.swiper-button-next1',
    //    prevButton: '.swiper-button-prev1',
    //})
    var mySwiper2 = new Swiper ('.swiper-container2', {
        loop: true,
        // 如果需要分页器
        nextButton: '.swiper-button-next2',
        prevButton: '.swiper-button-prev2',
    })
    var mySwiper3 = new Swiper ('.swiper-container3', {
        loop: true,
        // 如果需要分页器
        pagination: '.swiper-pagination',
        paginationClickable: true
    })
    $('.sec4_list li').on('click',function () {
        var index = $(this).index();
        $(this).addClass('active').siblings().removeClass('active');
    })
    $('.secTit_list li').on('click',function () {
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
            // Device();
        }
    })
    
    var scrollParam1 = {
        autohidemode:true,
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

    var arr = new Array();
    $.ajax({
        url: '../../KbsDeviceManage/Safeworkcontrol/GetWorkMonthGroupJson?type=0',
        type: "POST",
        async: false,
        dataType: "Json",
        success: function (result) {
            arr = result;
        }
    });


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
                name: '作业总数',
                type: 'bar',
                barWidth: '15%',
                data: arr,
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


function Camera() {
    var arr = new Array();
    $.ajax({
        url: '../../KbsDeviceManage/Safeworkcontrol/GetWorkMonthGroupJson?type=1',
        type: "POST",
        async: false,
        dataType: "Json",
        success: function (result) {
            arr = result;
        }
    });



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
                name: '异常数量',
                type: 'bar',
                barWidth: '15%',
                data: arr,
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

    //$("#ChartTable").html(html);
}
