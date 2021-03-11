using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Busines.OccupationalHealthManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ERCHTMS.Code;
using System.Data;
using System;

namespace ERCHTMS.Web.Areas.OccupationalHealthManage.Controllers
{
    /// <summary>
    /// 描 述：职业病危害因素监测
    /// </summary>
    public class HazarddetectionController : MvcControllerBase
    {
        private HazarddetectionBLL hazarddetectionbll = new HazarddetectionBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Example()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Phone() 
        {
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson, string type)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "HID";
            pagination.p_fields = "AREAVALUE,RISKVALUE,LOCATION,STARTTIME,ENDTIME,STANDARD,DETECTIONUSERNAME,ISEXCESSIVE,CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERORGCODE";//注：此处要替换成需要查询的列
            pagination.p_tablename = "BIS_HAZARDDETECTION";
            pagination.conditionJson = "1=1";
            pagination.sidx = "ENDTIME";
            pagination.sord = "desc";

            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                pagination.conditionJson += " and " + where;
            }

            var data = hazarddetectionbll.GetPageListByProc(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);

            //var data = occupationalstaffdetailbll.GetList(queryJson);
            //return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = hazarddetectionbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 根据数据字典中配置读取服务器上的txt 显示查看标准
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetTableHtml(string keyValue)
        {
            string table = "";
            string fileUrl = keyValue.Substring(1);//去除前面的~号
            string filePath = Server.MapPath(Request.ApplicationPath + fileUrl);
            //判断文件是否存在
            if (DirFileHelper.IsExistFile(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.Default))
                {
                    table = sr.ReadToEnd();
                }
            }
            //string table = @"<table><tr><td rowspan='2'>接触时间率</td><td colspan='4'>体力劳动强度</td></tr><tr><td>I</td><td>II</td><td>III</td><td>IV</td></tr><tr><td>100%</td><td>30</td><td>28</td><td>26</td><td>25</td></tr><tr><td>75%</td><td>31</td><td>29</td><td>28</td><td>26</td></tr><tr><td>50%</td><td>32</td><td>30</td><td>29</td><td>28</td></tr><tr><td>25%</td><td>33</td><td>32</td><td>31</td><td>30</td></tr><tr><td colspan='5'>接触时间率：劳动者在一个工作日内实际接触高温作业的累计时间与8h的比率。</td></tr></table>";
            return table;

        }

        /// <summary>
        /// 获取最近一次测量指标及标准
        /// </summary>
        /// <param name="RiskId">职业病危害因素ID</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetStandard(string RiskId)
        {
            string wheresql = "";
            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                wheresql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                wheresql += " and " + where;
            }

            string Sta = hazarddetectionbll.GetStandard(RiskId, wheresql);
            ComboxEntity cmb = new ComboxEntity();
            if (Sta != null && Sta != "")
            {
                cmb.itemName = "true";
                cmb.itemValue = Sta;
            }
            else
            {
                cmb.itemName = "false";
                cmb.itemValue = "";
            }

            return ToJsonResult(cmb);
        }


        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult Excel(string queryJson)
        {
            string column = "HID,AREAVALUE,RISKVALUE,LOCATION,STARTTIME,ENDTIME,STANDARD,DETECTIONUSERNAME,ISEXCESSIVE";
            string stringcolumn = "ISEXCESSIVE";
            string[] columns = column.Split(',');
            string[] stringcolumns = stringcolumn.Split(',');
            string whereSql = "";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                whereSql = "";
            }
            else
            {
                string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                whereSql += " and " + where;
            }

            DataTable dt = hazarddetectionbll.GetDataTable(queryJson, whereSql);
            DataTable Newdt = AsposeExcelHelper.UpdateDataTable(dt, columns, stringcolumns);//把部分字段转换成string 方便修改
            for (int i = 0; i < Newdt.Rows.Count; i++)
            {
                Newdt.Rows[i][0] = (i + 1).ToString();
                //if (Newdt.Rows[i]["ENDTIME"].ToString() != "")//转换时间格式
                //{
                //    Newdt.Rows[i]["ENDTIME"] = Convert.ToDateTime(Newdt.Rows[i]["ENDTIME"]).ToString("yyyy-MM-dd");
                //}
                if (Newdt.Rows[i]["STANDARD"].ToString() != "")
                {
                    string[] str = Newdt.Rows[i]["STANDARD"].ToString().Split(';');
                    string html = "";
                    for (var j = 0; j < str.Length; j++)
                    {
                        var group = str[j].Split(',');
                        if (j == 0)
                        {
                            html = "指标" + group[0] + ":" + group[1];
                        }
                        else
                        {
                            html += ";指标" + group[0] + ":" + group[1];
                        }
                    }
                    Newdt.Rows[i]["STANDARD"] = html;
                }
                if (Convert.ToInt32(Newdt.Rows[i]["ISEXCESSIVE"]) == 0)
                {
                    Newdt.Rows[i]["ISEXCESSIVE"] = "否";
                }
                else 
                {
                    Newdt.Rows[i]["ISEXCESSIVE"] = "是";
                }
            }
            string FileUrl = @"\Resource\ExcelTemplate\职业病危害因素监测_导出模板.xlsx";



            AsposeExcelHelper.ExecuteResult(Newdt, FileUrl, "职业病危害因素监测列表", "职业病危害因素监测列表");

            return Success("导出成功。");
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Remove(string keyValue)
        {
            hazarddetectionbll.Remove(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            hazarddetectionbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, HazarddetectionEntity entity)
        {
            hazarddetectionbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
