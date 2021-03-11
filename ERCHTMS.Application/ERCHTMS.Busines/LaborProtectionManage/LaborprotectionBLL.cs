using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.IService.LaborProtectionManage;
using ERCHTMS.Service.LaborProtectionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护用品
    /// </summary>
    public class LaborprotectionBLL
    {
        private LaborprotectionIService service = new LaborprotectionService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LaborprotectionEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LaborprotectionEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            return service.GetPageListByProc(pagination, queryJson);
        }

        /// <summary>
        /// 获取到当前
        /// </summary>
        /// <returns></returns>
        public string GetNo()
        {
            return service.GetNo();
        }

        /// <summary>
        /// 获取当前机构所有物资
        /// </summary>
        /// <returns></returns>
        public List<LaborprotectionEntity> GetLaborList()
        {
            return service.GetLaborList();
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
        public void SaveForm(string keyValue, LaborprotectionEntity entity)
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
