using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// 描 述：配置危化品检查项目表
    /// </summary>
    public interface CarcheckitemIService
    {
        #region 获取数据

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<CarcheckitemEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        CarcheckitemEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取去重的危害因素列表
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        IEnumerable<DataItemModel> GetHazardousList(string KeyValue);

        IEnumerable<DataItemModel> GetCurrentList(string KeyValue);
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
        void SaveForm(string keyValue, string CheckItemName, List<CarcheckitemhazardousEntity> HazardousArray, List<CarcheckitemmodelEntity> ItemArray);
        #endregion
    }
}
