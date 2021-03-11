using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using BSFramework.Util.WebControl;

namespace ERCHTMS.Busines.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：整改延期对象
    /// </summary>
    public class HTExtensionBLL
    {
        private HTExtensionIService service = new HTExtensionService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HTExtensionEntity> GetList(string hidCode)
        {
            return service.GetList(hidCode);
        }

        public IList<HTExtensionEntity> GetListByCondition(string hidCode) 
        {
            return service.GetListByCondition(hidCode);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HTExtensionEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        /// <summary>
        /// 获取最新的一个整改延期申请实体
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public HTExtensionEntity GetFirstEntity(string hidCode) 
        {
            return service.GetFirstEntity(hidCode);
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
        public void SaveForm(string keyValue, HTExtensionEntity entity)
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
        #endregion
    }
}