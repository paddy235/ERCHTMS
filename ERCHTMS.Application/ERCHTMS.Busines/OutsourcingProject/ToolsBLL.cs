using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using System.Collections.Generic;
using System;
using ERCHTMS.IService.OutsourcingProject;
using System.Data;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：工器具验收
    /// </summary>
    public class ToolsBLL
    {
        private IToolsService service = new ToolsService();

        #region 获取数据

        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ToolsEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ToolsEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 得到流程图
        /// </summary>
        /// <param name="keyValue">业务表ID</param>
        /// <param name="modulename">逐级审核模块名</param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue, string modulename)
        {
            return service.GetFlow(keyValue, modulename);
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
        public void SaveForm(string keyValue,string type, ToolsEntity entity)
        {
            try
            {
                service.SaveForm(keyValue,type, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}

