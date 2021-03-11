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
    /// �� ����ְҵ��Σ�����ؼ��
    /// </summary>
    public class HazarddetectionController : MvcControllerBase
    {
        private HazarddetectionBLL hazarddetectionbll = new HazarddetectionBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Example()
        {
            return View();
        }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Phone() 
        {
            return View();
        }

        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson, string type)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "HID";
            pagination.p_fields = "AREAVALUE,RISKVALUE,LOCATION,STARTTIME,ENDTIME,STANDARD,DETECTIONUSERNAME,ISEXCESSIVE,CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERORGCODE";//ע���˴�Ҫ�滻����Ҫ��ѯ����
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = hazarddetectionbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ���������ֵ������ö�ȡ�������ϵ�txt ��ʾ�鿴��׼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetTableHtml(string keyValue)
        {
            string table = "";
            string fileUrl = keyValue.Substring(1);//ȥ��ǰ���~��
            string filePath = Server.MapPath(Request.ApplicationPath + fileUrl);
            //�ж��ļ��Ƿ����
            if (DirFileHelper.IsExistFile(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath, System.Text.Encoding.Default))
                {
                    table = sr.ReadToEnd();
                }
            }
            //string table = @"<table><tr><td rowspan='2'>�Ӵ�ʱ����</td><td colspan='4'>�����Ͷ�ǿ��</td></tr><tr><td>I</td><td>II</td><td>III</td><td>IV</td></tr><tr><td>100%</td><td>30</td><td>28</td><td>26</td><td>25</td></tr><tr><td>75%</td><td>31</td><td>29</td><td>28</td><td>26</td></tr><tr><td>50%</td><td>32</td><td>30</td><td>29</td><td>28</td></tr><tr><td>25%</td><td>33</td><td>32</td><td>31</td><td>30</td></tr><tr><td colspan='5'>�Ӵ�ʱ���ʣ��Ͷ�����һ����������ʵ�ʽӴ�������ҵ���ۼ�ʱ����8h�ı��ʡ�</td></tr></table>";
            return table;

        }

        /// <summary>
        /// ��ȡ���һ�β���ָ�꼰��׼
        /// </summary>
        /// <param name="RiskId">ְҵ��Σ������ID</param>
        /// <returns>���ض���Json</returns>
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
        /// ������Excel
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
            DataTable Newdt = AsposeExcelHelper.UpdateDataTable(dt, columns, stringcolumns);//�Ѳ����ֶ�ת����string �����޸�
            for (int i = 0; i < Newdt.Rows.Count; i++)
            {
                Newdt.Rows[i][0] = (i + 1).ToString();
                //if (Newdt.Rows[i]["ENDTIME"].ToString() != "")//ת��ʱ���ʽ
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
                            html = "ָ��" + group[0] + ":" + group[1];
                        }
                        else
                        {
                            html += ";ָ��" + group[0] + ":" + group[1];
                        }
                    }
                    Newdt.Rows[i]["STANDARD"] = html;
                }
                if (Convert.ToInt32(Newdt.Rows[i]["ISEXCESSIVE"]) == 0)
                {
                    Newdt.Rows[i]["ISEXCESSIVE"] = "��";
                }
                else 
                {
                    Newdt.Rows[i]["ISEXCESSIVE"] = "��";
                }
            }
            string FileUrl = @"\Resource\ExcelTemplate\ְҵ��Σ�����ؼ��_����ģ��.xlsx";



            AsposeExcelHelper.ExecuteResult(Newdt, FileUrl, "ְҵ��Σ�����ؼ���б�", "ְҵ��Σ�����ؼ���б�");

            return Success("�����ɹ���");
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ����ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Remove(string keyValue)
        {
            hazarddetectionbll.Remove(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            hazarddetectionbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, HazarddetectionEntity entity)
        {
            hazarddetectionbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
