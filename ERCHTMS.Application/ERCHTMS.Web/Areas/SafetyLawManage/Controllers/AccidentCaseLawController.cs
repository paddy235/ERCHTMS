using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.Busines.SafetyLawManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Web;
using System;
using BSFramework.Util.Offices;
using System.Data;
using System.Collections.Generic;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Linq;

namespace ERCHTMS.Web.Areas.SafetyLawManage.Controllers
{
    /// <summary>
    /// �� �����¹ʰ�����
    /// </summary>
    public class AccidentCaseLawController : MvcControllerBase
    {
        private AccidentCaseLawBLL accidentcaselawbll = new AccidentCaseLawBLL();
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
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CaseForm()
        {
            return View();
        }
        /// <summary>
        /// �ҵ��ղ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult myStoreIndex()
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
            pagination.p_kid = "Id";
            pagination.p_fields = "CreateDate,FileName,AccRange,AccTime,Remark,FilesId,AccidentCompany,RelatedCompany,AccidentGrade,intDeaths,AccType,createuserid,createuserdeptcode,createuserorgcode";
            pagination.p_tablename = " bis_accidentCaseLaw";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
                //�糧��ȡʡ��˾�Ļ���ID
                if (user.RoleName.Contains("ʡ���û�"))
                {
                    pagination.conditionJson += " and ( createuserorgcode = '" + user.OrganizeCode + "' or createuserorgcode='00')";
                }
                else
                {
                    orgcodelist = departmentBLL.GetList().Where(t => user.NewDeptCode.Contains(t.DeptCode) && t.Nature == "ʡ��");
                    pagination.conditionJson += " and (";
                    foreach (DepartmentEntity item in orgcodelist)
                    {
                        pagination.conditionJson += "createuserorgcode ='" + item.EnCode + "' or ";
                    }
                    pagination.conditionJson += " createuserorgcode = '" + user.OrganizeCode + "' or createuserorgcode='00')";
                }
            }
            var data = accidentcaselawbll.GetPageDataTable(pagination, queryJson);
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
            var data = accidentcaselawbll.GetList(queryJson);
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
            var data = accidentcaselawbll.GetEntity(keyValue);
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
            accidentcaselawbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, AccidentCaseLawEntity entity)
        {
            entity.CaseSource = "0";//�ڲ�����
            accidentcaselawbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion




        #region ����
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
                pagination.p_fields = @"FileName,RelatedCompany,AccTime,case when AccidentGrade='1' then 'һ���¹�'
                                            when AccidentGrade ='2' then '�ϴ��¹�'
                                            when AccidentGrade='3'  then '�ش��¹�'
                                            when AccidentGrade='4'  then '�ر��ش��¹�' end AccidentGrade,intDeaths,AccType,
                                        case when AccRange='1' then '����λ�¹�'
                                            when AccRange ='2' then '�������¹�'
                                            when AccRange='3'  then '����ϵͳ�����¹�' end AccRange
                                    ,Remark,FilesId,AccidentCompany";
                pagination.p_tablename = " bis_accidentCaseLaw";
                pagination.conditionJson = "1=1";
                pagination.sidx = "createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
                    //�糧��ȡʡ��˾�Ļ���ID
                    if (user.RoleName.Contains("ʡ���û�"))
                    {
                        pagination.conditionJson += " and ( createuserorgcode = '" + user.OrganizeCode + "' or createuserorgcode='00')";
                    }
                    else
                    {
                        orgcodelist = departmentBLL.GetList().Where(t => user.NewDeptCode.Contains(t.DeptCode) && t.Nature == "ʡ��");
                        pagination.conditionJson += " and (";
                        foreach (DepartmentEntity item in orgcodelist)
                        {
                            pagination.conditionJson += "createuserorgcode ='" + item.EnCode + "' or ";
                        }
                        pagination.conditionJson += " createuserorgcode = '" + user.OrganizeCode + "' or createuserorgcode='00')";
                    }
                }
                DataTable exportTable = accidentcaselawbll.GetPageDataTable(pagination, queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "�¹ʰ�����Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "�¹ʰ�����Ϣ����.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "�¹�����", Width = 50 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "relatedcompany", ExcelColumn = "���µ�λ", Width = 50 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "acctime", ExcelColumn = "�¹�ʱ��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "accidentgrade", ExcelColumn = "�¹ʵȼ�", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "intdeaths", ExcelColumn = "��������", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "acctype", ExcelColumn = "�¹����", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "accrange", ExcelColumn = "���ݷ�Χ", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "remark", ExcelColumn = "��ע", Width = 15 });
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
