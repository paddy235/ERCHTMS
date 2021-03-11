using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PublicInfoManage;
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
    public class DailyExamineController : BaseApiController
    {
        private DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private HistorydailyexamineBLL historydailyexaminebll = new HistorydailyexamineBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        public HttpContext ctx { get { return HttpContext.Current; } }
        #region 获取日常考核列表
        /// <summary>
        /// 获取日常考核列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string actiontype = dy.data.actiontype;//0全部 1 我的 2 待审核
                long pageIndex = dy.pageindex;
                long pageSize = dy.pagesize;
                string examinetodept = res.Contains("examinetodept") ? dy.data.examinetodept : "";
                string startdate = res.Contains("startdate") ? dy.data.startdate : "";
                string enddate = res.Contains("enddate") ? dy.data.enddate : "";
                string examinetype = res.Contains("examinetype") ? dy.data.examinetype : "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "id";
                pagination.p_fields = @"examinetodept,examineperson,to_char(examinetime,'yyyy-mm-dd') examinetime,flowrolename,flowdept,'' as approveuserid,'' as approveusername,
                                        case when isover='1' then '1'
                                          when isover ='0' and issaved='1' then '2'
                                            when isover='0' and issaved ='0' then '0' end status,
                                              case when isover='1' then '审核完成'
                                          when isover ='0' and issaved='1' then '待审核'
                                            when isover='0' and issaved ='0' then '待提交' end statusinfo";
                pagination.p_tablename = "epg_dailyexamine ";
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();
                pagination.conditionJson += "1=1 ";


                string role = currUser.RoleName;
                if (actiontype == "0")
                {
                    if (currUser.IsSystem)
                    {

                    }
                    else
                    {
                        pagination.conditionJson += " and createuserorgcode ='" + currUser.OrganizeCode + "'";
                    }
                    //(s.issaved = '0' and s.createuserid ='{0}')

                    pagination.conditionJson += " and issaved = '1'";
                }
                else if (actiontype == "1")
                {
                    //查询我的
                    pagination.conditionJson += string.Format(" and ((flowdept ='{0}'", currUser.DeptId);

                    string[] arr = currUser.RoleName.Split(',');
                    if (arr.Length > 0)
                    {
                        pagination.conditionJson += " and (";
                        foreach (var item in arr)
                        {
                            pagination.conditionJson += string.Format(" flowrolename  like '%{0}%' or", item);
                        }
                        pagination.conditionJson = pagination.conditionJson.Substring(0, pagination.conditionJson.Length - 2);
                        pagination.conditionJson += " )";
                    }
                    pagination.conditionJson += string.Format(") or (issaved = '0' and createuserid ='{0}'))", currUser.UserId);
                }
                else if (actiontype == "2")//待我审核审批
                {
                    //查询我的
                    pagination.conditionJson += string.Format(" and ((flowdept ='{0}'", currUser.DeptId);

                    string[] arr = currUser.RoleName.Split(',');
                    if (arr.Length > 0)
                    {
                        pagination.conditionJson += " and (";
                        foreach (var item in arr)
                        {
                            pagination.conditionJson += string.Format(" flowrolename  like '%{0}%' or", item);
                        }
                        pagination.conditionJson = pagination.conditionJson.Substring(0, pagination.conditionJson.Length - 2);
                        pagination.conditionJson += " )";
                    }
                    pagination.conditionJson += string.Format(") and isover='0' and issaved='1')");
                }

                if (!string.IsNullOrEmpty(startdate))
                {
                    pagination.conditionJson += string.Format(" and examinetime>= to_date('{0}','yyyy-MM-dd') ", startdate);
                }
                if (!string.IsNullOrEmpty(enddate))
                {
                    pagination.conditionJson += string.Format(" and examinetime<= to_date('{0}','yyyy-MM-dd') ", Convert.ToDateTime(enddate).AddDays(1).ToString("yyyy-MM-dd"));
                }
                if (!string.IsNullOrEmpty(examinetodept))
                {
                    pagination.conditionJson += string.Format(" and examinetodeptid = '{0}' ", examinetodept);
                }
                if (!string.IsNullOrEmpty(examinetype))
                {
                    pagination.conditionJson += string.Format(" and examinetype like '%{0}%' ", examinetype);
                }
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                });
                var data = dailyexaminebll.GetPageList(pagination, queryJson);
                foreach (DataRow row in data.Rows)
                {
                    string str = new ScaffoldBLL().GetUserName(row["flowdept"].ToString(), row["flowrolename"].ToString(), "0");
                    row["approveuserid"] = str.Split('|')[1];
                    row["approveusername"] = str.Split('|')[0];
                }
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取表单
        /// <summary>
        /// 获取表单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetForm([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string keyvalue = dy.data.keyvalue;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                var dailyExamineEntity = dailyexaminebll.GetEntity(keyvalue);
                var files = new FileInfoBLL().GetFiles(keyvalue);//获取相关附件
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (DataRow dr in files.Rows)
                {
                    dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                }

                List<AptitudeinvestigateauditEntity> AptitudeList = aptitudeinvestigateauditbll.GetAuditList(dailyExamineEntity.Id);
                for (int i = 0; i < AptitudeList.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(AptitudeList[i].AUDITSIGNIMG)) AptitudeList[i].AUDITSIGNIMG = string.Empty;
                    else
                        AptitudeList[i].AUDITSIGNIMG = webUrl + AptitudeList[i].AUDITSIGNIMG.ToString().Replace("../../", "/").ToString();
                }
                //查询审核流程图
                List<CheckFlowData> nodeList = new AptitudeinvestigateinfoBLL().GetAppFlowList(keyvalue, "8", curUser);
                return new
                {
                    Code = 0,
                    Count = 1,
                    Info = "获取数据成功",
                    data = new
                    {
                        dailyexamineentity = new
                        {
                            examinecode = dailyExamineEntity.ExamineCode,
                            examinetodept = dailyExamineEntity.ExamineToDept,
                            examinetodeptid = dailyExamineEntity.ExamineToDeptId,
                            examinetype = dailyExamineEntity.ExamineType,
                            examinemoney = dailyExamineEntity.ExamineMoney,
                            examinecontent = dailyExamineEntity.ExamineContent,
                            examinebasis = dailyExamineEntity.ExamineBasis,
                            remark = dailyExamineEntity.Remark,
                            examineperson = dailyExamineEntity.ExaminePerson,
                            examinepersonid = dailyExamineEntity.ExaminePersonId,
                            examinetime = dailyExamineEntity.ExamineTime.Value.ToString("yyyy-MM-dd"),
                            examinedept = dailyExamineEntity.ExamineDept,
                            examinedeptid = dailyExamineEntity.ExamineDeptId
                        },
                        files = files,
                        auditinfo = AptitudeList.Select(g => new
                        {
                            auditresult = g.AUDITRESULT,
                            audittime = g.AUDITTIME.Value.ToString("yyyy-MM-dd"),
                            auditopinion = g.AUDITOPINION,
                            auditdept = g.AUDITDEPT,
                            auditpeople = g.AUDITPEOPLE,
                            auditsignimg = g.AUDITSIGNIMG
                        }).ToList(),
                        nodeList = nodeList
                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 提交表单
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SubmitForm()
        {
            try
            {
                string res = HttpContext.Current.Request["json"]; ;
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string keyValue = res.Contains("keyvalue") ? dy.data.keyvalue : "";
                string userid = dy.userid;
                string fileid = res.Contains("fileid") ? dy.data.fileid : "";

                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

                DailyexamineEntity entity = new DailyexamineEntity
                {
                    Id = keyValue,
                    ExamineCode = dy.data.examinecode,
                    ExamineDept = dy.data.examinedept,
                    ExamineDeptId = dy.data.examinedeptid,
                    ExamineType = dy.data.examinetype,
                    ExamineMoney = Convert.ToDouble(dy.data.examinemoney),
                    ExaminePerson = dy.data.examineperson,
                    ExaminePersonId = dy.data.examinepersonid,
                    ExamineTime = Convert.ToDateTime(dy.data.examinetime),
                    ExamineBasis = dy.data.examinebasis,
                    Remark = dy.data.remark,
                    IsSaved = 1,
                    IsOver = 0,
                    ExamineContent = dy.data.examinecontent,
                    ExamineToDept = dy.data.examinetodept,
                    ExamineToDeptId = dy.data.examinetodeptid
                };
                string state = string.Empty;

                string moduleName = "日常考核";
                string flowid = string.Empty;
                /// <param name="currUser">当前登录人</param>
                /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
                /// <param name="moduleName">模块名称</param>
                /// <param name="outengineerid">工程Id</param>
                ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

                List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, moduleName);
                List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
                //先查出执行部门编码
                for (int i = 0; i < powerList.Count; i++)
                {
                    if (powerList[i].CHECKDEPTCODE == "-3" || powerList[i].CHECKDEPTID == "-3")
                    {
                        if (curUser.RoleName.Contains("班组") || curUser.RoleName.Contains("专业"))
                        {
                            var pDept = new DepartmentBLL().GetParentDeptBySpecialArgs(curUser.ParentId, "部门");
                            powerList[i].CHECKDEPTCODE = pDept.EnCode;
                            powerList[i].CHECKDEPTID = pDept.DepartmentId;
                        }
                        else {
                            powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntity(curUser.DeptId).EnCode;
                            powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntity(curUser.DeptId).DepartmentId;
                        }
                    }
                }
                //登录人是否有审核权限--有审核权限直接审核通过
                for (int i = 0; i < powerList.Count; i++)
                {
                    if (powerList[i].CHECKDEPTID == curUser.DeptId)
                    {
                        var rolelist = curUser.RoleName.Split(',');
                        for (int j = 0; j < rolelist.Length; j++)
                        {
                            if (powerList[i].CHECKROLENAME.Contains(rolelist[j]))
                            {
                                checkPower.Add(powerList[i]);
                                break;
                            }
                        }
                    }
                }
                if (checkPower.Count > 0)
                {
                    ManyPowerCheckEntity check = checkPower.Last();//当前

                    for (int i = 0; i < powerList.Count; i++)
                    {
                        if (check.ID == powerList[i].ID)
                        {
                            flowid = powerList[i].ID;
                        }
                    }
                }
                //if (curUser.RoleName.Contains("公司级用户"))
                //{
                //    mpcEntity = null;
                //}
                if (null != mpcEntity)
                {
                    //保存日常考核
                    entity.FlowDept = mpcEntity.CHECKDEPTID;
                    entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                    entity.FlowRole = mpcEntity.CHECKROLEID;
                    entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                    entity.IsSaved = 1; //标记已经从登记到审核阶段
                    entity.IsOver = 0; //流程未完成，1表示完成
                    entity.FlowID = mpcEntity.ID;
                    entity.FlowName = mpcEntity.CHECKDEPTNAME + "审核中";
                }
                else  //为空则表示已经完成流程
                {
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.FlowRole = "";
                    entity.FlowRoleName = "";
                    entity.IsSaved = 1; //标记已经从登记到审核阶段
                    entity.IsOver = 1; //流程未完成，1表示完成
                    entity.FlowName = "";
                    entity.FlowID = flowid;
                }

                HttpFileCollection files = HttpContext.Current.Request.Files;
                keyValue = string.IsNullOrEmpty(keyValue) ? Guid.NewGuid().ToString() : keyValue;
                if (!string.IsNullOrEmpty(fileid))
                {
                    DeleteFile(fileid);
                }
                string path = string.Empty;
                UploadifyFile(keyValue, files, ref path);

                dailyexaminebll.SaveForm(keyValue, entity);

                //添加审核记录
                if (state == "1")
                {
                    //审核信息表
                    AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                    aidEntity.AUDITRESULT = "0"; //通过
                    aidEntity.AUDITTIME = DateTime.Now;
                    aidEntity.AUDITPEOPLE = curUser.UserName;
                    aidEntity.AUDITPEOPLEID = curUser.UserId;
                    aidEntity.APTITUDEID = entity.Id;  //关联的业务ID 
                    aidEntity.AUDITOPINION = ""; //审核意见
                    aidEntity.AUDITSIGNIMG = curUser.SignImg;
                    aidEntity.FlowId = flowid;
                    aidEntity.AUDITDEPTID = curUser.DeptId;
                    aidEntity.AUDITDEPT = curUser.DeptName;
                    aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
                }

                return new { Code = 0, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        #endregion

        #region 审核表单
        /// <summary>
        /// 审核表单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ApproveForm()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string keyValue = dy.data.keyvalue;
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

                AptitudeinvestigateauditEntity aentity = new AptitudeinvestigateauditEntity
                {
                    AUDITRESULT = dy.data.auditresult,
                    AUDITTIME = Convert.ToDateTime(dy.data.audittime),
                    AUDITPEOPLE = dy.data.auditpeople,
                    AUDITPEOPLEID = dy.data.auditpeopleid,
                    AUDITDEPTID = dy.data.auditdeptid,
                    AUDITDEPT = dy.data.auditdept,
                    AUDITOPINION = dy.data.auditopinion,
                    AUDITSIGNIMG = dy.data.auditsignimg
                };
                string state = string.Empty;

                string moduleName = "日常考核";

                DailyexamineEntity entity = dailyexaminebll.GetEntity(keyValue);
                /// <param name="currUser">当前登录人</param>
                /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
                /// <param name="moduleName">模块名称</param>
                /// <param name="createdeptid">创建人部门ID</param>
                ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, entity.CreateUserDeptId);


                #region //审核信息表
                AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                aidEntity.AUDITRESULT = aentity.AUDITRESULT; //通过
                aidEntity.AUDITTIME = Convert.ToDateTime(aentity.AUDITTIME.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")); //审核时间
                aidEntity.AUDITPEOPLE = aentity.AUDITPEOPLE;  //审核人员姓名
                aidEntity.AUDITPEOPLEID = aentity.AUDITPEOPLEID;//审核人员id
                aidEntity.APTITUDEID = keyValue;  //关联的业务ID 
                aidEntity.AUDITDEPTID = aentity.AUDITDEPTID;//审核部门id
                aidEntity.AUDITDEPT = aentity.AUDITDEPT; //审核部门
                aidEntity.AUDITOPINION = aentity.AUDITOPINION; //审核意见
                aidEntity.FlowId = entity.FlowID;
                aidEntity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace(new DataItemDetailBLL().GetItemValue("imgUrl"), "");
             
                HttpFileCollection files = HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = files[i];
                        string fileOverName = System.IO.Path.GetFileName(file.FileName);
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string FileEextension = Path.GetExtension(file.FileName);
                        //if (fileName == aidEntity.ID)
                        //{
                        string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                        string newFileName = fileName + FileEextension;
                        string newFilePath = dir + "\\" + newFileName;
                        file.SaveAs(newFilePath);
                        aidEntity.AUDITSIGNIMG = "/Resource/sign/" + fileOverName;
                        break;
                        //}
                    }
                }
                aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
                #endregion

                #region  //保存日常考核
                //审核通过
                if (aentity.AUDITRESULT == "0")
                {
                    //0表示流程未完成，1表示流程结束
                    if (null != mpcEntity)
                    {
                        entity.FlowDept = mpcEntity.CHECKDEPTID;
                        entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        entity.FlowRole = mpcEntity.CHECKROLEID;
                        entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        entity.IsSaved = 1;
                        entity.FlowID = mpcEntity.ID;
                        entity.IsOver = 0;
                        entity.FlowName = mpcEntity.CHECKDEPTNAME + "审核中";
                    }
                    else
                    {
                        entity.FlowDept = "";
                        entity.FlowDeptName = "";
                        entity.FlowRole = "";
                        entity.FlowRoleName = "";
                        entity.IsSaved = 1;
                        entity.IsOver = 1;
                        entity.FlowName = "";
                    }
                } 
                else //审核不通过 
                {
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.FlowRole = "";
                    entity.FlowRoleName = "";
                    entity.FlowID = "";
                    entity.IsOver = 0; //处于登记阶段
                    entity.IsSaved = 0; //是否完成状态赋值为未完成
                    entity.FlowName = "";

                }
                //更新日常考核基本状态信息
                dailyexaminebll.SaveForm(keyValue, entity);
                #endregion

                #region    //审核不通过
                if (aentity.AUDITRESULT == "1")
                {
                    //添加历史记录
                    HistorydailyexamineEntity hsentity = new HistorydailyexamineEntity();
                    hsentity.CreateUserId = entity.CreateUserId;
                    hsentity.CreateUserDeptCode = entity.CreateUserDeptCode;
                    hsentity.CreateUserOrgCode = entity.CreateUserOrgCode;
                    hsentity.CreateDate = entity.CreateDate;
                    hsentity.CreateUserName = entity.CreateUserName;
                    hsentity.CreateUserDeptId = entity.CreateUserDeptId;
                    hsentity.ModifyDate = entity.ModifyDate;
                    hsentity.ModifyUserId = entity.ModifyUserId;
                    hsentity.ModifyUserName = entity.ModifyUserName;
                    hsentity.ExamineCode = entity.ExamineCode;
                    hsentity.ExamineDept = entity.ExamineDept;
                    hsentity.ExamineDeptId = entity.ExamineDeptId;
                    hsentity.ExamineToDeptId = entity.ExamineToDeptId;
                    hsentity.ExamineToDept = entity.ExamineToDept;
                    hsentity.ExamineType = entity.ExamineType; //关联ID
                    hsentity.ExamineMoney = Convert.ToDouble(entity.ExamineMoney);
                    hsentity.ExaminePerson = entity.ExaminePerson;
                    hsentity.ExaminePersonId = entity.ExaminePersonId; //关联ID
                    hsentity.ExamineTime = entity.ExamineTime;
                    hsentity.ExamineContent = entity.ExamineContent;
                    hsentity.ExamineBasis = entity.ExamineBasis;
                    hsentity.Remark = entity.Remark;
                    hsentity.ContractId = entity.Id;//关联ID
                    hsentity.IsSaved = 2;
                    hsentity.IsOver = entity.IsOver;
                    hsentity.FlowDeptName = entity.FlowDeptName;
                    hsentity.FlowDept = entity.FlowDept;
                    hsentity.FlowRoleName = entity.FlowRoleName;
                    hsentity.FlowRole = entity.FlowRole;
                    hsentity.FlowName = entity.FlowName;
                    hsentity.Id = "";

                    historydailyexaminebll.SaveForm(hsentity.Id, hsentity);

                    //获取当前业务对象的所有审核记录
                    var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                    //批量更新审核记录关联ID
                    foreach (AptitudeinvestigateauditEntity mode in shlist)
                    {
                        mode.APTITUDEID = hsentity.Id; //对应新的ID
                        aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                    }
                    //批量更新附件记录关联ID
                    var flist = fileinfobll.GetImageListByObject(keyValue);
                    foreach (FileInfoEntity fmode in flist)
                    {
                        fmode.RecId = hsentity.Id; //对应新的ID
                        fileinfobll.SaveForm("", fmode);
                    }
                }
                #endregion

                return new { Code = 0, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        #endregion

        #region 获取考核编号
        /// <summary>
        /// 获取考核编号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetExamineCode()
        {
            try
            {
                string examineCode = dailyexaminebll.GetMaxCode();
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = new { examineCode = examineCode } };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 上传附件、删除附件
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="foldername"></param>
        /// <param name="fileList"></param>
        public void UploadifyFile(string folderId, HttpFileCollection fileList, ref string path)
        {
            try
            {
                if (fileList.Count > 0)
                {
                    for (int i = 0; i < fileList.AllKeys.Length; i++)
                    {
                        if (fileList.AllKeys[i] != "sign")
                        {
                            HttpPostedFile file = fileList[i];
                            //获取文件完整文件名(包含绝对路径)
                            //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                            string userId = OperatorProvider.Provider.Current().UserId;
                            string fileGuid = Guid.NewGuid().ToString();
                            long filesize = file.ContentLength;
                            string FileEextension = Path.GetExtension(file.FileName);
                            string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                            string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\ResourceFile";
                            string newFileName = fileGuid + FileEextension;
                            string newFilePath = dir + "\\" + newFileName;
                            //创建文件夹
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }
                            FileInfoEntity fileInfoEntity = new FileInfoEntity();
                            if (!System.IO.File.Exists(newFilePath))
                            {
                                //保存文件
                                file.SaveAs(newFilePath);
                                //文件信息写入数据库
                                fileInfoEntity.Create();
                                fileInfoEntity.FileId = fileGuid;
                                fileInfoEntity.RecId = folderId; //关联ID
                                fileInfoEntity.FileName = file.FileName;
                                fileInfoEntity.FilePath = "~/Resource/ResourceFile/" + newFileName;
                                fileInfoEntity.FileSize = (Math.Round(decimal.Parse(filesize.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                                fileInfoEntity.FileExtensions = FileEextension;
                                fileInfoEntity.FileType = FileEextension.Replace(".", "");
                                fileinfobll.SaveForm("", fileInfoEntity);
                            }
                        }
                        else
                        {
                            HttpPostedFile file = fileList[i];
                            string fileOverName = System.IO.Path.GetFileName(file.FileName);
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                            string FileEextension = Path.GetExtension(file.FileName);
                            //if (fileName == scEntity.ID)
                            //{
                            string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                            string newFileName = fileName + FileEextension;
                            string newFilePath = dir + "\\" + newFileName;
                            file.SaveAs(newFilePath);
                            //scEntity.SIGNPIC = "/Resource/sign/" + fileOverName;
                            //    break;
                            //}
                            path = fileOverName;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="fileInfoIds"></param>
        public bool DeleteFile(string fileInfoIds)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(fileInfoIds))
            {
                string ids = "";

                string[] strArray = fileInfoIds.Split(',');

                foreach (string s in strArray)
                {
                    ids += "'" + s + "',";
                    var entity = fileinfobll.GetEntity(s);
                    if (entity != null)
                    {
                        var filePath = ctx.Server.MapPath(entity.FilePath);
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }

                if (!string.IsNullOrEmpty(ids))
                {
                    ids = ids.Substring(0, ids.Length - 1);
                }
                int count = fileinfobll.DeleteFileForm(ids);

                result = count > 0 ? true : false;
            }

            return result;
        }
        public void DeleteFileByRec(string recId)
        {
            if (!string.IsNullOrWhiteSpace(recId))
            {
                var list = fileinfobll.GetFileList(recId);
                foreach (var file in list)
                {
                    fileinfobll.RemoveForm(file.FileId);
                    var filePath = ctx.Server.MapPath(file.FilePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
        }
        #endregion
    }
}
