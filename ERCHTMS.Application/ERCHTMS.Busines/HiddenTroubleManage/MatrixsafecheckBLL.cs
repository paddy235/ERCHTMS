using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：矩阵安全检查计划
    /// </summary>
    public class MatrixsafecheckBLL
    {
        private MatrixsafecheckIService service = new MatrixsafecheckService();

        #region 获取数据
        /// <summary>
        /// 获取待处理的数量
        /// </summary>
        /// <returns></returns>
        public string GetActionNum()
        {
            return service.GetActionNum();
        }

        /// <summary>
        /// 日历获取数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetCanlendarListJson(string queryJson)
        {
            return service.GetCanlendarListJson(queryJson);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            return service.GetPageListJson(pagination, queryJson);
        }
        /// <summary>
        /// 根据sql查询返回
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetInfoBySql(string sql)
        {
            return service.GetInfoBySql(sql);
        }
        public int ExecuteBySql(string sql)
        {
            return service.ExecuteBySql(sql);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<MatrixsafecheckEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MatrixsafecheckEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public MatrixsafecheckEntity SetFormJson(string keyValue, string recid)
        {
            return service.SetFormJson(keyValue, recid);
        }

        public DataTable GetContentPageJson(string queryJson)
        {
            return service.GetContentPageJson(queryJson);
        }

        public DataTable GetDeptPageJson(string queryJson)
        {
            return service.GetDeptPageJson(queryJson);
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
        public void SaveForm(string keyValue, MatrixsafecheckEntity entity)
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
