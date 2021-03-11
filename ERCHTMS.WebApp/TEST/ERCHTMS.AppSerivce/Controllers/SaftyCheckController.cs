using BSFramework.Util;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.SaftyCheck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ERCHTMS.Busines.PublicInfoManage;   
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.EquipmentManage;
using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class SaftyCheckController : BaseApiController
    {
        private DataItemCache dataItemCache = new DataItemCache();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
        private SaftyCheckDataBLL scbll = new SaftyCheckDataBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private UserBLL userbll = new UserBLL();
        private SaftyCheckDataDetailBLL sdbll = new SaftyCheckDataDetailBLL();
        private SaftyCheckDataRecordBLL srbll = new SaftyCheckDataRecordBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private PackageBLL packbll = new PackageBLL();
        private FileInfoBLL fi = new FileInfoBLL();

        private WfControlBLL wfcontrolbll = new WfControlBLL();//自动化流程服务

        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); //违章基本业务对象
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //违章核准业务对象
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //违章整改业务对象
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //违章验收业务对象
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); //考核业务对象

        #region 其他
        /// <summary>
        /// 10.1 获取安全检查类型列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getSafeCheckType([FromBody]JObject json)
        {
            try
            {
                var dataType = dataItemCache.GetDataItemList("SaftyCheckType").ToList();
                dataType.Add(new Entity.SystemManage.ViewModel.DataItemModel() { ItemName="上级单位安全检查",ItemValue="8x" });
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = dataType.Select(x => new { safechecktype = x.ItemName, safechecktypeid = x.ItemValue })                    
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 10.2 获取安全检查列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getSafeCheckList([FromBody]JObject json)
        {

            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string safeCheckTypeId = dy.data.safechecktypeid;
                string checkedtype = dy.data.checkedtype;
                string deptCode = dy.data.deptcode;
                string userId = dy.userid;
                long pageIndex = dy.data.pageIndex;
                long pageSize = dy.data.pageSize;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var dataType = srbll.GetSaftyDataList(safeCheckTypeId, checkedtype, user, deptCode);
                SafeCheckListArray array = new SafeCheckListArray();
                array.safechecktotalnums = dataType.Count();
                array.safechecklist = dataType.ToList().OrderByDescending(x => x.CreateDate).Select(x => new { safechecktimeitem = Convert.ToDateTime(x.CheckBeginTime).ToShortDateString(), safechecktitleitem = x.CheckDataRecordName, safechecktype = getName(x.CheckDataType), safecheckiditem = x.ID, checkeddepart = string.IsNullOrEmpty(x.CheckedDepart) ? x.CheckDept : x.CheckedDepart, checkeddepartid = string.IsNullOrEmpty(x.CheckedDepartID) ? x.CheckDeptID : x.CheckedDepartID });
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = array
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 10.3 获取安全检查详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getSafeCheckDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户基本信息
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                string safeCheckIdItem = dy.data.safecheckiditem;
                SaftyCheckDataRecordEntity entity = srbll.getSaftyCheckDataRecordEntity(safeCheckIdItem);
                //这里要根据区域进行distinct
                IEnumerable<SaftyCheckDataDetailEntity> list = sdbll.GetSaftyDataDetail(safeCheckIdItem);
                if(list!=null && list.Count()>0) 
                    list = list.Distinct(new SaftyComparer());
                var dSum = list.Select(x => x.Count).Sum();
                string checkpersonid = entity.CheckDataType.ToString() == "1" ? entity.CheckManID : entity.CheckUserIds;
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = new
                    {
                        checkid = entity.ID,
                        checktype = getName(entity.CheckDataType),
                        checkperson = entity.CheckDataType.ToString() == "1" ? entity.CheckMan : entity.SolvePersonName,
                        checkpersonid = checkpersonid,
                        solveperson = entity.SolvePerson,
                        cansubmit = checkpersonid.Contains(user.Account) && (string.IsNullOrWhiteSpace(entity.SolvePerson) || entity.SolvePerson.Contains(user.Account)),
                        checktime = string.IsNullOrEmpty(entity.CheckBeginTime.ToString()) ? "" : Convert.ToDateTime(entity.CheckBeginTime).ToString("yyyy-MM-dd"),
                        starttime = string.IsNullOrEmpty(entity.CheckBeginTime.ToString()) ? "" : Convert.ToDateTime(entity.CheckBeginTime).ToString("yyyy-MM-dd"),
                        endtime = string.IsNullOrEmpty(entity.CheckEndTime.ToString()) ? "" : Convert.ToDateTime(entity.CheckEndTime).ToString("yyyy-MM-dd"),
                        checklevel = entity.CheckLevel == "0" ? "省公司安全检查" : dataItemCache.GetDataItemList("SaftyCheckLevel").Where(a => a.ItemValue == entity.CheckLevel).FirstOrDefault().ItemName,
                        checkleader = entity.CheckManageMan,
                        checkname = entity.CheckDataRecordName,
                        checkeddepart = entity.CheckedDepart,
                        checkeddepartid = entity.CheckedDepartID,
                        createusername = entity.CreateUserName,
                        createdate = entity.CreateDate.Value.ToString("yyyy-MM-dd"),
                        checklist = list.Select(x => new { areaname = x.BelongDistrict, areanameid = x.BelongDistrictID, areanamecode = x.BelongDistrictCode, dangerCount = x.Count }),
                        totalDangerCount = dSum                        
                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 10.4 获取安全检查详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getCheckRecordDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string safeCheckIdItem = dy.data.checkid;
                string riskPointId = dy.data.areaid;
                //
                DataTable dt = srbll.getCheckRecordDetail(safeCheckIdItem, riskPointId);
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = new { dangercount = dt.Rows.Count, dangerlist = dt }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 10.5	新增日常安全检查
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object addDailySafeCheck([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string checkName = dy.data.checkname;
                string checkTime = dy.data.checktime;
                string checkPerson = dy.data.checkperson;
                string checkPersonId = dy.data.checkpersonid;

                string checkTypeID = dy.data.checktypeid;

                string checkLevelId = dy.data.checklevelid;

                long isplan = res.Contains("isplan")?dy.data.isplan:0;
                string plantype = res.Contains("plantype")?dy.data.plantype:"";
                long isskip = res.Contains("isskip")?dy.data.isskip:-1;
                string display = res.Contains("display")?dy.data.display:"";
                string weeks = res.Contains("weeks")?dy.data.weeks:"";
                long seltype = res.Contains("seltype")?dy.data.seltype:-1;
                string thweeks =  res.Contains("thweeks")?dy.data.thweeks:"";
                string days = res.Contains("days")?dy.data.days:"";
                string remark = res.Contains("range") ? dy.data.range : "";


                string userId = dy.userid;

                SaftyCheckDataRecordEntity se = new SaftyCheckDataRecordEntity();
                se.ID = Guid.NewGuid().ToString();
                se.CheckDataRecordName = checkName;
                se.CheckBeginTime = se.CheckEndTime = Convert.ToDateTime(checkTime);
                se.CheckMan = checkPerson;
                se.CheckManID = checkPersonId;
                se.CheckDeptCode = dy.data.checkdeptcode;
                se.CheckDataType = Convert.ToInt32(checkTypeID);
                se.CheckLevel = checkLevelId;
                se.IsAuto = int.Parse(isplan.ToString());
                if (!string.IsNullOrEmpty(plantype))
                {
                   se.AutoType = int.Parse(plantype.ToString());
                }
                se.IsSkip = int.Parse(isskip.ToString());
                se.Display = display;
                se.Weeks = weeks;
                se.Days = days;
                se.Remark = remark;
                se.ThWeeks = thweeks;
                se.DataType = 0;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                int count = srbll.addDailySafeCheck(se, user);

                //检查表ID
                string checkExcelId = dy.data.checkexcelid;
                //在新增日常检查表的时候，会选择检查表然后保存到检查详情里面去
                sdbll.insertIntoDetails(checkExcelId, se.ID);
                return new { Code = 0, Count = -1, Info = "获取数据成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 10.6	选择检查人员接口
        /// </summary>
        [HttpPost]
        public object selectCheckPerson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                DataTable dt = srbll.selectCheckPerson(user);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 10.7	选择检查级别接口
        /// </summary>
        [HttpPost]
        public object selectCheckLevel([FromBody]JObject json)
        {
            try
            {
                var dataType = dataItemCache.GetDataItemList("SaftyCheckLevel");

                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = dataType.Select(x =>
new { checklevel = x.ItemName, checklevelid = x.ItemValue })
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 10.8	选择检查表接口
        /// </summary>
        [HttpPost]
        public object selectCheckExcel([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (user == null)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                var list = scbll.selectCheckExcel(user);
                return new
                {
                    Code = 0,
                    Count = list.Count(),
                    Info = "获取数据成功",
                    data = list.Select(x =>
new { checkexcelname = x.CheckDataName, checkexcelid = x.ID })
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        [HttpPost]
        public object getCheckContents([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string recid = dy.data.recid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (user == null)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                var list = scbll.GetCheckContents(recid,0);
                return new
                {
                    Code = 0,
                    Count = list.Count,
                    Info = "获取数据成功",
                    data = list
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 10.9	检查内容接口
        /// </summary>
        [HttpPost]
        public object selectCheckContent([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string risknameid = dy.data.risknameid;
                string userid = dy.userid;
                string risktype = dy.data.risktype;

                DataItemBLL di = new DataItemBLL();
                //先获取到字典项
                DataItemEntity DataItems = di.GetEntityByCode("SaftyCheckType");

                DataItemDetailBLL did = new DataItemDetailBLL();
                //根据字典项获取值
                IEnumerable<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId);

                List<DataItemDetailEntity> typelist = didList.Where(it => it.ItemName == risktype).ToList();
                if (typelist.Count > 0)
                {
                    risktype = typelist[0].ItemValue;
                }

                //获取用户基本信息
                OperatorProvider.AppUserId = userid;
                Operator user = OperatorProvider.Provider.Current();
                DataTable dt = scbll.selectCheckContent(risknameid, user.Account, risktype);
                return new
                {
                    Code = 0,
                    Count = dt.Rows.Count,
                    Info = "获取数据成功",
                    data = dt
                };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 13.13获取安全检查记录关联的隐患列表
        /// </summary>
        [HttpPost]
        public object getHtList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                string checkid = dy.data.checkid;

                return new { Code = 0, Count = 0, Info = "获取数据成功", data = scbll.GetHtList(checkid, 0) };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 13.14获取安全检查记录关联的违章列表
        /// </summary>
        [HttpPost]
        public object getWzList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                string checkid = dy.data.checkid;

                return new { Code = 0, Count = 0, Info = "获取数据成功", data = scbll.GetWzList(checkid, 1) };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 13.15 获取检查内容详情
        /// </summary>
        [HttpPost]
        public object GetCheckContentInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                string checkid = dy.data.itemid;
                DataTable dt = scbll.GetCheckContentInfo(checkid);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    List<int> list = scbll.GetHtAndWzCount(checkid, 1);
                    string content = string.IsNullOrEmpty(dr["saftycontent"].ToString()) ? dr["checkcontent"].ToString() : dr["saftycontent"].ToString();
                    string type = dr["checkobjecttype"].ToString();
                    type = string.IsNullOrEmpty(type) ? "3" : type;
                    string objectId = dr["checkobjectid"].ToString();
                    string sno = "";
                    string equname = "";
                    string areacode = "";
                    string areaname = "";
                    if (type == "0")
                    {
                        DataTable dtEqu = scbll.GetEquimentInfo(objectId);
                        if (dtEqu.Rows.Count > 0)
                        {
                            sno = dtEqu.Rows[0][2].ToString();
                            equname = dtEqu.Rows[0][1].ToString();
                            areacode = dtEqu.Rows[0][3].ToString();
                            areaname = dtEqu.Rows[0][4].ToString();

                        }
                        dtEqu.Dispose();
                    }
                    return new { Code = 0, Count = 0, Info = "获取数据成功", data = new { checkid = dr[0].ToString(), checkcontent = content, issure = dr["issure"], remark = dr["remark"], htdesc = dr["htdesc"], status = dr["status"], htcount = list[0], wzcount = list[1], equipmentsno = sno, equipmentname = equname, checkobjectid = objectId, checkobjecttype = type, areacode = areacode, areaname = areaname } };
                }
                else
                {
                    return new { Code = 0, Count = 0, Info = "没有数据" };
                }


            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        /// <summary>
        /// 3.16	保存检查结果
        /// </summary>
        [HttpPost]
        public object SaveCheckResult([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                string checkid = dy.data.itemid;
                string ischeck = dy.data.issure;
                string remark = dy.data.remark;
                SaftyCheckDataDetailBLL scdBll = new SaftyCheckDataDetailBLL();
                SaftyCheckContentBLL scbll = new SaftyCheckContentBLL();
                SaftyCheckDataDetailEntity entity = scdBll.GetEntity(checkid);
                SaftyCheckDataRecordEntity scd = new SaftyCheckDataRecordBLL().GetEntity(entity.RecID);
                if (scd!=null)
                {
                    if (scd.CheckDataType==1)
                    {
                        entity.IsSure = ischeck;
                        entity.Remark = remark;
                        scdBll.Update(entity.ID,entity);
                    }
                    
                        SaftyCheckContentEntity sc = scbll.Get(entity.ID);
                        if (sc == null)
                        {
                            sc = new SaftyCheckContentEntity();
                            sc.CheckManName = user.UserName;
                            sc.CheckObject = entity.CheckObject;
                            sc.CheckObjectId = entity.CheckObjectId;
                            sc.CheckObjectType = entity.CheckObjectType;
                            sc.CheckManAccount = user.Account;
                            sc.Recid = entity.RecID;
                            sc.DetailId = entity.ID;
                            sc.IsSure = ischeck;
                            sc.Remark = remark;
                            scbll.SaveForm(sc.ID, sc);
                        }
                        else
                        {
                            sc.IsSure = ischeck;
                            sc.Remark = remark;
                            scbll.Update(sc.ID, sc);
                        }
                }
              
                return new { Code = 0, Count = 0, Info = "获取数据成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 10.10	获取检查计划列表接口
        /// </summary>
        [HttpPost]
        public object getCheckPlanList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string ctype = dy.data.type;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var dt = scbll.getCheckPlanList(user, ctype);
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = dt
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 扫描区域二维码获取信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getAreaInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                string areaId = dy.data.areaid;
                var area = new DistrictBLL().GetEntity(areaId);
                var dis = new DistrictBLL().GetEntity(areaId);
                DepartmentBLL deptBll=new DepartmentBLL();
                var dtEqus = deptBll.GetDataTable(string.Format("select t.id equid,t.equipmentname equname,t.equipmentno equsno,t.district areaname,districtcode areacode,0 equtype from BIS_EQUIPMENT t where t.districtid='{0}' union all select t.id equid,t.equipmentname equname,t.equipmentno equsno,t.district areaname,districtcode areacode,1 equtype from BIS_SPECIALEQUIPMENT t where t.districtid='{0}'", areaId));
                var dtRisks = deptBll.GetDataTable(string.Format("select id riskid,WorkTask worktask,Process,EquipmentName equname,Parts,DangerSource,MajorName riskpoint,RiskType,grade from BIS_RISKASSESS t where t.districtid='{0}'", areaId));
                var dtHts = deptBll.GetDataTable(string.Format(@"select t.id htid, t.HIDDESCRIBE htdesc,CHECKDATE,t.rankname lev,(case when t.hidbasefilepath is not null then ('{1}'||substr(t.hidbasefilepath,2)) else '' end) as picurl,b.actionpersonname checkuser,workstream,hidcode, ( case when workstream ='隐患登记' then '已登记' when workstream ='隐患评估' then  '评估中' when workstream ='隐患完善' then '完善中' when
                                                     workstream ='隐患整改' then '整改中' when  workstream ='隐患验收' then '验收中' when  workstream ='复查验证' then '复查中' when  workstream ='整改效果评估' then '效果评估中' when
                                                      workstream ='整改结束' then '已结束' end ) status from V_BASEHIDDENINFO t 
 left join (select a.id,a.participant,a.actionperson, (select listagg(b.realname,',') within group(order by b.account) from base_user b where instr(','|| a.actionperson ||',',','||b.account||',')>0) actionpersonname from v_workflow a
  ) b on t.id = b.id where t.HIDPOINT='{0}' and t.workstream<>'整改结束' ", area.DistrictCode, dataitemdetailbll.GetItemValue("imgUrl"))); 
                return new
                {
                    Code = 0,
                    Count = -1,  
                    Info = "获取数据成功",
                    data = new { areacode=dis.DistrictCode,areaname = dis.DistrictName, deptname = dis.ChargeDept, dutyman = dis.DisreictChargePerson, tel = dis.LinkTel, equtitems = new { count = dtEqus.Rows.Count, data = dtEqus }, riskitems = new { count = dtRisks.Rows.Count, data = dtRisks }, htitems = new { count = dtHts.Rows.Count, data = dtHts } }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        
        /// <summary>
        /// 10.11	获取安全检查信息接口
        /// </summary>
        [HttpPost]
        public object getSafeCheckInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                string checkExcelId = dy.data.checkexcelid;
                var entity = srbll.getSaftyCheckDataRecordEntity(checkExcelId);
                if (entity==null)
                {
                    return new { Code = -1, Count = 0, Info ="该记录已不存在！" };
                }
                else
                {
                    List<int> ltCount = scbll.GetHtAndWzCount(checkExcelId, 0);
                    int mode = entity.CheckDataType == 1 ? 0 : 1;
                    string checkpersonid = entity.CheckDataType.ToString() == "1" ? entity.CheckManID : entity.CheckUserIds;
                    checkpersonid = string.IsNullOrEmpty(checkpersonid) ? "" : checkpersonid;
                    string SolvePerson = string.IsNullOrEmpty(entity.SolvePerson) ? "" : entity.SolvePerson;
                    string CheckManageMan = string.IsNullOrEmpty(entity.CheckManageMan) ? "" : entity.CheckManageMan;
                    string CheckedDepart = string.IsNullOrEmpty(entity.CheckedDepart) ? "" : entity.CheckedDepart;
                    string CheckedDepartID = string.IsNullOrEmpty(entity.CheckedDepartID) ? "" : entity.CheckedDepartID;
                    CheckedDepart = string.IsNullOrEmpty(CheckedDepart) ? entity.CheckDept : CheckedDepart;
                    CheckedDepartID = string.IsNullOrEmpty(CheckedDepartID) ? entity.CheckDeptID : CheckedDepartID;
                    DataTable dtFiles = new DepartmentBLL().GetDataTable(string.Format("select t.filename,replace(t.filepath,'~','{1}') url from BASE_FILEINFO t where t.recid='{0}'", checkExcelId, dataitemdetailbll.GetItemValue("imgUrl")));
                    return new
                    {
                        Code = 0,
                        Count = -1,
                        Info = "获取数据成功",
                        data = new
                        {
                            checkname = entity.CheckDataRecordName,
                            checktime = string.IsNullOrEmpty(entity.CheckBeginTime.ToString()) ? "" : Convert.ToDateTime(entity.CheckBeginTime).ToString("yyyy-MM-dd"),
                            starttime = string.IsNullOrEmpty(entity.CheckBeginTime.ToString()) ? "" : Convert.ToDateTime(entity.CheckBeginTime).ToString("yyyy-MM-dd"),
                            endtime = string.IsNullOrEmpty(entity.CheckEndTime.ToString()) ? "" : Convert.ToDateTime(entity.CheckEndTime).ToString("yyyy-MM-dd"),
                            checkid = entity.ID,
                            checkperson = entity.CheckDataType.ToString() == "1" ? entity.CheckMan : entity.SolvePersonName,
                            checkpersonid = checkpersonid,
                            solveperson = SolvePerson,
                            cansubmit = checkpersonid.Contains(user.Account) && (string.IsNullOrWhiteSpace(SolvePerson) || SolvePerson.Contains(user.Account)),
                            checktype = getNameAndID(entity.CheckDataType)[0],
                            checktypeid = getNameAndID(entity.CheckDataType)[1],
                            checklevel = entity.CheckLevel == "0" ? "省公司安全检查" : dataItemCache.GetDataItemList("SaftyCheckLevel").Where(a => a.ItemValue == entity.CheckLevel).FirstOrDefault().ItemName,
                            checkleader = CheckManageMan,
                            checkeddepart = CheckedDepart,
                            checkeddepartid = CheckedDepartID,
                            createusername = entity.CreateUserName,
                            createdate = entity.CreateDate.Value.ToString("yyyy-MM-dd"),
                            htcount = ltCount[0],
                            wzcount = ltCount[1],
                            checkcontents = scbll.GetCheckContents(entity.ID, mode),
                            files = dtFiles,
                            range = entity.Remark
                        }
                    };
                }
               
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 10.12	提交安全检查
        /// </summary>
        [HttpPost]
        public object sendSafeCheck([FromBody]JObject json)
        {
            string mes = "操作成功";
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string saftycheckdatarecordid = dy.data.checkid;
                //---
                Operator curUser = OperatorProvider.Provider.Current();
                string keyValue = string.Empty;

                string changeperson = string.Empty;

                string hiddepart = string.Empty;

                string rankid = string.Empty;

                #region 隐患提交部分
                var dtHid = htbaseinfobll.GetList(saftycheckdatarecordid, curUser.UserId, "", "隐患登记");

                foreach (DataRow row in dtHid.Rows)
                {
                    keyValue = row["id"].ToString();

                    hiddepart = row["hiddepart"].ToString();

                    rankid = row["hidrank"].ToString(); //隐患级别

                    string upsubmit = row["upsubmit"].ToString(); //隐患级别

                    string isselfchange = row["isselfchange"].ToString(); //是否本部门整改

                    string addtype = row["addtype"].ToString(); //用于判断是否省级公司提交的隐患

                    //此处需要判断当前人是否为安全管理员
                    string wfFlag = string.Empty;
                    //参与人员
                    string participant = string.Empty;
                    WfControlObj wfentity = new WfControlObj();
                    wfentity.businessid = keyValue;
                    wfentity.startflow = "隐患登记";
                    //是否上报
                    if (upsubmit == "1")
                    {
                        wfentity.submittype = "上报";
                    }
                    else
                    {
                        wfentity.submittype = "提交";
                    }
                    wfentity.rankid = rankid;
                    wfentity.user = curUser;
                    wfentity.organizeid = hiddepart; //对应电厂id
                    //省级登记的
                    if (addtype == "2")
                    {
                        wfentity.mark = "省级隐患排查";
                    }
                    else
                    {
                        wfentity.mark = "厂级隐患排查";
                    }

                    #region 国电新疆版本
                    if (addtype == "3")
                    {    //非本部门提交
                        if (isselfchange == "0")
                        {
                            wfentity.submittype = "制定提交";
                        }
                    }
                    #endregion
                    //获取下一流程的操作人
                    WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                    //处理成功
                    if (result.code == WfCode.Sucess)
                    {
                        participant = result.actionperson;
                        wfFlag = result.wfflag;
                        if (!string.IsNullOrEmpty(participant))
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                            if (count > 0)
                            {
                                htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //更新业务流程状态
                            }
                        }
                    }
                } 
                #endregion

                #region 违章提交部分
                var dtLllegal = lllegalregisterbll.GetListByCheckId(saftycheckdatarecordid, curUser.UserId, "违章登记");

                keyValue = string.Empty;

                string reformpeopleid = string.Empty; //整改人

                string errorMsg = string.Empty;

                bool isAddScore = false; //是否添加到用户积分
               
                foreach (DataRow row in dtLllegal.Rows)
                {
                    keyValue = row["id"].ToString();

                    reformpeopleid = row["reformpeopleid"].ToString();

                    string startflow = "违章登记"; //起始
                    string endflow = string.Empty;//截止

                    LllegalRegisterEntity entity = lllegalregisterbll.GetEntity(keyValue);

                    LllegalPunishEntity pbEntity = lllegalpunishbll.GetEntityByBid(keyValue);

                    //此处需要判断当前人是否为安全管理员
                    string wfFlag = string.Empty;

                    IList<UserEntity> ulist = new List<UserEntity>();
                    //参与人员
                    string participant = string.Empty;

                    //省公司、省级用户
                    if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("GrpUser")).Rows.Count > 0)
                    {
                        endflow = "违章完善";
                        wfFlag = "4";  // 登记=>完善
                        errorMsg = "省公司、省级用户";
                        //取安全主管部门用户 完善
                        participant = userbll.GetSafetyDeviceDeptUser("0", entity.BELONGDEPARTID);

                    }
                    //外包单位人员提交到发包单位
                    else if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("EpibolyUser")).Rows.Count > 0)
                    {
                        endflow = "违章核准";
                        wfFlag = "3";  // 登记=>核准

                        errorMsg = "发包单位人员";
                        //取发包单位 核准
                        participant = userbll.GetSafetyDeviceDeptUser("3", curUser);
                    }
                    else  //其他层级的用户 
                    {
                        //安全管理部门用户提交
                        if (userbll.GetSafetyAndDeviceDept(curUser).Contains("1"))
                        {
                            //装置部门
                            if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                            {
                                endflow = "违章整改";
                                wfFlag = "2";  // 登记=>整改

                                errorMsg = "整改人";
                                //如果非装置类 则提交到整改
                                UserEntity reformUser = userbll.GetEntity(reformpeopleid); //整改用户对象
                                //取整改人
                                participant = reformUser.Account;

                                isAddScore = true;
                            }
                            else  //非装置部门
                            {
                                //装置类  则提交到装置部门核准
                                var lllegatypename = dataitemdetailbll.GetEntity(entity.LLLEGALTYPE).ItemName;
                                //如果当前选择的是装置类 取装置单位 下账户
                                if (lllegatypename == dataitemdetailbll.GetItemValue("ApplianceClass"))
                                {
                                    endflow = "违章核准";
                                    wfFlag = "3";  // 登记=>核准

                                    errorMsg = "装置部门用户";
                                    //取装置用户
                                    participant = userbll.GetSafetyDeviceDeptUser("1", curUser);
                                }
                                else  //非装置类
                                {
                                    endflow = "违章整改";
                                    wfFlag = "2";  // 登记=>整改

                                    errorMsg = "整改人";
                                    //如果非装置类 则提交到整改
                                    UserEntity reformUser = userbll.GetEntity(reformpeopleid); //整改用户对象
                                    //取整改人
                                    participant = reformUser.Account;

                                    isAddScore = true;
                                }
                            }

                        }
                        //装置部门人员
                        else if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                        {
                            endflow = "违章整改";
                            wfFlag = "2";  // 登记=>整改

                            //如果非装置类 则提交到整改
                            UserEntity reformUser = userbll.GetEntity(reformpeopleid); //整改用户对象


                            errorMsg = "整改人";
                            //取整改人
                            participant = reformUser.Account;

                            isAddScore = true;
                        }
                        else  //非安全管理部门提交到安全管理部门核准
                        {

                            //负责人提交，如果没有上报则直接整改，反之直接提交到安全管理部门核准(二次核准)
                            if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("PrincipalUser")).Rows.Count > 0)
                            {
                                //上报
                                if (entity.ISUPSAFETY == "1")
                                {
                                    endflow = "违章核准";
                                    wfFlag = "3";  // 登记=>核准

                                    if (curUser.RoleName.Contains("班组级用户") || curUser.RoleName.Contains("专业级用户"))
                                    {
                                        // errorMsg = "部门级安全管理员用户";//上级部门安全员
                                        //取安全管理部门用户 
                                        participant = userbll.GetSafetyDeviceDeptUser("4", curUser);  //取上级(部门级)安全管理员的用户
                                    }
                                    else
                                    {
                                        // errorMsg = "安全部门管理员用户";//上级部门安全员
                                        //取安全管理部门用户 
                                        participant = userbll.GetSafetyDeviceDeptUser("0", curUser);  //取安全部门管理员
                                    }
                                }
                                else  //不上报
                                {
                                    endflow = "违章整改";
                                    wfFlag = "2";  // 登记=>整改

                                    UserEntity reformUser = userbll.GetEntity(reformpeopleid); //整改用户对象

                                    errorMsg = "整改人";
                                    //取整改人
                                    participant = reformUser.Account;

                                    isAddScore = true;
                                }
                            }
                            else  //提交到班组负责人处核准
                            {
                                endflow = "违章核准";
                                wfFlag = "3";  // 登记=>核准

                                errorMsg = "本部门负责人";
                                //取班组负责人
                                participant = userbll.GetSafetyDeviceDeptUser("2", curUser);
                            }
                        }
                    }

                    ///添加用户积分关联
                    if (isAddScore)
                    {
                        lllegalpunishbll.SaveUserScore(pbEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                        //关联责任人
                        var relevanceList = lllegalpunishbll.GetListByLllegalId(entity.ID, "1");
                        foreach (LllegalPunishEntity lpEntity in relevanceList)
                        {
                            //违章责任人
                            lllegalpunishbll.SaveUserScore(lpEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                        }
                    }


                    //提交流程到下一节点
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(startflow,endflow, keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //更新业务流程状态
                        }
                    }
                }
                #endregion

                //更新(在检查记录表保存已登记人员)
                srbll.RegisterPer(curUser.Account, saftycheckdatarecordid);
                //在检查内容表保存已登记的隐患区域
                sdbll.RegisterPer(curUser.Account, saftycheckdatarecordid);
                //---
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = mes,
                    data = ""
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion


        #region 安全检查统计
        /// <summary>
        /// 10.2 获取安全检查统计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getCheckStatisticsList([FromBody]JObject json)
        {

            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string deptCode = dy.data.deptcode;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                object data = scbll.GetCheckStatistics(user, deptCode);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 省级公司查看下属电厂安全检查数据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
         [HttpPost]
        public object GetFactoryCheckList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();

                DataTable dt = new DepartmentBLL().GetDataTable(string.Format(@"select encode deptcode ,fullname  deptname,count(1) num from base_department d
 left join BIS_SAFTYCHECKDATARECORD r on d.departmentid=r.CheckedDepartID where 
 datatype=0 and  nature='厂级' and deptcode like '{0}%'   group by encode,fullname", user.OrganizeCode));
                return new { Code = 0, Count =dt.Rows.Count, Info = "获取数据成功", data = dt };
            }
            catch(Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
           
        }
        /// <summary>
        /// 省级公司查看厂级安全检查列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
         [HttpPost]
         public object GetCheckListForGroup([FromBody]JObject json)
         {
             try
             {
                 string res = json.Value<string>("json");
                 dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                 string userId = dy.userid;
                 string deptCode = dy.data.deptcode;//电厂部门编码
                 long mode = dy.data.mode;//查询方式，0：未执行，1：已执行
                 //获取用户基本信息
                 OperatorProvider.AppUserId = userId;  //设置当前用户
                 Operator user = OperatorProvider.Provider.Current();
                 var dept = new DepartmentBLL().GetEntityByCode(deptCode);
                 var deptBll = new DepartmentBLL();
                 string sql = string.Format("select a.id checkid,a.checkdatatype checktype,a.checkdatarecordname checkname,(to_char(a.checkbegintime,'yyyymmdd') || '-' ||  to_char(a.checkendtime,'yyyymmdd')) time,a.checkeddepart deptname from BIS_SAFTYCHECKDATARECORD a inner join BIS_SAFTYCONTENT b on a.id=b.recid where CheckedDepartID='{0}' group by a.id,a.checkdatarecordname,a.checkbegintime,a.checkendtime,a.checkeddepart,a.checkdatatype", dept.DepartmentId);
                 if(mode==0)
                 {
                     sql += " having count(1)=0";
                 }
                 else
                 {
                     sql += " having count(1)>0";
                 }
                 DataTable dt = deptBll.GetDataTable(sql);
                 return new { Code = 0, Count = dt.Rows.Count, Info = "获取数据成功", data = dt };
             }
             catch (Exception ex)
             {
                 return new { Code = -1, Count = 0, Info = ex.Message };
             }

         }
        #endregion


        #region 安全检查首页待办列表
        /// <summary>
        /// 10.2 获取安全检查列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getSafeIndexList([FromBody]JObject json)
        {

            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string safeCheckTypeId = dy.data.safechecktypeid;
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var dataType = srbll.GetSaftDataIndexList(user);
                if (!string.IsNullOrEmpty(safeCheckTypeId))
                {
                    dataType = dataType.FindAll(x => x.CheckDataType == int.Parse(safeCheckTypeId));
                }
                SafeCheckListArray array = new SafeCheckListArray();
                array.safechecktotalnums = dataType.Count();
                array.safechecklist = dataType.ToList().OrderByDescending(x => x.CreateDate).Select(x => new { safechecktimeitem = string.IsNullOrEmpty(x.CheckBeginTime.ToString()) ? "" : Convert.ToDateTime(x.CheckBeginTime).ToString("yyyy-MM-dd"), safechecktitleitem = x.CheckDataRecordName, safechecktype = getName(x.CheckDataType), safecheckiditem = x.ID });
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = array
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion
        /// <summary>
        /// 根据周期性计划自动创建任务
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpGet]
        public object AutoCreateCheckPlan([FromBody]JObject json)
        {
            try
            {
                string res=new SaftyCheckDataBLL().AutoCreateCheckPlan();
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "操作成功",
                    data = res
                };
            }
            catch(Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #region app版本更新
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetVersion([FromBody]JObject json)
        {

            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //string userId = dy.userid;
                ////获取用户基本信息
                //OperatorProvider.AppUserId = userId;  //设置当前用户
                //Operator user = OperatorProvider.Provider.Current();
                var dataList = packbll.GetList("");
                if (dataList == null || dataList.Count() == 0)
                {
                    return new { Code = 0, Count = -1, Info = "暂无更新版本", data = "" };
                }
                else
                {
                    var dataentity = dataList.OrderByDescending(x => x.ReleaseDate).First();
                    var packagedt = fi.GetFiles(dataentity.FileName);
                    var url = "";
                    if (packagedt.Rows.Count > 0)
                    {
                        var packageEntity = fi.GetEntity(packagedt.Rows[0][0].ToString());
                        url = dataitemdetailbll.GetItemValue("imgUrl") + packageEntity.FilePath.Substring(1);
                    }
                    return new
                    {
                        Code = 0,
                        Count = -1,
                        Info = "获取数据成功",
                        data = new { AppName = dataentity.AppName, PublishVersion = dataentity.PublishVersion, ReleaseVersion = dataentity.ReleaseVersion, ReleaseDate = string.IsNullOrEmpty(dataentity.ReleaseDate.ToString()) ? "" : Convert.ToDateTime(dataentity.ReleaseDate).ToString("yyyy-MM-dd"), Url = url }
                    };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 检查类型转换
        public string[] getNameAndID(int? type)
        {
            string[] str = new string[2];
            var entityItem = dataItemCache.GetDataItemList("SaftyCheckType").Where(a => a.ItemValue == type.ToString()).FirstOrDefault();
            if (entityItem != null)
            {
                str[0] = entityItem.ItemName;
                str[1] = entityItem.ItemDetailId;
            }
            return str;
        }

        public string getName(int? type)
        {
            var cName = dataItemCache.GetDataItemList("SaftyCheckType").Where(a => a.ItemValue == type.ToString()).FirstOrDefault().ItemName;
            return cName;
        }
        #endregion
        public class SafeCheckListArray
        {
            public int safechecktotalnums { get; set; }
            public object safechecklist { get; set; }
        }

        public class SaftyComparer : IEqualityComparer<SaftyCheckDataDetailEntity>
        {
            public bool Equals(SaftyCheckDataDetailEntity x, SaftyCheckDataDetailEntity y)
            {
                if (Object.ReferenceEquals(x, y)) return true;


                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;

                if (string.IsNullOrWhiteSpace(x.BelongDistrictID) || string.IsNullOrWhiteSpace(y.BelongDistrictID))
                    return false;

                return x.BelongDistrictID.Equals(y.BelongDistrictID, StringComparison.InvariantCultureIgnoreCase);

            }
            public int GetHashCode(SaftyCheckDataDetailEntity product)
            {
                if (Object.ReferenceEquals(product, null)) return 0;

                int hashProductName = string.IsNullOrEmpty(product.BelongDistrictID) ? 0 : product.BelongDistrictID.ToUpper().GetHashCode();

                return hashProductName;
            }

        }
    }

}
