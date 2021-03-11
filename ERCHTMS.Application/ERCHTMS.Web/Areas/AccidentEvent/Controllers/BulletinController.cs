using ERCHTMS.Entity.AccidentEvent;
using ERCHTMS.Busines.AccidentEvent;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Linq.Expressions;
using System.Linq;
using System.IO;
using Aspose.Words.Saving;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Busines.Desktop;

namespace ERCHTMS.Web.Areas.AccidentEvent.Controllers
{
    /// <summary>
    /// �� �����¹��¼��챨
    /// </summary>
    public class BulletinController : MvcControllerBase
    {
        private BulletinBLL bulletinbll = new BulletinBLL();

        #region ��ͼ����
        /// <returns></returns>
        [HttpGet]
        public ActionResult PdBZ()
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
        /// ͨ�ð��ҳ��
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
            pagination.p_fields = "IsSubmit,CREATEUSERID,SGNAME, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,RSSHSGTYPENAME,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode";
            pagination.p_tablename = "V_AEM_BULLETIN_Order t";
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
            pagination.p_fields = "IsSubmit,CREATEUSERID,SGNAME, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,RSSHSGTYPENAME,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode";
            pagination.p_tablename = "V_AEM_BULLETIN_Order t";
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
            var data = bulletinbll.GetList(queryJson);
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
            var data = bulletinbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region �ύ����

        [HandlerMonitor(0, "��ʱ������")]
        public void Down(string keyValue)
        {
            DesktopBLL dbll=new DesktopBLL();
            bool flag = dbll.IsGeneric();
            //��ѯ����
            var entity = bulletinbll.GetEntity(keyValue);
            DepartmentBLL orgbll = new DepartmentBLL();
            var org = orgbll.GetEntity(ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeId);
            //���ݺϲ�
            string[] text = new string[] { "SGHSJLX", "ORGNAME", "ORGADDRESS", "ORGTEL", "HAPPENTIME", 
                "AREANAME", 
                "SGTYPENAME",
                "SGLEVELNAME", "JYJG", "SWNUM", "SZNUM", "ZSNUM", "SHQKSHJE", "TDQK", "CBYY", "HFQK", "DEPARTMENTNAME", "SGKBUSERNAME", "MOBILE" };
            string[] value;
            var tempPath = Server.MapPath("~/Resource/Temp/�����¹�(�¼�)��ʱ���浥.doc");
            var outputPath = Server.MapPath("~/Resource/ExcelTemplate/�����¹�(�¼�)��ʱ���浥.doc");
            //�Ƿ���ͨ����ҵ��
            if (flag)
            {
                value = new string[] { 
                    (entity.SGTYPENAME.Contains("�¹�") ? "�¹ʱ��� �̡���          �¼����� ��" : " �¹ʱ��� ������          �¼����� ��"),
                    org.FullName,
                    "",
                    org.OuterPhone,
                    entity.HAPPENTIME.Value.ToString(),
                    entity.AREANAME,
                    entity.RSSHSGTYPENAME,
                    entity.SGLEVELNAME,
                    entity.JYJG,
                    entity.SWNUM.ToString(),
                    entity.SZNUM.ToString(),
                    entity.ZSNUM.ToString(),
                    entity.SHQKSHJE.ToString(),
                    entity.TDQK,
                    entity.CBYY,
                    entity.HFQK,
                    entity.DEPARTMENTNAME,
                    entity.SGKBUSERNAME,
                    entity.MOBILE
                };
                tempPath = Server.MapPath("~/Resource/Temp/�¹�(�¼�)��ʱ���浥.doc");
                outputPath = Server.MapPath("~/Resource/ExcelTemplate/�¹�(�¼�)��ʱ���浥.doc");
            }
            else
            {
                value = new string[] { 
                    (entity.SGTYPENAME.Contains("�¹�") ? "�¹ʱ��� �̡���          �¼����� ��" : " �¹ʱ��� ������          �¼����� ��"),
                    org.FullName,
                    "",
                    org.OuterPhone,
                    entity.HAPPENTIME.Value.ToString(),
                    entity.AREANAME,
                    entity.SGTYPENAME,
                    entity.SGLEVELNAME,
                    entity.JYJG,
                    entity.SWNUM.ToString(),
                    entity.SZNUM.ToString(),
                    entity.ZSNUM.ToString(),
                    entity.SHQKSHJE.ToString(),
                    entity.TDQK,
                    entity.CBYY,
                    entity.HFQK,
                    entity.DEPARTMENTNAME,
                    entity.SGKBUSERNAME,
                    entity.MOBILE
                };
            }

           

           
            Aspose.Words.Document doc = new Aspose.Words.Document(tempPath);
            //���ݺϲ�
            doc.MailMerge.Execute(text, value);
            doc.Save(outputPath);
            //���õ�������
            var docStream = new MemoryStream();
            doc.Save(docStream, SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
            Response.ContentType = "application/msword";
            if (flag)
            {
                Response.AddHeader("content-disposition", "attachment;filename=�¹�(�¼�)��ʱ���浥.doc");
            }
            else
            {
                Response.AddHeader("content-disposition", "attachment;filename=�����¹�(�¼�)��ʱ���浥.doc");
            }

            Response.BinaryWrite(docStream.ToArray());
            Response.End();
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
            Bulletin_dealBLL bulletin_dealbll = new Bulletin_dealBLL();

            foreach (var item in keyValue.Split(','))
            {
                bulletinbll.RemoveForm(item);
                Expression<Func<Bulletin_dealEntity, bool>> condition = e => e.BULLETINID == item;
                var list = bulletin_dealbll.GetListForCon(condition);
                if (list != null && list.Count() > 0)
                {
                    var keyvaluedeal = list.FirstOrDefault().ID;
                    bulletin_dealbll.RemoveForm(keyvaluedeal);
                }
            }
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
        public ActionResult SaveForm(string keyValue, BulletinEntity entity)
        {
            string HAPPENTIME = Request["HAPPENTIME"] ?? "";
            if (HAPPENTIME.Length > 0)
                entity.HAPPENTIME = DateTime.Parse(HAPPENTIME);
            bulletinbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �¹��¼��챨
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "�¹��¼��챨")]
        public ActionResult ExportBulletinList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "SGNAME, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,case WHEN  IsSubmit>0 then '���ύ' else 'δ�ύ' end  as IsSubmitStr";
            pagination.p_tablename = "V_AEM_BULLETIN_Order t";
            pagination.sord = "IsSubmitStr";
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
            excelconfig.Title = "�¹��¼��챨";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "�¹��¼��챨.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "SGNAME".ToLower(), ExcelColumn = "�¹�/�¼�����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SGTYPENAME".ToLower(), ExcelColumn = "�¹ʻ��¼�����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "HAPPENTIME".ToLower(), ExcelColumn = "����ʱ��", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "AREANAME".ToLower(), ExcelColumn = "�ص㣨����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SGKBUSERNAME".ToLower(), ExcelColumn = "�챨��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "IsSubmitStr".ToLower(), ExcelColumn = "�Ƿ��ύ" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
