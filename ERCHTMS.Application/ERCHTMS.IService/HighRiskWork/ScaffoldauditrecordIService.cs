using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：1.脚手架审核记录表
    /// </summary>
    public interface ScaffoldauditrecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="scaffoldid">查询参数</param>
        /// <returns>返回列表</returns>
        List<ScaffoldauditrecordEntity> GetList(string scaffoldid);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ScaffoldauditrecordEntity GetEntity(string keyValue);


        List<ScaffoldauditrecordEntity> GetApplyAuditList(string keyValue, int AuditType);

          /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="scaffoldid">脚手架信息ID</param>
        /// <param name="departname">部门名</param>
        /// <param name="rolename">角色名</param>
        /// <returns></returns>
        ScaffoldauditrecordEntity GetEntity(string scaffoldid, string departname, string rolename);

          /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="scaffoldid">脚手架信息ID</param>
        /// <param name="deppartcode">部门</param>
        /// <returns></returns>
        IEnumerable<ScaffoldauditrecordEntity> GetEntitys(string scaffoldid, string departcode);
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
        void SaveForm(string keyValue, ScaffoldauditrecordEntity entity);
        #endregion
    }
}
