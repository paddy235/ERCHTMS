/**
 * 每周期
 */
function everyTime(dom) {
    var item = $("input[name=v_" + dom.name + "]");
    item.val("*");
    item.change();
}

/**
 * 不指定
 */
function unAppoint(dom) {
    var name = dom.name;
    var val = "?";
    if (name == "year")
        val = "";
    var item = $("input[name=v_" + name + "]");
    item.val(val);
    item.change();
}

function appoint(dom) {

}

/**
 * 周期
 */
function cycle(dom) {
    var name = dom.name;
    var ns = $(dom).parent().find(".numberspinner");
    var start = ns.eq(0).val();
    var end = ns.eq(1).val();
    var item = $("input[name=v_" + name + "]");
    item.val(start + "-" + end);
    item.change();
}

/**
 * 从开始
 */
function startOn(dom) {
    var name = dom.name;
    var ns = $(dom).parent().find(".numberspinner");
    var start = ns.eq(0).val();
    var end = ns.eq(1).val();
    var item = $("input[name=v_" + name + "]");
    item.val(start + "/" + end);
    item.change();
}

function lastDay(dom) {
    var item = $("input[name=v_" + dom.name + "]");
    item.val("L");
    item.change();
}

function weekOfDay(dom) {
    var name = dom.name;
    var ns = $(dom).parent().find(".numberspinner");
    var start = ns.eq(0).val();
    var end = ns.eq(1).val();
    var item = $("input[name=v_" + name + "]");
    item.val(start + "#" + end);
    item.change();
}
function request(keyValue) {
    var search = location.search.slice(1);
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == keyValue) {
            if (decodeURIComponent(ar[1]) == 'undefined') {
                return "";
            } else {
                var val = decodeURIComponent(ar[1]);
                return val.replace(/\'/g, '');
            }
        }
    }
    return "";
}
function lastWeek(dom) {
    var item = $("input[name=v_" + dom.name + "]");
    var ns = $(dom).parent().find(".numberspinner");
    var start = ns.eq(0).val();
    item.val(start + "L");
    item.change();
}

function workDay(dom) {
    var name = dom.name;
    var ns = $(dom).parent().find(".numberspinner");
    var start = ns.eq(0).val();
    var item = $("input[name=v_" + name + "]");
    item.val(start + "W");
    item.change();
}

$(function () {
    var arrSpecial = ["?", "*"];
    //日索引位置
    var DayOfMonth = 3;
    //周索引位置
    var DayofWeek = 5;

    var vals = $("input[name^='v_']");
    var cron = $("#cron");
    vals.change(function () {
        var item = [];
        vals.each(function () {
            item.push(this.value);
        });
        //修复表达式错误BUG，如果后一项不为* 那么前一项肯定不为为*，要不然就成了每秒执行了
        //获取当前选中tab
        var currentIndex = 0;
        $(".tabs>li").each(function (i, item) {
            if ($(item).hasClass("tabs-selected")) {
                currentIndex = i;
                return false;
            }

        });
        //表达式需要遵守的几个规则

        //规则一:当前选中项从日开始算起之前的如果为*，则都设置成0
        //规则二:日和周不能同时为?或者同时为*,如果其中一个有值另外一个必须设置成?
        //规则三:当前选中项之后的如果不为*,则都设置成*

        //规则一实现
        if (item[currentIndex] != "*") {
            var startIndex = currentIndex >= 4 ? 4 : currentIndex;
            for (var i = 0; i < startIndex; i++) {
                if (item[i] == "*") {
                    if (i < 3) {
                        item[i] = "0";
                    } else {
                        item[i] = "1";
                    }
                }
            }
        }


        //规则三实现
        if (item[currentIndex] == "*") {
            for (var i = currentIndex + 1; i < item.length; i++) {
                if (i == 5) {
                    item[i] = "?";
                } else {
                    item[i] = "*";
                }
            }
        }

        //规则二实现
        if (currentIndex == DayOfMonth || currentIndex == DayofWeek) {

            var indexDaySpecial = $.inArray(item[DayOfMonth], arrSpecial);
            var indexWeekSpecial = $.inArray(item[DayofWeek], arrSpecial);

            if (currentIndex == DayOfMonth) {
                if (indexDaySpecial < 0) {
                    item[DayofWeek] = "?";
                } else if (indexDaySpecial == 0) {
                    if (indexWeekSpecial == 0) {
                        item[DayofWeek] = "*";
                    }
                } else {
                    if (indexWeekSpecial == 1) {
                        item[DayofWeek] = "?";
                    }
                }
            } else {
                if (indexWeekSpecial < 0) {
                    item[DayOfMonth] = "?";
                } else if (indexWeekSpecial == 0) {
                    if (indexDaySpecial == 0) {
                        item[DayOfMonth] = "*";
                    }
                } else {
                    if (indexDaySpecial == 1) {
                        item[DayOfMonth] = "?";
                    }
                }
            }
        }

        cron.val(item.join(" ")).change();
    });

    cron.change(function () {
        btnFan();
        //设置最近五次运行时间
        $.ajax({
            type: 'get',
            url: "/Tool/CalcRunTime",
            dataType: "json",
            data: { "CronExpression": $("#cron").val() },
            success: function (data) {
                if (data && data.length == 5) {
                    var strHTML = "<ul>";
                    for (var i = 0; i < data.length; i++) {
                        strHTML += "<li>" + data[i] + "</li>";
                    }
                    strHTML += "</ul>"
                    $("#runTime").html(strHTML);
                } else {
                    $("#runTime").html("");
                }
            }
        });
    });

    var secondList = $(".secondList").children();
    //$("#sencond_appoint").click(function () {
    //    if (this.checked) {
    //        if ($(secondList).filter(":checked").length == 0) {
    //            $(secondList.eq(0)).attr("checked", true);
    //        }
    //        //secondList.eq(0).change();
    //    }
    //});

    secondList.change(function () {
        var sencond_appoint = $("#sencond_appoint").prop("checked");
        if (sencond_appoint) {
            var vals = [];
            secondList.each(function () {
                if (this.checked) {
                    vals.push(this.value);
                }
            });
            var val = "?";
            if (vals.length > 0 && vals.length < 59) {
                val = vals.join(",");
            } else if (vals.length == 59) {
                val = "*";
            }
            var item = $("input[name=v_second]");
            item.val(val);
            item.change();
        }
    });

    var minList = $(".minList").children();
    //$("#min_appoint").click(function () {
    //    if (this.checked) {
    //        if ($(minList).filter(":checked").length == 0) {
    //            $(minList.eq(0)).attr("checked", true);
    //        }
    //       // minList.eq(0).change();
    //    }
    //});

    minList.change(function () {
        var min_appoint = $("#min_appoint").prop("checked");
        if (min_appoint) {
            var vals = [];
            minList.each(function () {
                if (this.checked) {
                    vals.push(this.value);
                }
            });
            var val = "?";
            if (vals.length > 0 && vals.length < 59) {
                val = vals.join(",");
            } else if (vals.length == 59) {
                val = "*";
            }
            var item = $("input[name=v_min]");
            item.val(val);
            item.change();
        }
    });

    var hourList = $(".hourList").children();
    //$("#hour_appoint").click(function () {
    //    if (this.checked) {
    //        if ($(hourList).filter(":checked").length == 0) {
    //            $(hourList.eq(0)).attr("checked", true);
    //        }
    //        //hourList.eq(0).change();
    //    }
    //});

    hourList.change(function () {
        var hour_appoint = $("#hour_appoint").prop("checked");
        if (hour_appoint) {
            var vals = [];
            hourList.each(function () {
                if (this.checked) {
                    vals.push(this.value);
                }
            });
            var val = "?";
            if (vals.length > 0 && vals.length < 24) {
                val = vals.join(",");
            } else if (vals.length == 24) {
                val = "*";
            }
            var item = $("input[name=v_hour]");
            item.val(val);
            item.change();
        }
    });

    var dayList = $(".dayList").children();
    //$("#day_appoint").click(function () {
    //    if (this.checked) {
    //        if ($(dayList).filter(":checked").length == 0) {
    //            $(dayList.eq(0)).attr("checked", true);
    //        }
    //        //dayList.eq(0).change();
    //    }
    //});

    dayList.change(function () {
        var day_appoint = $("#day_appoint").prop("checked");
        if (day_appoint) {
            var vals = [];
            dayList.each(function () {
                if (this.checked) {
                    vals.push(this.value);
                }
            });
            var val = "?";
            if (vals.length > 0 && vals.length < 31) {
                val = vals.join(",");
            } else if (vals.length == 31) {
                val = "*";
            }
            var item = $("input[name=v_day]");
            item.val(val);
            item.change();
        }
    });

    var mouthList = $(".mouthList").children();
    //$("#mouth_appoint").click(function () {
    //    if (this.checked) {
    //        if ($(mouthList).filter(":checked").length == 0) {
    //            $(mouthList.eq(0)).attr("checked", true);
    //        }
    //        //mouthList.eq(0).change();
    //    }
    //});

    mouthList.change(function () {
        var mouth_appoint = $("#mouth_appoint").prop("checked");
        if (mouth_appoint) {
            var vals = [];
            mouthList.each(function () {
                if (this.checked) {
                    vals.push(this.value);
                }
            });
            var val = "?";
            if (vals.length > 0 && vals.length < 12) {
                val = vals.join(",");
            } else if (vals.length == 12) {
                val = "*";
            }
            var item = $("input[name=v_mouth]");
            item.val(val);
            item.change();
        }
    });

    var weekList = $(".weekList").children();
    //$("#week_appoint").click(function () {
    //    if (this.checked) {
    //        if ($(weekList).filter(":checked").length == 0) {
    //            $(weekList.eq(0)).attr("checked", true);
    //        }
    //        //weekList.eq(0).change();
    //    }
    //});

    weekList.change(function () {
        var week_appoint = $("#week_appoint").prop("checked");
        if (week_appoint) {
            var vals = [];
            weekList.each(function () {
                if (this.checked) {
                    vals.push(this.value);
                }
            });
            var val = "?";
            if (vals.length > 0 && vals.length < 7) {
                val = vals.join(",");
            } else if (vals.length == 7) {
                val = "*";
            }
            var item = $("input[name=v_week]");
            item.val(val);
            item.change();
        }
    });
});