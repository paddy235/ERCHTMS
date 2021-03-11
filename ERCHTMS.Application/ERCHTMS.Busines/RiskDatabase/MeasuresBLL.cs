using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using ERCHTMS.Service.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.RiskDatabase
{
    /// <summary>
    /// 描 述：风险管控措施表
    /// </summary>
    public class MeasuresBLL
    {
        private MeasuresIService service = new MeasuresService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<MeasuresEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取管控措施
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <param name="riskId">风险库记录ID</param>
        /// <returns></returns>
        public IEnumerable<MeasuresEntity> GetList(string areaId,string riskId)
        {
            return service.GetList(areaId,riskId);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MeasuresEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取管控措施
        /// </summary>
        /// <param name="riskId">风险记录Id</param>
        /// <returns></returns>
        public DataTable GetDTList(string riskId, string typeName = "")
        {
            return service.GetDTList(riskId, typeName);
        }
        public string GetMeasures(string riskId, string typeName = "")
        {
            return service.GetMeasures(riskId, typeName);
        }
        public DataTable GetMeasuresDetail(string worktask, string areaid)
        {
            return service.GetMeasuresDetail(worktask, areaid);
        }
        public DataTable GetLinkAreaById( string Areaid)
        {
            return service.GetLinkAreaById( Areaid);
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
        /// 删除数据
        /// </summary>
        /// <param name="riskId">风险库记录Id</param>
        public int Remove(string riskId)
        {
            try
            {
               return service.Remove(riskId);
            }
            catch (Exception)
            {
                return -1;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, List<MeasuresEntity> entity)
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
        public bool Save(string keyValue, MeasuresEntity entity)
        {
            try
            {
                service.Save(keyValue, entity);
                return true;
            }
            catch (Exception)
            {
                return false;
               // throw;
            }
        }
        #endregion
    }
}
