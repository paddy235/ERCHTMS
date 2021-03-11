using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.IService.PowerPlantInside;
using ERCHTMS.Service.PowerPlantInside;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.PowerPlantInside
{
    /// <summary>
    /// 描 述：单位内部快报
    /// </summary>
    public class PowerplantinsideBLL
    {
        private PowerplantinsideIService service = new PowerplantinsideService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<PowerplantinsideEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
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
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PowerplantinsideEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public string GetStatisticsList(int year, string mode)
        {
            return service.GetStatisticsList(year,mode);
        }

        public string GetStatisticsHighchart(string year, string mode)
        {
            return service.GetStatisticsHighchart(year, mode);
        }

        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="createdeptid">创建人部门ID</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid)
        {
            return service.CheckAuditPower(currUser, out state, moduleName, createdeptid);
        }

        /// <summary>
        /// 待审核事故事件数量
        /// </summary>
        /// <returns></returns>
        public string GetAccidentEventNum()
        {
            return service.GetAccidentEventNum();
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
        public void SaveForm(string keyValue, PowerplantinsideEntity entity)
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
