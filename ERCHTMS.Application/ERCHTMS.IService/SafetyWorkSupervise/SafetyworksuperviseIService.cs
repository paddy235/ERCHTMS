using ERCHTMS.Entity.SafetyWorkSupervise;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.SafetyWorkSupervise
{
    /// <summary>
    /// 描 述：安全重点工作督办
    /// </summary>
    public interface SafetyworksuperviseIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafetyworksuperviseEntity> GetList(string queryJson);

        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafetyworksuperviseEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取实体/链表查
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DataTable GetEntityByT(string keyValue, string fid);
       /// <summary>
       /// 流程信息
       /// </summary>
       /// <param name="keyValue"></param>
       /// <param name="modulename"></param>
       /// <returns></returns>
        Flow GetFlow(string keyValue);
        int GetSuperviseNum(string userid, string type);
        /// <summary>
        /// 获取首页统计图
        /// </summary>
        /// <param name="keyValue">1表示上个月</param>
        /// <returns></returns>
        DataTable GetSuperviseStat(string keyValue);
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
        void SaveForm(string keyValue, SafetyworksuperviseEntity entity);
        #endregion
    }
}
