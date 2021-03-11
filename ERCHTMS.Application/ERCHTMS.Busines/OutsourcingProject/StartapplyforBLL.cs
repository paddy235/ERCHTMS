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
    /// 描 述：开工申请表
    /// </summary>
    public class StartapplyforBLL
    {
        private StartapplyforIService service = new StartapplyforService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StartapplyforEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StartapplyforEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取开工申请对象
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        public StartapplyforEntity GetApplyReturnTime(string outProjectId, string outEngId)
        {
            return service.GetApplyReturnTime(outProjectId, outEngId);
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson) {
            return service.GetPageList(pagination, queryJson);
        }

         /// <summary>
        /// 获取开工条件各项目完成情况
        /// </summary>
        /// <param name="projectId">工程Id</param>
        /// <returns></returns>
        public DataTable GetStartWorkStatus(string projectId)
        {
            return service.GetStartWorkStatus(projectId);
        }
                /// <summary>
        /// 获取工程施工现场负责人和安全员信息
        /// </summary>
        /// <param name="projectId">工程Id</param>
        /// <returns></returns>
        public List<string> GetSafetyUserInfo(string projectId)
        {
            return service.GetSafetyUserInfo(projectId);
        }
        public List<string> GetSafetyUserInfo(string projectId, string roletype, string deptid)
        {
            return service.GetSafetyUserInfo(projectId, roletype, deptid);
        }
         public DataTable GetApplyInfo(string keyValue)
        {
            return service.GetApplyInfo(keyValue);
        }
           /// <summary>
        /// 获取工程合同编号
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public object GetContractSno(string projectId)
         {
             return service.GetContractSno(projectId);
         }


        public DataTable GetStartForItem(string keyValue) {
            return service.GetStartForItem(keyValue);
        }
        /// <summary>
        /// 判断当前用户是否有审核权限
        /// </summary>
        /// <param name="nodeId">节点Id</param>
        /// <param name="user">当前用户</param>
        /// <param name="projectId">工程Id</param>
        /// <returns></returns>
        public bool HasCheckPower(string nodeId,ERCHTMS.Code.Operator user,string projectId)
        {
            return service.HasCheckPower(nodeId, user, projectId);
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
        public bool SaveForm(string keyValue, StartapplyforEntity entity)
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
        #endregion
    }
}
