using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HseToolManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HseManage.ViewModel;
using ERCHTMS.Entity.HseToolMange;
using ERCHTMS.Web.Areas.HseManage.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.HseManage.Controllers
{
    public class SelfEvaluateController : MvcControllerBase
    {
        // GET: HseManage/SelfEvaluate
        public ActionResult Index()
        {
            int year = DateTime.Now.Year;
            int minYear = year - 6;
            List<SelectListItem> yearList = new List<SelectListItem>();
            List<SelectListItem> monthList = new List<SelectListItem>();
            do
            {
                yearList.Add(new SelectListItem() { Text = year.ToString(), Value = year.ToString() });
                year--;
            }
            while (year > minYear);
            int month = 1;

            do
            {
                monthList.Add(new SelectListItem() { Text = month.ToString(), Value = month.ToString(), Selected = month == DateTime.Now.Month ? true : false });
                month++;
            } while (month <= 12);
            ViewBag.MonthList = monthList;
            ViewBag.YearList = yearList;
            return View();
        }
        /// <summary>
        ///台账 
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexRecord()
        {

            return View();
        }
        /// <summary>
        ///台账 
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailRecord(string keyValue)
        {
            var bll = new SelfEvaluateBLL();
            var model = bll.GetEntity(keyValue);
            return View(model);
        }
        /// <summary>
        /// 获取下钻条形图数据
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult GetChartsData(FormCollection form)
        {
            try
            {
                var user = OperatorProvider.Provider.Current();
                //先找到当前用户所在的电厂
                var searchDept = new DepartmentBLL().GetList().Where(p => p.DeptCode.StartsWith(user.OrganizeCode)).OrderBy(x => x.EnCode).ToList();//当前电厂底下的部门数据。分组组装数据用
                DataTable dt = new SelfEvaluateBLL().GetChartsData(form["year"], form["month"], user.OrganizeCode);
                ViewModel model = new ViewModel();
                var userList = new UserBLL().GetList().Where(x=>x.IsPresence=="1");
                model.InitData(searchDept, dt, user.OrganizeId, userList, user.IsSystem);
                return Json(new { Code = 0, Data = model });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Code = -1,
                    ex.Message,
                    Data = new ViewModel()
                });
            }
        }



        /// <summary>
        ///新增页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Form()
        {

            return View();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult DelEntity(string keyValue)
        {
            var bll = new SelfEvaluateBLL();
            bll.RemoveForm(keyValue);
            return Success("操作完成");
        }
        /// <summary>
        /// 新增修改
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveForm(SelfEvaluateEntity entity)
        {
            var userNow = OperatorProvider.Provider.Current();

            var bll = new SelfEvaluateBLL();
            try
            {
                if (string.IsNullOrEmpty(entity.Id)) //新增自我评估
                {
                    entity.Id = Guid.NewGuid().ToString();
                }
                entity.CreateUser = userNow.UserName;
                entity.CreateUserId = userNow.UserId;
                entity.DeptCode = userNow.DeptCode;
                entity.DeptId = userNow.DeptId;
                entity.DeptName = userNow.DeptName;
                if (string.IsNullOrEmpty(entity.A.Id)) //新增自我评估
                {
                    entity.A.Id = Guid.NewGuid().ToString();
                }
                if (string.IsNullOrEmpty(entity.B.Id)) //新增自我评估
                {
                    entity.B.Id = Guid.NewGuid().ToString();
                }
                if (string.IsNullOrEmpty(entity.C.Id)) //新增自我评估
                {
                    entity.C.Id = Guid.NewGuid().ToString();
                }
                if (string.IsNullOrEmpty(entity.D.Id)) //新增自我评估
                {
                    entity.D.Id = Guid.NewGuid().ToString();
                }
                if (string.IsNullOrEmpty(entity.E.Id)) //新增自我评估
                {
                    entity.E.Id = Guid.NewGuid().ToString();
                }
                entity.A.EvaId = entity.Id;
                entity.B.EvaId = entity.Id;
                entity.C.EvaId = entity.Id;
                entity.D.EvaId = entity.Id;
                entity.E.EvaId = entity.Id;
                if (entity.IsSubmit == "1") entity.IsFill = "1";
                bll.SaveForm(entity);
                return Success("操作完成");
            }
            catch (Exception ex)
            {

                return Error("操作失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult getEntity(string keyValue)
        {
            var bll = new SelfEvaluateBLL();
            var model = bll.GetEntity(keyValue);
            return Content(model.ToJson());

        }
        /// <summary>
        /// 获取台账数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDataList(Pagination pagination, string queryJson)
        {
            var userNow = OperatorProvider.Provider.Current();

            var bll = new SelfEvaluateBLL();
            var queryJsonTo = queryJson.ToJObject();
            var watch = CommonHelper.TimerStart();
            string deptcode = string.Empty;
            string keyword = string.Empty;
            string year = string.Empty;
            string month = string.Empty;
            if (queryJsonTo["mydata"].IsEmpty())
            {
                if (!queryJsonTo["DeptCode"].IsEmpty())
                {
                    deptcode = queryJsonTo["DeptCode"].ToString();
                }
                if (!queryJsonTo["keyword"].IsEmpty())
                {
                    keyword = queryJsonTo["keyword"].ToString();
                }
                if (!queryJsonTo["year"].IsEmpty())
                {
                    year = queryJsonTo["year"].ToString();
                }
                if (!queryJsonTo["month"].IsEmpty())
                {
                    month = queryJsonTo["month"].ToString();
                }
            }
            var list = bll.GetList("", deptcode, keyword, year, month);
            if (!queryJsonTo["mydata"].IsEmpty())
            {
                list = list.Where(x => x.CreateUserId == userNow.UserId).OrderByDescending(x => x.CreateDate);
            }
            else
            {
                list = list.Where(x => x.IsSubmit == "1").OrderByDescending(x => x.CreateDate);
            }
            pagination.records = list.Count();
            var datalist = (list.Skip(pagination.rows * (pagination.page - 1)).Take(pagination.rows)).ToList();
            List<object> userData = new List<object>();
            var userbll = new UserBLL();
            var deptbll = new DepartmentBLL();
            datalist.ForEach(x =>
            {
                var getuser = userbll.GetEntity(x.CreateUserId);
                var dept = string.Empty;
                var go = true;
                var getDept = deptbll.GetEntity(getuser.DepartmentId);
                var parDeptId = getDept.ParentId;
                if (getDept.Nature == "班组" || getDept.Nature == "专业")
                {
                    dept = getDept.FullName;
                    while (go)
                    {
                        var parDept = deptbll.GetEntity(parDeptId);
                        if (parDept == null)
                        {
                            go = false;
                            break;
                        }
                        if (parDept.Nature == "部门")
                        {
                            if (string.IsNullOrEmpty(dept))
                            {
                                dept = parDept.FullName;
                            }
                            else
                            {
                                dept = parDept.FullName + "/" + dept;
                            }
                            go = false;
                            break;
                        }
                        else
                        {
                            parDeptId = parDept.ParentId;
                        }
                    }
                }
                else
                {
                    dept = getDept.FullName;
                }

                var user = new
                {
                    id = x.Id,
                    name = getuser.RealName,
                    dept = dept,
                    rolename = getuser.DutyName,
                    time = x.CreateDate,
                    issubmit = x.IsSubmit == "0" ? "1" : string.Empty
                };
                userData.Add(user);
            });
            var JsonData = new
            {
                rows = userData,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());

        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTableData(FormCollection form)
        {
            try
            {
                dynamic queryJson = JsonConvert.DeserializeObject<ExpandoObject>(Request["queryJson"]);
                var user = OperatorProvider.Provider.Current();
                //先找到当前用户所在的电厂
                var searchDept = new DepartmentBLL().GetList().Where(p => p.EnCode.StartsWith(user.OrganizeCode)).OrderBy(x => x.EnCode).ToList();//当前电厂底下的部门数据。分组组装数据用
                var parentCode = user.OrganizeCode;
                DataTable dt = new SelfEvaluateBLL().GetChartsData(queryJson.year, queryJson.month, user.DeptCode);
                var userList = new UserBLL().GetList();

                var treeList = new List<TreeGridEntity>();
                if (searchDept != null && searchDept.Count > 0)
                {
                    //组装各个部门的数据
                    searchDept.ForEach(p =>
                    {
                        TreeGird treeGird = new TreeGird()
                        {
                            Id = p.DepartmentId,
                            Name = p.FullName,
                            ParentId = p.ParentId,
                            DeptId = p.DepartmentId,
                            DeptCode = p.DeptCode,
                            Nature = p.Nature
                        };
                        decimal userCount = userList.Count(x => x.DepartmentCode.StartsWith(p.EnCode) && x.IsPresence == "1");    //总人数
                        treeGird.AllUserCount = userCount.ToString(); ;
                        decimal allCount = 0;
                        var drItem = dt.Select(" DEPTCODE LIKE '" + p.EnCode + "%'");
                        decimal sbumitUserCount = 0;
                        foreach (DataRow dr in drItem)
                        {
                            allCount += dr["COUNT"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["COUNT"]);    //已提交总数 
                            sbumitUserCount += dr["USERCOUNT"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["USERCOUNT"]); //已提交总人数
                        }
                        treeGird.SubmitCount = allCount.ToString();
                        treeGird.NotSubmitCount = ((userCount - sbumitUserCount) <0 ? 0 : (userCount- sbumitUserCount)).ToString();
                        if (userCount < 1)
                        {
                            treeGird.CYD = "0%";
                        }
                        else
                        {
                            treeGird.CYD = Math.Round(allCount / userCount * 100, 2).ToString() + "%";//百分比 参与度=已提交/应提交*100%
                        }


                        TreeGridEntity treeGridEntity = new TreeGridEntity()
                        {
                            parentId = treeGird.ParentId,
                            entityJson = JsonConvert.SerializeObject(treeGird),
                            expanded = false,
                            hasChildren = true,
                            id = treeGird.Id,
                            text = treeGird.Name,
                            code = treeGird.CYD
                        };
                        treeList.Add(treeGridEntity);
                    });
                }
                var FristDpetDetail = searchDept.FirstOrDefault(x => x.EnCode == parentCode);
                if (FristDpetDetail != null && !user.IsSystem)
                {
                    var FristDpet = treeList.FirstOrDefault(x => x.id == FristDpetDetail.DepartmentId);
                    if (FristDpet != null)
                    {
                        FristDpet.parentId = "0";
                    }
                }

                return Content(treeList.TreeJson());
            }
            catch (Exception ex)
            {
                return Error("查询失败：" + ex.Message);
            }
        }
        /// <summary>
        /// 详情页面
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns></returns>
        public ActionResult Detail(string deptid, string year, string month)
        {
            var searchDept = new DepartmentBLL().GetList().FirstOrDefault(p => p.DepartmentId == deptid);
            if (searchDept == null) searchDept = new DepartmentEntity();
            List<string> submituserids = new SelfEvaluateBLL().GetSubmitByDeptCode(searchDept.EnCode, year, month);//该部门下已提交人的人的Id

            var allUser = new UserBLL().GetList().Where(p => p.IsPresence == "1" && p.DepartmentCode.Contains(searchDept.DeptCode)).ToList();
            List<string> submitUserNames = allUser.Where(p => submituserids.Contains(p.UserId) && p.IsPresence == "1").Select(p => p.RealName).ToList();//已提交的人
            List<string> notSubmitUserNames = allUser.Where(p => p.DepartmentCode != null && p.DepartmentCode.StartsWith(searchDept.EnCode) && !submituserids.Contains(p.UserId)).Select(x => x.RealName).ToList();//未提交的人
            EvaluateGroupSummaryEntity groupSummary = new SelfEvaluateBLL().GetSummary(year, month, searchDept.DepartmentId);//该部门的小结
            ViewBag.SubmitUser = submitUserNames;//已提交的人数
            ViewBag.NotSubmitUser = notSubmitUserNames;//未提交人
            ViewBag.GroupSummary = groupSummary;//本单位的填报情况小结

            //安全危害
            DataTable dangerDT = new SelfEvaluateBLL().GetDangerCount(searchDept.EnCode, year, month);
            Dictionary<string, int> dangerKV = new Dictionary<string, int>();
            if (dangerDT.Rows != null && dangerDT.Rows.Count > 0)
            {
                var dangerDrEnumerator = dangerDT.Rows.GetEnumerator();
                while (dangerDrEnumerator.MoveNext())
                {
                    DataRow dangerCurrent = dangerDrEnumerator.Current as DataRow;
                    string dangerName = dangerCurrent["DANGER"] == null ? "" : dangerCurrent["DANGER"].ToString();
                    if (!string.IsNullOrWhiteSpace(dangerName))
                    {
                        dangerKV.Add(dangerName, dangerCurrent["COUNT"] == null ? 0 : Convert.ToInt32(dangerCurrent["COUNT"]));
                    }
                }
            }

            //PPE需求
            DataTable ppeDT = new SelfEvaluateBLL().GetPPECount(searchDept.DeptCode, year, month);
            Dictionary<string, int> ppeKV = new Dictionary<string, int>();
            if (ppeDT.Rows != null && ppeDT.Rows.Count > 0)
            {
                var ppeDrEnumerator = ppeDT.Rows.GetEnumerator();
                while (ppeDrEnumerator.MoveNext())
                {
                    DataRow ppeCurrent = ppeDrEnumerator.Current as DataRow;
                    string ppeName = ppeCurrent["USEPPE"] == null ? "" : ppeCurrent["USEPPE"].ToString();
                    if (!string.IsNullOrWhiteSpace(ppeName))
                    {
                        ppeKV.Add(ppeName, ppeCurrent["COUNT"] == null ? 0 : Convert.ToInt32(ppeCurrent["COUNT"]));
                    }
                }
            }


            ViewBag.DangerKV = dangerKV;//安全危害
            ViewBag.PPEKV = ppeKV;//PPE需求

            //	HSE培训与授权
            DataTable hseDT = new SelfEvaluateBLL().GetHseCount(searchDept.DeptCode, year, month);
            List<HseEvaluateKv> hseKv = new List<HseEvaluateKv>();
            if (hseDT.Rows != null && hseDT.Rows.Count > 0)
            {
                #region hse授权   
                int DGPXrs = 0, QZDZPXrs = 0, CNJDCPXrs = 0, YLRQrs = 0, GLSPXrs = 0, GLZYPXrs = 0, DHZYPXrs = 0, JSJZYPXrs = 0, GKZYPXrs = 0, JJPXrs = 0;//应参加人数
                int DGPXwc = 0, QZDZPXwc = 0, CNJDCPXwc = 0, YLRQwc = 0, GLSPXwc = 0, GLZYPXwc = 0, DHZYPXwc = 0, JSJZYPXwc = 0, GKZYPXwc = 0, JJPXwc = 0;//完成的变量
                int DGPXmt = 0, QZDZPXmt = 0, CNJDCPXmt = 0, YLRQmt = 0, GLSPXmt = 0, GLZYPXmt = 0, DHZYPXmt = 0, JSJZYPXmt = 0, GKZYPXmt = 0, JJPXmt = 0;//帽贴
                int DGPXwwc = 0, QZDZPXwwc = 0, CNJDCPXwwc = 0, YLRQwwc = 0, GLSPXwwc = 0, GLZYPXwwc = 0, DHZYPXwwc = 0, JSJZYPXwwc = 0, GKZYPXwwc = 0, JJPXwwc = 0;//未完成
                var hseDrEnumerator = hseDT.Rows.GetEnumerator();
                while (hseDrEnumerator.MoveNext())
                {
                    DataRow hseCurrent = hseDrEnumerator.Current as DataRow;
                    string nonepx = hseCurrent["NONEPX"] == DBNull.Value ? "0" : hseCurrent["NONEPX"].ToString();//是否无具体培训需求
                    planning(hseCurrent["DGPX"], ref DGPXwc, ref DGPXmt, ref DGPXwwc, ref DGPXrs, nonepx);
                    planning(hseCurrent["QZDZPX"], ref QZDZPXwc, ref QZDZPXmt, ref QZDZPXwwc, ref QZDZPXrs, nonepx);
                    planning(hseCurrent["CNJDCPX"], ref CNJDCPXwc, ref CNJDCPXmt, ref CNJDCPXwwc, ref CNJDCPXrs, nonepx);
                    planning(hseCurrent["YLRQ"], ref YLRQwc, ref YLRQmt, ref YLRQwwc, ref YLRQrs, nonepx);
                    planning(hseCurrent["GLSPX"], ref GLSPXwc, ref GLSPXmt, ref GLSPXwwc, ref GLSPXrs, nonepx);
                    planning(hseCurrent["GLZYPX"], ref GLZYPXwc, ref GLZYPXmt, ref GLZYPXwwc, ref GLZYPXrs, nonepx);
                    planning(hseCurrent["DHZYPX"], ref DHZYPXwc, ref DHZYPXmt, ref DHZYPXwwc, ref DHZYPXrs, nonepx);
                    planning(hseCurrent["JSJZYPX"], ref JSJZYPXwc, ref JSJZYPXmt, ref JSJZYPXwwc, ref JSJZYPXrs, nonepx);
                    planning(hseCurrent["GKZYPX"], ref GKZYPXwc, ref GKZYPXmt, ref GKZYPXwwc, ref GKZYPXrs, nonepx);
                    planning(hseCurrent["JJPX"], ref JJPXwc, ref JJPXmt, ref JJPXwwc, ref JJPXrs, nonepx);
                }
                hseKv.Add(new HseEvaluateKv("电工培训", DGPXwc, DGPXmt, DGPXwwc, DGPXrs));
                hseKv.Add(new HseEvaluateKv("起重吊装培训", QZDZPXwc, QZDZPXmt, QZDZPXwwc, QZDZPXrs));
                hseKv.Add(new HseEvaluateKv("场内机动车培训", CNJDCPXwc, CNJDCPXmt, CNJDCPXwwc, CNJDCPXrs));
                hseKv.Add(new HseEvaluateKv("压力容器", YLRQwc, YLRQmt, YLRQwwc, YLRQrs));
                hseKv.Add(new HseEvaluateKv("锅炉水培训", GLSPXwc, GLSPXmt, GLSPXwwc, GLSPXrs));
                hseKv.Add(new HseEvaluateKv("锅炉作业培训", GLZYPXwc, GLZYPXmt, GLZYPXwwc, GLZYPXrs));
                hseKv.Add(new HseEvaluateKv("电焊作业培训", DHZYPXwc, DHZYPXmt, DHZYPXwwc, DHZYPXrs));
                hseKv.Add(new HseEvaluateKv("脚手架作业培训", JSJZYPXwc, JSJZYPXmt, JSJZYPXwwc, JSJZYPXrs));
                hseKv.Add(new HseEvaluateKv("高空作业培训", GKZYPXwc, GKZYPXmt, GKZYPXwwc, GKZYPXrs));
                hseKv.Add(new HseEvaluateKv("急救培训", JJPXwc, JJPXmt, JJPXwwc, JJPXrs));
                #endregion
            }
            ViewBag.hseKV = hseKv;//PPE需求

            #region  安全参与
            DataTable SafetyDT = new SelfEvaluateBLL().GetSafeCount(searchDept.DeptCode, year, month);
            List<HseEvaluateKv> safeKV = new List<HseEvaluateKv>();
            if (SafetyDT.Rows != null && SafetyDT.Rows.Count > 0)
            {
                safeKV.Add(new HseEvaluateKv("安全观察卡", SafetyDT.Select(" AQGCK=4").Length, SafetyDT.Select(" AQGCK=3").Length, SafetyDT.Select(" AQGCK=2").Length, SafetyDT.Select(" AQGCK=1").Length, SafetyDT.Select(" AQGCK=0").Length));
                safeKV.Add(new HseEvaluateKv("领先指标卡", SafetyDT.Select(" LXZBK=4").Length, SafetyDT.Select(" LXZBK=3").Length, SafetyDT.Select(" LXZBK=2").Length, SafetyDT.Select(" LXZBK=1").Length, SafetyDT.Select(" LXZBK=0").Length));
                safeKV.Add(new HseEvaluateKv("安全会议", SafetyDT.Select(" AQHY=4").Length, SafetyDT.Select(" AQHY=3").Length, SafetyDT.Select(" AQHY=2").Length, SafetyDT.Select(" AQHY=1").Length, SafetyDT.Select(" AQHY=0").Length));
                safeKV.Add(new HseEvaluateKv("作业安全交底", SafetyDT.Select(" ZYAQJD=4").Length, SafetyDT.Select(" ZYAQJD=3").Length, SafetyDT.Select(" ZYAQJD=2").Length, SafetyDT.Select(" ZYAQJD=1").Length, SafetyDT.Select(" ZYAQJD=0").Length));
                safeKV.Add(new HseEvaluateKv("安全检查", SafetyDT.Select(" AQJC=4").Length, SafetyDT.Select(" AQJC=3").Length, SafetyDT.Select(" AQJC=2").Length, SafetyDT.Select(" AQJC=1").Length, SafetyDT.Select(" AQJC=0").Length));
                safeKV.Add(new HseEvaluateKv("安全培训", SafetyDT.Select(" AQPX=4").Length, SafetyDT.Select(" AQPX=3").Length, SafetyDT.Select(" AQPX=2").Length, SafetyDT.Select(" AQPX=1").Length, SafetyDT.Select(" AQPX=0").Length));
            }
            #endregion
            ViewBag.SafetyDT = safeKV;//PPE需求
            //其他的统计页面上用ajax请求获取
            return View();
        }
        /// <summary>
        /// 工余安健环
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="type">1 交通，2用电，3防火，4体力操作，5其他</param>
        /// <returns></returns>
        public ActionResult GetFiveData(string deptid, string year, string month, int type)
        {
            try
            {
                var searchDept = new DepartmentBLL().GetList().FirstOrDefault(p => p.DepartmentId == deptid);
                if (searchDept == null) searchDept = new DepartmentEntity();
                DataTable dt = new SelfEvaluateBLL().GetFiveData(searchDept.DeptCode, year, month, type);
                List<HseKeyValue> kv = new List<HseKeyValue>();
                if (dt.Rows != null && dt.Rows.Count > 0)
                {
                    var drEnumerator = dt.Rows.GetEnumerator();
                    while (drEnumerator.MoveNext())
                    {
                        DataRow drCurrent = drEnumerator.Current as DataRow;
                        string key = drCurrent["KEY"] == null ? "" : drCurrent["KEY"].ToString();
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            kv.Add(new HseKeyValue(key, drCurrent["COUNT"] == null ? 0 : Convert.ToInt32(drCurrent["COUNT"])));
                        }
                    }
                }
                return Json(new { Code = 0, Data = kv });
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, ex.Message });
            }
        }
        /// <summary>
        ///  根据状体累计各项数据
        /// </summary>
        /// <param name="val">项的值 0未完成 1 完成 2 帽贴</param>
        /// <param name="wc">完成人数</param>
        /// <param name="mt">帽贴数量</param>
        /// <param name="wwc">未完成人数</param>
        /// <param name="ycjrs">应参加人数</param>
        /// <param name="nonepx">是否无具体培训需求，1是 ，其他否</param>
        private void planning(object val, ref int wc, ref int mt, ref int wwc, ref int ycjrs, string nonepx)
        {
            if (val != DBNull.Value)
            {
                //无具体培训需求，所有数据均不变，否的话判断是否是不用培训的，不用培训的所有数据也不变
                if (nonepx != "1")
                {
                    switch (val.ToString())
                    {
                        case "0"://未完成
                            ycjrs++;
                            wwc++;
                            break;
                        case "1"://完成
                            ycjrs++;
                            wc++;
                            break;
                        case "2"://帽贴
                            ycjrs++;
                            mt++;
                            wc++;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}