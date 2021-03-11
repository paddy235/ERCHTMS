using ERCHTMS.Entity.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.PersonManage
{
    /// <summary>
    /// 描 述：人员证书
    /// </summary>
    public interface CertificateIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<CertificateEntity> GetList(string queryJson, Pagination pag = null);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        CertificateEntity GetEntity(string keyValue);

        /// <summary>
        /// 计算人员持证率
        /// </summary>
        /// <param name="sum">人员总数量</param>
        /// <param name="deptCode">当前用户所属部门编码</param>
        /// <param name="certType">证书类别</param>
        /// <returns></returns>
        decimal GetCertPercent(int sum, string deptCode, string certType);
        /// <summary>
        /// 根据当前用户的权限范围获取相关联的即将过期和已过期的证书信息
        /// </summary>
        /// <param name="where">数据权限范围</param>
        /// <returns></returns>
        Dictionary<string, string> GetOverdueCertList(string where);
        /// <summary>
        /// 获取人员证书
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DataTable GetCertList(string userId);

        /// <summary>
        /// 获取证件复审记录
        /// </summary>
        /// <param name="certId"></param>
        /// <returns></returns>
        DataTable GetAuditList(string certId);

        /// <summary>
        /// 获取复审信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        CertAuditEntity GetAuditEntity(string keyValue);
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
        bool SaveForm(string keyValue, CertificateEntity entity);
        /// <summary>
        /// 新增或修改证书复审记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SaveCertAudit(CertAuditEntity entity);
        /// <summary>
        /// 删除证书复审记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        bool RemoveCertAudit(string keyValue);
        #endregion
    }
}
