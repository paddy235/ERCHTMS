using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// 描 述：标签管理
    /// </summary>
    public interface LablemanageIService
    {
        #region 获取数据

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        List<LablemanageEntity> GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取标签总数
        /// </summary>
        /// <returns></returns>
        int GetCount();
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<LablemanageEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        LablemanageEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取标签统计图
        /// </summary>
        /// <returns></returns>
        string GetLableChart();

        /// <summary>
        /// 获取标签统计信息
        /// </summary>
        /// <returns></returns>
        DataTable GetLableStatistics();

        /// <summary>
        /// 获取用户绑定标签
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        LablemanageEntity GetUserLable(string userid);

        /// <summary>
        /// 获取车辆是否绑定标签
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        LablemanageEntity GetCarLable(string CarNo);

        /// <summary>
        /// 获取标签是否重复绑定
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        bool GetIsBind(string LableId);

        /// <summary>
        /// 根据lableId获取是否有绑定信息
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        LablemanageEntity GetLable(string LableId);
        #endregion

        #region 提交数据

        /// <summary>
        /// 解绑标签
        /// </summary>
        /// <param name="keyValue"></param>
        void Untie(string keyValue);
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
        void SaveForm(string keyValue, LablemanageEntity entity);
        #endregion
    }
}
