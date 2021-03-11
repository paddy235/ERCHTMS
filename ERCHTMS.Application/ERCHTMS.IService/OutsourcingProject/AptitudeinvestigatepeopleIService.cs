using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查人员表
    /// </summary>
    public interface AptitudeinvestigatepeopleIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<AptitudeinvestigatepeopleEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        AptitudeinvestigatepeopleEntity GetEntity(string keyValue);

        IEnumerable<AptitudeinvestigatepeopleEntity> GetPersonInfo(string projectid, string pageindex, string pagesize);
        
        bool IsAuditByUserId(string userid);
        DataTable GetPageList(Pagination pagination, string queryJson);
        #endregion

        #region 提交数据
        bool ExistIdentifyID(string IdentifyID, string keyValue);

        bool ExistAccount(string Account, string keyValue);
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
        void SaveForm(string keyValue, AptitudeinvestigatepeopleEntity entity);
        /// <summary>
        /// 批量新增外包人员的体检信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        void SummitPhyInfo(string keyValue, PhyInfoEntity entity);
        #endregion
    }
}
