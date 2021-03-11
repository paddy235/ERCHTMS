using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System.Data;
using System.Web;
using System;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� �����߷�����ҵ�������
    /// </summary>
    public class HighRiskApplyController : MvcControllerBase
    {
        private HighRiskApplyBLL highriskapplybll = new HighRiskApplyBLL();
        private UserBLL userBLL = new UserBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

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
        /// ���(��)�б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckIndex()
        {
            return View();
        }

        /// <summary>
        /// ���(��)����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckForm()
        {
            return View();
        }

        /// <summary>
        /// ѡ��������ɵĽ���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageTableJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "a.Id";
            pagination.p_fields = "a.WorkFiles,a.CreateDate,c.itemname as applystatename,applystate,b.itemname as worktype,workplace,workcontent,workstarttime,workendtime,applyusername,applydeptname,a.createuserid,a.createuserdeptcode,a.createuserorgcode";
            pagination.p_tablename = " bis_highriskapply a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='WorkType') left join base_dataitemdetail c on a.applystate=c.itemvalue and c.itemid =(select itemid from base_dataitem where itemcode='WorkStatus')";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " and applyuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += " and ApplyDeptCode='" + user.DeptCode + "'";
                            break;
                        case "3"://���Ӳ���
                            pagination.conditionJson += string.Format(" and ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", user.DeptCode, user.DeptId);
                            break;
                        case "4":
                            pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                            break;
                    }
                }
                else
                {
                    pagination.conditionJson += " and 0=1";
                }
            }
            var data = highriskapplybll.GetPageDataTable(pagination, queryJson);
            var watch = CommonHelper.TimerStart();
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


        /// <summary>
        /// ��ȡ���(��)�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetVerifyPageTableJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "a.Id";
            pagination.p_fields = "a.WorkFiles,d.approvestate,d.approvestep,a.CreateDate,c.itemname as applystatename,applystate,b.itemname as worktype,workplace,workcontent,workstarttime,workendtime,applyusername,applydeptname,a.createuserid,a.createuserdeptcode,a.createuserorgcode";
            pagination.p_tablename = " bis_highriskapply a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='WorkType') left join base_dataitemdetail c on a.applystate=c.itemvalue and c.itemid =(select itemid from base_dataitem where itemcode='WorkStatus') right join bis_highriskcheck d on a.id=d.approveid";
            pagination.conditionJson = string.Format("d.approveperson='{0}' and  a.applystate!='1'", user.UserId);
            var data = highriskapplybll.GetVerifyPageTableJson(pagination, queryJson);
            var watch = CommonHelper.TimerStart();
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

        /// <summary>
        /// ��ȡ�����ɵ�ȫ����ҵ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetSelectWorkJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "a.Id as workid";
            pagination.p_fields = "a.CreateDate,b.itemname as worktype,workplace,workcontent,workstarttime,workendtime,ApplyDeptId,ApplyDeptCode,applydeptname";
            pagination.p_tablename = "bis_highriskapply a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='WorkType')";
            pagination.conditionJson = "applystate='6' and a.createuserorgcode='" + user.OrganizeCode + "'";
            var data = highriskapplybll.GetSelectDataTable(pagination, queryJson);
            var watch = CommonHelper.TimerStart();
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

        /// <summary>
        /// ��ȡ�����ɵ�ȫ����ҵ�б�(ͳ����ת)
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetStatisticsWorkJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "a.Id as workid";
            pagination.p_fields = "a.CreateDate,b.itemname as worktype,workplace,workcontent,workstarttime,workendtime,ApplyDeptId,ApplyDeptCode,applydeptname,WorkFiles";
            pagination.p_tablename = "bis_highriskapply a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='WorkType')";
            pagination.conditionJson = "applystate='6' and a.createuserorgcode='" + user.OrganizeCode + "'";
            var data = highriskapplybll.GetStatisticsWorkTable(pagination, queryJson);
            var watch = CommonHelper.TimerStart();
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

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = highriskapplybll.GetList(queryJson);
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
            var data = highriskapplybll.GetEntity(keyValue);
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
            highriskapplybll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HighRiskApplyEntity entity, string verifyids, string approveids)
        {
            Operator usercurrent = OperatorProvider.Provider.Current();
            if (highriskapplybll.SaveForm(keyValue, entity) > 0)
            {
                //��������Ĵӱ��¼(��Σ����ҵ���/������)
                if (verifyids.Length > 0)
                {
                    HighRiskCheckBLL highriskcheckbll = new HighRiskCheckBLL();
                    if (highriskcheckbll.Remove(entity.Id) > 0)
                    {
                        if (!string.IsNullOrEmpty(verifyids))//�����
                        {
                            string[] array = verifyids.Split(',');

                            foreach (var item in array)
                            {
                                UserEntity user = userBLL.GetEntity(item);
                                if (user != null)
                                {
                                    var dept = departmentBLL.GetEntity(user.DepartmentId);
                                    HighRiskCheckEntity checkentity = new HighRiskCheckEntity();
                                    checkentity.ApprovePerson = item;
                                    checkentity.ApprovePersonName = user.RealName;
                                    checkentity.ADeptId = user.DepartmentId;
                                    checkentity.ADeptCode = user.DepartmentCode;
                                    checkentity.ADeptName = dept == null ? usercurrent.OrganizeName : dept.FullName;
                                    checkentity.ApproveStep = "1";//���
                                    checkentity.ApproveState = "0";
                                    checkentity.ApproveId = entity.Id;
                                    highriskcheckbll.SaveForm("", checkentity);

                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(approveids))//������
                        {
                            string[] array = approveids.Split(',');
                            foreach (var item in array)
                            {
                                UserEntity user = userBLL.GetEntity(item);
                                if (user != null)
                                {
                                    var dept = departmentBLL.GetEntity(user.DepartmentId);
                                    HighRiskCheckEntity checkentity = new HighRiskCheckEntity();
                                    checkentity.ApprovePerson = item;
                                    checkentity.ApprovePersonName = user.RealName;
                                    checkentity.ADeptId = user.DepartmentId;
                                    checkentity.ADeptCode = user.DepartmentCode;
                                    checkentity.ADeptName = dept == null ? usercurrent.OrganizeName : dept.FullName;
                                    checkentity.ApproveStep = "2";//����
                                    checkentity.ApproveState = "0";
                                    checkentity.ApproveId = entity.Id;
                                    highriskcheckbll.SaveForm("", checkentity);

                                }
                            }
                        }
                    }
                }
            }
            return Success("�����ɹ���");
        }
        #endregion

        #region ����
        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult ExportData(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;

            pagination.p_kid = "a.Id";
            pagination.p_fields = "c.itemname as applystatename,b.itemname as worktype,workplace,workcontent,to_char(workstarttime,'yyyy-mm-dd hh24:mi') || ' - '||to_char(workendtime,'yyyy-mm-dd hh24:mi'),applyusername,applydeptname,to_char(a.createdate,'yyyy-mm-dd hh24:mi')";
            pagination.p_tablename = " bis_highriskapply a left join base_dataitemdetail b on a.worktype=b.itemvalue and b.itemid =(select itemid from base_dataitem where itemcode='WorkType') left join base_dataitemdetail c on a.applystate=c.itemvalue and c.itemid =(select itemid from base_dataitem where itemcode='WorkStatus')";
            pagination.conditionJson = "1=1";
            pagination.sidx = "a.createdate";
            pagination.sord = "desc";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " and applyuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += " and ApplyDeptCode='" + user.DeptCode + "'";
                            break;
                        case "3"://���Ӳ���
                            pagination.conditionJson += string.Format(" and ApplyDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", user.DeptCode, user.DeptId);
                            break;
                        case "4":
                            pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                            break;
                    }
                }
                else
                {
                    pagination.conditionJson += " and 0=1";
                }
            }
            DataTable exportTable = highriskapplybll.GetPageDataTable(pagination, queryJson);
            exportTable.Columns.Remove("id");
            exportTable.Columns.Remove("r");
            // ȷ�������ļ���
            string fileName = "�߷�����ҵ���������Ϣ";
            HttpResponse resp = System.Web.HttpContext.Current.Response;

            // ��ϸ�б�����
            string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            wb.Open(Server.MapPath("~/Resource/ExcelTemplate/�߷�����ҵ���������Ϣ.xlsx"));
            Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
            Aspose.Cells.Cell cell = sheet.Cells[1, 1];
            sheet.Cells.ImportDataTable(exportTable, false, 1, 0);
            wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);

            return Success("�����ɹ�!");
        }
        #endregion
    }
}
