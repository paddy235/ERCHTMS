
//删除附件
function removeFile(filename, recId, obj) {
    var dlg = $.ConfirmAjax({
        msg: "确定删除吗？",
        url: top.contentPath + "/PublicInfoManage/ResourceFile/RemoveFile",
        param: { recId: recId, fileName: filename },
        success: function (data) {
            var pObj;
            if (!!$(obj).parent().parent().parent().parent().parent()) {
                pObj = $(obj).parent().parent().parent().parent().parent();
                $(pObj).viewer("destroy");
            }
            $(obj).parent().parent().remove();
            if (!!pObj) {
                $(pObj).viewer({ url: "data-original" });
            }
            if (window.back != undefined) {
                window.cancelBack();
            }
        }
    })
}
function openFile(filename) {
    var fileobj = null;
    if (document.getElementById("filelink") == null) {
        fileobj = document.createElement("a");
        fileobj.id = "filelink";
    }
    else {
        fileobj = document.getElementById("filelink");
    }
    fileobj.src = filename;
    $(fileobj).click();
}
function onlineFile(obj, filename, url, path, recId) {

    var exts = ".jpg,.bmp,.gif,.jpeg,.txt,.png";
    var ext = filename.substring(filename.lastIndexOf(".")).toLowerCase();
    var html = "<div style='padding:10px;margin:10px;'>";

    //预览图片
    if (exts.indexOf(ext) >= 0) {
        $(obj).attr("href", url);
        $(obj).attr("target", "_blank");
        $(obj).removeAttr("onclick");
    }
    else if (".ppt,.pptx,.xls,.xlsx".indexOf(ext) >= 0) {     //在线预览office
        window.location.href = url;
        //<a href="' + top.contentPath + '/PublicInfoManage/ResourceFile/DownloadFile?keyValue=-1&filename=' + encodeURI(file.filename) + '&recId=' + keyValue + '\" target="_blank" style="cursor:pointer"  title="下载文件"><i class="fa fa-download"></i></a>
        //var linkUrl = top.contentPath + '/OccupationalHealthManage/Occupatioalstaff/OpenWord?fileUrl=' + path + '&topUrl=' + top.contentPath;
        //POBrowser.openWindowModeless(linkUrl, 'width=' + ($(top.window).width() - 100) + 'px;height=' + ($(top.window).height() - 80) + 'px;');
    }
     //预览office  word
    else if (".doc,.docx".indexOf(ext) >= 0)
    {
        //预览本地文件
        $.SetForm({
            url: "../../SafetyLawManage/SafetyLaw/FileToPdf",
            param: { url: path },
            success: function (data) {
                if (!!data && data != "0")
                {
                    var fileUrl = data.replace("~", top.contentPath);
                    var idx = dialogOpen({
                        id: "pdf",
                        title: "在线预览pdf",
                        url: "/content/pdfjs/web/viewer.html?fileUrl=" + fileUrl,
                        width: ($(top.window).width()-100) + "px",
                        height: $(top.window).height() + "px",
                        type: 2,
                        btns: 1,
                        btn: ["关闭"],
                        callBack: function (iframeId) {
                            top.layer.close(idx);
                        }
                    });
                }
       
            }
        })
    }
        //预览pdf
    else if (".pdf".indexOf(ext) >= 0) {
        var idx = dialogOpen({
            id: "pdf",
            title: "在线预览pdf",
            url: "/content/pdfjs/web/viewer.html?fileUrl=" + url,
            width: ($(top.window).width() - 80) + "px",
            height: $(top.window).height() + "px",
            type: 2,
            btns: 1,
            btn: ["关闭"],
            callBack: function (iframeId) {
                top.layer.close(idx);
            }
        });
    }
        //播放视频
    else if (".mp4,.webm,.ogv".indexOf(ext) >= 0) {
        var idx = dialogOpen({
            id: "Sound",
            title: "播放视频",
            url: "/content/view/video.html?filename=" + url,
            width: "945px",
            height: "500px",
            type: 2,
            btns: 1,
            btn: ["关闭"],
            callBack: function (iframeId) {
                top.layer.close(idx);
            }
        });
    }
        //播放音频
    else if (".mp3,.ogg".indexOf(ext) >= 0) {
        if (".mp3".indexOf(ext) >= 0) {
            html = '<audio controls><source src="' + url + '" type="audio/mpeg">您的浏览器不支持该音频格式。</audio></div>';
        } else {
            html = '<audio controls><source src="' + url + '" type="audio/ogg">您的浏览器不支持该音频格式。</audio></div>';
        }
        var idx = dialogOpen({
            id: "Auto",
            title: "播放音频",
            url: "/content/view/audio.html?filename=" + url,
            width: "400px",
            height: "200px",
            type: 2,
            btns: 1,
            btn: ["关闭"],
            callBack: function (iframeId) {
                top.layer.close(idx);
            }
        });

    }

}
//在线预览word,excel,ppt,pdf,图片
function doFile(obj, filepath, recId) {
  
    var filename = $(obj).text();
    var url = "";
    var path = "";
    if ($(obj).attr("url") == undefined) {
        $.ajax({
            type: "get",
            url: top.contentPath + "/PublicInfoManage/ResourceFile/GetFilePath",
            data: { recId: recId, filename: filename },
            success: function (data) {
                var json = $.parseJSON(data);
                if (json.type == 1) {
                    url = json.resultdata;
                    path = json.message;
                    $(obj).attr("url", url);
                    $(obj).attr("path", path);
                    onlineFile(obj, filename, url, path, recId);
                }
            }
        });

    } else {
        url = $(obj).attr("url");
        path = $(obj).attr("path");
        onlineFile(obj, filename, url, path, recId);
    }

}
//查看大图
function show(imgObj) {

    var path = $(imgObj).parent().attr("path");
    if (path != undefined) {
        path = top.contentPath + path.substring(1, path.length);
    } else {
        path = imgObj.src;
    }
    var content = "<div style='margin:20px; text-align:center;'><img src='" + path + "' /></div>";
    var dlg = dialogContent({
        title: "查看大图",
        content: content,
        height: "500px",
        width: "600px",
        btn: ["关闭"],
        callBack: function () {
            top.layer.close(dlg);
        }
    });
}
var $queue;
var file_upload = {
    //绑定记录相关的所有附件
    //files:附件信息,isDel:是否允许删除，isImage：是否是图片,keyValue:业务记录Id,el:dom容器Id,不要#,isUpload:是否允许上传文件
    bind: function (files, isDel, isImage, keyValue, el, isUpload) {
        var $queue1 = $("#" + el).find(".filelist").eq(0).get(0);
        if ($queue1 == null || $queue1 == undefined) {
            $queue1 = isImage ? $('<ul class="filelist"></ul>').appendTo($("#" + el).find('.queueList')) : $('<table class="filelist table1" cellpadding="0" cellspacing="0"  border="1" style="width:90%;margin-top:5px;"><tr style="font-weight: bold;"><td style="text-align:center;width:200px;">文件名称</td><td style="text-align:center;width:100px;">上传时间</td><td  style="text-align:center;width:70px;">操作</td></tr></table>').appendTo($("#" + el).find('.queueList'));
        }

        if (files.length > 0) {
            $("#" + el).find(".filelist").eq(0).find("tr.row1").remove();
            $(files).each(function (i, file) {

                var $li;
                if (isImage) {
                    $li = $('<li>' +
                    '<p class="imgWrap"><img title="点击查看大图" alt="点击查看大图" data-original="' + "/" + file.filepath.substring(1, file.filepath.length) + '" src="' + "/" + file.filepath.substring(1, file.filepath.length) + '" /><span class="imgName" style="display:none;">' + file.filename + '</span>' + '</p>' +
                    '<p class="progress"><span></span></p></li>');
                    if (isDel) {
                        $btns = $('<div class="file-panel">' +
                       '<span class="cancel">删除</span>').appendTo($li);

                        var $btns = $('<div class="file-panel"><span class="cancel">删除</span>').appendTo($li);
                        $li.on('mouseover', function () {
                            $btns.stop().animate({ height: 30 });
                        });

                        $li.on('mouseout', function () {
                            $btns.stop().animate({ height: 0 });
                        });

                        $btns.on('click', 'span', function () {
                            var index = $(this).index(),
                                deg;
                            switch (index) {
                                case 0:
                                    removeFile(file.filename, keyValue, this);
                                    return;
                            }

                        });
                    }
                } else {
                    var html = "";
                    var exts = ".jpg,.bmp,.gif,.jpeg,.txt,.png";
                    var ext = file.filename.substring(file.filename.lastIndexOf(".")).toLowerCase();
                    var url = window.location.protocol + "//" + window.location.host + file.filepath.replace("~", "");
                    if (exts.indexOf(ext) >= 0) {
                        html = '<tr><td align="left" class="filename"><a style="cursor:pointer; text-decoration:underline;" path="' + file.filepath + '" url="' + url + '" href="' + url + '" target="_blank">' + file.filename + '</a>';
                    }
                    else {
                        if (".pdf,.ppt,.pptx,.xls,.xlsx,.doc,.docx,.mp3,.mp4,.webm,.ogv,.ogg".indexOf(ext) < 0) {
                            url = top.contentPath + "/PublicInfoManage/ResourceFile/DownloadFile?keyValue=-1&filename=" + file.filename + "&recId=" + keyValue;
                            html = '<tr><td align="left" class="filename"><a style="cursor:pointer; text-decoration:underline;" path="' + file.filepath + '" url="' + url + '" href="' + url + '" target="_blank">' + file.filename + '</a>';
                        }
                        else {
                            html = '<tr><td align="left" class="filename"><a onclick="doFile(this,\'' + file.filename + '\',\'' + keyValue + '\')" style="cursor:pointer; text-decoration:underline;" path="' + file.filepath + '" url="' + url + '">' + file.filename + '</a>';
                        }
                    }
                    html += '</td><td>' + file.createdate + '</td><td><a href="' + top.contentPath + '/PublicInfoManage/ResourceFile/DownloadFile?keyValue=-1&filename=' + encodeURI(file.filename) + '&recId=' + keyValue + '\" target="_blank" style="cursor:pointer"  title="下载文件"><i class="fa fa-download"></i></a>';
                    if (isDel) {
                        html += '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style="cursor:pointer" onclick="removeFile(\'' + file.filename + '\',\'' + keyValue + '\',this)" title="删除文件"><i class="fa fa-trash-o"></i></a>';
                    }

                    html += "</td></tr>";
                    $li = $(html);
                }
                $li.appendTo($queue1);

            });

            //加载图片预览功能
            $("#" + el).viewer({ url: "data-original" });
        }
        if (isUpload != undefined) {
            if (!isUpload) {
                $("#" + el).children().eq(0).children().eq(0).remove();
            }
        }

    },
    //绑定记录相关的所有附件
    //isUpload:是否允许上传文件,isDel:是否允许删除，isImage：是否是图片,keyValue:业务记录Id,el:dom容器Id,不要#,默认为uploader
    bindFiles: function (isDel, isImage, keyValue, el, isUpload, append) {
        el = el == undefined ? "uploader" : el;
        var $queue1 = $("#" + el).find(".filelist").eq(0).get(0);
        if ($queue1 == null || $queue1 == undefined) {
            $queue1 = isImage ? $('<ul class="filelist"></ul>') : $('<table class="filelist table1" cellpadding="0" cellspacing="0"  border="1" style="width:98%;margin-top:5px;"><tr style="font-weight: bold;"><td style="text-align:center;width:200px;">文件名称</td><td style="text-align:center;width:100px;">上传时间</td><td  style="text-align:center;width:70px;">操作</td></tr></table>');
            $queue1.appendTo($("#" + el).find('.queueList'));
        }

        //$.ajaxSetup({
        //    async: false
        //});
        $.post(top.contentPath + "/ResourceFile/GetFilesByRecId", { recId: keyValue }, function (data) {
            var files = eval("(" + data + ")");
            if (files.length > 0) {
                if (append != true)
                    $("#" + el).find(".filelist").eq(0).find("tr.row1").remove();
                $(files).each(function (i, file) {
                    var $li;
                    if (isImage) {
                        $li = $('<li>' +
                        '<p class="imgWrap"><img title="点击查看大图" alt="点击查看大图"  data-original="' + "/" + file.filepath.substring(1, file.filepath.length) + '" src="' + "/" + file.filepath.substring(1, file.filepath.length) + '"  /><span class="imgName" style="display:none;">' + file.filename + '</span>' + '</p>' +
                        '<p class="progress"><span></span></p></li>');
                        if (isDel) {
                            $btns = $('<div class="file-panel">' +
                           '<span class="cancel">删除</span>').appendTo($li);

                            var $btns = $('<div class="file-panel"><span class="cancel">删除</span>').appendTo($li);
                            $li.on('mouseover', function () {
                                $btns.stop().animate({ height: 30 });
                            });

                            $li.on('mouseout', function () {
                                $btns.stop().animate({ height: 0 });
                            });

                            $btns.on('click', 'span', function () {
                                var index = $(this).index(),
                                    deg;
                                switch (index) {
                                    case 0:
                                        removeFile(file.filename, keyValue, this);
                                        return;
                                }

                            });
                        }
                    } else {
                        var html = "";
                        var exts = ".jpg,.bmp,.gif,.jpeg,.txt,.png";
                        var ext = file.filename.substring(file.filename.lastIndexOf(".")).toLowerCase();
                      
                        var url = window.location.protocol + "//" + window.location.host + file.filepath.replace("~", "");
                        if (exts.indexOf(ext) >= 0) {
                            html = '<tr><td align="left" class="filename"><a style="cursor:pointer; text-decoration:underline;" path="' + file.filepath + '" url="' + url + '" href="' + url + '" target="_blank">' + file.filename + '</a>';
                        }
                        else {
                            if (".pdf,.ppt,.pptx,.xls,.xlsx,.doc,.docx,.mp3,.mp4,.webm,.ogv,.ogg".indexOf(ext) < 0) {
                                url = top.contentPath + "/PublicInfoManage/ResourceFile/DownloadFile?keyValue=-1&filename=" + file.filename + "&recId=" + keyValue;
                                html = '<tr><td align="left" class="filename"><a style="cursor:pointer; text-decoration:underline;" path="' + file.filepath + '" url="' + url + '" href="' + url + '" target="_blank">' + file.filename + '</a>';
                            }
                            else {
                                html = '<tr><td align="left" class="filename"><a onclick="doFile(this,\'' + file.filename + '\',\'' + keyValue + '\')" style="cursor:pointer; text-decoration:underline;" path="' + file.filepath + '" url="' + url + '">' + file.filename + '</a>';
                            }
                        }
                        html += '</td><td>' + file.createdate + '</td><td><a href="' + top.contentPath + '/PublicInfoManage/ResourceFile/DownloadFile?keyValue=-1&filename=' + encodeURI(file.filename) + '&recId=' + keyValue + '\" target="_blank" style="cursor:pointer"  title="下载文件"><i class="fa fa-download"></i></a>';
                        if (isDel) {
                            html += '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style="cursor:pointer" onclick="removeFile(\'' + file.filename + '\',\'' + keyValue + '\',this)" title="删除文件"><i class="fa fa-trash-o"></i></a>';
                        }

                        html += "</td></tr>";
                        $li = $(html);
                        //if (!isDel) {
                        //    $li = $('<tr class="row1"><td align="left" class="filename">' + file.filename + '</td><td>' + file.createdate + '</td><td><a href="' + top.contentPath + '/PublicInfoManage/ResourceFile/DownloadFile?keyValue=-1&filename=' + encodeURIComponent(file.filename) + '&recId=' + keyValue + '\" target="_blank" style="cursor:pointer"  title="下载文件"><i class="fa fa-download"></i></a></td></tr>');

                        //} else {
                        //    $li = $('<tr class="row1"><td align="left" class="filename">' + file.filename + '</td><td>' + file.createdate + '</td><td><a href="' + top.contentPath + '/PublicInfoManage/ResourceFile/DownloadFile?keyValue=-1&filename=' + encodeURIComponent(file.filename) + '&recId=' + keyValue + '\" target="_blank" style="cursor:pointer"  title="下载文件"><i class="fa fa-download"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style="cursor:pointer" onclick="removeFile(\'' + file.filename + '\',\'' + keyValue + '\',this)" title="删除文件"><i class="fa fa-trash-o"></i></a></td></tr>');
                        //}
                    }

                    $li.appendTo($queue1);

                });

                //加载图片预览功能
                $("#" + el).viewer({ url: "data-original" });
            }
            if (isUpload != undefined) {
                if (!isUpload && append != true) {
                    $("#" + el).children().eq(0).children().eq(0).remove();
                }
            }
        });
    },
    //上传插件初始化
    init: function (defaults) {
        var options = {
            el: '#uploader', //容器Id
            extensions: 'ico,icon,raw,jpg,jpeg,gif,bmp,png,psd', //文件扩展名
            isImage: true,  //是否只是上传图片
            fileDir: 'ht/images',  //文件存储目录，相对于Resource
            isDate: true,  //是否新建日期目录存储文件
            fileNumLimit: 20, //允许上传的文件个数
            fileSizeLimit: 100 * 1024 * 1024, //上传文件总大小（KB）
            fileSingleSizeLimit: 50 * 1024 * 1024, //允许单个上传文件的大小（KB）
            multiple: false, //是否可以同时选择多个文件
            width: 0,
            height:0
        };
        $.extend(options, defaults);
        if (options.extensions == '') {
            options.extensions = 'doc,docx,xls,xlsx,zip,rar,jpg,png,gif,bmp,txt,ppt,pptx,pdf,mp3,mp4,avi';
        }
        options.extensions = options.isImage ? "ico,icon,raw,jpg,jpeg,gif,bmp,png,psd" : options.extensions;
        var $wrap = $(options.el),
       // 文件容器
        $queue = options.isImage ? $('<ul class="filelist"></ul>').appendTo($wrap.find('.queueList')) : $('<table class="filelist table1" cellpadding="0" cellspacing="0"  border="1" style="width:98%;margin-top:5px;"><tr style="font-weight: bold;"><td style="text-align:center;width:200px;">文件名称</td><td style="text-align:center;width:100px;">上传时间</td><td  style="text-align:center;width:70px;">操作</td></tr></table>').appendTo($wrap.find('.queueList')),

        // 状态栏，包括进度和控制按钮
        $statusBar = $wrap.find('.statusBar'),

        // 文件总体选择信息。
         $info = $statusBar.find('.info'),
        // 没选择文件之前的内容。
         $placeHolder = $wrap.find('.placeholder'),
        // 总体进度条
         $progress = $statusBar.find('.progress').hide(),

        // 添加的文件数量
         fileCount = 0,

         successCount = 0,

        //添加的文件总大小
         fileSize = 0,

        //优化retina, 在retina下这个值是2
          ratio = window.devicePixelRatio || 1,

        // 缩略图大小
         thumbnailWidth = 110 * ratio,
         thumbnailHeight = 110 * ratio,

        // 可能有pedding, ready, uploading, confirm, done.
         state = 'pedding',

        // 所有文件的进度信息，key为file id
          percentages = {},

      supportTransition = (function () {
          var s = document.createElement('p').style,
              r = 'transition' in s ||
                 'WebkitTransition' in s ||
                 'MozTransition' in s ||
                 'msTransition' in s ||
                 'OTransition' in s;
          s = null;
          return r;
      })();
        if (!WebUploader.Uploader.support()) {
            alert('Web Uploader 不支持您的浏览器！如果你使用的是IE浏览器，请尝试升级 flash 播放器');
            throw new Error('WebUploader does not support the browser you are using.');
        }
        // 实例化
        var uploader = WebUploader.create({
            auto: true,
            pick: {
                id: $(options.el).find('.filePicker'),
                innerHTML: '点击选择文件',
                multiple: options.multiple
            },
            dnd: options.el + ' .queueList', //拖拽容器
            paste: document.body,

            accept: {
                title: '选择文件',
                extensions: options.extensions,
                mimeTypes: options.isImage ? 'image/*' : '*/*'
            },
            // swf文件路径（在IE9以下浏览器中必须设置且要保证路径正确）
            swf: top.contentPath + '/content/scripts/plugins/webuploader/Uploader.swf',
            disableGlobalDnd: true,
            chunked: false,
            //服务端文件处理地址
            server: top.contentPath + '/Utility/PostFile?fileExts=' + options.extensions+ '&recId=' + options.keyValue + "&filePath=" + options.fileDir + "&isDate=" + (options.isDate ? 1 : 0),
            fileNumLimit: options.fileNumLimit,
            fileSizeLimit: options.fileSizeLimit,
            fileSingleSizeLimit: options.fileSingleSizeLimit,
            multiple: options.multiple
        });

        // 当有文件添加进来时执行，负责view的创建
        function addFile(file) {
            
            var $li;
            var $imgWrap;
            if (options.isImage) {
                $li = $('<li>' +
               '<p class="title">' + file.name + '</p>' +
               '<p class="imgWrap" id="' + file.id + '" title="点击查看大图" alt="点击查看大图"  ><span class="imgName" style="display:none;">' + file.name + '</span></p>' +
               '<p class="progress"><span></span></p></li>');

                $btns = $('<div class="file-panel">' +
                '<span class="cancel">删除</span></div>').appendTo($li);
            } else {
                $li = $('<tr class="row' + file.id + '" style="display:none;" fid="' + file.id + '"><td class="filename"><a onclick="doFile(this,\'' + options.keyValue + '\',\'' + file.filepath + '\')" style="cursor:pointer; text-decoration:underline;">' + file.name + '</a></td><td>' + formatDate(new Date(), 'yyyy-MM-dd hh:mm:ss') + '</td><td><a href="' + top.contentPath + '/PublicInfoManage/ResourceFile/DownloadFile?keyValue=-1&filename=' + encodeURIComponent(file.name) + '&recId=' + options.keyValue + '\" target="_blank" style="cursor:pointer"  title="下载文件"><i class="fa fa-download"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style="cursor:pointer"  title="删除文件" class="delFile"><i class="fa fa-trash-o"></i></a></td></tr>');

                $btns = $li.find(".delFile");

            }

            $li.appendTo($queue);
            $prgress = $li.find('p.progress span'),
            $imgWrap = $li.find('p.imgWrap'),
            //$imgWrap = $queue.find('#' + file.id).eq(0),
            $info = $('<p class="error"></p>');
            showError = function (code) {
                switch (code) {
                    case 'exceed_size':
                        text = '文件大小超出';
                        break;

                    case 'interrupt':
                        text = '上传暂停';
                        break;

                    default:
                        text = '上传失败，请重试';
                        break;
                }

                $info.text(text).appendTo($li);
            };

            if (file.getStatus() === 'invalid') {
                showError(file.statusText);
            } else {
                percentages[file.id] = [file.size, 0];
                file.rotation = 0;
            }

            file.on('statuschange', function (cur, prev) {
               
                if (prev === 'progress') {
                    $prgress.hide().width(0);
                } else if (prev === 'queued') {
                    $li.off('mouseenter mouseleave');
                }
                // 成功
                if (cur === 'error' || cur === 'invalid') {
                    console.log(file.statusText);
                    showError(file.statusText);
                    percentages[file.id][1] = 1;
                } else if (cur === 'interrupt') {
                    showError('interrupt');
                } else if (cur === 'queued') {
                    percentages[file.id][1] = 0;
                } else if (cur === 'progress') {
                    $info.remove();
                    $prgress.css('display', 'block');
                } else if (cur === 'complete') {
                    uploader.makeThumb(file, function (error, src) {
                        if (error) {
                            return;
                        }
                        var imgUrl = $imgWrap.attr("path");
                        if (!!imgUrl) {
                            imgUrl = top.contentPath + imgUrl.substring(1, imgUrl.length);
                        }
                        var img = $('<img src="' + src + '" data-original="' + imgUrl + '" id="img' + file.id + '"><span class="imgName" style="display:none;">' + file.name + '</span>');
                        $imgWrap.empty().append(img);
                        if (!!$imgWrap.parent().parent().parent().parent()) {
                            //先清除，再添加图片;
                            var uploaderId = $imgWrap.parent().parent().parent().parent().attr("id");
                            $("#" + uploaderId).viewer("destroy");
                            $("#" + uploaderId).viewer({ url: "data-original" });
                        }
                    }, thumbnailWidth, thumbnailHeight);
                    percentages[file.id] = [file.size, 0];
                    file.rotation = 0;
                    successCount++;

                }

                $li.removeClass('state-' + prev).addClass('state-' + cur);
            });
            if (options.isImage) {
                $li.on('mouseover', function () {
                    $li.find(".file-panel").stop().animate({ height: 30 });

                });

                $li.on('mouseout', function () {
                    $li.find(".file-panel").stop().animate({ height: 0 });
                });

                $btns.on('click', 'span', function () {
                    var index = $(this).index(),
                        deg;
                    switch (index) {
                        case 0:
                            uploader.removeFile(file);
                            removeFile(file.name, options.keyValue, this);
                            return;
                        case 1:
                            file.rotation += 90;
                            break;

                        case 2:
                            file.rotation -= 90;
                            break;
                    }

                    if (supportTransition) {
                        deg = 'rotate(' + file.rotation + 'deg)';
                        $imgWrap.css({
                            '-webkit-transform': deg,
                            '-mos-transform': deg,
                            '-o-transform': deg,
                            'transform': deg
                        });
                    } else {
                        $imgWrap.css('filter', 'progid:DXImageTransform.Microsoft.BasicImage(rotation=' + (~~((file.rotation / 90) % 4 + 4) % 4) + ')');
                    }
                });
            } else {
                //删除附件
                $li.find(".delFile").bind("click", function () {
                    var _self = $(this);
                    var dlg = $.ConfirmAjax({
                        msg: "确定删除吗？",
                        url: top.contentPath + "/PublicInfoManage/ResourceFile/RemoveFile",
                        param: { recId: options.keyValue, fileName: file.name },
                        success: function (data) {
                            uploader.removeFile(file);
                            _self.parent().parent().remove();
                            if (window.cancelBack != undefined) {
                                window.cancelBack();
                            }
                        }
                    });

                });

            }

        }

        function updateTotalProgress() {
            var loaded = 0,
                total = 0,
                spans = $progress.children(),
                percent;

            $.each(percentages, function (k, v) {
                total += v[0];
                loaded += v[0] * v[1];
            });

            percent = total ? loaded / total : 0;

            spans.eq(0).text(Math.round(percent * 100) + '%');
            spans.eq(1).css('width', Math.round(percent * 100) + '%');
            updateStatus();
        }

        function updateStatus() {
            var text = '', stats;

            //if (state === 'ready') {
            //    text = '选中' + fileCount + '个文件，共' +
            //            WebUploader.formatSize(fileSize) + '。';
            //} else if (state === 'confirm') {
            stats = uploader.getStats();
            //    if (stats.uploadFailNum) {
            //        text = '已成功上传' + stats.successNum + '个文件，' +
            //            stats.uploadFailNum + '个文件上传失败，<a class="retry" href="#">重新上传</a>'
            //    }

            //} else {
            stats = uploader.getStats();
            //    text = '共' + fileCount + '个（' +
            //            WebUploader.formatSize(fileSize) +
            //            '），已上传' + stats.successNum + '个';

            //    if (stats.uploadFailNum) {
            //        text += '，失败' + stats.uploadFailNum + '个';
            //    }
            //}

            //$info.html(text);
        }

        function setState(val) {
            var file, stats;

            if (val === state) {
                return;
            }
            state = val;
            switch (state) {
                case 'pedding':
                    $placeHolder.removeClass('element-invisible');
                    $queue.parent().removeClass('filled');
                    $queue.hide();
                    $statusBar.addClass('element-invisible');
                    uploader.refresh();
                    break;
                case 'ready':
                    $placeHolder.addClass('element-invisible');
                    $queue.parent().addClass('filled');
                    $queue.show();
                    $statusBar.removeClass('element-invisible');
                    uploader.refresh();
                    break;
                case 'uploading':
                    $progress.show();
                    break;
                case 'paused':
                    $progress.show();
                    break;
                case 'confirm':
                    $progress.hide();
                    stats = uploader.getStats();
                    if (stats.successNum && !stats.uploadFailNum) {
                        setState('finish');
                        return;
                    }
                    break;
                case 'finish':
                    stats = uploader.getStats();
                    if (stats.successNum) {
                        //dialogMsg('上传成功！', 0);
                    } else {
                        // 没有成功的图片，重设
                        state = 'done';
                        location.reload();
                    }
                    break;
            }

            updateStatus();
        }
        uploader.onBeforeFileQueued = function (file) {

            var isOk = true;
            var arr;
            if (options.isImage) {
                arr = $(options.el).find(".imgName");
            } else {
                arr = $(options.el).find(".filename");
            }

            if (arr.length >= options.fileNumLimit) {
                dialogMsg("对不起，上传文件总数量超过限制！", 0);
                isOk = false;
                return false;
            }
            else {
                arr.each(function (i, dom) {
                    if ($(dom).text() == file.name) {
                        isOk = false;
                        return false;
                    }
                });

            }
            if (!isOk) {
                dialogMsg("此文件已经上传过了！", 0);
                return false;
            }

            return isOk;
        }
        uploader.onUploadProgress = function (file, percentage) {
            var $li = $('#' + file.id),
                $percent = $li.find('.progress span');

            $percent.css('width', percentage * 100 + '%');
            percentages[file.id][1] = percentage;
            updateTotalProgress();
        };

        uploader.onFileQueued = function (file) {
            fileCount++;
            fileSize += file.size;

            if (fileCount === 1) {
                //$placeHolder.addClass('element-invisible');
                $statusBar.show();
            }

            addFile(file);
            setState('ready');
            updateTotalProgress();
        };
        uploader.onUploadSuccess = function (file, response) {
            if (options.isImage) {
                if (response.type == 3) {
                    //alert(response.message);

                    uploader.removeFile(file);
                    $(options.el).find(".filelist li:last").remove();
                    dialogMsg(response.message, 0);

                    return false;
                }
            }
            if (response.resultdata != null && response.resultdata.length>0) {
                $(options.el).find("tr[fid='" + file.id + "']").show();
                $(options.el).find("tr[fid='" + file.id + "']").attr("path", response.resultdata);
                $(options.el).find("p[id='" + file.id + "']").attr("path", response.resultdata);

                var exts = ".jpg,.bmp,.gif,.jpeg,.txt,.png";
                var ext = file.name.substring(file.name.lastIndexOf(".")).toLowerCase();
                var fileobj = $(options.el).find("tr[fid='" + file.id + "']").eq(0).find("a").eq(0);
                fileobj.attr("path", response.resultdata);
                var url = window.location.protocol + "//" + window.location.host + top.contentPath + response.resultdata.replace("~", "");
                fileobj.attr("url", url);
                if (exts.indexOf(ext) >= 0) {
                    fileobj.removeAttr("onclick");
                    fileobj.attr("href", url);
                    fileobj.attr("target", "_blank");
                }
                else {
                    if (".pdf,.ppt,.pptx,.xls,.xlsx,.doc,.docx,.mp3,.mp4,.webm,.ogv,.ogg".indexOf(ext) < 0) {
                        fileobj.removeAttr("onclick");
                        fileobj.attr("href", top.contentPath + "/PublicInfoManage/ResourceFile/DownloadFile?keyValue=-1&filename=" + file.name + "&recId=" + options.keyValue);
                        fileobj.attr("target", "_blank");
                    }
                }
                dialogMsg('上传成功！', 0);
                if (window.callBack != undefined) {
                    window.callBack(file.name, file.ext);
                }
            }
           
        };
        //uploader.onFileDequeued = function (file) {

        //    fileCount--;
        //    fileSize -= file.size;

        //    if (!fileCount) {
        //        setState('pedding');
        //    }
        //    //addFile(file);
        //removeFile(file);
        //    updateTotalProgress();

        //};

        uploader.on('all', function (type) {
            var stats;
            switch (type) {
                case 'uploadFinished':
                    setState('confirm');
                    if (!!uploader.id) {
                        //先清除，再添加图片;
                        $("#" + uploader.id).viewer("destroy");
                        $("#" + uploader.id).viewer({ url: "data-original" });
                    }
                    break;

                case 'startUpload':
                    setState('uploading');
                    break;

                case 'stopUpload':
                    setState('paused');
                    break;

            }
        });
        uploader.onError = function (code) {

            if (code == "F_DUPLICATE") {
                dialogMsg("此文件已经上传过了！", 0);
                uploader.reset();
                return false;
            }
            if (code == "Q_EXCEED_NUM_LIMIT") {
                dialogMsg("对不起，上传文件数据超过限制！", 0);
                uploader.reset();
                return false;
            }
            if (code == "Q_EXCEED_SIZE_LIMIT" || code == "F_EXCEED_SIZE") {
                dialogMsg("对不起，上传文件总大小超过限制！", 0);
                uploader.reset();
                return false;
            }
            if (code == "Q_TYPE_DENIED") {
                dialogMsg("对不起，不能上传空文件或不支持上传此文件格式（允许上传的文件格式为：" + options.extensions + ")", 0);
                uploader.reset();
                return false;
            }
        };
        updateTotalProgress();
    },
    //批量上传插件初始化（上传附件绑定多个ID,id用逗号分割）
    initAll: function (defaults) {
        var options = {
            el: '#uploader', //容器Id
            extensions: 'ico,icon,raw,jpg,jpeg,gif,bmp,png,psd', //文件扩展名
            isImage: true,  //是否只是上传图片
            fileDir: 'ht/images',  //文件存储目录，相对于Resource
            isDate: true,  //是否新建日期目录存储文件
            fileNumLimit: 20, //允许上传的文件个数
            fileSizeLimit: 100 * 1024 * 1024, //上传文件总大小（KB）
            fileSingleSizeLimit: 50 * 1024 * 1024, //允许单个上传文件的大小（KB）
            multiple: false, //是否可以同时选择多个文件
        };
        $.extend(options, defaults);
        if (options.extensions == '') {
            options.extensions = 'doc,docx,xls,xlsx,zip,rar,jpg,png,gif,bmp,txt,ppt,pptx,pdf,mp3,mp4,avi';
        }
        options.extensions = options.isImage ? "ico,icon,raw,jpg,jpeg,gif,bmp,png,psd" : options.extensions;
        var $wrap = $(options.el),
            // 文件容器
            $queue = options.isImage ? $('<ul class="filelist"></ul>').appendTo($wrap.find('.queueList')) : $('<table class="filelist table1" cellpadding="0" cellspacing="0"  border="1" style="width:98%;margin-top:5px;"><tr style="font-weight: bold;"><td style="text-align:center;width:200px;">文件名称</td><td style="text-align:center;width:100px;">上传时间</td><td  style="text-align:center;width:70px;">操作</td></tr></table>').appendTo($wrap.find('.queueList')),

            // 状态栏，包括进度和控制按钮
            $statusBar = $wrap.find('.statusBar'),

            // 文件总体选择信息。
            $info = $statusBar.find('.info'),
            // 没选择文件之前的内容。
            $placeHolder = $wrap.find('.placeholder'),
            // 总体进度条
            $progress = $statusBar.find('.progress').hide(),

            // 添加的文件数量
            fileCount = 0,

            successCount = 0,

            //添加的文件总大小
            fileSize = 0,

            //优化retina, 在retina下这个值是2
            ratio = window.devicePixelRatio || 1,

            // 缩略图大小
            thumbnailWidth = 110 * ratio,
            thumbnailHeight = 110 * ratio,

            // 可能有pedding, ready, uploading, confirm, done.
            state = 'pedding',

            // 所有文件的进度信息，key为file id
            percentages = {},

            supportTransition = (function () {
                var s = document.createElement('p').style,
                    r = 'transition' in s ||
                        'WebkitTransition' in s ||
                        'MozTransition' in s ||
                        'msTransition' in s ||
                        'OTransition' in s;
                s = null;
                return r;
            })();
        if (!WebUploader.Uploader.support()) {
            alert('Web Uploader 不支持您的浏览器！如果你使用的是IE浏览器，请尝试升级 flash 播放器');
            throw new Error('WebUploader does not support the browser you are using.');
        }
        // 实例化
        var uploader = WebUploader.create({
            auto: true,
            pick: {
                id: $(options.el).find('.filePicker'),
                innerHTML: '点击选择文件',
                multiple: options.multiple
            },
            dnd: options.el + ' .queueList', //拖拽容器
            paste: document.body,

            accept: {
                title: '选择文件',
                extensions: options.extensions,
                mimeTypes: options.isImage ? 'image/*' : '*/*'
            },
            // swf文件路径（在IE9以下浏览器中必须设置且要保证路径正确）
            swf: top.contentPath + '/content/scripts/plugins/webuploader/Uploader.swf',
            disableGlobalDnd: true,
            chunked: false,
            //服务端文件处理地址
            server: top.contentPath + '/Utility/AllPostFile?recId=' + options.keyValue + "&filePath=" + options.fileDir + "&isDate=" + (options.isDate ? 1 : 0),
            fileNumLimit: options.fileNumLimit,
            fileSizeLimit: options.fileSizeLimit,
            fileSingleSizeLimit: options.fileSingleSizeLimit,
            multiple: options.multiple
        });

        // 当有文件添加进来时执行，负责view的创建
        function addFile(file) {
            var $li;
            var $imgWrap;
            if (options.isImage) {
                $li = $('<li>' +
                    '<p class="title">' + file.name + '</p>' +
                    '<p class="imgWrap" id="' + file.id + '" title="点击查看大图" alt="点击查看大图"  ><span class="imgName" style="display:none;">' + file.name + '</span></p>' +
                    '<p class="progress"><span></span></p></li>');

                $btns = $('<div class="file-panel">' +
                    '<span class="cancel">删除</span></div>').appendTo($li);
            } else {
                $li = $('<tr class="row' + file.id + '" style="display:none;" fid="' + file.id + '"><td class="filename"><a onclick="doFile(this,\'' + options.keyValue + '\',\'' + file.filepath + '\')" style="cursor:pointer; text-decoration:underline;">' + file.name + '</a></td><td>' + formatDate(new Date(), 'yyyy-MM-dd hh:mm:ss') + '</td><td><a href="' + top.contentPath + '/PublicInfoManage/ResourceFile/DownloadFile?keyValue=-1&filename=' + encodeURIComponent(file.name) + '&recId=' + options.keyValue + '\" target="_blank" style="cursor:pointer"  title="下载文件"><i class="fa fa-download"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style="cursor:pointer"  title="删除文件" class="delFile"><i class="fa fa-trash-o"></i></a></td></tr>');

                $btns = $li.find(".delFile");

            }

            $li.appendTo($queue);
            $prgress = $li.find('p.progress span'),
                $imgWrap = $li.find('p.imgWrap'),
                //$imgWrap = $queue.find('#' + file.id).eq(0),
                $info = $('<p class="error"></p>');
            showError = function (code) {
                switch (code) {
                    case 'exceed_size':
                        text = '文件大小超出';
                        break;

                    case 'interrupt':
                        text = '上传暂停';
                        break;

                    default:
                        text = '上传失败，请重试';
                        break;
                }

                $info.text(text).appendTo($li);
            };

            if (file.getStatus() === 'invalid') {
                showError(file.statusText);
            } else {
                percentages[file.id] = [file.size, 0];
                file.rotation = 0;
            }

            file.on('statuschange', function (cur, prev) {
                if (prev === 'progress') {
                    $prgress.hide().width(0);
                } else if (prev === 'queued') {
                    $li.off('mouseenter mouseleave');
                }
                // 成功
                if (cur === 'error' || cur === 'invalid') {
                    console.log(file.statusText);
                    showError(file.statusText);
                    percentages[file.id][1] = 1;
                } else if (cur === 'interrupt') {
                    showError('interrupt');
                } else if (cur === 'queued') {
                    percentages[file.id][1] = 0;
                } else if (cur === 'progress') {
                    $info.remove();
                    $prgress.css('display', 'block');
                } else if (cur === 'complete') {
                    uploader.makeThumb(file, function (error, src) {
                        if (error) {
                            return;
                        }
                        var imgUrl = $imgWrap.attr("path");
                        if (!!imgUrl) {
                            imgUrl = top.contentPath + imgUrl.substring(1, imgUrl.length);
                        }
                        var img = $('<img src="' + src + '" data-original="' + imgUrl + '" id="img' + file.id + '"><span class="imgName" style="display:none;">' + file.name + '</span>');
                        $imgWrap.empty().append(img);
                        if (!!$imgWrap.parent().parent().parent().parent()) {
                            //先清除，再添加图片;
                            var uploaderId = $imgWrap.parent().parent().parent().parent().attr("id");
                            $("#" + uploaderId).viewer("destroy");
                            $("#" + uploaderId).viewer({ url: "data-original" });
                        }
                    }, thumbnailWidth, thumbnailHeight);
                    percentages[file.id] = [file.size, 0];
                    file.rotation = 0;
                    successCount++;

                }

                $li.removeClass('state-' + prev).addClass('state-' + cur);
            });
            if (options.isImage) {
                $li.on('mouseover', function () {
                    $li.find(".file-panel").stop().animate({ height: 30 });

                });

                $li.on('mouseout', function () {
                    $li.find(".file-panel").stop().animate({ height: 0 });
                });

                $btns.on('click', 'span', function () {
                    var index = $(this).index(),
                        deg;
                    switch (index) {
                        case 0:
                            uploader.removeFile(file);
                            removeFile(file.name, options.keyValue, this);
                            return;
                        case 1:
                            file.rotation += 90;
                            break;

                        case 2:
                            file.rotation -= 90;
                            break;
                    }

                    if (supportTransition) {
                        deg = 'rotate(' + file.rotation + 'deg)';
                        $imgWrap.css({
                            '-webkit-transform': deg,
                            '-mos-transform': deg,
                            '-o-transform': deg,
                            'transform': deg
                        });
                    } else {
                        $imgWrap.css('filter', 'progid:DXImageTransform.Microsoft.BasicImage(rotation=' + (~~((file.rotation / 90) % 4 + 4) % 4) + ')');
                    }
                });
            } else {
                //删除附件
                $li.find(".delFile").bind("click", function () {
                    var _self = $(this);
                    var dlg = $.ConfirmAjax({
                        msg: "确定删除吗？",
                        url: top.contentPath + "/PublicInfoManage/ResourceFile/RemoveFile",
                        param: { recId: options.keyValue, fileName: file.name },
                        success: function (data) {
                            uploader.removeFile(file);
                            _self.parent().parent().remove();
                            if (window.cancelBack != undefined) {
                                window.cancelBack();
                            }
                        }
                    });

                });

            }

        }

        function updateTotalProgress() {
            var loaded = 0,
                total = 0,
                spans = $progress.children(),
                percent;

            $.each(percentages, function (k, v) {
                total += v[0];
                loaded += v[0] * v[1];
            });

            percent = total ? loaded / total : 0;

            spans.eq(0).text(Math.round(percent * 100) + '%');
            spans.eq(1).css('width', Math.round(percent * 100) + '%');
            updateStatus();
        }

        function updateStatus() {
            var text = '', stats;

            //if (state === 'ready') {
            //    text = '选中' + fileCount + '个文件，共' +
            //            WebUploader.formatSize(fileSize) + '。';
            //} else if (state === 'confirm') {
            stats = uploader.getStats();
            //    if (stats.uploadFailNum) {
            //        text = '已成功上传' + stats.successNum + '个文件，' +
            //            stats.uploadFailNum + '个文件上传失败，<a class="retry" href="#">重新上传</a>'
            //    }

            //} else {
            stats = uploader.getStats();
            //    text = '共' + fileCount + '个（' +
            //            WebUploader.formatSize(fileSize) +
            //            '），已上传' + stats.successNum + '个';

            //    if (stats.uploadFailNum) {
            //        text += '，失败' + stats.uploadFailNum + '个';
            //    }
            //}

            //$info.html(text);
        }

        function setState(val) {
            var file, stats;

            if (val === state) {
                return;
            }
            state = val;
            switch (state) {
                case 'pedding':
                    $placeHolder.removeClass('element-invisible');
                    $queue.parent().removeClass('filled');
                    $queue.hide();
                    $statusBar.addClass('element-invisible');
                    uploader.refresh();
                    break;
                case 'ready':
                    $placeHolder.addClass('element-invisible');
                    $queue.parent().addClass('filled');
                    $queue.show();
                    $statusBar.removeClass('element-invisible');
                    uploader.refresh();
                    break;
                case 'uploading':
                    $progress.show();
                    break;
                case 'paused':
                    $progress.show();
                    break;
                case 'confirm':
                    $progress.hide();
                    stats = uploader.getStats();
                    if (stats.successNum && !stats.uploadFailNum) {
                        setState('finish');
                        return;
                    }
                    break;
                case 'finish':
                    stats = uploader.getStats();
                    if (stats.successNum) {
                        //dialogMsg('上传成功！', 0);
                    } else {
                        // 没有成功的图片，重设
                        state = 'done';
                        location.reload();
                    }
                    break;
            }

            updateStatus();
        }
        uploader.onBeforeFileQueued = function (file) {

            var isOk = true;
            var arr;
            if (options.isImage) {
                arr = $(options.el).find(".imgName");
            } else {
                arr = $(options.el).find(".filename");
            }

            if (arr.length >= options.fileNumLimit) {
                dialogMsg("对不起，上传文件总数量超过限制！", 0);
                isOk = false;
                return false;
            }
            else {
                arr.each(function (i, dom) {
                    if ($(dom).text() == file.name) {
                        isOk = false;
                        return false;
                    }
                });

            }
            if (!isOk) {
                dialogMsg("此文件已经上传过了！", 0);
                return false;
            }

            return isOk;
        }
        uploader.onUploadProgress = function (file, percentage) {
            var $li = $('#' + file.id),
                $percent = $li.find('.progress span');

            $percent.css('width', percentage * 100 + '%');
            percentages[file.id][1] = percentage;
            updateTotalProgress();
        };

        uploader.onFileQueued = function (file) {
            fileCount++;
            fileSize += file.size;

            if (fileCount === 1) {
                //$placeHolder.addClass('element-invisible');
                $statusBar.show();
            }

            addFile(file);
            setState('ready');
            updateTotalProgress();
        };
        uploader.onUploadSuccess = function (file, response) {
            if (response.resultdata != null && response.resultdata.length>0) {
                $(options.el).find("tr[fid='" + file.id + "']").show();
                $(options.el).find("tr[fid='" + file.id + "']").attr("path", response.resultdata);
                $(options.el).find("p[id='" + file.id + "']").attr("path", response.resultdata);

                var exts = ".jpg,.bmp,.gif,.jpeg,.txt,.png";
                var ext = file.name.substring(file.name.lastIndexOf(".")).toLowerCase();
                var fileobj = $(options.el).find("tr[fid='" + file.id + "']").eq(0).find("a").eq(0);
                fileobj.attr("path", response.resultdata);
                var url = window.location.protocol + "//" + window.location.host + top.contentPath + response.resultdata.replace("~", "");
                fileobj.attr("url", url);
                if (exts.indexOf(ext) >= 0) {
                    fileobj.removeAttr("onclick");
                    fileobj.attr("href", url);
                    fileobj.attr("target", "_blank");
                }
                else {
                    if (".pdf,.ppt,.pptx,.xls,.xlsx,.doc,.docx,.mp3,.mp4,.webm,.ogv,.ogg".indexOf(ext) < 0) {
                        fileobj.removeAttr("onclick");
                        fileobj.attr("href", top.contentPath + "/PublicInfoManage/ResourceFile/DownloadFile?keyValue=-1&filename=" + file.name + "&recId=" + options.keyValue);
                        fileobj.attr("target", "_blank");
                    }
                }
                dialogMsg('上传成功！', 0);
                if (window.callBack != undefined) {
                    window.callBack(file.name, file.ext);
                }
            }
        };
        //uploader.onFileDequeued = function (file) {

        //    fileCount--;
        //    fileSize -= file.size;

        //    if (!fileCount) {
        //        setState('pedding');
        //    }
        //    //addFile(file);
        //removeFile(file);
        //    updateTotalProgress();

        //};

        uploader.on('all', function (type) {
            var stats;
            switch (type) {
                case 'uploadFinished':
                    setState('confirm');
                    if (!!uploader.id) {
                        //先清除，再添加图片;
                        $("#" + uploader.id).viewer("destroy");
                        $("#" + uploader.id).viewer({ url: "data-original" });
                    }
                    break;

                case 'startUpload':
                    setState('uploading');
                    break;

                case 'stopUpload':
                    setState('paused');
                    break;

            }
        });
        uploader.onError = function (code) {

            if (code == "F_DUPLICATE") {
                dialogMsg("此文件已经上传过了！", 0);
                uploader.reset();
                return false;
            }
            if (code == "Q_EXCEED_NUM_LIMIT") {
                dialogMsg("对不起，上传文件数据超过限制！", 0);
                uploader.reset();
                return false;
            }
            if (code == "Q_EXCEED_SIZE_LIMIT" || code == "F_EXCEED_SIZE") {
                dialogMsg("对不起，上传文件总大小超过限制！", 0);
                uploader.reset();
                return false;
            }
            if (code == "Q_TYPE_DENIED") {
                dialogMsg("对不起，不能上传空文件或不支持上传此文件格式（允许上传的文件格式为：" + options.extensions + ")", 0);
                uploader.reset();
                return false;
            }
        };
        updateTotalProgress();
    }
};
