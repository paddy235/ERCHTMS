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
    /// �� �����Ž��豸����
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
        /// �鿴ͼƬ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImgShow()
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = hikdevicebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #region �Ž�ʵʱ���
        /// <summary>
        ///  �����豸�������� ����ȡ�����������е��豸
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public ActionResult GetDeviceByArea(string areaName)
        {
            List<HikdeviceEntity> data = hikdevicebll.GetDeviceByArea(areaName);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// �����豸��Id��ȡ��Ա������ǰ��������
        /// </summary>
        /// <param name="hikId">�豸��Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDoorRecord(string hikId)
        {
            var cacheKey = "PersonInOutList" + hikId;//�������ֵ
            var cacheService = CacheFactory.Cache();
            var cacheValue = cacheService.GetCache<System.Collections.IList>(cacheKey);
            if (cacheValue == null || cacheValue.Count < 1)
            {
                cacheValue = _hikInOutLogBll.GetTopFiveById(hikId);
                //д�뻺��
                Task.Run(() =>
                {
                    cacheService.WriteCache(cacheValue, cacheKey, DateTime.Now.AddSeconds(6));
                });
            }
            return ToJsonResult(cacheValue);
        }
        #endregion
        /// <summary>
        /// KmIndex�������β��ŷ����ȡ���ո߷�����ҵ 
        /// </summary>
        /// <returns>���ض���Json</returns>
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
                                                    where nature = '����'
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
            var JsonDataReturn = new { code = 0, count = tempData.Count, info = "��ȡ���ݳɹ�", data = jsonData };
            return Content(JsonDataReturn.ToJson());

        }
        /// <summary>
        /// KmIndex �������β��ŷ����ȡ������ʱ�������
        /// </summary>
        /// <returns>���ض���Json</returns>
        [HttpPost]
        public ActionResult WorkMeeting()
        {
            List<TempProjectData> tempData = new List<TempProjectData>();

            string sql = string.Format(@"select b.engineerletdept,e.engineerletdeptid,sum(b.realpernum) realpernum
                                              from bis_workmeeting b
                                              left join epg_outsouringengineer e on e.id=b.engineerid
                                         where b.iscommit=1 and b.meetingtype = '������' and to_char(b.meetingdate,'yyyy-MM-dd')=to_char(sysdate,'yyyy-MM-dd') and b.engineerid in 
                                         (select e.id from epg_outsouringengineer e where e.engineertype = '002') and  b.id not in(select startmeetingid from bis_workmeeting where  meetingtype = '�չ���' and startmeetingid is not null)
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
                                         where b.iscommit=1 and b.meetingtype = '������'
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
                                                             where b.iscommit=1 and b.meetingtype='������' and to_char(b.meetingdate,'yyyy-MM-dd')=to_char(sysdate,'yyyy-MM-dd') and b.engineerid in( select e.id from epg_outsouringengineer e where e.engineertype='002' and  b.id not in(select startmeetingid from bis_workmeeting where  meetingtype = '�չ���' and startmeetingid is not null)
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
            var jsonDataReturn = new { code = 0, count = tempData.Count, info = "��ȡ���ݳɹ�", data = jsonData };

            return Content(jsonDataReturn.ToJson());

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
            hikdevicebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HikdeviceEntity entity)
        {
            hikdevicebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ����ֵ�����
        #region ���β�����ҵ��
        /// <summary>
        /// 
        /// </summary>
        public class DutyDeptWork
        {
            public string DutyDeptId;//����id
            public string DutyDeptName;//��������
            public int WorkNum;//������ҵ����
            public int WorkPersonNum;//������ҵ����
            public IList<TodayWorkEntity> TodayWorkList;//��ҵ��ϸ  
            public string WorkDeptType;//��������
        }
        #endregion
        #region ��ҵ�б���

        public class TodayWorkEntity
        {
            public string WorkDept; //��ҵ��λ
            public string WorkType; //��ҵ����
            public string WorkPlace; //��ҵ�ص�
            public string RiskType;  //���յȼ�
            public string WorkTutelagePerson;  //�໤��
            public string id;
            public string RiskTypeValue; //���յȼ�value
            public string WorkContent; //��ҵ����
            public string AuditUserName; //������
            public string WorkDeptType; //��ҵ��λ����
        }
        #endregion
        #region �߷�����ҵ
        public class TempProjectData
        {
            /// <summary>
            /// ���β���
            /// </summary>
            public string DeptName { get; set; }
            public string DeptId { get; set; }
            /// <summary>
            /// ʩ������
            /// </summary>
            public int RealperNum { get; set; }
            /// <summary>
            /// ��������
            /// </summary>
            public int ProNum { get; set; }

            public List<ProEntity> ProList = new List<ProEntity>();
        }
        #endregion
        #region ʩ������
        public class ProEntity
        {
            public string Meetingid { get; set; }
            /// <summary>
            /// ��������
            /// </summary>
            public string ProName { get; set; }
            /// <summary>
            /// �����λ
            /// </summary>
            public string UnitName { get; set; }
            /// <summary>
            /// ���β���
            /// </summary>
            public string DeptName { get; set; }
            /// <summary>
            /// ʩ���ص�
            /// </summary>
            public string Address { get; set; }
            /// <summary>
            /// ���յȼ�
            /// </summary>
            public string RiskLevel { get; set; }
            /// <summary>
            /// ʩ������
            /// </summary>
            public int RealperNum { get; set; }
            /// <summary>
            /// ���β��Ÿ�����
            /// </summary>
            public string DeptPersonName { get; set; }

            public List<WorkEntity> workList = new List<WorkEntity>();
        }
        #endregion
        #region ��ҵ����
        public class WorkEntity
        {
            /// <summary>
            /// ��ҵ����
            /// </summary>
            public string WorkTask { get; set; }
            /// <summary>
            /// ���ڷ���
            /// </summary>
            public string DangerPoint { get; set; }
            /// <summary>
            /// Ԥ�ش�ʩ
            /// </summary>
            public string Measures { get; set; }
            /// <summary>
            /// ��ҵ�ص�
            /// </summary>
            public string WorkAddress { get; set; }
        }
        #endregion

        #endregion

    }
}
