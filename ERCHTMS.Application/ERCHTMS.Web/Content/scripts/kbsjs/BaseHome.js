



//初始化加载
$(function () {
    GetOutsourceUser();


});


function GetOutsourceUser() {
  

    //预警信息
    $.ajax({
        url: '../../KbsDeviceManage/SafeUserControl/GetUserRealtimeWarning?type=2',
        type: "POST",
        async: true,
        dataType: "Json",
        success: function (result) {
            var html = "<ul class=\"\" tabindex=\"1\" style=\"overflow: hidden;outline: none;\">";
            var no = 1;
            for (var i = 0; i < result.length; i++) {
                html += "<li><span class=\"titleNum active\">" + no++ + "</span>";
                html += "<span>" + result[i].WarningContent + "</span>";
                html += "<span class=\"buleColor2\">离线</span>";
                html += "</li>";
            }
            html += "</ul>";
            $("#RealtimeWarning").html(html);
        }
    });

   




}




























