using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.OutsourcingProject;
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
    /// <summary>
    /// 入厂许可申请
    /// </summary>
    public class EntranceController : BaseApiController
    {

        private IntromissionBLL intromissionbll = new IntromissionBLL(); //入厂许可申请操作
        private IntromissionHistoryBLL intromissionhistorybll = new IntromissionHistoryBLL();  //入厂许可申请历史操作
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private InvestigateBLL investigatebll = new InvestigateBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private InvestigateContentBLL investigatecontentbll = new InvestigateContentBLL();
        private InvestigateRecordBLL investigaterecordbll = new InvestigateRecordBLL();
        private InvestigateDtRecordBLL investigatedtrecordbll = new InvestigateDtRecordBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL(); //审核记录
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本对象
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL(); //外包工程
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
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

        #region 保存/提交入厂许可申请信息(初次登记)
        /// <summary>
        ///保存入厂许可申请信息(初次登记)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SubmitRegIntrom([FromBody]JObject json)
        {

            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string outengineerid = res.Contains("outengineerid") ? dy.data.outengineerid.ToString() : ""; //外包工程id
                string intromid = res.Contains("intromid") ? dy.data.intromid.ToString() : ""; //入厂许可申请id
                string issubmit = res.Contains("issubmit") ? dy.data.issubmit.ToString() : ""; //入厂许可不为空，则表示提交

                IntromissionEntity entity = new IntromissionEntity();

                entity.APPLYPEOPLE = curUser.UserName;
                entity.APPLYPEOPLEID = curUser.UserId;
                entity.APPLYTIME = DateTime.Now;
                entity.INVESTIGATESTATE = "0";
                //不为空则更新
                if (!string.IsNullOrEmpty(intromid))
                {
                    entity = intromissionbll.GetEntity(intromid);
                }
                entity.OUTENGINEERID = outengineerid;
                intromissionbll.SaveForm(intromid, entity);

                //提交到下一个流程
                #region 提交到下一个流程
                if (!string.IsNullOrEmpty(issubmit))
                {

                    bool isUseSetting = true;
                    string moduleName = "入厂许可审查";

                    //判断是否需要审查(审查配置表)
                    var list = investigatebll.GetList(curUser.OrganizeId).Where(p => p.SETTINGTYPE == "入厂许可").ToList();

                    ManyPowerCheckEntity mpcEntity = null;

                    InvestigateEntity investigateEntity = null;

                    if (list.Count() > 0)
                    {
                        investigateEntity = list.FirstOrDefault();
                    }

                    if (null != investigateEntity)
                    {
                        //启用审查
                        if (investigateEntity.ISUSE == "是")
                        {
                            entity.FLOWNAME = "审查中";
                            entity.INVESTIGATESTATE = "1"; //审查状态

                            //新增审查记录
                            InvestigateRecordEntity rcEntity = new InvestigateRecordEntity();
                            rcEntity.INTOFACTORYID = entity.ID;
                            rcEntity.INVESTIGATETYPE = "0";//当前记录标识
                            investigaterecordbll.SaveForm("", rcEntity);

                            //获取审查内容
                            var contentList = investigatecontentbll.GetList(investigateEntity.ID).ToList();

                            //批量增加审查内容到审查记录中
                            foreach (InvestigateContentEntity icEntity in contentList)
                            {
                                InvestigateDtRecordEntity dtEntity = new InvestigateDtRecordEntity();
                                dtEntity.INVESTIGATERECORDID = rcEntity.ID;
                                dtEntity.INVESTIGATECONTENT = icEntity.INVESTIGATECONTENT;
                                dtEntity.INVESTIGATECONTENTID = icEntity.ID;
                                investigatedtrecordbll.SaveForm("", dtEntity);
                            }
                        }
                        else  //未启用审查，直接跳转到审核 
                        {
                            entity.FLOWNAME = "审核中";
                            entity.INVESTIGATESTATE = "2"; //审核状态
                        }
                    }
                    else
                    {
                        //如果没有审查配置，直接到审核
                        isUseSetting = false;
                        entity.FLOWNAME = "审核中";
                        entity.INVESTIGATESTATE = "2"; //审核状态
                    }
                    //更改申请信息状态
                    mpcEntity = peoplereviewbll.CheckAuditForNextFlow(curUser, moduleName, outengineerid, entity.FLOWID, false, isUseSetting);

                    if (null != mpcEntity)
                    {
                        entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        entity.FLOWROLE = mpcEntity.CHECKROLEID;
                        entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        entity.FLOWID = mpcEntity.ID;
                        DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                        var userAccount = dt.Rows[0]["account"].ToString();
                        var userName = dt.Rows[0]["realname"].ToString();
                        JPushApi.PushMessage(userAccount, userName, "WB013", entity.ID);
                    }
                    else
                    {
                        entity.FLOWNAME = "已完结";
                        entity.INVESTIGATESTATE = "3"; //完结状态
                    }
                    intromissionbll.SaveForm(entity.ID, entity);

                }
                #endregion
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 保存审查记录
        /// <summary>
        /// 保存审查记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>

        [HttpPost]
        public object SaveAppIntrom()
        {
            string res = HttpContext.Current.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            try
            {
                /*
                 "record": [{"id":"39545acc-0bc0-4b2b-9845-19fe219d7ce8","result":"","people":""},{"id":"d7bb8b94-7889-48a3-8bed-60b72a7fb636","result":"","people":""},{"id":"b15130c4-0aa9-4b8c-b2a7-4fb082f59b98","result":"","people":""},{"id":"2ff21c22-8c96-41c2-a8b1-d4066c4b0148","result":"","people":""},{"id":"4bd9aeef-ff41-400e-8c30-3258c4e5033f","result":"","people":""},{"id":"4d6806fa-9204-4998-bad3-74fa54700d40","result":"","people":""}]
                 */
                //审查内容 (审核阶段可不用传入)
                List<object> record = res.Contains("record") ? (List<object>)dy.data.record : null;
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (dynamic rdy in record)
                {
                    string id = rdy.id.ToString();  //主键
                    string result = rdy.result.ToString(); //结果
                    string people = rdy.people.ToString(); //选择的人员
                    string peopleid = rdy.peopleid.ToString();
                    string signpic = rdy.signpic.ToString();

                    if (!string.IsNullOrEmpty(id))
                    {
                        var scEntity = investigatedtrecordbll.GetEntity(id); //审查内容项
                        scEntity.INVESTIGATERESULT = result;
                        scEntity.INVESTIGATEPEOPLE = people;
                        scEntity.INVESTIGATEPEOPLEID = peopleid;
                        scEntity.SIGNPIC = string.IsNullOrWhiteSpace(signpic) ? "" : signpic.Replace(webUrl, "").ToString();
                        HttpFileCollection files = HttpContext.Current.Request.Files;
                        if (files.Count > 0)
                        {
                            for (int i = 0; i < files.AllKeys.Length; i++)
                            {
                                HttpPostedFile file = files[i];
                                string fileOverName = System.IO.Path.GetFileName(file.FileName);
                                string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                                string FileEextension = Path.GetExtension(file.FileName);
                                if (fileName == scEntity.ID)
                                {
                                    string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                                    string newFileName = fileName + FileEextension;
                                    string newFilePath = dir + "\\" + newFileName;
                                    file.SaveAs(newFilePath);
                                    scEntity.SIGNPIC = "/Resource/sign/" + fileOverName;
                                    break;
                                }
                            }
                        }
                        investigatedtrecordbll.SaveForm(id, scEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 提交审查/审核入厂许可申请信息
        /// <summary>
        ///保存入厂许可申请信息(初次登记)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SubmitAppIntrom()
        {

            string res = HttpContext.Current.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string deptId = string.Empty;
            string deptName = string.Empty;
            string roleNames = curUser.RoleName;

            //公司级用户取机构对象
            if (roleNames.Contains("公司级用户"))
            {
                deptId = curUser.OrganizeId;  //机构ID
                deptName = curUser.OrganizeName;//机构名称
            }
            else
            {
                deptId = curUser.DeptId; //部门ID
                deptName = curUser.DeptName; //部门ID
            }

            int noDoneCount = 0; //未完成个数

            bool isUseSetting = true; //是否多个流程配置下的角色同时进行流程审核

            int isBack = 0;  //退回操作

            string newKeyValue = string.Empty;  //新的入厂许可申请Id

            string state = res.Contains("state") ? dy.data.state.ToString() : "";  //判定是否是审查，还是审核阶段

            string outengineerid = res.Contains("outengineerid") ? dy.data.outengineerid.ToString() : ""; //外包工程id

            string intromid = res.Contains("intromid") ? dy.data.intromid.ToString() : ""; //入厂许可申请id

            //审查内容 (审核阶段可不用传入)
            List<object> record = res.Contains("record") ? (List<object>)dy.data.record : null;//{ "record":"[{"scid":"xxx","result":"未完成","people":"xxxxx"},{"scid":"yyyy","result":"已完成","people":"yyyy"}]"}

            /*******审核详情*******/
            string approveresult = res.Contains("approveresult") ? (string.IsNullOrWhiteSpace(dy.data.approveresult) ? "" : dy.data.approveresult.ToString()) : ""; //审核结果  0 表示同意 1 表示不同意

            string approveopinion = res.Contains("approveopinion") ? (string.IsNullOrWhiteSpace(dy.data.approveopinion) ? "" : dy.data.approveopinion.ToString()) : "";  //审核意见


            //当前入厂许可审查对象
            IntromissionEntity entity = intromissionbll.GetEntity(intromid);

            //审核对象
            AptitudeinvestigateauditEntity aentity = new AptitudeinvestigateauditEntity();

            //更改申请信息状态
            ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditForNextFlow(curUser, "入厂许可审查", entity.OUTENGINEERID, entity.FLOWID, false, isUseSetting);

            string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
            //审查状态下更新审查内容
            if (state == "1")
            {
                //只更新审查内容
                foreach (dynamic rdy in record)
                {
                    string id = rdy.id.ToString();  //主键
                    string result = rdy.result.ToString(); //结果
                    string people = rdy.people.ToString(); //选择的人员
                    string peopleid = rdy.peopleid.ToString();
                    string signpic= rdy.signpic.ToString();

                    if (!string.IsNullOrEmpty(id))
                    {
                        var scEntity = investigatedtrecordbll.GetEntity(id); //审查内容项
                        scEntity.INVESTIGATERESULT = result;
                        if (result == "未完成") { noDoneCount += 1; } //存在未完成的则累加
                        scEntity.INVESTIGATEPEOPLE = people;
                        scEntity.INVESTIGATEPEOPLEID = peopleid;
                        scEntity.SIGNPIC = string.IsNullOrWhiteSpace(signpic) ? "" : signpic.Replace(webUrl, "").ToString();
                        HttpFileCollection files = HttpContext.Current.Request.Files;
                        if (files.Count > 0)
                        {
                            for (int i = 0; i < files.AllKeys.Length; i++)
                            {
                                HttpPostedFile file = files[i];
                                string fileOverName = System.IO.Path.GetFileName(file.FileName);
                                string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                                string FileEextension = Path.GetExtension(file.FileName);
                                if (fileName == scEntity.ID)
                                {
                                    string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                                    string newFileName = fileName + FileEextension;
                                    string newFilePath = dir + "\\" + newFileName;
                                    file.SaveAs(newFilePath);
                                    scEntity.SIGNPIC = "/Resource/sign/" + fileOverName;
                                    break;
                                }
                            }
                        }
                        //更新当前流程进行中的审查内容
                        investigatedtrecordbll.SaveForm(id, scEntity);
                    }
                }

                //退回操作
                if (noDoneCount > 0)
                {
                    entity.FLOWDEPT = " ";
                    entity.FLOWDEPTNAME = " ";
                    entity.FLOWROLE = " ";
                    entity.FLOWROLENAME = " ";
                    entity.FLOWID = " ";
                    entity.INVESTIGATESTATE = "0"; //更改状态为登记状态
                    entity.FLOWNAME = "";

                    isBack = 1; //审查退回
                    var applyUser = new UserBLL().GetEntity(entity.APPLYPEOPLEID);
                    if (applyUser != null)
                    {
                        JPushApi.PushMessage(applyUser.Account, entity.CREATEUSERNAME, "WB012", entity.ID);
                    }
                }
                else
                {
                    if (null != mpcEntity)
                    {
                        entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        entity.FLOWROLE = mpcEntity.CHECKROLEID;
                        entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        entity.FLOWID = mpcEntity.ID;
                        DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                        var userAccount = dt.Rows[0]["account"].ToString();
                        var userName = dt.Rows[0]["realname"].ToString();
                        JPushApi.PushMessage(userAccount, userName, "WB013", entity.ID);
                    }
                    entity.FLOWNAME = "审核中";
                    entity.INVESTIGATESTATE = "2"; //更改状态为审核
                }
            }
            else
            {
                //同意进行下一步
                if (approveresult == "0")
                {
                    //添加审核记录
                    aentity.AUDITRESULT = approveresult;
                    aentity.AUDITOPINION = approveopinion;
                    aentity.AUDITPEOPLE = curUser.UserName;
                    aentity.AUDITPEOPLEID = curUser.UserId;
                    aentity.AUDITDEPT = deptName;
                    aentity.AUDITDEPTID = deptId;
                    aentity.AUDITTIME = DateTime.Now;
                    aentity.APTITUDEID = intromid; //关联id 
                    aentity.FlowId = entity.FLOWID;
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.AllKeys.Length; i++)
                        {
                            HttpPostedFile file = files[i];
                            string fileOverName = System.IO.Path.GetFileName(file.FileName);
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                            string FileEextension = Path.GetExtension(file.FileName);
                            //if (fileName == aentity.ID)
                            //{
                            string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                            string newFileName = fileName + FileEextension;
                            string newFilePath = dir + "\\" + newFileName;
                            file.SaveAs(newFilePath);
                            aentity.AUDITSIGNIMG = "/Resource/sign/" + fileOverName;
                            break;
                            //}
                        }
                    }
                    aptitudeinvestigateauditbll.SaveForm("", aentity);
                    //下一步流程不为空
                    if (null != mpcEntity)
                    {
                        entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        entity.FLOWROLE = mpcEntity.CHECKROLEID;
                        entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        entity.FLOWID = mpcEntity.ID;
                        DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                        var userAccount = dt.Rows[0]["account"].ToString();
                        var userName = dt.Rows[0]["realname"].ToString();
                        JPushApi.PushMessage(userAccount, userName, "WB013", entity.ID);
                    }
                    else
                    {
                        entity.FLOWDEPT = " ";
                        entity.FLOWDEPTNAME = " ";
                        entity.FLOWROLE = " ";
                        entity.FLOWROLENAME = " ";
                        //entity.FLOWID = " ";
                        entity.FLOWNAME = "已完结";
                        entity.INVESTIGATESTATE = "3"; //更改状态为完结状态
                    }
                }
                else  //退回到申请人
                {
                    entity.FLOWDEPT = " ";
                    entity.FLOWDEPTNAME = " ";
                    entity.FLOWROLE = " ";
                    entity.FLOWROLENAME = " ";
                    entity.FLOWID = " ";
                    entity.INVESTIGATESTATE = "0"; //更改状态为登记状态
                    entity.FLOWNAME = "";
                    isBack = 2; //审核退回
                    var applyUser = new UserBLL().GetEntity(entity.APPLYPEOPLEID);
                    if (applyUser != null)
                    {
                        JPushApi.PushMessage(applyUser.Account, entity.CREATEUSERNAME, "WB012", entity.ID);
                    }
                }
            }
            //更改入厂许可申请单
            intromissionbll.SaveForm(intromid, entity);

            if (isBack > 0)
            {
                AddBackData(intromid, out newKeyValue);  //添加历史记录

                //审核退回
                if (isBack > 1)
                {
                    //获取当前业务对象的所有历史审核记录
                    var shlist = aptitudeinvestigateauditbll.GetAuditList(intromid);
                    //批量更新审核记录关联ID
                    foreach (AptitudeinvestigateauditEntity mode in shlist)
                    {
                        mode.APTITUDEID = newKeyValue; //对应新的关联ID
                        aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                    }
                    //添加审核记录
                    aentity.AUDITRESULT = approveresult;
                    aentity.AUDITOPINION = approveopinion;
                    aentity.AUDITPEOPLE = curUser.UserName;
                    aentity.AUDITPEOPLEID = curUser.UserId;
                    aentity.AUDITDEPT = deptName;
                    aentity.AUDITDEPTID = deptId;
                    aentity.AUDITTIME = DateTime.Now;
                    aentity.APTITUDEID = newKeyValue; //关联id 
                    aentity.FlowId = entity.FLOWID;
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.AllKeys.Length; i++)
                        {
                            HttpPostedFile file = files[i];
                            string fileOverName = System.IO.Path.GetFileName(file.FileName);
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                            string FileEextension = Path.GetExtension(file.FileName);
                            //if (fileName == aentity.ID)
                            //{
                                string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                                string newFileName = fileName + FileEextension;
                                string newFilePath = dir + "\\" + newFileName;
                                file.SaveAs(newFilePath);
                                aentity.AUDITSIGNIMG = "/Resource/sign/" + fileOverName;
                                break;
                            //}
                        }
                    }
                    aptitudeinvestigateauditbll.SaveForm("", aentity);
                }
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 退回添加到历史记录信息
        /// <summary>
        /// 退回添加到历史记录信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="arr"></param>
        public void AddBackData(string keyValue, out string newKeyValue)
        {
            //退回的同时保存原始的申请记录
            var dentity = intromissionbll.GetEntity(keyValue); //原始记录
            IntromissionHistoryEntity hentity = new IntromissionHistoryEntity();
            hentity.CREATEUSERID = dentity.CREATEUSERID;
            hentity.CREATEUSERDEPTCODE = dentity.CREATEUSERDEPTCODE;
            hentity.CREATEUSERORGCODE = dentity.CREATEUSERORGCODE;
            hentity.CREATEDATE = dentity.CREATEDATE;
            hentity.CREATEUSERNAME = dentity.CREATEUSERNAME;
            hentity.MODIFYDATE = dentity.MODIFYDATE;
            hentity.MODIFYUSERID = dentity.MODIFYUSERID;
            hentity.MODIFYUSERNAME = dentity.MODIFYUSERNAME;
            hentity.OUTENGINEERID = dentity.OUTENGINEERID;
            hentity.INTROMISSIONID = dentity.ID;
            hentity.APPLYPEOPLEID = dentity.APPLYPEOPLEID;
            hentity.APPLYPEOPLE = dentity.APPLYPEOPLE;
            hentity.APPLYTIME = dentity.APPLYTIME;
            hentity.INVESTIGATESTATE = dentity.INVESTIGATESTATE;
            hentity.REMARK = dentity.REMARK;
            hentity.FLOWDEPTNAME = dentity.FLOWDEPTNAME;
            hentity.FLOWDEPT = dentity.FLOWDEPT;
            hentity.FLOWROLENAME = dentity.FLOWROLENAME;
            hentity.FLOWROLE = dentity.FLOWROLE;
            hentity.FLOWNAME = dentity.FLOWNAME;
            hentity.FLOWID = dentity.FLOWNAME;
            intromissionhistorybll.SaveForm("", hentity);

            newKeyValue = hentity.ID;

            //更新审查记录单关联ID
            InvestigateRecordEntity irEntity = investigaterecordbll.GetEntityByIntroKey(keyValue); //审查记录单
            if (null != irEntity)
            {
                irEntity.INTOFACTORYID = newKeyValue;
                irEntity.INVESTIGATETYPE = "1"; //历史记录标识
                investigaterecordbll.SaveForm(irEntity.ID, irEntity);
            }
        }
        #endregion

        #region 获取入场申请记录
        /// <summary>
        /// 获取入场申请记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetIntromJson([FromBody]JObject json)
        {

            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            int pageSize = res.Contains("pagesize") ? int.Parse(dy.pagesize.ToString()) : 10; //每页的记录数

            int pageIndex = res.Contains("pageindex") ? int.Parse(dy.pageindex.ToString()) : 1;  //当前页索引

            int actiontype = res.Contains("actiontype") ? int.Parse(dy.data.actiontype.ToString()) : 0; //查询类型

            string startdate = res.Contains("startdate") ? dy.data.startdate.ToString() : "";  //起始时间 

            string enddate = res.Contains("enddate") ? dy.data.enddate.ToString() : "";  //截止时间

            string outengineerid = res.Contains("outengineerid") ? dy.data.outengineerid.ToString() : "";  //外包工程

            string outengineerdept = res.Contains("outengineerdept") ? dy.data.outengineerdept.ToString() : ""; //外包单位

            string senddept = res.Contains("senddept") ? dy.data.senddept.ToString() : ""; //发包单位

            Pagination pagination = new Pagination();
            pagination.page = pageIndex;
            pagination.rows = pageSize;
            pagination.sidx = "a.createdate desc ,a.modifydate desc";
            pagination.sord = "";
            pagination.p_kid = "a.id";
            pagination.p_fields = @"a.createuserid,a.createdate,a.modifydate, a.applypeople,a.applytime,a.investigatestate,a.outengineerid,b.engineername,c.fullname,
                                    a.flowdeptname,a.flowdept,a.flowrolename,a.flowrole,a.flowname,a.flowid ,'' as approveuserid,'' as approveusername";
            pagination.p_tablename = @" epg_intromission a  
                                        left join epg_outsouringengineer b on a.outengineerid = b.id 
                                        left join base_department c on b.outprojectid=c.departmentid";
            string role = curUser.RoleName;
            string deptId = string.Empty;
            string deptName = string.Empty;

            //公司级用户取机构对象
            if (role.Contains("公司级用户"))
            {
                deptId = curUser.OrganizeId;  //机构ID
                deptName = curUser.OrganizeName;//机构名称
            }
            else
            {
                deptId = curUser.DeptId; //部门ID
                deptName = curUser.DeptName; //部门ID
            }

            //我的
            if (actiontype == 1)
            {
                string[] arrRole = role.Split(',');

                string strWhere = string.Empty;

                foreach (string str in arrRole)
                {
                    //审查内容
                    strWhere += string.Format(@" select distinct a.id  from epg_intromission a 
                                                left join  bis_manypowercheck b on a.flowid = b.id
                                                left join bis_manypowercheck c on  b.serialnum = c.serialnum and b.moduleno = c.moduleno
                                                left join epg_outsouringengineer d on a.outengineerid = d.id where  a.investigatestate ='1'  and  ((d.engineerletdeptid='{0}'  and c.checkrolename like '%{1}%')  or ( c.checkdeptid =  '{0}'  and c.checkrolename like '%{1}%'))
                                                union 
                                                ", deptId, str);

                    //审核内容
                    strWhere += string.Format(@"   select  distinct a.id from epg_intromission a  where a.flowdept='{0}' and a.flowrolename like '%{1}%' and  a.investigatestate ='2' 
                                                union", deptId, str);
                }

                //我创建
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strWhere += string.Format(@"  select distinct a.id from epg_intromission a  where a.createuserid ='{0}' and  a.investigatestate ='0'", curUser.UserId); //strWhere.Substring(0, strWhere.Length - 5);
                }
                // pagination.conditionJson = string.Format(" a.id in ({0})", strWhere);

                var conditionDt = intromissionbll.GetDataTableBySql(strWhere);

                string ids = string.Empty;

                foreach (DataRow row in conditionDt.Rows)
                {
                    ids += "'" + row["id"].ToString() + "',";
                }
                if (!string.IsNullOrEmpty(ids))
                {
                    ids = ids.Substring(0, ids.Length - 1);

                    //我要处理的
                    pagination.conditionJson = string.Format(" a.id in ({0})", ids);
                }
                else
                {
                    pagination.conditionJson = string.Format(" 1!=1 ");
                }


            }
            else if (actiontype == 0) //全部
            {
                string allrangedept = "";
                try
                {
                    allrangedept = dataitemdetailbll.GetDataItemByDetailCode("SBDept", "SBDeptId").FirstOrDefault().ItemValue;
                }
                catch (Exception)
                {

                }
                if (role.Contains("公司级用户") || role.Contains("厂级部门用户") || allrangedept.Contains(curUser.DeptId))
                {
                    pagination.conditionJson = string.Format(" a.createuserorgcode  = '{0}'", curUser.OrganizeCode);
                }
                else if (role.Contains("承包商级用户"))
                {
                    pagination.conditionJson = string.Format(" c.departmentid = '{0}'", curUser.DeptId);
                }
                else
                {
                    pagination.conditionJson = string.Format(" b.engineerletdeptid = '{0}'", curUser.DeptId);
                }
                pagination.conditionJson += string.Format(" and a.investigatestate !='0'"); //无法查看登记的数据
            }
            else if (actiontype == 2) //待审核审批
            {
                string[] arrRole = role.Split(',');

                string strWhere = string.Empty;

                foreach (string str in arrRole)
                {
                    //审查内容
                    strWhere += string.Format(@" select distinct a.id  from epg_intromission a 
                                                left join  bis_manypowercheck b on a.flowid = b.id
                                                left join bis_manypowercheck c on  b.serialnum = c.serialnum and b.moduleno = c.moduleno
                                                left join epg_outsouringengineer d on a.outengineerid = d.id where  a.investigatestate ='1'  and  ((d.engineerletdeptid='{0}'  and c.checkrolename like '%{1}%')  or ( c.checkdeptid =  '{0}'  and c.checkrolename like '%{1}%'))
                                                union 
                                                ", deptId, str);

                    //审核内容
                    strWhere += string.Format(@"   select  distinct a.id from epg_intromission a  where a.flowdept='{0}' and a.flowrolename like '%{1}%' and  a.investigatestate ='2' 
                                                union", deptId, str);
                }

                if (!string.IsNullOrEmpty(strWhere))
                {
                    strWhere = strWhere.Substring(0, strWhere.Length - 5);
                }
           

                var conditionDt = intromissionbll.GetDataTableBySql(strWhere);

                string ids = string.Empty;

                foreach (DataRow row in conditionDt.Rows)
                {
                    ids += "'" + row["id"].ToString() + "',";
                }
                if (!string.IsNullOrEmpty(ids))
                {
                    ids = ids.Substring(0, ids.Length - 1);

                    //我要处理的
                    pagination.conditionJson = string.Format(" a.id in ({0})", ids);
                }
                else
                {
                    pagination.conditionJson = string.Format(" 1!=1 ");
                }
            } else//已审核审批
            {
                if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
                {
                    pagination.conditionJson = string.Format(" a.createuserorgcode  = '{0}'", curUser.OrganizeCode);
                }
                else if (role.Contains("承包商级用户"))
                {
                    pagination.conditionJson = string.Format(" c.departmentid = '{0}'", curUser.DeptId);
                }
                else
                {
                    pagination.conditionJson = string.Format(" b.engineerletdeptid = '{0}'", curUser.DeptId);
                }
                pagination.conditionJson += string.Format(" and a.investigatestate !='0' and investigatestate='3' "); //无法查看登记的数据
            }

            ////时间范围
            if (!string.IsNullOrEmpty(startdate) || !string.IsNullOrEmpty(enddate))
            {
                if (string.IsNullOrEmpty(startdate))
                {
                    startdate = "1899-01-01";
                }
                if (string.IsNullOrEmpty(enddate))
                {
                    enddate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and to_date(to_char(a.applytime,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startdate, enddate);
            }
            //外包工程
            if (!string.IsNullOrEmpty(outengineerid))
            {
                pagination.conditionJson += string.Format(" and  b.id = '{0}'", outengineerid);
            }
            //外包单位
            if (!string.IsNullOrEmpty(outengineerdept))
            {
                pagination.conditionJson += string.Format(" and  c.id = '{0}'", outengineerdept);
            }
            //发包单位
            if (!string.IsNullOrEmpty(senddept))
            {
                pagination.conditionJson += string.Format(" and  b.engineerletdeptid = '{0}'", senddept);
            }
            var data = htbaseinfobll.GetBaseInfoForApp(pagination);

            List<IntromResult> list = new List<IntromResult>();
    
            foreach (DataRow row in data.Rows)
            {
                IntromResult entity = new IntromResult();
                entity.intromid = row["id"].ToString();
                entity.outengineer = row["engineername"].ToString();
                entity.applypeople = row["applypeople"].ToString();
                entity.state = row["investigatestate"].ToString();
                entity.applytime = !string.IsNullOrEmpty(row["applytime"].ToString()) ? Convert.ToDateTime(row["applytime"].ToString()) : DateTime.Now;
                string str = new ScaffoldBLL().GetUserName(row["flowdept"].ToString(), row["flowrolename"].ToString(), "0");
                entity.approveuserid = str.Split('|')[1];
                entity.approveusername = str.Split('|')[0];
                list.Add(entity);
            }

            var jsonData = new
            {
                rows = list,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return new { code = 0, count = jsonData.records, info = "获取成功", data = jsonData };
        }
        #endregion

        #region 获取入场申请记录/详细信息
        /// <summary>
        /// 获取入场申请记录/详细信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetIntromDetail([FromBody]JObject json)
        {

            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            IntromData data = new IntromData();
            try
            {
                string intromid = res.Contains("intromid") ? dy.data.intromid.ToString() : ""; //入厂许可申请

                //查询入厂许可申请记录
                var entity = intromissionbll.GetEntity(intromid);
                //查询审核流程图
                List<CheckFlowData> nodeList = new AptitudeinvestigateinfoBLL().GetAppFlowList(intromid, "6", curUser);
                if (null != entity)
                {
                    //审查id
                    data.intromid = entity.ID;
                    data.outengineerid = entity.OUTENGINEERID;
                    //外包工程
                    var outengineer = outsouringengineerbll.GetEntity(data.outengineerid);
                    if (null != outengineer)
                    {
                        var didBll = new DataItemDetailBLL();

                        data.outengineer = outengineer.ENGINEERNAME;
                        data.deptName = outengineer.ENGINEERLETDEPT;//发包单位
                        data.projectcode = outengineer.ENGINEERCODE; //工程编号
                        //工程类型
                        var listType = didBll.GetDataItem("ProjectType", outengineer.ENGINEERTYPE).ToList();
                        if (listType != null && listType.Count > 0)
                        {
                            data.projecttype = listType[0].ItemName;
                        }
                        //var areaEntity = new DistrictBLL().GetEntity(outengineer.ENGINEERAREA);
                        //if (null != areaEntity)
                        //{
                        //    data.areaname = areaEntity.DistrictName; //所属区域
                        //}
                        data.areaname = outengineer.EngAreaName;
                        //工程风险等级
                        var listLevel = didBll.GetDataItem("ProjectLevel", outengineer.ENGINEERLEVEL).ToList();
                        if (listLevel != null && listLevel.Count > 0)
                        {
                            data.projectlevel = listLevel[0].ItemName; //工程风险等级
                        }
                        data.projectcontent = outengineer.ENGINEERCONTENT; //工程内容
                    }
                    data.applypeople = entity.APPLYPEOPLE;
                    data.applytime = entity.APPLYTIME;
                    string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                    //审查记录
                    List<IntromDetail> list = new List<IntromDetail>();
                    var rcdt = intromissionbll.GetDtRecordList(intromid); //获取对应的审查内容
                    if (rcdt.Rows.Count > 0)
                    {
                        foreach (DataRow row in rcdt.Rows)
                        {
                            IntromDetail dentity = new IntromDetail();
                            dentity.detailid = row["id"].ToString();
                            dentity.contentid = row["investigatecontentid"].ToString();
                            dentity.content = row["investigatecontent"].ToString();
                            dentity.result = row["investigateresult"].ToString();
                            dentity.peoplename = row["investigatepeople"].ToString();
                            dentity.peopleid = row["investigatepeopleid"].ToString();
                            dentity.signpic = string.IsNullOrWhiteSpace(row["signpic"].ToString()) ? "" : webUrl + row["signpic"].ToString().Replace("../../", "/");
                            list.Add(dentity);
                        }
                    }
                    data.detaildata = list;
                    //审核记录
                    List<AuditResult> alist = new List<AuditResult>();
                    var auditdt = aptitudeinvestigateauditbll.GetAuditRecList(intromid);
                    foreach (DataRow row in auditdt.Rows)
                    {
                        AuditResult aentity = new AuditResult();
                        aentity.approveopinion = row["auditopinion"].ToString();
                        aentity.approveresult = row["auditresult"].ToString();
                        aentity.auditdept = row["auditdept"].ToString();
                        aentity.auditpeople = row["auditpeople"].ToString();
                        aentity.auditsignimg = string.IsNullOrWhiteSpace(row["auditsignimg"].ToString())  ? "" : webUrl + row["auditsignimg"].ToString().Replace("../../", "/");
                        aentity.audittime = !string.IsNullOrEmpty(row["audittime"].ToString()) ? Convert.ToDateTime(row["audittime"].ToString()).ToString("yyyy-MM-dd") : "";
                        alist.Add(aentity);
                    }
                    data.auditdata = alist;
                    data.nodeList = nodeList;
                }
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "获取失败" };
            }
            return new { code = 0, count = 0, info = "获取成功", data = data };
        }
        #endregion

        #region 获取历史入场申请记录
        /// <summary>
        /// 获取入场申请记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHistoryIntromJson([FromBody]JObject json)
        {

            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            List<HistoryIntrom> list = new List<HistoryIntrom>();

            try
            {
                string intromid = res.Contains("intromid") ? dy.data.intromid.ToString() : ""; //入场申请id 

                var historyDt = intromissionhistorybll.GetEntityByIntromId(intromid); //集合

                foreach (DataRow row in historyDt.Rows)
                {
                    HistoryIntrom entity = new HistoryIntrom();
                    entity.historyintromid = row["id"].ToString();
                    entity.outengineer = row["engineername"].ToString();
                    entity.applypeople = row["applypeople"].ToString();
                    entity.applytime = !string.IsNullOrEmpty(row["applytime"].ToString()) ? Convert.ToDateTime(row["applytime"].ToString()) : DateTime.Now;

                    list.Add(entity);
                }
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "获取失败" };
            }
            return new { code = 0, count = 0, info = "获取成功", data = list };
        }
        #endregion

        #region 获取历史入场申请记录/详细信息
        /// <summary>
        /// 获取入场申请记录/详细信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHistoryIntromDetail([FromBody]JObject json)
        {

            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            IntromData data = new IntromData();
            try
            {
                string historyintromid = res.Contains("historyintromid") ? dy.data.historyintromid.ToString() : ""; //历史入厂许可申请id

                //查询入厂许可申请记录
                var entity = intromissionhistorybll.GetEntity(historyintromid);

                if (null != entity)
                {
                    data.intromid = entity.ID;
                    data.outengineerid = entity.OUTENGINEERID;
                    //获取外包工程
                    var outengineer = outsouringengineerbll.GetEntity(data.outengineerid);
                    if (null != outengineer)
                    {
                        var didBll = new DataItemDetailBLL();

                        data.outengineer = outengineer.ENGINEERNAME;
                        data.deptName = outengineer.ENGINEERLETDEPT;//发包单位
                        data.projectcode = outengineer.ENGINEERCODE; //工程编号
                        //工程类型
                        var listType = didBll.GetDataItem("ProjectType", outengineer.ENGINEERTYPE).ToList();
                        if (listType != null && listType.Count > 0)
                        {
                            data.projecttype = listType[0].ItemName;
                        }
                        //var areaEntity = new DistrictBLL().GetEntity(outengineer.ENGINEERAREA);
                        //if (null != areaEntity)
                        //{
                        //    data.areaname = areaEntity.DistrictName; //所属区域
                        //}
                        data.areaname = outengineer.EngAreaName;
                        //工程风险等级
                        var listLevel = didBll.GetDataItem("ProjectLevel", outengineer.ENGINEERLEVEL).ToList();
                        if (listLevel != null && listLevel.Count > 0)
                        {
                            data.projectlevel = listLevel[0].ItemName; //工程风险等级
                        }
                        data.projectcontent = outengineer.ENGINEERCONTENT; //工程内容

                    }
                    data.applypeople = entity.APPLYPEOPLE;
                    data.applytime = entity.APPLYTIME;
                    string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                    //审查记录
                    List<IntromDetail> list = new List<IntromDetail>();

                    var rcdt = intromissionbll.GetHistoryDtRecordList(historyintromid); //获取对应的审查内容

                    if (rcdt.Rows.Count > 0)
                    {
                        foreach (DataRow row in rcdt.Rows)
                        {
                            IntromDetail dentity = new IntromDetail();
                            dentity.detailid = row["id"].ToString();
                            dentity.contentid = row["investigatecontentid"].ToString();
                            dentity.content = row["investigatecontent"].ToString();
                            dentity.result = row["investigateresult"].ToString();
                            dentity.peoplename = row["investigatepeople"].ToString();
                            dentity.peopleid = row["investigatepeopleid"].ToString();
                            dentity.signpic =string.IsNullOrWhiteSpace(row["signpic"].ToString()) ? "" : webUrl + row["signpic"].ToString().Replace("../../", "/");
                            list.Add(dentity);
                        }
                    }
                    data.detaildata = list;

                    //审核记录
                    List<AuditResult> alist = new List<AuditResult>();
                    var auditdt = aptitudeinvestigateauditbll.GetAuditRecList(historyintromid);
                    foreach (DataRow row in auditdt.Rows)
                    {
                        AuditResult aentity = new AuditResult();
                        aentity.approveopinion = row["auditopinion"].ToString();
                        aentity.approveresult = row["auditresult"].ToString();
                        aentity.auditdept = row["auditdept"].ToString();
                        aentity.auditpeople = row["auditpeople"].ToString();
                        aentity.auditsignimg = string.IsNullOrWhiteSpace(row["auditsignimg"].ToString()) ? "" : webUrl + row["auditsignimg"].ToString().Replace("../../", "/");
                        aentity.audittime = !string.IsNullOrEmpty(row["audittime"].ToString()) ? Convert.ToDateTime(row["audittime"].ToString()).ToString("yyyy-MM-dd") : "";
                        alist.Add(aentity);
                    }
                    data.auditdata = alist;

                }

            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "获取失败" };
            }
            return new { code = 0, count = 0, info = "获取成功", data = data };
        }
        #endregion
    }
}