using ERCHTMS.Entity.AssessmentManage;
using ERCHTMS.Busines.AssessmentManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Web.Areas.AssessmentManage.Controllers
{
    /// <summary>
    /// 描 述：自评扣分明细
    /// </summary>
    public class KScoreDetailController : MvcControllerBase
    {
        private KScoreDetailBLL kscoredetailbll = new KScoreDetailBLL();
        private AssessmentChaptersBLL assessmentchaptersbll = new AssessmentChaptersBLL();
        private AssessmentStandardBLL assessmentstandardbll = new AssessmentStandardBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 扣分项详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult KScoreResult()
        {
            return View();
        }

        /// <summary>
        /// 总结页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GradeSum()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = kscoredetailbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="smallchaperid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFormJson(string planid, string smallchaperid)
        {
            KScoreDetailEntity resultEntity = kscoredetailbll.GetKScoreByPlanOrChapID(planid, smallchaperid);
            return ToJsonResult(resultEntity);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "a.Id";
            pagination.p_fields = "a.PlanName,'' as progress,'' as Status,'' as score,'' as isUpdate,'' as able,b.count";
            pagination.p_tablename = string.Format("bis_assessmentplan a left join (select assessmentplanid,count(assessmentplanid) as count from bis_kscoredetail where createuserid='{0}' group by assessmentplanid) b on a.id=b.assessmentplanid", user.UserId);
            pagination.conditionJson = "1=1";
            var data = kscoredetailbll.GetPageListJson(pagination, queryJson);
            var watch = CommonHelper.TimerStart();
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }


        /// <summary>
        /// 大项的所有小项列表(排除选择的不适宜项)
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="chaperOrfrist"></param>
        /// <param name="planid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllDetailPage(Pagination pagination, string chaperOrfrist, string planid)
        {
            pagination.p_kid = "a.Id as sid";
            pagination.p_fields = "b.id as nsuitid,MajorNumber,ChaptersName,Content,Score,kScore,ChaptersParentID";
            pagination.p_tablename = string.Format(@"bis_assessmentchapters a left join (select * from bis_kscoredetail where assessmentplanid='{0}') b on a.id=b.chapterid", planid);
            pagination.conditionJson = string.Format("ChaptersParentID='{1}' and a.id not in(select chapterid from bis_nosuitabledetail where assessmentplanid='{0}') and Content is not null", planid, chaperOrfrist);
            var data = kscoredetailbll.GetAllDetailPage(pagination);
            var watch = CommonHelper.TimerStart();
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }


        /// <summary>
        /// 扣分项列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="chaperOrfrist"></param>
        /// <param name="planid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetKScoreDetailPage(Pagination pagination, string chaperOrfrist, string planid)
        {
            pagination.p_kid = "a.Id";
            pagination.p_fields = "MajorNumber,ChaptersName,Content,Score,kScore,b.createusername";
            pagination.p_tablename = " bis_kscoredetail  b  left join bis_assessmentchapters a  on a.id=b.chapterid";
            pagination.conditionJson = string.Format("chapterid in( select id from bis_assessmentchapters where  ChaptersParentID='{0}') and assessmentplanid='{1}'", chaperOrfrist, planid);
            var data = kscoredetailbll.GetAllDetailPage(pagination);
            var watch = CommonHelper.TimerStart();
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }


        /// <summary>
        /// 获取章节信息
        /// </summary>
        /// <param name="chaperid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetChaperInfo(string chaperid)
        {
            var resultEntity = assessmentchaptersbll.GetEntity(chaperid);
            var list = assessmentstandardbll.GetListByChaptersId(chaperid);
            foreach (var item in list)
            {
                resultEntity.ScoreRule += item.Content;
            }
            return ToJsonResult(resultEntity);
        }

        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        [HttpGet]
        public ActionResult GetDetailInfo(string planid)
        {
            var data = kscoredetailbll.GetDetailInfo(planid);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="smallchaperid"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string planid, string smallchaperid)
        {
            KScoreDetailEntity resultEntity = kscoredetailbll.GetKScoreByPlanOrChapID(planid, smallchaperid);
            if (resultEntity != null)
            {
                kscoredetailbll.RemoveForm(resultEntity.Id);
            }
            return Success("删除成功。");
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="smallchaperid"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string planid, string smallchaperid, KScoreDetailEntity entity)
        {
            KScoreDetailEntity resultEntity = kscoredetailbll.GetKScoreByPlanOrChapID(planid, smallchaperid);
            if (resultEntity != null)
            {
                resultEntity.kScore = entity.kScore;//扣分
                resultEntity.Measure = entity.Measure;//整改措施
                resultEntity.kScoreReason = entity.kScoreReason;//扣分原因
                kscoredetailbll.SaveForm(resultEntity.Id, resultEntity);
            }
            else
            {
                resultEntity = new KScoreDetailEntity();
                resultEntity.kScore = entity.kScore;//扣分
                resultEntity.Measure = entity.Measure;//整改措施
                resultEntity.kScoreReason = entity.kScoreReason;//扣分原因
                resultEntity.kScoreDate = DateTime.Now;
                resultEntity.AssessmentPlanID = planid;
                resultEntity.ChapterID = smallchaperid;
                kscoredetailbll.SaveForm("", resultEntity);
            }
            return Success("操作成功。");
        }
        #endregion
    }
}
