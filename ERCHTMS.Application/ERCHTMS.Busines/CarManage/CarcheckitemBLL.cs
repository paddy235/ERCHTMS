using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// 描 述：配置危化品检查项目表
    /// </summary>
    public class CarcheckitemBLL
    {
        private CarcheckitemIService service = new CarcheckitemService();

        #region 获取数据

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarcheckitemEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CarcheckitemEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取去重的危害因素列表
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public IEnumerable<DataItemModel> GetHazardousList(string KeyValue)
        {
            return service.GetHazardousList(KeyValue);
        }

        /// <summary>
        /// 获取通行门岗
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public IEnumerable<DataItemModel> GetCurrentList(string KeyValue)
        {
            return service.GetCurrentList(KeyValue);
        }

        

        #endregion

        #region 提交数据
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
        public void SaveForm(string keyValue, string CheckItemName, List<CarcheckitemhazardousEntity> HazardousArray, List<CarcheckitemmodelEntity> ItemArray)
        {
            try
            {
                service.SaveForm(keyValue, CheckItemName, HazardousArray, ItemArray);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
