using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// 描 述：车辆路线配置树
    /// </summary>
    public interface RouteconfigIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<RouteconfigEntity> GetList(string queryJson);

        /// <summary>
        /// 获取树节点的数据
        /// </summary>
        /// <returns></returns>
        List<RouteconfigEntity> GetTree(int type);

        /// <summary>
        /// 获取所有路线
        /// </summary>
        /// <returns></returns>
        List<Route> GetRoute();

        /// <summary>
        /// 获取拜访路线父节点
        /// </summary>
        /// <returns></returns>
        string GetVisitParentid();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        RouteconfigEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        IEnumerable<DataItemModel> GetWlList();


        /// <summary>
        /// 获取路线下拉数据
        /// </summary>
        /// <returns></returns>
        List<RouteconfigEntity> RouteDropdown();
        #endregion

        #region 提交数据
        /// <summary>
        /// 选择路线
        /// </summary>
        /// <param name="ID"></param>
        void SelectLine(string ID);
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
        void SaveForm(string keyValue, RouteconfigEntity entity);

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="rlist"></param>
        void SaveList(List<RouteconfigEntity> rlist);

        #endregion
    }
}
