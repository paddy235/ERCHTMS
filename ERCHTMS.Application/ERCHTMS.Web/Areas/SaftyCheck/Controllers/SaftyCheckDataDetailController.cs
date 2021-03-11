using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.Busines.SaftyCheck;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Extension;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERCHTMS.Code;
using ERCHTMS.Busines.RiskDatabase;

namespace ERCHTMS.Web.Areas.SaftyCheck.Controllers
{
    /// <summary>
    /// �� ������ȫ��������
    /// </summary>
    public class SaftyCheckDataDetailController : MvcControllerBase
    {
        private SaftyCheckDataDetailBLL sdbll = new SaftyCheckDataDetailBLL();
        private RiskAssessBLL riskassessbll = new RiskAssessBLL();
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
        #endregion

        #region ��ȡ����
        /// <summary>
        /// �������м�¼
        /// </summary>
        [HttpGet]
        public ActionResult GetPageListJsonAll(string dictrictid, string risk,string chargedeptcode)
        {
            string sql = "select status,ID,DISTRICTNAME,riskdesc DANGERSOURCE,DISTRICTID,AreaCode from BIS_RISKASSESS where 1=1";
           
            if (!string.IsNullOrEmpty(chargedeptcode))
            {
                StringBuilder sb = new StringBuilder();
                var d = OperatorProvider.Provider.Current();
                if (d.RoleName != null)
                {
                    if (!d.RoleName.Contains("����"))
                        sb.Append(string.Format(" and  deptcode like '{0}%'", chargedeptcode));
                    else
                        sb.Append(string.Format(" and  deptcode like '{0}%'", chargedeptcode.Substring(0, 3)));
                }
                sql += sb.ToString();
            }
            if (!string.IsNullOrEmpty(dictrictid))
            {
                sql += " and DISTRICTID='" + dictrictid + "'";
            }
            if (!string.IsNullOrEmpty(risk))
            {
                sql += " and riskdesc like '%" + risk.Trim() + "%'";
            }
            sql += string.Format(" and  status=1  and deleteMark='0'");
            var data = riskassessbll.GetPageListRiskAll(sql);
            var JsonData = new
            {
                rows = data
               
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = sdbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        ///<param name="ids">id����</param>
        [HttpGet]
        public ActionResult GetDetails(string ids)
        {
            var data = sdbll.GetDetails(ids);
            return ToJsonResult(data);

        }
        [HttpGet]
        public ActionResult GetCount(string recId)
        {
            var count = sdbll.GetCount(recId);
            return Success(count.ToString());

        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = sdbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȫ���������б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {

            var queryParam = queryJson.ToJObject();
            //��������
            if (queryParam["recid"].IsEmpty() && queryParam["newstate"].IsEmpty() && queryParam["isdata"].IsEmpty())
            {
                string sql = " 1=1";
                pagination.p_kid = "ID";
                pagination.p_fields = "status,DISTRICTNAME,riskdesc DANGERSOURCE,DISTRICTID,AreaCode";
                pagination.p_tablename = @"BIS_RISKASSESS";
                if (!queryParam["chargedeptcode"].IsEmpty())
                {
                    string chargedeptcode = queryParam["chargedeptcode"].ToString();
                    StringBuilder sb = new StringBuilder();
                    var d = OperatorProvider.Provider.Current();
                    if (d.RoleName != null)
                    {
                        if (!d.RoleName.Contains("����"))
                            sb.Append(string.Format(" and  deptcode like '{0}%'", chargedeptcode));
                        else
                            sb.Append(string.Format(" and  deptcode like '{0}%'", chargedeptcode.Substring(0, 3)));
                    }
                    sql += sb.ToString();
                }
                if (!queryParam["dictrictid"].IsEmpty())
                {
                    sql += " and DISTRICTID='"+ queryParam["dictrictid"].ToString()+ "'";
                }
                if (!queryParam["risk"].IsEmpty())
                {
                    sql += " and riskdesc like'%" + queryParam["risk"].ToString() + "%'";
                }
                sql += string.Format(" and  status=1  and deleteMark='0'");
                if (!queryParam["firstNull"].IsEmpty())
                {
                    sql += " and 1>2";
                }
                pagination.conditionJson = sql;
                var watch = CommonHelper.TimerStart();
                var data = riskassessbll.GetPageListRisk(pagination, queryJson);
                var JsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return Content(JsonData.ToJson());
            }
            else
            {
                //������������
                pagination.p_kid = "ID";
                pagination.p_fields = "CreateDate,BelongDistrict,BelongDistrictID,RiskName,CheckContent,CHECKMAN,CHECKMANID,CheckState,BelongDistrictCode,recid,checkobject,checkobjectid,checkobjecttype,issure,remark,CheckDataId,BelongDept";
                pagination.p_tablename = "BIS_SAFTYCHECKDATADETAILED t";
                pagination.conditionJson = " 1=1";
                var watch = CommonHelper.TimerStart();
                IEnumerable<SaftyCheckDataDetailEntity> data = sdbll.GetPageList(pagination, queryJson);
                var JsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return Content(JsonData.ToJson());
                //return Content(null);
            }
        }
        [HttpGet]
        public ActionResult GetTableListJson(Pagination pagination, string queryJson)
        {

            var queryParam = queryJson.ToJObject();
           
                //������������
                pagination.p_kid = "t.ID pkid";
                pagination.p_fields = "t.CreateDate,t.CheckContent require,t.recid,t.checkobject name,t.checkobjectid stid,t.checkobjecttype,t.CheckDataId rid,t.riskname content,t.BelongDept,t.CheckMan,t.CheckManID,t.createuserdeptcode,t.issure,t.remark,0 count,0 wzcount,0 wtcount,0 count1,0 wzcount1,0 wtcount1";
                pagination.p_tablename = "BIS_SAFTYCHECKDATADETAILED t ";
                pagination.conditionJson = " 1=1";
                var watch = CommonHelper.TimerStart();
                var data = sdbll.GetDataTableList(pagination, queryJson);
                var JsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return Content(JsonData.ToJson());
            
        }
        /// <summary>
        /// �ճ�����Ӧ����ҳ��
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
        [HttpGet]
        public ActionResult GetPageListContentJson(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //��������
            if (queryParam["recid"].IsEmpty() && queryParam["newstate"].IsEmpty())
            {
                //����DataTable
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("ID");//ID
                dtNew.Columns.Add("BelongDistrict");//����
                dtNew.Columns.Add("BelongDistrictID");//����ID
                dtNew.Columns.Add("RiskName");//���յ�����
                dtNew.Columns.Add("CheckContent");//���Ĵ�ʩ
                dtNew.Columns.Add("Count");//
                dtNew.Columns.Add("BelongDistrictCode");//�������
                //
                pagination.p_kid = "ID";
                pagination.p_fields = "DISTRICTNAME,riskdesc DANGERSOURCE,DISTRICTID,AreaCode";
                pagination.p_tablename = "BIS_RISKASSESS t";
                pagination.conditionJson = " 1=1";

                var watch = CommonHelper.TimerStart();
                DataTable dt = sdbll.GetPageOfSysCreate(pagination, queryJson);
                foreach (DataRow item in dt.Rows)
                {
                    DataRow dr = dtNew.NewRow();
                    dr["Id"] = "";
                    dr["BelongDistrict"] = item[2].ToString();
                    dr["RiskName"] = item[3].ToString();
                    dr["BelongDistrictID"] = item[4].ToString();
                    dr["BelongDistrictCode"] = item[5].ToString();
                    string content = "";
                    DataTable dtContent = sdbll.GetPageContent(item[1].ToString());
                    if (dtContent.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtContent.Rows.Count; i++)
                        {
                            content += dtContent.Rows[i][0].ToString() + "|";
                        }
                    }
                    content = content.TrimEnd('|');
                    dr["CheckContent"] = content;
                    dr["Count"] = "";
                    dtNew.Rows.Add(dr);
                }
                var JsonData = new
                {
                    rows = dtNew,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return Content(JsonData.ToJson());
            }
            else
            {
                //������������
                pagination.p_kid = "ID";
                pagination.p_fields = "*";
                pagination.p_tablename = "BIS_SAFTYCHECKDATADETAILED t";
                pagination.conditionJson = " 1=1";
                var watch = CommonHelper.TimerStart();
                var data = sdbll.GetPageList(pagination, queryJson);
                var JsonData = new
                {
                    rows = data,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return Content(JsonData.ToJson());
            }
        }
        /// <summary>
        /// ��ȫ���������б�(�Ѽ��)
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
        public ActionResult GetPageListJsonForResult(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //������������
            pagination.p_kid = "ID";
            pagination.p_fields = "CreateDate,BelongDistrict,BelongDistrictID,RiskName,CheckContent,CHECKMAN,CheckState";
            pagination.p_tablename = "BIS_SAFTYCHECKDATADETAILED t";
            pagination.conditionJson = " 1=1";
            var watch = CommonHelper.TimerStart();
            var data = sdbll.GetPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ���ĵǼ�״̬
        /// </summary>
        [HttpGet]
        public ActionResult RegisterPer(string userAccount, string id)
        {
            sdbll.RegisterPer(userAccount, id);
            return Success("�ɹ���");
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "ɾ�������Ŀ")]
        public ActionResult RemoveForm(string keyValue)
        {
            sdbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="list">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "���������޸ļ����Ŀ")]
        public ActionResult SaveForm(string keyValue, List<SaftyCheckDataDetailEntity> list)
        {
            sdbll.SaveForm(keyValue, list);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
