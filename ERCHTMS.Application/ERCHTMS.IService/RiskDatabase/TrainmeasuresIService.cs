﻿using ERCHTMS.Entity.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.RiskDatabase
{
    /// <summary>
    /// 描 述：预知训练作业风险措施
    /// </summary>
    public interface TrainmeasuresIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<TrainmeasuresEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TrainmeasuresEntity GetEntity(string keyValue);
         /// <summary>
        /// 根据作业Id获取列表
        /// </summary>
        /// <param name="workId">作业Id</param>
        /// <returns>返回列表</returns>
        IEnumerable<TrainmeasuresEntity> GetListByWorkId(string workId);

        DataTable GetPageListByWorkId(Pagination pagination, string queryJson);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, TrainmeasuresEntity entity);
        #endregion
    }
}