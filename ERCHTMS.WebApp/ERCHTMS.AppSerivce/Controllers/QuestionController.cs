using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Attributes;
using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HazardsourceManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Busines.LllegalStandard;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.QuestionManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SaftyCheck;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.QuestionManage;
using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    /// <summary>
    /// 问题管理
    /// </summary>
    [HandlerLogin(LoginMode.Enforce)]
    public class QuestionController : BaseApiController
    {
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentCache departmentCache = new DepartmentCache();
        private RoleCache roleCache = new RoleCache();
        private AccountBLL accountBLL = new AccountBLL();


        private UserBLL userbll = new UserBLL(); //用户操作对象

        private SaftyCheckDataRecordBLL saftycheckdatarecordbll = new SaftyCheckDataRecordBLL();
        private DistrictBLL districtbll = new DistrictBLL();//区域
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private ProjectBLL projectbll = new ProjectBLL();
        private OrganizeBLL orgBLL = new OrganizeBLL();
        private FileFolderBLL fileFolderBLL = new FileFolderBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        //private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();

        private WfControlBLL wfcontrolbll = new WfControlBLL();//自动化流程服务

        //问题相关
        private QuestionInfoBLL questioninfobll = new QuestionInfoBLL();
        private QuestionReformBLL questionreformbll = new QuestionReformBLL();
        private QuestionVerifyBLL questionverifybll = new QuestionVerifyBLL();

        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL(); //隐患整改信息
        private HTApprovalBLL htapprovalbll = new HTApprovalBLL(); //隐患评估信息
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL(); //隐患验收信息

        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // 违章基本信息
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //整改信息对象
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //验收信息对象

        private FindQuestionInfoBLL findquestioninfobll = new FindQuestionInfoBLL();
        private FindQuestionHandleBLL findquestionhandlebll = new FindQuestionHandleBLL();

        //private FileInfoBLL fileinfobll = new FileInfoBLL();

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



        /*********************问题流程*************************/

        #region 基础信息

        #region 判断是否有流程
        /// <summary>
        /// 判断是否有流程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object IsHavaQuestionWorkFlow()
        {
            bool isHavaWorkFlow = htworkflowbll.IsHaveCurWorkFlow("厂级问题流程"); //问题流程状态

            return new { code = 0, info = "获取数据成功", count = 0, data = new { ishavaworkflow = isHavaWorkFlow } };
        }
        #endregion

        #region 问题流程状态
        /// <summary>
        /// 问题流程状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetQuestionFlowState()
        {
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'QuestionFlowState'"); //问题流程状态

            return new { code = 0, info = "获取数据成功", count = 0, data = itemlist.Select(x => new { flowstateid = x.ItemName, flowstatename = x.ItemName }) };
        }
        #endregion

        #region 获取所有问题列表接口
        /// <summary>
        /// 获取所有问题列表接口
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetQuestionList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string tokenId = res.Contains("tokenid") ? dy.tokenid.ToString() : ""; //设备唯一标识
            int pageSize = res.Contains("pagesize") ? int.Parse(dy.pagesize.ToString()) : 10; //每页的记录数
            int pageIndex = res.Contains("pageindex") ? int.Parse(dy.pageindex.ToString()) : 1; //当前页索引
            string action = res.Contains("action") ? dy.data.action.ToString() : ""; //请求类型
            string checkid = res.Contains("checkid") ? dy.data.checkid : ""; //检查id 
            string relevanceid = res.Contains("relevanceid") ? dy.data.relevanceid : ""; //应用id 
            string correlationid = res.Contains("correlationid") ? dy.data.correlationid : ""; //应用id
            string flowstate = res.Contains("flowstate") ? dy.data.flowstate : ""; //流程状态
            string createdeptcode = res.Contains("createdeptcode") ? dy.data.createdeptcode : ""; //登记单位
            string reformdept = res.Contains("reformdept") ? dy.data.reformdept : ""; //整改单位 
            string questiondescribe = res.Contains("questiondescribe") ? dy.data.questiondescribe : "";
            string standingmark = res.Contains("standingmark") ? dy.data.standingmark : "";  //台账标记
            string tablename = string.Empty;

            tablename = string.Format(@"( 
                                            select a.*, ( case when  a.flowstate ='问题登记' then '问题登记'  when  a.flowstate ='问题整改' then '整改中' when  a.flowstate ='问题验证' then '验证中'
                                         when a.flowstate ='流程结束' then '流程结束' end ) actionstatus ,b.filepath  from v_questioninfo a
                                         left join  ( select a.id,wm_concat((case when b.filepath is not null then ('{0}'||substr(b.filepath,2)) else '' end)) filepath from bis_questioninfo a
                                                     left join base_fileinfo b on a.questionpic = b.recid  group by a. id ) b on  a.id = b.id   
                                         ) a ", dataitemdetailbll.GetItemValue("imgUrl"));

            Pagination pagination = new Pagination();
            pagination.p_tablename = tablename;
            pagination.page = pageIndex;
            pagination.rows = pageSize;
            pagination.sidx = "createdate desc,modifydate desc";
            pagination.sord = "";
            pagination.conditionJson = " 1=1 ";
            pagination.p_fields = @"belongdeptname,belongdeptid,createuserid,createuserdeptcode,createuserorgcode,to_char(createdate,'yyyy-MM-dd') createdate,createusername,questionnumber,questionaddress,questiondescribe,
                                    questionpic,checkpersonname,checkpersonid,checkdeptid,checkdeptname,checktype,checktypename,checkname,checkid,to_char(checkdate,'yyyy-MM-dd') checkdate,checkcontent,relevanceid,flowstate,
                                    qflag,appsign,username,deptcode,deptname,reformdeptid,reformdeptcode,reformdeptname,dutydeptid,dutydeptcode,dutydeptname,reformpeople,reformpeoplename,reformtel,to_char(reformplandate,'yyyy-MM-dd') reformplandate,
                                    reformdescribe,reformmeasure,reformstatus,reformstatusname,reformreason,reformsign,to_char(reformfinishdate,'yyyy-MM-dd') reformfinishdate,reformpic,verifyopinion,verifyresult,verifypeople,verifypeoplename,
                                    verifysign,verifydeptid,verifydeptcode,verifydeptname,to_char(verifydate,'yyyy-MM-dd') verifydate,questionfilepath,reformfilepath,actionperson,participantname,actionstatus,filepath";

            pagination.p_kid = "id";

            //台账标记
            if (!string.IsNullOrEmpty(standingmark))
            {
                pagination.conditionJson += @" and flowstate != '问题登记'";
            }
            //当前单位的
            pagination.conditionJson += string.Format(@" and belongdeptid  ='{0}'", curUser.OrganizeId);

            //登记单位
            if (!string.IsNullOrEmpty(createdeptcode))
            {
                pagination.conditionJson += string.Format(@" and  createuserdeptcode ='{0}' ", createdeptcode);
            }
            //整改单位编码
            if (!string.IsNullOrEmpty(reformdept))
            {
                pagination.conditionJson += string.Format(@" and  reformdeptcode ='{0}' ", reformdept);
            }
            //应用id
            if (!string.IsNullOrEmpty(relevanceid))
            {
                pagination.conditionJson += string.Format(@" and  relevanceid ='{0}' ", relevanceid);
            }
            //应用id
            if (!string.IsNullOrEmpty(correlationid))
            {
                pagination.conditionJson += string.Format(@" and  correlationid ='{0}' ", correlationid);
            }
            //检查id
            if (!string.IsNullOrEmpty(checkid))
            {
                pagination.conditionJson += string.Format(@" and  checkid ='{0}' ", checkid);
            }
            //问题描述
            if (!string.IsNullOrEmpty(questiondescribe))
            {
                pagination.conditionJson += string.Format(@" and  questiondescribe like '%{0}%' ", questiondescribe);
            }
            //流程状态
            if (!string.IsNullOrEmpty(flowstate))
            {
                if (flowstate == "已闭环")
                {
                    pagination.conditionJson += string.Format(@" and  flowstate ='流程结束' ");
                }
                else if (flowstate == "未闭环")
                {
                    pagination.conditionJson += string.Format(@" and  flowstate !='流程结束' ");
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and  flowstate ='{0}' ", flowstate);
                }
            }
            switch (action)
            {
                //未上传违章
                case "1":
                    pagination.conditionJson += string.Format(@" and  flowstate  = '问题登记'  and  createuserid ='{0}'", curUser.UserId);
                    break;
                //已上传违章
                case "2":
                    pagination.conditionJson += string.Format(@" and  flowstate  !='问题登记'  and  createuserid ='{0}' ", curUser.UserId);
                    break;
                //个人处理-待整改列表
                case "3":
                    pagination.conditionJson += string.Format(@" and  actionperson  like  '%{0}%' and flowstate  = '问题整改'  ", "," + curUser.Account + ",");
                    break;
                //个人处理-待验证列表
                case "4":
                    pagination.conditionJson += string.Format(@" and  actionperson  like  '%{0}%' and flowstate  = '问题验证'", "," + curUser.Account + ",");
                    break;
                //个人处理-逾期未整改列表
                case "5":
                    pagination.conditionJson += string.Format(@" and  actionperson  like  '%{0}%' and flowstate  = '问题整改'  and  to_date('{1}','yyyy-mm-dd hh24:mi:ss') > (reformplandate + 1)", "," + curUser.Account + ",", DateTime.Now);
                    break;
                //个人处理-已验证
                case "6":
                    pagination.conditionJson += string.Format(@" and ','||verifypeople||','  like  '%{0}%' and  flowstate='流程结束'", "," + curUser.Account + ",");
                    break;
            }
            var dt = htbaseinfobll.GetBaseInfoForApp(pagination);

            return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
        }
        #endregion

        #region 获取整改历史列表
        /// <summary>
        /// 获取整改历史列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetReformList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string questionid = res.Contains("questionid") ? dy.data.questionid : ""; //问题id 
            List<QuestionReformModel> data = new List<QuestionReformModel>(); //返回的结果集合
            //问题id
            if (!string.IsNullOrEmpty(questionid))
            {
                List<QuestionReformEntity> list = questionreformbll.GetHistoryList(questionid).OrderBy(p => p.CREATEDATE).ToList(); //排序

                foreach (QuestionReformEntity entity in list)
                {
                    QuestionReformModel model = new QuestionReformModel();
                    model.reformid = entity.ID; //整改id
                    model.questionid = entity.QUESTIONID; //问题id
                    model.reformstatus = entity.REFORMSTATUS; //整改完成情况
                    model.reformplandate = null != entity.REFORMPLANDATE ? entity.REFORMPLANDATE.Value.ToString("yyyy-MM-dd") : ""; //计划完成时间
                    model.reformfinishdate = null != entity.REFORMFINISHDATE ? entity.REFORMFINISHDATE.Value.ToString("yyyy-MM-dd") : ""; //整改完成时间
                    model.reformpeoplename = entity.REFORMPEOPLENAME; //整改人
                    model.reformtel = entity.REFORMTEL; // 整改人电话
                    model.reformdeptname = entity.REFORMDEPTNAME; //整改部门
                    model.dutydeptname = entity.DUTYDEPTNAME; //联责部门
                    model.reformmeasure = entity.REFORMMEASURE;//整改措施
                    model.reformdescribe = entity.REFORMDESCRIBE; //整改描述
                    model.reformsign = !string.IsNullOrEmpty(entity.REFORMSIGN) ? (dataitemdetailbll.GetItemValue("imgUrl") + entity.REFORMSIGN.Substring(5)) : "";  //整改签名
                    List<Photo> picdata = new List<Photo>(); //整改图片  
                    IEnumerable<FileInfoEntity> reformfile = fileInfoBLL.GetImageListByObject(entity.REFORMPIC);
                    foreach (FileInfoEntity fentity in reformfile)
                    {
                        Photo p = new Photo();
                        p.id = fentity.FileId;
                        p.filename = fentity.FileName;
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                        p.folderid = fentity.FolderId;
                        picdata.Add(p);
                    }
                    model.reformpic = picdata;
                    data.Add(model);
                }
            }

            return new { code = 0, info = "获取数据成功", count = data.Count(), data = data };
        }
        #endregion

        #region 获取问题验证历史列表
        /// <summary>
        /// 获取问题验证历史列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetVerifyList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string questionid = res.Contains("questionid") ? dy.data.questionid : ""; //问题id

            List<QuestionVerifyModel> data = new List<QuestionVerifyModel>(); //返回的结果集合

            if (!string.IsNullOrEmpty(questionid))
            {
                List<QuestionVerifyEntity> list = questionverifybll.GetHistoryList(questionid).OrderBy(p => p.CREATEDATE).ToList(); //排序

                foreach (QuestionVerifyEntity entity in list)
                {
                    QuestionVerifyModel model = new QuestionVerifyModel();
                    model.verifyid = entity.ID; //验证id
                    model.questionid = entity.QUESTIONID; //问题id
                    model.verifyresult = entity.VERIFYRESULT; //验证结果
                    model.verifypeoplename = entity.VERIFYPEOPLENAME; //验证人
                    model.verifydeptname = entity.VERIFYDEPTNAME; //验证部门
                    model.verifyopinion = entity.VERIFYOPINION; //验证意见
                    model.verifydate = null != entity.VERIFYDATE ? entity.VERIFYDATE.Value.ToString("yyyy-MM-dd") : "";//验证时间
                    model.verifysign = !string.IsNullOrEmpty(entity.VERIFYSIGN) ? (dataitemdetailbll.GetItemValue("imgUrl") + entity.VERIFYSIGN.Substring(5)) : "";  //验证签名
                    data.Add(model);
                }
            }

            return new { code = 0, info = "获取数据成功", count = data.Count(), data = data };
        }
        #endregion

        #region 获取问题详情信息
        /// <summary>
        /// 获取问题详情信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetQuestionDetail([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string questionid = res.Contains("questionid") ? dy.data.questionid : ""; //问题主键

            var baseInfo = questioninfobll.GetQuestionModel(questionid);

            QuestionModel entity = new QuestionModel();
            if (baseInfo.Rows.Count == 1)
            {
                //问题基本信息
                bool isHavaWorkFlow = htworkflowbll.IsHaveCurWorkFlow("厂级问题流程"); //问题流程状态
                entity.ishavaworkflow = isHavaWorkFlow;
                entity.questionid = baseInfo.Rows[0]["id"].ToString(); //问题id
                entity.createuserid = baseInfo.Rows[0]["createuserid"].ToString();//创建人id
                entity.createusername = baseInfo.Rows[0]["createusername"].ToString();//创建用户姓名
                entity.createdate = !string.IsNullOrEmpty(baseInfo.Rows[0]["createdate"].ToString()) ? Convert.ToDateTime(baseInfo.Rows[0]["createdate"].ToString()).ToString("yyyy-MM-dd") : "";
                entity.questionnumber = baseInfo.Rows[0]["questionnumber"].ToString();  //问题编号
                entity.belongdeptname = baseInfo.Rows[0]["belongdeptname"].ToString();//所属单位名称
                entity.belongdeptid = baseInfo.Rows[0]["belongdeptid"].ToString();//所属单位id 
                entity.questiondescribe = baseInfo.Rows[0]["questiondescribe"].ToString();//问题描述
                entity.questionaddress = baseInfo.Rows[0]["questionaddress"].ToString(); //问题地点
                string questionpic = baseInfo.Rows[0]["questionpic"].ToString();  //问题图片 
                List<Photo> questionpiclist = new List<Photo>(); //问题图片 
                IEnumerable<FileInfoEntity> lllegalfile = fileInfoBLL.GetImageListByTop5Object(questionpic);
                foreach (FileInfoEntity fentity in lllegalfile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    questionpiclist.Add(p);
                }
                entity.questionpic = questionpiclist; //问题图片 
                entity.checkpersonname = baseInfo.Rows[0]["checkpersonname"].ToString();//检查人姓名
                entity.checkpersonid = baseInfo.Rows[0]["checkpersonid"].ToString();//检查人部门id
                entity.checkdeptid = baseInfo.Rows[0]["checkdeptid"].ToString();//检查人部门id
                entity.checkdeptname = baseInfo.Rows[0]["checkdeptname"].ToString();//检查人部门名称
                entity.checktype = baseInfo.Rows[0]["checktype"].ToString(); //检查类型
                entity.checktypename = baseInfo.Rows[0]["checktypename"].ToString(); //检查类型名称
                entity.checkname = baseInfo.Rows[0]["checkname"].ToString();//检查名称
                entity.checkid = baseInfo.Rows[0]["checkid"].ToString();//检查id
                entity.checkdate = baseInfo.Rows[0]["checkdate"].ToString();//检查日期
                entity.checkcontent = baseInfo.Rows[0]["checkcontent"].ToString();//检查重点内容
                entity.relevanceid = baseInfo.Rows[0]["relevanceid"].ToString();//应用关联id(检查对象id)
                entity.correlationid = baseInfo.Rows[0]["correlationid"].ToString();//应用关联id(检查内容id)
                entity.flowstate = baseInfo.Rows[0]["flowstate"].ToString(); //流程状态
                entity.actionperson = baseInfo.Rows[0]["actionperson"].ToString();//当前流程操作人账户
                entity.participantname = baseInfo.Rows[0]["participantname"].ToString(); //当前流程操作人姓名
                /***整改信息***/
                entity.reformdeptcode = baseInfo.Rows[0]["reformdeptcode"].ToString(); //整改部门编码
                entity.reformdeptname = baseInfo.Rows[0]["reformdeptname"].ToString(); //整改部门名称
                entity.reformdeptid = baseInfo.Rows[0]["reformdeptid"].ToString(); //整改部门id
                entity.dutydeptcode = baseInfo.Rows[0]["dutydeptcode"].ToString();  //联责部门编码
                entity.dutydeptname = baseInfo.Rows[0]["dutydeptname"].ToString(); //联责部门名称
                entity.dutydeptid = baseInfo.Rows[0]["dutydeptid"].ToString(); //联责部门id
                entity.reformpeoplename = baseInfo.Rows[0]["reformpeoplename"].ToString();//整改人姓名
                entity.reformpeople = baseInfo.Rows[0]["reformpeople"].ToString();//整改人
                entity.reformtel = baseInfo.Rows[0]["reformtel"].ToString(); //整改人联系方式
                entity.reformplandate = !string.IsNullOrEmpty(baseInfo.Rows[0]["reformplandate"].ToString()) ? Convert.ToDateTime(baseInfo.Rows[0]["reformplandate"].ToString()).ToString("yyyy-MM-dd") : ""; //计划完成时间
                entity.reformmeasure = baseInfo.Rows[0]["reformmeasure"].ToString();//整改措施
                entity.reformdescribe = baseInfo.Rows[0]["reformdescribe"].ToString();//整改情况描述
                entity.reformstatus = baseInfo.Rows[0]["reformstatus"].ToString(); //整改完成情况
                entity.reformreason = baseInfo.Rows[0]["reformreason"].ToString(); //整改未完成原因
                entity.reformsign = !string.IsNullOrEmpty(baseInfo.Rows[0]["reformsign"].ToString()) ? (dataitemdetailbll.GetItemValue("imgUrl") + baseInfo.Rows[0]["reformsign"].ToString().Substring(5)) : "";  //整改签名
                entity.reformfinishdate = !string.IsNullOrEmpty(baseInfo.Rows[0]["reformfinishdate"].ToString()) ? Convert.ToDateTime(baseInfo.Rows[0]["reformfinishdate"].ToString()).ToString("yyyy-MM-dd") : "";//整改完成时间
                string reformphoto = baseInfo.Rows[0]["reformpic"].ToString();   //问题整改图片
                List<Photo> reformpic = new List<Photo>(); //整改图片
                IEnumerable<FileInfoEntity> reformfile = fileInfoBLL.GetImageListByTop5Object(reformphoto);
                foreach (FileInfoEntity fentity in reformfile)
                {
                    Photo p = new Photo();
                    p.id = fentity.FileId;
                    p.filename = fentity.FileName;
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                    p.folderid = fentity.FolderId;
                    reformpic.Add(p);
                }
                entity.reformpic = reformpic;
                /***验证信息***/
                entity.verifypeoplename = baseInfo.Rows[0]["verifypeoplename"].ToString(); //验证人姓名
                entity.verifypeople = baseInfo.Rows[0]["verifypeople"].ToString();//验证人
                entity.verifydeptname = baseInfo.Rows[0]["verifydeptname"].ToString(); //验证部门名称
                entity.verifydeptcode = baseInfo.Rows[0]["verifydeptcode"].ToString(); //验证部门编码
                entity.verifydeptid = baseInfo.Rows[0]["verifydeptid"].ToString(); //验证部门编码
                entity.verifyresult = baseInfo.Rows[0]["verifyresult"].ToString(); //验证结果
                entity.verifyopinion = baseInfo.Rows[0]["verifyopinion"].ToString();//验证意见
                entity.verifydate = !string.IsNullOrEmpty(baseInfo.Rows[0]["verifydate"].ToString()) ? Convert.ToDateTime(baseInfo.Rows[0]["verifydate"].ToString()).ToString("yyyy-MM-dd") : "";//验证时间
                entity.verifysign = !string.IsNullOrEmpty(baseInfo.Rows[0]["verifysign"].ToString()) ? (dataitemdetailbll.GetItemValue("imgUrl") + baseInfo.Rows[0]["verifysign"].ToString().Substring(5)) : "";  //验证签名
                /**流程图**/
                entity.checkflow = GetCheckFlowData(questionid, "Question");
            }

            return new { code = 0, count = 0, info = "获取成功", data = entity };
        }
        #endregion


        #endregion

        #region 问题流程部分

        #region 新增保存问题 / 提交问题
        /// <summary>
        /// 新增保存问题 / 提交问题  
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddQuestionPush()
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                string wfFlag = string.Empty;
                string participant = string.Empty;
                string startflow = string.Empty;//起始
                string endflow = string.Empty; //截止
                string questionid = res.Contains("questionid") ? dy.data.questionid : "";  //主键
                #region 问题登记信息
                QuestionInfoEntity entity = null;
                if (!string.IsNullOrEmpty(questionid))
                {
                    entity = questioninfobll.GetEntity(questionid);
                }
                if (null == entity)
                {
                    entity = new QuestionInfoEntity();

                    entity.ID = res.Contains("questionid") ? dy.data.questionid : "";  //主键
                }
                //新增时
                if (string.IsNullOrEmpty(questionid))
                {
                    entity.BELONGDEPTID = curUser.OrganizeId;
                    entity.BELONGDEPTNAME = curUser.OrganizeName;
                    entity.QUESTIONNUMBER = questioninfobll.GenerateCode("bis_questioninfo", "questionnumber", 4);
                }
                //问题基本信息                
                entity.QUESTIONDESCRIBE = res.Contains("questiondescribe") ? dy.data.questiondescribe : "";//问题描述
                entity.QUESTIONADDRESS = res.Contains("questionaddress") ? dy.data.questionaddress : "";//问题地点
                entity.CHECKPERSONNAME = res.Contains("checkpersonname") ? dy.data.checkpersonname : ""; //检查人姓名
                entity.CHECKPERSONID = res.Contains("checkpersonid") ? dy.data.checkpersonid : ""; //检查人部门id
                entity.CHECKDEPTID = res.Contains("checkdeptid") ? dy.data.checkdeptid : ""; //检查人部门id
                entity.CHECKDEPTNAME = res.Contains("checkdeptname") ? dy.data.checkdeptname : ""; //检查人部门名称
                entity.CHECKTYPE = res.Contains("checktype") ? dy.data.checktype : "";  //检查类型
                entity.CHECKNAME = res.Contains("checkname") ? dy.data.checkname : ""; //检查名称
                entity.CHECKID = res.Contains("checkid") ? dy.data.checkid : ""; //检查id
                string checkdate = res.Contains("checkdate") ? dy.data.checkdate : ""; //检查日期
                if (!string.IsNullOrEmpty(checkdate))
                {
                    entity.CHECKDATE = Convert.ToDateTime(checkdate.ToString());
                }
                entity.CHECKCONTENT = res.Contains("checkcontent") ? dy.data.checkcontent : ""; //检查重点内容
                entity.CORRELATIONID = res.Contains("correlationid") ? dy.data.correlationid : ""; //应用id
                entity.RELEVANCEID = res.Contains("relevanceid") ? dy.data.relevanceid : ""; //应用关联id
                //问题图片
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件
                /**********图片部分缺省**********/
                if (string.IsNullOrEmpty(questionid))
                {
                    entity.QUESTIONPIC = Guid.NewGuid().ToString(); //问题图片
                }
                DeleteFile(fileids);  //先删除图片
                UploadifyFile(entity.QUESTIONPIC, "questionpic", files); //上传问题图片
                /********************************/
                entity.APPSIGN = AppSign; //移动端标记
                //新增
                questioninfobll.SaveForm(questionid, entity);
                #endregion
                #region 创建主体流程
                //主键为空  创建主体流程
                if (string.IsNullOrEmpty(questionid))
                {
                    bool isSucess = htworkflowbll.CreateWorkFlowObj("09", entity.ID, curUser.UserId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", entity.ID);  //更新业务流程状态
                    }
                }
                #endregion
                #region 问题整改信息
                QuestionReformEntity centity = new QuestionReformEntity();
                string reformid = res.Contains("reformid") ? dy.data.reformid : "";  //整改id
                if (!string.IsNullOrEmpty(questionid))
                {
                    centity = questionreformbll.GetEntityByBid(questionid);
                }
                if (null != centity)
                {
                    reformid = centity.ID;
                }
                centity.QUESTIONID = entity.ID; //问题id
                centity.REFORMDEPTCODE = res.Contains("reformdeptcode") ? dy.data.reformdeptcode : "";  //整改部门编码
                centity.REFORMDEPTNAME = res.Contains("reformdeptname") ? dy.data.reformdeptname : "";  //整改部门名称
                centity.REFORMDEPTID = res.Contains("reformdeptid") ? dy.data.reformdeptid : "";  //整改部门ID
                centity.DUTYDEPTCODE = res.Contains("dutydeptcode") ? dy.data.dutydeptcode : "";  //联责部门编码
                centity.DUTYDEPTNAME = res.Contains("dutydeptname") ? dy.data.dutydeptname : "";  //联责部门名称
                centity.DUTYDEPTID = res.Contains("dutydeptid") ? dy.data.dutydeptid : "";  //联责部门ID
                centity.REFORMPEOPLE = res.Contains("reformpeople") ? dy.data.reformpeople : "";  //整改人
                centity.REFORMPEOPLENAME = res.Contains("reformpeoplename") ? dy.data.reformpeoplename : "";  //整改人姓名
                centity.REFORMTEL = res.Contains("reformtel") ? dy.data.reformtel : "";  //整改人电话
                string reformplandate = res.Contains("reformplandate") ? dy.data.reformplandate : null; //计划完成时间
                if (!string.IsNullOrEmpty(reformplandate))
                {
                    centity.REFORMPLANDATE = Convert.ToDateTime(reformplandate); //计划完成时间
                }
                centity.REFORMMEASURE = res.Contains("reformmeasure") ? dy.data.reformmeasure : "";//整改措施 
                centity.REFORMDESCRIBE = res.Contains("reformdescribe") ? dy.data.reformdescribe : "";//整改情况描述 
                centity.REFORMSTATUS = res.Contains("reformstatus") ? dy.data.reformstatus : "";//整改完成情况 
                centity.REFORMREASON = res.Contains("reformreason") ? dy.data.reformreason : ""; //整改未完成情况 
                centity.REFORMSIGN = res.Contains("reformsign") ? dy.data.reformsign : "";//整改签名 

                string reformfinishdate = res.Contains("reformfinishdate") ? dy.data.reformfinishdate : null;  //整改完成时间
                if (!string.IsNullOrEmpty(reformfinishdate))
                {
                    centity.REFORMFINISHDATE = Convert.ToDateTime(reformfinishdate);
                }
                if (string.IsNullOrEmpty(centity.REFORMPIC))//图片
                {
                    centity.REFORMPIC = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                UploadifyFile(centity.REFORMPIC, "reformpic", files);                //上传问题整改图片
                /********************************/
                centity.APPSIGN = AppSign; //移动端标记
                questionreformbll.SaveForm(reformid, centity);

                #endregion
                #region 问题验证信息
                bool isHavaWorkFlow = htworkflowbll.IsHaveCurWorkFlow("厂级问题流程");
                //当前没有配置流程，则新加验证信息
                if (!isHavaWorkFlow)
                {
                    QuestionVerifyEntity aentity = new QuestionVerifyEntity();
                    string verifyid = res.Contains("verifyid") ? dy.data.verifyid : "";  //验证id
                    if (!string.IsNullOrEmpty(questionid))
                    {
                        aentity = questionverifybll.GetEntityByBid(questionid);
                    }
                    if (null != aentity)
                    {
                        verifyid = aentity.ID;
                    }
                    aentity.QUESTIONID = entity.ID;
                    aentity.VERIFYPEOPLENAME = res.Contains("verifypeoplename") ? dy.data.verifypeoplename : ""; //验证人姓名
                    aentity.VERIFYPEOPLE = res.Contains("verifypeople") ? dy.data.verifypeople : ""; //验证人
                    aentity.VERIFYDEPTNAME = res.Contains("verifydeptname") ? dy.data.verifydeptname : ""; //验证部门名称
                    aentity.VERIFYDEPTCODE = res.Contains("verifydeptcode") ? dy.data.verifydeptcode : ""; //验证部门编码
                    aentity.VERIFYDEPTID = res.Contains("verifydeptid") ? dy.data.verifydeptid : ""; //验证部门id
                    aentity.VERIFYRESULT = res.Contains("verifyresult") ? dy.data.verifyresult : ""; //验证结果
                    aentity.VERIFYOPINION = res.Contains("acceptmind") ? dy.data.acceptmind : ""; //验证意见
                    string verifydate = res.Contains("verifydate") ? dy.data.verifydate : "";
                    if (!string.IsNullOrEmpty(verifydate))
                    {
                        aentity.VERIFYDATE = Convert.ToDateTime(verifydate);
                    }
                    aentity.VERIFYSIGN = res.Contains("verifysign") ? dy.data.verifysign : ""; //验证签名
                    aentity.APPSIGN = AppSign; //移动端标记
                    questionverifybll.SaveForm(verifyid, aentity);
                }
                #endregion
                #region 流程控制

                wfFlag = "1"; //到整改

                participant = centity.REFORMPEOPLE;  //整改人

                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(entity.ID, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", entity.ID);  //更新业务流程状态
                    }
                }
                else
                {
                    return new { code = -1, count = 0, info = "请选择整改责任人!" };
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message + ",异常目标:" + ex.InnerException.ToString() };
            }
            return new { code = 0, count = 0, info = "提交成功" };
        }
        #endregion

        #region 保存问题信息  安全检查专用
        /// <summary>
        /// 保存问题信息  安全检查专用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveQuestion()
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                string wfFlag = string.Empty;
                string participant = string.Empty;
                string startflow = string.Empty;//起始
                string endflow = string.Empty; //截止

                string questionid = res.Contains("questionid") ? dy.data.questionid : "";  //主键
                #region 问题登记信息
                QuestionInfoEntity entity = null;
                if (!string.IsNullOrEmpty(questionid))
                {
                    entity = questioninfobll.GetEntity(questionid);
                }
                if (null == entity)
                {
                    entity = new QuestionInfoEntity();

                    entity.ID = res.Contains("questionid") ? dy.data.questionid : "";  //主键
                }
                //新增时
                if (string.IsNullOrEmpty(questionid))
                {
                    entity.BELONGDEPTID = curUser.OrganizeId;
                    entity.BELONGDEPTNAME = curUser.OrganizeName;
                    entity.QUESTIONNUMBER = questioninfobll.GenerateCode("bis_questioninfo", "questionnumber", 4);
                }
                //问题基本信息                
                entity.QUESTIONDESCRIBE = res.Contains("questiondescribe") ? dy.data.questiondescribe : "";//问题描述
                entity.QUESTIONADDRESS = res.Contains("questionaddress") ? dy.data.questionaddress : "";//问题地点
                entity.CHECKPERSONNAME = res.Contains("checkpersonname") ? dy.data.checkpersonname : ""; //检查人姓名
                entity.CHECKPERSONID = res.Contains("checkpersonid") ? dy.data.checkpersonid : ""; //检查人部门id
                entity.CHECKDEPTID = res.Contains("checkdeptid") ? dy.data.checkdeptid : ""; //检查人部门id
                entity.CHECKDEPTNAME = res.Contains("checkdeptname") ? dy.data.checkdeptname : ""; //检查人部门名称
                entity.CHECKTYPE = res.Contains("checktype") ? dy.data.checktype : "";  //检查类型
                entity.CHECKNAME = res.Contains("checkname") ? dy.data.checkname : ""; //检查名称
                entity.CHECKID = res.Contains("checkid") ? dy.data.checkid : ""; //检查id
                string checkdate = res.Contains("checkdate") ? dy.data.checkdate : ""; //检查日期
                if (!string.IsNullOrEmpty(checkdate))
                {
                    entity.CHECKDATE = Convert.ToDateTime(checkdate.ToString());
                }
                entity.CHECKCONTENT = res.Contains("checkcontent") ? dy.data.checkcontent : ""; //检查重点内容
                entity.CORRELATIONID = res.Contains("correlationid") ? dy.data.correlationid : ""; //应用id
                entity.RELEVANCEID = res.Contains("relevanceid") ? dy.data.relevanceid : ""; //应用关联id
                //问题图片
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件
                /**********图片部分缺省**********/
                if (string.IsNullOrEmpty(questionid))
                {
                    entity.QUESTIONPIC = Guid.NewGuid().ToString(); //问题图片
                }
                DeleteFile(fileids);  //先删除图片
                UploadifyFile(entity.QUESTIONPIC, "questionpic", files); //上传问题图片
                /********************************/
                entity.APPSIGN = AppSign; //移动端标记
                //新增
                questioninfobll.SaveForm(questionid, entity);
                #endregion
                #region 创建主体流程
                //主键为空  创建主体流程
                if (string.IsNullOrEmpty(questionid))
                {
                    bool isSucess = htworkflowbll.CreateWorkFlowObj("09", entity.ID, curUser.UserId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", entity.ID);  //更新业务流程状态
                    }
                }
                #endregion
                #region 问题整改信息
                QuestionReformEntity centity = new QuestionReformEntity();
                string reformid = res.Contains("reformid") ? dy.data.reformid : "";  //整改id
                if (!string.IsNullOrEmpty(questionid))
                {
                    centity = questionreformbll.GetEntityByBid(questionid);
                }
                if (null != centity)
                {
                    reformid = centity.ID;
                }
                centity.QUESTIONID = entity.ID; //问题id
                centity.REFORMDEPTCODE = res.Contains("reformdeptcode") ? dy.data.reformdeptcode : "";  //整改部门编码
                centity.REFORMDEPTNAME = res.Contains("reformdeptname") ? dy.data.reformdeptname : "";  //整改部门名称
                centity.REFORMDEPTID = res.Contains("reformdeptid") ? dy.data.reformdeptid : "";  //整改部门ID
                centity.DUTYDEPTCODE = res.Contains("dutydeptcode") ? dy.data.dutydeptcode : "";  //联责部门编码
                centity.DUTYDEPTNAME = res.Contains("dutydeptname") ? dy.data.dutydeptname : "";  //联责部门名称
                centity.DUTYDEPTID = res.Contains("dutydeptid") ? dy.data.dutydeptid : "";  //联责部门ID
                centity.REFORMPEOPLE = res.Contains("reformpeople") ? dy.data.reformpeople : "";  //整改人
                centity.REFORMPEOPLENAME = res.Contains("reformpeoplename") ? dy.data.reformpeoplename : "";  //整改人姓名
                centity.REFORMTEL = res.Contains("reformtel") ? dy.data.reformtel : "";  //整改人电话
                string reformplandate = res.Contains("reformplandate") ? dy.data.reformplandate : null; //计划完成时间
                if (!string.IsNullOrEmpty(reformplandate))
                {
                    centity.REFORMPLANDATE = Convert.ToDateTime(reformplandate); //计划完成时间
                }
                centity.REFORMMEASURE = res.Contains("reformmeasure") ? dy.data.reformmeasure : "";//整改措施 
                centity.REFORMDESCRIBE = res.Contains("reformdescribe") ? dy.data.reformdescribe : "";//整改情况描述 
                centity.REFORMSTATUS = res.Contains("reformstatus") ? dy.data.reformstatus : "";//整改完成情况 
                centity.REFORMREASON = res.Contains("reformreason") ? dy.data.reformreason : ""; //整改未完成情况 
                centity.REFORMSIGN = res.Contains("reformsign") ? dy.data.reformsign : "";//整改签名 
                string reformfinishdate = res.Contains("reformfinishdate") ? dy.data.reformfinishdate : null;  //整改完成时间
                if (!string.IsNullOrEmpty(reformfinishdate))
                {
                    centity.REFORMFINISHDATE = Convert.ToDateTime(reformfinishdate);
                }
                if (string.IsNullOrEmpty(centity.REFORMPIC))//图片
                {
                    centity.REFORMPIC = Guid.NewGuid().ToString();
                }
                /**********图片部分缺省**********/
                UploadifyFile(centity.REFORMPIC, "reformpic", files);                //上传问题整改图片
                /********************************/
                centity.APPSIGN = AppSign; //移动端标记
                questionreformbll.SaveForm(reformid, centity);

                #endregion
                #region 问题验证信息
                bool isHavaWorkFlow = htworkflowbll.IsHaveCurWorkFlow("厂级问题流程");
                //当前没有配置流程，则新加验证信息
                if (!isHavaWorkFlow)
                {
                    QuestionVerifyEntity aentity = new QuestionVerifyEntity();
                    string verifyid = res.Contains("verifyid") ? dy.data.verifyid : "";  //验证id
                    if (!string.IsNullOrEmpty(questionid))
                    {
                        aentity = questionverifybll.GetEntityByBid(questionid);
                    }
                    if (null != aentity)
                    {
                        verifyid = aentity.ID;
                    }
                    aentity.QUESTIONID = entity.ID;
                    aentity.VERIFYPEOPLENAME = res.Contains("verifypeoplename") ? dy.data.verifypeoplename : ""; //验证人姓名
                    aentity.VERIFYPEOPLE = res.Contains("verifypeople") ? dy.data.verifypeople : ""; //验证人
                    aentity.VERIFYDEPTNAME = res.Contains("verifydeptname") ? dy.data.verifydeptname : ""; //验证部门名称
                    aentity.VERIFYDEPTCODE = res.Contains("verifydeptcode") ? dy.data.verifydeptcode : ""; //验证部门编码
                    aentity.VERIFYDEPTID = res.Contains("verifydeptid") ? dy.data.verifydeptid : ""; //验证部门id
                    aentity.VERIFYRESULT = res.Contains("verifyresult") ? dy.data.verifyresult : ""; //验证结果
                    aentity.VERIFYOPINION = res.Contains("acceptmind") ? dy.data.acceptmind : ""; //验证意见
                    string verifydate = res.Contains("verifydate") ? dy.data.verifydate : "";
                    if (!string.IsNullOrEmpty(verifydate))
                    {
                        aentity.VERIFYDATE = Convert.ToDateTime(verifydate);
                    }
                    aentity.VERIFYSIGN = res.Contains("verifysign") ? dy.data.verifysign : ""; //验证签名
                    aentity.APPSIGN = AppSign; //移动端标记
                    questionverifybll.SaveForm(verifyid, aentity);
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message + ",异常目标:" + ex.InnerException.ToString() };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 问题整改
        /// <summary>
        /// 问题整改
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddReformPush()
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                WfControlResult result = new WfControlResult();
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                string participant = string.Empty;
                string wfFlag = string.Empty;  //流程标识
                string questionid = res.Contains("questionid") ? dy.data.questionid : "";  //主键
                QuestionInfoEntity entity = questioninfobll.GetEntity(questionid);

                #region 违章整改信息
                QuestionReformEntity centity = new QuestionReformEntity();
                string reformid = "";  //整改id
                if (!string.IsNullOrEmpty(questionid))
                {
                    centity = questionreformbll.GetEntityByBid(questionid);
                }
                if (null != centity)
                {
                    reformid = centity.ID;
                }
                centity.QUESTIONID = questionid; //整改id 

                string verifypeople = res.Contains("verifypeople") ? dy.data.verifypeople : "";

                centity.REFORMDESCRIBE = res.Contains("reformdescribe") ? dy.data.reformdescribe : ""; //整改情况描述
                centity.REFORMSTATUS = res.Contains("reformstatus") ? dy.data.reformstatus : ""; //整改完成情况
                string reformsign = res.Contains("reformsign") ? dy.data.reformsign.ToString() : "";//整改责任人签名 
                centity.REFORMSIGN = "../../Resource/sign" + reformsign.Substring(reformsign.LastIndexOf("/"));
                string reformfinishdate = res.Contains("reformfinishdate") ? dy.data.reformfinishdate : null;//整改结束时间
                if (!string.IsNullOrEmpty(reformfinishdate))
                {
                    centity.REFORMFINISHDATE = Convert.ToDateTime(reformfinishdate);
                }
                //问题整改图片
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件
                //先删除图片
                DeleteFile(fileids);

                /**********图片部分缺省**********/
                //上传问题整改图片
                //图片
                if (string.IsNullOrEmpty(centity.REFORMPIC))
                {
                    centity.REFORMPIC = Guid.NewGuid().ToString();
                }
                UploadifyFile(centity.REFORMPIC, "reformpic", files);
                /********************************/
                //更改
                centity.APPSIGN = AppSign; //移动端标记
                #endregion

                #region 流程推进


                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = questionid;
                wfentity.startflow = "问题整改";
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.organizeid = entity.BELONGDEPTID; //对应电厂id
                wfentity.argument1 = curUser.UserId;
                wfentity.mark = "厂级问题流程";
                wfentity.submittype = "提交";

                //获取下一流程的操作人
                result = wfcontrolbll.GetWfControl(wfentity);
                //处理成功
                if (result.code == WfCode.Sucess)
                {
                    //参与者
                    participant = result.actionperson;
                    //状态
                    wfFlag = result.wfflag;

                    //提交流程到下一节点
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, questionid, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            //更新整改内容
                            //centity.REFORMDEPTCODE = curUser.DeptCode; //当前人
                            //centity.REFORMDEPTID = curUser.DeptId;
                            //centity.REFORMDEPTNAME = curUser.DeptName;
                            centity.REFORMPEOPLE = curUser.Account;
                            centity.REFORMPEOPLENAME = curUser.UserName;
                            questionreformbll.SaveForm(reformid, centity);

                            htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", questionid);  //更新业务流程状态
                        }
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "请联系系统管理员，确认提交问题!" };
                    }
                }
                else if (result.code == WfCode.NoInstance || result.code == WfCode.NoEnable)
                {
                    wfFlag = "1"; //到验证阶段

                    participant = verifypeople;  //验证人

                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(questionid, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            //更新整改内容
                            //centity.REFORMDEPTCODE = curUser.DeptCode; //当前人
                            //centity.REFORMDEPTID = curUser.DeptId;
                            //centity.REFORMDEPTNAME = curUser.DeptName;
                            centity.REFORMPEOPLE = curUser.Account;
                            centity.REFORMPEOPLENAME = curUser.UserName;
                            questionreformbll.SaveForm(reformid, centity);

                            htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", questionid);  //更新业务流程状态
                        }
                        return new { code = 0, count = 0, info = "保存成功" };
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "操作失败,请联系管理员!" };
                    }
                }

                if (result.code == WfCode.Sucess)
                {
                    return new { code = 0, count = 0, info = "保存成功" };
                }
                else //其他返回状态
                {
                    return new { code = -1, count = 0, info = result.message };
                }
                #endregion
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

        }
        #endregion

        #region  问题验证
        /// <summary>
        /// 问题验证
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddVerifyPush()
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            WfControlResult result = new WfControlResult();
            HttpFileCollection files = ctx.Request.Files;//上传的文件 
            string questionid = res.Contains("questionid") ? dy.data.questionid : "";  //主键 
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员 (取验收人)
            try
            {
                #region 问题验证信息
                QuestionInfoEntity baseEntity = questioninfobll.GetEntity(questionid); //问题基本信息
                QuestionReformEntity reformEntity = questionreformbll.GetEntityByBid(questionid); //问题整改信息


                QuestionVerifyEntity aptEntity = new QuestionVerifyEntity();
                if (!string.IsNullOrEmpty(questionid))
                {
                    aptEntity = questionverifybll.GetEntityByBid(questionid);
                    if (null == aptEntity)
                    {
                        aptEntity = new QuestionVerifyEntity();
                        aptEntity.QUESTIONID = questionid;//关联id
                    }
                    aptEntity.APPSIGN = AppSign;
                }
                aptEntity.VERIFYRESULT = res.Contains("verifyresult") ? dy.data.verifyresult : "";  //验收结果
                aptEntity.VERIFYOPINION = res.Contains("verifyopinion") ? dy.data.verifyopinion : ""; //验收意见
                string verifydate = res.Contains("verifydate") ? dy.data.verifydate : "";
                if (!string.IsNullOrEmpty(verifydate))
                {
                    aptEntity.VERIFYDATE = Convert.ToDateTime(verifydate);
                }
                string verifysign = res.Contains("verifysign") ? dy.data.verifysign.ToString() : "";//验证人签名 
                aptEntity.VERIFYSIGN = "../../Resource/sign" + verifysign.Substring(verifysign.LastIndexOf("/"));
                /********************************/
                #endregion
                #region 流程推进
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = questionid;
                wfentity.startflow = baseEntity.FLOWSTATE;
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.argument1 = curUser.UserId;
                wfentity.organizeid = baseEntity.BELONGDEPTID; //对应电厂id
                wfentity.mark = "厂级问题流程"; //厂级
                //验证通过
                if (aptEntity.VERIFYRESULT == "1")
                {
                    wfentity.submittype = "提交";
                }
                else //验证不通过
                {
                    wfentity.submittype = "退回";
                }

                //获取下一流程的操作人
                result = wfcontrolbll.GetWfControl(wfentity);

                //返回操作结果成功
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;

                    wfFlag = result.wfflag;

                    //提交流程到下一节点
                    #region 提交流程到下一节点
                    //如果是更改状态
                    #region 如果是更改状态
                    if (result.ischangestatus)
                    {
                        //提交流程到下一节点
                        if (!string.IsNullOrEmpty(participant))
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, questionid, participant, wfFlag, curUser.UserId);
                            if (count > 0)
                            {
                                //添加问题验证记录
                                aptEntity.ID = null;
                                aptEntity.VERIFYPEOPLENAME = curUser.UserName; //验收人id
                                aptEntity.VERIFYPEOPLE = curUser.Account; //验收人姓名
                                aptEntity.VERIFYDEPTNAME = curUser.DeptName; //验收部门名称
                                aptEntity.VERIFYDEPTCODE = curUser.DeptCode; //验收部门编码
                                aptEntity.VERIFYDEPTID = curUser.DeptId; //验收部门id
                                questionverifybll.SaveForm("", aptEntity);

                                //退回则重新添加验证记录
                                if (wfFlag == "2")
                                {
                                    QuestionReformEntity newEntity = new QuestionReformEntity();
                                    newEntity = reformEntity;
                                    newEntity.CREATEDATE = DateTime.Now;
                                    newEntity.MODIFYDATE = DateTime.Now;
                                    newEntity.MODIFYUSERID = curUser.UserId;
                                    newEntity.MODIFYUSERNAME = curUser.UserName;
                                    newEntity.REFORMPIC = null; //重新生成图片GUID
                                    newEntity.REFORMSTATUS = null; //整改完成情况
                                    newEntity.REFORMDESCRIBE = null; //整改情况描述
                                    newEntity.REFORMFINISHDATE = null; //整改完成时间
                                    newEntity.ID = "";
                                    questionreformbll.SaveForm("", newEntity);
                                    //验证记录记录
                                    QuestionVerifyEntity cptEntity = new QuestionVerifyEntity();
                                    cptEntity = aptEntity;
                                    cptEntity.ID = null;
                                    cptEntity.CREATEDATE = DateTime.Now;
                                    cptEntity.MODIFYDATE = DateTime.Now;
                                    cptEntity.VERIFYRESULT = null;
                                    cptEntity.VERIFYOPINION = null;
                                    cptEntity.VERIFYSIGN = null;
                                    questionverifybll.SaveForm("", cptEntity);
                                }
                                htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", questionid);  //更新业务流程状态
                            }
                        }
                        else
                        {
                            return new { code = -1, count = 0, info = "操作失败,请联系管理员!" };
                        }
                    }
                    #endregion
                    #region 不更改状态的情况下
                    else  //不更改状态的情况下
                    {
                        //保存隐患评估信息
                        #region 提交流程到下一节点
                        if (!string.IsNullOrEmpty(participant))
                        {
                            //添加问题验证记录
                            aptEntity.ID = null;
                            aptEntity.VERIFYDEPTID = curUser.DeptId;
                            aptEntity.VERIFYDEPTCODE = curUser.DeptCode;
                            aptEntity.VERIFYDEPTNAME = curUser.DeptName;
                            aptEntity.VERIFYPEOPLE = curUser.Account;
                            aptEntity.VERIFYPEOPLENAME = curUser.UserName;
                            questionverifybll.SaveForm("", aptEntity);

                            htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, questionid, participant, curUser.UserId);
                        }
                        else
                        {
                            return new { code = -1, count = 0, info = "操作失败,请联系管理员!" };
                        }
                        #endregion
                    }
                    #endregion
                    #endregion
                }
                //不按照配置
                else if (result.code == WfCode.NoInstance || result.code == WfCode.NoEnable)
                {
                    //验证通过
                    if (aptEntity.VERIFYRESULT == "1")
                    {
                        wfFlag = "1";

                        participant = curUser.Account;
                    }
                    else //验证不通过
                    {
                        wfFlag = "2";

                        participant = reformEntity.REFORMPEOPLE; //整改人
                    }

                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(questionid, participant, wfFlag, curUser.UserId, wfentity.submittype);

                        if (count > 0)
                        {
                            //添加问题验证记录
                            aptEntity.VERIFYDEPTID = curUser.DeptId;
                            aptEntity.VERIFYDEPTCODE = curUser.DeptCode;
                            aptEntity.VERIFYDEPTNAME = curUser.DeptName;
                            aptEntity.VERIFYPEOPLE = curUser.Account;
                            aptEntity.VERIFYPEOPLENAME = curUser.UserName;
                            questionverifybll.SaveForm(aptEntity.ID, aptEntity);

                            //退回则重新添加验证记录
                            if (wfFlag == "2")
                            {
                                QuestionReformEntity newEntity = new QuestionReformEntity();
                                newEntity = reformEntity;
                                newEntity.CREATEDATE = DateTime.Now;
                                newEntity.MODIFYDATE = DateTime.Now;
                                newEntity.MODIFYUSERID = curUser.UserId;
                                newEntity.MODIFYUSERNAME = curUser.UserName;
                                newEntity.REFORMPIC = null; //重新生成图片GUID
                                newEntity.REFORMSTATUS = null; //整改完成情况
                                newEntity.REFORMDESCRIBE = null; //整改情况描述
                                newEntity.REFORMFINISHDATE = null; //整改完成时间
                                newEntity.ID = "";
                                questionreformbll.SaveForm("", newEntity);
                                //验证记录记录
                                QuestionVerifyEntity cptEntity = new QuestionVerifyEntity();
                                cptEntity = aptEntity;
                                cptEntity.ID = null;
                                cptEntity.CREATEDATE = DateTime.Now;
                                cptEntity.MODIFYDATE = DateTime.Now;
                                cptEntity.VERIFYRESULT = null;
                                cptEntity.VERIFYOPINION = null;
                                cptEntity.VERIFYSIGN = null;
                                questionverifybll.SaveForm("", cptEntity);
                            }


                            htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", questionid);  //更新业务流程状态
                        }
                        result.message = "操作成功!";
                    }
                    else
                    {
                        return new { code = -1, count = 0, info = "操作失败,请联系管理员!" };
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
            return new { code = 0, count = 0, info = result.message };
        }
        #endregion

        #region  重新指定问题验证
        /// <summary>
        /// 重新指定问题验证
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object PointVerifyPush()
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            WfControlResult result = new WfControlResult();
            string questionid = res.Contains("questionid") ? dy.data.questionid : "";  //主键 
            string wfFlag = string.Empty;  //流程标识
            string participant = string.Empty;  //获取流程下一节点的参与人员 (取验收人)
            try
            {
                #region 问题验证信息
                QuestionInfoEntity baseEntity = questioninfobll.GetEntity(questionid); //问题基本信息
                QuestionReformEntity reformEntity = questionreformbll.GetEntityByBid(questionid); //问题整改信息

                QuestionVerifyEntity aptEntity = new QuestionVerifyEntity();
                aptEntity.QUESTIONID = questionid;
                aptEntity.VERIFYPEOPLENAME = res.Contains("verifypeoplename") ? dy.data.verifypeoplename : ""; //验证人姓名
                aptEntity.VERIFYPEOPLE = res.Contains("verifypeople") ? dy.data.verifypeople : ""; //验证人
                aptEntity.VERIFYDEPTNAME = res.Contains("verifydeptname") ? dy.data.verifydeptname : ""; //验证部门名称
                aptEntity.VERIFYDEPTCODE = res.Contains("verifydeptcode") ? dy.data.verifydeptcode : ""; //验证部门编码
                aptEntity.VERIFYDEPTID = res.Contains("verifydeptid") ? dy.data.verifydeptid : ""; //验证部门id
                string verifydate = res.Contains("verifydate") ? dy.data.verifydate : "";
                if (!string.IsNullOrEmpty(verifydate))
                {
                    aptEntity.VERIFYDATE = Convert.ToDateTime(verifydate);
                }
                aptEntity.VERIFYSIGN = res.Contains("verifysign") ? dy.data.verifysign : "";
                aptEntity.APPSIGN = AppSign;
                /********************************/
                #endregion

                wfFlag = "1";

                participant = aptEntity.VERIFYPEOPLE;

                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(questionid, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        //添加问题验证信息
                        questionverifybll.SaveForm("", aptEntity);

                        htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", questionid);  //更新业务流程状态
                    }
                    result.message = "操作成功!";
                }
                else
                {
                    return new { code = -1, count = 0, info = "操作失败,请联系管理员!" };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
            return new { code = 0, count = 0, info = result.message };
        }
        #endregion

        #endregion

        #region  图片上传
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

                        if (fileList.AllKeys[i].Contains(foldername))
                        {
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
                            }
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
                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = 4;
                logEntity.OperateTypeId = ((int)OperationType.Exception).ToString();
                logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Exception);
                logEntity.OperateAccount = curUser.UserName;
                logEntity.OperateUserId = curUser.UserId;
                logEntity.ExecuteResult = -1;
                logEntity.ExecuteResultJson = ex.Message;
                logEntity.Module = SystemInfo.CurrentModuleName;
                logEntity.ModuleId = SystemInfo.CurrentModuleId;
                logEntity.WriteLog();
            }
        }
        #endregion

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

        #region 流程图
        /// <summary>
        /// 流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public List<ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData> GetCheckFlowData(string keyValue, string mode)
        {
            var flowdt = htworkflowbll.QueryWorkFlowMapForApp(keyValue, mode);
            List<ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData> checkflow = new List<ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData>();
            if (flowdt.Rows.Count > 1)
            {
                //已经处理的部分
                foreach (DataRow row in flowdt.Rows)
                {
                    ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData checkentity = new Entity.HighRiskWork.ViewModel.CheckFlowData();
                    //如果当前起始为空，则是登记阶段
                    if (!string.IsNullOrEmpty(row["fromname"].ToString()))
                    {
                        checkentity.isoperate = "0"; //是否正在处理  0 否 1 是
                        checkentity.isapprove = "1"; //是否已经处理过 0 否 1 是
                        checkentity.auditdate = row["createdate"].ToString();
                        checkentity.auditdeptname = row["deptname"].ToString();
                        checkentity.auditstate = row["fromname"].ToString() + "已处理";
                        checkentity.auditusername = row["username"].ToString();
                        checkentity.auditremark = row["contents"].ToString();
                        checkflow.Add(checkentity);
                    }
                }
                //最后结点
                string lastflow = flowdt.Rows[flowdt.Rows.Count - 1]["toname"].ToString();
                ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData checkflowdata = new Entity.HighRiskWork.ViewModel.CheckFlowData();
                if (lastflow == "整改结束" || lastflow == "流程结束")
                {
                    checkflowdata.isoperate = "0"; //是否正在处理  0 否 1 是
                    checkflowdata.isapprove = "1"; //是否已经处理过 0 否 1 是
                    checkflowdata.auditdate = flowdt.Rows[flowdt.Rows.Count - 1]["createdate"].ToString();
                    checkflowdata.auditdeptname = flowdt.Rows[flowdt.Rows.Count - 1]["deptname"].ToString();
                    checkflowdata.auditstate = lastflow;
                    checkflowdata.auditusername = flowdt.Rows[flowdt.Rows.Count - 1]["username"].ToString();
                    checkflowdata.auditremark = flowdt.Rows[flowdt.Rows.Count - 1]["contents"].ToString();
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastflow))
                    {
                        checkflowdata.isoperate = "1"; //是否正在处理  0 否 1 是
                        checkflowdata.isapprove = "0"; //是否已经处理过 0 否 1 是
                        checkflowdata.auditdate = null;
                        checkflowdata.auditstate = lastflow + "处理中";
                        checkflowdata.auditusername = flowdt.Rows[flowdt.Rows.Count - 1]["participantname"].ToString();
                        checkflowdata.auditremark = string.Empty;
                        string[] lastuser = flowdt.Rows[flowdt.Rows.Count - 1]["participant"].ToString().Replace("$", "").ToString().Split(',');
                        string newuserStr = "";
                        foreach (string s in lastuser)
                        {
                            newuserStr += "'" + s + "',";
                        }
                        if (!string.IsNullOrEmpty(newuserStr))
                        {
                            newuserStr = newuserStr.Substring(0, newuserStr.Length - 1).ToString();
                        }
                        string newSql = string.Format(@"select a.userid,a.account,a.realname, b.fullname from base_user a 
                                        left join (
                                           select departmentid,encode, fullname from base_department 
                                        ) b on  a.departmentcode = b.encode where  a.account in ({0})", newuserStr);
                        var newDt = htbaseinfobll.GetGeneralQueryBySql(newSql);
                        string lastNodeDept = ",";
                        foreach (DataRow lastNodeRow in newDt.Rows)
                        {
                            string tempDept = "," + lastNodeRow["fullname"].ToString() + ",";
                            if (!lastNodeDept.Contains(tempDept))
                            {
                                lastNodeDept += lastNodeRow["fullname"].ToString() + ",";
                            }
                        }
                        if (!string.IsNullOrEmpty(lastNodeDept) && lastNodeDept != ",")
                        {
                            lastNodeDept = lastNodeDept.Substring(1, lastNodeDept.Length - 2).ToString();
                        }
                        else
                        {
                            lastNodeDept = "";
                        }
                        checkflowdata.auditdeptname = lastNodeDept;
                    }
                }
                checkflow.Add(checkflowdata);
            }
            else //仅有登记记录 
            {
                if (!string.IsNullOrEmpty(flowdt.Rows[0]["toname"].ToString()))
                {
                    ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData checkflowdata = new Entity.HighRiskWork.ViewModel.CheckFlowData();
                    checkflowdata.isoperate = "0"; //是否正在处理  0 否 1 是
                    checkflowdata.isapprove = "1"; //是否已经处理过 0 否 1 是
                    checkflowdata.auditdate = flowdt.Rows[0]["createdate"].ToString();
                    checkflowdata.auditdeptname = flowdt.Rows[0]["deptname"].ToString();
                    checkflowdata.auditstate = flowdt.Rows[0]["toname"].ToString() + "已处理";
                    checkflowdata.auditusername = flowdt.Rows[0]["username"].ToString();
                    checkflowdata.auditremark = flowdt.Rows[0]["contents"].ToString();
                    checkflow.Add(checkflowdata);
                }
            }
            return checkflow;
        }
        #endregion


        /**************************发现问题***************************/

        #region 发现问题流程状态
        /// <summary>
        /// 发现问题流程状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetFindQuestionFlowState() 
        {
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'FindQuestionFlowState'"); //问题流程状态

            return new { code = 0, info = "获取数据成功", count = 0, data = itemlist.Select(x => new { flowstateid = x.ItemValue, flowstatename = x.ItemName }) };
        }
        #endregion


        #region 获取所有发现问题列表接口
        /// <summary>
        /// 获取所有问题列表接口
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetFindQuestionList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string tokenId = res.Contains("tokenid") ? dy.tokenid.ToString() : ""; //设备唯一标识
            int pageSize = res.Contains("pagesize") ? int.Parse(dy.pagesize.ToString()) : 10; //每页的记录数
            int pageIndex = res.Contains("pageindex") ? int.Parse(dy.pageindex.ToString()) : 1; //当前页索引
            string action = res.Contains("action") ? dy.data.action.ToString() : ""; //请求类型

            string flowstate = res.Contains("flowstate") ? dy.data.flowstate : ""; //流程状态
            string deptid = res.Contains("deptid") ? dy.data.deptid : ""; //所属部门
            string questioncontent = res.Contains("questioncontent") ? dy.data.questioncontent : ""; //问题内容
            string stdate = res.Contains("stdate") ? dy.data.stdate : ""; //开始时间
            string etdate = res.Contains("etdate") ? dy.data.etdate : ""; //结束时间

            Pagination pagination = new Pagination();
            pagination.page = pageIndex;
            pagination.rows = pageSize;
            pagination.sidx = "createdate desc";
            pagination.sord = "";
            pagination.conditionJson = " 1=1 ";
            pagination.p_fields = @" createuserid ,createusername ,organizeid,organizename,deptid,deptname,questioncontent,questionpic,flowstate,appsign,actionperson,flowdescribe,createdate,filepath";

            pagination.p_kid = "id";

            pagination.p_tablename = string.Format(@" ( select a.* ,(case when a.flowstate ='开始' then '问题登记' when a.flowstate ='评估' then '问题评估' else '已处理' end ) flowdescribe,b.actionperson,b.participantname,c.filepath from bis_findquestioninfo a  
                                          left join v_findquestionworkflow  b on a.id =b.id 
                                          left join ( select a.id,wm_concat((case when b.filepath is not null then ('{0}'||substr(b.filepath,2)) else '' end)) filepath from bis_findquestioninfo a left join base_fileinfo b on a.questionpic = b.recid  group by a.id ) c on  a.id = c.id ) a", dataitemdetailbll.GetItemValue("imgUrl"));

            //数据范围
            pagination.conditionJson += string.Format(@" and (createuserid ='{0}'  or   (actionperson  like  '%{1}%' and flowstate='评估')  or  flowstate='结束') ", curUser.UserId, curUser.Account + ",");

            //整改单位编码
            if (!string.IsNullOrEmpty(deptid))
            {
                pagination.conditionJson += string.Format(@" and  deptid ='{0}' ", deptid);
            }
            //问题描述
            if (!string.IsNullOrEmpty(questioncontent))
            {
                pagination.conditionJson += string.Format(@" and  questioncontent like '%{0}%' ", questioncontent);
            }
            //流程状态
            if (!string.IsNullOrEmpty(flowstate))
            {
                pagination.conditionJson += string.Format(@" and  flowstate ='{0}' ", flowstate);
            }
            //问题时间开始时间
            if (!string.IsNullOrEmpty(stdate))
            {
                pagination.conditionJson += string.Format(@" and createdate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", stdate);
            }
            //问题时间结束时间
            if (!string.IsNullOrEmpty(etdate))
            {
                pagination.conditionJson += string.Format(@" and createdate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(etdate).AddDays(1).ToString("yyyy-MM-dd"));
            }

            switch (action)
            {
                //我发现的(创建的)
                case "1":
                    pagination.conditionJson += string.Format(@" and   createuserid ='{0}'", curUser.UserId);
                    break;
                //待评估列表
                case "2":
                    pagination.conditionJson += string.Format(@" and  actionperson  like  '%{0}%' and flowstate  = '评估'", "," + curUser.Account + ",");
                    break;
            }
            var dt = htbaseinfobll.GetBaseInfoForApp(pagination);

            return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
        }
        #endregion

        #region 获取发现问题详情信息
        /// <summary>
        /// 获取发现问题详情信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetFindQuestionDetail([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string questionid = res.Contains("questionid") ? dy.data.questionid : ""; //问题主键

            //发现问题基本信息
            var baseInfo = findquestioninfobll.GetEntity(questionid);
            #region 图片
            List<Photo> questionpiclist = new List<Photo>(); //问题图片 
            IEnumerable<FileInfoEntity> lllegalfile = fileInfoBLL.GetImageListByTop5Object(baseInfo.QUESTIONPIC);
            foreach (FileInfoEntity fentity in lllegalfile)
            {
                Photo p = new Photo();
                p.id = fentity.FileId;
                p.filename = fentity.FileName;
                p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + fentity.FilePath.Substring(1);
                p.folderid = fentity.FolderId;
                questionpiclist.Add(p);
            }
            #endregion


            //获取问题处理情况
            var handledata = new List<object>();
            var baseHandle = findquestionhandlebll.GetQuestionHandleTable(questionid);
            foreach (DataRow row in baseHandle.Rows)
            {
                handledata.Add(new
                {
                    handleid = row["id"].ToString(),
                    handlerid = row["handlerid"].ToString(),
                    handlername = row["handlername"].ToString(),
                    handledate = row["handledate"].ToString(),
                    handlestatus = row["handlestatus"].ToString(),
                    apptype = row["relevancetype"].ToString(),
                    appstate = row["appstate"].ToString(),
                    appid = row["appid"].ToString()
                });
            }

            string flowdescribe = string.Empty;
            if (baseInfo.FLOWSTATE == "开始") { flowdescribe = "问题登记"; }
            else if (baseInfo.FLOWSTATE == "评估") { flowdescribe = "问题评估"; }
            else { flowdescribe = "问题登记"; }
            //返回对象
            var data = new
            {
                questionid = baseInfo.ID,
                questioncontent = baseInfo.QUESTIONCONTENT,
                createdate = baseInfo.CREATEDATE.Value.ToString("yyyy-MM-dd"),
                deptid = baseInfo.DEPTID,
                deptname = baseInfo.DEPTNAME,
                flowstate = baseInfo.FLOWSTATE,
                flowdescribe = flowdescribe,
                questionpic = questionpiclist,
                handledata = handledata
            };

            return new { code = 0, count = 0, info = "获取成功", data = data };
        }
        #endregion

        #region 提交发现问题
        /// <summary>
        /// 提交发现问题
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddFindQuestionInfo()
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                string wfFlag = string.Empty;
                string participant = string.Empty;
                string keyValue = string.Empty;
                string questionid = res.Contains("questionid") ? dy.data.questionid : "";  //主键
                string isback = res.Contains("isback") ? dy.data.isback : ""; //是否回退

                #region 发现问题登记信息
                FindQuestionInfoEntity entity = new FindQuestionInfoEntity();
                if (!string.IsNullOrEmpty(questionid))
                {
                    entity = findquestioninfobll.GetEntity(questionid);
                }
                else
                {
                    entity.ORGANIZEID = curUser.OrganizeId;
                    entity.ORGANIZENAME = curUser.OrganizeName;
                }
                //问题基本信息       
                entity.DEPTID = res.Contains("deptid") ? dy.data.deptid : "";//所属部门id
                entity.DEPTNAME = res.Contains("deptname") ? dy.data.deptname : ""; //所属部门名称
                entity.QUESTIONCONTENT = res.Contains("questioncontent") ? dy.data.questioncontent : "";//问题内容
                //问题图片
                string fileids = res.Contains("deleteids") ? dy.data.deleteids : ""; //要删除的文件
                /**********图片部分缺省**********/
                if (string.IsNullOrEmpty(questionid))
                {
                    entity.QUESTIONPIC = Guid.NewGuid().ToString(); //问题图片
                }
                DeleteFile(fileids);  //先删除图片
                UploadifyFile(entity.QUESTIONPIC, "questionpic", files); //上传问题图片
                /********************************/
                entity.APPSIGN = AppSign; //移动端标记
                //新增
                entity.APPSIGN = AppSign;
                findquestioninfobll.SaveForm(questionid, entity);
                keyValue = entity.ID;
                #endregion

                #region 创建主体流程
                //主键为空  创建主体流程
                if (string.IsNullOrEmpty(questionid))
                {
                    bool isSucess = htworkflowbll.CreateWorkFlowObj("10", keyValue, curUser.UserId);
                    if (isSucess)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_findquestioninfo", "flowstate", keyValue);  //更新业务流程状态
                    }
                }
                #endregion

                #region 推进流程
                var nEntity = findquestioninfobll.GetEntity(keyValue);
                //参与人员
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.startflow = nEntity.FLOWSTATE;
                wfentity.submittype = "提交";
                if (isback == "0")
                {
                    wfentity.submittype = "退回";
                }
                wfentity.rankid = string.Empty;
                wfentity.user = curUser;
                wfentity.mark = "发现问题流程";
                wfentity.organizeid = entity.ORGANIZEID;
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
                            if (isback == "0")
                            {
                                FindQuestionHandleEntity qentity = new FindQuestionHandleEntity();
                                qentity.HANDLESTATUS = "已退回";
                                qentity.HANDLEDATE = DateTime.Now;
                                qentity.HANDLERID = curUser.UserId;
                                qentity.HANDLERNAME = curUser.UserName;
                                qentity.QUESTIONID = keyValue;
                                qentity.APPSIGN = AppSign;
                                findquestionhandlebll.SaveForm("", qentity);
                            }
                            htworkflowbll.UpdateFlowStateByObjectId("bis_findquestioninfo", "flowstate", keyValue);  //更新业务流程状态
                        }
                    }
                    return new { code = 0, count = 0, info = result.message };

                }
                else
                {
                    return new { code = -1, count = 0, info = result.message };
                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message + ",异常目标:" + ex.InnerException.ToString() };
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
        public object ToTargetContent([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string keyValue = res.Contains("questionid") ? dy.data.questionid.ToString() : ""; //主键

            int mode = res.Contains("mode") ? int.Parse(dy.data.mode.ToString()) : 0; //  

            var entity = findquestioninfobll.GetEntity(keyValue); //发现问题对象
            var userInfo = userbll.GetUserInfoEntity(entity.CREATEUSERID); //创建人对象
            List<FileInfoEntity> filelist = fileInfoBLL.GetImageListByObject(entity.QUESTIONPIC).ToList(); //问题图片集合
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

            string webPath = dataitemdetailbll.GetItemValue("imgPath");
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
                        bentity.APPSIGN = AppSign;
                        foreach (FileInfoEntity fentity in filelist)
                        {
                            string virtualPath = fentity.FilePath.Substring(1);
                            string sourcefile = webPath + virtualPath; 
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
                            newfileEntity.RecId = bentity.HIDPHOTO;
                            fileInfoBLL.SaveForm("", newfileEntity);
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
                        centity.APPSIGN = AppSign;
                        htchangeinfobll.SaveForm("", centity);
                        #endregion

                        #region 验收信息
                        HTAcceptInfoEntity aentity = new HTAcceptInfoEntity();
                        aentity.HIDCODE = HidCode;
                        aentity.APPSIGN = AppSign;
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
                        wzentity.APPSIGN = AppSign;
                        wzentity.LLLEGALPIC = Guid.NewGuid().ToString();
                        foreach (FileInfoEntity fentity in filelist)
                        {
                            string virtualPath = fentity.FilePath.Substring(1);
                            string sourcefile = webPath + virtualPath; 
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
                            fileInfoBLL.SaveForm("", newfileEntity);
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
                        reformEntity.APPSIGN = AppSign;
                        lllegalreformbll.SaveForm("", reformEntity);
                        #endregion

                        #region 违章验收信息
                        LllegalAcceptEntity acceptEntity = new LllegalAcceptEntity();
                        acceptEntity.LLLEGALID = applicationId;
                        acceptEntity.APPSIGN = AppSign;
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
                        qtEntity.APPSIGN = AppSign;
                        qtEntity.BELONGDEPTID = userInfo.OrganizeId;
                        qtEntity.BELONGDEPTNAME = userInfo.OrganizeName;

                        qtEntity.QUESTIONPIC = Guid.NewGuid().ToString();
                        foreach (FileInfoEntity fentity in filelist)
                        {
                            string virtualPath = fentity.FilePath.Substring(1);
                            string sourcefile = webPath + virtualPath; 
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
                            fileInfoBLL.SaveForm("", newfileEntity);
                        }
                        qtEntity.QUESTIONDESCRIBE = entity.QUESTIONCONTENT;

                        qtEntity.CHECKDATE = DateTime.Now;
                        qtEntity.CHECKPERSONID = userInfo.UserId;
                        qtEntity.CHECKPERSONNAME = userInfo.RealName;
                        qtEntity.CHECKDEPTID = userInfo.DepartmentId;
                        qtEntity.CHECKDEPTNAME = userInfo.DeptName;

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
                        qtreformEntity.APPSIGN = AppSign;
                        questionreformbll.SaveForm("", qtreformEntity);
                        #endregion

                        //极光消息推送
                        JPushApi.PushMessage(userInfo.Account, userInfo.RealName, "WT001", "您有一条问题需完善，请到问题登记进行处理", "您" + entity.CREATEDATE.Value.ToString("yyyy-MM-dd") + "发现的问题已确定为问题，请您到问题登记下对该问题进行完善并指定对应整改责任人。", applicationId);

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
                                    qentity.APPSIGN = AppSign;
                                    findquestionhandlebll.SaveForm("", qentity);
                                    #endregion

                                    htworkflowbll.UpdateFlowStateByObjectId("bis_findquestioninfo", "flowstate", keyValue);  //更新业务流程状态
                                }
                            }
                            return new { code = 0, count = 0, info = resultMsg };
                        }
                        else
                        {
                            return new { code = -1, count = 0, info = result.message };
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
                        qentity.APPSIGN = AppSign;
                        findquestionhandlebll.SaveForm("", qentity);
                        #endregion
                        return new { code = 0, count = 0, info = resultMsg };
                    }
                }
                else
                {
                    return new { code = -1, count = 0, info = resultMsg };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion
    }

    #region 问题对象
    /// <summary>
    /// 问题对象
    /// </summary>
    public class QuestionModel
    {
        public string questionid { get; set; } //id 
        public string createuserid { get; set; } //创建人id
        public string createdate { get; set; } //创建时间
        public string createusername { get; set; } //创建用户姓名
        public string questionnumber { get; set; }  //问题编号 
        public string belongdeptname { get; set; }//所属单位名称
        public string belongdeptid { get; set; }//所属单位id 
        public string questiondescribe { get; set; } //问题描述
        public string questionaddress { get; set; } //问题地点
        public List<Photo> questionpic { get; set; }  // 问题图片
        public string checkpersonname { get; set; }  //检查人姓名
        public string checkpersonid { get; set; }  //检查人姓名
        public string checkdeptid { get; set; }  //检查人部门id
        public string checkdeptname { get; set; }  //检查人部门名称
        public string checktype { get; set; }  //检查类型
        public string checktypename { get; set; }  //检查类型名称
        public string checkname { get; set; }  //检查名称
        public string checkid { get; set; }  //检查id
        public string checkdate { get; set; }  //检查日期
        public string checkcontent { get; set; }  //检查重点内容
        public string relevanceid { get; set; }  //应用关联id(检查对象id) 
        public string correlationid { get; set; }  //应用关联id(检查内容id) 
        public string flowstate { get; set; }  //流程状态
        public string actionperson { get; set; } //当前流程操作人账户
        public string participantname { get; set; } //当前流程操作人姓名
        public bool ishavaworkflow { get; set; } //是否存在问题流程


        /****整改内容***/
        public string reformdeptcode { get; set; } //整改部门编码
        public string reformdeptname { get; set; } //整改部门名称
        public string reformdeptid { get; set; } //整改部门id
        public string dutydeptcode { get; set; } //联责部门编码
        public string dutydeptname { get; set; } //联责部门名称
        public string dutydeptid { get; set; } //联责部门id 
        public string reformpeoplename { get; set; } //整改人姓名
        public string reformpeople { get; set; } //整改人
        public string reformtel { get; set; } //整改人电话
        public string reformplandate { get; set; } //计划完成时间
        public string reformmeasure { get; set; } //整改措施
        public string reformdescribe { get; set; }  //整改情况描述
        public string reformstatus { get; set; } //整改完成情况
        public string reformreason { get; set; } //未完成原因
        public string reformsign { get; set; }  //整改签名
        public string reformfinishdate { get; set; } //整改完成时间
        public List<Photo> reformpic { get; set; }  //整改图片

        /****验证内容***/
        public string verifypeoplename { get; set; }  //验证人id
        public string verifypeople { get; set; } //验证人姓名
        public string verifydeptname { get; set; } //验证部门名称
        public string verifydeptcode { get; set; } //验证部门编码
        public string verifydeptid { get; set; } //验证部门编码
        public string verifyresult { get; set; }  //验证结果
        public string verifyopinion { get; set; } //验证意见
        public string verifydate { get; set; } //验收时间  
        public string verifysign { get; set; } //验证签名 
        public List<ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData> checkflow { get; set; }  //流程图
    }
    #endregion

    #region 整改历史记录
    /// <summary>
    /// 整改历史记录
    /// </summary>
    public class QuestionReformModel
    {
        public string questionid { get; set; }   //问题id 
        public string reformid { get; set; }   //整改id 
        public string reformstatus { get; set; }  //整改结果
        public string reformplandate { get; set; }  //计划完成时间
        public string reformfinishdate { get; set; }  //整改完成时间
        public string reformpeoplename { get; set; }  //整改人  
        public string reformtel { get; set; }  //整改电话
        public string reformdeptname { get; set; } //整改部门 
        public string dutydeptname { get; set; } //联责部门
        public string reformmeasure { get; set; }  //整改措施
        public string reformdescribe { get; set; } //整改情况描述
        public string reformsign { get; set; } //整改人签名
        public List<Photo> reformpic { get; set; }  //整改图片 
    }
    #endregion

    #region 验证历史记录
    /// <summary>
    /// 验证历史记录
    /// </summary>
    public class QuestionVerifyModel
    {
        public string questionid { get; set; }   //问题id
        public string verifyid { get; set; }   //验收id 
        public string verifyresult { get; set; }  //验证结果
        public string verifypeoplename { get; set; }  //验证人 
        public string verifydeptname { get; set; } //验证单位
        public string verifyopinion { get; set; }  //验证意见
        public string verifydate { get; set; } //验证时间
        public string verifysign { get; set; } //验证人签名 
    }
    #endregion
}