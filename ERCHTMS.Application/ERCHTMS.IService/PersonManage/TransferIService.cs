using ERCHTMS.Entity.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.PersonManage
{
    /// <summary>
    /// 描 述：转岗信息表
    /// </summary>
    public interface TransferIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<TransferEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TransferEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TransferEntity GetUsertraEntity(string keyValue);

        /// <summary>
        /// 获取当前用户所有转岗代办事项
        /// </summary>
        /// <returns></returns>
        int GetTransferNum();

        /// <summary>
        /// 获取代办列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        DataTable GetTransferList(Pagination pagination, string queryJson);

        /// <summary>
        /// 根据当前部门id获取层级显示部门
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        string GetDeptName(string deptid);
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
        void SaveForm(string keyValue, TransferEntity entity);

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void AppSaveForm(string keyValue, TransferEntity entity, string Userid);

        ///// <summary>
        ///// 转岗确认操作
        ///// </summary>
        ///// <param name="keyValue"></param>
        ///// <param name="entity"></param>
        //void Update(string keyValue, TransferEntity entity);

        #endregion
    }
}
