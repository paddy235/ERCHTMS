using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.PersonManage
{
    /// <summary>
    /// 描 述：转岗信息表
    /// </summary>
    public class TransferBLL
    {
        private TransferIService service = new TransferService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TransferEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TransferEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TransferEntity GetUsertraEntity(string keyValue)
        {
            return service.GetUsertraEntity(keyValue);
        }

        /// <summary>
        /// 获取当前用户所有转岗代办事项
        /// </summary>
        /// <returns></returns>
        public int GetTransferNum()
        {
            return service.GetTransferNum();
        }

        /// <summary>
        /// 获取代办列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            return service.GetTransferList(pagination, queryJson);
        }

        /// <summary>
        /// 根据当前部门id获取层级显示部门
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public string GetDeptName(string deptid)
        {
            return service.GetDeptName(deptid);
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
        public void SaveForm(string keyValue, TransferEntity entity)
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void AppSaveForm(string keyValue, TransferEntity entity, string Userid)
        {
            try
            {
                service.AppSaveForm(keyValue, entity, Userid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 转岗确认操作
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void Update(string keyValue, TransferEntity entity)
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

        #endregion
    }
}
