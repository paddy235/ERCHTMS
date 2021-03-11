using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HazardsourceManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Entity.SystemManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 描 述：集团、省级违章基本信息表
    /// </summary>
    public class LllegalRegisterGrpController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // 违章基本信息
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //整改信息对象
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //核准信息对象
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //验收信息对象
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); // 考核信息对象
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
      
        #region 视图
        /// <summary>
        /// 列表页面  各流程页面使用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 列表页面  各流程页面使用(省公司)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SdIndex()
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
        #endregion

        #region 协助方法
        /// <summary>
        /// 获取所属单位（省公司下属电厂，稍后完善。）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLllegalDepartListJson()
        {
            Operator opertator = new OperatorProvider().Current();
            var data = new DepartmentBLL().GetList().Where(t => t.DeptCode.StartsWith(opertator.NewDeptCode) && (t.Nature == "电厂" || t.Nature == "厂级"));
            var listDept = data.Select(x => { return new { DeptId = x.DepartmentId, DeptName = x.FullName }; });
            return Content(listDept.ToJson());
        }
        /// <summary>
        /// 获取所属单位（省公司及下属电厂，稍后完善。）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLllegalDepartListJsonGrp()
        {
            Operator opertator = new OperatorProvider().Current();
            var data = new DepartmentBLL().GetList().Where(t => t.DeptCode.StartsWith(opertator.OrganizeCode) && (t.Nature == "集团" || t.Nature == "分子公司" || t.Nature == "电厂" || t.Nature == "厂级"));
            var listDept = data.OrderBy(x=>x.DeptCode).Select(x => { return new { DeptId = x.DepartmentId, DeptName = x.FullName }; });           
            return Content(listDept.ToJson());
        }
        #endregion

        #region  保存表单（新增、修改）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">违章基本信息</param>
        /// <param name="pbEntity">考核记录</param>
        /// <param name="rEntity">整改信息</param>
        /// <param name="aEntity">验收信息</param>
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
        /// <param name="workFlow">工作流编码</param>
        /// <param name="entity">违章基本信息</param>
        /// <param name="pbEntity">考核记录</param>
        /// <param name="rEntity">整改信息</param>
        /// <param name="aEntity">验收信息</param>
        public void CommonSaveForm(string keyValue, string workFlow, LllegalRegisterEntity entity, LllegalPunishEntity pbEntity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            //提交通过
            string userId = OperatorProvider.Provider.Current().UserId;

            //保存违章基本信息
            entity.RESEVERFOUR = "";
            entity.RESEVERFIVE = "";
            if (string.IsNullOrEmpty(keyValue))
            {
                string lenNum = !string.IsNullOrEmpty(dataitemdetailbll.GetItemValue("LllegalSerialNumberLen")) ? dataitemdetailbll.GetItemValue("LllegalSerialNumberLen") : "3";

                entity.LLLEGALNUMBER = lllegalregisterbll.GenerateHidCode("bis_lllegalregister", "lllegalnumber", int.Parse(lenNum));
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

        #region 提交流程（同时新增、修改违章信息，含推送流程。）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>       
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">违章基本信息</param>
        /// <param name="pbEntity">考核记录</param>
        /// <param name="rEntity">整改信息</param>
        /// <param name="aEntity">验收信息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalPunishEntity pbEntity,
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
            string errorMsg = string.Empty;

            //bool isAddScore = false; //是否添加到用户积分

            string startflow = string.Empty;//起始
            string endflow = string.Empty;//截止

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
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'FlowState'");
                string startnode = itemlist.Where(p => p.ItemName == "违章登记").Count() > 0 ? "违章登记" : "违章举报";
                //省公司、省级用户
                if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("GrpUser")).Rows.Count > 0)
                {
                    startflow = startnode;
                    endflow = "违章完善";
                    wfFlag = "4";  // 登记=>完善
                    errorMsg = "省公司、省级用户";
                    //取安全主管部门用户 完善
                    participant = userbll.GetSafetyDeviceDeptUser("0", entity.BELONGDEPARTID);

                }                

                //添加用户积分关联
                //if (isAddScore)
                //{
                //    lllegalpunishbll.SaveUserScore(pbEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                //    //关联责任人
                //    var relevanceList = lllegalpunishbll.GetListByLllegalId(entity.ID, "1");
                //    foreach (LllegalPunishEntity lpEntity in relevanceList)
                //    {
                //        //违章责任人
                //        lllegalpunishbll.SaveUserScore(lpEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                //    }
                //}
                
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(startflow,endflow, keyValue, participant, wfFlag, curUser.UserId);

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
    }         
}
