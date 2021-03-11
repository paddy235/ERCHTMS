using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using ERCHTMS.Service.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急演练记录步骤表
    /// </summary>
    public class DrillplanrecordstepBLL
    {
        private DrillplanrecordstepIService service = new DrillplanrecordstepService();

        #region 获取数据


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DrillplanrecordstepEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DrillplanrecordstepEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 根据recid获取步骤列表
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        public IList<DrillplanrecordstepEntity> GetListByRecid(string recid)
        {
            return service.GetListByRecid(recid);
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
        public void SaveForm(string keyValue, DrillplanrecordstepEntity entity)
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
        /// 根据关联ID删除数据
        /// </summary>
        /// <param name="recid"></param>
        public void RemoveFormByRecid(string recid)
        {
            try
            {
                service.RemoveFormByRecid(recid);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
    }
}
