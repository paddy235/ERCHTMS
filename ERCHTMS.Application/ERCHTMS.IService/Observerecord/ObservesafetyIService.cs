using ERCHTMS.Entity.Observerecord;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.Observerecord
{
    /// <summary>
    /// 描 述：观察记录安全行为
    /// </summary>
    public interface ObservesafetyIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<ObservesafetyEntity> GetList(string queryJson);
        DataTable GetSafetyPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 根据类型和行为获取列表
        /// </summary>
        /// <param name="issafety">是否安全0-安全行为 1 -不安全行为</param>
        /// <param name="type">观察类别-7大类</param>
        /// <param name="recordid">观察记录Id</param>
        /// <returns></returns>
        DataTable GetDataByType(string issafety, string type, string recordid);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ObservesafetyEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, ObservesafetyEntity entity);
        void SaveForm(string recordId,List<ObservesafetyEntity> entity);
        #endregion
    }
}