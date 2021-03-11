using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.PersonManage
{
    /// <summary>
    /// 描 述：人员证书
    /// </summary>
    public class CertificateBLL
    {
        private CertificateIService service = new CertificateService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CertificateEntity> GetList(string queryJson, Pagination pag = null)
        {
            return service.GetList(queryJson, pag);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CertificateEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 计算人员持证率
        /// </summary>
        /// <param name="sum">人员总数量</param>
        /// <param name="deptCode">当前用户所属部门编码</param>
        /// <param name="certType">证书类别</param>
        /// <returns></returns>
        public decimal GetCertPercent(int sum, string deptCode, string certType)
        {
            return service.GetCertPercent(sum, deptCode, certType);
        }
        /// <summary>
        /// 根据当前用户的权限范围获取相关联的即将过期和已过期的证书信息
        /// </summary>
        /// <param name="where">数据权限范围</param>
        /// <returns></returns>
        public Dictionary<string, string> GetOverdueCertList(string where)
        {
            return service.GetOverdueCertList(where);
        }
        /// <summary>
        /// 获取人员证书
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable GetCertList(string userId)
        {
            return service.GetCertList(userId);
        }
        /// <summary>
        /// 获取证件复审记录
        /// </summary>
        /// <param name="certId"></param>
        /// <returns></returns>
        public DataTable GetAuditList(string certId)
        {
            return service.GetCertList(certId);
        }
        /// <summary>
        /// 获取复审信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public CertAuditEntity GetAuditEntity(string keyValue)
        {
            return service.GetAuditEntity(keyValue);
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
        public bool SaveForm(string keyValue, CertificateEntity entity)
        {
            try
            {
                return service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 新增或修改证书复审记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveCertAudit(CertAuditEntity entity)
        {
            return service.SaveCertAudit(entity);
        }
        /// <summary>
        /// 删除证书复审记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool RemoveCertAudit(string keyValue)
        {
            return service.RemoveCertAudit(keyValue);
        }
        #endregion
    }
}
