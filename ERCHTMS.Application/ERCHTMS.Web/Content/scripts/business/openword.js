//此处路径基于直接从文件表直接拿出的路径
function OpenWord(Url,recId) {
    var topUrl = top.contentPath;
    //用于判断文件是否存在
    //$.ajax({
    //    url: top.contentPath + "/OccupationalHealthManage/Occupatioalstaff/IsUrl?fileUrl=" + Url,
    //        type: "post",
    //        dataType: "text",
    //        async: false,
    //        success: function (data) {
    //            if (data == "True") {
    //                var linkUrl = top.contentPath + '/OccupationalHealthManage/Occupatioalstaff/OpenWord?fileUrl=' + Url + '&topUrl=' + topUrl;
    //                POBrowser.openWindowModeless(linkUrl, 'width=1200px;height=800px;')
    //            } else
    //            {
    //                dialogMsg('该文件已被删除', 0);
    //            }
    //        }
    //});

    $.SetForm({
        url: "../../SafetyLawManage/SafetyLaw/WordToPdf",
        param: { fileid: recId },
        success: function (data) {
            if (data == "0") {
                dialogMsg('该文件已不存在!', 0);
                return false;
            } else {
                var fileUrl = data.replace("~", top.contentPath);
                dialogContent({
                    id: "dlgFile",
                    title: '在线预览文件',
                    width: ($(top.window).width()-50) + "px",
                    height: ($(top.window).height()-100) +"px",
                    content: '<iframe src="' + top.contentPath + "/content/pdfjs/web/viewer.html?fileUrl=" + fileUrl+ '" style="height: 800px;width: 100%;border:0;"></iframe>',
                    btn: null,
                });
               
            }

        }
    })
  
}