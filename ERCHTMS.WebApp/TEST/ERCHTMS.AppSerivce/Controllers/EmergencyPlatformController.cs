using ERCHTMS.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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
        private DataItemCache dataItemCache = new DataItemCache();

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

                    pagination.conditionJson += " and (a.departid_bz='" + currUser.DeptId + "' or a.departid_bz=(select parentid from base_department where departmentid ='" + currUser.DeptId + "')";
                    if (!string.IsNullOrEmpty(new DataItemDetailBLL().GetItemValue(currUser.OrganizeId)))
                    {
                        foreach (var item in new DataItemDetailBLL().GetItemValue(currUser.OrganizeId).Split('#'))
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                pagination.conditionJson += " or a.departid_bz='" + item.Split('|')[0] + "' ";
                            }
                        }
                    }
                    pagination.conditionJson += ")";
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
                pagination.p_fields = string.Format("a.suppliesname,a.num,a.storageplace,a.suppliesuntilname,'{0}' || substr(b.filepath,2,length(b.filepath)-1) as filepath",new DataItemDetailBLL().GetItemValue("imgUrl"));
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
                string userid = dy.userid;
                string keyvalue = dy.data.keyvalue;
                OperatorProvider.AppUserId = userid;
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DrillplanrecordEntity entity = drillplanrecordbll.GetEntity(keyvalue);
                if (entity == null)
                {
                    entity = new DrillplanrecordEntity();
                    entity.IsConnectPlan = "否";
                    entity.DrillStepRecordId = Guid.NewGuid().ToString();
                    entity.YLXCFILES = Guid.NewGuid().ToString();
                    entity.YLZJFILES = Guid.NewGuid().ToString();
                }
                entity.NAME = dy.data.name;
                entity.DRILLMODE = dy.data.drillmode;
                entity.DRILLMODENAME = dy.data.drillmodename;
                entity.DRILLTIME = Convert.ToDateTime(dy.data.drilltime);
                entity.Compere = dy.data.compere;
                entity.CompereName = dy.data.comperename;
                entity.DRILLPLACE = dy.data.drillplace;
                entity.DrillPeople = dy.data.drillpeople;
                entity.DrillPeopleName = dy.data.drillpeoplename;
                entity.DRILLPEOPLENUMBER = int.Parse(dy.data.drillpeoplenum.ToString());
                entity.DRILLPLANID = dy.data.drillplanid;
                entity.DRILLPLANNAME = dy.data.drillplanname;
                ReserverplanEntity reserverplanentity = reserverplanbll.GetEntity(dy.data.drillplanid);
                entity.DRILLTYPE = reserverplanentity.PLANTYPE;
                entity.DRILLTYPENAME = reserverplanentity.PLANTYPENAME;
                entity.DEPARTID = curUser.DeptId;
                entity.DEPARTNAME = curUser.DeptName;
                entity.WarnTime = dy.data.warntime;
                entity.Status = dy.data.status;
                keyvalue = string.IsNullOrEmpty(keyvalue) ? Guid.NewGuid().ToString() : keyvalue;
                entity.ID = keyvalue;
                string message = "请选择格式正确的文件再导入!";
                int count = HttpContext.Current.Request.Files.Count;
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
                        try
                        {
                            stepEntity.SortId = int.Parse(dt.Rows[i][0].ToString());

                        }
                        catch (Exception)
                        {
                            return new { Code = -1, Count = 0, Info = "序号请填写数字." };
                        }
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
                
                drillplanrecordbll.SaveForm(keyvalue, entity);
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
                pagination.p_fields = "status,name,drillplanid,drillplanname,drillmode,drillmodename,to_char(drilltime,'yyyy-mm-dd hh24:mi:ss') as drilltime,compere,comperename,drillplace,drillpeople,drillpeoplename,drillpeoplenumber as drillpeoplenum,warntime,drillschemename";
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
                    pagination.conditionJson += " and createuserdeptcode='" + currUser.DeptCode + "'";

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
                string userId = dy.userid;
                string keyValue = dy.data.keyvalue;
                OperatorProvider.AppUserId = userId;  //设置当前用户
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
                string userid = dy.userid; 
                string keyvalue = dy.data.keyvalue;
                OperatorProvider.AppUserId = userid;
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DrillplanrecordEntity drillplanrecordentity = drillplanrecordbll.GetEntity(keyvalue);
                drillplanrecordentity.DrillPurpose = dy.data.drillpurpose;
                drillplanrecordentity.SceneSimulation = dy.data.scenesimulation;
                drillplanrecordentity.DrillKeyPoint = dy.data.drillkeypoint;
                drillplanrecordentity.SelfScore = int.Parse(dy.data.selfscore.ToString());
                drillplanrecordentity.Status = "2";
                drillplanrecordbll.SaveForm(keyvalue, drillplanrecordentity);
                HttpFileCollection files = HttpContext.Current.Request.Files;
                UploadifyFile(drillplanrecordentity.YLXCFILES, files);
                DrillassessEntity entity = new DrillassessEntity()
                {
                    Suitable = dy.data.suitable,
                    Fullable = dy.data.fullable,
                    PersonStandBy = dy.data.personstandby,
                    PersonStandByDuty = dy.data.personstandbyduty,
                    SiteSupplies = dy.data.sitesupplies,
                    SiteSuppliesDuty = dy.data.sitesuppliesduty,
                    WholeOrganize = dy.data.wholeorganize,
                    DivideWork = dy.data.dividework,
                    EffecteValuate = dy.data.effectevaluate,
                    ReportSuperior = dy.data.reportsuperior,
                    Rescue = dy.data.rescue,
                    Evacuate = dy.data.evacuate,
                    ValuatePerson = dy.data.valuateperson,
                    ValuatePersonName = dy.data.valuatepersonname,
                    Problem = dy.data.problem,
                    Measure = dy.data.measure,
                    DrillId = keyvalue,
                    DrillName = drillplanrecordentity.DRILLPLANNAME,
                    DrillPlace = drillplanrecordentity.DRILLPLACE,
                    OrganizeDept = drillplanrecordentity.DEPARTNAME,
                    TopPerson = drillplanrecordentity.CompereName,
                    DrillTime = drillplanrecordentity.DRILLTIME,
                    DrillType = drillplanrecordentity.DRILLMODENAME,
                    DrillContent = drillplanrecordentity.MAINCONTENT
                };
                IList<DrillplanrecordstepEntity> steplist = JsonConvert.DeserializeObject<List<DrillplanrecordstepEntity>>(JsonConvert.SerializeObject(dy.data.steplist));
                foreach (var item in steplist)
                {
                    item.DrillStepRecordId = drillplanrecordentity.DrillStepRecordId;
                    drillplanrecordstepbll.SaveForm(item.Id, item);
                }
                drillassessbll.SaveForm("", entity);

                return new { Code = 0, Count = 0, Info = "保存成功" };
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
                DrillassessEntity drillassessentity = drillassessbll.GetList("").Where(t => t.DrillId == drillplanrecordentity.ID).FirstOrDefault();
                IList<FileInfoEntity> filelist = fileinfobll.GetFileList(drillplanrecordentity.YLXCFILES);
                IList<FileInfoEntity> filelist1 = fileinfobll.GetFileList(drillplanrecordentity.ID);
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
                        filepath = new DataItemDetailBLL().GetItemValue("imgUrl") + g.FilePath.Replace("~", "")
                    }).ToList(),
                    picturelist = filelist.Select(g => new
                    {
                        filename = g.FileName,
                        filepath = new DataItemDetailBLL().GetItemValue("imgUrl") + g.FilePath.Replace("~", "")
                    }).ToList(),
                    drillassessentity = drillassessentity
                };
                    Dictionary < string,
                    string > dict_props = new Dictionary<string, string>();
                //Id 转换前的列名  keyvalue 转换后的列名
                //dict_props.Add("Id", "keyvalue");

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                    //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                };
                return new { code = 0,Info = "获取数据成功", data = JObject.Parse(JsonConvert.SerializeObject(model, Formatting.None, settings)) };
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
            var data = drillassessbll.GetList("").Where(t => t.DrillId == keyvalue).FirstOrDefault();
            if (data != null)
            {
                row["DrillName"] = data.DrillName;
                row["DrillPlace"] = data.DrillPlace;
                row["OrganizeDept"] = data.OrganizeDept;
                row["TopPerson"] = data.TopPerson;
                row["DrillTime"] = data.DrillTime;
                row["DrillType"] = data.DrillType;
                row["DrillContent"] = data.DrillContent;
                //适宜性
                if (data.Suitable == "0")
                    row["Suitable"] = @"适宜性: ☑全部能够执行 □执行过程不够顺利  □明显不适宜";
                else if (data.Suitable == "1")
                    row["Suitable"] = @"适宜性: □全部能够执行 ☑执行过程不够顺利  □明显不适宜";
                else if (data.Suitable == "2")
                    row["Suitable"] = @"适宜性: □全部能够执行 □执行过程不够顺利  ☑明显不适宜";
                //充分性
                if (data.Fullable == "0")
                    row["Fullable"] = @"充分性: ☑完全满足应急要求 □基本满足应急要求  □不充分必须修改";
                else if (data.Fullable == "1")
                    row["Fullable"] = @"充分性: □完全满足应急要求 ☑基本满足应急要求  □不充分必须修改";
                else if (data.Fullable == "2")
                    row["Fullable"] = @"充分性: □完全满足应急要求 □基本满足应急要求  ☑不充分必须修改";
                //人员到位
                if (data.PersonStandBy == "0")
                    row["PersonStandBy"] = @"☑迅速准确 : □基本按时到位 □个别人员不到位  □重点人员不到位";
                else if (data.PersonStandBy == "1")
                    row["PersonStandBy"] = @"□迅速准确 : ☑基本按时到位 □个别人员不到位  □重点人员不到位";
                else if (data.PersonStandBy == "2")
                    row["PersonStandBy"] = @"□迅速准确 : □基本按时到位 ☑个别人员不到位  □重点人员不到位";
                else if (data.PersonStandBy == "3")
                    row["PersonStandBy"] = @"□迅速准确 : □基本按时到位 □个别人员不到位  ☑重点人员不到位";

                //人员到位职责
                if (data.PersonStandByDuty == "0")
                    row["PersonStandByDuty"] = @"☑职责明确，操作熟练 : □职责明确，操作不熟练 □职责不明确，操作不熟练";
                else if (data.PersonStandByDuty == "1")
                    row["PersonStandByDuty"] = @"□职责明确，操作熟练 : ☑职责明确，操作不熟练 □职责不明确，操作不熟练";
                else if (data.PersonStandByDuty == "2")
                    row["PersonStandByDuty"] = @"□职责明确，操作熟练 : □职责明确，操作不熟练 ☑职责不明确，操作不熟练";

                //现场物资
                if (data.SiteSupplies == "0")
                    row["SiteSupplies"] = @"现场物资:   ☑充分、有效      □不充分      □严重缺乏";
                else if (data.SiteSupplies == "1")
                    row["SiteSupplies"] = @"现场物资:   □充分、有效      ☑不充分      □严重缺乏";
                else if (data.SiteSupplies == "2")
                    row["SiteSupplies"] = @"现场物资:   □充分、有效      □不充分      ☑严重缺乏";

                //物资到位人员职责
                if (data.SiteSuppliesDuty == "0")
                    row["SiteSuppliesDuty"] = @"个人防护:   ☑防护到位      □防护不到位      □部分防护不到位";
                else if (data.SiteSuppliesDuty == "1")
                    row["SiteSuppliesDuty"] = @"个人防护:   □防护到位      ☑防护不到位      □部分防护不到位";
                else if (data.SiteSuppliesDuty == "2")
                    row["SiteSuppliesDuty"] = @"个人防护:   □防护到位      □防护不到位      ☑部分防护不到位";

                //整体组织
                if (data.WholeOrganize == "0")
                    row["WholeOrganize"] = @"整体组织:   ☑准确、高效、满足要求      □效率低、有待改进 ";
                else if (data.WholeOrganize == "1")
                    row["WholeOrganize"] = @"整体组织:   □准确、高效、满足要求     ☑ 效率低、有待改进 ";

                //组织分工
                if (data.DivideWork == "0")
                    row["DivideWork"] = @"组织分工:   ☑安全、快速      □基本完全任务      □效率低未完全任务";
                else if (data.DivideWork == "1")
                    row["DivideWork"] = @"组织分工:   □安全、快速      ☑基本完全任务      □效率低未完全任务";
                else if (data.DivideWork == "2")
                    row["DivideWork"] = @"组织分工:   □安全、快速      □基本完全任务      ☑效率低未完全任务";
                //实战效果评价
                if (data.EffecteValuate == "0")
                    row["EffecteValuate"] = @"☑达到预期目标      □基本达到目的，部分环节有待改进   □没有达到目标，需要重新演练";
                else if (data.EffecteValuate == "1")
                    row["EffecteValuate"] = @"□达到预期目标      ☑基本达到目的，部分环节有待改进   □没有达到目标，需要重新演练";
                else if (data.EffecteValuate == "2")
                    row["EffecteValuate"] = @"□达到预期目标      □基本达到目的，部分环节有待改进   ☑没有达到目标，需要重新演练";
                //报告上级
                if (data.ReportSuperior == "0")
                    row["ReportSuperior"] = @"报告上级:   ☑报告及时      □联系不上 ";
                else if (data.ReportSuperior == "1")
                    row["ReportSuperior"] = @"报告上级:   □报告及时      ☑联系不上 ";
                //救援后援配合
                if (data.Rescue == "0")
                    row["Rescue"] = @"救援后勤、配合:   ☑按要求协作      □行动迟缓 ";
                else if (data.Rescue == "1")
                    row["Rescue"] = @"救援后勤、配合:   □按要求协作      ☑行动迟缓 ";
                //警戒、撤离配合
                if (data.Evacuate == "0")
                    row["Evacuate"] = @"警戒、撤离配合:   ☑按要求配合      □不配合";
                else if (data.Evacuate == "1")
                    row["Evacuate"] = @"警戒、撤离配合:   □按要求配合      ☑不配合";

                row["Problem"] = data.Problem.Replace("$", "");
                row["Measure"] = data.Measure.Replace("$", "");
                DocumentBuilder builder = new DocumentBuilder(doc);
                builder.MoveToMergeField("Problem");
                builder.InsertHtml("<td style=\"word-warp;break-word;width:30;\">" + data.Problem.Replace("$", "<br/>") + "</td>");
                builder.MoveToMergeField("Measure");
                builder.InsertHtml("<td style=\"word-warp;break-word;\">" + data.Measure.Replace("$", "<br/>") + "</td>");
                row["ValuatePersonName"] = data.ValuatePersonName;
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
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, Info = "请求失败,请登录!" };
                }
                list = dataItemCache.GetDataItemList(EnCode).ToList();
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
                UploadifyFile(drillplanrecordentity.YLXCFILES, files);

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
