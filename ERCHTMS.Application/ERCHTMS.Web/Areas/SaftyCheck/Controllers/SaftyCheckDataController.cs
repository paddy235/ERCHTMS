using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.Busines.SaftyCheck;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Code;
using System.Collections.Generic;
using System.Web;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.PublicInfoManage;
using System.IO;
using ERCHTMS.Entity.PublicInfoManage;
using System.Threading;
using System.Data;
using BSFramework.Util.Offices;
using System.Linq;

namespace ERCHTMS.Web.Areas.SaftyCheck.Controllers
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class SaftyCheckDataController : MvcControllerBase
    {
        private SaftyCheckDataBLL saftycheckdatabll = new SaftyCheckDataBLL();
        private SaftyCheckDataDetailBLL sdbll = new SaftyCheckDataDetailBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private OrganizeBLL organizeBLL = new OrganizeBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
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
        public ActionResult Details()
        {
            return View();
        }

        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// ѡ��������
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckNameSet()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ��ȫ��������Ϣ
        /// </summary>
        [HttpGet]
        public ActionResult GetListJsonByFolder(string folderId)
        {
            var data = saftycheckdatabll.GetListByObject(folderId);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ������ȫ�������
        /// </summary>
        [HttpPost]
        public ActionResult GetCheckStat(string orgCode = "", string orgId = "")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.RoleName.Contains("�����û�"))
            {
                if (!string.IsNullOrEmpty(orgCode))
                {
                    user = new Operator
                    {
                        OrganizeId = orgId,
                        OrganizeCode = orgCode,
                        DeptCode = orgCode,
                        RoleName = "��˾���û�,��˾�쵼"
                    };
                }
                else
                {
                    user = new Operator
                    {
                        OrganizeId = orgId,
                        OrganizeCode = "00",
                        DeptCode = "00",
                        RoleName = "��˾���û�,��˾�쵼"
                    };
                }
            }
            var data = saftycheckdatabll.GetCheckStat(user);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = saftycheckdatabll.GetList(queryJson);
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
            SaftyCheckDataEntity data = saftycheckdatabll.GetEntity(keyValue);
            if (data == null)
            {
                data = new SaftyCheckDataEntity();
                data.CreateDate = DateTime.Now;
                data.CreateUserName = OperatorProvider.Provider.Current().UserName;
            }
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȫ�����б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   

        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "ID";
            pagination.p_fields = "CreateDate,CHECKDATANAME,CHECKDATATYPE,b.usetime,CHECKDATATYPENAME,BelongDeptCode";
            pagination.p_tablename = @"BIS_SAFTYCHECKDATA t left join (select checkdataid,count(1) as usetime from bis_saftycheckdatarecord a left join (select recid,checkdataid from bis_saftycheckdatadetailed  group by recid,checkdataid)
 b on a.id=b.recid group by checkdataid) b
on t.id=b.checkdataid";
            var user = OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson = string.Format("belongdeptcode like '{0}%'", user.OrganizeCode); ;
            }
            else
            {
                pagination.conditionJson = " 1=1";
            }
            var watch = CommonHelper.TimerStart();
            var data = saftycheckdatabll.GetPageList(pagination, queryJson);
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
        /// ��ȫ�����б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
        public ActionResult GetCheckNameList(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.p_kid = "ID cid";
                pagination.p_fields = "CreateDate,CHECKNAME,DeptCode,Status,sortcode";
                pagination.p_tablename = "BIS_CHECKNAMESET";
                var user = OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    pagination.conditionJson = string.Format(" orgcode='{0}'", user.OrganizeCode);
                    if(!(user.RoleName.Contains("��˾����Ա") || user.RoleName.Contains("���������û�")))
                    {
                        pagination.conditionJson += " and status=1";
                    }
                }
                else
                {
                    pagination.conditionJson = " 1=1";
                }
                var watch = CommonHelper.TimerStart();
                var data = saftycheckdatabll.GetCheckNamePageList(pagination, queryJson);
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
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
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
        [HandlerMonitor(6, "ɾ������")]
        public ActionResult RemoveForm(string keyValue)
        {
            saftycheckdatabll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "ɾ���������")]
        public ActionResult RemoveCheckName(string keyValue)
        {
            try
            {
                saftycheckdatabll.RemoveCheckName(keyValue);
                return Success("ɾ���ɹ���");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "����������")]
        public ActionResult SaveCheckName(string keyValue,string itemJson)
        {
            try
            {
                var user = OperatorProvider.Provider.Current();
                List<CheckNameSetEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CheckNameSetEntity>>(itemJson);
                saftycheckdatabll.SaveCheckName(user, list);
                return Success("�����ɹ���");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [AjaxOnly]
        [HandlerMonitor(5, "���ɰ�ȫ��鹫ʾ��")]
        public ActionResult Make(string keyValue, CheckNoticeEntity sn)
        {
            try
            {
                SaftyCheckDataRecordBLL srbll = new SaftyCheckDataRecordBLL();
                SaftyCheckDataRecordEntity entity=srbll.GetEntity(sn.CheckId);
                if (entity!=null)
                {
                    sn.StartDate = entity.CheckBeginTime;
                    sn.EndDate = entity.CheckEndTime;
                }
                SaftyCheckContentBLL scbll = new SaftyCheckContentBLL();
                scbll.SaveNotice(keyValue, sn);
                return Success("�����ɹ�");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// �������ݵ�cache��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportObj(string objname, string objid, string objtype, string qyid, string qyname)
        {

            int error = 0;
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
                objtype = string.IsNullOrEmpty(objtype) ? "3" : objtype;
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int success=0;
                DistrictBLL districtbll = new DistrictBLL();
                //�Ȼ�ȡ����
                IEnumerable<DistrictEntity> AreaList = districtbll.GetOrgList(OperatorProvider.Provider.Current().OrganizeId);
                List<SaftyCheckModel> sclist = new List<SaftyCheckModel>();
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    string checkObj = dt.Rows[i][0].ToString();//������
                    string content = dt.Rows[i][1].ToString();//�������
                   // string desc = dt.Rows[i][2].ToString();//��������
                    if (!string.IsNullOrEmpty(objname)) //ѡ���˼����������������ö����е�����
                    {
                        if (!string.IsNullOrEmpty(content))
                        {
                            string[] arrContent = content.TrimEnd('$').Split('$');
                          
                            foreach (string str in arrContent)
                            {
                                if (!string.IsNullOrWhiteSpace(str))
                                {
                                    SaftyCheckModel sc = new SaftyCheckModel();
                                    sc.CheckObject = objname;
                                    sc.CheckObjectId = objid;
                                    sc.CheckObjectType = objtype;
                                    sc.CheckContent = str;
                                    sc.RiskName = "";
                                    if (AreaList.Where(it => it.DistrictID == qyid).FirstOrDefault() != null)
                                    {
                                        sc.BelongDistrictCode = AreaList.Where(it => it.DistrictID == qyid).FirstOrDefault().DistrictCode;
                                    }
                                    //else
                                    //{
                                    //    falseMessage += "</br>" + "����������ֵ���ڿ�ѡ��Χ��,δ�ܵ���.";
                                    //    error++;
                                    //    continue;
                                    //}
                                    sc.BelongDistrictID = qyid;
                                    sc.BelongDistrict = qyname;
                                    sclist.Add(sc);
                                    success++;
                                }

                            }
                        }
                        
                    }
                    else//ѡ�����������ö��������  û�ж��������� ���õ����ĵ�������
                    {
                        if (!string.IsNullOrEmpty(content))
                        {
                            string[] arrContent = content.TrimEnd('$').Split('$');
                            objid = Guid.NewGuid().ToString();
                            int j = 0;
                            foreach (string str in arrContent)
                            {
                                if (!string.IsNullOrWhiteSpace(str))
                                {
                                    SaftyCheckModel sc = new SaftyCheckModel();
                                    sc.CheckObject = checkObj;
                                    sc.CheckObjectId = objid;
                                    sc.CheckObjectType = "";
                                    sc.CheckContent = str;
                                    sc.RiskName = "";
                                    sclist.Add(sc);
                                    success++;
                                }
                            }  
                        }
                    }
                }

                //������ɹ������ݴ��뻺����
                CacheHelper.SetChache(sclist, "SaftyCheck");
                count = dt.Rows.Count - 1;
                message = "����" + success + "����¼,�ɹ�����" + (success - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }

            return message;
        }


        /// <summary>
        /// �������ݵ�cache��
        /// </summary>
        /// <returns></returns>
        public ActionResult GetObj()
        {
            List<SaftyCheckModel> sc = CacheHelper.GetChache("SaftyCheck");
            ////string json = sc.ToJson();
            CacheHelper.RemoveChache("SaftyCheck");
            return ToJsonResult(sc);
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="projectItem">�����Ŀ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "���������޸ļ���")]
        public ActionResult SaveForm(string keyValue, string projectItem, SaftyCheckDataEntity entity)
        {
            var user = OperatorProvider.Provider.Current();

            if (!string.IsNullOrEmpty(entity.BelongDeptID))
            {
                if (!user.IsSystem)
                {
                    if (!string.IsNullOrEmpty(user.DeptId))
                    {
                        entity.BelongDeptID = user.DeptId;
                    }
                    else
                    {
                        entity.BelongDeptID = user.OrganizeId;
                    }
                }
                DepartmentEntity deptC = departmentBLL.GetEntity(entity.BelongDeptID);
                if (deptC != null)
                    entity.BelongDeptCode = deptC.EnCode;
                else
                {
                    var orgentity = organizeBLL.GetEntity(entity.BelongDeptID);
                    entity.BelongDeptCode = orgentity.EnCode;
                }

            }
            //���氲ȫ����
            projectItem = HttpUtility.UrlDecode(projectItem);
            int count = saftycheckdatabll.SaveForm(keyValue, entity);
            //���氲ȫ������Ŀ
            if (count > 0 && projectItem.Length > 0)
            {
                if (sdbll.Remove(entity.ID) >= 0)
                {
                    List<SaftyCheckDataDetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaftyCheckDataDetailEntity>>(projectItem);
                    sdbll.Save(entity.ID, list);
                }
            }
            return Success("�����ɹ���");
        }
        /// <summary>
        /// �ϴ��ļ�
        /// </summary>
        /// <param name="folderId">�ļ���Id</param>
        /// <param name="userId">�û�Id</param>
        /// <param name="Filedata">�ļ�����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadifyFile(string folderId, HttpPostedFileBase Filedata)
        {
            try
            {
                Thread.Sleep(500);////�ӳ�500����
                //û���ļ��ϴ���ֱ�ӷ���
                if (Filedata == null || string.IsNullOrEmpty(Filedata.FileName) || Filedata.ContentLength == 0)
                {
                    return HttpNotFound();
                }
                //��ȡ�ļ������ļ���(��������·��)
                //�ļ����·����ʽ��/Resource/ResourceFile/{userId}{data}/{guid}.{��׺��}
                string userId = OperatorProvider.Provider.Current().UserId;
                string fileGuid = DateTime.Now.ToString("yyyyMMddhhmmss");
                long filesize = Filedata.ContentLength;
                string FileEextension = Path.GetExtension(Filedata.FileName);
                string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                string virtualPath = string.Format("~/Resource/DocumentFile/{0}/{1}/{2}{3}", userId, uploadDate, fileGuid, FileEextension);
                string fullFileName = this.Server.MapPath(virtualPath);
                //�����ļ���
                string path = Path.GetDirectoryName(fullFileName);
                Directory.CreateDirectory(path);
                FileInfoEntity fileInfoEntity = new FileInfoEntity();

                //
                if (!System.IO.File.Exists(fullFileName))
                {
                    //�����ļ�
                    Filedata.SaveAs(fullFileName);
                    //�ļ���Ϣд�����ݿ�

                    fileInfoEntity.Create();
                    fileInfoEntity.FileId = fileGuid;
                    if (!string.IsNullOrEmpty(folderId))
                    {
                        fileInfoEntity.FolderId = folderId;
                    }
                    else
                    {
                        fileInfoEntity.FolderId = "0";
                    }
                    fileInfoEntity.FileName = Filedata.FileName;
                    fileInfoEntity.FilePath = virtualPath;
                    fileInfoEntity.FileSize = filesize.ToString();
                    fileInfoEntity.FileExtensions = FileEextension;
                    fileInfoEntity.FileType = FileEextension.Replace(".", "");
                    fileInfoBLL.SaveForm("", fileInfoEntity);
                }
                return Success("�ϴ��ɹ���", fileInfoEntity.FileId);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// �ж�ֵ�Ƿ�����ڼ�����
        /// </summary>
        /// <param name="ReskList"></param>
        /// <param name="value"></param>
        /// <param name="RiskValue"></param>
        /// <returns></returns>
        public bool GetAreaIsTrue(IEnumerable<DistrictEntity> ReskList, string value, out string RiskValue)
        {
            RiskValue = "";
            bool listFlag = false;
            foreach (DistrictEntity item in ReskList)
            {
                if (value.Trim() == item.DistrictName)
                {
                    RiskValue = item.DistrictID;
                    listFlag = true;
                }
            }
            return listFlag;
        }
    }
}
