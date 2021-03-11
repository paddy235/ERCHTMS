using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Cache.Factory;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using BSFramework.Util.Offices;
using System.Text;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// �� �����豸��¼��Ա������־
    /// </summary>
    public class HikinoutlogController : MvcControllerBase
    {
        private HikinoutlogBLL hikinoutlogbll = new HikinoutlogBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            ViewBag.KMHikImgIp = pdata.GetItemValue("KMHikImgIp");//����ͼƬ����ip��ַ
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
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "username,deptname,devicename,areaname,inout,devicetype,eventtype,screenshot,createdate";
            pagination.p_tablename = @"BIS_HIKINOUTLOG";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //if (user.IsSystem)
            //{
            pagination.conditionJson = "1=1";
            //}
            //else
            //{




            //    string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
            //    pagination.conditionJson += " and " + where;




            //}

            var data = hikinoutlogbll.GetPageList(pagination, queryJson);

            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// ��ȡȫ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPersonNums()
        {
            var dt = hikinoutlogbll.GetNums();
            List<string> eareaNames = new List<string>();
            eareaNames.Add("һ�Ÿ�");
            eareaNames.Add("���Ÿ�");
            eareaNames.Add("��ͷ��");
            List<int> userType = new List<int>();
            userType.Add(0);
            userType.Add(1);
            userType.Add(2);
            List<dynamic> PersonData = new List<dynamic>();
            foreach (int type in userType)
            {
                DataRow[] typeRows = dt.Select(string.Format(" type='{0}'", type));
                int station1 = 0, station2 = 0, station3 = 0;
                foreach (DataRow row in typeRows)
                {
                    if (row[2].ToString() == "һ�Ÿ�")
                        station1 = Convert.ToInt32(row[1]);
                    if (row[2].ToString() == "���Ÿ�")
                        station2 = Convert.ToInt32(row[1]);
                    if (row[2].ToString() == "��ͷ��")
                        station3 = Convert.ToInt32(row[1]);
                }
                PersonData.Add(new
                {
                    userType = type,
                    stationCount1 = station1,
                    stationCount2 = station2,
                    stationCount3 = station3,
                    total = station1 + station2 + station3
                });
            }
            var CarData = hikinoutlogbll.GetCarStatistic();

            var LastData = hikinoutlogbll.GetLastInoutLog();

            var returnData = new { PersonData, CarData, LastData };

            return Content(returnData.ToJson());
        }

        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = hikinoutlogbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        #region �豸��ʵʱ���
        [HttpGet]
        public ActionResult DeviceWatch()
        {
            
            var cacheKey = "DeviceWatch";//�������ֵ
            var cacheService = CacheFactory.Cache();
            HikinoutlogEntity cacheValue = cacheService.GetCache<HikinoutlogEntity>(cacheKey);
            if (cacheValue == null )
            {
                cacheValue =    hikinoutlogbll.GetFirsetData();
                //д�뻺��
                Task.Run(() =>
                {
                    cacheService.WriteCache(cacheValue, cacheKey, DateTime.Now.AddSeconds(6));
                });
            }
            return ToJsonResult(cacheValue);
        }
        #endregion
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
            hikinoutlogbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HikinoutlogEntity entity)
        {
            hikinoutlogbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

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
                pagination.p_kid = "ID";
                pagination.p_fields = @"username,deptname,devicename,areaname,inout,devicetype,eventtype,screenshot,createdate, 
                                                        case
                                                        when inout = 0 then '����'
                                                        when inout = 1 then '����'
                                                        else ''
                                                        end inoutname,
                                                        case
                                                        when EVENTTYPE = 1 then '����ͨ���¼�'
                                                        when EVENTTYPE = 2 then '���������¼�'
                                                        when EVENTTYPE = 3 then '�Ž�ˢ���¼�'
                                                        when EVENTTYPE = 4 then '�Ž�ָ��ͨ���¼�'
                                                        else ''
                                                        end eventtypename ";
                pagination.p_tablename = @"BIS_HIKINOUTLOG";
                pagination.conditionJson = " 1=1";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DataTable exportTable = hikinoutlogbll.GetPageList(pagination, queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "��Ա������¼";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "��Ա������¼����.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "username", ExcelColumn = "����", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "����", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "devicename", ExcelColumn = "�Ž���", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "areaname", ExcelColumn = "�Ž�������", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "ʱ��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "inoutname", ExcelColumn = "��/��", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "eventtypename", ExcelColumn = "�¼�����", Width = 20 });

                //���õ�������
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }
    }
}
