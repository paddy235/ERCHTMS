using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全技术交底
    /// </summary>
    public interface TechDisclosureIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        DataTable GetList(Pagination pagination, string queryJson);
        IEnumerable<TechDisclosureEntity> GetList();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TechDisclosureEntity GetEntity(string keyValue);
        DataTable GetNameByPorjectId(string projectId, string type);
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
        void SaveForm(string keyValue, TechDisclosureEntity entity);
        /// <summary>
        /// 审核表单
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="aentity"></param>
        void ApporveForm(string keyValue, TechDisclosureEntity entity, AptitudeinvestigateauditEntity aentity);
        #endregion
    }
}
