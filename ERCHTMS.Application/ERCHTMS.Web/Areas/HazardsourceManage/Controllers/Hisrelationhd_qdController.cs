using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.Busines.HazardsourceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using BSFramework.Util.Extension;
using System.Linq;
using System;
using System.Data;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.HazardsourceManage.Controllers
{
    /// <summary>
    /// �� ����Σ��Դ�嵥
    /// </summary>
    public class Hisrelationhd_qdController : MvcControllerBase
    {
        private Hisrelationhd_qdBLL hisrelationhd_qdbll = new Hisrelationhd_qdBLL();
        private HazardsourceBLL hazardsourcebll = new HazardsourceBLL();
        private HistoryBLL historybll = new HistoryBLL();
        private Hisrelationhd_qdBLL hisrelationhd_qdbLL = new Hisrelationhd_qdBLL();
        private DistrictBLL bis_districtbll = new DistrictBLL();
        #region ��ͼ����


        [HttpGet]
        public ActionResult Report()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ShowMeaSure()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangerList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }
        [HttpGet]
        public ActionResult StatisticsDefault()
        {
            return View();
        }
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
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = hisrelationhd_qdbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = hisrelationhd_qdbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region �ύ����
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
            hisrelationhd_qdbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, Hisrelationhd_qdEntity entity)
        {
            hisrelationhd_qdbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion


        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "��������")]
        public ActionResult Export(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = "districtname, DANGERSOURCE, ACCIDENTNAME,DEPTNAME,JDGLZRRFULLNAME,case WHEN  ISDANGER>0 then '��' else '��' end as ISDANGERNAME";
            pagination.p_tablename = "HSD_HAZARDSOURCE t";
            pagination.conditionJson = "1=1";
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                if (user.RoleName.Contains("ʡ��"))
                {
                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.NewDeptCode + "' connect by  prior  departmentid = parentid))";
                }
                else
                {
                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.DeptCode + "' connect by  prior  departmentid = parentid))";
                }
            }
            var title = "Σ��Դ�嵥";
            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["type"].IsEmpty())
                {
                    pagination.p_fields = "districtname, DANGERSOURCE, ACCIDENTNAME,DEPTNAME,JDGLZRRFULLNAME,   case WHEN  gradeval=1 then 'һ��' WHEN  gradeval=3 then '����' WHEN  gradeval=2 then '����' WHEN  gradeval=4 then '�ļ�' else 'δ����' end as gradevalstr";
                    title = "�ش�Σ��Դ�嵥";
                }

            }

            pagination.p_fields += @",case WHEN way='LEC' then 'LEC�����ձ�ʶ' else 'Σ�ջ�ѧƷ�ش�Σ��Դ��ʶ' end as way";
            if (title == "Σ��Դ�嵥")
                pagination.p_fields += @",case WHEN way='LEC' then ITEMA else 0 end as ITEMA
                                     ,case WHEN way='LEC' then ITEMB else 0 end as ITEMB
                                     ,case WHEN way='LEC' then ITEMC else 0 end as ITEMC
                                     ,case WHEN way='LEC' then ITEMR else 0 end as ITEMR";

            pagination.p_fields += @",case WHEN way='DEC' then ITEMDECQ else '' end as ITEMDECQ
                                     ,case WHEN way='DEC' then ITEMDECQ1 else '' end as ITEMDECQ1
                                     ,case WHEN way='DEC' then ITEMDECB1 else '' end as ITEMDECB1
                                     ,case WHEN way='DEC' then ITEMDECB else 0 end as ITEMDECB
                                     ,case WHEN way='DEC' then ITEMDECR else 0 end as ITEMDECR
                                     ,case WHEN way='DEC' then ITEMDECR1 else 0 end as ITEMDECR1";
            var watch = CommonHelper.TimerStart();
            var data = hazardsourcebll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = title;
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = title + ".xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "districtname", ExcelColumn = "��������" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DANGERSOURCE".ToLower(), ExcelColumn = "Σ��Դ����/����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "ACCIDENTNAME".ToLower(), ExcelColumn = "���ܵ��µ��¹�����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "deptname".ToLower(), ExcelColumn = "���β���" });
            listColumnEntity.Add(new ColumnEntity() { Column = "jdglzrrfullname".ToLower(), ExcelColumn = "�ල����������" });
            if (title == "Σ��Դ�嵥")
            {
                listColumnEntity.Add(new ColumnEntity() { Column = "isdangername".ToLower(), ExcelColumn = "�Ƿ�Ϊ�ش�Σ��Դ" });

            }
            else
                listColumnEntity.Add(new ColumnEntity() { Column = "gradevalstr".ToLower(), ExcelColumn = "�ش�Σ��Դ����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "way".ToLower(), ExcelColumn = "������������" });
            if (title == "Σ��Դ�嵥")
            {
                listColumnEntity.Add(new ColumnEntity() { Column = "ITEMA".ToLower(), ExcelColumn = "L" });
                listColumnEntity.Add(new ColumnEntity() { Column = "ITEMB".ToLower(), ExcelColumn = "E" });
                listColumnEntity.Add(new ColumnEntity() { Column = "ITEMC".ToLower(), ExcelColumn = "C" });
                listColumnEntity.Add(new ColumnEntity() { Column = "ITEMR".ToLower(), ExcelColumn = "D" });
            }



            listColumnEntity.Add(new ColumnEntity() { Column = "ITEMDECQ".ToLower(), ExcelColumn = "Σ�ջ�ѧƷʵ�ʴ�����q" });
            listColumnEntity.Add(new ColumnEntity() { Column = "ITEMDECQ1".ToLower(), ExcelColumn = "Σ�ջ�ѧƷ�ٽ���Q" });
            listColumnEntity.Add(new ColumnEntity() { Column = "ITEMDECB1".ToLower(), ExcelColumn = "У��ϵ����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "ITEMDECB".ToLower(), ExcelColumn = "У��ϵ����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "ITEMDECR".ToLower(), ExcelColumn = "R" });
            listColumnEntity.Add(new ColumnEntity() { Column = "ITEMDECR1".ToLower(), ExcelColumn = "R1" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }
        #endregion


        #region ����ͳ��
        /// <summary>
        /// Ӧ�������ƻ������
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetReport()
        {
            string sqlwhere = "";
            var Time = Request["Time"] ?? "";
            var type = int.Parse(Request["type"] ?? "0");
            var returnListObj = new List<Object>();
            var data = new object();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                sqlwhere += " and (CreateUserId='" + user.UserId + "' or DeptCode like '" + user.DeptCode + "%')";
            }
            if (type == 2 && Time.Length > 0)
            {
                sqlwhere += " and (select  to_char(createdate, 'yyyy') from dual)='" + Time + "' ";
            }

            sqlwhere += " and gradeval>0 ";

            switch (type)
            {
                //������ͳ��
                case 1:
                    var list = hazardsourcebll.GetList(sqlwhere + " and IsDanger=1 ").Where(e => (Time.Length > 0 ? e.CreateDate.Value.ToString("yyyy") == Time : 1 == 1));

                    var listjb = from l in list
                                 group l by new { l.GradeVal, l.Grade } into t
                                 select new
                                 {
                                     text = t.Key.Grade,
                                     value = t.Count(),
                                 };
                    var count = 0M;
                    foreach (var item in listjb)
                    {
                        count += decimal.Parse(item.value.ToString());
                    }
                    foreach (var item in listjb)
                    {
                        var itemCount = decimal.Parse(item.value.ToString());
                        returnListObj.Add(new { text = item.text, value = itemCount, bfb = decimal.Round((itemCount / count) * 100, 2) });
                    }
                    data = new { list = returnListObj };
                    break;
                //������ͳ��
                default:
                    var list2 = hisrelationhd_qdbll.GetReportForDistrictName(sqlwhere);
                    var ListNow = new List<NewReportForDistrictName>();
                    foreach (DataRow datarow in list2.Rows)
                    {
                        ListNow.Add(new NewReportForDistrictName { DistrictCode = datarow["DistrictCode"].ToString(), Grade = datarow["Grade"].ToString() });
                    }
                    //��ѯ������
                    var list3 = from l in ListNow
                                group l by new { l.DistrictCode } into t
                                select new
                                {
                                    text = bis_districtbll.GetListForCon(e => e.DistrictCode == t.Key.DistrictCode).ToList().Count>0?bis_districtbll.GetListForCon(e => e.DistrictCode == t.Key.DistrictCode).FirstOrDefault().DistrictName:"",
                                    code = t.Key.DistrictCode,
                                    value = t.Count(),
                                    one = ListNow.Where(e => e.Grade == "һ��" && e.DistrictCode == t.Key.DistrictCode).Count(),
                                    two = ListNow.Where(e => e.Grade == "����" && e.DistrictCode == t.Key.DistrictCode).Count(),
                                    three = ListNow.Where(e => e.Grade == "����" && e.DistrictCode == t.Key.DistrictCode).Count(),
                                    four = ListNow.Where(e => e.Grade == "�ļ�" && e.DistrictCode == t.Key.DistrictCode).Count(),
                                };
                    //��ӵ�����������
                    data = new { list = list3 };
                    break;
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return json;

        }


        [HttpGet]
        public string StaQueryList(string queryJson)
        {

            return hisrelationhd_qdbll.StaQueryList(queryJson);
        }
        /// <summary>
        /// �����ȡ����
        /// </summary>
        public class NewReportForDistrictName
        {
            public string DistrictName { get; set; }
            public string DistrictCode { get; set; }
            public string Grade { get; set; }
        }
        #endregion
    }
}
