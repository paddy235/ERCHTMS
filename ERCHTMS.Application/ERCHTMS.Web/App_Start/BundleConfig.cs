using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using ERCHTMS.Busines.MatterManage;

namespace ERCHTMS.Web
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //jqgrid表格组件
            bundles.Add(new StyleBundle("~/Content/scripts/plugins/jqgrid/css").Include(
                        "~/Content/scripts/plugins/jqgrid/jqgrid.css"));
            bundles.Add(new ScriptBundle("~/Content/scripts/plugins/jqgrid/js").Include(
                       "~/Content/scripts/plugins/jqgrid/grid.locale-cn.js",
                       "~/Content/scripts/plugins/jqgrid/jqgrid.js"));
            //树形组件
            bundles.Add(new StyleBundle("~/Content/scripts/plugins/tree/css").Include(
                        "~/Content/scripts/plugins/tree/tree.css"));
            bundles.Add(new ScriptBundle("~/Content/scripts/plugins/tree/js").Include(
                       "~/Content/scripts/plugins/tree/tree.js"));
            //表单验证
            bundles.Add(new ScriptBundle("~/Content/scripts/plugins/validator/js").Include(
                      "~/Content/scripts/plugins/validator/validator.js"));
            //日期控件
            bundles.Add(new StyleBundle("~/Content/scripts/plugins/datetime/css").Include(
                        "~/Content/scripts/plugins/datetime/pikaday.css"));
            bundles.Add(new ScriptBundle("~/Content/scripts/plugins/datepicker/js").Include(
                       "~/Content/scripts/plugins/datetime/pikaday.js"));
            //导向组件
            bundles.Add(new StyleBundle("~/Content/scripts/plugins/wizard/css").Include(
                        "~/Content/scripts/plugins/wizard/wizard.css"));
            bundles.Add(new ScriptBundle("~/Content/scripts/plugins/wizard/js").Include(
                       "~/Content/scripts/plugins/wizard/wizard.js" ));
            bundles.Add(new StyleBundle("~/Content/styles/framework-ui.css").Include(
                        "~/Content/styles/framework-ui.css"));
            bundles.Add(new ScriptBundle("~/Content/scripts/utils/js").Include(
                       "~/Content/scripts/utils/framework-ui.js",
                       "~/Content/scripts/utils/framework-form.js"));
            bundles.Add(new ScriptBundle("~/Content/scripts/plugins/printTable/js").Include(
                "~/Content/scripts/plugins/printTable/jquery.printTable.js"));

            //工作流
            bundles.Add(new StyleBundle("~/Content/styles/framework-flowall.css").Include(
            "~/Content/styles/framework-ckbox-radio.css",
            "~/Content/styles/framework-applayout.css",
            "~/Content/styles/framework-flow.css"));
            bundles.Add(new ScriptBundle("~/Content/scripts/flow/js").Include(
              "~/Content/scripts/utils/framework-applayout.js",
              "~/Content/scripts/plugins/flow-ui/flow.js",
              "~/Content/scripts/utils/framework-flowlayout.js"));

            //风险统计
            bundles.Add(new ScriptBundle("~/Content/scripts/business/stat").Include(
           "~/Content/scripts/business/Common.js",
           "~/Content/scripts/business/RiskStat.js"));
            //车辆管理
            bundles.Add(new ScriptBundle("~/Content/scripts/business/CarInLog").Include(
                "~/Content/scripts/business/Common.js",
                "~/Content/scripts/business/CarInLog.js"));

            //根据数据字典路线配置三维 
            string sql = string.Format("select t.itemname,t.itemvalue,t.itemcode from base_dataitem d join BASE_DATAITEMDETAIL t on d.itemid=t.itemid  where d.itemcode='KmConfigure' order by t.sortcode asc");
            DataTable dt = new OperticketmanagerBLL().GetDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == "RouteConfig")
                {
                    bundles.Add(new ScriptBundle("~/Content/scripts/3D/RouteConfig").Include(
                        "~/Content/scripts/3D/" + dt.Rows[i][1] + "/TemplateData/UnityProgress.js",
                        "~/Content/scripts/3D/" + dt.Rows[i][1] + "/Build/UnityLoader.js"));
                }
            }
            dt.Dispose();
           
           

            //登录
            bundles.Add(new ScriptBundle("~/Content/scripts/business/login").Include(
           "~/Content/scripts/utils/framework-ui.js",
           "~/Content/scripts/plugins/crypto/crypto.min.js",
           "~/Content/scripts/business/login.js",
           "~/Content/scripts/business/qrlogin.js"
           ));

            //即时通讯
            bundles.Add(new ScriptBundle("~/Content/scripts/business/webim").Include(
            //"~/Content/scripts/layim/layui.js",
           "~/Content/scripts/layim/webchat.js"));
        }
    }
}