using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PersonManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.PersonManage
{
    public interface LeaveApproveIService
    {
        #region [获取数据]
        DataTable GetLeaveApproveList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取表单数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        LeaveApproveEntity GetEntity(string keyValue);

        /// <summary>
        /// 流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        Flow GetFlow(string keyValue);

        DataTable GetLeaveApproveData(string sql);

        /// <summary>
        /// 首页待办事项
        /// </summary>
        /// <param name="curUser"></param>
        /// <returns></returns>
        int GetDBSXNum(Operator curUser);
        #endregion

        #region [提交数据]
        /// <summary>
        /// 提交离场申请
        /// </summary>
        /// <param name="entity"></param>
        bool SaveForm(LeaveApproveEntity entity);

        /// <summary>
        /// 离场审批
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="aentity"></param>
        PushMessageData LeaveApprove(string keyValue, LeaveApproveEntity entity, AptitudeinvestigateauditEntity aentity);
        #endregion
    }
}
