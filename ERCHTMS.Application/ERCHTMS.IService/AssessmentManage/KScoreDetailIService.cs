using ERCHTMS.Entity.AssessmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.AssessmentManage
{
    /// <summary>
    /// 描 述：自评扣分明细
    /// </summary>
    public interface KScoreDetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<KScoreDetailEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        KScoreDetailEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageListJson(Pagination pagination, string queryJson);

        /// <summary>
        /// 根据计划id获取数据
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        DataTable GetDetailInfo(string planid);


        /// <summary>
        /// 大项的所有小项列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetAllDetailPage(Pagination pagination);

        /// <summary>
        /// 根据计划id和小项节点id获取扣分项
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        KScoreDetailEntity GetKScoreByPlanOrChapID(string planid, string chapterid);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, KScoreDetailEntity entity);
        #endregion
    }
}
