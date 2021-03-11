(function ($) {
    $.fn.inputComplete = function (options) {
        //====================
        //初始参数
        //====================
        var defaults = {
            url: "",
            type: "get",
            params: {},
            completeCallback: function () { },
            inputkey: "RealName-ParentName/DeptName",   //当前输入框赋键值
            inputval: "RealName",
            isdisabled: false,
            relevancekeys: [{ key: "#LLLEGALPERSONID", value: "UserId" }, { key: "#LLLEGALTEAM", value: "DeptName" }, { key: "#LLLEGALTEAMCODE", value: "DeptCode" }]   //关联键值 
        };
        var globaldata = new Array(); 
        var opt = $.extend(defaults, options);
        var $searchInput = $(this);
        //var iptid = $searchInput.attr("id");
        //关闭浏览器提供给输入框的自动完成
        $searchInput.attr('autocomplete', 'off');
        //创建自动完成的下拉列表，用于显示服务器返回的数据,插入在input的后面，等显示的时候再调整位置
        var $autocomplete = $('<div class="custom-menu custom-widget-content custom-autocomplete"></div>').hide().insertAfter($searchInput);
        //清空下拉列表的内容并且隐藏下拉列表区
        var clear = function () {
            $autocomplete.empty().hide();
        };
        //注册事件，当输入框失去焦点的时候清空下拉列表并隐藏
        $searchInput.blur(function () {
            setTimeout(clear, 500);
        });
        //下拉列表中高亮的项目的索引，当显示下拉列表项的时候，移动鼠标或者键盘的上下键就会移动高亮的项目，想百度搜索那样
        var selectedItem = null;
        //timeout的ID
        var timeoutid = null;
        //设置下拉项的高亮背景
        var setSelectedItem = function (item) {
            //更新索引变量
            selectedItem = item;
            //按上下键是循环显示的，小于0就置成最大的值，大于最大值就置成0
            if (selectedItem < 0) {
                selectedItem = $autocomplete.find('li').length - 1;
            }
            else if (selectedItem > $autocomplete.find('li').length - 1) {
                selectedItem = 0;
            }
            //首先移除其他列表项的高亮背景，然后再高亮当前索引的背景
            $autocomplete.find('li').removeClass('highlight').eq(selectedItem).addClass('highlight');
        };

        //请求获取相应数据
        var ajaxrequest = function () {
            var requrl = opt.url.replace("$(this)", $searchInput.val().trim());
            $.ajax({
                url: requrl,
                data: opt.params , //请求参数
                dataType:"json", //返回数据类型
                type: opt.type, //请求类型
                success: function (data) {
                    if (!!data) {
                        var ldata = undefined == data.length ? data.rows : data; //强制判定
                        //遍历data，添加到自动完成区
                        if (ldata.length > 0) {
                            globaldata = ldata;
                            $.each(ldata, function (index, ele) {
                                var newInput = opt.inputkey;  //获得要显示的下拉列表数据
                                var inputVal = ele[opt.inputval];
                                var eleKeys = Object.keys(ele);
                                $.each(eleKeys, function (j, iele) {
                                    if (newInput.indexOf(iele) >= 0) {
                                        newInput = newInput.replace(iele, ele[iele]);
                                    }
                                });
                                //创建li标签,添加到下拉列表中
                                $('<li class="custom-menu-item"></li>').text(newInput).appendTo($autocomplete).hover(function () {
                                    //下拉列表每一项的事件，鼠标移进去的操作
                                    $(this).siblings().removeClass('highlight');
                                    $(this).addClass('highlight');
                                    selectedItem = index;
                                }, function () {
                                    //下拉列表每一项的事件，鼠标离开的操作
                                    $(this).removeClass('highlight');
                                    //当鼠标离开时索引置-1，当作标记
                                    selectedItem = -1;
                                }).click(function () {
                                    //鼠标单击下拉列表的这一项的话，就将这一项的值添加到输入框中
                                    $searchInput.val(inputVal);
                                    //锁定
                                    if (opt.isdisabled) { $searchInput.attr("disabled", "disabled"); }
                                    //设置关联值
                                    var rkeys = opt.relevancekeys;
                                    $(rkeys).each(function (f, fele) {
                                        $(fele.key).val(ele[fele.value]);
                                        if (undefined!=fele.isdisabled) {
                                            if (fele.isdisabled)
                                            {
                                                $(fele.key).attr("disabled", "disabled");
                                            }
                                        }
                                    });
                                    //清空并隐藏下拉列表
                                    $autocomplete.empty().hide();
                                    });
                            });//事件注册完毕
                        }
                        else {
                            //清空关联值
                            var rkeys = opt.relevancekeys;
                            $(rkeys).each(function (f, fele) {
                                $(fele.key).val("");
                            });
                        }
                  
                        //设置下拉列表的位置，然后显示下拉列表
                        var ypos = $searchInput.position().top;
                        var xpos = $searchInput.position().left;
                        //$autocomplete.css({ 'position': 'relative', 'left': xpos + "px", 'top': ypos + "px" });
                        $autocomplete.css('width', $searchInput.css('width'));
                        $autocomplete.css({ 'position': 'absolute', 'z-index': 1000 });
                        setSelectedItem(0);
                        //显示下拉列表
                        $autocomplete.show();

                        if ($.isFunction(opt.completeCallback)) {
                            opt.completeCallback(data);
                        }
                    }
                },
                error: function (msg)
                {
                    console.log(msg)
                }
            });
        };

        /*
        
         */

        //对输入框进行事件注册
        $searchInput.keyup(function (event) {
            //字母数字，退格，空格
            if (event.keyCode > 40 || event.keyCode == 8 || event.keyCode == 32) {
                //首先删除下拉列表中的信息
                $autocomplete.empty().hide();
                clearTimeout(timeoutid);
                timeoutid = setTimeout(ajaxrequest, 100);
            }
            else if (event.keyCode == 38) {
                //上
                //selectedItem = -1 代表鼠标离开
                if (selectedItem == -1) {
                    setSelectedItem($autocomplete.find('li').length - 1);
                }
                else {
                    //索引减1
                    setSelectedItem(selectedItem - 1);
                }
                event.preventDefault();
            }
            else if (event.keyCode == 40) {
                //下
                //selectedItem = -1 代表鼠标离开
                if (selectedItem == -1) {
                    setSelectedItem(0);
                }
                else {
                    //索引加1
                    setSelectedItem(selectedItem + 1);
                }
                event.preventDefault();
            }
        }).keypress(function (event) {
            //enter键
            if (event.keyCode == 13) {
                //列表为空或者鼠标离开导致当前没有索引值
                if ($autocomplete.find('li').length == 0 || selectedItem == -1) {
                    return;
                }
                var selitem = globaldata[selectedItem];
                var inputVal = selitem[opt.inputval];
                $searchInput.val(inputVal);
                //锁定
                if (opt.isdisabled) { $searchInput.attr("disabled", "disabled"); }
                //设置关联值
                var rkeys = opt.relevancekeys;
                $(rkeys).each(function (index, ele) {
                    $(ele.key).val(selitem[ele.value]);
                    if (undefined != ele.isdisabled) {
                        if (ele.isdisabled) {
                            $(ele.key).attr("disabled", "disabled");
                        }
                    }
                });
               // $searchInput.val($autocomplete.find('li').eq(selectedItem).text());

                //清空并隐藏下拉列表
                $autocomplete.empty().hide();
                event.preventDefault();
            }
        }).keydown(function (event) {
                //esc键
                if (event.keyCode == 27) {
                    $autocomplete.empty().hide();
                    event.preventDefault();
                }
            });

        //注册窗口大小改变的事件，重新调整下拉列表的位置                    
        $(window).resize(function () {
            var ypos = $searchInput.position().top;
            var xpos = $searchInput.position().left;
            $autocomplete.css('width', $searchInput.css('width'));
            //$autocomplete.css({ 'position': 'relative', 'left': xpos + "px", 'top': ypos + "px" });
            $autocomplete.css({ 'position': 'absolute', 'z-index': 1000 });
        });
    }
})(jQuery);