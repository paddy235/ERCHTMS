using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.EnvironmentalManage;
using ERCHTMS.Busines.EnvironmentalManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Web.Areas.EnvironmentalManage.Controllers
{
    /// <summary>
    /// �� �����������
    /// </summary>
    public class NoisecheckController : MvcControllerBase
    {
        private NoisecheckBLL noisecheckbll = new NoisecheckBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ehsDepartCode = "";
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
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
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "ID";
            pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,checkuserid,checkusername,to_char(checkdate,'yyyy-MM-dd') checkdate
            ,zj1,zj2,zj3,zj4,zj5,zj6,yj1,yj2,yj3,yj4,yj5,yj6";
            pagination.p_tablename = @"BIS_NoiseCheck a ";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }


            var watch = CommonHelper.TimerStart();
            var data = noisecheckbll.GetPageList(pagination, queryJson);
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
            var data = noisecheckbll.GetList(queryJson);
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
            var data = noisecheckbll.GetEntity(keyValue);
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
            noisecheckbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, NoisecheckEntity entity)
        {
            noisecheckbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ����excel�б�

        /// <summary>
        /// ������ⱨ��
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "������ⱨ��")]
        public ActionResult exportExcelData(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = @"checkusername,zj1,yj1,zj2,yj2,zj3,yj3,zj4,yj4,zj5,yj5,zj6,yj6,to_char(checkdate,'yyyy-MM-dd') checkdate";
            pagination.p_tablename = @"BIS_NoiseCheck a ";
            pagination.sord = "CreateDate";
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
            var data = noisecheckbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "������ⱨ��";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "������ⱨ��.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();
            listColumnEntity.Add(new ColumnEntity() { Column = "checkusername", ExcelColumn = "�����Ա" });
            listColumnEntity.Add(new ColumnEntity() { Column = "zj1", Width = 20, ExcelColumn = "1#����������λ���" });
            listColumnEntity.Add(new ColumnEntity() { Column = "yj1", Width = 20, ExcelColumn = "1#����������λҹ��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "zj2", Width = 20, ExcelColumn = "2#����������λ���" });
            listColumnEntity.Add(new ColumnEntity() { Column = "yj2", Width = 20, ExcelColumn = "2#����������λҹ��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "zj3", Width = 20, ExcelColumn = "3#����������λ���" });
            listColumnEntity.Add(new ColumnEntity() { Column = "yj3", Width = 20, ExcelColumn = "3#����������λҹ��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "zj4", Width = 20, ExcelColumn = "4#����������λ���" });
            listColumnEntity.Add(new ColumnEntity() { Column = "yj4", Width = 20, ExcelColumn = "4#����������λҹ��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "zj5", Width = 20, ExcelColumn = "5#����������λ���" });
            listColumnEntity.Add(new ColumnEntity() { Column = "yj5", Width = 20, ExcelColumn = "5#����������λҹ��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "zj6", Width = 20, ExcelColumn = "6#����������λ���" });
            listColumnEntity.Add(new ColumnEntity() { Column = "yj6", Width = 20, ExcelColumn = "6#����������λҹ��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "checkdate", ExcelColumn = "�������" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }

        #endregion
    }
}
