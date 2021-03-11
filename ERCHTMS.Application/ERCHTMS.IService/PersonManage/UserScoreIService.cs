using ERCHTMS.Entity.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.PersonManage
{
    /// <summary>
    /// 描 述：人员积分
    /// </summary>
    public interface UserScoreIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        IEnumerable<UserScoreEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        DataTable GetList(string userId);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        UserScoreEntity GetEntity(string keyValue);
         /// <summary>
        /// 存储过程分页查询
        /// </summary>
        /// <param name="pagination">分页条件</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageJsonList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取人员积分考核明细
        /// </summary>
        /// <param name="keyValue">记录Id</param>
        /// <returns></returns>
        object GetInfo(string keyValue);
         /// <summary>
        /// 获取用户指定年份的积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="year">年度</param>
        /// <returns></returns>
        decimal GetUserScore(string userId, string year);
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
        void SaveForm(string keyValue, UserScoreEntity entity);
        /// <summary>
        /// 批量保存积分考核记录
        /// </summary>
        /// <param name="list"></param>
        void Save(List<UserScoreEntity> list);
         /// <summary>
        /// 获取人员本年底积分和累计积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        string GetScoreInfo(string userId);
        #endregion
    }
}
