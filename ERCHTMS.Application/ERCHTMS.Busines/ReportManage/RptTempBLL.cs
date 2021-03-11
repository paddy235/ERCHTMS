﻿using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.Entity.ReportManage;
using ERCHTMS.IService.ReportManage;
using ERCHTMS.Service.ReportManage;
using System;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.Busines.ReportManage
{
    /// <summary>
    /// 描 述：报表模板管理
    /// </summary>
    public class RptTempBLL
    {
        private IRptTempService service = new RptTempService();

        #region 获取数据
        /// <summary>
        /// 报表模板列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<RptTempEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 报表模板实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RptTempEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获得报表数据
        /// </summary>
        /// <param name="reportId">主键</param>
        /// <returns></returns>
        public string GetReportData(string reportId)
        {
            return service.GetReportData(reportId);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除报表模板
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
        /// 保存报表模板表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="rptTempEntity">报表实体</param>
        /// <param name="moduleEntity">模块实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, RptTempEntity rptTempEntity, ModuleEntity moduleEntity)
        {
            try
            {
                service.SaveForm(keyValue, rptTempEntity, moduleEntity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
