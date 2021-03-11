using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查人员表
    /// </summary>
    public class AptitudeinvestigatepeopleBLL
    {
        private AptitudeinvestigatepeopleIService service = new AptitudeinvestigatepeopleService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<AptitudeinvestigatepeopleEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }


        public IEnumerable<AptitudeinvestigatepeopleEntity> GetPersonInfo(string projectid, string pageindex, string pagesize) {
            return service.GetPersonInfo(projectid, pageindex, pagesize);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public AptitudeinvestigatepeopleEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 根据用户Id获取该用户所在工程是否通过人员资质审核
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool IsAuditByUserId(string userid) {
            return service.IsAuditByUserId(userid);
        }
        #endregion

        #region 提交数据
        public bool ExistIdentifyID(string IdentifyID, string keyValue)
        {
            return service.ExistIdentifyID(IdentifyID,keyValue);
        }

        public bool ExistAccount(string Account, string keyValue)
        {
            return service.ExistAccount(Account, keyValue);
        }
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
        public void SaveForm(string keyValue, AptitudeinvestigatepeopleEntity entity)
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
        /// 批量新增外包人员的体检信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SummitPhyInfo(string keyValue, PhyInfoEntity entity) {
            try
            {
                service.SummitPhyInfo(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
