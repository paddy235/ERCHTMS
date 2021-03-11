using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包单位黑名单表
    /// </summary>
    public class OutprojectblacklistBLL
    {
        private OutprojectblacklistIService service = new OutprojectblacklistService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OutprojectblacklistEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OutprojectblacklistEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public DataTable GetPageBlackListJson(Pagination pagination, string queryJson)
        {
            return service.GetPageBlackListJson(pagination, queryJson);
        }
        /// <summary>
        /// 待审（核）批单位资质审查、待审（核）批人员资质审查、待审（核）批三措两案、待审（核）批特种设备验收、待审（核）批安全/电动工器具验收、待审（核）批入厂许可、待审（核）批开工申请
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<int> ToAuditOutPeoject(Operator user)
        {
            return service.ToAuditOutPeoject(user);
        }
        public List<int> ToIndexData(Operator user)
        {
            return service.ToIndexData(user);
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
        public void SaveForm(string keyValue, OutprojectblacklistEntity entity)
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
