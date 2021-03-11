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
    /// 描 述：自评总结
    /// </summary>
    public class AssessmentSumBLL
    {
        private AssessmentSumIService service = new AssessmentSumService();

        #region 获取数据
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<AssessmentSumEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public AssessmentSumEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        /// <summary>
        /// 根据计划id和大项节点id获取
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public AssessmentSumEntity GetSumByPlanOrChapID(string planid, string chapterid)
        {
            return service.GetSumByPlanOrChapID(planid, chapterid);
        }

        /// <summary>
        /// 根据计划id和大项节点id获取
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public DataTable GetSummarizeInfo(string planid, string chapterid)
        {
            return service.GetSummarizeInfo(planid, chapterid);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetSumUpPageJson(Pagination pagination, string queryJson)
        {
            return service.GetSumUpPageJson(pagination, queryJson);
        }

        /// <summary>
        /// 获取综述等相关数据
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public DataTable GetSumDataInfo(string planid)
        {
            return service.GetSumDataInfo(planid);
        }

        /// <summary>
        /// 根据计划id统计数据
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public string GetSumDataCount(string planid)
        {
            return service.GetSumDataCount(planid);
        }

        public string GetEveryBigPerson(string planid, string type)
        {
            return service.GetEveryBigPerson(planid, type);
        }

        /// <summary>
        /// 该计划每个项扣分和（除第六项,七项和第十项之外）
        /// </summary>
        /// <returns></returns>
        public int GetEverySumScore(string planid)
        {
            return service.GetEverySumScore(planid);
        }

        /// <summary>
        /// 该计划每个项扣分值和扣分原因（除第六项,七项和第十项之外）
        /// </summary>
        /// <returns></returns>
        public string GetEveryResonAndScore(string planid)
        {
            return service.GetEveryResonAndScore(planid);
        }
         /// <summary>
        /// 该计划第七项和第十项扣分和
        /// </summary>
        /// <returns></returns>
        public int GetEverySumScore2(string planid)
        {
            return service.GetEverySumScore2(planid);
        }

          /// <summary>
        /// 该计划第七项和第十项扣分值和扣分原因
        /// </summary>
        /// <returns></returns>
        public string GetEveryResonAndScore2(string planid)
        {
            return service.GetEveryResonAndScore2(planid);
        }

          /// <summary>
        /// 该计划以下元素扣分和
        /// </summary>
        /// <returns></returns>
        public int GetEverySumScore3(string planid, string strMarjor)
        {
            return service.GetEverySumScore3(planid,strMarjor);
        }

          /// <summary>
        /// 该计划以下元素扣分值和扣分原因
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="strMarjor"></param>
        /// <returns></returns>
        public string GetEveryResonAndScore3(string planid, string strMarjor)
        {
            return service.GetEveryResonAndScore3(planid,strMarjor);
        }

         /// <summary>
        /// 附件一的统计
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        public DataTable GetAffixOne(string planid)
        {
            return service.GetAffixOne(planid);
        }

         /// <summary>
        /// 每个项的标准得分，不适宜项分，扣分，最终得分
        /// </summary>
        /// <returns></returns>
        public DataTable GetEveryBigNoSuitScore(string planid)
        {
            return service.GetEveryBigNoSuitScore(planid);
        }

          /// <summary>
        /// 获取根节点的章节的所有标准分
        /// </summary>
        /// <param name="code"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public int GetBigChapterScore()
        {
            return service.GetBigChapterScore();
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
        /// 根据自评计划id删除数据
        /// </summary>
        /// <param name="planId">计划id</param>
        public int Remove(string planId)
        {
            try
            {
                service.Remove(planId);
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
        public void SaveForm(string keyValue, AssessmentSumEntity entity)
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
