using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// 描 述：标签上下线日志
    /// </summary>
    public interface LableonlinelogIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<LableonlinelogEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        LableonlinelogEntity GetEntity(string keyValue);

        /// <summary>
        /// 根据标签ID获取在线标签
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        LableonlinelogEntity GetOnlineEntity(string LableId);
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
        void SaveForm(string keyValue, LableonlinelogEntity entity);

        /// <summary>
        /// 储存上下线数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SaveStatus(LableonlinelogEntity entity);

        #endregion
    }
}
