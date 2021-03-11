using ERCHTMS.Entity.AssessmentManage;
using ERCHTMS.IService.AssessmentManage;
using ERCHTMS.Service.AssessmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.AssessmentManage
{
    /// <summary>
    /// 描 述：自评扣分明细
    /// </summary>
    public class KScoreDetailBLL
    {
        private KScoreDetailIService service = new KScoreDetailService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<KScoreDetailEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public KScoreDetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            return service.GetPageListJson(pagination, queryJson);
        }


        /// <summary>
        /// 根据计划id获取数据
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public DataTable GetDetailInfo(string planid)
        {
            return service.GetDetailInfo(planid);
        }

        /// <summary>
        /// 大项的所有小项列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetAllDetailPage(Pagination pagination)
        {
            return service.GetAllDetailPage(pagination);
        }


        /// <summary>
        /// 根据计划id和小项节点id获取不适宜项数据
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public KScoreDetailEntity GetKScoreByPlanOrChapID(string planid, string chapterid)
        {
            return service.GetKScoreByPlanOrChapID(planid, chapterid);
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
        public void SaveForm(string keyValue, KScoreDetailEntity entity)
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
