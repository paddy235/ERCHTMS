using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// 描 述：危化品车辆检查项目表
    /// </summary>
    public interface CarcheckitemdetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<CarcheckitemdetailEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        CarcheckitemdetailEntity GetEntity(string keyValue);
        #endregion

        #region 提交数据

        /// <summary>
        /// 修改详情列表并提交状态
        /// </summary>
        /// <param name="detaillist"></param>
        void Update(List<CarcheckitemdetailEntity> detaillist);
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
        void SaveForm(string keyValue, CarcheckitemdetailEntity entity);
        #endregion
    }
}
