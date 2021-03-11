using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：入厂许可申请
    /// </summary>
    public class IntromissionBLL
    {
        private IntromissionIService service = new IntromissionService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<IntromissionEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        #endregion



        #region 通过入厂许可获取外包工程相关信息

        /// <summary>
        /// 通过入厂许可获取外包工程相关信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetOutSourcingProjectByIntromId(string keyValue)
        {
            return service.GetOutSourcingProjectByIntromId(keyValue);
        }
        #endregion

        #region 获取实体
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public IntromissionEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 通过sql返回表集合
        public DataTable GetDataTableBySql(string sql)
        {
            return service.GetDataTableBySql(sql);
        }
        #endregion

        #region MyRegion
        /// <summary>  //获取审查记录信息
        /// 通过入厂许可申请ID获取对应的审查记录内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetDtRecordList(string keyValue)
        {
            return service.GetDtRecordList(keyValue);
        }
        /// <summary>  
        /// 通过开工申请ID获取对应的审查记录内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetStartRecordList(string keyValue)
        {
            return service.GetStartRecordList(keyValue);
        }

        #endregion

        #region 通过入厂许可申请ID获取对应的审查记录内容
        /// <summary>  
        /// 通过入厂许可申请ID获取对应的审查记录内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetHistoryDtRecordList(string keyValue)
        {
            return service.GetHistoryDtRecordList(keyValue);
        }
        public DataTable GetHistoryStartRecordList(string keyValue)
        {
            return service.GetHistoryStartRecordList(keyValue);
        }

        #endregion

        #region 获取审查数据
        /// <summary>
        /// 获取审查数据queryJson
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetIntromissionPageList(Pagination pagination, string queryJson)
        {
            return service.GetIntromissionPageList(pagination, queryJson);
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
        public void SaveForm(string keyValue, IntromissionEntity entity)
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