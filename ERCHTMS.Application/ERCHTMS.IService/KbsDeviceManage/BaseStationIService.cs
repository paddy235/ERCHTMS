using ERCHTMS.Entity.KbsDeviceManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// 描述：基站管理
    /// </summary>
    public interface BaseStationIService
    {
        #region 获取数据

        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <returns>返回分页列表</returns>
        List<BaseStationEntity> GetPageList();
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<BaseStationEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        BaseStationEntity GetEntity(string keyValue);

        /// <summary>
        /// 根据状态获取基站数量
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        int GetBaseStationNum(string status);
        /// <summary>
        /// 获取基站统计图
        /// </summary>
        /// <returns></returns>
        string GetLableChart();

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
        void SaveForm(string keyValue, BaseStationEntity entity);

        /// <summary>
        /// 接口修改状态用方法
        /// </summary>
        /// <param name="entity"></param>
        void UpdateState(BaseStationEntity entity);

        #endregion
    }

}
