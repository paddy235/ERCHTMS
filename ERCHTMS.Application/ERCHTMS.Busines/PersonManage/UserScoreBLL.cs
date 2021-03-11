using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.PersonManage
{
    /// <summary>
    /// 描 述：人员积分
    /// </summary>
    public class UserScoreBLL
    {
        private UserScoreIService service = new UserScoreService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<UserScoreEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetList(string userId)
        {
            return service.GetList(userId);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public UserScoreEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
         /// <summary>
        /// 存储过程分页查询
        /// </summary>
        /// <param name="pagination">分页条件</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageJsonList(Pagination pagination, string queryJson)
        {
            return service.GetPageJsonList(pagination, queryJson);
        }
        /// <summary>
        /// 获取人员积分考核明细
        /// </summary>
        /// <param name="keyValue">记录Id</param>
        /// <returns></returns>
        public object GetInfo(string keyValue)
        {
            return service.GetInfo(keyValue);
        }
         /// <summary>
        /// 获取用户指定年份的积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="year">年度</param>
        /// <returns></returns>
        public decimal GetUserScore(string userId,string year)
        {
            return service.GetUserScore(userId, year);
        }
         /// <summary>
        /// 获取人员本年底积分和累计积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public string GetScoreInfo(string userId)
        {
            return service.GetScoreInfo(userId);
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, UserScoreEntity entity)
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
        /// 批量保存积分考核记录
        /// </summary>
        /// <param name="list"></param>
        public void Save(List<UserScoreEntity> list)
        {
            service.Save(list);
        }
        #endregion
    }
}
