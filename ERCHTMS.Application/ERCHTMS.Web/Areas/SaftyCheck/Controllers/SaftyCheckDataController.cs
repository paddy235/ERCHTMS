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
    /// 描 述：安全检查表
    /// </summary>
    public class SaftyCheckDataController : MvcControllerBase
    {
        private SaftyCheckDataBLL saftycheckdatabll = new SaftyCheckDataBLL();
        private SaftyCheckDataDetailBLL sdbll = new SaftyCheckDataDetailBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private OrganizeBLL organizeBLL = new OrganizeBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details()
        {
            return View();
        }

        /// <summary>
        /// 导入页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// 选择检查名称
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckNameSet()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取安全检查表附件信息
        /// </summary>
        [HttpGet]
        public ActionResult GetListJsonByFolder(string folderId)
        {
            var data = saftycheckdatabll.GetListByObject(folderId);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取各个安全检查数量
        /// </summary>
        [HttpPost]
        public ActionResult GetCheckStat(string orgCode = "", string orgId = "")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.RoleName.Contains("集团用户"))
            {
                if (!string.IsNullOrEmpty(orgCode))
                {
                    user = new Operator
                    {
                        OrganizeId = orgId,
                        OrganizeCode = orgCode,
                        DeptCode = orgCode,
                        RoleName = "公司级用户,公司领导"
                    };
                }
                else
                {
                    user = new Operator
                    {
                        OrganizeId = orgId,
                        OrganizeCode = "00",
                        DeptCode = "00",
                        RoleName = "公司级用户,公司领导"
                    };
                }
            }
            var data = saftycheckdatabll.GetCheckStat(user);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = saftycheckdatabll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
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
        /// 安全检查表列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   

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
        /// 安全检查表列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
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
                    if(!(user.RoleName.Contains("公司管理员") || user.RoleName.Contains("厂级部门用户")))
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

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除检查表")]
        public ActionResult RemoveForm(string keyValue)
        {
            saftycheckdatabll.RemoveForm(keyValue);
            return Success("删除成功。");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除检查名称")]
        public ActionResult RemoveCheckName(string keyValue)
        {
            try
            {
                saftycheckdatabll.RemoveCheckName(keyValue);
                return Success("删除成功。");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "保存检查名称")]
        public ActionResult SaveCheckName(string keyValue,string itemJson)
        {
            try
            {
                var user = OperatorProvider.Provider.Current();
                List<CheckNameSetEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CheckNameSetEntity>>(itemJson);
                saftycheckdatabll.SaveCheckName(user, list);
                return Success("操作成功。");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpPost]
        [AjaxOnly]
        [HandlerMonitor(5, "生成安全检查公示表")]
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
                return Success("操作成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 导入数据到cache中
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportObj(string objname, string objid, string objtype, string qyid, string qyname)
        {

            int error = 0;
            string message = "请选择格式正确的文件再导入!";
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
                //先获取区域
                IEnumerable<DistrictEntity> AreaList = districtbll.GetOrgList(OperatorProvider.Provider.Current().OrganizeId);
                List<SaftyCheckModel> sclist = new List<SaftyCheckModel>();
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    string checkObj = dt.Rows[i][0].ToString();//检查对象
                    string content = dt.Rows[i][1].ToString();//检查内容
                   // string desc = dt.Rows[i][2].ToString();//隐患描述
                    if (!string.IsNullOrEmpty(objname)) //选择了检查对象的情况下区域用对象中的数据
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
                                    //    falseMessage += "</br>" + "检查对象区域值不在可选范围内,未能导入.";
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
                    else//选择对象情况下用对象的区域  没有对象的情况下 才用导入文档中区域
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

                //将导入成功的数据存入缓存中
                CacheHelper.SetChache(sclist, "SaftyCheck");
                count = dt.Rows.Count - 1;
                message = "共有" + success + "条记录,成功导入" + (success - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }


        /// <summary>
        /// 导入数据到cache中
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="projectItem">检查项目</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "新增或者修改检查表")]
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
            //保存安全检查表
            projectItem = HttpUtility.UrlDecode(projectItem);
            int count = saftycheckdatabll.SaveForm(keyValue, entity);
            //保存安全检查表项目
            if (count > 0 && projectItem.Length > 0)
            {
                if (sdbll.Remove(entity.ID) >= 0)
                {
                    List<SaftyCheckDataDetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaftyCheckDataDetailEntity>>(projectItem);
                    sdbll.Save(entity.ID, list);
                }
            }
            return Success("操作成功。");
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="folderId">文件夹Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="Filedata">文件对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadifyFile(string folderId, HttpPostedFileBase Filedata)
        {
            try
            {
                Thread.Sleep(500);////延迟500毫秒
                //没有文件上传，直接返回
                if (Filedata == null || string.IsNullOrEmpty(Filedata.FileName) || Filedata.ContentLength == 0)
                {
                    return HttpNotFound();
                }
                //获取文件完整文件名(包含绝对路径)
                //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                string userId = OperatorProvider.Provider.Current().UserId;
                string fileGuid = DateTime.Now.ToString("yyyyMMddhhmmss");
                long filesize = Filedata.ContentLength;
                string FileEextension = Path.GetExtension(Filedata.FileName);
                string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                string virtualPath = string.Format("~/Resource/DocumentFile/{0}/{1}/{2}{3}", userId, uploadDate, fileGuid, FileEextension);
                string fullFileName = this.Server.MapPath(virtualPath);
                //创建文件夹
                string path = Path.GetDirectoryName(fullFileName);
                Directory.CreateDirectory(path);
                FileInfoEntity fileInfoEntity = new FileInfoEntity();

                //
                if (!System.IO.File.Exists(fullFileName))
                {
                    //保存文件
                    Filedata.SaveAs(fullFileName);
                    //文件信息写入数据库

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
                return Success("上传成功。", fileInfoEntity.FileId);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 判断值是否包含在集合中
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
