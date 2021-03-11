using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.BaseManage;
using System.Linq;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� ���������ȫ�
    /// </summary>
    public class SafetyActivityController : MvcControllerBase
    {
        private SafetyActivityBLL SafetyActivitybll = new SafetyActivityBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
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
            var data = SafetyActivitybll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
        //[HandlerMonitor(3, "��ҳ��ѯ�û���Ϣ!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "t.Id";
            pagination.p_fields = "t.EngineerId,t.StartTime,t.EndTime,t.PeopleNum,t.PracticalPeopleNum,t.Condition,t.StudyRecord,t.StudyDetails,t.CREATEUSERID,t.CREATEUSERDEPTCODE,t.CREATEUSERORGCODE,t.CREATEDATE,t.CREATEUSERNAME,o.engineerletdept,o.engineername,p.outsourcingname";
            pagination.p_tablename = "EPG_SAFETYACTIVITY t left join EPG_OUTSOURINGENGINEER o on t.engineerid=o.id left join EPG_OUTSOURCINGPROJECT p on o.outprojectid=p.outprojectid";
            //pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            string allrangedept = "";
            try
            {
                allrangedept = dataitemdetailbll.GetDataItemByDetailCode("SBDept", "SBDeptId").FirstOrDefault().ItemValue;
            }
            catch (Exception)
            {

            }

            if (role.Contains("ʡ��"))
            {
                pagination.conditionJson = string.Format(@" t.createuserorgcode  in (select encode
                from BASE_DEPARTMENT d
                        where d.deptcode like '{0}%' and d.nature = '����' and d.description is null)", user.NewDeptCode);
            }
            else if (role.Contains("��˾���û�") || role.Contains("���������û�") || allrangedept.Contains(user.DeptId))
            {
                pagination.conditionJson = string.Format(" t.createuserorgcode  = '{0}'", user.OrganizeCode);
            }
            else if (role.Contains("�а��̼��û�"))
            {
                pagination.conditionJson = string.Format(" (o.outprojectid ='{0}' or o.supervisorid='{0}' or t.createuserid = '{1}')", user.DeptId, user.UserId);
            }
            else
            {
                var deptentity = departmentbll.GetEntity(user.DeptId);
                while (deptentity.Nature == "����" || deptentity.Nature == "רҵ")
                {
                    deptentity = departmentbll.GetEntity(deptentity.ParentId);
                }
                pagination.conditionJson = string.Format(" o.engineerletdeptid  in (select departmentid from base_department where encode like '{0}%') ", deptentity.EnCode);

            }

            var watch = CommonHelper.TimerStart();
            var data = SafetyActivitybll.GetPageList(pagination, queryJson);
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = SafetyActivitybll.GetEntity(keyValue);
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
            SafetyActivitybll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafetyActivityEntity entity)
        {
            SafetyActivitybll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "����֪ͨ��������")]
        public ActionResult ExportData(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "Id";
            pagination.p_fields = "(case isremind when 1 then '��' else '��' end) as IsRemind,Title,IssueDeptName,IssuerName,IssueTime";
            pagination.p_tablename = "HRS_SafetyActivity";
            pagination.conditionJson = "1=1";

            var watch = CommonHelper.TimerStart();
            var data = SafetyActivitybll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "֪ͨ����";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "֪ͨ����.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "r", ExcelColumn = "���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "isremind", ExcelColumn = "��Ҫ", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "title", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedeptname", ExcelColumn = "��������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuername", ExcelColumn = "������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuetime", ExcelColumn = "����ʱ��", Alignment = "center" });
            
            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("�����ɹ���");
        }
        #endregion
    }
}
