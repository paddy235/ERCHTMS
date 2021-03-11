using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：起重吊装作业操作人员表
    /// </summary>
    public class LifthoistpersonBLL
    {
        private LifthoistpersonIService service = new LifthoistpersonService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LifthoistpersonEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// 获取起重吊装相关人员信息
        /// </summary>
        /// <param name="workid"></param>
        /// <returns></returns>
        public IEnumerable<LifthoistpersonEntity> GetRelateList(string workid)
        {
            return service.GetRelateList(workid);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LifthoistpersonEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 证件号不能重复
        /// </summary>
        /// <param name="CertificateNum">证件号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistCertificateNum(string CertificateNum, string keyValue)
        {
            return service.ExistCertificateNum(CertificateNum, keyValue);
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
        public void SaveForm(string keyValue, LifthoistpersonEntity entity)
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

        /// <summary>
        /// 删除起重吊装作业相关的人员信息
        /// </summary>
        /// <param name="WorkId"></param>
        public void RemoveFormByWorkId(string WorkId)
        {
            try
            {
                service.RemoveFormByWorkId(WorkId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
