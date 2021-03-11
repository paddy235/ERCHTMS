using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.Busines.SafetyLawManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Code;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.SafetyLawManage.Controllers
{
    /// <summary>
    /// �� �����ղط��ɷ���
    /// </summary>
    public class StoreLawController : MvcControllerBase
    {
        private StoreLawBLL storelawbll = new StoreLawBLL();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = storelawbll.GetList(queryJson);
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
            var data = storelawbll.GetEntity(keyValue);
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
            if (!string.IsNullOrEmpty(keyValue))
            {
                if (keyValue.Contains(","))
                {
                    string[] array = keyValue.TrimEnd(',').Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        storelawbll.RemoveForm(array[i]);
                    }
                }
            }
            return Success("ȡ���ղسɹ���");
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
        public ActionResult SaveForm(string keyValue, StoreLawEntity entity)
        {
            storelawbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion


        /// <summary>
        /// ����������ȡ����
        /// </summary>
        [HttpPost]
        [AjaxOnly]
        public ActionResult storeSafetyLaw(string idsData, string ctype)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(idsData))
            {
                if (idsData.Contains(","))
                {
                    string[] array = idsData.TrimEnd(',').Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        int result = storelawbll.GetStoreBylawId(array[i].ToString());
                        if (result == 0)
                        {
                            StoreLawEntity entity = new StoreLawEntity();
                            entity.UserId = user.UserId;
                            entity.LawId = array[i].ToString();
                            entity.cType = ctype;
                            entity.StoreTime = DateTime.Now;
                            storelawbll.SaveForm("", entity);
                        }
                    }
                }
                return Success("�ղسɹ���");
            }
            return Error("�������ղء�");
        }


        #region  �ҵ��ղ�
        #region ��ȫ�������ɷ���
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "storeid";
            pagination.p_fields = "a.lawid,b.CreateDate,FileName,LawArea,IssueDept,FileCode,ValidVersions,CarryDate,FilesId,effetstate,updatedate,channeltype";
            pagination.p_tablename = " bis_storelaw a left join bis_safetylaw b on a.lawid=b.id";
            pagination.conditionJson = "userid='" + user.UserId + "' and ctype='1'";
            var data = storelawbll.GetPageDataTable(pagination, queryJson);
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
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportDataLaw(string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "storeid";
                pagination.p_fields = @"FileName,FileCode,to_char(b.CreateDate,'yyyy-MM-dd') as CreateDate,to_char(CarryDate,'yyyy-MM-dd') as CarryDate,IssueDept,case when Effetstate='1' then '������Ч'
                                            when Effetstate ='2' then '����ʵʩ'
                                            when Effetstate='3'  then '���޶�'
                                            when Effetstate='4'  then '��ֹ' end Effetstate";
                pagination.p_tablename = " bis_storelaw a left join bis_safetylaw b on a.lawid=b.id";
                pagination.conditionJson = "userid='" + user.UserId + "' and ctype='1'";
                DataTable exportTable = storelawbll.GetPageDataTable(pagination, queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "��ȫ�������ɷ�����Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "��ȫ�������ɷ�����Ϣ����.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "��������", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filecode", ExcelColumn = "�ĺ�/��׼��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "����ʱ��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "ʵʩ����", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedept", ExcelColumn = "��������", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "effetstate", ExcelColumn = "ʱЧ��", Width = 15 });
                //���õ�������
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }
        #endregion


        #region ��ȫ�����ƶ�
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageJsonInstitution(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "storeid as id";
            pagination.p_fields = "c.lawid,t.CreateDate,t.FileName,t.IssueDept,t.FileCode,t.ValidVersions,t.CarryDate,t.FilesId,t.releasedate,t.revisedate,t.lawtypename";
            pagination.p_tablename = " bis_storelaw c left join (select id as lid,CreateDate,FileName,IssueDept,FileCode,ValidVersions,CarryDate,FilesId,releasedate,revisedate,lawtypename,LawTypeCode,createuserorgcode from  bis_safeinstitution a ) t on c.lawid=t.lid";
            pagination.conditionJson = string.Format("userid='{0}' and ctype='2'", user.UserId);
            var data = storelawbll.GetPageJsonInstitution(pagination, queryJson);
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


        #region ����
        /// <summary>
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportDataInstitution(string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "storeid";
                pagination.p_fields = "FileName,FileCode,IssueDept,to_char(releasedate,'yyyy-MM-dd') as releasedate,to_char(revisedate,'yyyy-MM-dd') as revisedate,to_char(carrydate,'yyyy-MM-dd') as carrydate,lawtypename";
                pagination.p_tablename = " bis_storelaw c left join (select id as lid,CreateDate,FileName,IssueDept,FileCode,ValidVersions,CarryDate,FilesId,releasedate,revisedate,lawtypename,LawTypeCode,createuserorgcode from  bis_safeinstitution a ) t on c.lawid=t.lid";
                pagination.conditionJson = string.Format("userid='{0}' and ctype='2'", user.UserId);
                DataTable exportTable = storelawbll.GetPageJsonInstitution(pagination, queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "��ȫ�����ƶ���Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "��ȫ�����ƶ���Ϣ����.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "�ļ�����", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filecode", ExcelColumn = "�ļ����", Width = 25 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedept", ExcelColumn = "������λ(����)", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "releasedate", ExcelColumn = "����ʱ��", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "revisedate", ExcelColumn = "�޶�ʱ��", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "ʵʩʱ��", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "lawtypename", ExcelColumn = "����", Width = 20 });
                //���õ�������
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }
        #endregion
        #endregion


        #region ��ȫ�������
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageJsonStandards(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "storeid as id";
            pagination.p_fields = "c.lawid,t.CreateDate,t.FileName,t.IssueDept,t.FileCode,t.ValidVersions,t.CarryDate,t.FilesId,t.releasedate,t.revisedate,t.lawtypename";
            pagination.p_tablename = " bis_storelaw c left join (select id as lid,CreateDate,FileName,IssueDept,FileCode,ValidVersions,CarryDate,FilesId,releasedate,revisedate,lawtypename,LawTypeCode,createuserorgcode from  bis_safestandards a ) t on c.lawid=t.lid";
            pagination.conditionJson = string.Format("userid='{0}' and ctype='3'", user.UserId);
            var data = storelawbll.GetPageJsonStandards(pagination, queryJson);
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
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportDataStandards(string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "storeid";
                pagination.p_fields = "FileName,FileCode,IssueDept,to_char(releasedate,'yyyy-MM-dd') as releasedate,to_char(revisedate,'yyyy-MM-dd') as revisedate,to_char(carrydate,'yyyy-MM-dd') as carrydate,lawtypename";
                pagination.p_tablename = " bis_storelaw c left join (select id as lid,CreateDate,FileName,IssueDept,FileCode,ValidVersions,CarryDate,FilesId,releasedate,revisedate,lawtypename,LawTypeCode,createuserorgcode from  bis_safestandards a ) t on c.lawid=t.lid";
                pagination.conditionJson = string.Format("userid='{0}' and ctype='3'", user.UserId);
                DataTable exportTable = storelawbll.GetPageJsonStandards(pagination, queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "��ȫ���������Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "��ȫ������̵���.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "�ļ�����", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filecode", ExcelColumn = "�ļ����", Width = 25 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedept", ExcelColumn = "������λ(����)", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "releasedate", ExcelColumn = "����ʱ��", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "revisedate", ExcelColumn = "�޶�ʱ��", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "ʵʩʱ��", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "lawtypename", ExcelColumn = "����", Width = 20 });
                //���õ�������
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }
        #endregion

        #region �¹ʰ���
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJsonCase(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "storeid";
            pagination.p_fields = "a.lawid,b.CreateDate,FileName,AccRange,AccTime,Remark,FilesId,AccidentCompany,RelatedCompany,AccidentGrade,intDeaths,AccType";
            pagination.p_tablename = " bis_storelaw a left join bis_accidentCaseLaw b on a.lawid=b.id";
            pagination.conditionJson = "userid='" + user.UserId + "' and ctype='6'";
            var data = storelawbll.GetPageDataTable(pagination, queryJson);
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
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportDataCase(string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "storeid";
                pagination.p_fields = @"FileName,RelatedCompany,AccTime,case when AccidentGrade='1' then 'һ���¹�'
                                            when AccidentGrade ='2' then '�ϴ��¹�'
                                            when AccidentGrade='3'  then '�ش��¹�'
                                            when AccidentGrade='4'  then '�ر��ش��¹�' end AccidentGrade,intDeaths,AccType,
                                        case when AccRange='1' then '����λ�¹�'
                                            when AccRange ='2' then '�������¹�'
                                            when AccRange='3'  then '����ϵͳ�����¹�' end AccRange
                                    ,Remark,FilesId,AccidentCompany";
                pagination.p_tablename = " bis_storelaw a left join bis_accidentCaseLaw b on a.lawid=b.id";
                pagination.conditionJson = "userid='" + user.UserId + "' and ctype='6'";
                DataTable exportTable = storelawbll.GetPageDataTable(pagination, queryJson);
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
        #endregion
    }
}
