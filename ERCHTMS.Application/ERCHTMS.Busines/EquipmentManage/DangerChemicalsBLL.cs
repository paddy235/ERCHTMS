using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.IService.EquipmentManage;
using ERCHTMS.Service.EquipmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.EquipmentManage
{
    /// <summary>
    /// 描 述：危险化学品库存管理
    /// </summary>
    public class DangerChemicalsBLL
    {
        private DangerChemicalsIService service = new DangerChemicalsService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DangerChemicalsEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取工作流节点
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public DataTable GetWorkDetailList(string objectId)
        {
            return service.GetWorkDetailList(objectId);
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DangerChemicalsEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public int GetDangerChemicalsBMNum(ERCHTMS.Code.Operator user)
        {
            return service.GetDangerChemicalsBMNum(user);
        }
        /// <summary>
        /// 获取待审核个人工作计划
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetDangerChemicalsGRNum(ERCHTMS.Code.Operator user) {
            return service.GetDangerChemicalsGRNum(user);
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
        public void SaveForm(string keyValue, DangerChemicalsEntity entity)
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
