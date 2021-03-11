using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using ERCHTMS.Service.FireManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Collections;

namespace ERCHTMS.Busines.FireManage
{
    /// <summary>
    /// 描 述：消防设施
    /// </summary>
    public class FirefightingBLL
    {
        private FirefightingIService service = new FirefightingService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<FirefightingEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FirefightingEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public DataTable StatisticsData(string queryJson)
        {
            return service.StatisticsData(queryJson);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 根据id数组批量删除
        /// </summary>
        /// <param name="Ids"></param>
        public void Remove(string Ids)
        {
            try
            {
                service.Remove(Ids);
            }
            catch (Exception)
            {

                throw;
            }
        }
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
        public void SaveForm(string keyValue, FirefightingEntity entity)
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

        /// <summary>
        /// 同一类型，编号不能重复
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Code"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool ExistCode(string Type, string Code, string keyValue)
        {
            return service.ExistCode(Type,Code, keyValue);
        }

        /// <summary>
        /// 根据区域获取区域底下的消防设施
        /// </summary>
        /// <param name="areaCodes">区域编码</param>
        /// <returns></returns>
        public IList GetCountByArea(List<string> areaCodes)
        {
            return service.GetCountByArea(areaCodes);
        }
    }
}
