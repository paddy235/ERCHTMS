using ERCHTMS.Entity.AccidentEvent;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.IService.AccidentEvent
{
    /// <summary>
    /// 描 述：未遂事件报告与调查处理
    /// </summary>
    public interface IWssjbg_dealService
    {
        #region 获取数据
        IEnumerable<Wssjbg_dealEntity> GetListForCon(Expression<Func<Wssjbg_dealEntity, bool>> condition);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<Wssjbg_dealEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        Wssjbg_dealEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, Wssjbg_dealEntity entity);
        #endregion
    }
}
