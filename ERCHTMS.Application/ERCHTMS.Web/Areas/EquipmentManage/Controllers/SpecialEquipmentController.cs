using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.Busines.EquipmentManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Data;
using System.Collections.Generic;
using BSFramework.Data;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using System;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Linq;
using System.Web;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.EquipmentManage.Controllers
{
    /// <summary>
    /// �� ���������豸������Ϣ��
    /// </summary>
    public class SpecialEquipmentController : MvcControllerBase
    {
        private SpecialEquipmentBLL specialequipmentbll = new SpecialEquipmentBLL();
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private ERCHTMS.Busines.PublicInfoManage.FileInfoBLL fileInfoBLL = new ERCHTMS.Busines.PublicInfoManage.FileInfoBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
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
        [HttpGet]
        public ActionResult IndexList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Import()
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
        /// ͳ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Stat()
        {
            return View();
        }
        /// <summary>
        /// ѡ���豸
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Select()
        {
            return View();
        }

        /// <summary>
        /// ѡ���豸
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult RemoteSelect()
        {
            return View();
        }

        


        /// <summary>
        /// ʡ���б�ҳ��
        /// </summary>
        /// <returns></returns>
        public ActionResult SJIndex()
        {
            return View();
        }

        /// <summary>
        /// ʡ��ͳ��ҳ��
        /// </summary>
        /// <returns></returns>
        public ActionResult SJStat()
        {
            return View();
        }


        [HttpGet]
        public ActionResult SJIndexList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CaseList()
        {
            return View();
        }

        /// <summary>
        /// ʡ���볡�嵥
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SJLeaveList()
        {
            return View();
        }

        /// <summary>
        /// �볡��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Leave()
        {
            return View();
        }

        /// <summary>
        /// �볡�嵥
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LeaveList()
        {
            return View();
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Checkout()
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
            pagination.p_fields = @"EquipmentName,EquipmentName as normalequipmentname,EquipmentNo,Specifications,CertificateNo,CheckDate,NextCheckDate,district,districtid,districtcode,case when t.state=1 then 'δ����'
when t.state=2 then '����' when t.state=3 then 'ͣ��' when t.state=4 then '����' when t.state=5 then '�볧' end as state,certificateid,decode(checkfileid,null,''),acceptance,ControlDeptCode,CreateUserId,affiliation,CheckFileID,ExamineUnit";
            pagination.p_tablename = "BIS_SPECIALEQUIPMENT t";

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE in(select  encode from BASE_DEPARTMENT start with encode='{0}' connect by  prior  departmentid = parentid)", user.OrganizeCode);
            }

            //if (user.IsSystem)
            //{
            //    pagination.conditionJson = "1=1";
            //}
            //else
            //{
            //    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
            //    pagination.conditionJson = where;
            //}

            var watch = CommonHelper.TimerStart();
            var data = specialequipmentbll.GetPageList(pagination, queryJson);
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
        /// ��ȡʡ���б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJsonForSJ(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "ID";
            pagination.p_fields = @"EquipmentName,EquipmentNo,Specifications,CertificateNo,NextCheckDate,t.district,t.districtid,districtcode,case when t.state=1 then 'δ����'
when t.state=2 then '����' when t.state=3 then 'ͣ��' when t.state=4 then '����' when t.state=5 then '�볧' end as state,certificateid,checkfileid,acceptance,ControlDeptCode,t.CreateUserId,affiliation,b.organizeid createuserorgid";
            pagination.p_tablename = "BIS_SPECIALEQUIPMENT t left join base_department b on t.createuserorgcode=b.encode";
            pagination.sidx = "t.createdate";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
                orgcodelist = departmentBLL.GetList().Where(t => t.DeptCode.Contains(user.NewDeptCode) && t.Nature == "����");
                pagination.conditionJson += "  (";
                foreach (DepartmentEntity item in orgcodelist)
                {
                    pagination.conditionJson += " createuserorgcode ='" + item.EnCode + "' or ";
                }
                pagination.conditionJson += " createuserorgcode = '" + user.OrganizeCode + "') ";
            }

            //if (user.IsSystem)
            //{
            //    pagination.conditionJson = "1=1";
            //}
            //else
            //{
            //    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
            //    pagination.conditionJson = where;
            //}

            var watch = CommonHelper.TimerStart();
            var data = specialequipmentbll.GetPageList(pagination, queryJson);
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
            var data = specialequipmentbll.GetList(queryJson);
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
            var data = specialequipmentbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�豸���
        /// </summary>
        /// <param name="EquipmentNo">�豸���</param>
        /// <param name="orgcode">��������</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEquipmentNo(string EquipmentNo, string orgcode)
        {
            return ToJsonResult(specialequipmentbll.GetEquipmentNo(EquipmentNo, orgcode));
        }
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFiles(string keyValue)
        {
            var data = fileInfoBLL.GetFiles(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�豸���ͳ��ͼ���б�
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEquipmentTypeStat(string queryJson)
        {
            DataTable dt = specialequipmentbll.GetEquipmentTypeStat(queryJson, null);
            return ToJsonResult(dt);
        }


        /// <summary>
        /// ��ȡ�豸���й���ͳ��ͼ���б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetOperationFailureStat(string queryJson)
        {
            object obj = specialequipmentbll.GetOperationFailureStat(queryJson, null);
            return ToJsonResult(obj);
        }
        /// <summary>
        /// ��ȡ�豸����ͳ��ͼ���б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="se"></param>
        /// <returns></returns>
        public ActionResult GetEquipmentHidStat(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sqlwhere = string.Empty;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["StartTime"].IsEmpty())
            {
                string startTime = queryParam["StartTime"].ToString();
                string endTime = queryParam["EndTime"].ToString();
                if (queryParam["EndTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                sqlwhere = string.Format(" and t2.checkdate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            string sql = string.Format(@"  select s.itemname,s.itemvalue,(select count(1) from v_basehiddeninfo t1 left join BIS_specialequipment t2 on t1.deviceid=t2.id where t2.id is not null and t2.createuserorgcode='{1}' and t2.affiliation='1'and t2.equipmenttype=s.itemvalue {0}) as OwnEquipment ,
 (select count(1) from v_basehiddeninfo t1 left join BIS_specialequipment t2 on t1.deviceid=t2.id where t2.id is not null and t2.createuserorgcode='{1}' and t2.affiliation='2' and t2.equipmenttype=s.itemvalue {0}) as ExternalEquipment,s.sortcode
 from (select a.itemname,a.itemvalue,a.sortcode from BASE_DATAITEMDETAIL a
 left join base_dataitem b on a.itemid = b.itemid  where b.itemcode ='EQUIPMENTTYPE' ) s 
 group by s.itemname,s.itemvalue,s.sortcode order by s.sortcode ", sqlwhere, user.OrganizeCode);
            DataTable dt = specialequipmentbll.SelectData(sql);
            return ToJsonResult(dt);
        }

        public string SaveEquipmentStat(string TableHtml)
        {
            string PID = Guid.NewGuid().ToString();
            try
            {
                if (System.IO.File.Exists(Server.MapPath("~/Resource/Temp/") + PID + ".txt"))
                {
                    System.IO.File.Delete(Server.MapPath("~/Resource/Temp/") + PID + ".txt");
                }
                System.IO.File.AppendAllText(Server.MapPath("~/Resource/Temp/") + PID + ".txt", TableHtml, System.Text.Encoding.UTF8);
            }
            catch (Exception)
            {

                return "0";
            }

            return PID;
        }
        public void ExportEquipmentStat(string PID, string filename)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(Server.MapPath("~/Resource/Temp/" + PID + ".txt"), System.Text.Encoding.UTF8);
            string res = sr.ReadToEnd();
            sr.Close();
            if (System.IO.File.Exists(Server.MapPath("~/Resource/Temp/") + PID + ".txt"))
            {
                System.IO.File.Delete(Server.MapPath("~/Resource/Temp/") + PID + ".txt");
            }

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(Server.UrlDecode(filename), System.Text.Encoding.UTF8) + ".xls");
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";
            System.Web.HttpContext.Current.Response.Write(Server.UrlDecode(res.ToString()));
            System.Web.HttpContext.Current.Response.End();
        }

        /// <summary> 
        /// ͨ���豸id��ȡ�����豸�б�
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSpecialEquipmentTable(string ids)
        {
            DataTable dt = specialequipmentbll.GetSpecialEquipmentTable(ids.Split(','));
            return Content(dt.ToJson());
        }
        #endregion


        #region ʡ��ͳ������

        /// <summary>
        /// ��ȡʡ���豸����б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetEquipmentTypeStatForSJ(string queryJson)
        {
            DataTable dt = specialequipmentbll.GetEquipmentTypeStatGridForSJ(queryJson);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }

        /// <summary>
        /// ��ȡʡ���豸���ͼ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetEquipmentTypeStatDataForSJ(string queryJson)
        {
            return specialequipmentbll.GetEquipmentTypeStatDataForSJ(queryJson);
        }


        /// <summary>
        /// ����ʡ���豸���
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportEquipmentTypeStatDataForSJ(string queryJson)
        {
            try
            {
                DataTable dt = specialequipmentbll.GetEquipmentTypeStatGridForSJ(queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "��ͬ���������豸����ͳ��";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "��ͬ���������豸����ͳ��.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "name", ExcelColumn = "�糧����", Width = 20 });
                if (dt.Rows.Count > 0)
                {
                    for (int i = 2; i < dt.Columns.Count; i++)
                    {
                        excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = dt.Columns[i].ColumnName, ExcelColumn = dt.Rows[0][i].ToString().Split(',')[1].ToString(), Width = 20 });
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 2; j < dt.Columns.Count; j++)
                    {
                        dt.Rows[i][j] = dt.Rows[i][j].ToString().Split(',')[0].ToString();
                    }
                }
                //���õ�������
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��ȡʡ�������������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetEquipmentHidGridForSJ(string queryJson)
        {
            DataTable dt = specialequipmentbll.GetEquipmentHidGridForSJ(queryJson);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });

        }

        /// <summary>
        /// ��ȡʡ����������ͼ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetEquipmentHidDataForSJ(string queryJson)
        {
            return specialequipmentbll.GetEquipmentHidDataForSJ(queryJson);
        }

        /// <summary>
        /// ����ʡ����������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportEquipmentHidDataForSJ(string queryJson)
        {
            try
            {
                DataTable dt = specialequipmentbll.GetEquipmentHidGridForSJ(queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "��������ͳ��";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "��������ͳ��.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "typename", ExcelColumn = "�糧����", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num", ExcelColumn = "����", Width = 30 });
                //���õ�������
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��ȡʡ�����������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetEquipmentCheckGridForSJ(string queryJson)
        {
            DataTable dt = specialequipmentbll.GetEquipmentCheckGridForSJ(queryJson);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }

        /// <summary>
        /// ��ȡʡ��������ͼ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetEquipmentCheckDataForSJ(string queryJson)
        {
            return specialequipmentbll.GetEquipmentCheckDataForSJ(queryJson);
        }

        /// <summary>
        /// ����ʡ��������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportEquipmentCheckDataForSJ(string queryJson)
        {
            try
            {
                DataTable dt = specialequipmentbll.GetEquipmentCheckGridForSJ(queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "������ͳ��";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "������ͳ��.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "typename", ExcelColumn = "�糧����", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num", ExcelColumn = "����", Width = 30 });
                //���õ�������
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��ȡʡ�����й���ͼ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetEquipmentFailureDataForSJ(string queryJson)
        {
            return specialequipmentbll.GetEquipmentFailureDataForSJ(queryJson);
        }

        /// <summary>
        /// ��ȡʡ�����й����б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetEquipmentFailureGridForSJ(string queryJson)
        {
            DataTable dt = specialequipmentbll.GetEquipmentFailureGridForSJ(queryJson);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }

        /// <summary>
        /// ����ʡ�����й���
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportEquipmentFailureDataForSJ(string queryJson)
        {
            try
            {
                DataTable dt = specialequipmentbll.GetEquipmentFailureGridForSJ(queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "���й���ͳ��";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "���й���ͳ��.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "typename", ExcelColumn = "�糧����", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "num", ExcelColumn = "����", Width = 30 });
                //���õ�������
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��ȡʡ����������¼
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSafetyCheckRecordForSJ(string queryJson)
        {
            DataTable dt = specialequipmentbll.GetSafetyCheckRecordForSJ(queryJson);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
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
            specialequipmentbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SpecialEquipmentEntity entity)
        {
            specialequipmentbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// �����豸�볡
        /// </summary>
        /// <param name="leaveTime">�볡ʱ��</param>
        /// <param name="equipmentId">�豸Id,����ö��ŷָ�</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "�����豸�볡")]
        public ActionResult Leave(string leaveTime, [System.Web.Http.FromBody]string specialequipmentId, [System.Web.Http.FromBody]string DepartureReason)
        {
            try
            {
                if (specialequipmentbll.SetLeave(specialequipmentId, leaveTime, DepartureReason) > 0)
                {
                    return Success("�����ɹ���");
                }
                else
                {
                    return Error("�������ɹ���");
                }

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// �����豸�޸ļ�������
        /// </summary>
        /// <param name="CheckDate">�볡ʱ��</param>
        /// <param name="equipmentId">�豸Id,����ö��ŷָ�</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "�����豸�����޸ļ�������")]
        public ActionResult Checkout(string CheckDate, [System.Web.Http.FromBody]string specialequipmentId)
        {
            try
            {
                if (specialequipmentbll.SetCheck(specialequipmentId, CheckDate) > 0)
                {
                    return Success("�����ɹ���");
                }
                else
                {
                    return Error("�������ɹ���");
                }

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "���������豸����")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "rownum idx";
            pagination.p_fields = @"EmployDept,equipmentregisterno,specifications,factoryno,employsite,location,
case when t.state=1 then 'δ����'
when t.state=2 then '����' when t.state=3 then 'ͣ��' when t.state=4 then '����' when t.state=5 then '�볧' end as state ,
equipmentno,b.itemname equipmenttype,c.itemname equipmentkind,d.itemname equipmentbreed,
t.ReportNumber,
checkdate,
nextcheckdate,
t.ExamineVerdict,
examineappeardate,
reportexaminedate,
acceptstate";
            pagination.p_tablename = @"BIS_SPECIALEQUIPMENT t 
                                      left join base_dataitemdetail b
    on t.equipmenttype = b.itemvalue
   and b.itemid =
       (select itemid from base_dataitem where itemcode = 'EQUIPMENTTYPE')
  left join base_dataitemdetail c
    on t.equipmentkind = c.itemvalue
   and c.itemid =
       (select itemid from base_dataitem where itemcode = 'EquipmentKind')
  left join base_dataitemdetail d
    on t.equipmentbreed = d.itemvalue
   and d.itemid =
       (select itemid from base_dataitem where itemcode = 'EquipmentBreed')";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
                pagination.conditionJson = where;
            }
            pagination.sidx = "t.CreateDate";
            pagination.sord = "desc";
            var watch = CommonHelper.TimerStart();
            var data = specialequipmentbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "�����豸����";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "�����豸����.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentname", ExcelColumn = "�豸����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "employdept", ExcelColumn = "ʹ�õ�λ", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentregisterno", ExcelColumn = "�豸ע�����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "specifications", ExcelColumn = "�豸�ͺ�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "factoryno", ExcelColumn = "�������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "employsite", ExcelColumn = "ʹ�õص�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "location", ExcelColumn = "�豸���ڵ�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "state", ExcelColumn = "ʹ��״̬", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentno", ExcelColumn = "�豸�ڲ����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmenttype", ExcelColumn = "�豸����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentkind", ExcelColumn = "�豸���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentbreed", ExcelColumn = "�豸Ʒ��", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "reportnumber", ExcelColumn = "���鱨����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdate", ExcelColumn = "��������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "nextcheckdate", ExcelColumn = "�´μ�������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examineverdict", ExcelColumn = "�������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examineappeardate", ExcelColumn = "��쵥�ϱ�����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "reportexaminedate", ExcelColumn = "��������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "acceptstate", ExcelColumn = "����״̬", Alignment = "center" });
            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("�����ɹ���");
        }


        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "���������豸����")]
        public ActionResult ExportForSJ(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = @"EquipmentName,EquipmentNo,Specifications,CertificateNo,NextCheckDate,district,case when t.state=1 then 'δ����'
when t.state=2 then '����' when t.state=3 then 'ͣ��' when t.state=4 then '����' when t.state=5 then '�볧'  end as state ,certificateid,checkfileid";
            pagination.p_tablename = "BIS_SPECIALEQUIPMENT t";
            pagination.sidx = " t.createdate";
            pagination.sord = "desc";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
                orgcodelist = departmentBLL.GetList().Where(t => t.DeptCode.Contains(user.NewDeptCode) && t.Nature == "����");
                pagination.conditionJson += "  (";
                foreach (DepartmentEntity item in orgcodelist)
                {
                    pagination.conditionJson += " createuserorgcode ='" + item.EnCode + "' or ";
                }
                pagination.conditionJson += " createuserorgcode = '" + user.OrganizeCode + "') ";
            }

            var watch = CommonHelper.TimerStart();
            var data = specialequipmentbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "�����豸����";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "�����豸����.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentname", ExcelColumn = "�豸����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentno", ExcelColumn = "�豸���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "specifications", ExcelColumn = "����ͺ�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "certificateno", ExcelColumn = "ʹ�õǼ�֤���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "nextcheckdate", ExcelColumn = "�´μ�������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "district", ExcelColumn = "��������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "state", ExcelColumn = "״̬", Alignment = "center" });
            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("�����ɹ���");
        }

        /// <summary>
        /// ����ʡ����������¼
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "����ʡ����������¼")]
        public ActionResult ExportSafetyCheckRecordForSJ(string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            var data = specialequipmentbll.GetSafetyCheckRecordForSJ(queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "ʡ�������豸������ͳ��";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "ʡ�������豸��������¼.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkendtime", ExcelColumn = "���ʱ��", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdatarecordname", ExcelColumn = "�������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "CHECKMAN", ExcelColumn = "�����Ա", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "Count", ExcelColumn = "�������������", Alignment = "center" });
            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("�����ɹ���");
        }

        #endregion

        #region ������ͨ�豸
        /// <summary>
        /// ������ͨ�豸
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportEquipment()
        {
            try
            {
                int error = 0;
                int sussceed = 0;
                string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return message;
                    }
                    if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                    {
                        return message;
                    }
                    var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    string filePath = Server.MapPath("~/Resource/temp/" + fileName);
                    file.SaveAs(filePath);
                    DataTable dt = ExcelHelper.ExcelImport(filePath);
                    var districtList = new DistrictBLL().GetList().Where(x => x.OrganizeId == user.OrganizeId).ToList();
                    var deptList = new DepartmentBLL().GetList().Where(x => x.OrganizeId == user.OrganizeId).ToList();
                    var users = new UserBLL().GetUserInfoByDeptCode(user.OrganizeCode);
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        object[] vals = dt.Rows[i].ItemArray;
                        if (IsEndRow(vals) == true)
                            break;

                        var msg = "";
                        string a = "";
                        string Affiliation = "";
                        string EquipmentType = "";

                        var en = this.GetTypeValue(vals[4].ToString(), "'EQUIPMENTTYPE'");
                        if (en != null) {
                            EquipmentType = en.ItemValue;
                        }
                        var obj = vals[13];//������λ
                        if (obj.ToString().Length > 0)
                        {
                            var dept = deptList.Where(t => t.FullName == obj.ToString().Trim()).FirstOrDefault();
                            if (dept != null)
                            {
                                if (dept.Nature == "�а���" || dept.Nature == "�ְ���" || dept.Description == "������̳а���")
                                {
                                    Affiliation = "�����λ����";
                                }
                                else
                                {
                                    Affiliation = "����λ����";
                                }
                            }
                        }
                        else
                        {
                            Affiliation = "����λ����";
                        }
                        switch (Affiliation)
                        {
                            case "����λ����":
                                a = "T1-";
                                break;
                            case "�����λ����":
                                a = "T2-";
                                break;
                            default:
                                a = "T1-";
                                break;
                        }
                        switch (EquipmentType)
                        {
                            case "1":
                                a += "GL";
                                break;
                            case "2":
                                a += "RQ";
                                break;
                            case "3":
                                a += "GD";
                                break;
                            case "4":
                                a += "QZ";
                                break;
                            case "5":
                                a += "CL";
                                break;
                            case "6":
                                a += "FJ";
                                break;
                            case "7":
                                a += "DT";
                                break;
                            case "8"://ѹ���ܵ�Ԫ��
                                a += "YJ";
                                break;
                            case "9"://��������
                                a += "SD";
                                break;
                            case "10"://����������ʩ
                                a += "SS";
                                break;
                            default:
                                break;
                        }

                        if (Validate(i, vals, deptList, districtList, out msg, users) == true)
                        {
                            var eno = int.Parse(specialequipmentbll.GetEquipmentNo(a, user.OrganizeCode)) + 1;
                            var entity = GenEntity(vals, deptList, districtList, a + eno.ToString().PadLeft(4, '0'), users);
                            specialequipmentbll.SaveForm("", entity);
                            eno++;
                            sussceed++;
                        }
                        else
                        {
                            falseMessage += "��" + (i + 2) + "��" + msg + "</br>";
                            error++;
                        }
                    }
                    count = dt.Rows.Count;
                    message = "����" + (count - 1) + "����¼,�ɹ�����" + sussceed + "����ʧ��" + error + "��";
                    if (error > 0)
                    {
                        message += "��������Ϣ���£�</br>" + falseMessage;
                    }

                    //ɾ����ʱ�ļ�
                    System.IO.File.Delete(filePath);
                }
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        private bool IsEndRow(object[] vals)
        {
            bool r = false;

            r = Array.TrueForAll(vals, x => (x == null || x == DBNull.Value || x.ToString() == ""));

            return r;
        }
        private bool Validate(int index, object[] vals, List<DepartmentEntity> deptList, List<DistrictEntity> districtList, out string msg, IList<UserInfoEntity> users)
        {
            var r = true;
            msg = "��";
            var obj = vals[1];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "����������Ϊ�գ�";
                r = false;
            }
            else
            {
                int ncount = districtList.Count(x => x.DistrictName == obj.ToString().Trim());
                if (ncount <= 0)
                {
                    msg += "��������������ϵͳ�е��������Ʋ�ƥ�䣬";
                    r = false;
                }
            }
            obj = vals[3];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "�豸���Ʋ���Ϊ�գ�";
                r = false;
            }
            var zl = "";
            var lb = "";
            obj = vals[4];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "�豸���಻��Ϊ�գ�";
                r = false;
            }
            else
            {
                var en = GetTypeValue(obj.ToString(), "'EQUIPMENTTYPE'");
                if (en != null)
                {
                    zl = en.ItemValue;
                }
                else {
                    msg += "�豸����ϵͳ�в����ڣ�";
                    r = false;
                }
            }
            obj = vals[5];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {

            }
            else
            {
                var en = GetTypeValue(obj.ToString(), "'EquipmentKind'");
                if (en != null)
                {
                    if (zl != en.ItemCode)
                    {
                        msg += "���豸����ڶ�Ӧ�豸�����У�";
                        r = false;
                    }
                    else
                    {

                        lb = en.ItemValue;
                    }
                }
                else
                {
                    msg += "�豸����ϵͳ�в����ڣ�";
                    r = false;
                }
            }
            obj = vals[6];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {

            }
            else
            {
                var en = GetTypeValue(obj.ToString(), "'EquipmentBreed'");
                if (en != null)
                {
                    if (lb != en.ItemCode)
                    {
                        msg += "���豸Ʒ�ֲ��ڶ�Ӧ�豸����У�";
                        r = false;
                    }
                    else
                    {

                        lb = en.ItemValue;
                    }
                }
                else
                {
                    msg += "�豸Ʒ��ϵͳ�в����ڣ�";
                    r = false;
                }
            }

            obj = vals[11];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "����������ڲ���Ϊ�գ�";
                r = false;
            }
            else
            {
                DateTime checkDate;
                if (!DateTime.TryParse(obj.ToString(), out checkDate))
                {
                    msg += "����������ڸ�ʽ����";
                    r = false;
                }
            }

            obj = vals[12];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "�������ڲ���Ϊ�գ�";
                r = false;
            }
            else
            {
                int period;
                if (!int.TryParse(obj.ToString(), out period))
                {
                    msg += "�������ڸ�ʽ����";
                    r = false;
                }
            }
            obj = vals[13];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                //msg += "������λ����Ϊ�գ�";
                //r = false;
            }
            else
            {
                var dept = deptList.Where(t => t.FullName == obj.ToString().Trim()).FirstOrDefault();
                if (dept == null)
                {
                    msg += "������λ������ϵͳ�еĵ�λ���Ʋ�ƥ�䣬";
                    r = false;
                }
                else
                {
                    obj = vals[18];
                    if (obj.ToString().Trim().Length > 0)
                    {
                        DataRow[] rows = outsouringengineerbll.GetEngineerDataByWBId(dept.DepartmentId).Select("ENGINEERNAME='" + obj.ToString().Trim() + "'");
                        if (rows.Length == 0)
                        {
                            msg += "�������������ϵͳ�еĹ������Ʋ�ƥ�䣬";
                            r = false;
                        }
                    }
                }
            }
            obj = vals[14];//ʹ�õ�λ
            if (obj.ToString().Trim().Length > 0)
            {
                var dept = deptList.Where(t => t.FullName == obj.ToString().Trim()).FirstOrDefault();
                if (dept == null)
                {
                    msg += "ʹ�õ�λ������ϵͳ�еĵ�λ���Ʋ�ƥ�䣬";
                    r = false;
                }
            }
            obj = vals[15];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "�ܿز��Ų���Ϊ�գ�";
                r = false;
            }
            else
            {
                int ncount = deptList.Count(x => x.FullName == obj.ToString().Trim());
                if (ncount <= 0)
                {
                    msg += "�ܿز��Ų���ȷ��";
                    r = false;
                }
            }
            obj = vals[16];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "��ȫ������Ա����Ϊ�գ�";
                r = false;
            }
            else
            {
                int count = users.Where(t => t.RealName == obj.ToString().Trim()).Count();
                if (count == 0)
                {
                    msg += "��ȫ������Ա��ϵͳ�в����ڣ�";
                    r = false;
                }
            }
            obj = vals[17];
            if (obj.ToString().Trim().Length > 0)
            {
                int count = users.Where(t => t.RealName == obj.ToString().Trim()).Count();
                if (count == 0)
                {
                    msg += "������Ա��ϵͳ�в����ڣ�";
                    r = false;
                }
            }
            obj = vals[19];
            if (obj == null || obj == DBNull.Value || obj.ToString() == "")
            {
                msg += "ʹ��״������Ϊ�գ�";
                r = false;
            }
            else
            {
                var v = obj.ToString();
                if (v != "δ����" && v != "����" && v != "ͣ��" && v != "����" && v != "�볧")
                {
                    msg += "ʹ��״��ֵ����ȷ��";
                    r = false;
                }
            }
            obj = vals[20];
            if (obj == null || obj == DBNull.Value || obj.ToString().Trim() == "")
            {
                msg += "�Ƿ������ղ���Ϊ�գ�";
                r = false;
            }
            else
            {
                var v = obj.ToString();
                if (v != "��" && v != "��" )
                {
                    msg += "�Ƿ�������ֵ����ȷ��";
                    r = false;
                }
            }
            msg = msg.TrimEnd('��');

            return r;
        }
        public DataItemModel GetTypeValue(string text,string str)
        {
            //string val = "";
            DataItemModel di = new DataItemModel();
            var list = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetDataItemListByItemCode(str);
            foreach (ERCHTMS.Entity.SystemManage.ViewModel.DataItemModel dataitem in list)
            {
                if (dataitem.ItemName == text)
                {
                    //val = dataitem.ItemValue;
                    di = dataitem;
                    break;
                }
            }
            return di;
        }
        private SpecialEquipmentEntity GenEntity(object[] vals, List<DepartmentEntity> deptList, List<DistrictEntity> districtList, string eno, IList<UserInfoEntity> users)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            SpecialEquipmentEntity entity = new SpecialEquipmentEntity();
            var obj = vals[1];
            if (obj != null && obj != DBNull.Value)
            {
                var district = districtList.FirstOrDefault(x => x.DistrictName == obj.ToString().Trim());
                if (district != null)
                {
                    entity.District = district.DistrictName;
                    entity.DistrictId = district.DistrictID;
                    entity.DistrictCode = district.DistrictCode;
                }
            }
            entity.EmploySite = vals[2].ToString().Trim();
            entity.EquipmentName = vals[3].ToString().Trim();
            obj = vals[4];//�豸����
            if (obj.ToString().Length > 0)
            {
                entity.EquipmentType = GetTypeValue(obj.ToString(), "'EQUIPMENTTYPE'").ItemValue;
            }
            obj = vals[5];//�豸���
            if (obj.ToString().Length > 0)
            {
                entity.EquipmentKind = GetTypeValue(obj.ToString(), "'EquipmentKind'").ItemValue;
            }
            obj = vals[6];//�豸Ʒ��
            if (obj.ToString().Length > 0)
            {
                entity.EquipmentBreed = GetTypeValue(obj.ToString(), "'EquipmentBreed'").ItemValue;
            }
            entity.EquipmentNo = vals[7].ToString().Trim();//�豸�ڲ����
            entity.Specifications = vals[8].ToString().Trim();
            entity.EquipmentRegisterNo = vals[9].ToString().Trim();//�豸ע�����
            entity.CertificateNo = vals[10].ToString().Trim();//ʹ�õǼ�֤���
            entity.CheckDate = DateTime.Parse(DateTime.Parse(vals[11].ToString()).ToString("yyyy-MM-dd"));//�����������
            entity.CheckDateCycle = vals[12].ToString();//��������
            obj = vals[13];//������λ
            if (obj.ToString().Length > 0)
            {
                var dept = deptList.Where(t => t.FullName == obj.ToString().Trim()).FirstOrDefault();
                if (dept != null)
                {
                    entity.EPIBOLYDEPTID = dept.DepartmentId;//������λ
                    entity.EPIBOLYDEPT = dept.FullName;//������λ

                    if (dept.Nature == "�а���" || dept.Nature == "�ְ���" || dept.Description == "������̳а���")
                    {
                        entity.Affiliation = "2";
                    }
                    else
                    {
                        entity.Affiliation = "1";
                    }

                    DataRow[] rows = outsouringengineerbll.GetEngineerDataByWBId(dept.DepartmentId).Select("ENGINEERNAME='" + vals[18].ToString().Trim() + "'");
                    if (rows.Length > 0)
                    {
                        entity.EPIBOLYPROJECTID = rows[0][1].ToString();//�������
                        entity.EPIBOLYPROJECT = vals[18].ToString().Trim();//�������
                    }
                }
            }
            else
            {
                entity.EPIBOLYDEPTID = user.OrganizeId;
                entity.EPIBOLYDEPT = user.OrganizeName;
                entity.Affiliation = "1";
            }
            obj = vals[14];//ʹ�õ�λ
            if (obj.ToString().Length > 0)
            {
                var dept = deptList.Where(t => t.FullName == obj.ToString().Trim()).FirstOrDefault();
                if (dept != null)
                {
                    entity.EmployDeptId = dept.DepartmentId;//ʹ�õ�λID
                    entity.EmployDept = dept.FullName;//ʹ�õ�λ
                }
            }
            obj = vals[15];//�ܿز���
            if (obj != null && obj != DBNull.Value)
            {
                var dept = deptList.FirstOrDefault(x => x.FullName == obj.ToString().Trim());
                if (dept != null)
                {
                    entity.ControlDept = dept.FullName;
                    entity.ControlDeptCode = dept.EnCode;
                    entity.ControlDeptID = dept.DepartmentId;
                }
            }
            obj = vals[16];//��ȫ������Ա
            var listUsers = users.Where(t => t.RealName == obj.ToString().Trim());
            if (listUsers.Count() > 0)
            {
                UserInfoEntity ManagerUser = listUsers.FirstOrDefault();
                entity.SecurityManagerUser = ManagerUser.RealName;
                entity.SecurityManagerUserID = ManagerUser.UserId;
                entity.Telephone = ManagerUser.Telephone;
            }
            obj = vals[17];//������Ա
            if (obj != null || obj != DBNull.Value || obj.ToString().Trim().Length > 0)
            {
                var listUser = users.Where(t => t.RealName == obj.ToString().Trim());
                if (listUser.Count() > 0)
                {
                    entity.OperUser = listUser.FirstOrDefault().RealName;
                    entity.OperUserID = listUser.FirstOrDefault().UserId;
                }
            }
            entity.RelWord = "";
            entity.State = "1";
            var state = vals[19].ToString();
            if (state == "����")
                entity.State = "2";
            else if (state == "ͣ��")
                entity.State = "3";
            else if (state == "����")
                entity.State = "4";
            else if (state == "�볧")
                entity.State = "5";

            entity.IsCheck = vals[20].ToString();//�Ƿ�������
           
            entity.NextCheckDate = entity.CheckDate.Value.AddDays(int.Parse(entity.CheckDateCycle));

            entity.EquipmentNo = eno;
            return entity;
        }
        #endregion

    }
}
