using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;
using System.Linq;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.IService.BaseManage;
using System.Collections.Generic;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Collections;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：高风险通用作业申请
    /// </summary>
    public class HighRiskCommonApplyBLL
    {
        private HighRiskCommonApplyIService service = new HighRiskCommonApplyService();
        private IManyPowerCheckService manypowercheckservice = new ManyPowerCheckService();
        private IUserService userservice = new UserService();

        #region 获取数据

        /// <summary>
        /// 得到当前最大编号
        /// 编号规则：类型首字母+年份+3位数（如J2018001、J2018002）
        /// </summary>
        /// <returns></returns>
        public string GetMaxCode()
        {
            object o = service.GetMaxCode();
            if (o == null || o.ToString() == "")
                return "G" + DateTime.Now.Year + "001";
            int num = Convert.ToInt32(o.ToString().Substring(4));
            return "G" + DateTime.Now.Year + num.ToString().PadLeft(3, '0');
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<HighRiskCommonApplyEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HighRiskCommonApplyEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HighRiskCommonApplyEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取高风险通用台账
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetLedgerList(Pagination pagination, string queryJson, Boolean GetOperate = true)
        {
            return service.GetLedgerList(pagination, queryJson, GetOperate);
        }

        /// <summary>
        /// 获取高风险通用作业申请列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            return service.GetPageDataTable(pagination, queryJson);
        }

        public DataTable GetTable(string sql)
        {
            return service.GetTable(sql);
        }

        /// <summary>
        /// 转交短消息
        /// </summary>
        /// <param name="keyValue"></param>
        public void TransformSendMessage(TransferrecordEntity entity)
        {
            //1、判断当前是措施确认还是审批阶段
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HighRiskCommonApplyEntity commonEntity = service.GetEntity(entity.RecId);
            string moduleName = GetModuleName(commonEntity);
            PushMessageData pushdata = new PushMessageData();
            if (commonEntity.FlowName == "确认中")
            {
                //措施确认转交
                pushdata.SendCode = "ZY001";
            }
            else
            {
                //审核转交
                pushdata.SendCode = "ZY002";
            }
            //极光推送
            pushdata.EntityId = commonEntity.Id;
            pushdata.UserAccount = entity.InTransferUserAccount;
            PushMessageForCommon(pushdata);
        }

        /// <summary>
        /// 获取审核流程名称
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetModuleName(HighRiskCommonApplyEntity entity)
        {
            return service.GetModuleName(entity);
        }

        /// <summary>
        /// 获取执行部门
        /// </summary>
        /// <param name="workdepttype">作业单位类型</param>
        /// <param name="workdept">作业单位</param>
        /// <param name="projectid">外包工程ID</param>
        /// <param name="Executedept">执行部门</param>
        public void GetExecutedept(string workdepttype, string workdept, string projectid, out string Executedept)
        {
            service.GetExecutedept(workdepttype, workdept, projectid, out Executedept);
        }

        /// <summary>
        /// 获取外包单位
        /// </summary>
        /// <param name="workdept">作业单位</param>
        /// <param name="outsouringengineerdept"></param>
        public void GetOutsouringengineerDept(string workdept, out string outsouringengineerdept)
        {
            service.GetOutsouringengineerDept(workdept, out outsouringengineerdept);
        }

        public List<CheckFlowData> GetAppFlowList(string keyValue, string modulename)
        {
            return service.GetAppFlowList(keyValue, modulename);
        }

        public Flow GetFlow(string keyValue, string modulename)
        {
            return service.GetFlow(keyValue, modulename);
        }

        /// <summary>
        /// 修改sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int UpdateData(string sql)
        {
            return service.UpdateData(sql);
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
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateForm(HighRiskCommonApplyEntity entity)
        {
            try
            {
                service.UpdateForm(entity);
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
        public void SaveForm(string keyValue, string type, HighRiskCommonApplyEntity entity, List<HighRiskRecordEntity> list, List<HighRiskApplyMBXXEntity> mbList)
        {
            try
            {
                PushMessageData pushdata = service.SaveForm(keyValue, type, entity, list, mbList);
                if (pushdata != null)
                {
                    if (pushdata.Success == 1 && !string.IsNullOrEmpty(pushdata.SendCode))
                    {
                        pushdata.Content = getName(entity.WorkType, "CommonType");
                        if (pushdata.SendCode == "ZY018")
                        {
                            var high = GetEntity(entity.Id);
                            if (high != null)
                            {
                                String CommonType = pushdata.Content;
                                pushdata.Content = "作业内容：" + high.WorkContent + "&#10;作业时间：" + high.WorkStartTime.Value.ToString("yyyy年MM月dd日 HH时mm分") + " 到 " + high.WorkEndTime.Value.ToString("yyyy年MM月dd日 HH时mm分") + "&#10;作业地点：" + high.WorkPlace;
                                UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                                UserEntity tutelageuserEntity = userservice.GetEntity(high.WorkTutelageUserId);
                                string[] workuserlist = (high.WorkDutyUserId + "," + high.WorkUserIds).Split(',');
                                //List<string> b = workuserlist.ToList();
                                //b.Add(high.WorkDutyUserId);
                                //workuserlist = b.ToArray();
                                DataTable dutyuserDt = new DataTable();
                                dutyuserDt = userservice.GetUserTable(workuserlist);
                                //推送给作业申请人
                                if (userEntity != null)
                                {
                                    JPushApi.PushMessage(userEntity.Account, userEntity.RealName, pushdata.SendCode, "高风险作业(" + CommonType + "）申请已通过，请及时处理。", pushdata.Content, pushdata.EntityId);
                                }
                                //推送给作业负责人/作业人
                                if (dutyuserDt.Rows.Count > 0)
                                {
                                    string Account = "";
                                    string RealName = "";
                                    foreach (DataRow item in dutyuserDt.Rows)
                                    {
                                        Account += item["account"].ToString() + ",";
                                        RealName += item["realname"].ToString() + ",";
                                    }
                                    if (!string.IsNullOrEmpty(Account))
                                    {
                                        Account = Account.Substring(0, Account.Length - 1);
                                        RealName = RealName.Substring(0, RealName.Length - 1);
                                    }
                                    JPushApi.PushMessage(Account, RealName, pushdata.SendCode, "您有一条新的高风险作业(" + CommonType + ")任务，请及时处理。", pushdata.Content, pushdata.EntityId);
                                }
                                //推送给作业监护人
                                if (tutelageuserEntity != null)
                                {
                                    JPushApi.PushMessage(tutelageuserEntity.Account, tutelageuserEntity.RealName, pushdata.SendCode, "您有一条新的高风险作业(" + CommonType + ")监护任务，请及时处理。", pushdata.Content, pushdata.EntityId);
                                }
                            }
                        }
                        else
                        {
                            //极光推送
                            PushMessageForCommon(pushdata);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveApplyForm(string keyValue, HighRiskCommonApplyEntity entity)
        {
            try
            {
                service.SaveApplyForm(keyValue, entity);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 确认，审核
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="state"></param>
        /// <param name="recordData"></param>
        /// <param name="entity"></param>
        /// <param name="aentity"></param>
        public void SubmitCheckForm(string keyValue, string state, string recordData, HighRiskCommonApplyEntity entity, ScaffoldauditrecordEntity aentity)
        {
            try
            {
                PushMessageData pushdata = service.SubmitCheckForm(keyValue, state, recordData, entity, aentity);
                if (pushdata != null)
                {
                    if (pushdata.Success == 1 && !string.IsNullOrEmpty(pushdata.SendCode))
                    {
                        pushdata.Content = getName(entity.WorkType, "CommonType");
                        if (pushdata.SendCode == "ZY003")
                        {
                            pushdata.Content = "您提交的" + pushdata.Content + "申请未通过，请及时处理。";
                            var high = GetEntity(entity.Id);
                            if (high != null)
                            {
                                UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                                if (userEntity != null)
                                {
                                    JPushApi.PushMessage(userEntity.Account, userEntity.RealName, pushdata.SendCode, "", pushdata.Content, pushdata.EntityId);
                                }
                            }
                        }
                        else if (pushdata.SendCode == "ZY018")
                        {
                            var high = GetEntity(entity.Id);
                            if (high != null)
                            {
                                String CommonType = pushdata.Content;
                                pushdata.Content = "作业内容：" + high.WorkContent + "&#10;作业时间：" + high.WorkStartTime.Value.ToString("yyyy年MM月dd日 HH时mm分") + " 到 " + high.WorkEndTime.Value.ToString("yyyy年MM月dd日 HH时mm分") + "&#10;作业地点：" + high.WorkPlace;
                                UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                                UserEntity tutelageuserEntity = userservice.GetEntity(high.WorkTutelageUserId);
                                string[] workuserlist = (high.WorkDutyUserId + "," + high.WorkUserIds).Split(',');
                                //List<string> b = workuserlist.ToList();
                                //b.Add(high.WorkDutyUserId);
                                //workuserlist = b.ToArray();
                                DataTable dutyuserDt = new DataTable();
                                dutyuserDt = userservice.GetUserTable(workuserlist);
                                //推送给作业申请人
                                if (userEntity != null)
                                {
                                    JPushApi.PushMessage(userEntity.Account, userEntity.RealName, pushdata.SendCode, "高风险作业(" + CommonType + "）申请已通过，请及时处理。", pushdata.Content, pushdata.EntityId);
                                }
                                //推送给作业负责人/作业人
                                if (dutyuserDt.Rows.Count > 0)
                                {
                                    string Account = "";
                                    string RealName = "";
                                    foreach (DataRow item in dutyuserDt.Rows)
                                    {
                                        Account += item["account"].ToString() + ",";
                                        RealName += item["realname"].ToString() + ",";
                                    }
                                    if (!string.IsNullOrEmpty(Account))
                                    {
                                        Account = Account.Substring(0, Account.Length - 1);
                                        RealName = RealName.Substring(0, RealName.Length - 1);
                                    }
                                    JPushApi.PushMessage(Account, RealName, pushdata.SendCode, "您有一条新的高风险作业(" + CommonType + ")任务，请及时处理。", pushdata.Content, pushdata.EntityId);
                                }
                                //推送给作业监护人
                                if (tutelageuserEntity != null)
                                {
                                    JPushApi.PushMessage(tutelageuserEntity.Account, tutelageuserEntity.RealName, pushdata.SendCode, "您有一条新的高风险作业(" + CommonType + ")监护任务，请及时处理。", pushdata.Content, pushdata.EntityId);
                                }
                            }
                        }
                        else
                        {
                            //极光推送
                            PushMessageForCommon(pushdata);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        public string getName(string type, string encode)
        {
            var cName = new DataItemDetailBLL().GetDataItemListByItemCode("'" + encode + "'").Where(a => a.ItemValue == type).FirstOrDefault().ItemName;
            return cName;
        }


        /// <summary>
        /// 极光推送
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="nextActivityName"></param>
        /// <param name="control"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public void PushMessageForCommon(PushMessageData pushdata)
        {
            if (!string.IsNullOrEmpty(pushdata.UserAccount))
            {
                DataTable dtuser = userservice.GetUserTable(pushdata.UserAccount.Split(','));
                string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                if (pushdata.SendCode == "ZY001")
                {
                    pushdata.Content = "您有一条" + pushdata.Content + "申请待安全措施确认，请及时处理。";
                }
                else if (pushdata.SendCode == "ZY002")
                {
                    pushdata.Content = "您有一条" + pushdata.Content + "申请待审批，请及时处理。";
                }
                else
                {
                    pushdata.Content = "";
                }
                JPushApi.PushMessage(pushdata.UserAccount, string.Join(",", usernames), pushdata.SendCode, "", pushdata.Content, pushdata.EntityId);
            }
            else
            {
                string flowdeptids = "'" + pushdata.UserDept.Replace(",", "','") + "'";
                string flowroleids = "'" + pushdata.UserRole.Replace(",", "','") + "'";
                IList<UserEntity> users = new UserService().GetUserListByDeptId(flowdeptids, flowroleids, true, string.Empty);
                if (users != null && users.Count > 0)
                {
                    string names = "";
                    string accounts = "";
                    if (!string.IsNullOrEmpty(pushdata.SpecialtyType) && !string.IsNullOrEmpty(pushdata.IsSpecial) && pushdata.IsSpecial == "1")
                    {
                        foreach (var item in users)
                        {
                            if (item.RoleName.Contains("专工"))
                            {
                                if (!string.IsNullOrEmpty(item.SpecialtyType) && item.SpecialtyType != "null")
                                {
                                    string[] str = item.SpecialtyType.Split(',');
                                    for (int i = 0; i < str.Length; i++)
                                    {
                                        if (str[i] == pushdata.SpecialtyType)
                                        {
                                            names += item.RealName + ",";
                                            accounts += item.Account + ",";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                names += item.RealName + ",";
                                accounts += item.Account + ",";
                            }
                        }
                        if (!string.IsNullOrEmpty(names))
                        {
                            names = names.TrimEnd(',');
                        }
                        if (!string.IsNullOrEmpty(accounts))
                        {
                            accounts = accounts.TrimEnd(',');
                        }
                    }
                    else
                    {
                        names = string.Join(",", users.Select(x => x.RealName).ToArray());
                        accounts = string.Join(",", users.Select(x => x.Account).ToArray());
                    }
                    if (pushdata.SendCode == "ZY001")
                    {
                        pushdata.Content = "您有一条" + pushdata.Content + "申请待安全措施确认，请及时处理。";
                    }
                    else if (pushdata.SendCode == "ZY002")
                    {
                        pushdata.Content = "您有一条" + pushdata.Content + "申请待审批，请及时处理。";
                    }
                    else
                    {
                        pushdata.Content = "";
                    }
                    JPushApi.PushMessage(accounts, names, pushdata.SendCode, "", pushdata.Content, pushdata.EntityId);
                }
            }

        }

        #region 统计
        /// <summary>
        /// 按作业类型统计
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetHighWorkCount(starttime, endtime, deptid, deptcode);
        }


        /// <summary>
        ///作业类型统计(统计表格)
        /// </summary>
        /// <returns></returns>
        public string GetHighWorkList(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetHighWorkList(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        /// 月度趋势(统计图)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkYearCount(string year, string deptid, string deptcode)
        {
            return service.GetHighWorkYearCount(year, deptid, deptcode);
        }

        /// <summary>
        /// 月度趋势(表格)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkYearList(string year, string deptid, string deptcode)
        {
            return service.GetHighWorkYearList(year, deptid, deptcode);
        }

        /// <summary>
        /// 单位对比(统计图)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public string GetHighWorkDepartCount(string starttime, string endtime)
        {
            return service.GetHighWorkDepartCount(starttime, endtime);
        }

        /// <summary>
        ///单位对比(统计表格)
        /// </summary>
        /// <returns></returns>
        public string GetHighWorkDepartList(string starttime, string endtime)
        {
            return service.GetHighWorkDepartList(starttime, endtime);
        }
        #endregion


        #region 获取今日高风险作业
        /// <summary>
        /// 获取今日高风险作业(作业台账中作业中的数据)
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTodayWorkList(Pagination pagination, string queryJson)
        {
            return service.GetTodayWorkList(pagination, queryJson);
        }
        #endregion

        #region 手机端高风险作业统计
        public DataTable AppGetHighWork(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.AppGetHighWork(starttime, endtime, deptid, deptcode);
        }
        #endregion
        #region
        public bool GetProjectNum(string outProjectId)
        {
            return service.GetProjectNum(outProjectId);
        }

        /// <summary>
        /// 根据区域编码获取高风险作业的数量
        /// </summary>
        /// <param name="areaCodes"></param>
        /// <returns></returns>
        public DataTable GetCountByArea(List<string> areaCodes)
        {
            return service.GetCountByArea(areaCodes);
        }
        #endregion
    }
}
