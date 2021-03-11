using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// 描 述：闯入人员抓拍记录表
    /// </summary>
    public interface WorkcameracaptureIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<WorkcameracaptureEntity> GetList(string queryJson);
        /// <summary>
        /// 根据工作区域查询抓拍图片
        /// </summary>
        /// <param name="workid"></param>
        /// <returns></returns>
        List<WorkcameracaptureEntity> GetCaptureList(string workid, string userid, string cameraid);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        WorkcameracaptureEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, WorkcameracaptureEntity entity);
        #endregion
    }
}
