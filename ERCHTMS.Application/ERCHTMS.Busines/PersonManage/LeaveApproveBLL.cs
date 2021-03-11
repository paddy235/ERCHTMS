using BSFramework.Util.WebControl;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.PersonManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.PersonManage
{
    /// <summary>
    /// 外包人员离场审核
    /// </summary>
    public class LeaveApproveBLL
    {
        private LeaveApproveIService service = new LeaveApproveService();
        private IUserService userservice = new UserService();

        #region [获取数据]
        /// <summary>
        /// 离场审核列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetLeaveApproveList(Pagination pagination, string queryJson)
        {
            return service.GetLeaveApproveList(pagination, queryJson);
        }

        /// <summary>
        /// 获取表单数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public LeaveApproveEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue)
        {
            return service.GetFlow(keyValue);
        }


        public DataTable GetLeaveInfo(string userid)
        {
            string sql = string.Format("select leaveuserids as UserId,leaveusernames as UserName,to_char(LEAVETIME,'yyyy-MM-dd') as LEAVETIME,LEAVEREASON as DepartureReason from bis_leaveapprove where approvestate=0 and instr(leaveuserids,'{0}')>-1", userid);
            DataTable dt= service.GetLeaveApproveData(sql);
            foreach (DataRow row in dt.Rows)
            {
                string[] arrUserIds = row["UserId"].ToString().Split(',');
                string[] arrNames = row["UserName"].ToString().Split(',');
                int index = Array.IndexOf(arrUserIds, userid);
               
                row["UserId"] = arrUserIds[index];
                row["UserName"] = arrNames[index];
            }
            return dt;
        }

        /// <summary>
        /// 首页待办事项
        /// </summary>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public int GetDBSXNum(Operator curUser)
        {
            return service.GetDBSXNum(curUser);
        }
        #endregion

        #region [提交数据]
        /// <summary>
        /// 提交离场申请
        /// </summary>
        /// <param name="entity"></param>
        public bool SaveForm(LeaveApproveEntity entity)
        {
            bool flag= service.SaveForm(entity);
            if (flag)
            {
                userservice.UpdateUserLeaveState(entity.LeaveUserIds, "1");
            }
            return flag;
        }

        /// <summary>
        /// 离场审批
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="aentity"></param>
        public void LeaveApprove(string keyValue, LeaveApproveEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            try
            {
                PushMessageData pushdata = service.LeaveApprove(keyValue, entity, aentity);
                UserEntity userEntity = userservice.GetEntity(entity.CreateUserId);
                JPushApi.PushMessage(userEntity.Account, userEntity.RealName, pushdata.SendCode, "", "", pushdata.EntityId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
