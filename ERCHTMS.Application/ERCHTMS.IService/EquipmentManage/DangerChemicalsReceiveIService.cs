using ERCHTMS.Entity.EquipmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.EquipmentManage
{
    /// <summary>
    /// 描 述：危险化学品领用
    /// </summary>
    public interface DangerChemicalsReceiveIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<DangerChemicalsReceiveEntity> GetList(string queryJson);        
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DangerChemicalsReceiveEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取工作流节点
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        DataTable GetWorkDetailList(string objectId);

        int GetDangerChemicalsReceiveBMNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取待审核个人工作计划
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int GetDangerChemicalsReceiveGRNum(ERCHTMS.Code.Operator user);
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
        void SaveForm(string keyValue, DangerChemicalsReceiveEntity entity);
        #endregion
    }
}
