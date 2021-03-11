using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.Busines.StandardSystem;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Data;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.StandardSystem.Controllers
{
    /// <summary>
    /// �� �����ղر�׼
    /// </summary>
    public class StorestandardController : MvcControllerBase
    {

        private StorestandardBLL storestandardbll = new StorestandardBLL();
        private StandardsystemBLL standardsystembll = new StandardsystemBLL();

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
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "c.storeid";
            pagination.p_fields = "a.id,filename,b.name as categorycode,relevantelementname,relevantelementid,carrydate,a.createdate,consultnum,d.fullname as createuserdeptname";
            pagination.p_tablename = "hrs_storestandard c left join hrs_standardsystem a on c.standardid = a.id left join hrs_stcategory b on a.categorycode=b.id left join base_department d on a.createuserdeptcode = d.encode";
            pagination.conditionJson = "c.userid='" + user.UserId + "'";
            var data = standardsystembll.GetPageList(pagination, queryJson);
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
            var data = storestandardbll.GetList(queryJson);
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
            var data = storestandardbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "c.storeid";
                if (queryParam["standardtype"].ToString() == "1" || queryParam["standardtype"].ToString() == "2" || queryParam["standardtype"].ToString() == "3" || queryParam["standardtype"].ToString() == "4" || queryParam["standardtype"].ToString() == "5" || queryParam["standardtype"].ToString() == "6")
                {
                    pagination.p_fields = "filename,b.name as categorycode,relevantelementname,to_char(carrydate,'yyyy-MM-dd') as carrydate,to_char(a.createdate,'yyyy-MM-dd') as createdate,consultnum";
                }
                else
                {
                    pagination.p_fields = "filename,to_char(a.createdate,'yyyy-MM-dd') as createdate,d.fullname as createuserdeptname,consultnum";
                }
                pagination.p_tablename = "hrs_storestandard c left join hrs_standardsystem a on c.standardid = a.id left join hrs_stcategory b on a.categorycode=b.id left join base_department d on a.createuserdeptcode = d.encode";
                pagination.conditionJson = "c.userid='" + user.UserId + "'";
                pagination.sidx = "a.createdate";
                pagination.sord = "desc";
                DataTable exportTable = standardsystembll.GetPageList(pagination, queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                if (!queryParam["standardtype"].IsEmpty())
                {
                    switch (queryParam["standardtype"].ToString())
                    {
                        case "1":
                            excelconfig.Title = "������׼��ϵ";
                            excelconfig.FileName = "������׼��ϵ��Ϣ����.xls";
                            break;
                        case "2":
                            excelconfig.Title = "�����׼��ϵ";
                            excelconfig.FileName = "�����׼��ϵ��Ϣ����.xls";
                            break;
                        case "3":
                            excelconfig.Title = "��λ��׼��ϵ";
                            excelconfig.FileName = "��λ��׼��ϵ��Ϣ����";
                            break;
                        case "4":
                            excelconfig.Title = "�ϼ���׼���ļ�";
                            excelconfig.FileName = "�ϼ���׼���ļ���Ϣ����.xls";
                            break;
                        case "5":
                            excelconfig.Title = "ָ����׼";
                            excelconfig.FileName = "ָ����׼��Ϣ����.xls";
                            break;
                        case "6":
                            excelconfig.Title = "���ɷ���";
                            excelconfig.FileName = "���ɷ�����Ϣ����.xls";
                            break;
                        case "7":
                            excelconfig.Title = "��׼��ϵ�߻��빹��";
                            excelconfig.FileName = "��׼��ϵ�߻��빹����Ϣ����.xls";
                            break;
                        case "8":
                            excelconfig.Title = "��׼��ϵ������Ľ�";
                            excelconfig.FileName = "��׼��ϵ������Ľ���Ϣ����.xls";
                            break;
                        case "9":
                            excelconfig.Title = "��׼����ѵ";
                            excelconfig.FileName = "��׼����ѵ��Ϣ����.xls";
                            break;
                        default:
                            break;
                    }
                }
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "�ļ�����", Width = 300 });
                if (queryParam["standardtype"].ToString() == "1" || queryParam["standardtype"].ToString() == "2" || queryParam["standardtype"].ToString() == "3" || queryParam["standardtype"].ToString() == "4" || queryParam["standardtype"].ToString() == "5" || queryParam["standardtype"].ToString() == "6")
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "categorycode", ExcelColumn = "���", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "relevantelementname", ExcelColumn = "��ӦԪ��", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "ʩ������", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "��������", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "consultnum", ExcelColumn = "����Ƶ��", Width = 300 });
                }
                else
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "��������", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createuserdeptname", ExcelColumn = "������λ/����", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "consultnum", ExcelColumn = "����Ƶ��", Width = 300 });
                }
                //���õ�������
                //ExcelHelper.ExcelDownload(exportTable, excelconfig);
                ExcelHelper.ExportByAspose(exportTable, excelconfig.FileName, excelconfig.ColumnEntity);

            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="idsData">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string idsData)
        {
            if (!string.IsNullOrEmpty(idsData))
            {
                if (idsData.Contains(","))
                {
                    string[] array = idsData.TrimEnd(',').Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        storestandardbll.RemoveForm(array[i]);
                    }
                }
                else
                {
                    storestandardbll.RemoveForm(idsData);
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
        public ActionResult SaveForm(string keyValue, StorestandardEntity entity)
        {
            storestandardbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="idsData">��׼����</param>
        /// <param name="standardType">��׼����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult StoreStandard(string idsData, string standardType)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(idsData))
            {
                if (idsData.Contains(","))
                {
                    string[] array = idsData.TrimEnd(',').Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        int result = storestandardbll.GetStoreByStandardID(array[i].ToString());
                        if (result == 0)
                        {
                            StorestandardEntity entity = new StorestandardEntity();
                            entity.USERID = user.UserId;
                            entity.STANDARDID = array[i].ToString();
                            entity.STANDARDTYPE = standardType;
                            entity.STORETIME = DateTime.Now;
                            storestandardbll.SaveForm("", entity);
                        }
                    }
                }
                else
                {
                    int result = storestandardbll.GetStoreByStandardID(idsData);
                    if (result == 0)
                    {
                        StorestandardEntity entity = new StorestandardEntity();
                        entity.USERID = user.UserId;
                        entity.STANDARDID = idsData;
                        entity.STANDARDTYPE = standardType;
                        entity.STORETIME = DateTime.Now;
                        storestandardbll.SaveForm("", entity);
                    }
                }
                return Success("�ղسɹ���");
            }
            return Error("�������ղء�");
        }
        #endregion
    }
}
