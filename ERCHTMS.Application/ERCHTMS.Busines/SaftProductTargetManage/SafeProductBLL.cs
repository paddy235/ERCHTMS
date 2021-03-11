using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.SaftProductTargetManage;
using ERCHTMS.IService.SaftProductTargetManage;
using ERCHTMS.Service.SaftProductTargetManage;

namespace ERCHTMS.Busines.SaftProductTargetManage
{
    /// <summary>
    /// 描 述：安全生产目标
    /// </summary>
    public class SafeProductBLL
    {
        private SafeProductIService service = new SafeProductService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafeProductEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafeProductEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        ///根据条件获取相关数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetSafeByCondition(string dateYear, string belongId)
        {
            return service.GetSafeByCondition(dateYear, belongId);
        }

        /// <summary>
        /// 计算目标值
        /// </summary>
        /// <param name="belongtype"></param>
        /// <returns></returns>
        public string calculateGoal(string belongtype, string belongdeptid, string year)
        {
            return service.calculateGoal(belongtype, belongdeptid, year);
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
        public int SaveForm(string keyValue, SafeProductEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion
    }
}
