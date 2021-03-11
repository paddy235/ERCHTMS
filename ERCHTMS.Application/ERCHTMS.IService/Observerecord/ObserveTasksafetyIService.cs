using BSFramework.Util.WebControl;
using ERCHTMS.Entity.Observerecord;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.Observerecord
{
   public interface ObserveTasksafetyIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<ObserveTasksafetyEntity> GetList(string queryJson);
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
        ObserveTasksafetyEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, ObserveTasksafetyEntity entity);
        void SaveForm(string recordId, List<ObserveTasksafetyEntity> entity);
        #endregion
    }
}
