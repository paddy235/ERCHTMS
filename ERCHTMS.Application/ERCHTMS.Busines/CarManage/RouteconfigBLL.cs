using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// 描 述：车辆路线配置树
    /// </summary>
    public class RouteconfigBLL
    {
        private RouteconfigIService service = new RouteconfigService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RouteconfigEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// 获取树节点的数据
        /// </summary>
        /// <returns></returns>
        public List<RouteconfigEntity> GetTree(int type)
        {
            return service.GetTree(type);
        }

        /// <summary>
        /// 获取所有路线
        /// </summary>
        /// <returns></returns>
        public List<Route> GetRoute()
        {
            return service.GetRoute();
        }

        /// <summary>
        /// 获取拜访车辆父节点ID
        /// </summary>
        /// <returns></returns>
        public string GetVisitParentid()
        {
            return service.GetVisitParentid();
        }

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns> 
        public IEnumerable<DataItemModel> GetWlList()
        {
            return service.GetWlList();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RouteconfigEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        /// <summary>
        /// 获取路线下拉数据
        /// </summary>
        /// <returns></returns>
        public List<RouteconfigEntity> RouteDropdown()
        {
            return service.RouteDropdown();
        }
        #endregion

        #region 提交数据

        public void SelectLine(string ID)
        {
            service.SelectLine(ID);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, RouteconfigEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="rlist"></param>
        public void SaveList(List<RouteconfigEntity> rlist)
        {
            try
            {
                service.SaveList(rlist);
            }
            catch (Exception e)
            {
                throw ;
            }
        }

        #endregion
    }
}
