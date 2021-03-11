using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using ERCHTMS.Service.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Busines.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病危害因素监测
    /// </summary>
    public class HazarddetectionBLL
    {
        private HazarddetectionIService service = new HazarddetectionService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HazarddetectionEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HazarddetectionEntity GetEntity(string keyValue)
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
        /// 获取全部列表数据
        /// </summary>
        /// <param name="riskid">危害因素</param>
        /// <param name="areaid">区域</param>
        /// <param name="starttime">时间范围</param>
        /// <param name="endtime">时间范围</param>
        /// <param name="isexcessive">是否超标</param>
        /// <param name="detectionuserid">检测人id</param>
        /// <returns></returns>
        public DataTable GetDataTable(string queryJson, string where)
        {

            return service.GetDataTable(queryJson, where);
        }
        /// <summary>
        /// 获取测量指标及标准
        /// </summary>
        /// <param name="RiskId">职业病id</param>
        /// <returns></returns>
        public string GetStandard(string RiskId, string where)
        {
            return service.GetStandard(RiskId, where);
        }

        /// <summary>
        /// 获取危害因素监测统计数据
        /// </summary>
        /// <param name="year">哪一年数据</param>
        /// <param name="risk">职业病种类</param>
        /// <param name="type">true查询全部 false查询超标数据</param>
        /// <returns></returns>
        public DataTable GetStatisticsHazardTable(int year, string risk, bool type, string where)
        {
            return service.GetStatisticsHazardTable(year, risk, type, where);
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
        public void SaveForm(string keyValue, HazarddetectionEntity entity)
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

        #region 手机端
        /// <summary>
        /// 新增危险因素监测数据
        /// </summary>
        /// <param name="assess">实体</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public int SaveHazard(HazarddetectionEntity hazard, ERCHTMS.Code.Operator user)
        {
            try
            {
                return service.SaveHazard(hazard, user);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <param name="riskid">危害因素</param>
        /// <param name="areaid">区域</param>
        /// <param name="starttime">时间范围</param>
        /// <param name="endtime">时间范围</param>
        /// <param name="isexcessive">是否超标</param>
        /// <param name="detectionuserid">检测人id</param>
        /// <returns></returns>
        public DataTable GetDataTable(string riskid, string areaid, string starttime, string endtime, string isexcessive, string detectionuserid, string where)
        {
            try
            {
                return service.GetDataTable(riskid, areaid, starttime, endtime, isexcessive, detectionuserid, where);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
