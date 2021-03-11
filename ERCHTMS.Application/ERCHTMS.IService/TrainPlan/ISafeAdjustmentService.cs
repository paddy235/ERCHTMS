using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.TrainPlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.TrainPlan
{
    public interface ISafeAdjustmentService
    {
        bool InsertAdjustment(SafeAdjustmentEntity entity);
        void UpdateAdjustment(SafeAdjustmentEntity entity);
        SafeAdjustmentEntity GetEntity(string keyValue);

        SafeAdjustmentEntity GetAdjustInfo(string keyValue);

        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="createdeptid">创建人部门ID</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid,out string curFlowId);

        /// <summary>
        /// 获取调整申请/审批记录
        /// </summary>
        /// <param name="measureId"></param>
        /// <returns></returns>
        DataTable GetAdjustList(string measureId);

        /// <summary>
        /// 删除调整信息
        /// </summary>
        /// <param name="measureId"></param>
        void DeleteAdjustment(string measureId);
    }
}
