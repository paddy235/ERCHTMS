using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：起重吊装作业操作人员表
    /// </summary>
    public interface LifthoistpersonIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<LifthoistpersonEntity> GetList(string queryJson);

        /// <summary>
        /// 获取起重吊装相关人员信息
        /// </summary>
        /// <param name="workid"></param>
        /// <returns></returns>
        IEnumerable<LifthoistpersonEntity> GetRelateList(string workid);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        LifthoistpersonEntity GetEntity(string keyValue);

        /// <summary>
        /// 证件号不能重复
        /// </summary>
        /// <param name="CertificateNum">证件号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistCertificateNum(string CertificateNum, string keyValue);
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
        void SaveForm(string keyValue, LifthoistpersonEntity entity);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 删除起重吊装作业相关的人员信息
        /// </summary>
        /// <param name="WorkId"></param>
        void RemoveFormByWorkId(string WorkId);
        #endregion
    }
}
