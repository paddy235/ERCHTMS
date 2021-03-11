using ERCHTMS.Entity.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患复查验证信息表
    /// </summary>
    public interface HtReCheckIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HtReCheckEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HtReCheckEntity GetEntity(string keyValue);
        #endregion

                /// <summary>
        /// 获取历史的所有验收信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<HtReCheckEntity> GetHistoryList(string hidCode);
         /// <summary>
        /// 评估信息
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        HtReCheckEntity GetEntityByHidCode(string hidCode);

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
        void SaveForm(string keyValue, HtReCheckEntity entity);
        #endregion
    }
}