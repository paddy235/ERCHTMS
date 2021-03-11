using BSFramework.Util.WebControl;
using ERCHTMS.Entity.EmergencyPlatform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.EmergencyPlatform
{ /// <summary>
    /// 描 述：演练记录评价表
    /// </summary>
    public interface DrillrecordevaluateIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<DrillrecordevaluateEntity> GetList();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DrillrecordevaluateEntity GetEntity(string keyValue);
        DataTable GetEvaluateList(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, DrillrecordevaluateEntity entity);
        /// <summary>
        /// 保存评估记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        void AssessRecordSaveForm(string keyValue, DrillrecordAssessEntity entity);
        #endregion
    }
}
