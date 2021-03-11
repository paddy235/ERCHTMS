using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.BaseManage;
using System.Web;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using BSFramework.Cache.Factory;
using System.Linq;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.SystemManage;
using BSFramework.Util.Offices;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.TwoTickets;
using ERCHTMS.Entity.TwoTickets;
namespace ERCHTMS.Web.Areas.TwoTicketsMange.Controllers
{
    /// <summary>
    /// �� ��������������ҵ���
    /// </summary>
    public class TwoTicketController : MvcControllerBase
    {
        private TwoTicketsBLL threepeoplecheckbll = new TwoTicketsBLL();

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
        /// �������嵥�б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
        /// <summary>
        /// �����˵���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// ������Ա
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form2()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form1()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Form3()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Form4()
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
        public ActionResult GetListJson(Pagination pagination, string queryJson,int mode=1)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                pagination.p_kid = "t.ID";
                pagination.p_fields = "createtime,createusername,worktime,sno,tickettype,deptname,dutyuser,senduser,audituser,tsdsno,audittime,registeruser,registertime,audituser1,address,tutelageuser,createuserid,t.iscommit,t.status,1 issubmit,0 state,content,workpermittime,monitor";
                pagination.p_tablename = "XSS_TWOTICKETS t";
                pagination.conditionJson = string.Format("datatype={1} and (t.iscommit=1 or (t.iscommit=0 and t.createuserid='{0}')) and CREATEUSERDEPTCODE like '{2}%'", user.UserId, mode, user.OrganizeCode);
                var watch = CommonHelper.TimerStart();
                DataTable data = threepeoplecheckbll.GetPageList(pagination, queryJson);
                DepartmentBLL deptBll=new DepartmentBLL();
                foreach(DataRow dr in data.Rows)
                {
                    DataTable dt= deptBll.GetDataTable(string.Format("select iscommit,status from XLD_TWOTICKETRECORD where ticketid='{0}' order by createdate desc",dr["id"].ToString()));
                    if (dt.Rows.Count>0)
                    {
                        dr["issubmit"] = dt.Rows[0]["iscommit"];
                        dr["state"] = dt.Rows[0]["status"];
                    }
                }
                DateTime nowTime = DateTime.Now;
                int weeknow = Convert.ToInt32(nowTime.DayOfWeek);
                weeknow = (weeknow == 0 ? (7 - 1) : weeknow - 1);
                int daydiff = -1 * weeknow;
                DateTime firstDay = nowTime.AddDays(daydiff);
               
                //���ܿ�Ʊ����
                DataTable dtCount = deptBll.GetDataTable(string.Format("select count(1)from XSS_TWOTICKETS t where iscommit=1 and datatype='{0}' and t.CreateTime between to_date('{1}','yyyy-mm-dd') and to_date('{2}','yyyy-mm-dd')", mode, firstDay.ToString("yyyy-MM-dd"), firstDay.AddDays(6).ToString("yyyy-MM-dd")));
                int count1=int.Parse(dtCount.Rows[0][0].ToString());
                //���¿�Ʊ����
                dtCount = deptBll.GetDataTable(string.Format("select count(1) from XSS_TWOTICKETS t where iscommit=1 and datatype='{0}' and t.CreateTime between to_date('{1}','yyyy-mm-dd') and to_date('{2}','yyyy-mm-dd')", mode, DateTime.Now.ToString("yyyy-MM-01"), DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")));
                int count2 = int.Parse(dtCount.Rows[0][0].ToString());
                var JsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    userdata = new {weekCount=count1,monthCount=count2 }
                };
                return Content(JsonData.ToJson());
            }
            catch(Exception ex)
            {
                return Error(ex.Message);

            }
            
        }
        [HttpGet]
        public ActionResult GetRecordJson(string keyValue)
        {
            try
            {
                var data = threepeoplecheckbll.GetAuditRecord(keyValue);
                return Success("��ȡ���ݳɹ�",data);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = threepeoplecheckbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ���ɹ���Ʊ���
        /// </summary>
        /// <param name="dataType">Ʊ����</param>
        /// <param name="ticketType">Ʊ����</param>
        /// <returns></returns>
        [HttpGet]
        public string CreateTicketCode(string keyValue, string dataType, string ticketType)
        {
            try
            {
                return threepeoplecheckbll.CreateTicketCode(keyValue,dataType, ticketType);
            }
            catch (Exception ex)
            {

                return "";
            }
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
            threepeoplecheckbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="ApplyUsers">��Ա��Ϣ</param>
        ///<param name="AuditInfo">�����Ϣ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, TwoTicketsEntity entity)
        {
            try
            {
                threepeoplecheckbll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
            
        }

        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        ///<param name="record">�����Ϣ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveRecord(string keyValue, TwoTicketRecordEntity record)
        {
            try
            {
                threepeoplecheckbll.InsertRecord(record);
                return Success("�����ɹ�");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
         
        }
        #endregion

        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        ///<param name="record">�����Ϣ</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Export(string queryJson,int mode = 1)
        {
            try
            {
                List<ColumnEntity> cols = new List<ColumnEntity>();
                string fileName = "����Ʊ";
                Operator user = OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                string[] columnNames = { "����״̬",  "����Ʊ���", "����Ʊ���", "��������", "����/����", "����Ʊ������", "����Ʊǩ����", "����Ʊ�����", "����Ʊ���ʱ��", "ֵ��/�೤", "��׼����ʱ��" };
                string[] fields = { "status", "sno", "tickettype", "content", "deptname", "dutyuser", "senduser", "audituser", "workpermittime", "monitor", "audittime" };
                if (mode==1)
                {
                    pagination.p_fields = "case when status=1 and iscommit=1 then '�ѿ�Ʊ' when status=1 and iscommit=0 then '�ѿ�Ʊ(����δ�ύ)' when status=2 and iscommit=1 then '������' when status=2 and iscommit=0 then '������(����δ�ύ)' when status=3 and iscommit=1 then '����Ʊ' when status=3 and iscommit=0 then '����Ʊ(����δ�ύ)' when status=4 and iscommit=0 then '������(����δ�ύ)' else '������' end status,sno,tickettype,to_char(content) content,deptname,dutyuser,senduser,audituser,to_char(workpermittime,'yyyy-mm-dd hh24:mi') workpermittime,monitor,case when audittime is not null then (to_char(audittime,'yyyy-mm-dd hh24:mi') || '��' || to_char(RegisterTime,'yyyy-mm-dd hh24:mi') ) else '' end audittime";
                }
                if (mode == 2)
                {
                    fileName = "����Ʊ";
                    pagination.p_fields = "case when status=1 and iscommit=1 then '�ѿ�Ʊ' when status=1 and iscommit=0 then '�ѿ�Ʊ(����δ�ύ)' when status=2 and iscommit=1 then '������' when status=2 and iscommit=0 then '������(����δ�ύ)' when status=3 and iscommit=1 then '����Ʊ' when status=3 and iscommit=0 then '����Ʊ(����δ�ύ)' when status=4 and iscommit=0 then '������(����δ�ύ)' else '������' end status,sno,tickettype,to_char(content) content,deptname,dutyuser,senduser,audituser,case when audittime is not null then (to_char(audittime,'yyyy-mm-dd hh24:mi') || '��' || to_char(registertime,'yyyy-mm-dd hh24:mi'))  else '' end audittime,createusername,to_char(createtime,'yyyy-mm-dd hh24:mi')createtime";
                    columnNames = new string[] { "����״̬", "����Ʊ���", "����Ʊ���", "��������", "����/����", "������", "������", "�໤��", "��׼����ʱ��", "�Ǽ���", "�Ǽ�ʱ��" };
                    fields = new string[] { "status","sno", "tickettype", "content", "deptname", "dutyuser", "senduser", "audituser", "audittime", "createusername", "createtime" };
                }
                if (mode ==3)
                {
                    fileName = "��ϵƱ";
                    pagination.p_fields = "case when status=1 and iscommit=1 then '�ѿ�Ʊ' when status=1 and iscommit=0 then '�ѿ�Ʊ(����δ�ύ)' when status=2 and iscommit=1 then '������' when status=2 and iscommit=0 then '������(����δ�ύ)' when status=3 and iscommit=1 then '����Ʊ' when status=3 and iscommit=0 then '����Ʊ(����δ�ύ)' when status=4 and iscommit=0 then '������(����δ�ύ)' else '������' end status,sno,tickettype,to_char(content) content,deptname,dutyuser,audituser,to_char(audittime,'yyyy-mm-dd hh24:mi') audittime,tsdsno,createusername,to_char(createtime,'yyyy-mm-dd hh24:mi') registertime";
                    columnNames = new string[] { "����״̬", "��ϵƱ���", "��ϵƱ���", "��������", "����/����", "��ϵ��", "�����", "���ʱ��", "ͣ�͵���", "�Ǽ���", "�Ǽ�ʱ��" };
                    fields = new string[] { "status", "sno", "tickettype", "content", "deptname", "dutyuser", "audituser", "audittime", "tsdsno", "createusername", "registertime" };
                }
                if (mode ==4)
                {
                    fileName = "����Ʊ";
                    pagination.p_fields = "case when status=1 and iscommit=1 then '�ѿ�Ʊ' when status=1 and iscommit=0 then '�ѿ�Ʊ(����δ�ύ)' when status=2 and iscommit=1 then '������' when status=2 and iscommit=0 then '������(����δ�ύ)' when status=3 and iscommit=1 then '����Ʊ' when status=3 and iscommit=0 then '����Ʊ(����δ�ύ)' when status=4 and iscommit=0 then '������(����δ�ύ)' else '������' end status,sno,tickettype,to_char(content) content,deptname,dutyuser,audituser,address,to_char(audittime,'yyyy-mm-dd hh24:mi') audittime";
                    columnNames = new string[] { "����״̬","����Ʊ���", "��ҵƱ���", "��������", "����/����", "��������", "�����/ֵ��", "����ص�", "������ʱ��" };
                    fields = new string[] { "status", "sno", "tickettype", "content", "deptname", "dutyuser", "audituser", "address",  "audittime" };
                }
                int j = 0;
                foreach (string name in columnNames)
                {
                    cols.Add(new ColumnEntity {
                        Column = fields[j],
                        ExcelColumn=name,
                        Width = j == fields.Length-1?300:150
                    });
                    j++;
                }
                pagination.page = 1;
                pagination.sidx = "createtime";
                pagination.sord = "desc";
                pagination.rows = 1000000;
                pagination.p_kid = "t.ID";
                pagination.p_tablename = "XSS_TWOTICKETS t";
                pagination.conditionJson = string.Format("datatype={1} and (t.iscommit=1 or (t.iscommit=0 and t.createuserid='{0}')) and CREATEUSERDEPTCODE like '{2}%'", user.UserId, mode, user.OrganizeCode);
                var watch = CommonHelper.TimerStart();
                DataTable data = threepeoplecheckbll.GetPageList(pagination, queryJson);
                data.Columns.Remove("id"); data.Columns.Remove("r");
                BSFramework.Util.Offices.ExcelHelper.ExportByAspose(data, fileName, cols);
                return Success("�����ɹ�");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

    }
}
