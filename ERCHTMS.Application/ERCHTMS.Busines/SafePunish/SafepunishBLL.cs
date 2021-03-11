using ERCHTMS.Entity.SafePunish;
using ERCHTMS.IService.SafePunish;
using ERCHTMS.Service.SafePunish;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.Busines.SafePunish
{
    /// <summary>
    /// 描 述：安全惩罚
    /// </summary>
    public class SafepunishBLL
    {
        private SafepunishIService service = new SafepunishService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafepunishEntity> GetList(string queryJson)
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
        public SafepunishEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }



        public string GetPunishStatisticsCount(string year, string statMode)
        {
            return service.GetPunishStatisticsCount(year, statMode);
        }

        public string GetPunishStatisticsList(string year, string statMode)
        {
            return service.GetPunishStatisticsList(year, statMode);
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
        /// <param name="kpiEntity">考核信息</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafepunishEntity entity ,SafekpidataEntity kpiEntity)
        {
            try
            {
                service.SaveForm(keyValue, entity, kpiEntity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void CommitApply(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            try
            {
                service.CommitApply(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public Flow GetFlow(string keyValue)
        {
          return  service.GetFlow(keyValue);
        }

        public string GetPunishCode()
        {
           return service.GetPunishCode();
        }

        public string GetPunishNum()
        {
            return service.GetPunishNum();
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
