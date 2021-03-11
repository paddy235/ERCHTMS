using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.SaftProductTargetManage;

namespace ERCHTMS.IService.SaftProductTargetManage
{
    /// <summary>
    /// 描 述：安全生产目标
    /// </summary>
    public interface SafeProductIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafeProductEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafeProductEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取条件获取相关数据
        /// </summary>
        /// <returns></returns>
        DataTable GetSafeByCondition(string dateYear, string belongId);

         /// <summary>
        /// 计算目标值
        /// </summary>
        /// <param name="belongtype"></param>
        /// <returns></returns>
        string calculateGoal(string belongtype, string belongdeptid,string year);
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
        int SaveForm(string keyValue, SafeProductEntity entity);
        #endregion
    }
}
