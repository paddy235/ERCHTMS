using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.EquipmentManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.SaftyCheck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class EquipmentController : BaseApiController
    {
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DailyUseRecordBLL drBll = new DailyUseRecordBLL();//日常记录
        private OperationFailureBLL ofBll = new OperationFailureBLL();
        private SpecialEquipmentBLL seBll = new SpecialEquipmentBLL();
        private MaintainingRecordBLL mrBll = new MaintainingRecordBLL();//维护保养记录
        private SaftyCheckDataRecordBLL srbll = new SaftyCheckDataRecordBLL();//安全检查
        public HttpContext ctx { get { return HttpContext.Current; } }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {

        }
        /// <summary>
        /// 获取操作人员证书信息
        /// </summary>
        /// <param name="json">请求参数</param>
        /// <returns>对象的JSON字符串</returns>
        [HttpPost]
        public object GetEquipmentOperUserDetails([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new { code = -1, count = 0, info = "用户编号不能为空。" };
                }
                UserBLL ubll = new UserBLL();
                var  User = ubll.GetEntity(userId);
                if (User == null)
                {
                    return new { code = -1, count = 0, info = "用户不存在。" };
                }
                else
                {
                    var url = dataitemdetailbll.GetItemValue("imgUrl");
                    var list = new CertificateBLL().GetList(userId);
                    var certfiles = list.Where(x => x.CertName == "特种设备作业人员证").FirstOrDefault();
                    var data = new
                    {
                        username = User.RealName,
                        gender = User.Gender,
                        identifyid = User.IdentifyID,
                        deptname = new DepartmentBLL().GetEntity(User.DepartmentId).FullName,
                        certname = certfiles != null ? certfiles.CertName : "",
                        certnno = certfiles != null ? certfiles.CertNum : "",
                        senddate = certfiles != null ? (certfiles.SendDate.HasValue ? certfiles.SendDate.Value.ToString("yyyy-MM-dd") : "") : "",
                        enddate = certfiles != null ? (certfiles.EndDate.HasValue ? certfiles.EndDate.Value.ToString("yyyy-MM-dd") : "") : "",
                        sendorgan = certfiles != null ? certfiles.SendOrgan : "",
                        filepath = certfiles != null ? (certfiles.FilePath.IsNullOrWhiteSpace()?"":url + certfiles.FilePath.Replace("~", "")) : ""
                    };
                    return new { code = 0, info = "获取数据成功", count = 1, data = data };
                }
            }
            catch (Exception ex)
            {

                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        /// <summary>
        /// 获取普通设备详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEquipmentNormalEntity([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.EquipmentId;//特种设备ID
                EquipmentEntity entity = new EquipmentBLL().GetEntity(id);
                dynamic obj = new ExpandoObject();
                obj.Id = entity.Id;
                obj.EquipmentName = entity.EquipmentName;//设备名称
                if (entity.Affiliation == "1")
                {
                    obj.Affiliation = "本单位自有"; //所属关系
                }
                else
                {
                    obj.Affiliation = "外委单位所有"; //所属关系
                }
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'EQUIPMENTXTYPE'").Where(a => a.ItemValue == entity.EquipmentType).ToList().ToList();
                string EquipmentType = itemlist.Count>0?itemlist[0].ItemName:"";
                obj.EpibolyDept = entity.EPIBOLYDEPT;//外包单位
                obj.EpibolyProject = entity.EPIBOLYPROJECT;//外包工程
                obj.EquipmentType = EquipmentType;//设备类别
                obj.EquipmentNo = entity.EquipmentNo;//设备编号
                obj.Specifications = entity.Specifications;//规格型号
                obj.District = entity.District;//所在区域   
                obj.DistrictCode = entity.DistrictCode;//所在区域编号        
                obj.SecurityManagerUser = entity.SecurityManagerUser;//管理人员
                obj.Telephone = entity.Telephone;//联系电话               
                obj.ControlDept = entity.ControlDept;//管控部门                
                obj.IsCheck = entity.IsCheck;//是否经过验收
                obj.PurchaseTime = entity.PurchaseTime.HasValue?entity.PurchaseTime.Value.ToString("yyyy-MM-dd"):"";//购置时间
                obj.OutputDeptName = !string.IsNullOrEmpty(entity.OutputDeptName) ? entity.OutputDeptName : "";//制造单位名称
                obj.FactoryNo = !string.IsNullOrEmpty(entity.FactoryNo) ? entity.FactoryNo : "";//出厂编号
                obj.FactoryDate = entity.FactoryDate != null ? entity.FactoryDate.Value.ToString("yyyy-MM-dd") : "";//出厂年月
                //使用状况
                string state = entity.State;
                switch (state)
                {
                    case "1":
                        state = "未启用";
                        break;
                    case "2":
                        state = "在用";
                        break;
                    case "3":
                        state = "停用";
                        break;
                    case "4":
                        state = "报废";
                        break;
                    default:
                        break;
                }
                obj.State = state;
                obj.UseAddress = entity.UseAddress;
                obj.RelWord = entity.RelWord;
                obj.Remark = entity.Remark;
                IList<Photo> pList = new List<Photo>(); //附件
                DataTable file = fileInfoBLL.GetFiles(entity.Id);
                foreach (DataRow dr in file.Rows)
                {
                    Photo p = new Photo();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    pList.Add(p);
                }
                obj.file = pList;
                obj.RiskAssess = GetRiskAssess(entity.RelWord);
                obj.HidStd = GetHidStdGrid(entity.RelWord, curUser.OrganizeCode);
                obj.HidBase = GetHiddBaseGrid(entity.Id);

                return new { Code = 0, Count = 1, Info = "获取数据成功", data = obj };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 风险辨识评估
        /// </summary>
        /// <param name="relword"></param>
        /// <returns></returns>
        private object GetRiskAssess(string relword)
        {
            object data = null;

            if (!string.IsNullOrWhiteSpace(relword))
            {
                var list = new RiskAssessBLL().GetListFor(string.Format(" and status>0 and deletemark=0 and equipmentname=N'{0}'", relword)).OrderByDescending(x => x.CreateDate);

                data = list.Select(x => { return new { id = x.Id, riskdesc = x.RiskDesc }; });
            }

            return data;
        }
        /// <summary>
        /// 隐患排查标准
        /// </summary>
        /// <param name="relword"></param>
        /// <param name="orgcode"></param>
        /// <returns></returns>
        private object GetHidStdGrid(string relword,string orgcode)
        {
            object data = null;

            if (!string.IsNullOrWhiteSpace(relword) && !string.IsNullOrWhiteSpace(orgcode))
            {
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = @"require stdname,CONTENT hiddesc,require hidmeasure";
                pagination.p_tablename = "BIS_HTSTANDARDITEM";
                pagination.conditionJson = "1=1";
                pagination.page = 1;//页数
                pagination.rows = 100;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式  
                var queryJson = new { equipmentname= relword,orgcode = orgcode};
                var dt = new HtStandardItemBLL().GetList(pagination, queryJson.ToJson());
                var list = new List<object>();
                foreach(DataRow dr in dt.Rows)
                {
                    list.Add(new
                    {
                        id= dr["id"].ToString(),
                        name=dr["stdname"].ToString(),
                        hiddesc=dr["hiddesc"].ToString(),
                        hidmeasure = dr["hidmeasure"].ToString()
                    });
                }
                data = list;
            }

            return data;
        }
        /// <summary>
        /// 关联隐患
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public object GetHiddBaseGrid(string deviceId)
        {
            object data = null;

            if (!string.IsNullOrWhiteSpace(deviceId))
            {
                var list = new HTBaseInfoBLL().GetList(string.Format(" and deviceid='{0}'", deviceId)).OrderByDescending(x => x.CREATEDATE);
                data = list.Select(x =>
                {
                    var dt = fileInfoBLL.GetFiles(x.HIDPHOTO);
                    var itemdetail = dataitemdetailbll.GetEntity(x.HIDRANK);
                    return new
                    {
                        id = x.ID,
                        hidcode = x.HIDCODE,
                        hiddescribe = x.HIDDESCRIBE,
                        workstream = x.WORKSTREAM,
                        hidrank = itemdetail==null?"": itemdetail.ItemName,
                        fileurl = dt != null && dt.Rows.Count > 0 ? dataitemdetailbll.GetItemValue("imgUrl") + dt.Rows[0]["filepath"].ToString().Substring(1) : ""
                    };
                });
            }

            return data;
        }
        /// <summary>
        /// 获取普通设备集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEquipmentNormalList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = "equipmentname,equipmentno,district,districtcode";
                pagination.p_tablename = "bis_equipment t";
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", curUser.OrganizeCode);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式  
                //所属区域
                string AreaCode = dy.data.AreaCode;
                if (!string.IsNullOrEmpty(AreaCode))
                {
                    pagination.conditionJson += string.Format(@" and districtcode like '{0}%'", AreaCode);
                }
                //所属关系
                string Affiliation = dy.data.Affiliation;
                if (!string.IsNullOrEmpty(Affiliation))
                {
                    pagination.conditionJson += string.Format(@" and Affiliation = '{0}'", Affiliation);
                }
                //设备名称
                string EquipmentName = dy.data.EquipmentName;
                if (!string.IsNullOrEmpty(EquipmentName))
                {
                    pagination.conditionJson += string.Format(@" and EquipmentName like '%{0}%'", EquipmentName);
                }
                //设备编号
                string EquipmentNo = dy.data.EquipmentNo;
                if (!string.IsNullOrEmpty(EquipmentNo))
                {
                    pagination.conditionJson += string.Format(@" and EquipmentNo = '{0}'", EquipmentNo);
                }
                DataTable dt = new EquipmentBLL().GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        /// <summary>
        /// 获取所属关系
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>       
        [HttpPost]
        public object GetAffiliation()
        {
            var list = dataitemdetailbll.GetDataItemListByItemCode("'AFFILIATION'");
            var data = list.Select(x =>
            {
                return new
                {
                    x.ItemId,
                    x.ItemName,
                    x.ItemValue
                };
            });

            //return Json(new { code = 0, info = "获取数据成功", count = list.Count(), data = data }, new JsonSerializerSettings() {DateFormatString = "yyyy-MM-dd HH:mm:ss" });
            return new { code = 0, info = "获取数据成功", count = list.Count(), data = data };
        }
        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetArea([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
            }
            List<DistrictEntity> districtdata = new DistrictBLL().GetList().ToList();            
            districtdata = districtdata.Where(a => a.OrganizeId == curUser.OrganizeId).ToList();            
            districtdata = districtdata.OrderBy(t => t.DistrictCode).ThenBy(t => t.SortCode).ToList();
            List<TreeEntity> treeList = new List<TreeEntity>();
            foreach (DistrictEntity item in districtdata)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = districtdata.Count(t => t.ParentID == item.DistrictID) == 0 ? false : true;
                tree.id = item.DistrictID;
                tree.text = item.DistrictName;
                tree.value = item.DistrictCode;
                tree.Attribute = "Code";
                tree.AttributeValue = item.DistrictCode;
                tree.parentId = districtdata.Count(t => t.DistrictID == item.ParentID) == 0 ? "0" : item.ParentID;              
                tree.hasChildren = hasChildren;              
                treeList.Add(tree);
            }
            var data = treeList.TreeToJson("0").ToJson();

            return new { code = 0, info = "获取数据成功", count = treeList.Count, data = data };
            //return Json(new { code = 0, info = "获取数据成功", count = treeList.Count, data = data }, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" });
        }
        /// <summary>
        /// 新增维护保养记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object AddMaintainingRecord()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                string EquipmentId = dy.data.EquipmentId;//特种设备ID
                //维护保养记录ID
                string recordId = dy.data.recordId;
                MaintainingRecordEntity entity = new MaintainingRecordEntity();
                if (!string.IsNullOrEmpty(recordId))
                {
                    entity = mrBll.GetEntity(recordId);
                    //获取删除附件ID
                    string deleteFileId = dy.data.deleteFileId;
                    if (!string.IsNullOrEmpty(deleteFileId))
                    {
                        DeleteFile(deleteFileId);
                    }
                }
                else {
                    SpecialEquipmentEntity Equipment = seBll.GetEntity(EquipmentId);
                    entity.Id = Guid.NewGuid().ToString();
                    entity.RegisterDate = DateTime.Now;//登记时间
                    entity.RegisterUser = curUser.UserName;//登记人员
                    entity.RegisterUserId = curUser.UserId;//登记人员ID
                    entity.District = Equipment.District;//所在区域
                    entity.DistrictID = Equipment.DistrictId;//所在区域ID
                    entity.EquipmentId = Equipment.Id;//设备ID
                    entity.EquipmentName = Equipment.EquipmentName;//设备名称
                    entity.EquipmentNo = Equipment.EquipmentNo;//设备编号
                }
                //string str = JsonConvert.SerializeObject(dy.data);
                //MaintainingRecordEntity entity1 = JsonConvert.DeserializeObject<MaintainingRecordEntity>(str);
                entity.RecordName = dy.data.recordname;//记录名称
                entity.MaintainingContent = string.IsNullOrEmpty(dy.data.MaintainingContent) ? "" : dy.data.MaintainingContent;//保养内容
                entity.MaintainingDate = string.IsNullOrEmpty(dy.data.MaintainingDate) ? null : Convert.ToDateTime(dy.data.MaintainingDate);//保养时间
                entity.MaintainingDept = dy.data.MaintainingDept;//保养单位
                entity.MaintainingResult = string.IsNullOrEmpty(dy.data.MaintainingResult) ? "" : dy.data.MaintainingResult;//保养结果
                entity.MaintainingUser = string.IsNullOrEmpty(dy.data.MaintainingUser) ? "" : dy.data.MaintainingUser;//保养人员
                entity.ResultProving = string.IsNullOrEmpty(dy.data.ResultProving) ? "" : dy.data.ResultProving;//验证结果
                
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                //上传设备图片
                UploadifyFile(entity.Id, "equipment", files);
                mrBll.SaveForm(recordId, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }

        /// <summary>
        /// 新增日常使用状况记录表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object AddDailyUseRecord()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                string EquipmentId = dy.data.EquipmentId;//特种设备ID
                //日常使用记录ID
                string recordId = dy.data.recordId;
                DailyUseRecordEntity entity = new DailyUseRecordEntity();
                if (!string.IsNullOrEmpty(recordId))
                {
                    entity = drBll.GetEntity(recordId);
                    //获取删除附件ID
                    string deleteFileId = dy.data.deleteFileId;
                    if (!string.IsNullOrEmpty(deleteFileId))
                    {
                        DeleteFile(deleteFileId);
                    }
                }
                else {
                    SpecialEquipmentEntity Equipment = seBll.GetEntity(EquipmentId);
                    entity.Id = Guid.NewGuid().ToString();
                    entity.RegisterUser = curUser.UserName;//登记人员
                    entity.RegisterUserId = curUser.UserId;//登记人员ID
                    entity.RegisterDate = DateTime.Now;//登记时间
                    entity.District = Equipment.District;//所在区域
                    entity.DistrictID = Equipment.DistrictId;//所在区域ID
                    entity.EquipmentId = Equipment.Id;//设备ID
                    entity.EquipmentName = Equipment.EquipmentName;//设备名称
                    entity.EquipmentNo = Equipment.EquipmentNo;//设备编号
                }
                entity.IsNormal = dy.data.IsNormal;//是否运行正常
                if (entity.IsNormal == "否")
                {
                    entity.AbnormalSituation = dy.data.AbnormalSituation;//异常情况描述
                    entity.ProcessResult = dy.data.ProcessResult;//处理结果
                    entity.TreatmentMeasures = dy.data.TreatmentMeasures;//异常处理措施
                }
                else {
                    entity.AbnormalSituation = "";//异常情况描述
                    entity.ProcessResult = "";//处理结果
                    entity.TreatmentMeasures = "";//异常处理措施
                }
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                //上传设备图片
                UploadifyFile(entity.Id, "equipment", files);
                drBll.SaveForm(recordId, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }

        /// <summary>
        /// 新增运行故障记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object AddOperationFailure()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                string EquipmentId = dy.data.EquipmentId;//特种设备ID
                //记录iD
                string recordId = dy.data.recordId;
                OperationFailureEntity entity = new OperationFailureEntity();
                if (!string.IsNullOrEmpty(recordId))
                {
                    entity = ofBll.GetEntity(recordId);
                    //获取删除附件ID
                    string deleteFileId = dy.data.deleteFileId;
                    if (!string.IsNullOrEmpty(deleteFileId)) {
                        DeleteFile(deleteFileId);
                    }
                }
                else {
                    SpecialEquipmentEntity Equipment = seBll.GetEntity(EquipmentId);
                    entity.Id = Guid.NewGuid().ToString();
                    entity.RegisterUser = curUser.UserName;//登记人员
                    entity.RegisterUserId = curUser.UserId;//登记人员ID
                    entity.RegisterDate = DateTime.Now;//登记时间
                    entity.District = Equipment.District;//所在区域
                    entity.DistrictID = Equipment.DistrictId;//所在区域ID
                    entity.EquipmentId = Equipment.Id;//设备ID
                    entity.EquipmentName = Equipment.EquipmentName;//设备名称
                    entity.EquipmentNo = Equipment.EquipmentNo;//设备编号
                }
                entity.RecordName = dy.data.recordname;//记录名称
                entity.FailureNature = dy.data.FailureNature;//事故性质
                entity.FailureReason = dy.data.FailureReason;//故障原因
                entity.HandleResult = dy.data.HandleResult;//处理结果
                entity.TakeSteps = dy.data.TakeSteps;//采取措施
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                //上传设备图片
                UploadifyFile(entity.Id, "equipment", files);
                ofBll.SaveForm(recordId, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }


        /// <summary>
        /// 获取特种设备集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEquipmentList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                //获取设备类别
                string EquipmentType = dy.data.EquipmentType;
                string OrgCode = "";//电厂编码
                string lres = res.ToLower();
                if(lres.Contains("data") && lres.Contains("orgcode"))
                {
                    OrgCode = dy.data.OrgCode;
                }

                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = "equipmentname,equipmentno,district,districtcode";
                pagination.p_tablename = "bis_specialequipment t";
                if (curUser.RoleName.Contains("省级用户"))
                {
                    pagination.conditionJson = string.Format(" CREATEUSERORGCODE in (select encode from base_department where deptcode like '{0}%')", curUser.OrganizeCode);
                    if (!string.IsNullOrWhiteSpace(OrgCode))
                    {
                        pagination.conditionJson = string.Format(" CREATEUSERORGCODE in (select encode from base_department where deptcode like '{0}%')", OrgCode);
                    }
                }
                else
                {
                    pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", curUser.OrganizeCode);
                    if (!string.IsNullOrWhiteSpace(OrgCode))
                    {
                        pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", OrgCode);
                    }
                }
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式
                string sqlwhere = string.Empty;
                if (!string.IsNullOrEmpty(EquipmentType)) {
                    pagination.conditionJson = string.Format(@" equipmenttype='{0}'", EquipmentType);
                }
                DataTable dt = seBll.GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
            

        }

        /// <summary>
        /// 设备类别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetEquipmentType()
        {
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'EQUIPMENTTYPE'");
            return new { code = 0, info = "获取数据成功", count = 0, data = itemlist.Select(x => new { ItemValue = x.ItemValue, ItemName = x.ItemName }) };
        }

        /// <summary>
        /// 获取特种设备详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEquipmentEntity([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.EquipmentId;//特种设备ID
                SpecialEquipmentEntity entity = seBll.GetEntity(id);
                dynamic obj = new ExpandoObject();
                obj.Id = entity.Id;
                obj.EquipmentName = entity.EquipmentName;//设备名称
                if (entity.Affiliation == "1")
                {
                    obj.Affiliation = "本单位自有"; //所属关系
                }
                else {
                    obj.Affiliation = "外委单位所有"; //所属关系
                }
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'EQUIPMENTTYPE'");
                string EquipmentType = itemlist.Where(a => a.ItemValue == entity.EquipmentType).ToList()[0].ItemName;
                obj.EpibolyDept = entity.EPIBOLYDEPT;//外包单位
                obj.EpibolyProject = entity.EPIBOLYPROJECT;//外包工程
                obj.EquipmentType = EquipmentType;//设备类别
                obj.EquipmentNo = entity.EquipmentNo;//设备编号
                obj.Specifications = entity.Specifications;//规格型号
                obj.District = entity.District;//所在区域
                obj.DistrictCode = entity.DistrictCode;//所在区域编号
                obj.OperUser = entity.OperUser;//操作人员
                obj.SecurityManagerUser = entity.SecurityManagerUser;//管理人员
                obj.Telephone = entity.Telephone;//联系电话
                obj.CheckDate = entity.CheckDate.Value.ToString("yyyy-MM-dd");//最近检验日期
                obj.NextCheckDate = entity.NextCheckDate.Value.ToString("yyyy-MM-dd");//下次检验日期
                obj.ControlDept = entity.ControlDept;//管控部门
                obj.CertificateNo = entity.CertificateNo;//证书编号
                obj.IsCheck = entity.IsCheck;//是否经过验收
                obj.PurchaseTime = entity.PurchaseTime.HasValue?entity.PurchaseTime.Value.ToString("yyyy-MM-dd"):"";//购置时间
                obj.OutputDeptName = !string.IsNullOrEmpty(entity.OutputDeptName) ? entity.OutputDeptName : "";//制造单位名称
                obj.FactoryNo = !string.IsNullOrEmpty(entity.FactoryNo) ? entity.FactoryNo : "";//出厂编号
                obj.FactoryDate = entity.FactoryDate != null ? entity.FactoryDate.Value.ToString("yyyy-MM-dd") : "";//出厂年月
                //使用状况
                string state = entity.State;
                switch (state)
                {
                    case "1":
                        state = "未启用";
                        break;
                    case "2":
                        state = "在用";
                        break;
                    case "3":
                        state = "停用";
                        break;
                    case "4":
                        state = "报废";
                        break;
                    default:
                        break;
                }
                obj.State = state;
                obj.RelWord = entity.RelWord;
                IList<Photo> pList = new List<Photo>(); //附件
                DataTable file = fileInfoBLL.GetFiles(entity.CertificateID);
                foreach (DataRow dr in file.Rows)
                {
                    Photo p = new Photo();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    pList.Add(p);
                }
                obj.file = pList;
                IList<Photo> pList1 = new List<Photo>(); //附件
                file = fileInfoBLL.GetFiles(entity.Acceptance);
                foreach (DataRow dr in file.Rows)
                {
                    Photo p = new Photo();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    pList1.Add(p);
                }
                obj.checkfile = pList1;
                obj.RiskAssess = GetRiskAssess(entity.RelWord);
                obj.HidStd = GetHidStdGrid(entity.RelWord, curUser.OrganizeCode);
                obj.HidBase = GetHiddBaseGrid(entity.Id);

                return new { Code = 0, Count = 1, Info = "获取数据成功", data = obj };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取相关技术资料和文件附件
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEquipmentFile([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                //获取设备ID
                string EquipmentId = dy.data.EquipmentId;
                //附件类别(0技术资料文件,1定期检验记录)
                string filetype = dy.data.filetype;
                //搜索条件附件名称
                string filename = dy.data.filename;
                SpecialEquipmentEntity entity = seBll.GetEntity(EquipmentId);
                //获取页数和条数
                List<object> pList = new List<object>(); //附件
                DataTable file = new DataTable();
                if (filetype == "0")
                {
                    file = fileInfoBLL.GetFiles(entity.Id);

                }
                else {
                    file = fileInfoBLL.GetFiles(entity.CheckFileID);
                }
                DataRow[] drs = null;
                if (!string.IsNullOrEmpty(filename))
                {
                    string sqlwhere = string.Format("filename like '%{0}%'", filename);
                    drs = file.Select(sqlwhere);
                }
                else {
                    drs = file.Select("1=1");
                }
                foreach (DataRow dr in drs)
                {
                    dynamic p = new ExpandoObject();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    p.filecreatedate =Convert.ToDateTime(dr["createdate"].ToString()).ToString("yyyy-MM-dd");
                    pList.Add(p);
                }
                return new { code = 0, info = "获取数据成功", count = pList.Count, data = pList };
            }
            catch (Exception ex)
            {

                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 获取日常记录列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDailyUseRecord([FromBody]JObject json) {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取设备ID
                string EquipmentId = dy.data.EquipmentId;
                //获取时间
                string StartTime = dy.data.StartTime;
                string EndTime = dy.data.EndTime;
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = "'日常使用状况记录' as recordName,to_char(registerdate,'yyyy-MM-dd') as registerdate";
                pagination.p_tablename = "BIS_DailyUseRecord t";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "registerdate";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = string.Format(" equipmentid='{0}'", EquipmentId);
                if (!string.IsNullOrEmpty(StartTime)) {
                    if (string.IsNullOrEmpty(EndTime)) {
                        EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    pagination.conditionJson += string.Format(" and registerdate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", StartTime, EndTime);
                }
                DataTable dt = seBll.GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = dt };
            }
            catch (Exception ex)
            {

                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 获取日常记录详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDailyUseRecordEntity([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.DailyUseRecordId;//日常记录ID
                DailyUseRecordEntity entity = drBll.GetEntity(id);
                dynamic obj = new ExpandoObject();
                obj.Id = entity.Id;
                obj.EquipmentId = entity.EquipmentId;//设备ID
                obj.AbnormalSituation = !string.IsNullOrEmpty(entity.AbnormalSituation) ? entity.AbnormalSituation : ""; //异常情况描述
                obj.District = entity.District;//所在区域
                obj.EquipmentName = entity.EquipmentName;//设备名称
                obj.EquipmentNo = entity.EquipmentNo;//设备编号
                obj.IsNormal = entity.IsNormal;//是否运行正常
                obj.ProcessResult = !string.IsNullOrEmpty(entity.ProcessResult) ? entity.ProcessResult : "";//处理结果
                obj.RegisterDate = entity.RegisterDate.Value.ToString("yyyy-MM-dd");//登记时间
                obj.RegisterUser = entity.RegisterUser;//登记人员
                obj.RegisterUserId = entity.RegisterUserId;//登记人员
                obj.TreatmentMeasures = !string.IsNullOrEmpty(entity.TreatmentMeasures) ? entity.TreatmentMeasures : "";//异常处理措施
                IList<Photo> pList = new List<Photo>(); //附件
                DataTable file = fileInfoBLL.GetFiles(entity.Id);
                foreach (DataRow dr in file.Rows)
                {
                    Photo p = new Photo();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    pList.Add(p);
                }
                obj.file = pList;
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = obj };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取维护保养记录列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetMaintainingRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取设备ID
                string EquipmentId = dy.data.EquipmentId;
                //获取时间
                string StartTime = dy.data.StartTime;
                string EndTime = dy.data.EndTime;
                //搜索名称
                string recordname = dy.data.recordname;
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = "recordname,to_char(registerdate,'yyyy-MM-dd') registerdate";
                pagination.p_tablename = "bis_maintainingrecord t";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "registerdate";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = string.Format(" equipmentid='{0}'", EquipmentId);
                if (!string.IsNullOrEmpty(recordname)) {
                    pagination.conditionJson += string.Format(" and recordname like '%{0}%'", recordname);
                }
                if (!string.IsNullOrEmpty(StartTime))
                {
                    if (string.IsNullOrEmpty(EndTime))
                    {
                        EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    pagination.conditionJson += string.Format(" and to_date(to_char(registerdate,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", StartTime, EndTime);
                }
                DataTable dt = seBll.GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = dt };
            }
            catch (Exception ex)
            {

                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 获取维护保养记录详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetMaintainingRecordEntity([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.MaintainingRecordId;//维护保养记录ID
                MaintainingRecordEntity entity = mrBll.GetEntity(id);
                dynamic obj = new ExpandoObject();
                obj.Id = entity.Id;
                obj.EquipmentId = entity.EquipmentId;//设备ID
                obj.recordname = entity.RecordName;//记录名称
                obj.MaintainingContent = !string.IsNullOrEmpty(entity.MaintainingContent) ? entity.MaintainingContent : ""; //保养内容
                obj.District = entity.District;//所在区域
                obj.EquipmentName = entity.EquipmentName;//设备名称
                obj.EquipmentNo = entity.EquipmentNo;//设备编号
                obj.MaintainingDate = entity.MaintainingDate != null ? entity.MaintainingDate.Value.ToString("yyyy-MM-dd") : "";//保养时间
                obj.MaintainingDept = !string.IsNullOrEmpty(entity.MaintainingDept) ? entity.MaintainingDept : "";//保养单位
                obj.RegisterDate = entity.RegisterDate != null ? entity.RegisterDate.Value.ToString("yyyy-MM-dd") : "";//登记时间
                obj.RegisterUser = entity.RegisterUser;//登记人员
                obj.RegisterUserId = entity.RegisterUserId;//登记人员ID
                obj.MaintainingResult = !string.IsNullOrEmpty(entity.MaintainingResult) ? entity.MaintainingResult : "";//保养结果
                obj.MaintainingUser = !string.IsNullOrEmpty(entity.MaintainingUser) ? entity.MaintainingUser : "";//保养人员
                obj.ResultProving = !string.IsNullOrEmpty(entity.ResultProving) ? entity.ResultProving: "";//验证结果
                IList<Photo> pList = new List<Photo>(); //附件
                DataTable file = fileInfoBLL.GetFiles(entity.Id);
                foreach (DataRow dr in file.Rows)
                {
                    Photo p = new Photo();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    pList.Add(p);
                }
                obj.file = pList;
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = obj };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取运行故障记录集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetOperationFailure([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取设备ID
                string EquipmentId = dy.data.EquipmentId;
                //获取时间
                string StartTime = dy.data.StartTime;
                string EndTime = dy.data.EndTime;
                //搜索名称
                string recordname = dy.data.recordname;
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = "recordname,to_char(registerdate,'yyyy-MM-dd') registerdate";
                pagination.p_tablename = "bis_operationfailure t";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "registerdate";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = string.Format(" equipmentid='{0}'", EquipmentId);
                if (!string.IsNullOrEmpty(recordname))
                {
                    pagination.conditionJson += string.Format(" and recordname like '%{0}%'", recordname);
                }
                if (!string.IsNullOrEmpty(StartTime))
                {
                    if (string.IsNullOrEmpty(EndTime))
                    {
                        EndTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    pagination.conditionJson += string.Format(" and to_date(to_char(registerdate,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", StartTime, EndTime);
                }
                DataTable dt = seBll.GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = dt };
            }
            catch (Exception ex)
            {

                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 获取运行故障记录详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetOperationFailureEntity([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.OperationFailureId;//运行故障记录ID
                OperationFailureEntity entity = ofBll.GetEntity(id);
                dynamic obj = new ExpandoObject();
                obj.Id = entity.Id;
                obj.EquipmentId = entity.EquipmentId;//设备ID
                obj.recordname = entity.RecordName;//记录名称
                obj.FailureNature = entity.FailureNature; //故障性质
                obj.District = entity.District;//所在区域
                obj.EquipmentName = entity.EquipmentName;//设备名称
                obj.EquipmentNo = entity.EquipmentNo;//设备编号
                obj.FailureReason = !string.IsNullOrEmpty(entity.FailureReason) ? entity.FailureReason : "";//故障原因
                obj.HandleResult = !string.IsNullOrEmpty(entity.HandleResult) ? entity.HandleResult : "";//处理结果
                obj.RegisterDate = entity.RegisterDate.Value.ToString("yyyy-MM-dd");//登记时间
                obj.RegisterUser = entity.RegisterUser;//登记人员
                obj.RegisterUserId = entity.RegisterUserId;//登记人员ID
                obj.TakeSteps = !string.IsNullOrEmpty(entity.TakeSteps) ? entity.TakeSteps : "";//采取措施
                IList<Photo> pList = new List<Photo>(); //附件
                DataTable file = fileInfoBLL.GetFiles(entity.Id);
                foreach (DataRow dr in file.Rows)
                {
                    Photo p = new Photo();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    pList.Add(p);
                }
                obj.file = pList;
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = obj };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取事故记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAccidentEventRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取设备ID
                string EquipmentId = dy.data.EquipmentId;
                //搜索名称
                string recordname = dy.data.recordname;
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "t.id";
                pagination.p_fields = "t.sgname_deal,to_char(t.happentime_deal,'yyyy-MM-dd') as happentime_deal,t.sglevelname_deal,y.jyjg,t.dcbgfiles";
                pagination.p_tablename = "AEM_BULLETIN_DEAL t left join AEM_BULLETIN y on t.bulletinid=y.id";
                pagination.conditionJson = string.Format(@" y.equipmentid='{0}'", EquipmentId);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "happentime_deal";//排序字段
                pagination.sord = "desc";//排序方式
                if (!string.IsNullOrEmpty(recordname)) {
                    pagination.conditionJson += string.Format(@" and t.sgname_deal like '%{0}%'", recordname);
                }
                DataTable dt = seBll.GetPageList(pagination, null);
                List<object> data = new List<object>();
                foreach (DataRow dr in dt.Rows)
                {
                    dynamic item = new ExpandoObject();
                    item.AccidentId = dr["id"].ToString();
                    item.AccidentName = dr["sgname_deal"].ToString();
                    item.AccidentDate = dr["happentime_deal"].ToString();
                    item.AccidentLevel = dr["sglevelname_deal"].ToString();
                    item.AccidentBrief = !string.IsNullOrEmpty(dr["jyjg"].ToString()) ? dr["jyjg"].ToString() : "";
                    IList<Photo> pList = new List<Photo>(); //附件
                    if (!string.IsNullOrEmpty(dr["dcbgfiles"].ToString())) {
                        DataTable file = fileInfoBLL.GetFiles(dr["dcbgfiles"].ToString());
                        foreach (DataRow drs in file.Rows)
                        {
                            Photo p = new Photo();
                            p.id = drs["fileid"].ToString();
                            p.filename = drs["filename"].ToString();
                            p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + drs["filepath"].ToString().Substring(1);
                            pList.Add(p);
                        }
                    }
                    
                    item.file = pList;
                    data.Add(item);
                }
                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = data };
            }
            catch (Exception ex)
            {

                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 获取设备操作人员、安全管理人员
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEquipmentOperUser([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取设备ID
                string EquipmentId = dy.data.EquipmentId;
                //获取人员类型(0安全管理人员,1操作人员)
                string type = dy.data.type;
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "a.userid";
                pagination.p_fields = " b.filepath,b.certname,a.Gender,b.certnum,b.senddate,b.sendorgan,b.years,a.realname,a.identifyid,a.deptname,a.mobile,b.enddate";
                pagination.p_tablename = @"v_userinfo a left join (select id,filepath,t.userid,certname,Gender,certnum,senddate,sendorgan,years,realname,identifyid,deptname,enddate from BIS_CERTIFICATE t  left join v_userinfo u on t.userid=u.userid where t.certname='特种设备作业人员证') b 
on a.userid=b.userid";
                if (type == "0")
                {
                    pagination.conditionJson = string.Format(@" instr((select securityManagerUserID from BIS_specialequipment e where e.id='{0}'),a.userid)>0", EquipmentId);
                }
                else {
                    pagination.conditionJson = string.Format(@" instr((select operUserID from BIS_specialequipment e where e.id='{0}'),a.userid)>0", EquipmentId);
                }
                
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "realname";//排序字段
                pagination.sord = "desc";//排序方式
                DataTable dt = seBll.GetPageList(pagination, null);
                List<object> data = new List<object>();
                foreach (DataRow dr in dt.Rows)
                {
                    dynamic item = new ExpandoObject();
                    item.username = dr["realname"].ToString();//姓名
                    item.gender = dr["gender"].ToString();//性别
                    item.identifyid = dr["identifyid"].ToString();//身份证号
                    item.deptname = dr["deptname"].ToString();//部门
                    item.certname = dr["certname"].ToString();//证书名称
                    item.certno = dr["certnum"].ToString();//证书编号
                    if (!string.IsNullOrEmpty(dr["senddate"].ToString()))
                    {
                        item.senddate = Convert.ToDateTime(dr["senddate"].ToString()).ToString("yyyy-MM-dd");//发证日期
                    }
                    else {
                        item.senddate = "";
                    }
                    if (!string.IsNullOrEmpty(dr["enddate"].ToString()))
                    {
                        item.enddate = Convert.ToDateTime(dr["enddate"].ToString()).ToString("yyyy-MM-dd");//失效日期
                    }
                    else
                    {
                        item.enddate = "";
                    }
                    item.sendorgan = dr["sendorgan"].ToString();//发证机关
                    if (!string.IsNullOrEmpty(dr["filepath"].ToString()))
                    {   //证件照片附件
                        item.filepath = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString();
                    }
                    else {
                        item.filepath = "";//证件照片附件
                    }
                    
                    data.Add(item);
                }
                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = data };
            }
            catch (Exception ex)
            {

                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 获取特种设备管理关联安全检查
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEquipmentCheckRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取设备ID
                string EquipmentId = dy.data.EquipmentId;
                //搜索名称
                string recordname = dy.data.recordname;
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "b.ID";
                pagination.p_fields = string.Format(@"to_char(b.checkendtime,'yyyy-MM-dd') as checkendtime,b.checkdatarecordname,b.CHECKMAN,b.CHECKDATATYPE,b.CHECKLEVEL,(select count(id) from bis_htbaseinfo o where o.safetycheckobjectid=b.id and o.deviceid like '%{0}%') as Count", EquipmentId);
                pagination.p_tablename = @"bis_saftycheckdatarecord b ";
                pagination.conditionJson = string.Format(@" b.id in(
select t.id from bis_saftycheckdatarecord t left join BIS_SAFTYCHECKDATADETAILED d on t.id=d.RECID
where d.checkobjecttype like '%0%' and d.checkobjectid like '%{0}%' group by t.id) ", EquipmentId);
                if (!string.IsNullOrEmpty(recordname)) {
                    pagination.conditionJson += string.Format(" and b.checkdatarecordname like '%{0}%'", recordname);
                }
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "checkendtime";//排序字段
                pagination.sord = "desc";//排序方式

                DataTable dt = srbll.GetPageListJsonByTz(pagination);
                List<object> data = new List<object>();
                foreach (DataRow dr in dt.Rows)
                {
                    dynamic item = new ExpandoObject();
                    item.Id = dr["id"].ToString();//姓名
                    item.checkendtime = dr["checkendtime"].ToString();//检查时间
                    item.checkdatarecordname = dr["checkdatarecordname"].ToString();//检查名称
                    item.checkman = !string.IsNullOrEmpty(dr["checkman"].ToString()) ? dr["checkman"].ToString() : "";//检查人员
                    var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'SaftyCheckType'");
                    string checkdatatype = itemlist.Where(a => a.ItemValue == dr["checkdatatype"].ToString()).ToList()[0].ItemName;
                    item.checkdatatype = checkdatatype;//检查类型
                    itemlist = dataitemdetailbll.GetDataItemListByItemCode("'SaftyCheckLevel'");
                    string checklevel = itemlist.Where(a => a.ItemValue == dr["checklevel"].ToString()).ToList()[0].ItemName;
                    item.checklevel = checklevel;//检查级别
                    item.count = dr["count"].ToString();
                    data.Add(item);
                }

                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = data };
            }
            catch (Exception ex)
            {

                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="Filedata"></param>
        public void UploadifyFile(string folderId, string foldername, HttpFileCollection fileList)
        {

            try
            {
                if (fileList.Count > 0)
                {
                    for (int i = 0; i < fileList.AllKeys.Length; i++)
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
                        string fullFileName = dataitemdetailbll.GetItemValue("imgPath") + virtualPath1;
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
                            fileInfoBLL.SaveForm("", fileInfoEntity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region 删除图片
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="recId">各图片Id  xxxxx,xxxxxx,xxxxxxxx</param>
        /// <param name="folderId">关联ID</param>
        /// <returns></returns>
        public bool DeleteFile(string recId)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(recId))
            {
                string ids = "";

                string[] strArray = recId.Split(',');

                foreach (string s in strArray)
                {
                    ids += "'" + s + "',";
                }

                if (!string.IsNullOrEmpty(ids))
                {
                    ids = ids.Substring(0, ids.Length - 1);
                }

                int count = fileInfoBLL.DeleteFileForm(ids);

                result = count > 0 ? true : false;
            }

            return result;
        }
        #endregion
    }
}