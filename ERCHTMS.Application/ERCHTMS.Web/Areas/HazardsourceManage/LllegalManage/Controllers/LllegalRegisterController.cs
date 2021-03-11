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
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); // 考核信息对象
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private HazardsourceBLL hazardsourcebll = new HazardsourceBLL();

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
            //违章编码
            string LllegalNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff").ToString();
            //考核方式 违章类型 违章等级 流程状态
            string itemCode = "'ExamineWay','LllegalType','LllegalLevel','FlowState'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            //公司级用户
            if (userbll.HaveRoleListByKey(CreateUser.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
            {
                CreateUser.DeptCode = CreateUser.OrganizeCode;
                CreateUser.DeptName = CreateUser.OrganizeName;
            }

            string mark = userbll.GetSafetyAndDeviceDept(CreateUser); //用于标记可立即整改违章

            string isPrincipal = userbll.HaveRoleListByKey(CreateUser.UserId, dataitemdetailbll.GetItemValue("PrincipalUser")).Rows.Count > 0 ? "1" : "";

            string isEpiboly = userbll.HaveRoleListByKey(CreateUser.UserId, dataitemdetailbll.GetItemValue("EpibolyUser")).Rows.Count > 0 ? "1" : "";  //承包商
            //返回值
            var josnData = new
            {
                CreateUser = CreateUser.UserName,
                User = CreateUser,  //用户对象
                Mark = mark, // 1 为安全管理部门  2 为装置部门
                IsPrincipal = isPrincipal,
                IsEpiboly = isEpiboly,  //承包商
                ApplianceClass = dataitemdetailbll.GetItemValue("ApplianceClass"),  //装置类对象 
                LllegalNumber = LllegalNumber,   //违章编号
                //LllegalPic = Guid.NewGuid().ToString(), //违章图片 
                //ReformPic = Guid.NewGuid().ToString(), //整改图片
                //AcceptPic = Guid.NewGuid().ToString(), //验收图片  
                ExamineWay = itemlist.Where(p => p.EnCode == "ExamineWay"), //考核方式
                LllegalType = itemlist.Where(p => p.EnCode == "LllegalType"), //违章类型
                LllegalLevel = itemlist.Where(p => p.EnCode == "LllegalLevel"),  //违章级别
                FlowState = itemlist.Where(p => p.EnCode == "FlowState")  //违章流程状态
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
            string itemCode = "'ExamineWay','LllegalType','LllegalLevel','FlowState','LllegalDataScope'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            string mark = userbll.GetSafetyAndDeviceDept(CreateUser); //用于标记可立即整改违章

            string isPrincipal = userbll.HaveRoleListByKey(CreateUser.UserId, dataitemdetailbll.GetItemValue("PrincipalUser")).Rows.Count > 0 ? "1" : ""; //负责人

            string isEpiboly = userbll.HaveRoleListByKey(CreateUser.UserId, dataitemdetailbll.GetItemValue("EpibolyUser")).Rows.Count > 0 ? "1" : "";  //承包商
            //返回值
            var josnData = new
            {
                Mark = mark, // 1 为安全管理部门  2 为装置部门
                IsPrincipal = isPrincipal,
                IsEpiboly = isEpiboly,  //承包商
                ApplianceClass = dataitemdetailbll.GetItemValue("ApplianceClass"),  //装置类对象 
                ExamineWay = itemlist.Where(p => p.EnCode == "ExamineWay"), //考核方式
                LllegalType = itemlist.Where(p => p.EnCode == "LllegalType"), //违章类型
                LllegalLevel = itemlist.Where(p => p.EnCode == "LllegalLevel"),  //违章级别 
                FlowState = itemlist.Where(p => p.EnCode == "FlowState"),  //违章流程状态
                DataScope = itemlist.Where(p => p.EnCode == "LllegalDataScope")  //数据范围 
            };

            return Content(josnData.ToJson());
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

            var punishInfo = lllegalpunishbll.GetEntityByBid(baseInfo.ID); //考核内容

            var acceptInfo = lllegalacceptbll.GetEntityByBid(baseInfo.ID); //验收信息

            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户

            //厂级
            if (userInfo.isPlanLevel == "1")
            {
                userInfo.DeptName = userInfo.OrganizeName;
                userInfo.DeptCode = userInfo.OrganizeCode;
            }

            var data = new { baseInfo = baseInfo, approveInfo = approveInfo, reformInfo = reformInfo, punishInfo = punishInfo, acceptInfo = acceptInfo, userInfo = userInfo };

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
        public ActionResult SaveForm(string keyValue, LllegalRegisterEntity entity, LllegalPunishEntity pbEntity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            CommonSaveForm(keyValue, "03", entity, pbEntity, rEntity, aEntity);
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
        public void CommonSaveForm(string keyValue, string workFlow, LllegalRegisterEntity entity, LllegalPunishEntity pbEntity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            //提交通过
            string userId = OperatorProvider.Provider.Current().UserId;

            //保存违章基本信息
            entity.RESEVERFOUR = "";
            entity.RESEVERFIVE = "";
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


            string PUNISHID = Request.Form["PUNISHID"].ToString();
            pbEntity.LLLEGALID = entity.ID;
            pbEntity.MARK = "0"; //这里的"0"表示考核记录信息，"1"表示违章核准的核准考核记录
            //新增的时候，增加考核
            if (!string.IsNullOrEmpty(PUNISHID))
            {
                var tempEntity = lllegalpunishbll.GetEntity(PUNISHID);
                pbEntity.ID = tempEntity.ID;
                pbEntity.AUTOID = tempEntity.AUTOID;
                pbEntity.APPROVEID = null;
            }
            lllegalpunishbll.SaveForm(PUNISHID, pbEntity);

            /********整改信息************/
            string REFORMID = Request.Form["REFORMID"].ToString();
            rEntity.LLLEGALID = entity.ID;

            //新增状态下添加
            if (!string.IsNullOrEmpty(REFORMID))
            {
                var tempEntity = lllegalreformbll.GetEntity(REFORMID);
                rEntity.AUTOID = tempEntity.AUTOID;
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
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalPunishEntity pbEntity,
            LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            //判断重复编号过程
            var curHtBaseInfor = lllegalregisterbll.GetListByNumber(entity.LLLEGALNUMBER).FirstOrDefault();

            if (null != curHtBaseInfor)
            {
                if (curHtBaseInfor.ID != keyValue && string.IsNullOrEmpty(keyValue))
                {
                    return Error("违章编号重复,请重新新增!");
                }
            }
            string errorMsg = string.Empty;

            bool isAddScore = false; //是否添加到用户积分

            try
            {
                //创建流程，保存对应信息
                CommonSaveForm(keyValue, "03", entity, pbEntity, rEntity, aEntity);
                //创建完流程实例后
                if (string.IsNullOrEmpty(keyValue))
                {
                    keyValue = entity.ID;
                }
                //此处需要判断当前人是否为安全管理员
                string wfFlag = string.Empty;
                //当前用户
                Operator curUser = OperatorProvider.Provider.Current();

                IList<UserEntity> ulist = new List<UserEntity>();
                //参与人员
                string participant = string.Empty;

                //外包单位人员提交到发包单位
                if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("EpibolyUser")).Rows.Count > 0)
                {
                    wfFlag = "1";  // 登记=>I级核准


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
                            wfFlag = "2";  // 登记=>整改

                            errorMsg = "整改人";
                            //如果非装置类 则提交到整改
                            UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //整改用户对象
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

                                wfFlag = "3";  // 登记=>II级核准

                                errorMsg = "装置部门用户";
                                //取装置用户
                                participant = userbll.GetSafetyDeviceDeptUser("1", curUser);
                            }
                            else  //非装置类
                            {
                                wfFlag = "2";  // 登记=>整改

                                errorMsg = "整改人";
                                //如果非装置类 则提交到整改
                                UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //整改用户对象
                                //取整改人
                                participant = reformUser.Account;

                                isAddScore = true;
                            }
                        }

                    }
                    //装置部门人员
                    else if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                    {
                        wfFlag = "2";  // 登记=>整改

                        //如果非装置类 则提交到整改
                        UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //整改用户对象


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
                                wfFlag = "3";  // 登记=>II级核准

                                errorMsg = "安全管理部门用户";

                                //取安全管理部门用户
                                participant = userbll.GetSafetyDeviceDeptUser("0", curUser);
                            }
                            else  //不上报
                            {
                                wfFlag = "2";  // 登记=>整改

                                UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //整改用户对象

                                errorMsg = "整改人";
                                //取整改人
                                participant = reformUser.Account;

                                isAddScore = true;
                            }
                        }
                        else  //提交到班组负责人处核准
                        {
                            wfFlag = "1";  // 登记=>I级核准

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
                    lllegalpunishbll.SaveUserScore(pbEntity.FIRSTINCHARGEID, entity.LLLEGALLEVEL);
                    lllegalpunishbll.SaveUserScore(pbEntity.SECONDINCHARGEID, entity.LLLEGALLEVEL);
                }


                //提交流程到下一节点
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //更新业务流程状态
                    }
                }
                else
                {
                    return Error("请联系系统管理员，确认" + errorMsg + "!");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Success("操作成功!");
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
            LllegalPunishEntity pbEntity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
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
            CommonSaveForm(keyValue, "04", entity, pbEntity, rEntity, aEntity);

            ////保存核准信息
            pEntity.LLLEGALID = entity.ID;
            lllegalapprovebll.SaveForm(ApproveID, pEntity);

            //新增考核记录
            LllegalPunishEntity punishentity = new LllegalPunishEntity();
            punishentity = pbEntity;
            punishentity.AUTOID = null;
            punishentity.ID = null;
            punishentity.LLLEGALID = entity.ID;
            punishentity.APPROVEID = pEntity.ID;
            punishentity.MARK = "1";
            lllegalpunishbll.SaveForm("", punishentity);

            wfFlag = "1";//整改结束

            participant = curUser.Account;

            //提交流程
            int count = htworkflowbll.SubmitWorkFlow(entity.ID, participant, wfFlag, curUser.UserId);

            if (count > 0)
            {
                lllegalregisterbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //更新业务流程状态
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

            var dtHid = lllegalregisterbll.GetListByCheckId(checkid, curUser.UserId, "违章登记");

            string keyValue = string.Empty;

            string reformpeopleid = string.Empty; //整改人

            string errorMsg = string.Empty;

            bool isAddScore = false; //是否添加到用户积分

            foreach (DataRow row in dtHid.Rows)
            {
                keyValue = row["id"].ToString();

                reformpeopleid = row["reformpeopleid"].ToString();


                LllegalRegisterEntity entity = lllegalregisterbll.GetEntity(keyValue);

                LllegalPunishEntity pbEntity = lllegalpunishbll.GetEntityByBid(keyValue);

                //此处需要判断当前人是否为安全管理员
                string wfFlag = string.Empty;

                IList<UserEntity> ulist = new List<UserEntity>();
                //参与人员
                string participant = string.Empty;

                //外包单位人员提交到发包单位
                if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("EpibolyUser")).Rows.Count > 0)
                {
                    wfFlag = "1";  // 登记=>I级核准

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

                                wfFlag = "3";  // 登记=>II级核准

                                errorMsg = "装置部门用户";
                                //取装置用户
                                participant = userbll.GetSafetyDeviceDeptUser("1", curUser);
                            }
                            else  //非装置类
                            {
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
                                wfFlag = "3";  // 登记=>II级核准

                                errorMsg = "安全管理部门用户";

                                //取安全管理部门用户
                                participant = userbll.GetSafetyDeviceDeptUser("0", curUser);
                            }
                            else  //不上报
                            {
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
                            wfFlag = "1";  // 登记=>I级核准

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
                    lllegalpunishbll.SaveUserScore(pbEntity.FIRSTINCHARGEID, entity.LLLEGALLEVEL);
                    lllegalpunishbll.SaveUserScore(pbEntity.SECONDINCHARGEID, entity.LLLEGALLEVEL);
                }


                //提交流程到下一节点
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //更新业务流程状态
                    }
                }
            }

            return Success("操作成功!");
        }
        #endregion

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

            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户

            string fileName = "违章考核通知单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/违章考核通知单导出模板.doc");

            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);

            DataTable dt = new DataTable();
            dt.Columns.Add("LllegalYear");
            dt.Columns.Add("LllegalMonth");
            dt.Columns.Add("LllegalDate");
            dt.Columns.Add("LllegalTime");
            dt.Columns.Add("LllegalAddress");
            dt.Columns.Add("LllegalPeople");
            dt.Columns.Add("LllegalDescribe");
            dt.Columns.Add("FirstPerson");
            dt.Columns.Add("FirstMoney");
            dt.Columns.Add("FirstPoint");
            dt.Columns.Add("SecondPerson");
            dt.Columns.Add("SecondMoney");
            dt.Columns.Add("SecondPoint");
            dt.Columns.Add("ThirdPerson");
            dt.Columns.Add("ThirdMoney");
            dt.Columns.Add("ThirdPoint");
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
                        builder.InsertImage(url, Aspose.Words.Drawing.RelativeHorizontalPosition.Margin, 1, Aspose.Words.Drawing.RelativeVerticalPosition.Margin, 320, 50, 50, Aspose.Words.Drawing.WrapType.Inline);
                    }
                }
            }
            row["LllegalYear"] = year;
            row["LllegalMonth"] = month;
            row["LllegalDate"] = date;
            row["LllegalAddress"] = lllegaldt.Rows[0]["lllegaladdress"].ToString();
            row["LllegalPeople"] = lllegaldt.Rows[0]["lllegalperson"].ToString();
            row["LllegalDescribe"] = lllegaldt.Rows[0]["lllegaldescribe"].ToString();
            row["FirstPerson"] = lllegaldt.Rows[0]["personinchargename"].ToString();
            row["FirstMoney"] = !string.IsNullOrEmpty(lllegaldt.Rows[0]["economicspunish"].ToString()) ? lllegaldt.Rows[0]["economicspunish"].ToString() : "0";
            row["FirstPoint"] = !string.IsNullOrEmpty(lllegaldt.Rows[0]["lllegalpoint"].ToString()) ? lllegaldt.Rows[0]["lllegalpoint"].ToString() : "0";
            row["SecondPerson"] = lllegaldt.Rows[0]["firstinchargename"].ToString();
            row["SecondMoney"] = !string.IsNullOrEmpty(lllegaldt.Rows[0]["firsteconomicspunish"].ToString()) ? lllegaldt.Rows[0]["firsteconomicspunish"].ToString() : "0";
            row["SecondPoint"] = !string.IsNullOrEmpty(lllegaldt.Rows[0]["firstlllegalpoint"].ToString()) ? lllegaldt.Rows[0]["firstlllegalpoint"].ToString() : "0";
            row["ThirdPerson"] = lllegaldt.Rows[0]["secondinchargename"].ToString();
            row["ThirdMoney"] = !string.IsNullOrEmpty(lllegaldt.Rows[0]["secondeconomicspunish"].ToString()) ? lllegaldt.Rows[0]["secondeconomicspunish"].ToString() : "0";
            row["ThirdPoint"] = !string.IsNullOrEmpty(lllegaldt.Rows[0]["secondlllegalpoint"].ToString()) ? lllegaldt.Rows[0]["secondlllegalpoint"].ToString() : "0";
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

        #region 获取积分档案

        public List<OrganizeEntity> organizedata = new List<OrganizeEntity>();
        public List<DepartmentEntity> departmentdata = new List<DepartmentEntity>();
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
                organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).ToList();
                departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).ToList();
            }
            else
            {
                if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户") || user.DeptName.Contains("安环部"))
                {
                    organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).Where(e => e.EnCode == user.OrganizeCode).ToList();
                    departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).Where(e => e.OrganizeId == user.OrganizeId).ToList();
                }
                else
                {
                    departmentdata = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode) || t.Description == "外包工程承包商" || t.SendDeptID == user.DeptId).OrderBy(x => x.SortCode).ToList();

                }
            }
            //异步加载数据
            //将函数加入委托
            MyDelegate dele = new MyDelegate(AddOrg);
            //添加参数
            var result = dele.BeginInvoke(Year, null, null);
            treeList.AddRange(dele.EndInvoke(result));
            dele = new MyDelegate(AddDept);
            result = dele.BeginInvoke(Year, null, null);
            treeList.AddRange(dele.EndInvoke(result));

            var json = treeList.TreeJson(parentId);
            //返回数据
            return Content(json);

        }

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

        /// <summary>
        /// 获取部门的违章次数
        /// </summary>
        /// <param name="DepartmentId"></param>
        /// <param name="Year"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int GetWZNum(string DepartmentId, string Year)
        {
            string sql = "select count(1) as Num from v_LllegalAll2 where DepartmentId='" + DepartmentId + "'";

            if (Year.Length > 0)
            {
                sql += " and to_char(lllegaltime, 'yyyy')='" + Year + "'";

            }
            var dt = hazardsourcebll.FindTableBySql(sql);
            return int.Parse(dt.Rows[0]["Num"].ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DepartmentId"></param>
        /// <returns></returns>
        public decimal GetWZScore(string DepartmentId, string Year)
        {

            string sql = "select lllegallevelname,Id,lllegalpoint from v_LllegalAll where DepartmentId='" + DepartmentId + "'";
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

            var DeptId = Request["DeptId"] ?? "";
            string tWhere = " and DepartmentId='" + DeptId + "'";
            var Year = Request["Year"] ?? "";
            if (Year.Length > 0)
            {
                tWhere += "and  to_char(lllegaltime, 'yyyy')='" + Year + "'";

            }
            queryJson = queryJson ?? "";
            pagination.p_fields = "deptname,realname,wznum,lllegalpoint";
            pagination.p_tablename = @"(select min(deptname) as deptname,realname,count(1) as wznum,sum(lllegalpoint) as lllegalpoint  from v_LllegalAll2 where 1=1 " + tWhere + " group by RealName) t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

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
                organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).ToList();
                departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).ToList();
            }
            else
            {
                if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户") || user.DeptName.Contains("安环部"))
                {
                    organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).Where(e => e.EnCode == user.OrganizeCode).ToList();
                    departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).Where(e => e.EnCode.Substring(0, user.DeptCode.Length) == user.DeptCode).ToList();
                }
                else
                {
                    departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).Where(e => e.EnCode.Length >= user.DeptCode.Length && e.EnCode.Substring(0, user.DeptCode.Length) == user.DeptCode).ToList();
                    parentId = user.ParentId;
                }
            }
            var treeList = new List<TreeGridEntity>();
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
                var Ids = "";
                itemJson = itemJson.Insert(1, "\"DepartWZNum\":\"" + GetWZNum(item.OrganizeId, Year) + "\",");

                itemJson = itemJson.Insert(1, "\"DepartWZScore\":\"" + GetWZScore(item.OrganizeId, Year) + "\",");
                itemJson = itemJson.Insert(1, "\"Ids\":\"" + Ids + "\",");
                tree.entityJson = itemJson;
                treeList.Add(tree);
            }
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
    }
}
