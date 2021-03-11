using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� ������վ�ල��Ա(�߷�����ҵ)
    /// </summary>
    public class SidePersonController : MvcControllerBase
    {
        private SidePersonBLL sidepersonbll = new SidePersonBLL();

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
        /// ѡ��ලԱ����
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
            pagination.p_kid = "a.Id as sideid";
            pagination.p_fields = "SideUserDeptId,SideUserId,a.CreateDate,SideUserName,SideUserSex,SideUserIdCard,SideUserDeptName,itemname as  SideUserLevel,a.createuserid,a.createuserdeptcode,a.createuserorgcode";
            pagination.p_tablename = " bis_sideperson a left join base_dataitemdetail b on a.SideUserLevel=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='SideLevel')";
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
                            pagination.conditionJson += " and a.createuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += " and a.createuserdeptcode='" + user.DeptCode + "'";
                            break;
                        case "3":
                            pagination.conditionJson += " and a.createuserdeptcode like'" + user.DeptCode + "%'";
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
            var data = sidepersonbll.GetPageDataTable(pagination, queryJson);
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
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetSelectPersonJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "a.Id as sideid";
            pagination.p_fields = "SideUserDeptId,SideUserId,a.CreateDate,SideUserName,SideUserSex,SideUserIdCard,SideUserDeptName,itemname as  SideUserLevel,a.createuserid,a.createuserdeptcode,a.createuserorgcode";
            pagination.p_tablename = " bis_sideperson a left join base_dataitemdetail b on a.SideUserLevel=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='SideLevel')";
            pagination.conditionJson = string.Format("a.createuserorgcode='{0}'", user.OrganizeCode);
            var data = sidepersonbll.GetPageDataTable(pagination, queryJson);
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
            var data = sidepersonbll.GetList(queryJson);
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
            var data = sidepersonbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// �Ƿ���ڼල��Ա
        /// </summary>
        /// <param name="userid"></param>
        [HttpGet]
        public bool ExistSideUser(string userid)
        {
            var data = sidepersonbll.ExistSideUser(userid);
            return data;
        }

        /// <summary>
        ///���мල��Ա
        /// </summary>
        /// <param name="userid"></param>
        [HttpGet]
        public string AllSidePerson()
        {
            string siduserids = "";
            var list = sidepersonbll.GetList("");
            foreach (SidePersonEntity item in list)
            {
                siduserids += item.SideUserId + ",";
            }
            if (!string.IsNullOrEmpty(siduserids))
            {
                siduserids = siduserids.TrimEnd(',');
            }
            return siduserids;
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
            sidepersonbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SidePersonEntity entity)
        {
            sidepersonbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion


        #region
        /// <summary>
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "Id";
                pagination.p_fields = "SideUserName,SideUserSex,SideUserIdCard,SideUserDeptName,itemname as SideUserLevel";
                pagination.p_tablename = " bis_sideperson a left join base_dataitemdetail b on a.SideUserLevel=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='SideLevel')";
                pagination.conditionJson = "1=1";
                pagination.sidx = "a.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {
                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and a.createuserid='" + user.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and a.createuserdeptcode='" + user.DeptCode + "'";
                                break;
                            case "3":
                                pagination.conditionJson += " and a.createuserdeptcode like'" + user.DeptCode + "%'";
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
                DataTable exportTable = sidepersonbll.GetPageDataTable(pagination, queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "��վ�ල��Ա��Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "��վ�ල��Ա��Ϣ����.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sideusername", ExcelColumn = "����", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sideusersex", ExcelColumn = "�Ա�", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sideuseridcard", ExcelColumn = "���֤��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sideuserdeptname", ExcelColumn = "��λ/����", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sideuserlevel", ExcelColumn = "��վ�ල����", Width = 10 });
                //���õ�������
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }
        #endregion
    }
}
