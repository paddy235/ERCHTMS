using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查审核表
    /// </summary>
    public interface AptitudeinvestigateauditIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<AptitudeinvestigateauditEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        AptitudeinvestigateauditEntity GetEntity(string keyValue);
        AptitudeinvestigateauditEntity GetAuditEntity(string FKId);



        List<AptitudeinvestigateauditEntity> GetAuditList(string keyValue);
        DataTable GetPageList(Pagination pagination, string queryJson);
          /// <summary>
        /// 获取业务相关的审核记录
        /// </summary>
        /// <param name="recId">业务记录Id</param>
        /// <returns></returns>
        DataTable GetAuditRecList(string recId);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);

        /// <summary>
        /// 根据业务主键删除流程
        /// </summary>
        /// <param name="aptitudeId">业务主键</param>
        void DeleteFormByAptitudeId(string aptitudeId);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, AptitudeinvestigateauditEntity entity);
        /// <summary>
        /// 保存表单（新增、修改）
        /// 资质审查审核通过同步 工程 单位 人员信息
        /// 修改外包单位入场状态,修改流程表资质审核状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        void SaveSynchrodata(string keyValue, AptitudeinvestigateauditEntity entity);
        /// <summary>
        /// 保存表单（新增、修改）
        /// 安全保证金审核
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveSafetyEamestMoney(string keyValue, AptitudeinvestigateauditEntity entity);


          /// <summary>
        /// 保存表单（新增、修改）
        /// 复工申请审核:更新工程状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void AuditReturnForWork(string keyValue, AptitudeinvestigateauditEntity entity);

        /// <summary>
        /// 保存表单（新增、修改）
        /// 开工申请审核:更新工程状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void AuditStartApply(string keyValue, AptitudeinvestigateauditEntity entity);


        List<string> AuditPeopleReview(string keyValue, AptitudeinvestigateauditEntity entity);
        #endregion
    }
}
