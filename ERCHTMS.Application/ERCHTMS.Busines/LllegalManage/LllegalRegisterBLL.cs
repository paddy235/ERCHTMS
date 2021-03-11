using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using ERCHTMS.Service.LllegalManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Service.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using System.Net;
using System.Dynamic;
using Newtonsoft.Json;
using ERCHTMS.Busines.SystemManage;
using System.Web;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.Attributes;
using ERCHTMS.Code;
using System.Linq;
using ERCHTMS.Entity.LllegalManage.ViewModel;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Entity.SystemManage.ViewModel;
using BSFramework.Util;
using System.Text;

namespace ERCHTMS.Busines.LllegalManage
{
    /// <summary>
    /// 描 述：违章基本信息
    /// </summary>
    public class LllegalRegisterBLL
    {
        private LllegalRegisterIService service = new LllegalRegisterService();
        private HTWorkFlowIService htservice = new HTWorkFlowService();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private UserBLL userbll = new UserBLL();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LllegalRegisterEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// 获取通用查询分页
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public DataTable GetGeneralQuery(Pagination pagination)
        {
            return service.GetGeneralQuery(pagination);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalRegisterEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        #region 根据检查id获取对应的违章集合
        /// <summary>
        /// 根据检查id获取对应的违章集合
        /// </summary>
        /// <param name="checkId"></param>
        /// <param name="checkman"></param>
        /// <param name="flowstate"></param>
        /// <returns></returns>
        public DataTable GetListByCheckId(string checkId, string checkman, string flowstate)
        {
            return service.GetListByCheckId(checkId, checkman, flowstate);
        }
        #endregion

        #region 获取新编码
        /// <summary>
        /// 获取新编码
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="maxfields"></param>
        /// <param name="seriallen"></param>
        /// <returns></returns>
        public string GenerateHidCode(string tablename, string maxfields, int seriallen)
        {
            return service.GenerateHidCode(tablename, maxfields, seriallen);
        }
        #endregion

        #region 通过违章编号，来判断是否存在重复现象
        /// <summary>
        /// 通过违章编号，来判断是否存在重复现象
        /// </summary>
        /// <param name="LllegalNumber"></param>
        /// <returns></returns>
        public IList<LllegalRegisterEntity> GetListByNumber(string LllegalNumber)
        {
            return service.GetListByNumber(LllegalNumber);
        }
        #endregion

        #region 通过当前用户获取对应违章的违章描述(取前十个)
        /// <summary>
        /// 通过当前用户获取对应违章的违章描述
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetLllegalDescribeList(string userId, string lllegaldescribe)
        {
            return service.GetLllegalDescribeList(userId, lllegaldescribe);
        }
        #endregion


        /// <summary>
        /// 获取个人(反)违章档案
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetLllegalForPersonRecord(string userId)
        {
            try
            {
                return service.GetLllegalForPersonRecord(userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LllegalRegisterEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 删除违章相关所有内容
        /// <summary>
        /// 删除违章相关所有内容
        /// </summary>
        /// <param name="keyValue"></param>
        public void RemoveFormByBid(string keyValue)
        {
            try
            {
                service.RemoveFormByBid(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #endregion

        #region  违章基础信息查询
        /// <summary>
        /// 违章基础信息查询    
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetLllegalBaseInfo(Pagination pagination, string queryJson)
        {
            try
            {
                return service.GetLllegalBaseInfo(pagination, queryJson);
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }
        #endregion

        #region 违章实体所有元素对象
        /// <summary>
        /// 违章实体所有元素对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetLllegalModel(string keyValue)
        {
            try
            {
                return service.GetLllegalModel(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 更新业务状态
        /// <summary>
        /// 更新业务状态
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public bool UpdateFlowStateByObjectId(string tableName, string fieldName, string objectId)
        {
            try
            {
                return htservice.UpdateWorkStreamByObjectId(tableName, fieldName, objectId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 获取违章档案(班组端)
        /// <summary>
        /// 获取违章档案(班组端)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public LllegalRecord GetLllegalRecord(string userid, string year)
        {
            try
            {
                return service.GetLllegalRecord(userid, year);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 获取积分数据
        /// <summary>
        /// 获取积分数据
        /// </summary>
        /// <param name="basePoint"></param>
        /// <param name="year"></param>
        /// <param name="userids"></param>
        /// <returns></returns>
        public DataTable GetLllegalPointData(string basePoint, string year, string userids, string condition)
        {
            return service.GetLllegalPointData(basePoint, year, userids, condition);
        }
        #endregion


        #region 评分及违章考核
        public void AddLllegalScore(LllegalRegisterEntity entity)
        {
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            #region 违章评分对象
            try
            {
                string fileName = "推送违章对接培训平台接口" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL();
                #region 添加用户积分
                string lllegaluserids = string.Empty;
                string lllegaldeptcode = string.Empty;
                //考核人
                var relevanceList = lllegalpunishbll.GetListByLllegalId(entity.ID, "");
                foreach (LllegalPunishEntity lpEntity in relevanceList)
                {
                    if (lpEntity.ASSESSOBJECT.Contains("人员") && !string.IsNullOrEmpty(lpEntity.PERSONINCHARGEID))
                    {
                        //违章责任人
                        lllegalpunishbll.SaveUserScore(lpEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                        lllegaluserids += lpEntity.PERSONINCHARGEID + ",";
                    }
                    else if (lpEntity.ASSESSOBJECT.Contains("单位"))
                    {
                        lllegaldeptcode += lpEntity.PERSONINCHARGEID + ","; //记录单位编码
                    }
                }
                #endregion
                //考核用户id
                if (!string.IsNullOrEmpty(lllegaluserids)) { lllegaluserids = lllegaluserids.Substring(0, lllegaluserids.Length - 1); }
                //考核部门
                if (!string.IsNullOrEmpty(lllegaldeptcode)) { lllegaldeptcode = lllegaldeptcode.Substring(0, lllegaldeptcode.Length - 1); }

                #region 消息提醒考核的人员、考核的单位
                //消息通知的用户
                string pushaccount = string.Empty;
                string pushusernames = string.Empty;
                string pushcode = "WZ012"; //违章考核消息通知
                //考核人员
                if (!string.IsNullOrEmpty(lllegaluserids))
                {
                    DataTable dt = userbll.GetUserByDeptCodeAndRoleName(lllegaluserids, null, null);
                    if (dt.Rows.Count == 1)
                    {
                        pushaccount += dt.Rows[0]["account"].ToString() + ","; //账户
                        pushusernames += dt.Rows[0]["realname"].ToString() + ","; //姓名
                    }
                }
                //考核单位(获取单位负责人)
                if (!string.IsNullOrEmpty(lllegaldeptcode))
                {
                    DataTable dt = userbll.GetUserByDeptCodeAndRoleName(null, lllegaldeptcode, "负责人");
                    if (dt.Rows.Count == 1)
                    {
                        pushaccount += dt.Rows[0]["account"].ToString() + ","; //账户
                        pushusernames += dt.Rows[0]["realname"].ToString() + ","; //姓名
                    }
                }
                if (!string.IsNullOrEmpty(pushaccount))
                {
                    pushaccount = pushaccount.Substring(0, pushaccount.Length - 1);
                    pushusernames = pushusernames.Substring(0, pushusernames.Length - 1);
                    //极光消息推送
                    JPushApi.PushMessage(pushaccount, pushusernames, pushcode, entity.ID);
                }
                #endregion

    
                #region 违章评分对象
                List<FwzTrainUserInfo> fwzlist = new List<FwzTrainUserInfo>();
                var lllegalPoint = dataitemdetailbll.GetDataItemListByItemCode("'LllegalTrainPointSetting'");

                if (lllegalPoint.Count() > 0)
                {
                    var LllegalTrainingPointValue = lllegalPoint.Where(p => p.ItemName == "LllegalTrainingPointValue").FirstOrDefault();//违章培训积分值
                    var LllegalTrainingPointStandard = lllegalPoint.Where(p => p.ItemName == "LllegalTrainingPointStandard").FirstOrDefault();//违章培训积分标准
                    string basePoint = string.Empty; //基础分数值
                    string pointStandard = string.Empty; //评分标准
                    string lllegaltypetraineeprojectid = string.Empty; //违章类型培训项目
                    string safetyruletraineeprojectid = string.Empty;//安规培训项目
                    if (null != LllegalTrainingPointValue)
                    {
                        basePoint = LllegalTrainingPointValue.ItemValue;
                    }
                    if (null != LllegalTrainingPointStandard)
                    {
                        pointStandard = LllegalTrainingPointStandard.ItemValue;
                    }
                    ////违章类型培训项目
                    string wzKey = "LllegalTypeTraineeProjectId_" + curUser.OrganizeCode;
                    if (lllegalPoint.Where(p => p.ItemName == wzKey).Count() > 0)
                    {
                        var LllegalTypeTraineeProjectId = lllegalPoint.Where(p => p.ItemName == wzKey).FirstOrDefault();
                        if (null != LllegalTypeTraineeProjectId)
                        {
                            if (LllegalTypeTraineeProjectId.EnabledMark == 1)
                            {
                                lllegaltypetraineeprojectid = LllegalTypeTraineeProjectId.ItemValue;
                            }
                        }
                    }
                    //安规培训项目
                    string agKey = "SafetyRuleTraineeProjectId_" + curUser.OrganizeCode;
                    var SafetyRuleTraineeProjectId = lllegalPoint.Where(p => p.ItemName == agKey).FirstOrDefault();
                    if (null != SafetyRuleTraineeProjectId)
                    {
                        if (SafetyRuleTraineeProjectId.EnabledMark == 1)
                        {
                            safetyruletraineeprojectid = SafetyRuleTraineeProjectId.ItemValue;
                        }
                    }
                    //评分标准
                    #region 评分标准
                    if (!string.IsNullOrEmpty(pointStandard))
                    {
                        DataTable resultDt = new DataTable();
                        resultDt.Columns.Add("username");
                        resultDt.Columns.Add("account");
                        resultDt.Columns.Add("score");
                        resultDt.Columns.Add("lllegalpoint");

                        string[] standardarr = pointStandard.Split(',');

                        //0分人员
                        foreach (string standard in standardarr)
                        {
                            DataTable upointdt = new DataTable();

                            upointdt = GetLllegalPointData(basePoint, DateTime.Now.Year.ToString(), lllegaluserids, standard);

                            foreach (DataRow row in upointdt.Rows)
                            {
                                decimal score = !string.IsNullOrEmpty(row["score"].ToString()) ? Convert.ToDecimal(row["score"].ToString()) : 0; //剩余分数
                                decimal lllegalpoint = !string.IsNullOrEmpty(row["lllegalpoint"].ToString()) ? Convert.ToDecimal(row["lllegalpoint"].ToString()) : 0; //扣除分数
                                DataRow rrow = resultDt.NewRow();
                                rrow["username"] = row["realname"].ToString();//人员姓名
                                rrow["account"] = row["account"].ToString();//人员账号
                                rrow["score"] = score.ToString();//剩余分数
                                rrow["lllegalpoint"] = lllegalpoint.ToString();//扣除分数
                                resultDt.Rows.Add(rrow);

                                FwzTrainUserInfo fwzuser = new FwzTrainUserInfo();
                                fwzuser.Useraccount = row["account"].ToString();
                                fwzuser.TrainStartTime = null != entity.LLLEGALTIME ? entity.LLLEGALTIME.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                fwzuser.TrainEndTime = null != entity.LLLEGALTIME ? entity.LLLEGALTIME.Value.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss");
                                //安规培训
                                if (standard.Trim() == "score=0")
                                {
                                    fwzuser.StationName = "所有人员";
                                    fwzuser.TrainProjectid = safetyruletraineeprojectid; //安规
                                }
                                else
                                {
                                    fwzuser.StationName = dataitemdetailbll.GetEntity(entity.LLLEGALTYPE).ItemName;
                                    if (!string.IsNullOrEmpty(lllegaltypetraineeprojectid))
                                    {
                                        fwzuser.TrainProjectid = lllegaltypetraineeprojectid;
                                    }
                                    else
                                    {
                                        var lllegalPointObj = lllegalPoint.Where(P => P.ItemName == fwzuser.StationName);
                                        if (lllegalPointObj.Count() > 0)
                                        {
                                            fwzuser.TrainProjectid = lllegalPointObj.FirstOrDefault().ItemValue;
                                        }
                                    }
                                }
                                fwzlist.Add(fwzuser);
                            }
                        }

                        if (fwzlist.Count() > 0)
                        {
                            dynamic lllegaldy = PushUserToTrainee(fwzlist);

                            if (null != lllegaldy)
                            {
                                //返回成功状态下
                                if (lllegaldy.Code.ToString() == "0")
                                {
                                    string curcode = "WZ013";
                                    string content = string.Empty;
                                    foreach (DataRow rrow in resultDt.Rows)
                                    {
                                        string score = rrow["score"].ToString(); //剩余分数
                                        string lllegalpoint = rrow["lllegalpoint"].ToString(); //扣除分数
                                        string curaccount = rrow["account"].ToString(); //账户
                                        string curname = rrow["username"].ToString(); //姓名
                                        content = "您于" + DateTime.Now.ToString("yyyy-MM-dd") + "因违章被考核了" + lllegalpoint + "分,现反违章积分为" + score + "分,需进行反违章培训,请前去参加培训.";
                                        JPushApi.PushMessage(curaccount, curname, curcode, "违章考核信息", content, entity.ID);
                                    }
                                }
                                string resultInfo = JsonConvert.SerializeObject(lllegaldy);
                                LogEntity logEntity = new LogEntity();
                                logEntity.Browser = System.Web.HttpContext.Current.Request.Browser.Browser;
                                logEntity.CategoryId = 5;
                                logEntity.OperateTypeId = ((int)OperationType.Submit).ToString();
                                logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Submit);
                                logEntity.OperateAccount = curUser.UserName;
                                logEntity.OperateUserId = curUser.UserId;
                                logEntity.ExecuteResult = 1;
                                logEntity.Module = SystemInfo.CurrentModuleName;
                                logEntity.ModuleId = SystemInfo.CurrentModuleId;
                                logEntity.ExecuteResultJson = resultInfo;
                                LogBLL.WriteLog(logEntity);
                            }
                        }
                    }
                    #endregion
                }

                #endregion
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
                logEntity.ExecuteResultJson = ex.ToJson();
                logEntity.WriteLog();

                string fileName = "推送当前用户到培训平台，建立相关培训及考试任务_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "推送当前用户到培训平台，建立相关培训及考试任务:" + ex.ToJson() + "\r\n");

            }
            #endregion
        }
        #endregion


        #region 推送当前用户到培训平台，建立相关培训及考试任务
        /// <summary>
        /// 推送当前用户到培训平台，建立相关培训及考试任务
        /// </summary>
        /// <param name="userAccount">学员账号</param>
        /// <param name="projectid">培训项目id</param>
        /// <param name="startdate">培训开始时间</param>
        /// <param name="enddate">培训结束时间</param>
        /// <param name="stationname">受训角色名称</param>
        public dynamic PushUserToTrainee(List<FwzTrainUserInfo> ulist)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            try
            {
                string fileName = "推送违章对接培训平台接口" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                //服务请求地址
                var trainserviceurl = dataitemdetailbll.GetItemValue("TrainServiceUrl"); //.net 版本
                var wztrainserviceurl = dataitemdetailbll.GetItemValue("WzTrainServiceUrl");//java版本
                var whatway = dataitemdetailbll.GetItemValue("WhatWay");
                //.net版本
                if (!string.IsNullOrEmpty(trainserviceurl) && whatway == "0")
                {
                    WebClient wc = new WebClient();
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    //发送请求到web api并获取返回值，默认为post方式
                    System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                    string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        Business = "SaveFwzTrainUserInfo",
                        FwzTrainUserInfoList = ulist
                    });
                    nc.Add("json", queryJson);
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), "推送违章对接培训平台接口:" + queryJson + ",地址:" + wztrainserviceurl + ",请求入口:Net" + "\r\n");
                    wc.UploadValuesCompleted += wc_UploadValuesCompleted;
                    byte[] arr = wc.UploadValues(new Uri(trainserviceurl), nc);
                    return JsonConvert.DeserializeObject<ExpandoObject>(System.Text.Encoding.UTF8.GetString(arr));
                }
                //java 版本
                else  if (!string.IsNullOrEmpty(wztrainserviceurl) && whatway == "1")
                {
                    WebClient wc = new WebClient();
                    wc.Headers.Add("Content-Type", "application/json;charset=UTF-8");
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    //发送请求到web api并获取返回值，默认为post方式
                    System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                    List<object> resultlist = new List<object>();
                    foreach (FwzTrainUserInfo entity in ulist)
                    {
                        resultlist.Add(new {
                            trainProjectId = entity.TrainProjectid,
                            userAccount = entity.Useraccount,
                            trainStartTime = entity.TrainStartTime,
                            trainEndTime = entity.TrainEndTime
                        });
                    }
                    string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        Business = "SaveFwzTrainUserInfo",
                        FwzTrainUserInfoList = resultlist
                    });
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), "推送违章对接培训平台接口:" + jsonData.ToJson() + ",地址:" + wztrainserviceurl + ",请求入口:java" + ";\r\n");
                    byte[] bsdata = Encoding.UTF8.GetBytes(jsonData);
                    wc.UploadValuesCompleted += wc_UploadValuesCompleted;
                    byte[] arr = wc.UploadData(new Uri(wztrainserviceurl), "POST", bsdata);
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), "推送违章对接培训平台接口:返回的结果:" + System.Text.Encoding.UTF8.GetString(arr)  + ";\r\n");
                    return new
                    {
                        Code = 0,
                        Info = "推送成功!"
                    };
                }
                else
                {
                    return null;
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
                logEntity.ExecuteResultJson = ex.ToJson();
                logEntity.Module = SystemInfo.CurrentModuleName;
                logEntity.ModuleId = SystemInfo.CurrentModuleId;
                logEntity.WriteLog();
                string fileName = "推送当前用户到培训平台，建立相关培训及考试任务_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "推送当前用户到培训平台，建立相关培训及考试任务:" + ex.ToJson() + "\r\n");

                return null;
            }
        }
        #endregion

        void wc_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            Operator user = OperatorProvider.Provider.Current();

            string fileName = "推送当前用户到培训平台，建立相关培训及考试任务_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            //将同步结果写入日志文件
            try
            {
                int _actionType = 4;
                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = _actionType;
                logEntity.OperateTypeId = _actionType.ToString();
                logEntity.OperateType = EnumAttribute.GetDescription(GetOperationType(_actionType.ToString()));
                logEntity.OperateAccount = user.UserName;
                logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
                logEntity.ExecuteResult = 1;
                logEntity.Module = SystemInfo.CurrentModuleName;
                logEntity.ModuleId = SystemInfo.CurrentModuleId;
                logEntity.ExecuteResultJson = "操作信息:推送当前用户到培训平台，建立相关培训及考试任务, 返回结果:" + System.Text.Encoding.UTF8.GetString(e.Result) + ",Json信息:" + e.ToJson();
                logEntity.WriteLog();

                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "推送当前用户到培训平台，建立相关培训及考试任务:" + e.ToJson() + "\r\n");
            }
            catch (Exception ex)
            {
                int _actionType = 5;
                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = _actionType;
                logEntity.OperateTypeId = _actionType.ToString();
                logEntity.OperateType = EnumAttribute.GetDescription(GetOperationType(_actionType.ToString()));
                logEntity.OperateAccount = user.UserName;
                logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
                logEntity.ExecuteResult = 1;
                logEntity.Module = SystemInfo.CurrentModuleName;
                logEntity.ModuleId = SystemInfo.CurrentModuleId;
                logEntity.ExecuteResultJson = "操作信息:推送当前用户到培训平台，建立相关培训及考试任务, 错误信息:" + ex.Message + " , 异常信息:" + ex.InnerException +
                    " , 异常源:" + ex.Source + " , 异常目标:" + ex.TargetSite + ",异常JSON:" + ex.ToJson();
                logEntity.WriteLog();

                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyyMMddHHmmss-") + "推送当前用户到培训平台，建立相关培训及考试任务:" + ex.Message + " , 异常信息:" + ex.InnerException +
                    " , 异常源:" + ex.Source + " , 异常目标:" + ex.TargetSite + ",异常JSON:" + ex.ToJson() + ";\r\n");
            }
        }

        /// <summary>
        /// 常用操作枚举
        /// </summary>
        /// <param name="operationType"></param>
        /// <returns></returns>
        public OperationType GetOperationType(string operationType)
        {
            OperationType opera = new OperationType();
            switch (operationType)
            {
                case "0":
                    opera = OperationType.Other;
                    break;
                case "Other":
                    opera = OperationType.Other;
                    break;

                case "1":
                    opera = OperationType.Login;
                    break;
                case "Login":
                    opera = OperationType.Login;
                    break;

                case "2":
                    opera = OperationType.Exit;
                    break;
                case "Exit":
                    opera = OperationType.Exit;
                    break;

                case "3":
                    opera = OperationType.Visit;
                    break;
                case "Visit":
                    opera = OperationType.Visit;
                    break;

                case "4":
                    opera = OperationType.Leave;
                    break;
                case "Leave":
                    opera = OperationType.Leave;
                    break;

                case "5":
                    opera = OperationType.Create;
                    break;
                case "Create":
                    opera = OperationType.Create;
                    break;

                case "6":
                    opera = OperationType.Delete;
                    break;
                case "Delete":
                    opera = OperationType.Delete;
                    break;

                case "7":
                    opera = OperationType.Update;
                    break;
                case "Update":
                    opera = OperationType.Update;
                    break;

                case "8":
                    opera = OperationType.Submit;
                    break;
                case "Submit":
                    opera = OperationType.Submit;
                    break;

                case "9":
                    opera = OperationType.Exception;
                    break;
                case "Exception":
                    opera = OperationType.Exception;
                    break;

                case "10":
                    opera = OperationType.AppLogin;
                    break;
                case "AppLogin":
                    opera = OperationType.AppLogin;
                    break;
            }

            return opera;

        }

        #region 获取违章曝光
        /// <summary>
        /// 获取违章曝光
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public DataTable QueryExposureLllegal(string num)
        {
            try
            {
                return service.QueryExposureLllegal(num);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 通过安全检查id获取对应的违章统计数据
        /// <summary>
        /// 通过安全检查id获取对应的违章统计数据
        /// </summary>
        /// <param name="checkids"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetLllegalBySafetyCheckIds(List<string> checkids, int mode)
        {
            try
            {
                return service.GetLllegalBySafetyCheckIds(checkids, mode);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }

    /// <summary>
    /// 违章培训对象
    /// </summary>
    public class FwzTrainUserInfo
    {
        public string Useraccount { get; set; }
        public string TrainProjectid { get; set; }
        public string TrainStartTime { get; set; }
        public string TrainEndTime { get; set; }
        public string StationName { get; set; }
    }
}