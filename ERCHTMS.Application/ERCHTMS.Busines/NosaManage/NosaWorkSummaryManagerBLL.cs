using BSFramework.Util.WebControl;
using ERCHTMS.Entity.NosaManage;
using ERCHTMS.IService.NosaManage;
using ERCHTMS.Service.NosaManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.NosaManage
{
    /// <summary>
    /// 工作总结管理
    /// </summary>
    public class NosaWorkSummaryManagerBLL
    {
        private NosaAreaWorkSummaryIService areaService = new NosaAreaWorkSummaryService();
        private NosaPersonWorkSummaryIService personService = new NosaPersonWorkSummaryService();
        /// <summary>
        /// 元素负责人生产月度工作总结
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool SyncPersonWorkSummary(string sql)
        {
            return personService.SyncPersonWorkSummary(sql);
        }
        /// <summary>
        /// 获取元素负责人工作总结分页列表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetElementPageJson(Pagination pagination, string queryJson) {
            return personService.GetElementPageJson(pagination, queryJson);
        }

        /// <summary>
        /// 获取区域代表工作总结分页列表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetAreaWorkPageJson(Pagination pagination, string queryJson) {
            return areaService.GetAreaWorkPageJson(pagination, queryJson);
        }
        /// <summary>
        /// 执行SQL获取DataTable数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetTable(string sql) {
            return personService.GetTable(sql);
        }
        /// <summary>
        /// 工作总结提交--元素负责人
        /// </summary>
        /// <param name="keyValue"></param>
        public void CommitPeopleSummary(string keyValue) {
            personService.CommitPeopleSummary(keyValue);
        }
        /// <summary>
        /// 工作总结提交--区域代表
        /// </summary>
        /// <param name="keyValue"></param>
        public void CommitAreaSummary(string keyValue)
        {
            areaService.CommitAreaSummary(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public NosaPersonWorkSummaryEntity GetEntity(string keyValue)
        {
            return personService.GetEntity(keyValue);
        }
        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                personService.RemoveForm(keyValue);
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
        public void SaveForm(string keyValue, NosaPersonWorkSummaryEntity entity)
        {
            try
            {
                personService.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
