using BSFramework.Data;
using BSFramework.Data.Repository;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;

namespace ERCHTMS.Service.HiddenTroubleManage
{

    //<summary>
    //隐患 流程测试
    //</summary>
    public class HTWorkFlowService : RepositoryFactory, HTWorkFlowIService
    {

        private IWfTBInstanceService iwftbinstanceservice = new WfTBInstanceService();
        private IWfTBInstanceDetailService iwftbinstancedetailservice = new WfTBInstanceDetailService();
        private IWfTBProcessService iwftbprocessservice = new WfTBProcessService();
        private IWfTBActivityService iwftbactivityservice = new WfTBActivityService();
        private IWfTBConditionService iwftbconditionservice = new WfTBConditionService();
        private IUserService userservice = new UserService();
        #region 判断当前流程是否存在
        /// <summary>
        /// 判断当前流程是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool IsHaveCurWorkFlow(string mark)
        {
            try
            {
                //当前用户
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

                int count = int.Parse(this.BaseRepository().FindObject(string.Format(@"select count(1) from bis_wfinstance t where t.mark = '{0}' and t.organizeid ='{1}' and ISENABLE='是'", mark, user.OrganizeId)).ToString());

                return count > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 删除工作流程实例
        /// <summary>
        /// 删除工作流程实例
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public bool DeleteWorkFlowObj(string objectID)
        {
            bool r = true;

            try
            {
                var resp = this.BaseRepository();

                string sql = string.Format("delete from sys_wftbinstancedetail where instanceid in (select id from sys_wftbinstance where objectid='{0}')", objectID);

                resp.ExecuteBySql(sql);

                sql = string.Format("delete from sys_wftbinstance where objectid='{0}'", objectID);

                resp.ExecuteBySql(sql);
            }
            catch
            {
                r = false;
            }

            return r;
        }
        #endregion

        #region  创建流程实例
        /// <summary>
        /// 创建流程实例
        /// </summary>
        /// <param name="wfObj">隐患流程编码</param>
        /// <param name="objectID">实例ID</param>
        /// <param name="curUser">当前用户ID</param>
        /// <returns></returns>
        public bool CreateWorkFlowObj(string wfObj, string objectID, string curUser, string submittype = "")
        {
            bool isSuccess = false;
            //流程ID
            string processID = string.Empty;

            //开始节点
            string activtyID = string.Empty;

            try
            {
                //通过编码获取流程对象
                DataTable dt = this.BaseRepository().FindTable(string.Format(@"select t.*, t.rowid from sys_wftbprocess t where t.code = '{0}'", wfObj));
                //读取流程ID
                if (dt.Rows.Count == 1)
                {
                    processID = dt.Rows[0]["ID"].ToString(); //获取ID
                }

                //获取当前人部门
                UserInfoEntity uInfor = new UserInfoService().GetUserInfoEntity(curUser);

                //当前流程节点  ，初始化为隐患登记
                string curWfNode = Guid.NewGuid().ToString();

                //当前流程对象ID
                string InstanceID = Guid.NewGuid().ToString();

                //创建一个Instance 下的开始节点
                DataTable reader = this.BaseRepository().FindTable(string.Format(@"select * from sys_wftbactivity where  processid ='{0}' and kind ='开始节点'", processID));

                if (reader.Rows.Count == 1)
                {
                    activtyID = reader.Rows[0]["ID"].ToString();    //开始节点ID
                }

                string instanceSql = string.Format(@"insert into sys_wftbinstance (ID, OPERDATE, OPERUSER, CREATEUSER, CREATEDATE, INSTANCECONTENT, 
                                                    OBJECTID, STATE, PRIORITY, CURRENTDETAILID, PARENTID, PROCESSID, CREATEUSERID, CREATEUSERDEPTID, CREATEUSERDEPTCODE,
                                                    OPERATEDUSER, DATAFROM, UPLOADDATE, COMMONDATE)
                                                    values ('{0}', to_date('{1}','yyyy-mm-dd hh24:mi:ss'), '{2}', '{3}', to_date('{4}','yyyy-mm-dd hh24:mi:ss'), '{5}', '{6}', {7}, {8}, '{9}', 
                                                    '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', to_date('{17}','yyyy-mm-dd hh24:mi:ss'), to_date('{18}','yyyy-mm-dd hh24:mi:ss'))",
                                                        InstanceID, DateTime.Now.ToString(), uInfor.Account, uInfor.Account, DateTime.Now.ToString(), "", objectID,
                                                        1, 0, curWfNode, "", processID, uInfor.UserId, uInfor.DepartmentId, uInfor.DepartmentCode, "|" + uInfor.Account + "|", "",
                                                        DateTime.Now.ToString(), DateTime.Now.ToString());

                //创建一个Instance
                int reValue = this.BaseRepository().ExecuteBySql(instanceSql);

                string instanceDetails = @"insert into sys_wftbinstancedetail (ID, OPERDATE, OPERUSER, CREATEUSER, CREATEDATE, STATE, PARTICIPANT, REMARK, 
                                        FROMACTIVITYID, TOACTIVITYID, INSTANCEID, CREATEUSERID, CREATEUSERDEPTID, CREATEUSERDEPTCODE, TEMPUSERS,PARTICIPANTNAME)values('" + curWfNode + "', to_date('" +
                        DateTime.Now.ToString() + "','yyyy-mm-dd hh24:mi:ss'), '" + uInfor.Account + "', '" + uInfor.Account + "', to_date('" + DateTime.Now.ToString() + "','yyyy-mm-dd hh24:mi:ss'), 0, '$" +
                        uInfor.Account + "', '', '', '" + activtyID + "', '" + InstanceID + "', '" + uInfor.UserId + "', '" + uInfor.DepartmentId + "', '" + uInfor.DepartmentCode + "', '','" + uInfor.RealName + "')";


                Thread.Sleep(1000);

                //流转记录，当前为隐患登记状态
                int deReValue = this.BaseRepository().ExecuteBySql(instanceDetails);

                if (reValue > 0 && deReValue > 0)
                {
                    isSuccess = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isSuccess;
        }
        #endregion



        #region  创建流程实例
        /// <summary>
        /// 创建流程实例
        /// </summary>
        /// <param name="wfObj">隐患流程编码</param>
        /// <param name="objectID">实例ID</param>
        /// <param name="curUser">当前用户ID</param>
        /// <returns></returns>
        public bool CreateWorkFlowObj(string wfObj, string objectID, string curUser, string businessid, string businesstype)
        {
            bool isSuccess = false;
            //流程ID
            string processID = string.Empty;

            //开始节点
            string activtyID = string.Empty;

            try
            {
                //通过编码获取流程对象
                DataTable dt = this.BaseRepository().FindTable(string.Format(@"select t.*, t.rowid from sys_wftbprocess t where t.code = '{0}'", wfObj));
                //读取流程ID
                if (dt.Rows.Count == 1)
                {
                    processID = dt.Rows[0]["ID"].ToString(); //获取ID
                }

                //获取当前人部门
                UserInfoEntity uInfor = new UserInfoService().GetUserInfoEntity(curUser);

                //当前流程对象ID
                string InstanceID = Guid.NewGuid().ToString();

                //创建一个Instance 下的开始节点
                DataTable reader = this.BaseRepository().FindTable(string.Format(@"select * from sys_wftbactivity where  processid ='{0}' and kind ='开始节点'", processID));

                if (reader.Rows.Count == 1)
                {
                    activtyID = reader.Rows[0]["ID"].ToString();    //开始节点ID
                }
                //创建流程业务实例
                WfTBInstanceEntity entity = new WfTBInstanceEntity();
                entity.OBJECTID = objectID;
                entity.STATE = 1;
                entity.PRIORITY = 0;
                entity.PROCESSID = processID;
                entity.OPERATEDUSER = "|" + uInfor.Account + "|";
                entity.UPLOADDATE = DateTime.Now;
                entity.COMMONDATE = DateTime.Now;
                iwftbinstanceservice.SaveForm("", entity); //新增实例

                //等待一秒
                Thread.Sleep(1000);

                //添加流程业务流转明细
                WfTBInstanceDetailEntity dentity = new WfTBInstanceDetailEntity();
                dentity.STATE = 0;
                dentity.PARTICIPANT = "$" + uInfor.Account;
                dentity.FROMACTIVITYID = null;
                dentity.TOACTIVITYID = activtyID;
                dentity.INSTANCEID = entity.ID;
                dentity.PARTICIPANTNAME = uInfor.RealName;
                dentity.BUSINESSID = businessid;
                dentity.BUSINESSTYPE = businesstype;
                iwftbinstancedetailservice.SaveForm("", dentity);

                //更新实例
                entity.CURRENTDETAILID = dentity.ID; //流转明细ID
                iwftbinstanceservice.SaveForm(entity.ID, entity); //新增实例

                isSuccess = true;
            }
            catch (Exception)
            {
                throw;
            }
            return isSuccess;
        }
        #endregion

        #region 流程提交 / 退回
        /// <summary>
        /// 流程提交  
        /// </summary>
        /// <param name="objectId">实例ID</param>
        /// <param name="participant">参与者</param>
        /// <param name="wfFlag">流程转向</param>
        /// <param name="curUser">当前用户ID</param>
        /// <returns></returns>
        public PushData SubmitWorkFlow(string objectId, string participant, string wfFlag, string curUser,string submittype ="")
        {
            int isSuccess = -1;  //返回值  为0的时候表示当前用户无提交操作权限 ,为1的时候表示操作成功,为-2则数据操作失败
            PushData result = new PushData();
            try
            {
                //获取当前人部门
                UserInfoEntity uInfor = new UserInfoService().GetUserInfoEntity(curUser);

                //引用对象
                string instanceSql = string.Format(@"select  b.name, a.*, a.rowid from sys_wftbinstance a  
                                                     left join sys_wftbprocess b on a.processid = b.id  where a.objectid ='{0}'", objectId);

                DataTable idr = this.BaseRepository().FindTable(instanceSql);

                string processId = string.Empty;  //流程处理ID 

                string currentDetailId = string.Empty;  //当前明细

                string instanceID = string.Empty; //InstanceID 

                string curActivity = string.Empty; //当前Activity 

                string nextActivity = string.Empty; //下一个Activity 

                string nextActivityName = string.Empty; //下一个结点名称

                string curParticipant = string.Empty; //当前节点权限下的提交人员

                string operatedUser = string.Empty; //操作人

                string participantname = string.Empty;

                wfFlag = "@{wfFlag}==" + wfFlag;

                if (idr.Rows.Count == 1)
                {
                    result.ProcessName = idr.Rows[0]["NAME"].ToString();  //当前实例流程名称

                    instanceID = idr.Rows[0]["ID"].ToString();  // InstanceID  

                    processId = idr.Rows[0]["PROCESSID"].ToString();  //处理ID

                    currentDetailId = idr.Rows[0]["CURRENTDETAILID"].ToString(); //当前流程节点

                    operatedUser = idr.Rows[0]["OPERATEDUSER"].ToString();  //操作人及其历史操作人
                }

                //根据InstanceId获取详细的流程节点信息 ,取最新的一条，来确定当前的流程状态，同时通过当前的流程状态加推送条件找到下一条流程
                string idetailSql = string.Format(@"select t.*, t.rowid from sys_wftbinstancedetail t where t.instanceid ='{0}' order by  t.autoid desc,t.createdate desc", instanceID);

                DataTable detailIdr = this.BaseRepository().FindTable(idetailSql);
                //读取第一条
                if (detailIdr.Rows.Count > 0)
                {
                    curParticipant = detailIdr.Rows[0]["PARTICIPANT"].ToString(); //当前流程节点下能够提交的人员
                    curActivity = detailIdr.Rows[0]["TOACTIVITYID"].ToString();    //当前Activity
                }

                //推送到下一个流程
                string nextActivitySql = string.Format(@"select b.name toname, t.*, t.rowid from sys_wftbcondition t
                                                         left join sys_wftbactivity b on t.toactivityid = b.id where activityid ='{0}' and expression ='{1}'", curActivity, wfFlag);

                DataTable nextIdr = this.BaseRepository().FindTable(nextActivitySql);
                //读取第一条
                if (nextIdr.Rows.Count == 1)
                {
                    nextActivity = nextIdr.Rows[0]["TOACTIVITYID"].ToString();    //下一个节点的Activity
                    result.NextActivityName = nextIdr.Rows[0]["TONAME"].ToString();  //下一个结点名称
                }

                //参与者
                if (!string.IsNullOrEmpty(participant))
                {
                    string tempparticipant = "'" + participant.Replace(",", "','") + "'";
                    DataTable userdt = this.BaseRepository().FindTable(string.Format(@" select wm_concat(realname)  realname from base_user where account in ({0})", tempparticipant));
                    if (userdt.Rows.Count == 1)
                    {
                        participantname = userdt.Rows[0]["realname"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(curParticipant))
                {
                    string[] curpStr = curParticipant.Replace("$", "").Split(','); //当前节点下可以操作的人员
                    //当前节点下允许当前用户提交
                    if (curpStr.Contains(uInfor.Account))
                    {
                        string detailsID = Guid.NewGuid().ToString();
                        #region  具体操作
                        //新增一条节点信息
                        string insertDetailSql = string.Format(@"insert into sys_wftbinstancedetail (ID, OPERDATE, OPERUSER, CREATEUSER, CREATEDATE,
                                                             STATE, PARTICIPANT, REMARK, FROMACTIVITYID, TOACTIVITYID, INSTANCEID, CREATEUSERID,
                                                            CREATEUSERDEPTID, CREATEUSERDEPTCODE, TEMPUSERS,PARTICIPANTNAME)
                                                            values('{0}', to_date('{1}','yyyy-mm-dd hh24:mi:ss'), '{2}', '{3}', to_date('{4}','yyyy-mm-dd hh24:mi:ss'), {5}, '${6}', '{7}', '{8}', '{9}', '{10}', 
                                                            '{11}', '{12}','{13}', '{14}','{15}')", detailsID, DateTime.Now.ToString(), uInfor.Account, uInfor.Account,
                                                        DateTime.Now.ToString(), 0, participant, submittype, curActivity, nextActivity, instanceID, uInfor.UserId, uInfor.DepartmentId, uInfor.DepartmentCode, "", participantname);




                        string newOperatedUser = operatedUser + uInfor.Account + "|";

                        string updateInstanceSql = string.Format(@"update sys_wftbinstance set operdate =to_date('{0}','yyyy-mm-dd hh24:mi:ss'),operuser='{1}',currentdetailid ='{2}',
                                                       operateduser='{3}' where id='{4}'", DateTime.Now.ToString(), uInfor.Account, detailsID, newOperatedUser, instanceID);



                        int tempVal = this.BaseRepository().ExecuteBySql(insertDetailSql);

                        int tempValues = this.BaseRepository().ExecuteBySql(updateInstanceSql);

                        if (tempVal > 0 && tempValues > 0)
                        {
                            isSuccess = 1;  //操作成功
                        }
                        #endregion
                    }
                    else  //反之，则不允许提交 
                    {
                        isSuccess = 0;
                    }
                }

            }
            catch (Exception)
            {
                isSuccess = -2;  //操作失败，出现错误
                throw;
            }

            result.IsSucess = isSuccess;

            return result;
        }
        #endregion



        #region 流程提交 / 退回
        /// <summary>
        /// 流程提交  
        /// </summary>
        /// <param name="objectId">实例ID</param>
        /// <param name="participant">参与者</param>
        /// <param name="wfFlag">流程转向</param>
        /// <param name="curUser">当前用户ID</param>
        /// <returns></returns>
        public PushData SubmitWorkFlow(string objectId, string participant, string wfFlag, string curUser, string busineesId, string businessType, string submittype = "")
        {
            int isSuccess = -1;  //返回值  为0的时候表示当前用户无提交操作权限 ,为1的时候表示操作成功,为-2则数据操作失败
            PushData result = new PushData();
            try
            {
                //获取当前人部门
                UserInfoEntity uInfor = new UserInfoService().GetUserInfoEntity(curUser);



                //引用对象
                string instanceSql = string.Format(@"select  b.name, a.*, a.rowid from sys_wftbinstance a  
                                                     left join sys_wftbprocess b on a.processid = b.id  where a.objectid ='{0}'", objectId);

                DataTable idr = this.BaseRepository().FindTable(instanceSql);

                string processId = string.Empty;  //流程处理ID 

                string currentDetailId = string.Empty;  //当前明细

                string instanceID = string.Empty; //InstanceID 

                string curActivity = string.Empty; //当前Activity 

                string nextActivity = string.Empty; //下一个Activity 

                string nextActivityName = string.Empty; //下一个结点名称

                string curParticipant = string.Empty; //当前节点权限下的提交人员

                string operatedUser = string.Empty; //操作人

                string participantname = string.Empty;

                string prevparticipants = string.Empty; //上一结点的参与者

                string prevparticipantnames = string.Empty; //上一结点的参与人

                wfFlag = "@{wfFlag}==" + wfFlag;

                if (idr.Rows.Count == 1)
                {
                    result.ProcessName = idr.Rows[0]["NAME"].ToString();  //当前实例流程名称

                    instanceID = idr.Rows[0]["ID"].ToString();  // InstanceID  

                    processId = idr.Rows[0]["PROCESSID"].ToString();  //处理ID

                    currentDetailId = idr.Rows[0]["CURRENTDETAILID"].ToString(); //当前流程节点

                    operatedUser = idr.Rows[0]["OPERATEDUSER"].ToString();  //操作人及其历史操作人
                }

                WfTBInstanceEntity entity = iwftbinstanceservice.GetEntity(instanceID); //获取历史流程实例记录

                //根据InstanceId获取详细的流程节点信息 ,取最新的一条，来确定当前的流程状态，同时通过当前的流程状态加推送条件找到下一条流程
                string idetailSql = string.Format(@"select t.*, t.rowid from sys_wftbinstancedetail t where t.instanceid ='{0}' order by  t.createdate desc", instanceID);

                DataTable detailIdr = this.BaseRepository().FindTable(idetailSql);
                //读取第一条
                if (detailIdr.Rows.Count > 0)
                {
                    curParticipant = detailIdr.Rows[0]["PARTICIPANT"].ToString(); //当前流程节点下能够提交的人员
                    curActivity = detailIdr.Rows[0]["TOACTIVITYID"].ToString();    //当前Activity
                    prevparticipants = detailIdr.Rows[0]["PARTICIPANT"].ToString(); //上一条参与者记录
                    prevparticipantnames = detailIdr.Rows[0]["PARTICIPANTNAME"].ToString(); //上一条参与者记录
                }

                //推送到下一个流程
                string nextActivitySql = string.Format(@"select b.name toname, t.*, t.rowid from sys_wftbcondition t
                                                         left join sys_wftbactivity b on t.toactivityid = b.id where activityid ='{0}' and expression ='{1}'", curActivity, wfFlag);

                DataTable nextIdr = this.BaseRepository().FindTable(nextActivitySql);
                //读取第一条
                if (nextIdr.Rows.Count == 1)
                {
                    nextActivity = nextIdr.Rows[0]["TOACTIVITYID"].ToString();    //下一个节点的Activity
                    result.NextActivityName = nextIdr.Rows[0]["TONAME"].ToString();  //下一个结点名称
                }

                //参与者
                if (!string.IsNullOrEmpty(participant))
                {
                    string tempparticipant = "'" + participant.Replace(",", "','") + "'";
                    DataTable userdt = this.BaseRepository().FindTable(string.Format(@" select wm_concat(realname)  realname from base_user where account in ({0})", tempparticipant));
                    if (userdt.Rows.Count == 1)
                    {
                        participantname = userdt.Rows[0]["realname"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(curParticipant))
                {
                    string[] curpStr = curParticipant.Replace("$", "").Split(','); //当前节点下可以操作的人员
                    //当前节点下允许当前用户提交
                    if (curpStr.Contains(uInfor.Account))
                    {
                        WfTBInstanceDetailEntity dentity = new WfTBInstanceDetailEntity();
                        dentity.STATE = 0;
                        dentity.PARTICIPANT = participant;
                        dentity.FROMACTIVITYID = curActivity;
                        dentity.TOACTIVITYID = nextActivity;
                        dentity.INSTANCEID = instanceID;
                        dentity.REMARK = submittype;
                        dentity.TEMPUSERS = "";
                        dentity.PARTICIPANTNAME = participantname;
                        dentity.BUSINESSID = busineesId;
                        dentity.BUSINESSTYPE = businessType;
                        dentity.PREVPARTICIPANTS = prevparticipants;
                        dentity.PREVPARTICIPANTNAMES = prevparticipantnames;

                        iwftbinstancedetailservice.SaveForm("", dentity);

                        //更新流程业务实例
                        entity.CURRENTDETAILID = dentity.ID;
                        entity.OPERATEDUSER = operatedUser + uInfor.Account + "|";
                        iwftbinstanceservice.SaveForm(instanceID, entity);

                        isSuccess = 1;  //操作成功
                    }
                    else  //反之，则不允许提交 
                    {
                        isSuccess = 0;
                    }
                }

            }
            catch (Exception)
            {
                isSuccess = -2;  //操作失败，出现错误
                throw;
            }

            result.IsSucess = isSuccess;

            return result;
        }
        #endregion

        #region 流程提交，只提交实例不更改当前流程状态,(例如,当前是隐患评估，提交后可能还是隐患评估，并且评估人员发生变化)
        /// <summary>
        /// 流程提交，只更改实例不更改当前流程状态
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="participant"></param>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public PushData SubmitWorkFlowNoChangeStatus(string objectId, string participant, string curUser, string submittype = "")
        {

            PushData result = new PushData();

            //获取当前人部门
            UserInfoEntity uInfor = new UserInfoService().GetUserInfoEntity(curUser);

            bool isSuccess = false;

            string instanceID = string.Empty;

            string curentDetailID = string.Empty;

            string operateduser = string.Empty;

            string newDetailId = Guid.NewGuid().ToString();  //新的Guid

            int reVal = 0;

            string sql = string.Format(@"select a.id, a.objectid, a.currentdetailid ,a.operateduser,b.name from sys_wftbinstance  a  
                                        left join sys_wftbprocess b on a.processid = b.id   where a.objectid='{0}'", objectId);

            DataTable idt = this.BaseRepository().FindTable(sql);

            if (idt.Rows.Count == 1)
            {
                instanceID = idt.Rows[0]["id"].ToString();  //
                curentDetailID = idt.Rows[0]["currentdetailid"].ToString(); //获取当前流程节点实例ID
                operateduser = idt.Rows[0]["operateduser"].ToString(); //操作过的人员
                result.ProcessName = idt.Rows[0]["name"].ToString(); //流程实例
            }

            string participantname = string.Empty;

            //参与者
            if (!string.IsNullOrEmpty(participant))
            {
                string tempparticipant = "'" + participant.Replace(",", "','") + "'";
                DataTable userdt = this.BaseRepository().FindTable(string.Format(@" select wm_concat(realname)  realname from base_user where account in ({0})", tempparticipant));
                if (userdt.Rows.Count == 1)
                {
                    participantname = userdt.Rows[0]["realname"].ToString();
                }
            }

            string newDetailID = Guid.NewGuid().ToString();
            string detailsql = string.Format(@"insert into sys_wftbinstancedetail (id, operdate, operuser, createuser, createdate,
                                                             state, participant, remark, fromactivityid, toactivityid, instanceid, createuserid,
                                                            createuserdeptid, createuserdeptcode, tempusers,participantname)
                                                            select '{0}' id, to_date('{1}','yyyy-mm-dd hh24:mi:ss') operdate, '{2}' operuser, '{3}' createuser,to_date('{4}','yyyy-mm-dd hh24:mi:ss') createdate, 
                                                           {5} state, '${6}' participant, '{7}' remark,  toactivityid,  toactivityid, instanceid, 
                                                            '{8}' createuserid, '{9}' createuserdeptid,'{10}' createuserdeptcode, '{11}' tempusers,'{12}' participantname from sys_wftbinstancedetail where id ='{13}'", newDetailID, DateTime.Now.ToString(), uInfor.Account, uInfor.Account,
                            DateTime.Now.ToString(), 0, participant, submittype, uInfor.UserId, uInfor.DepartmentId, uInfor.DepartmentCode, "", participantname, curentDetailID);

            reVal = this.BaseRepository().ExecuteBySql(detailsql);

            if (reVal > 0)
            {

                operateduser = operateduser + uInfor.Account + "|";

                string instancesql = string.Format(@"update sys_wftbinstance set operdate =to_date('{0}','yyyy-mm-dd hh24:mi:ss'),operuser='{1}',currentdetailid ='{2}', 
                                                       operateduser='{3}' where id='{4}'", DateTime.Now.ToString(), uInfor.Account, newDetailID, operateduser, instanceID);

                reVal = this.BaseRepository().ExecuteBySql(instancesql);

                isSuccess = reVal > 0 ? true : false;
            }

            result.IsSucess = reVal;

            return result;
        }
        #endregion

        #region 获取当前的流程节点名称
        /// <summary>
        /// 获取当前的流程节点
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public string QueryTagNameByCurrentWF(string objectID)
        {
            string returnValue = string.Empty; //返回值

            string tagName = string.Empty;  //当前流程 

            string instanceSql = string.Format(@" select b.toactivityid,c.tag,c.name from sys_wftbinstance a
                                                            left join sys_wftbinstancedetail b on a.currentdetailid = b.id
                                                            left join  sys_wftbactivity c on b.toactivityid = c.id
                                                            where a.objectid ='{0}'", objectID);

            DataTable idr = this.BaseRepository().FindTable(instanceSql);

            if (idr.Rows.Count == 1)
            {
                tagName = idr.Rows[0]["NAME"].ToString(); //当前流程节点
            }
            return tagName;
        }
        #endregion

        #region 获取当前的流程节点标记
        /// <summary>
        /// 获取当前的流程节点标记
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public string QueryTagByCurrentWF(string objectID)
        {
            string returnValue = string.Empty; //返回值

            string tag = string.Empty;  //当前流程 

            string instanceSql = string.Format(@" select b.toactivityid,c.tag,c.name from sys_wftbinstance a
                                                            left join sys_wftbinstancedetail b on a.currentdetailid = b.id
                                                            left join  sys_wftbactivity c on b.toactivityid = c.id
                                                            where a.objectid ='{0}'", objectID);

            DataTable idr = this.BaseRepository().FindTable(instanceSql);

            if (idr.Rows.Count == 1)
            {
                tag = idr.Rows[0]["NAME"].ToString(); //当前流程节点
            }
            return tag;
        }
        #endregion

        #region  通过当前实例ID，查询数据库，判断当前流程是否存在
        /// <summary>
        /// 通过当前实例ID，查询数据库，判断当前流程是否存在
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public bool IsHavaWFCurrentObject(string objectID)
        {
            bool isTrue = false;

            try
            {
                string sql = string.Format(@"select count(t.id) from sys_wftbinstance t where objectid = '{0}'", objectID);

                int count = Convert.ToInt32(this.BaseRepository().FindTable(sql).Rows[0][0].ToString());

                if (count > 0)
                {
                    isTrue = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return isTrue;
        }
        #endregion

        #region 更新业务流程状态
        /// <summary>
        /// 更新业务流程状态
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public bool UpdateWorkStreamByObjectId(string tableName, string fieldName, string objectId)
        {
            bool isValue = false;

            string tagName = QueryTagNameByCurrentWF(objectId);
            string sql = string.Format("update {0} set {1} ='{2}' where id ='{3}'", tableName, fieldName, tagName, objectId);

            int count = this.BaseRepository().ExecuteBySql(sql);
            if (count > 0)
            {
                isValue = true;
            }
            return isValue;
        }
        #endregion

        #region 获取隐患的流程图对象
        /// <summary>
        /// 获取隐患的流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetActionList(string keyValue)
        {
            //当前用户
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            //开封电厂
            string KFDC_ORGCODE = new DataItemDetailService().GetItemValue("KFDC_ORGCODE");

            try
            {
                List<nodes> nlist = new List<nodes>();
                List<lines> llist = new List<lines>();
                string sql = string.Empty;
                string sqlline = string.Empty;
                string sqlnode = string.Empty;

                #region  获取隐患基本数据
                //获取隐患基本数据
                sql = string.Format(@"select a.addtype,a.workstream,a.hidrank,b.itemname,a.isbreakrule,a.id,a.hidcode,a.hiddepart from BIS_HTBASEINFO a
                                        left join base_dataitemdetail b on a.hidrank = b.itemdetailid where a.id ='{0}'", keyValue);

                var hidDt = this.BaseRepository().FindTable(sql);



                string workstream = string.Empty;
                string isbreakrule = string.Empty;
                string addtype = string.Empty;
                string hidrank = string.Empty;
                string orgid = string.Empty;

                if (hidDt.Rows.Count == 1)
                {
                    workstream = hidDt.Rows[0]["workstream"].ToString();
                    isbreakrule = hidDt.Rows[0]["isbreakrule"].ToString();
                    addtype = hidDt.Rows[0]["addtype"].ToString();
                    hidrank = !string.IsNullOrEmpty(hidDt.Rows[0]["itemname"].ToString()) ? hidDt.Rows[0]["itemname"].ToString() : "无";
                    orgid =  hidDt.Rows[0]["hiddepart"].ToString(); 
                }
                #endregion

                Flow flow = new Flow();
                flow.title = "隐患流程图";
                flow.initNum = 22;

                #region 创建nodes对象

                var majoritemlist = new DataItemDetailService().GetDataItemListByItemCode("'HIdWorkFlowNodeView'");// 隐患流程节点显示配置

                if (majoritemlist.Where(p => p.ItemName.Trim() == orgid && p.ItemCode == hidrank).Count() > 0)
                {
                    //立即整改隐患
                    if (addtype == "1")
                    {
                        sqlnode = @"select id,name,autoid,kind from sys_wftbactivity where autoid in ('10000','10008') and processid ='2142740610' order by autoid ";  //nodes;

                        sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                            left join sys_wftbactivity b on b.id = a.activityid
                            left join sys_wftbactivity c on c.id = a.toactivityid where remark='1'";
                    }
                    else 
                    {
                        string itemValue = majoritemlist.Where(p => p.ItemName.Trim() == orgid && p.ItemCode == hidrank).FirstOrDefault().ItemValue;
                        //通用电厂
                        sqlnode = string.Format(@"select id,name,autoid,kind from sys_wftbactivity  where  autoid in ({0}) and processid ='2142740610' order by autoid ", itemValue);  //nodes;

                        sqlline = string.Format(@"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                            left join sys_wftbactivity b on b.id = a.activityid
                            left join sys_wftbactivity c on c.id = a.toactivityid where  b.autoid in ({0}) and c.autoid in ({0}) ", itemValue);
                    }
                }
                else 
                {
                    #region MyRegion
                    //立即整改隐患
                    if (addtype == "1")
                    {
                        sqlnode = @"select id,name,autoid,kind from sys_wftbactivity where autoid in ('10000','10008') and processid ='2142740610' order by autoid ";  //nodes;

                        sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                            left join sys_wftbactivity b on b.id = a.activityid
                            left join sys_wftbactivity c on c.id = a.toactivityid where remark='1'";
                    }
                    //一般隐患
                    else if (addtype != "1" && hidrank.Contains("一般隐患"))
                    {
                        //省级版本 减去制定整改计划、整改效果评估
                        if (addtype == "2")
                        {
                            sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where autoid != 10007  and autoid != 10001  and processid ='2142740610' and autoid <= 10008 order by autoid ";  //nodes;

                            //流程转向
                            sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                            left join sys_wftbactivity b on b.id = a.activityid
                            left join sys_wftbactivity c on c.id = a.toactivityid where remark='0' and b.autoid!='10001' and c.autoid!='10001'";
                        }
                        else if (addtype == "3")  //国电新疆版本 减去完善、复查验证、整改效果评估
                        {
                            sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where autoid != 10007  and  autoid != 10002  and  autoid != 10006 and processid ='2142740610' and autoid <= 10008 order by autoid ";  //nodes;

                            //流程转向
                            sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                            left join sys_wftbactivity b on b.id = a.activityid
                            left join sys_wftbactivity c on c.id = a.toactivityid where remark='0' and b.autoid not in ('10002','10006') and c.autoid not in ('10002','10006')";
                        }
                        else    //非省级、减去完善、复查验证、整改效果评估、制定整改计划
                        {
                            //当前机构为开封电厂的
                            if (curUser.OrganizeCode == KFDC_ORGCODE)
                            {
                                sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where autoid != 10007 and  autoid != 10002  and processid ='2142740610' and autoid <= 10008 order by autoid ";  //nodes;

                                //流程转向
                                sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                                    left join sys_wftbactivity b on b.id = a.activityid
                                    left join sys_wftbactivity c on c.id = a.toactivityid where remark='0' and b.autoid not in ('10002') and c.autoid not in ('10002') and a.id !='a429a30e-1ee9-4f3d-8ded-2bfd294260b0'";
                            }
                            else     //通用电厂
                            {
                                sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where autoid != 10007 and  autoid != 10001 and  autoid != 10002  and  autoid != 10006  and processid ='2142740610' and autoid <= 10008 order by autoid ";  //nodes;

                                //流程转向
                                sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                            left join sys_wftbactivity b on b.id = a.activityid
                            left join sys_wftbactivity c on c.id = a.toactivityid where remark='0' and b.autoid not in ('10001','10002','10006') and c.autoid not in ('10001','10002','10006')";
                            }

                        }
                    }
                    //重大隐患
                    else if (addtype != "1" && hidrank.Contains("重大隐患"))
                    {
                        //省级版本
                        if (addtype == "2")
                        {
                            sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where autoid != 10001 and  autoid != 10006 and processid ='2142740610' and  autoid <= 10008 order by autoid ";  //nodes;


                            sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                            left join sys_wftbactivity b on b.id = a.activityid
                            left join sys_wftbactivity c on c.id = a.toactivityid where a.remark like '0%' and b.autoid not in ('10001','10006') and c.autoid not in ('10001','10006') ";
                        }
                        else if (addtype == "3")  //国电新疆版本 减去完善、复查验证
                        {
                            sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where  autoid != 10002  and  autoid != 10006 and processid ='2142740610' and autoid <= 10008 order by autoid ";  //nodes;

                            sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                            left join sys_wftbactivity b on b.id = a.activityid
                            left join sys_wftbactivity c on c.id = a.toactivityid where a.remark like '0%' and b.autoid not in ('10002','10006') and c.autoid not in ('10002','10006') ";
                        }
                        else  //非省级
                        {
                            //当前机构为开封电厂的
                            if (curUser.OrganizeCode == KFDC_ORGCODE)
                            {
                                sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where autoid != 10006 and  autoid != 10002  and processid ='2142740610' and autoid <= 10008 order by autoid ";  //nodes;

                                //流程转向
                                sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                                    left join sys_wftbactivity b on b.id = a.activityid
                                    left join sys_wftbactivity c on c.id = a.toactivityid where a.remark like '0%'  and b.autoid not in ('10002','10006') and c.autoid not in ('10002','10006') and a.id !='a429a30e-1ee9-4f3d-8ded-2bfd294260b0'";
                            }
                            else
                            {
                                //通用电厂
                                sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where  autoid != 10006  and  autoid != 10002  and  autoid != 10001 and processid ='2142740610' and  autoid <= 10008 order by autoid ";  //nodes;

                                sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                            left join sys_wftbactivity b on b.id = a.activityid
                            left join sys_wftbactivity c on c.id = a.toactivityid where a.remark like '0%' and b.autoid not in ('10001','10002','10006') and c.autoid not in ('10001','10002','10006') ";
                            }
                        }
                    }
                    #endregion
                }
                
                var nodeDt = this.BaseRepository().FindTable(sqlnode);

                var lineDt = this.BaseRepository().FindTable(sqlline);

                var activityDt = GetWorkFlowDetail(keyValue, null);  //获取活动的当前实例历史流程记录
                //获取流程节点下所有的
                string allworkstream = string.Empty; //所有
                string preallworkstream = string.Empty; //所有
                string lastworkstream = string.Empty; //获取最后一个流程
                string lastprevworkstream = string.Empty; //获取最后一个流程的上一节点
                int totalrow = activityDt.Rows.Count;
                foreach (DataRow ativityRow in activityDt.Rows)
                {
                    allworkstream += ativityRow["prevnode"].ToString() + ",";
                    preallworkstream += ativityRow["curnode"].ToString() + ",";
                }
                lastworkstream = activityDt.Rows[totalrow - 1]["curnode"].ToString();
                lastprevworkstream = activityDt.Rows[totalrow - 1]["prevnode"].ToString();

                #region 构建nodes对象
                for (int i = 0; i < nodeDt.Rows.Count; i++)
                {
                    if (nodeDt.Rows[i]["name"].ToString() == workstream && nodeDt.Rows[i]["name"].ToString() != "整改结束")
                    {
                        flow.activeID = nodeDt.Rows[i]["id"].ToString();
                    }

                    nodes nodes = new nodes();

                    nodes.alt = true;
                    nodes.isclick = false;
                    nodes.css = "";
                    nodes.id = nodeDt.Rows[i]["id"].ToString(); //主键
                    nodes.img = "";
                    nodes.name = nodeDt.Rows[i]["name"].ToString();
                    if (nodeDt.Rows[i]["name"].ToString() == "隐患登记")
                    {
                        nodes.type = "startround";
                        nodes.height = 67;
                        nodes.left = 121;
                        nodes.top = 54;
                        nodes.width = 152;
                    }
                    else if (nodeDt.Rows[i]["name"].ToString() == "制定整改计划")
                    {
                        nodes.type = "stepnode";
                        nodes.height = 67;
                        nodes.left = 381;
                        nodes.top = 150;
                        nodes.width = 152;
                    }
                    else if (nodeDt.Rows[i]["name"].ToString() == "隐患完善")
                    {
                        nodes.type = "stepnode";
                        nodes.height = 67;
                        nodes.left = 381;
                        nodes.top = 150;
                        nodes.width = 152;
                    }
                    else if (nodeDt.Rows[i]["name"].ToString() == "隐患评估")
                    {
                        nodes.type = "stepnode";
                        nodes.height = 67;
                        nodes.left = 641;
                        nodes.top = 54;
                        nodes.width = 152;
                    }
                    else if (nodeDt.Rows[i]["name"].ToString() == "隐患整改")
                    {
                        nodes.type = "stepnode";

                        nodes.height = 67;
                        nodes.left = 381;
                        nodes.top = 273;
                        nodes.width = 152;
                    }
                    else if (nodeDt.Rows[i]["name"].ToString() == "隐患验收")
                    {
                        nodes.type = "stepnode";

                        nodes.height = 67;
                        nodes.left = 637;
                        nodes.top = 173;
                        nodes.width = 152;
                    }
                    else if (nodeDt.Rows[i]["name"].ToString() == "复查验证")
                    {
                        nodes.type = "stepnode";

                        nodes.height = 67;
                        nodes.left = 637;
                        nodes.top = 307;
                        nodes.width = 152;
                    }
                    else if (nodeDt.Rows[i]["name"].ToString() == "整改效果评估")
                    {
                        nodes.type = "stepnode";

                        nodes.height = 67;
                        nodes.left = 637;
                        nodes.top = 441;
                        nodes.width = 152;
                    }
                    else if (nodeDt.Rows[i]["name"].ToString() == "整改结束")
                    {
                        nodes.type = "endround";
                        if (addtype == "1")
                        {
                            nodes.height = 67;
                            nodes.left = 381;
                            nodes.top = 54;
                            nodes.width = 152;
                        }
                        else
                        {
                            nodes.height = 67;
                            nodes.left = 381;
                            nodes.top = 417;
                            nodes.width = 152;
                        }
                    }

                    //已经处理的部分(针对特殊的整改结束)
                    if (nodeDt.Rows[i]["name"].ToString() == "整改结束" && preallworkstream.Contains("整改结束"))
                    {
                        setInfo sinfo = new setInfo();
                        sinfo.Taged = 1;
                        sinfo.NodeName = lastworkstream;
                        //List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        //sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }


                    #region  已经处理的部分  最后一个流程节点是整改结束
                    if (allworkstream.Contains(nodeDt.Rows[i]["name"].ToString()) && !allworkstream.Contains("无"))
                    {

                        var activityData = GetWorkFlowDetail(keyValue, nodeDt.Rows[i]["name"].ToString());  //获取活动的当前实例历史流程记录

                        setInfo sinfo = new setInfo();
                        sinfo.Taged = 1;
                        sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        foreach (DataRow adRow in activityData.Rows)
                        {
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = adRow["createdate"].ToString();
                            nodedesignatedata.creatdept = adRow["creatdept"].ToString();
                            nodedesignatedata.createuser = adRow["createuser"].ToString();
                            nodedesignatedata.status = adRow["status"].ToString();
                            nodedesignatedata.prevnode = adRow["prevnode"].ToString();
                            nodelist.Add(nodedesignatedata);
                        }
                        sinfo.NodeDesignateData = nodelist;

                        nodes.setInfo = sinfo;
                    }
                    else
                    {
                        string sqlStr = string.Format(@"select a.id,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join (
                                             select departmentid,encode, fullname from base_department 
                                        ) e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  and b.name = '{1}'   order by a.autoid", keyValue, nodeDt.Rows[i]["name"].ToString());



                        DataTable strDt = this.BaseRepository().FindTable(sqlStr);



                        if (strDt.Rows.Count > 0)
                        {
                            setInfo sinfo = new setInfo();
                            sinfo.Taged = 1;
                            sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            foreach (DataRow adRow in strDt.Rows)
                            {
                                NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                nodedesignatedata.createdate = adRow["createdate"].ToString();
                                nodedesignatedata.creatdept = adRow["fullname"].ToString();
                                nodedesignatedata.createuser = adRow["realname"].ToString();
                                nodedesignatedata.status = "已处理";
                                string sqlStrTemp = string.Format(@"select a.id,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join (
                                           select departmentid,encode, fullname from base_department 
                                        ) e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  and c.name = '{1}' and a.autoid < {2}  order by a.autoid", keyValue, adRow["fromactivityname"].ToString(), adRow["autoid"].ToString());

                                DataTable strDtTemp = this.BaseRepository().FindTable(sqlStrTemp);

                                if (strDtTemp.Rows.Count > 0)
                                {
                                    int lastLength = strDtTemp.Rows.Count - 1; //取最后一条
                                    nodedesignatedata.prevnode = string.IsNullOrEmpty(strDtTemp.Rows[lastLength]["fromactivityname"].ToString()) ? "无" : strDtTemp.Rows[lastLength]["fromactivityname"].ToString();
                                }


                                nodelist.Add(nodedesignatedata);
                            }
                            sinfo.NodeDesignateData = nodelist;

                            nodes.setInfo = sinfo;
                        }
                    }
                    #endregion

                    #region 正在处理的部分
                    if (lastworkstream != "隐患登记" && lastworkstream != "整改结束" && nodeDt.Rows[i]["name"].ToString() == lastworkstream)
                    {
                        string[] lastuser = activityDt.Rows[totalrow - 1]["nextuser"].ToString().Replace("$", "").ToString().Split(',');
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
                        var newDt = this.BaseRepository().FindTable(newSql);
                        string lastNodeUser = ",";
                        string lastNodeDept = ",";
                        foreach (DataRow lastNodeRow in newDt.Rows)
                        {
                            lastNodeUser += lastNodeRow["realname"].ToString() + ",";
                            string tempDept = "," + lastNodeRow["fullname"].ToString() + ",";
                            if (!lastNodeDept.Contains(tempDept))
                            {
                                lastNodeDept += lastNodeRow["fullname"].ToString() + ",";
                            }
                        }
                        if (!string.IsNullOrEmpty(lastNodeUser) && lastNodeUser != ",")
                        {
                            lastNodeUser = lastNodeUser.Substring(1, lastNodeUser.Length - 2).ToString();
                        }
                        else
                        {
                            lastNodeUser = "";
                        }
                        if (!string.IsNullOrEmpty(lastNodeDept) && lastNodeDept != ",")
                        {
                            lastNodeDept = lastNodeDept.Substring(1, lastNodeDept.Length - 2).ToString();
                        }
                        else
                        {
                            lastNodeDept = "";
                        }
                        setInfo sinfo = new setInfo();
                        sinfo.Taged = 0;
                        sinfo.NodeName = lastworkstream;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = "待定";
                        nodedesignatedata.creatdept = lastNodeDept;
                        nodedesignatedata.createuser = lastNodeUser;
                        nodedesignatedata.status = "正在处理...";
                        nodedesignatedata.prevnode = lastprevworkstream;
                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    #endregion

                    nlist.Add(nodes);
                }
                #endregion

                #endregion

                #region 创建lines对象
                for (int i = 0; i < lineDt.Rows.Count; i++)
                {
                    lines lines = new lines();
                    lines.alt = true;
                    lines.id = lineDt.Rows[i]["id"].ToString();
                    lines.from = lineDt.Rows[i]["activityid"].ToString();
                    lines.to = lineDt.Rows[i]["toactivityid"].ToString();
                    lines.name = "";
                    lines.type = "sl";
                    llist.Add(lines);
                }
                #endregion

                flow.nodes = nlist;
                flow.lines = llist;

                return flow;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 获取违章的流程图对象
        /// <summary>
        /// 获取违章的流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetLllegalActionList(string keyValue)
        {
            //当前用户
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //开封电厂
            string KFDC_ORGCODE = new DataItemDetailService().GetItemValue("KFDC_ORGCODE");

            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            string sql = string.Empty;
            string sqlline = string.Empty;
            string sqlnode = string.Empty;

            #region  获取违章基本数据
            //获取违章基本数据
            sql = string.Format(@"select a.addtype,a.flowstate,b.rolename,a.belongdepartid  from bis_lllegalregister a
                                  left join base_user b on a.createuserid = b.userid where a.id ='{0}'", keyValue);

            var hidDt = this.BaseRepository().FindTable(sql);

            string flowstate = string.Empty;
            string addtype = string.Empty;
            string rolename = string.Empty;
            string orgid = string.Empty;
            if (hidDt.Rows.Count == 1)
            {
                flowstate = hidDt.Rows[0]["flowstate"].ToString();
                addtype = hidDt.Rows[0]["addtype"].ToString();
                rolename = hidDt.Rows[0]["rolename"].ToString();
                orgid = hidDt.Rows[0]["belongdepartid"].ToString();
            }
            #endregion

            Flow flow = new Flow();
            flow.title = "违章流程图";
            flow.initNum = 22;


            #region 创建nodes对象

            var majoritemlist = new DataItemDetailService().GetDataItemListByItemCode("'LllegalWorkFlowNodeView'");// 违章流程节点显示配置

            if (majoritemlist.Where(p => p.ItemName.Trim() == orgid ).Count() > 0)
            {
                //立即整改隐患
                if (addtype == "1")
                {
                    sqlnode = @"select id,name,autoid,kind from sys_wftbactivity where  processid='1f469854-a8aa-4a21-8886-c0579f64b984' and autoid in ('10014','10015') order by autoid ";  //nodes;

                    sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                            left join sys_wftbactivity b on b.id = a.activityid
                            left join sys_wftbactivity c on c.id = a.toactivityid where remark='3' and a.processid ='1f469854-a8aa-4a21-8886-c0579f64b984' ";
                }
                else
                {
                    string itemValue = majoritemlist.Where(p => p.ItemName.Trim() == orgid ).FirstOrDefault().ItemValue;
                    //通用电厂
                    sqlnode = string.Format(@"select id,name,autoid,kind from sys_wftbactivity  where  autoid in ({0}) and processid ='cc12f144-487b-4ac1-a12f-f842d620ca81' order by autoid ", itemValue);  //nodes;

                    sqlline = string.Format(@"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                            left join sys_wftbactivity b on b.id = a.activityid
                            left join sys_wftbactivity c on c.id = a.toactivityid where  b.autoid in ({0}) and c.autoid in ({0}) ", itemValue);
                }
            }
            else
            {
                //立即整改违章
                if (addtype == "1")
                {
                    sqlnode = @"select id,name,autoid,kind from sys_wftbactivity where  processid='1f469854-a8aa-4a21-8886-c0579f64b984' and autoid in ('10014','10015') order by autoid ";  //nodes;

                    sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                            left join sys_wftbactivity b on b.id = a.activityid
                            left join sys_wftbactivity c on c.id = a.toactivityid where remark='3' and a.processid ='1f469854-a8aa-4a21-8886-c0579f64b984' ";
                }
                //违章流程(非立即整改)
                else if (addtype == "0")
                {
                    //当前机构为开封电厂的
                    if (curUser.OrganizeCode == KFDC_ORGCODE)
                    {
                        sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where processid='cc12f144-487b-4ac1-a12f-f842d620ca81' and autoid !=10015 order by autoid ";  //nodes;

                        sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                                    left join sys_wftbactivity b on b.id = a.activityid
                                    left join sys_wftbactivity c on c.id = a.toactivityid where  remark='2' and a.processid ='cc12f144-487b-4ac1-a12f-f842d620ca81' and  b.autoid !='10015' and c.autoid !='10015'";
                    }
                    else
                    {
                        sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where processid='cc12f144-487b-4ac1-a12f-f842d620ca81' and autoid>= 10009 and autoid <= 10014  and autoid !='10012'  order by autoid ";  //nodes;

                        sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                                    left join sys_wftbactivity b on b.id = a.activityid
                                    left join sys_wftbactivity c on c.id = a.toactivityid where  remark='2' and a.processid ='cc12f144-487b-4ac1-a12f-f842d620ca81' and  b.autoid not in ('10012','10015','10016') and c.autoid not in ('10012','10015','10016')";
                    }
                }
                else if (rolename.Contains("省级用户"))
                {
                    sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where processid='cc12f144-487b-4ac1-a12f-f842d620ca81' and autoid>= 10009 and autoid <= 10016  and autoid !='10012' order by autoid ";  //nodes;

                    sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                            left join sys_wftbactivity b on b.id = a.activityid
                            left join sys_wftbactivity c on c.id = a.toactivityid where remark='2' and a.processid ='cc12f144-487b-4ac1-a12f-f842d620ca81'";
                }
            }

            var nodeDt = this.BaseRepository().FindTable(sqlnode);

            var lineDt = this.BaseRepository().FindTable(sqlline);

            var activityDt = GetWorkFlowDetail(keyValue, null);  //获取活动的当前实例历史流程记录 
            //获取流程节点下所有的
            string allworkstream = string.Empty; //所有
            string preallworkstream = string.Empty;//所有
            string lastworkstream = string.Empty; //获取最后一个流程
            string lastworknodeid = string.Empty; ///获取最后一个流程对应的流程id
            string lastprevworkstream = string.Empty; //获取最后一个流程的上一节点
            int totalrow = activityDt.Rows.Count;
            foreach (DataRow ativityRow in activityDt.Rows)
            {
                allworkstream += ativityRow["prevnode"].ToString() + ",";
                preallworkstream += ativityRow["curnode"].ToString() + ",";
            }
            lastworknodeid = activityDt.Rows[totalrow - 1]["curnodeid"].ToString();
            lastworkstream = activityDt.Rows[totalrow - 1]["curnode"].ToString();
            lastprevworkstream = activityDt.Rows[totalrow - 1]["prevnode"].ToString();

            #region 构建nodes对象
            for (int i = 0; i < nodeDt.Rows.Count; i++)
            {
                //流程相同情况下，保证流程id是流程节点最后一个。
                if (nodeDt.Rows[i]["name"].ToString() == flowstate && nodeDt.Rows[i]["id"].ToString() == lastworknodeid)
                {
                    flow.activeID = nodeDt.Rows[i]["id"].ToString();
                }
                nodes nodes = new nodes();
                nodes.alt = true;
                nodes.isclick = false;
                nodes.css = "";
                nodes.id = nodeDt.Rows[i]["id"].ToString(); //主键
                nodes.img = "";
                nodes.name = nodeDt.Rows[i]["name"].ToString();
                if (nodeDt.Rows[i]["name"].ToString() == "违章登记" || nodeDt.Rows[i]["name"].ToString() == "违章举报")
                {
                    nodes.type = "startround";
                    nodes.height = 67;
                    nodes.left = 150;
                    nodes.top = 60;
                    nodes.width = 152;
                }
                else if (nodeDt.Rows[i]["name"].ToString() == "违章完善")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 460;
                    nodes.top = 60;
                    nodes.width = 152;
                }
                else if (nodeDt.Rows[i]["name"].ToString() == "违章核准" || nodeDt.Rows[i]["name"].ToString() == "违章审核")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 150;
                    nodes.top = 200;
                    nodes.width = 152;
                }
                else if (nodeDt.Rows[i]["name"].ToString() == "制定整改计划")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 660;
                    nodes.top = 200;
                    nodes.width = 152;
                }
                else if (nodeDt.Rows[i]["name"].ToString() == "违章整改")
                {
                    nodes.type = "stepnode";

                    nodes.height = 67;
                    nodes.left = 460;
                    nodes.top = 320;
                    nodes.width = 152;
                }
                else if (nodeDt.Rows[i]["name"].ToString() == "违章验收")
                {
                    nodes.type = "stepnode";

                    nodes.height = 67;
                    nodes.left = 150;
                    nodes.top = 340;
                    nodes.width = 152;
                }
                else if (nodeDt.Rows[i]["name"].ToString() == "验收确认")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 150;
                    nodes.top = 480;
                    nodes.width = 152;
                }
                else if (nodeDt.Rows[i]["name"].ToString() == "流程结束")
                {
                    nodes.type = "endround";
                    if (addtype == "1")
                    {
                        nodes.height = 67;
                        nodes.left = 680;
                        nodes.top = 60;
                        nodes.width = 152;
                    }
                    else
                    {
                        nodes.height = 67;
                        nodes.left = 460;
                        nodes.top = 480;
                        nodes.width = 152;
                    }
                }


                //已经处理的部分(针对特殊的整改结束)
                if (nodeDt.Rows[i]["name"].ToString() == "流程结束" && preallworkstream.Contains("流程结束"))
                {
                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 1;
                    sinfo.NodeName = lastworkstream;
                    nodes.setInfo = sinfo;
                }

                //已经处理的部分
                #region  已经处理的部分
                if (allworkstream.Contains(nodeDt.Rows[i]["name"].ToString()) && !allworkstream.Contains("无"))
                {
                    var activityData = GetWorkFlowDetail(keyValue, nodeDt.Rows[i]["name"].ToString(), nodeDt.Rows[i]["id"].ToString());  //获取活动的当前实例历史流程记录

                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 1;
                    sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    foreach (DataRow adRow in activityData.Rows)
                    {
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = adRow["createdate"].ToString();
                        nodedesignatedata.creatdept = adRow["creatdept"].ToString();
                        nodedesignatedata.createuser = adRow["createuser"].ToString();
                        nodedesignatedata.status = adRow["status"].ToString();
                        nodedesignatedata.prevnode = adRow["prevnode"].ToString();
                        nodelist.Add(nodedesignatedata);
                    }
                    sinfo.NodeDesignateData = nodelist;

                    nodes.setInfo = sinfo;
                }
                else
                {
                    string sqlStr = string.Format(@"select a.id,b.id  activityid,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join (
                                            select departmentid,encode, fullname from base_department 
                                        ) e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  and b.name = '{1}' and b.id ='{2}'   order by a.autoid", keyValue, nodeDt.Rows[i]["name"].ToString(), nodeDt.Rows[i]["id"].ToString());

                    DataTable strDt = this.BaseRepository().FindTable(sqlStr);

                    if (strDt.Rows.Count > 0)
                    {
                        setInfo sinfo = new setInfo();
                        sinfo.Taged = 1;
                        sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        foreach (DataRow adRow in strDt.Rows)
                        {
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = adRow["createdate"].ToString();
                            nodedesignatedata.creatdept = adRow["fullname"].ToString();
                            nodedesignatedata.createuser = adRow["realname"].ToString();
                            nodedesignatedata.status = "已处理";
                            string sqlStrTemp = string.Format(@"select a.id,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join (
                                           select departmentid,encode, fullname from base_department 
                                        ) e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  and c.name = '{1}' and a.autoid < {2}  order by a.autoid", keyValue, adRow["fromactivityname"].ToString(), adRow["autoid"].ToString());

                            DataTable strDtTemp = this.BaseRepository().FindTable(sqlStrTemp);

                            if (strDtTemp.Rows.Count > 0)
                            {
                                int lastLength = strDtTemp.Rows.Count - 1; //取最后一条
                                nodedesignatedata.prevnode = string.IsNullOrEmpty(strDtTemp.Rows[lastLength]["fromactivityname"].ToString()) ? "无" : strDtTemp.Rows[lastLength]["fromactivityname"].ToString();
                            }
                            nodelist.Add(nodedesignatedata);
                        }
                        sinfo.NodeDesignateData = nodelist;

                        nodes.setInfo = sinfo;
                    }
                }
                #endregion

                #region 正在处理的部分
                if (lastworkstream != "流程结束" && nodeDt.Rows[i]["name"].ToString() == lastworkstream && nodeDt.Rows[i]["id"].ToString() == lastworknodeid)
                {
                    string[] lastuser = activityDt.Rows[totalrow - 1]["nextuser"].ToString().Replace("$", "").ToString().Split(',');
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
                    var newDt = this.BaseRepository().FindTable(newSql);
                    string lastNodeUser = ",";
                    string lastNodeDept = ",";
                    foreach (DataRow lastNodeRow in newDt.Rows)
                    {
                        lastNodeUser += lastNodeRow["realname"].ToString() + ",";
                        string tempDept = "," + lastNodeRow["fullname"].ToString() + ",";
                        if (!lastNodeDept.Contains(tempDept))
                        {
                            lastNodeDept += lastNodeRow["fullname"].ToString() + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(lastNodeUser) && lastNodeUser.Length > 2)
                    {
                        lastNodeUser = lastNodeUser.Substring(1, lastNodeUser.Length - 2).ToString();
                    }
                    if (!string.IsNullOrEmpty(lastNodeDept) && lastNodeDept.Length > 2)
                    {
                        lastNodeDept = lastNodeDept.Substring(1, lastNodeDept.Length - 2).ToString();
                    }
                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 0;
                    sinfo.NodeName = lastworkstream;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                    nodedesignatedata.createdate = "待定";
                    nodedesignatedata.creatdept = lastNodeDept;
                    nodedesignatedata.createuser = lastNodeUser;
                    nodedesignatedata.status = "正在处理...";
                    nodedesignatedata.prevnode = lastprevworkstream;
                    nodelist.Add(nodedesignatedata);
                    sinfo.NodeDesignateData = nodelist;
                    nodes.setInfo = sinfo;
                }
                #endregion

                nlist.Add(nodes);
            }
            #endregion

            #endregion

            #region 创建lines对象
            for (int i = 0; i < lineDt.Rows.Count; i++)
            {
                lines lines = new lines();
                lines.alt = true;
                lines.id = lineDt.Rows[i]["id"].ToString();
                lines.from = lineDt.Rows[i]["activityid"].ToString();
                lines.to = lineDt.Rows[i]["toactivityid"].ToString();
                lines.name = "";
                lines.type = "sl";
                llist.Add(lines);
            }
            #endregion

            flow.nodes = nlist;
            flow.lines = llist;

            return flow;
        }
        #endregion

        #region 获取问题的流程图对象
        /// <summary>
        /// 获取问题的流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetQuestionActionList(string keyValue)
        {
            //当前用户
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            string sql = string.Empty;
            string sqlline = string.Empty;
            string sqlnode = string.Empty;

            #region  获取问题基本数据
            //获取问题基本数据
            sql = string.Format(@"select a.flowstate,b.rolename from bis_questioninfo a
                                  left join base_user b on a.createuserid = b.userid where a.id ='{0}'", keyValue);

            var dt = this.BaseRepository().FindTable(sql);

            string flowstate = string.Empty;
            string rolename = string.Empty;

            if (dt.Rows.Count == 1)
            {
                flowstate = dt.Rows[0]["flowstate"].ToString();
                rolename = dt.Rows[0]["rolename"].ToString();
            }
            #endregion

            Flow flow = new Flow();
            flow.title = "问题流程图";
            flow.initNum = 22;

            #region 创建nodes对象

            sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where processid='82803e3a-f126-4945-9206-1fca7158c60b' order by autoid ";  //nodes;

            sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a 
                        left join sys_wftbactivity b on b.id = a.activityid
                        left join sys_wftbactivity c on c.id = a.toactivityid where  remark='3' and a.processid ='82803e3a-f126-4945-9206-1fca7158c60b'";


            var nodeDt = this.BaseRepository().FindTable(sqlnode);

            var lineDt = this.BaseRepository().FindTable(sqlline);

            var activityDt = GetWorkFlowDetail(keyValue, null);  //获取活动的当前实例历史流程记录 
            //获取流程节点下所有的
            string allworkstream = string.Empty; //所有
            string preallworkstream = string.Empty;//所有
            string lastworkstream = string.Empty; //获取最后一个流程
            string lastworknodeid = string.Empty; ///获取最后一个流程对应的流程id
            string lastprevworkstream = string.Empty; //获取最后一个流程的上一节点
            int totalrow = activityDt.Rows.Count;
            foreach (DataRow ativityRow in activityDt.Rows)
            {
                allworkstream += ativityRow["prevnode"].ToString() + ",";
                preallworkstream += ativityRow["curnode"].ToString() + ",";
            }
            lastworknodeid = activityDt.Rows[totalrow - 1]["curnodeid"].ToString();
            lastworkstream = activityDt.Rows[totalrow - 1]["curnode"].ToString();
            lastprevworkstream = activityDt.Rows[totalrow - 1]["prevnode"].ToString();

            #region 构建nodes对象
            for (int i = 0; i < nodeDt.Rows.Count; i++)
            {
                //流程相同情况下，保证流程id是流程节点最后一个。
                if (nodeDt.Rows[i]["name"].ToString() == flowstate && nodeDt.Rows[i]["id"].ToString() == lastworknodeid)
                {
                    flow.activeID = nodeDt.Rows[i]["id"].ToString();
                }
                nodes nodes = new nodes();
                nodes.alt = true;
                nodes.isclick = false;
                nodes.css = "";
                nodes.id = nodeDt.Rows[i]["id"].ToString(); //主键
                nodes.img = "";
                nodes.name = nodeDt.Rows[i]["name"].ToString();
                if (nodeDt.Rows[i]["name"].ToString() == "问题登记")
                {
                    nodes.type = "startround";
                    nodes.height = 67;
                    nodes.left = 150;
                    nodes.top = 60;
                    nodes.width = 152;
                }
                else if (nodeDt.Rows[i]["name"].ToString() == "问题整改")
                {
                    nodes.type = "stepnode";

                    nodes.height = 67;
                    nodes.left = 460;
                    nodes.top = 200;
                    nodes.width = 152;
                }
                else if (nodeDt.Rows[i]["name"].ToString() == "问题验证")
                {
                    nodes.type = "stepnode";

                    nodes.height = 67;
                    nodes.left = 150;
                    nodes.top = 200;
                    nodes.width = 152;
                }

                else if (nodeDt.Rows[i]["name"].ToString() == "流程结束")
                {
                    nodes.type = "endround";
                    nodes.height = 67;
                    nodes.left = 460;
                    nodes.top = 350;
                    nodes.width = 152;
                }


                //已经处理的部分(针对特殊的整改结束) && preallworkstream.Contains("流程结束")
                if (nodeDt.Rows[i]["name"].ToString() == "流程结束" && lastworkstream == nodeDt.Rows[i]["name"].ToString())
                {
                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 1;
                    sinfo.NodeName = lastworkstream;
                    nodes.setInfo = sinfo;
                }

                //已经处理的部分
                #region  已经处理的部分
                if (allworkstream.Contains(nodeDt.Rows[i]["name"].ToString()) && !allworkstream.Contains("无"))
                {
                    var activityData = GetWorkFlowDetail(keyValue, nodeDt.Rows[i]["name"].ToString(), nodeDt.Rows[i]["id"].ToString());  //获取活动的当前实例历史流程记录

                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 1;
                    sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    foreach (DataRow adRow in activityData.Rows)
                    {
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = adRow["createdate"].ToString();
                        nodedesignatedata.creatdept = adRow["creatdept"].ToString();
                        nodedesignatedata.createuser = adRow["createuser"].ToString();
                        nodedesignatedata.status = adRow["status"].ToString();
                        nodedesignatedata.prevnode = adRow["prevnode"].ToString();
                        nodelist.Add(nodedesignatedata);
                    }
                    sinfo.NodeDesignateData = nodelist;

                    nodes.setInfo = sinfo;
                }
                else
                {
                    string sqlStr = string.Format(@"select a.id,b.id  activityid,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join (
                                            select departmentid,encode, fullname from base_department 
                                        ) e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  and b.name = '{1}' and b.id ='{2}'   order by a.autoid", keyValue, nodeDt.Rows[i]["name"].ToString(), nodeDt.Rows[i]["id"].ToString());

                    DataTable strDt = this.BaseRepository().FindTable(sqlStr);

                    if (strDt.Rows.Count > 0)
                    {

                        bool isadd = false;
                        setInfo sinfo = new setInfo();
                        sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                        if (lastworkstream == "流程结束" && sinfo.NodeName == lastworkstream)
                        {
                            isadd = true;
                        }
                        if (sinfo.NodeName != "流程结束")
                        {
                            isadd = true;
                        }
                        if (isadd)
                        {
                            sinfo.Taged = 1;
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            foreach (DataRow adRow in strDt.Rows)
                            {
                                if (adRow["fromactivityname"].ToString() != "流程结束")
                                {
                                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                    nodedesignatedata.createdate = adRow["createdate"].ToString();
                                    nodedesignatedata.creatdept = adRow["fullname"].ToString();
                                    nodedesignatedata.createuser = adRow["realname"].ToString();
                                    nodedesignatedata.status = "已处理";
                                    string sqlStrTemp = string.Format(@"select a.id,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                    c.name as toactivityname from sys_wftbinstancedetail  a
                                    left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                    left join sys_wftbactivity c on a.toactivityid = c.id 
                                    left join base_user d on a.createuser = d.account 
                                    left join (
                                        select departmentid,encode, fullname from base_department 
                                    ) e on d.departmentcode = e.encode               
                                    left join  sys_wftbinstance f on a.instanceid = f.id 
                                    where f.objectid ='{0}'  and c.name = '{1}' and a.autoid < {2}  order by a.autoid", keyValue, adRow["fromactivityname"].ToString(), adRow["autoid"].ToString());

                                    DataTable strDtTemp = this.BaseRepository().FindTable(sqlStrTemp);

                                    if (strDtTemp.Rows.Count > 0)
                                    {
                                        int lastLength = strDtTemp.Rows.Count - 1; //取最后一条
                                        nodedesignatedata.prevnode = string.IsNullOrEmpty(strDtTemp.Rows[lastLength]["fromactivityname"].ToString()) ? "无" : strDtTemp.Rows[lastLength]["fromactivityname"].ToString();
                                    }
                                    nodelist.Add(nodedesignatedata);
                                }
                            }
                            sinfo.NodeDesignateData = nodelist;
                            nodes.setInfo = sinfo;
                        }

                    }
                }
                #endregion

                #region 正在处理的部分
                if (lastworkstream != "流程结束" && nodeDt.Rows[i]["name"].ToString() == lastworkstream && nodeDt.Rows[i]["id"].ToString() == lastworknodeid)
                {
                    string[] lastuser = activityDt.Rows[totalrow - 1]["nextuser"].ToString().Replace("$", "").ToString().Split(',');
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
                    var newDt = this.BaseRepository().FindTable(newSql);
                    string lastNodeUser = ",";
                    string lastNodeDept = ",";
                    foreach (DataRow lastNodeRow in newDt.Rows)
                    {
                        lastNodeUser += lastNodeRow["realname"].ToString() + ",";
                        string tempDept = "," + lastNodeRow["fullname"].ToString() + ",";
                        if (!lastNodeDept.Contains(tempDept))
                        {
                            lastNodeDept += lastNodeRow["fullname"].ToString() + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(lastNodeUser) && lastNodeUser.Length > 2)
                    {
                        lastNodeUser = lastNodeUser.Substring(1, lastNodeUser.Length - 2).ToString();
                    }
                    if (!string.IsNullOrEmpty(lastNodeDept) && lastNodeDept.Length > 2)
                    {
                        lastNodeDept = lastNodeDept.Substring(1, lastNodeDept.Length - 2).ToString();
                    }
                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 0;
                    sinfo.NodeName = lastworkstream;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                    nodedesignatedata.createdate = "待定";
                    nodedesignatedata.creatdept = lastNodeDept;
                    nodedesignatedata.createuser = lastNodeUser;
                    nodedesignatedata.status = "正在处理...";
                    nodedesignatedata.prevnode = lastprevworkstream;
                    nodelist.Add(nodedesignatedata);
                    sinfo.NodeDesignateData = nodelist;
                    nodes.setInfo = sinfo;
                }
                #endregion

                nlist.Add(nodes);
            }
            #endregion

            #endregion

            #region 创建lines对象
            for (int i = 0; i < lineDt.Rows.Count; i++)
            {
                lines lines = new lines();
                lines.alt = true;
                lines.id = lineDt.Rows[i]["id"].ToString();
                lines.from = lineDt.Rows[i]["activityid"].ToString();
                lines.to = lineDt.Rows[i]["toactivityid"].ToString();
                lines.name = "";
                lines.type = "sl";
                llist.Add(lines);
            }
            #endregion

            flow.nodes = nlist;
            flow.lines = llist;

            return flow;
        }
        #endregion

        #region 获取流程节点详情
        /// <summary>
        /// 获取流程节点详情
        /// </summary>
        /// <param name="instanceid">引用id</param>
        /// <param name="fromwork">工作流程</param>
        /// <returns></returns>
        public DataTable GetWorkFlowDetail(string instanceid, string fromwork, string fromworkid = "")
        {
            string sql = string.Empty;

            string sqlStr = string.Empty;
            string whereStr = "";
            if (!string.IsNullOrEmpty(fromwork))
            {
                whereStr = string.Format("  and  b.name ='{0}'", fromwork);
            }
            if (!string.IsNullOrEmpty(fromworkid))
            {
                whereStr = string.Format("  and  c.id ='{0}'", fromworkid);
            }

            sql = string.Format(@"select a.id,c.id cid,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join (
                                          select departmentid,encode, fullname from base_department 
                                          union 
                                          select organizeid as departmentid,encode ,fullname from base_organize
                                        ) e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  {1}   order by a.autoid", instanceid, whereStr);

            DataTable dt = this.BaseRepository().FindTable(sql);


            DataTable rdt = new DataTable();
            rdt.Columns.Add("curnodeid"); //当前节点
            rdt.Columns.Add("curnode"); //当前节点
            rdt.Columns.Add("createdate"); //处理时间
            rdt.Columns.Add("creatdept");//处理单位
            rdt.Columns.Add("createuser"); //处理人
            rdt.Columns.Add("status"); //处理状态
            rdt.Columns.Add("prevnode");//上一节点
            rdt.Columns.Add("nextuser");//下一节点处理人员0

            //存在提交的流程流转记录
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow rRow = rdt.NewRow();
                    rRow["curnodeid"] = row["cid"].ToString();   //当前流程id
                    rRow["curnode"] = row["toactivityname"].ToString();   //当前流程
                    rRow["createdate"] = row["createdate"].ToString(); //处理时间
                    rRow["creatdept"] = row["fullname"].ToString(); //处理单位
                    rRow["createuser"] = row["realname"].ToString(); //处理人
                    rRow["status"] = "已处理"; //处理状态

                    if (!string.IsNullOrEmpty(row["fromactivityname"].ToString()))
                    {
                        rRow["prevnode"] = row["fromactivityname"].ToString();  //上一节点
                    }
                    else
                    {
                        rRow["prevnode"] = "无";
                    }
                    rRow["nextuser"] = row["participant"].ToString();  //下一节点处理人员
                    rdt.Rows.Add(rRow);
                }
            }
            return rdt;
        }
        #endregion

        #region 根据实例id获取对应的退回流程实例相关对象
        /// <summary>
        /// 根据实例id获取对应的流程实例相关对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetBackFlowObjectByKey(string keyValue)
        {

            //当前用户
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            DataTable dt = new DataTable();
            dt.Columns.Add("participant"); //参与人
            dt.Columns.Add("wfflag"); //流程走向
            dt.Columns.Add("createuser"); //流程走向
            dt.Columns.Add("fromname");//上一节点
            dt.Columns.Add("curname"); //当前节点
            dt.Columns.Add("isupdate"); //是否更改流程状态
           
            DataRow row = dt.NewRow();
            try
            {
                string sql = string.Format(@"select b.id, b.createuser,c.name fromname,d.name curname,c.autoid fromautoid,d.autoid toautoid, b.fromactivityid,  b.toactivityid, b.operuser,b.participant from sys_wftbinstance a 
                                                        left join sys_wftbinstancedetail b on a.id = b.instanceid 
                                                        left join sys_wftbactivity c on b.fromactivityid = c.id 
                                                        left join sys_wftbactivity d on b.toactivityid = d.id 
                                                        where a.objectid ='{0}' order by b.autoid ", keyValue);

                DataTable ztDt = this.BaseRepository().FindTable(sql);
                if (ztDt.Rows.Count > 0)
                {
                    int lastIndex = ztDt.Rows.Count - 1;
                   
                    string fromactivityid = string.Empty;
                    string toactivityid = string.Empty;
                    string wfflag = string.Empty;
                    string curactivityid = ztDt.Rows[lastIndex]["toactivityid"].ToString(); //当前所处节点id

                    string tempsql = string.Format(@"select b.id, b.createuser,c.name fromname,d.name curname,c.autoid fromautoid,d.autoid toautoid, b.fromactivityid,  b.toactivityid, b.operuser,b.participant from sys_wftbinstance a 
                                                        left join sys_wftbinstancedetail b on a.id = b.instanceid 
                                                        left join sys_wftbactivity c on b.fromactivityid = c.id 
                                                        left join sys_wftbactivity d on b.toactivityid = d.id 
                                                        where a.objectid ='{0}' and b.toactivityid ='{1}' and b.remark !='退回' and   (','|| substr(b.participant,2,length(b.participant)-1)||',')  like '%,{2},%' and  d.autoid >= c.autoid  order by b.autoid ", keyValue, curactivityid, curUser.Account);

                    DataTable tempDt = this.BaseRepository().FindTable(tempsql);

                    if (tempDt.Rows.Count > 0)
                    {
                        int prevLastIndex = tempDt.Rows.Count - 1;
                        fromactivityid = tempDt.Rows[prevLastIndex]["fromactivityid"].ToString();
                        toactivityid = tempDt.Rows[prevLastIndex]["toactivityid"].ToString();
                        row["participant"] = tempDt.Rows[prevLastIndex]["operuser"].ToString(); //上一个流程节点的操作人
                        string createuser = tempDt.Rows[prevLastIndex]["createuser"].ToString();  //上一个流程节点的创建人
                        row["createuser"] = createuser; //创建人
                        row["fromname"] = tempDt.Rows[prevLastIndex]["fromname"].ToString(); //上一节点
                    }
                    else 
                    {
                        fromactivityid = ztDt.Rows[lastIndex]["fromactivityid"].ToString();
                        toactivityid = ztDt.Rows[lastIndex]["toactivityid"].ToString();
                        row["participant"] = ztDt.Rows[lastIndex]["operuser"].ToString(); //上一个流程节点的操作人
                        string createuser = ztDt.Rows[lastIndex]["createuser"].ToString();  //上一个流程节点的创建人
                        row["createuser"] = createuser; //创建人
                        row["fromname"] = ztDt.Rows[lastIndex]["fromname"].ToString(); //上一节点
                    }
                    row["curname"] = ztDt.Rows[lastIndex]["curname"].ToString(); //当前节点
                    row["isupdate"] ="1";


          

                    //流程活动转向
                    string tsql = string.Format(@"select * from  sys_wftbcondition where toactivityid ='{0}' and activityid ='{1}'", fromactivityid, toactivityid);
                    //获取流程转向
                    DataTable tDt = this.BaseRepository().FindTable(tsql);
                    if (tDt.Rows.Count > 0)
                    {
                        string expression = tDt.Rows[0]["expression"].ToString();
                        if (!string.IsNullOrEmpty(expression))
                        {
                            wfflag = expression.Substring(expression.Length - 1);
                        }
                        row["wfflag"] = wfflag;
                    }
                    else
                    {
                        //不更改流程状态
                        if (fromactivityid == toactivityid) 
                        {
                            row["isupdate"] = "0";
                        }
                        row["wfflag"] = "";
                    }
                }
                else
                {
                    row["participant"] = "";
                    row["wfflag"] = "";
                    row["createuser"] = ""; //创建人
                    row["isupdate"] = "0";
                }
            }
            catch (Exception)
            {
                throw;
            }
            dt.Rows.Add(row);
            return dt;
        }
        #endregion

        #region 获取标准流程图对象
        /// <summary>
        /// 获取标准修编的流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetStandardApplyActionList(string keyValue)
        {

            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            string sql = string.Empty;
            string sqlline = string.Empty;
            string sqlnode = string.Empty;

            #region  获取基本数据
            //获取违章基本数据
            sql = string.Format(@"select a.flowstate from hrs_standardapply a where a.id ='{0}'", keyValue);

            var hidDt = this.BaseRepository().FindTable(sql);

            string flowstate = string.Empty;

            if (hidDt.Rows.Count == 1)
            {
                flowstate = hidDt.Rows[0]["flowstate"].ToString();
            }
            #endregion

            Flow flow = new Flow();
            flow.title = "标准修（订）审核（批）流程图";
            flow.initNum = 22;

            #region 创建nodes对象

            sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where processid='76c4c857-a3e1-45eb-9c61-e8e5dd9bf880' order by autoid ";  //nodes;

            sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a
                    left join sys_wftbactivity b on b.id = a.activityid
                    left join sys_wftbactivity c on c.id = a.toactivityid where remark='2' and a.processid='76c4c857-a3e1-45eb-9c61-e8e5dd9bf880'";


            var nodeDt = this.BaseRepository().FindTable(sqlnode);

            var lineDt = this.BaseRepository().FindTable(sqlline);

            var activityDt = GetWorkFlowDetail(keyValue, null);  //获取活动的当前实例历史流程记录 
            //获取流程节点下所有的
            string allworkstream = string.Empty; //所有
            string preallworkstream = string.Empty;//所有
            string lastworkstream = string.Empty; //获取最后一个流程
            string lastworknodeid = string.Empty; ///获取最后一个流程对应的流程id
            string lastprevworkstream = string.Empty; //获取最后一个流程的上一节点
            int totalrow = activityDt.Rows.Count;
            foreach (DataRow ativityRow in activityDt.Rows)
            {
                allworkstream += ativityRow["prevnode"].ToString() + ",";
                preallworkstream += ativityRow["curnode"].ToString() + ",";
            }
            lastworknodeid = activityDt.Rows[totalrow - 1]["curnodeid"].ToString();
            lastworkstream = activityDt.Rows[totalrow - 1]["curnode"].ToString();
            lastprevworkstream = activityDt.Rows[totalrow - 1]["prevnode"].ToString();

            #region 构建nodes对象
            for (int i = 0; i < nodeDt.Rows.Count; i++)
            {
                //流程相同情况下，保证流程id是流程节点最后一个。
                if (nodeDt.Rows[i]["name"].ToString() == flowstate && nodeDt.Rows[i]["id"].ToString() == lastworknodeid)
                {
                    flow.activeID = nodeDt.Rows[i]["id"].ToString();
                }
                nodes nodes = new nodes();
                nodes.alt = true;
                nodes.isclick = false;
                nodes.css = "";
                nodes.id = nodeDt.Rows[i]["id"].ToString(); //主键
                nodes.img = "";
                nodes.name = nodeDt.Rows[i]["name"].ToString();
                if (nodes.name == "申请人申请")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 90;
                    nodes.top = 243;
                    nodes.width = 152;
                }
                else if (nodes.name == "1级审核")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 100;
                    nodes.top = 86;
                    nodes.width = 152;
                }
                else if (nodes.name == "2级审核")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 400;
                    nodes.top = 86;
                    nodes.width = 152;
                }
                else if (nodes.name == "审核分配会签")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 680;
                    nodes.top = 86;
                    nodes.width = 152;
                }
                else if (nodes.name == "部门会签")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 680;
                    nodes.top = 243;
                    nodes.width = 152;
                }
                else if (nodes.name == "分配分委会")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 680;
                    nodes.top = 426;
                    nodes.width = 152;
                }
                else if (nodes.name == "分委会审核")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 400;
                    nodes.top = 426;
                    nodes.width = 152;
                }
                else if (nodes.name == "审批")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 100;
                    nodes.top = 426;
                    nodes.width = 152;
                }
                else if (nodes.name == "结束")
                {
                    nodes.type = "endround";
                    nodes.height = 67;
                    nodes.left = 400;
                    nodes.top = 253;
                    nodes.width = 152;
                }

                //已经处理的部分(针对特殊的整改结束)
                if (nodes.name == "结束" && preallworkstream.Contains("结束"))
                {
                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 1;
                    sinfo.NodeName = lastworkstream;
                    nodes.setInfo = sinfo;
                }

                //已经处理的部分
                #region  已经处理的部分
                if (allworkstream.Contains(nodeDt.Rows[i]["name"].ToString()) && !allworkstream.Contains("无"))
                {
                    var activityData = GetWorkFlowDetail(keyValue, nodeDt.Rows[i]["name"].ToString(), nodeDt.Rows[i]["id"].ToString());  //获取活动的当前实例历史流程记录

                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 1;
                    sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    foreach (DataRow adRow in activityData.Rows)
                    {
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = adRow["createdate"].ToString();
                        nodedesignatedata.creatdept = adRow["creatdept"].ToString();
                        nodedesignatedata.createuser = adRow["createuser"].ToString();
                        nodedesignatedata.status = adRow["status"].ToString();
                        nodedesignatedata.prevnode = adRow["prevnode"].ToString();
                        nodelist.Add(nodedesignatedata);
                    }
                    sinfo.NodeDesignateData = nodelist;

                    nodes.setInfo = sinfo;
                }
                else
                {
                    string sqlStr = string.Format(@"select a.id,b.id  activityid,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join (
                                          select departmentid,encode, fullname from base_department 
                                          union 
                                          select organizeid as departmentid,encode ,fullname from base_organize
                                        ) e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  and b.name = '{1}' and b.id ='{2}'   order by a.autoid", keyValue, nodeDt.Rows[i]["name"].ToString(), nodeDt.Rows[i]["id"].ToString());

                    DataTable strDt = this.BaseRepository().FindTable(sqlStr);

                    if (strDt.Rows.Count > 0)
                    {
                        setInfo sinfo = new setInfo();
                        sinfo.Taged = 1;
                        sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        foreach (DataRow adRow in strDt.Rows)
                        {
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = adRow["createdate"].ToString();
                            nodedesignatedata.creatdept = adRow["fullname"].ToString();
                            nodedesignatedata.createuser = adRow["realname"].ToString();
                            nodedesignatedata.status = "已处理";
                            string sqlStrTemp = string.Format(@"select a.id,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join (
                                          select departmentid,encode, fullname from base_department 
                                          union 
                                          select organizeid as departmentid,encode ,fullname from base_organize
                                        ) e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  and c.name = '{1}' and a.autoid < {2}  order by a.autoid", keyValue, adRow["fromactivityname"].ToString(), adRow["autoid"].ToString());

                            DataTable strDtTemp = this.BaseRepository().FindTable(sqlStrTemp);

                            if (strDtTemp.Rows.Count > 0)
                            {
                                int lastLength = strDtTemp.Rows.Count - 1; //取最后一条
                                nodedesignatedata.prevnode = string.IsNullOrEmpty(strDtTemp.Rows[lastLength]["fromactivityname"].ToString()) ? "无" : strDtTemp.Rows[lastLength]["fromactivityname"].ToString();
                            }
                            nodelist.Add(nodedesignatedata);
                        }
                        sinfo.NodeDesignateData = nodelist;

                        nodes.setInfo = sinfo;
                    }
                }
                #endregion

                #region 正在处理的部分
                if (lastworkstream != "结束" && nodeDt.Rows[i]["name"].ToString() == lastworkstream && nodeDt.Rows[i]["id"].ToString() == lastworknodeid)
                {
                    string[] lastuser = activityDt.Rows[totalrow - 1]["nextuser"].ToString().Replace("$", "").ToString().Split(',');
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
                                          union 
                                          select organizeid as departmentid,encode ,fullname from base_organize
                                        ) b on  a.departmentcode = b.encode where  a.account in ({0})", newuserStr);
                    var newDt = this.BaseRepository().FindTable(newSql);
                    string lastNodeUser = ",";
                    string lastNodeDept = ",";
                    foreach (DataRow lastNodeRow in newDt.Rows)
                    {
                        lastNodeUser += lastNodeRow["realname"].ToString() + ",";
                        string tempDept = "," + lastNodeRow["fullname"].ToString() + ",";
                        if (!lastNodeDept.Contains(tempDept))
                        {
                            lastNodeDept += lastNodeRow["fullname"].ToString() + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(lastNodeUser) && lastNodeUser.Length > 2)
                    {
                        lastNodeUser = lastNodeUser.Substring(1, lastNodeUser.Length - 2).ToString();
                    }
                    if (!string.IsNullOrEmpty(lastNodeDept) && lastNodeDept.Length > 2)
                    {
                        lastNodeDept = lastNodeDept.Substring(1, lastNodeDept.Length - 2).ToString();
                    }
                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 0;
                    sinfo.NodeName = lastworkstream;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                    nodedesignatedata.createdate = "待定";
                    nodedesignatedata.creatdept = lastNodeDept;
                    nodedesignatedata.createuser = lastNodeUser;
                    nodedesignatedata.status = "正在处理...";
                    nodedesignatedata.prevnode = lastprevworkstream;
                    nodelist.Add(nodedesignatedata);
                    sinfo.NodeDesignateData = nodelist;
                    nodes.setInfo = sinfo;
                }
                #endregion

                nlist.Add(nodes);
            }
            #endregion




            #endregion

            #region 创建lines对象
            for (int i = 0; i < lineDt.Rows.Count; i++)
            {
                lines lines = new lines();
                lines.alt = true;
                lines.id = lineDt.Rows[i]["id"].ToString();
                lines.from = lineDt.Rows[i]["activityid"].ToString();
                lines.to = lineDt.Rows[i]["toactivityid"].ToString();
                lines.name = "";
                lines.type = "sl";
                llist.Add(lines);
            }
            #endregion

            flow.nodes = nlist;
            flow.lines = llist;

            return flow;
        }
        #endregion

        #region 获取工作计划流程图对象
        /// <summary>
        /// 获取部门工作计划流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetDepartPlanApplyActionList(string keyValue)
        {

            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            string sql = string.Empty;
            string sqlline = string.Empty;
            string sqlnode = string.Empty;

            #region  获取基本数据
            //获取基本数据
            sql = string.Format(@"select a.flowstate from hrs_planapply a where a.id ='{0}'", keyValue);

            var hidDt = this.BaseRepository().FindTable(sql);

            string flowstate = string.Empty;

            if (hidDt.Rows.Count == 1)
            {
                flowstate = hidDt.Rows[0]["flowstate"].ToString();
            }
            #endregion

            Flow flow = new Flow();
            flow.title = "部门工作计划审核（批）流程图";
            flow.initNum = 22;

            #region 创建nodes对象

            sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where processid='998ceb5a-2957-4d3f-a66d-585ceb330653' order by autoid ";  //nodes;

            sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a
                    left join sys_wftbactivity b on b.id = a.activityid
                    left join sys_wftbactivity c on c.id = a.toactivityid where remark='2' and a.processid='998ceb5a-2957-4d3f-a66d-585ceb330653'";


            var nodeDt = this.BaseRepository().FindTable(sqlnode);

            var lineDt = this.BaseRepository().FindTable(sqlline);

            var activityDt = GetWorkFlowDetail(keyValue, null);  //获取活动的当前实例历史流程记录 
            //获取流程节点下所有的
            string allworkstream = string.Empty; //所有
            string preallworkstream = string.Empty;//所有
            string lastworkstream = string.Empty; //获取最后一个流程
            string lastworknodeid = string.Empty; ///获取最后一个流程对应的流程id
            string lastprevworkstream = string.Empty; //获取最后一个流程的上一节点
            int totalrow = activityDt.Rows.Count;
            foreach (DataRow ativityRow in activityDt.Rows)
            {
                allworkstream += ativityRow["prevnode"].ToString() + ",";
                preallworkstream += ativityRow["curnode"].ToString() + ",";
            }
            lastworknodeid = activityDt.Rows[totalrow - 1]["curnodeid"].ToString();
            lastworkstream = activityDt.Rows[totalrow - 1]["curnode"].ToString();
            lastprevworkstream = activityDt.Rows[totalrow - 1]["prevnode"].ToString();

            #region 构建nodes对象
            for (int i = 0; i < nodeDt.Rows.Count; i++)
            {
                //流程相同情况下，保证流程id是流程节点最后一个。
                if (nodeDt.Rows[i]["name"].ToString() == flowstate && nodeDt.Rows[i]["id"].ToString() == lastworknodeid)
                {
                    flow.activeID = nodeDt.Rows[i]["id"].ToString();
                }
                nodes nodes = new nodes();
                nodes.alt = true;
                nodes.isclick = false;
                nodes.css = "";
                nodes.id = nodeDt.Rows[i]["id"].ToString(); //主键
                nodes.img = "";
                nodes.name = nodeDt.Rows[i]["name"].ToString();
                if (nodes.name == "上报计划")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 140;
                    nodes.top = 180;
                    nodes.width = 152;
                }
                else if (nodes.name == "1级审核")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 357;
                    nodes.top = 61;
                    nodes.width = 152;
                }
                else if (nodes.name == "2级审核")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 357;
                    nodes.top = 179;
                    nodes.width = 152;
                }
                else if (nodes.name == "审批")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 357;
                    nodes.top = 291;
                    nodes.width = 152;
                }
                else if (nodes.name == "结束")
                {
                    nodes.type = "endround";
                    nodes.height = 67;
                    nodes.left = 581;
                    nodes.top = 181;
                    nodes.width = 152;
                }

                //已经处理的部分(针对特殊的整改结束)
                if (nodes.name == "结束" && preallworkstream.Contains("结束"))
                {
                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 1;
                    sinfo.NodeName = lastworkstream;
                    nodes.setInfo = sinfo;
                }

                //已经处理的部分
                #region  已经处理的部分
                if (allworkstream.Contains(nodeDt.Rows[i]["name"].ToString()) && !allworkstream.Contains("无"))
                {
                    var activityData = GetWorkFlowDetail(keyValue, nodeDt.Rows[i]["name"].ToString(), nodeDt.Rows[i]["id"].ToString());  //获取活动的当前实例历史流程记录

                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 1;
                    sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    foreach (DataRow adRow in activityData.Rows)
                    {
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = adRow["createdate"].ToString();
                        nodedesignatedata.creatdept = adRow["creatdept"].ToString();
                        nodedesignatedata.createuser = adRow["createuser"].ToString();
                        nodedesignatedata.status = adRow["status"].ToString();
                        nodedesignatedata.prevnode = adRow["prevnode"].ToString();
                        nodelist.Add(nodedesignatedata);
                    }
                    sinfo.NodeDesignateData = nodelist;

                    nodes.setInfo = sinfo;
                }
                else
                {
                    string sqlStr = string.Format(@"select a.id,b.id  activityid,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join (
                                          select departmentid,encode, fullname from base_department 
                                          union 
                                          select organizeid as departmentid,encode ,fullname from base_organize
                                        ) e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  and b.name = '{1}' and b.id ='{2}'   order by a.autoid", keyValue, nodeDt.Rows[i]["name"].ToString(), nodeDt.Rows[i]["id"].ToString());

                    DataTable strDt = this.BaseRepository().FindTable(sqlStr);

                    if (strDt.Rows.Count > 0)
                    {
                        setInfo sinfo = new setInfo();
                        sinfo.Taged = 1;
                        sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        foreach (DataRow adRow in strDt.Rows)
                        {
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = adRow["createdate"].ToString();
                            nodedesignatedata.creatdept = adRow["fullname"].ToString();
                            nodedesignatedata.createuser = adRow["realname"].ToString();
                            nodedesignatedata.status = "已处理";
                            string sqlStrTemp = string.Format(@"select a.id,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join (
                                          select departmentid,encode, fullname from base_department 
                                          union 
                                          select organizeid as departmentid,encode ,fullname from base_organize
                                        ) e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  and c.name = '{1}' and a.autoid < {2}  order by a.autoid", keyValue, adRow["fromactivityname"].ToString(), adRow["autoid"].ToString());

                            DataTable strDtTemp = this.BaseRepository().FindTable(sqlStrTemp);

                            if (strDtTemp.Rows.Count > 0)
                            {
                                int lastLength = strDtTemp.Rows.Count - 1; //取最后一条
                                nodedesignatedata.prevnode = string.IsNullOrEmpty(strDtTemp.Rows[lastLength]["fromactivityname"].ToString()) ? "无" : strDtTemp.Rows[lastLength]["fromactivityname"].ToString();
                            }
                            nodelist.Add(nodedesignatedata);
                        }
                        sinfo.NodeDesignateData = nodelist;

                        nodes.setInfo = sinfo;
                    }
                }
                #endregion

                #region 正在处理的部分
                if (lastworkstream != "结束" && nodeDt.Rows[i]["name"].ToString() == lastworkstream && nodeDt.Rows[i]["id"].ToString() == lastworknodeid)
                {
                    string[] lastuser = activityDt.Rows[totalrow - 1]["nextuser"].ToString().Replace("$", "").ToString().Split(',');
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
                                          union 
                                          select organizeid as departmentid,encode ,fullname from base_organize
                                        ) b on  a.departmentcode = b.encode where  a.account in ({0})", newuserStr);
                    var newDt = this.BaseRepository().FindTable(newSql);
                    string lastNodeUser = ",";
                    string lastNodeDept = ",";
                    foreach (DataRow lastNodeRow in newDt.Rows)
                    {
                        lastNodeUser += lastNodeRow["realname"].ToString() + ",";
                        string tempDept = "," + lastNodeRow["fullname"].ToString() + ",";
                        if (!lastNodeDept.Contains(tempDept))
                        {
                            lastNodeDept += lastNodeRow["fullname"].ToString() + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(lastNodeUser) && lastNodeUser.Length > 2)
                    {
                        lastNodeUser = lastNodeUser.Substring(1, lastNodeUser.Length - 2).ToString();
                    }
                    if (!string.IsNullOrEmpty(lastNodeDept) && lastNodeDept.Length > 2)
                    {
                        lastNodeDept = lastNodeDept.Substring(1, lastNodeDept.Length - 2).ToString();
                    }
                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 0;
                    sinfo.NodeName = lastworkstream;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                    nodedesignatedata.createdate = "待定";
                    nodedesignatedata.creatdept = lastNodeDept;
                    nodedesignatedata.createuser = lastNodeUser;
                    nodedesignatedata.status = "正在处理...";
                    nodedesignatedata.prevnode = lastprevworkstream;
                    nodelist.Add(nodedesignatedata);
                    sinfo.NodeDesignateData = nodelist;
                    nodes.setInfo = sinfo;
                }
                #endregion

                nlist.Add(nodes);
            }
            #endregion




            #endregion

            #region 创建lines对象
            for (int i = 0; i < lineDt.Rows.Count; i++)
            {
                lines lines = new lines();
                lines.alt = true;
                lines.id = lineDt.Rows[i]["id"].ToString();
                lines.from = lineDt.Rows[i]["activityid"].ToString();
                lines.to = lineDt.Rows[i]["toactivityid"].ToString();
                lines.name = "";
                lines.type = "sl";
                llist.Add(lines);
            }
            #endregion

            flow.nodes = nlist;
            flow.lines = llist;

            return flow;
        }
        /// <summary>
        /// 获取个人工作计划流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetPersonPlanApplyActionList(string keyValue)
        {

            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            string sql = string.Empty;
            string sqlline = string.Empty;
            string sqlnode = string.Empty;

            #region  获取基本数据
            //获取基本数据
            sql = string.Format(@"select a.flowstate from hrs_planapply a where a.id ='{0}'", keyValue);

            var hidDt = this.BaseRepository().FindTable(sql);

            string flowstate = string.Empty;

            if (hidDt.Rows.Count == 1)
            {
                flowstate = hidDt.Rows[0]["flowstate"].ToString();
            }
            #endregion

            Flow flow = new Flow();
            flow.title = "个人工作计划审核（批）流程图";
            flow.initNum = 22;

            #region 创建nodes对象

            sqlnode = @"select id,name,autoid,kind from sys_wftbactivity  where processid='2fdb8273-a648-45c3-8c12-4b1ac331f653' order by autoid ";  //nodes;

            sqlline = @"select a.id,a.activityid,a.toactivityid,a.remark, b.name as fromname ,b.autoid as fromautoid,c.name as toname ,c.autoid as toautoid from sys_wftbcondition a
                    left join sys_wftbactivity b on b.id = a.activityid
                    left join sys_wftbactivity c on c.id = a.toactivityid where remark='2' and a.processid='2fdb8273-a648-45c3-8c12-4b1ac331f653'";


            var nodeDt = this.BaseRepository().FindTable(sqlnode);

            var lineDt = this.BaseRepository().FindTable(sqlline);

            var activityDt = GetWorkFlowDetail(keyValue, null);  //获取活动的当前实例历史流程记录 
            //获取流程节点下所有的
            string allworkstream = string.Empty; //所有
            string preallworkstream = string.Empty;//所有
            string lastworkstream = string.Empty; //获取最后一个流程
            string lastworknodeid = string.Empty; ///获取最后一个流程对应的流程id
            string lastprevworkstream = string.Empty; //获取最后一个流程的上一节点
            int totalrow = activityDt.Rows.Count;
            foreach (DataRow ativityRow in activityDt.Rows)
            {
                allworkstream += ativityRow["prevnode"].ToString() + ",";
                preallworkstream += ativityRow["curnode"].ToString() + ",";
            }
            lastworknodeid = activityDt.Rows[totalrow - 1]["curnodeid"].ToString();
            lastworkstream = activityDt.Rows[totalrow - 1]["curnode"].ToString();
            lastprevworkstream = activityDt.Rows[totalrow - 1]["prevnode"].ToString();

            #region 构建nodes对象
            for (int i = 0; i < nodeDt.Rows.Count; i++)
            {
                //流程相同情况下，保证流程id是流程节点最后一个。
                if (nodeDt.Rows[i]["name"].ToString() == flowstate && nodeDt.Rows[i]["id"].ToString() == lastworknodeid)
                {
                    flow.activeID = nodeDt.Rows[i]["id"].ToString();
                }
                nodes nodes = new nodes();
                nodes.alt = true;
                nodes.isclick = false;
                nodes.css = "";
                nodes.id = nodeDt.Rows[i]["id"].ToString(); //主键
                nodes.img = "";
                nodes.name = nodeDt.Rows[i]["name"].ToString();
                if (nodes.name == "上报计划")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 380;
                    nodes.top = 41;
                    nodes.width = 152;
                }
                else if (nodes.name == "上级领导审批")
                {
                    nodes.type = "stepnode";
                    nodes.height = 67;
                    nodes.left = 380;
                    nodes.top = 167;
                    nodes.width = 152;
                }
                else if (nodes.name == "结束")
                {
                    nodes.type = "endround";
                    nodes.height = 67;
                    nodes.left = 380;
                    nodes.top = 293;
                    nodes.width = 152;
                }

                //已经处理的部分(针对特殊的整改结束)
                if (nodes.name == "结束" && preallworkstream.Contains("结束"))
                {
                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 1;
                    sinfo.NodeName = lastworkstream;
                    nodes.setInfo = sinfo;
                }

                //已经处理的部分
                #region  已经处理的部分
                if (allworkstream.Contains(nodeDt.Rows[i]["name"].ToString()) && !allworkstream.Contains("无"))
                {
                    var activityData = GetWorkFlowDetail(keyValue, nodeDt.Rows[i]["name"].ToString(), nodeDt.Rows[i]["id"].ToString());  //获取活动的当前实例历史流程记录

                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 1;
                    sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    foreach (DataRow adRow in activityData.Rows)
                    {
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = adRow["createdate"].ToString();
                        nodedesignatedata.creatdept = adRow["creatdept"].ToString();
                        nodedesignatedata.createuser = adRow["createuser"].ToString();
                        nodedesignatedata.status = adRow["status"].ToString();
                        nodedesignatedata.prevnode = adRow["prevnode"].ToString();
                        nodelist.Add(nodedesignatedata);
                    }
                    sinfo.NodeDesignateData = nodelist;

                    nodes.setInfo = sinfo;
                }
                else
                {
                    string sqlStr = string.Format(@"select a.id,b.id  activityid,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join (
                                          select departmentid,encode, fullname from base_department 
                                          union 
                                          select organizeid as departmentid,encode ,fullname from base_organize
                                        ) e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  and b.name = '{1}' and b.id ='{2}'   order by a.autoid", keyValue, nodeDt.Rows[i]["name"].ToString(), nodeDt.Rows[i]["id"].ToString());

                    DataTable strDt = this.BaseRepository().FindTable(sqlStr);

                    if (strDt.Rows.Count > 0)
                    {
                        setInfo sinfo = new setInfo();
                        sinfo.Taged = 1;
                        sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        foreach (DataRow adRow in strDt.Rows)
                        {
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = adRow["createdate"].ToString();
                            nodedesignatedata.creatdept = adRow["fullname"].ToString();
                            nodedesignatedata.createuser = adRow["realname"].ToString();
                            nodedesignatedata.status = "已处理";
                            string sqlStrTemp = string.Format(@"select a.id,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join (
                                          select departmentid,encode, fullname from base_department 
                                          union 
                                          select organizeid as departmentid,encode ,fullname from base_organize
                                        ) e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  and c.name = '{1}' and a.autoid < {2}  order by a.autoid", keyValue, adRow["fromactivityname"].ToString(), adRow["autoid"].ToString());

                            DataTable strDtTemp = this.BaseRepository().FindTable(sqlStrTemp);

                            if (strDtTemp.Rows.Count > 0)
                            {
                                int lastLength = strDtTemp.Rows.Count - 1; //取最后一条
                                nodedesignatedata.prevnode = string.IsNullOrEmpty(strDtTemp.Rows[lastLength]["fromactivityname"].ToString()) ? "无" : strDtTemp.Rows[lastLength]["fromactivityname"].ToString();
                            }
                            nodelist.Add(nodedesignatedata);
                        }
                        sinfo.NodeDesignateData = nodelist;

                        nodes.setInfo = sinfo;
                    }
                }
                #endregion

                #region 正在处理的部分
                if (lastworkstream != "结束" && nodeDt.Rows[i]["name"].ToString() == lastworkstream && nodeDt.Rows[i]["id"].ToString() == lastworknodeid)
                {
                    string[] lastuser = activityDt.Rows[totalrow - 1]["nextuser"].ToString().Replace("$", "").ToString().Split(',');
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
                                          union 
                                          select organizeid as departmentid,encode ,fullname from base_organize
                                        ) b on  a.departmentcode = b.encode where  a.account in ({0})", newuserStr);
                    var newDt = this.BaseRepository().FindTable(newSql);
                    string lastNodeUser = ",";
                    string lastNodeDept = ",";
                    foreach (DataRow lastNodeRow in newDt.Rows)
                    {
                        lastNodeUser += lastNodeRow["realname"].ToString() + ",";
                        string tempDept = "," + lastNodeRow["fullname"].ToString() + ",";
                        if (!lastNodeDept.Contains(tempDept))
                        {
                            lastNodeDept += lastNodeRow["fullname"].ToString() + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(lastNodeUser) && lastNodeUser.Length > 2)
                    {
                        lastNodeUser = lastNodeUser.Substring(1, lastNodeUser.Length - 2).ToString();
                    }
                    if (!string.IsNullOrEmpty(lastNodeDept) && lastNodeDept.Length > 2)
                    {
                        lastNodeDept = lastNodeDept.Substring(1, lastNodeDept.Length - 2).ToString();
                    }
                    setInfo sinfo = new setInfo();
                    sinfo.Taged = 0;
                    sinfo.NodeName = lastworkstream;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();
                    nodedesignatedata.createdate = "待定";
                    nodedesignatedata.creatdept = lastNodeDept;
                    nodedesignatedata.createuser = lastNodeUser;
                    nodedesignatedata.status = "正在处理...";
                    nodedesignatedata.prevnode = lastprevworkstream;
                    nodelist.Add(nodedesignatedata);
                    sinfo.NodeDesignateData = nodelist;
                    nodes.setInfo = sinfo;
                }
                #endregion

                nlist.Add(nodes);
            }
            #endregion




            #endregion

            #region 创建lines对象
            for (int i = 0; i < lineDt.Rows.Count; i++)
            {
                lines lines = new lines();
                lines.alt = true;
                lines.id = lineDt.Rows[i]["id"].ToString();
                lines.from = lineDt.Rows[i]["activityid"].ToString();
                lines.to = lineDt.Rows[i]["toactivityid"].ToString();
                lines.name = "";
                lines.type = "sl";
                llist.Add(lines);
            }
            #endregion

            flow.nodes = nlist;
            flow.lines = llist;

            return flow;
        }
        #endregion

        #region 获取流程导向图
        /// <summary>
        ///获取流程导向图
        /// </summary>
        /// <param name="keyValue">主键id</param>
        /// <param name="mode">标记是隐患、违章</param>
        /// <returns></returns>
        public DataTable QueryWorkFlowMapForApp(string keyValue, string mode)
        {
            string sql = string.Empty;

            if (mode == "Hidden")
            {
                sql = string.Format(@"select  a.id, a.createdate,a.createuser,a.operdate, e.realname username,e.userid, f.fullname deptname, a.participant,a.participantname,c.name fromname,d.name toname,a.autoid,g.contents from sys_wftbinstancedetail  a
                                        left join sys_wftbinstance  b on a.instanceid = b.id 
                                        left join sys_wftbactivity  c on a.fromactivityid = c.id
                                        left join sys_wftbactivity  d on a.toactivityid = d.id
                                        left join base_user e on a.createuser = e.account
                                        left join base_department  f on e.departmentid = f.departmentid 
                                        left join (
                                                select a.id, b.acceptidea contents from (
                                                select  row_number() over (order by a.operdate) numbers , a.id  from sys_wftbinstancedetail  a
                                                left join sys_wftbinstance  b on a.instanceid = b.id 
                                                left join sys_wftbactivity  c on a.fromactivityid = c.id
                                                where b.objectid ='{0}'   and c.name ='隐患验收' order by a.autoid
                                            ) a 
                                            left join (
                                                select row_number() over (order by a.createdate) numbers ,a.acceptidea  from bis_htacceptinfo a 
                                                left join bis_htbaseinfo b on a.hidcode = b.hidcode where b.id ='{0}'
                                            ) b on a.numbers = b.numbers
                                            union
                                                select a.id, b.approvalreason contents from (
                                                select  row_number() over (order by a.operdate) numbers , a.id  from sys_wftbinstancedetail  a
                                                left join sys_wftbinstance  b on a.instanceid = b.id 
                                                left join sys_wftbactivity  c on a.fromactivityid = c.id
                                                where b.objectid ='{0}'   and c.name ='隐患评估' order by a.autoid
                                            ) a 
                                            left join (
                                                select row_number() over (order by a.createdate) numbers ,a.approvalreason   from bis_htapproval a 
                                                left join bis_htbaseinfo b on a.hidcode = b.hidcode where b.id ='{0}'
                                            ) b on a.numbers = b.numbers
                                       ) g on a.id = g.id  where b.objectid ='{0}' order by a.autoid", keyValue);
            }
            else if (mode == "Lllegal") //反违章
            {
                sql = string.Format(@"select  a.id, a.createdate,a.createuser,a.operdate, e.realname username,e.userid, f.fullname deptname, a.participant,a.participantname,c.name fromname,d.name toname,a.autoid,g.contents from sys_wftbinstancedetail  a
                                        left join sys_wftbinstance  b on a.instanceid = b.id 
                                        left join sys_wftbactivity  c on a.fromactivityid = c.id
                                        left join sys_wftbactivity  d on a.toactivityid = d.id
                                        left join base_user e on a.createuser = e.account
                                        left join base_department  f on e.departmentid = f.departmentid 
                                        left join (
                                            select a.id, b.acceptmind contents from (
                                                select  row_number() over (order by a.operdate) numbers , a.id  from sys_wftbinstancedetail  a
                                                left join sys_wftbinstance  b on a.instanceid = b.id 
                                                left join sys_wftbactivity  c on a.fromactivityid = c.id
                                                where b.objectid ='{0}' and c.name ='违章验收' order by a.autoid
                                            ) a 
                                            left join (
                                                select row_number() over (order by a.createdate) numbers ,a.acceptmind  from bis_lllegalaccept a 
                                                left join bis_lllegalregister b on a.lllegalid = b.id where b.id ='{0}'
                                            ) b on a.numbers = b.numbers
                                            union
                                                select a.id, b.approvereason contents from (
                                                select  row_number() over (order by a.operdate) numbers , a.id  from sys_wftbinstancedetail  a
                                                left join sys_wftbinstance  b on a.instanceid = b.id  
                                                left join sys_wftbactivity  c on a.fromactivityid = c.id
                                                where b.objectid ='{0}' and c.name in ('违章核准','违章审核') order by a.autoid
                                            ) a 
                                            left join (
                                                select row_number() over (order by a.createdate) numbers ,a.approvereason  from bis_lllegalapprove a 
                                                left join bis_lllegalregister b on a.lllegalid = b.id where b.id ='{0}'
                                            ) b on a.numbers = b.numbers
                                        ) g on a.id = g.id  where b.objectid ='{0}' order by a.autoid", keyValue);
            }
            else if (mode == "Question") //问题管理
            {
                sql = string.Format(@"select  a.id, a.createdate,a.createuser,a.operdate, e.realname username,e.userid, f.fullname deptname, a.participant,a.participantname,c.name fromname,d.name toname,a.autoid,g.contents from sys_wftbinstancedetail  a
                                        left join sys_wftbinstance  b on a.instanceid = b.id 
                                        left join sys_wftbactivity  c on a.fromactivityid = c.id
                                        left join sys_wftbactivity  d on a.toactivityid = d.id
                                        left join base_user e on a.createuser = e.account
                                        left join base_department  f on e.departmentid = f.departmentid 
                                        left join (
                                            select a.id, b.verifyopinion contents from (
                                                select  row_number() over (order by a.operdate) numbers , a.id  from sys_wftbinstancedetail  a
                                                left join sys_wftbinstance  b on a.instanceid = b.id 
                                                left join sys_wftbactivity  c on a.fromactivityid = c.id
                                                where b.objectid ='{0}' and c.name ='问题验证' order by a.autoid
                                            ) a 
                                            left join (
                                                select row_number() over (order by a.createdate) numbers ,a.verifyopinion  from bis_questionverify a 
                                                left join bis_questioninfo b on a.questionid = b.id where b.id ='{0}'
                                            ) b on a.numbers = b.numbers
                                        ) g on a.id = g.id  where b.objectid ='{0}' order by a.autoid", keyValue);
            }
            DataTable dt = this.BaseRepository().FindTable(sql);

            return dt;
        }
        #endregion

        #region 获取公共的流程图对象
        /// <summary>
        /// 获取隐患的流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetCommonFlow(Flow flow, string keyValue)
        {
            //当前用户
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            try
            {
                List<nodes> nlist = new List<nodes>();
                List<lines> llist = new List<lines>();
                string sql = string.Empty;
                string sqlline = string.Empty;
                string sqlnode = string.Empty;

                //源对象
                string sourcenode = string.Format(@"select * from sys_wftbactivity where processid = ( select distinct processid  from sys_wftbinstance where objectid ='{0}')  order by autoid ", keyValue);

                var sourceDt = this.BaseRepository().FindTable(sourcenode);

                var startnodeDt = sourceDt.Select(" formname='startround'").FirstOrDefault(); //开始第一个节点

                string startnodename = startnodeDt["name"].ToString(); //开始第一个节点 

                var endnodeDt = sourceDt.Select(" formname='endround'").FirstOrDefault(); //最后一个节点

                string lastnodename = endnodeDt["name"].ToString(); //最后一个节点名称

                //当前业务所处流程节点
                string currentnode = string.Format(@" select c.* from sys_wftbinstance  a left join sys_wftbinstancedetail b on a.currentdetailid = b.id  left join sys_wftbactivity c on b.toactivityid = c.id 
                                                   where a.objectid ='{0}'", keyValue);

                var currentnodeDt = this.BaseRepository().FindTable(currentnode); //当前业务流程对象

                #region 创建nodes对象
                sqlnode = string.Format(@"select id,name,autoid,kind from sys_wftbactivity  where id in ( select b.toactivityid from sys_wftbinstance  a 
                                          left join sys_wftbinstancedetail b on a.id = b.instanceid 
                                          where a.objectid ='{0}') ", keyValue);

                sqlline = string.Format(@"select c.id,c.activityid,c.toactivityid,c.remark from sys_wftbinstance  a 
                                          left join sys_wftbinstancedetail b on a.id = b.instanceid 
                                          left join sys_wftbcondition c on b.fromactivityid = c.activityid  and b.toactivityid = c.toactivityid
                                          where a.objectid ='{0}'  and c.id is not null  ", keyValue);

                var nodeDt = this.BaseRepository().FindTable(sqlnode);

                var lineDt = this.BaseRepository().FindTable(sqlline);

                var activityDt = GetWorkFlowDetail(keyValue, null);  //获取活动的当前实例历史流程记录
                //获取流程节点下所有的
                string allworkstream = string.Empty; //所有
                string preallworkstream = string.Empty; //所有
                string lastworkstream = string.Empty; //获取最后一个流程
                string lastprevworkstream = string.Empty; //获取最后一个流程的上一节点
                int totalrow = activityDt.Rows.Count;
                foreach (DataRow ativityRow in activityDt.Rows)
                {
                    allworkstream += ativityRow["prevnode"].ToString() + ",";
                    preallworkstream += ativityRow["curnode"].ToString() + ",";
                }
                lastworkstream = activityDt.Rows[totalrow - 1]["curnode"].ToString();
                lastprevworkstream = activityDt.Rows[totalrow - 1]["prevnode"].ToString();

                #region 构建nodes对象
                for (int i = 0; i < nodeDt.Rows.Count; i++)
                {
                    if (nodeDt.Rows[i]["name"].ToString() == currentnodeDt.Rows[0]["name"].ToString() && nodeDt.Rows[i]["name"].ToString() != lastnodename)
                    {
                        flow.activeID = nodeDt.Rows[i]["id"].ToString();
                    }

                    nodes nodes = new nodes();
                    nodes.alt = true;
                    nodes.isclick = false;
                    nodes.css = "";
                    nodes.id = nodeDt.Rows[i]["id"].ToString(); //主键
                    nodes.img = "";
                    nodes.name = nodeDt.Rows[i]["name"].ToString();
                    var newNode = sourceDt.Select(string.Format(" name='{0}'", nodes.name)).FirstOrDefault();
                    nodes.type = newNode["formname"].ToString();
                    nodes.height = int.Parse(newNode["formheight"].ToString());
                    nodes.left = int.Parse(newNode["graphleft"].ToString());
                    nodes.top = int.Parse(newNode["graphtop"].ToString());
                    nodes.width = int.Parse(newNode["formwidth"].ToString());



                    //已经处理的部分(针对特殊的整改结束)
                    if (nodeDt.Rows[i]["name"].ToString() == lastnodename && preallworkstream.Contains(lastnodename))
                    {
                        setInfo sinfo = new setInfo();
                        sinfo.Taged = 1;
                        sinfo.NodeName = lastworkstream;
                        nodes.setInfo = sinfo;
                    }


                    #region  已经处理的部分  最后一个流程节点是整改结束
                    if (allworkstream.Contains(nodeDt.Rows[i]["name"].ToString()) && !allworkstream.Contains("无"))
                    {

                        var activityData = GetWorkFlowDetail(keyValue, nodeDt.Rows[i]["name"].ToString());  //获取活动的当前实例历史流程记录

                        setInfo sinfo = new setInfo();
                        sinfo.Taged = 1;
                        sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        foreach (DataRow adRow in activityData.Rows)
                        {
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = adRow["createdate"].ToString();
                            nodedesignatedata.creatdept = adRow["creatdept"].ToString();
                            nodedesignatedata.createuser = adRow["createuser"].ToString();
                            nodedesignatedata.status = adRow["status"].ToString();
                            nodedesignatedata.prevnode = adRow["prevnode"].ToString();
                            nodelist.Add(nodedesignatedata);
                        }
                        sinfo.NodeDesignateData = nodelist;

                        nodes.setInfo = sinfo;
                    }
                    else
                    {
                        string sqlStr = string.Format(@"select a.id,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join base_department e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  and b.name = '{1}'   order by a.autoid", keyValue, nodeDt.Rows[i]["name"].ToString());


                        DataTable strDt = this.BaseRepository().FindTable(sqlStr);



                        if (strDt.Rows.Count > 0)
                        {
                            setInfo sinfo = new setInfo();
                            sinfo.Taged = 1;
                            sinfo.NodeName = nodeDt.Rows[i]["name"].ToString();
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            foreach (DataRow adRow in strDt.Rows)
                            {
                                NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                nodedesignatedata.createdate = adRow["createdate"].ToString();
                                nodedesignatedata.creatdept = adRow["fullname"].ToString();
                                nodedesignatedata.createuser = adRow["realname"].ToString();
                                nodedesignatedata.status = "已处理";
                                string sqlStrTemp = string.Format(@"select a.id,a.autoid,a.createdate,a.operdate, a.createuser,d.realname,e.fullname, a.operuser, a.createuserid,a.participant,a.instanceid,b.name as fromactivityname,
                                        c.name as toactivityname from sys_wftbinstancedetail  a
                                        left join  sys_wftbactivity b on a.fromactivityid = b.id 
                                        left join sys_wftbactivity c on a.toactivityid = c.id 
                                        left join base_user d on a.createuser = d.account 
                                        left join base_department e on d.departmentcode = e.encode               
                                        left join  sys_wftbinstance f on a.instanceid = f.id 
                                        where f.objectid ='{0}'  and c.name = '{1}' and a.autoid < {2}  order by a.autoid", keyValue, adRow["fromactivityname"].ToString(), adRow["autoid"].ToString());

                                DataTable strDtTemp = this.BaseRepository().FindTable(sqlStrTemp);

                                if (strDtTemp.Rows.Count > 0)
                                {
                                    int lastLength = strDtTemp.Rows.Count - 1; //取最后一条
                                    nodedesignatedata.prevnode = string.IsNullOrEmpty(strDtTemp.Rows[lastLength]["fromactivityname"].ToString()) ? "无" : strDtTemp.Rows[lastLength]["fromactivityname"].ToString();
                                }


                                nodelist.Add(nodedesignatedata);
                            }
                            sinfo.NodeDesignateData = nodelist;

                            nodes.setInfo = sinfo;
                        }
                    }
                    #endregion

                    #region 正在处理的部分
                    if (lastworkstream != startnodename && lastworkstream != lastnodename && nodeDt.Rows[i]["name"].ToString() == lastworkstream)
                    {
                        string[] lastuser = activityDt.Rows[totalrow - 1]["nextuser"].ToString().Replace("$", "").ToString().Split(',');
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
                                        left join base_department  b on  a.departmentcode = b.encode where  a.account in ({0})", newuserStr);
                        var newDt = this.BaseRepository().FindTable(newSql);
                        string lastNodeUser = ",";
                        string lastNodeDept = ",";
                        foreach (DataRow lastNodeRow in newDt.Rows)
                        {
                            lastNodeUser += lastNodeRow["realname"].ToString() + ",";
                            string tempDept = "," + lastNodeRow["fullname"].ToString() + ",";
                            if (!lastNodeDept.Contains(tempDept))
                            {
                                lastNodeDept += lastNodeRow["fullname"].ToString() + ",";
                            }
                        }
                        if (!string.IsNullOrEmpty(lastNodeUser) && lastNodeUser != ",")
                        {
                            lastNodeUser = lastNodeUser.Substring(1, lastNodeUser.Length - 2).ToString();
                        }
                        else
                        {
                            lastNodeUser = "";
                        }
                        if (!string.IsNullOrEmpty(lastNodeDept) && lastNodeDept != ",")
                        {
                            lastNodeDept = lastNodeDept.Substring(1, lastNodeDept.Length - 2).ToString();
                        }
                        else
                        {
                            lastNodeDept = "";
                        }
                        setInfo sinfo = new setInfo();
                        sinfo.Taged = 0;
                        sinfo.NodeName = lastworkstream;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = "待定";
                        nodedesignatedata.creatdept = lastNodeDept;
                        nodedesignatedata.createuser = lastNodeUser;
                        nodedesignatedata.status = "正在处理...";
                        nodedesignatedata.prevnode = lastprevworkstream;
                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    #endregion

                    nlist.Add(nodes);
                }
                #endregion

                #endregion

                #region 创建lines对象
                for (int i = 0; i < lineDt.Rows.Count; i++)
                {
                    lines lines = new lines();
                    lines.alt = true;
                    lines.id = lineDt.Rows[i]["id"].ToString();
                    lines.from = lineDt.Rows[i]["activityid"].ToString();
                    lines.to = lineDt.Rows[i]["toactivityid"].ToString();
                    lines.name = "";
                    lines.type = "sl";
                    llist.Add(lines);
                }
                #endregion

                flow.nodes = nlist;
                flow.lines = llist;

                return flow;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
