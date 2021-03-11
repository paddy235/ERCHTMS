using ERCHTMS.Entity.AssessmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.AssessmentManage
{
    /// <summary>
    /// 描 述：自评总结
    /// </summary>
    public interface AssessmentSumIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<AssessmentSumEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        AssessmentSumEntity GetEntity(string keyValue);

        /// <summary>
        /// 根据计划id和大项节点id获取
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        AssessmentSumEntity GetSumByPlanOrChapID(string planid, string chapterid);

        /// <summary>
        /// 根据计划id和大项节点id获取
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        DataTable GetSummarizeInfo(string planid, string chapterid);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetSumUpPageJson(Pagination pagination, string queryJson);


        /// <summary>
        /// 获取综述等相关数据
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        DataTable GetSumDataInfo(string planid);

        /// <summary>
        /// 根据计划id统计数据
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        string GetSumDataCount(string planid);


        string GetEveryBigPerson(string planid, string type);


        /// <summary>
        /// 该计划每个项扣分和（除第六项,七项和第十项之外）
        /// </summary>
        /// <returns></returns>
        int GetEverySumScore(string planid);

        /// <summary>
        /// 该计划每个项扣分值和扣分原因（除第六项,七项和第十项之外）
        /// </summary>
        /// <returns></returns>
        string GetEveryResonAndScore(string planid);

        /// <summary>
        /// 该计划第七项和第十项扣分和
        /// </summary>
        /// <returns></returns>
        int GetEverySumScore2(string planid);

        /// <summary>
        /// 该计划第七项和第十项扣分值和扣分原因
        /// </summary>
        /// <returns></returns>
        string GetEveryResonAndScore2(string planid);

        /// <summary>
        /// 该计划以下元素扣分和
        /// </summary>
        /// <returns></returns>
        int GetEverySumScore3(string planid, string strMarjor);

        /// <summary>
        /// 该计划以下元素扣分值和扣分原因
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="strMarjor"></param>
        /// <returns></returns>
        string GetEveryResonAndScore3(string planid, string strMarjor);


        /// <summary>
        /// 附件一的统计
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        DataTable GetAffixOne(string planid);


        /// <summary>
        /// 每个项的标准得分，不适宜项分，扣分，最终得分
        /// </summary>
        /// <returns></returns>
        DataTable GetEveryBigNoSuitScore(string planid);

        /// <summary>
        /// 获取根节点的章节的所有标准分
        /// </summary>
        /// <param name="code"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        int GetBigChapterScore();
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
        void SaveForm(string keyValue, AssessmentSumEntity entity);


        /// <summary>
        /// 根据自评计划id删除数据
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        int Remove(string planId);
        #endregion
    }
}
