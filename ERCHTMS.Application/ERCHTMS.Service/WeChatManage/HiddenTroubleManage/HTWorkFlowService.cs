using BSFramework.Data.Repository;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.BaseManage;
using System;
using System.Collections.Generic;
using System.Data;
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

        private IUserService userservice = new UserService();

        #region  创建流程实例
        /// <summary>
        /// 创建流程实例
        /// </summary>
        /// <param name="wfObj">隐患流程编码</param>
        /// <param name="objectID">实例ID</param>
        /// <param name="curUser">当前用户ID</param>
        /// <returns></returns>
        public bool CreateWorkFlowObj(string wfObj, string objectID, string curUser)
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
                                         FROMACTIVITYID, TOACTIVITYID, INSTANCEID, CREATEUSERID, CREATEUSERDEPTID, CREATEUSERDEPTCODE, TEMPUSERS)values('" + curWfNode + "', to_date('" +
                                          DateTime.Now.ToString() + "','yyyy-mm-dd hh24:mi:ss'), '" + uInfor.Account + "', '" + uInfor.Account + "', to_date('" + DateTime.Now.ToString() + "','yyyy-mm-dd hh24:mi:ss'), 0, '$" +
                                          uInfor.Account + "', '', '', '" + activtyID + "', '" + InstanceID + "', '" + uInfor.UserId + "', '" + uInfor.DepartmentId + "', '" + uInfor.DepartmentCode + "', '')";


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

        #region 流程提交 / 退回
        /// <summary>
        /// 流程提交  
        /// </summary>
        /// <param name="objectId">实例ID</param>
        /// <param name="participant">参与者</param>
        /// <param name="wfFlag">流程转向</param>
        /// <param name="curUser">当前用户ID</param>
        /// <returns></returns>
        public int SubmitWorkFlow(string objectId, string participant, string wfFlag, string curUser)
        {
            int isSuccess = -1;  //返回值  为0的时候表示当前用户无提交操作权限 ,为1的时候表示操作成功,为-2则数据操作失败

            try
            {
                //获取当前人部门
                UserInfoEntity uInfor = new UserInfoService().GetUserInfoEntity(curUser);

                //引用对象
                string instanceSql = string.Format(@" select t.*, t.rowid from sys_wftbinstance t where objectid ='{0}'", objectId);

                DataTable idr = this.BaseRepository().FindTable(instanceSql);

                string processId = string.Empty;  //流程处理ID 

                string currentDetailId = string.Empty;  //当前明细

                string instanceID = string.Empty; //InstanceID 

                string curActivity = string.Empty; //当前Activity 

                string nextActivity = string.Empty; //下一个Activity 

                string curParticipant = string.Empty; //当前节点权限下的提交人员

                string operatedUser = string.Empty; //操作人

                wfFlag = "@{wfFlag}==" + wfFlag;

                if (idr.Rows.Count == 1)
                {
                    instanceID = idr.Rows[0]["ID"].ToString();  // InstanceID  

                    processId = idr.Rows[0]["PROCESSID"].ToString();  //处理ID

                    currentDetailId = idr.Rows[0]["CURRENTDETAILID"].ToString(); //当前流程节点

                    operatedUser = idr.Rows[0]["OPERATEDUSER"].ToString();  //操作人及其历史操作人
                }

                //根据InstanceId获取详细的流程节点信息 ,取最新的一条，来确定当前的流程状态，同时通过当前的流程状态加推送条件找到下一条流程
                string idetailSql = string.Format(@"select t.*, t.rowid from sys_wftbinstancedetail t where t.instanceid ='{0}' order by  t.autoid desc", instanceID);

                DataTable detailIdr = this.BaseRepository().FindTable(idetailSql);
                //读取第一条
                if (detailIdr.Rows.Count > 0)
                {
                    curParticipant = detailIdr.Rows[0]["PARTICIPANT"].ToString(); //当前流程节点下能够提交的人员
                    curActivity = detailIdr.Rows[0]["TOACTIVITYID"].ToString();    //当前Activity
                }

                //推送到下一个流程
                string nextActivitySql = string.Format(@"select t.*, t.rowid from sys_wftbcondition t where activityid ='{0}' and expression ='{1}'", curActivity, wfFlag);

                DataTable nextIdr = this.BaseRepository().FindTable(nextActivitySql);
                //读取第一条
                if (nextIdr.Rows.Count == 1)
                {
                    nextActivity = nextIdr.Rows[0]["TOACTIVITYID"].ToString();    //下一个节点的Activity
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
                                                            CREATEUSERDEPTID, CREATEUSERDEPTCODE, TEMPUSERS)
                                                            values('{0}', to_date('{1}','yyyy-mm-dd hh24:mi:ss'), '{2}', '{3}', to_date('{4}','yyyy-mm-dd hh24:mi:ss'), {5}, '${6}', '{7}', '{8}', '{9}', '{10}', 
                                                            '{11}', '{12}','{13}', '{14}')", detailsID, DateTime.Now.ToString(), uInfor.Account, uInfor.Account,
                                                    DateTime.Now.ToString(), 0, participant, "", curActivity, nextActivity, instanceID, uInfor.UserId, uInfor.DepartmentId, uInfor.DepartmentCode, "");


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
            return isSuccess;
        }
        #endregion

        #region 流程提交，只提交实例不更改当前流程状态,即当前是隐患核准，提交后可能还是隐患核准，并且核准人员发生变化
        /// <summary>
        /// 流程提交，只更改实例不更改当前流程状态
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="participant"></param>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public bool SubmitWorkFlowNoChangeStatus(string objectId, string participant, string curUser)
        {

            //获取当前人部门
            UserInfoEntity uInfor = new UserInfoService().GetUserInfoEntity(curUser);

            bool isSuccess = false;

            string curentDetailID = string.Empty;

            string operateduser = string.Empty;

            string newDetailId = Guid.NewGuid().ToString();  //新的Guid

            int reVal = 0;

            string sql = string.Format(@"select currentdetailid ,operateduser from sys_wftbinstance  where objectid='{0}'", objectId);

            DataTable idt = this.BaseRepository().FindTable(sql);

            if (idt.Rows.Count == 1)
            {
                curentDetailID = idt.Rows[0]["currentdetailid"].ToString(); //获取当前流程节点实例ID
                operateduser = idt.Rows[0]["operateduser"].ToString(); //操作过的人员
            }

            string isql = string.Format(@"update sys_wftbinstancedetail set operdate = to_date('{0}','yyyy-mm-dd hh24:mi:ss'),operuser='{1}',participant='${2}' where id ='{3}'",
                DateTime.Now.ToString(), uInfor.Account, participant, curentDetailID);

            //string isql = string.Format(@"insert into sys_wftbinstancedetail(id,operdate,operuser,createuser,createdate,state, participant ,remark ,fromactivityid ,toactivityid,
            //instanceid, createuserid, createuserdeptid, createuserdeptcode)
            //select '{0}' as id ,to_date('{1}','yyyy-mm-dd hh24:mi:ss') as operdate ,'{2}' as operuser,'{2}' as createuser,
            //to_date('{1}','yyyy-mm-dd hh24:mi:ss') as createdate,state,'{3}' as participant ,remark ,fromactivityid ,toactivityid,
            //instanceid,'{4}' as createuserid,'{5}' as createuserdeptid,'{6}' as createuserdeptcode  
            //from sys_wftbinstancedetail where id ='{7}'", newDetailId, DateTime.Now.ToString(), uInfor.Account, participant,
            //uInfor.UserId, uInfor.DepartmentId, uInfor.DepartmentCode, curentDetailID);

            reVal = this.BaseRepository().ExecuteBySql(isql);

            if (reVal > 0)
            {
                operateduser = operateduser + uInfor.Account + "|";
                isql = string.Format(@"update sys_wftbinstance set operdate= to_date('{0}','yyyy-mm-dd hh24:mi:ss'),operuser ='{1}',operateduser='{2}' where objectid ='{3}'",
                                   DateTime.Now.ToString(), uInfor.Account, operateduser, objectId);

                reVal = this.BaseRepository().ExecuteBySql(isql);

                isSuccess = reVal > 0 ? true : false;
            }

            return isSuccess;
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
                tag = idr.Rows[0]["TAG"].ToString(); //当前流程节点
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
        public bool UpdateWorkStreamByObjectId(string objectId)
        {
            bool isValue = false;

            string tagName = QueryTagNameByCurrentWF(objectId);
            string sql = "update bis_htbaseinfo set workstream ='" + tagName + "' where id ='" + objectId + "'";

            int count = this.BaseRepository().ExecuteBySql(sql);
            if (count > 0)
            {
                isValue = true;
            }
            return isValue;
        }
        #endregion

        #region 更新业务流程及上报标识
        /// <summary>
        /// 更新业务流程及上报标识 QP
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="upFlag"></param>
        /// <returns></returns>
        //public bool UpdateWorkStreamAndUpFlagByObjectId(string objectId, string upFlag)
        //{
        //    bool isValue = false;

        //    string tagName = QueryTagNameByCurrentWF(objectId);
        //    if (upFlag == "-1") { upFlag = ""; }
        //    string sql = "update bis_htbaseinfo set workstream ='" + tagName + "',state ='" + upFlag + "' where id ='" + objectId + "'";

        //    int count = this.BaseRepository().ExecuteBySql(sql);
        //    if (count > 0)
        //    {
        //        isValue = true;
        //    }
        //    return isValue;
        //}
        #endregion

    }
}
