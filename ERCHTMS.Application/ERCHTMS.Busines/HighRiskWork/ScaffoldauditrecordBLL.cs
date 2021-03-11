using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using BSFramework.Util.Extension;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：1.脚手架审核记录表
    /// </summary>
    public class ScaffoldauditrecordBLL
    {
        private ScaffoldauditrecordIService service = new ScaffoldauditrecordService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="scaffoldid">查询参数</param>
        /// <returns>返回列表</returns>
        public List<ScaffoldauditrecordEntity> GetList(string scaffoldid)
        {
            return service.GetList(scaffoldid);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ScaffoldauditrecordEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public List<ScaffoldauditrecordEntity> GetApplyAuditList(string keyValue, int AuditType)
        {
            return service.GetApplyAuditList(keyValue, AuditType);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="scaffoldid">脚手架信息ID</param>
        /// <param name="departname">部门名</param>
        /// <param name="rolename">角色名</param>
        /// <returns></returns>
        public ScaffoldauditrecordEntity GetEntity(string scaffoldid, string departname, string rolename)
        {
            return service.GetEntity(scaffoldid, departname, rolename);
        }

          /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="scaffoldid">脚手架信息ID</param>
        /// <param name="deppartcode">部门</param>
        /// <returns></returns>
        public IEnumerable<ScaffoldauditrecordEntity> GetEntitys(string scaffoldid, string departcode)
        {
            return service.GetEntitys(scaffoldid,departcode);
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
        public void SaveForm(string keyValue, ScaffoldauditrecordEntity entity)
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
