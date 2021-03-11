﻿using ERCHTMS.Entity.QuestionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.QuestionManage
{
    /// <summary>
    /// 描 述：问题验证信息表
    /// </summary>
    public interface QuestionVerifyIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<QuestionVerifyEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        QuestionVerifyEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, QuestionVerifyEntity entity);
        #endregion
        QuestionVerifyEntity GetEntityByBid(string questionId);
        IEnumerable<QuestionVerifyEntity> GetHistoryList(string QuestionId);
    }
}