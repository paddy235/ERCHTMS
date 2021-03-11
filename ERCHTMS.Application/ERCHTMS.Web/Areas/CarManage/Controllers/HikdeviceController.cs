using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Collections.Generic;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.BaseManage;
using System.Data;
using System;
using ERCHTMS.Busines.OutsourcingProject;
using System.Linq;
using System.Collections.Generic;
using BSFramework.Cache.Factory;
using System;
using System.Threading.Tasks;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// 描 述：门禁设备管理
    /// </summary>
    public class HikdeviceController : MvcControllerBase
    {
        private HikdeviceBLL hikdevicebll;
        private HikinoutlogBLL _hikInOutLogBll;
        private HighRiskCommonApplyBLL highriskcommonapplybll;
        private DepartmentBLL departmentBLL;
            private WorkMeetingBLL workMeetingbll;
        public HikdeviceController()
        {
            _hikInOutLogBll = new HikinoutlogBLL();
            hikdevicebll =  new HikdeviceBLL();
            highriskcommonapplybll = new HighRiskCommonApplyBLL();
            departmentBLL = new DepartmentBLL();
            workMeetingbll = new WorkMeetingBLL();
        }

       

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
        /// 查看图片
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImgShow()
        {
            return View();
        }


        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "DEVICENAME,OUTTYPE,AREANAME,DEVICEIP,DEVICETYPE";
            pagination.p_tablename = @"BIS_HIKDEVICE";
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

            var data = hikdevicebll.GetPageList(pagination, queryJson);

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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = hikdevicebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #region 门禁实时监控
        /// <summary>
        ///  根据设备归属区域 ，获取该区域下所有的设备
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public ActionResult GetDeviceByArea(string areaName)
        {
            List<HikdeviceEntity> data = hikdevicebll.GetDeviceByArea(areaName);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据设备的Id获取人员进出的前五条数据
        /// </summary>
        /// <param name="hikId">设备的Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDoorRecord(string hikId)
        {
            var cacheKey = "PersonInOutList" + hikId;//缓存键的值
            var cacheService = CacheFactory.Cache();
            var cacheValue = cacheService.GetCache<System.Collections.IList>(cacheKey);
            if (cacheValue == null || cacheValue.Count < 1)
            {
                cacheValue = _hikInOutLogBll.GetTopFiveById(hikId);
                //写入缓存
                Task.Run(() =>
                {
                    cacheService.WriteCache(cacheValue, cacheKey, DateTime.Now.AddSeconds(6));
                });
            }
            return ToJsonResult(cacheValue);
        }
        #endregion
        /// <summary>
        /// KmIndex根据责任部门分组获取今日高风险作业 
        /// </summary>
        /// <returns>返回对象Json</returns>
        [HttpPost]
        public ActionResult HighRiskWork()
        {

            List<DutyDeptWork> tempData = new List<DutyDeptWork>();
            string sql = string.Format(@"select departmentid,departmentname from ( select    to_char(e.engineerletdeptid) as departmentid,to_char(e.engineerletdept) as departmentname,count(*) as worknum
                                      from (select workdeptcode, engineeringid, workdepttype
                                              from v_xssunderwaywork) t
                                      left join epg_outsouringengineer e
                                        on e.id = t.engineeringid
                                      where workdepttype=1
                                      group by e.engineerletdeptid,e.engineerletdept
                                      union all
                                      select to_char(b.departmentid)  as departmentid,to_char(b.fullname) as departmentname,count(*) as worknum
                                        from v_xssunderwaywork a
                                        left join (select encode, fullname, sortcode,departmentid
                                                     from base_department
                                                    where nature = '部门'
                                                   ) b
                                          on substr(a.workdeptcode, 0, length(b.encode)) = b.encode
                                          where a.workdepttype =0 group by  b.departmentid,b.fullname) where departmentid is not null group by departmentid,departmentname");
            var data = highriskcommonapplybll.GetTable(sql);
            var totalProNum = 0;
            var totalPersonNum = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DutyDeptWork itemData = new DutyDeptWork();
                List<TodayWorkEntity> ProList = new List<TodayWorkEntity>();
                itemData.DutyDeptId = data.Rows[i]["departmentid"].ToString();
                itemData.DutyDeptName = data.Rows[i]["departmentname"].ToString();
                string sql1 = string.Format(@"select t.id,b.itemname as risktypename,t.risktype as risktypevalue,t.workdeptname,t.workplace, t.worktypename,t.WorkUserNames,t.WorkDutyUserName,t.WorkTutelageUserName
                                      from v_xssunderwaywork t
                                      left join base_dataitemdetail b
                                        on t.risktype = b.itemvalue
                                       and b.itemid =
                                           (select itemid from base_dataitem where itemcode = 'CommonRiskType')
                                     where ((workdeptcode in
                                           (select encode from base_department where encode like '{0}%')) or
                                           (engineeringid in
                                           (select id
                                                from epg_outsouringengineer a
                                               where a.engineerletdeptid = '{1}')))", departmentBLL.GetEntity(data.Rows[i]["departmentid"].ToString()).EnCode, data.Rows[i]["departmentid"].ToString());
                DataTable dt = highriskcommonapplybll.GetTable(sql1);
                itemData.WorkNum = dt.Rows.Count;
                totalProNum += itemData.WorkNum;
                foreach (DataRow item in dt.Rows)
                {
                    TodayWorkEntity pro = new TodayWorkEntity();
                    pro.WorkDept = item["workdeptname"].ToString();
                    pro.WorkType = item["worktypename"].ToString();
                    pro.WorkPlace = item["workplace"].ToString();
                    pro.RiskType = item["risktypename"].ToString();
                    pro.RiskTypeValue = item["risktypevalue"].ToString();
                    pro.WorkTutelagePerson = item["WorkTutelageUserName"].ToString();
                    pro.id = item["id"].ToString();
                    ProList.Add(pro);
                    itemData.WorkPersonNum += string.IsNullOrEmpty(item["WorkUserNames"].ToString()) ? 0 : item["WorkUserNames"].ToString().Split(',').Length;
                    itemData.WorkPersonNum += string.IsNullOrEmpty(item["WorkDutyUserName"].ToString()) ? 0 : 1;
                    itemData.WorkPersonNum += string.IsNullOrEmpty(item["WorkTutelageUserName"].ToString()) ? 0 : 1;
                }
                itemData.TodayWorkList = ProList;
                totalPersonNum += itemData.WorkPersonNum;
                tempData.Add(itemData);
            }
            var jsonData = new
            {
                tempData = tempData,
                totalProNum = totalProNum,
                totalPersonNum = totalPersonNum
            };
            var JsonDataReturn = new { code = 0, count = tempData.Count, info = "获取数据成功", data = jsonData };
            return Content(JsonDataReturn.ToJson());

        }
        /// <summary>
        /// KmIndex 根据责任部门分组获取今日临时外包工程
        /// </summary>
        /// <returns>返回对象Json</returns>
        [HttpPost]
        public ActionResult WorkMeeting()
        {
            List<TempProjectData> tempData = new List<TempProjectData>();

            string sql = string.Format(@"select b.engineerletdept,e.engineerletdeptid,sum(b.realpernum) realpernum
                                              from bis_workmeeting b
                                              left join epg_outsouringengineer e on e.id=b.engineerid
                                         where b.iscommit=1 and b.meetingtype = '开工会' and to_char(b.meetingdate,'yyyy-MM-dd')=to_char(sysdate,'yyyy-MM-dd') and b.engineerid in 
                                         (select e.id from epg_outsouringengineer e where e.engineertype = '002') and  b.id not in(select startmeetingid from bis_workmeeting where  meetingtype = '收工会' and startmeetingid is not null)
                                         group by b.engineerletdept,e.engineerletdeptid");
            var data = workMeetingbll.GetTable(sql);
            var totalProNum = 0;
            var totalPersonNum = 0;
            if (data.Rows.Count > 0)
            {
                totalPersonNum = Convert.ToInt32(data.Compute("Sum(realpernum)", ""));
            }

            for (int i = 0; i < data.Rows.Count; i++)
            {
                TempProjectData itemData = new TempProjectData();
                List<ProEntity> ProList = new List<ProEntity>();
                itemData.DeptName = data.Rows[i]["engineerletdept"].ToString();
                itemData.DeptId = data.Rows[i]["engineerletdeptid"].ToString();
                itemData.RealperNum = Convert.ToInt32(data.Rows[i]["realpernum"].ToString());
                string sqlWhere = string.Format(@"select distinct b.engineerid
                                          from bis_workmeeting b
                                         where b.iscommit=1 and b.meetingtype = '开工会'
                                           and to_char(b.meetingdate, 'yyyy-MM-dd') =
                                               to_char(sysdate, 'yyyy-MM-dd') 
                                           and b.engineerid in (select e.id
                                                                  from epg_outsouringengineer e
                                                                 where e.engineertype = '002' and e.engineerletdeptid='{0}')", data.Rows[i]["engineerletdeptid"].ToString());
                itemData.ProNum = workMeetingbll.GetTable(sqlWhere).Rows.Count;
                totalProNum += itemData.ProNum;
                string Sql1 = string.Format(@" select b.id,b.engineername,b.engineerletdept,b.address,realpernum,risklevel,e.engineerletpeople,e.outprojectid
                                                             from bis_workmeeting b
                                                             left join epg_outsouringengineer e on e.id=b.engineerid        
                                                             where b.iscommit=1 and b.meetingtype='开工会' and to_char(b.meetingdate,'yyyy-MM-dd')=to_char(sysdate,'yyyy-MM-dd') and b.engineerid in( select e.id from epg_outsouringengineer e where e.engineertype='002' and  b.id not in(select startmeetingid from bis_workmeeting where  meetingtype = '收工会' and startmeetingid is not null)
                                                            and e.engineerletdeptid='{0}')", data.Rows[i]["engineerletdeptid"].ToString());
                var dt = workMeetingbll.GetTable(Sql1);
                foreach (DataRow item in dt.Rows)
                {
                    ProEntity pro = new ProEntity();
                    List<WorkEntity> workList = new List<WorkEntity>();
                    pro.Address = item["address"].ToString();
                    pro.DeptName = data.Rows[i]["engineerletdept"].ToString();
                    pro.DeptPersonName = item["engineerletpeople"].ToString();
                    pro.ProName = item["engineername"].ToString();
                    pro.RealperNum = Convert.ToInt32(item["realpernum"].ToString());
                    pro.RiskLevel = item["risklevel"].ToString();
                    pro.UnitName = new DepartmentBLL().GetEntity(item["outprojectid"].ToString()) == null ? "" : new DepartmentBLL().GetEntity(item["outprojectid"].ToString()).FullName;
                    pro.Meetingid = item["id"].ToString();
                    var list = new WorkmeetingmeasuresBLL().GetList("").Where(x => x.Workmeetingid == item["id"].ToString()).Select(x => new
                    {
                        x.WorkTask,
                        x.DangerPoint,
                        x.Measures,
                        x.Remark1
                    }).ToList();

                    foreach (var workitem in list)
                    {
                        WorkEntity e = new WorkEntity();
                        e.DangerPoint = workitem.DangerPoint;
                        e.WorkTask = workitem.WorkTask;
                        e.Measures = workitem.Measures;
                        e.WorkAddress = workitem.Remark1;
                        workList.Add(e);
                    }
                    pro.workList = workList;
                    ProList.Add(pro);
                }
                itemData.ProList = ProList;
                tempData.Add(itemData);
            }
            var jsonData = new
            {
                tempData = tempData,
                totalProNum = totalProNum,
                totalPersonNum = totalPersonNum
            };
            var jsonDataReturn = new { code = 0, count = tempData.Count, info = "获取数据成功", data = jsonData };

            return Content(jsonDataReturn.ToJson());

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
        public ActionResult RemoveForm(string keyValue)
        {
            hikdevicebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, HikdeviceEntity entity)
        {
            hikdevicebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 返回值结果集
        #region 责任部门作业类
        /// <summary>
        /// 
        /// </summary>
        public class DutyDeptWork
        {
            public string DutyDeptId;//部门id
            public string DutyDeptName;//部门名称
            public int WorkNum;//部门作业数量
            public int WorkPersonNum;//部门作业人数
            public IList<TodayWorkEntity> TodayWorkList;//作业明细  
            public string WorkDeptType;//部门类型
        }
        #endregion
        #region 作业列表类

        public class TodayWorkEntity
        {
            public string WorkDept; //作业单位
            public string WorkType; //作业类型
            public string WorkPlace; //作业地点
            public string RiskType;  //风险等级
            public string WorkTutelagePerson;  //监护人
            public string id;
            public string RiskTypeValue; //风险等级value
            public string WorkContent; //作业内容
            public string AuditUserName; //审批人
            public string WorkDeptType; //作业单位类型
        }
        #endregion
        #region 高风险作业
        public class TempProjectData
        {
            /// <summary>
            /// 责任部门
            /// </summary>
            public string DeptName { get; set; }
            public string DeptId { get; set; }
            /// <summary>
            /// 施工人数
            /// </summary>
            public int RealperNum { get; set; }
            /// <summary>
            /// 工程数量
            /// </summary>
            public int ProNum { get; set; }

            public List<ProEntity> ProList = new List<ProEntity>();
        }
        #endregion
        #region 施工详情
        public class ProEntity
        {
            public string Meetingid { get; set; }
            /// <summary>
            /// 工程名称
            /// </summary>
            public string ProName { get; set; }
            /// <summary>
            /// 外包单位
            /// </summary>
            public string UnitName { get; set; }
            /// <summary>
            /// 责任部门
            /// </summary>
            public string DeptName { get; set; }
            /// <summary>
            /// 施工地点
            /// </summary>
            public string Address { get; set; }
            /// <summary>
            /// 风险等级
            /// </summary>
            public string RiskLevel { get; set; }
            /// <summary>
            /// 施工人数
            /// </summary>
            public int RealperNum { get; set; }
            /// <summary>
            /// 责任部门负责人
            /// </summary>
            public string DeptPersonName { get; set; }

            public List<WorkEntity> workList = new List<WorkEntity>();
        }
        #endregion
        #region 作业内容
        public class WorkEntity
        {
            /// <summary>
            /// 作业内容
            /// </summary>
            public string WorkTask { get; set; }
            /// <summary>
            /// 存在风险
            /// </summary>
            public string DangerPoint { get; set; }
            /// <summary>
            /// 预控措施
            /// </summary>
            public string Measures { get; set; }
            /// <summary>
            /// 作业地点
            /// </summary>
            public string WorkAddress { get; set; }
        }
        #endregion

        #endregion

    }
}
