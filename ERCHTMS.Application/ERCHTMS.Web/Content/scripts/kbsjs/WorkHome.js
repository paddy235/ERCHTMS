



//初始化加载
$(function () {
    GetOutsourceUser("本周");


});



function GetOutsourceUser(obj) {
    //作业异常数量
    $.ajax({
        url: '../../KbsDeviceManage/Safeworkcontrol/GetWorkWarningGroupJson?type=' + obj,
        type: "POST",
        async: false,
        dataType: "Json",
        success: function (result) {
            var html = "<ul class=\"\" tabindex=\"1\" style=\"overflow: hidden;outline: none;\">";
            var no = 1;
            for (var i = 0; i < result.length; i++) {
                html += "<li><span class=\"titleNum active\">" + no++ + "</span>";
                html += "<span>" + result[i].deptname + "</span>";
                html += "<span class=\"buleColor3\">" + result[i].num + "</span>";
                html += "</li>";
            }
            html += "</ul>";
            $("#ChartTable").html(html);
        }
    });


    //作业预警
    $.ajax({
        url: '../../KbsDeviceManage/SafeUserControl/GetUserRealtimeWarning?type=0',
        type: "POST",
        async: true,
        dataType: "Json",
        success: function (result) {
            var html = "<ul class=\"\" tabindex=\"1\" style=\"overflow: hidden;outline: none;\">";
            var no = 1;
            for (var i = 0; i < result.length; i++) {
                html += "<li><span class=\"titleNum active\">" + no++ + "</span>";
                html += "<span  class=\"buleColor2\" >（" + result[i].LiableName + "）" + result[i].WarningContent + "</span>";
                //html += " <span class=\"buleColor2\">离线</span>";
                html += "</li>";
            }
            html += "</ul>";
            $("#UserRealtimeWarning").html(html);
        }
    });

    //作业实时分布统计表
    $.ajax({
        url: '../../KbsDeviceManage/Safeworkcontrol/GetWorkRealTimeTableJson',
        type: "POST",
        async: true,
        dataType: "Json",
        success: function (result) {
            var html = "";
            var no = 1;
            var znum = 0;
            for (var i = 0; i < result.length; i++) {
                znum = result[i].OnNum;
                html += "<tr>";
                html += "<td>" + result[i].Name + "</td>";
                html += "<td><a  href=\"javascript:ShowWorkInfo('" + encodeURIComponent(result[i].DistrictCode) + "',0)\">" + result[i].Num + "</a></td>";
                html += "<td>" + result[i].OnProportion + "</td>";
                html += "</tr>";
            }
            $("#ZNum0").html("总数：" + znum);
            $("#BaseStationbody").html(html);
        }
    });





}




























