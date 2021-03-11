using System;
using System.Data;
using ERCHTMS.Entity.EnvironmentalManage;
using ERCHTMS.Busines.EnvironmentalManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Web.Areas.EnvironmentalManage.Controllers
{
    /// <summary>
    /// �� ����ˮ�ʷ���
    /// </summary>
    public class WaterqualityController : MvcControllerBase
    {
        private WaterqualityBLL waterqualitybll = new WaterqualityBLL();
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
        /// <summary>
        /// �ο�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Standard()
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
            pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,a.sampleno,b.itemname as sampletype,a.testusername,to_char(testdate,'yyyy-MM-dd')as testdate";
            pagination.p_tablename = @"bis_waterquality a left join  ( select itemname,itemvalue from base_dataitemdetail where  itemid = 
                        (select itemid from base_dataitem where itemname = 'ˮ�ʷ���' ))  b on a.sampletype = b.itemvalue";
            //pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,sampleno,sampletype,testusername,to_char(testdate,'yyyy-MM-dd') testdate";
            //pagination.p_tablename = @"bis_waterquality a ";
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
            var data = waterqualitybll.GetPageList(pagination, queryJson);
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
            var data = waterqualitybll.GetList(queryJson);
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
            var data = waterqualitybll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�ο���׼
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetStandardJson(string sampletype)
        {
            var data = waterqualitybll.GetStandardJson(sampletype);
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
            waterqualitybll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, WaterqualityEntity entity)
        {
            waterqualitybll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion


        #region 
        [HandlerMonitor(0, "����ˮ�ʷ�������Word")]
        public ActionResult ExportWaterqualityWord(Pagination pagination, string keyValue)
        {
            try
            {
                pagination.p_fields = @"a.sampleno ,sampletype,a.testusername,to_char(testdate,'yyyy-MM-dd')as testdate,PH,XFW,CODCR,FHW,ZS,ZYD,GE,GONG,ZGE,ZL,ZQ,ZX,ZXIU,DZWY";
                pagination.p_tablename = @"bis_waterquality a";
                //pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,sampleno,sampletype,testusername,to_char(testdate,'yyyy-MM-dd') testdate";
                //pagination.p_tablename = @"bis_waterquality a ";
                pagination.conditionJson = "1=1";
                pagination.p_kid = "ID";
                pagination.page = 1;
                pagination.rows = 1;
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

                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    keyvalue = keyValue               
                }); 
                
                var data = waterqualitybll.GetPageList(pagination, queryJson);
                string sampletype = data.Rows[0]["SampleType"].ToString();
       

                string queryJsonree = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    sampletype = sampletype
                });

                //��ȡ����ָ��
                IEnumerable<WaterrecordEntity> list = new WaterrecordBLL().GetList(queryJsonree);
                int kpisum = 0;
                foreach (var entity in list)
                {
                    if (entity.PROJECTCODE == "PH")
                    {
                        var ph = !string.IsNullOrEmpty(data.Rows[0]["PH"].ToString())? Convert.ToDouble(data.Rows[0]["PH"]):0;
                        string[] kpi = entity.KPITARGET.Split('~');
                        if (ph < Convert.ToDouble(kpi[0]) || ph > Convert.ToDouble(kpi[1]))
                        {
                            kpisum++;
                        }
                    }
                    else
                    {
                        var item = !string.IsNullOrEmpty(data.Rows[0][entity.PROJECTCODE].ToString()) ? Convert.ToDouble(data.Rows[0][entity.PROJECTCODE]) : 0;
                        string[] sp = entity.KPITARGET.Split('��');
                        if (item > Convert.ToDouble(sp[1]))
                        {
                            kpisum++;
                        }              
                    }                 
                    
                }

                data.Columns.Add("Result");

                if (kpisum == 0)
                {
                    data.Rows[0]["Result"] = "���μ����Ŀ��δ���ꡣ";
                }
                else
                {
                    data.Rows[0]["Result"]= "���μ����ڳ�����Ŀ��";
                }

                if (!System.IO.Directory.Exists(Server.MapPath("~/Resource/ExcelTemplate")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Resource/ExcelTemplate"));
                }

                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/ExcelTemplate/ˮ�ʷ������鱨��ģ��" + sampletype + ".docx"));
                Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
                doc.MailMerge.Execute(data);
                doc.MailMerge.DeleteFields();
                string filePath = Server.MapPath("~/Resource/temp/" + "ˮ�ʷ������鱨��" + ".doc");
                doc.Save(filePath);
                string url = "../../Utility/DownloadFile?filePath=" + filePath + "&speed=102400&newFileName=" + "ˮ�ʷ������鱨��" + ".doc";
                return Redirect(url);        

                #region  �Զ���table
                //DataTable dt = new DataTable("U");
                //dt.Columns.Add("SampleNo");
                //dt.Columns.Add("TestDate");                   
                //dt.Columns.Add("PH");
                //dt.Columns.Add("CODCR");
                //dt.Columns.Add("FHW");
                //dt.Columns.Add("ZS");
                //dt.Columns.Add("ZYD");
                //dt.Columns.Add("GE");
                //dt.Columns.Add("GONG");
                //dt.Columns.Add("Result");
                //if (data.Rows.Count > 0)
                //{
                //    foreach (DataRow item in data.Rows)
                //    {
                //        DataRow dr =  dt.NewRow();
                //        dr["SampleNo"] = item["SampleNo"];
                //        dr["TestDate"] = item["TestDate"];
                //        dr["PH"] = item["PH"];
                //        dr["CODCR"] = item["CODCR"];
                //        dr["FHW"] = item["FHW"];
                //        dr["ZS"] = item["ZS"];
                //        dr["ZYD"] = item["ZYD"];
                //        dr["GE"] = item["GE"];
                //        dr["GONG"] = item["GONG"];
                //        dr["Result"] = "���μ����Ŀ��δ���ꡣ";

                //    }
                //}
                #endregion

             
            }
            catch (Exception e)
            {
                return Success(e.Message);
            }
          

           
        }

        /// <summary>
        /// ˮ�ʷ������鱨��
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "ˮ�ʷ������鱨��")]
        public ActionResult exportExcelData(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = @"a.sampleno,b.itemname as sampletype,a.testusername,to_char(testdate,'yyyy-MM-dd')as testdate";
            pagination.p_tablename = @"bis_waterquality a left join  ( select itemname,itemvalue from base_dataitemdetail where  itemid = 
                        (select itemid from base_dataitem where itemname = 'ˮ�ʷ���' ))  b on a.sampletype = b.itemvalue";
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
            var data = waterqualitybll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "ˮ�ʷ������鱨��";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "ˮ�ʷ������鱨��.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "sampleno".ToLower(), ExcelColumn = "��Ʒ���" });
            listColumnEntity.Add(new ColumnEntity() { Column = "sampletype".ToLower(), ExcelColumn = "��Ʒ�����Ʒ����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "testusername".ToLower(), ExcelColumn = "������Ա" });
            listColumnEntity.Add(new ColumnEntity() { Column = "testdate".ToLower(), ExcelColumn = "��������" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
