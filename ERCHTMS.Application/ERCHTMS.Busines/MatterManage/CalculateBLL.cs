using ERCHTMS.Entity.MatterManage;
using ERCHTMS.IService.MatterManage;
using ERCHTMS.Service.MatterManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.MatterManage
{
    /// <summary>
    /// 描 述：计量管理
    /// </summary>
    public class CalculateBLL
    {
        private CalculateIService service = new CalculateService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CalculateEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 称重列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetNewPageList(Pagination pagination, string queryJson)
        {
            return service.GetNewPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取计量统计列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetCountPageList(Pagination pagination, string queryJson)
        {
            return service.GetCountPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取地磅员列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageUserList(Pagination pagination, string queryJson,string res)
        {
            return service.GetPageUserList(pagination, queryJson,res);
        }

        /// <summary>
        /// 获取用户授权记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public UserEmpowerRecordEntity GetUserRecord(string keyValue)
        {
            try
            {
                return service.GetUserRecord(keyValue);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CalculateEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取最新称重计量信息
        /// </summary>
        /// <param name="keyValue">称重单号</param>
        /// <returns></returns>
        public CalculateEntity GetNewEntity(string keyValue)
        {
            return service.GetNewEntity(keyValue);
        }


        /// <summary>
        ///返回未出场订单
        /// </summary>
        /// <returns>返回列表</returns>
        public CalculateEntity GetEntranceTicket(string carNo)
        {
            return service.GetEntranceTicket(carNo);
        }


    /// <summary>
    /// 获取记录管理详情记录实体
    /// </summary>
    /// <param name="keyValue">主键值</param>
    /// <returns></returns>
    public CalculateDetailedEntity GetAppDetailedEntity(string keyValue)
        {
            return service.GetAppDetailedEntity(keyValue);
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
        public void SaveForm(string keyValue, CalculateEntity entity)
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
        /// 手机接口保存
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveAppForm(string keyValue, CalculateEntity entity)
        {
            try
            {
                service.SaveAppForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 手机接口保存
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveWeightBridgeDetail(string keyValue, CalculateDetailedEntity entity)
        {
            try
            {
                service.SaveWeightBridgeDetail(keyValue, entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        /// <summary>
        /// 保存用户授权信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveUserForm(string keyValue, UserEmpowerRecordEntity entity)
        {
            try
            {
                service.SaveUserForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

      



        #endregion
    }
}
