using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using System.Data;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：使用消防水
    /// </summary>
    public class FireWaterBLL
    {
        private FireWaterIService service = new FireWaterService();
        private PeopleReviewIService peopleReviwservice = new PeopleReviewService();
        private IUserService userservice = new UserService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetList(Pagination page, string queryJson, string authType, Operator user)
        {
            return service.GetList(page, queryJson, authType, user);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FireWaterEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public FireWaterCondition GetConditionEntity(string fireWaterId)
        {
            return service.GetConditionEntity(fireWaterId);
        }

        /// <summary>
        /// 获取执行情况集合
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public IList<FireWaterCondition> GetConditionList(string keyValue)
        {
            return service.GetConditionList(keyValue);
        }

        /// <summary>
        /// 得到流程图
        /// </summary>
        /// <param name="keyValue">业务表ID</param>
        /// <param name="modulename">逐级审核模块名</param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue, string modulename)
        {
            return service.GetFlow(keyValue, modulename);
        }

        /// <summary>
        /// 获取APP流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public List<CheckFlowData> GetAppFlowList(string keyValue, string modulename)
        {
            return service.GetAppFlowList(keyValue, modulename);
        }
        #endregion

        #region 台账列表
        /// <summary>
        /// 获取消防水使用台账
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetLedgerList(Pagination pagination, string queryJson, Operator user)
        {
            return service.GetLedgerList(pagination, queryJson,user);
        }
        #endregion
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
        public void SaveForm(string keyValue, FireWaterEntity entity)
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

        /// <summary>
        /// 保存表单（新增、修改）
        /// 适用于保存及修改时，处理业务数据及初始状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="model">实体对象</param>
        /// <param name="auditEntity">审核实体对象</param>
        /// <returns></returns> 
        public void SaveForm(string keyValue, FireWaterModel model)
        {
            try
            {
                Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                ManyPowerCheckEntity mpcEntity = null;
                if (model.WorkDeptType == "0")
                {
                    mpcEntity = peopleReviwservice.CheckAuditForNextByWorkUnit(currUser, "消防水使用-内部审核", model.WorkDeptId, model.FlowId, false);
                }
                else
                {
                    mpcEntity = peopleReviwservice.CheckAuditForNextByOutsourcing(currUser, "消防水使用-外部审核", model.WorkDeptId, model.FlowId, false, true, model.EngineeringId);
                }
                if (model.ApplyState == "0")
                {
                    model.FlowName = "申请中";
                    model.InvestigateState = "0";
                }
                if (model.ApplyState == "1")
                {
                    //如果审核步骤不为空，处理流程信息及状态
                    if (mpcEntity != null)
                    {
                        model.FlowDept = mpcEntity.CHECKDEPTID;
                        model.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        model.FlowRole = mpcEntity.CHECKROLEID;
                        model.FlowRoleName = mpcEntity.CHECKROLENAME;
                        model.FlowId = mpcEntity.ID;
                        model.FlowName = mpcEntity.FLOWNAME;
                        model.ApplyState = "1";
                        model.InvestigateState = "2";
                        model.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                        //推送消息到有审批权限的人
                        string type = model.FlowRemark != "1" ? "0" : "1";
                        new ScaffoldBLL().SendMessage(model.FlowDept, model.FlowRole, "ZY100", model.Id, "", "", type, !string.IsNullOrEmpty(model.SpecialtyType) ? model.SpecialtyType : "");

                    }
                    else
                    {
                        model.FlowRemark = "";
                        model.FlowDept = " ";
                        model.FlowDeptName = " ";
                        model.FlowRole = " ";
                        model.FlowRoleName = " ";
                        model.ApplyState = "3";
                        model.FlowName = "已完结";
                        model.InvestigateState = "3";

                        string Content = "作业内容：" + model.WorkContent + "&#10;作业时间：" + model.WorkStartTime.Value.ToString("yyyy年MM月dd日 HH时mm分") + " 到 " + model.WorkEndTime.Value.ToString("yyyy年MM月dd日 HH时mm分") + "&#10;作业地点：" + model.WorkPlace;
                        UserEntity userEntity = userservice.GetEntity(model.CreateUserId);
                        string[] workuserlist = model.WorkUserIds.Split(',');
                        DataTable dutyuserDt = new DataTable();
                        dutyuserDt = userservice.GetUserTable(workuserlist);
                        //推送给作业申请人
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY020", "消防水使用许可申请已通过，请及时处理", Content, keyValue);
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
                            JPushApi.PushMessage(Account, RealName, "ZY020", "您有一条新的消防水使用作业任务，请及时处理", Content, keyValue);
                        }
                    }
                }

                service.SaveForm(keyValue, model);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 申请审核
        /// 适用于审核时，修改业务状态信息
        /// </summary>
        /// <param name="key">申请信息主键ID</param>
        /// <param name="auditEntity">审核记录</param>
        public void ApplyCheck(string keyValue, ScaffoldauditrecordEntity auditEntity)
        {

            FireWaterEntity fireWaterEntity = service.GetEntity(keyValue);
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            ManyPowerCheckEntity mpcEntity = null;

            //把当前业务流程节点赋值到审核记录中
            auditEntity.FlowId = fireWaterEntity.FlowId;
            if (fireWaterEntity.WorkDeptType == "0")
            {
                mpcEntity = peopleReviwservice.CheckAuditForNextByWorkUnit(currUser, "消防水使用-内部审核", fireWaterEntity.WorkDeptId, fireWaterEntity.FlowId, false);
            }
            else
            {
                mpcEntity = peopleReviwservice.CheckAuditForNextByOutsourcing(currUser, "消防水使用-外部审核", fireWaterEntity.WorkDeptId, fireWaterEntity.FlowId, false, true, fireWaterEntity.EngineeringId);
            }
            //如果审核记录不为空，且为不同意，流程结束
            if (auditEntity.AuditState == 1)
            {
                //下一步流程不为空
                if (null != mpcEntity)
                {
                    fireWaterEntity.FlowDept = mpcEntity.CHECKDEPTID;
                    fireWaterEntity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                    fireWaterEntity.FlowRole = mpcEntity.CHECKROLEID;
                    fireWaterEntity.FlowRoleName = mpcEntity.CHECKROLENAME;
                    fireWaterEntity.FlowId = mpcEntity.ID;
                    fireWaterEntity.FlowName = mpcEntity.FLOWNAME;
                    fireWaterEntity.ApplyState = "1";
                    fireWaterEntity.InvestigateState = "2";
                    fireWaterEntity.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                    //推送消息到有审批权限的人
                    string type = fireWaterEntity.FlowRemark != "1" ? "0" : "1";
                    new ScaffoldBLL().SendMessage(fireWaterEntity.FlowDept, fireWaterEntity.FlowRole, "ZY100", fireWaterEntity.Id, "", "", type, !string.IsNullOrEmpty(fireWaterEntity.SpecialtyType) ? fireWaterEntity.SpecialtyType : "");
                }
                else
                {
                    fireWaterEntity.FlowRemark = "";
                    fireWaterEntity.FlowDept = " ";
                    fireWaterEntity.FlowDeptName = " ";
                    fireWaterEntity.FlowRole = " ";
                    fireWaterEntity.FlowRoleName = " ";
                    fireWaterEntity.ApplyState = "3";
                    fireWaterEntity.FlowName = "已完结";
                    fireWaterEntity.InvestigateState = "3";
                    fireWaterEntity.FlowId = "";


                    var high = GetEntity(fireWaterEntity.Id);
                    if (high != null)
                    {
                        string Content = "作业内容：" + high.WorkContent + "&#10;作业时间：" + high.WorkStartTime.Value.ToString("yyyy年MM月dd日 HH时mm分") + " 到 " + high.WorkEndTime.Value.ToString("yyyy年MM月dd日 HH时mm分") + "&#10;作业地点：" + high.WorkPlace;
                        UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                        string[] workuserlist = high.WorkUserIds.Split(',');
                        string[] copyuserlist = high.CopyUserIds.Split(',');
                        DataTable dutyuserDt = new DataTable();
                        dutyuserDt = userservice.GetUserTable(workuserlist);
                        //推送给作业申请人
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY020", "消防水使用许可申请已通过，请及时处理", Content, fireWaterEntity.Id);
                        }
                        DataTable copyuserdt = new DataTable();
                        copyuserdt = userservice.GetUserTable(copyuserlist);
                       
                       
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
                            JPushApi.PushMessage(Account, RealName, "ZY020", "您有一条新的消防水使用作业任务，请及时处理", Content, fireWaterEntity.Id);
                        }
                        //推送给抄送人
                        if (copyuserdt.Rows.Count > 0)
                        {
                            string Account = "";
                            string RealName = "";
                            foreach (DataRow item in copyuserdt.Rows)
                            {
                                Account += item["account"].ToString() + ",";
                                RealName += item["realname"].ToString() + ",";
                            }
                            if (!string.IsNullOrEmpty(Account))
                            {
                                Account = Account.Substring(0, Account.Length - 1);
                                RealName = RealName.Substring(0, RealName.Length - 1);
                            }
                            JPushApi.PushMessage(Account, RealName, "ZY020", "消防水使用许可申请审核已经通过，请知晓", Content, fireWaterEntity.Id);
                        }
                    }
                }
            }
            else
            {
                fireWaterEntity.FlowRemark = "";
                fireWaterEntity.FlowDept = " ";
                fireWaterEntity.FlowDeptName = " ";
                fireWaterEntity.FlowRole = " ";
                fireWaterEntity.FlowRoleName = " ";
                fireWaterEntity.ApplyState = "2";
                fireWaterEntity.FlowName = "已完结";
                fireWaterEntity.InvestigateState = "3";
                fireWaterEntity.FlowId = "";
                //审批不通过,推消息到申请人
                UserEntity userEntity = new UserService().GetEntity(fireWaterEntity.CreateUserId);
                if (userEntity != null)
                {
                    var high = GetEntity(fireWaterEntity.Id);
                    if (high != null)
                    {
                        string Content = "作业内容：" + high.WorkContent + "&#10;作业时间：" + high.WorkStartTime.Value.ToString("yyyy年MM月dd日 HH时mm分") + " 到 " + high.WorkEndTime.Value.ToString("yyyy年MM月dd日 HH时mm分") + "&#10;作业地点：" + high.WorkPlace;
                        JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY019", "消防水使用许可申请未通过，请及时处理", Content, fireWaterEntity.Id);
                    }
                }
            }
            this.service.UpdateForm(fireWaterEntity, auditEntity);

        }


        /// <summary>
        /// 提交执行情况
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public void SubmitCondition(string keyValue, FireWaterCondition entity) {
            this.service.SubmitCondition(keyValue, entity);
        }
    }
}
