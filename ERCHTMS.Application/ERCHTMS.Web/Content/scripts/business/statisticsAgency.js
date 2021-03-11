//======================
//统计页面代理类options
//{
//    url: '../../LllegalManage/LllegalStatistics/QueryLllegalNumberLine',
//    chartTable: [
//        {
//            chartId: 'container1',
//            type: 'spline',
//            categories: "['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']",
//            series: "data.lineTotal",
//            gridId: "gridTable1",
//            gridEvalData: "data.lineList"
//        }
//    ],
//    completeCallback: function () { },
//    pageControls: {
//        queryId: "queryArea",
//        searchId: "btn_Search",
//        resetId: "btn_Reset",
//        btns: [
//            { id: 'btn_Export', url: '../../LllegalManage/LllegalStatistics/ExportTrendNumber', args: "javascript:getExpQuery()" },
//            { id: 'lr-panel-one', url: 'Index' },
//            { id: 'lr-panel-two', url: 'TrendIndex' },
//            { id: 'lr-panel-three', url: 'CompareIndex' },
//            { id: "DepartmentName", onClick: selectDepart }
//        ]
//    }
//}
//======================
function statisticsAgency(options) {
    var agc = this;
    options = options || {};
    var chartOptions = [];
    var gridOptions = [];
    //====================
    //初始参数
    //====================
    var settings = {
        url: "",//请求图表数据的url
        type: "get",
        chartTable: [
            { chartId: '', type: 'column', categories: [], series: [], gridId: "", gridEvalData: "" }
        ],
        completeCallback: function () { },
        pageControls: {
            queryId: "queryArea",
            searchId: "btn_Search",
            resetId: "btn_Reset",
            btns: [{id:'btn_Export',url:'',args:''}]
        }
    };
    //====================
    //事件
    //====================
    var events = {
        //====================
        //构造查询条件
        //====================
        onBuildQuery: function () { return buildQuery(); },
        //====================
        //查询
        //====================
        onQueryData: function () { agc.queryData(); },
        //====================
        //重置
        //====================
        onReset: function () { reset(); }
    };
    $.extend(true,settings, options);
    //表格对象
    var grid = $("#" + settings.gridId);
    //====================
    //绑定页面控件ctrls
    //{searchId:'',resetId:'',expId:''}
    //====================
    this.bindPageControls = function (ctrls) {
        var it = this;
        $.extend(settings.pageControls, ctrls);
        var searcId = settings.pageControls.searchId;
        if (!!searcId) {//查询
            $("#" + searcId).click(function () {
                events.onQueryData();
            })
        }
        var resetId = settings.pageControls.resetId;
        if (!!resetId) {//重置
            $("#" + resetId).click(function () {
                events.onReset();
            })
        }
        //按钮事件
        for (var i = 0; i < settings.pageControls.btns.length; i++) {
            var btn = settings.pageControls.btns[i];
            if (!!btn.id) {                       
                var evt = new pageEvent({ id: btn.id, url: btn.url, onClick: btn.onClick, args: btn.args });
                $("#" + btn.id).bind("click", evt.touch);
            }
        }
    };
    //====================
    //更新参数设置
    //====================
    this.setParam = function (param) {
        param = param || {};
        $.extend(true, settings, param);
    }
    //====================
    //绑定页面事件evts
    //{onBuildQuery:fn,onReset:fn,onQueryData:fn}
    //====================
    this.addPageEvents = function (evts) {
        $.extend(events, evts);
    };
    //====================
    //添加jgGrid列表参数
    //{
    //    gridId: "gridTable1",
    //    colModel: [
    //        {
    //            label: '月份', name: 'month', index: 'month', width: 400, align: 'center', sortable: false, formatter: function (val, opt, row) {
    //                return val + "月";
    //            }
    //        },
    //        {
    //            label: '一般违章', name: '一般违章', index: '一般违章', width: 300, align: 'center', sortable: false,
    //            formatter: function (cellvalue, options, rowObject) {                    
    //                return "...";
    //            }
    //        }
    //    ]
    //}
    //====================
    this.addGridOptions = function (opt) {
        var opts = {
            gridId: "gridTable",
            autowidth: true,
            height: 'auto',
            datatype: "local",
            colModel: [],
            rownumbers: false,
            shrinkToFit: true,
            gridview: true
        }
        $.extend(opts, opt);
        gridOptions.push(opts);
    };
    //====================
    //添加Chart图形参数
    //{
    //    chartId: "container1",
    //    title: { text: title },
    //    tooltip: {
    //        formatter: function () {
    //            return "<b>"+this.point.name+"</b><br/>数量：<b>"+ this.point.y + "条</b><br/>"+this.series.name + ":<b>" + this.point.percentage.toFixed(1) + "%</b>";
    //        }
    //    },
    //    plotOptions: {
    //        pie: {
    //            showInLegend: true,
    //            allowPointSelect: true,
    //            cursor: 'pointer',
    //            dataLabels: {
    //                enabled: true,
    //                //format: '<b>{point.name}</b>: {point.percentage:.1f} %',
    //                formatter: function () {
    //                    return '<b>' + this.point.name + '</b>: ' + this.point.percentage.toFixed(1) + ' %';
    //                },
    //                style: {
    //                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
    //                }
    //            }
    //        }
    //    }
    //}
    //====================
    this.addChartOptions = function (opt) {
        var opts = {
            chartId: "container1",
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: 1,
                plotShadow: false
            },
            exporting: { enabled: false },
            lang: { noData: "暂无数据可显示!" },
            noData: {
                position: {
                    align: 'center',
                    verticalAlign: 'middle'
                },
                attr: {
                    'stroke-width': 1
                },
                style: {
                    fontWeight: 'bold',
                    fontSize: '15px',
                    color: '#202030'
                }
            }
        }
        $.extend(opts, opt);
        chartOptions.push(opts);
    };
    //====================
    //初始化args=
    //([
    //{
    //  url:'',
    //  callBack:fn, 
    //  ctrls:[{Id:'',memberId:'',memberText:'',description:'',dataProName:''},{...}]
    //},
    //{
    //  data:[{id:'',name:''},{...}],
    //  ctrl:{Id:'',memberId:'',memberText:'',description:''}
    //},
    //{...}
    //])
    //====================
    this.initialPage = function (args) {
        args = args || {};
        //URL加载数据
        for (var i = 0; i < args.length; i++) {
            var arg = args[i];
            if (!!arg.data && !!arg.ctrl) {
                var ctrl = arg.ctrl;
                $("#" + ctrl.Id).bindCombobox(ctrl.memberId, ctrl.memberText, ctrl.description, arg.data);
            }
            if (!!arg.url) {
                $.SetForm({
                    url: arg.url,
                    success: function (data) {
                        for (var j = 0; j < arg.ctrls.length; j++) {
                            var ctrl = arg.ctrls[j];
                            $("#" + ctrl.Id).bindCombobox(ctrl.memberId, ctrl.memberText, ctrl.description, eval(ctrl.dataProName));
                        }
                        if (arg.callBack && typeof arg.callBack == "function") {
                            arg.callBack(data);
                        }
                    }
                });
            }
            else {
                //回调
                if (!!arg.callBack && typeof arg.callBack == "function") {
                    arg.callBack();
                }
            }
            if (!!arg.conditionData) {
                $("#" + settings.pageControls.queryId).formDeserialize(arg.conditionData);
            }
        }
        searchData();
    };
    //====================
    //首次加载查询
    //====================
    var searchData = function (async) {
        async = async != undefined ? async : true;
        if (!!settings.url) {
            var queryJson = events.onBuildQuery();
            $.ajax({
                type: settings.type,
                url: settings.url,
                data: { queryJson: queryJson },
                async: async,
                success: function (data) {
                    if (!!data) {
                        data = eval("(" + data + ")");
                        for (var i = 0; i < settings.chartTable.length; i++) {
                            var cht = $.extend(true,{ chartId: '', type: '', categories: '', series: '', gridId: '', gridEvalData: '' }, settings.chartTable[i]);
                            loadChart(cht.chartId, cht.type, eval(cht.categories), eval(cht.series));
                            loadGridTable(cht.gridId, eval(cht.gridEvalData));
                        }
                        if ($.isFunction(settings.completeCallback)) {
                            settings.completeCallback(data);
                        }
                    }
                }
            });
        }
    };
    //====================
    //构造查询条件
    //====================
    var buildQuery = function () {
        var data = '';

        var sel = $("#" + settings.pageControls.queryId).find(".ui-select");
        sel.each(function () {
            var it = $(this);
            var pro = it.attr("queryPro");
            pro = !!pro ? pro : it.attr("id");
            var val = it.attr("data-value");
            if (!val)
                val = "";
            data += pro + ":'" + val + "',";
        })

        var ipt = $("#" + settings.pageControls.queryId).find("input");
        ipt.each(function () {
            var it = $(this);
            var pro = it.attr("queryPro");
            pro = !!pro ? pro : it.attr("id");
            var val = it.val();
            if (!val)
                val = "";
            data += pro + ":'" + val + "',";
        })

        if (!!data)
            data = '{' + data.substring(0, data.length - 1) + '}';

        return data;
    };
    //====================
    //根据条件查询
    //====================
    this.queryData = function (async) {
        searchData(async);
    };
    //====================
    //重置
    //====================
    var reset = function () {
        var sel = $("#" + settings.pageControls.queryId).find(".ui-select");
        sel.each(function () {
            $(this).resetCombobox("==全部==", "")
        })

        var ipt = $("#" + settings.pageControls.queryId).find("input:text");
        ipt.each(function () {
            $(this).val("");
        })
        var chk = $("#" + settings.pageControls.queryId).find("input:checkbox");
        chk.each(function () {
            $(this).removeAttr("checked");
        })
    };    
    //
    //加载图形
    //
    var loadChart = function (chartId, type, categories, series) {
        var opt = findChartOpt(chartId);
        if (opt != null) {
            opt.chart.type = type;
            opt.xAxis = { categories: categories};
            opt.series = series;
            $('#' + chartId).highcharts(opt);
        }
    };
    //
    //查找图形参数
    //
    var findChartOpt = function (chartId) {
        var opt = null;

        if (!!chartId) {
            for (var i = 0; i < chartOptions.length; i++) {
                var chOpt = chartOptions[i];
                if (chOpt.chartId == chartId) {
                    opt = chOpt;
                    break;
                }
            }
        }

        return opt;
    };
    //
    //加载表格
    //
    var loadGridTable = function (gridId, data) {
        var opt = findGridOpt(gridId);
        if (opt != null) {
            var $gridTable = $('#' + gridId);
            $gridTable.jqGrid(opt);//.jqGrid('setGridParam', {data: data }).trigger('reloadGrid');
            $gridTable.jqGrid('clearGridData');
            $gridTable.jqGrid('setGridParam', { dataType: 'local', data: data }).trigger('reloadGrid');
        }
    };
    //
    //查找表格参数
    //
    var findGridOpt = function (gridId) {
        var opt = null;

        if (!!gridId) {
            for (var i = 0; i < gridOptions.length; i++) {
                var gOpt = gridOptions[i];
                if (gOpt.gridId == gridId) {
                    opt = gOpt;
                    break;
                }
            }
        }

        return opt;
    };    
    //
    //获取导出条件
    //
    getExpQuery = function () {
        return "queryJson=" + events.onBuildQuery();
    };
}
//====================
//页面按钮事件args
//{
//    Id: '',    
//    url: null,
//    onClick: null,
//    args: null
//}
//====================
function pageEvent(args) {
    //规则执行条件及执行事件
    var r = {
        Id: '',    
        url: null,
        onClick: null,
        args: null
    };
    $.extend(r, args);
    //规则成立，执行相应方法。
    this.touch = function () {
        var args = "";
        if (!!r.args && r.args.indexOf("javascript") > -1)
            args = eval(r.args);
        else
            args = r.args;
        if (!!r.onClick) {
            if ($.isFunction(r.onClick))
                r.onClick(args);
            else
                eval(r.onClick);
        }
        else if (!!r.url) {
            var url = r.url;
            var lnkChar = url.indexOf("?") < 0 ? "?" : "&";
            url += lnkChar + args;
            window.location.href = url;
        }
    }
}