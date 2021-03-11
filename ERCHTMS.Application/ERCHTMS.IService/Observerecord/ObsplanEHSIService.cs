using ERCHTMS.Entity.Observerecord;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.IService.Observerecord
{
    /// <summary>
    /// 描 述：观察计划
    /// </summary>
    public interface ObsplanEHSIService
    {
        #region 获取数据
      
        ObsplanEHSEntity GetEntity(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, ObsplanEHSEntity entity);
        #endregion

     
    }
}
