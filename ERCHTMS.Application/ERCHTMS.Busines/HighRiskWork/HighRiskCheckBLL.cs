using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：高危险作业审核/审批表
    /// </summary>
    public class HighRiskCheckBLL
    {
        private HighRiskCheckIService service = new HighRiskCheckService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HighRiskCheckEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HighRiskCheckEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        /// <summary>
        /// 根据申请id获取审核信息[不包括没审的]
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public IEnumerable<HighRiskCheckEntity> GetCheckListInfo(string approveid) 
        {
            return service.GetCheckListInfo(approveid);
        }

        /// <summary>
        /// 根据申请id获取审批信息[不包括没审的]
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public HighRiskCheckEntity GetApproveInfo(string approveid)
        {
            return service.GetApproveInfo(approveid);
        }

        /// <summary>
        /// 根据申请id获取没审核的条数
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public int GetNoCheckNum(string approveid)
        {
            return service.GetNoCheckNum(approveid);
        }

          /// <summary>
        /// 根据申请id和当前登录人获取审核(批)记录
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public HighRiskCheckEntity GetNeedCheck(string approveid)
        {
            return service.GetNeedCheck(approveid);
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
        /// 根据申请表id删除数据
        /// </summary>
        public int Remove(string workid)
        {
            try
            {
                service.Remove(workid);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HighRiskCheckEntity entity)
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
