using ERCHTMS.Entity.QuestionManage;
using ERCHTMS.Busines.QuestionManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Linq;
using ERCHTMS.Code;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Collections.Generic;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.JPush;

namespace ERCHTMS.Web.Areas.QuestionManage.Controllers
{
    /// <summary>
    /// 描 述：发现问题基本信息表
    /// </summary>
    public class FindQuestionInfoController : MvcControllerBase
    {
        private FindQuestionInfoBLL findquestioninfobll = new FindQuestionInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
        private WfControlBLL wfcontrolbll = new WfControlBLL();//自动化流程服务
        private FindQuestionHandleBLL findquestionhandlebll = new FindQuestionHandleBLL(); //问题处理

        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL(); //隐患整改信息
        private HTApprovalBLL htapprovalbll = new HTApprovalBLL(); //隐患评估信息
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL(); //隐患验收信息

        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // 违章基本信息
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //整改信息对象
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //验收信息对象

        private QuestionInfoBLL questioninfobll = new QuestionInfoBLL();
        private QuestionReformBLL questionreformbll = new QuestionReformBLL();

        private FileInfoBLL fileinfobll = new FileInfoBLL();

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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DoneForm()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SituationForm() 
        {
            return View();
        }
        #endregion


        #region 获取发现问题列表数据
        /// <summary>
        /// 获取发现问题列表数据
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson) 
        {
            var watch = CommonHelper.TimerStart();
            var data = findquestioninfobll.GetFindQuestionInfoPageList(pagination, queryJson);
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

        #region 初始化查询条件
        /// <summary>
        /// 初始化查询条件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueryConditionJson()
        {
            Operator CreateUser = OperatorProvider.Provider.Current();

            //流程状态
            string itemCode = "'FindQuestionFlowState'";
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);

            //返回值
            var josnData = new
            {
                FlowState = itemlist.Where(p => p.EnCode == "FindQuestionFlowState")  //发现问题流程状态
            };
            return Content(josnData.ToJson());
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var baseInfo = findquestioninfobll.GetEntity(keyValue);  //问题基本信息

            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户

            var data = new { baseInfo = baseInfo, userInfo = userbll.GetUserInfoEntity(userInfo.UserId) };

            return ToJsonResult(data);
        }
        #endregion

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetFindQuestionHandleListJson(string keyValue)
        {
            var data = findquestionhandlebll.GetQuestionHandleTable(keyValue);

            return ToJsonResult(data);
        }

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
            findquestioninfobll.RemoveForm(keyValue);
            return Success("删除成功。");
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
        public ActionResult SaveForm(string keyValue, FindQuestionInfoEntity entity)
        {
            CommonSaveForm(keyValue, entity);
            return Success("操作成功!");
        }
        #endregion

        #region 公用方法，保存数据
        /// <summary>
        /// 公用方法，保存数据
        /// </summary>
        /// <param name="keyValue">主键ID</param>
        /// <param name="entity">问题基本信息</param>
        /// <param name="rEntity">整改信息</param>
        /// <param name="aEntity">验收信息</param>
        public void CommonSaveForm(string keyValue, FindQuestionInfoEntity entity)
        {
            try
            {
                var userInfo = OperatorProvider.Provider.Current();  //获取当前用户

                string userId = userInfo.UserId;

                findquestioninfobll.SaveForm(keyValue, entity);

                //创建流程实例
                if (string.IsNullOrEmpty(keyValue))
                {
                    bool isSucess = htworkflowbll.CreateWorkFlowObj("10", entity.ID, userId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_findquestioninfo", "flowstate", entity.ID);  //更新业务流程状态
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 提交流程（同时新增、修改问题信息）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, FindQuestionInfoEntity entity)
        {
            try
            {
                //创建流程，保存对应信息
                CommonSaveForm(keyValue, entity);
                //创建完流程实例后
                if (string.IsNullOrEmpty(keyValue))
                {
                    keyValue = entity.ID;
                }
                //此处需要判断当前人是否为安全管理员
                string wfFlag = string.Empty;
                //当前用户
                Operator curUser = OperatorProvider.Provider.Current();

                string mode = Request.Form["mode"];

                bool isback = !string.IsNullOrEmpty(mode) && mode == "3"; //是否退回

                var nEntity = findquestioninfobll.GetEntity(keyValue);

                //参与人员
                string participant = string.Empty;
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.startflow = nEntity.FLOWSTATE;
                wfentity.submittype = "提交";
                if (isback)
                {
                    wfentity.submittype = "退回";
                }
                wfentity.rankid = string.Empty;
                wfentity.user = curUser;
                wfentity.mark = "发现问题流程";
                wfentity.organizeid = entity.ORGANIZEID; //对应电厂id
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
                            //退回操作记录内容
                            if (isback)
                            {
                                FindQuestionHandleEntity qentity = new FindQuestionHandleEntity();
                                qentity.HANDLESTATUS = "已退回";
                                qentity.HANDLEDATE = DateTime.Now;
                                qentity.HANDLERID = curUser.UserId;
                                qentity.HANDLERNAME = curUser.UserName;
                                qentity.QUESTIONID = keyValue;
                                qentity.APPSIGN = "Web";
                                findquestionhandlebll.SaveForm("", qentity);
                            }
                            htworkflowbll.UpdateFlowStateByObjectId("bis_findquestioninfo", "flowstate", keyValue);  //更新业务流程状态
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

        #region 转隐患、转违章、转问题
        /// <summary>
        /// 转隐患、转违章、转问题
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ToTargetContent(string keyValue, int mode)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            var entity = findquestioninfobll.GetEntity(keyValue); //发现问题对象
            var userInfo = userbll.GetUserInfoEntity(entity.CREATEUSERID); //创建人对象
            List<FileInfoEntity> filelist = fileinfobll.GetImageListByObject(entity.QUESTIONPIC).ToList(); //问题图片集合
            string resultMsg = string.Empty;//返回结果信息
            bool isSucess = false;  //创建流程是否成功
            bool isSucessful = true; //返回流程推进结果
            string wfFlag = string.Empty; //流程流转标记
            string participant = string.Empty; //下一步流程参与者
            string workFlow = string.Empty; //流程实例代码
            string applicationId = string.Empty; //关联的应用id
            string applicationType = string.Empty; //关联的应用类型 
            WfControlObj wfentity = new WfControlObj();
            WfControlResult result = new WfControlResult();
            switch (mode)
            {
                //转隐患         
                case 0:
                    applicationType = "yh";
                    #region 转隐患
                    string HidCode = DateTime.Now.ToString("yyyyMMddHHmmssfff").ToString();
                    try
                    {
                        #region 隐患基本信息
                        HTBaseInfoEntity bentity = new HTBaseInfoEntity();
                        bentity.ADDTYPE = "0";
                        bentity.CREATEUSERID = userInfo.UserId;
                        bentity.CREATEUSERNAME = userInfo.RealName;
                        bentity.CREATEUSERDEPTCODE = userInfo.DepartmentCode;
                        bentity.CREATEUSERORGCODE = userInfo.OrganizeCode;
                        bentity.HIDCODE = HidCode;
                        bentity.HIDDEPART = userInfo.OrganizeId;
                        bentity.HIDDEPARTNAME = userInfo.OrganizeName;
                        bentity.HIDPHOTO = Guid.NewGuid().ToString(); //图片

                        foreach (FileInfoEntity fentity in filelist)
                        {
                            string sourcefile = Server.MapPath(fentity.FilePath);
                            string targetFileName = Guid.NewGuid().ToString() + "." + fentity.FileType;
                            string targetUrl = fentity.FilePath.Substring(0, fentity.FilePath.LastIndexOf("/"));
                            if (System.IO.File.Exists(sourcefile)) 
                            {
                                System.IO.FileInfo sfileInfo = new System.IO.FileInfo(sourcefile); 
                                string targetDir = sfileInfo.DirectoryName;
                                string targetFile = targetDir + "\\" +  targetFileName; 
                                System.IO.File.Copy(sourcefile, targetFile);
                            }
                            FileInfoEntity newfileEntity = new FileInfoEntity();
                            newfileEntity = fentity;
                            newfileEntity.FilePath = targetUrl + "/" + targetFileName;
                            newfileEntity.FileId = string.Empty;
                            newfileEntity.RecId = bentity.HIDPHOTO;
                            fileinfobll.SaveForm("", newfileEntity);
                        }

                        bentity.HIDBMID = entity.DEPTID; //所属部门id
                        bentity.HIDBMNAME = entity.DEPTNAME; //所属部门名称
                        bentity.HIDDESCRIBE = entity.QUESTIONCONTENT; //隐患描述(问题内容)

                        //排查信息
                        bentity.CHECKDATE = DateTime.Now;
                        bentity.CHECKMAN = userInfo.UserId;
                        bentity.CHECKMANNAME = userInfo.RealName;
                        bentity.CHECKDEPARTID = userInfo.DepartmentCode;
                        bentity.CHECKDEPARTNAME = userInfo.DeptName;
                        //bentity.CHECKTYPE = dataitemdetailbll.GetDataItemListByItemCode("'SaftyCheckType'").Where(p => p.ItemName.Contains("日常")).FirstOrDefault().ItemDetailId; //检查类型
                        //添加
                        htbaseinfobll.SaveForm("", bentity);

                        applicationId = bentity.ID;
                        #endregion

                        #region 创建隐患流程
                        workFlow = "01";//隐患处理
                        isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, applicationId, userInfo.UserId);
                        if (isSucess)
                        {
                            htworkflowbll.UpdateWorkStreamByObjectId(applicationId);  //更新业务流程状态
                        }
                        #endregion

                        #region 整改信息
                        HTChangeInfoEntity centity = new HTChangeInfoEntity();
                        centity.HIDCODE = HidCode;
                        htchangeinfobll.SaveForm("", centity);
                        #endregion

                        #region 验收信息
                        HTAcceptInfoEntity aentity = new HTAcceptInfoEntity();
                        aentity.HIDCODE = HidCode;
                        htacceptinfobll.SaveForm("", aentity);
                        #endregion

                        #region 推进流程

                        wfentity.businessid = applicationId; //隐患主键
                        wfentity.argument1 = string.Empty; //专业分类
                        wfentity.argument2 = userInfo.DepartmentId; //当前部门
                        wfentity.argument3 = string.Empty; //隐患类别
                        wfentity.argument4 = bentity.HIDBMID; //所属部门
                        wfentity.startflow = "隐患登记";
                        wfentity.submittype = "提交";
                        wfentity.rankid = string.Empty;
                        wfentity.spuser = userInfo;
                        wfentity.mark = "厂级隐患排查";
                        wfentity.organizeid = bentity.HIDDEPART; //对应电厂id
                        //获取下一流程的操作人
                        result = wfcontrolbll.GetWfControl(wfentity);
                        //处理成功
                        if (result.code == WfCode.Sucess)
                        {
                            participant = result.actionperson;
                            wfFlag = result.wfflag;
                            if (!string.IsNullOrEmpty(participant))
                            {
                                int count = htworkflowbll.SubmitWorkFlow(wfentity, result, applicationId, participant, wfFlag, userInfo.UserId);

                                if (count > 0)
                                {
                                    htworkflowbll.UpdateWorkStreamByObjectId(applicationId);  //更新业务流程状态
                                }
                            }
                            else
                            {
                                isSucessful = false;
                                resultMsg = "请联系系统管理员，添加本单位及相关单位评估人员!";
                            }
                            resultMsg = "已成功转为隐患，并进入对应流程，请知晓";
                        }
                        else
                        {
                            isSucessful = false;
                            resultMsg = result.message;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        isSucessful = false;
                        resultMsg = ex.Message;
                    }
                    #endregion
                    break;
                //转违章
                case 1:
                    applicationType = "wz";
                    #region 转违章
                    try
                    {
                        #region 违章基础信息
                        string lenNum = !string.IsNullOrEmpty(dataitemdetailbll.GetItemValue("LllegalSerialNumberLen")) ? dataitemdetailbll.GetItemValue("LllegalSerialNumberLen") : "3";
                        LllegalRegisterEntity wzentity = new LllegalRegisterEntity();
                        wzentity.ADDTYPE = "0";
                        wzentity.LLLEGALNUMBER = lllegalregisterbll.GenerateHidCode("bis_lllegalregister", "lllegalnumber", int.Parse(lenNum)); //违章编码
                        wzentity.CREATEUSERID = userInfo.UserId;
                        wzentity.CREATEUSERNAME = userInfo.RealName;
                        wzentity.CREATEUSERDEPTCODE = userInfo.DepartmentCode;
                        wzentity.CREATEUSERORGCODE = userInfo.OrganizeCode;
                        wzentity.CREATEDEPTID = userInfo.DepartmentId;
                        wzentity.CREATEDEPTNAME = userInfo.DeptName;
                        //所属单位
                        wzentity.BELONGDEPARTID = userInfo.OrganizeId;
                        wzentity.BELONGDEPART = userInfo.OrganizeName;

                        wzentity.LLLEGALPIC = Guid.NewGuid().ToString();
                        foreach (FileInfoEntity fentity in filelist)
                        {
                            string sourcefile = Server.MapPath(fentity.FilePath);
                            string targetFileName = Guid.NewGuid().ToString() + "." + fentity.FileType;
                            string targetUrl = fentity.FilePath.Substring(0, fentity.FilePath.LastIndexOf("/"));
                            if (System.IO.File.Exists(sourcefile))
                            {
                                System.IO.FileInfo sfileInfo = new System.IO.FileInfo(sourcefile);
                                string targetDir = sfileInfo.DirectoryName;
                                string targetFile = targetDir + "\\" + targetFileName;
                                System.IO.File.Copy(sourcefile, targetFile);
                            }
                            FileInfoEntity newfileEntity = new FileInfoEntity();
                            newfileEntity = fentity;
                            newfileEntity.FilePath = targetUrl + "/" + targetFileName;
                            newfileEntity.FileId = string.Empty;
                            newfileEntity.RecId = wzentity.LLLEGALPIC;
                            fileinfobll.SaveForm("", newfileEntity);
                        }
                        wzentity.LLLEGALDESCRIBE = entity.QUESTIONCONTENT;
                        lllegalregisterbll.SaveForm("", wzentity);
                        applicationId = wzentity.ID;
                        #endregion

                        #region 创建流程
                        workFlow = "03";
                        isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, applicationId, userInfo.UserId);
                        if (isSucess)
                        {
                            lllegalregisterbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", applicationId);  //更新业务流程状态
                        }
                        #endregion

                        if (!string.IsNullOrEmpty(wzentity.ID))
                        {
                            wzentity = lllegalregisterbll.GetEntity(wzentity.ID);
                        }

                        #region 违章整改信息
                        LllegalReformEntity reformEntity = new LllegalReformEntity();
                        reformEntity.LLLEGALID = applicationId;
                        lllegalreformbll.SaveForm("", reformEntity);
                        #endregion

                        #region 违章验收信息
                        LllegalAcceptEntity acceptEntity = new LllegalAcceptEntity();
                        acceptEntity.LLLEGALID = applicationId;
                        lllegalacceptbll.SaveForm("", acceptEntity);
                        #endregion

                        #region 推进流程
                        wfentity.businessid = applicationId; //主键
                        wfentity.argument3 = userInfo.DepartmentId;//当前部门id
                        wfentity.startflow = wzentity.FLOWSTATE;
                        wfentity.submittype = "提交";
                        wfentity.rankid = null;
                        wfentity.spuser = userInfo;
                        wfentity.mark = "厂级违章流程";
                        wfentity.organizeid = wzentity.BELONGDEPARTID; //对应电厂id
                        //获取下一流程的操作人
                        result = wfcontrolbll.GetWfControl(wfentity);

                        //处理成功
                        if (result.code == WfCode.Sucess)
                        {
                            participant = result.actionperson;
                            wfFlag = result.wfflag;

                            //提交流程到下一节点
                            if (!string.IsNullOrEmpty(participant))
                            {
                                int count = htworkflowbll.SubmitWorkFlow(wfentity, result, applicationId, participant, wfFlag, userInfo.UserId);

                                if (count > 0)
                                {
                                    htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", applicationId);  //更新业务流程状态
                                }
                            }
                            resultMsg = "已成功转为违章，并进入对应流程，请知晓";
                        }
                        else
                        {
                            isSucessful = false;
                            resultMsg = result.message;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        isSucessful = false;
                        resultMsg = ex.Message;
                    }
                    #endregion
                    break;
                //转问题
                case 2:
                    applicationType = "wt";
                    #region 转问题
                    try
                    {
                        #region 基础信息
                        QuestionInfoEntity qtEntity = new QuestionInfoEntity();
                        qtEntity.QUESTIONNUMBER = questioninfobll.GenerateCode("bis_questioninfo", "questionnumber", 4);
                        qtEntity.CREATEUSERID = userInfo.UserId;
                        qtEntity.CREATEUSERNAME = userInfo.RealName;
                        qtEntity.CREATEUSERDEPTCODE = userInfo.DepartmentCode;
                        qtEntity.CREATEUSERORGCODE = userInfo.OrganizeCode;

                        qtEntity.BELONGDEPTID = userInfo.OrganizeId;
                        qtEntity.BELONGDEPTNAME = userInfo.OrganizeName;

                        qtEntity.QUESTIONPIC = Guid.NewGuid().ToString();
                        foreach (FileInfoEntity fentity in filelist)
                        {
                            string sourcefile = Server.MapPath(fentity.FilePath);
                            string targetFileName = Guid.NewGuid().ToString() + "." + fentity.FileType;
                            string targetUrl = fentity.FilePath.Substring(0, fentity.FilePath.LastIndexOf("/"));
                            if (System.IO.File.Exists(sourcefile))
                            {
                                System.IO.FileInfo sfileInfo = new System.IO.FileInfo(sourcefile);
                                string targetDir = sfileInfo.DirectoryName;
                                string targetFile = targetDir + "\\" + targetFileName;
                                System.IO.File.Copy(sourcefile, targetFile);
                            }
                            FileInfoEntity newfileEntity = new FileInfoEntity();
                            newfileEntity = fentity;
                            newfileEntity.FilePath = targetUrl + "/" + targetFileName;
                            newfileEntity.FileId = string.Empty;
                            newfileEntity.RecId = qtEntity.QUESTIONPIC;
                            fileinfobll.SaveForm("", newfileEntity);
                        }
                        qtEntity.QUESTIONDESCRIBE = entity.QUESTIONCONTENT;

                        qtEntity.CHECKDATE = DateTime.Now;
                        qtEntity.CHECKPERSONID = userInfo.UserId;
                        qtEntity.CHECKPERSONNAME = userInfo.RealName;
                        qtEntity.CHECKDEPTID = userInfo.DepartmentId;
                        qtEntity.CHECKDEPTNAME = userInfo.DeptName;
                        //qtEntity.CHECKTYPE = dataitemdetailbll.GetDataItemListByItemCode("'SaftyCheckType'").Where(p => p.ItemName.Contains("日常")).FirstOrDefault().ItemDetailId; //检查类型

                        questioninfobll.SaveForm("", qtEntity);
                        applicationId = qtEntity.ID;
                        #endregion

                        #region 创建流程
                        workFlow = "09";//问题处理
                        isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, applicationId, userInfo.UserId);
                        if (isSucess)
                        {
                            htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", applicationId);  //更新业务流程状态
                        }
                        #endregion

                        #region 整改信息
                        QuestionReformEntity qtreformEntity = new QuestionReformEntity();
                        qtreformEntity.QUESTIONID = applicationId;
                        questionreformbll.SaveForm("", qtreformEntity);
                        #endregion

                        //极光消息推送
                        JPushApi.PushMessage(userInfo.Account, userInfo.RealName, "WT001", "您有一条问题需完善，请到问题登记进行处理", "您" + entity .CREATEDATE.Value.ToString("yyyy-MM-dd")+ "发现的问题已确定为问题，请您到问题登记下对该问题进行完善并指定对应整改责任人。", applicationId);

                        resultMsg = "已成功转为问题，并进入对应流程，请知晓";
                    }
                    catch (Exception ex)
                    {
                        isSucessful = false;
                        resultMsg = ex.Message;
                    }
                    #endregion
                    break;
            }

            try
            {
                if (isSucessful)
                {
                    //评估阶段转
                    if (entity.FLOWSTATE == "评估")
                    {
                        #region 推进发现问题流程
                        wfentity = new WfControlObj();
                        wfentity.businessid = keyValue; //
                        wfentity.startflow = entity.FLOWSTATE;
                        wfentity.submittype = "提交";
                        wfentity.rankid = string.Empty;
                        wfentity.user = curUser;
                        wfentity.spuser = null;
                        wfentity.mark = "发现问题流程";
                        wfentity.organizeid = entity.ORGANIZEID; //对应电厂id
                        //获取下一流程的操作人
                        result = wfcontrolbll.GetWfControl(wfentity);
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
                                    //返回成功的结果
                                    #region 返回成功的结果
                                    FindQuestionHandleEntity qentity = new FindQuestionHandleEntity();
                                    if (mode == 0)
                                    {
                                        qentity.HANDLESTATUS = "已转隐患";
                                    }
                                    else if (mode == 1)
                                    {
                                        qentity.HANDLESTATUS = "已转违章";
                                    }
                                    else if (mode == 2)
                                    {
                                        qentity.HANDLESTATUS = "已转问题";
                                    }
                                    qentity.HANDLEDATE = DateTime.Now;
                                    qentity.HANDLERID = curUser.UserId;
                                    qentity.HANDLERNAME = curUser.UserName;
                                    qentity.QUESTIONID = keyValue;
                                    qentity.RELEVANCEID = applicationId;
                                    qentity.RELEVANCETYPE = applicationType;
                                    qentity.APPSIGN = "Web";
                                    findquestionhandlebll.SaveForm("", qentity);
                                    #endregion

                                    htworkflowbll.UpdateFlowStateByObjectId("bis_findquestioninfo", "flowstate", keyValue);  //更新业务流程状态
                                }
                            }
                            return Success(resultMsg);
                        }
                        else
                        {
                            return Error(result.message);
                        }
                        #endregion
                    }
                    else  //结束阶段转 列表转
                    {
                        //返回成功的结果
                        #region 返回成功的结果
                        FindQuestionHandleEntity qentity = new FindQuestionHandleEntity();
                        if (mode == 0)
                        {
                            qentity.HANDLESTATUS = "已转隐患";
                        }
                        else if (mode == 1)
                        {
                            qentity.HANDLESTATUS = "已转违章";
                        }
                        else if (mode == 2)
                        {
                            qentity.HANDLESTATUS = "已转问题";
                        }
                        qentity.HANDLEDATE = DateTime.Now;
                        qentity.HANDLERID = curUser.UserId;
                        qentity.HANDLERNAME = curUser.UserName;
                        qentity.QUESTIONID = keyValue;
                        qentity.RELEVANCEID = applicationId;
                        qentity.RELEVANCETYPE = applicationType;
                        qentity.APPSIGN = "Web";
                        findquestionhandlebll.SaveForm("", qentity);
                        #endregion

                        return Success(resultMsg);
                    }
                }
                else
                {
                    return Error(resultMsg);
                }
               

            }
            catch (Exception ex)
            {
              return Error(ex.Message);
            }
        }
        #endregion

    }
}