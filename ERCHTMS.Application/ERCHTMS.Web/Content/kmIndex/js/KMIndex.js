/**
 * 可门安全智能管控中心首页数据绑定js 
 * heming
 * */

var hikId = "";//设备Id
var interval;//定时器
/**加载设备区域 */
var loadDeviceArea = () => {
    //加载设备归属区域
    $.ajax({
        url: "../SystemManage/DataItemDetail/GetDataItemListJson",
        data: { EnCode: "DoorPost" },
        type: "GET",
        dataType: "JSON",
        success: function (data) {
            let html = "";
            if (!!data && data.length > 0) {
                data.forEach(item => {
                    html += '<option value="' + item.ItemValue + '">' + item.ItemName + '</option>';
                });
            } else {
                html = '<option value="">未找到设备归属区域</option>';
            }
            $("#DeviceArea option").remove();
            $("#DeviceArea").append(html);
            $("#DeviceArea").trigger("change");
        },
        error: function (error) {
            console.log(error)
            console.log("门岗实时监控加载失败： 获取设备归属区域请求失败");
        }
    });
};

/**
 * 加载设备数据
 * @param {string} areaName  区域名称
 */
var loadDevice = (areaName) => {
    $.ajax({
        url: "../CarManage/HikDevice/GetDeviceByArea",
        data: { areaName: areaName },
        type: "GET",
        dataType: "JSON",
        success: function (data) {
            let html = "";
            if (!!data && data.length > 0) {
                data.forEach(item => {
                    html += '<option value="' + item.HikID + '">' + item.DeviceName + '</option>';
                });
            } else {
                html = '<option value="">未找到所选区域的设备</option>';
            }
            $("#DeviceList option").remove();
            $("#DeviceList").append(html);
            $("#DeviceList").trigger("change");
        },
        error: function (error) {
            console.log(error)
            console.log("门岗实时监控加载失败： 获取所选区域的设备请求失败");
        }
    });
}


/** 加载设备区域下拉列表的事件**/
var loadAreaEvent = () => {
    //设备区域改变事件
    $("#DeviceArea").change(function () {
        if (!!interval) { window.clearInterval(interval); console.log("已经清除") }//清除定时器
        var areaName = $(this).val();
        loadDevice(areaName);
    });
};

/**加载设备选择事件 */
var loadDeviceEvent = () => {
    $("#DeviceList").change(function () {
        hikId = $(this).val();
        LoadPerson();//先立即执行一次
        interval = setInterval(() => {
            LoadPerson();
        }, 5000);
    });
};
/**获取人员进出信息 */
var LoadPerson = () => {
    $.ajax({
        url: "../CarManage/HikDevice/GetDoorRecord",
        data: { hikId: hikId },
        type: "GET",
        dataType: "JSON",
        success: function (data) {
            let html = "";
            if (!!data && data.length > 0) {
                data.forEach(item => {
                    var thisDate = new Date(item.CreateDate);
                    html += '<tr>';
                    html += '<td>' + thisDate.getHours() + ':' + thisDate.getMinutes() + '</td>';
                    html += '<td>' + item.UserName + ' </td>';
                    html += ' <td>' + item.InOut + '</td>';
                    html += ' <td><div style="width:150px; overflow:hidden;white-space:nowrap;">' + item.DeptName + '</div></td>';
                    html += '</tr>';
                });
                //var urls = window.location.href.split("/");
                //$("#ShotImg").attr("src", urls[0] + "//" + urls[2] + "/" + urls[3] + data[0].ScreenShot);
                //if (!!data[0].ScreenShot) {
                //    $("#ShotImg").attr("src", ".." + data[0].ScreenShot);
                //}
                if (!!data[0].ScreenShot) {
                    $("#ShotImg").attr("src", GetHikImgUrl(data[0].ScreenShot));
                }
            };
            $("#tb_PersonInOut tr").remove();
            $("#tb_PersonInOut").append(html);
        },
        error: function (error) {
            console.log(error)
            console.log("门岗实时监控加载失败： 获取人员进出数据失败请求失败");
        }
    });
}
/**加载设备间实时监控 */
var LoadDeviceWatch = () => {
    $.ajax({
        url: "../CarManage/Hikinoutlog/DeviceWatch",
        type: "GET",
        dataType: "JSON",
        success: function (data) {
            if (!!data) {
                $("#deviceWatch_userName").text(data.UserName);
                $("#deviceWatch_deptName").text(data.DeptName);
                $("#deviceWatch_inOut").text(data.InOut == 0 ? "进门" : "出门");
                $("#deviceWatch_Datetime").text(data.CreateDate);
                if (!!data.ScreenShot) {
                    $("#deviceWatch_img").attr("src", ".." + data.ScreenShot);
                }
            }
        },
        error: function (error) {
            console.log(error)
            console.log("获取设备间实时监控失败");
        }
    });
}
//根据责任部门分组获取今日高风险作业
var HighRiskWork = () => {
    $.ajax({
        url: "../CarManage/Hikdevice/HighRiskWork",
        type: "POSt",
        async: true,
        dataType: "Json",
        success: function (result) {
            if (result.code == "0") {
                HighRiskWorkHtml(result.data);

            }
        }
        ,
        error: function (error) {
            console.log(error)
            console.log("加载失败： 获取高风险作业请求失败");
        }
    });
}
var HighRiskWorkHtml = (data) => {
    $("#HighRiskWork").html("");
    $("#tabHighRiskWork").html("");
    var Html = "";
    var tabHtml = "";

    for (var i = 0; i < data.tempData.length; i++) {
        var sort = 0;
        for (var j = 0; j < data.tempData[i].TodayWorkList.length; j++) {
            if (sort == 0 || sort % 2 == 0) {
                if (sort == 0) {
                    Html += ' <div class="bluid bluid_one">';
                } else {
                    Html += '  </div>';
                    Html += ' <div class="bluid bluid_display bluid_one">';
                }

                Html += '     <div class="one">';
                Html += '        <div class="computer background">';
                Html += '             <img src="../Content/kmlsIndex/img/two/computer2.png"  alt="">';
                Html += '                    </div>';
                Html += '             <div class="number white background">【全厂】高风险作业监督</div>';
                Html += '           <div class=" roadwork">';
                Html += '                <span class="background">&nbsp;作业数&nbsp;&nbsp;' + data.totalProNum + '个</span>';
                Html += '               <span class="background">&nbsp;施工人数&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + data.totalPersonNum + '人</span>';
                Html += '       </div>';
                Html += '    </div>';
            }



            Html += '    <div class="epiboly">';
            Html += '  <div class=" company white background">作业单位：' + data.tempData[i].TodayWorkList[j].WorkDept + '</div>';
            Html += '</div>';
            Html += '  <div class="background project">';
            Html += '     <ul>';
            Html += '          <li>';
            Html += '              <span>作业类型：' + data.tempData[i].TodayWorkList[j].WorkType + '</span>&nbsp;&nbsp;&nbsp;&nbsp;';
            Html += '                                  <span>作业地点：' + data.tempData[i].TodayWorkList[j].WorkPlace + '</span>';
            Html += '          </li>';
            Html += '          <li>';
            Html += '             <span>';
            Html += '                    风险等级：';
            Html += '                      <span class="one_risk">' + data.tempData[i].TodayWorkList[j].RiskType + '</span>';
            Html += '                       </span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;';
            Html += '                                  <span>监护人：' + data.tempData[i].TodayWorkList[j].WorkTutelagePerson + '</span>';
            Html += '           </li>';
            Html += '      </ul>';
            Html += '  </div>';
            sort++;
        }


    }
    var ulHtmlSort = 0;
    for (var i = 1; i <= data.totalProNum; i++) {
        if (i == 1) {
            tabHtml += '    <li class="cur"></li>';
            ulHtmlSort = 0;
        } else
            if (i % 2 == 1) {
                if (ulHtmlSort == 0) {
                    tabHtml += '    <li class="align"></li>';
                    ulHtmlSort = 1;
                } else {
                    tabHtml += '    <li class="cur"></li>';
                    ulHtmlSort = 0;
                }

            }
    }
    $("#HighRiskWork").append(Html);
    $("#tabHighRiskWork").append(tabHtml);
    // $("#HighRiskWork img").attr("src", "../Content/kmlsIndex/img/two/computer2.png");
}


//根据责任部门分组获取今日临时外包工程
var WorkMeeting = () => {
    $.ajax({
        url: "../CarManage/Hikdevice/WorkMeeting",
        type: "POST",
        async: true,
        dataType: "Json",
        success: function (result) {
            if (result.code == "0") {
                WorkMeetingHtml(result.data);

            }
        }
        ,
        error: function (error) {
            console.log(error)
            alert("加载失败： 获取外包工程数据统计请求失败");
        }
    });
}

var WorkMeetingHtml = (data) => {
    $("#WorkMeeting").html("");
    $("#tabWorkMeeting").html("");
    var Html = "";

    var tabHtml = "";

    for (var i = 0; i < data.tempData.length; i++) {
        var sort = 0;
        for (var j = 0; j < data.tempData[i].ProList.length; j++) {
            if (sort == 0 || sort % 2 == 0) {
                if (sort == 0) {
                    Html += ' <div class="bluid bluid_one">';
                } else {
                    Html += '  </div>';
                    Html += ' <div class="bluid bluid_display bluid_one">';
                }

                Html += '     <div class="one">';
                Html += '        <div class="computer background">';
                Html += '             <img src="../Content/kmlsIndex/img/two/computer.png" alt="">';
                Html += '                    </div>';
                Html += '             <div class="number white background">【全厂】外包工程数量</div>';
                Html += '           <div class=" roadwork">';
                Html += '                <span class="background">&nbsp;进行中工程数量&nbsp;&nbsp;' + data.totalProNum + '个</span>';
                Html += '               <span class="background">&nbsp;施工人数&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + data.totalPersonNum + '个</span>';
                Html += '       </div>';
                Html += '    </div>';
            }


            Html += '    <div class="epiboly">';
            Html += '  <div class=" company white background">外包单位：' + data.tempData[i].ProList[j].UnitName + '</div>';
            Html += '</div>';
            Html += '  <div class="background project">';
            Html += '     <ul>';
            Html += '          <li>';
            Html += '              <span>工程名称：' + data.tempData[i].ProList[j].ProName + '</span>&nbsp;&nbsp;&nbsp;&nbsp;';
            Html += '                                  <span>责任部门负责人：' + data.tempData[i].ProList[j].DeptPersonName + '</span>';
            Html += '          </li>';
            Html += '           <li>';
            Html += '               <span>施工地点：' + data.tempData[i].ProList[j].Address + '</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;';
            Html += '                                    <span>施工人数：' + data.tempData[i].ProList[j].RealperNum + '人</span>';
            Html += '          </li>';
            Html += '      </ul>';
            Html += '  </div>';

            sort++;
        }


    }
    var ulHtmlSort = 0;
    for (var i = 1; i <= data.totalProNum; i++) {
        if (i == 1) {
            tabHtml += '    <li class="cur"></li>';
            ulHtmlSort = 0;
        } else
            if (i % 2 == 1) {
                if (ulHtmlSort == 0) {
                    tabHtml += '    <li class="align"></li>';
                    ulHtmlSort = 1;
                } else {
                    tabHtml += '    <li class="cur"></li>';
                    ulHtmlSort = 0;
                }

            }
    }
    $("#WorkMeeting").append(Html);
    $("#tabWorkMeeting").append(tabHtml);
}

/** 加载预警中心数据 */
var loadWaringData = () => {
    //加载列表数据
    $.ajax({
        url: "../CarManage/Carviolation/GetIndexWaring",
        type: "GET",
        dataType: "JSON",
        success: function (data) {
            if (!!data && data.length > 0) {
                var html = "";
                data.forEach(item => {
                    if (item.IsProcess == 0)
                        html += '<tr  class="table_cur">';
                    else
                        html += "<tr>";
                    switch (item.ViolationType) {
                        case 0:
                            html += '<td>超速预警</td>';
                            break;
                        case 1:
                            html += '<td>航迹偏离</td>';
                            break;
                        case 2:
                            html += '<td>超时预警</td>';
                            break;
                        default:
                            html += '<td>其他预警</td>';
                            break;
                    }
                    html += '<td>' + item.ViolationMsg + '</td>';
                    var createdate = new Date(item.CreateDate);
                    html += '  <td>' + (createdate.getHours()) + ':' + (createdate.getMinutes()) + '</br>' + (createdate.getFullYear()) + '-' + (createdate.getMonth() + 1) + '-' + (createdate.getDate()) + '</td>';
                    if (item.IsProcess == 0)
                        html += '  <td>未处理</td>';
                    else
                        html += '  <td>已处理</td>';
                    html += '   <td>' + (!!item.ProcessMeasure ? item.ProcessMeasure : "")+ '</td>';
                    //html += '   <td>';
                    //html += '      <span>处理</span>';
                    //html += '   </td>';
                    html += '  </tr>';
                });
                $("#warning_list tr").remove();
                $("#warning_list").append(html);
            }
        },
        error: function (error) { console.error("加载预警中心数据失败！"); console.log(error); }
    });
    //加载统计数据
    $.ajax({
        url: "../CarManage/Carviolation/GetIndexWaringCount",
        type: "GET",
        dataType: "JSON",
        success: function (data) {
            $("#warning_process").text(data.YCL);
            $("#warning_notProcess").text(data.WCL);
        },
        error: function (error) { console.error("加载预警中心统计数据失败！"); console.log(error); }
    });
}

/**加载实时工作  */
var loadRealTimeWork = () => {
    $.ajax({
        dataType: "json",
        type: "get",
        url: "../desktop/getrealtimework",
        dataType: "JSON",
        success: function (json) {
            if (json.type == 1) {
                var html = "";
                $(json.resultdata).each(function (j, item) {
                    if (j < 5) {
                        var deptName = item.DeptName == null ? "" : item.DeptName;
                        deptName = deptName.length > 8 ? deptName.substring(0, 8) : deptName;
                        var Time = new Date(item.Time);
                        html += '<tr>';
                        html += '  <td>' + (Time.getFullYear()) + '年' + (Time.getMonth() + 1) + '月' + (Time.getDate())+ '日</td>';
                        html += '  <td>' + (Time.getHours()) + ':' + (Time.getMinutes()) + ' </td>';
                        html += '<td>' + deptName + '</td>';
                        html += '<td>' + item.WorkDescribe + '</td>';
                        html += '</tr>';
                        //html += '<tr><td>' + formatDate(item.Time, "yyyy-MM-dd hh:mm") + '</td><td>' + deptName + '</td><td title=\"' + item.WorkDescribe + '\">' + (item.WorkDescribe.length > 25 ? item.WorkDescribe.substring(0, 25) + "..." : item.WorkDescribe) + '</td><tr>';
                    }
                });
                $("#realTimeWork_list").html(html);
            }
        }
    });
}

/**通知公告 */
var getNews = () => {
    $.ajax({
        dataType: "json",
        type: "post",
        url: "../desktop/getNotices",
        success: function (json) {
            if (json.type == 1) {
                var data = json.resultdata;
                var html = "";
                if (data.length == 0) {
                   
                    html = ' <span class="gray">暂无通知公告 </span>';
                } else {
                    var title = data[0].title;
                    title = title.length > 30 ? title.substring(0, 30) : title;
                    //$(json).each(function (j, item) {
                    html = '  <span class="gray">' + title+'</span>';
                    html += '&nbsp;&nbsp;&nbsp;&nbsp;';
                    html += '<span style="color:#ffffff;">' + data[0].time+'</span>'
                    html += '&nbsp;&nbsp;&nbsp;&nbsp;';
                    //});
                }
                $("#notic_list").html(html);
            }
        }
    });
}

/**
 * 加载页面定时器刷新数据
 * @param {any} timer 刷新时间
 * @param {any} functionName 方法名
 */
var LoadInterval = (timer, functionName) => {
    let globalInterval = setInterval(functionName, timer);
}


//var IndexJs = function () {
//    var test = () => {
//        console.log("222");
//    }
//    var LoadInterval = (timer, functionName) => {
//        setInterval(functionName, timer);
//    }
//    return {
//        /**初始化 */
//        init: () => {

//        },
//        /**定时器*/
//        loadInterval: (timer, functionName) => {
//            LoadInterval(timer, functionName);
//        },
//        setVal: test()
//    };
//}();

$(function () {
    loadDeviceArea();//加载区域
    //加载事件
    loadAreaEvent();
    loadDeviceEvent();
    // HighRiskWork();
    //WorkMeeting();
    LoadInterval(5000, "LoadDeviceWatch()");//定时器
    //刷新最新的设备间联动监控
    LoadInterval(5000, "cameraplay()");//定时器
    LoadInterval(40000, "WorkMeeting()");//定时器
    LoadInterval(40000, "HighRiskWork()");//定时器
    LoadInterval(5000, "loadWaringData()");//加载预警中心数据
    LoadInterval(5000, "loadRealTimeWork()");//加载实时工作
    LoadInterval(5000, "getNews()");
});//加载通知公告

//判读是否是海康的图片路径不是则返回我们自己存储路径
function GetHikImgUrl(url) {
    if (url) {
        var start = url.indexOf("/Resource");
        if (start == 0) {//是
            return ".." + url;

        } else {//否
            console.log(0);
            return KMHikImgIp + url;
        }
    } else {
        return '';
    }
}