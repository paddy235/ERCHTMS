using ERCHTMS.Entity.AccidentEvent;
using ERCHTMS.Busines.AccidentEvent;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Web.Areas.AccidentEvent.Controllers
{
    /// <summary>
    /// �� �����¹��¼����鴦��
    /// </summary>
    public class Bulletin_dealController : MvcControllerBase
    {
        private Bulletin_dealBLL bulletin_dealbll = new Bulletin_dealBLL();
        private BulletinBLL bulletinbll = new BulletinBLL();
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
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenericIndex()
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
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenericForm()
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
        //[HandlerMonitor(3, "��ҳ��ѯ�û���Ϣ!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = "IsSubmit_DEAL,CREATEUSERID ,SGNAME, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,DEALID,RSSHSGTYPENAME,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode";
            pagination.p_tablename = "V_AEM_BULLETIN_DEAL_ORDER t";
            pagination.conditionJson = "1=1";
            //pagination.sord = "HAPPENTIME";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }


            var pMode = Request["pMode"] ?? "";
            if (pMode.Length > 0)
            {
                pagination.conditionJson += " and IsSubmit_Deal>0 and to_char(createdate,'yyyy')='" + DateTime.Now.Year + "'";
            }

            var watch = CommonHelper.TimerStart();
            var data = bulletinbll.GetPageList(pagination, queryJson);
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
        //[HandlerMonitor(3, "��ҳ��ѯ�û���Ϣ!")]
        public ActionResult GetGenericPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = "IsSubmit_DEAL,CREATEUSERID ,SGNAME, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,DEALID,RSSHSGTYPENAME,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode";
            pagination.p_tablename = "V_AEM_BULLETIN_DEAL_ORDER t";
            pagination.conditionJson = "1=1";
            //pagination.sord = "HAPPENTIME";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }


            var pMode = Request["pMode"] ?? "";
            if (pMode.Length > 0)
            {
                pagination.conditionJson += " and IsSubmit_Deal>0 and to_char(createdate,'yyyy')='" + DateTime.Now.Year + "'";
            }

            var watch = CommonHelper.TimerStart();
            var data = bulletinbll.GetGenericPageList(pagination, queryJson);
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = bulletin_dealbll.GetList(queryJson);
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
            var data = bulletin_dealbll.GetEntity(keyValue);
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
            bulletin_dealbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, Bulletin_dealEntity entity)
        {
            string HAPPENTIME_DEAL = Request["HAPPENTIME_DEAL"] ?? "";
            if (HAPPENTIME_DEAL.Length > 0)
                entity.HAPPENTIME_DEAL = Convert.ToDateTime(HAPPENTIME_DEAL);
            bulletin_dealbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �¹��¼��챨
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "�¹��¼��챨")]
        public ActionResult ExportBulletinDealList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "SGNAME, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,case WHEN  IsSubmit_DEAL>0 then '�ѵ��鴦��' else 'δ���鴦��' end  as DCCLZT";
            pagination.p_tablename = "V_AEM_BULLETIN_DEAL_ORDER t";
            pagination.sord = "DCCLZT";
            #region Ȩ��У��
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            #endregion
            var data = bulletinbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "�¹��¼����鴦��";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "�¹��¼����鴦��.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "SGNAME".ToLower(), ExcelColumn = "�¹�/�¼�����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SGTYPENAME".ToLower(), ExcelColumn = "�¹ʻ��¼�����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "HAPPENTIME".ToLower(), ExcelColumn = "����ʱ��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "AREANAME".ToLower(), ExcelColumn = "�ص㣨����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SGKBUSERNAME".ToLower(), ExcelColumn = "�챨��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DCCLZT".ToLower(), ExcelColumn = "���鴦��״̬" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
