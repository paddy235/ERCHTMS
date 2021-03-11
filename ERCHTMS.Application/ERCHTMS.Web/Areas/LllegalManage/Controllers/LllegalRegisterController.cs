using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using System.Web;
using System.IO;
using NPOI.HSSF.UserModel;
using ERCHTMS.Busines.SaftyCheck;
using System.Data;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.Offices;
using Aspose.Words;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.HazardsourceManage;
using Newtonsoft.Json;
using System.Threading;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using System.Collections;
using Newtonsoft.Json.Linq;
using ERCHTMS.Busines.AuthorizeManage;
using System.Drawing;
using Aspose.Cells;
using BSFramework.Util.Attributes;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 描 述：违章基本信息表
    /// </summary>
    public class LllegalRegisterController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // 违章基本信息
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //整改信息对象
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //核准信息对象
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //验收信息对象
        private LllegalConfirmBLL lllegalconfirmbll = new LllegalConfirmBLL(); //验收确认信息对象
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); // 考核信息对象
        private LllegalAwardDetailBLL lllegalawarddetailbll = new LllegalAwardDetailBLL(); //违章奖励信息
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private HazardsourceBLL hazardsourcebll = new HazardsourceBLL();
        private ModuleListColumnAuthBLL modulelistcolumnauthbll = new ModuleListColumnAuthBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();

        private WfControlBLL wfcontrolbll = new WfControlBLL();//自动化流程服务

        #region 视图功能
        [HttpGet]
        public ActionResult Intergral()
        {
            return View();
        }

        [HttpGet]
        public ActionResult IntergralPerson()
        {
            //获取信息
            var Ids = Request["Ids"] ?? "";
            var list = new List<LllegalPunishEntity>();
            var list2 = new List<LllegalRegisterEntity>();
            if (Ids.Length > 0)
            {
                var Idsp = Ids.Split(',');

                foreach (var Id in Idsp)
                {
                    var baseInfo = lllegalregisterbll.GetEntity(Id);  //违章基本信息
                    var approveInfo = lllegalpunishbll.GetEntityByBid(Id);
                    list.Add(approveInfo);
                    list2.Add(baseInfo);
                }
            }
            ViewBag.listLA = list;
            ViewBag.listBaseInfo = list2;
            return View();
        }

        [HttpGet]
        public ActionResult IntergralPersonHistory()
        {
            //获取信息
            return View();
        }

        /// <summary>
        /// 积分提醒设置页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RemindForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult XssIntergralPerson()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ExaminIndex()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AwardForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AwardIndex()
        {
            return View();
        }

        #region 列表页面
        /// <summary>
        /// 列表页面  各流程页面使用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }



        #endregion

        #region 表单页面
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            string km_major_role = dataitemdetailbll.GetItemValue("KM_MAJOR_ROLE"); //可门配置  
            //可门
            if (!string.IsNullOrEmpty(km_major_role))
            {
                string actionName = string.Empty;
                string[] allKeys = Request.QueryString.AllKeys;
                if (allKeys.Count() > 0)
                {
                    actionName = "KmForm?";
                    int num = 0;
                    foreach (string str in allKeys)
                    {
                        string strValue = Request.QueryString[str];
                        if (num == 0)
                        {
                            actionName += str + "=" + strValue;
                        }
                        else
                        {
                            actionName += "&" + str + "=" + strValue;
                        }
                        num++;
                    }
                }
                else
                {
                    actionName = "KmForm";
                }
                return Redirect(actionName);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult KmForm()
        {
            return View();
        }
        #endregion

        #region 台账页面
        /// <summary>
        ///  立即整改详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SdIndex()
        {
            return View();
        }
        #endregion

        #region 台账页面
        /// <summary>
        ///  西塞山详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult XSSIndex()
        {
            return View();
        }
        #endregion

        #region 考核内容
        /// <summary>
        /// 考核内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExaminForm()
        {
            return View();
        }
        #endregion

        #region 立即整改详情页面
        /// <summary>
        ///  立即整改详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewForm()
        {
            return View();
        }
        #endregion

        #region 现有违章页面
        /// <summary>
        ///  立即整改详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DetailList()
        {
            return View();
        }
        #endregion


        #region 应用层违章页面
        /// <summary>
        ///  立即整改详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AppIndex()
        {
            return View();
        }
        #endregion

        #endregion

        #region 基础数据查询工作



        #region 页面组件初始化
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInitDataJson()
        {
            Operator CreateUser = OperatorProvider.Provider.Current();
            //考核方式 违章类型 违章等级 流程状态
            string itemCode = "'ExamineWay','LllegalType','LllegalLevel','FlowState','HidMajorClassify','ChangeDeptRelevancePerson','IsUseConciseRegister','ControlPicMustUpload','LllegalAwardDetailAuth'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);

            string mark = userbll.GetSafetyAndDeviceDept(CreateUser); //用于标记可立即整改违章

            string RelevancePersonRole = string.Empty;
            if (itemlist.Where(p => p.EnCode == "ChangeDeptRelevancePerson").Count() > 0)
            {
                RelevancePersonRole = itemlist.Where(p => p.EnCode == "ChangeDeptRelevancePerson").Where(p => p.ItemName == CreateUser.OrganizeCode).FirstOrDefault().ItemValue;
            }
            //相关图片必传控制配置
            string ControlPicMustUpload = string.Empty;
            var cpmu = itemlist.Where(p => p.EnCode == "ControlPicMustUpload").Where(p => p.ItemName == CreateUser.OrganizeId);
            if (cpmu.Count() > 0)
            {
                ControlPicMustUpload = cpmu.FirstOrDefault().ItemValue;
            }
            //是否能操作违章奖励
            var awardauth = itemlist.Where(p => p.EnCode == "LllegalAwardDetailAuth").Where(p => p.ItemName == CreateUser.OrganizeId);
            string LllegalAwardDetailAuth = awardauth.Count() > 0 ? "1" : "";
            //返回值
            var josnData = new
            {
                CreateUser = CreateUser.UserName,
                User = CreateUser,  //用户对象
                Mark = mark, // 1 为安全管理部门  2 为装置部门
                ApplianceClass = dataitemdetailbll.GetItemValue("ApplianceClass"),  //装置类对象 
                ExamineWay = itemlist.Where(p => p.EnCode == "ExamineWay"), //考核方式
                LllegalType = itemlist.Where(p => p.EnCode == "LllegalType"), //违章类型
                LllegalLevel = itemlist.Where(p => p.EnCode == "LllegalLevel"),  //违章级别
                FlowState = itemlist.Where(p => p.EnCode == "FlowState"),  //违章流程状态
                MajorClassify = itemlist.Where(p => p.EnCode == "HidMajorClassify"),  //专业分类
                IsHrdl = dataitemdetailbll.GetItemValue("IsOpenPassword"), //是否华润电力
                RelevancePersonRole = RelevancePersonRole,
                IsDeliver = htworkflowbll.GetCurUserWfAuth("", "违章整改", "违章整改", "厂级违章流程", "转交") == "1" ? "1" : "",
                IsAcceptDeliver = htworkflowbll.GetCurUserWfAuth("", "违章验收", "违章验收", "厂级违章流程", "转交") == "1" ? "1" : "",
                IsUseConciseRegister = itemlist.Where(p => p.EnCode == "IsUseConciseRegister").Where(p => p.ItemValue == CreateUser.OrganizeId).Count(), //是否采用简洁版登记
                ControlPicMustUpload = ControlPicMustUpload,
                LllegalAwardDetailAuth = LllegalAwardDetailAuth
            };

            return Content(josnData.ToJson());
        }
        #endregion

        #region 初始化查询条件
        /// <summary>
        /// 初始化查询条件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueryConditionJson()
        {
            Operator CreateUser = OperatorProvider.Provider.Current();

            //考核方式，违章类型，违章级别，流程状态
            string itemCode = "'ExamineWay','LllegalType','LllegalLevel','FlowState','LllegalDataScope','HidStandingType','LllegalStatus'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            string mark = userbll.GetSafetyAndDeviceDept(CreateUser); //用于标记可立即整改违章
            //返回值
            var josnData = new
            {
                Mark = mark, // 1 为安全管理部门  2 为装置部门
                ApplianceClass = dataitemdetailbll.GetItemValue("ApplianceClass"),  //装置类对象 
                ExamineWay = itemlist.Where(p => p.EnCode == "ExamineWay"), //考核方式
                LllegalType = itemlist.Where(p => p.EnCode == "LllegalType"), //违章类型
                LllegalLevel = itemlist.Where(p => p.EnCode == "LllegalLevel"),  //违章级别 
                FlowState = itemlist.Where(p => p.EnCode == "FlowState"),  //违章流程状态
                DataScope = itemlist.Where(p => p.EnCode == "LllegalDataScope"),//数据范围 
                HidStandingType = itemlist.Where(p => p.EnCode == "HidStandingType"),//台账类型  
                LllegalStatus = itemlist.Where(p => p.EnCode == "LllegalStatus") //违章状态
            };

            return Content(josnData.ToJson());
        }
        #endregion


        #region 获取编码下的权限控制
        /// <summary>
        /// 获取编码下的权限控制
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int GetCurOperAuthByItemCode(string itemcode)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            string code = "'" + itemcode + "'";

            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(code);

            var item = itemlist.Where(p => p.ItemName.Trim() == curUser.DeptId);

            int rcount = 0;

            if (item.Count() > 0)
            {
                string rolename = item.FirstOrDefault().ItemValue.Trim();

                if (!string.IsNullOrEmpty(rolename))
                {
                    foreach (string rolestr in rolename.Split(','))
                    {
                        if (!string.IsNullOrEmpty(rolestr))
                        {
                            if (curUser.RoleName.Contains(rolestr))
                            {
                                rcount += 1;
                            }
                        }
                    }
                }
            }

            return rcount;
        }
        #endregion



        /// <summary>
        /// 获取专业主管
        /// </summary>
        /// <param name="reformdeptcode"></param>
        /// <param name="majorclassify"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetKmSpecialPerson(string reformdeptcode, string majorclassify)
        {
            UserInfoEntity userinfo = new UserInfoEntity();

            //专业不为空，获取专业主管
            if (!string.IsNullOrEmpty(reformdeptcode) && !string.IsNullOrEmpty(majorclassify))
            {
                List<UserInfoEntity> ulist = userbll.GetUserListByCodeAndRole(reformdeptcode, "").ToList();
                List<UserInfoEntity> lastlist = new List<UserInfoEntity>();
                string km_major_role = dataitemdetailbll.GetItemValue("KM_MAJOR_ROLE"); //可门配置 
                var mcItem = dataitemdetailbll.GetDataItemListByItemCode("'HidMajorClassify'").Where(p => p.ItemDetailId == majorclassify).FirstOrDefault();
                var templist = ulist.Where(p => p.RoleName.Contains(km_major_role)).ToList();
                string SpecialtyType = "," + mcItem.ItemValue + ",";
                foreach (UserInfoEntity entity in templist)
                {
                    string sptype = !string.IsNullOrEmpty(entity.SpecialtyType) ? "," + entity.SpecialtyType + "," : "";
                    if (sptype.Contains(SpecialtyType))
                    {
                        lastlist.Add(entity);
                    }
                }
                if (lastlist.Count() > 0)
                {
                    userinfo = lastlist.FirstOrDefault();
                }
            }
            return Content(userinfo.ToJson());
        }

        #region 获取当前用户的流程权限
        /// <summary>
        /// 获取当前用户的流程权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCurUserWfAuth(string rankname, string workflow, string endflow, string mark, string submittype, string arg1 = "", string arg2 = "", string arg3 = "", string businessid = "")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = businessid; //
            wfentity.startflow = workflow;
            wfentity.endflow = endflow;
            wfentity.submittype = submittype;
            wfentity.rankname = rankname;
            wfentity.user = curUser;
            wfentity.mark = mark;
            wfentity.isvaliauth = true;
            wfentity.argument1 = arg1;
            wfentity.argument2 = arg2;
            wfentity.argument3 = arg3;

            //获取下一流程的操作人
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            if (result.ishave)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }
        #endregion

        #region 获取对应的权限
        /// <summary>
        /// 获取对应的权限
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCurUserWfAuthByCondition(string queryJson)
        {
            List<object> list = new List<object>();
            if (!string.IsNullOrEmpty(queryJson))
            {

                Operator curUser = OperatorProvider.Provider.Current();
                JArray jarray = (JArray)JsonConvert.DeserializeObject(queryJson);
                foreach (JObject obj in jarray)
                {
                    string rankname = obj["rankname"].ToString();
                    string workflow = obj["workflow"].ToString();
                    string endflow = obj["endflow"].ToString();
                    string mark = obj["mark"].ToString();
                    string submittype = obj["submittype"].ToString();
                    string arg1 = queryJson.Contains("arg1") ? obj["arg1"].ToString() : string.Empty;
                    string arg2 = queryJson.Contains("arg2") ? obj["arg2"].ToString() : string.Empty;
                    string arg4 = queryJson.Contains("arg4") ? obj["arg4"].ToString() : string.Empty;
                    string arg5 = queryJson.Contains("arg5") ? obj["arg5"].ToString() : string.Empty;
                    string businessid = obj["businessid"].ToString();
                    string action = obj["action"].ToString();

                    WfControlObj wfentity = new WfControlObj();
                    wfentity.businessid = businessid;
                    wfentity.startflow = workflow;
                    wfentity.endflow = endflow;
                    wfentity.submittype = submittype;
                    wfentity.rankname = rankname;
                    wfentity.user = curUser;
                    wfentity.mark = mark;
                    wfentity.isvaliauth = true;
                    wfentity.argument1 = arg1;
                    wfentity.argument2 = arg2;
                    wfentity.argument3 = curUser.DeptId;
                    wfentity.argument4 = arg4;
                    wfentity.argument5 = arg5;
                    //获取下一流程的操作人
                    WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                    list.Add(new { key = action, value = result.ishave });
                }
            }
            return Content(list.ToJson());
        }
        #endregion

        #region 获取违章列表数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            Operator opertator = new OperatorProvider().Current();
            queryJson = queryJson.Insert(1, "\"userId\":\"" + opertator.UserId + "\","); //添加当前用户
            var data = lllegalregisterbll.GetLllegalBaseInfo(pagination, queryJson);
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
        #endregion

        #region 获取违章的详情对象
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            //隐患基本信息
            var baseInfo = lllegalregisterbll.GetEntity(keyValue);  //违章基本信息

            var approveInfo = lllegalapprovebll.GetEntityByBid(baseInfo.ID); //核准信息

            var reformInfo = lllegalreformbll.GetEntityByBid(baseInfo.ID); // 整改信息

            var punishInfo = lllegalpunishbll.GetEntityByBid(baseInfo.ID); //考核内容(违章负责人)

            var relevanceInfo = lllegalpunishbll.GetListByLllegalId(baseInfo.ID, "");//考核内容（关联责任人）

            var acceptInfo = lllegalacceptbll.GetEntityByBid(baseInfo.ID); //验收信息

            var confirmInfo = lllegalconfirmbll.GetEntityByBid(baseInfo.ID); //验收信息

            var awardInfo = lllegalawarddetailbll.GetListByLllegalId(baseInfo.ID);//违章奖励信息

            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户

            int isReformBack = 0;

            var historyacceptList = lllegalacceptbll.GetHistoryList(keyValue).ToList();
            if (historyacceptList.Count() > 0)
            {
                isReformBack = 1;
            }
            var data = new { baseInfo = baseInfo, approveInfo = approveInfo, reformInfo = reformInfo, punishInfo = punishInfo, relevanceInfo = relevanceInfo, acceptInfo = acceptInfo, confirmInfo = confirmInfo, awardInfo = awardInfo, userInfo = userInfo, isreformback = isReformBack };

            return ToJsonResult(data);
        }

        #endregion

        #endregion

        #region 保存数据  包含新增、修改、 删除

        #region  保存表单（新增、修改）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, LllegalRegisterEntity entity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            CommonSaveForm(keyValue, "03", entity, rEntity, aEntity);
            return Success("操作成功!");
        }
        #endregion

        #region 公用方法，保存数据
        /// <summary>
        /// 公用方法，保存数据
        /// </summary>
        /// <param name="keyValue">主键ID</param>
        /// <param name="entity">违章基本信息</param>
        /// <param name="rEntity">整改信息</param>
        /// <param name="aEntity">验收信息</param>
        public void CommonSaveForm(string keyValue, string workFlow, LllegalRegisterEntity entity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            try
            {
                //提交通过
                string userId = OperatorProvider.Provider.Current().UserId;
                var userInfo = OperatorProvider.Provider.Current();  //获取当前用户
                //保存违章基本信息
                entity.RESEVERFOUR = "";
                entity.RESEVERFIVE = "";
                //通过违章编码判断是否存在重复
                if (string.IsNullOrEmpty(keyValue))
                {
                    string lenNum = !string.IsNullOrEmpty(dataitemdetailbll.GetItemValue("LllegalSerialNumberLen")) ? dataitemdetailbll.GetItemValue("LllegalSerialNumberLen") : "3";

                    entity.LLLEGALNUMBER = lllegalregisterbll.GenerateHidCode("bis_lllegalregister", "lllegalnumber", int.Parse(lenNum));

                    entity.CREATEDEPTID = userInfo.DeptId;
                    entity.CREATEDEPTNAME = userInfo.DeptName;
                    entity.BELONGDEPARTID = userInfo.OrganizeId;
                    entity.BELONGDEPART = userInfo.OrganizeName;
                }

                lllegalregisterbll.SaveForm(keyValue, entity);

                //创建流程实例
                if (string.IsNullOrEmpty(keyValue))
                {
                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, userId);
                    if (isSucess)
                    {
                        lllegalregisterbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //更新业务流程状态
                    }
                }

                /************考核信息**********/
                #region 考核内容
                string RELEVANCEDATA = Request.Form["RELEVANCEDATA"];
                if (!string.IsNullOrEmpty(RELEVANCEDATA))
                {  //先删除关联责任人集合
                    lllegalpunishbll.DeleteLllegalPunishList(entity.ID, "");

                    JArray jarray = (JArray)JsonConvert.DeserializeObject(RELEVANCEDATA);

                    foreach (JObject rhInfo in jarray)
                    {
                        //string relevanceId = rhInfo["ID"].ToString(); //主键id
                        string assessobject = rhInfo["ASSESSOBJECT"].ToString();
                        string personinchargename = rhInfo["PERSONINCHARGENAME"].ToString(); //关联责任人姓名
                        string personinchargeid = rhInfo["PERSONINCHARGEID"].ToString();//关联责任人id
                        string performancepoint = rhInfo["PERFORMANCEPOINT"].ToString();//EHS绩效考核 
                        string economicspunish = rhInfo["ECONOMICSPUNISH"].ToString(); // 经济处罚
                        string education = rhInfo["EDUCATION"].ToString(); //教育培训
                        string lllegalpoint = rhInfo["LLLEGALPOINT"].ToString();//违章扣分
                        string awaitjob = rhInfo["AWAITJOB"].ToString();//待岗
                        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                        newpunishEntity.LLLEGALID = entity.ID;
                        newpunishEntity.ASSESSOBJECT = assessobject; //考核对象
                        newpunishEntity.PERSONINCHARGEID = personinchargeid;
                        newpunishEntity.PERSONINCHARGENAME = personinchargename;
                        newpunishEntity.PERFORMANCEPOINT = !string.IsNullOrEmpty(performancepoint) ? Convert.ToDecimal(performancepoint) : 0;
                        newpunishEntity.ECONOMICSPUNISH = !string.IsNullOrEmpty(economicspunish) ? Convert.ToDecimal(economicspunish) : 0;
                        newpunishEntity.EDUCATION = !string.IsNullOrEmpty(education) ? Convert.ToDecimal(education) : 0;
                        newpunishEntity.LLLEGALPOINT = !string.IsNullOrEmpty(lllegalpoint) ? Convert.ToDecimal(lllegalpoint) : 0;
                        newpunishEntity.AWAITJOB = !string.IsNullOrEmpty(awaitjob) ? Convert.ToDecimal(awaitjob) : 0;
                        newpunishEntity.MARK = assessobject.Contains("考核") ? "0" : "1"; //标记0考核，1联责
                        lllegalpunishbll.SaveForm("", newpunishEntity);
                    }
                }
                #endregion

                #region 违章奖励信息
                string AWARDDATA = Request.Form["AWARDDATA"];
                if (!string.IsNullOrEmpty(AWARDDATA))
                {  //先删除关联集合
                    lllegalawarddetailbll.DeleteLllegalAwardList(entity.ID);

                    JArray jarray = (JArray)JsonConvert.DeserializeObject(AWARDDATA);

                    foreach (JObject rhInfo in jarray)
                    {
                        string userid = rhInfo["USERID"].ToString(); //奖励用户
                        string username = rhInfo["USERNAME"].ToString();
                        string deptid = rhInfo["DEPTID"].ToString();//奖励用户部门
                        string deptname = rhInfo["DEPTNAME"].ToString();
                        string points = rhInfo["POINTS"].ToString();  //奖励积分
                        string money = rhInfo["MONEY"].ToString(); //奖励金额

                        LllegalAwardDetailEntity awardEntity = new LllegalAwardDetailEntity();
                        awardEntity.LLLEGALID = entity.ID;
                        awardEntity.USERID = userid; //奖励对象
                        awardEntity.USERNAME = username;
                        awardEntity.DEPTID = deptid;
                        awardEntity.DEPTNAME = deptname;
                        awardEntity.POINTS = !string.IsNullOrEmpty(points) ? int.Parse(points) : 0;
                        awardEntity.MONEY = !string.IsNullOrEmpty(money) ? Convert.ToDecimal(money) : 0;
                        lllegalawarddetailbll.SaveForm("", awardEntity);
                    }
                }
                #endregion

                /********整改信息************/
                string REFORMID = Request.Form["REFORMID"].ToString();
                rEntity.LLLEGALID = entity.ID;

                //新增状态下添加
                if (!string.IsNullOrEmpty(REFORMID))
                {
                    var tempEntity = lllegalreformbll.GetEntity(REFORMID);
                    rEntity.AUTOID = tempEntity.AUTOID;
                    rEntity.REFORMSTATUS = string.Empty;
                }
                lllegalreformbll.SaveForm(REFORMID, rEntity);


                /********验收信息************/
                string ACCEPTID = Request.Form["ACCEPTID"].ToString();
                aEntity.LLLEGALID = entity.ID;
                if (!string.IsNullOrEmpty(ACCEPTID))
                {
                    var tempEntity = lllegalacceptbll.GetEntity(ACCEPTID);
                    aEntity.AUTOID = tempEntity.AUTOID;
                }
                lllegalacceptbll.SaveForm(ACCEPTID, aEntity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 删除违章信息数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除违章基本信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            LllegalRegisterEntity entity = lllegalregisterbll.GetEntity(keyValue);
            Operator user = OperatorProvider.Provider.Current();
            //删除违章所有信息
            lllegalregisterbll.RemoveForm(keyValue);


            LogEntity logEntity = new LogEntity();
            logEntity.Browser = this.Request.Browser.Browser;
            logEntity.CategoryId = 6;
            logEntity.OperateTypeId = "6";
            logEntity.OperateType = "删除";
            logEntity.OperateAccount = user.UserName;
            logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
            logEntity.ExecuteResult = 1;
            logEntity.Module = SystemInfo.CurrentModuleName;
            logEntity.ModuleId = SystemInfo.CurrentModuleId;
            logEntity.ExecuteResultJson = "操作信息:删除违章编号为" + entity.LLLEGALNUMBER + ",违章描述为" + entity.LLLEGALDESCRIBE + "的违章信息, 请求引用: 无, 其他信息:无";
            LogBLL.WriteLog(logEntity);

            return Success("删除成功!");
        }
        #endregion

        #endregion

        #region 提交数据  包含流程提交

        #region 提交流程（同时新增、修改隐患信息）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity,
            LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            //判断重复编号过程
            if (!string.IsNullOrEmpty(entity.LLLEGALNUMBER))
            {
                var curHtBaseInfor = lllegalregisterbll.GetListByNumber(entity.LLLEGALNUMBER).FirstOrDefault();

                if (null != curHtBaseInfor)
                {
                    if (curHtBaseInfor.ID != keyValue && string.IsNullOrEmpty(keyValue))
                    {
                        return Error("违章编号重复,请重新新增!");
                    }
                }
            }
            try
            {
                //创建流程，保存对应信息
                CommonSaveForm(keyValue, "03", entity, rEntity, aEntity);

                if (!string.IsNullOrEmpty(entity.ID))
                {
                    entity = lllegalregisterbll.GetEntity(entity.ID);
                }
                //创建完流程实例后
                if (string.IsNullOrEmpty(keyValue))
                {
                    keyValue = entity.ID;
                }
                //此处需要判断当前人是否为安全管理员
                string wfFlag = string.Empty;
                //当前用户
                Operator curUser = OperatorProvider.Provider.Current();

                //参与人员
                string participant = string.Empty;

                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.argument1 = entity.MAJORCLASSIFY; //专业分类
                wfentity.argument2 = entity.LLLEGALTYPE;//违章类型
                wfentity.argument3 = curUser.DeptId;//当前部门id
                wfentity.argument4 = entity.LLLEGALTEAMCODE;//违章部门
                wfentity.argument5 = entity.LLLEGALLEVEL; //违章级别
                wfentity.startflow = entity.FLOWSTATE;
                //是否上报
                if (entity.ISUPSAFETY == "1")
                {
                    wfentity.submittype = "上报";
                }
                else
                {
                    wfentity.submittype = "提交";
                    //不指定整改责任人
                    if (rEntity.ISAPPOINT == "0")
                    {
                        wfentity.submittype = "制定提交";
                    }
                }
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.mark = "厂级违章流程";
                wfentity.organizeid = entity.BELONGDEPARTID; //对应电厂id
                //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);

                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;

                    //提交流程到下一节点
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //更新业务流程状态
                        }
                    }
                    return Success(result.message);
                }
                else
                {
                    return Error(result.message);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region 提交制定整改计划流程（同时修改隐患信息）
        /// <summary>
        /// 提交制定整改计划流程（同时修改隐患信息）
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitChangePlanForm(string keyValue, LllegalRegisterEntity entity,
            LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string participant = string.Empty;

            //判断重复编号过程
            if (!string.IsNullOrEmpty(entity.LLLEGALNUMBER))
            {
                var curHtBaseInfor = lllegalregisterbll.GetListByNumber(entity.LLLEGALNUMBER).FirstOrDefault();

                if (null != curHtBaseInfor)
                {
                    if (curHtBaseInfor.ID != keyValue && string.IsNullOrEmpty(keyValue))
                    {
                        return Error("违章编号重复,请重新新增!");
                    }
                }
            }
            //创建流程，保存对应信息
            CommonSaveForm(keyValue, "03", entity, rEntity, aEntity);

            //创建完流程实例后
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }

            //此处需要判断当前人是否为安全管理员
            string wfFlag = string.Empty;

            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //
            wfentity.startflow = "制定整改计划";
            wfentity.submittype = "提交";
            wfentity.rankid = null;
            wfentity.user = curUser;
            if (entity.ADDTYPE == "2")
            {
                wfentity.mark = "省级违章流程";
            }
            else
            {
                wfentity.mark = "厂级违章流程";
            }
            wfentity.organizeid = entity.BELONGDEPARTID; //对应电厂id
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
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //更新业务流程状态
                    }
                }
                else
                {
                    return Error("请联系系统管理员，添加当前流程下的参与者!");
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

        #region 转交制定整改计划/违章整改,违章验收流程
        /// <summary>
        /// 转交制定整改计划/违章整改,违章验收流程
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult DeliverPlanForm(string keyValue, LllegalReformEntity rentity,LllegalAcceptEntity aentity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string participant = string.Empty;
            try
            {
                //违章基本信息
                LllegalRegisterEntity entity = lllegalregisterbll.GetEntity(keyValue);

                /********整改信息************/
                if (!Request.Form["REFORMID"].IsEmpty())
                {
                    string REFORMID = Request.Form["REFORMID"].ToString();
                    //更新整改信息
                    if (!string.IsNullOrEmpty(REFORMID))
                    {
                        var reformEntity = lllegalreformbll.GetEntity(REFORMID);
                        reformEntity.REFORMCHARGEDEPTID = rentity.REFORMCHARGEDEPTID;
                        reformEntity.REFORMCHARGEDEPTNAME = rentity.REFORMCHARGEDEPTNAME;
                        reformEntity.REFORMCHARGEPERSON = rentity.REFORMCHARGEPERSON;
                        reformEntity.REFORMCHARGEPERSONNAME = rentity.REFORMCHARGEPERSONNAME;
                        reformEntity.REFORMPEOPLE = rentity.REFORMPEOPLE;
                        reformEntity.REFORMPEOPLEID = rentity.REFORMPEOPLEID;
                        reformEntity.REFORMDEPTNAME = rentity.REFORMDEPTNAME;
                        reformEntity.REFORMDEPTCODE = rentity.REFORMDEPTCODE;
                        reformEntity.REFORMTEL = rentity.REFORMTEL;
                        lllegalreformbll.SaveForm(REFORMID, reformEntity);
                    }
                }
                /*******验收信息*****/
                if (!Request.Form["ACCEPTID"].IsEmpty())
                {
                    string ACCEPTID = Request.Form["ACCEPTID"].ToString();
                    //更新验收信息
                    if (!string.IsNullOrEmpty(ACCEPTID))
                    {
                        var acceptEntity = lllegalacceptbll.GetEntity(ACCEPTID);
                        acceptEntity.ACCEPTDEPTCODE = aentity.ACCEPTDEPTCODE;
                        acceptEntity.ACCEPTDEPTNAME = aentity.ACCEPTDEPTNAME;
                        acceptEntity.ACCEPTPEOPLE = aentity.ACCEPTPEOPLE;
                        acceptEntity.ACCEPTPEOPLEID = aentity.ACCEPTPEOPLEID;
                        lllegalacceptbll.SaveForm(ACCEPTID, acceptEntity);
                    }
                }

                //此处需要判断当前人是否为安全管理员
                string wfFlag = string.Empty;
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.startflow = entity.FLOWSTATE;
                wfentity.endflow = entity.FLOWSTATE;
                wfentity.submittype = "转交";
                wfentity.rankid = null;
                wfentity.user = curUser;
                if (entity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级违章流程";
                }
                else
                {
                    wfentity.mark = "厂级违章流程";
                }
                wfentity.organizeid = entity.BELONGDEPARTID; //对应电厂id
                                                             //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;
                    if (!string.IsNullOrEmpty(participant))
                    {
                        //不更改状态
                        if (!result.ischangestatus)
                        {
                            htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);
                            return Success(result.message);
                        }
                    }
                    else
                    {
                        return Error("请联系系统管理员，添加当前流程下的参与者!");
                    }
                    return Success(result.message);
                }
                else
                {
                    return Error(result.message);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region 一次性提交违章及整改信息
        /// <summary>
        /// 一次性提交违章及整改信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pEntity"></param>
        /// <param name="rEntity"></param>
        /// <param name="aEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult OneSubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalApproveEntity pEntity,
            LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            string ApproveID = Request.Form["APPROVEID"].ToString();
            string ReformID = Request.Form["REFORMID"].ToString();
            string AcceptID = Request.Form["ACCEPTID"].ToString();
            //此处需要判断当前人是否为安全管理员
            string wfFlag = string.Empty;

            Operator curUser = OperatorProvider.Provider.Current();

            //参与人员
            string participant = string.Empty;

            ////保存违章信息
            CommonSaveForm(keyValue, "04", entity, rEntity, aEntity);

            ////保存核准信息
            pEntity.LLLEGALID = entity.ID;

            lllegalapprovebll.SaveForm(ApproveID, pEntity);

            wfFlag = "1";//整改结束

            participant = curUser.Account;

            //提交流程
            int count = htworkflowbll.SubmitWorkFlow(entity.ID, participant, wfFlag, curUser.UserId);

            if (count > 0)
            {
                htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //更新业务流程状态

                //string tagName = htworkflowbll.QueryTagNameByCurrentWF(entity.ID);

                //#region 违章评分对象
                //if (tagName == "违章整改" || tagName == "流程结束")
                //{
                //    //添加分数到
                //    lllegalregisterbll.AddLllegalScore(entity);
                //}
                //#endregion
            }
            return Success("操作成功!");
        }
        #endregion

        #region 一次性提交多个关联检查人的登记违章信息
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CheckLllegalForm(string checkid)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'FlowState'");
            string startnode = itemlist.Where(p => p.ItemName == "违章登记").Count() > 0 ? "违章登记" : "违章举报";
            var dtHid = lllegalregisterbll.GetListByCheckId(checkid, curUser.UserId, startnode);

            string keyValue = string.Empty;

            string reformpeopleid = string.Empty; //整改人

            foreach (DataRow row in dtHid.Rows)
            {
                keyValue = row["id"].ToString();

                reformpeopleid = row["reformpeopleid"].ToString();

                string createuserid = row["createuserid"].ToString();

                LllegalRegisterEntity entity = lllegalregisterbll.GetEntity(keyValue);

                LllegalPunishEntity pbEntity = lllegalpunishbll.GetEntityByBid(keyValue);

                //此处需要判断当前人是否为安全管理员
                string wfFlag = string.Empty;
                //参与人员
                string participant = string.Empty;

                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.argument1 = entity.MAJORCLASSIFY; //专业分类
                wfentity.argument2 = entity.LLLEGALTYPE;//违章类型
                wfentity.argument3 = curUser.DeptId;//当前部门id
                wfentity.argument4 = entity.LLLEGALTEAMCODE;//违章部门
                wfentity.argument5 = entity.LLLEGALLEVEL; //违章级别
                wfentity.startflow = startnode;
                //是否上报
                if (entity.ISUPSAFETY == "1")
                {
                    wfentity.submittype = "上报";
                }
                else
                {
                    wfentity.submittype = "提交";
                    //不指定整改责任人
                    if (row["isappoint"].ToString() == "0")
                    {
                        wfentity.submittype = "制定提交";
                    }
                }
                wfentity.rankid = null;
                wfentity.spuser = userbll.GetUserInfoEntity(createuserid);
                //省级登记的
                if (entity.ADDTYPE == "2")
                {
                    wfentity.mark = "省级违章流程";
                }
                else
                {
                    wfentity.mark = "厂级违章流程";
                }
                wfentity.organizeid = entity.BELONGDEPARTID; //对应电厂id
                //获取下一流程的操作人
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);

                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;

                    //提交流程到下一节点
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, createuserid);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //更新业务流程状态
                        }
                    }
                }
            }

            return Success("操作成功!");
        }
        #endregion

        #endregion


        #region 导出违章基本信息
        /// <summary>
        /// 导出违章基本信息
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportExcel(string queryJson, string fileName, string currentModuleId, string mode = "")
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            queryJson = queryJson.Insert(1, "\"userId\":\"" + curUser.UserId + "\","); //添加当前用户
            string userId = curUser.UserId;
            string p_fields = string.Empty;
            string p_fieldsName = string.Empty;
            try
            {
                //系统默认的列表设置
                var defaultdata = modulelistcolumnauthbll.GetEntity(currentModuleId, "", 0);
                if (null != defaultdata)
                {
                    p_fields = defaultdata.DEFAULTCOLUMNFIELDS;
                    p_fieldsName = defaultdata.DEFAULTCOLUMNNAME;
                }
                //当前用户的列表设置
                var data = modulelistcolumnauthbll.GetEntity(currentModuleId, curUser.UserId, 1);
                //为空，自动读取系统默认
                if (null != data)
                {
                    p_fields = data.DEFAULTCOLUMNFIELDS;
                    p_fieldsName = data.DEFAULTCOLUMNNAME;
                }
                p_fields = "flowstate," + p_fields + ",participantname";
                p_fieldsName = "流程状态," + p_fieldsName + ",流程处理人";
                pagination.p_fields = p_fields;
                //取出数据源
                DataTable exportTable = lllegalregisterbll.GetLllegalBaseInfo(pagination, queryJson);
                exportTable.Columns.Remove("id");
                exportTable.Columns["r"].SetOrdinal(0);

                // 详细列表内容
                string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;

                Aspose.Cells.Cell cell = sheet.Cells[0, 0];
                cell.PutValue("违章基本信息"); //标题
                cell.Style.Pattern = BackgroundType.Solid;
                cell.Style.Font.Size = 14;
                cell.Style.Font.Color = Color.Black;

                DataTable dt = new DataTable();
                dt.Columns.Add("r");
                //再设置相关行列
                if (!string.IsNullOrEmpty(p_fields))
                {
                    string[] p_fieldsArray = p_fields.Split(',');
                    for (int i = 0; i < p_fieldsArray.Length; i++)
                    {
                        dt.Columns.Add(p_fieldsArray[i].ToString(), typeof(string));//列头
                    }

                    foreach (DataRow row in exportTable.Rows)
                    {
                        DataRow newrow = dt.NewRow();
                        newrow["r"] = row["r"].ToString();
                        for (int i = 0; i < p_fieldsArray.Length; i++)
                        {
                            string curColName = p_fieldsArray[i].ToString();

                            if (curColName != "lllegalfilepath" && curColName != "reformfilepath")
                            {
                                newrow[curColName] = row[curColName].ToString();
                            }
                            else
                            {
                                newrow[curColName] = "";
                            }
                        }
                        dt.Rows.Add(newrow);
                    }
                }

                //再设置相关行列
                if (!string.IsNullOrEmpty(p_fieldsName))
                {
                    //动态加列
                    string[] p_filedsNameArray = p_fieldsName.Split(',');
                    //序号列
                    Aspose.Cells.Cell serialcell = sheet.Cells[1, 0];
                    serialcell.PutValue("序号"); //填报单位

                    for (int i = 0; i < p_filedsNameArray.Length; i++)
                    {
                        Aspose.Cells.Cell curcell = sheet.Cells[1, i + 1];
                        curcell.PutValue(p_filedsNameArray[i].ToString()); //列头
                    }
                    //合并单元格
                    Aspose.Cells.Cells cells = sheet.Cells;
                    cells.Merge(0, 0, 1, p_filedsNameArray.Length + 1);
                }

                //先导入数据
                sheet.Cells.ImportDataTable(dt, false, 2, 0);

                //插入图片操作
                if (!string.IsNullOrEmpty(p_fieldsName))
                {
                    int picnum = 0;
                    foreach (DataRow row in exportTable.Rows)
                    {
                        //违章图片
                        if (exportTable.Columns.Contains("lllegalfilepath"))
                        {
                            int colIndex = exportTable.Columns.IndexOf("lllegalfilepath");
                            string lllegalfilepath = row["lllegalfilepath"].ToString();
                            if (!string.IsNullOrEmpty(lllegalfilepath))
                            {
                                string imageUrl = System.Web.HttpContext.Current.Server.MapPath(lllegalfilepath);
                                if (System.IO.File.Exists(imageUrl))
                                {
                                    sheet.Pictures.Add(2 + picnum, colIndex, 3 + picnum, colIndex + 1, imageUrl);
                                }
                            }
                            row["lllegalfilepath"] = "";
                        }
                        //整改图片
                        if (exportTable.Columns.Contains("reformfilepath"))
                        {
                            int colIndex = exportTable.Columns.IndexOf("reformfilepath");
                            string reformfilepath = row["reformfilepath"].ToString();
                            if (!string.IsNullOrEmpty(reformfilepath))
                            {
                                string imageUrl = System.Web.HttpContext.Current.Server.MapPath(reformfilepath);
                                if (System.IO.File.Exists(imageUrl))
                                {
                                    sheet.Pictures.Add(2 + picnum, colIndex, 3 + picnum, colIndex + 1, imageUrl);
                                }
                            }
                            row["reformfilepath"] = "";
                        }
                        picnum++;
                    }
                }
                if (!string.IsNullOrEmpty(mode))
                {
                    string tempSavePath = Server.MapPath("~/Resource/Temp/") + fielname;
                    wb.Save(tempSavePath);
                    string url = "../../Utility/DownloadFile?filePath=~/Resource/Temp/" + fielname + "&speed=10240000&newFileName=" + fielname;
                    return Redirect(url);
                }
                else
                {
                    HttpResponse resp = System.Web.HttpContext.Current.Response;
                    resp.Clear();
                    resp.Buffer = true;
                    wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("导出成功!");
        }
        #endregion

        #region 违章考核通知单导出模板
        /// <summary>
        /// 违章考核通知单导出模板
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportExamReport(string keyValue)
        {
            //违章基本信息
            DataTable lllegaldt = lllegalregisterbll.GetLllegalModel(keyValue);

            List<LllegalPunishEntity> list = lllegalpunishbll.GetListByLllegalId(keyValue, "");

            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户

            string fileName = "违章考核通知单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/违章考核通知单导出模板.doc");

            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);

            DataTable dt = new DataTable();
            dt.Columns.Add("LllegalTime");
            dt.Columns.Add("LllegalAddress");
            dt.Columns.Add("LllegalPeople");
            dt.Columns.Add("LllegalDescribe");
            dt.Columns.Add("LllegalContent");
            dt.Columns.Add("ReformRequire");
            dt.Columns.Add("ApproveDept");
            dt.Columns.Add("CurDate");
            HttpResponse resp = System.Web.HttpContext.Current.Response;

            DataRow row = dt.NewRow();
            string lllegalpic = lllegaldt.Rows[0]["lllegalpic"].ToString();
            IEnumerable<FileInfoEntity> reformfile = fileinfobll.GetImageListByObject(lllegalpic);
            if (reformfile.Count() > 0)
            {
                DocumentBuilder builder = new DocumentBuilder(doc);
                foreach (FileInfoEntity fentity in reformfile)
                {
                    string url = AppDomain.CurrentDomain.BaseDirectory + fentity.FilePath.Substring(1).Replace("~/", "");
                    if (System.IO.File.Exists(url))
                    {
                        builder.MoveToBookmark("LllegalPic");
                        builder.InsertImage(url, Aspose.Words.Drawing.RelativeHorizontalPosition.Margin, 1, Aspose.Words.Drawing.RelativeVerticalPosition.Margin, 320, 50, 50, Aspose.Words.Drawing.WrapType.Inline);
                    }
                }
            }
            string lllegaltime = lllegaldt.Rows[0]["lllegaltime"].ToString();
            row["LllegalTime"] = !string.IsNullOrEmpty(lllegaltime) ? Convert.ToDateTime(lllegaltime).ToString("yyyy年MM月dd日") : "";
            row["LllegalAddress"] = lllegaldt.Rows[0]["lllegaladdress"].ToString();
            row["LllegalPeople"] = lllegaldt.Rows[0]["lllegalperson"].ToString();
            row["LllegalDescribe"] = lllegaldt.Rows[0]["lllegaldescribe"].ToString();//EconomicsPunish LllegalPoint
            string lllegalcontent = string.Empty;
            if (list.Count() > 0)
            {
                foreach (LllegalPunishEntity entity in list)
                {
                    if (entity.ASSESSOBJECT.Contains("单位"))
                    {
                        lllegalcontent += entity.PERSONINCHARGENAME + "经济处罚" + entity.ECONOMICSPUNISH.ToString() + "元,EHS绩效考核" + entity.PERFORMANCEPOINT.ToString() +
                            "分,违章扣分" + entity.LLLEGALPOINT.ToString() + "分;";
                    }
                    else  //人员 
                    {
                        lllegalcontent += entity.PERSONINCHARGENAME + "经济处罚" + entity.ECONOMICSPUNISH.ToString() + "元,EHS绩效考核" + entity.PERFORMANCEPOINT.ToString() +
                            "分,违章扣分" + entity.LLLEGALPOINT.ToString() + "分,教育培训" + entity.EDUCATION.ToString() + "学时,待岗" + entity.AWAITJOB + "月;";
                    }
                }
            }
            else
            {
                lllegalcontent = "暂未发现考核信息.";
            }
            row["LllegalContent"] = lllegalcontent;
            row["ReformRequire"] = lllegaldt.Rows[0]["reformrequire"].ToString();
            row["ApproveDept"] = lllegaldt.Rows[0]["approvedeptname"].ToString();
            row["CurDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            dt.Rows.Add(row);

            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));

            return Success("导出成功!");
        }
        #endregion

        #region 违章整改通知单导出模板
        /// <summary>
        /// 违章整改通知单导出模板
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportReformReport(string keyValue)
        {
            //违章基本信息
            DataTable lllegaldt = lllegalregisterbll.GetLllegalModel(keyValue);

            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户

            string fileName = "违章整改通知单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/违章整改通知单导出模板.doc");

            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);

            DataTable dt = new DataTable();
            dt.Columns.Add("ReformYear");
            dt.Columns.Add("ReformMonth");
            dt.Columns.Add("ReformDate");
            dt.Columns.Add("LllegalAddress");
            dt.Columns.Add("LllegalPeople");
            dt.Columns.Add("LllegalDescribe");
            dt.Columns.Add("ReformRequire");
            dt.Columns.Add("ApproveDept");
            dt.Columns.Add("CurDate");
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            DataRow row = dt.NewRow();
            string lllegaltime = lllegaldt.Rows[0]["lllegaltime"].ToString();
            string year = string.Empty;
            string month = string.Empty;
            string date = string.Empty;
            string lllegalpic = lllegaldt.Rows[0]["lllegalpic"].ToString();
            if (!string.IsNullOrEmpty(lllegaltime))
            {
                year = Convert.ToDateTime(lllegaltime).Year.ToString();
                month = Convert.ToDateTime(lllegaltime).Month.ToString();
                date = Convert.ToDateTime(lllegaltime).Day.ToString();
            }
            IEnumerable<FileInfoEntity> reformfile = fileinfobll.GetImageListByObject(lllegalpic);
            if (reformfile.Count() > 0)
            {
                DocumentBuilder builder = new DocumentBuilder(doc);
                foreach (FileInfoEntity fentity in reformfile)
                {
                    string url = AppDomain.CurrentDomain.BaseDirectory + fentity.FilePath.Substring(1).Replace("~/", "");
                    if (System.IO.File.Exists(url))
                    {
                        builder.MoveToBookmark("LllegalPic");
                        builder.InsertImage(url, Aspose.Words.Drawing.RelativeHorizontalPosition.Margin, 1, Aspose.Words.Drawing.RelativeVerticalPosition.Margin, 180, 60, 60, Aspose.Words.Drawing.WrapType.Inline);
                    }
                }
            }
            row["ReformYear"] = year;
            row["ReformMonth"] = month;
            row["ReformDate"] = date;
            row["LllegalAddress"] = lllegaldt.Rows[0]["lllegaladdress"].ToString();
            row["LllegalPeople"] = lllegaldt.Rows[0]["lllegalperson"].ToString();
            row["LllegalDescribe"] = lllegaldt.Rows[0]["lllegaldescribe"].ToString();
            row["ReformRequire"] = lllegaldt.Rows[0]["reformrequire"].ToString();
            row["ApproveDept"] = lllegaldt.Rows[0]["approvedeptname"].ToString();
            row["CurDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            dt.Rows.Add(row);

            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));

            return Success("导出成功!");
        }
        #endregion



        #region 导出个人违章档案
        /// <summary>
        /// 导出个人违章档案
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportPersonWzRecord(string userId)
        {
            try
            {
                //违章基本信息
                DataTable lllegaldt = lllegalregisterbll.GetLllegalForPersonRecord(userId);
                var userInfo = userbll.GetUserInfoEntity(userId);  //获取当前用户
                string fileName = "个人(反)违章档案_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/个人(反)违章档案.doc");
                Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                //用户信息
                DataTable dt = new DataTable();
                dt.Columns.Add("UserName");
                dt.Columns.Add("DutyName");
                dt.Columns.Add("DeptName");
                DataRow urow = dt.NewRow();
                urow["UserName"] = !string.IsNullOrEmpty(userInfo.RealName) ? userInfo.RealName : "";
                urow["DutyName"] = !string.IsNullOrEmpty(userInfo.DutyName) ? userInfo.DutyName : "";
                urow["DeptName"] = userInfo.DeptName;
                if (userInfo.RoleName.Contains("班组") || userInfo.RoleName.Contains("专业"))
                {
                    urow["DeptName"] = userInfo.ParentName + "/" + userInfo.DeptName;
                }
                dt.Rows.Add(urow);
                doc.MailMerge.Execute(dt);

                DataTable datadt = new DataTable("LllegalInfo");
                datadt.Columns.Add("SerialNumber");
                datadt.Columns.Add("LllegalDescribe");
                datadt.Columns.Add("LllegalTime");
                datadt.Columns.Add("EconomicsPunish");
                datadt.Columns.Add("LllegalPoint");
                datadt.Columns.Add("Money");
                datadt.Columns.Add("Points");

                int rownum = 1;
                foreach (DataRow wzrow in lllegaldt.Rows)
                {
                    DataRow row = datadt.NewRow();
                    row["SerialNumber"] = rownum;
                    row["LllegalDescribe"] = wzrow["lllegaldescribe"].ToString();
                    row["LllegalTime"] = wzrow["lllegaltime"].ToString();
                    row["EconomicsPunish"] = wzrow["economicspunish"].ToString();
                    row["LllegalPoint"] = wzrow["lllegalpoint"].ToString();
                    row["Money"] = wzrow["money"].ToString();
                    row["Points"] = wzrow["points"].ToString();
                    datadt.Rows.Add(row);
                    rownum++;
                }
                doc.MailMerge.ExecuteWithRegions(datadt);
                doc.MailMerge.DeleteFields();
                doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));

                return Success("导出成功!");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region 获取积分档案

        public List<OrganizeEntity> organizedata = new List<OrganizeEntity>();
        public List<DepartmentEntity> departmentdata = new List<DepartmentEntity>();

        #region 获取部门的树形结构
        /// <summary>
        /// 获取部门的树形结构
        /// </summary>
        /// <param name="EnCode"></param>
        /// <param name="ItemNameLike"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDataDeptWZ(string condition, string keyword)
        {
            var Year = Request["Year"] ?? "";
            Operator user = OperatorProvider.Provider.Current();
            List<TreeGridEntity> treeList = new List<TreeGridEntity>();
            var parentId = "0";
            var parentEntiy = departmentBLL.GetEntity(user.DeptId);
            if (parentEntiy != null)
                parentId = departmentBLL.GetEntity(user.DeptId).ParentId;

            if (user.IsSystem)
            {
                //organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).ToList();
                departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).ToList();
            }
            else
            {
                if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户") || user.DeptName.Contains("安环部"))
                {
                    //organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).Where(e => e.EnCode == user.OrganizeCode).ToList();
                    departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).Where(e => e.OrganizeId == user.OrganizeId).ToList();
                }
                else
                {
                    departmentdata = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode) || t.Description == "外包工程承包商" || t.SendDeptID == user.DeptId).OrderBy(x => x.SortCode).ToList();

                }
            }
            //异步加载数据
            //将函数加入委托
            //MyDelegate dele = new MyDelegate(AddOrg);
            ////添加参数
            //var result = dele.BeginInvoke(Year, null, null);
            //treeList.AddRange(dele.EndInvoke(result));
            MyDelegate dele = new MyDelegate(AddDept);
            var result = dele.BeginInvoke(Year, null, null);
            treeList.AddRange(dele.EndInvoke(result));

            var json = treeList.TreeJson(parentId);
            //返回数据
            return Content(json);

        }
        #endregion

        #region 违章积分档案(单位) 

        /// <summary>
        /// 违章积分档案(单位) 
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDataDeptWZY(string condition, string keyword)
        {

            Operator user = OperatorProvider.Provider.Current();
            //获取部门分数
            decimal deptScore = 12; //默认初始
            var dataitem = dataitemdetailbll.GetEntityByItemName("LllegalDeptPoint");
            if (null != dataitem)
            {
                if (!string.IsNullOrEmpty(dataitem.ItemValue))
                {
                    deptScore = Convert.ToDecimal(dataitem.ItemValue);//获取部门总分
                }
            }

            List<TreeGridEntity> treeList = new List<TreeGridEntity>();
            var parentId = "0";
            var parentEntiy = departmentBLL.GetEntity(user.DeptId);
            if (parentEntiy != null)
                parentId = departmentBLL.GetEntity(user.DeptId).ParentId;

            string yearStr = string.Empty;
            string Year = Request["Year"] ?? "";
            if (!string.IsNullOrEmpty(Year))
            {
                yearStr = string.Format(@" and to_char(lllegaltime, 'yyyy')='{0}'", Year);
            }
            var sql = "";
            var where = " 1=1 ";
            var order = " order by SortCode asc";
            if (!user.IsSystem)
            {
                if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户") || user.DeptName.Contains("安环部"))
                {
                    where += string.Format(" and OrganizeId='{0}'", user.OrganizeId);
                }
                else
                {
                    where += string.Format(" and OrganizeId='{0}' and (encode like '{1}%' or Description='外包工程承包商' or SendDeptID='{2}')", user.OrganizeId, user.DeptCode, user.DeptId);
                }
            }
            sql += string.Format(@"select SortCode as Sort,DepartmentId,ParentId,OrganizeId,FullName,EnCode,HasChild ,
                                  (select count(1) as Num from v_lllegalassesforperson where DepartmentId=d.departmentid {0}) as PersonWZNum,
                                  (select nvl(sum(lllegalpoint),0) from  v_lllegalassesforperson where departmentid=d.departmentid {0}) as PersonWZScore,
                                  (select nvl(sum(lllegalpoint),0) from v_lllegalassesfordepart where DepartmentId=d.departmentid {0}) as DeptWZScore,
                                  {1} DeptScore  from ( select SendDeptID,Description,departmentid,organizeid,parentid,encode,fullname,sortcode ,haschild,1 serialnumber from base_department where nature !='承包商'  and nature !='分包商'  and fullname !='外包工程承包商' 
                                    union  select SendDeptID,Description,'cx100' departmentid, organizeid, parentid,encode,'长协外包单位' fullname,sortcode,haschild,2 serialnumber  from base_department where fullname ='外包工程承包商'
                                    union  select SendDeptID,Description,departmentid,organizeid, 'cx100' parentid,encode,fullname,sortcode,haschild,3 serialnumber  from base_department where nature ='承包商' and depttype ='长协'
                                    union  select SendDeptID,Description,'ls100' departmentid,organizeid, parentid,encode,'临时外包单位' fullname,sortcode,haschild,4 serialnumber  from base_department where fullname ='外包工程承包商'
                                    union  select SendDeptID,Description,departmentid,organizeid, 'ls100' parentid,encode,fullname,sortcode,haschild,5 serialnumber  from base_department where nature ='承包商' and depttype ='临时'  order by serialnumber asc ,sortcode asc ,encode) d where {2} {3} ", yearStr, deptScore, where, order);
            DataTable dt = hazardsourcebll.FindTableBySql(sql);
            foreach (DataRow dr in dt.Rows)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = dt.Select("parentid='" + dr["DepartmentId"].ToString() + "'").Count() == 0 ? false : true;
                var item = new DepartmentEntity()
                {
                    DepartmentId = dr["DepartmentId"].ToString(),
                    OrganizeId = dr["OrganizeId"].ToString(),
                    FullName = dr["FullName"].ToString(),
                    EnCode = dr["EnCode"].ToString(),
                    HasChild = hasChildren.ToString()
                };

                tree.id = dr["DepartmentId"].ToString();
                if (dr["ParentId"].ToString() == "0")
                {
                    tree.parentId = "0";
                }
                else
                {
                    tree.parentId = dr["ParentId"].ToString();
                }
                tree.expanded = true;
                tree.hasChildren = hasChildren;
                string itemJson = item.ToJson();
                itemJson = itemJson.Insert(1, "\"Sort\":\"Department\",");
                itemJson = itemJson.Insert(1, "\"PersonWZNum\":\"" + dr["PersonWZNum"].ToString() + "\",");
                itemJson = itemJson.Insert(1, "\"PersonWZScore\":\"" + dr["PersonWZScore"].ToString() + "\",");
                itemJson = itemJson.Insert(1, "\"DeptWZScore\":\"" + dr["DeptWZScore"].ToString() + "\",");
                itemJson = itemJson.Insert(1, "\"DeptScore\":\"" + dr["DeptScore"].ToString() + "\",");
                tree.entityJson = itemJson;
                treeList.Add(tree);
            }

            var json = treeList.TreeJson(parentId);
            //返回数据
            return Content(json);
        }
        #endregion

        #region 获取人员违章数据
        /// <summary>
        /// 获取人员违章数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetDataPersonWZY(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            //获取个人分数
            decimal personScore = 12; //默认初始
            var dataitem = dataitemdetailbll.GetEntityByItemName("LllegalPointInitValue"); // 个人默认违章积分
            if (null != dataitem)
            {
                if (!string.IsNullOrEmpty(dataitem.ItemValue))
                {
                    personScore = Convert.ToDecimal(dataitem.ItemValue);//获取个人默认违章积分 
                }
            }
            string dwhere = string.Empty;
            string cwhere = string.Empty;
            string Year = Request["Year"] ?? "";
            if (!string.IsNullOrEmpty(Year))
            {
                dwhere += string.Format(" and  to_char(b.lllegaltime, 'yyyy')='{0}'", Year);
                cwhere += string.Format(" and  to_char(a.createdate, 'yyyy')='{0}'", Year);
            }
            string twhere = string.Empty;
            string _parentId = Request["_parentId"] ?? "";
            if (!string.IsNullOrEmpty(_parentId))
            {
                twhere = string.Format(" and a.deptcode like '{0}%'", _parentId);
            }
            queryJson = queryJson ?? "";
            pagination.p_fields = "userid,realname,account,personwznum,personwzscore,awardnum,awardscore,persontotal,departmentid,deptcode,deptname,sortcode,deptsort";
           
            pagination.p_tablename = string.Format(@"（
                                                        with lllegalpointrecoverdetail as ( select count(1) pnum ,recoveruserid userid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate ),
                                                        lllegalasses as ( 
                                                            select  count(a.id) PersonWZNum, sum(nvl(a.lllegalpoint,0)) PersonWZScore ,a.personinchargeid userid from bis_lllegalpunish a left join bis_lllegalregister b on a.lllegalid = b.id  left join lllegalpointrecoverdetail  c on a.personinchargeid = c.userid  where  a.personinchargeid is not null and  b.flowstate in (select itemname from v_yesqrwzstatus) and (nvl(c.pnum,0)=0  or (nvl(c.pnum,0)>0  and  b.createdate > c.createdate ))  {0}  group by a.personinchargeid
                                                         ),
                                                        lllegalawarddetail as (
                                                            select count(a.id) AwardNum, a.userid,sum(nvl(a.points,0)) points from bis_lllegalawarddetail a inner join bis_lllegalregister b on  a.lllegalid = b.id where b.flowstate in (select itemname from v_yesqrwzstatus)  {0}  group by a.userid 
                                                        ),
                                                        lllegalreward as ( 
                                                            select  a.createuserid userid, sum(b.lllegalpoint) repoints from  bis_lllegalregister a  left join  bis_lllegalreward b on a.id = b.lllegalid where  a.flowstate ='流程结束'   and b.status ='已确认'  {3}  group by a.createuserid 
                                                        ) 
                                                        select a.deptcode ,a.deptname,a.realname,a.account,a.userid,a.departmentid ,a.sortcode,a.deptsort,nvl(b.PersonWZNum,0) PersonWZNum,nvl(b.PersonWZScore,0) PersonWZScore,({1} - nvl(b.PersonWZScore,0) + nvl(c.points,0) + nvl(d.repoints,0))  PersonTotal, nvl(c.AwardNum,0) AwardNum, (nvl(c.points,0) + nvl(d.repoints,0)) AwardScore  from v_userinfo a  left join lllegalasses b on a.userid = b.userid  left join lllegalawarddetail c on a.userid = c.userid left join lllegalreward d on a.userid = d.userid  where 1=1 {2} 
                                                      ) t ", dwhere, personScore, twhere, cwhere);
            pagination.conditionJson = "1=1";


            var watch = CommonHelper.TimerStart();
            var data = hazardsourcebll.GetPageList(pagination, queryJson);
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
        #endregion

        //定义一个委托
        public delegate List<TreeGridEntity> MyDelegate(string Year);


        public List<TreeGridEntity> AddOrg(string Year)
        {
            List<TreeGridEntity> treeListO = new List<TreeGridEntity>();
            foreach (OrganizeEntity item in organizedata)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = organizedata.Count(t => t.ParentId == item.OrganizeId) == 0 ? false : true;
                if (hasChildren == false)
                {
                    hasChildren = departmentdata.Count(t => t.OrganizeId == item.OrganizeId) == 0 ? false : true;
                    //if (hasChildren == false)
                    //{
                    //    continue;
                    //}
                }
                tree.id = item.OrganizeId;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.expanded = true;
                string itemJson = item.ToJson();
                itemJson = itemJson.Insert(1, "\"DepartmentId\":\"" + item.OrganizeId + "\",");
                itemJson = itemJson.Insert(1, "\"Sort\":\"Organize\",");
                itemJson = itemJson.Insert(1, "\"DepartWZNum\":\"" + GetWZNum(item.OrganizeId, Year) + "\",");
                itemJson = itemJson.Insert(1, "\"DepartWZScore\":\"" + GetWZScore(item.OrganizeId, Year) + "\",");
                tree.entityJson = itemJson;
                treeListO.Add(tree);
            }
            return treeListO;
        }

        public List<TreeGridEntity> AddDept(object YearObj)
        {
            List<TreeGridEntity> treeListD = new List<TreeGridEntity>();
            var Year = YearObj.ToString();
            foreach (DepartmentEntity item in departmentdata)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = departmentdata.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                tree.id = item.DepartmentId;
                if (item.ParentId == "0")
                {
                    tree.parentId = "0";
                }
                else
                {
                    tree.parentId = item.ParentId;
                }
                item.HasChild = hasChildren.ToString();
                tree.expanded = true;
                tree.hasChildren = hasChildren;
                string itemJson = item.ToJson();
                itemJson = itemJson.Insert(1, "\"Sort\":\"Department\",");
                itemJson = itemJson.Insert(1, "\"DepartWZNum\":\"" + GetWZNum(item.DepartmentId, Year) + "\",");
                itemJson = itemJson.Insert(1, "\"DepartWZScore\":\"" + GetWZScore(item.DepartmentId, Year) + "\",");
                tree.entityJson = itemJson;
                treeListD.Add(tree);
            }
            return treeListD;
        }

        #region 获取部门的违章次数
        /// <summary>
        /// 获取部门的违章次数
        /// </summary>
        /// <param name="DepartmentId"></param>
        /// <param name="Year"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int GetWZNum(string DepartmentId, string Year)
        {
            string sql = "select count(1) as Num from v_lllegalassesforperson where DepartmentId='" + DepartmentId + "'";

            if (Year.Length > 0)
            {
                sql += " and to_char(lllegaltime, 'yyyy')='" + Year + "'";

            }
            var dt = hazardsourcebll.FindTableBySql(sql);
            return int.Parse(dt.Rows[0]["Num"].ToString());
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DepartmentId"></param>
        /// <returns></returns>
        public decimal GetWZScore(string DepartmentId, string Year)
        {

            string sql = "select lllegallevelname,Id,lllegalpoint from v_lllegalassesforperson where DepartmentId='" + DepartmentId + "'";
            if (Year.Length > 0)
            {
                sql += " and to_char(lllegaltime, 'yyyy')='" + Year + "'";

            }
            var dt = hazardsourcebll.FindTableBySql(sql);
            decimal score = 0;
            foreach (DataRow dr in dt.Rows)
            {
                score += decimal.Parse(dr["lllegalpoint"].ToString());

            }

            return score;
        }
        private DataTable GetCount()
        {
            DataTable dt = new DataTable();
            string sql = "";

            dt = hazardsourcebll.FindTableBySql(sql);

            return dt;
        }
        #endregion

        #region 人员违章信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetPersonWzInfo(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            //获取个人分数
            decimal personScore = 12; //默认初始
            var dataitem = dataitemdetailbll.GetEntityByItemName("LllegalPointInitValue"); // 个人默认违章积分
            if (null != dataitem)
            {
                if (!string.IsNullOrEmpty(dataitem.ItemValue))
                {
                    personScore = Convert.ToDecimal(dataitem.ItemValue);//获取个人默认违章积分 
                }
            }
            string deptid = Request["DeptId"] ?? "";

            string tWhere = string.Empty;
            string dWhere = string.Empty;
            string tempWhere = string.Empty;
            if (!string.IsNullOrEmpty(deptid))
            {
                tWhere = string.Format(" and d.departmentid='{0}'", deptid);
                dWhere = string.Format(" and a.departmentid='{0}'", deptid); 
            }
            string Year = Request["Year"] ?? "";
            if (!string.IsNullOrEmpty(Year))
            {
                tWhere += string.Format(" and  to_char(b.lllegaltime, 'yyyy')='{0}'", Year);
                tempWhere += string.Format(" and  to_char(a.createdate, 'yyyy')='{0}'", Year);
            }
            string mode = Request["mode"] ?? "";
            decimal recoverPoint = 4; //默认4分
            var recoveritem = dataitemdetailbll.GetEntityByItemName("LllegalRecoverPoint"); // 违章恢复积分查询条件
            if (null != recoveritem)
            {
                if (!string.IsNullOrEmpty(recoveritem.ItemValue))
                {
                    recoverPoint = Convert.ToDecimal(recoveritem.ItemValue);//获取个人默认违章积分 
                }
            }
            queryJson = queryJson ?? "";
            pagination.p_fields = "deptname,realname,wznum,points,lllegalpoint,personscore,userid,organizeid";

            pagination.p_tablename = string.Format(@"( with lllegalpointrecoverdetail as ( 
                                                              select count(1) pnum ,recoveruserid userid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate  
                                                          ),
                                                         lllegalasses as ( 
                                                              select count(a.id) wznum,sum(nvl(a.lllegalpoint,0)) lllegalpoint,a.personinchargeid userid  from bis_lllegalpunish a left join bis_lllegalregister b on a.lllegalid = b.id left join lllegalpointrecoverdetail  c on a.personinchargeid = c.userid  left join base_user d on a.personinchargeid = d.userid where a.personinchargeid is not null and  b.flowstate in (select itemname from v_yesqrwzstatus) and (nvl(c.pnum,0)=0  or (nvl(c.pnum,0)>0  and  b.createdate > c.createdate )) {1} group by a.personinchargeid
                                                          ),
                                                          lllegalawarddetail as (
                                                              select a.userid,sum(nvl(a.points,0)) points from bis_lllegalawarddetail a inner join bis_lllegalregister b on  a.lllegalid = b.id  left join base_user d on a.userid = d.userid where b.flowstate in (select itemname from v_yesqrwzstatus) {1} group by a.userid
                                                          ),
                                                          lllegalreward as ( 
                                                              select  a.createuserid userid, sum(b.lllegalpoint) repoints from  bis_lllegalregister a  left join  bis_lllegalreward b on a.id = b.lllegalid  where  a.flowstate ='流程结束'  and b.status ='已确认' {2}  group by a.createuserid 
                                                          )
                                                          select a.deptname,a.realname,nvl(b.wznum,0) wznum, nvl(b.lllegalpoint,0) lllegalpoint, ({0} - nvl(b.lllegalpoint,0) + nvl(c.points,0) + nvl(d.repoints,0))  personscore,(nvl(c.points,0) + nvl(d.repoints,0)) points,a.userid,a.organizeid  from lllegalasses b left join  v_userinfo a  on b.userid = a.userid  left join lllegalawarddetail c on a.userid = c.userid left join lllegalreward d on a.userid = d.userid  where 1=1 {3} 
                                                    ) t", personScore, tWhere, tempWhere, dWhere);

            pagination.conditionJson = "1=1";

            if (!string.IsNullOrEmpty(mode))
            {
                //积分恢复
                if (mode == "recover")
                {
                    pagination.conditionJson += string.Format(" and  personscore <={0}  ", recoverPoint);
                }
                else if (mode == "history")  //历史记录  恢复的违章人员数据
                {
                    pagination.p_tablename = string.Format(@"( with lllegalpointrecoverdetail as ( 
                                                               select count(1) pnum ,recoveruserid userid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate  
                                                              ),
                                                             lllegalasses as ( 
                                                                  select count(a.id) wznum,sum(nvl(a.lllegalpoint,0)) lllegalpoint,a.personinchargeid userid  from bis_lllegalpunish a left join bis_lllegalregister b on a.lllegalid = b.id left join lllegalpointrecoverdetail  c on a.personinchargeid = c.userid  left join base_user d on a.personinchargeid = d.userid where a.personinchargeid is not null and  b.flowstate in (select itemname from v_yesqrwzstatus) and  nvl(c.pnum,0)>0  and  b.createdate < c.createdate {1} group by a.personinchargeid
                                                              ),
                                                              lllegalawarddetail as (
                                                                  select a.userid,sum(nvl(a.points,0)) points from bis_lllegalawarddetail a inner join bis_lllegalregister b on  a.lllegalid = b.id  left join base_user d on a.userid = d.userid where b.flowstate in (select itemname from v_yesqrwzstatus) {1} group by a.userid
                                                              ),
                                                              lllegalreward as ( 
                                                                  select  a.createuserid userid, sum(b.lllegalpoint) repoints from  bis_lllegalregister a  left join  bis_lllegalreward b on a.id = b.lllegalid  where  a.flowstate ='流程结束'  and b.status ='已确认' {2}  group by a.createuserid 
                                                              )
                                                              select a.deptname,a.realname,nvl(b.wznum,0) wznum, nvl(b.lllegalpoint,0) lllegalpoint,({0} - nvl(b.lllegalpoint,0) + nvl(c.points,0) + nvl(d.repoints,0))  personscore,(nvl(c.points,0) + nvl(d.repoints,0)) points,a.userid,a.organizeid  from lllegalasses b left join  v_userinfo a  on b.userid = a.userid  left join lllegalawarddetail c on a.userid = c.userid left join lllegalreward d on a.userid = d.userid  where 1=1 {3} and a.userid in  (select distinct recoveruserid from v_lllegalpointrecoverdetail)
                                                    ) t", personScore, tWhere, tempWhere, dWhere);
                }
                else if (mode == "underEight") // 违章积分低于8分的人员
                {
                    pagination.p_tablename = string.Format(@"( with lllegalpointrecoverdetail as ( 
                                                              select count(1) pnum ,recoveruserid userid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate  
                                                          ),
                                                         lllegalasses as ( 
                                                              select count(a.id) wznum,sum(nvl(a.lllegalpoint,0)) lllegalpoint,a.personinchargeid userid  from bis_lllegalpunish a left join bis_lllegalregister b on a.lllegalid = b.id left join lllegalpointrecoverdetail  c on a.personinchargeid = c.userid  left join base_user d on a.personinchargeid = d.userid where a.personinchargeid is not null and  b.flowstate in (select itemname from v_yesqrwzstatus) and (nvl(c.pnum,0)=0  or (nvl(c.pnum,0)>0  and  b.createdate > c.createdate ))  and d.organizeid='{1}' and  to_char(b.lllegaltime,'yyyy')='{2}'  group by a.personinchargeid
                                                          ),
                                                          lllegalawarddetail as (
                                                              select a.userid,sum(nvl(a.points,0)) points from bis_lllegalawarddetail a inner join bis_lllegalregister b on  a.lllegalid = b.id  where b.flowstate in (select itemname from v_yesqrwzstatus) and a.deptid in (select distinct departmentid from  base_department where organizeid= '{1}') and  to_char(b.lllegaltime,'yyyy')='{2}' group by a.userid
                                                          ),
                                                          lllegalreward as ( 
                                                              select  a.createuserid userid, sum(b.lllegalpoint) repoints from  bis_lllegalregister a  left join  bis_lllegalreward b on a.id = b.lllegalid  where  a.flowstate ='流程结束'  and b.status ='已确认' and  to_char(a.createdate, 'yyyy')='{2}'  group by a.createuserid 
                                                          )
                                                          select a.deptname,a.realname,nvl(b.wznum,0) wznum, nvl(b.lllegalpoint,0) lllegalpoint, ({0} - nvl(b.lllegalpoint,0) + nvl(c.points,0) + nvl(d.repoints,0))  personscore,(nvl(c.points,0) + nvl(d.repoints,0)) points,a.userid,a.organizeid  from lllegalasses b left join  v_userinfo a  on b.userid = a.userid  left join lllegalawarddetail c on a.userid = c.userid left join lllegalreward d on a.userid = d.userid  where 1=1 
                                                    ) t", personScore, user.OrganizeId, DateTime.Now.Year.ToString());

                    pagination.conditionJson += string.Format(" and  personscore <=8  ");
                }
            }
            var watch = CommonHelper.TimerStart();
            var data = hazardsourcebll.GetPageList(pagination, queryJson);
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
        #endregion

        #region 人员违章信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetXssWzPersonInfo(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            string strWhere = string.Empty;
            decimal basePoint = 12; //基础分数
            var lllegalPoint = dataitemdetailbll.GetDataItemListByItemCode("'LllegalTrainPointSetting'");
            if (lllegalPoint.Count() > 0)
            {
                var LllegalPointInitValue = lllegalPoint.Where(p => p.ItemName == "LllegalPointInitValue").FirstOrDefault();
                if (null != LllegalPointInitValue)
                {
                    basePoint = Convert.ToDecimal(LllegalPointInitValue.ItemValue);
                }
            }

            //部门
            if (!string.IsNullOrEmpty(queryParam["Code"].ToString()))
            {
                strWhere += string.Format(@" and c.encode  like '{0}%'", queryParam["Code"].ToString());
            }
            //年度
            if (!string.IsNullOrEmpty(queryParam["TimeScope"].ToString()))
            {
                strWhere += string.Format(@" and a.lllegalid in (select id from v_lllegalbaseinfo where to_char(lllegaltime, 'yyyy')='{0}' and createuserorgcode ='{1}')", queryParam["TimeScope"].ToString(), user.OrganizeCode);
            }
            //公司级  厂级
            if (user.RoleName.Contains("公司级") || user.RoleName.Contains("厂级"))
            {
                strWhere += @" and  a.lllegalid  in (select distinct objectid from v_xsslllegalpointsdata where  rolename  like '%公司级%' or  rolename like '%厂级%')";
            }
            //厂级
            if (user.RoleName.Contains("部门级") && !user.RoleName.Contains("厂级"))
            {
                strWhere += string.Format(@" and  a.lllegalid  in (select distinct objectid from v_xsslllegalpointsdata where  rolename  like '%部门级%' and   rolename  not like '%厂级%'  and  encode ='{0}')", user.DeptCode);
            }
            //班组级
            if (user.RoleName.Contains("班组级"))
            {
                strWhere += string.Format(@" and  a.lllegalid in (select distinct objectid from v_xsslllegalpointsdata where  rolename  like '%班组级%' and  encode ='{0}')", user.DeptCode);
            }
            //表名
            string tablename = string.Format(@"(select  sum(nvl(a.lllegalpoint,0)) lllegalpoint, a.personinchargeid userid ,b.realname ,c.fullname  deptname  ,c.encode,count(a.lllegalid) wznum ,({0}- sum(nvl(a.lllegalpoint,0))) lllegaljf   from bis_lllegalpunish a 
                                            left join base_user  b on a.personinchargeid = b.userid
                                            left join base_department c on b.departmentid = c.departmentid
                                            left join base_user d on a.createuserid = d.userid  where a.assessobject like '%人员%' and a.personinchargeid is not null and c.departmentid is not null  {1}
                                            group by a.personinchargeid ,b.realname,c.fullname,c.encode) a", basePoint, strWhere);
            pagination.p_fields = "userid,deptname,realname,wznum,lllegalpoint,lllegaljf";
            pagination.p_tablename = tablename;
            pagination.conditionJson = "1=1  and lllegalpoint>0 ";

            var watch = CommonHelper.TimerStart();
            var data = lllegalregisterbll.GetGeneralQuery(pagination);
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
        #endregion

        #region 数据导出
        /// <summary>
        /// 未遂事件报告与调查处理
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "数据导出")]
        public ActionResult Export()
        {
            var Year = Request["Year"] ?? "";
            Operator user = OperatorProvider.Provider.Current();
            List<OrganizeEntity> organizedata = new List<OrganizeEntity>();
            List<DepartmentEntity> departmentdata = new List<DepartmentEntity>();
            var parentId = "0";
            if (user.IsSystem)
            {
                departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).ToList();
            }
            else
            {
                if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户") || user.DeptName.Contains("安环部"))
                {
                    departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).Where(e => e.EnCode.Substring(0, user.DeptCode.Length) == user.DeptCode).ToList();
                }
                else
                {
                    departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).Where(e => e.EnCode.Length >= user.DeptCode.Length && e.EnCode.Substring(0, user.DeptCode.Length) == user.DeptCode).ToList();
                    parentId = user.ParentId;
                }
            }
            var treeList = new List<TreeGridEntity>();

            foreach (DepartmentEntity item in departmentdata)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = departmentdata.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                tree.id = item.DepartmentId;
                if (item.ParentId == "0")
                {
                    tree.parentId = item.OrganizeId;
                }
                else
                {
                    tree.parentId = item.ParentId;
                }
                item.HasChild = hasChildren.ToString();
                tree.expanded = true;
                tree.hasChildren = hasChildren;
                string itemJson = item.ToJson();
                itemJson = itemJson.Insert(1, "\"Sort\":\"Department\",");
                var Ids = "";
                itemJson = itemJson.Insert(1, "\"DepartWZNum\":\"" + GetWZNum(item.DepartmentId, Year) + "\",");

                itemJson = itemJson.Insert(1, "\"DepartWZScore\":\"" + GetWZScore(item.DepartmentId, Year) + "\",");
                itemJson = itemJson.Insert(1, "\"Ids\":\"" + Ids + "\",");
                tree.entityJson = itemJson;
                treeList.Add(tree);
            }


            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "积分档案";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "积分档案.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "fullname".ToLower(), ExcelColumn = "部门名称" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DepartWZNum".ToLower(), ExcelColumn = "违章次数" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DepartWZScore".ToLower(), ExcelColumn = "违章扣分" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DepartWZJF".ToLower(), ExcelColumn = "违章积分" });
            excelconfig.ColumnEntity = listColumnEntity;
            List<TemplateMode> list = new List<TemplateMode>();
            var dt = new DataTable();
            DataColumn dc = dt.Columns.Add("fullname", Type.GetType("System.String"));
            dc = dt.Columns.Add("departwznum", Type.GetType("System.String"));
            dc = dt.Columns.Add("departwzscore", Type.GetType("System.String"));
            dc = dt.Columns.Add("departwzjf", Type.GetType("System.String"));
            foreach (var item in treeList)
            {
                var entity = JsonConvert.DeserializeObject<DataEntity>(item.entityJson);
                DataRow dr = dt.NewRow();
                dr["fullname"] = entity.FullName;
                dr["departwznum"] = entity.DepartWZNum;
                dr["departwzscore"] = entity.DepartWZScore;
                dr["departwzjf"] = 12 - decimal.Parse(entity.DepartWZScore.ToString());
                dt.Rows.Add(dr);
            }
            //调用导出方法
            ExcelHelper.ExcelDownload(dt, excelconfig);
            return Success("导出成功。");
        }
        public class DataEntity
        {
            public string FullName { get; set; }
            public string DepartWZNum { get; set; }
            public string DepartWZScore { get; set; }
            public string DepartWZJF { get; set; }
        }


        #endregion

        #region 获取人员违章积分提醒数据
        /// <summary>
        /// 获取人员违章积分提醒数据
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public ActionResult GetLllegalPointRemindDataJson()
        {
            Operator curUser = OperatorProvider.Provider.Current();
            //人员违章积分提醒设置
            string itemCode = "'LllegalPointRemindSetting'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);

            decimal remindValue = 0;

            var remindList = itemlist.Where(p => p.ItemName == curUser.OrganizeCode).ToList();

            if (remindList.Count() > 0)
            {
                remindValue = decimal.Parse(remindList.FirstOrDefault().ItemValue);
            }
            //返回值
            var josnData = new
            {
                RemindValue = remindValue
            };

            return Content(josnData.ToJson());
        }
        #endregion

        #region  保存表单（新增、修改）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveRemindForm(string keyValue)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            decimal RemindValue = decimal.Parse(Request.Form["RemindValue"].ToString());
            //人员违章积分提醒设置
            string itemCode = "'LllegalPointRemindSetting'";

            DataItemDetailEntity entity = new DataItemDetailEntity();

            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);

            var itemdata = new DataItemBLL().GetEntityByCode("LllegalPointRemindSetting");

            var remindList = itemlist.Where(p => p.ItemName == curUser.OrganizeCode).ToList();

            if (remindList.Count() > 0)
            {
                var model = remindList.FirstOrDefault();
                entity.ItemDetailId = model.ItemDetailId;
                entity.ItemCode = model.ItemCode;
                entity.ItemName = model.ItemName;
                entity.ItemValue = RemindValue.ToString();
                entity.ItemId = model.ItemId;
                entity.Description = model.Description;
                entity.EnabledMark = model.EnabledMark;
                entity.ParentId = model.ParentId;
                entity.SortCode = model.SortCode;
            }
            else
            {
                entity.ItemName = curUser.OrganizeCode;
                entity.ItemValue = RemindValue.ToString();
                entity.ItemCode = "WZJFLJZ";
                entity.Description = curUser.OrganizeName + "违章记分临界值";
                entity.ItemId = itemdata.ItemId;
                entity.ParentId = "0";
                entity.EnabledMark = 1;
            }
            dataitemdetailbll.SaveForm(entity.ItemDetailId, entity);
            return Success("操作成功!");
        }
        #endregion

        #region 日常检查导出
        /// <summary>
        /// 日常检查导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult DailyExportData(string queryJson, string mtype, string mode = "")
        {
            try
            {

                // 详细列表内容
                string fielname = DateTime.Now.Year.ToString() + "安全日督查违章数据((统计用).xls";
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/安全日督查违章数据((统计用).xls"));


                SaftyCheckDataRecordBLL srbll = new SaftyCheckDataRecordBLL();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "t.ID";
                pagination.p_fields = "CheckBeginTime,checkendtime,CheckDataRecordName,CheckLevel,CheckMan,'未开始检查' SolveCount,'' count,t.SolvePerson,'' count1";
                pagination.conditionJson = "1=1";
                pagination.sidx = "CreateDate desc,id";
                pagination.sord = "desc";
                var user = OperatorProvider.Provider.Current();
                string where1 = "";
                string arg = user.DeptCode;
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级") || user.RoleName.Contains("集团") || user.RoleName.Contains("省级用户"))
                    {
                        pagination.conditionJson += string.Format(" and (t1.id is not null or createuserdeptcode in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid))", user.OrganizeCode);
                    }
                    else
                    {
                        arg = user.DeptCode;
                        where1 = string.Format(" or senddeptid='{0}'", user.DeptId);
                        pagination.conditionJson += string.Format("  and (t1.id is not null or createuserdeptcode='{0}')", arg);

                    }
                    pagination.conditionJson += string.Format(" and checkeddepartid is null and createuserdeptcode in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", user.OrganizeCode);
                    pagination.p_tablename = string.Format(@"bis_saftycheckdatarecord t left join(select id from (select id,(','||checkmanid||',') as checkmanid  from  bis_saftycheckdatarecord) a 
                                                             left join (
                                                                 select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where deptcode like '{0}%' {1} )
                                                             ) b on a.checkmanid  like '%'||b.account||'%' where account is not null group by id)t1
                                                             on t.ID=t1.id ", arg, where1);
                }
                else
                {
                    pagination.p_tablename = "bis_saftycheckdatarecord t";
                }
                //获取安全检查项
                DataTable dt = srbll.ExportData(pagination, queryJson);

                List<string> ids = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    ids.Add(row["id"].ToString());
                }
                //违章考核总表
                DataTable resultDt = lllegalregisterbll.GetLllegalBySafetyCheckIds(ids, 0);

                //外委违章统计 第二张表
                Aspose.Cells.Worksheet sheet1 = wb.Worksheets[1] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell1 = sheet1.Cells[0, 0];
                cell1.PutValue(DateTime.Now.Year.ToString() + "年各部门违章统计"); //标题
                cell1.Style.Pattern = BackgroundType.Solid;
                cell1.Style.Font.Size = 14;
                cell1.Style.Font.Color = Color.Black;

                //外委违章统计 第三张表
                Aspose.Cells.Worksheet sheet2 = wb.Worksheets[2] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell2 = sheet2.Cells[0, 0];
                cell2.PutValue(DateTime.Now.Year.ToString() + "年外委单位违章统计"); //标题
                cell2.Style.Pattern = BackgroundType.Solid;
                cell2.Style.Font.Size = 14;
                cell2.Style.Font.Color = Color.Black;

                List<string> haveDept = new List<string>();

                int wwIndex = 0;
                foreach (DataRow row in resultDt.Rows)
                {
                    if (!string.IsNullOrEmpty(row["expteam"].ToString()))
                    {
                        if (!haveDept.Contains(row["expteamcode"].ToString()))
                        {
                            haveDept.Add(row["expteamcode"].ToString());

                            Aspose.Cells.Cell wwcell = sheet2.Cells[3 + wwIndex, 1];
                            wwcell.PutValue(row["expteam"].ToString());
                            Aspose.Cells.Cell zrbmcell = sheet2.Cells[3 + wwIndex, 2];
                            zrbmcell.PutValue(row["glbmname"].ToString());

                            wwIndex++;
                        }
                    }
                }

                //第一张表
                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell = sheet.Cells[0, 0];
                cell.PutValue(DateTime.Now.Year.ToString() + "年安全日督查汇总表"); //标题
                cell.Style.Pattern = BackgroundType.Solid;
                cell.Style.Font.Size = 14;
                cell.Style.Font.Color = Color.Black;

                resultDt.Columns.Remove("lllegalteam");
                resultDt.Columns.Remove("lllegaldepart");
                resultDt.Columns.Remove("lllegalteamcode");
                resultDt.Columns.Remove("lllegaldepartcode");
                resultDt.Columns.Remove("expteamcode");
                resultDt.Columns.Remove("expdepartcode");
                resultDt.Columns.Remove("wzdwnature");
                resultDt.Columns.Remove("wzzrdwnature");
                resultDt.Columns.Remove("wwparent");
                resultDt.Columns.Remove("bmparent");
                resultDt.Columns.Remove("glbmname");
                resultDt.Columns.Remove("id");
                //先导入数据
                sheet.Cells.ImportDataTable(resultDt, false, 2, 0);

                if (!string.IsNullOrEmpty(mode))
                {
                    string tempSavePath = Server.MapPath("~/Resource/Temp/") + fielname;
                    wb.Save(tempSavePath);
                    string url = "../../Utility/DownloadFile?filePath=~/Resource/Temp/" + fielname + "&speed=10240000&newFileName=" + fielname;
                    return Redirect(url);
                }
                else
                {
                    HttpResponse resp = System.Web.HttpContext.Current.Response;
                    resp.Clear();
                    resp.Buffer = true;
                    wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                }

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("导出成功。");
        }
        #endregion
    }
}
