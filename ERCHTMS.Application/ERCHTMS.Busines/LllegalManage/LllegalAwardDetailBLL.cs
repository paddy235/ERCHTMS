﻿using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using ERCHTMS.Service.LllegalManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.LllegalManage
{
    /// <summary>
    /// 描 述：反违章奖励表
    /// </summary>
    public class LllegalAwardDetailBLL
    {
        private LllegalAwardDetailIService service = new LllegalAwardDetailService();

        #region 获取数据
        /// <summary>
                /// 获取列表
                /// </summary>
                /// <param name="queryJson">查询参数</param>
                /// <returns>返回列表</returns>
        public IEnumerable<LllegalAwardDetailEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalAwardDetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 获取违章奖励信息

        /// <summary>
        /// 获取违章奖励信息
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public List<LllegalAwardDetailEntity> GetListByLllegalId(string LllegalId)
        {
            try
            {
                return service.GetListByLllegalId(LllegalId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 删除违章奖励
        /// <summary>
        /// 删除违章奖励
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public int DeleteLllegalAwardList(string LllegalId)
        {
            try
            {
                return service.DeleteLllegalAwardList(LllegalId);
            }
            catch (Exception)
            {
                throw;
            }
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
        public void SaveForm(string keyValue, LllegalAwardDetailEntity entity)
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