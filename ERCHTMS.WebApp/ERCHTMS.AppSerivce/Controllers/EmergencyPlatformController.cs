using ERCHTMS.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.EmergencyPlatform;
using ERCHTMS.Busines.PublicInfoManage;
using System.IO;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.PublicInfoManage;
using Newtonsoft.Json.Linq;
using BSFramework.Util.WebControl;
using System.Data;
using BSFramework.Util;
using Aspose.Words;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Cache;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Extension;

namespace ERCHTMS.AppSerivce.Controllers
{
    /// <summary>
    /// 应急管理
    /// </summary> 
    public class EmergencyPlatformController : BaseApiController
    {
        private ReserverplanBLL reserverplanbll = new ReserverplanBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private SuppliesBLL suppliesbll = new SuppliesBLL();
        private InoroutrecordBLL inoroutrecordbll = new InoroutrecordBLL();
        private DrillplanrecordBLL drillplanrecordbll = new DrillplanrecordBLL();
        private DrillplanrecordstepBLL drillplanrecordstepbll = new DrillplanrecordstepBLL();
        private DrillassessBLL drillassessbll = new DrillassessBLL();
        private DrillplanBLL drillplanbll = new DrillplanBLL();
        private DataItemCache dataItemCache = new DataItemCache();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private DrillrecordevaluateBLL drillrecordevaluatebll = new DrillrecordevaluateBLL();
        public HttpContext ctx { get { return HttpContext.Current; } }
        #region 提交应急预案
        /// <summary>
        /// 提交应急预案
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveReserverplan()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                OperatorProvider.AppUserId = userid;
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                HttpFileCollection files = HttpContext.Current.Request.Files;
                ReserverplanEntity entity = new ReserverplanEntity()
                {
                    NAME = dy.data.name,
                    PLANTYPE = dy.data.plantype,
                    PLANTYPENAME = dy.data.plantypename,
                    USERID_BZ = dy.data.userid_bz,
                    USERNAME_BZ = dy.data.username_bz,
                    DEPARTID_BZ = dy.data.departid_bz,
                    DEPARTNAME_BZ = dy.data.departname_bz,
                    DATATIME_BZ = Convert.ToDateTime(dy.data.datetime_bz)
                };
                string keyValue = Guid.NewGuid().ToString();
                entity.ID = keyValue;
                entity.FILES = Guid.NewGuid().ToString();
                UploadifyFile(entity.FILES, files);
                reserverplanbll.SaveForm(keyValue, entity);

                return new { Code = 0, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        #endregion

        #region 获取应急预案列表
        /// <summary>
        /// 获取应急预案列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetReserverplanList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string reserverName = dy.data.name ?? "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(dy.pageindex.ToString());
                pagination.rows = int.Parse(dy.pagesize.ToString());
                pagination.p_kid = "a.id";
                pagination.p_fields = string.Format("a.name,a.plantypename,a.username_bz,a.departname_bz,to_char(a.datatime_bz,'yyyy-mm-dd hh24:mi:ss') as datetime_bz,decode(filepath,null,filepath,'{0}' || substr(f.filepath,2,length(f.filepath)-1)) as filepath,f.filename", new DataItemDetailBLL().GetItemValue("imgUrl"));
                pagination.p_tablename = "MAE_RESERVERPLAN a left join ( select recid,filepath,filename,row_number() over(partition by recid order by createdate asc) as num from  base_fileinfo) f on a.files=f.recid and f.num = 1";
                pagination.conditionJson = "1=1";
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();


                if (currUser.IsSystem)
                {
                    pagination.conditionJson = " and 1=1";
                }
                else
                {
                    pagination.conditionJson += " and a.createuserorgcode ='" + currUser.OrganizeCode + "'";

                    //pagination.conditionjson += " and (a.departid_bz='" + curruser.deptid + "' or a.departid_bz=(select parentid from base_department where departmentid ='" + curruser.deptid + "')";
                    //if (!string.isnullorempty(new dataitemdetailbll().getitemvalue(curruser.organizeid)))
                    //{
                    //    foreach (var item in new dataitemdetailbll().getitemvalue(curruser.organizeid).split('#'))
                    //    {
                    //        if (!string.isnullorempty(item))
                    //        {
                    //            pagination.conditionjson += " or a.departid_bz='" + item.split('|')[0] + "' ";
                    //        }
                    //    }
                    //}
                    //pagination.conditionjson += ")";
                }


                if (!string.IsNullOrEmpty(reserverName))
                {
                    pagination.conditionJson += string.Format(" and a.name like '%{0}%' ", reserverName);
                }
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                });
                var data = reserverplanbll.GetPageList(pagination, queryJson);
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取应急物资库列表
        /// <summary>
        /// 获取应急预案列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSuppliesFactoryList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string suppliesName = dy.data.name;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 10000;
                pagination.p_kid = "a.id";
                pagination.p_fields = string.Format("a.name,a.suppliestype,a.suppliestypename,'{0}' || substr(b.filepath,2,length(b.filepath)-1) as filepath,a.recid", new DataItemDetailBLL().GetItemValue("imgUrl"));
                pagination.p_tablename = "mae_suppliesfactory a left join base_fileinfo b on a.recid = b.recid";
                pagination.conditionJson = "1=1";
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();


                if (currUser.IsSystem)
                {
                    pagination.conditionJson = "and 1=1";
                }
                else
                {
                    pagination.conditionJson += " and a.createuserorgcode='" + currUser.OrganizeCode + "'";

                }

                if (!string.IsNullOrEmpty(suppliesName))
                {
                    pagination.conditionJson += string.Format(" and a.name like '%{0}%' ", suppliesName);
                }
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                });
                var data = reserverplanbll.GetPageList(pagination, queryJson);

                //DataRow[]  selectRow = data.Select(" filepath ='{0}'",))

                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        #endregion

        #region 保存应急物资
        /// <summary>
        /// 保存应急物资
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveSupplies()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                OperatorProvider.AppUserId = userid;
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                HttpFileCollection files = HttpContext.Current.Request.Files;
                SuppliesEntity entity = new SuppliesEntity()
                {
                    SUPPLIESNAME = dy.data.suppliesname,
                    SUPPLIESTYPE = dy.data.suppliestype,
                    SUPPLIESTYPENAME = dy.data.suppliestypename,
                    NUM = Convert.ToInt32(dy.data.num),
                    SUPPLIESUNTIL = dy.data.suppliesuntil,
                    SUPPLIESUNTILNAME = dy.data.suppliesuntilname,
                    USERID = dy.data.dutypersonid,
                    USERNAME = dy.data.dutypersonname,
                    DEPARTID = dy.data.departid,
                    DEPARTNAME = dy.data.departname,
                    STORAGEPLACE = dy.data.storageplace,
                    CREATEDATE = Convert.ToDateTime(dy.data.createdate),
                    Models = res.Contains("models") ? dy.data.models : "",
                    FILES = dy.data.recid,
                    SUPPLIESCODE = suppliesbll.GetMaxCode()
                };
                entity.FILES = string.IsNullOrEmpty(entity.FILES) ? Guid.NewGuid().ToString() : entity.FILES;
                UploadifyFile(entity.FILES, files);
                suppliesbll.SaveForm("", entity);
                var entityInorOut = new InoroutrecordEntity
                {
                    USERID = entity.USERID,
                    USERNAME = entity.USERNAME,
                    DEPARTID = entity.DEPARTID,
                    DEPARTNAME = entity.DEPARTNAME,
                    INOROUTTIME = DateTime.Now,
                    SUPPLIESCODE = entity.SUPPLIESCODE,
                    SUPPLIESTYPE = entity.SUPPLIESTYPE,
                    SUPPLIESTYPENAME = entity.SUPPLIESTYPENAME,
                    SUPPLIESNAME = entity.SUPPLIESNAME,
                    SUPPLIESUNTIL = entity.SUPPLIESUNTIL,
                    SUPPLIESUNTILNAME = entity.SUPPLIESUNTILNAME,
                    NUM = int.Parse(entity.NUM.ToString()),
                    STORAGEPLACE = entity.STORAGEPLACE,
                    MOBILE = entity.MOBILE,
                    SUPPLIESID = entity.ID,
                    STATUS = 0
                };
                var inoroutrecordbll = new InoroutrecordBLL();
                inoroutrecordbll.SaveForm("", entityInorOut);
                return new { Code = 0, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }

        #endregion

        #region 获取应急物资列表
        /// <summary>
        /// 获取应急物资列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSuppliesList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string suppliesName = dy.data.suppliesname;
                long pageIndex = dy.pageindex;
                long pageSize = dy.pagesize;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "a.id";
                pagination.p_fields = string.Format("a.suppliesname,a.num,a.storageplace,a.suppliesuntilname,'{0}' || substr(b.filepath,2,length(b.filepath)-1) as filepath", new DataItemDetailBLL().GetItemValue("imgUrl"));
                pagination.p_tablename = "MAE_SUPPLIES a left join base_fileinfo b on a.files = b.recid";
                pagination.conditionJson = "1=1";
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();


                if (currUser.IsSystem)
                {
                    pagination.conditionJson = " and 1=1";
                }
                else
                {
                    pagination.conditionJson += " and a.createuserdeptcode='" + currUser.DeptCode + "'";

                }

                if (!string.IsNullOrEmpty(suppliesName))
                {
                    pagination.conditionJson += string.Format(" and a.suppliesname like '%{0}%' ", suppliesName);
                }
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                });
                var data = suppliesbll.GetPageList(pagination, queryJson);
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取应急物资详细
        /// <summary>
        /// 获取应急物资详细
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSuppliesForm([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string keyValue = dy.data.keyvalue;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                SuppliesEntity suppliesentity = suppliesbll.GetEntity(keyValue);
                var filelist = fileinfobll.GetFileList(suppliesentity.FILES);
                return new
                {
                    Code = 0,
                    Count = 1,
                    Info = "获取数据成功",
                    data = new
                    {
                        id = suppliesentity.ID,
                        suppliesname = suppliesentity.SUPPLIESNAME,
                        suppliestype = suppliesentity.SUPPLIESTYPE,
                        suppliestypename = suppliesentity.SUPPLIESTYPENAME,
                        num = suppliesentity.NUM,
                        suppliesuntil = suppliesentity.SUPPLIESUNTIL,
                        suppliesuntilname = suppliesentity.SUPPLIESUNTILNAME,
                        dutypersonid = suppliesentity.USERID,
                        dutypersonname = suppliesentity.USERNAME,
                        departid = suppliesentity.DEPARTID,
                        departname = suppliesentity.DEPARTNAME,
                        storageplace = suppliesentity.STORAGEPLACE,
                        createdate = suppliesentity.CREATEDATE.Value.ToString("yyyy-MM-dd hh:mm:ss"),
                        filepath = filelist.Count > 0 ? new DataItemDetailBLL().GetItemValue("imgUrl") + filelist[0].FilePath.Substring(1, filelist[0].FilePath.Length - 1) : ""
                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 应急物资变更
        /// <summary>
        /// 应急物资变更
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveSuppliesChange([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                OperatorProvider.AppUserId = userid;
                string keyValue = dy.data.keyvalue;
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                SuppliesEntity suppliesentity = suppliesbll.GetEntity(keyValue);
                InoroutrecordEntity entity = new InoroutrecordEntity()
                {
                    SUPPLIESCODE = suppliesentity.SUPPLIESCODE,
                    DEPARTID = suppliesentity.DEPARTID,
                    DEPARTNAME = suppliesentity.DEPARTNAME,
                    SUPPLIESTYPE = suppliesentity.SUPPLIESTYPE,
                    SUPPLIESTYPENAME = suppliesentity.SUPPLIESTYPENAME,
                    SUPPLIESNAME = suppliesentity.SUPPLIESNAME,
                    SUPPLIESUNTIL = suppliesentity.SUPPLIESUNTIL,
                    SUPPLIESUNTILNAME = suppliesentity.SUPPLIESUNTILNAME,
                    STORAGEPLACE = suppliesentity.STORAGEPLACE,
                    MOBILE = suppliesentity.MOBILE,
                    NUM = int.Parse(dy.data.num.ToString()),
                    USERID = dy.data.userid,
                    USERNAME = dy.data.username,
                    INOROUTTIME = Convert.ToDateTime(dy.data.inorouttime),
                    STATUS = int.Parse(dy.data.status),
                    STATUNAME = dy.data.statusname,
                    SUPPLIESID = keyValue
                };
                if (entity.STATUS == 1)
                {
                    entity.OUTREASON = dy.data.outreason;
                    entity.OUTREASONNAME = dy.data.outreasonname;
                }
                inoroutrecordbll.SaveForm("", entity);

                //入库
                if (entity.STATUS == 2)
                    suppliesentity.NUM += entity.NUM;
                else
                    suppliesentity.NUM -= entity.NUM;
                suppliesbll.SaveForm(suppliesentity.ID, suppliesentity);
                return new { Code = 0, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        #endregion

        #region 保存应急演练记录
        /// <summary>
        /// 保存应急演练记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveDrillRecord()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string keyvalue = res.Contains("keyvalue") ? dy.data.keyvalue : "";
                DrillplanrecordEntity entity = drillplanrecordbll.GetEntity(keyvalue);
                if (null == entity)
                {
                    entity = new DrillplanrecordEntity();
                    entity.DrillStepRecordId = Guid.NewGuid().ToString();
                    entity.YLXCFILES = Guid.NewGuid().ToString();
                    entity.YLZJFILES = Guid.NewGuid().ToString();
                    entity.VideoFiles = Guid.NewGuid().ToString();//演练现场附件
                }
                entity.IsConnectPlan = res.Contains("isconnectplan") ? dy.data.isconnectplan : ""; //是否关联应急演练计划
                entity.DrillLevel = res.Contains("drilllevel") ? dy.data.drilllevel : ""; //演练级别
                entity.MAINCONTENT = res.Contains("maincontent") ? dy.data.maincontent : "";  //演练内容
                entity.NAME = dy.data.name;
                entity.DRILLMODE = dy.data.drillmode;
                entity.DRILLMODENAME = dy.data.drillmodename;
                entity.DRILLTIME = Convert.ToDateTime(dy.data.drilltime);//演练时间
                entity.Compere = dy.data.compere;
                entity.CompereName = dy.data.comperename;
                entity.DRILLPLACE = dy.data.drillplace;
                entity.DrillPeople = dy.data.drillpeople;
                entity.DrillPeopleName = dy.data.drillpeoplename;
                entity.DRILLPEOPLENUMBER = int.Parse(dy.data.drillpeoplenum.ToString());
                entity.DRILLPLANID = dy.data.drillplanid;
                entity.DRILLPLANNAME = dy.data.drillplanname;
                entity.OrgDept = res.Contains("orgdept") ? dy.data.orgdept : curUser.DeptName;
                entity.OrgDeptId = res.Contains("orgdeptid") ? dy.data.orgdeptid : curUser.DeptId; 
                entity.OrgDeptCode = res.Contains("orgdeptcode") ? dy.data.orgdeptcode : curUser.DeptCode;
                if (entity.IsConnectPlan == "是")
                {
                    DrillplanEntity planentity = drillplanbll.GetEntity(dy.data.drillplanid.ToString());
                    entity.DRILLTYPE = planentity.DRILLTYPE;
                    entity.DRILLTYPENAME = planentity.DRILLTYPENAME;
                }
                else
                {
                    ReserverplanEntity reserverplanentity = reserverplanbll.GetEntity(dy.data.drillplanid.ToString());
                    if (null != reserverplanentity)
                    {
                        entity.DRILLTYPE = reserverplanentity.PLANTYPE;
                        entity.DRILLTYPENAME = reserverplanentity.PLANTYPENAME;
                    }
                }
                entity.DEPARTID = curUser.DeptId;
                entity.DEPARTNAME = curUser.DeptName;
                entity.WarnTime = dy.data.warntime;
                entity.Status = dy.data.status;
                keyvalue = string.IsNullOrEmpty(keyvalue) ? Guid.NewGuid().ToString() : keyvalue;
                entity.ID = keyvalue;

                string message = "请选择格式正确的文件再导入!";
                int count = HttpContext.Current.Request.Files.Count;
                #region MyRegion
                List<DrillplanrecordstepEntity> list = new List<DrillplanrecordstepEntity>();
                if (count > 0)
                {
                    var file = HttpContext.Current.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return new { Code = -1, Count = 0, Info = message };
                    }
                    if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                    {
                        return new { Code = -1, Count = 0, Info = message };
                    }
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\ResourceFile";
                    //创建文件夹
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    file.SaveAs(dir + fileName);
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                    wb.Open(dir + fileName);
                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                    DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, false);
                    entity.DrillPurpose = dt.Rows[0][1].ToString();
                    entity.SceneSimulation = dt.Rows[1][1].ToString();
                    entity.DrillKeyPoint = dt.Rows[2][1].ToString();
                    //---****值存在空验证*****--
                    if (string.IsNullOrEmpty(entity.DrillPurpose) || string.IsNullOrEmpty(entity.SceneSimulation) || string.IsNullOrEmpty(entity.DrillKeyPoint))
                    {
                        return new { Code = -1, Count = 0, Info = "请填写必填栏位." };
                    }
                    if (dt.Rows.Count <= 4)
                    {
                        return new { Code = -1, Count = 0, Info = "请填写演练步骤." };
                    }
                    for (int i = 4; i < dt.Rows.Count; i++)
                    {
                        DrillplanrecordstepEntity stepEntity = new DrillplanrecordstepEntity();
                        stepEntity.SortId = dt.Rows[i][0].IsEmpty() ? "" : dt.Rows[i][0].ToString();
                        if (string.IsNullOrEmpty(dt.Rows[i][1].ToString()))
                        {
                            return new { Code = -1, Count = 0, Info = "请填写演练步骤." };
                        }
                        stepEntity.Content = dt.Rows[i][1].ToString();
                        stepEntity.DrillStepRecordId = entity.DrillStepRecordId;
                        list.Add(stepEntity);
                    }
                    drillplanrecordstepbll.RemoveFormByRecid(entity.DrillStepRecordId);
                    foreach (var item in list)
                    {
                        drillplanrecordstepbll.SaveForm("", item);
                    }
                    entity.DrillSchemeName = file.FileName;
                }
                #endregion

                drillplanrecordbll.SaveForm(keyvalue, entity);
                return new { Code = 0, Count = 0, Info = "保存成功", Data = entity.ID };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 保存应急演练记录
        /// <summary>
        /// 保存应急演练记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveDrillRecordContent()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string keyvalue = res.Contains("keyvalue") ? dy.data.keyvalue : "";
                DrillplanrecordEntity entity = drillplanrecordbll.GetEntity(keyvalue);
                if (!string.IsNullOrEmpty(entity.YLXCFILES))
                {
                    entity.YLXCFILES = Guid.NewGuid().ToString();
                }
                if (!string.IsNullOrEmpty(entity.VideoFiles))
                {
                    entity.VideoFiles = Guid.NewGuid().ToString();
                }
                entity.MAINCONTENT = res.Contains("maincontent") ? dy.data.maincontent : "";  //演练内容

                //演练现场附件
                HttpFileCollection files = HttpContext.Current.Request.Files;

                List<HttpPostedFile> imgfile = new List<HttpPostedFile>();

                List<HttpPostedFile> vediofile = new List<HttpPostedFile>();

                for (int i = 0; i < files.Count; i++)
                {
                    string FileEextension = Path.GetExtension(files[i].FileName);

                    if (FileEextension.ToLower().Contains("jpg") || FileEextension.ToLower().Contains("gif") || FileEextension.ToLower().Contains("bmp") || FileEextension.ToLower().Contains("png"))
                    {
                        imgfile.Add(files[i]);
                    }

                    if (FileEextension.ToLower().Contains("mp4") || FileEextension.ToLower().Contains("mp3") || FileEextension.ToLower().Contains("avi") || FileEextension.ToLower().Contains("mc"))
                    {
                        vediofile.Add(files[i]);
                    }
                }

                UploadifyFile(entity.YLXCFILES, imgfile); //演练现场图片

                UploadifyFile(entity.VideoFiles, vediofile);  //演练现场视频

                drillplanrecordbll.SaveForm(keyvalue, entity);
                return new { Code = 0, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 保存应急演练记录开展日常活动
        /// <summary>
        /// 保存应急演练记录开展日常活动开始
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveDrillRecordForRCHDStart()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string keyvalue = res.Contains("keyvalue") ? dy.data.keyvalue : "";
                DrillplanrecordEntity entity = drillplanrecordbll.GetEntity(keyvalue);
                if (null == entity)
                {
                    entity = new DrillplanrecordEntity();
                    entity.DrillStepRecordId = Guid.NewGuid().ToString();
                    entity.YLXCFILES = Guid.NewGuid().ToString();
                    entity.YLZJFILES = Guid.NewGuid().ToString();
                    entity.VideoFiles = Guid.NewGuid().ToString();//演练现场附件
                }
                entity.IsConnectPlan = "是"; //是否关联应急演练计划
                entity.DrillLevel = res.Contains("drilllevel") ? dy.data.drilllevel : ""; //演练级别
                entity.MAINCONTENT = res.Contains("maincontent") ? dy.data.maincontent : "";  //演练内容
                entity.NAME = dy.data.name;
                entity.DRILLTIME = Convert.ToDateTime(dy.data.drilltime);//演练时间
                entity.Compere = dy.data.compere;
                entity.CompereName = dy.data.comperename;
                entity.DrillPeople = dy.data.drillpeople;
                entity.DrillPeopleName = dy.data.drillpeoplename;
                entity.DRILLPLACE = dy.data.drillplace;
                entity.OrgDept = res.Contains("orgdept") ? dy.data.orgdept : curUser.DeptName;
                entity.OrgDeptId = res.Contains("orgdeptid") ? dy.data.orgdeptid : curUser.DeptId;
                entity.OrgDeptCode = res.Contains("orgdeptcode") ? dy.data.orgdeptcode : curUser.DeptCode;
                entity.DEPARTID = curUser.DeptId;
                entity.DEPARTNAME = curUser.DeptName;
                entity.WarnTime = dy.data.warntime;
                entity.Status = "0";
                keyvalue = string.IsNullOrEmpty(keyvalue) ? Guid.NewGuid().ToString() : keyvalue;
                entity.ID = keyvalue;
                drillplanrecordbll.SaveForm(keyvalue, entity);
                return new { Code = 0, Count = 0, Info = "保存成功", data = keyvalue };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 保存应急演练记录开展日常活动
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveDrillRecordForRCHD()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string keyvalue = res.Contains("keyvalue") ? dy.data.keyvalue : "";
                string deletefileids = res.Contains("deletefileids") ? dy.data.deletefileids : "";
                DrillplanrecordEntity entity = drillplanrecordbll.GetEntity(keyvalue);
                if (null == entity)
                {
                    entity = new DrillplanrecordEntity();
                    entity.DrillStepRecordId = Guid.NewGuid().ToString();
                    entity.YLXCFILES = Guid.NewGuid().ToString();
                    entity.YLZJFILES = Guid.NewGuid().ToString();
                    entity.VideoFiles = Guid.NewGuid().ToString();//演练现场附件
                }
                entity.IsConnectPlan = res.Contains("isconnectplan") ? dy.data.isconnectplan : ""; //是否关联应急演练计划
                entity.DrillLevel = res.Contains("drilllevel") ? dy.data.drilllevel : ""; //演练级别
                entity.MAINCONTENT = res.Contains("maincontent") ? dy.data.maincontent : "";  //演练内容
                entity.NAME = dy.data.name;
                entity.DRILLMODE = dy.data.drillmode;
                entity.DRILLMODENAME = dy.data.drillmodename;
                entity.DRILLTIME = Convert.ToDateTime(dy.data.drilltime);//演练时间
                entity.Compere = dy.data.compere;
                entity.CompereName = dy.data.comperename;
                entity.DRILLPLACE = dy.data.drillplace;
                entity.DrillPeople = dy.data.drillpeople;
                entity.DrillPeopleName = dy.data.drillpeoplename;
                entity.DRILLPEOPLENUMBER = int.Parse(dy.data.drillpeoplenum.ToString());
                entity.DRILLPLANID = dy.data.drillplanid;
                entity.DRILLPLANNAME = dy.data.drillplanname;
                entity.OrgDept = res.Contains("orgdept") ? dy.data.orgdept : curUser.DeptName;
                entity.OrgDeptId = res.Contains("orgdeptid") ? dy.data.orgdeptid : curUser.DeptId;
                entity.OrgDeptCode = res.Contains("orgdeptcode") ? dy.data.orgdeptcode : curUser.DeptCode;
                if (entity.IsConnectPlan == "是")
                {
                    DrillplanEntity planentity = drillplanbll.GetEntity(dy.data.drillplanid.ToString());
                    entity.DRILLTYPE = planentity.DRILLTYPE;
                    entity.DRILLTYPENAME = planentity.DRILLTYPENAME;
                }
                else
                {
                    ReserverplanEntity reserverplanentity = reserverplanbll.GetEntity(dy.data.drillplanid.ToString());
                    if (null != reserverplanentity)
                    {
                        entity.DRILLTYPE = reserverplanentity.PLANTYPE;
                        entity.DRILLTYPENAME = reserverplanentity.PLANTYPENAME;
                    }
                }
                entity.DEPARTID = curUser.DeptId;
                entity.DEPARTNAME = curUser.DeptName;
                entity.WarnTime = dy.data.warntime;
                entity.Status = dy.data.status;
                keyvalue = string.IsNullOrEmpty(keyvalue) ? Guid.NewGuid().ToString() : keyvalue;
                entity.ID = keyvalue;
                //演练现场附件
                HttpFileCollection files = HttpContext.Current.Request.Files;

                List<HttpPostedFile> imgfile = new List<HttpPostedFile>();

                List<HttpPostedFile> vediofile = new List<HttpPostedFile>();

                List<HttpPostedFile> ylfile = new List<HttpPostedFile>();

                for (int i = 0; i < files.Count; i++)
                {
                    string FileEextension = Path.GetExtension(files[i].FileName);

                    if (files.AllKeys[i] == "ylfile")
                    {
                        ylfile.Add(files[i]);
                    }
                    else
                    {
                        if (FileEextension.ToLower().Contains("jpg") || FileEextension.ToLower().Contains("gif") || FileEextension.ToLower().Contains("bmp") || FileEextension.ToLower().Contains("png"))
                        {
                            imgfile.Add(files[i]);
                        }

                        if (FileEextension.ToLower().Contains("mp4") || FileEextension.ToLower().Contains("mp3") || FileEextension.ToLower().Contains("avi") || FileEextension.ToLower().Contains("mc"))
                        {
                            vediofile.Add(files[i]);
                        }
                    }

                }
                //如果有删除的文件，则进行删除
                if (!string.IsNullOrEmpty(deletefileids))
                {
                    DeleteFile(deletefileids);
                }
                UploadifyFile(entity.YLZJFILES, ylfile); //演练附件

                UploadifyFile(entity.YLXCFILES, imgfile); //演练现场图片

                UploadifyFile(entity.VideoFiles, vediofile);  //演练现场视频
                drillplanrecordbll.SaveForm(keyvalue, entity);
                return new { Code = 0, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 保存评估表
        /// <summary>
        /// 保存评估表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveDrillAssess()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string keyvalue = dy.data.keyvalue;
                string deletefileids = res.Contains("deletefileids") ? dy.data.deletefileids : "";
                DrillplanrecordEntity drillplanrecordentity = drillplanrecordbll.GetEntity(keyvalue);
                drillplanrecordentity.DrillPurpose = dy.data.drillpurpose;
                drillplanrecordentity.SceneSimulation = dy.data.scenesimulation;
                drillplanrecordentity.DrillKeyPoint = dy.data.drillkeypoint;
                drillplanrecordentity.SelfScore = int.Parse(dy.data.selfscore.ToString());
                drillplanrecordentity.Status = res.Contains("status") ? (string.IsNullOrWhiteSpace(dy.data.status) ? "2" : dy.data.status) : "2";
                string jsondata = string.Empty;
                List<object> jsonlist = new List<object>();
                jsonlist.Add(new { key = "Suitable", value = dy.data.suitable });
                jsonlist.Add(new { key = "Fullable", value = dy.data.fullable });
                jsonlist.Add(new { key = "PersonStandBy", value = dy.data.personstandby });
                jsonlist.Add(new { key = "PersonStandByDuty", value = dy.data.personstandbyduty });
                jsonlist.Add(new { key = "SiteSupplies", value = dy.data.sitesupplies });
                jsonlist.Add(new { key = "SiteSuppliesDuty", value = dy.data.sitesuppliesduty });
                jsonlist.Add(new { key = "WholeOrganize", value = dy.data.wholeorganize });
                jsonlist.Add(new { key = "DivideWork", value = dy.data.dividework });
                jsonlist.Add(new { key = "EffecteValuate", value = dy.data.effectevaluate });
                jsonlist.Add(new { key = "ReportSuperior", value = dy.data.reportsuperior });
                jsonlist.Add(new { key = "Rescue", value = dy.data.rescue });
                jsonlist.Add(new { key = "Evacuate", value = dy.data.evacuate });
                jsonlist.Add(new { key = "ValuatePerson", value = dy.data.valuateperson });
                jsonlist.Add(new { key = "ValuatePersonName", value = dy.data.valuatepersonname });
                jsonlist.Add(new { key = "Problem", value = dy.data.problem });
                jsonlist.Add(new { key = "Measure", value = dy.data.measure });
                jsonlist.Add(new { key = "DrillName", value = drillplanrecordentity.DRILLPLANNAME });
                jsonlist.Add(new { key = "DrillPlace", value = drillplanrecordentity.DRILLPLACE });
                jsonlist.Add(new { key = "OrganizeDept", value = drillplanrecordentity.DEPARTNAME });
                jsonlist.Add(new { key = "TopPerson", value = drillplanrecordentity.CompereName });
                jsonlist.Add(new { key = "DrillTime", value = drillplanrecordentity.DRILLTIME });
                jsonlist.Add(new { key = "DrillType", value = drillplanrecordentity.DRILLMODENAME });
                jsonlist.Add(new { key = "DrillContent", value = drillplanrecordentity.MAINCONTENT });

                jsondata = JsonConvert.SerializeObject(jsonlist);
                #region 注释
                //DrillassessEntity entity = new DrillassessEntity()
                //{
                //    Suitable = dy.data.suitable,
                //    Fullable = dy.data.fullable,
                //    PersonStandBy = dy.data.personstandby,
                //    PersonStandByDuty = dy.data.personstandbyduty,
                //    SiteSupplies = dy.data.sitesupplies,
                //    SiteSuppliesDuty = dy.data.sitesuppliesduty,
                //    WholeOrganize = dy.data.wholeorganize,
                //    DivideWork = dy.data.dividework,
                //    EffecteValuate = dy.data.effectevaluate,
                //    ReportSuperior = dy.data.reportsuperior,
                //    Rescue = dy.data.rescue,
                //    Evacuate = dy.data.evacuate,
                //    ValuatePerson = dy.data.valuateperson,
                //    ValuatePersonName = dy.data.valuatepersonname,
                //    Problem = dy.data.problem,
                //    Measure = dy.data.measure,
                //    DrillId = keyvalue,
                //    DrillName = drillplanrecordentity.DRILLPLANNAME,
                //    DrillPlace = drillplanrecordentity.DRILLPLACE,
                //    OrganizeDept = drillplanrecordentity.DEPARTNAME,
                //    TopPerson = drillplanrecordentity.CompereName,
                //    DrillTime = drillplanrecordentity.DRILLTIME,
                //    DrillType = drillplanrecordentity.DRILLMODENAME,
                //    DrillContent = drillplanrecordentity.MAINCONTENT
                //};
                //drillassessbll.SaveForm("", entity); 
                #endregion
                drillplanrecordentity.ASSESSDATA = jsondata;
                string moduleName = "班组级演练记录评价";
                //drillplanrecordentity.DrillLevel = res.Contains("dirlllevel") ? dy.data.drilllevel : "班组级";
                drillplanrecordentity.IsCommit = 1;
                ManyPowerCheckEntity mpcEntity = null;
                if (string.IsNullOrWhiteSpace(drillplanrecordentity.CREATEUSERID) && string.IsNullOrWhiteSpace(drillplanrecordentity.CREATEUSERDEPTCODE))
                {
                    drillplanrecordentity.Create();
                }
                mpcEntity = peoplereviewbll.CheckEvaluateForNextFlow(curUser, moduleName, drillplanrecordentity);
                if (null != mpcEntity)
                {
                    drillplanrecordentity.EvaluateDept = mpcEntity.CHECKDEPTNAME;
                    drillplanrecordentity.EvaluateDeptId = mpcEntity.CHECKDEPTID;
                    drillplanrecordentity.EvaluateDeptCode = mpcEntity.CHECKDEPTCODE;
                    drillplanrecordentity.EvaluateRoleId = mpcEntity.CHECKROLEID;
                    drillplanrecordentity.EvaluateRole = mpcEntity.CHECKROLENAME;
                    drillplanrecordentity.NodeId = mpcEntity.ID;
                    drillplanrecordentity.NodeName = mpcEntity.FLOWNAME;
                    drillplanrecordentity.IsStartConfig = 1;
                    drillplanrecordentity.IsOverEvaluate = 0;
                }
                else
                {
                    drillplanrecordentity.IsStartConfig = 0;
                    drillplanrecordentity.IsOverEvaluate = 0;
                    //未配置审核项
                    drillplanrecordentity.EvaluateDept = "";
                    drillplanrecordentity.EvaluateDeptId = "";
                    drillplanrecordentity.EvaluateDeptCode = "";
                    drillplanrecordentity.EvaluateRoleId = "";
                    drillplanrecordentity.NodeId = "";
                    drillplanrecordentity.NodeName = "";
                }
                drillplanrecordbll.SaveForm(keyvalue, drillplanrecordentity);
                HttpFileCollection files = HttpContext.Current.Request.Files;
                UploadifyFile(drillplanrecordentity.YLXCFILES, files);
                //如果有删除的文件，则进行删除
                if (!string.IsNullOrEmpty(deletefileids))
                {
                    DeleteFile(deletefileids);
                }
                IList<DrillplanrecordstepEntity> steplist = JsonConvert.DeserializeObject<List<DrillplanrecordstepEntity>>(JsonConvert.SerializeObject(dy.data.steplist));
                foreach (var item in steplist)
                {
                    item.DrillStepRecordId = drillplanrecordentity.DrillStepRecordId;
                    drillplanrecordstepbll.SaveForm(item.Id, item);
                }


                return new { Code = 0, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        #endregion

        #region 获取应急演练记录列表
        /// <summary>
        /// 获取应急预案列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetListDrillRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                long pageIndex = 1;
                long pageSize = 10000;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "id";
                pagination.p_fields = "status,name,isconnectplan,drilllevel,drillplanid,drillplanname,drillmode,drillmodename,to_char(drilltime,'yyyy-mm-dd hh24:mi:ss') as drilltime,compere,comperename,drillplace,drillpeople,drillpeoplename,drillpeoplenumber as drillpeoplenum,warntime,drillschemename";
                pagination.p_tablename = "mae_drillplanrecord";
                pagination.conditionJson = "status!='2' and status is not null";
                pagination.sidx = "status";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();

                if (currUser.IsSystem)
                {
                    pagination.conditionJson = " and 1=1";
                }
                else
                {
                    //20210118 更改为平台端所有人都可以新增厂级、部门级、班组级数据  但是班组终端只能看到本部门新增的班组级数据
                    pagination.conditionJson += " and createuserdeptcode='" + currUser.DeptCode + "' and DrillLevel='班组级'";

                }

                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                });
                var data = drillplanrecordbll.GetPageList(pagination, queryJson);
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取应急记录详细
        /// <summary>
        /// 获取应急记录详细
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDrillRecordForm([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string keyValue = res.Contains("keyvalue") ? dy.data.keyvalue : "";
                DrillplanrecordEntity drillplanrecordentity = drillplanrecordbll.GetEntity(keyValue);
                IList<DrillplanrecordstepEntity> steplist = drillplanrecordstepbll.GetListByRecid(drillplanrecordentity.DrillStepRecordId);
                return new
                {
                    Code = 0,
                    Count = 1,
                    Info = "获取数据成功",
                    data = new
                    {
                        id = drillplanrecordentity.ID,
                        name = drillplanrecordentity.NAME,
                        drillpurpose = drillplanrecordentity.DrillPurpose,
                        scenesimulation = drillplanrecordentity.SceneSimulation,
                        drillkeypoint = drillplanrecordentity.DrillKeyPoint,
                        isconnectplan = drillplanrecordentity.IsConnectPlan,
                        drilllevel = drillplanrecordentity.DrillLevel,
                        maincontent = drillplanrecordentity.MAINCONTENT,
                        drillsteplist = steplist.Select(g => new
                        {
                            stepid = g.Id,
                            sortid = g.SortId,
                            content = g.Content
                        }).ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取应急记录台账列表
        /// <summary>
        /// 获取应急记录台账列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDrillRecordBaseList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string startdate = dy.data.startdate ?? "";
                string enddate = dy.data.enddate ?? "";
                long pageIndex = dy.pageindex;
                long pageSize = dy.pagesize;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());

                Operator currUser = OperatorProvider.Provider.Current();

                string strsql = string.Format(@"select a.id,a.name,to_char(a.drilltime,'yyyy-mm-dd hh24:mi:ss') as drilltime,decode(max(b.filepath),'','','{0}' || substr(max(b.filepath),2,length(max(b.filepath))-1)) as filepath from MAE_DRILLPLANRECORD a left join base_fileinfo b on a.ylxcfiles = b.recid
                                       where a.status='2' ", new DataItemDetailBLL().GetItemValue("imgUrl"));
                if (currUser.IsSystem)
                {
                    strsql += " and 1=1";
                }
                else
                {
                    strsql += " and a.createuserdeptcode='" + currUser.DeptCode + "'";

                }

                if (!string.IsNullOrEmpty(startdate))
                {
                    strsql += string.Format(" and to_char(a.drilltime,'yyyy-mm-dd') >=to_char(to_date('{0}','yyyy-mm-dd '),'yyyy-mm-dd')", startdate);
                }
                if (!string.IsNullOrEmpty(enddate))
                {
                    strsql += string.Format(" and to_char(drilltime,'yyyy-mm-dd') <=to_char(to_date('{0}','yyyy-mm-dd'),'yyyy-mm-dd')", enddate);
                }
                strsql += " group by a.id,a.name,drilltime order by a.drilltime asc";
                pagination.p_kid = "id";
                pagination.p_fields = "name,drilltime,filepath";
                pagination.p_tablename = "(" + strsql + ")";
                pagination.conditionJson = "1=1";
                pagination.sidx = "drilltime";//排序字段
                pagination.sord = "desc";//排序方式
                var data = drillplanrecordbll.GetPageList(pagination, "");

                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取应急记录台账详细

        /// <summary>
        /// 获取应急记录台账详细
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDrillRecordBase([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string keyValue = dy.data.keyvalue;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                DrillplanrecordEntity drillplanrecordentity = drillplanrecordbll.GetEntity(keyValue);
                IList<DrillplanrecordstepEntity> steplist = drillplanrecordstepbll.GetListByRecid(drillplanrecordentity.DrillStepRecordId);

                IList<FileInfoEntity> filelist = fileinfobll.GetFileList(drillplanrecordentity.YLXCFILES);//演练现场图片附件
                IList<FileInfoEntity> filelist1 = fileinfobll.GetFileList(drillplanrecordentity.YLZJFILES);
                IList<FileInfoEntity> filelist2 = fileinfobll.GetFileList(drillplanrecordentity.VideoFiles);//音频视频附件
                IList<DrillrecordevaluateEntity> evaluatelist = drillrecordevaluatebll.GetList().Where(x => x.DrillRecordId == keyValue).ToList();
                string jsondata = drillplanrecordentity.ASSESSDATA;
                DrillassessEntity drillassessentity = new DrillassessEntity();
                drillassessentity.DrillId = keyValue;
                if (!string.IsNullOrEmpty(jsondata))
                {
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(jsondata);

                    foreach (JObject jobj in jarray)
                    {
                        string key = jobj["key"].ToString();
                        string value = jobj["value"].ToString();
                        if (key == "DrillName") { drillassessentity.DrillName = value; }
                        else if (key == "DrillPlace") { drillassessentity.DrillPlace = value; }
                        else if (key == "OrganizeDept") { drillassessentity.OrganizeDept = value; }
                        else if (key == "TopPerson") { drillassessentity.TopPerson = value; }
                        else if (key == "DrillTime") { drillassessentity.DrillTime = Convert.ToDateTime(value); }
                        else if (key == "DrillType") { drillassessentity.DrillType = value; }
                        else if (key == "DrillContent") { drillassessentity.DrillContent = value; }
                        else if (key == "Suitable") { drillassessentity.Suitable = value; }
                        else if (key == "Fullable") { drillassessentity.Fullable = value; }
                        else if (key == "PersonStandBy") { drillassessentity.PersonStandBy = value; }
                        else if (key == "PersonStandByDuty") { drillassessentity.PersonStandByDuty = value; }
                        else if (key == "SiteSupplies") { drillassessentity.SiteSupplies = value; }
                        else if (key == "SiteSuppliesDuty") { drillassessentity.SiteSuppliesDuty = value; }
                        else if (key == "WholeOrganize") { drillassessentity.WholeOrganize = value; }
                        else if (key == "DivideWork") { drillassessentity.DivideWork = value; }
                        else if (key == "EffecteValuate") { drillassessentity.EffecteValuate = value; }
                        else if (key == "ReportSuperior") { drillassessentity.ReportSuperior = value; }
                        else if (key == "Rescue") { drillassessentity.Rescue = value; }
                        else if (key == "Evacuate") { drillassessentity.Evacuate = value; }
                        else if (key == "Problem") { drillassessentity.Problem = value; }
                        else if (key == "Measure") { drillassessentity.Measure = value; }
                        else if (key == "ValuatePersonName") { drillassessentity.ValuatePersonName = value; }
                        else if (key == "ValuatePerson") { drillassessentity.ValuatePerson = value; }
                    }
                }
                var model = new
                {
                    id = drillplanrecordentity.ID,
                    drillplanrecordentity = drillplanrecordentity,
                    drillsteplist = steplist.Select(g => new
                    {
                        stepid = g.Id,
                        sortid = g.SortId,
                        content = g.Content,
                        dutypersonname = g.DutyPersonName,
                        dutyperson = g.DutyPerson
                    }).ToList(),
                    ylfilelist = filelist1.Select(g => new
                    {
                        id = g.FileId,
                        filename = g.FileName,
                        filepath = new DataItemDetailBLL().GetItemValue("imgUrl") + g.FilePath.Replace("~", "")
                    }).ToList(),
                    picturelist = filelist.Select(g => new
                    {
                        id = g.FileId,
                        filename = g.FileName,
                        filepath = new DataItemDetailBLL().GetItemValue("imgUrl") + g.FilePath.Replace("~", "")
                    }).ToList(),
                    vediolist = filelist2.Select(g => new
                    {
                        id = g.FileId,
                        filename = g.FileName,
                        filepath = new DataItemDetailBLL().GetItemValue("imgUrl") + g.FilePath.Replace("~", "")
                    }).ToList(),
                    drillassessentity = drillassessentity,  //评估对象
                    evaluatelist = evaluatelist  //评价对象
                };
                Dictionary<string,
                string> dict_props = new Dictionary<string, string>();
                //Id 转换前的列名  keyvalue 转换后的列名
                //dict_props.Add("Id", "keyvalue");

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                    //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                };
                return new { code = 0, Info = "获取数据成功", data = JObject.Parse(JsonConvert.SerializeObject(model, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取应急物资台账
        /// <summary>
        /// 获取应急物资台账
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSuppliesInorOutRecord([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            string name = dy.data.name;
            string startdate = dy.data.startdate;
            string enddate = dy.data.enddate;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, Info = "请求失败,请登录!" };
            }
            Pagination pagination = new Pagination();
            pagination.page = int.Parse(dy.pageindex.ToString());
            pagination.rows = int.Parse(dy.pagesize.ToString());
            pagination.p_kid = "id";
            pagination.p_fields = " suppliesname,to_char(InOrOutTime,'yyyy-mm-dd hh24:mi:ss') as inorouttime,userid,username,num,statuname";
            pagination.p_tablename = "MAE_INOROUTRECORD t";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CreateDate";
            pagination.sord = "desc";

            if (curUser.IsSystem)
            {
                pagination.conditionJson = " and 1=1";
            }
            else
            {
                pagination.conditionJson += " and createuserdeptcode='" + curUser.DeptCode + "'";

            }

            if (!string.IsNullOrEmpty(name))
            {
                pagination.conditionJson += " and SUPPLIESNAME like '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(startdate))
            {
                pagination.conditionJson += string.Format(" and to_char(InOrOutTime,'yyyy-mm-dd') >=to_char(to_date('{0}','yyyy-mm-dd '),'yyyy-mm-dd')", startdate);
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                pagination.conditionJson += string.Format(" and to_char(InOrOutTime,'yyyy-mm-dd') <=to_char(to_date('{0}','yyyy-mm-dd '),'yyyy-mm-dd')", enddate);
            }
            var data = inoroutrecordbll.GetPageList(pagination, "");
            //var JsonData = new
            //{
            //    data = data,
            //    total = pagination.total,
            //    page = pagination.page,
            //    records = pagination.records,
            //};
            return new { code = 0, count = pagination.records, Info = "获取数据成功", data = data };
        }
        #endregion

        #region 导出评估表
        /// <summary>
        /// 导出评估表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object DownloadAssess([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            string keyvalue = dy.data.keyvalue;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, Info = "请求失败,请登录!" };
            }
            HttpResponse resp = System.Web.HttpContext.Current.Response;

            string dir = new DataItemDetailBLL().GetItemValue("imgPath");
            string fileName = "应急演练记录评估表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string strDocPath = dir + @"\Resource\ExcelTemplate\应急演练记录评估表模板.doc";
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            DataTable dt = new DataTable();
            dt.Columns.Add("DrillName"); //演练名称
            dt.Columns.Add("DrillPlace"); //演练地点
            dt.Columns.Add("OrganizeDept"); //组织部门
            dt.Columns.Add("TopPerson"); //总指挥
            dt.Columns.Add("DrillTime"); //演练时间
            dt.Columns.Add("DrillType"); //演练类别
            dt.Columns.Add("DrillContent"); //演练内容
            dt.Columns.Add("Suitable"); //适宜性
            dt.Columns.Add("Fullable"); //充分性
            dt.Columns.Add("PersonStandBy"); //人员到位
            dt.Columns.Add("PersonStandByDuty"); //人员到位职责
            dt.Columns.Add("SiteSupplies"); //现场物资
            dt.Columns.Add("SiteSuppliesDuty"); //物资到位人员职责
            dt.Columns.Add("WholeOrganize"); //整体组织
            dt.Columns.Add("DivideWork"); //组织分工
            dt.Columns.Add("EffecteValuate"); //实战效果评价
            dt.Columns.Add("ReportSuperior"); //报告上级
            dt.Columns.Add("Rescue"); //救援、后援配合
            dt.Columns.Add("Evacuate"); //警戒撤离配合
            dt.Columns.Add("Problem");//存在问题
            dt.Columns.Add("Measure");//改进措施
            dt.Columns.Add("ValuatePersonName"); //评价人员姓名
            DataRow row = dt.NewRow();
            DrillplanrecordEntity drillplanrecordentity = drillplanrecordbll.GetEntity(keyvalue);
            string jsondata = drillplanrecordentity.ASSESSDATA;
            DrillassessEntity drillassessentity = new DrillassessEntity();
            if (!string.IsNullOrEmpty(jsondata))
            {
                JArray jarray = (JArray)JsonConvert.DeserializeObject(jsondata);

                foreach (JObject jobj in jarray)
                {
                    string key = jobj["key"].ToString();
                    string value = jobj["value"].ToString();
                    if (key == "DrillName") { drillassessentity.DrillName = value; }
                    else if (key == "DrillPlace") { drillassessentity.DrillPlace = value; }
                    else if (key == "OrganizeDept") { drillassessentity.OrganizeDept = value; }
                    else if (key == "TopPerson") { drillassessentity.TopPerson = value; }
                    else if (key == "DrillTime") { drillassessentity.DrillTime = Convert.ToDateTime(value); }
                    else if (key == "DrillType") { drillassessentity.DrillType = value; }
                    else if (key == "DrillContent") { drillassessentity.DrillContent = value; }
                    else if (key == "Suitable") { drillassessentity.Suitable = value; }
                    else if (key == "Fullable") { drillassessentity.Fullable = value; }
                    else if (key == "PersonStandBy") { drillassessentity.PersonStandBy = value; }
                    else if (key == "PersonStandByDuty") { drillassessentity.PersonStandByDuty = value; }
                    else if (key == "SiteSupplies") { drillassessentity.SiteSupplies = value; }
                    else if (key == "SiteSuppliesDuty") { drillassessentity.SiteSuppliesDuty = value; }
                    else if (key == "WholeOrganize") { drillassessentity.WholeOrganize = value; }
                    else if (key == "DivideWork") { drillassessentity.DivideWork = value; }
                    else if (key == "EffecteValuate") { drillassessentity.EffecteValuate = value; }
                    else if (key == "ReportSuperior") { drillassessentity.ReportSuperior = value; }
                    else if (key == "Rescue") { drillassessentity.Rescue = value; }
                    else if (key == "Evacuate") { drillassessentity.Evacuate = value; }
                    else if (key == "Problem") { drillassessentity.Problem = value; }
                    else if (key == "Measure") { drillassessentity.Measure = value; }
                    else if (key == "ValuatePersonName") { drillassessentity.ValuatePersonName = value; }
                    else if (key == "ValuatePerson") { drillassessentity.ValuatePerson = value; }
                }
            }
            if (drillassessentity != null)
            {
                row["DrillName"] = drillassessentity.DrillName;
                row["DrillPlace"] = drillassessentity.DrillPlace;
                row["OrganizeDept"] = drillassessentity.OrganizeDept;
                row["TopPerson"] = drillassessentity.TopPerson;
                row["DrillTime"] = drillassessentity.DrillTime;
                row["DrillType"] = drillassessentity.DrillType;
                row["DrillContent"] = drillassessentity.DrillContent;
                //适宜性
                if (drillassessentity.Suitable == "0")
                    row["Suitable"] = @"适宜性: ☑全部能够执行 □执行过程不够顺利  □明显不适宜";
                else if (drillassessentity.Suitable == "1")
                    row["Suitable"] = @"适宜性: □全部能够执行 ☑执行过程不够顺利  □明显不适宜";
                else if (drillassessentity.Suitable == "2")
                    row["Suitable"] = @"适宜性: □全部能够执行 □执行过程不够顺利  ☑明显不适宜";
                //充分性
                if (drillassessentity.Fullable == "0")
                    row["Fullable"] = @"充分性: ☑完全满足应急要求 □基本满足应急要求  □不充分必须修改";
                else if (drillassessentity.Fullable == "1")
                    row["Fullable"] = @"充分性: □完全满足应急要求 ☑基本满足应急要求  □不充分必须修改";
                else if (drillassessentity.Fullable == "2")
                    row["Fullable"] = @"充分性: □完全满足应急要求 □基本满足应急要求  ☑不充分必须修改";
                //人员到位
                if (drillassessentity.PersonStandBy == "0")
                    row["PersonStandBy"] = @"☑迅速准确 : □基本按时到位 □个别人员不到位  □重点人员不到位";
                else if (drillassessentity.PersonStandBy == "1")
                    row["PersonStandBy"] = @"□迅速准确 : ☑基本按时到位 □个别人员不到位  □重点人员不到位";
                else if (drillassessentity.PersonStandBy == "2")
                    row["PersonStandBy"] = @"□迅速准确 : □基本按时到位 ☑个别人员不到位  □重点人员不到位";
                else if (drillassessentity.PersonStandBy == "3")
                    row["PersonStandBy"] = @"□迅速准确 : □基本按时到位 □个别人员不到位  ☑重点人员不到位";

                //人员到位职责
                if (drillassessentity.PersonStandByDuty == "0")
                    row["PersonStandByDuty"] = @"☑职责明确，操作熟练 : □职责明确，操作不熟练 □职责不明确，操作不熟练";
                else if (drillassessentity.PersonStandByDuty == "1")
                    row["PersonStandByDuty"] = @"□职责明确，操作熟练 : ☑职责明确，操作不熟练 □职责不明确，操作不熟练";
                else if (drillassessentity.PersonStandByDuty == "2")
                    row["PersonStandByDuty"] = @"□职责明确，操作熟练 : □职责明确，操作不熟练 ☑职责不明确，操作不熟练";

                //现场物资
                if (drillassessentity.SiteSupplies == "0")
                    row["SiteSupplies"] = @"现场物资:   ☑充分、有效      □不充分      □严重缺乏";
                else if (drillassessentity.SiteSupplies == "1")
                    row["SiteSupplies"] = @"现场物资:   □充分、有效      ☑不充分      □严重缺乏";
                else if (drillassessentity.SiteSupplies == "2")
                    row["SiteSupplies"] = @"现场物资:   □充分、有效      □不充分      ☑严重缺乏";

                //物资到位人员职责
                if (drillassessentity.SiteSuppliesDuty == "0")
                    row["SiteSuppliesDuty"] = @"个人防护:   ☑防护到位      □防护不到位      □部分防护不到位";
                else if (drillassessentity.SiteSuppliesDuty == "1")
                    row["SiteSuppliesDuty"] = @"个人防护:   □防护到位      ☑防护不到位      □部分防护不到位";
                else if (drillassessentity.SiteSuppliesDuty == "2")
                    row["SiteSuppliesDuty"] = @"个人防护:   □防护到位      □防护不到位      ☑部分防护不到位";

                //整体组织
                if (drillassessentity.WholeOrganize == "0")
                    row["WholeOrganize"] = @"整体组织:   ☑准确、高效、满足要求      □效率低、有待改进 ";
                else if (drillassessentity.WholeOrganize == "1")
                    row["WholeOrganize"] = @"整体组织:   □准确、高效、满足要求     ☑ 效率低、有待改进 ";

                //组织分工
                if (drillassessentity.DivideWork == "0")
                    row["DivideWork"] = @"组织分工:   ☑安全、快速      □基本完全任务      □效率低未完全任务";
                else if (drillassessentity.DivideWork == "1")
                    row["DivideWork"] = @"组织分工:   □安全、快速      ☑基本完全任务      □效率低未完全任务";
                else if (drillassessentity.DivideWork == "2")
                    row["DivideWork"] = @"组织分工:   □安全、快速      □基本完全任务      ☑效率低未完全任务";
                //实战效果评价
                if (drillassessentity.EffecteValuate == "0")
                    row["EffecteValuate"] = @"☑达到预期目标      □基本达到目的，部分环节有待改进   □没有达到目标，需要重新演练";
                else if (drillassessentity.EffecteValuate == "1")
                    row["EffecteValuate"] = @"□达到预期目标      ☑基本达到目的，部分环节有待改进   □没有达到目标，需要重新演练";
                else if (drillassessentity.EffecteValuate == "2")
                    row["EffecteValuate"] = @"□达到预期目标      □基本达到目的，部分环节有待改进   ☑没有达到目标，需要重新演练";
                //报告上级
                if (drillassessentity.ReportSuperior == "0")
                    row["ReportSuperior"] = @"报告上级:   ☑报告及时      □联系不上 ";
                else if (drillassessentity.ReportSuperior == "1")
                    row["ReportSuperior"] = @"报告上级:   □报告及时      ☑联系不上 ";
                //救援后援配合
                if (drillassessentity.Rescue == "0")
                    row["Rescue"] = @"救援后勤、配合:   ☑按要求协作      □行动迟缓 ";
                else if (drillassessentity.Rescue == "1")
                    row["Rescue"] = @"救援后勤、配合:   □按要求协作      ☑行动迟缓 ";
                //警戒、撤离配合
                if (drillassessentity.Evacuate == "0")
                    row["Evacuate"] = @"警戒、撤离配合:   ☑按要求配合      □不配合";
                else if (drillassessentity.Evacuate == "1")
                    row["Evacuate"] = @"警戒、撤离配合:   □按要求配合      ☑不配合";

                row["Problem"] = drillassessentity.Problem.Replace("$", "");
                row["Measure"] = drillassessentity.Measure.Replace("$", "");
                DocumentBuilder builder = new DocumentBuilder(doc);
                builder.MoveToMergeField("Problem");
                builder.InsertHtml("<td style=\"word-warp;break-word;width:30;\">" + drillassessentity.Problem.Replace("$", "<br/>") + "</td>");
                builder.MoveToMergeField("Measure");
                builder.InsertHtml("<td style=\"word-warp;break-word;\">" + drillassessentity.Measure.Replace("$", "<br/>") + "</td>");
                row["ValuatePersonName"] = drillassessentity.ValuatePersonName;
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            //创建文件夹
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            doc.Save(dir + "\\Resource\\ResourceFile\\" + fileName);
            var JsonData = new
            {
                filepath = new DataItemDetailBLL().GetItemValue("imgUrl") + "/Resource/ResourceFile/" + fileName
            };
            return new { code = 0, Info = "获取数据成功", data = JsonData };
        }
        #endregion

        #region 提交应急演练计划
        /// <summary>
        /// 提交应急演练计划
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveDrillplan()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string keyValue = res.Contains("drillplanid") ? dy.data.drillplanid : ""; //主键
                string deptid = res.Contains("orgdeptid") ? dy.data.orgdeptid : ""; //组织部门id
                string orgdeptcode = res.Contains("orgdeptcode") ? dy.data.orgdeptcode : "";  //组织部门code
                DepartmentBLL departmentbll = new DepartmentBLL(); //部门对象
                if (!string.IsNullOrEmpty(deptid) && string.IsNullOrEmpty(orgdeptcode)) 
                {
                    var deptentity = departmentbll.GetEntity(deptid);
                    orgdeptcode = deptentity.EnCode;
                }
                DrillplanEntity entity = new DrillplanEntity()
                {
                    NAME = res.Contains("name") ? dy.data.name : "", //演练预案名称
                    OrgDept = res.Contains("orgdept") ? dy.data.orgdept : "",//组织部门
                    OrgDeptCode = orgdeptcode,//组织部门
                    OrgDeptId = res.Contains("orgdeptid") ? dy.data.orgdeptid : "", //组织部门
                    DEPARTNAME = res.Contains("departname") ? dy.data.departname : "", //演练部门
                    DEPARTID = res.Contains("departid") ? dy.data.departid : "", //演练部门
                    DRILLTYPE = res.Contains("drilltype") ? dy.data.drilltype : "", //演练预案类型
                    DRILLTYPENAME = res.Contains("drilltypename") ? dy.data.drilltypename : "", //演练预案类型
                    DRILLMODE = res.Contains("drillmode") ? dy.data.drillmode : "",  //演练方式
                    DRILLMODENAME = res.Contains("drillmodename") ? dy.data.drillmodename : "", //演练方式
                    PLANTIME = res.Contains("plantime") ? Convert.ToDateTime(dy.data.plantime.ToString()) : null, //计划时间
                    RPLANID = res.Contains("rplanid") ? dy.data.rplanid : ""  //演练预案id
                };
                drillplanbll.SaveForm(keyValue, entity);

                return new { Code = 0, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 删除应急演练计划
        /// <summary>
        /// 删除应急演练计划
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public object RemoveDirllPlan([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string keyValue = res.Contains("drillplanid") ? dy.data.drillplanid : ""; //主键
                drillplanbll.RemoveForm(keyValue);
                return new { Code = 0, Count = 0, Info = "删除成功!" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取应急演练计划列表
        /// <summary>
        /// 获取应急演练计划列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDrillPlanList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string startdate = res.Contains("startdate") ? dy.data.startdate : "";  //起始时间
                string enddate = res.Contains("enddate") ? dy.data.enddate : "";  //截止时间
                string plantime = res.Contains("plantime") ? dy.data.plantime : "";  //计划时间
                string orgdeptid = res.Contains("orgdeptid") ? dy.data.orgdeptid.ToString() : ""; //组织部门
                string departid = res.Contains("departid") ? dy.data.departid.ToString() : ""; //演练部门
                string name = res.Contains("name") ? dy.data.name.ToString() : ""; //演练名称
                long pageIndex = dy.pageindex; //索引页
                long pageSize = dy.pagesize;  // 每页记录数
                //获取用户基本信息
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "id";
                pagination.sord = "desc";
                pagination.sidx = "plantime";
                pagination.p_fields = @"id drillplanid ,createuserid,createuserdeptcode,createuserorgcode, departid, departname,drilltype,drillmode,to_char(plantime,'yyyy-mm') as plantime,
                drilltypename,drillmodename,name,rplanid,orgdeptid,orgdept,orgdeptcode,planstate";
                pagination.p_tablename = @"(
                            select a.* , (case when b.num > 0 then '已完成' else '未开展' end) planstate  from  mae_drillplan a 
                            left join (
                            select count(1) num,drillplanid   from   mae_drillplanrecord where isconnectplan ='是' and iscommit =1  group by drillplanid
                        ) b on a.id = b.drillplanid 
               ) a";
                pagination.conditionJson = "1=1";
                //系统管理员
                if (curUser.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and a.createuserorgcode='{0}'", curUser.OrganizeCode);
                }
                //起始时间
                if (!string.IsNullOrEmpty(startdate))
                {
                    pagination.conditionJson += string.Format(@" and a.plantime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss') ", startdate);
                }
                //截止时间
                if (!string.IsNullOrEmpty(enddate))
                {
                    pagination.conditionJson += string.Format(@" and a.plantime <= to_date('{0}','yyyy-mm-dd hh24:mi:ss') ", enddate);
                }
                //计划时间
                if (!string.IsNullOrEmpty(plantime))
                {
                    pagination.conditionJson += string.Format(@" and to_char(a.plantime,'yyyy-MM')= '{0}' ", plantime);
                }
                //组织部门
                if (!string.IsNullOrEmpty(orgdeptid))
                {
                    pagination.conditionJson += string.Format(@" and a.orgdeptid ='{0}' ", orgdeptid);
                }
                //演练部门
                if (!string.IsNullOrEmpty(departid))
                {
                    pagination.conditionJson += string.Format(@" and a.departid ='{0}' ", departid);
                }
                //演练名称
                if (!string.IsNullOrEmpty(name))
                {
                    pagination.conditionJson += string.Format(@" and a.name like '%{0}%' ", name);
                }
                var data = drillplanbll.GetPageList(pagination, "");

                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取应急演练计划详细
        /// <summary>
        /// 获取应急演练计划详细
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDrillPlanDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string keyValue = res.Contains("drillplanid") ? dy.data.drillplanid : "";


                List<object> recorddata = new List<object>();

                DrillplanEntity entity = drillplanbll.GetEntity(keyValue);

                Expression<Func<DrillplanrecordEntity, bool>> condition = ex => ex.DRILLPLANID == keyValue;

                List<DrillplanrecordEntity> list = drillplanrecordbll.GetListForCon(condition).ToList();
                foreach (DrillplanrecordEntity dentity in list)
                {
                    recorddata.Add(new
                    {
                        drillrecordid = dentity.ID,
                        departname = dentity.DEPARTNAME,
                        drillmodename = dentity.DRILLMODENAME,
                        drilltime = dentity.DRILLTIME.Value.ToString("yyyy-MM-dd"),
                        name = dentity.NAME,
                        drilltype = dentity.DRILLTYPENAME,
                        orgdeptname = dentity.OrgDept,
                        maincontent = dentity.MAINCONTENT
                    });
                }

                return new
                {
                    Code = 0,
                    Count = 1,
                    Info = "获取数据成功",
                    data = new
                    {
                        drillplanid = entity.ID,
                        name = entity.NAME,
                        orgdept = entity.OrgDept,
                        orgdeptcode = entity.OrgDeptCode,
                        orgdeptid = entity.OrgDeptId,
                        departname = entity.DEPARTNAME,
                        departid = entity.DEPARTID,
                        drilltype = entity.DRILLTYPE,
                        drilltypename = entity.DRILLTYPENAME,
                        drillmode = entity.DRILLMODE,
                        drillmodename = entity.DRILLMODENAME,
                        plantime = entity.PLANTIME,
                        rplanid = entity.RPLANID,
                        recorddata = recorddata
                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取数据字典

        /// <summary>
        /// 应急物资入库登记
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDataItemListJson([FromBody]JObject json)
        {
            var list = new List<DataItemModel>();
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string EnCode = dy.data.enCode ?? "";
                string Remark = res.Contains("Remark") ? dy.data.Remark : "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, Info = "请求失败,请登录!" };
                }
                list = dataItemCache.GetDataItemList(EnCode).ToList();
                if (!string.IsNullOrWhiteSpace(Remark))
                {
                    list = list.Where(x => x.ItemCode == Remark).ToList();
                }
            }
            catch (Exception)
            {

                return new { code = -0, count = 0, Info = "获取数据失败" };
            }

            return new { code = 0, Info = "获取数据成功", data = list };
        }

        #endregion

        #region 获取模板路径
        [HttpPost]
        public object GetTempUrl()
        {
            return new { code = 0, Info = "获取数据成功", data = new { tempurl = new DataItemDetailBLL().GetItemValue("imgUrl") + "/Resource/ExcelTemplate/演练方案模板.xls" } };
        }
        #endregion

        #region 手机APP获取应急记录列表
        /// <summary>
        /// 手机APP获取应急记录列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAppDrillRecordList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(dy.pageindex.ToString());
                pagination.rows = int.Parse(dy.pagesize.ToString());
                pagination.p_kid = "id";
                pagination.p_fields = "name,to_char(drilltime,'yyyy-mm-dd hh24:mi:ss') as drilltime";
                pagination.p_tablename = "mae_drillplanrecord";
                pagination.conditionJson = "1=1";
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();

                if (currUser.IsSystem)
                {
                    pagination.conditionJson = " and 1=1";
                }
                else
                {
                    pagination.conditionJson += " and createuserorgcode='" + currUser.OrganizeCode + "'";

                }

                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                });
                var data = drillplanrecordbll.GetPageList(pagination, queryJson);
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 上传附件接口
        /// <summary>
        /// 保存应急演练记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ScanUploadFile()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string keyvalue = dy.data.keyvalue;
                OperatorProvider.AppUserId = userid;
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DrillplanrecordEntity drillplanrecordentity = drillplanrecordbll.GetEntity(keyvalue);
                HttpFileCollection files = HttpContext.Current.Request.Files;
                UploadifyFile(drillplanrecordentity.YLXCFILES, "photo", files);
                UploadifyFile(drillplanrecordentity.VideoFiles, "RecordPath", files);

                return new { Code = 0, Count = 0, Info = "上传成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }

        #endregion

        #region 上传附件、删除附件
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="foldername"></param>
        /// <param name="fileList"></param>
        public void UploadifyFile(string folderId, HttpFileCollection fileList)
        {
            try
            {
                if (fileList.Count > 0)
                {
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        HttpPostedFile file = fileList[i];
                        //获取文件完整文件名(包含绝对路径)
                        //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                        string userId = OperatorProvider.Provider.Current().UserId;
                        string fileGuid = Guid.NewGuid().ToString();
                        long filesize = file.ContentLength;
                        string FileEextension = Path.GetExtension(file.FileName);
                        string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                        string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\ResourceFile";
                        string newFileName = fileGuid + FileEextension;
                        string newFilePath = dir + "\\" + newFileName;
                        //创建文件夹
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        FileInfoEntity fileInfoEntity = new FileInfoEntity();
                        if (!System.IO.File.Exists(newFilePath))
                        {
                            //保存文件
                            file.SaveAs(newFilePath);
                            //文件信息写入数据库
                            fileInfoEntity.Create();
                            fileInfoEntity.FileId = fileGuid;
                            fileInfoEntity.RecId = folderId; //关联ID
                            fileInfoEntity.FileName = file.FileName;
                            fileInfoEntity.FilePath = "~/Resource/ResourceFile/" + newFileName;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(filesize.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                            fileInfoEntity.FileExtensions = FileEextension;
                            fileInfoEntity.FileType = FileEextension.Replace(".", "");
                            fileinfobll.SaveForm("", fileInfoEntity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        public void UploadifyFile(string folderId, List<HttpPostedFile> fileList)
        {
            try
            {
                if (fileList.Count > 0)
                {
                    foreach (HttpPostedFile file in fileList)
                    {
                        //获取文件完整文件名(包含绝对路径)
                        //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                        string userId = OperatorProvider.Provider.Current().UserId;
                        string fileGuid = Guid.NewGuid().ToString();
                        long filesize = file.ContentLength;
                        string FileEextension = Path.GetExtension(file.FileName);
                        string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                        string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\ResourceFile";
                        string newFileName = fileGuid + FileEextension;
                        string newFilePath = dir + "\\" + newFileName;
                        //创建文件夹
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        FileInfoEntity fileInfoEntity = new FileInfoEntity();
                        if (!System.IO.File.Exists(newFilePath))
                        {
                            //保存文件
                            file.SaveAs(newFilePath);
                            //文件信息写入数据库
                            fileInfoEntity.Create();
                            fileInfoEntity.FileId = fileGuid;
                            fileInfoEntity.RecId = folderId; //关联ID
                            fileInfoEntity.FileName = file.FileName;
                            fileInfoEntity.FilePath = "~/Resource/ResourceFile/" + newFileName;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(filesize.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                            fileInfoEntity.FileExtensions = FileEextension;
                            fileInfoEntity.FileType = FileEextension.Replace(".", "");
                            fileinfobll.SaveForm("", fileInfoEntity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="Filedata"></param>
        public string UploadifyFile(string folderId, string filekey, HttpFileCollection fileList)
        {
            string filePaths = string.Empty;
            try
            {
                if (fileList.Count > 0)
                {
                    for (int i = 0; i < fileList.AllKeys.Length; i++)
                    {
                        if (fileList.AllKeys[i] == filekey)
                        {
                            HttpPostedFile file = fileList[i];
                            //获取文件完整文件名(包含绝对路径)
                            //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                            string userId = OperatorProvider.Provider.Current().UserId;
                            string fileGuid = Guid.NewGuid().ToString();
                            long filesize = file.ContentLength;
                            string FileEextension = Path.GetExtension(file.FileName);
                            string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                            string virtualPath = string.Format("~/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                            string virtualPath1 = string.Format("/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                            string fullFileName = new DataItemDetailBLL().GetItemValue("imgPath") + virtualPath1;
                            //创建文件夹
                            string path = Path.GetDirectoryName(fullFileName);
                            Directory.CreateDirectory(path);
                            FileInfoEntity fileInfoEntity = new FileInfoEntity();
                            if (!System.IO.File.Exists(fullFileName))
                            {
                                //保存文件
                                file.SaveAs(fullFileName);
                                //文件信息写入数据库

                                fileInfoEntity.Create();
                                fileInfoEntity.FileId = fileGuid;
                                fileInfoEntity.RecId = folderId; //关联ID
                                fileInfoEntity.FolderId = "ht/images";
                                fileInfoEntity.FileName = file.FileName;
                                fileInfoEntity.FilePath = virtualPath;
                                fileInfoEntity.FileSize = filesize.ToString();
                                fileInfoEntity.FileExtensions = FileEextension;
                                fileInfoEntity.FileType = FileEextension.Replace(".", "");
                                fileinfobll.SaveForm("", fileInfoEntity);

                                filePaths += "," + virtualPath;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(filePaths))
                        return filePaths.Substring(1);
                }
            }
            catch (Exception ex)
            {
            }
            return filePaths;
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="fileInfoIds"></param>
        public bool DeleteFile(string fileInfoIds)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(fileInfoIds))
            {
                string ids = "";

                string[] strArray = fileInfoIds.Split(',');

                foreach (string s in strArray)
                {
                    ids += "'" + s + "',";
                    var entity = fileinfobll.GetEntity(s);
                    if (entity != null)
                    {
                        var filePath = ctx.Server.MapPath(entity.FilePath);
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }

                if (!string.IsNullOrEmpty(ids))
                {
                    ids = ids.Substring(0, ids.Length - 1);
                }
                int count = fileinfobll.DeleteFileForm(ids);

                result = count > 0 ? true : false;
            }

            return result;
        }
        public void DeleteFileByRec(string recId)
        {
            if (!string.IsNullOrWhiteSpace(recId))
            {
                var list = fileinfobll.GetFileList(recId);
                foreach (var file in list)
                {
                    fileinfobll.RemoveForm(file.FileId);
                    var filePath = ctx.Server.MapPath(file.FilePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
        }
        #endregion
    }
}
