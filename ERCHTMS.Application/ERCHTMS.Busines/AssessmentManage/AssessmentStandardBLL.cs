using ERCHTMS.Entity.AssessmentManage;
using ERCHTMS.IService.AssessmentManage;
using ERCHTMS.Service.AssessmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.AssessmentManage
{
    /// <summary>
    /// 描 述：自评评分标准
    /// </summary>
    public class AssessmentStandardBLL
    {
        private AssessmentStandardIService service = new AssessmentStandardService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public IEnumerable<AssessmentStandardEntity> GetListByChaptersId(string chaptersId)
        {
            return service.GetListByChaptersId(chaptersId);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<AssessmentStandardEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<AssessmentStandardEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public AssessmentStandardEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
        public void SaveForm(string keyValue, AssessmentStandardEntity entity)
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
