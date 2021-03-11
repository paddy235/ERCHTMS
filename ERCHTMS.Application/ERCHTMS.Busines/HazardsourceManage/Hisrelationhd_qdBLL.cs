using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.IService.HazardsourceManage;
using ERCHTMS.Service.HazardsourceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.HazardsourceManage
{
    /// <summary>
    /// 描 述：危险源清单
    /// </summary>
    public class Hisrelationhd_qdBLL
    {
        private IHisrelationhd_qdService service = new Hisrelationhd_qdService();

        #region 获取数据
        public IEnumerable<Hisrelationhd_qdEntity> GetListForRecord(string queryJson)
        {
            return service.GetListForRecord(queryJson);
        }
        public DataTable GetReportForDistrictName(string queryJson)
        {
            return service.GetReportForDistrictName(queryJson);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<Hisrelationhd_qdEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public Hisrelationhd_qdEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public string StaQueryList(string queryJson) {
            return service.StaQueryList(queryJson);
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
        public void SaveForm(string keyValue, Hisrelationhd_qdEntity entity)
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
