using System;
using ERCHTMS.Entity.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.IService.PersonManage
{
    /// <summary>
    /// 描 述：人员证书
    /// </summary>
    public interface WorkRecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取人员的工作经历
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>返回列表</returns>
        IEnumerable<WorkRecordEntity> GetList(string userId);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        WorkRecordEntity GetEntity(string keyValue);
 
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, WorkRecordEntity entity);

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void NewSaveForm(string keyValue, WorkRecordEntity entity);
          /// <summary>
        /// 人员离场时写工作记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="deptId">部门Id</param>
        /// <returns></returns>
        int WriteWorkRecord(UserInfoEntity user, ERCHTMS.Code.Operator currUser);

        /// <summary>
        /// 人员操作换部门时写记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="deptId">部门Id</param>
        /// <returns></returns>
        int WriteChangeRecord(UserInfoEntity user, ERCHTMS.Code.Operator currUser);

        /// <summary>
        /// 修改离场原因则修改最近的工作记录结束时间
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        int EidtRecord(string userid, string time);

        #endregion

    }
}
