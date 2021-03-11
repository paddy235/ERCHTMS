using ERCHTMS.Entity.SafeReward;
using ERCHTMS.IService.SafeReward;
using ERCHTMS.Service.SafeReward;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.Busines.SafeReward
{
    /// <summary>
    /// 描 述：安全奖励
    /// </summary>
    public class SaferewardBLL
    {
        private SaferewardIService service = new SaferewardService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SaferewardEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SaferewardEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        /// <summary>
        ///获取统计表格数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public DataTable GetRewardStatisticsList(string year)
        {
            return service.GetRewardStatisticsList(year);
        }

        /// <summary>
        ///获取奖励次数表格数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public DataTable GetRewardStatisticsTimeList(string year)
        {
            return service.GetRewardStatisticsTimeList(year);
        }


        public string GetRewardStatisticsCount(string year)
        {
            return service.GetRewardStatisticsCount(year);
        }

        public string GetRewardStatisticsTime(string year)
        {
            return service.GetRewardStatisticsTime(year);
        }


        /// <summary>
        /// 获取编号
        /// </summary>
        /// <returns></returns>
        public string GetRewardCode()
        {
            return service.GetRewardCode();
        }

        public string GetRewardStatisticsExcel(string year = "")
        {
            return service.GetRewardStatisticsExcel(year);
        }

        public string GetRewardStatisticsTimeExcel(string year = "")
        {
            return service.GetRewardStatisticsTimeExcel(year);
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
        public void SaveForm(string keyValue, SaferewardEntity entity)
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

        public void CommitApply(string keyValue, AptitudeinvestigateauditEntity entity, string leaderShipId)
        {
            try
            {
                service.CommitApply(keyValue, entity, leaderShipId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object GetStandardJson()
        {
            try
            {
               return service.GetStandardJson();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Flow GetFlow(string keyValue)
        {
          return  service.GetFlow(keyValue);
        }

        public List<object> GetLeaderList()
        {
            return service.GetLeaderList();
        }


        public string GetRewardNum()
        {
            return service.GetRewardNum();
        }

        public string GetDeptPId(string deptId)
        {
            return service.GetDeptPId(deptId);
        }

        public List<object> GetSpecialtyPrincipal(string applyDeptId)
        {
            return service.GetSpecialtyPrincipal(applyDeptId);
        }

        /// <summary>
        /// 获取审核信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetAptitudeInfo(string keyValue)
        {
            return service.GetAptitudeInfo(keyValue);
        }
    }
}
