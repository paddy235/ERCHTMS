using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� �������������
    /// </summary>
    public class ReturntoworkController : MvcControllerBase
    {
        private ReturntoworkBLL returntoworkbll = new ReturntoworkBLL();
        private AptitudeinvestigateauditBLL auditbll = new AptitudeinvestigateauditBLL();
        private HisReturnWorkBLL hisreturnworkbll = new HisReturnWorkBLL();
        private HistoryAuditBLL historyauditbll = new HistoryAuditBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();

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
        [HttpGet]
        public ActionResult HistoryIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult HistoryForm()
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
            var data = returntoworkbll.GetList(queryJson);
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
            var data = returntoworkbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�����Ϣ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetAuditEntity(string keyValue) {
            var data = auditbll.GetAuditEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡʵ��--��ʷ��¼���� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue, string HisAuditId)
        {
            var hisReturn = hisreturnworkbll.GetEntity(keyValue);
            var hisauditData = historyauditbll.GetEntity(HisAuditId);
            var hisData = new
            {
                hisReturn = hisReturn,
                hisAudit = hisauditData
            };
            return ToJsonResult(hisData);
           
        }

       
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson) {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id tid";
                pagination.p_fields = @" t.applypeople,t.createuserid,
                                           t.applypeopleid,
                                           t.applytime,
                                           t.applyno,
                                           t.applytype,
                                           t.applyreturntime,
                                           b.fullname,
                                           b.senddeptid,
                                           t.iscommit,
                                           e.engineername,
                                           e.engineerletdept,
                                           e.engineerletdeptid,
                                             decode(a.auditresult, '0', 'ͬ��', '1', '��ͬ��', '2', '�����', '') auditresult,
                                           a.id aid";
                pagination.p_tablename = @"epg_returntowork t
                                              left join epg_outsouringengineer e on e.id = t.outengineerid
                                              left join base_department b on b.departmentid = t.outprojectid
                                              left join epg_aptitudeinvestigateaudit a on a.aptitudeid=t.id";
                pagination.sidx = "t.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator currUser = OperatorProvider.Provider.Current();
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = " 1=1  ";
                }
                else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�"))
                {
                    pagination.conditionJson = string.Format("  (t.iscommit='1'or t.createuserid='{0}') ", currUser.UserId);
                }
                else if (currUser.RoleName.Contains("�а��̼��û�"))
                {
                    pagination.conditionJson = string.Format("  (t.outprojectid ='{0}' or e.supervisorid='{0}' or t.createuserid='{1}' )", currUser.DeptId, currUser.UserId);
                }
                else
                {
                    var deptentity = departmentbll.GetEntity(currUser.DeptId);
                    while (deptentity.Nature == "����" || deptentity.Nature == "רҵ")
                    {
                        deptentity = departmentbll.GetEntity(deptentity.ParentId);
                    }
                    pagination.conditionJson = string.Format(" (e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') and t.iscommit='1' or t.createuserid='{1}') ", deptentity.EnCode, currUser.UserId);

                    //pagination.conditionJson = string.Format("  (e.engineerletdeptid ='{0}' and t.iscommit='1' or t.createuserid='{1}') ", currUser.DeptId, currUser.UserId);
                }

                var data = returntoworkbll.GetPageList(pagination, queryJson);
                var jsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetHistoryPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id tid";
                pagination.p_fields = @" t.applypeople,
                                           t.applypeopleid,
                                           t.applytime,
                                           t.applyno,
                                           t.applytype,
                                           t.applyreturntime,
                                           b.fullname,
                                           b.senddeptid,
                                           t.iscommit,
                                           e.engineername,
                                           e.engineerletdept,
                                           e.engineerletdeptid,
                                             decode(a.auditresult, '0', 'ͬ��', '1', '��ͬ��', '2', '�����', '') auditresult,
                                           a.id auditid";
                pagination.p_tablename = @"epg_historyreturntowork t
                                          left join epg_outsouringengineer e on e.id = t.outengineerid
                                          left join base_department b on b.departmentid = t.outprojectid
                                          left join epg_historyaudit a on a.aptitudeid = t.id";
                pagination.sidx = "t.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                var data = hisreturnworkbll.GetHistoryPageList(pagination, queryJson);
                var jsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
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
            returntoworkbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ReturntoworkEntity entity)
        {
            returntoworkbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
